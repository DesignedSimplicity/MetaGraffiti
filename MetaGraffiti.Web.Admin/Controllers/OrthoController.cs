using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Info;
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
    public class OrthoController : Controller
    {
		// ==================================================
		// Initialization

		private TripSheetService _tripSheetService;
		private CartoPlaceService _cartoPlaceService;

		private TrackExtractService _trackExtractService = new TrackExtractService();
		private TopoTrailService _trailDataService;


		public OrthoController()
		{
			_tripSheetService = ServiceConfig.TripSheetService;
			_trailDataService = ServiceConfig.TopoTrailService;
			_cartoPlaceService = ServiceConfig.CartoPlaceService;
		}

		public ActionResult Index()
		{
			var model = new OrthoViewModel();

			model.Years = _tripSheetService.ListYears();

			return View(model);
		}

		public ActionResult Sheets(string id)
		{
			var model = new OrthoSheetsViewModel();

			model.Sheets = _tripSheetService.ListSheets();
			model.SelectedSheet = id;

			return View(model);
		}

		public ActionResult Places(int? year, string country = "")
		{
			var model = new OrthoPlacesViewModel();

			model.Years = _tripSheetService.ListYears();
			model.SelectedYear = year;

			model.Countries = _tripSheetService.ListCountries();
			model.SelectedCountry = GeoCountryInfo.Find(country);

			var places = new List<OrthoPlaceImportModel>();
			var import = _tripSheetService.ListPlaces(year, country);
			foreach (var data in import)
			{
				var place = new OrthoPlaceImportModel();
				place.Data = data;
				places.Add(place);

				// find existing place by region
				var r = GeoRegionInfo.Find(data.Region);
				if (r != null) place.Place = _cartoPlaceService.FindPlace(r, data.Name, true);

				// find existing place by country
				if (place.Place == null)
				{
					var c = GeoCountryInfo.Find(data.Country);
					if (c != null) place.Place = _cartoPlaceService.FindPlace(c, data.Name, true);
				}
			}
			model.Places = places;

			return View(model);
		}

		public ActionResult Tracks(string path = "")
		{
			var model = new OrthoTracksViewModel();

			model.TrackSourceRoot = new DirectoryInfo(AutoConfig.TrackSourceUri);

			var dir = new DirectoryInfo(Path.Combine(AutoConfig.TrackSourceUri, path));
			if (!dir.Exists) throw new Exception($"Path {dir.FullName} does not exist");

			var root = model.TrackSourceRoot.FullName.TrimEnd('\\') + '\\';
			if (!dir.FullName.StartsWith(root)) throw new Exception($"Path {dir.FullName} is not in root {root}");

			model.SelectedDirectory = dir;

			model.Sources = new List<OrthoTrackImportModel>();
			foreach (var file in dir.GetFiles("*.gpx"))
			{
				var source = InitOrthoTrackImportModel(file);
				model.Sources.Add(source);
			}

			return View(model);
		}

		public ActionResult Reset()
		{
			ServiceConfig.ResetTripSheet();

			return new RedirectResult(OrthoViewModel.GetOrthoUrl());
		}


		private OrthoTrackImportModel InitOrthoTrackImportModel(FileInfo file)
		{
			var model = new OrthoTrackImportModel();

			// init model
			model.File = file;

			// load raw file data
			model.Data = new GpxFileInfo(file.FullName);

			// match with existing trail
			var existing = _trailDataService.FindTrackSource(file.Name);
			model.Trail = existing?.Trail;

			// build trail preview
			var trail = new TopoTrailInfo2();
			model.Preview = trail;

			// find intersecting places
			var points = model.Data.Tracks?.SelectMany(x => x.Points);
			if (points != null)
			{
				var bounds = new GeoPerimeter(points.Select(x => new GeoPosition(x.Latitude, x.Longitude)));
				model.Places = _cartoPlaceService.ListPlacesContainingBounds(bounds).OrderBy(x => x.Bounds.Area).ToList();
				model.Nearby = _cartoPlaceService.ListPlacesContainedInBounds(bounds).OrderBy(x => x.Bounds.Area).ToList();
			}
			
			// set trail information
			var first = points?.FirstOrDefault();
			if (first != null)
			{
				trail.Country = Base.Modules.Geo.Graffiti.Geo.NearestCountry(first);
				trail.Region = Base.Modules.Geo.Graffiti.Geo.NearestRegion(first);
				trail.Timezone = Base.Modules.Geo.Graffiti.Geo.GuessTimezone(trail.Country);
				//trail.StartLocal = trail.Timezone.FromUTC(first.Timestamp.Value);

				// discover all regions
				model.Regions = Base.Modules.Geo.Graffiti.Geo.NearbyRegions(first);
				foreach (var region in Base.Modules.Geo.Graffiti.Geo.NearbyRegions(points.Last()))
				{
					if (!model.Regions.Any(x => x.RegionID == region.RegionID)) model.Regions.Add(region);
				}
			}

			// build track previews
			foreach (var t in model.Data.Tracks)
			{
				var track = new TopoTrackInfo2(trail, t);

				var start = t.Points.First();
				track.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(start).OrderBy(x => x.Bounds.Area).FirstOrDefault();

				var finish = t.Points.Last();
				track.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(finish).OrderBy(x => x.Bounds.Area).FirstOrDefault();				

				trail.AddTrack_TO_DEPRECATE(track);
			}

			return model;
		}
	}
}