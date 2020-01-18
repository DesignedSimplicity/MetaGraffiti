using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Tests.Ortho
{
	[TestClass]
	public class TcxFileReaderTests
	{
		[TestMethod]
		public void TcxFileReader_Activities()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Tcx\PolarBeat.tcx");

			var r = new TcxFileReader(uri);
			var d = r.ReadFile();

			Assert.AreEqual(1, d.Activities.Count);

			Assert.AreEqual("Hiking", d.Activities.First().TrainingName);
		}

		[TestMethod]
		public void TcxFileReader_Laps()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Tcx\PolarBeat.tcx");

			var r = new TcxFileReader(uri);
			var d = r.ReadFile();

			Assert.AreEqual(1, d.Activities.First().Laps.Count);

			var l = d.Activities.First().Laps.First();
			Assert.AreEqual(1875.0M, Math.Round(l.TotalTimeSeconds, 4));
			Assert.AreEqual(2504.5220M, Math.Round(l.DistanceMeters, 4));
			Assert.AreEqual(3.1667M, Math.Round(l.MaximumSpeed, 4));
			Assert.AreEqual(357, l.Calories);
			Assert.AreEqual(137, l.AverageHeartRateBpm);
			Assert.AreEqual(174, l.MaximumHeartRateBpm);
		}

		[TestMethod]
		public void TcxFileReader_Tracks()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Tcx\PolarBeat.tcx");

			var r = new TcxFileReader(uri);
			var d = r.ReadFile();
			Assert.AreEqual(9, d.Activities.First().Laps.First().Tracks.Count);

			var t = d.Activities.First().Laps.First().Tracks;			
			var t0 = t.First();
			Assert.AreEqual("2017-10-31", t0.Timestamp.ToString("yyyy-MM-dd"));
			Assert.AreEqual(90, t0.HeartRateBpm);
			Assert.AreEqual("Present", t0.SensorState);
			Assert.IsNull(t0.DistanceMeters);

			var t1 = t.Skip(4).First();
			Assert.AreEqual("2017-10-31", t1.Timestamp.ToString("yyyy-MM-dd"));
			Assert.AreEqual(3.0701M, Math.Round(t1.DistanceMeters.Value, 4));
			Assert.AreEqual(-41.2953, Math.Round(t1.Latitude.Value, 4));
			Assert.AreEqual(174.7885, Math.Round(t1.Longitude.Value, 4));
		}
	}
}