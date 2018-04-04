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

		private TopoTrailService _trailDataService;

		public TopoController()
		{
			_trailDataService = ServiceConfig.TopoTrailService;
		}

		private TopoViewModel InitModel()
		{
			var model = new TopoViewModel();

			model.Countries = _trailDataService.ListCountries().OrderBy(x => x.Name);
			model.Trails = _trailDataService.ListTrails();

			model.FirstDate = model.Trails.Min(x => x.StartLocal);
			model.LastDate = model.Trails.Max(x => x.StartLocal);

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

			model.SelectedSort = sort;

			if (!String.IsNullOrWhiteSpace(region))
			{
				var r = GeoRegionInfo.Find(region);
				if (r == null)
					model.ErrorMessages.Add($"Invalid region: {region}");
				else
				{
					model.SelectedRegion = r;
					model.SelectedCountry = r.Country;
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
					model.SelectedCountry = c;
					model.Trails = _trailDataService.ListByCountry(c);
				}
			}

			return View(model);
		}

		/// <summary>
		/// Displays a list of TopoTrailInfo GPX data files filtered by report criteria
		/// </summary>
		public ActionResult Report(TopoTrailReportRequest report)
		{
			var model = InitModel();

			model.SelectedSort = "Oldest";
			model.SelectedYear = report.Year;
			model.SelectedMonth = report.Month;

			model.Trails = _trailDataService.Report(report);

			return View(model);
		}

		/// <summary>
		/// Resets the current TopoTrailInfo GPX data file cache and reloads from disk
		/// </summary>
		public ActionResult Refresh()
		{
			ServiceConfig.ResetTopoTrail();

			return Redirect(TopoViewModel.GetTopoUrl());
		}
	}
}