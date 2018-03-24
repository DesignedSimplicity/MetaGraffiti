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
		public List<string> PlaceTypes { get; set; }
		public List<GeoCountryInfo> Countries { get; set; }

		public List<CartoPlaceInfo> Places { get; set; }

		public List<CartoPlaceInfo> ReportPlaces { get; set; }

		public CartoPlaceInfo Place { get; set; }
		public GeoCountryInfo Country { get; set; }



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


		public HtmlString GetPlacesJson()
		{
			return JsonViewModel.GetJson(Places);
		}

		public HtmlString GetJson(CartoPlaceInfo place)
		{
			if (place == null) return new HtmlString("{}");

			return new HtmlString(place.ToJson());
		}


		public static string GetCartoUrl() { return $"/carto/"; }
		public static string GetPersistUrl() { return $"/carto/persist/"; }
		public static string GetReloadUrl() { return $"/carto/reload/"; }


		public static string GetPlacesUrl() { return $"/carto/places/"; }
		public static string GetReportUrl(string placeType = "") { return $"/carto/report/?placeType={placeType}"; }
		public static string GetCountryUrl(GeoCountryInfo country) { return $"/carto/country/{country.Name}"; }

		
		public static string GetSaveUrl() { return $"/carto/update/"; }		
		public static string GetEditUrl(string key) { return $"/carto/place/{key}"; }
		public static string GetDeleteUrl(string key) { return $"/carto/delete/{key}"; }
	}

	public class CartoPlaceFormModel
	{
		public CartoPlaceFormModel(CartoPlaceInfo place) { Place = place; }

		public CartoPlaceInfo Place { get; set; }

		public bool IsPreview { get { return Place.GoogleKey == Place.Key; } }

		public HtmlString GetPlaceJson()
		{
			if (Place == null) return new HtmlString("{}");

			return new HtmlString(Place.ToJson());
		}

	}


	public class CartoPlaceSearch
	{
		public string Name { get; set; }

		public string Region { get; set; }

		public string Country { get; set; }

		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}


	// TODO: break into create and update models
	public class CartoLocationUpdateModel
	{
		public string ID { get; set; }
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