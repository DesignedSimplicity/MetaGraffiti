using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Gpx;
using MetaGraffiti.Base.Modules.Gpx.Data;

namespace MetaGraffiti.Base.Tests.Gpx
{
	[TestClass]
	public class GpxFileTests
	{
		[TestMethod]
		public void GpxFile_Load1()
		{
			var g = new GpxFile();
			g.Load(TestsHelper.GetTestDataFileUri("1.gpx"));

			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(137, g.Points.Count());

			Assert.AreEqual(9314, Math.Round(g.ElapsedTime.TotalSeconds));
			Assert.AreEqual(9314, Math.Round(g.RecordedTime.TotalSeconds));
		}

		[TestMethod]
		public void GpxFile_Load2()
		{
			var g = new GpxFile();
			g.Load(TestsHelper.GetTestDataFileUri("2.gpx"));

			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(204, g.Points.Count());

			Assert.AreEqual(7389, Math.Round(g.ElapsedTime.TotalSeconds));
			Assert.AreEqual(7389, Math.Round(g.RecordedTime.TotalSeconds));
		}

		[TestMethod]
		public void GpxFile_Load3()
		{
			var g = new GpxFile();
			g.Load(TestsHelper.GetTestDataFileUri("3.gpx"));

			Assert.AreEqual(1, g.Tracks.Count);
			Assert.AreEqual(137 + 204, g.Points.Count());
			Assert.IsTrue(g.Points.Any(x => x.Segment == 1));
			Assert.IsTrue(g.Points.Any(x => x.Segment == 2));

			Assert.AreEqual(25324529, Math.Round(g.ElapsedTime.TotalSeconds));
		}

		[TestMethod]
		public void GpxFile_Load4()
		{
			var g = new GpxFile();
			g.Load(TestsHelper.GetTestDataFileUri("4.gpx"));

			Assert.AreEqual(2, g.Tracks.Count);
			Assert.AreEqual(137 + 204, g.Points.Count());

			Assert.AreEqual(25324529, Math.Round(g.ElapsedTime.TotalSeconds));
			Assert.AreEqual(9314 + 7389, Math.Round(g.RecordedTime.TotalSeconds));
		}
	}
}