using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests.Google
{
	[TestClass]
	public class GoogleElevationTests
	{
		private GoogleApiService GetService()
		{
			return new GoogleApiService(Web.Admin.AutoConfig.GoogleMapsApiKey);
		}

		[TestMethod]
		public void GoogleApiService_RequestElevation()
		{
			var service = GetService();

			var elevation = service.RequestElevation(new GeoPosition(0.0, 0.0));

			Assert.AreEqual(0, elevation.Elevation);
		}
	}
}