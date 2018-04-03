using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Topo
{
    public class TopoStats
    {
		public static TopoStats FromTrail(ITopoTrailInfo trail)
		{
			var stats = new TopoStats();

			var hours = 0.0;
			var ascent = 0.0;
			var descent = 0.0;
			var distance = 0.0;
			var distanceWithElevation = 0.0;
			foreach (var track in trail.Tracks)
			{
				hours += track.FinishUTC.Subtract(track.StartUTC).TotalHours;
				ascent += GeoDistance.ElevationBetweenPoints(track.Points, 1).Meters;
				descent += GeoDistance.ElevationBetweenPoints(track.Points, -1).Meters;
				distance += GeoDistance.BetweenPoints(track.Points, true).Meters;
				distanceWithElevation += GeoDistance.BetweenPoints(track.Points, true).Meters;
			}
			stats.ElapsedTime = TimeSpan.FromHours(hours);
			stats.Distance = GeoDistance.FromMeters(distance);
			stats.DistanceWithElevation = GeoDistance.FromMeters(distanceWithElevation);
			stats.EstimatedMetersAscent = ascent;
			stats.EstimatedMetersDescent = descent;

			return stats;
		}

		public static TopoStats FromTrack(ITopoTrackInfo track)
		{
			var stats = new TopoStats();

			stats.ElapsedTime = track.FinishUTC.Subtract(track.StartUTC);
			stats.Distance = GeoDistance.BetweenPoints(track.Points, true);
			stats.DistanceWithElevation = GeoDistance.BetweenPoints(track.Points, true);
			stats.EstimatedMetersAscent = GeoDistance.ElevationBetweenPoints(track.Points, 1).Meters;
			stats.EstimatedMetersDescent = GeoDistance.ElevationBetweenPoints(track.Points, -1).Meters;

			return stats;
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
