using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Web.Admin.Models;
using MetaGraffiti.Web.Admin.Services;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GpxController : Controller
    {
		private string _rootUri = @"E:\Annuals\_GPS";
		private GpxService _gpxService = new GpxService();

		public GpxViewModel InitView()
		{
			var model = new GpxViewModel();
			model.Files = _gpxService.Init(_rootUri);
			model.Cache = _gpxService.LoadDirectory(_rootUri, true);
			return model;
		}

		public ActionResult Index()
		{
			var model = InitView();
			return View(model);
		}

		public ActionResult Debug()
		{
			var model = InitView();
			return View(model);
		}

		public ActionResult Report(int year, int? month = null)
		{
			var model = InitView();

			model.SelectedYear = year;
			model.SelectedMonth = month;

			return View(model);
		}

		[HttpGet]
		public ActionResult Display(string id, DateTime? start, DateTime? finish, int? sat, decimal? dop)
		{
			var model = InitView();

			model.SelectGpxFile(id);
			//model.SelectedGpx = new GpxDisplayModel(gpx);

			model.SelectedGpx.FilterGPS = sat;
			model.SelectedGpx.FilterDOP = dop;

			if (start.HasValue) model.SelectedGpx.FilterStart = start.Value;
			if (finish.HasValue) model.SelectedGpx.FilterFinish = finish.Value;

			return View(model);
		}

		[HttpPost]
		public ActionResult Display(GpxUpdateModel update)
		{
			var model = InitView();

			model.SelectGpxFile(update.ID);
			//model.SelectedGpx = new GpxDisplayModel(gpx);

			model.SelectedGpx.Name = update.Name;
			//model.SelectedGpx.File.Data.Description = update.Description;

			model.SelectedGpx.FilterGPS = update.SAT;
			model.SelectedGpx.FilterDOP = update.DOP;

			if (update.Start.HasValue) model.SelectedGpx.FilterStart = update.Start.Value;
			if (update.Finish.HasValue) model.SelectedGpx.FilterFinish = update.Finish.Value;

			return View(model);
		}


		public void Index2()
		{
			// list all GPX files in known directory
			// parse into calendar display model
		}

		public void Report2()
		{
			// display all GPX files for a given year/month
			// ensure each file is loaded with basic details
		}

		public void Display2()
		{
			// load and cache specific GPX file
			// process additional metadata/information
			// display all data and map
		}

		public void Update2()
		{
			// saves filter and/or metadata updates in cache
			// applies updates and displays data and map
		}

		public void Export2()
		{
			// exports filterd data as GPX or KML file
		}
	}
}