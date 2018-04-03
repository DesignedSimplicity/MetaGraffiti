using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MetaGraffiti.Base.Services
{
    public class TopoTrailService
    {
		// ==================================================
		// Internals
		private static object _init = false;
		private static BasicCacheService<TopoTrailInfo> _trails;
		private CartoPlaceService _cartoPlaceService;


		public TopoTrailService(CartoPlaceService cartoPlaceService)
		{
			_cartoPlaceService = cartoPlaceService;
		}


		// ==================================================
		// Methods

		/// <summary>
		/// Initializes repository with all local GPX files
		/// </summary>
		public void Init(string uri)
		{
			lock (_init)
			{
				if (Convert.ToBoolean(_init)) return;

				_trails = new BasicCacheService<TopoTrailInfo>();
				var root = new DirectoryInfo(uri);
				foreach (var dir in root.EnumerateDirectories())
				{
					var country = GeoCountryInfo.ByName(dir.Name);
					if (country != null)
					{
						foreach (var file in dir.EnumerateFiles("*.gpx"))
						{
							var trail = LoadTrail(file);
							_trails.Add(trail.ID.ToUpperInvariant(), trail);
						}
					}
				}
				_init = true;
			}
		}


		/// <summary>
		/// Lists all loaded GPX files
		/// </summary>
		public List<TopoTrailInfo> ListTrails()
		{
			return _trails.All;
		}

		/// <summary>
		/// Lists all countries referenced across all GPX files
		/// </summary>
		public List<GeoCountryInfo> ListCountries()
		{
			var list = new List<GeoCountryInfo>();
			foreach (var id in _trails.All.Select(x => x.Country.CountryID).Distinct())
			{
				list.Add(GeoCountryInfo.ByID(id));
			}
			return list;
		}

		/// <summary>
		/// Retrieves a specific GPX file from the cache
		/// </summary>
		public TopoTrailInfo GetTrail(string id)
		{
			return _trails[id.ToUpperInvariant()];
		}

		public TopoTrackInfo FindTrackSource(string uri)
		{
			return _trails.All.SelectMany(x => x.Tracks).FirstOrDefault(x => x.Source == Path.GetFileNameWithoutExtension(uri));
		}

		public List<TopoTrailInfo> ListByDate(int year, int? month = null, int? day = null)
		{
			return Report(new TopoTrailReportRequest() { Year = year, Month = month, Day = day });
		}

		public List<TopoTrailInfo> ListByCountry(GeoCountryInfo country)
		{
			return Report(new TopoTrailReportRequest() { Country = country.ISO3 });
		}

		public List<TopoTrailInfo> ListByRegion(GeoRegionInfo region)
		{
			return Report(new TopoTrailReportRequest() { Region = region.RegionISO });
		}

		/*
		public List<TopoTrailInfo> ListByPerimeter(IGeoPerimeter perimeter)
		{
			return null;
		}
		*/

		public List<TopoTrailInfo> Report(TopoTrailReportRequest request)
		{
			var query = _trails.All.AsQueryable();

			if (request.Year.HasValue) query = query.Where(x => x.LocalDate.Year == request.Year);
			if (request.Month.HasValue) query = query.Where(x => x.LocalDate.Month == request.Month);
			if (request.Day.HasValue) query = query.Where(x => x.LocalDate.Day == request.Day);

			if (!String.IsNullOrWhiteSpace(request.Country))
			{
				var country = GeoCountryInfo.Find(request.Country);
				if (country != null) query = query.Where(x => x.Country.CountryID == country.CountryID);
			}

			if (!String.IsNullOrWhiteSpace(request.Region))
			{
				var region = GeoRegionInfo.Find(request.Region);
				if (region != null) query = query.Where(x => x.Region != null && x.Region.RegionID == region.RegionID);
			}

			return query.ToList();
		}



		/// <summary>
		/// Clears the GPX file cache
		/// </summary>
		public void ResetCache()
		{
			_trails = null;
			_init = false;
		}

		// ==================================================
		// Helpers
		private TopoTrailInfo LoadTrail(FileInfo file)
		{
			// initial topo trail setup
			var trail = new TopoTrailInfo();
			var filename = Path.GetFileNameWithoutExtension(file.Name);
			trail.ID = filename.ToUpperInvariant();
			trail.Uri = file.FullName;

			// setup local timestamps
			var datetime = filename.Substring(0, 8);
			var timestamp = DateTime.ParseExact(datetime, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
			trail.LocalDate = DateTime.SpecifyKind(timestamp, DateTimeKind.Unspecified);

			// load gpx file data
			var reader = new GpxFileReader(file.FullName);
			var data = reader.ReadFile();

			// load primary information
			trail.Name = data.Name;
			trail.Description = data.Description;

			// load secondary information
			trail.Url = data.Url;
			trail.UrlName = data.UrlName;
			trail.Keywords = data.Keywords;

			// populate defaults
			trail.Country = GeoCountryInfo.ByName(file.Directory.Name);
			trail.Timezone = GeoTimezoneInfo.ByKey("UTC");

			// process custom info
			var extensions = reader.ReadExtension();
			if (!String.IsNullOrWhiteSpace(extensions.Timezone)) trail.Timezone = GeoTimezoneInfo.ByKey(extensions.Timezone);
			if (!String.IsNullOrWhiteSpace(extensions.Country)) trail.Country = GeoCountryInfo.Find(extensions.Country);
			if (!String.IsNullOrWhiteSpace(extensions.Region)) trail.Region = GeoRegionInfo.Find(extensions.Region);
			trail.Location = extensions.Location;

			// create track data
			var places = new List<CartoPlaceInfo>();
			foreach (var track in data.Tracks.OrderBy(x => x.Points.First().Timestamp.Value))
			{
				var tt = new TopoTrackInfo(trail, track);

				var f = track.Points.First();
				tt.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(f).OrderBy(x => x.Bounds.Area).FirstOrDefault();
				if (tt.StartPlace != null && !places.Any(x => x.Key == tt.StartPlace.Key)) places.Add(tt.StartPlace);

				var l = track.Points.Last();
				tt.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(l).OrderBy(x => x.Bounds.Area).FirstOrDefault();
				if (tt.FinishPlace != null && !places.Any(x => x.Key == tt.FinishPlace.Key)) places.Add(tt.FinishPlace);

				trail.Tracks.Add(tt);
			}

			// consolidate carto place information
			trail.ViaPlaces = places;

			// built trail
			return trail;
		}
	}

	public class TopoTrailReportRequest
	{
		public string Country { get; set; }
		public string Region { get; set; }

		public int? Year { get; set; }
		public int? Month { get; set; }
		public int? Day { get; set; }
	}
}
