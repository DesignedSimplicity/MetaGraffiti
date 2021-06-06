using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Web.Core.Models
{
	public class CountryFilterModel
	{
		public CountryFilterModel(IEnumerable<GeoCountryInfo> countries, GeoCountryInfo selected = null, string cssClass = "geo-color")
		{
			Countries = countries;
			SelectedCountry = selected;
			CssClass = cssClass;
		}

		public IEnumerable<GeoCountryInfo> Countries { get; set; }
		public GeoCountryInfo SelectedCountry { get; set; }
		public string CssClass { get; set; }

		public bool IsSelected(GeoCountryInfo country)
		{
			if (country == null || SelectedCountry == null) return false;
			return (country.CountryID == SelectedCountry.CountryID);
		}
	}
}