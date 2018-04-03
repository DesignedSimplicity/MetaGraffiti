using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TrailViewModel : AdminViewModel
	{
		public TopoTrailInfo Trail { get; set; }
		public List<TopoTrackInfo> Tracks { get; set; }

		public bool IsTimezoneValid { get { return Trail.Timezone != null && Trail.Timezone.Key != "UTC;"; } }
		public bool IsCountryValid { get { return Trail.Country != null; } }
		public bool IsRegionValid { get { return IsCountryValid && Trail.Country.HasRegions && Trail.Region != null; } }
	}
}