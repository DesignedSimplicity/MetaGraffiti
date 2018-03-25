using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class PlaceViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public List<int> Years { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }

		// ==================================================
		// Optional
		public int? SelectedYear { get; set; }
		public GeoCountryInfo SelectedCountry { get; set; }

		public List<CartoPlaceData> ImportPlaces { get; set; }
		public List<PlaceReportModel> ReportPlaces { get; set; }

		public CartoPlaceInfo SelectedPlace { get; set; }
		public PlaceSearchModel SearchCriteria { get; set; }
		public List<CartoPlaceInfo> SearchResults { get; set; }



		// ==================================================
		// Helpers
		public IEnumerable<PlaceReportModel> ListReportedPlaces()
		{
			return ReportPlaces.OrderBy(x => x.Data.Country).ThenBy(x => x.Data.Name);
		}

		public bool IsSelected(int year)
		{
			return (SelectedYear ?? -1) == year;
		}

		public bool IsSelected(GeoCountryInfo country)
		{
			if (SelectedCountry == null && country == null) return true;
			if (SelectedCountry == null || country == null) return false;
			return (SelectedCountry.CountryID == country.CountryID);
		}


		public string GetStatusCss(PlaceReportModel model)
		{
			if (String.IsNullOrWhiteSpace(model.Data.Country))
				return "table-danger";
			else if (model.Place != null)
				return "table-success";
			else
				return "";
		}


		// ==================================================
		// Navigation
		public static string GetReportUrl(int year) { return $"/place/report/?year={year}"; }
		public static string GetReportUrl(string country) { return $"/place/report/?country={country}"; }

		public static string GetSearchUrl() { return "/place/search/"; }
		public static string GetSearchUrl(string name, string country = "") { return $"/place/search/?name={name}&country={country}"; }

		public static string GetPreviewUrl(string googlePlaceID, string text = "") { return $"/place/preview/{googlePlaceID}/?search={text}"; }

		public static string GetCreateUrl() { return "/place/create/"; }
	}

	public class PlaceReportModel
	{
		public CartoPlaceData Data { get; set; }
		public CartoPlaceInfo Place { get; set; }
	}

	public class PlaceSearchModel
	{
		public string Name { get; set; }

		public string Region { get; set; }

		public string Country { get; set; }

		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}