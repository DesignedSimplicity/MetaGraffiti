using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Base.Services
{
	public class GoogleLookupService
	{
		// ==================================================
		// Internals
		private GoogleApiService _google = null;


		// ==================================================
		// Constructors
		public GoogleLookupService(GoogleApiService google)
		{
			_google = google;
		}


		// ==================================================
		// Methods

		// --------------------------------------------------
		// Elevation
		public double LookupElevation(IGeoLatLon point)
		{
			return _google.RequestElevation(point).Elevation;
		}

		// --------------------------------------------------
		// Timezone
		public GeoTimezoneInfo LookupTimezone(IGeoLatLon point)
		{
			var response = _google.RequestTimezone(point);
			return GeoTimezoneInfo.ByTZID(response.TimeZoneId);
		}
	}
}
