using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Services
{
    public class GoogleApiServiceBase
    {
		protected string _apiKey;

		public GoogleApiServiceBase(string apiKey) { _apiKey = apiKey; }

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
