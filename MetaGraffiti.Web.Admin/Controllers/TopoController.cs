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
	// topo/						GET Displays a calendar and a list of countries with their respective TopoTrailInfo data files
	// topo/trail/{id}				GET Dispalys a single TopoTrailInfo GPX data file on a map
	// topo/report/?				GET Displays a list of TopoTrailInfo GPX data files filtered by report criteria
	// topo/country/{id}/?region=	GET Displays all of the TopoTrailInfo GPX data files in a list and on a map for a given country with optional region filter
	// topo/refresh/				GET Resets the current TopoTrailInfo GPX data file cache and reloads from disk

	public class TopoController : Controller
    {
		// ==================================================
		// Initialization

		private TrailDataService _trailDataService;

		public TopoController()
		{
			_trailDataService = ServiceConfig.TrailDataService;
		}

		private TopoViewModel InitModel()
		{
			var model = new TopoViewModel();

			model.Trails = _trailDataService.ListTrails();
			model.Countries = _trailDataService.ListCountries().OrderBy(x => x.Name);

			return model;
		}


		// ==================================================
		// Actions

		/// <summary>
		/// Displays a calendar and a list of countries with their respective TopoTrailInfo data files
		/// </summary>
		public ActionResult Index()
		{
			var model = InitModel();

			return View(model);
		}

		/// <summary>
		/// Displays all of the TopoTrailInfo GPX data files in a list and on a map for a given country with optional region filter
		/// </summary>
		public ActionResult Country(string id, string region, string sort)
		{
			var model = InitModel();

			if (!String.IsNullOrWhiteSpace(region))
			{
				var r = GeoRegionInfo.Find(region);
				if (r == null)
					model.ErrorMessages.Add($"Invalid region: {region}");
				else
				{
					model.Region = r;
					model.Country = r.Country;
					model.Trails = _trailDataService.ListByRegion(r);
				}
			}
			else
			{
				var c = GeoCountryInfo.Find(id);
				if (c == null)
					model.ErrorMessages.Add($"Invalid country: {id}");
				else
				{
					model.Country = c;
					model.Trails = _trailDataService.ListByCountry(c);
				}
			}

			model.SelectedSort = (String.IsNullOrWhiteSpace(sort) ? "Newest" : sort);
			if (model.IsSortSelected("Region"))
				model.Trails = model.Trails.OrderBy(x => (x.Region == null ? "" : x.Region.RegionName)).ThenBy(x => x.Name).ThenByDescending(x => x.LocalDate).ToList();
			else if (model.IsSortSelected("Name"))
				model.Trails = model.Trails.OrderBy(x => x.Name).ThenByDescending(x => x.LocalDate).ToList();
			else if (model.IsSortSelected("Newest"))
				model.Trails = model.Trails.OrderByDescending(x => x.LocalDate).ThenBy(x => x.Name).ToList();
			else if (model.IsSortSelected("Oldest"))
				model.Trails = model.Trails.OrderBy(x => x.LocalDate).ThenBy(x => x.Name).ToList();

			return View(model);
		}

		public ActionResult Map(TopoReportModel filters)
		{
			var model = InitModel();

			return View();
		}

		public ActionResult List(TopoReportModel filters)
		{
			var model = InitModel();

			return View();
		}

		/// <summary>
		/// Dispalys the details of a single TopoTrailInfo data file
		/// </summary>
		public ActionResult Trail(string id)
		{
			var model = InitModel();

			var trail = _trailDataService.GetTrail(id);

			if (trail.Timezone.Key == "UTC") model.ErrorMessages.Add("Timezone missing! Default to UTC.");

			model.SelectedTrail = trail;

			return View(model);
		}

		/// <summary>
		/// Resets the current TopoTrailInfo GPX data file cache and reloads from disk
		/// </summary>
		public ActionResult Refresh()
		{
			ServiceConfig.ResetTrailDataService();

			return Redirect(TopoViewModel.GetTopoUrl());
		}
	}



	public class TopoReportModel
	{
		public string Country { get; set; }
		public string Region { get; set; }

		public int? Year { get; set; }
		public int? Month { get; set; }

		public string Tags { get; set; }
		public string Sort { get; set; }
	}
}