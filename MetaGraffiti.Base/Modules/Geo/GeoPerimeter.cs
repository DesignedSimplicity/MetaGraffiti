using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
    public class GeoPerimeter : IGeoPerimeter
	{
		// ==================================================
		// Constructors
		public GeoPerimeter(IEnumerable<IGeoLatLon> points)
		{
			var minLat = points.Min(x => x.Latitude);
			var maxLat = points.Max(x => x.Latitude);
			var minLon = points.Min(x => x.Longitude);
			var maxLon = points.Max(x => x.Longitude);

			Points = points;
			NorthWest = new GeoPosition(minLat, minLon);
			SouthEast = new GeoPosition(maxLat, maxLon);
			Center = new GeoPosition((minLat + maxLat) / 2, (minLon + maxLon) / 2);

			// TODO: HACK: fix this calculation - https://www.pmel.noaa.gov/maillists/tmap/ferret_users/fu_2004/msg00023.html
			Area = Math.Abs(maxLat - minLat) * Math.Abs(maxLon - minLon);
		}

		// ==================================================
		// Properties
		public IEnumerable<IGeoLatLon> Points { get; private set; }
		public IGeoLatLon Center { get; private set; }
		public IGeoLatLon NorthWest { get; private set; }
		public IGeoLatLon SouthEast { get; private set; }
		public double Area { get; private set; }

		// ==================================================
		// Methods
		public bool Contains(IGeoLatLon point)
		{
			return point.Latitude >= NorthWest.Latitude
					&& point.Latitude <= SouthEast.Latitude
					&& point.Longitude >= NorthWest.Longitude
					&& point.Longitude <= SouthEast.Longitude;
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
	}
}
