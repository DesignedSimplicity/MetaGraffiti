using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoLookupServiceTests
	{
		private GeoLookupService GetService()
		{
			var google = new GoogleApiService(Web.Admin.AutoConfig.GoogleMapsApiKey);
			return new GeoLookupService(google);
		}

		[TestMethod]
		public void GeoLookupService_LookupTimezone()
		{
			var service = GetService();

			var timezone = service.LookupTimezone(TestsHelper.GetNYC());

			Assert.AreEqual("America/New_York", timezone.TZID);
		}

		[TestMethod]
		public void GeoLookupService_LookupElevation()
		{
			var service = GetService();

			var elevation = service.LookupElevation(new GeoPosition(0.0, 0.0));

			Assert.AreEqual(0, elevation);
		}

		[TestMethod]
		public void GeoLookupService_LookupLocations()
		{
			var service = GetService();

			var locations = service.LookupLocations(TestsHelper.GetNYC());
			var location = locations.First();

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
	}
}
