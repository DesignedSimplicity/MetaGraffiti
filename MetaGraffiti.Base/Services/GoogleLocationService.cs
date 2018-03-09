using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Services
{
	public class GoogleLocationSearchRequest
	{
		public string Name { get; set; }
		public string LocationType { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
	}

	// https://developers.google.com/maps/documentation/geocoding/start
	// TODO: https://developers.google.com/maps/documentation/javascript/examples/geocoding-simple
	// TODO: https://developers.google.com/maps/documentation/javascript/examples/geocoding-reverse
	public class GoogleLocationService : GoogleApiServiceBase
	{
		private Dictionary<string, object> _cache = new Dictionary<string, object>();

		public GoogleLocationService(string apiKey) : base(apiKey) { }

		public GeoLocationInfo LookupGeoLocation(IGeoLatLon point)
		{
			return null;// response.Data;
		}

		public object RequestLocation(IGeoLatLon point)
		{
			var location = GetLocationString(point);

			var key = location;
			if (_cache.ContainsKey(key)) return _cache[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/geocode/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("latlng", location);

			var response = client.Execute<object>(request);

			if (!_cache.ContainsKey(key)) _cache.Add(key, response.Data);
			return response.Data;
		}

		/*
		public List<GeoLocationInfo> FindLocations(IGeoLatLon point)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(string search)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(string search, GeoCountryInfo country)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(GoogleLocationSearchRequest search)
		{
			return null;
		}*/
	}
}
