using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Modules.Geo
{
	public class GeoPolitical
	{
		private IGeoPoliticalData _data { get; }

		public GeoPolitical(IGeoPoliticalData data)
		{
			_data = data;

			Timezone = GeoTimezoneInfo.Find(_data.Timezone);
			Country = GeoCountryInfo.Find(_data.Country);
			Region = GeoRegionInfo.Find(_data.Region);

			// attempt to backfill timezone
			if (Timezone == null && Region != null) Timezone = Graffiti.Geo.GuessTimezone(Region);
			if (Timezone == null && Country != null) Timezone = Graffiti.Geo.GuessTimezone(Country);
		}


		public GeoTimezoneInfo Timezone { get; private set; }
		public GeoCountryInfo Country { get; private set; }
		public GeoRegionInfo Region { get; private set; }

		public string TimezoneName { get { return (Timezone?.Name ?? ""); } }
		public string CountryName { get { return (Country?.Name ?? ""); } }
		public string RegionName { get { return (Region?.RegionName ?? ""); } }

		/// <summary>
		/// Timezone is required
		/// </summary>
		public bool IsTimezoneValid() { return Timezone != null; }

		/// <summary>
		/// Timezone is UTC
		/// </summary>
		public bool IsTimezoneUTC() { return IsTimezoneValid() && Timezone.WinTZ == "UTC"; }

		/// <summary>
		/// Timezone is valid but not UTC
		/// </summary>
		public bool IsTimezoneNotUTCValid() { return IsTimezoneValid() && !IsTimezoneUTC(); }


		/// <summary>
		/// Timezone is optional
		/// </summary>
		/// <returns></returns>
		public bool IsTimezoneOptionalButNotUTCValid() { return (IsTimezoneValid() && !IsTimezoneUTC()) || String.IsNullOrWhiteSpace(_data.Timezone); }


		/// <summary>
		/// Country is required
		/// </summary>
		public bool IsCountryValid() { return Country != null; }


		/// <summary>
		/// Region is required for countries with regions
		/// </summary>
		public bool IsRegionValid()
		{
			// no valid country, check region text is blank
			if (Country == null) return String.IsNullOrWhiteSpace(_data.Region);

			// valid country with regions, check region exists and matches
			if (Country.HasRegions) return Region != null && Region.CountryID == Country.CountryID;

			// valid country without regions, check region text is blank
			return String.IsNullOrWhiteSpace(_data.Region);
		}

		/// <summary>
		/// Region is optional for countries with regions
		/// </summary>
		public bool IsRegionOptionalValid()
		{
			// no valid country, check region text is blank
			if (Country == null) return String.IsNullOrWhiteSpace(_data.Region);

			// valid country with regions, check region is blank or exists and matches
			if (Country.HasRegions) return (String.IsNullOrWhiteSpace(_data.Region) || (Region != null && Region.CountryID == Country.CountryID));

			// valid country without regions, check region text is blank
			return String.IsNullOrWhiteSpace(_data.Region);
		}

	}
}
