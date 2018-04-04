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
		public TopoTrailInfo SelectedTrail { get; set; }


		// ==================================================
		// Optional



		// ==================================================
		// Helpers

		public IEnumerable<CartoPlaceInfo> ConsolidatePlaces(TopoTrailInfo trail)
		{
			var places = new List<CartoPlaceInfo>();
			foreach (var track in trail.TopoTracks)
			{
				var start = track.StartPlace;
				if (start != null && !places.Any(x => x.Key == start.Key)) places.Add(start);

				var finish = track.FinishPlace;
				if (finish != null && !places.Any(x => x.Key == finish.Key)) places.Add(finish);
			}
			return places;
		}



		// ==================================================
		// Navigation

		public static string GetTrailUrl(string key) { return $"/trail/display/{key}"; }
		public static string GetTrailUrl(ITopoTrailInfo trail) { return GetTrailUrl(trail.Key); }



		public static string GetUpdateUrl() { return $"/trail/update/"; }
		public static string GetModifyUrl() { return $"/trail/modify/"; }
		public static string GetImportUrl() { return $"/trail/import/"; }
	}
}