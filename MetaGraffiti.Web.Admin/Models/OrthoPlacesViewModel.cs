using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class OrthoPlacesViewModel : OrthoViewModel
	{
		// ==================================================
		// Required
		public List<GeoCountryInfo> Countries { get; set; }
		public List<OrthoPlaceImportModel> Places { get; set; }


		// ==================================================
		// Optional
		public GeoCountryInfo SelectedCountry { get; set; }


		// ==================================================
		// Helpers
		public IEnumerable<OrthoPlaceImportModel> ListPlaces()
		{
			var places = Places.AsQueryable();
			if (SelectedCountry == null) places = places.OrderBy(x => x.Data.Country);
			return places.OrderBy(x => x.Data.Name);
		}

		public bool IsSelected(GeoCountryInfo country)
		{
			if (SelectedCountry == null && country == null) return true;
			if (SelectedCountry == null || country == null) return false;
			return (SelectedCountry.CountryID == country.CountryID);
		}

		public string GetStatusCss(OrthoPlaceImportModel model)
		{
			if (String.IsNullOrWhiteSpace(model.Data.Country))
				return "table-danger";
			else if (model.Place != null)
				return "table-success";
			else
				return "";
		}
	}

	// ==================================================
	// Additional
	public class OrthoPlaceImportModel
	{
		public CartoPlaceData Data { get; set; }
		public CartoPlaceInfo Place { get; set; }
	}
}