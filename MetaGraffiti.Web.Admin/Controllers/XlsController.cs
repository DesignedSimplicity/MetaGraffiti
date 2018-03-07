using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo.Info;
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

		public XlsViewModel InitView()
		{
			var model = new XlsViewModel();

			model.Countries = new List<GeoCountryInfo>();
			foreach (var place in _service.ListPlaces())
			{
				var c = GeoCountryInfo.ByName(place.Country, true);
				if (c != null && !model.Countries.Any(x => x.CountryID == c.CountryID)) model.Countries.Add(c);
			}

			return model;
		}

		public ActionResult Index()
		{
			var model = InitView();

			return View(model);
		}

		public ActionResult Year(string id = "")
		{
			var model = InitView();

			int year = TypeConvert.ToInt(id);
			model.RawCount = _service.ListRawPlaces(year).Count;
			model.Places = _service.ListPlaces(year);

			return View(model);
		}

		public ActionResult Country(string id = "")
		{
			var model = InitView();

			var country = GeoCountryInfo.ByName(id, true);
			model.Places = _service.ListPlaces().Where(x => x.Country == country.Name).ToList();

			return View(model);
		}

		public ActionResult Sheet(string id = "")
		{
			var model = InitView();

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