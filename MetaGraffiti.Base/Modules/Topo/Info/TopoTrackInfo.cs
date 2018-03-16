using System;
using System.Collections.Generic;
using System.Text;

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

		public TopoTrackInfo(TopoTrailInfo trail, GpxTrackData track)
		{
			Trail = trail;
			TrackData = track;
		}

		private List<TopoPointInfo> ListPoints()
		{
			if (_points != null) return _points;

			_points = new List<TopoPointInfo>();
			foreach(var p in TrackData.Points)
			{
				_points.Add(new TopoPointInfo(p));
			}
			return _points;
		}
	}

	public class TopoPointInfo
	{
		public GpxPointData PointData { get; private set; }

		public TopoPointInfo(GpxPointData point)
		{
			PointData = point;
		}
	}
}
