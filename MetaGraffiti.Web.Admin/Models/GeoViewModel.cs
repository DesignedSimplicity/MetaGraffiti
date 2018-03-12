using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GeoViewModel : AdminViewModel
	{
		public List<GeoTimezoneInfo> Timezones { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }
		public List<GeoRegionInfo> Regions { get; set; }

		public List<GeoLocationInfo> Locations { get; set; }

		public GeoLocationInfo SelectedLocation { get; set; }

		public GeoCountryInfo SelectedCountry { get; set; }

		public List<GeoCountryInfo> VisitedCountries { get; set; }


		public GeoLocationSearchModel Search { get; set; } = new GeoLocationSearchModel();



		public HtmlString GetJson(GeoLocationInfo location)
		{
			if (location == null) return new HtmlString("{}");

			return new HtmlString(location.ToJson());
		}

		public HtmlString GetData(GeoLocationInfo location)
		{
			return new HtmlString(Json.Encode(location.Data));
		}
	}

	public class GeoLocationSearchModel
	{
		public string Name { get; set; }

		public string Region { get; set; }

		public string Country { get; set; }

		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}

	public class GeoLocationUpdateModel
	{
		public string PlaceKey { get; set; }
		public string PlaceType { get; set; }

		//public string GoogleKey { get; set; }
		public string IconKey { get; set; }

		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }


		public string Name { get; set; }
		public string LocalName { get; set; }
		public string DisplayAs { get; set; }
		public string Description { get; set; }

		public string Address { get; set; }
		public string Locality { get; set; }
		public string Postcode { get; set; }

		public string Premise { get; set; }

		public string Subregions { get; set; }
		public string Localities { get; set; }


		public IGeoCoordinate Center { get; set; }
		public IGeoPerimeter Bounds { get; set; }
	}
}