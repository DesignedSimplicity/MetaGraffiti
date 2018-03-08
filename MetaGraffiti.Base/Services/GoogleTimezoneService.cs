using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RestSharp;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Services
{
	public class GoogleTimezoneResponse
	{
		public string Status { get; set; }
		public string TimeZoneId { get; set; }
		public string TimeZoneName { get; set; }
	}

	public class GoogleTimezoneService : GoogleApiServiceBase
	{
		private Dictionary<string, GoogleTimezoneResponse> _cache = new Dictionary<string, GoogleTimezoneResponse>();

		public GoogleTimezoneService(string apiKey) : base(apiKey) { }

		public GeoTimezoneInfo LookupGeoTimezone(IGeoLatLon point)
		{
			var response = RequestTimezone(point);
			return GeoTimezoneInfo.ByTZID(response.TimeZoneId);
		}

		public GoogleTimezoneResponse RequestTimezone(IGeoLatLon point, DateTime? timestamp = null)
		{
			var location = GetLocationString(point);

			if (!timestamp.HasValue) timestamp = DateTime.Today;
			var unixTimestamp = GetUnixTimeStampFromDateTime(timestamp.Value);

			var key = $"{location}_{unixTimestamp}";
			if (_cache.ContainsKey(key)) return _cache[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/timezone/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("location", location);
			request.AddParameter("timestamp", unixTimestamp);
			request.AddParameter("sensor", "false");

			var response = client.Execute<GoogleTimezoneResponse>(request);

			if (!_cache.ContainsKey(key)) _cache.Add(key, response.Data);
			return response.Data;
		}
	}
}