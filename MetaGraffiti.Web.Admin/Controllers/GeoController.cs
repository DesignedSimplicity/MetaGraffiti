using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GeoController : Controller
    {
		private GeoLookupService _service = new GeoLookupService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));
		private static Dictionary<string, GeoLocationInfo> _cache = new Dictionary<string, GeoLocationInfo>();

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


		public ActionResult Search(GeoLocationSearchModel search)
		{
			var model = InitModel();

			model.Search = (search == null ? new GeoLocationSearchModel() : search);
			model.Locations = new List<GeoLocationInfo>();

			if (!String.IsNullOrWhiteSpace(search.Name))
			{
				model.Locations = _service.LookupLocations(search.Name);
			}
			else if (search.Latitude.HasValue && search.Longitude.HasValue)
			{
				var position = new GeoPosition(search.Latitude.Value, search.Longitude.Value);
				model.Locations = _service.LookupLocations(position);
			}

			return View(model);
		}

		public ActionResult Locations()
		{
			var model = InitModel();

			model.Locations = _cache.Values.ToList();

			return View(model);
		}

		[HttpGet]
		public ActionResult Location(string id = "", string key = "")
		{
			var model = InitModel();

			if (!String.IsNullOrWhiteSpace(key))
			{
				var location = _service.LoadLocation(key);
				id = location.ID.ToUpperInvariant();
				_cache.Add(id, location);
				return new RedirectResult($"/geo/location/{id}");
			}

			if (!String.IsNullOrWhiteSpace(id))
				model.SelectedLocation = _cache[id];

			return View(model);
		}

		[HttpPost]
		public ActionResult Location(GeoLocationUpdateModel update)
		{
			var model = InitModel();

			var id = update.PlaceKey.ToUpperInvariant();

			var location = _cache[id];

			location.Name = update.Name;
			location.LocalName = update.LocalName;
			location.DisplayAs = update.DisplayAs;

			location.Description = update.Description;

			location.PlaceType = update.PlaceType;

			location.Address = update.Address;
			location.City = update.City;
			location.Postcode = update.Postcode;

			location.Region = GeoRegionInfo.Find(update.Region);
			//location.Timezone = GeoTimezoneInfo.Find

			return new RedirectResult($"/geo/location/{id}");
		}

		[HttpGet]
		public ActionResult Remove(string id)
		{
			_cache.Remove(id.ToUpperInvariant());

			return new RedirectResult($"/geo/locations/");
		}
	}
}