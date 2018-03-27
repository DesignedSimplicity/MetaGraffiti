using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Geo
{
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
	public interface IGeoCoordinate : IGeoLatLon
	{
		double? Elevation { get; }
	}

	/// <summary>
	/// 4 dimensional point including a timestamp
	/// </summary>
	public interface IGeoPoint : IGeoCoordinate
	{
		DateTime? Timestamp { get; }
	}

	/// <summary>
	/// 4 dimensional point with an orientation
	/// </summary>
	public interface IGeoPosition : IGeoPoint
	{
		GeoDirection Direction { get; }
	}

	// --------------------------------------------------
	public interface IGeoPerimeter
	{
		double Area { get; }
		IGeoLatLon Center { get; }
		IGeoLatLon NorthWest { get; }
		IGeoLatLon SouthEast { get; }
		IEnumerable<IGeoLatLon> Points { get; }
		bool Contains(IGeoLatLon point);
		bool Contains(IGeoPerimeter point);
	}
}