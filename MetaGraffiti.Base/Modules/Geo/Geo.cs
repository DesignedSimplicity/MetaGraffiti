using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	// ==================================================
	// Enumerations
	public enum GeoContinent { Africa, Antarctica, Asia, Europe, NorthAmerica, Oceania, SouthAmerica }

	public enum GeoTimezoneTypes { Windows = 1, Olson = 2 }

	// ==================================================
	// Interfaces
	public interface IGeoLatLong
	{
		double Latitude { get; }
		double Longitude { get; }
	}

	public interface IGeoLocation : IGeoLatLong
	{
		double? Elevation { get; }
		DateTime? Timestamp { get; }
	}

	public interface IGeoPosition : IGeoLocation
	{
		double? Direction { get; }
	}

	public interface IGeoPerimeter
	{
		IGeoLatLong Center { get; }
		IGeoLatLong NorthWest { get; }
		IGeoLatLong SouthEast { get; }
		IList<IGeoLatLong> Points { get; }
		bool Contains(IGeoLatLong point);
	}


	// ==================================================
	// Structures


}
