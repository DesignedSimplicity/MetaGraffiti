using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Google
{
	[TestClass]
	public class GoogleLocationServiceTests
	{
		private GoogleLocationService GetService()
		{
			return new GoogleLocationService(Web.Admin.AutoConfig.GoogleMapsApiKey);
		}

		[TestMethod]
		public void GoogleLocationService_TestRequest()
		{
			var service = GetService();

			var locations = service.RequestLocations(TestsHelper.GetNYC());

			Assert.IsNotNull(locations);
		}

		[TestMethod]
		public void GoogleLocationService_TestLookup()
		{
			var service = GetService();

			var location = service.LookupGeoLocation(TestsHelper.GetNYC());

			Assert.AreEqual("New York City Hall", location.Name);

			Assert.AreEqual("USA", location.Country.ISO3);
			Assert.AreEqual("New York", location.Region.RegionName);

			Assert.AreEqual(40.7127461, location.Latitude);
			Assert.AreEqual(-74.005974, location.Longitude);

			Assert.IsNotNull(location.Bounds);
			Assert.IsTrue(location.Bounds.NorthWest.Latitude != 0);
			Assert.IsTrue(location.Bounds.NorthWest.Longitude != 0);
			Assert.IsTrue(location.Bounds.SouthEast.Latitude != 0);
			Assert.IsTrue(location.Bounds.SouthEast.Longitude != 0);
		}

		[TestMethod]
		public void GoogleLocationService_TestLookupList()
		{
			var service = GetService();

			var locations = service.LookupGeoLocations(TestsHelper.GetNYC());
			var location = locations.First();

			Assert.AreEqual("New York City Hall", location.Name);
		}
	}
}