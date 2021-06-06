using MetaGraffiti.Base.Modules.Geo.Info;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class JsonController : Controller
    {
		public ActionResult PlaceTypes()
		{
			return Json(ServiceConfig.CartoPlaceService.ListPlaceTypes()); //, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Countries()
		{
			return Json(GeoCountryInfo.All.Select(x => x.Name)); //,  JsonRequestBehavior.AllowGet);
		}


		public class PlaceJson
		{
			public string key { get; set; }
			public string type { get; set; }
			public int country { get; set; }
			public string name { get; set; }
			public double lat { get; set; }
			public double lng { get; set; }
		}

		public class CountryJson
		{
			public int id { get; set; }
			public int continent { get; set; }
			public string key { get; set; }
			public string name { get; set; }
			public double lat { get; set; }
			public double lng { get; set; }

			public double latMin { get; set; }
			public double lngMin { get; set; }
			public double latMax { get; set; }
			public double lngMax { get; set; }
		}

		public ActionResult AllLegacyReport()
		{
			var all = new List<PlaceJson>();
			var places = ServiceConfig.CartoPlaceService.ListPlaces();
			//foreach(var place in places.OrderBy(x => x.Country.CountryID).ThenBy(x => x.PlaceType).ThenBy(x => x.Name))
			//foreach (var place in places.OrderBy(x => x.Country.CountryID).ThenByDescending(x => x.Bounds.Area).ThenByDescending(x => (-x.Center.Longitude + x.Center.Latitude)))
			foreach (var place in places.OrderBy(x => x.Country.CountryID).ThenByDescending(x => x.Center.Latitude).ThenBy(x => x.Center.Longitude))
			{
				var one = new PlaceJson();
				one.key = place.Key;
				one.type = place.PlaceType.ToString();
				one.country = place.Country.CountryID;
				one.name = place.Name;
				one.lat = place.Center.Latitude;
				one.lng = place.Center.Longitude;
				all.Add(one);
			}

			return Json(all); //,  JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListCountries()
		{
			var all = new List<CountryJson>();
			var places = GeoCountryInfo.All;
			foreach (var place in places.OrderBy(x => x.CountryID))
			{
				var one = new CountryJson();
				one.key = place.ISO2;
				one.id = place.CountryID;
				one.continent = Convert.ToInt32(place.Continent);
				one.name = place.Name;
				one.lat = place.Center.Latitude;
				one.lng = place.Center.Longitude;

				one.latMin = place.Bounds.SouthEast.Latitude;
				one.latMax = place.Bounds.NorthWest.Latitude;

				one.lngMin = place.Bounds.NorthWest.Longitude;
				one.lngMax = place.Bounds.SouthEast.Longitude;

				all.Add(one);
			}

			return Json(all); //, JsonRequestBehavior.AllowGet);
		}
	}
}