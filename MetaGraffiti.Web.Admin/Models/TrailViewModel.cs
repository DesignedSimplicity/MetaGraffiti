using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;

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
		public IEnumerable<CartoPlaceInfo> Places { get; set; }

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
	}
}