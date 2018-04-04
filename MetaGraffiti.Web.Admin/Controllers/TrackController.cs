using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Base.Modules;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Web.Admin.Models;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Topo.Info;

namespace MetaGraffiti.Web.Admin.Controllers
{
	/// <summary>
	/// Extracts points from GPX files in order to regroup and export to a new file
	/// </summary>
	public class TrackController : Controller
	{
		// ==================================================
		// Initialization

		private CartoPlaceService _cartoPlaceService;
		private TrackEditService _trackEditService;

		public TrackController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_trackEditService = new TrackEditService();
		}

		private TrackViewModel InitModel()
		{
			var model = new TrackViewModel();

			model.Tracks = _trackEditService.ListTracks();

			return model;
		}

		private TrackEditModel InitEditModel(TrackEditData track)
		{
			var model = new TrackEditModel();
			model.File = new FileInfo(track.Source);
			model.Track = track;			

			var topoTrail = new TopoTrailInfo();
			var first = track.Points.FirstOrDefault();
			var country = Graffiti.Geo.NearestCountry(first);
			if (country != null)
			{
				topoTrail.Country = country;
				if (country.HasRegions) topoTrail.Region = Graffiti.Geo.NearestRegion(first);
				topoTrail.Timezone = Graffiti.Geo.GuessTimezone(country);
			}
			if (topoTrail.Timezone == null) topoTrail.Timezone = GeoTimezoneInfo.UTC;

			// start and finish places
			var topoTrack = new TopoTrackInfo(topoTrail, track);
			topoTrack.StartPlace = _cartoPlaceService.NearestPlace(topoTrack.StartPoint);
			topoTrack.FinishPlace = _cartoPlaceService.NearestPlace(topoTrack.FinishPoint);
			model.TopoTrack = topoTrack;

			// discover additional places
			var bounds = GeoPerimeter.FromPoints(track.Points);
			model.NearbyPlaces = _cartoPlaceService.ListPlacesContainingBounds(bounds);
			model.ContainedPlaces = _cartoPlaceService.ListPlacesContainedInBounds(bounds);

			// default filter values
			var filters = new TrackEditFilter();
			filters.MaximumDilution = track.Points.Max(x => _trackEditService.GetMaxDOP(x));
			filters.MaximumVelocity = track.Points.Max(x => x.Speed ?? 0);
			filters.MinimumSatellite = track.Points.Min(x => x.Sats ?? 0);
			model.Filters = filters;

			return model;
		}

		// TODO: move this into a service
		private string GetSourceUri(string source)
		{
			if (source.Contains(@"\"))
				return source;
			else
			{
				var year = source.Substring(0, 4);
				var month = source.Substring(4, 2);
				var name = source;
				return Path.Combine(AutoConfig.TrackSourceUri, year, month, name + ".gpx");
			}
		}


		// ==================================================
		// Actions

		/// <summary>
		/// Displays all segments in current edit session
		/// </summary>
		public ActionResult Manage()
		{
			var model = InitModel();

			return View(model);
		}

		/// <summary>
		/// Displays the track on a map before extraction
		/// </summary>
		public ActionResult Preview(string source)
		{
			var model = InitModel();

			var uri = GetSourceUri(source);

			var track = _trackEditService.PreviewTrack(uri);
			model.EditTrack = InitEditModel(track);

			return View(model);
		}

		/// <summary>
		/// Creates a track extract for editing
		/// </summary>
		public ActionResult Extract(string source, string name)
		{
			var uri = GetSourceUri(source);

			var track = _trackEditService.CreateTrack(new TrackEditCreateRequest() { Uri = uri, Name = name });

			return Redirect(TrackViewModel.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Displays a single set of points from the current edit session
		/// </summary>
		[HttpGet]
		public ActionResult Modify(string id)
		{
			var model = InitModel();

			var track = _trackEditService.GetTrack(id);
			if (track == null) throw new HttpException(404, $"Track {id} not found!");
			model.EditTrack = InitEditModel(track);			

			return View(model);
		}

		/// <summary>
		/// Updates metadata for current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string key, string name, string description)
		{
			var model = InitModel();

			var track = _trackEditService.GetTrack(key);
			track.Name = TextHelper.GetTrimmed(name);
			track.Description = TextHelper.GetTrimmed(description);

			model.EditTrack = InitEditModel(track);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View(model);
		}

		/// <summary>
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Filter(TrackEditFilterRequest filter)
		{
			var track = _trackEditService.ApplyFilter(filter);

			if (track == null)
			{
				var model = InitModel();

				track = _trackEditService.GetTrack(filter.Key);
				model.ErrorMessages.Add("Filter contains no points and was not applied.");

				model.EditTrack = InitEditModel(track);

				return View("Modify", model);
			}
			else
				return Redirect(TrackViewModel.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Resets the filtered points back to the full initial data set
		/// </summary>
		public ActionResult Revert(string id)
		{
			var track = _trackEditService.RevertFilter(id);

			return Redirect(TrackViewModel.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Remove(TrackEditRemovePointsRequest remove)
		{
			var track = _trackEditService.RemovePoints(remove);

			return Redirect(TrackViewModel.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Removes the set of extracted points from the current edit session
		/// </summary>
		public ActionResult Delete(string id)
		{
			_trackEditService.RemoveTrack(id);

			return Redirect(TrackViewModel.GetManageUrl());
		}

		/// <summary>
		/// Clears all extracts from the current edit session
		/// </summary>
		public ActionResult Reset()
		{
			_trackEditService.RemoveAll();

			return Redirect(TrackViewModel.GetManageUrl());
		}



		







		/// <summary>
		/// Creates an internal file from all of the tracks in the current edit session
		/// </summary>
		public ActionResult Import(bool overwrite = false)
		{
			var model = InitModel();

			// TODO: move this into TrailDataService
			var track = new TrackGroupData(); // _trackExtractService.GetTrackGroup();
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
			//_trackExtractService.WriteTrackFile(uri);

			// reset track extract cache
			//_trackExtractService.ResetSession();

			// reload trails data before redirect
			ServiceConfig.ResetTopoTrail();

			// redirect to new trail page
			return Redirect(TrailViewModel.GetTrailUrl(filename));
		}

		

		

		/// <summary>
		/// Exports all of the tracks in the current edit session to a new file
		/// </summary>
		public ActionResult Export(string format)
		{
			/*
			var data = _trackExtractService.CreateTrackFile(format);
			var name = _trackExtractService.GetTrackGroup().Name;

			return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, $"{name}.{format.ToLowerInvariant()}");
			*/
			return null;
		}
	}
}
