using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TrailViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public TopoTrailInfo Trail { get; set; }


		// ==================================================
		// Optional



		// ==================================================
		// Helpers
		public bool IsTimezoneValid { get { return Trail.Timezone != null && Trail.Timezone.Key != "UTC;"; } }
		public bool IsCountryValid { get { return Trail.Country != null; } }
		public bool IsRegionValid { get { return IsCountryValid && Trail.Country.HasRegions && Trail.Region != null; } }




		// ==================================================
		// Navigation

		public static string GetTrailUrl(string key) { return $"/trail/display/{key}"; }
		public static string GetTrailUrl(ITopoTrailInfo trail) { return GetTrailUrl(trail.Key); }

		public static string GetUpdateUrl(ITopoTrailInfo trail) { return $"/trail/update/{trail.Key}"; }
		public static string GetModifyUrl(ITopoTrailInfo trail) { return $"/trail/modify/{trail.Key}"; }


		public static string GetUpdateUrl() { return $"/trail/update/"; }
		public static string GetModifyUrl() { return $"/trail/modify/"; }
		public static string GetImportUrl() { return $"/trail/import/"; }
	}
}