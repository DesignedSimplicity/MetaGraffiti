using MetaGraffiti.Base.Modules.Topo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TopoStatsPillsModel
	{
		public TopoStats Stats { get; set; }

		public bool ShowDetails { get; set; }

		public bool AlignJustify { get; set; }

		public TopoStatsPillsModel(TopoStats stats, bool showDetails = false, bool alignJustify = false)
		{
			Stats = stats;
			ShowDetails = showDetails;
			AlignJustify = alignJustify;
		}

		public TopoStatsPillsModel(IEnumerable<ITopoTrackInfo> tracks, bool showDetails = false, bool fixedWidth = false)
		{
			Stats = TopoStats.FromTracks(tracks);
			ShowDetails = showDetails;
			AlignJustify = fixedWidth;
		}
	}
}