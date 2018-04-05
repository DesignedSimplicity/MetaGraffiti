using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Services;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TopoTrailFormModel : ITopoTrailUpdateRequest
	{
		// ==================================================
		// Required
		public string Key { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public string Keywords { get; set; }

		public string UrlLink { get; set; }
		public string UrlText { get; set; }

		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string Location { get; set; }


		// ==================================================
		// Helpers
		public static bool IsTimezoneValid(string timezone) { return (GeoTimezoneInfo.Find(timezone)?.Key ?? "UTC") != "UTC;"; }
		public static bool IsCountryValid(string country) { return GeoCountryInfo.Find(country) != null; }
		public static bool IsRegionValid(string country, string region)
		{
			if (!IsCountryValid(country) || String.IsNullOrWhiteSpace(region)) return true; // no country or empty region
			var r = GeoRegionInfo.Find(region);
			var c = GeoCountryInfo.Find(country);
			return (r != null && r.CountryID == (c?.CountryID ?? r.CountryID));
		}


		// ==================================================
		// Constructors
		public TopoTrailFormModel() { }
		public TopoTrailFormModel(ITopoTrailInfo trail)
		{
			if (trail == null) return;

			Key = trail.Key;
			Name = trail.Name;
			Description = trail.Description;
			Keywords = trail.Keywords;
			UrlLink = trail.UrlLink;
			UrlText = trail.UrlText;

			Timezone = trail.Timezone?.TZID ?? "";
			Country = trail.Country?.Name ?? "";
			Region = trail.Region?.RegionName ?? "";
			Location = trail.Location;
		}
	}
}