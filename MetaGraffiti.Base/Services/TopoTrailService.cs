﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services.Internal;

namespace MetaGraffiti.Base.Services
{
    public class TopoTrailService
    {
		// ==================================================
		// Internals
		private static string _rootUri = "";
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
				_rootUri = root.FullName;
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
		/// Builds the full path for a specific GPX file
		/// </summary>
		public string GetTrailUri(string key)
		{
			return GetFilename(GetTrail(key));
		}

		/// <summary>
		/// Locates the track with the given file as the source
		/// </summary>
		public TopoTrackInfo FindTrackSource_TODO(string uri)
		{
			var key = GetSourceNameKey(uri);
			return _trails.All.SelectMany(x => x.TopoTracks).FirstOrDefault(x => GetSourceNameKey(x.Source) == key);
		}
		private string GetSourceNameKey(string source)
		{
			var index = source.LastIndexOf(@"\");
			if (index > 0) source = source.Substring(index + 1);
			index = source.LastIndexOf(".");
			if (index > 0) source = source.Substring(0, index);
			//System.Diagnostics.Debug.WriteLine(source);
			return source.Trim();
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

		/// <summary>
		/// Updates the start and finish places on the given track
		/// </summary>
		public List<CartoPlaceInfo> UpdateTrackPlaces(TopoTrackInfo track)
		{
			var places = new List<CartoPlaceInfo>();
			track.StartPlace = _cartoPlaceService.ListPlacesByContainingPoint(track.TopoPoints.First()).OrderBy(x => x.Bounds.Area).FirstOrDefault();
			track.FinishPlace = _cartoPlaceService.ListPlacesByContainingPoint(track.TopoPoints.Last()).OrderBy(x => x.Bounds.Area).FirstOrDefault();

			if (track.StartPlace != null) places.Add(track.StartPlace);
			if (track.FinishPlace != null && (!places.Any(x => x.Key == track.FinishPlace.Key))) places.Add(track.FinishPlace);

			return places;
		}


		/// <summary>
		/// Validates the request to create a new trail
		/// </summary>
		public ValidationServiceResponse<TopoTrailInfo> ValidateCreate(ITopoTrailUpdateRequest request)
		{
			// load existing trail
			var response = new ValidationServiceResponse<TopoTrailInfo>(null);

			// do basic validation
			if (String.IsNullOrWhiteSpace(request.Name)) response.AddError("Name", "Name is required!");

			// TODO: REFACTOR: use GeoPolitical
			var timezone = GeoTimezoneInfo.Find(request.Timezone);
			if (timezone == null) response.AddError("Timezone", "Timezone is missing or invalid!");

			var country = GeoCountryInfo.Find(request.Country);
			if (country == null) response.AddError("Country", "Country is missing or invalid!");

			var region = GeoRegionInfo.Find(request.Region);
			if (region == null && !String.IsNullOrWhiteSpace(request.Region)) response.AddError("Region", "Region is invalid!");

			return response;
		}

		/// <summary>
		/// Create the trail metadata, file and cache
		/// </summary>
		public string CreateTrail(TopoTrailInfo trail)
		{
			var response = new ValidationServiceResponse<TopoTrailInfo>(trail);

			// check file system
			if (!Directory.Exists(_rootUri)) throw new Exception($"Directory not initalized: {_rootUri}");
			var folder = Path.Combine(_rootUri, trail.Country.Name);
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			// generate new and rename/remove existing files
			var filename = GetFilename(trail);

			// check if overwrite file
			if (File.Exists(filename)) throw new Exception($"{filename} already exists!");

			// write new file
			var contents = BuildGpx(trail);
			File.WriteAllText(filename, contents);

			// add new file to cache
			trail.Key = Path.GetFileNameWithoutExtension(filename).ToUpperInvariant();
			_trails.Add(trail);

			// return new key
			return trail.Key;
		}

		/// <summary>
		/// Validates the request to update an existing trail
		/// </summary>
		public ValidationServiceResponse<TopoTrailInfo> ValidateUpdate(ITopoTrailUpdateRequest request)
		{
			// validate basic request first
			var response = ValidateCreate(request);
			if (response.HasErrors) return response;

			// load existing trail
			var trail = GetTrail(request.Key);
			if (trail == null) response.AddError("Trail", $"Trail {request.Key} does not exist!");

			// fresh result with trail attached
			return new ValidationServiceResponse<TopoTrailInfo>(trail);
		}

		/// <summary>
		/// Updates the trail metadata and synchronizes file and cache
		/// </summary>
		public ValidationServiceResponse<TopoTrailInfo> UpdateTrail(ITopoTrailUpdateRequest request)
		{
			// validate update request
			var response = ValidateUpdate(request);
			if (response.HasErrors) return response;

			// process update request
			var trail = response.Data;

			// save current filename for later
			var existing = GetFilename(trail);

			// check file system
			if (!Directory.Exists(_rootUri)) throw new Exception($"Directory not initalized: {_rootUri}");
			var folder = Path.Combine(_rootUri, trail.Country.Name);
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			// update trail properties
			trail.Timezone = GeoTimezoneInfo.Find(request.Timezone);
			trail.Country = GeoCountryInfo.Find(request.Country);
			trail.Region = GeoRegionInfo.Find(request.Region);

			trail.Name = TextMutate.TrimSafe(request.Name);
			trail.Description = TextMutate.TrimSafe(request.Description);
			trail.Location = TextMutate.TrimSafe(request.Location);
			trail.Keywords = TextMutate.FixKeywords(request.Keywords);

			// TODO: BUG: fix url writing for v1.1 files!
			//trail.UrlLink = TextMutate.FixUrl(request.UrlLink);
			//trail.UrlText = TextMutate.TrimSafe(request.UrlText);

			// generate new and rename/remove existing files
			var filename = GetFilename(trail);

			// check if overwrite file
			var renamed = String.Compare(existing, filename, true) != 0;
			if (!renamed)
			{
				// temp rename current file
				File.Move(existing, existing + "~temp");
				existing = existing + "~temp";
			}

			// write new file
			var contents = BuildGpx(trail);
			File.WriteAllText(filename, contents);

			// delete old file
			File.Delete(existing);

			// refresh key and cache data
			if (renamed)
			{
				trail.Key = Path.GetFileNameWithoutExtension(filename).ToUpperInvariant();
				_trails.Remove(request.Key.ToUpperInvariant());
				_trails.Add(trail);
			}

			// return successful response
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
				UpdateTrackPlaces(track);
			}

			return trail;
		}

		private string GetFilename(TopoTrailInfo trail)
		{
			var name = $"{String.Format("{0:yyyyMMdd}", trail.StartLocal)} {trail.Name}.gpx";
			return Path.Combine(_rootUri, trail.Country.Name, name);
		}

		private string BuildGpx(TopoTrailInfo trail)
		{
			var writer = new GpxFileWriter();
			
			// write file header
			writer.SetVersion(GpxSchemaVersion.Version1_1);
			writer.WriteHeader(trail);

			// write custom data
			var data = new GpxExtensionData()
			{
				Timezone = trail.Timezone.TZID,
				Country = (trail.Country?.Name ?? ""),
				Region = (trail.Region?.RegionName ?? ""),
				Location = trail.Location
			};
			writer.WriteMetadata(data);

			// write tracks
			foreach (var track in trail.TopoTracks)
			{
				writer.WriteTrack(track);
			}

			return writer.GetXml();
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

	public interface ITopoTrailUpdateRequest : IGeoPoliticalData, IEntityInfo
	{
		string Name { get; }
		string Description { get; }

		string Keywords { get; }

		string UrlLink { get; }
		string UrlText { get; }

		string Location { get; }

		/* IEntityInfo
		string Key { get; }
		*/

		/* IGeoPoliticalData
		string Timezone { get; }
		string Country { get; }
		string Region { get; }
		*/
	}
}
