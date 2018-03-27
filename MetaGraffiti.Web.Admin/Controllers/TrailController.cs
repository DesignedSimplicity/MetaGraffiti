using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// TODO: rename to TopoController
namespace MetaGraffiti.Web.Admin.Controllers
{
	// trail/						GET Displays a calendar and a list of countries with their respective GPX imported track data
	// trail/report/?				GET Displays a list of GPX track files filtered by report criteria
	// trail/country/{id}/?region=	GET Displays all of the tracks in a list and on a map for a given country with optional region filter
	// trail/display/{id}			GET Dispalys a single GPX track data file on a map
	// trail/update/				POST Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
	// trail/refresh/				GET Resets the current GPX track data file cache and reloads from disk

	public class TrailController : Controller
    {
		private TrackExtractService _trackExtractService = new TrackExtractService();
		private TrailDataService _trailDataService;

		public TrailController()
		{
			_trailDataService = ServiceConfig.TrailDataService;
		}

		private TrailViewModel InitModel()
		{
			var model = new TrailViewModel();

			model.Trails = _trailDataService.ListTrails();
			model.FirstDate = model.Trails.Min(x => x.LocalDate);
			model.LastDate = model.Trails.Max(x => x.LocalDate);

			model.Countries = _trailDataService.ListCountries().OrderBy(x => x.Name);

			return model;
		}

		/// <summary>
		/// Displays a calendar and a list of countries with their respective GPX imported track data
		/// </summary>
		public ActionResult Index()
		{
			var model = InitModel();

			return View(model);
		}

		/// <summary>
		/// Displays a list of GPX track files filtered by report criteria
		/// </summary>
		public ActionResult Report(TrailReportRequest report)
		{
			var model = InitModel();

			model.SelectedYear = report.Year;
			model.SelectedMonth = report.Month;

			model.Trails = _trailDataService.Report(report);

			return View(model);
		}

		/// <summary>
		/// Displays all of the tracks in a list and on a map for a given country with optional region filter
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

			model.SelectedSort = sort;
			if (String.IsNullOrWhiteSpace(sort) || (sort.ToUpperInvariant() == "REGION"))
				model.Trails = model.Trails.OrderBy(x => (x.Region == null ? "" : x.Region.RegionName)).ThenBy(x => x.Name).ThenByDescending(x => x.LocalDate).ToList();
			else if (sort.ToUpperInvariant() == "NAME")
				model.Trails = model.Trails.OrderBy(x => x.Name).ThenByDescending(x => x.LocalDate).ToList();
			else if (sort.ToUpperInvariant() == "NEWEST")
				model.Trails = model.Trails.OrderByDescending(x => x.LocalDate).ThenBy(x => x.Name).ToList();
			else if (sort.ToUpperInvariant() == "OLDEST")
				model.Trails = model.Trails.OrderBy(x => x.LocalDate).ThenBy(x => x.Name).ToList();

			return View(model);

		}

		/// <summary>
		/// Dispalys a single GPX track data file on a map
		/// </summary>
		public ActionResult Display(string id)
		{
			var model = InitModel();

			var trail =_trailDataService.GetTrail(id);

			if (trail.Timezone.Key == "UTC") model.ErrorMessages.Add("Timezone missing! Default to UTC.");

			model.Trail = trail;

			return View(model);
		}

		/// <summary>
		/// Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string id)
		{
			var trail = _trailDataService.GetTrail(id);

			_trackExtractService.EditTrail(trail.Uri);

			return Redirect(TrackViewModel.GetManageUrl());
		}

		/// <summary>
		/// Creates an internal file from all of the tracks in the current edit session
		/// </summary>
		public ActionResult Import(bool overwrite = false)
		{
			var model = InitModel();

			// TODO: move this into TrailDataService
			var track = _trackExtractService.GetTrackGroup();
			if (String.IsNullOrWhiteSpace(track.Name)) model.ErrorMessages.Add("Name is missing.");
			if (track.Timezone == null) model.ErrorMessages.Add("Timezone is missing.");
			if (track.Country == null) model.ErrorMessages.Add("Country is missing.");
			if (track.Country != null && track.Country.HasRegions && track.Region == null) model.ErrorMessages.Add("Region is missing.");

			// show error messages if necessary
			if (model.HasError) return View(model);

			// check folders are initialized
			var folder = Path.Combine(AutoConfig.TrailSourceUri, track.Country.Name);
			if (!Directory.Exists(AutoConfig.TrailSourceUri)) throw new Exception($"TrackRoot not initalized: {AutoConfig.TrailSourceUri}");
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			// check existing filename and if overwrite
			var filename = $"{String.Format("{0:yyyyMMdd}", track.Timestamp)} {track.Name}";
			var uri = Path.Combine(folder, filename + ".gpx");

			// show overwrite confirmation if necessary
			if (System.IO.File.Exists(uri) && !overwrite)
			{
				model.ConfirmMessage = uri;
				return View(model);
			}

			// create internal file
			_trackExtractService.WriteTrackFile(uri);

			// reset track extract cache
			_trackExtractService.ResetSession();

			// reload trails data before redirect
			ServiceConfig.ResetTrailDataService(); //_trailDataService.ResetCache();

			// redirect to new trail page
			return Redirect(TrailViewModel.GetDisplayUrl(filename));
		}

		/// <summary>
		/// Resets the current GPX track data file cache and reloads from disk
		/// </summary>
		public ActionResult Refresh()
		{
			ServiceConfig.ResetTrailDataService();

			return Redirect(TrailViewModel.GetTrailUrl());
		}
	}
}