using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Modules.Geo
{
	// ==================================================
	// Extensions

	public static class GeoInfoDataExtensions
	{
		public static GeoRegionInfo ToInfo(this GeoRegionData data) { return (data == null ? null : new GeoRegionInfo(data)); }
		public static IEnumerable<GeoRegionInfo> ToInfo(this IEnumerable<GeoRegionData> data) { return data.Select(x => new GeoRegionInfo(x)); }

		public static GeoCountryInfo ToInfo(this GeoCountryData data) { return (data == null ? null : new GeoCountryInfo(data)); }
		public static IEnumerable<GeoCountryInfo> ToInfo(this IEnumerable<GeoCountryData> data) { return data.Select(x => new GeoCountryInfo(x)); }

		public static GeoTimezoneInfo ToInfo(this GeoTimezoneData data) { return (data == null ? null : new GeoTimezoneInfo(data)); }
		public static IEnumerable<GeoTimezoneInfo> ToInfo(this IEnumerable<GeoTimezoneData> data) { return data.Select(x => new GeoTimezoneInfo(x)); }
	}
}