﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GeoViewModel : AdminViewModel
	{
		public List<GeoTimezoneInfo> Timezones { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }
		public List<GeoRegionInfo> Regions { get; set; }

		public List<GeoLocationInfo> Locations { get; set; }

		public GeoLocationInfo SelectedLocation { get; set; }

		public GeoCountryInfo SelectedCountry { get; set; }

		public List<GeoCountryInfo> VisitedCountries { get; set; }


		public GeoSearchModel Search { get; set; } = new GeoSearchModel();



		public HtmlString GetJson(GeoLocationInfo location)
		{
			if (location == null) return new HtmlString("{}");

			return new HtmlString(location.ToJson());
		}
	}

	public class GeoSearchModel
	{
		public string Name { get; set; }
		
		public string Region { get; set; }

		public string Country { get; set; }

		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}