using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	// ==================================================
	// Enumerations
	public enum GeoContinent { Africa, Antarctica, Asia, Europe, NorthAmerica, Oceania, SouthAmerica }

	public enum GeoTimezoneTypes { Windows = 1, Olson = 2 }

	// ==================================================
	// Interfaces
	public interface IGeoPoint
	{
		double Latitude { get; }
		double Longitude { get; }
	}

	public interface IGeoLocation : IGeoPoint
	{
		double? Elevation { get; }
		DateTime? Timestamp { get; }
	}

	public interface IGeoPerimeter
	{
		IGeoPoint Center { get; }
		IList<IGeoPoint> Points { get; }
	}


	// ==================================================
	// Structures
	
	public class GeoRectangle : IGeoPerimeter
	{
		public virtual IGeoPoint Center { get; private set; }
		public IList<IGeoPoint> Points { get; private set; }

		public IGeoPoint NorthWest { get { return Points[0]; } }
		public IGeoPoint SouthEast { get { return Points[1]; } }

		public double LatitudeMin { get { return Points.Min(x => x.Latitude); } }
		public double LatitudeMax { get { return Points.Max(x => x.Latitude); } }
		public double LongitudeMin { get { return Points.Min(x => x.Longitude); } }
		public double LongitudeMax { get { return Points.Max(x => x.Longitude); } }

		public bool Contains(IGeoPoint point)
		{
			return point.Latitude >= LatitudeMin && point.Latitude <= LatitudeMax && point.Longitude >= LongitudeMin && point.Longitude <= LongitudeMax;
		}

		public GeoRectangle(double nwLat, double nwLon, double seLat, double seLon)
		{
			Init(nwLat, nwLon, seLat, seLon);
		}

		public GeoRectangle(decimal nwLat, decimal nwLon, decimal seLat, decimal seLon)
		{
			Init(Convert.ToDouble(nwLat), Convert.ToDouble(nwLon), Convert.ToDouble(seLat), Convert.ToDouble(seLon));
		}

		private void Init(double nwLat, double nwLon, double seLat, double seLon)
		{
			Points = new List<IGeoPoint>();
			Points.Insert(0, new GeoPoint(nwLat, nwLon));
			Points.Insert(1, new GeoPoint(seLat, seLon));
			Center = new GeoPoint((nwLat + seLat) / 2, (nwLon + seLon) / 2);
		}
	}
}
