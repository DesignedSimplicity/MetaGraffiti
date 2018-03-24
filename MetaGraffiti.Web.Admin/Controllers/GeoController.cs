using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GeoController : Controller
    {
		private GeoLookupService _geoLookupService;
		private CartoPlaceService _cartoPlaceService;

		public GeoController()
		{
			_geoLookupService = ServiceConfig.GeoLookupService;
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		public GeoViewModel InitModel()
		{
			var model = new GeoViewModel()
			{
				Timezones = GeoTimezoneInfo.All,
				Countries = GeoCountryInfo.All,
				Regions = GeoRegionInfo.All
			};
			
			model.Visited = _cartoPlaceService.ListCountries();

			return model;
		}

		public ActionResult Index()
		{
			var model = InitModel();
			return View(model);
		}

		public ActionResult Timezones()
		{
			var model = InitModel();
			return View(model);
		}

		public ActionResult Countries()
		{
			var model = InitModel();
			return View(model);
		}

		public ActionResult Country(string id)
		{
			var model = InitModel();

			model.SelectedCountry = GeoCountryInfo.Find(id);

			return View(model);
		}

		public ActionResult Regions(string country = null)
		{
			var model = InitModel();
			return View(model);
		}
	}
}