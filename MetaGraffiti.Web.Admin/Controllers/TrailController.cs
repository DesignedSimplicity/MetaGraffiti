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
		private TopoTrailService _trailDataService;

		public TrailController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_trailDataService = ServiceConfig.TopoTrailService;
		}

		private TrailViewModel InitModel()
		{
			var model = new TrailViewModel();

			//model.Trail = _trackExtractService.GetTrail();
			//model.Tracks = _trackExtractService.ListTracks();

			return model;
		}


		// ==================================================
		// Actions

		/// <summary>
		/// Displays trail profile and all track segments in current edit session
		/// </summary>
		[HttpGet]
		public ActionResult Update()
		{
			var model = InitModel();

			return View(model);
		}

		/// <summary>
		/// Updates trail profile for current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Update(TrailUpdateRequest update)
		{
			var model = InitModel();

			_trackExtractService.UpdateTrail(update);

			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View(model);
		}

		/// <summary>
		/// Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string id)
		{
			var trail = _trailDataService.GetTrail(id);

			_trackExtractService.ModifyTrail(trail.Source);

			return Redirect(TopoViewModel.GetUpdateUrl());
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
			return Redirect(TopoViewModel.GetTrailUrl(filename));
		}
	}
}