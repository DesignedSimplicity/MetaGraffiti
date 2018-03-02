using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Gpx.Info;

namespace MetaGraffiti.Web.Admin.Services
{
	public class GpxCache
	{
		public GpxFileInfo File { get; private set; }
		public Exception Error { get; private set; }

		public GpxFileMetaData MetaData { get; set; }

		public GpxCache(GpxFileInfo file) { File = file; }
		public GpxCache(Exception error) { Error = error; }
	}

	public class GpxService
	{
		private static List<FileInfo> _gpxFiles = null;
		private static Dictionary<string, GpxCache> _gpxCache = null;

		/// <summary>
		/// Recursively discovers all GPX files located in URI specified
		/// </summary>
		public void Init(string uri)
		{
			if (_gpxFiles == null)
			{
				_gpxFiles = new List<FileInfo>();
				_gpxCache = new Dictionary<string, GpxCache>();
				lock (_gpxFiles)
				{
					var root = new DirectoryInfo(uri);
					foreach(var file in root.EnumerateFiles("*.*", SearchOption.AllDirectories))
					{
						if (file.Extension.ToLowerInvariant() == ".gpx")
						{
							_gpxFiles.Add(file);
						}
					}
				}
			}
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
						try
						{
							var gpx = new GpxFileInfo(file.FullName);
							var cache = new GpxCache(gpx);
							_gpxCache.Add(key, cache);
						}
						catch (Exception ex)
						{
							// load with exception information
							_gpxCache.Add(key, new GpxCache(ex));
						}
					}
					
					// add new or exsting item to list
					list.Add(_gpxCache[key]);
				}
			}
			return list;
		}

		public GpxFileMetaData LoadMetaData(string uri)
		{
			var key = uri.ToLowerInvariant();
			var cache = _gpxCache[key];
			var data = cache.MetaData;
			if (data == null)
			{
				var file = cache.File;

				// copy simple data from file
				data = new GpxFileMetaData();
				data.Name = String.IsNullOrWhiteSpace(file.Name)
					? Path.GetFileNameWithoutExtension(file.Uri)
					: file.Name;
				data.Description = file.Description;

				// calcuate data bounds

				// determine country and region info
				var first = file.Points.First();
				var regions = GeoRegionInfo.ListByLocation(first).OrderByDescending(x => GeoDistance.BetweenPoints(x.Center, first));
				var countries = GeoCountryInfo.ListByLocation(first).OrderByDescending(x => GeoDistance.BetweenPoints(x.Center, first));
				data.Region = regions.FirstOrDefault();
				if (data.Region != null)
				{
					data.Country = data.Region.Country;
					data.LocationName = $"{data.Region.RegionName}, {data.Country.Name}";
				}
				else
				{
					data.Country = countries.FirstOrDefault();
					data.LocationName = data.Country.Name;
				}

				// best guess for timezone
				data.Timezone = GuessTimezone(countries, regions);

				// update cache
				cache.MetaData = data;
			}
			return data;
		}

		public GpxCache SaveMetaData(string uri, GpxFileMetaData data)
		{
			var key = uri.ToLowerInvariant();
			var cache = _gpxCache[key];
			cache.MetaData = data;
			return cache;
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

	public class GpxFileMetaData
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public string LocationName { get; set; }

		public IGeoPerimeter Bounds { get; set; }

		public GeoTimezoneInfo Timezone { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }


		public int? FilterMinSAT { get; set; }
		public double? FilterMaxDOP { get; set; }
		public DateTime? FilterStart { get; set; }
		public DateTime? FilterFinish { get; set; }
	}
}