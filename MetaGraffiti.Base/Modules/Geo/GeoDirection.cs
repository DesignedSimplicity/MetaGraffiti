using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Geo
{
	public class GeoDirection
	{
		/*
		// ==================================================
		// Constructors

		public GeoDirection(double degrees) { Degrees = degrees; }
		public GeoDirection(decimal degrees) { Degrees = Convert.ToDouble(degrees); }

		// ==================================================
		// Properties

		public double Degrees { get; private set; }

		// ==================================================
		// Static Factory

		public static GeoDirection FromPoints(IGeoLatLon a, IGeoLatLon b)
		{
			var dLon = GeoDirection.DegreesInRadians(b.Longitude - a.Longitude);
			var dPhi = Math.Log(
				Math.Tan(GeoDirection.DegreesInRadians(b.Latitude) / 2 + Math.PI / 4) / Math.Tan(GeoDirection.DegreesInRadians(a.Latitude) / 2 + Math.PI / 4));
			if (Math.Abs(dLon) > Math.PI)
				dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);

			var d = GeoDirection.BearingInDegrees(Math.Atan2(dLon, dPhi));
			return new GeoDirection(d);
		}
		*/

		// ==================================================
		// Static Helpers

		public static double DegreesInRadians(double d)
		{
			return d * Math.PI / 180.000000;
		}

		public static double RadiansInDegrees(double r)
		{
			return r / Math.PI * 180.000000;
		}

		public static double BearingInDegrees(double radians)
		{
			return (RadiansInDegrees(radians) + 360) % 360;
		}
	}
}
