using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class CartoController : Controller
    {
		private CartoPlaceService _cartoPlaceService;

		public CartoController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		public CartoViewModel InitModel()
		{
			var model = new CartoViewModel();

			model.Places = _cartoPlaceService.ListPlaces();

			model.Countries = _cartoPlaceService.ListCountries();
			model.PlaceTypes = _cartoPlaceService.ListPlaceTypes();

			return model;
		}

		public ActionResult Index()
        {
			var model = InitModel();

			return View(model);
		}


		public ActionResult Places()
		{
			var model = InitModel();

			return View(model);
		}

		public ActionResult Country(string id, string placeType = "")
		{
			var model = InitModel();

			model.Country = GeoCountryInfo.Find(id);
			model.Places = _cartoPlaceService.ReportPlaces(new CartoPlaceReportRequest() { Country = model.Country.ISO2, PlaceType = placeType });			

			return View(model);
		}


		public ActionResult Report(CartoPlaceReportRequest report)
		{
			var model = InitModel();

			model.ReportPlaces = _cartoPlaceService.ReportPlaces(report);

			return View(model);
		}


		public ActionResult Place(string id)
		{
			var key = id.ToUpperInvariant();

			var model = InitModel();

			model.Place = _cartoPlaceService.GetPlace(key);

			return View("Place", model);
		}

		public ActionResult Update(CartoPlaceUpdateRequest request)
		{
			var model = InitModel();

			model.Place = _cartoPlaceService.UpdatePlace(request);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View("Place", model);
		}

		public ActionResult Delete(string id)
		{
			var key = id.ToUpperInvariant();

			_cartoPlaceService.DeletePlace(key);

			return new RedirectResult(CartoViewModel.GetPlacesUrl());
		}



		public ActionResult Persist()
		{
			_cartoPlaceService.Save();
			_cartoPlaceService.Reload();

			return new RedirectResult(CartoViewModel.GetCartoUrl());
		}

		public ActionResult Reload()
		{
			_cartoPlaceService.Reload();

			return new RedirectResult(CartoViewModel.GetCartoUrl());
		}
	}
}