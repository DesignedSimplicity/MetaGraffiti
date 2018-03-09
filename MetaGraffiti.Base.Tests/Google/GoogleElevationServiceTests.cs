using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Google
{
	[TestClass]
	public class GoogleElevationServiceTests
	{
		private GoogleElevationService GetService()
		{
			return new GoogleElevationService(Web.Admin.AutoConfig.GoogleMapsApiKey);
		}

		[TestMethod]
		public void GoogleElevationService_TestRequest()
		{
			var service = GetService();

			var elevation = service.RequestElevation(new GeoPosition(0.0, 0.0));

			Assert.AreEqual(0, elevation.Elevation);
		}

		[TestMethod]
		public void GoogleElevationService_TestLookup()
		{
			var service = GetService();

			var elevation = service.LookupElevation(new GeoPosition(0.0, 0.0));

			Assert.AreEqual(0, elevation);
		}
	}
}