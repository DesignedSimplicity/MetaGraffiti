using System;
using System.Collections.Generic;
using System.Linq;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Topo
{
    public class TopoStats
    {
		public static TopoStats FromTrail(ITopoTrailInfo trail)
		{
			return FromTracks(trail.Tracks);
		}

		public static TopoStats FromTracks(IEnumerable<ITopoTrackInfo> tracks)
		{
			var stats = new TopoStats();
			if ((tracks?.Count() ?? 0) == 0) return stats;

			var points = 0;
			var hours = 0.0;
			var ascent = 0.0;
			var descent = 0.0;
			var distance = 0.0;
			var distanceWithElevation = 0.0;
			var start = DateTime.MaxValue;
			var finish = DateTime.MinValue;
			foreach (var track in tracks)
			{
				points += track.TopoPoints.Count();
				hours += track.FinishUTC.Subtract(track.StartUTC).TotalHours;
				ascent += GeoDistance.ElevationBetweenPoints(track.TopoPoints, 1).Meters;
				descent += GeoDistance.ElevationBetweenPoints(track.TopoPoints, -1).Meters;
				distance += GeoDistance.BetweenPoints(track.TopoPoints, true).Meters;
				distanceWithElevation += GeoDistance.BetweenPoints(track.TopoPoints, true).Meters;
				if (track.TopoPoints.First().LocalTime < start) start = track.TopoPoints.First().LocalTime;
				if (track.TopoPoints.Last().LocalTime > finish) finish = track.TopoPoints.Last().LocalTime;
			}

			if (points == 0) return stats;

			stats.ElapsedTime = TimeSpan.FromHours(hours);
			stats.Distance = GeoDistance.FromMeters(distance);
			stats.DistanceWithElevation = GeoDistance.FromMeters(distanceWithElevation);
			stats.EstimatedMetersAscent = ascent;
			stats.EstimatedMetersDescent = descent;
			stats.SecondsBetweenPoints = (hours * 60 * 60) / points;
			stats.PointCount = points;
			stats.DayCount = (finish.DayOfYear - start.DayOfYear) + 1;

			return stats;
		}

		public static TopoStats FromTrack(ITopoTrackInfo track)
		{
			var stats = new TopoStats();
			if (track == null || track.TopoPoints.Count() == 0) return stats;

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

		public int DayCount { get; private set; } = 0;
		public int PointCount { get; private set; } = 0;
		public double SecondsBetweenPoints { get; private set; } = 0;

		public TimeSpan ElapsedTime { get; private set; } = TimeSpan.FromHours(0);
		public GeoDistance Distance { get; private set; } = GeoDistance.FromMeters(0);
		public GeoDistance DistanceWithElevation { get; private set; } = GeoDistance.FromMeters(0);

		public double EstimatedMetersAscent { get; private set; }
		public double EstimatedMetersDescent { get; private set; }
		public double EstimatedKM { get { return DistanceWithElevation.KM; } }
		public double EstimatedKPH { get { return DistanceWithElevation.KM / ElapsedTime.TotalHours; } }
	}
}
