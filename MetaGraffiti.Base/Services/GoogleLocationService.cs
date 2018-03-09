using System;
using System.Collections.Generic;
using System.Text;

using RestSharp;
using Newtonsoft.Json.Linq;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using Newtonsoft.Json;

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
		private Dictionary<string, dynamic> _cache = new Dictionary<string, dynamic>();

		public GoogleLocationService(string apiKey) : base(apiKey) { }

		public GeoLocationInfo LookupGeoLocation(IGeoLatLon point)
		{
			var response = RequestLocations(point);
			var results = response.results;
			var first = results[0];
			var address = first.address_components[0];

			var location = new GeoLocationInfo()
			{
				Name = address.short_name,
				NameLong = address.long_name,
			};

			return location;
		}

		public dynamic RequestLocations(IGeoLatLon point)
		{
			var location = GetLocationString(point);

			var key = location;
			if (_cache.ContainsKey(key)) return _cache[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/geocode/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("latlng", location);

			var response = client.Execute(request);
			dynamic data = JsonConvert.DeserializeObject(response.Content);

			if (!_cache.ContainsKey(key)) _cache.Add(key, data);
			return data;
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
