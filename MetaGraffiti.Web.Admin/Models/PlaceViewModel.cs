using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class PlaceViewModel : AdminViewModel
	{
		public List<int> Years { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }

		public int? SelectedYear { get; set; }
		public GeoCountryInfo SelectedCountry { get; set; }



		public List<CartoPlaceInfo> ExistingPlaces { get; set; }

		public List<CartoPlaceData> ImportPlaces { get; set; }

		public List<PlaceModel> FilteredPlaces { get; set; }


		public CartoPlaceInfo Place { get; set; }
		public CartoPlaceSearch SearchCriteria { get; set; }
		public List<CartoPlaceInfo> SearchResults { get; set; }




		public IEnumerable<PlaceModel> ListPlaces()
		{
			return FilteredPlaces.OrderBy(x => x.Data.Country).ThenBy(x => x.Data.Name);
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


		public string GetStatusCss(PlaceModel model)
		{
			if (String.IsNullOrWhiteSpace(model.Data.Country))
				return "table-danger";
			else if (model.Place != null)
				return "table-success";
			else
				return "";
		}


		public HtmlString GetPlacesJson()
		{
			return JsonHelper.GetJson(SearchResults);
		}


		public static string GetReportUrl(int year) { return $"/place/report/?year={year}"; }
		public static string GetReportUrl(string country) { return $"/place/report/?country={country}"; }

		public static string GetSearchUrl() { return "/place/search/"; }
		public static string GetSearchUrl(string name, string country = "") { return $"/place/search/?name={name}&country={country}"; }

		public static string GetPreviewUrl(string id) { return $"/place/preview/{id}"; }

		public static string GetCreateUrl() { return "/place/create/"; }
	}

	public class PlaceModel
	{
		public CartoPlaceData Data { get; set; }
		public CartoPlaceInfo Place { get; set; }

		//public CartoPlaceInfo Locality { get; set; }
		//public CartoPlaceInfo RegionPlace { get; set; }
	}

	// TODO: refactor name
	public class CartoPlaceSearch
	{
		public string Name { get; set; }

		public string Region { get; set; }

		public string Country { get; set; }

		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}