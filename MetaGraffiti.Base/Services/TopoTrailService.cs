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
		private static BasicCacheService<TopoTrailInfo2> _trails;
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

				_trails = new BasicCacheService<TopoTrailInfo2>();
				var root = new DirectoryInfo(uri);
				foreach (var dir in root.EnumerateDirectories())
				{
					var country = GeoCountryInfo.ByName(dir.Name);
					if (country != null)
					{
						foreach (var file in dir.EnumerateFiles("*.gpx"))
						{
							var trail = LoadTrail(file);
							_trails.Add(trail);
						}
					}
				}
				_init = true;
			}
		}


		/// <summary>
		/// Lists all loaded GPX files
		/// </summary>
		public List<TopoTrailInfo2> ListTrails()
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
		public TopoTrailInfo2 GetTrail(string id)
		{
			return _trails[id.ToUpperInvariant()];
		}

		public TopoTrackInfo2 FindTrackSource(string uri)
		{
			return _trails.All.SelectMany(x => x.TopoTracks).FirstOrDefault(x => x.Source == Path.GetFileNameWithoutExtension(uri));
		}

		public List<TopoTrailInfo2> ListByDate(int year, int? month = null, int? day = null)
		{
			return Report(new TopoTrailReportRequest() { Year = year, Month = month, Day = day });
		}

		public List<TopoTrailInfo2> ListByCountry(GeoCountryInfo country)
		{
			return Report(new TopoTrailReportRequest() { Country = country.ISO3 });
		}

		public List<TopoTrailInfo2> ListByRegion(GeoRegionInfo region)
		{
			return Report(new TopoTrailReportRequest() { Region = region.RegionISO });
		}

		/*
		public List<TopoTrailInfo> ListByPerimeter(IGeoPerimeter perimeter)
		{
			return null;
		}
		*/

		public List<TopoTrailInfo2> Report(TopoTrailReportRequest request)
		{
			var query = _trails.All.AsQueryable();

			if (request.Year.HasValue) query = query.Where(x => x.StartLocal.Year == request.Year);
			if (request.Month.HasValue) query = query.Where(x => x.StartLocal.Month == request.Month);
			if (request.Day.HasValue) query = query.Where(x => x.StartLocal.Day == request.Day);

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
		private TopoTrailInfo2 LoadTrail(FileInfo file)
		{
			// load gpx data into trail
			var reader = new GpxFileReader(file.FullName);
			var data = reader.ReadFile();
			var trail = new TopoTrailInfo2(data);

			// setup trail details
			var filename = Path.GetFileNameWithoutExtension(file.Name);
			trail.Key = filename.ToUpperInvariant();
			trail.Source = file.FullName;			

			// discover places for each track
			foreach (var track in trail.TopoTracks)
			{
				track.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(track.Points.First()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
				track.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(track.Points.Last()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
			}

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
