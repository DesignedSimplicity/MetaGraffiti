using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Geo.Data;

namespace MetaGraffiti.Base.Tests.Geo
{
	[TestClass]
	public class GeoTimezoneDataTests
	{
		[TestMethod]
		public void GeoTimezoneData_CheckSystemTimezones()
		{
			foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
			{
				var g = GeoTimezoneData.BySystem(tz.Id.ToLowerInvariant());
				Assert.IsNotNull(g);
				Assert.AreEqual(tz.Id, g.WinTZ);
			}
		}

		[TestMethod]
		public void GeoTimezoneData_ByTZID()
		{
			// given a TZ ID, find the right GeoTimezone object and check its properties
			foreach (var tz in GeoTimezoneData.Windows)
			{
				var g = GeoTimezoneData.ByTZID(tz.TZID.ToLowerInvariant());
				Assert.IsNotNull(g);
				Assert.AreEqual(tz.TZID, g.TZID);
			}
		}

		[TestMethod]
		public void GeoTimezoneData_WindowsBaseTimezone()
		{
			// windows timezones are mapped to themselves
			foreach (var tz in GeoTimezoneData.Windows)
			{
				Assert.AreEqual(tz.Key, tz.BaseTimezone.Key);
			}
		}

		[TestMethod]
		public void GeoTimezoneData_OlsonBaseTimezone()
		{
			// windows timezones are mapped to themselves
			foreach (var tz in GeoTimezoneData.Olson)
			{
				Assert.AreEqual(tz.WinTZ, tz.BaseTimezone.WinTZ);
			}
		}

		/*
		[TestMethod]
		public void GeoTimezoneData_GetDefaultByUTC()
		{
			// get the default TZID timezone for a given UTC offset
		}

		[TestMethod]
		public void GeoTimezoneData_GetDefaultByCountry()
		{
			// get the default TZID timezone for a given country
		}

		[TestMethod]
		public void GeoTimezoneData_ListByCountry()
		{
			// list all of the timezones for a given country
		}

		[TestMethod]
		public void GeoTimezoneData_ListByLocation()
		{
			// list all of the timezones for a given lat/long location
		}

		[TestMethod]
		public void GeoTimezoneData_ToLocalTime()
		{
			// convert a UTC date/time to a local time
		}

		[TestMethod]
		public void GeoTimezoneData_ToUniversalTime()
		{
			// convert a local time to a UTC date/time
		}

		[TestMethod]
		public void GeoTimezoneData_FromLocalToLocal()
		{
			// convert a local time to a different local time
		}
		*/
	}
}
