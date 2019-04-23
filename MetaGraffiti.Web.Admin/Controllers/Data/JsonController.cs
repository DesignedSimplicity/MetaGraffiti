using MetaGraffiti.Base.Modules.Geo.Info;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class JsonController : Controller
    {
		public ActionResult PlaceTypes()
		{
			return Json(ServiceConfig.CartoPlaceService.ListPlaceTypes(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult Countries()
		{
			return Json(GeoCountryInfo.All.Select(x => x.Name), JsonRequestBehavior.AllowGet);
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

		public ActionResult AllLegacyReport()
		{
			var all = new List<PlaceJson>();
			var places = ServiceConfig.CartoPlaceService.ListPlaces();
			foreach(var place in places.OrderBy(x => x.Country.CountryID).ThenBy(x => x.PlaceType).ThenBy(x => x.Name))
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

			return Json(all, JsonRequestBehavior.AllowGet);
		}


		public ActionResult Places()
		{
			return AllLegacyReport();
		}
	}
}