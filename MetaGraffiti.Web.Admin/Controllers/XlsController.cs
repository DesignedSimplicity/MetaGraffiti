using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Xls;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class XlsController : Controller
    {
		private CartoDataService _service = new CartoDataService();
		private const string CartoDataUri = @"C:\Code\KnE\ConsolidatedTrips.xlsx";

		public ActionResult Index(int? year = 2010)
        {
			var model = new XlsViewModel();

			var reader = new XlsFileReader(CartoDataUri);
			model.Sheets = reader.ReadFile().Sheets;

			_service.Init(CartoDataUri);

			

			model.RawPlaceCount = _service.ListRawPlaces(year.Value).Count;
			model.Places = _service.ListPlaces(year.Value);


			return View(model);
        }

		public ActionResult Reset()
		{
			_service.Reset();
			return new RedirectResult("/xls/");
		}
    }
}