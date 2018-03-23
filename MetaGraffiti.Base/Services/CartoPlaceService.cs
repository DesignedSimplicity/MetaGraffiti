using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Crypto;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Services.External;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetaGraffiti.Base.Services
{
	public class CartoPlaceService
	{
		// ==================================================
		// Internals
		private static string _uri = "";
		private static bool _cached = false;
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

		public void Reload()
		{
			_cached = false;
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
			}
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
			var search = _cache.All.Where(x => x.Region.RegionID == region.RegionID);
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

			return place;
		}

		public CartoPlaceInfo UpdatePlace(CartoPlaceUpdateRequest update)
		{
			var place = new CartoPlaceInfo(update);

			_cache.Update(place.Key, place);

			return place;
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

	public class CartoPlaceCreateRequest : CartoPlaceData
	{
		/*
		public string PlaceType { get; set; }

		public string GoogleKey { get; set; }
		public string IconKey { get; set; }


		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }


		public string Name { get; set; }
		public string LocalName { get; set; }
		public string DisplayAs { get; set; }

		public string Description { get; set; }

		public string Address { get; set; }
		public string Locality { get; set; }
		public string Postcode { get; set; }

		public string Subregions { get; set; }

		public double CenterLatitude { get; set; }
		public double CenterLongitude { get; set; }
		public double NorthLatitude { get; set; }
		public double WestLongitude { get; set; }
		public double SouthLatitude { get; set; }
		public double EastLongitude { get; set; }
		*/
	}

	public class CartoPlaceUpdateRequest : CartoPlaceCreateRequest
	{
		public string ID { get; set; }
	}
}
