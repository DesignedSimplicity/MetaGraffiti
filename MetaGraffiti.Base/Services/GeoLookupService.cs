﻿using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Base.Services
{
	// TODO: refactor this into a general GeoGraffiti service layer
	public class GeoLookupService
	{
		// ==================================================
		// Internals
		private GoogleApiService _google = null;


		// ==================================================
		// Constructors
		public GeoLookupService(GoogleApiService google)
		{
			_google = google;
		}


		// ==================================================
		// Methods

		// --------------------------------------------------
		// Elevation
		public double LookupElevation(IGeoLatLon point)
		{
			return _google.RequestElevation(point).Elevation;
		}

		// --------------------------------------------------
		// Timezone
		public GeoTimezoneInfo LookupTimezone(IGeoLatLon point)
		{
			var response = _google.RequestTimezone(point);
			return GeoTimezoneInfo.ByTZID(response.TimeZoneId);
		}

		public GeoTimezoneInfo FindTimezone(string name)
		{
			return GeoTimezoneInfo.All.Where(x => x.TZID.EndsWith(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
		}

		public GeoTimezoneInfo GuessTimezone(GeoCountryInfo country)
		{
			if (country == null) return null;

			switch (country.ISO2)
			{
				case "AR": return GeoTimezoneInfo.ByTZID("America/Buenos_Aires");
				case "CL": return GeoTimezoneInfo.ByTZID("America/Santiago");
				case "JP": return GeoTimezoneInfo.ByTZID("Asia/Tokyo");
				case "NZ": return GeoTimezoneInfo.ByTZID("Pacific/Auckland");
				default: return null;
			}
		}


		// TODO: update and fix this
		private GeoTimezoneInfo GuessTimezone(IEnumerable<GeoCountryInfo> countries, IEnumerable<GeoRegionInfo> regions)
		{
			//string[] countryOrder = AutoConfig.VisitedCountries;// { "UR", "CL", "AR", "AU", "BE", "BR", "CH", "CN", "DK", "FR", "HK", "IN", "IS", "JP", "MN", "NL", "NZ", "RU", "SG", "CA", "MX", "JM", "AN" };
			GeoCountryInfo country = countries.FirstOrDefault();

			var regionCountries = regions.Select(x => x.Country).Distinct().Count();
			if (regionCountries == 1) // 1 country with multiple regions
				country = regions.First().Country;
			/*
			else if (countries.Count() == 1) // 1 country without regions
				country = countries.First();
			else // multiple countries
			{
				foreach(var c in countryOrder)
				{
					// pick first visited country
					if (countries.Any(x => x.ISO2 == c))
					{
						country = GeoCountryInfo.ByISO(c);
						break;
					}
				}
			}
			*/

			// now pick timezone
			if (country == null)
				return GeoTimezoneInfo.LocalTimezone;

			// simple country level
			switch (country.ISO2)
			{
				case "AR": return GeoTimezoneInfo.ByKey("Argentina");
				case "CL": return GeoTimezoneInfo.ByKey("Pacific SA");
				case "NZ": return GeoTimezoneInfo.ByKey("New Zealand");
				case "JP": return GeoTimezoneInfo.ByKey("Tokyo");
				case "SG": return GeoTimezoneInfo.ByKey("Singapore");
			}

			// assume europe is single timezone
			if (country.Continent == GeoContinents.Europe)
				return GeoTimezoneInfo.ByKey("W. Europe");

			// break australia down into regions
			if (country.ISO2 == "AU")
			{
				if (regions.Any(x => x.RegionName == "Tasmania"))
					return GeoTimezoneInfo.ByKey("Tasmania");
				else if (regions.Any(x => x.RegionName == "Western Australia"))
					return GeoTimezoneInfo.ByKey("W. Australia");
				else if (regions.Any(x => x.RegionName == "New South Wales"))
					return GeoTimezoneInfo.ByKey("AUS Eastern");
				else if (regions.Any(x => x.RegionName == "Queensland"))
					return GeoTimezoneInfo.ByKey("AUS Eastern");
			}

			// default to UTC
			return GeoTimezoneInfo.ByKey("UTC");
		}


		// --------------------------------------------------
		// Country
		public GeoCountryInfo NearestCountry(IGeoLatLon point)
		{
			var countries = GeoCountryInfo.ListByLocation(point).OrderBy(x => GeoDistance.BetweenPoints(x.Center, point).Meters);
			return countries.FirstOrDefault();
		}

		public List<GeoCountryInfo> SearchCountries(string name)
		{
			var list = new List<GeoCountryInfo>();
			if (String.IsNullOrWhiteSpace(name)) return list;

			var country = GeoCountryInfo.Find(name);
			if (country != null)
			{
				list.Add(country);
				return list;
			}

			return GeoCountryInfo.All.Where(x => x.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)).ToList();
		}

		// --------------------------------------------------
		// Region
		public List<GeoRegionInfo> NearbyRegions(IGeoLatLon point)
		{
			return GeoRegionInfo.ListByLocation(point).OrderBy(x => GeoDistance.BetweenPoints(x.Center, point).Meters).ToList();
		}

		public GeoRegionInfo NearestRegion(IGeoLatLon point)
		{
			return NearbyRegions(point).FirstOrDefault();
		}

		public List<GeoRegionInfo> SearchRegions(string name, GeoCountryInfo country)
		{
			var list = new List<GeoRegionInfo>();
			if (String.IsNullOrWhiteSpace(name)) return list;

			var countryID = (country?.CountryID ?? 0);
			var region = GeoRegionInfo.Find(name);
			if (region != null)
			{
				if (countryID == 0 || region.CountryID == countryID)
				{
					list.Add(region);
					return list;
				}
			}

			var locals = GeoRegionInfo.All.Where(x => x.CountryID == countryID);
			return locals.Where(x => x.RegionName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)
				|| x.RegionAbbr.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)
				|| x.RegionNameLocal.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)
				).ToList();
		}
	}
}
