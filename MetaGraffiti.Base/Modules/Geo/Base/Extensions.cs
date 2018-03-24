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



		public static string ToJson(this GeoRegionInfo region)
		{
			if (region == null) return "{}";

			dynamic json = new JObject();
			json.id = region.RegionID;
			json.iso = region.RegionISO;
			json.name = region.RegionName;
			json.abbr = region.RegionAbbr;
			json.country = region.Country.ISO3;

			json.center = new JObject();
			json.center.lat = region.Center.Latitude;
			json.center.lng = region.Center.Longitude;

			json.bounds = new JObject();
			json.bounds.north = (region.Bounds?.NorthWest?.Latitude ?? 0);
			json.bounds.south = (region.Bounds?.SouthEast?.Latitude ?? 0);
			json.bounds.east = (region.Bounds?.SouthEast?.Longitude ?? 0);
			json.bounds.west = (region.Bounds?.NorthWest?.Longitude ?? 0);

			return json.ToString();
		}

		public static string ToJson(this GeoCountryInfo country)
		{
			if (country == null) return "{}";

			dynamic json = new JObject();
			json.id = country.CountryID;
			json.iso2 = country.ISO2;
			json.iso3 = country.ISO3;
			json.name = country.Name;
			json.continent = country.Continent;

			json.center = new JObject();
			json.center.lat = country.Center.Latitude;
			json.center.lng = country.Center.Longitude;

			json.bounds = new JObject();
			json.bounds.north = (country.Bounds?.NorthWest?.Latitude ?? 0);
			json.bounds.south = (country.Bounds?.SouthEast?.Latitude ?? 0);
			json.bounds.east = (country.Bounds?.SouthEast?.Longitude ?? 0);
			json.bounds.west = (country.Bounds?.NorthWest?.Longitude ?? 0);

			return json.ToString();
		}
	}
}