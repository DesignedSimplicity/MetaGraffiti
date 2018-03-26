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
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Carto.Info;

namespace MetaGraffiti.Web.Admin.Controllers
{
	/// <summary>
	/// Extracts points from GPX files in order to regroup and export to a new file
	/// </summary>
	public class TrackController : Controller
	{
		// ==================================================
		// Initialization

		private TrackExtractService _trackExtractService = new TrackExtractService();
		private CartoPlaceService _cartoPlaceService;
		private GeoLookupService _geoLookupService;
		private TrailDataService _trailDataService;

		public TrackController()
		{
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
			_geoLookupService = ServiceConfig.GeoLookupService;
			_trailDataService = ServiceConfig.TrailDataService;
		}

		private TrackViewModel InitModel()
		{
			var model = new TrackViewModel();

			model.TrackSourceRoot = new DirectoryInfo(AutoConfig.TrackSourceUri);

			model.TrackGroup = _trackExtractService.GetTrackGroup();
			model.TrackExtracts = _trackExtractService.ListExtracts();

			return model;
		}


		// ==================================================
		// Actions

		public ActionResult Index()
		{
			var model = InitModel();

			return Browse(model.TrackSourceRoot.FullName);
		}

		/// <summary>
		/// Displays a list of GPX files in referenced directory path
		/// </summary>
		public ActionResult Browse(string uri)
		{
			var model = InitModel();

			var path = uri.TrimEnd('\\') + '\\';
			var dir = new DirectoryInfo(path);
			if (!dir.Exists) throw new Exception($"Path {path} does not exist");

			var root = model.TrackSourceRoot.FullName.TrimEnd('\\') + '\\';
			if (!path.StartsWith(root)) throw new Exception($"Path {path} is not in root {root}");

			model.SelectedDirectory = dir;

			model.Sources = new List<TrackFileModel>();
			foreach (var file in dir.GetFiles("*.gpx"))
			{
				var source = InitTrackFileModel(file);
				model.Sources.Add(source);
			}

			return View("Browse", model);
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
		/// Updates metadata for current edit session
		/// </summary>
		[HttpPost]
		public ActionResult Update(TrackUpdateRequest update)
		{
			var model = InitModel();

			_trackExtractService.UpdateTrack(update);
			model.ConfirmMessage = $"Updated at {DateTime.Now}";

			return View("Track", model);
		}

		/// <summary>
		/// Displays the track on a map before extraction
		/// </summary>
		public ActionResult Preview(string uri)
		{
			var model = InitModel();

			// TODO: move this into a service
			if (!uri.Contains(@"\"))
			{
				var year = uri.Substring(0, 4);
				var month = uri.Substring(4, 2);
				var name = uri;
				uri = Path.Combine(AutoConfig.TrackSourceUri, year, month, name + ".gpx");
			}

			var extract = _trackExtractService.PrepareExtract(uri);
			model.SelectedExtract = extract;

			var file = new FileInfo(uri);
			var source = InitTrackFileModel(file);
			model.SelectedSource = source;

			model.SelectedDirectory = file.Directory;

			return View(model);
		}

		/// <summary>
		/// Extracts a given set of points data from an existing GPX file into the current edit session
		/// </summary>
		public ActionResult Extract(TrackExtractCreateRequest extract)
		{
			// TODO: make this HTTPPOST only
			var extracted = _trackExtractService.CreateExtract(extract);

			return Redirect(TrackViewModel.GetEditUrl(extracted.ID));
		}


		/// <summary>
		/// Displays a single set of points from the current edit session
		/// </summary>
		public ActionResult Edit(string id)
		{
			var model = InitModel();

			var extract = _trackExtractService.GetExtract(id);
			UpdateTrackPlaces(extract);
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
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Filter(TrackFilterPointsRequest filter)
		{
			var filtered = _trackExtractService.ApplyFilter(filter);

			if (filtered == null)
			{
				var model = InitModel();

				model.SelectedExtract = _trackExtractService.GetExtract(filter.ID);
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
			var filtered = _trackExtractService.RevertFilter(ID);
			UpdateTrackPlaces(filtered);

			return Redirect(TrackViewModel.GetEditUrl(ID));
		}

		/// <summary>
		/// Filters an existing extract track specified by the ID
		/// </summary>
		[HttpPost]
		public ActionResult Remove(TrackRemovePointsRequest remove)
		{
			var removed = _trackExtractService.RemovePoints(remove);

			return Redirect(TrackViewModel.GetEditUrl(removed.ID));
		}

		/// <summary>
		/// Removes the set of extracted points from the current edit session
		/// </summary>
		public ActionResult Delete(string id)
		{
			var model = InitModel();

			_trackExtractService.DeleteExtract(id);

			return Redirect(TrackViewModel.GetManageUrl());
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


		private void UpdateTrackPlaces(TrackExtractData extract)
		{
			var startPlaces = _cartoPlaceService.ListPlacesByBounds(extract.Points.First());
			var finishPlaces = _cartoPlaceService.ListPlacesByBounds(extract.Points.Last());
			extract.Places = startPlaces.Union(finishPlaces).ToList();
		}


		private TrackFileModel InitTrackFileModel(FileInfo file)
		{
			// TODO: show all possible countries
			// TODO: use track bounds to find places

			var source = new TrackFileModel();
			source.Uri = file.FullName;
			source.FileName = file.Name;
			source.Directory = file.Directory.FullName;

			var existing = _trailDataService.FindTrackSource(file.Name);
			source.Trail = existing?.Trail;

			source.Metadata = _trackExtractService.ReadTrack(file.FullName);
			var data = source.Metadata.Data;

			var points = data.Tracks.SelectMany(x => x.Points);
			var start = points.First();
			var finish = points.Last();
			source.Elapsed = finish.Timestamp.Value.Subtract(start.Timestamp.Value);

			source.Distance = GeoDistance.BetweenPoints(points).KM;

			if (!source.Metadata.Timestamp.HasValue) source.Metadata.Timestamp = start.Timestamp;

			source.Metadata.Country = _geoLookupService.NearestCountry(start);
			source.Metadata.Region = _geoLookupService.NearestRegion(start);
			source.Metadata.Timezone = _geoLookupService.GuessTimezone(source.Metadata.Country);

			source.Regions = _geoLookupService.NearbyRegions(start);
			foreach (var region in _geoLookupService.NearbyRegions(finish))
			{
				if (!source.Regions.Any(x => x.RegionID == region.RegionID)) source.Regions.Add(region);
			}

			var startPlaces = _cartoPlaceService.ListPlacesByBounds(start);
			var finishPlaces = _cartoPlaceService.ListPlacesByBounds(finish);
			source.Places = startPlaces.Union(finishPlaces).ToList();

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
