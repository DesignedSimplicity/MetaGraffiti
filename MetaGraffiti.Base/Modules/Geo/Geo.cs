using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Modules.Geo
{
	// ==================================================
	// Enumerations
	public enum GeoContinents // ordered by size large -> small
	{
		Asia = 1,
		Africa = 2,
		NorthAmerica = 3,
		SouthAmerica = 4,
		Antarctica = 5,
		Europe = 6,
		Oceania = 7
	}

	public enum GeoTimezoneTypes
	{
		Windows = 1,
		Olson = 2
	}

	public enum GeoCardinalPoints
	{
		North = 1,      //0001
		East = 2,       //0010	
		South = 4,      //0100
		West = 8,       //1000
	}

	public enum GeoCompassDirections
	{
		North = 1,      //0001
		East = 2,       //0010
		South = 4,      //0100
		West = 8,       //1000
		NorthEast = 3,  //0011
		SouthEast = 6,  //0110
		NorthWest = 9,  //1001
		SouthWest = 12, //1100
	}

	// ==================================================
	// Interfaces

	/// <summary>
	/// 2 dimensional point with latitude and longitude
	/// </summary>
	public interface IGeoLatLon
	{
		double Latitude { get; }
		double Longitude { get; }
	}

	/// <summary>
	/// 3 dimensional point including elevation
	/// </summary>
	public interface IGeoPoint : IGeoLatLon
	{
		double? Elevation { get; }
	}

	/// <summary>
	/// 4 dimensional point including a timestamp
	/// </summary>
	public interface IGeoLocation : IGeoPoint
	{
		DateTime? Timestamp { get; }
	}

	// --------------------------------------------------
	public interface IGeoPerimeter
	{
		IGeoLatLon Center { get; }
		IGeoLatLon NorthWest { get; }
		IGeoLatLon SouthEast { get; }
		IList<IGeoLatLon> Points { get; }
		bool Contains(IGeoLatLon point);
	}


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
