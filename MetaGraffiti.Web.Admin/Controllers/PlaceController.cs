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
		private CartoPlaceService _cartoPlaceService;
		private TripSheetService _tripSheetService;
		private GeoLookupService _geoLookupService;

		public PlaceController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_tripSheetService = ServiceConfig.TripSheetService;
		}

		public PlaceViewModel InitModel()
		{
			var model = new PlaceViewModel();

			model.Countries = _tripSheetService.ListCountries();
			model.Years = _tripSheetService.ListYears();

			return model;
		}

		public ActionResult Index()
        {
			var model = InitModel();

			// client side search: https://developers.google.com/maps/documentation/javascript/examples/places-searchbox

			return View(model);
        }

		public ActionResult Report(int? year, string country)
		{
			var model = InitModel();

			model.SelectedYear = year;
			model.SelectedCountry = GeoCountryInfo.Find(country);

			var places = new List<PlaceModel>();
			var import = _tripSheetService.ListPlaces(year, country);
			foreach(var data in import)
			{
				var place = new PlaceModel();
				place.Data = data;
				places.Add(place);

				var r = GeoRegionInfo.Find(data.Region);
				if (r != null) place.Place = _cartoPlaceService.FindPlace(r, data.Name, true);

				if (place.Place == null)
				{
					var c = GeoCountryInfo.Find(data.Country);
					if (c != null) place.Place = _cartoPlaceService.FindPlace(c, data.Name, true);
				}				
			}
			model.FilteredPlaces = places;
			model.ImportPlaces = import;

			return View(model);
		}

		/// <summary>
		/// Searches google for places by name
		/// </summary>
		public ActionResult Search(CartoPlaceSearch search)
		{
			var model = InitModel();

			model.SearchCriteria = (search == null ? new CartoPlaceSearch() : search);
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
		public ActionResult Preview(string id)
		{
			var model = InitModel();

			var place = _cartoPlaceService.FindByGooglePlaceID(id);
			if (place != null)
			{
				model.Place = place;
				model.ConfirmMessage = "Place already exists!";
			}
			else
			{
				model.Place = _cartoPlaceService.LookupByPlaceID(id);
				// TODO: backfill country and region if they are not set
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult Create(CartoPlaceCreateRequest request)
		{
			// TODO: do validation
			var place = _cartoPlaceService.CreatePlace(request);
			if (place != null) return new RedirectResult(CartoViewModel.GetEditUrl(place.Key));

			// show validation error messages
			var model = InitModel();

			return View(model);
		}
	}
}