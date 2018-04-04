using System;
using System.Linq;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Topo
{
    public class TopoStats
    {
		public static TopoStats FromTrail(ITopoTrailInfo trail)
		{
			var stats = new TopoStats();

			var points = 0;
			var hours = 0.0;
			var ascent = 0.0;
			var descent = 0.0;
			var distance = 0.0;
			var distanceWithElevation = 0.0;
			foreach (var track in trail.Tracks)
			{
				points += track.TopoPoints.Count();
				hours += track.FinishUTC.Subtract(track.StartUTC).TotalHours;
				ascent += GeoDistance.ElevationBetweenPoints(track.TopoPoints, 1).Meters;
				descent += GeoDistance.ElevationBetweenPoints(track.TopoPoints, -1).Meters;
				distance += GeoDistance.BetweenPoints(track.TopoPoints, true).Meters;
				distanceWithElevation += GeoDistance.BetweenPoints(track.TopoPoints, true).Meters;
			}
			stats.ElapsedTime = TimeSpan.FromHours(hours);
			stats.Distance = GeoDistance.FromMeters(distance);
			stats.DistanceWithElevation = GeoDistance.FromMeters(distanceWithElevation);
			stats.EstimatedMetersAscent = ascent;
			stats.EstimatedMetersDescent = descent;
			stats.SecondsBetweenPoints = (hours * 60 * 60) / points;
			stats.PointCount = points;
			stats.DayCount = trail.FinishLocal.DayOfYear - trail.StartLocal.DayOfYear;

			return stats;
		}

		public static TopoStats FromTrack(ITopoTrackInfo track)
		{
			var stats = new TopoStats();

			stats.ElapsedTime = track.FinishUTC.Subtract(track.StartUTC);
			stats.Distance = GeoDistance.BetweenPoints(track.TopoPoints, true);
			stats.DistanceWithElevation = GeoDistance.BetweenPoints(track.TopoPoints, true);
			stats.EstimatedMetersAscent = GeoDistance.ElevationBetweenPoints(track.TopoPoints, 1).Meters;
			stats.EstimatedMetersDescent = GeoDistance.ElevationBetweenPoints(track.TopoPoints, -1).Meters;
			stats.SecondsBetweenPoints = stats.ElapsedTime.TotalSeconds / track.TopoPoints.Count();
			stats.PointCount = track.TopoPoints.Count();
			stats.DayCount = track.FinishLocal.DayOfYear - track.StartLocal.DayOfYear;

			return stats;
		}

		public int DayCount { get; private set; }
		public int PointCount { get; private set; }
		public double SecondsBetweenPoints { get; private set; }

		public TimeSpan ElapsedTime { get; private set; }
		public GeoDistance Distance { get; private set; }
		public GeoDistance DistanceWithElevation { get; private set; }

		public double EstimatedMetersAscent { get; private set; }
		public double EstimatedMetersDescent { get; private set; }
		public double EstimatedKM { get { return DistanceWithElevation.KM; } }
		public double EstimatedKPH { get { return DistanceWithElevation.KM / ElapsedTime.TotalHours; } }
	}
}
