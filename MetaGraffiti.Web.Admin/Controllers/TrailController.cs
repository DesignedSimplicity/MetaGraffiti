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
	// trail/						GET Displays a calendar and a list of countries with their respective GPX imported track data
	// trail/report/?				GET Displays a list of GPX track files filtered by report criteria
	// trail/country/{id}/?region=	GET Displays all of the tracks in a list and on a map for a given country with optional region filter
	// trail/display/{id}			GET Dispalys a single GPX track data file on a map
	// trail/update/				POST Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
	// trail/refresh/				GET Resets the current GPX track data file cache and reloads from disk

	public class TrailController : Controller
    {
		private TrailDataService _service = new TrailDataService();

		public TrailController()
		{
			_service.Init(AutoConfig.TrackRootUri);
		}

		private TrailViewModel InitModel()
		{
			var model = new TrailViewModel();

			model.Countries = _service.ListCountries().OrderBy(x => x.Name);

			return model;
		}

		/// <summary>
		/// Displays a calendar and a list of countries with their respective GPX imported track data
		/// </summary>
		public ActionResult Index()
		{
			var model = InitModel();

			model.Trails = _service.ListAll();
			model.FirstDate = model.Trails.Min(x => x.LocalDate);
			model.LastDate = model.Trails.Max(x => x.LocalDate);

			return View(model);
		}

		/// <summary>
		/// Displays a list of GPX track files filtered by report criteria
		/// </summary>
		public ActionResult Report(TrailReportRequest report)
		{
			var model = InitModel();

			model.Trails = _service.Report(report);

			return View(model);
		}

		/// <summary>
		/// Displays all of the tracks in a list and on a map for a given country with optional region filter
		/// </summary>
		public ActionResult Country(string id, string region)
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
					model.Trails = _service.ListByRegion(r);
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
					model.Trails = _service.ListByCountry(c);
				}
			}

			return View(model);

		}

		/// <summary>
		/// Dispalys a single GPX track data file on a map
		/// </summary>
		public ActionResult Display(string id)
		{
			var model = InitModel();

			var trail =_service.GetTrail(id);
			//model.Country = 
			//model.Region = 

			model.Trail = trail;

			return View(model);
		}

		/// <summary>
		/// Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string id)
		{
			var trail = _service.GetTrail(id);

			var trackService = new TrackExtractService();
			
			trackService.Modify(trail.Uri);

			return Redirect(TrackViewModel.GetTrackUrl());
		}

		/// <summary>
		/// Resets the current GPX track data file cache and reloads from disk
		/// </summary>
		public ActionResult Refresh()
		{
			_service.Reload(AutoConfig.TrackRootUri);

			return Redirect(TrailViewModel.GetTrailUrl());
		}
	}
}