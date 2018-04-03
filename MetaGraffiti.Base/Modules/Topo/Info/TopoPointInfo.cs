using System;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoPointInfo : IGeoPoint
	{
		private GpxPointData _point;

		public TopoTrackInfo Track { get; private set; }


		public TopoPointInfo(TopoTrackInfo track, GpxPointData point)
		{
			Track = track;
			_point = point;
		}


		public double Latitude => _point.Latitude;

		public double Longitude => _point.Longitude;

		public double? Elevation => _point.Elevation;

		public DateTime? Timestamp => _point.Timestamp;

		public DateTime LocalTime => Track.Trail.Timezone.FromUTC(_point.Timestamp.Value);
	}
}
