using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoTrackInfo
	{
		private GpxTrackData _track;

		public TopoTrailInfo Trail { get; private set; }		

		private List<TopoPointInfo> _points;
		public List<TopoPointInfo> Points { get { return ListPoints(); } }


		public string Name => _track.Name;
		public string Description => _track.Description;
		public string Source => _track.Source;

		public DateTime StartTime { get { return Points.First().Timestamp; } }
		public DateTime FinishTime { get { return Points.Last().Timestamp; } }

		public TimeSpan ElapsedTime { get { return FinishTime.Subtract(StartTime); } }
		public string ElapsedTimeText { get { var ts = ElapsedTime; return String.Format("{0:0} hr{1} {2:0} min{3}", Math.Floor(ts.TotalHours), (Math.Floor(ts.TotalHours) == 1 ? "" : "s"), ts.Minutes, (ts.Minutes == 1 ? "" : "s")); } }
		public double EstimatedKilometers { get { return GeoDistance.BetweenPoints(_track.Points, true).KM; } }

		public double EstimatedAscent { get { return GeoDistance.ElevationBetweenPoints(_track.Points, 1).Meters; } }
		public double EstimatedDescent { get { return GeoDistance.ElevationBetweenPoints(_track.Points, -1).Meters; } }

		public TopoTrackInfo(TopoTrailInfo trail, GpxTrackData track)
		{
			Trail = trail;
			_track = track;
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