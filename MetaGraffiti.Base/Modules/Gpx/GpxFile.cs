using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Gpx.Data;

namespace MetaGraffiti.Base.Modules.Gpx
{
	public class GpxFile
    {
		private GpxXmlData _data;

		public string Name => _data.Name;
		public string Description => _data.Description;
		public DateTime? Timestamp => _data.Timestamp;
		public decimal Version => _data.Version;
		public string Creator => _data.Creator;

		// optional
		/*
		public string Author;
		public string Email;
		public string Url;
		public string UrlName;
		public string Keywords;
		public DateTime? Created;
		*/


		public List<GpxTrack> Tracks => _data.Tracks;
		//public List<GpxRoute> Routes => _data.Routes;

		public List<GpxPoint> Points { get { return Tracks.SelectMany(x => x.Points).ToList(); } }
		
		public GpxPoint FirstPoint { get { return Points.OrderBy(x => x.Timestamp).FirstOrDefault(); } }
		public GpxPoint LastPoint { get { return Points.OrderByDescending(x => x.Timestamp).FirstOrDefault(); } }

		
		// inferrered
		//public GeoPerimeter Bounds { get { return new GeoPerimeter(Positions); } }
		public List<IGeoLocation> Positions { get { return Tracks.SelectMany(x => x.Points).ToList<IGeoLocation>(); } }
		//public GeoDistance LinearDistance { get { return CalculateDistance(); } }
		//public GeoDistance ActualDistance { get { return CalculateDistance(true); } }

		/// <summary>
		/// Total elapsed time from first to last point recorded
		/// </summary>
		public TimeSpan ElapsedTime { get { return TimeSpan.FromSeconds(LastPoint.Timestamp.Value.Subtract(FirstPoint.Timestamp.Value).TotalSeconds); } }
		
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

		public GpxStats Satellites { get { return new GpxStats(Points.Select(x => x.Sats).ToArray()); } }
		public GpxStats HDOP { get { return new GpxStats(Points.Select(x => x.HDOP).ToArray()); } }
		public GpxStats VDOP { get { return new GpxStats(Points.Select(x => x.VDOP).ToArray()); } }
		public GpxStats PDOP { get { return new GpxStats(Points.Select(x => x.PDOP).ToArray()); } }
		public GpxStats Velocity { get { return new GpxStats(Points.Select(x => x.Speed).ToArray()); } }
		public GpxStats Elevation { get { return new GpxStats(Points.Select(x => SafeConvert.ToDecimalNull(x.Elevation)).ToArray()); } }


		public void Load(string uri)
		{
			_data = new GpxXmlData();
			_data.ReadXml(uri);
		}

		public void Save(string uri)
		{
			_data.WriteXml(uri);
		}






		public void PurgePointsByDOP(decimal maxDOP = 10)
		{
			PurgePoints(maxDOP);
		}

		public void PurgePointsByGPS(decimal minSatellites = 10)
		{
			PurgePoints(null, minSatellites, true);
		}

		public void PurgePoints(decimal? maxDOP = null, decimal? minSatellites = null, bool requireGPS = false)
		{
			foreach (var t in Tracks)
			{
				var remove = new List<GpxPoint>();
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

					if (bad) remove.Add(p);
				}
				if (remove.Count > 0)
				{
					foreach (var b in remove)
					{
						t.Points.Remove(b);
					}
				}
			}
		}
	}
}
