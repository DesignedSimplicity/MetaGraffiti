using System;

using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoPointInfo
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

		public DateTime Timestamp => _point.Timestamp.Value;

		public DateTime LocalTime => Track.Trail.Timezone.FromUTC(_point.Timestamp.Value);
	}
}
