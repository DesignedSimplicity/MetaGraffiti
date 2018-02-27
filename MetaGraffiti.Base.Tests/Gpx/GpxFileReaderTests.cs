﻿using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Gpx;
using MetaGraffiti.Base.Modules.Gpx.Data;

namespace MetaGraffiti.Base.Tests.Gpx
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
			Assert.AreEqual(4, d.Tracks.First().Points.Count);
		}

		[TestMethod]
		public void GpxFileReader_Version1_1()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Version1_1.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().Points.Count);
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
			Assert.AreEqual(4, d.Routes.First().Points.Count);
			Assert.AreEqual(4, d.Routes.Last().Points.Count);
		}

		[TestMethod]
		public void GpxFileReader_Tracks()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Tracks.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(2, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().Points.Count);
			Assert.AreEqual(4, d.Tracks.Last().Points.Count);
		}

		[TestMethod]
		public void GpxFileReader_TrackWithSegments()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\TrackWithSegments.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(1, d.Tracks.First().Points.Min(x => x.Segment));
			Assert.AreEqual(2, d.Tracks.First().Points.Max(x => x.Segment));
		}

		[TestMethod]
		public void GpxFileReader_TrackWithoutSegments()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\TrackWithoutSegments.gpx");

			var r = new GpxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(1, d.Tracks.Count);
			Assert.AreEqual(4, d.Tracks.First().Points.Count);
		}
	}
}