using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho.Data;

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

		public GeoTimezoneInfo Timezone { get; set; }

		
		public List<TopoTrackInfo> Tracks { get; set; }
	}

	public class TopoTrackInfo
	{
		public TopoTrailInfo Trail { get; private set; }
		public GpxTrackData TrackData { get; private set; }

		private List<TopoPointInfo> _points;
		public List<TopoPointInfo> Points { get { return ListPoints(); } }


		public string Name => TrackData.Name;
		public string Description => TrackData.Description;
		public string Source => TrackData.Source;

		public DateTime StartTime { get { return Points.First().Timestamp; } }
		public DateTime FinishTime { get { return Points.Last().Timestamp; } }

		public TimeSpan ElapsedTime { get { return FinishTime.Subtract(StartTime); } }
		public string ElapsedTimeText { get { var ts = ElapsedTime; return String.Format("{0:0} hr{1} {2:0} min{3}", Math.Floor(ts.TotalHours), (Math.Floor(ts.TotalHours) == 1 ? "" : "s"), ts.Minutes, (ts.Minutes == 1 ? "" : "s")); } }
		public double EstimatedKilometers { get { return GeoDistance.BetweenPoints(TrackData.Points, true).KM; } }

		public double EstimatedAscent { get { return GeoDistance.ElevationBetweenPoints(TrackData.Points, 1).Meters; } }
		public double EstimatedDescent { get { return GeoDistance.ElevationBetweenPoints(TrackData.Points, -1).Meters; } }

		public TopoTrackInfo(TopoTrailInfo trail, GpxTrackData track)
		{
			Trail = trail;
			TrackData = track;
		}

		private List<TopoPointInfo> ListPoints()
		{
			if (_points != null) return _points;

			_points = new List<TopoPointInfo>();
			foreach(var p in TrackData.Points.OrderBy(x => x.Timestamp))
			{
				_points.Add(new TopoPointInfo(this, p));
			}
			return _points;
		}
	}

	public class TopoPointInfo
	{
		public TopoTrackInfo Track { get; private set; }
		public GpxPointData PointData { get; private set; }

		public double Latitude => PointData.Latitude;

		public double Longitude => PointData.Longitude;

		public DateTime Timestamp
		{
			get { return Track.Trail.Timezone.FromUTC(PointData.Timestamp.Value); }
		}

		public TopoPointInfo(TopoTrackInfo track, GpxPointData point)
		{
			Track = track;
			PointData = point;
		}
	}
}
