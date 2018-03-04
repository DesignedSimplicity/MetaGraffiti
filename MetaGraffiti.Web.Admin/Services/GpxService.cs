using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Gpx;
using MetaGraffiti.Base.Modules.Gpx.Data;
using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Base.Modules.Kml;

namespace MetaGraffiti.Web.Admin.Services
{
	public class GpxService
	{
		private static List<FileInfo> _gpxFiles = null;
		private static Dictionary<string, GpxCache> _gpxCache = null;

		/// <summary>
		/// Recursively discovers all GPX files located in URI specified
		/// </summary>
		public List<FileInfo> Init(string uri)
		{
			if (_gpxFiles == null)
			{
				_gpxFiles = new List<FileInfo>();
				_gpxCache = new Dictionary<string, GpxCache>();
				lock (_gpxFiles)
				{
					var root = new DirectoryInfo(uri);
					foreach (var file in root.EnumerateFiles("*.*", SearchOption.AllDirectories))
					{
						if (file.Extension.ToLowerInvariant() == ".gpx")
						{
							_gpxFiles.Add(file);
						}
					}
				}
			}
			return _gpxFiles;
		}

		/// <summary>
		/// Loads and parses all of the files in the given directory
		/// </summary>
		public List<GpxCache> LoadDirectory(string uri, bool recursive = false)
		{
			// list matching files from cache
			var start = new DirectoryInfo(uri).FullName;
			var files = _gpxFiles.AsQueryable();
			if (recursive)
			{
				if (!start.EndsWith(Path.DirectorySeparatorChar.ToString())) start += Path.DirectorySeparatorChar;
				files = files.Where(x => x.FullName.StartsWith(start, StringComparison.InvariantCultureIgnoreCase));
			}
			else
				files = files.Where(x => String.Compare(x.Directory.FullName, start, true) == 0);

			// enumerate each file and load if necessary
			var list = new List<GpxCache>();
			lock (_gpxCache)
			{
				foreach (var file in files)
				{
					var key = file.FullName.ToLowerInvariant();
					if (!_gpxCache.ContainsKey(key))
					{
						var gpx = new GpxFileInfo(file.FullName);
						var cache = new GpxCache(gpx);
						if (gpx.Valid) InitMetaData(cache);
						_gpxCache.Add(key, cache);
					}

					// add new or exsting item to list
					list.Add(_gpxCache[key]);
				}
			}
			return list;
		}

		/// <summary>
		/// Loads and caches the requested file and metadata
		/// </summary>
		public GpxCache LoadFile(string uri)
		{
			var key = uri.ToLowerInvariant();
			if (!_gpxCache.ContainsKey(key)) return null;
			var cache = _gpxCache[key];
			if (cache.MetaData == null) InitMetaData(cache);
			return cache;
		}



		/*
		public GpxFileMetaData LoadMetaData(string uri)
		{
			var key = uri.ToLowerInvariant();
			var cache = _gpxCache[key];
			if (cache.MetaData == null) InitMetaData(cache);
			return cache.MetaData;
		}
		*/

		public GpxCache UpdateMetaData(string uri, GpxUpdateData update)
		{
			var key = uri.ToLowerInvariant();
			var cache = _gpxCache[key];
			var data = cache.MetaData;

			data.Name = update.Name;
			data.Description = update.Description;
			data.LocationName = update.LocationName;

			//TODO: deal with changes to country/region
			//TODO: deal with timezone/recalcuating local time

			return cache;
		}

		public GpxCache UpdateFilters(string uri, GpxFilterData filter)
		{
			var key = uri.ToLowerInvariant();
			var cache = _gpxCache[key];

			cache.Filter = filter;

			return cache;
		}



		public List<GpxPointData> FilterPoints(IEnumerable<GpxPointData> points, GpxFilterData filter)
		{
			if (filter.FilterStart.HasValue) points = points.Where(x => x.Timestamp >= filter.FilterStart.Value);
			if (filter.FilterFinish.HasValue) points = points.Where(x => x.Timestamp <= filter.FilterFinish.Value);
			if ((filter.FilterDOP ?? 0) > 0) points = points.Where(x => x.MaxDOP <= filter.FilterDOP.Value);
			if ((filter.FilterGPS ?? 0) > 0) points = points.Where(x => x.Sats <= filter.FilterGPS.Value);
			return points.ToList();
		}


