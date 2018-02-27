using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
    public class GpxFileData
    {
		public GpxFileHeader Header { get; set; }
		public List<GpxTrackData> Tracks { get; set; }
		public List<GpxRouteData> Routes { get; set; }
		public List<GpxPointData> Waypoints { get; set; }
	}
}
