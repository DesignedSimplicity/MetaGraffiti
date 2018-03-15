using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
	// trail/						GET Displays a calendar and a list of countries with their respective GPX imported track data
	// trail/report/?				GET Displays a list of GPX track files filtered by report criteria
	// trail/country/{id}/?region=	GET Displays all of the tracks in a list and on a map for a given country with optional region filter
	// trail/display/{id}			GET Dispalys a single GPX track data file on a map
	// trail/update/				POST Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
	// trail/refresh/				GET Resets the current GPX track data file cache and reloads from disk

	public class TrailController : Controller
    {
		/// <summary>
		/// Displays a calendar and a list of countries with their respective GPX imported track data
		/// </summary>
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Displays a list of GPX track files filtered by report criteria
		/// </summary>
		public ActionResult Report(object report)
		{
			return View();
		}

		/// <summary>
		/// Displays all of the tracks in a list and on a map for a given country with optional region filter
		/// </summary>
		public ActionResult Country(string id, string region)
		{
			return View();
		}

		/// <summary>
		/// Dispalys a single GPX track data file on a map
		/// </summary>
		public ActionResult Display(string id)
		{
			return View();
		}

		/// <summary>
		/// Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
		/// </summary>
		[HttpPost]
		public ActionResult Update(string id)
		{
			return View();
		}

		/// <summary>
		/// Resets the current GPX track data file cache and reloads from disk
		/// </summary>
		public ActionResult Refresh()
		{
			return View();
		}
	}
}