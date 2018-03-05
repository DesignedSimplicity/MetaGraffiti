using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoRegionInfoTests
	{
		[TestMethod]
		public void GeoRegionInfo_All()
		{
			var r = GeoRegionInfo.All;
			Assert.AreEqual(862, r.Count);
			//Assert.AreEqual(899, r.Count);
		}

		[TestMethod]
		public void GeoRegionInfo_ByID()
		{
			var r = GeoRegionInfo.ByName("MaRYlAnD");
			Assert.AreEqual("MD", r.RegionAbbr);
			Assert.AreEqual(840, r.Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionInfo_ByISO()
		{
			var r = GeoRegionInfo.ByISO("uS-Md");
			Assert.AreEqual("MD", r.RegionAbbr);
			Assert.AreEqual(840, r.Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionInfo_ByAbbr()
		{
			var r = GeoRegionInfo.ByAbbr("mD");
			Assert.AreEqual("MD", r.RegionAbbr);
			Assert.AreEqual(840, r.Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionInfo_ListByCountry()
		{
			var states = GeoRegionInfo.ListByCountry(840);
			Assert.AreEqual(51, states.Count());
			Assert.AreEqual(840, states.First().Country.CountryID);
		}

		[TestMethod]
		public void GeoRegionInfo_CountryRegions()
		{
			var usa = GeoCountryInfo.ByID(840);
			Assert.AreEqual(51, usa.Regions.Count());
		}

		[TestMethod]
		public void GeoRegionInfo_ListByLocation()
		{
			var r = GeoRegionInfo.ListByLocation(new GeoLocation(38.90723, -77.036464));
			Assert.AreEqual(3, r.Count());
			Assert.IsTrue(r.Any(x => x.RegionAbbr == "DC"));
			Assert.IsTrue(r.Any(x => x.RegionAbbr == "MD"));
			Assert.IsTrue(r.Any(x => x.RegionAbbr == "VA"));
		}

		[TestMethod]
		public void GeoRegionInfo_VerifyBounds()
		{
			string[] _swapLon = { "RU-CHU" };

			foreach (var c in GeoCountryInfo.All)
			{
				foreach (var r in GeoRegionInfo.ListByCountry(c.CountryID))
				{
					var lat = r.Bounds.NorthWest.Latitude > r.Bounds.SouthEast.Latitude;
					Assert.IsTrue(lat, $"Latitude Error for {c.Name} {r.RegionName} = {r.Bounds.NorthWest.Latitude}");

					var lon = r.Bounds.NorthWest.Longitude < r.Bounds.SouthEast.Longitude;
					if (_swapLon.Contains(r.RegionISO)) lon = !lon;
					Assert.IsTrue(lon, $"Longitude Error for {c.Name} {r.RegionName} = {r.Bounds.NorthWest.Longitude}");
				}
			}
		}
	}
}
