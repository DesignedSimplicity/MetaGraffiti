using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	public class GeoRectangle : IGeoPerimeter
	{
		// ==================================================
		// Constructors
		public GeoRectangle(double latNorthWest, double lonNorthWest, double latSouthEast, double lonSouthEast)
		{
			Init(latNorthWest, lonNorthWest, latSouthEast, lonSouthEast);
		}

		public GeoRectangle(decimal latNorthWest, decimal lonNorthWest, decimal latSouthEast, decimal lonSouthEast)
		{
			Init(Convert.ToDouble(latNorthWest), Convert.ToDouble(lonNorthWest), Convert.ToDouble(latSouthEast), Convert.ToDouble(lonSouthEast));
		}

		// ==================================================
		// Properties
		public IList<IGeoLatLon> Points { get; private set; }
		public IGeoLatLon Center { get; private set; }
		public IGeoLatLon NorthWest { get { return Points[0]; } }
		public IGeoLatLon SouthEast { get { return Points[1]; } }

		// ==================================================
		// Methods
		public bool Contains(IGeoLatLon point)
		{
			return point.Latitude >= NorthWest.Latitude 
					&& point.Latitude <= SouthEast.Latitude
					&& point.Longitude >= NorthWest.Longitude 
					&& point.Longitude <= SouthEast.Longitude;
		}

		// ==================================================
		// Internal
		private void Init(double latNorthWest, double lonNorthWest, double latSouthEast, double lonSouthEast)
		{
			Points = new List<IGeoLatLon>();
			Points.Insert(0, new GeoLocation(latNorthWest, lonNorthWest));
			Points.Insert(1, new GeoLocation(latSouthEast, lonSouthEast));
			Center = new GeoLocation((latNorthWest + latSouthEast) / 2, (lonNorthWest + lonSouthEast) / 2);
		}
	}
}
