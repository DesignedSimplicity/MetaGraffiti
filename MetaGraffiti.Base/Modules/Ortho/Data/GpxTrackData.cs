using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	/*
	<trkpt lat="-41.2420763" lon="174.78955016">
		<ele>193.0</ele>
		<time>2016-01-16T22:48:25Z</time>
		<speed>0.0</speed>
		<src>gps</src>
		<sat>21</sat>
		<hdop>1.0</hdop>
		<vdop>0.9</vdop>
		<pdop>1.3</pdop>
		<geoidheight>17.0</geoidheight>
	</trkpt>
	*/

	public class GpxTrackData : GpxRouteData, IGpxTrack
	{
		public IList<IGpxPoint> Points => PointData.ToList<IGpxPoint>();
	}
}
