using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;

namespace MetaGraffiti.Web.Admin.Models
{
	public class CartoViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public List<string> PlaceTypes { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }
		public List<CartoPlaceInfo> Places { get; set; }

		public bool HasChanges { get; set; }
		public DateTime? LastSaved { get; set; }


		// ==================================================
		// Optional
		public List<int> SourceYears{ get; set; }
		public List<GeoCountryInfo> SourceCountries { get; set; }

		public GeoCountryInfo SelectedCountry { get; set; }
		public string SelectedPlaceType { get; set; }

		public List<CartoPlaceInfo> ReportPlaces { get; set; }
		public CartoPlaceReportRequest ReportFilters { get; set; }


		// ==================================================
		// Helpers
		public bool IsFiltered { get { return ReportPlaces != null && Places.Count() != ReportPlaces.Count(); } }

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
		public static string GetPersistUrl(string url = "") { return $"/carto/persist/?url={url}"; }
		public static string GetReloadUrl() { return $"/carto/reload/"; }

		public static string GetCountryUrl(GeoCountryInfo country) { return $"/carto/country/{country.Name}/"; }
		public static string GetCountryUrl(GeoCountryInfo country, string types) { return $"/carto/country/{country.Name}/?types={types}"; }


		public static string GetSearchUrl() { return "/carto/report/"; }

		public static string GetPlacesUrl() { return $"/carto/places/"; }
		public static string GetPlacesUrl(GeoCountryInfo country) { return $"/carto/places/?Country={country.Name}"; }
		public static string GetPlacesUrl(CartoPlaceReportRequest filter) { return GetReportUrl(filter, "places"); }

		public static string GetReportUrl(string placeType = "") { return $"/carto/report/?PlaceType={placeType}"; }
		public static string GetReportUrl(GeoCountryInfo country) { return $"/carto/report/?Country={country.Name}"; }
		public static string GetReportUrl(CartoPlaceReportRequest filter, string mode = "report")
		{
			var url = $"/carto/{mode}/?";

			if (!String.IsNullOrWhiteSpace(filter.PlaceType)) url += $"PlaceType={filter.PlaceType}&";
			if (!String.IsNullOrWhiteSpace(filter.Country)) url += $"Country={filter.Country}&";
			if (!String.IsNullOrWhiteSpace(filter.Region)) url += $"Region={filter.Region}&";
			if (!String.IsNullOrWhiteSpace(filter.Name)) url += $"Name={filter.Name}&";

			return url;
		}
		
	}
}