﻿using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class CartoController : Controller
    {
		// ==================================================
		// Initialization

		private CartoPlaceService _cartoPlaceService;
		private TripSheetService _tripSheetService;

		public CartoController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_tripSheetService = ServiceConfig.TripSheetService;
		}

		private CartoViewModel InitModel()
		{
			var model = new CartoViewModel();

			model.LastSaved = _cartoPlaceService.LastSaved;
			model.HasChanges = _cartoPlaceService.HasChanges;

			model.Places = _cartoPlaceService.ListPlaces();

			model.Countries = _cartoPlaceService.ListCountries();
			model.PlaceTypes = _cartoPlaceService.ListPlaceTypes();

			model.SourceCountries = _tripSheetService.ListCountries();
			model.SourceYears = _tripSheetService.ListYears();

			return model;
		}


		// ==================================================
		// Actions

		public ActionResult Index()
        {
			var model = InitModel();

			return View(model);
		}

		// TODO: rename this to Map
		public ActionResult Places()
		{
			var model = InitModel();

			return View(model);
		}

		public ActionResult Country(string id)
		{
			var model = InitModel();

			model.SelectedCountry = GeoCountryInfo.Find(id);
			model.Places = _cartoPlaceService.ReportPlaces(new CartoPlaceReportRequest() { Country = model.SelectedCountry.ISO2, Sort = "Region" });

			return View(model);
		}

		public ActionResult Report(CartoPlaceReportRequest report)
		{
			var model = InitModel();

			var political = new GeoPolitical(report);
			model.SelectedCountry = political.Country;
			model.SelectedPlaceType = report.PlaceType;

			model.ReportPlaces = _cartoPlaceService.ReportPlaces(report);

			return View(model);
		}

		public ActionResult Persist(string url = "")
		{
			_cartoPlaceService.Save();
			_cartoPlaceService.Reload();

			return new RedirectResult(String.IsNullOrWhiteSpace(url) ? CartoViewModel.GetCartoUrl() : url);
		}

		public ActionResult Reload()
		{
			_cartoPlaceService.Reload();

			return new RedirectResult(CartoViewModel.GetCartoUrl());
		}
	}
}