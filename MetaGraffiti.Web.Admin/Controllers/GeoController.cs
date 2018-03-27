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
		// ==================================================
		// Initialization

		private GeoLookupService _geoLookupService;
		private CartoPlaceService _cartoPlaceService;

		public GeoController()
		{
			_geoLookupService = ServiceConfig.GeoLookupService;
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		private GeoViewModel InitModel()
		{
			var model = new GeoViewModel()
			{
				Timezones = GeoTimezoneInfo.All,
				Countries = GeoCountryInfo.All,
				Regions = GeoRegionInfo.All
			};

			model.Visited = ServiceConfig.TripSheetService.ListCountries(); //_cartoPlaceService.ListCountries();

			return model;
		}


		// ==================================================
		// Actions

		/// <summary>
		/// Displays all countries with bounds on a map
		/// </summary>
		public ActionResult Index()
		{
			var model = InitModel();
			return View(model);
		}

		/// <summary>
		/// Displays table of all timezone data
		/// </summary>
		public ActionResult Timezones()
		{
			var model = InitModel();
			return View(model);
		}

		/// <summary>
		/// Displays table of all country data
		/// </summary>
		public ActionResult Countries()
		{
			var model = InitModel();
			return View(model);
		}

		/// <summary>
		/// Displays table of all region data grouped by country
		/// </summary>
		public ActionResult Regions()
		{
			var model = InitModel();
			return View(model);
		}

		/// <summary>
		/// Displays all of a country's regions and their bounds on a map
		/// </summary>
		public ActionResult Country(string id)
		{
			var model = InitModel();

			model.SelectedCountry = GeoCountryInfo.Find(id);

			return View(model);
		}
	}
}