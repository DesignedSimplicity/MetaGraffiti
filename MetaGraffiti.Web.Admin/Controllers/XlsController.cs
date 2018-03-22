using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OfficeOpenXml;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;

// TODO: migrate to PlaceController
namespace MetaGraffiti.Web.Admin.Controllers
{
    public class XlsController : Controller
    {
		private CartoPlaceService _cartoPlaceService;
		private OrthoXlsService _xlsService;

		public XlsController()
		{
			_cartoPlaceService = new CartoPlaceService(null);
			_cartoPlaceService.LoadPlaces(AutoConfig.CartoDataUri);

			_xlsService = new OrthoXlsService();
			_xlsService.Init(AutoConfig.PlaceDataUri);
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
				if (country != null) row.Location = _cartoPlaceService.FindPlace(place.Name, country);

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
				row.Location = _cartoPlaceService.FindPlace(place.Name, country);

				rows.Add(row);
			}
			model.SelectedRows = rows;

			return View(model);
		}



		public ActionResult Reset()
		{
			_xlsService.Reset();
			return new RedirectResult("/xls/");
		}




		public ActionResult Export()
		{
			using (var ep = new ExcelPackage())
			{
				var ws = ep.Workbook.Worksheets.Add("Locations");

				// build header
				int row = 1;
				int cell = 1;

				// place idenity
				ws.Cells[row, cell++].Value = "PlaceID";
				ws.Cells[row, cell++].Value = "PlaceKey";
				ws.Cells[row, cell++].Value = "PlaceType";
				ws.Cells[row, cell++].Value = "GoogleKey";

				// geo political
				ws.Cells[row, cell++].Value = "Timezone";
				ws.Cells[row, cell++].Value = "Country";
				ws.Cells[row, cell++].Value = "Region";

				// logical name
				ws.Cells[row, cell++].Value = "Name";
				ws.Cells[row, cell++].Value = "LocalName";
				ws.Cells[row, cell++].Value = "DisplayAs";
				ws.Cells[row, cell++].Value = "Description";

				// logical location
				ws.Cells[row, cell++].Value = "Address";
				ws.Cells[row, cell++].Value = "Locality";
				ws.Cells[row, cell++].Value = "Postcode";
				ws.Cells[row, cell++].Value = "Subregions";
				ws.Cells[row, cell++].Value = "Sublocalities";

				// physical location
				ws.Cells[row, cell++].Value = "CenterLatitude";
				ws.Cells[row, cell++].Value = "CenterLongitude";
				ws.Cells[row, cell++].Value = "NorthLatitude";
				ws.Cells[row, cell++].Value = "SouthLatitude";
				ws.Cells[row, cell++].Value = "WestLongitude";
				ws.Cells[row, cell++].Value = "EastLongitude";

				// add location rows
				var id = 1;
				var locations = _cartoPlaceService.ListPlaces();
				foreach (var location in locations)
				{
					row++;
					cell = 1;

					// place idenity
					ws.Cells[row, cell++].Value = id++;
					ws.Cells[row, cell++].Value = location.PlaceType;
					ws.Cells[row, cell++].Value = location.Key;
					ws.Cells[row, cell++].Value = location.GoogleKey;

					// geo political
					ws.Cells[row, cell++].Value = (location.Timezone?.TZID ?? "");
					ws.Cells[row, cell++].Value = (location.Country?.Name ?? "");
					ws.Cells[row, cell++].Value = (location.Region?.RegionName ?? "");

					// logical name
					ws.Cells[row, cell++].Value = location.Name;
					ws.Cells[row, cell++].Value = location.LocalName;
					ws.Cells[row, cell++].Value = location.DisplayAs;
					ws.Cells[row, cell++].Value = location.Description;

					// logical location
					ws.Cells[row, cell++].Value = location.Address;
					ws.Cells[row, cell++].Value = location.Locality;
					ws.Cells[row, cell++].Value = location.Postcode;
					ws.Cells[row, cell++].Value = location.Subregions;
					//ws.Cells[row, cell++].Value = location.Sublocalities;

					// physical location
					ws.Cells[row, cell++].Value = location.Center.Latitude;
					ws.Cells[row, cell++].Value = location.Center.Longitude;
					ws.Cells[row, cell++].Value = (location.Bounds?.NorthWest.Latitude ?? 0.0);
					ws.Cells[row, cell++].Value = (location.Bounds?.SouthEast.Latitude ?? 0.0);
					ws.Cells[row, cell++].Value = (location.Bounds?.NorthWest.Longitude ?? 0.0);
					ws.Cells[row, cell++].Value = (location.Bounds?.SouthEast.Longitude ?? 0.0);
				}

				// return file
				return File(ep.GetAsByteArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "Locations.xlsx");
			}
		}
	}
}