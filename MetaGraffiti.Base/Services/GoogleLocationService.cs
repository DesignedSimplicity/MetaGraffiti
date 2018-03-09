using System;
using System.Collections.Generic;
using System.Text;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Data;
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

	// https://developers.google.com/maps/documentation/geocoding/start
	// TODO: https://developers.google.com/maps/documentation/javascript/examples/geocoding-simple
	// TODO: https://developers.google.com/maps/documentation/javascript/examples/geocoding-reverse
	public class GoogleLocationService : GoogleApiServiceBase
	{
		private Dictionary<string, dynamic> _cache = new Dictionary<string, dynamic>();

		public GoogleLocationService(string apiKey) : base(apiKey) { }

		public GeoLocationInfo LookupGeoLocation(IGeoLatLon point)
		{
			var response = RequestLocations(point);
			var results = response.results;
			var first = results[0];

			var location = ParseLocationResult(first);

			return location;
		}

		public List<GeoLocationInfo> LookupGeoLocations(IGeoLatLon point)
		{
			var response = RequestLocations(point);
			var results = response.results;

			var list = new List<GeoLocationInfo>();
			foreach (var result in results)
			{
				var location = ParseLocationResult(result);
				list.Add(location);
			}

			return list;
		}

		public dynamic RequestLocations(IGeoLatLon point)
		{
			var location = GetLocationString(point);

			var key = location;
			if (_cache.ContainsKey(key)) return _cache[key];

			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/geocode/json", Method.GET);

			request.AddParameter("key", _apiKey);
			request.AddParameter("latlng", location);

			var response = client.Execute(request);
			dynamic data = JsonConvert.DeserializeObject(response.Content);
			
			if (!_cache.ContainsKey(key)) _cache.Add(key, data);
			return data;
		}

		private GeoLocationInfo ParseLocationResult(dynamic result)
		{
			var data = new GeoLocationData();

			data.PlaceKey = result.place_id;
			data.FullAddress = result.formatted_address;

			var isCountry = false;
			var firstComponent = result.address_components[0];
			if (firstComponent != null)
			{
				data.Name = firstComponent.short_name;
				data.DisplayName = firstComponent.short_name;
				data.DisplayNameType = "Political";
				isCountry = firstComponent.types[0].Value == "country" && result.address_components.Count == 1;
			}

			#region Address Components
			foreach (var component in result.address_components)
			{
				foreach (var item in component.types)
				{
					if (item.Value == "country")
						data.Country = component.short_name;

					if (item.Value == "administrative_area_level_1")
						data.Region = component.short_name;
					if (item.Value == "administrative_area_level_2")
						data.Region2 = component.short_name;
					if (item.Value == "administrative_area_level_3")
						data.Region3 = component.short_name;
					if (item.Value == "administrative_area_level_4")
						data.Region4 = component.short_name;
					if (item.Value == "administrative_area_level_5")
						data.Region5 = component.short_name;

					if (item.Value == "premise")
						data.Premise = component.short_name;

					if (item.Value == "locality")
						data.Locality = component.short_name;
					if (item.Value == "sublocality")
						data.SubLocality = component.short_name;

					if (item.Value == "neighborhood")
						data.Neighborhood = component.short_name;
					if (item.Value == "colloquial_area")
						data.AreaName = component.short_name;

					if (item.Value == "street_number")
						data.StreeNumber = component.short_name;
					if (item.Value == "route")
						data.Route = component.short_name;
					if (item.Value == "postal_code")
						data.PostalCode = component.short_name;

					if (item.Value == "point_of_interest")
						data.PointOfInterest = component.short_name;
					if (item.Value == "park")
						data.Park = component.short_name;
					if (item.Value == "airport")
						data.Airport = component.short_name;
					if (item.Value == "natural_feature")
						data.NaturalFeature = component.short_name;
				}
			}
			#endregion

			#region Default Name
			if (isCountry && !String.IsNullOrWhiteSpace(data.Country))
			{
				data.DisplayName = data.Country;
				data.DisplayNameType = "Country";
			}
			if (!String.IsNullOrWhiteSpace(data.Region))
			{
				data.DisplayName = data.Region;
				data.DisplayNameType = "Region";
			}
			if (!String.IsNullOrWhiteSpace(data.Region2))
			{
				data.DisplayName = data.Region2 + ", " + data.DisplayName;
				data.DisplayNameType = "Region2";
			}
			if (!String.IsNullOrWhiteSpace(data.Region3))
			{
				data.DisplayName = data.Region3 + ", " + data.DisplayName;
				data.DisplayNameType = "Region3";
			}
			if (!String.IsNullOrWhiteSpace(data.Region4))
			{
				data.DisplayName = data.Region4 + ", " + data.DisplayName;
				data.DisplayNameType = "Region4";
			}
			if (!String.IsNullOrWhiteSpace(data.Region5))
			{
				data.DisplayName = data.Region5 + ", " + data.DisplayName;
				data.DisplayNameType = "Region5";
			}

			if (!String.IsNullOrWhiteSpace(data.Locality))
			{
				data.DisplayName = data.Locality;
				data.DisplayNameType = "Locality";
			}
			if (!String.IsNullOrWhiteSpace(data.SubLocality))
			{
				data.DisplayName = data.SubLocality + ", " + data.DisplayName;
				data.DisplayNameType = "SubLocality";
			}
			if (!String.IsNullOrWhiteSpace(data.Neighborhood))
			{
				data.DisplayName = data.Neighborhood;
				data.DisplayNameType = "Neighborhood";
			}
			if (!String.IsNullOrWhiteSpace(data.AreaName))
			{
				data.DisplayName = data.AreaName;
				data.DisplayNameType = "AreaName";
			}

			if (!String.IsNullOrWhiteSpace(data.PostalCode))
			{
				data.DisplayName = data.FullAddress;
				data.DisplayNameType = "PostalCode";
			}
			if (!String.IsNullOrWhiteSpace(data.StreeNumber))
			{
				data.DisplayName = data.FullAddress;
				data.DisplayNameType = "Address";
			}

			if (!String.IsNullOrWhiteSpace(data.PointOfInterest))
			{
				data.DisplayName = data.PointOfInterest;
				data.DisplayNameType = "POI";
			}
			if (!String.IsNullOrWhiteSpace(data.Park))
			{
				data.DisplayName = data.Park;
				data.DisplayNameType = "Park";
			}
			if (!String.IsNullOrWhiteSpace(data.Airport))
			{
				data.DisplayName = data.Airport;
				data.DisplayNameType = "Airport";
			}
			if (!String.IsNullOrWhiteSpace(data.NaturalFeature))
			{
				data.DisplayName = data.NaturalFeature;
				data.DisplayNameType = "NaturalFeature";
			}
			#endregion

			#region Perimeter
			var location = new GeoLocationInfo(data);

			var center = result.geometry.location;
			if (center != null) data.Center = new GeoPosition(
				TypeConvert.ToDouble(center.lat.Value), 
				TypeConvert.ToDouble(center.lng.Value)
				);

			var bounds = result.geometry.viewport;
			if (bounds != null) data.Bounds = new GeoRectangle(
				TypeConvert.ToDouble(bounds.northeast.lat.Value),
				TypeConvert.ToDouble(bounds.southwest.lng.Value),
				TypeConvert.ToDouble(bounds.southwest.lat.Value),
				TypeConvert.ToDouble(bounds.northeast.lng.Value)
				);
			#endregion


			data.RawData = JsonConvert.SerializeObject(result);

			return location;
		}

		/*
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
		}*/
	}
}
