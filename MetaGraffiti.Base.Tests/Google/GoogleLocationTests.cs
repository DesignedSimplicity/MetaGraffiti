using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Base.Tests.Google
{
	[TestClass]
	public class GoogleLocationTests
	{
		private GoogleApiService GetService()
		{
			return new GoogleApiService(Web.Admin.AutoConfig.GoogleMapsApiKey);
		}

		[TestMethod]
		public void GoogleApiService_RequestLocations()
		{
			var service = GetService();

			var locations = service.RequestLocations(TestsHelper.GetNYC());

			Assert.IsNotNull(locations);
			Assert.IsNotNull(locations.Results);
			Assert.IsTrue(locations.Results.Count > 0);
		}
	}
}