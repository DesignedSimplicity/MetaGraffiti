using System;
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
