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

		public TopoStatsPillsModel(TopoStats stats, bool showDetails = false)
		{
			Stats = stats;
			ShowDetails = showDetails;
		}
	}
}