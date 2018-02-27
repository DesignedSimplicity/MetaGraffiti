using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

		public ActionResult List(int year, int? month = null)
		{
			var model = new GpxViewModel();

			model.SelectedYear = year;
			model.SelectedMonth = month;

			model.PageName = "List";

			return View(model);
		}

		public ActionResult View(int year, int month, string file)
		{
			var model = new GpxViewModel();

			model.SelectedYear = year;
			model.SelectedMonth = month;
			model.SelectedFile = file;

			model.PageName = "View";

			return View(model);
		}
	}
}