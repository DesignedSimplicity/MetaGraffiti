﻿using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Crypto;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Services.External;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Services
{
	public class CartoPlaceService
	{
		// ==================================================
		// Internals
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

		public void ResetCache()
		{
			_cached = false;
			_cache = new BasicCacheService<CartoPlaceInfo>();
		}

		/// <summary>
		/// Loads cached places from local storage
		/// </summary>
		public void Init(string uri)
		{
			lock (_cache)
			{
				if (_cached) return;

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
