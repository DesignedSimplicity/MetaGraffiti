using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GpxController : Controller
    {
		public ActionResult Index()
		{
			var model = new GpxViewModel();
			return View(model);
		}

		public ActionResult Report(int year, int? month = null)
		{
			var model = new GpxViewModel();

			model.SelectedYear = year;
			model.SelectedMonth = month;

			return View(model);
		}

		public ActionResult Display(string id, DateTime? start, DateTime? finish, int? sat, decimal? dop)
		{
			var model = new GpxViewModel();

			var gpx = LoadGpxFile(id);
			model.SelectedGpx = new GpxFileModel(gpx);

			model.SelectedGpx.FilterGPS = sat;
			model.SelectedGpx.FilterDOP = dop;

			if (start.HasValue) model.SelectedGpx.FilterStart = start.Value;
			if (finish.HasValue) model.SelectedGpx.FilterFinish = finish.Value;

			return View(model);
		}


		private static Dictionary<string, GpxFileInfo> _gpxCache = new Dictionary<string, GpxFileInfo>();
		private GpxFileInfo LoadGpxFile(string uri)
		{
			var key = uri.ToLowerInvariant();
			if (_gpxCache.ContainsKey(key))
				return _gpxCache[key];
			else
			{
				var gpx = new GpxFileInfo(uri);
				_gpxCache.Add(key, gpx);
				return gpx;
			}
		}
	}
}