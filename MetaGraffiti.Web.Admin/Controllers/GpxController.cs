using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetaGraffiti.Base.Modules.Gpx.Data;
using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Web.Admin.Models;
using MetaGraffiti.Web.Admin.Services;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GpxController : Controller
    {
		private GpxService _gpxService = new GpxService();

		private List<GpxTrackData> ExtractedTracks
		{
			get
			{
				var tracks = (List<GpxTrackData>)Session["ExtractedTracks"];
				if (tracks == null) tracks = new List<GpxTrackData>();
				Session["ExtractedTracks"] = tracks;
				return tracks;
			}
		}

		public GpxViewModel InitView()
		{
			string rootUri = Path.Combine(AutoConfig.RootConfigUri, "GPS");

			var model = new GpxViewModel();
			model.Files = _gpxService.Init(rootUri);
			model.Cache = _gpxService.LoadDirectory(rootUri, true);
			model.Tracks = ExtractedTracks;

			return model;
		}

		public ActionResult Index()
		{
			var model = InitView();
			return View(model);
		}

		public ActionResult Debug()
		{
			var model = InitView();
			return View(model);
		}

		public ActionResult Report(int year, int? month = null)
		{
			var model = InitView();

			model.SelectedYear = year;
			model.SelectedMonth = month;

			return View(model);
		}

		[HttpGet]
		public ActionResult Display(string uri)
		{
			var model = InitView();

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
		public ActionResult Extract(string uri)
		{
			var cache = _gpxService.LoadFile(uri);

			var track = _gpxService.ExtractTrack(cache.MetaData, cache.File.Points, cache.Filter);
			ExtractedTracks.Add(track);

			return Redirect(GpxViewModel.GetManageUrl());
		}

		/// <summary>
		/// Combines multiple extract tracks into a single GPX or KML file
		/// </summary>
		public ActionResult Manage()
		{
			var model = InitView();

			return View(model);
		}

		/// <summary>
		/// Exports the updated and filtered file data as a new GPX or KML file
		/// </summary>
		public ActionResult Export(string uri, string format = "gpx")
		{
			var cache = _gpxService.LoadFile(uri);

			if (format == "kml")
			{
				var kml = _gpxService.ExportKmlFile(cache.MetaData, cache.File.Tracks, cache.Filter);
				return File(kml, System.Net.Mime.MediaTypeNames.Application.Octet, $"{cache.MetaData.Name}.kml");
			}
			else
			{
				var gpx = _gpxService.ExportGpxFile(cache.MetaData, cache.File.Tracks, cache.Filter);
				return File(gpx, System.Net.Mime.MediaTypeNames.Application.Octet, $"{cache.MetaData.Name}.gpx");
			}
		}
	}
}