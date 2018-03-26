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
		// ==================================================
		// Initialization

		private CartoPlaceService _cartoPlaceService;

		public CartoController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		private CartoViewModel InitModel()
		{
			var model = new CartoViewModel();

			model.LastSaved = _cartoPlaceService.LastSaved;
			model.HasChanges = _cartoPlaceService.HasChanges;

			model.Places = _cartoPlaceService.ListPlaces();

			model.Countries = _cartoPlaceService.ListCountries();
			model.PlaceTypes = _cartoPlaceService.ListPlaceTypes();

			return model;
		}


		// ==================================================
		// Actions

		public ActionResult Index()
        {
			var model = InitModel();

			return View(model);
		}


		// TODO: make type, country and region columns links
		public ActionResult Places()
		{
			var model = InitModel();

			return View(model);
		}

		// TODO: add place type filter navigation
		public ActionResult Country(string id, string placeType = "")
		{
			var model = InitModel();

			model.SelectedCountry = GeoCountryInfo.Find(id);
			model.Places = _cartoPlaceService.ReportPlaces(new CartoPlaceReportRequest() { Country = model.SelectedCountry.ISO2, PlaceType = placeType });			

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

			model.SelectedPlace = _cartoPlaceService.GetPlace(key);

			return View("Place", model);
		}

		public ActionResult Update(CartoPlaceUpdateRequest request)
		{
			var model = InitModel();

			model.SelectedPlace = _cartoPlaceService.UpdatePlace(request);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			model.HasChanges = true;

			return View("Place", model);
		}

		// TODO: make this a post only or get with confirmation
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