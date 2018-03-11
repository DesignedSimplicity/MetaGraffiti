using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Base.Tests.Google
{
	[TestClass]
	public class GoogleTimezoneTests
	{
		private GoogleApiService GetService()
		{
			return new GoogleApiService(Web.Admin.AutoConfig.GoogleMapsApiKey);
		}

		[TestMethod]
		public void GoogleApiService_RequestTimezone()
		{
			var service = GetService();

			var timezone = service.RequestTimezone(TestsHelper.GetNYC());

			Assert.AreEqual("America/New_York", timezone.TimeZoneId);
		}
	}
}