using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class TrailController : Controller
    {
		// ==================================================
		// Initialization

		private TrackExtractService _trackExtractService = new TrackExtractService();
		private CartoPlaceService _cartoPlaceService;
		private TopoTrailService _topoTrailService;

		public TrailController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_topoTrailService = ServiceConfig.TopoTrailService;
		}

		private TrailViewModel InitModel(string id = "")
		{
			var model = new TrailViewModel();

			if (!String.IsNullOrWhiteSpace(id))
			{
				model.Trail = _topoTrailService.GetTrail(id);
				if (!model.IsTimezoneValid) model.ErrorMessages.Add("Timezone missing! Defaulting to UTC.");
			}

			return model;
		}


		// ==================================================
		// Actions

		public ActionResult Display(string id)
		{
			var model = InitModel(id);

			return View(model);
		}

		/// <summary>
		/// Displays trail profile and all track segments in current edit session
		/// </summary>
		[HttpGet]
		public ActionResult Update(string id)
		{
			var model = InitModel(id);

			return View(model);
		}

		/// <summary>
		/// Updates trail profile for current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Update(TopoTrailUpdateRequest update)
		{
			// TODO: do validation here!!!
			var response = _topoTrailService.UpdateTrail(update);

			if (response.OK)
			{
				var model = InitModel(response.Data.Key);
				model.ConfirmMessage = $"Trail updated at {DateTime.Now}";
				return View(model);
			}
			else
			{
				var model = InitModel(update.Key);
				foreach(var error in response.ValidationErrors)
				{
					model.ErrorMessages.Add($"Error {error.Field} = {error.Message}");
				}				
				return View(model);
			}
		}

		/// <summary>
		/// Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string id)
		{
			var trail = _topoTrailService.GetTrail(id);

			_trackExtractService.ModifyTrail(trail.Source);

			return Redirect(TrailViewModel.GetUpdateUrl());
		}

		/// <summary>
		/// Creates an internal file from all of the tracks in the current edit session
		/// </summary>
		public ActionResult Import(bool overwrite = false)
		{
			var model = InitModel();

			// TODO: move this into TrailDataService
			var trail = _trackExtractService.GetTrackGroup();
			if (String.IsNullOrWhiteSpace(trail.Name)) model.ErrorMessages.Add("Name is missing.");
			if (trail.Timezone == null) model.ErrorMessages.Add("Timezone is missing.");
			if (trail.Country == null) model.ErrorMessages.Add("Country is missing.");
			if (trail.Country != null && trail.Country.HasRegions && trail.Region == null) model.ErrorMessages.Add("Region is missing.");

			// show error messages if necessary
			if (model.HasError) return View(model);

			// check folders are initialized
			var folder = Path.Combine(AutoConfig.TrailSourceUri, trail.Country.Name);
			if (!Directory.Exists(AutoConfig.TrailSourceUri)) throw new Exception($"TrackRoot not initalized: {AutoConfig.TrailSourceUri}");
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			// check existing filename and if overwrite
			var filename = $"{String.Format("{0:yyyyMMdd}", trail.Timestamp)} {trail.Name}";
			var uri = Path.Combine(folder, filename + ".gpx");

			// show overwrite confirmation if necessary
			if (System.IO.File.Exists(uri) && !overwrite)
			{
				model.ConfirmMessage = uri;
				return View(model);
			}

			// check old file requires cleanup
			var previous = trail.Uri;
			if (!String.IsNullOrWhiteSpace(previous) && previous.StartsWith(AutoConfig.TrailSourceUri) && System.IO.File.Exists(previous) && !overwrite)
			{
				model.ConfirmMessage = uri;
				return View(model);
			}

			// create internal file
			_trackExtractService.WriteTrackFile(uri);

			// clean up old renamed file
			if (System.IO.File.Exists(previous)) System.IO.File.Delete(previous);

			// reset track extract cache
			_trackExtractService.ResetSession();

			// reload trails data before redirect
			ServiceConfig.ResetTopoTrail();

			// redirect to new trail page
			return Redirect(TrailViewModel.GetTrailUrl(filename));
		}
	}
}