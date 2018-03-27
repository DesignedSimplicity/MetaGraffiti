using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;
using System;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoRectangleTests
	{
		[TestMethod]
		public void GeoRectangle_Points()
		{
			var r = new GeoRectangle(10.0, 20.0, 20.0, 40.0);
			Assert.AreEqual(10.0, r.Points.First().Latitude);
			Assert.AreEqual(20.0, r.Points.First().Longitude);
			Assert.AreEqual(20.0, r.Points.Last().Latitude);
			Assert.AreEqual(40.0, r.Points.Last().Longitude);
		}

		[TestMethod]
		public void GeoRectangle_Center()
		{
			var r = new GeoRectangle(-10.0, -20.0, 20.0, 40.0);
			Assert.AreEqual(5.0, r.Center.Latitude);
			Assert.AreEqual(10.0, r.Center.Longitude);
		}

		[TestMethod]
		public void GeoRectangle_NorthWest()
		{
			var r = new GeoRectangle(10.0, 20.0, 20.0, 40.0);
			Assert.AreEqual(10.0, r.NorthWest.Latitude);
			Assert.AreEqual(20.0, r.NorthWest.Longitude);
		}

		[TestMethod]
		public void GeoRectangle_SouthEast()
		{
			var r = new GeoRectangle(10.0, 20.0, 20.0, 40.0);
			Assert.AreEqual(20.0, r.SouthEast.Latitude);
			Assert.AreEqual(40.0, r.SouthEast.Longitude);
		}
	}
}
