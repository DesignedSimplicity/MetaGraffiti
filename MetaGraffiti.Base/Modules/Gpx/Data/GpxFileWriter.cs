using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
	public class GpxFileWriter
	{
		public bool _header = false;

		public GpxFileWriter(string uri)
		{
		}

		public GpxFileWriter(Stream uri)
		{
		}

		public decimal Version { get; private set; }
		public string Creator { get; private set; }

		public void WriteHeader(string name, string description = null, DateTime? timestamp = null)
		{
			var header = new GpxFileHeader()
			{
				Name = name,
				Description = description,
				Timestamp = timestamp
			};
			WriteHeader(header);
		}

		public void WriteHeader(GpxFileHeader header)
		{
			_header = true;


		}

		public void WriteTrack(string name, string description = null)
		{
		}

		public void WriteTrack(GpxTrackData track)
		{
		}

		public void WriteRoute(GpxRouteData route)
		{
		}

		public void WritePoint(GpxPointData point)
		{
		}

		public void WritePoint(IGeoLocation point)
		{
		}

		public void WritePoint(double latitude, double longitude, double? elevation, DateTime? timestamp)
		{
		}

		public void WritePoints(IEnumerable<GpxPointData> points)
		{
		}

		public void WritePoints(IEnumerable<IGeoLocation> points)
		{
		}

		public void WriteWaypoint(GpxPointData waypoint)
		{
		}

		public void WriteWaypoints(IEnumerable<GpxPointData> waypoints)
		{
		}
	}
}
