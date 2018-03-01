using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoTimezoneInfoTests
	{
		[TestMethod]
		public void GeoTimezoneInfo_CheckSystemTimezones()
		{
			foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
			{
				var g = GeoTimezoneInfo.BySystem(tz.Id.ToLowerInvariant());
				Assert.IsNotNull(g);
				Assert.AreEqual(tz.Id, g.WinTZ);
			}
		}

		[TestMethod]
		public void GeoTimezoneInfo_ByTZID()
		{
			// given a TZ ID, find the right GeoTimezone object and check its properties
			foreach (var tz in GeoTimezoneInfo.Windows)
			{
				var g = GeoTimezoneInfo.ByTZID(tz.TZID.ToLowerInvariant());
				Assert.IsNotNull(g);
				Assert.AreEqual(tz.TZID, g.TZID);
			}
		}

		[TestMethod]
		public void GeoTimezoneInfo_WindowsBaseTimezone()
		{
			// windows timezones are mapped to themselves
			foreach (var tz in GeoTimezoneInfo.Windows)
			{
				Assert.AreEqual(tz.Key, tz.BaseTimezone.Key);
			}
		}

		[TestMethod]
		public void GeoTimezoneInfo_OlsonBaseTimezone()
		{
			// windows timezones are mapped to themselves
			foreach (var tz in GeoTimezoneInfo.Olson)
			{
				Assert.AreEqual(tz.WinTZ, tz.BaseTimezone.WinTZ);
			}
		}

		/*
		[TestMethod]
		public void GeoTimezoneInfo_GetDefaultByUTC()
		{
			// get the default TZID timezone for a given UTC offset
		}

		[TestMethod]
		public void GeoTimezoneInfo_GetDefaultByCountry()
		{
			// get the default TZID timezone for a given country
		}

		[TestMethod]
		public void GeoTimezoneInfo_ListByCountry()
		{
			// list all of the timezones for a given country
		}

		[TestMethod]
		public void GeoTimezoneInfo_ListByLocation()
		{
			// list all of the timezones for a given lat/long location
		}

		[TestMethod]
		public void GeoTimezoneInfo_ToLocalTime()
		{
			// convert a UTC date/time to a local time
		}

		[TestMethod]
		public void GeoTimezoneInfo_ToUniversalTime()
		{
			// convert a local time to a UTC date/time
		}

		[TestMethod]
		public void GeoTimezoneInfo_FromLocalToLocal()
		{
			// convert a local time to a different local time
		}
		*/
	}
}
