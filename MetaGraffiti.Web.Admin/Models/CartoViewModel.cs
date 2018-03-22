﻿using MetaGraffiti.Base.Modules.Carto.Info;
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
		public CartoPlaceInfo Place { get; set; }

		public List<CartoPlaceInfo> Places { get; set; }

		public CartoPlaceSearch Search { get; set; } = new CartoPlaceSearch();


		public HtmlString GetPlacesJson()
		{
			var json = "";
			foreach (var place in Places)
			{
				json += place.ToJson() + ",";
			}
			return new HtmlString("[" + json + "]");
		}

		public HtmlString GetJson(CartoPlaceInfo place)
		{
			if (place == null) return new HtmlString("{}");

			return new HtmlString(place.ToJson());
		}


		public static string GetCartoUrl() { return $"/carto/"; }
		public static string GetReloadUrl() { return $"/carto/reload/"; }
		public static string GetSearchUrl(string name, string country = "") { return $"/carto/search/?name={name} {country}"; }
		public static string GetPreviewUrl(string googlePlaceID) { return $"/carto/preview/?googlePlaceID={googlePlaceID}"; }
		public static string GetPlacesUrl() { return $"/carto/places/"; }
		public static string GetEditUrl(string key) { return $"/carto/place/{key}"; }
		public static string GetDeleteUrl(string key) { return $"/carto/delete/{key}"; }
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