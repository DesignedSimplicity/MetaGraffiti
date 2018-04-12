using System;
using System.Collections.Generic;
using System.Text;

using RestSharp;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Services.External
{
	public class GoogleApiService
	{
		// ==================================================
		// Internals
		private string _apiKey;

		//TODO: refactor this to use basic cache classes
		private Dictionary<string, GooglePlacesSearchReponse> _cacheSearch = new Dictionary<string, GooglePlacesSearchReponse>();
		private Dictionary<string, GooglePlaceDetailReponse> _cachePlace = new Dictionary<string, GooglePlaceDetailReponse>();
		private Dictionary<string, GoogleLocationResponse> _cacheLocation = new Dictionary<string, GoogleLocationResponse>();
		private Dictionary<string, GoogleTimezoneResponse> _cacheTimezone = new Dictionary<string, GoogleTimezoneResponse>();
		private Dictionary<string, GoogleElevationResponse> _cacheElevation = new Dictionary<string, GoogleElevationResponse>();

		
		// ==================================================
		// Constructors
		public GoogleApiService(string apiKey) { _apiKey = apiKey; }


		// ==================================================
		// Methods
		public GoogleTimezoneResponse RequestTimezone(IGeoLatLon point, DateTime? timestamp = null)
		{
			var location = GetLocationString(point);

			if (!timestamp.HasValue) timestamp = DateTime.Today;
			var unixTimestamp = GetUnixTimeStampFromDateTime(timestamp.Value);

			var key = $"{location}_{unixTimestamp}";
			if (_cacheTimezone.ContainsKey(key)) return _cacheTimezone[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/timezone/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("location", location);
			request.AddParameter("timestamp", unixTimestamp);
			request.AddParameter("sensor", "false");

			var response = client.Execute<GoogleTimezoneResponse>(request);

			lock (_cacheTimezone) { if (!_cacheTimezone.ContainsKey(key)) _cacheTimezone.Add(key, response.Data); }
			return response.Data;
		}

		public GoogleElevationResponse RequestElevation(IGeoLatLon point)
		{
			var location = GetLocationString(point);

			var key = location;
			if (_cacheElevation.ContainsKey(key)) return _cacheElevation[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/elevation/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("location", location);
			request.AddParameter("sensor", "false");

			var response = client.Execute<GoogleElevationResponse>(request);

			lock (_cacheElevation) { if (!_cacheElevation.ContainsKey(key)) _cacheElevation.Add(key, response.Data); }
			return response.Data;
		}

		public GooglePlaceDetailReponse RequestPlace(string place_id)
		{
			var key = place_id;
			if (_cachePlace.ContainsKey(key)) return _cachePlace[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/place/details/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("place_id", place_id);

			var response = client.Execute(request);
			var data = new GooglePlaceDetailReponse(response.Content);

			lock (_cachePlace) { if (!_cachePlace.ContainsKey(key)) _cachePlace.Add(key, data); }
			return data;
		}


		public GoogleLocationResponse RequestLocation(string place_id)
		{
			var key = place_id;
			if (_cacheLocation.ContainsKey(key)) return _cacheLocation[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/geocode/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("place_id", place_id);

			var response = client.Execute(request);
			var data = new GoogleLocationResponse(response.Content);

			lock (_cacheLocation) { if (!_cacheLocation.ContainsKey(key)) _cacheLocation.Add(key, data); }
			return data;
		}

		// https://developers.google.com/places/web-service/search
		public GooglePlacesSearchReponse RequestPlaces(string text)
		{
			var key = text.Trim().ToLowerInvariant();
			if (_cacheSearch.ContainsKey(key)) return _cacheSearch[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/place/textsearch/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("query", text);

			var response = client.Execute(request);
			var data = new GooglePlacesSearchReponse(response.Content);

			lock (_cacheSearch) { if (!_cacheSearch.ContainsKey(key)) _cacheSearch.Add(key, data); }
			return data;
		}

		public GoogleLocationResponse RequestLocations(string text)
		{
			var key = text.Trim().ToLowerInvariant();
			if (_cacheLocation.ContainsKey(key)) return _cacheLocation[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/geocode/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("address", text);

			var response = client.Execute(request);
			var data = new GoogleLocationResponse(response.Content);
			//dynamic data = JsonConvert.DeserializeObject(response.Content);

			lock (_cacheLocation) { if (!_cacheLocation.ContainsKey(key)) _cacheLocation.Add(key, data); }
			return data;
		}

		public GoogleLocationResponse RequestLocations(IGeoLatLon point)
		{
			var location = GetLocationString(point);

			var key = location;
			if (_cacheLocation.ContainsKey(key)) return _cacheLocation[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/geocode/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("latlng", location);

			var response = client.Execute(request);
			var data = new GoogleLocationResponse(response.Content);
			//dynamic data = JsonConvert.DeserializeObject(response.Content);

			lock (_cacheLocation) { if (!_cacheLocation.ContainsKey(key)) _cacheLocation.Add(key, data); }
			return data;
		}



		// ==================================================
		// Helpers
		protected string GetLocationString(IGeoLatLon point)
		{
			return point.Latitude.ToString("0.00000000") + "," + point.Longitude.ToString("0.00000000");
		}

		protected long GetUnixTimeStampFromDateTime(DateTime dt)
		{
			DateTime epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan ts = dt - epochDate;
			return (long)ts.TotalSeconds;
		}

		protected DateTime GetDateTimeFromUnixTimeStamp(double ts)
		{
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dt = dt.AddSeconds(ts);
			return dt;
		}
	}
}
