using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using MetaGraffiti.Base.Modules;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using Microsoft.AspNetCore.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers.Api
{
	public class TimezonesController : ControllerBase
    {
		private IActionResult GetTimezone(string id)
		{
			var t = GeoTimezoneInfo.Find(id);
			if (t == null)
			{
				t = Graffiti.Geo.FindTimezone(id);
			}
			if (t == null) return new NotFoundResult();

			var d = MapTimezone(t);
			return Ok(d);
		}

		[HttpGet]
		public IActionResult FindTimezoneByPoint(double lat, double lng)
		{
			var service = new GoogleApiService(AutoConfig.GoogleMapsApiKey);

			var response = service.RequestTimezone(new GeoPosition(lat, lng));

			return GetTimezone(response.TimeZoneId);
		}

		[HttpGet]
		public IActionResult FindTimezoneByLocation(string name)
		{
			var timezone = Graffiti.Geo.GuessTimezone(GeoRegionInfo.Find(name));
			if (timezone != null) return GetTimezone(timezone?.TZID ?? "");

			timezone = Graffiti.Geo.GuessTimezone(GeoCountryInfo.Find(name));
			return GetTimezone(timezone?.TZID ?? "");
		}


		private dynamic MapTimezone(GeoTimezoneInfo t)
		{
			dynamic d = new ExpandoObject();
			d.tzid = t.TZID;
			d.name = t.Name;
			return d;
		}
	}
}