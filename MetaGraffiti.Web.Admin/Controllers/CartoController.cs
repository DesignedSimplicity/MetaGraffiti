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

			return model;
		}

		public ActionResult Index()
        {
			var model = InitModel();

			return View(model);
		}


		/// <summary>
		/// Lists all cached locations in system
		/// </summary>
		/// <returns></returns>
		public ActionResult Places()
		{
			var model = InitModel();

			model.Places = _cartoPlaceService.ListPlaces();

			return View(model);
		}


		/// <summary>
		/// Displays an existing cached place for edit
		/// </summary>
		[HttpGet]
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