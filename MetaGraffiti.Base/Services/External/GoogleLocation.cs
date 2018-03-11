﻿using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Common;

namespace MetaGraffiti.Base.Services.External
{
	// https://developers.google.com/maps/documentation/geocoding/start
	// https://developers.google.com/maps/documentation/javascript/examples/geocoding-simple
	// https://developers.google.com/maps/documentation/javascript/examples/geocoding-reverse
	public class GoogleLocationResponse
	{
		private string _data;

		public GoogleLocationResponse(string data)
		{
			_data = data;

			dynamic d = JsonConvert.DeserializeObject(_data);
			var results = d.results;

			Results = new List<GoogleLocationResult>();
			foreach (var result in results)
			{
				Results.Add(new GoogleLocationResult(result));
			}
		}

		public string Status { get; set; }
		public List<GoogleLocationResult> Results { get; private set; }
	}

	public class GoogleLocationResult
	{
		private dynamic _data;

		public GoogleLocationResult(dynamic data) { _data = data; ParseData(); }

		public dynamic Data => _data;


		public string Name { get; private set; }
		public string ShortName { get; private set; }
		public string NameSource { get; private set; }

		public string PlaceID { get; private set; }

		public string Country { get; private set; } // country indicates the national political entity, and is typically the highest order type returned by the Geocoder
		public string Region { get; private set; } // administrative_area_level_1
		public string Region2 { get; private set; } // administrative_area_level_2
		public string Region3 { get; private set; } // administrative_area_level_3
		public string Region4 { get; private set; } // administrative_area_level_4
		public string Region5 { get; private set; } // administrative_area_level_5


		public IGeoCoordinate Center { get; private set; }

		public IGeoPerimeter Bounds { get; private set; }

		public string ColloquialArea { get; private set; } // colloquial_area indicates a commonly-used alternative name for the entity.

		public string FullAddress { get; private set; } // formatted_address
		public string StreeNumber { get; private set; } // street_address indicates a precise street address.
		public string Route { get; private set; } // route indicates a named route (such as "US 101").
		public string Intersection { get; private set; } // intersection indicates a major intersection, usually of two major roads.
		public string PostalCode { get; private set; } // postal_code indicates a postal code as used to address postal mail within the country.

		public string Neighborhood { get; private set; } // neighborhood indicates a named neighborhood
												 //public string Ward { get; set; } // ward indicates a specific type of Japanese locality, to facilitate distinction between multiple locality components within a Japanese address.

		public string Locality { get; private set; } // locality indicates an incorporated city or town political entity.
		public string SubLocality { get; private set; } // sublocality indicates a first-order civil entity below a locality. For some locations may receive one of the additional types: sublocality_level_1 to sublocality_level_5. Each sublocality level is a civil entity. Larger numbers indicate a smaller geographic area.
												//public string[] SubLocalities { get; set; }

		public string Premise { get; private set; } // premise indicates a named location, usually a building or collection of buildings with a common name
		public string SubPremise { get; private set; } // subpremise indicates a first-order entity below a named location, usually a singular building within a collection of buildings with a common name

		public string NaturalFeature { get; private set; } // natural_feature indicates a prominent natural feature.
		public string Airport { get; private set; } // airport indicates an airport.
		public string Park { get; private set; } // park indicates a named park.
		public string PointOfInterest { get; private set; } // point_of_interest indicates a named point of interest. Typically, these "POI"s are prominent local entities that don't easily fit in another category, such as "Empire State Building" or "Statue of Liberty."


