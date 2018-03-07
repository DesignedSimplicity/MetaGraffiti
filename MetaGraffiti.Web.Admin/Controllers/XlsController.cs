using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Xls;
//using MetaGraffiti.Base.Modules.Xls.Data;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class XlsController : Controller
    {
        // GET: Xls
        public ActionResult Index()
        {
			var model = new XlsViewModel();

			var reader = new XlsFileReader(@"C:\Code\KnE\ConsolidatedTrips.xlsx");
			model.Sheets = reader.ReadFile().Sheets;

			return View(model);
        }
    }
}