using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GeoController : Controller
    {
		public ActionResult Index()
		{
			var model = new GeoViewModel();
			return View(model);
		}

		public ActionResult Timezone()
		{
			var model = new GeoViewModel();
			return View(model);
		}

		public ActionResult Country()
		{
			var model = new GeoViewModel();
			return View(model);
		}

		public ActionResult Region(string country = null)
		{
			var model = new GeoViewModel();
			return View(model);
		}
	}
}