		private void ParseData()
		{
			this.PlaceID = _data.place_id;
			this.FullAddress = _data.formatted_address;

			var isCountry = false;
			var firstComponent = _data.address_components[0];
			if (firstComponent != null)
			{
				this.ShortName = firstComponent.short_name;
				this.Name = firstComponent.short_name;
				this.NameSource = "Political";
				isCountry = firstComponent.types[0].Value == "country" && _data.address_components.Count == 1;
			}

			#region Address Components
			foreach (var component in _data.address_components)
			{
				foreach (var item in component.types)
				{
					if (item.Value == "country")
						this.Country = component.short_name;

					if (item.Value == "administrative_area_level_1")
						this.Region = component.short_name;
					if (item.Value == "administrative_area_level_2")
						this.Region2 = component.short_name;
					if (item.Value == "administrative_area_level_3")
						this.Region3 = component.short_name;
					if (item.Value == "administrative_area_level_4")
						this.Region4 = component.short_name;
					if (item.Value == "administrative_area_level_5")
						this.Region5 = component.short_name;

					if (item.Value == "premise")
						this.Premise = component.short_name;

					if (item.Value == "locality")
						this.Locality = component.short_name;
					if (item.Value == "sublocality")
						this.SubLocality = component.short_name;

					if (item.Value == "neighborhood")
						this.Neighborhood = component.short_name;
					if (item.Value == "colloquial_area")
						this.ColloquialArea = component.short_name;

					if (item.Value == "street_number")
						this.StreeNumber = component.short_name;
					if (item.Value == "intersection")
						this.Intersection = component.short_name;
					if (item.Value == "route")
						this.Route = component.short_name;
					if (item.Value == "postal_code")
						this.PostalCode = component.short_name;

					if (item.Value == "point_of_interest")
						this.PointOfInterest = component.short_name;
					if (item.Value == "park")
						this.Park = component.short_name;
					if (item.Value == "airport")
						this.Airport = component.short_name;
					if (item.Value == "natural_feature")
						this.NaturalFeature = component.short_name;
				}
			}
			#endregion

			#region Default Name
			if (isCountry && !String.IsNullOrWhiteSpace(this.Country))
			{
				this.Name = this.Country;
				this.NameSource = "Country";
			}
			if (!String.IsNullOrWhiteSpace(this.Region))
			{
				this.Name = this.Region;
				this.NameSource = "Region";
			}
			if (!String.IsNullOrWhiteSpace(this.Region2))
			{
				this.Name = this.Region2 + ", " + this.Name;
				this.NameSource = "Region2";
			}
			if (!String.IsNullOrWhiteSpace(this.Region3))
			{
				this.Name = this.Region3 + ", " + this.Name;
				this.NameSource = "Region3";
			}
			if (!String.IsNullOrWhiteSpace(this.Region4))
			{
				this.Name = this.Region4 + ", " + this.Name;
				this.NameSource = "Region4";
			}
			if (!String.IsNullOrWhiteSpace(this.Region5))
			{
				this.Name = this.Region5 + ", " + this.Name;
				this.NameSource = "Region5";
			}

			if (!String.IsNullOrWhiteSpace(this.Locality))
			{
				this.Name = this.Locality;
				this.NameSource = "Locality";
			}
			if (!String.IsNullOrWhiteSpace(this.SubLocality))
			{
				this.Name = this.SubLocality + ", " + this.Name;
				this.NameSource = "SubLocality";
			}
			if (!String.IsNullOrWhiteSpace(this.Neighborhood))
			{
				this.Name = this.Neighborhood;
				this.NameSource = "Neighborhood";
			}
			if (!String.IsNullOrWhiteSpace(this.ColloquialArea))
			{
				this.Name = this.ColloquialArea;
				this.NameSource = "AreaName";
			}

			if (!String.IsNullOrWhiteSpace(this.PostalCode))
			{
				this.Name = this.FullAddress;
				this.NameSource = "PostalCode";
			}
			if (!String.IsNullOrWhiteSpace(this.StreeNumber))
			{
				this.Name = this.FullAddress;
				this.NameSource = "Address";
			}

			if (!String.IsNullOrWhiteSpace(this.PointOfInterest))
			{
				this.Name = this.PointOfInterest;
				this.NameSource = "POI";
			}
			if (!String.IsNullOrWhiteSpace(this.Park))
			{
				this.Name = this.Park;
				this.NameSource = "Park";
			}
			if (!String.IsNullOrWhiteSpace(this.Airport))
			{
				this.Name = this.Airport;
				this.NameSource = "Airport";
			}
			if (!String.IsNullOrWhiteSpace(this.NaturalFeature))
			{
				this.Name = this.NaturalFeature;
				this.NameSource = "NaturalFeature";
			}
			#endregion

			#region Perimeter

			var center = _data.geometry.location;
			if (center != null) this.Center = new GeoPosition(
				TypeConvert.ToDouble(center.lat.Value),
				TypeConvert.ToDouble(center.lng.Value)
				);

			var bounds = _data.geometry.viewport;
			if (bounds != null) this.Bounds = new GeoRectangle(
				TypeConvert.ToDouble(bounds.northeast.lat.Value),
				TypeConvert.ToDouble(bounds.southwest.lng.Value),
				TypeConvert.ToDouble(bounds.southwest.lat.Value),
				TypeConvert.ToDouble(bounds.northeast.lng.Value)
				);
			#endregion
		}

