using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class PlaceViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public bool HasChanges { get; set; }


		// ==================================================
		// Optional
		public CartoPlaceFormModel Edit { get; set; }
		public GeoCountryInfo SelectedCountry { get; set; }
		public CartoPlaceInfo SelectedPlace { get; set; }
		public PlaceSearchModel SearchCriteria { get; set; }
		public List<CartoPlaceInfo> SearchResults { get; set; }

		public List<CartoPlaceInfo> NearbyPlaces { get; set; }
		public List<CartoPlaceInfo> ContainedPlaces { get; set; }


		// ==================================================
		// Helpers



		// ==================================================
		// Navigation
		public static string GetDisplayUrl(string key) { return $"/place/update/{key}/"; } // TODO: CARTO: RC1: build display page
		public static string GetDisplayUrl(CartoPlaceInfo place) { return GetDisplayUrl(place.Key); }

		public static string GetUpdateUrl(string key) { return $"/place/update/{key}/"; }
		public static string GetUpdateUrl(CartoPlaceInfo place) { return GetUpdateUrl(place.Key); }

		public static string GetDeleteUrl(CartoPlaceInfo place) { return $"/place/delete/{place.Key}/"; }

		public static string GetPreviewUrl(string googlePlaceID, string text = "") { return $"/place/preview/{googlePlaceID}/?search={text}"; }
		public static string GetCreateUrl() { return "/place/create/"; }

		public static string GetSearchUrl() { return "/place/search/"; }
		public static string GetSearchUrl(IGeoLatLon point) { return $"/place/search/?latitude={point.Latitude}&longitude={point.Longitude}"; }
		public static string GetSearchUrl(string name, string country = "") { return $"/place/search/?name={name}&country={country}"; }

		public static string GetImportUrl(string country, string name) { return $"/place/search/?name={name}&country={country}"; }
	}

	public class PlaceSearchModel
	{
		// used for place search
		public string Name { get; set; }

		// used to limit country in place search
		public string Country { get; set; }

		// used for location search
		public string Location { get; set; }

		// used for reverse geocoding
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}