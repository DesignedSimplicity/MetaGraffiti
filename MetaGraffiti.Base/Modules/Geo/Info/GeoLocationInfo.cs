using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Modules.Geo.Data;

namespace MetaGraffiti.Base.Modules.Geo.Info
{
    public class GeoLocationInfo : IGeoCoordinate
    {
		// ==================================================
		// Internals
		private GeoLocationData _data;

		private GeoTimezoneInfo _timezone;
		private GeoCountryInfo _country;
		private GeoRegionInfo _region;


		// ==================================================
		// Constructors
		public GeoLocationInfo() { _data = new GeoLocationData(); }
		public GeoLocationInfo(GeoLocationData data)
		{
			_data = data;
			Region = GeoRegionInfo.Find(data.Region);
			Country = GeoCountryInfo.Find(data.Country);
		}


		// ==================================================
		// Properties

		public string Name { get { return _data.Name; } set { _data.Name = value; } }

		// --------------------------------------------------
		// Inferred
		public double Latitude => (_data.Center == null ? 0 : _data.Center.Latitude);
		public double Longitude => (_data.Center == null ? 0 : _data.Center.Longitude);
		public double? Elevation => (_data.Center == null ? 0 : _data.Center.Elevation);

		public IGeoPerimeter Bounds => _data.Bounds;

		// --------------------------------------------------
		// Derived

		// --------------------------------------------------
		// Instance
		public GeoTimezoneInfo Timezone { get { return GetTimezone(); } set { _timezone = value; } }
		public GeoCountryInfo Country { get { return _country; } set { SetCountry(value); } }
		public GeoRegionInfo Region { get { return _region; } set { SetRegion(value); } }



		// ==================================================
		// Methods




		// ==================================================
		// Privates
		private void SetCountry(GeoCountryInfo country)
		{
			// set country
			_country = country;

			// reset region
			if (_country == null) Region = null;

			// validate region in country
			if (Region != null && Region.CountryID != _country.CountryID) Region = null;
		}

		private void SetRegion(GeoRegionInfo region)
		{
			// set region
			_region = region;

			// update country
			if (_region != null)
			{
				// init country
				if (_country == null) _country = _region.Country;

				// reset country
				if (_region.CountryID != _country.CountryID) Country = _region.Country;
			}
		}

		private GeoTimezoneInfo GetTimezone()
		{
			if (_timezone == null)
			{
				// use region default
				if (_region != null) _timezone = _region.Timezone;

				// use country default
				if (_timezone == null && _country != null) _timezone = _country.Timezone;
			}

			return _timezone;
		}
	}
}
