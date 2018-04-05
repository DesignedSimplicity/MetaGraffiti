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

		private TopoTrailService _topoTrailService;

		public TrailController()
		{
			_topoTrailService = ServiceConfig.TopoTrailService;
		}

		private TrailViewModel InitModel(string id = "")
		{
			var model = new TrailViewModel();

			if (!String.IsNullOrWhiteSpace(id))
			{
				var trail = _topoTrailService.GetTrail(id);
				model.Trail = trail;
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

			model.Edit = new TopoTrailFormModel(model.Trail);

			return View(model);
		}

		/// <summary>
		/// Updates trail profile for current edit session
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
			var trackEditService = new TrackEditService();

			var edits = trackEditService.ListTracks().Count;

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
					trackEditService.RemoveAll();
				}
			}

			// perform extracts
			var trail = _topoTrailService.GetTrail(id);
			foreach(var track in trail.TopoTracks)
			{
				trackEditService.EditTrack(track);
			}

			// go to track manage page
			return Redirect(TrackViewModel.GetManageUrl());			
		}
	}
}