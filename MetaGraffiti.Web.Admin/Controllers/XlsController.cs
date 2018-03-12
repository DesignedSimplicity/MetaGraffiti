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
		private CartoLocationService _cartoService = new CartoLocationService();
		private OrthoXlsService _xlsService = new OrthoXlsService();
		private const string CartoDataUri = @"C:\Code\KnE\ConsolidatedTrips.xlsx";

		public XlsController()
		{
			_xlsService.Init(CartoDataUri);
		}

		public XlsViewModel InitModel()
		{
			var model = new XlsViewModel();

			model.Countries = new List<GeoCountryInfo>();
			foreach (var place in _xlsService.ListPlaces())
			{
				var c = GeoCountryInfo.ByName(place.Country, true);
				if (c != null && !model.Countries.Any(x => x.CountryID == c.CountryID)) model.Countries.Add(c);
			}

			return model;
		}

		public ActionResult Index()
		{
			var model = InitModel();

			return View(model);
		}

		public ActionResult Year(string id = "")
		{
			var model = InitModel();

			int year = TypeConvert.ToInt(id);
			model.RawCount = _xlsService.ListRawPlaces(year).Count;

			var places = _xlsService.ListPlaces(year);

			var rows = new List<XlsRowModel>();
			foreach (var place in places)
			{
				var row = new XlsRowModel();
				row.Place = place;

				var country = GeoCountryInfo.Find(place.Country);
				if (country != null) row.Location = _cartoService.FindLocation(place.Name, country);

				rows.Add(row);
			}
			model.SelectedRows = rows;

			return View(model);
		}

		public ActionResult Country(string id = "")
		{
			var model = InitModel();

			var country = GeoCountryInfo.ByName(id, true);
			var places = _xlsService.ListPlaces().Where(x => x.Country == country.Name).ToList();

			var rows = new List<XlsRowModel>();
			foreach(var place in places)
			{
				var row = new XlsRowModel();
				row.Place = place;
				row.Location = _cartoService.FindLocation(place.Name, country);

				rows.Add(row);
			}
			model.SelectedRows = rows;

			return View(model);
		}

		public ActionResult Sheet(string id = "")
		{
			var model = InitModel();

			model.Sheets = _xlsService.ListSheets();
			model.SelectedSheet = id;

			return View(model);
		}


		public ActionResult Reset()
		{
			_xlsService.Reset();
			return new RedirectResult("/xls/");
		}
	}
}