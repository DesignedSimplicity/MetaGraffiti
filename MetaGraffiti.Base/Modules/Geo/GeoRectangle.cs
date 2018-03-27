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
		public IEnumerable<IGeoLatLon> Points { get; private set; }
		public IGeoLatLon Center { get; private set; }
		public IGeoLatLon NorthWest { get { return Points.First(); } }
		public IGeoLatLon SouthEast { get { return Points.Last(); } }
		public double Area { get; private set; }

		// ==================================================
		// Methods
		public bool Contains(IGeoLatLon point)
		{
			return 
				NorthWest.Latitude >= point.Latitude && point.Latitude >= SouthEast.Latitude
				&&
				NorthWest.Longitude <= point.Longitude && point.Longitude <= SouthEast.Longitude;
		}
		public bool Contains(IGeoPerimeter bounds)
		{
			if (Contains(bounds.Center))
				return true;
			else if (Contains(bounds.NorthWest))
				return true;
			else if (Contains(bounds.SouthEast))
				return true;
			else if (Contains(new GeoPosition(bounds.NorthWest.Latitude, bounds.SouthEast.Longitude)))
				return true;
			else if (Contains(new GeoPosition(bounds.SouthEast.Latitude, bounds.NorthWest.Longitude)))
				return true;
			else
				return false;
		}

		// ==================================================
		// Internal
		private void Init(double latNorthWest, double lonNorthWest, double latSouthEast, double lonSouthEast)
		{
			var points = new List<IGeoLatLon>();
			points.Insert(0, new GeoPosition(latNorthWest, lonNorthWest));
			points.Insert(1, new GeoPosition(latSouthEast, lonSouthEast));
			Points = points;
			Center = new GeoPosition((latNorthWest + latSouthEast) / 2, (lonNorthWest + lonSouthEast) / 2);

			// TODO: HACK: fix this calculation - https://www.pmel.noaa.gov/maillists/tmap/ferret_users/fu_2004/msg00023.html
			Area = Math.Abs(latSouthEast - latNorthWest) * Math.Abs(lonNorthWest - lonSouthEast);
		}
	}
}
