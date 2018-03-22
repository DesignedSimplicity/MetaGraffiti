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
			_cartoPlaceService = new CartoPlaceService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));
			_cartoPlaceService.LoadPlaces(AutoConfig.CartoDataUri);
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
		/// Searches google for places by name
		/// </summary>
		public ActionResult Search(CartoPlaceSearch search)
		{
			var model = InitModel();

			model.Search = (search == null ? new CartoPlaceSearch() : search);
			model.Places = new List<CartoPlaceInfo>();

			if (!String.IsNullOrWhiteSpace(search.Name))
			{
				model.Places = _cartoPlaceService.LookupLocations(search.Name);
			}
			else if (search.Latitude.HasValue && search.Longitude.HasValue)
			{
				var position = new GeoPosition(search.Latitude.Value, search.Longitude.Value);
				model.Places = _cartoPlaceService.LookupLocations(position);
			}

			return View(model);
		}

		/// <summary>
		/// Displays a google location for import
		/// </summary>
		[HttpGet]
		public ActionResult Preview(string googlePlaceID)
		{
			var model = InitModel();
			
			var place = _cartoPlaceService.FindByGooglePlaceID(googlePlaceID);
			if (place != null) return new RedirectResult(CartoViewModel.GetEditUrl(place.Key));

			model.Place = _cartoPlaceService.LookupByPlaceID(googlePlaceID);

			return View("Place", model);
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


		public ActionResult Delete(string id)
		{
			var key = id.ToUpperInvariant();

			_cartoPlaceService.DeletePlace(key);

			return new RedirectResult(CartoViewModel.GetPlacesUrl());
		}


		public ActionResult Reload()
		{
			_cartoPlaceService.ResetCache();

			return new RedirectResult(CartoViewModel.GetCartoUrl());
		}





		[HttpPost]
		public ActionResult Location(CartoLocationUpdateModel update)
		{
			// TODO: migrate to _cartoPlaceService
			var model = InitModel();

			var id = update.ID.ToUpperInvariant();
			/*

			var location = _cartoLocationService.GetLocation(id);

			location.Name = update.Name;
			location.LocalName = update.LocalName;
			location.DisplayAs = update.DisplayAs;

			location.Description = update.Description;

			location.PlaceType = update.PlaceType;

			location.Address = update.Address;
			location.Locality = update.Locality;
			location.Postcode = update.Postcode;

			location.Region = GeoRegionInfo.Find(update.Region);
			//location.Timezone = GeoTimezoneInfo.Find
			*/
			return new RedirectResult($"/carto/location/{id}");
		}
	}
}