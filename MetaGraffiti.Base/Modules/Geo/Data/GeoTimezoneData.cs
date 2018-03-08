using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Geo.Data
{
    public class GeoTimezoneData
    {
		// ==================================================
		// Constructors
		public GeoTimezoneData(int geotzid, string key, string tzid, string wintz)
		{
			TimezoneType = GeoTimezoneTypes.Windows;
			GeoTZID = geotzid;

			Key = key;
			TZID = tzid;
			Name = WinTZ = wintz;

			TimeZone = TimeZoneInfo.FindSystemTimeZoneById(wintz);
			DisplayAs = TimeZone.DisplayName;
			OffsetUTC = OffsetDST = TimeZone.BaseUtcOffset;
			if (TimeZone.SupportsDaylightSavingTime) OffsetDST.Add(new TimeSpan(1, 0, 0));
		}

		public GeoTimezoneData(int geotzid, string tzid, string wintz)
		{
			TimezoneType = GeoTimezoneTypes.Olson;
			GeoTZID = geotzid;

			Key = Name = TZID = tzid;
			WinTZ = wintz;

			TimeZone = TimeZoneInfo.FindSystemTimeZoneById(wintz);
			DisplayAs = tzid.Replace("_", " ");
			OffsetUTC = OffsetDST = TimeZone.BaseUtcOffset;
			if (TimeZone.SupportsDaylightSavingTime) OffsetDST.Add(new TimeSpan(1, 0, 0));
		}

		// ==================================================
		// Properties
		public GeoTimezoneTypes TimezoneType { get; private set; } // windows vs olson timezone
		public int GeoTZID { get; private set; } // generated ID for core timezones +/- UTC DST #

		public string Key { get; private set; } // Geo level unique id
		public string Name { get; private set; } // default display name
		public string TZID { get; private set; } // Olson TZID
		public string WinTZ { get; private set; } // Windows system TimeZoneInfo.Id
		public string DisplayAs { get; private set; } // formatted display friendly name

		public TimeSpan OffsetUTC { get; private set; }
		public TimeSpan OffsetDST { get; private set; }
		public TimeZoneInfo TimeZone { get; private set; }
	}
}
