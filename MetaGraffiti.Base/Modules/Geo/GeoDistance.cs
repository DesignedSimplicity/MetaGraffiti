using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	/// <summary>
	/// 2 or 3 dimensional distance between two points
	/// </summary>
	public class GeoDistance
	{
		private const int _earthRadiusMeter = 6371000;
		private const double _nauticalMeter = 0.000539957;
		private const double _nauticalMile = 0.86897624;
		private const double _mileMeter = 0.001609344;
		private const double _meterFoot = 3.280839895;
		private const double _mileFoot = 5280;

		//==================================================
		private double _meters = 0;

		public double Meters { get { return _meters; } set { _meters = value; } }
		public double KM { get { return _meters / 1000; } set { _meters = value * 1000; } }
		public double Feet { get { return _meters * _meterFoot; } set { _meters = value / _meterFoot; } }
		public double Miles { get { return _meters * _meterFoot / _mileFoot; } set { _meters = value / _meterFoot * _mileFoot; } }
		//public decimal NauticalMiles { get { return _meters * _nauticalMeter; } }
		//public decimal NauticalMeters { get { return _meters * _nauticalMile; } }

		public void Add(GeoDistance d)
		{
			_meters += d._meters;
		}


		// ==================================================
		// Static Factory

		public static GeoDistance FromKM(double km) { return new GeoDistance() { Meters = km / 1000 }; }
		public static GeoDistance FromMeters(double meters) { return new GeoDistance() { Meters = meters }; }
		public static GeoDistance FromMiles(double miles) { return new GeoDistance() { Miles = miles }; }
		public static GeoDistance FromFeet(double feet, double inches = 0) { return new GeoDistance() { Feet = feet + (inches / 12) }; }

		public static GeoDistance BetweenPoints(IGeoLocation a, IGeoLocation b, bool includeElevation = false)
		{
			var m = GeoDistance.DistanceMeters(a, b, includeElevation);
			return GeoDistance.FromMeters(m);
		}

		public static GeoDistance BetweenPoints(IEnumerable<IGeoLocation> points, bool includeElevation = false)
		{
			double m = 0;
			var a = points.First();
			foreach (var b in points)
			{
				if (a != b) // skip first
				{
					m += GeoDistance.DistanceMeters(a, b, includeElevation);
					a = b; // go next
				}
			}
			return GeoDistance.FromMeters(m);
		}

		public static GeoDistance ElevationBetweenPoints(IEnumerable<IGeoLocation> points, int direction = 0)
		{
			double m = 0;
			var a = points.First();
			foreach (var b in points)
			{
				if (a != b) // skip first
				{
					if (a.Elevation.HasValue && b.Elevation.HasValue)
					{
						var d = a.Elevation.Value - b.Elevation.Value;
						if (direction == 0)
							m += Math.Abs(d);
						else if (direction < 0 && d < 0)
							m += Math.Abs(d);
						else if (direction > 0 && d > 0)
							m += Math.Abs(d);
					}
					a = b; // go next
				}
			}
			return GeoDistance.FromMeters(m);
		}


		// ==================================================
		// Static Helpers

		public static double MilesToKilometers(double miles) { return miles * _mileMeter / 1000; }
		public static double KilometersToMiles(double km) { return km * 1000 / _mileMeter; }
		public static double MilesToNautical(double miles) { return miles * _nauticalMile; }

		public static double DistanceMeters(IGeoLatLon a, IGeoLatLon b)
		{
			var r1 = GeoDirection.DegreesInRadians(a.Latitude);
			var r2 = GeoDirection.DegreesInRadians(b.Latitude);
			var rLat = GeoDirection.DegreesInRadians(b.Latitude - a.Latitude);
			var rLon = GeoDirection.DegreesInRadians(b.Longitude - a.Longitude);

			var c1 = Math.Sin(rLat / 2) * Math.Sin(rLat / 2) + Math.Cos(r1) * Math.Cos(r2) * Math.Sin(rLon / 2) * Math.Sin(rLon / 2);
			return Math.Atan2(Math.Sqrt(c1), Math.Sqrt(1 - c1)) * 2.0 * _earthRadiusMeter;
		}
		public static double DistanceMeters(IGeoLocation a, IGeoLocation b, bool includeElevation)
		{
			var m = DistanceMeters(a, b);
			if (includeElevation && a.Elevation.HasValue && b.Elevation.HasValue)
			{
				var h = Math.Abs(a.Elevation.Value - b.Elevation.Value);
				return Math.Sqrt(Convert.ToDouble(h * h) + Convert.ToDouble(m * m));
			}
			else
				return m;
		}
	}
}
