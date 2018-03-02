using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class GpxController : Controller
    {
		public ActionResult Index()
		{
			var model = new GpxViewModel();
			return View(model);
		}

		public ActionResult Report(int year, int? month = null)
		{
			var model = new GpxViewModel();

			model.SelectedYear = year;
			model.SelectedMonth = month;

			return View(model);
		}

		[HttpGet]
		public ActionResult Display(string id, DateTime? start, DateTime? finish, int? sat, decimal? dop)
		{
			var model = new GpxViewModel();

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
			var model = new GpxViewModel();

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