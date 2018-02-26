using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo;
using System;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoLocationTests
	{
		[TestMethod]
		public void GeoLocation_IsValid()
		{
			var l = new GeoLocation(0.0, 0.0);
			Assert.IsTrue(l.IsValidLocation);

			l.Latitude = -91;
			l.Longitude = -180;
			Assert.IsFalse(l.IsValidLocation);

			l.Latitude = 91;
			l.Longitude = 180;
			Assert.IsFalse(l.IsValidLocation);

			l.Latitude = -90;
			l.Longitude = -181;
			Assert.IsFalse(l.IsValidLocation);

			l.Latitude = 90;
			l.Longitude = 181;
			Assert.IsFalse(l.IsValidLocation);
		}

		[TestMethod]
		public void GeoLocation_Elevation()
		{
			var l = new GeoLocation(0.0, 0.0);
			Assert.IsFalse(l.HasElevation);

			var le = new GeoLocation(0.0, 0.0, 123.4);
			Assert.IsTrue(le.HasElevation);
			Assert.AreEqual(123.4, le.Elevation);
		}

		[TestMethod]
		public void GeoLocation_Timestamp()
		{
			var now = DateTime.Now;
			var l = new GeoLocation(0.0, 0.0, 0.0, now);
			Assert.IsTrue(l.HasTimestamp);
			Assert.IsFalse(l.IsTimestampUTC);
			Assert.AreEqual(now, l.Timestamp);
		}

		[TestMethod]
		public void GeoLocation_TimestampUTC()
		{
			var now = DateTime.UtcNow;
			var l = new GeoLocation(0.0, 0.0, 0.0, now);
			Assert.IsTrue(l.HasTimestamp);
			Assert.IsTrue(l.IsTimestampUTC);
			Assert.AreEqual(now, l.Timestamp);
		}
	}
}
