using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
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
    public class PlaceController : Controller
    {
		// ==================================================
		// Initialization

		private CartoPlaceService _cartoPlaceService;
		private GeoLookupService _geoLookupService;

		public PlaceController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_geoLookupService = ServiceConfig.GeoLookupService;
		}

		private PlaceViewModel InitModel()
		{
			var model = new PlaceViewModel();

			return model;
		}


		// ==================================================
		// Actions

		public ActionResult Index()
        {
			var model = InitModel();

			return View(model);
        }

		/// <summary>
		/// Searches google for places by name
		/// </summary>
		public ActionResult Search(PlaceSearchModel search)
		{
			var model = InitModel();

			model.SearchCriteria = (search == null ? new PlaceSearchModel() : search);
			model.SearchResults = new List<CartoPlaceInfo>();

			var text = search.Name + " " + search.Country;
			if (!String.IsNullOrWhiteSpace(text))
			{
				model.SearchResults = _cartoPlaceService.LookupLocations(text.Trim());
			}
			else if (search.Latitude.HasValue && search.Longitude.HasValue)
			{
				var position = new GeoPosition(search.Latitude.Value, search.Longitude.Value);
				model.SearchResults = _cartoPlaceService.LookupLocations(position);
			}

			return View(model);
		}

		/// <summary>
		/// Displays a google location for import
		/// </summary>
		[HttpGet]
		public ActionResult Preview(string id, string search = "")
		{
			var model = InitModel();

			model.SearchCriteria = new PlaceSearchModel();
			model.SearchCriteria.Name = search;

			var place = _cartoPlaceService.FindByGooglePlaceID(id);
			if (place != null)
			{
				model.SelectedPlace = place;
				model.ConfirmMessage = "Place already exists!";
			}
			else
			{
				// TODO: backfill country and region if they are not set
				model.SelectedPlace = _cartoPlaceService.LookupByPlaceID(id);
			}

			return View("Create", model);
		}

		[HttpPost]
		public ActionResult Create(CartoPlaceCreateRequest request)
		{
			// TODO: do validation
			var place = _cartoPlaceService.CreatePlace(request);
			if (place != null) return new RedirectResult(CartoViewModel.GetEditUrl(place.Key));

			// show validation error messages
			var model = InitModel();

			return View("Create", model);
		}
	}
}