using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoCountryInfoTests
	{
		[TestMethod]
		public void GeoCountryInfo_All()
		{
			var g = GeoCountryInfo.All;
			Assert.AreEqual(249, g.Count);
		}

		[TestMethod]
		public void GeoCountryInfo_ByID()
		{
			var g2 = GeoCountryInfo.ByID(36);
			Assert.AreEqual("AU", g2.ISO2);

			var g3 = GeoCountryInfo.ByID(840);
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryInfo_ByISO()
		{
			var g2 = GeoCountryInfo.ByISO("AU");
			Assert.AreEqual("AU", g2.ISO2);

			var g3 = GeoCountryInfo.ByISO("USA");
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryInfo_ByName()
		{
			var g2 = GeoCountryInfo.ByName("Australia");
			Assert.AreEqual("AU", g2.ISO2);

			var g3 = GeoCountryInfo.ByName("United States");
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryInfo_ByNameDeep()
		{
			var g2 = GeoCountryInfo.ByName("Republik Oesterreich", true);
			Assert.AreEqual("AT", g2.ISO2);

			var g3 = GeoCountryInfo.ByName("United States of America", true);
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryInfo_ListByLocation()
		{
			var c = GeoCountryInfo.ListByLocation(new GeoLocation(39.0, -67.0));
			Assert.AreEqual(1, c.Count);
			Assert.AreEqual("USA", c[0].ISO3);
		}
	}
}