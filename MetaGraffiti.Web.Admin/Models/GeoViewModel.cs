﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GeoViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public List<GeoTimezoneInfo> Timezones { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }
		public List<GeoRegionInfo> Regions { get; set; }
		public List<GeoCountryInfo> Visited { get; set; }


		// ==================================================
		// Optional
		public GeoCountryInfo SelectedCountry { get; set; }


		// ==================================================
		// Helpers
		public bool IsVisited(GeoCountryInfo country)
		{
			return Visited.Any(x => x.CountryID == country.CountryID);
		}


		// ==================================================
		// Navigation
		public static string GetGeoUrl() { return "/geo/"; }
		public static string GetTimezonesUrl() { return "/geo/timezones/"; }
		public static string GetTimezoneUrl(GeoTimezoneInfo timezone) { return $"/geo/timezones/#{timezone.GeoTZID}"; }
		public static string GetCountriesUrl() { return "/geo/countries/"; }
		public static string GetCountryUrl(GeoCountryInfo country) { return $"/geo/country/{country.CountryID}/"; }
		public static string GetRegionsUrl(GeoCountryInfo country = null) { return "/geo/regions/" + (country == null ? "" : $"#{country.ISO3}"); }
	}
}