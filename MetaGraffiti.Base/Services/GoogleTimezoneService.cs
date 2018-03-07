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
		public GoogleTimezoneService(string apiKey) : base(apiKey) { }

		public GeoTimezoneInfo LookupGeoTimezone(IGeoLatLon point)
		{
			var response = LookupTimezone(point);
			return GeoTimezoneInfo.ByTZID(response.TimeZoneId);
		}

		public GoogleTimezoneResponse LookupTimezone(IGeoLatLon point, DateTime? timestamp = null)
		{
			if (!timestamp.HasValue) timestamp = DateTime.Now;
			
			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/timezone/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("location", point.Latitude.ToString("0.00000000") + "," + point.Longitude.ToString("0.00000000"));
			request.AddParameter("timestamp", GetUnixTimeStampFromDateTime(timestamp.Value));
			request.AddParameter("sensor", "false");

			var response = client.Execute<GoogleTimezoneResponse>(request);			
			return response.Data;
		}

		private long GetUnixTimeStampFromDateTime(DateTime dt)
		{
			DateTime epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan ts = dt - epochDate;
			return (long)ts.TotalSeconds;
		}

		private DateTime GetDateTimeFromUnixTimeStamp(double ts)
		{
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dt = dt.AddSeconds(ts);
			return dt;
		}
	}
}