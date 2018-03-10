using System;
using System.Collections.Generic;
using System.Text;

using RestSharp;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Services
{
	public class GoogleElevationResponse
	{
		public double Elevation { get; set; }
		public double Resolution { get; set; }
	}

	// https://developers.google.com/maps/documentation/elevation/start
	// TOOD: https://developers.google.com/maps/documentation/javascript/examples/elevation-paths
	public class GoogleElevationService : GoogleApiServiceBase
	{
		private Dictionary<string, GoogleElevationResponse> _cache = new Dictionary<string, GoogleElevationResponse>();

		public GoogleElevationService(string apiKey) : base(apiKey) { }


		public double LookupElevation(IGeoLatLon point)
		{
			return RequestElevation(point).Elevation;
		}

		public GoogleElevationResponse RequestElevation(IGeoLatLon point)
		{
			var location = GetLocationString(point);

			var key = location;
			if (_cache.ContainsKey(key)) return _cache[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/elevation/json", Method.GET);			

			request.AddParameter("key", _apiKey);
			request.AddParameter("location", location);
			request.AddParameter("sensor", "false");

			var response = client.Execute<GoogleElevationResponse>(request);

			lock (_cache) { if (!_cache.ContainsKey(key)) _cache.Add(key, response.Data); }
			return response.Data;
		}
	}
}