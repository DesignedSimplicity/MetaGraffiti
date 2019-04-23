using MetaGraffiti.Base.Modules.Topo.Info;
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

		private TrackEditService _trackEditService;
		private TopoTrailService _topoTrailService;
		private CartoPlaceService _cartoPlaceService;
		private static TopoTrailInfo _editing;

		public TrailController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_topoTrailService = ServiceConfig.TopoTrailService;
			_trackEditService = new TrackEditService();
		}

		private TrailViewModel InitModel(string id = "")
		{
			var model = new TrailViewModel();

			if (!String.IsNullOrWhiteSpace(id))
			{
				var trail = _topoTrailService.GetTrail(id);
				model.Trail = trail;
				model.Tracks = trail.TopoTracks;
			}

			return model;
		}


		// ==================================================
		// Actions

		/// <summary>
		/// Displays a trail and tracks in read only mode
		/// </summary>
		public ActionResult Display(string id)
		{
			var model = InitModel(id);

			// load places
			model.Places = _cartoPlaceService.ReportPlaces(new CartoPlaceReportRequest()
			{
				Country = model.Trail.Country.ISO2,
			});


			return View(model);
		}

		/// <summary>
		/// Displays trail profile for editing
		/// </summary>
		[HttpGet]
		public ActionResult Update(string id)
		{
			var model = InitModel(id);

			model.Edit = new TopoTrailFormModel(model.Trail);

			return View(model);
		}

		/// <summary>
		/// Updates trail profile
		/// </summary>
		[HttpPost]
		public ActionResult Update(TopoTrailFormModel update)
		{
			var response = _topoTrailService.UpdateTrail(update);

			if (response.OK)
			{
				var model = InitModel(response.Data.Key);
				model.Edit = new TopoTrailFormModel(model.Trail);
				model.ConfirmMessage = $"Trail updated at {DateTime.Now}";
				return View(model);
			}
			else
			{
				var model = InitModel(update.Key);
				model.Edit = update;
				model.AddValidationErrors(response.ValidationErrors);
				return View(model);
			}
		}

		/// <summary>
		/// Extracts tracks from an existing trail for editing
		/// </summary>
		public ActionResult Modify(string id, TrailViewModel.MergeConfirmTypes confirm = TrailViewModel.MergeConfirmTypes.Intent)
		{
			var edits = _trackEditService.ListTracks().Count;

			if (edits > 0)
			{
				if (confirm == TrailViewModel.MergeConfirmTypes.Intent)
				{
					// show confirmation message
					var model = InitModel(id);
					model.ErrorMessages.Add(edits.ToString());
					return View(model);
				}
				else if (confirm == TrailViewModel.MergeConfirmTypes.Discard)
				{
					// discard existing
					_trackEditService.RemoveAll();
				}
			}

			// perform track extracts
			var uri = _topoTrailService.GetTrailUri(id);
			_trackEditService.CreateTracks(uri);

			// cache trail level data
			var trail = _topoTrailService.GetTrail(id);
			_editing = trail;

			// go to track manage page
			return Redirect(TrackViewModel.GetManageUrl());
		}

		/// <summary>
		/// Displays form to update existing or create new trail from track edit data
		/// </summary>
		[HttpGet]
		public ActionResult Import()
		{
			// TODO: REFACTOR: move to service and fix TopoTrail.Track useage
			var model = InitModel();

			model.Trail = _editing;
			model.Edit = new TopoTrailFormModel(_editing);

			// TODO: duplicated in GET + POST
			var tracks = new List<TopoTrackInfo>();
			foreach (var t in _trackEditService.ListTracks())
			{
				var track = new TopoTrackInfo(_editing, t);
				_topoTrailService.UpdateTrackPlaces(track);
				tracks.Add(track);
			}
			model.Tracks = tracks;

			return View(model);
		}

		/// <summary>
		/// Processes update or create import track edit data request
		/// </summary>
		[HttpPost]
		public ActionResult Import(TopoTrailFormModel update)
		{
			// TODO: REFACTOR: move to service and fix TopoTrail.Track useage

			// validate form inputs
			var response = _topoTrailService.ValidateCreate(update);

			if (response.OK)
			{
				var key = "";

				// do create
				if (String.IsNullOrWhiteSpace(update.Key))
				{
					var trail = new TopoTrailInfo(update, null);
					foreach (var t in _trackEditService.ListTracks())
					{
						var track = new TopoTrackInfo(trail, t);
						trail.AddTrack_TODO_DEPRECATE(track);
					}
					key = _topoTrailService.CreateTrail(trail);
				}
				else // do update
				{
					// replace trails on existing track
					var trail = _topoTrailService.GetTrail(update.Key);
					trail.ClearTracks_TODO_DEPRECATE();
					foreach (var t in _trackEditService.ListTracks())
					{
						var track = new TopoTrackInfo(_editing, t);
						trail.AddTrack_TODO_DEPRECATE(track);
					}
					key = _topoTrailService.UpdateTrail(update).Data.Key;
				}

				// show results
				var model = InitModel(key);
				model.Edit = new TopoTrailFormModel(model.Trail);
				model.ConfirmMessage = $"Trail imported at {DateTime.Now}";
				return View(model);
			}
			else
			{
				var model = InitModel();
				model.Trail = _editing;
				model.Edit = update;

				// TODO: duplicated in GET + POST
				var tracks = new List<TopoTrackInfo>();
				foreach (var t in _trackEditService.ListTracks())
				{
					var track = new TopoTrackInfo(_editing, t);
					_topoTrailService.UpdateTrackPlaces(track);
					tracks.Add(track);
				}
				model.Tracks = tracks;

				model.AddValidationErrors(response.ValidationErrors);
				return View(model);
			}
		}

		/// <summary>
		/// Clears the current trail data edit session
		/// </summary>
		public ActionResult Discard()
		{
			_editing = null;

			return Redirect(TrailViewModel.GetImportUrl());
		}
	}
}