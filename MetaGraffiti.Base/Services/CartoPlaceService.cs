using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using OfficeOpenXml;

using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Crypto;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Base.Services
{
	public class CartoPlaceService
	{
		// ==================================================
		// Internals
		private static string _uri = "";
		private static bool _cached = false;
		private static bool _dirty = false;
		private static DateTime? _saved;
		private static BasicCacheService<CartoPlaceInfo> _cache = new BasicCacheService<CartoPlaceInfo>();
		private GoogleApiService _google = null;

		// ==================================================
		// Constructors
		public CartoPlaceService(GoogleApiService google)
		{
			_google = google;
		}


		// ==================================================
		// Methods

		public bool HasChanges => _dirty;
		public DateTime? LastSaved => _saved;

		public void Reload()
		{
			_cached = false;
			_dirty = false;
			_cache = new BasicCacheService<CartoPlaceInfo>();

			Init(_uri);
		}

		/// <summary>
		/// Loads cached places from local storage
		/// </summary>
		public void Init(string uri)
		{
			lock (_cache)
			{
				if (_cached) return;

				_uri = uri;
				var reader = new XlsFileReader(uri);
				var file = reader.ReadFile();

				var sheet = new CartoPlaceSheetData(file.Sheets[0]);
				foreach(var row in sheet.Rows)
				{
					var info = new CartoPlaceInfo(row);
					_cache.Add(info);
				}				
				
				_cached = true;
				_dirty = false;
			}
		}

		public void Save()
		{
			lock (_cache)
			{
				// create new file
				var data = BuildWorkbook();

				// backup existing file
				if (File.Exists(_uri)) File.Move(_uri, _uri.Replace(".xlsx", "." + DateTime.Now.Ticks + ".xlsx"));

				// write updated file
				File.WriteAllBytes(_uri, data);

				// update save timestamp
				_saved = DateTime.Now;
			}
		}


		public List<string> ListPlaceTypes()
		{
			return _cache.All.Select(x => x.PlaceType).Distinct().OrderBy(x => x).ToList();
		}

		public List<GeoCountryInfo> ListCountries()
		{
			return GeoCountryInfo.ListAsDistinct(_cache.All.Select(x => x.Country.ISO2).Distinct());
		}

		/// <summary>
		/// Lists all places in current cache
		/// </summary>
		public List<CartoPlaceInfo> ListPlaces()
		{
			return _cache.All;
		}

		public CartoPlaceInfo GetPlace(string key)
		{
			return _cache[key.ToUpperInvariant()];
		}

		public CartoPlaceInfo FindByGooglePlaceID(string googlePlaceID)
		{
			return _cache.All.FirstOrDefault(x => x.GoogleKey == googlePlaceID);
		}

		public void DeletePlace(string key)
		{
			_cache.RemoveOrIgnore(key.ToUpperInvariant());
		}


		public CartoPlaceInfo FindPlace(GeoCountryInfo country, string name, bool deep = false)
		{
			var search = _cache.All.Where(x => x.Country.CountryID == country.CountryID);
			search = FindPlace(search, name, deep);
			return search.FirstOrDefault();
		}

		public CartoPlaceInfo FindPlace(GeoRegionInfo region, string name, bool deep = false)
		{
			var search = _cache.All.Where(x => x.Region != null && x.Region.RegionID == region.RegionID);
			search = FindPlace(search, name, deep);
			return search.FirstOrDefault();
		}

		private IEnumerable<CartoPlaceInfo> FindPlace(IEnumerable<CartoPlaceInfo> places, string name, bool deep = false)
		{
			if (deep)
				return places.Where(x => (String.Compare(x.Name, name, true) == 0)
					|| (String.Compare(x.LocalName, name, true) == 0)
					|| (String.Compare(x.DisplayAs, name, true) == 0));
			else
				return places.Where(x => String.Compare(x.Name, name, true) == 0);
		}


		public List<CartoPlaceInfo> ReportPlaces(CartoPlaceReportRequest request)
		{
			var query = _cache.All.AsQueryable();

			var placeType = request.PlaceType;
			if (!String.IsNullOrWhiteSpace(placeType))
			{
				placeType = placeType.Replace(',', ';').ToLowerInvariant();
				if (placeType.Contains(';'))
				{
					var placeTypes = placeType.Split(';').Select(x => x.Trim()).ToList();
					query = query.Where(x => placeTypes.Contains(x.PlaceType.ToLowerInvariant()));
				}
				else
					query = query.Where(x => (String.Compare(x.PlaceType, placeType, true) == 0));
			}

			var country = GeoCountryInfo.Find(request.Country);
			if (country != null) query = query.Where(x => x.Country.CountryID == country.CountryID);

			var region = GeoRegionInfo.Find(request.Region);
			if (region != null) query = query.Where(x => x.Region.RegionID == region.RegionID);

			var locality = request.Locality;
			if (!String.IsNullOrWhiteSpace(locality)) query = query.Where(x => String.Compare(x.Locality, locality, true) == 0);

			var name = request.Name;
			if (!String.IsNullOrWhiteSpace(name))
			{
				name = name.Replace("%", "*");
				var n = name.Trim('*').ToLowerInvariant();
				if (name.StartsWith("*") && name.EndsWith("*"))
					query = query.Where(x => x.Name.ToLowerInvariant().Contains(n));
				else if (name.StartsWith("*"))
					query = query.Where(x => x.Name.ToLowerInvariant().StartsWith(n));
				else if (name.EndsWith("*"))
					query = query.Where(x => x.Name.ToLowerInvariant().EndsWith(n));
				else
					query = query.Where(x => x.Name.ToLowerInvariant() == n);
			}

			var text = request.Text;
			if (!String.IsNullOrWhiteSpace(text))
			{
				var t = text.ToLowerInvariant();
				query = query.Where(x => GetFullTextSearch(x).Contains(t));
			}

			query = query.OrderBy(x => x.Country.Name).ThenBy(x => (x.Region == null ? "" : x.Region.RegionName)).ThenBy(x => x.Name);
			return query.ToList();
		}




		public CartoPlaceInfo LookupByPlaceID(string googlePlaceID)
		{
			var existing = FindByGooglePlaceID(googlePlaceID);
			if (existing != null) return existing;

			var response = _google.RequestLocation(googlePlaceID);
			var result = response.Results.FirstOrDefault();
			if (result == null)
				return null;
			else
				return new CartoPlaceInfo(result);
		}

		public List<CartoPlaceInfo> LookupLocations(string text)
		{
			var response = _google.RequestLocations(text);

			var list = new List<CartoPlaceInfo>();
			foreach (var result in response.Results)
			{
				var place = FindByGooglePlaceID(result.PlaceID);
				if (place == null) place = new CartoPlaceInfo(result);
				list.Add(place);
			}

			return list;
		}

		public List<CartoPlaceInfo> LookupLocations(IGeoLatLon point)
		{
			var response = _google.RequestLocations(point);

			var list = new List<CartoPlaceInfo>();
			foreach (var result in response.Results)
			{
				var place = FindByGooglePlaceID(result.PlaceID);
				if (place == null) place = new CartoPlaceInfo(result);
				list.Add(place);
			}

			return list;
		}


		public CartoPlaceInfo CreatePlace(CartoPlaceCreateRequest create)
		{
			create.PlaceKey = CryptoGraffiti.NewHashID();
			var place = new CartoPlaceInfo(create);

			_cache.Add(place);

			_dirty = true;
			return place;
		}

		public CartoPlaceInfo UpdatePlace(CartoPlaceUpdateRequest update)
		{
			var place = new CartoPlaceInfo(update);

			_cache.Update(place.Key, place);

			_dirty = true;
			return place;
		}





		private string GetFullTextSearch(CartoPlaceInfo place)
		{
			return $"{place.Name} {place.LocalName} {place.DisplayAs} {place.Address}".ToLowerInvariant();
		}

		private byte[] BuildWorkbook()
		{
			using (var excel = new ExcelPackage())
			{
				using (var sheet = excel.Workbook.Worksheets.Add("CartoPlaceInfo"))
				{
					// build header
					int row = 1;
					int cell = 1;

					sheet.Cells[row, cell++].Value = "PlaceID";
					sheet.Cells[row, cell++].Value = "PlaceKey";
					sheet.Cells[row, cell++].Value = "PlaceType";
					sheet.Cells[row, cell++].Value = "GoogleKey";

					// geo political
					sheet.Cells[row, cell++].Value = "Timezone";
					sheet.Cells[row, cell++].Value = "Country";
					sheet.Cells[row, cell++].Value = "Region";

					// logical name
					sheet.Cells[row, cell++].Value = "Name";
					sheet.Cells[row, cell++].Value = "LocalName";
					sheet.Cells[row, cell++].Value = "DisplayAs";
					sheet.Cells[row, cell++].Value = "Description";

					// logical location
					sheet.Cells[row, cell++].Value = "Address";
					sheet.Cells[row, cell++].Value = "Locality";
					sheet.Cells[row, cell++].Value = "Postcode";
					sheet.Cells[row, cell++].Value = "Subregions";
					//sheet.Cells[row, cell++].Value = "Sublocalities";

					// physical location
					sheet.Cells[row, cell++].Value = "CenterLatitude";
					sheet.Cells[row, cell++].Value = "CenterLongitude";
					sheet.Cells[row, cell++].Value = "NorthLatitude";
					sheet.Cells[row, cell++].Value = "SouthLatitude";
					sheet.Cells[row, cell++].Value = "WestLongitude";
					sheet.Cells[row, cell++].Value = "EastLongitude";

					// add location rows
					var id = 1;
					var places = ListPlaces();
					foreach (var place in places)
					{
						row++;
						cell = 1;

						// place idenity
						sheet.Cells[row, cell++].Value = id++;
						sheet.Cells[row, cell++].Value = place.Key;
						sheet.Cells[row, cell++].Value = place.PlaceType;
						sheet.Cells[row, cell++].Value = place.GoogleKey;

						// geo political
						sheet.Cells[row, cell++].Value = (place.Timezone?.TZID ?? "");
						sheet.Cells[row, cell++].Value = (place.Country?.Name ?? "");
						sheet.Cells[row, cell++].Value = (place.Region?.RegionName ?? "");

						// logical name
						sheet.Cells[row, cell++].Value = place.Name;
						sheet.Cells[row, cell++].Value = place.LocalName;
						sheet.Cells[row, cell++].Value = place.DisplayAs;
						sheet.Cells[row, cell++].Value = place.Description;

						// logical location
						sheet.Cells[row, cell++].Value = place.Address;
						sheet.Cells[row, cell++].Value = place.Locality;
						sheet.Cells[row, cell++].Value = place.Postcode;
						sheet.Cells[row, cell++].Value = place.Subregions;
						//ws.Cells[row, cell++].Value = location.Sublocalities;

						// physical location
						sheet.Cells[row, cell++].Value = place.Center.Latitude;
						sheet.Cells[row, cell++].Value = place.Center.Longitude;
						sheet.Cells[row, cell++].Value = (place.Bounds?.NorthWest.Latitude ?? 0.0);
						sheet.Cells[row, cell++].Value = (place.Bounds?.SouthEast.Latitude ?? 0.0);
						sheet.Cells[row, cell++].Value = (place.Bounds?.NorthWest.Longitude ?? 0.0);
						sheet.Cells[row, cell++].Value = (place.Bounds?.SouthEast.Longitude ?? 0.0);
					}

					// return built worksheet
					return excel.GetAsByteArray();
				}
			}
		}
	}

	public class CartoPlaceReportRequest
	{
		/// <summary>
		/// Supports a , or ; seperated list for multiple types
		/// </summary>
		public string PlaceType { get; set; }

		/// <summary>
		/// Matches on name field only, supports wildcards (* or %)
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Partial match in multiple common text fields
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Exact match filters
		/// </summary>
		public string Country { get; set; }
		public string Region { get; set; }
		public string Locality { get; set; }
	}

	public class CartoPlaceCreateRequest : CartoPlaceData
	{
	}

	public class CartoPlaceUpdateRequest : CartoPlaceCreateRequest
	{
		public string ID { get; set; }
	}
}