		public byte[] ExportGpxFile(GpxCacheMetaData metadata, IEnumerable<GpxTrackData> tracks, GpxFilterData filter = null)
		{
			var writer = new GpxFileWriter();
			writer.WriteHeader(metadata.Name, metadata.Description);
			foreach (var track in tracks)
			{
				var points = (filter == null ? track.Points : FilterPoints(track.Points, filter));
				writer.WriteTrack(track.Name, track.Description, points);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}

		public byte[] ExportKmlFile(GpxCacheMetaData metadata, IEnumerable<GpxTrackData> tracks, GpxFilterData filter = null)
		{
			var writer = new KmlFileWriter();
			writer.WriteHeader(metadata.Name, metadata.Description);
			foreach (var track in tracks)
			{
				var points = (filter == null ? track.Points : FilterPoints(track.Points, filter));
				writer.WriteTrack(track.Name, track.Description, points);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}



		private void InitMetaData(GpxCache cache)
		{
			var file = cache.File;

			// copy simple data from file
			var data = new GpxCacheMetaData();
			data.Uri = file.Uri;
			data.Name = String.IsNullOrWhiteSpace(file.Name)
				? Path.GetFileNameWithoutExtension(file.Uri)
				: file.Name;
			data.Description = file.Description;
			data.Timestamp = file.Points.First().Timestamp.Value;

			// calcuate geo perimiter

			// determine country and region info
			var first = file.Points.First();
			var regions = GeoRegionInfo.ListByLocation(first); //TODO: order by distance 
			var countries = GeoCountryInfo.ListByLocation(first); //TODO: order by distance .OrderByDescending(x => GeoDistance.BetweenPoints(x.Center, first));
			data.Region = regions.FirstOrDefault();
			if (data.Region != null)
			{
				data.Country = data.Region.Country;
				data.LocationName = $"{data.Region.RegionName}, {data.Country.Name}";
			}
			else
			{
				data.Country = countries.FirstOrDefault();
				if (data.Country != null) data.LocationName = data.Country.Name;
			}

			// best guess for timezone
			data.Timezone = GuessTimezone(countries, regions);
			data.LocalTime = data.Timezone.FromUTC(first.Timestamp.Value);

			// update cache
			cache.MetaData = data;
		}








		private GeoTimezoneInfo GuessTimezone(IEnumerable<GeoCountryInfo> countries, IEnumerable<GeoRegionInfo> regions)
		{
			string[] countryOrder = { "UR", "CL", "AR", "AU", "BE", "BR", "CH", "CN", "DK", "FR", "HK", "IN", "IS", "JP", "MN", "NL", "NZ", "RU", "SG", "CA", "MX", "JM", "AN" };
			GeoCountryInfo country = null;

			var regionCountries = regions.Select(x => x.Country).Distinct().Count();
			if (regionCountries == 1) // 1 country with multiple regions
				country = regions.First().Country;
			else if (countries.Count() == 1) // 1 country without regions
				country = countries.First();
			else // multiple countries
			{
				foreach(var c in countryOrder)
				{
					// pick first visited country
					if (countries.Any(x => x.ISO2 == c))
					{
						country = GeoCountryInfo.ByISO(c);
						break;
					}
				}
			}

			// now pick timezone
			if (country == null)
				return GeoTimezoneInfo.LocalTimezone;

			// simple country level
			switch (country.ISO2)
			{
				case "AR": return GeoTimezoneInfo.ByKey("Argentina");
				case "CL": return GeoTimezoneInfo.ByKey("Pacific SA");
				case "NZ": return GeoTimezoneInfo.ByKey("New Zealand");
				case "JP": return GeoTimezoneInfo.ByKey("Tokyo");
				case "SG": return GeoTimezoneInfo.ByKey("Singapore");
			}

			// assume europe is single timezone
			if (country.Continent == GeoContinents.Europe)
				return GeoTimezoneInfo.ByKey("W. Europe");

			// break australia down into regions
			if (country.ISO2 == "AU")
			{
				if (regions.Any(x => x.RegionName == "Tasmania"))
					return GeoTimezoneInfo.ByKey("Tasmania");
				else if (regions.Any(x => x.RegionName == "Western Australia"))
					return GeoTimezoneInfo.ByKey("W. Australia");
				else if (regions.Any(x => x.RegionName == "New South Wales"))
					return GeoTimezoneInfo.ByKey("AUS Eastern");
				else if (regions.Any(x => x.RegionName == "Queensland"))
					return GeoTimezoneInfo.ByKey("AUS Eastern");
			}

			// default to UTC
			return GeoTimezoneInfo.ByKey("UTC");
		}
	}

	public class GpxCache
	{
		public bool IsCached { get { return File != null && MetaData != null; } }

		public GpxFileInfo File { get; private set; }
		public GpxCacheMetaData MetaData { get; set; }

		public GpxFilterData Filter { get; set; } = new GpxFilterData();


		public GpxCache(GpxFileInfo file) { File = file; }
	}

	public class GpxUpdateData
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string LocationName { get; set; }
	}

	public class GpxFilterData
	{
		public int? FilterGPS { get; set; }
		public decimal? FilterDOP { get; set; }
		public DateTime? FilterStart { get; set; }
		public DateTime? FilterFinish { get; set; }
	}

	public class GpxCacheMetaData
	{
		public string Uri { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public DateTime Timestamp { get; set; }
		public DateTime LocalTime { get; set; }

		public string LocationName { get; set; }


		public GeoTimezoneInfo Timezone { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
	}
}