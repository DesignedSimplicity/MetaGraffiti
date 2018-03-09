using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GeoController : Controller
    {
		private GoogleLocationService _service = new GoogleLocationService(AutoConfig.GoogleMapsApiKey);

		public GeoViewModel InitModel()
		{
			var model = new GeoViewModel()
			{
				Timezones = GeoTimezoneInfo.All,
				Countries = GeoCountryInfo.All,
				Regions = GeoRegionInfo.All
			};

			model.VisitedCountries = GeoCountryInfo.All.Where(x => AutoConfig.VisitedCountries.Contains(x.ISO2)).OrderBy(x => x.Name).ToList();

			return model;
		}

		public ActionResult Index()
		{
			var model = InitModel();
			return View(model);
		}


		public ActionResult Timezones()
		{
			var model = InitModel();
			return View(model);
		}


		public ActionResult Countries(string id)
		{
			var model = InitModel();
			return View(model);
		}

		public ActionResult Country(int id)
		{
			var model = InitModel();

			model.SelectedCountry = GeoCountryInfo.ByID(id);

			return View(model);
		}


		public ActionResult Regions(string country = null)
		{
			var model = InitModel();
			return View(model);
		}


		// if search is lat/lng show reverse geocode
		// if search is text show 

		[HttpGet]
		public ActionResult Locations()
		{
			var model = InitModel();

			model.Search = new GeoSearchModel();
			model.Locations = new List<GeoLocationInfo>();

			return View(model);
		}

		[HttpPost]
		public ActionResult Locations(GeoSearchModel search)
		{
			var model = InitModel();

			model.Search = search;
			model.Locations = new List<GeoLocationInfo>();

			if (search.Latitude.HasValue && search.Longitude.HasValue)
			{
				var position = new GeoPosition(search.Latitude.Value, search.Longitude.Value);
				model.Locations = _service.LookupGeoLocations(position);
			}

			return View(model);
		}

		public ActionResult Location(string id)
		{
			var model = InitModel();

			// if id is guid load from local cache
			// if id contains _ load by google place id
			// if id is null show map with coordinate picker

			model.SelectedLocation = new GeoLocationInfo();

			return View(model);
		}
	}
}