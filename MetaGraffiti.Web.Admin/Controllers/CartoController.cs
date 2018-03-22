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
		private CartoLocationService _cartoLocationService = new CartoLocationService();
		private GeoLookupService _geoLookupService = new GeoLookupService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));

		public CartoController()
		{
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


		public ActionResult Locations()
		{
			var model = InitModel();

			model.Locations = _cartoLocationService.ListLocations();

			return View(model);
		}




		public ActionResult Search(CartoLocationSearchModel search)
		{
			var model = InitModel();

			model.Search = (search == null ? new CartoLocationSearchModel() : search);
			model.Locations = new List<GeoLocationInfo>();

			if (!String.IsNullOrWhiteSpace(search.Name))
			{
				model.Locations = _geoLookupService.LookupLocations(search.Name);
			}
			else if (search.Latitude.HasValue && search.Longitude.HasValue)
			{
				var position = new GeoPosition(search.Latitude.Value, search.Longitude.Value);
				model.Locations = _geoLookupService.LookupLocations(position);
			}

			return View(model);
		}

		[HttpGet]
		public ActionResult Location(string id = "", string key = "")
		{
			var model = InitModel();

			if (!String.IsNullOrWhiteSpace(key))
			{
				var location = _geoLookupService.LoadLocation(key);
				id = location.ID.ToUpperInvariant();
				_cartoLocationService.CacheLocation(location);
				return new RedirectResult($"/carto/location/{id}");
			}

			if (!String.IsNullOrWhiteSpace(id))
				model.SelectedLocation = _cartoLocationService.GetLocation(id);

			return View(model);
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