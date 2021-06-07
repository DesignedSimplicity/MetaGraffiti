using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

// TODO: DEPRECATE cleanup after purge of GpxController
// TODO:ORTHO:REMOVE
namespace MetaGraffiti.Base.Modules.Ortho.Info
{
	public class GpxFileInfo
	{
		private GpxFileData _data;

		public GpxFileInfo(string uri)
		{
			Uri = uri;
			Load(uri);
		}


		public Exception Error { get; private set; }
		public bool Valid { get { return Error == null; } }

		public string Uri { get; private set; }
		public string Name { get { return (String.IsNullOrWhiteSpace(_data.Name) ? Path.GetFileNameWithoutExtension(Uri) : _data.Name); } }
		public string Description => _data.Description;
		public DateTime? Timestamp => _data.Timestamp;

		public List<GpxTrackData> Tracks => _data.Tracks;

		public IEnumerable<GpxPointData> Points { get { return Tracks.SelectMany(x => x.PointData); } }


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
					var f = t.PointData.OrderBy(x => x.Timestamp).First();
					var l = t.PointData.OrderByDescending(x => x.Timestamp).First();
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
			try
			{
				var reader = new GpxFileReader(uri);
				_data = reader.ReadFile();
			}
			catch (Exception ex)
			{
				Error = ex;
			}
		}
	}
}
