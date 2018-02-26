using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Geo.Data
{
	[TestClass]
	public class GeoRegionDataTests
	{
		[TestMethod]
		public void GeoRegionData_All()
		{
			var r = GeoRegionData.All;
			Assert.AreEqual(899, r.Count);
		}

		[TestMethod]
		public void GeoRegionData_ByID()
		{
			var r = GeoRegionData.ByName("MaRYlAnD");
			Assert.AreEqual("MD", r.RegionAbbr);
			Assert.AreEqual(840, r.Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionData_ByISO()
		{
			var r = GeoRegionData.ByISO("uS-Md");
			Assert.AreEqual("MD", r.RegionAbbr);
			Assert.AreEqual(840, r.Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionData_ByAbbr()
		{
			var r = GeoRegionData.ByAbbr("mD");
			Assert.AreEqual("MD", r.RegionAbbr);
			Assert.AreEqual(840, r.Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionData_ListByCountry()
		{
			var states = GeoRegionData.ListByCountry(840);
			Assert.AreEqual(51, states.Count);
			Assert.AreEqual(840, states[0].Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionData_CountryRegions()
		{
			var usa = GeoCountryData.ByID(840);
			Assert.AreEqual(51, usa.Regions.Count);
		}

		[TestMethod]
		public void GeoRegionData_ListByLatLon()
		{
			var r = GeoRegionData.ListByLatLon(new GeoPoint(38.90723, -77.036464));
			Assert.AreEqual(3, r.Count);
			Assert.IsTrue(r.Any(x => x.RegionAbbr == "DC"));
			Assert.IsTrue(r.Any(x => x.RegionAbbr == "MD"));
			Assert.IsTrue(r.Any(x => x.RegionAbbr == "VA"));
		}
	}
}
