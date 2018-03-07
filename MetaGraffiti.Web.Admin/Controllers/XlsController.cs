using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Xls;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class XlsController : Controller
    {
		private CartoDataService _service = new CartoDataService();
		private const string CartoDataUri = @"C:\Code\KnE\ConsolidatedTrips.xlsx";

		public XlsController()
		{
			_service.Init(CartoDataUri);
		}

		public ActionResult Index()
		{
			var model = new XlsViewModel();

			return View(model);
		}

		public ActionResult Years(string id = "")
		{
			var model = new XlsViewModel();

			int year = TypeConvert.ToInt(id);
			model.RawCount = _service.ListRawPlaces(year).Count;
			model.Places = _service.ListPlaces(year);

			return View(model);
		}

		public ActionResult Sheets(string id = "")
		{
			var model = new XlsViewModel();

			model.Sheets = _service.ListSheets();
			model.SelectedSheet = id;

			return View(model);
		}


		public ActionResult Reset()
		{
			_service.Reset();
			return new RedirectResult("/xls/");
		}
	}
}