using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoDistanceTests
	{
		[TestMethod]
		public void GeoDistance_FeetToMeters()
		{
			GeoDistance g = new GeoDistance();
			g.Feet = 3280839895;
			Assert.AreEqual(1000000000, Math.Round(g.Meters, 0));

			g.Meters = 1000000000;
			Assert.AreEqual(3280839895, Math.Round(g.Feet, 0));
		}

		[TestMethod]
		public void GeoDistance_FeetToMiles()
		{
			GeoDistance g = new GeoDistance();
			g.Feet = 5280;
			Assert.AreEqual(1, Math.Round(g.Miles, 0));

			g.Miles = 0.5;
			Assert.AreEqual(2640, Math.Round(g.Feet, 0));
		}

		[TestMethod]
		public void GeoDistance_DistanceMeters()
		{
			GeoLocation a = new GeoLocation(0.0, 0.0);
			GeoLocation b = new GeoLocation(12.34, 56.78);

			var m = GeoDistance.DistanceMeters(a, b);
			Assert.AreEqual(6409572, Math.Round(m, 0));
		}

		[TestMethod]
		public void GeoDistance_BetweenPoints()
		{
			GeoLocation a = new GeoLocation(12.34, 56.78);
			GeoLocation b = new GeoLocation(43.21, 87.65);

			GeoDistance g = GeoDistance.BetweenPoints(a, b);

			Assert.AreEqual(4532161, Math.Round(g.Meters, 0));
		}

		[TestMethod]
		public void GeoDistance_BetweenMultiplePoints()
		{
			List<GeoLocation> p = new List<GeoLocation>();
			p.Add(new GeoLocation(0.0, 0.0));
			p.Add(new GeoLocation(12.34, 56.78));
			p.Add(new GeoLocation(43.21, 87.65));

			GeoDistance g = GeoDistance.BetweenPoints(p);

			Assert.AreEqual(6409572 + 4532161, Math.Round(g.Meters, 0));
		}
	}
}
