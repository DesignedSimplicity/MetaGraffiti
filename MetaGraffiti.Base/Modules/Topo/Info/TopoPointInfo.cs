using System;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoPointInfo : ITopoPointInfo
	{
		private ITopoTrackInfo _track;
		private IGpxPoint _point;

		public TopoPointInfo(ITopoTrackInfo track, IGpxPoint point)
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

		public DateTime LocalTime => GetTimezone().FromUTC(_point.Timestamp.Value);

		private GeoTimezoneInfo GetTimezone()
		{
			return _track.Trail?.Timezone ?? GeoTimezoneInfo.UTC;
		}
	}
}
