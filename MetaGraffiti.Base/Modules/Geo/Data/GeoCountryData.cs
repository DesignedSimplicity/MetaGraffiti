using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Geo.Data
{
    public class GeoCountryData
    {
		// ==================================================
		// Constructors
		public GeoCountryData(GeoContinents continent, int id, int division, string iso2, string iso3, string oc, string name, string nameLong, string local, string localLong, string abbr10, string abbr15, string abbr30, double latCenter, double lonCenter, double latNorthWest, double lonNorthWest, double latSouthEast, double lonSouthEast)
		{
			Continent = continent;
			CountryID = id;
			Division = division;

			ISO2 = iso2.ToUpperInvariant();
			ISO3 = iso3.ToUpperInvariant();
			OC = oc.ToUpperInvariant();

			Name = name;
			NameLong = nameLong;
			NameLocal = local;
			NameLocalLong = localLong;

			Abbr10 = abbr10;
			Abbr15 = abbr15;
			Abbr30 = abbr30;

			Center = new GeoPosition(latCenter, lonCenter);
			Bounds = new GeoRectangle(latNorthWest, lonNorthWest, latSouthEast, lonSouthEast);
		}

		// ==================================================
		// Properties
		public int CountryID { get; private set; }

		public IGeoLatLon Center { get; private set; }
		public GeoRectangle Bounds { get; private set; }
		public GeoContinents Continent { get; private set; }

		public int Division { get; private set; }

		public string ISO2 { get; private set; }
		public string ISO3 { get; private set; }
		public string OC { get; private set; }

		public string Abbr10 { get; private set; }
		public string Abbr15 { get; private set; }
		public string Abbr30 { get; private set; }

		public string Name { get; private set; }
		public string NameLocal { get; private set; }
		public string NameLong { get; private set; }
		public string NameLocalLong { get; private set; }
	}
}
