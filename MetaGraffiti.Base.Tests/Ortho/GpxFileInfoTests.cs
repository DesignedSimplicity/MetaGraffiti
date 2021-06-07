using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Info;

namespace MetaGraffiti.Base.Tests.Ortho
{
	// TODO:ORTHO:REMOVE - migrate to GpxFileReader
	[TestClass]
	public class GpxFileInfoTests
	{
		[TestMethod]
		public void GpxFileInfo_Example1()
		{
			var g = new GpxFileInfo(TestsHelper.GetTestDataFileUri(@"Gpx\Example1.gpx"));

			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(137, g.Points.Count());

			Assert.AreEqual(9314, Math.Round(g.ElapsedTime.TotalSeconds));
			Assert.AreEqual(9314, Math.Round(g.RecordedTime.TotalSeconds));

			Assert.AreEqual(DateTime.Parse("2016-01-16T22:47:22Z").ToUniversalTime(), g.Timestamp.Value);
		}

		[TestMethod]
		public void GpxFileInfo_Example2()
		{
			var g = new GpxFileInfo(TestsHelper.GetTestDataFileUri(@"Gpx\Example2.gpx"));

			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(204, g.Points.Count());

			Assert.AreEqual(7389, Math.Round(g.ElapsedTime.TotalSeconds));
			Assert.AreEqual(7389, Math.Round(g.RecordedTime.TotalSeconds));

			Assert.AreEqual(DateTime.Parse("2016-11-04T23:19:42Z").ToUniversalTime(), g.Timestamp.Value);
		}

		[TestMethod]
		public void GpxFileInfo_Example3()
		{
			var g = new GpxFileInfo(TestsHelper.GetTestDataFileUri(@"Gpx\Example3.gpx"));

			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(137 + 204, g.Points.Count());
			Assert.IsTrue(g.Points.Any(x => x.Segment == 1));
			Assert.IsTrue(g.Points.Any(x => x.Segment == 2));

			Assert.AreEqual(25324529, Math.Round(g.ElapsedTime.TotalSeconds));
		}

		[TestMethod]
		public void GpxFileInfo_Example4()
		{
			var g = new GpxFileInfo(TestsHelper.GetTestDataFileUri(@"Gpx\Example4.gpx"));

			Assert.AreEqual(2, g.Tracks.Count);
			Assert.AreEqual(137 + 204, g.Points.Count());

			Assert.AreEqual(25324529, Math.Round(g.ElapsedTime.TotalSeconds));
			Assert.AreEqual(9314 + 7389, Math.Round(g.RecordedTime.TotalSeconds));
		}
	}
}