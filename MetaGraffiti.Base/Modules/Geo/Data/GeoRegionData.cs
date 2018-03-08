using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Geo.Data
{
    public class GeoRegionData
    {
		// ==================================================
		// Constructors
		public GeoRegionData(int country, int id, int division, string iso, string abbr, string name, string nameLocal, double latCenter, double lonCenter, double latNorthWest, double lonNorthWest, double latSouthEast, double lonSouthEast)
		{
			CountryID = country;
			RegionID = id;

			RegionDiv = division;
			RegionName = name;

			RegionISO = iso.ToUpperInvariant();
			RegionAbbr = (String.IsNullOrWhiteSpace(abbr) ? "" : abbr); ;
			RegionNameLocal = (String.IsNullOrWhiteSpace(nameLocal) ? "" : nameLocal);

			Center = new GeoPosition(latCenter, lonCenter);
			Bounds = new GeoRectangle(latNorthWest, lonNorthWest, latSouthEast, lonSouthEast);
		}

		// ==================================================
		// Properties
		public int RegionID { get; set; }
		public int CountryID { get; set; }

		public int RegionDiv { get; set; }

		public string RegionISO { get; set; }
		public string RegionAbbr { get; set; }
		public string RegionName { get; set; }
		public string RegionNameLocal { get; set; }

		public IGeoLatLon Center { get; set; }
		public IGeoPerimeter Bounds { get; set; }
	}
}
