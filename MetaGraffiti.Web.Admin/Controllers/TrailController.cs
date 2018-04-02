using MetaGraffiti.Base.Modules.Geo.Info;
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
		private TrackExtractService _trackExtractService = new TrackExtractService();
		private TopoTrailService _trailDataService;

		public TrailController()
		{
			_trailDataService = ServiceConfig.TopoTrailService;
		}

		private TrailViewModel InitModel()
		{
			var model = new TrailViewModel();

			model.Trails = _trailDataService.ListTrails();
			model.FirstDate = model.Trails.Min(x => x.LocalDate);
			model.LastDate = model.Trails.Max(x => x.LocalDate);

			model.Countries = _trailDataService.ListCountries().OrderBy(x => x.Name);

			return model;
		}

		/// <summary>
		/// Updates the metadata in an existing GPX track data file (name, description, keywords, but NOT track/point data)
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string id)
		{
			var trail = _trailDataService.GetTrail(id);

			_trackExtractService.EditTrail(trail.Uri);

			return Redirect(TrackViewModel.GetManageUrl());
		}

		/// <summary>
		/// Creates an internal file from all of the tracks in the current edit session
		/// </summary>
		public ActionResult Import(bool overwrite = false)
		{
			var model = InitModel();

			// TODO: move this into TrailDataService
			var track = _trackExtractService.GetTrackGroup();
			if (String.IsNullOrWhiteSpace(track.Name)) model.ErrorMessages.Add("Name is missing.");
			if (track.Timezone == null) model.ErrorMessages.Add("Timezone is missing.");
			if (track.Country == null) model.ErrorMessages.Add("Country is missing.");
			if (track.Country != null && track.Country.HasRegions && track.Region == null) model.ErrorMessages.Add("Region is missing.");

			// show error messages if necessary
			if (model.HasError) return View(model);

			// check folders are initialized
			var folder = Path.Combine(AutoConfig.TrailSourceUri, track.Country.Name);
			if (!Directory.Exists(AutoConfig.TrailSourceUri)) throw new Exception($"TrackRoot not initalized: {AutoConfig.TrailSourceUri}");
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			// check existing filename and if overwrite
			var filename = $"{String.Format("{0:yyyyMMdd}", track.Timestamp)} {track.Name}";
			var uri = Path.Combine(folder, filename + ".gpx");

			// show overwrite confirmation if necessary
			if (System.IO.File.Exists(uri) && !overwrite)
			{
				model.ConfirmMessage = uri;
				return View(model);
			}

			// create internal file
			_trackExtractService.WriteTrackFile(uri);

			// reset track extract cache
			_trackExtractService.ResetSession();

			// reload trails data before redirect
			ServiceConfig.ResetTopoTrail(); //_trailDataService.ResetCache();

			// redirect to new trail page
			return Redirect(TrailViewModel.GetDisplayUrl(filename));
		}
	}
}