using System;
using System.Collections.Generic;
using System.Text;

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
}
