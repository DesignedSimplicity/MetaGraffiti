using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Web.Admin.Models;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Web.Admin.Controllers
{
	// gpx/							GET displays a summary of the GPX source files
	// gpx/import/					GET displays a list of all GPX source files which have not yet been imported
	// gpx/report/?					GET displays a detailed and filterable report and include links to imported files
	// gpx/preview/{id}				GET displays a preview of a given GPX file on a map and provides actions to extract point data


	// TODO: deprecate
	public class GpxController : Controller
    {
		private GpxCacheService _gpxService;

		public GpxController()
		{
			_gpxService = ServiceConfig.GpxSourceService;
		}

		//TODO: refactor this to a repository pattern
		private GpxManagerModel TrackManager
		{
			get
			{
				var manager = (GpxManagerModel)Session["TrackManager"];
				if (manager == null) manager = new GpxManagerModel();
				Session["TrackManager"] = manager;
				return manager;
			}
		}

		public GpxViewModel InitModel()
		{
			string rootUri = Path.Combine(AutoConfig.RootConfigUri, "GPS");

			var model = new GpxViewModel();
			//model.Files = _gpxService.Init(rootUri);
			//model.Cache = _gpxService.LoadDirectory(rootUri, true);
			model.Cache = _gpxService.ListCached();
			model.Manager = this.TrackManager;

			return model;
		}

		/// <summary>
		/// Displays a calendar of GPX files and a list of all extracted tracks
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Index()
		{
			var model = InitModel();
			return View(model);
		}

		/// <summary>
		/// Lists any GPX files that had an error loading
		/// </summary>
		[HttpGet]
		public ActionResult Debug()
		{
			var model = InitModel();
			return View(model);
		}

		/// <summary>
		/// Lists all GPX files in a given year with optional month
		/// </summary>
		[HttpGet]
		public ActionResult Report(int year, int? month = null)
		{
			var model = InitModel();

			model.SelectedYear = year;
			model.SelectedMonth = month;

			return View(model);
		}

		/// <summary>
		/// Displays a single GPX file with a list of points and a map
		/// </summary>
		[HttpGet]
		public ActionResult Display(string uri)
		{
			var model = InitModel();

			// TODO: refactor away from complex GpxDisplayModel
			var cache = _gpxService.LoadFile(uri);
			var gpx = new GpxDisplayModel(cache);

			gpx.Points = _gpxService.FilterPoints(cache.File.Points, cache.Filter);

			model.SelectedGpx = gpx;

			model.SelectedYear = gpx.StartTime.Year;
			model.SelectedMonth = gpx.StartTime.Month;

			return View(model);
		}

		/// <summary>
		/// Updates cached metadata from form values
		/// </summary>
		[HttpPost]
		public ActionResult Update(string uri, GpxUpdateRequest update)
		{
			_gpxService.UpdateMetaData(uri, update);

			return Redirect(GpxViewModel.GetDisplayUrl(uri));
		}

		/// <summary>
		/// Applies filter on POST, clears filter on GET
		/// </summary>
		public ActionResult Filter(string uri, GpxFilterRequest filter)
		{
			_gpxService.UpdateFilters(uri, filter);

			return Redirect(GpxViewModel.GetDisplayUrl(uri));
		}

		/// <summary>
		/// Extracts the updated and filtered file data as a new track in memory
		/// </summary>
		[HttpGet]
		public ActionResult Extract(string uri)
		{
			var model = InitModel();
			var cache = _gpxService.LoadFile(uri);

			var track = _gpxService.ExtractTrack(cache);
			model.Manager.Tracks.Add(track);

			return Redirect(GpxViewModel.GetManageUrl());
		}

		/// <summary>
		/// Manages the combiation of multiple extract tracks into a single GPX or KML file
		/// </summary>
		[HttpGet]
		public ActionResult Manage()
		{
			var model = InitModel();

			return View(model);
		}

		/// <summary>
		/// Updates the metadata and/or exports it to a combined file
		/// </summary>
		[HttpPost]
		public ActionResult Manage(string action = "", string name = "", string description = "", string timezone = "")
		{
			//TODO: convert inputs to a proper model
			//TODO: move each action into separate methods

			var model = InitModel();

			var manager = model.Manager;

			switch(action.ToUpperInvariant())
			{
				case "UPDATE":
					manager.Name = name;
					manager.Description = description;
					manager.Timezone = GeoTimezoneInfo.ByName(timezone);
					break;

				case "TIMEZONE":
					var google = new GeoLookupService(new GoogleApiService(AutoConfig.GoogleMapsApiKey));
					manager.Timezone = google.LookupTimezone(manager.FirstPoint);
					break;

				case "GPX":
					var gpx = _gpxService.ExportGpxFile(manager.Name, manager.Description, manager.Tracks);
					return File(gpx, System.Net.Mime.MediaTypeNames.Application.Octet, $"{manager.Name}.gpx");

				case "KML":
					var kml = _gpxService.ExportKmlFile(manager.Name, manager.Description, manager.Tracks);
					return File(kml, System.Net.Mime.MediaTypeNames.Application.Octet, $"{manager.Name}.kml");
			}

			return View(model);
		}

		/// <summary>
		/// Removes an extract track from the cache
		/// </summary>
		[HttpGet]
		public ActionResult Remove(string id)
		{
			var model = InitModel();

			var track = model.Manager.Tracks.FirstOrDefault(x => String.Compare(x.ID, id, true) == 0);
			if (track != null) model.Manager.Tracks.Remove(track);

			return Redirect(GpxViewModel.GetManageUrl());
		}

		/// <summary>
		/// Exports the updated and filtered file data as a new GPX or KML file
		/// </summary>
		[HttpGet]
		public ActionResult Export(string uri, string format = "gpx")
		{
			var cache = _gpxService.LoadFile(uri);
			var metadata = cache.MetaData;

			if (format == "kml")
			{
				var kml = _gpxService.ExportKmlFile(metadata.Name, metadata.Description, cache.File.Tracks, cache.Filter);
				return File(kml, System.Net.Mime.MediaTypeNames.Application.Octet, $"{metadata.Name}.kml");
			}
			else
			{
				var gpx = _gpxService.ExportGpxFile(metadata.Name, metadata.Description, cache.File.Tracks, cache.Filter);
				return File(gpx, System.Net.Mime.MediaTypeNames.Application.Octet, $"{metadata.Name}.gpx");
			}
		}
	}
}