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

		public GeoController()
		{
			_geoLookupService = new GeoLookupService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));
		}

		public GeoViewModel InitModel()
		{
			var model = new GeoViewModel()
			{
				Timezones = GeoTimezoneInfo.All,
				Countries = GeoCountryInfo.All,
				Regions = GeoRegionInfo.All
			};

			model.VisitedCountries = GeoCountryInfo.All.Where(x => AutoConfig.VisitedCountries.Contains(x.ISO2)).OrderBy(x => x.Name).ToList();

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

		public ActionResult Countries(string id)
		{
			var model = InitModel();
			return View(model);
		}

		public ActionResult Country(int id)
		{
			var model = InitModel();

			model.SelectedCountry = GeoCountryInfo.ByID(id);

			return View(model);
		}

		public ActionResult Regions(string country = null)
		{
			var model = InitModel();
			return View(model);
		}
	}
}