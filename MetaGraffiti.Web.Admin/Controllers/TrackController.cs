using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Web.Admin.Models;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Web.Admin.Controllers
{
	/// <summary>
	/// Extracts points from GPX files in order to regroup and export to a new filed or import into internal storage
	/// </summary>
	public class TrackController : Controller
	{
		private TrackExtractService _service = new TrackExtractService();

		private TrackViewModel InitModel()
		{
			var model = new TrackViewModel();

			model.Track = _service.Track;
			model.Extracts = _service.List();

			return model;
		}

		/// <summary>
		/// Displays all segments in current edit session
		/// </summary>
		public ActionResult Index()
		{
			// TODO: lookup timezone
			// TODO: lookup country
			// TODO: display/lookup region

			var model = InitModel();

			return View("Track", model);
		}

		/// <summary>
		/// Updates metadata for current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Update(TrackUpdateRequest update)
		{
			var model = InitModel();

			_service.Update(update);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View("Track", model);
		}

		/// <summary>
		/// Creates an internal file from all of the tracks in the current edit session
		/// </summary>
		public ActionResult Import(bool overwrite = false)
		{
			// TODO: move this to trail controller

			var model = InitModel();

			var track = _service.Track;
			if (String.IsNullOrWhiteSpace(track.Name)) model.ErrorMessages.Add("Name is missing.");
			if (track.Timezone == null) model.ErrorMessages.Add("Timezone is missing.");
			if (track.Country == null) model.ErrorMessages.Add("Country is missing.");
			// TODO: check region 

			// show error messages if necessary
			if (model.HasError) return View(model);

			// check folders are initialized
			var folder = Path.Combine(AutoConfig.TrackRootUri, track.Country.Name);
			if (!Directory.Exists(AutoConfig.TrackRootUri)) throw new Exception($"TrackRoot not initalized: {AutoConfig.TrackRootUri}");
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			// check existing filename and if overwrite
			var filename = _service.GenerateFilename();
			var uri = Path.Combine(folder, filename + ".gpx");

			// show overwrite confirmation if necessary
			if (System.IO.File.Exists(uri) && !overwrite)
			{
				model.ConfirmMessage = uri;
				return View(model);
			}

			// create internal file
			_service.Import(uri);

			// reset track extract cache
			_service.Reset();

			// reload trails data before redirect
			new TrailDataService().Reset();

			// redirect to new trail page
			return Redirect(TrailViewModel.GetDisplayUrl(filename));
		}

		/// <summary>
		/// Exports all of the tracks in the current edit session to a new file
		/// </summary>
		public ActionResult Export(string format)
		{
			var data = _service.Export(format);
			var name = _service.Track.Name;

			return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, $"{name}.{format.ToLowerInvariant()}");
		}

		/// <summary>
		/// Extracts a given set of points data from an existing GPX file into the current edit session
		/// </summary>
		public ActionResult Extract(TrackExtractCreateRequest extract)
		{
			var extracted = _service.Create(extract);

			return Redirect(TrackViewModel.GetEditUrl(extracted.ID));
		}

		/// <summary>
		/// Displays a single set of points from the current edit session
		/// </summary>
		public ActionResult Edit(string id)
		{
			var model = InitModel();

			model.SelectedExtract = _service.Get(id);

			return View(model);
		}

		/// <summary>
		/// Updates the set of points from the current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Save(TrackExtractUpdateRequest save)
		{
			var model = InitModel();

			model.SelectedExtract = _service.Update(save);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View("Edit", model);
		}

		/// <summary>
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Filter(TrackFilterPointsRequest filter)
		{
			var filtered = _service.Filter(filter);

			if (filtered == null)
			{
				var model = InitModel();

				model.SelectedExtract = _service.Get(filter.ID);
				model.ErrorMessages.Add("Filter contains no points and was not applied.");

				return View("Edit", model);
			}
			else
				return Redirect(TrackViewModel.GetEditUrl(filtered.ID));
		}

		/// <summary>
		/// Resets the filtered points back to the full initial data set
		/// </summary>
		public ActionResult Revert(string ID)
		{
			var filtered = _service.Revert(ID);

			return Redirect(TrackViewModel.GetEditUrl(ID));
		}

		/// <summary>
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Remove(TrackRemovePointsRequest remove)
		{
			var removed = _service.Remove(remove);

			return Redirect(TrackViewModel.GetEditUrl(removed.ID));
		}

		/// <summary>
		/// Removes the set of points from the current edit session
		/// </summary>
		public ActionResult Delete(string id)
		{
			var model = InitModel();

			_service.Delete(id);

			return Redirect(TrackViewModel.GetTrackUrl());
		}

		/// <summary>
		/// Clears all tracks from the current edit session
		/// </summary>
		public ActionResult Reset()
		{
			_service.Reset();

			return Redirect(TrackViewModel.GetTrackUrl());
		}
	}
}
