using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Data;

namespace MetaGraffiti.Base.Tests.Geo.Data
{
	[TestClass]
	public class GeoCountryDataTests
	{
		[TestMethod]
		public void GeoCountryData_All()
		{
			var g = GeoCountryData.All;
			Assert.AreEqual(249, g.Count);
		}

		[TestMethod]
		public void GeoCountryData_ByID()
		{
			var g2 = GeoCountryData.ByID(36);
			Assert.AreEqual("AU", g2.ISO2);

			var g3 = GeoCountryData.ByID(840);
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryData_ByISO()
		{
			var g2 = GeoCountryData.ByISO("AU");
			Assert.AreEqual("AU", g2.ISO2);

			var g3 = GeoCountryData.ByISO("USA");
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryData_ByName()
		{
			var g2 = GeoCountryData.ByName("Australia");
			Assert.AreEqual("AU", g2.ISO2);

			var g3 = GeoCountryData.ByName("United States");
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryData_ByNameDeep()
		{
			var g2 = GeoCountryData.ByName("Republik Oesterreich", true);
			Assert.AreEqual("AT", g2.ISO2);

			var g3 = GeoCountryData.ByName("United States of America", true);
			Assert.AreEqual("USA", g3.ISO3);
		}

		[TestMethod]
		public void GeoCountryData_ListByLocation()
		{
			var c = GeoCountryData.ListByLocation(new GeoLocation(39.0, -67.0));
			Assert.AreEqual(1, c.Count);
			Assert.AreEqual("USA", c[0].ISO3);
		}
	}
}