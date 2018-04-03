using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Topo
{
	public interface ITopoTrailBase
	{
		string Name { get; }
		string Description { get; }

		string Keywords { get; }
		string UrlLink { get; }
		string UrlText { get; }
	}

	public interface ITopoTrailData : ITopoTrailBase
	{
		string Location { get; }
		GeoRegionInfo Region { get; }
		GeoCountryInfo Country { get; }
		GeoTimezoneInfo Timezone { get; }
	}

	public interface ITopoTrailInfo : ITopoTrailData
	{
		string Key { get; }
		string Source { get; }

		DateTime StartUTC { get; }
		DateTime FinishUTC { get; }

		IEnumerable<ITopoTrackInfo> Tracks { get; }
	}

	public interface ITopoTrackData
	{
		string Key { get; }
		string Source { get; }

		string Name { get; }
		string Description { get; }

		IEnumerable<IGeoPoint> Points { get; }
	}

	public interface ITopoTrackInfo
	{
		ITopoTrailInfo Trail { get; }

		//string Key { get; }
		string Source { get; }

		string Name { get; }
		string Description { get; }

		DateTime StartUTC { get; }
		DateTime StartLocal { get; }

		DateTime FinishUTC { get; }
		DateTime FinishLocal { get; }

		TimeSpan ElapsedTime { get; }


		CartoPlaceInfo StartPlace { get; }
		CartoPlaceInfo FinishPlace { get; }

		IEnumerable<ITopoPointInfo> Points { get; }
	}


	public interface ITopoPointInfo : IGeoPoint
	{
		ITopoTrackInfo Track { get; }

		DateTime LocalTime { get; }

		string Description { get; }
	}
}
