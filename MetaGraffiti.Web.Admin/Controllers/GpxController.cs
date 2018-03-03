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
			model.SelectedGpx = new GpxDisplayModel(cache);

			var filter = cache.MetaData;

			model.SelectedGpx.FilterGPS = filter.FilterGPS;
			model.SelectedGpx.FilterDOP = filter.FilterDOP;

			model.SelectedGpx.FilterStart = filter.FilterStart;
			model.SelectedGpx.FilterFinish = filter.FilterFinish;

			return View(model);
		}

		/// <summary>
		/// Updates cached metadata from form values
		/// </summary>
		[HttpPost]
		public ActionResult Update(GpxCacheMetaData update)
		{
			var cache = _gpxService.LoadFile(update.Uri);
			var data = cache.MetaData;

			data.Name = update.Name;
			data.Description = update.Description;
			data.LocationName = update.LocationName;

			//TODO: deal with changes to country/region
			//TODO: deal with timezone/recalcuating local time

			return Redirect("/gpx/display/?uri=" + update.Uri);
		}

		/// <summary>
		/// Applies filter on POST, clears filter on GET
		/// </summary>
		public ActionResult Filter(GpxCacheMetaData update)
		{
			var cache = _gpxService.LoadFile(update.Uri);
			var data = cache.MetaData;

			data.FilterDOP = update.FilterDOP;
			data.FilterGPS = update.FilterGPS;
			data.FilterStart = update.FilterStart;
			data.FilterFinish = update.FilterFinish;

			return Redirect("/gpx/display/?uri=" + update.Uri);
		}

		public ActionResult Export(string uri, string format = "gpx")
		{
			var cache = _gpxService.LoadFile(uri);
			var data = cache.MetaData;

			return null;// Redirect("/gpx/display/?uri=" + update.Uri);
		}
	}
}