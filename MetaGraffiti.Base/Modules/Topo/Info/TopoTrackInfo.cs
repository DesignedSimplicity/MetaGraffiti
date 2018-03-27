using System;
using System.Collections.Generic;
using System.Linq;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoTrackInfo
	{
		private GpxTrackData _track;

		public TopoTrailInfo Trail { get; private set; }

		public TopoTrackInfo(TopoTrailInfo trail, GpxTrackData track)
		{
			Trail = trail;
			_track = track;
		}


		private List<TopoPointInfo> _points;
		public List<TopoPointInfo> Points { get { return ListPoints(); } }


		public string Name => _track.Name;
		public string Source => _track.Source;
		public string Description => _track.Description;


		public DateTime StartTime { get { return Points.First().LocalTime; } }
		public DateTime FinishTime { get { return Points.Last().LocalTime; } }

		public DateTime FirstTimestamp { get { return Points.First().Timestamp; } }
		public DateTime LastTimestamp { get { return Points.Last().Timestamp; } }


		public TimeSpan ElapsedTime { get { return FinishTime.Subtract(StartTime); } }
		public string ElapsedTimeText { get { return String.Format("{0:0} h {1:00} m", Math.Floor(ElapsedTime.TotalHours), ElapsedTime.Minutes); } }


		private GeoDistance _distance = null;
		public GeoDistance EstimatedDistance { get { if (_distance == null) _distance = GeoDistance.BetweenPoints(_track.Points, false); return _distance; } }
		public double EstimatedKmh { get { return EstimatedDistance.KM / ElapsedTime.TotalHours; } }



		public double EstimatedKilometers { get { if (_distance == null) _distance = GeoDistance.BetweenPoints(_track.Points, true); return _distance.KM; } }

		public double EstimatedAscent { get { return GeoDistance.ElevationBetweenPoints(_track.Points, 1).Meters; } }
		public double EstimatedDescent { get { return GeoDistance.ElevationBetweenPoints(_track.Points, -1).Meters; } }


		private string[] _autoTags;
		public string[] AutoTags { get { return GetAutoTags();} }



		public CartoPlaceInfo StartPlace { get; set; }
		public CartoPlaceInfo FinishPlace { get; set; }
		//public List<CartoPlaceInfo> ViaPlaces { get; set; }




		private string[] GetAutoTags()
		{
			if (_autoTags != null) return _autoTags;

			var tags = new List<string>();
			var points = _track.Points;
			var count = points.Count();

			// is closed loop
			var loop = GeoDistance.BetweenPoints(points.First(), points.Last());
			if (loop.Meters <= 200) tags.Add("Loop");

			// average speed
			if (EstimatedKmh <= 5) // km/h
				tags.Add("Walk");
			else if (EstimatedKmh <= 15) // km/h
				tags.Add("Bike");

			// max speed (of data)
			var max = points.Max(x => x.Speed ?? 0);
			if (max > 5) tags.Add("Fast"); // m/s

			// number of stops
			var stops = _track.Points.Count(x => x.Speed == 0);
			if ((stops / count) >= 0.01) tags.Add("Stops"); // 1%

			// few options
			if (count < 30) tags.Add("Short");

			// bad data points
			var bad = max > 33; // m/s
			if (!bad) bad = points.Average(x => (x.Sats ?? 0)) <= 5;
			if (!bad) bad = points.Count(x => !x.HDOP.HasValue) >= 20; // TODO: fix this threshold for old files without DOP data
			if (bad) tags.Add("Bad");

			// return tags
			_autoTags = tags.ToArray();
			return _autoTags;
		}

		private List<TopoPointInfo> ListPoints()
		{
			if (_points != null) return _points;

			_points = new List<TopoPointInfo>();
			foreach (var p in _track.Points.OrderBy(x => x.Timestamp))
			{
				_points.Add(new TopoPointInfo(this, p));
			}
			return _points;
		}
	}
}