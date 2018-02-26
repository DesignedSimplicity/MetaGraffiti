using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Gpx;
using MetaGraffiti.Base.Modules.Gpx.Data;

namespace MetaGraffiti.Base.Tests.Gpx
{
	[TestClass]
	public class GpxXmlDataTests
	{
		[TestMethod]
		public void GpxXmlData_ReadXml1()
		{
			var g = new GpxXmlData();
			g.ReadXml(TestsHelper.GetTestDataFileUri("1.gpx"));

			Assert.AreEqual("GPSLogger - http://gpslogger.mendhak.com/", g.Creator);
			Assert.AreEqual(Convert.ToDecimal(1.0), g.Version);
			Assert.AreEqual(DateTime.Parse("2016-01-16T22:47:22Z").ToUniversalTime(), g.Timestamp.Value);

			Assert.AreEqual(0, g.Routes.Count);
			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(137, g.Tracks.First().Points.Count());
		}

		[TestMethod]
		public void GpxXmlData_ReadXml2()
		{
			var g = new GpxXmlData();
			g.ReadXml(TestsHelper.GetTestDataFileUri("2.gpx"));

			Assert.AreEqual("GPSLogger 80 - http://gpslogger.mendhak.com/", g.Creator);
			Assert.AreEqual(Convert.ToDecimal(1.0), g.Version);
			Assert.AreEqual(DateTime.Parse("2016-11-04T23:19:42Z").ToUniversalTime(), g.Timestamp.Value);

			Assert.AreEqual(0, g.Routes.Count);
			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(204, g.Tracks.First().Points.Count());
		}
	}
}