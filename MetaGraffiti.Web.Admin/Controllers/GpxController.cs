using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Web.Admin.Models;
using MetaGraffiti.Web.Admin.Services;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GpxController : Controller
    {
		private GpxService _gpxService = new GpxService();

		public GpxViewModel InitView()
		{
			string rootUri = Path.Combine(AutoConfig.RootConfigUri, "GPS");

			var model = new GpxViewModel();
			model.Files = _gpxService.Init(rootUri);
			model.Cache = _gpxService.LoadDirectory(rootUri, true);

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

			return View(model);
		}

		/// <summary>
		/// Updates cached metadata from form values
		/// </summary>
		[HttpPost]
		public ActionResult Update(string uri, GpxUpdateData update)
		{
			_gpxService.UpdateMetaData(uri, update);

			return Redirect("/gpx/display/?uri=" + uri);
		}

		/// <summary>
		/// Applies filter on POST, clears filter on GET
		/// </summary>
		public ActionResult Filter(string uri, GpxFilterData filter)
		{
			_gpxService.UpdateFilters(uri, filter);

			return Redirect("/gpx/display/?uri=" + uri);
		}

		public ActionResult Export(string uri, string format = "gpx")
		{
			var cache = _gpxService.LoadFile(uri);

			var data = _gpxService.ExportFile(cache.MetaData, cache.File.Tracks, cache.Filter);

			return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, $"{cache.MetaData.Name}.gpx");
		}
	}
}