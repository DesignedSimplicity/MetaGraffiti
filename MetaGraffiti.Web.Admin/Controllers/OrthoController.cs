﻿using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class OrthoController : Controller
    {
		private OrthoXlsService _xlsService;

		public OrthoController()
		{
			_xlsService = new OrthoXlsService();
			_xlsService.Init(AutoConfig.PlaceDataUri);
		}

		public ActionResult Index()
		{
			return Sheet("");
		}

		public ActionResult Sheet(string id)
		{
			var model = new OrthoViewModel();

			model.Sheets = _xlsService.ListSheets();
			model.SelectedSheet = id;

			return View("Sheet", model);
		}
	}
}