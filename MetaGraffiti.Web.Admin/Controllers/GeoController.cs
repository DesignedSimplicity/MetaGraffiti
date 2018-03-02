using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo.Info;
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

		public ActionResult Countries(string id)
		{
			var model = new GeoViewModel();
			return View(model);
		}

		public ActionResult Country(int id)
		{
			var model = new GeoViewModel();
			model.SelectedCountry = GeoCountryInfo.ByID(id);
			return View(model);
		}

		public ActionResult Regions(string country = null)
		{
			var model = new GeoViewModel();
			return View(model);
		}
	}
}