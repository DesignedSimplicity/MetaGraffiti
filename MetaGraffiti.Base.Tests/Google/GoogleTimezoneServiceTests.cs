using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Google
{
	[TestClass]
	public class GoogleTimezoneServiceTests
	{
		private GoogleTimezoneService GetService()
		{
			return new GoogleTimezoneService(Web.Admin.AutoConfig.GoogleMapsApiKey);
		}

		[TestMethod]
		public void GoogleTimezoneService_TestRequest()
		{
			var service = GetService();

			var timezone = service.RequestTimezone(TestsHelper.GetNYC());

			Assert.AreEqual("America/New_York", timezone.TimeZoneId);
		}

		[TestMethod]
		public void GoogleTimezoneService_TestLookup()
		{
			var service = GetService();

			var timezone = service.LookupGeoTimezone(TestsHelper.GetNYC());

			Assert.AreEqual("America/New_York", timezone.TZID);
		}
	}
}