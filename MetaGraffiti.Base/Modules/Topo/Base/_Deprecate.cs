using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public interface ITrailInfo
	{
		string Name { get; }
		string Description { get; }
		string Keywords { get; }
		string UrlLink { get; }
		string UrlText { get; }

		string Location { get; }
		GeoRegionInfo Region { get; }
		GeoCountryInfo Country { get; }
		GeoTimezoneInfo Timezone { get; }

		/*
		TimeSpan ElapsedTime { get; }
		GeoDistance Distance { get; }
		*/
	}
}
