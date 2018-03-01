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

		public ActionResult Display(string id)
		{
			var model = new GpxViewModel();

			model.SelectedGpx = new GpxFileModel(new GpxFileInfo(id));

			return View(model);
		}
	}
}