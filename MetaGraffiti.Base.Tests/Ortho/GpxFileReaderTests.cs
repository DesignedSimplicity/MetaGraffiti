using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Tests.Ortho
{
	[TestClass]
	public class GpxFileReaderTests
	{
		[TestMethod]
		public void GpxFileReader_Version1()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Version1.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().PointData.Count);
		}

		[TestMethod]
		public void GpxFileReader_Version1_1()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Version1_1.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().PointData.Count);
		}

		[TestMethod]
		public void GpxFileReader_Waypoints()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Waypoints.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(4, d.Waypoints.Count);
		}

		[TestMethod]
		public void GpxFileReader_Routes()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Routes.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(2, d.Routes.Count);
			Assert.AreEqual(4, d.Routes.First().PointData.Count);
			Assert.AreEqual(4, d.Routes.Last().PointData.Count);
		}

		[TestMethod]
		public void GpxFileReader_Tracks()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Tracks.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(2, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().PointData.Count);
			Assert.AreEqual(4, d.Tracks.Last().PointData.Count);
		}

		[TestMethod]
		public void GpxFileReader_TrackWithSegments()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\TrackWithSegments.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(1, d.Tracks.First().PointData.Min(x => x.Segment));
			Assert.AreEqual(2, d.Tracks.First().PointData.Max(x => x.Segment));
		}

		[TestMethod]
		public void GpxFileReader_TrackWithoutSegments()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\TrackWithoutSegments.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().PointData.Count);
		}

		[TestMethod]
		public void GpxFileReader_CreatorAndVersion()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Example1.gpx");

			var r = new GpxFileReader(uri);
			r.ReadFile();

			Assert.AreEqual("GPSLogger - http://gpslogger.mendhak.com/", r.Creator);
			Assert.AreEqual(Convert.ToDecimal(1.0), r.Version);
		}
	}
}