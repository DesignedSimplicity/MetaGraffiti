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
	// gpx/							GET displays a summary of the GPX source files
	// gpx/import/					GET displays a list of all GPX source files which have not yet been imported
	// gpx/report/?					GET displays a detailed and filterable report and include links to imported files
	// gpx/preview/{id}				GET displays a preview of a given GPX file on a map and provides actions to extract point data


	// track/						GET Displays all segments in current edit session
	// track/reset/					GET Clears the current cache of tracks extracts
	// track/update/				POST Updates metadata for current edit session
	// track/import/				POST saves current edit session into known location/format
	// track/export/				POST creates GPX or KML file for download from current edit session
	// track/filter/				POST Filters an existing extract track specified by the ID
	// track/revert/				POST Resets the filtered points back to the full initial data set
	// track/extract/				POST Extracts a given set of points data from an existing GPX file into the current edit session
	// track/remove/				POST Removes the given list of points from the current edit session
	// track/edit/{id}				GET Displays a single set of points from the current edit session
	// track/save/{id}				POST Updates the set of points from the current edit session
	// track/delete/{id}			POST Removes the set of points from the current edit session


	
	// TODO: lookup timezone
	// TODO: lookup country

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
			var model = InitModel();

			var track = _service.Track;
			if (String.IsNullOrWhiteSpace(track.Name)) model.ErrorMessages.Add("Name is missing.");
			if (track.Timezone == null) model.ErrorMessages.Add("Timezone is missing.");
			if (track.Country == null) model.ErrorMessages.Add("Country is missing.");
			// TODO: check region 

			if (model.HasError) return View(model);

			// check existing filename and if overwrite
			var filename = _service.GenerateFilename();
			var uri = Path.Combine(AutoConfig.TrackRootUri, track.Country.Name, filename + ".gpx");

			if (System.IO.File.Exists(uri) && !overwrite)
			{
				model.ConfirmMessage = uri;
				return View(model);
			}

			// do import
			_service.Import(uri);

			// TODO: redirect to display page
			return Redirect(TrackViewModel.GetTrackUrl());
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
