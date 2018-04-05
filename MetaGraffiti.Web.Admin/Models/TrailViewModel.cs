using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TrailViewModel : AdminViewModel
	{
		// ==================================================
		// Globals
		public enum MergeConfirmTypes { Intent, Combine, Discard }


		// ==================================================
		// Required
		public TopoTrailInfo Trail { get; set; }
		public IEnumerable<TopoTrackInfo> Tracks { get; set; }
		public ITopoTrailUpdateRequest Edit { get; set; }


		// ==================================================
		// Optional



		// ==================================================
		// Helpers
		public bool HasTracks { get { return (Tracks?.Count() ?? 0) > 0; } }
		public string GetSourceName(ITopoTrackInfo track) { return Path.GetFileNameWithoutExtension(track.Source); }

		// ==================================================
		// Navigation

		public static string GetTrailUrl(string key) { return $"/trail/display/{key}/"; }
		public static string GetTrailUrl(ITopoTrailInfo trail) { return GetTrailUrl(trail.Key); }

		public static string GetUpdateUrl(ITopoTrailInfo trail) { return $"/trail/update/{trail.Key}/"; }
		public static string GetModifyUrl(ITopoTrailInfo trail) { return $"/trail/modify/{trail.Key}/"; }
		public static string GetModifyUrl(ITopoTrailInfo trail, MergeConfirmTypes confirm) { return $"/trail/modify/{trail.Key}/?confirm={confirm}"; }

		public static string GetImportUrl() { return $"/trail/import/"; }
		public static string GetDiscardUrl() { return $"/trail/discard/"; }

		//public static string GetUpdateUrl() { return $"/trail/update/"; }
		//public static string GetModifyUrl() { return $"/trail/modify/"; }

	}

	public class TopoTrailFormModel : ITopoTrailUpdateRequest
	{
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

		public static bool IsTimezoneValid(string timezone) { return (GeoTimezoneInfo.Find(timezone)?.Key ?? "UTC") != "UTC;"; }
		public static bool IsCountryValid(string country) { return GeoCountryInfo.Find(country) != null; }
		public static bool IsRegionValid(string country, string region)
		{
			if (!IsCountryValid(country) || String.IsNullOrWhiteSpace(region)) return true; // no country or empty region
			var r = GeoRegionInfo.Find(region);
			var c = GeoCountryInfo.Find(country);
			return (r != null && r.CountryID == (c?.CountryID ?? r.CountryID));
		}
	}
}