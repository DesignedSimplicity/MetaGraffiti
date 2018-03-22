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
		public List<CartoPlaceInfo> ExistingPlaces { get; set; }

		public List<CartoPlaceData> ImportPlaces { get; set; }

		public List<PlaceModel> Places { get; set; }

		public List<GeoCountryInfo> Countries { get; set; }
		public List<int> Years { get; set; }



		public IEnumerable<PlaceModel> ListPlaces()
		{
			return Places.OrderBy(x => x.Data.Country).ThenBy(x => x.Data.Name);
		}



		public int? SelectedYear { get; set; }
		public GeoCountryInfo SelectedCountry { get; set; }



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



		public static string GetSearchUrl(string name, string country = "") { return $"/places/search/?name={name} {country}".TrimEnd(); }
	}

	public class PlaceModel
	{
		public CartoPlaceData Data { get; set; }
		public CartoPlaceInfo Place { get; set; }
	}
}