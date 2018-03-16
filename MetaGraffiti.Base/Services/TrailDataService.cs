using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MetaGraffiti.Base.Services
{
    public class TrailDataService
    {
		private static object _init = false;
		private static BasicCacheService<TopoTrailInfo> _trails;

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
			trail.Name = data.Name; //filename.Substring(9).Trim();
			trail.Description = data.Description;

			// populate info classes
			trail.Country = GeoCountryInfo.ByName(file.Directory.Name);
			trail.Timezone = GeoTimezoneInfo.ByCountry(trail.Country);

			// create track data
			trail.Tracks = new List<TopoTrackInfo>();
			foreach(var track in data.Tracks)
			{
				trail.Tracks.Add(new TopoTrackInfo(trail, track));
			}

			return trail;
		}

		public List<TopoTrailInfo> ListAll()
		{
			return _trails.All;
		}

		public List<GeoCountryInfo> ListCountries()
		{
			var list = new List<GeoCountryInfo>();
			foreach (var id in _trails.All.Select(x => x.Country.CountryID).Distinct())
			{
				list.Add(GeoCountryInfo.ByID(id));
			}
			return list;
		}

		public TopoTrailInfo GetTrail(string id)
		{
			return _trails[id.ToUpperInvariant()];
		}

		public List<TopoTrailInfo> ListByDate(int year, int? month = null, int? day = null)
		{
			return null;
		}

		public List<TopoTrailInfo> ListByCountry(GeoCountryInfo country)
		{
			return _trails.All.Where(x => x.Country.CountryID == country.CountryID).ToList();
		}

		public List<TopoTrailInfo> ListByRegion(GeoRegionInfo region)
		{
			return null;
		}

		public List<TopoTrailInfo> ListByPerimeter(IGeoPerimeter perimeter)
		{
			return null;
		}

		public List<TopoTrailInfo> Report(TrailReportRequest request)
		{
			var query = _trails.All.AsQueryable();

			if (request.Year.HasValue) query = query.Where(x => x.LocalDate.Year == request.Year);
			if (request.Month.HasValue) query = query.Where(x => x.LocalDate.Month == request.Month);
			if (request.Day.HasValue) query = query.Where(x => x.LocalDate.Day == request.Day);

			var country = GeoCountryInfo.Find(request.Country);
			if (country != null) query = query.Where(x => x.Country.CountryID == country.CountryID);

			return query.ToList();
		}

		public void Reload(string uri)
		{
			_trails = null;
			Init(uri);
		}
	}

	public class TrailReportRequest
	{
		public string Country { get; set; }
		public string Region { get; set; }

		public int? Year { get; set; }
		public int? Month { get; set; }
		public int? Day { get; set; }
	}
}
