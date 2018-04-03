using System;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoPointInfo : ITopoPointInfo
	{
		private ITopoTrackInfo _track;
		private IGeoPoint _point;

		public TopoPointInfo(ITopoTrackInfo track, IGeoPoint point)
		{
			_track = track;
			_point = point;
		}

		public string Description { get; set; }

		public ITopoTrackInfo Track => _track;

		public double Latitude => _point.Latitude;

		public double Longitude => _point.Longitude;

		public double? Elevation => _point.Elevation;

		public DateTime? Timestamp => _point.Timestamp;

		public DateTime LocalTime => _track.Trail.Timezone.FromUTC(_point.Timestamp.Value);
	}
}
