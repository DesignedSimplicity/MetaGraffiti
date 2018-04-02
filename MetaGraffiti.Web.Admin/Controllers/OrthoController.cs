using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class OrthoController : Controller
    {
		// ==================================================
		// Initialization

		private TripSheetService _tripSheetService;
		private CartoPlaceService _cartoPlaceService;

		public OrthoController()
		{
			_tripSheetService = ServiceConfig.TripSheetService;
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		public ActionResult Index()
		{
			var model = new OrthoViewModel();

			model.Years = _tripSheetService.ListYears();

			return View(model);
		}



		public ActionResult Places(int? year, string country = "")
		{
			var model = new OrthoPlacesViewModel();

			model.Years = _tripSheetService.ListYears();
			model.SelectedYear = year;

			model.Countries = _tripSheetService.ListCountries();
			model.SelectedCountry = GeoCountryInfo.Find(country);

			var places = new List<OrthoPlaceImportModel>();
			var import = _tripSheetService.ListPlaces(year, country);
			foreach (var data in import)
			{
				var place = new OrthoPlaceImportModel();
				place.Data = data;
				places.Add(place);

				// find existing place by region
				var r = GeoRegionInfo.Find(data.Region);
				if (r != null) place.Place = _cartoPlaceService.FindPlace(r, data.Name, true);

				// find existing place by country
				if (place.Place == null)
				{
					var c = GeoCountryInfo.Find(data.Country);
					if (c != null) place.Place = _cartoPlaceService.FindPlace(c, data.Name, true);
				}
			}
			model.Places = places;

			return View(model);
		}

		public ActionResult Sheets(string id)
		{
			var model = new OrthoSheetsViewModel();

			model.Sheets = _tripSheetService.ListSheets();
			model.SelectedSheet = id;

			return View(model);
		}

		public ActionResult Reset()
		{
			ServiceConfig.ResetTripSheetService();

			return new RedirectResult(OrthoViewModel.GetOrthoUrl());
		}
	}
}