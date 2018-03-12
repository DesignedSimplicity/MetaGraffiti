using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Modules.Crypto;
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
		public GeoLocationInfo()
		{
			_data = new GeoLocationData();
			ID = CryptoGraffiti.NewHashID();
		}
		public GeoLocationInfo(GeoLocationData data)
		{
			_data = data;
			
			Region = GeoRegionInfo.Find(data.Region);
			Country = GeoCountryInfo.Find(data.Country);

			ID = (String.IsNullOrWhiteSpace(data.PlaceKey)
					? CryptoGraffiti.NewHashID().ToUpperInvariant()
					: data.PlaceKey.ToUpperInvariant()
					);

			PlaceKey = data.PlaceKey;
			PlaceType = data.PlaceType;
			GoogleKey = data.GoogleKey;

			Name = data.Name;
			LocalName = data.LocalName;
			DisplayAs = data.DisplayAs;

			Description = data.Description;

			Locality = data.Locality;
			Address = data.Address;
			Postcode = data.Postcode;

			Subregions = data.Subregions;
			Sublocalities = data.Sublocalities;

			// override place type
			if (Name == Locality)
				PlaceType = "City";
			//else if (Region != null && Name == Region.RegionName)
				//PlaceType = "Region";
		}


		public string PlaceKey { get; set; }
		public string PlaceType { get; set; }
		public string GoogleKey { get; set; }


		public string Name { get; set; }
		public string LocalName { get; set; }
		public string DisplayAs { get; set; }

		public string Description { get; set; }

		//public string Website { get; set; }
		//public string Keywords { get; set; }


		public string Locality { get; set; }

		public string Address { get; set; }
		public string Postcode { get; set; }


		public string Subregions { get; set; } // Region2 \ 3 \ 4 \ 5

		public string Sublocalities { get; set; } // Sublocalities






		// ==================================================
		// Properties

		public string ID { get; private set; }

		public GeoLocationData Data => _data;



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
