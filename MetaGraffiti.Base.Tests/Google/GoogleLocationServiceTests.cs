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
		}
	}
}