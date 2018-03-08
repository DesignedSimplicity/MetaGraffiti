using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Services
{
	public class GoogleLocationSearchRequest
	{
		public string Name { get; set; }
		public string LocationType { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
	}

	public class GoogleLocationService : GoogleApiServiceBase
	{
		public GoogleLocationService(string apiKey) : base(apiKey) { }

		public GeoLocationInfo LookupLocation(IGeoLatLon point)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(IGeoLatLon point)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(string search)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(string search, GeoCountryInfo country)
		{
			return null;
		}

		public List<GeoLocationInfo> FindLocations(GoogleLocationSearchRequest search)
		{
			return null;
		}
	}
}
