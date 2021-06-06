using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class FotoController : Controller
    {
		// ==================================================
		// Initialization

		//private CartoPlaceService _cartoPlaceService;

		public FotoController()
		{
			//_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		private FotoViewModel InitModel()
		{
			var model = new FotoViewModel()
			{
			};

			return model;
		}


		// ==================================================
		// Actions

		/// <summary>
		/// 
		/// </summary>
		public ActionResult Index()
		{
			var model = InitModel();
			return View(model);
		}
	}
}