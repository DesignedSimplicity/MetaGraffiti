using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class PlaceController : Controller
    {
		private CartoPlaceService _cartoPlaceService;
		private TripSheetService _tripSheetService;

		public PlaceController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_tripSheetService = ServiceConfig.TripSheetService;
		}

		public PlaceViewModel InitModel()
		{
			var model = new PlaceViewModel();

			model.Countries = _tripSheetService.ListCountries();
			model.Years = _tripSheetService.ListYears();

			return model;
		}

		public ActionResult Index()
        {
			var model = InitModel();

			// client side search: https://developers.google.com/maps/documentation/javascript/examples/places-searchbox

			return View(model);
        }

		public ActionResult Report(int? year, string country)
		{
			var model = InitModel();

			model.SelectedYear = year;
			model.SelectedCountry = GeoCountryInfo.Find(country);

			model.FilteredPlaces = new List<PlaceModel>();
			var import = _tripSheetService.ListPlaces(year, country);
			foreach(var data in import)
			{
				var place = new PlaceModel();
				place.Data = data;
				model.FilteredPlaces.Add(place);

				var r = GeoRegionInfo.Find(data.Region);
				if (r != null) place.Place = _cartoPlaceService.FindPlace(r, data.Name, true);

				if (place.Place == null)
				{
					var c = GeoCountryInfo.Find(data.Country);
					if (c != null) place.Place = _cartoPlaceService.FindPlace(c, data.Name, true);
				}				
			}
			model.ImportPlaces = import;

			return View(model);
		}

		/// <summary>
		/// Searches google for places by name
		/// </summary>
		public ActionResult Search(CartoPlaceSearch search)
		{
			var model = InitModel();

			model.SearchCriteria = (search == null ? new CartoPlaceSearch() : search);
			model.SearchResults = new List<CartoPlaceInfo>();

			if (!String.IsNullOrWhiteSpace(search.Name))
			{
				model.SearchResults = _cartoPlaceService.LookupLocations(search.Name);
			}
			else if (search.Latitude.HasValue && search.Longitude.HasValue)
			{
				var position = new GeoPosition(search.Latitude.Value, search.Longitude.Value);
				model.SearchResults = _cartoPlaceService.LookupLocations(position);
			}

			return View(model);
		}

		/// <summary>
		/// Displays a google location for import
		/// </summary>
		[HttpGet]
		public ActionResult Preview(string id)
		{
			var model = InitModel();

			var place = _cartoPlaceService.FindByGooglePlaceID(id);
			if (place != null)
			{
				//return new RedirectResult(CartoViewModel.GetEditUrl(place.Key));
				model.Place = place;
				model.ConfirmMessage = "Place already exists!";
			}
			else
				model.Place = _cartoPlaceService.LookupByPlaceID(id);

			return View(model);
		}

		[HttpPost]
		public ActionResult Create(CartoPlaceCreateRequest request)
		{
			// TODO: do validation
			var place = _cartoPlaceService.CreatePlace(request);
			if (place != null) return new RedirectResult(CartoViewModel.GetEditUrl(place.Key));

			// show validation error messages
			var model = InitModel();

			return View(model);
		}



		/*
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
		*/
	}
}