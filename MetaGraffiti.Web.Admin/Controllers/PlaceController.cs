using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class PlaceController : Controller
    {
		// ==================================================
		// Initialization

		private CartoPlaceService _cartoPlaceService;

		public PlaceController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		private PlaceViewModel InitModel()
		{
			var model = new PlaceViewModel();

			model.HasChanges = _cartoPlaceService.HasChanges;

			return model;
		}


		// ==================================================
		// Actions

		[HttpGet]
		public ActionResult Display(string id)
		{
			var model = InitModel();

			var place = _cartoPlaceService.GetPlace(id);

			model.SelectedPlace = place;
			model.SelectedCountry = place.Country;

			model.NearbyPlaces = _cartoPlaceService.ListPlacesContainingBounds(place.Bounds);
			model.ContainedPlaces = _cartoPlaceService.ListPlacesContainedInBounds(place.Bounds);

			return View(model);
		}


		/// <summary>
		/// Searches google for places by name
		/// </summary>
		public ActionResult Search(PlaceSearchModel search)
		{
			var model = InitModel();

			search = search ?? new PlaceSearchModel();
			model.SearchCriteria = search;
			model.SearchResults = new List<CartoPlaceInfo>();

			if (!String.IsNullOrWhiteSpace(search.Location))
			{
				model.SearchResults = _cartoPlaceService.LookupLocations(search.Location.Trim());
			}
			else if (!String.IsNullOrWhiteSpace(search.Name))
			{
				model.SearchResults = _cartoPlaceService.LookupPlaces(search.Name, search.Country);
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
			model.Edit = new CartoPlaceFormModel(model.SelectedPlace);

			return View("Create", model);
		}

		// TODO: CARTO: RC2: allow create place from scratch
		[HttpGet]
		public ActionResult Create(string search = "")
		{
			return null;
		}

		[HttpPost]
		public ActionResult Create(CartoPlaceCreateRequest request)
		{
			// TODO: do validation
			var response = _cartoPlaceService.CreatePlace(request);

			if (response.OK)
			{
				return Redirect(PlaceViewModel.GetDisplayUrl(response.Data.Key));
			}
			else
			{
				// show validation error messages
				var model = InitModel();

				model.SelectedPlace = response.Data;
				model.Edit = new CartoPlaceFormModel(response.Data, request);
				model.AddValidationErrors(response.ValidationErrors);

				return View("Create", model);
			}
		}

		/// <summary>
		/// Displays place edit form
		/// </summary>
		[HttpGet]
		public ActionResult Update(string id)
		{
			var model = InitModel();

			var place = _cartoPlaceService.GetPlace(id);
			model.Edit = new CartoPlaceFormModel(place);
			model.SelectedPlace = place;
			model.SelectedCountry = place.Country;

			return View(model);
		}

		/// <summary>
		/// Processes place edit request
		/// </summary>
		[HttpPost]
		public ActionResult Update(CartoPlaceUpdateRequest request)
		{
			var model = InitModel();

			var source = _cartoPlaceService.GetPlace(request.Key);
			model.SelectedCountry = source.Country;

			var response = _cartoPlaceService.UpdatePlace(request);
			var place = response.Data;
			model.SelectedPlace = place;

			if (response.OK)
			{
				model.HasChanges = true;
				model.Edit = new CartoPlaceFormModel(place);
				model.ConfirmMessage = $"Place {request.PlaceKey} updated at {DateTime.Now}";
			}
			else
			{
				model.Edit = new CartoPlaceFormModel(place, request);
				model.AddValidationErrors(response.ValidationErrors);
			}

			return View(model);
		}

		/// <summary>
		/// Removes a place from the cache
		/// </summary>
		public ActionResult Delete(string id)
		{
			var key = id.ToUpperInvariant();

			_cartoPlaceService.DeletePlace(key);

			return new RedirectResult(CartoViewModel.GetPlacesUrl());
		}
	}
}