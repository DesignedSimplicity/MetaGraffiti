using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class DemoController : Controller
    {
		// ==================================================
		// Initialization

		private CartoPlaceService _cartoPlaceService;
		private TripSheetService _tripSheetService;

		public DemoController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_tripSheetService = ServiceConfig.TripSheetService;
		}

		private DemoViewModel InitModel()
		{
			var model = new DemoViewModel();

			model.Places = _cartoPlaceService.ListPlaces();

			model.Countries = _cartoPlaceService.ListCountries();
			model.PlaceTypes = _cartoPlaceService.ListPlaceTypes();

			return model;
		}

		// ==================================================
		// Actions

		/// <summary>
		/// Carto landing page
		/// </summary>
		public ActionResult Index(CartoPlaceReportRequest report)
		{
			var model = InitModel();

			var political = new GeoPolitical(report);
			model.SelectedCountry = political.Country;
			model.SelectedPlaceType = report.PlaceType;

			model.ReportFilters = report;
			model.ReportPlaces = _cartoPlaceService.ReportPlaces(report)
				.OrderBy(x => x.Country.Continent)
				.ThenBy(x => x.Country.Name)
				.ThenBy(x => x.Region?.RegionName ?? "")
				.ThenBy(x => x.PlaceType.ToString())
				.ThenBy(x => x.Name)
				.ToList();

			return View(model);
		}

		public ActionResult Globe()
		{
			var model = InitModel();
			return View(model);
		}

		public ActionResult Country(string id)
		{
			var model = InitModel();
			model.SelectedCountry = model.Countries.Where(x => String.Compare(x.ISO2, id, true) == 0).FirstOrDefault();
			return View(model);
		}
	}
}