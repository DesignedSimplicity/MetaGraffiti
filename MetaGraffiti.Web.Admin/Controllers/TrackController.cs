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

		private TrackEditService _trackEditService = new TrackEditService();


		private TrackExtractService _trackExtractService = new TrackExtractService();
		private CartoPlaceService _cartoPlaceService;
		private TopoTrailService _trailDataService;

		public TrackController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_trailDataService = ServiceConfig.TopoTrailService;
		}

		private TrackViewModel2 InitModel2()
		{
			var model = new TrackViewModel2();

			model.Tracks = _trackEditService.ListTracks();

			return model;
		}

		private TrackViewModel InitModel()
		{
			var model = new TrackViewModel();

			model.TrackGroup = _trackExtractService.GetTrackGroup();
			model.TrackExtracts = _trackExtractService.ListExtracts();

			return model;
		}

		private TrackEditModel InitModel(TrackEditData track)
		{
			var model = new TrackEditModel();
			model.Track = track;

			var topoTrail = new TopoTrailInfo();
			var country = Graffiti.Geo.NearestCountry(track.Points.FirstOrDefault());
			topoTrail.Country = country;
			topoTrail.Timezone = Graffiti.Geo.GuessTimezone(country);

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


		// ==================================================
		// Actions

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

		/// <summary>
		/// Displays the track on a map before extraction
		/// </summary>
		public ActionResult Preview(string source)
		{
			var model = InitModel2();

			var uri = GetSourceUri(source);

			var track = _trackEditService.PreviewTrack(uri);
			model.EditTrack = InitModel(track);

			return View(model);
		}

		public ActionResult Extract(string source, string name)
		{
			var uri = GetSourceUri(source);

			var track = _trackEditService.CreateTrack(new TrackEditCreateRequest() { Uri = uri, Name = name });

			return Redirect(TrackViewModel2.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Displays a single set of points from the current edit session
		/// </summary>
		[HttpGet]
		public ActionResult Modify(string id)
		{
			var model = InitModel2();

			var track = _trackEditService.GetTrack(id);
			if (track == null) throw new HttpException(404, $"Track {id} not found!");
			model.EditTrack = InitModel(track);			

			return View(model);
		}

		/// <summary>
		/// Updates metadata for current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Modify(string key, string name, string description)
		{
			var model = InitModel2();

			var track = _trackEditService.GetTrack(key);
			track.Name = name;
			track.Description = description;

			model.EditTrack = InitModel(track);
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
				var model = InitModel2();

				track = _trackEditService.GetTrack(filter.Key);
				model.ErrorMessages.Add("Filter contains no points and was not applied.");

				model.EditTrack = InitModel(track);

				return View("Modify", model);
			}
			else
				return Redirect(TrackViewModel2.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Resets the filtered points back to the full initial data set
		/// </summary>
		public ActionResult Revert(string id)
		{
			var track = _trackEditService.RevertFilter(id);

			return Redirect(TrackViewModel2.GetModifyUrl(track.Key));
		}

		/// <summary>
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Remove(TrackEditRemovePointsRequest remove)
		{
			var track = _trackEditService.RemovePoints(remove);

			return Redirect(TrackViewModel2.GetModifyUrl(track.Key));
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
		/// Displays all segments in current edit session
		/// </summary>
		public ActionResult Manage()
		{
			var model = InitModel();

			return View("Track", model);
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
			ServiceConfig.ResetTopoTrail();

			// redirect to new trail page
			return Redirect(TopoViewModel.GetTrailUrl(filename));
		}

		

		

		/// <summary>
		/// Displays a single set of points from the current edit session
		/// </summary>
		public ActionResult Edit(string id)
		{
			var model = InitModel();

			var extract = _trackExtractService.GetExtract(id);
			//UpdateTrackPlaces(extract);
			model.SelectedExtract = extract;

			return View(model);
		}

		/// <summary>
		/// Updates the set of points from the current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Save(TrackExtractUpdateRequest save)
		{
			var model = InitModel();

			model.SelectedExtract = _trackExtractService.UpdateExtract(save);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View("Edit", model);
		}

		

		/// <summary>
		/// Exports all of the tracks in the current edit session to a new file
		/// </summary>
		public ActionResult Export(string format)
		{
			var data = _trackExtractService.CreateTrackFile(format);
			var name = _trackExtractService.GetTrackGroup().Name;

			return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, $"{name}.{format.ToLowerInvariant()}");
		}

		/// <summary>
		/// Clears all extracts from the current edit session
		/// </summary>
		public ActionResult Reset()
		{
			_trackExtractService.ResetSession();

			return Redirect(TrackViewModel.GetManageUrl());
		}


		private void UpdateTrackPlaces(TrackExtractData extract, TrackFileModel source)
		{
			// TODO: update this
			// *********************************************
			var points = extract.Points;
			var bounds = new GeoPerimeter(points.Select(x => new GeoPosition(x.Latitude, x.Longitude)));
			source.Places = _cartoPlaceService.ListPlacesContainingBounds(bounds).OrderBy(x => x.Bounds.Area).ToList();
			source.NearbyPlaces = _cartoPlaceService.ListPlacesContainedInBounds(bounds).OrderBy(x => x.Bounds.Area).ToList();
			source.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(points.First()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
			source.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(points.Last()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
		}


		private TrackFileModel InitTrackFileModel(FileInfo file)
		{
			// TODO: show all possible countries
			// TODO: use track bounds to find places

			var source = new TrackFileModel();
			source.Uri = file.FullName;
			source.FileName = file.Name;
			source.Directory = file.Directory.FullName;

			//TODO: fix var existing = _trailDataService.FindTrackSource(file.Name);
			//TODO: fix source.Trail = existing?.Trail;

			source.Metadata = _trackExtractService.ReadTrail(file.FullName);
			var data = source.Metadata.Data;

			var points = data.Tracks.SelectMany(x => x.PointData);
			var start = points.First();
			var finish = points.Last();
			source.Elapsed = finish.Timestamp.Value.Subtract(start.Timestamp.Value);

			source.Distance = GeoDistance.BetweenPoints(points).KM;

			if (!source.Metadata.Timestamp.HasValue) source.Metadata.Timestamp = start.Timestamp;

			source.Metadata.Country = Graffiti.Geo.NearestCountry(start);
			source.Metadata.Region = Graffiti.Geo.NearestRegion(start);
			source.Metadata.Timezone = Graffiti.Geo.GuessTimezone(source.Metadata.Country);

			source.Regions = Graffiti.Geo.NearbyRegions(start);
			foreach (var region in Graffiti.Geo.NearbyRegions(finish))
			{
				if (!source.Regions.Any(x => x.RegionID == region.RegionID)) source.Regions.Add(region);
			}

			//var startPlaces = _cartoPlaceService.ListPlacesByBounds(start);
			//var finishPlaces = _cartoPlaceService.ListPlacesByBounds(finish);			
			//source.Places = startPlaces.Union(finishPlaces).ToList();
			var bounds = new GeoPerimeter(points.Select(x => new GeoPosition(x.Latitude, x.Longitude)));
			source.Places = _cartoPlaceService.ListPlacesContainingBounds(bounds).OrderBy(x => x.Bounds.Area).ToList();
			source.NearbyPlaces = _cartoPlaceService.ListPlacesContainedInBounds(bounds).OrderBy(x => x.Bounds.Area).ToList();
			source.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(start).OrderBy(x => x.Bounds.Area).FirstOrDefault();
			source.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(finish).OrderBy(x => x.Bounds.Area).FirstOrDefault();


			var speed = source.Distance / source.Elapsed.TotalHours;
			source.IsWalk = speed <= 5; // km/h
			source.IsBike = speed > 5 && speed < 15; // km/h

			var max = points.Max(x => x.Speed ?? 0);
			source.IsFast = max > 5; // m/s

			var loop = GeoDistance.BetweenPoints(start, finish).Meters;
			source.IsLoop = loop < 200;

			var bad = max > 33; // m/s
			if (!bad) bad = points.Average(x => (x.Sats ?? 0)) < 5;
			if (!bad) bad = points.Count(x => !x.HDOP.HasValue) > 20; // TODO: fix this threshold for old files without DOP data
			source.IsBad = bad;

			source.IsShort = points.Count() < 30;

			return source;
		}
	}
}