		/*
		https://developers.google.com/maps/documentation/geocoding/intro#RegionCodes

		result_type — A filter of one or more address types, separated by a pipe (|). If the parameter contains multiple address types, the API returns all addresses that match any of the types. A note about processing: The result_type parameter does not restrict the search to the specified address type(s). Rather, the result_type acts as a post-search filter: the API fetches all results for the specified latlng, then discards those results that do not match the specified address type(s). Note: This parameter is available only for requests that include an API key or a client ID. The following values are supported:
			street_address indicates a precise street address.
			route indicates a named route (such as "US 101").
			intersection indicates a major intersection, usually of two major roads.
			political indicates a political entity. Usually, this type indicates a polygon of some civil administration.
			country indicates the national political entity, and is typically the highest order type returned by the Geocoder.
			administrative_area_level_1 indicates a first-order civil entity below the country level. Within the United States, these administrative levels are states. Not all nations exhibit these administrative levels. In most cases, administrative_area_level_1 short names will closely match ISO 3166-2 subdivisions and other widely circulated lists; however this is not guaranteed as our geocoding results are based on a variety of signals and location this.
			administrative_area_level_2 indicates a second-order civil entity below the country level. Within the United States, these administrative levels are counties. Not all nations exhibit these administrative levels.
			administrative_area_level_3 indicates a third-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
			administrative_area_level_4 indicates a fourth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
			administrative_area_level_5 indicates a fifth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
			colloquial_area indicates a commonly-used alternative name for the entity.
			locality indicates an incorporated city or town political entity.
			ward indicates a specific type of Japanese locality, to facilitate distinction between multiple locality components within a Japanese address.
			sublocality indicates a first-order civil entity below a locality. For some locations may receive one of the additional types: sublocality_level_1 to sublocality_level_5. Each sublocality level is a civil entity. Larger numbers indicate a smaller geographic area.
			
			* neighborhood indicates a named neighborhood
			premise indicates a named location, usually a building or collection of buildings with a common name
			subpremise indicates a first-order entity below a named location, usually a singular building within a collection of buildings with a common name
			postal_code indicates a postal code as used to address postal mail within the country.
			
			* natural_feature indicates a prominent natural feature.
			* airport indicates an airport.
			* park indicates a named park.
			* point_of_interest indicates a named point of interest. Typically, these "POI"s are prominent local entities that don't easily fit in another category, such as "Empire State Building" or "Statue of Liberty."
		
		location_type — A filter of one or more location types, separated by a pipe (|). If the parameter contains multiple location types, the API returns all addresses that match any of the types. A note about processing: The location_type parameter does not restrict the search to the specified location type(s). Rather, the location_type acts as a post-search filter: the API fetches all results for the specified latlng, then discards those results that do not match the specified location type(s). Note: This parameter is available only for requests that include an API key or a client ID. The following values are supported:
			"ROOFTOP" returns only the addresses for which Google has location information accurate down to street address precision.
			"RANGE_INTERPOLATED" returns only the addresses that reflect an approximation (usually on a road) interpolated between two precise points (such as intersections). An interpolated range generally indicates that rooftop geocodes are unavailable for a street address.
			"GEOMETRIC_CENTER" returns only geometric centers of a location such as a polyline (for example, a street) or polygon (region).
			"APPROXIMATE" returns only the addresses that are characterized as approximate.
 */
	}
}
