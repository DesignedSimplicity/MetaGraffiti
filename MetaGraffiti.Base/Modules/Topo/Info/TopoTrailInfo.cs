using System;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoTrailInfo
	{
		public string ID { get; set; }

		public string Uri { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Keywords { get; set; }

		public string Url { get; set; }
		public string UrlName { get; set; }


		public DateTime LocalDate { get; set; }

		public GeoCountryInfo Country { get; set; }

		public GeoRegionInfo Region { get; set; }

		public GeoTimezoneInfo Timezone { get; set; }


		public List<TopoTrackInfo> Tracks { get; set; }
	}
}
