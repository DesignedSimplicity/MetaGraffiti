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

namespace MetaGraffiti.Web.Admin.Controllers
{
	/// <summary>
	/// Extracts points from GPX files in order to regroup and export to a new file
	/// </summary>
	public class TrackController : Controller
	{
		private TrackExtractService _trackExtractService = new TrackExtractService();
		private GeoLookupService _geoLookupService = new GeoLookupService(null);

		private TrackViewModel InitModel()
		{
			var model = new TrackViewModel();

			model.Track = _trackExtractService.Track;
			model.Extracts = _trackExtractService.ListExtracts();

			return model;
		}

		/// <summary>
		/// 
		/// </summary>
		public ActionResult Index()
		{
			var model = InitModel();

			return View(model);
		}

		/// <summary>
		/// Displays a list of GPX files in referenced directory path
		/// </summary>
		public ActionResult Browse(string uri)
		{
			var model = InitModel();

			var dir = new DirectoryInfo(uri);
			model.Directory = dir;
			if (!dir.Exists)
			{
				model.ErrorMessages.Add($"Directory does not exist! {uri}");
				return View(model);
			}

			model.Sources = new List<TrackFileModel>();
			foreach (var file in dir.GetFiles("*.gpx"))
			{
				var source = new TrackFileModel();
				source.Uri = file.FullName;
				source.FileName = file.Name;
				source.Directory = dir.FullName;

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
				foreach(var region in _geoLookupService.NearbyRegions(finish))
				{
					if (!source.Regions.Any(x => x.RegionID == region.RegionID)) source.Regions.Add(region);
				}

				source.IsWalk = ((source.Distance / source.Elapsed.TotalHours) < 10);
				source.IsLoop = (GeoDistance.BetweenPoints(start, finish).Meters < 200);

				model.Sources.Add(source);
			}

			return View(model);
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
				uri = Path.Combine(AutoConfig.SourceRootUri, year, month, name + ".gpx");
			}

			var extract = _trackExtractService.PrepareExtract(uri);
			model.SelectedExtract = extract;

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

			model.SelectedExtract = _trackExtractService.GetExtract(id);

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
			var name = _trackExtractService.Track.Name;

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
	}
}
