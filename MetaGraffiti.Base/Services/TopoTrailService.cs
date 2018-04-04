using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services.Internal;

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
			lock (_init) // TODO: use internal init for basic cache
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
		public TopoTrailInfo GetTrail(string key)
		{
			return _trails[key.ToUpperInvariant()];
		}

		/// <summary>
		/// Locates the track with the given file as the source
		/// </summary>
		public TopoTrackInfo FindTrackSource_TODO(string uri)
		{
			return _trails.All.SelectMany(x => x.TopoTracks).FirstOrDefault(x => x.Source == Path.GetFileNameWithoutExtension(uri));
		}

		/// <summary>
		/// Lists all trails in a country
		/// </summary>
		public List<TopoTrailInfo> ListByCountry(GeoCountryInfo country)
		{
			return Report(new TopoTrailReportRequest() { Country = country.ISO3 });
		}

		/// <summary>
		/// Lists all trals in a region
		/// </summary>
		public List<TopoTrailInfo> ListByRegion(GeoRegionInfo region)
		{
			return Report(new TopoTrailReportRequest() { Region = region.RegionISO });
		}

		/// <summary>
		/// Lists all trails meeting the report request
		/// </summary>
		public List<TopoTrailInfo> Report(TopoTrailReportRequest request)
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

		// TODO: implement this
		public ValidationServiceResponse<TopoTrailInfo> UpdateTrail(TopoTrailUpdateRequest request)
		{
			var response = new ValidationServiceResponse<TopoTrailInfo>(GetTrail(request.Key));
			response.AddError("Field", "Message");
			return response;
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
			// load gpx data into trail
			var reader = new GpxFileReader(file.FullName);
			var data = reader.ReadFile();
			var trail = new TopoTrailInfo(data);

			// setup trail details
			var filename = Path.GetFileNameWithoutExtension(file.Name);
			trail.Key = filename.ToUpperInvariant();
			trail.Source = file.FullName;

			// discover places for each track
			foreach (var track in trail.TopoTracks)
			{
				track.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(track.TopoPoints.First()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
				track.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(track.TopoPoints.Last()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
			}

			return trail;
		}
	}

	// TODO: refactor this to use an interface
	public class TopoTrailReportRequest
	{
		public string Country { get; set; }
		public string Region { get; set; }

		public int? Year { get; set; }
		public int? Month { get; set; }
		public int? Day { get; set; }
	}

	public class TopoTrailUpdateRequest
	{
		public string Key { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public string Keywords { get; set; }

		public string Url { get; set; }
		public string UrlName { get; set; }

		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string Location { get; set; }
	}
}
