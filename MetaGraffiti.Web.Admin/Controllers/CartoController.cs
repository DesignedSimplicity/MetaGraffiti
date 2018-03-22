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
		private CartoLocationService _cartoLocationService = new CartoLocationService();
		private GeoLookupService _geoLookupService = new GeoLookupService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));

		public CartoController()
		{
			_cartoPlaceService = new CartoPlaceService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));
			_cartoPlaceService.LoadPlaces(AutoConfig.CartoDataUri);

			_cartoLocationService.Init(AutoConfig.CartoDataUri);
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


		public ActionResult Places()
		{
			var model = InitModel();

			model.Places = _cartoPlaceService.ListPlaces();

			return View(model);
		}


		public ActionResult Locations()
		{
			var model = InitModel();

			model.Locations = _cartoLocationService.ListLocations();

			return View(model);
		}




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

		[HttpGet]
		public ActionResult Preview(string googlePlaceID)
		{
			var model = InitModel();
			
			var place = _cartoPlaceService.FindByGooglePlaceID(googlePlaceID);
			if (place != null) return new RedirectResult(CartoViewModel.GetPlaceEditUrl(place.Key));

			model.Place = _cartoPlaceService.GetLocation(googlePlaceID);

			return View("Place", model);
		}

		[HttpGet]
		public ActionResult Place(string id)
		{
			var key = id.ToUpperInvariant();

			var model = InitModel();

			model.Place = _cartoPlaceService.GetPlace(key);

			return View("Place", model);
		}


		[HttpPost]
		public ActionResult Location(CartoLocationUpdateModel update)
		{
			var model = InitModel();

			var id = update.ID.ToUpperInvariant();

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

			return new RedirectResult($"/carto/location/{id}");
		}

		[HttpGet]
		public ActionResult Remove(string id)
		{
			_cartoLocationService.RemoveLocation(id);

			return new RedirectResult($"/carto/locations/");
		}
	}
}