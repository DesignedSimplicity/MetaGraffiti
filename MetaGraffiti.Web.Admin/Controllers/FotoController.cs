using System;
using System.Collections.Generic;
using System.IO;
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

		private FotoImageService _fotoImageService;

		public FotoController()
		{
			_fotoImageService = new FotoImageService();
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

		/// <summary>
		/// 
		/// </summary>
		public ActionResult Import(string uri)
		{
			var model = InitModel();

			model.SelectedUri = uri;

			return View(model);
		}

		/// <summary>
		/// 
		/// </summary>
		public ActionResult Thumb(string uri, int size = 0)
		{
			if (size == 0)
			{
				return File(uri, "image/jpeg");
			}
			
			var jpg = _fotoImageService.GetThumb(uri, size);
			return new FileContentResult(jpg, "image/jpeg");
		}
	}
}