using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Modules.Crypto;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Modules.Geo.Data
{
	public class GeoLocationData
	{
		public GeoLocationData() { PlaceKey = CryptoGraffiti.GetHashID(); }


		public string PlaceKey { get; set; } // place_id

		public string FullAddress { get; set; } // formatted_address

		public string Name { get; set; } // defaults to locality, district, region, country if not specified





		public string Country { get; set; } // country indicates the national political entity, and is typically the highest order type returned by the Geocoder
		public string Region { get; set; } // administrative_area_level_1 (State)
		public string District { get; set; } // administrative_area_level_2 (County)
		public string Sector { get; set; } // administrative_area_level_3 (Sector)
		public string Zone { get; set; } // administrative_area_level_4 (Zone)



		public IGeoCoordinate Center { get; set; }

		public IGeoPerimeter Bounds { get; set; }


		public string AlternativeName { get; set; } // colloquial_area indicates a commonly-used alternative name for the entity.
		public string StreetAddress { get; set; } // street_address indicates a precise street address.
		public string Route { get; set; } // route indicates a named route (such as "US 101").
		public string Intersection { get; set; } // intersection indicates a major intersection, usually of two major roads.
		public string PostalCode { get; set; } // postal_code indicates a postal code as used to address postal mail within the country.

		public string Neighborhood { get; set; } // neighborhood indicates a named neighborhood
		public string Ward { get; set; } // ward indicates a specific type of Japanese locality, to facilitate distinction between multiple locality components within a Japanese address.
		public string Locality { get; set; } // locality indicates an incorporated city or town political entity.
		public string[] SubLocalities { get; set; } // sublocality_level_1-5

		public string Premise { get; set; } // premise indicates a named location, usually a building or collection of buildings with a common name
		public string SubPremise { get; set; } // subpremise indicates a first-order entity below a named location, usually a singular building within a collection of buildings with a common name

		public string NaturalFeature { get; set; } // natural_feature indicates a prominent natural feature.
		public string Airport { get; set; } // airport indicates an airport.
		public string Park { get; set; } // park indicates a named park.
		public string PointOfInterest { get; set; } // point_of_interest indicates a named point of interest. Typically, these "POI"s are prominent local entities that don't easily fit in another category, such as "Empire State Building" or "Statue of Liberty."

		/*
		https://developers.google.com/maps/documentation/geocoding/intro#RegionCodes

		result_type — A filter of one or more address types, separated by a pipe (|). If the parameter contains multiple address types, the API returns all addresses that match any of the types. A note about processing: The result_type parameter does not restrict the search to the specified address type(s). Rather, the result_type acts as a post-search filter: the API fetches all results for the specified latlng, then discards those results that do not match the specified address type(s). Note: This parameter is available only for requests that include an API key or a client ID. The following values are supported:
			street_address indicates a precise street address.
			route indicates a named route (such as "US 101").
			intersection indicates a major intersection, usually of two major roads.
			political indicates a political entity. Usually, this type indicates a polygon of some civil administration.
			country indicates the national political entity, and is typically the highest order type returned by the Geocoder.
			administrative_area_level_1 indicates a first-order civil entity below the country level. Within the United States, these administrative levels are states. Not all nations exhibit these administrative levels. In most cases, administrative_area_level_1 short names will closely match ISO 3166-2 subdivisions and other widely circulated lists; however this is not guaranteed as our geocoding results are based on a variety of signals and location data.
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
