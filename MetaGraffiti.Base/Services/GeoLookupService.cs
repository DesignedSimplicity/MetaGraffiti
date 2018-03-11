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
		public double LookupElevation(IGeoLatLon point)
		{
			return _google.RequestElevation(point).Elevation;
		}

		public GeoTimezoneInfo LookupTimezone(IGeoLatLon point)
		{
			var response = _google.RequestTimezone(point);
			return GeoTimezoneInfo.ByTZID(response.TimeZoneId);
		}

		public GeoLocationInfo LoadLocation(string googlePlaceId)
		{
			var response = _google.RequestLocation(googlePlaceId);
			var result = response.Results.FirstOrDefault();

			return result == null 
				? null 
				: ParseLocationResult(result);
		}

		public List<GeoLocationInfo> LookupLocations(string text)
		{
			var response = _google.RequestLocations(text);

			var list = new List<GeoLocationInfo>();
			foreach (var result in response.Results)
			{
				var location = ParseLocationResult(result);
				list.Add(location);
			}

			return list;
		}

		public List<GeoLocationInfo> LookupLocations(IGeoLatLon point)
		{
			var response = _google.RequestLocations(point);

			var list = new List<GeoLocationInfo>();
			foreach (var result in response.Results)
			{
				var location = ParseLocationResult(result);
				list.Add(location);
			}

			return list;
		}


		// ==================================================
		// Helpers
		private GeoLocationInfo ParseLocationResult(GoogleLocationResult result)
		{
			var data = new GeoLocationData();

			//data.PlaceKey = Cr
			data.PlaceType = result.TypedNameSource;
			//data.IconKey

			data.GoogleKey = result.PlaceID;

			//data.Timezone =
			data.Country = result.Country;
			data.Region = result.Region;

			data.Subregions = "";
			if (!String.IsNullOrWhiteSpace(result.Region2))
				data.Subregions += result.Region2;
			if (!String.IsNullOrWhiteSpace(result.Region3))
				data.Subregions += @" \ " + result.Region3;
			if (!String.IsNullOrWhiteSpace(result.Region4))
				data.Subregions += @" \ " + result.Region4;
			if (!String.IsNullOrWhiteSpace(result.Region5))
				data.Subregions += @" \ " + result.Region5;

			data.Name = TextTranslate.StripAccents(result.ShortName);
			data.LocalName = (data.Name == result.LongName ? "" : result.LongName);
			data.DisplayAs = result.ColloquialArea;
			data.Description = String.IsNullOrWhiteSpace(result.FormattedAddress) 
				? result.TypedName
				: result.FormattedAddress;

			var address = $"{result.StreeNumber} {result.Route}";
			data.StreetAddress = (String.IsNullOrWhiteSpace(address) ? result.Intersection : address);
			//City
			data.PostCode = result.PostalCode;

			data.Premise = result.Premise;
			if (!String.IsNullOrWhiteSpace(result.SubPremise)) data.Premise += @" \ " + result.SubPremise;

			data.Localities = result.Locality;
			if (!String.IsNullOrWhiteSpace(result.SubLocality)) data.Localities += @" \ " + result.SubLocality;

			data.Center = result.Center;
			data.Bounds = result.Bounds;

			return new GeoLocationInfo(data);
		}
	}
}
