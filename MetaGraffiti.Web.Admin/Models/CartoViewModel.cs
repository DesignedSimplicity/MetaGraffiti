using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MetaGraffiti.Web.Admin.Models
{
	public class CartoViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public List<string> PlaceTypes { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }

		public List<CartoPlaceInfo> Places { get; set; }


		// ==================================================
		// Optional
		public CartoPlaceInfo SelectedPlace { get; set; }
		public GeoCountryInfo SelectedCountry { get; set; }

		public List<CartoPlaceInfo> ReportPlaces { get; set; }


		// ==================================================
		// Helpers
		public CartoPlaceInfo FindLocalityPlace(CartoPlaceInfo place)
		{
			if (String.IsNullOrWhiteSpace(place.Locality)) return null;
			var places = Places.Where(x => x.Country.CountryID == place.Country.CountryID && String.Compare(x.Name, place.Locality, true) == 0);
			if (place.Region == null)
				return places.FirstOrDefault();
			else
			{
				var p = places.Where(x => x.Region != null && x.Region.RegionID == place.Region.RegionID).FirstOrDefault();
				if (p != null)
					return p;
				else
					return places.FirstOrDefault();				
			}
		}


		// ==================================================
		// Navigation
		public static string GetCartoUrl() { return $"/carto/"; }
		public static string GetPersistUrl() { return $"/carto/persist/"; }
		public static string GetReloadUrl() { return $"/carto/reload/"; }

		public static string GetPlacesUrl() { return $"/carto/places/"; }
		public static string GetReportUrl(string placeType = "") { return $"/carto/report/?placeType={placeType}"; }
		public static string GetCountryUrl(GeoCountryInfo country) { return $"/carto/country/{country.Name}/"; }
		
		public static string GetSaveUrl() { return $"/carto/update/"; }		
		public static string GetEditUrl(string key) { return $"/carto/place/{key}/"; }
		public static string GetDeleteUrl(string key) { return $"/carto/delete/{key}/"; }
	}
}