using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class JsonController : Controller
    {
		public ActionResult PlaceTypes()
		{
			return Json(ServiceConfig.CartoPlaceService.ListPlaceTypes(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult Countries()
		{
			return Json(GeoCountryInfo.All.Select(x => x.Name), JsonRequestBehavior.AllowGet);
		}
	}
}