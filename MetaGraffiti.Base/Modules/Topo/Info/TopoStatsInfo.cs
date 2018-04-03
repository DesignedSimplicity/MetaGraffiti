using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
    public class TopoStatsInfo
    {
		public TopoStatsInfo(TopoTrailInfo trail)
		{

		}

		public TopoStatsInfo(TopoTrackInfo track)
		{
			ElapsedTime = track.FinishTime.Subtract(track.StartTime);
			Distance = GeoDistance.BetweenPoints(track.GeoPoints, false);
			DistanceWithElevation = GeoDistance.BetweenPoints(track.GeoPoints, true);
			EstimatedMetersAscent = GeoDistance.ElevationBetweenPoints(track.GeoPoints, 1).Meters;
			EstimatedMetersDescent = GeoDistance.ElevationBetweenPoints(track.GeoPoints, -1).Meters;
		}

		public TimeSpan ElapsedTime { get; private set; }
		public GeoDistance Distance { get; private set; }
		public GeoDistance DistanceWithElevation { get; private set; }

		public double EstimatedMetersAscent { get; private set; }
		public double EstimatedMetersDescent { get; private set; }
		public double EstimatedKM { get { return DistanceWithElevation.KM; } }
		public double EstimatedKPH { get { return DistanceWithElevation.KM / ElapsedTime.TotalHours; } }
	}
}
