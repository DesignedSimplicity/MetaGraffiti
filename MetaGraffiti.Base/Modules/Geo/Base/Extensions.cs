using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

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

		public static string ToJson(this IGeoLatLon latlon)
		{
			if (latlon == null) return "{}";

			dynamic json = new JObject();
			json.lat = latlon.Latitude;
			json.lng = latlon.Longitude;

			return json.ToString();
		}

		public static string ToJson(this GeoLocationInfo location)
		{
			if (location == null) return "{}";

			dynamic json = new JObject();
			json.name = location.Name;

			json.center = new JObject();
			json.center.lat = location.Latitude;
			json.center.lng = location.Longitude;

			json.bounds = new JObject();
			json.bounds.north = (location.Bounds?.NorthWest?.Latitude ?? 0);
			json.bounds.south = (location.Bounds?.SouthEast?.Latitude ?? 0);
			json.bounds.east = (location.Bounds?.SouthEast?.Longitude ?? 0);
			json.bounds.west = (location.Bounds?.NorthWest?.Longitude ?? 0);

			return json.ToString();
		}
	}
}