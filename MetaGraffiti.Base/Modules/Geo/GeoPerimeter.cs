using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
    public class GeoPerimeter : IGeoPerimeter
	{
		// ==================================================
		// Constructors
		public GeoPerimeter(IList<IGeoLatLon> points)
		{
			var minLat = points.Min(x => x.Latitude);
			var maxLat = points.Max(x => x.Latitude);
			var minLon = points.Min(x => x.Longitude);
			var maxLon = points.Max(x => x.Longitude);

			Points = points;
			NorthWest = new GeoLocation(minLat, minLon);
			SouthEast = new GeoLocation(maxLat, maxLon);
			Center = new GeoLocation((minLat + maxLat) / 2, (minLon + maxLon) / 2);
		}

		// ==================================================
		// Properties
		public IList<IGeoLatLon> Points { get; private set; }
		public IGeoLatLon Center { get; private set; }
		public IGeoLatLon NorthWest { get; private set; }
		public IGeoLatLon SouthEast { get; private set; }

		// ==================================================
		// Methods
		public bool Contains(IGeoLatLon point)
		{
			return point.Latitude >= NorthWest.Latitude
					&& point.Latitude <= SouthEast.Latitude
					&& point.Longitude >= NorthWest.Longitude
					&& point.Longitude <= SouthEast.Longitude;
		}
	}
}
