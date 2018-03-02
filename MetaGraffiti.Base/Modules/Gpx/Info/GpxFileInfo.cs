using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Gpx.Data;

namespace MetaGraffiti.Base.Modules.Gpx.Info
{
	public class GpxFileInfo
	{
		private GpxFileData _data;

		public GpxFileInfo(string uri)
		{
			Uri = uri;
			Load(uri);
		}

		public string Uri { get; private set; }
		public string Name { get { return (String.IsNullOrWhiteSpace(_data.Name) ? Path.GetFileNameWithoutExtension(Uri) : _data.Name); } }
		public string Description => _data.Description;
		public DateTime? Timestamp => _data.Timestamp;

		public List<GpxTrackData> Tracks => _data.Tracks;

		public IEnumerable<GpxPointData> Points { get { return Tracks.SelectMany(x => x.Points); } }

		/* TODO: make this find the earliest timestamp
		public GpxPointData FirstPoint
		{
			get
			{
				var point = Tracks.First().Points.First();
				if (point == null) point = _data.Routes.First().Points.First();
				if (point == null) point = _data.Waypoints.First();
				return point;
			}
		}
		*/

		/// <summary>
		/// Total elapsed time from first to last point recorded
		/// </summary>
		public TimeSpan ElapsedTime { get { return TimeSpan.FromSeconds(Points.Max(x => x.Timestamp.Value).Subtract(Points.Min(x => x.Timestamp.Value)).TotalSeconds); } }

		/// <summary>
		/// Total sum of time between recorded points excluding periods with no recording
		/// </summary>
		public TimeSpan RecordedTime
		{
			get
			{
				double s = 0;
				foreach (var t in Tracks)
				{
					var f = t.Points.OrderBy(x => x.Timestamp).First();
					var l = t.Points.OrderByDescending(x => x.Timestamp).First();
					s += l.Timestamp.Value.Subtract(f.Timestamp.Value).TotalSeconds;
				}
				return TimeSpan.FromSeconds(s);
			}
		}

		public GpxStatData Satellites { get { return new GpxStatData(Points.Select(x => x.Sats).ToArray()); } }
		public GpxStatData HDOP { get { return new GpxStatData(Points.Select(x => x.HDOP).ToArray()); } }
		public GpxStatData VDOP { get { return new GpxStatData(Points.Select(x => x.VDOP).ToArray()); } }
		public GpxStatData PDOP { get { return new GpxStatData(Points.Select(x => x.PDOP).ToArray()); } }
		public GpxStatData Velocity { get { return new GpxStatData(Points.Select(x => x.Speed).ToArray()); } }
		public GpxStatData Elevation { get { return new GpxStatData(Points.Select(x => TypeConvert.ToDecimalNull(x.Elevation)).ToArray()); } }


		private void Load(string uri)
		{
			var reader = new GpxFileReader(uri);
			_data = reader.ReadFile();
		}

		/*
		public List<GpxPointData> ListPointsByMaxDOP(decimal maxDOP = 10)
		{
			return ListPoints(maxDOP);
		}

		public List<GpxPointData> ListPointsByMinSatellites(decimal minSatellites = 10)
		{
			return ListPoints(null, minSatellites, true);
		}

		public List<GpxPointData> ListPoints(decimal? maxDOP = null, decimal? minSatellites = null, bool requireGPS = false)
		{
			var list = new List<GpxPointData>();
			foreach (var t in Tracks)
			{
				foreach (var p in t.Points)
				{
					bool bad = false;
					if (maxDOP.HasValue)
					{
						if (!p.HDOP.HasValue && !p.VDOP.HasValue && !p.PDOP.HasValue)
							bad = true; // no dilution of precision values
						else if (p.MaxDOP > maxDOP.Value)
							bad = true; // DOP exceeds max value
					}

					if (minSatellites.HasValue)
					{
						if (!p.Sats.HasValue)
							bad = true; // no satellite value
						else if (p.Sats.Value < minSatellites.Value)
							bad = true; // insufficient number of satellite
					}

					if (requireGPS)
					{
						if (p.Source != "gps") bad = true; // not from GPS satellite
					}

					if (!bad) list.Add(p);
				}
			}
			return list;
		}
		*/
	}
}
