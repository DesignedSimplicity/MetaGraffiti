using MetaGraffiti.Base.Modules.Crypto;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetaGraffiti.Base.Services
{
	public class TrackExtractService
	{
		private static GeoLookupService _lookupService = new GeoLookupService(null);
		private static BasicCacheService<TrackExtractData> _extracts = new BasicCacheService<TrackExtractData>();
		private static TrackData _track = new TrackData();

		public TrackData Track { get { return _track; } }

		public void Reset()
		{
			_track = new TrackData();
			_extracts = new BasicCacheService<TrackExtractData>();
		}

		public List<TrackExtractData> List()
		{
			return _extracts.All;
		}

		public TrackExtractData Get(string ID)
		{
			return _extracts[ID.ToUpperInvariant()];
		}

		public TrackExtractData Delete(string ID)
		{
			var deleted = _extracts[ID.ToUpperInvariant()];
			_extracts.Remove(ID.ToUpperInvariant());
			return deleted;
		}

		public string GenerateFilename()
		{
			return $"{String.Format("{0:yyyyMMdd}", _track.Timestamp)} {_track.Name}";
		}

		public TrackData Modify(string uri)
		{
			_track = new TrackData();

			var reader = new GpxFileReader(uri);
			var source = reader.ReadFile();

			_track.Name = source.Name;
			_track.Description = source.Description;
			_track.Keywords = source.Keywords;
			_track.Url = source.Url;
			_track.UrlName = source.UrlName;

			/*
			_track.Timezone = GeoTimezoneInfo.Find(update.Timezone);
			_track.Country = GeoCountryInfo.Find(update.Country);
			_track.Region = GeoRegionInfo.Find(update.Region);
			*/
			foreach (var track in source.Tracks)
			{
				var extract = new TrackExtractCreateRequest();
				extract.Uri = uri;
				extract.StartTimestamp = track.Points.First().Timestamp;
				extract.FinishTimestamp = track.Points.Last().Timestamp;
				Extract(extract);
			}

			return _track;
		}

		public TrackData Update(TrackUpdateRequest update)
		{
			_track.Name = update.Name;
			_track.Description = update.Description;
			_track.Keywords = update.Keywords;

			_track.Url = update.Url; // TODO: make sure url is prefixed with http/s
			_track.UrlName = update.UrlName;

			_track.Country = _lookupService.SearchCountries(update.Country).FirstOrDefault();
			_track.Region = _lookupService.SearchRegions(update.Region, _track.Country).FirstOrDefault();
			_track.Location = update.Location;

			_track.Timezone = GeoTimezoneInfo.Find(update.Timezone);
			if (_track.Timezone == null && _track.Country != null) _track.Timezone = _lookupService.GuessTimezone(_track.Country);

			return _track;
		}

		public TrackExtractData Create(string uri)
		{
			var file = new FileInfo(uri);
			var reader = new GpxFileReader(file.FullName);
			var gpx = reader.ReadFile();

			var extract = new TrackExtractData();

			extract.ID = CryptoGraffiti.NewHashID();
			extract.SourceUri = uri;

			extract.Name = Path.GetFileNameWithoutExtension(uri);
			if (String.IsNullOrWhiteSpace(extract.Name)) extract.Name = gpx.Name;
			if (String.IsNullOrWhiteSpace(extract.Name)) extract.Name = Path.GetFileNameWithoutExtension(file.Name);

			extract.SourcePoints = gpx.Tracks.SelectMany(x => x.Points).ToList();
			extract.Points = extract.SourcePoints.ToList();

			return extract;
		}

		public TrackExtractData Extract(TrackExtractCreateRequest request)
		{
			var extract = Create(request.Uri);

			extract.Points = FilterPoints(extract.SourcePoints, request);

			InitTrack(extract);

			_extracts.Add(extract.ID, extract);
			return extract;
		}

		public TrackExtractData Update(TrackExtractUpdateRequest request)
		{
			var save = Get(request.ID);

			save.Name = request.Name;
			save.Description = request.Description;

			return save;
		}

		public TrackExtractData Filter(TrackFilterPointsRequest request)
		{
			var extract = Get(request.ID);
			var points = FilterPoints(extract.Points, request);

			if (points.Count == 0) return null;

			extract.Points = points;
			return extract;
		}

		public TrackExtractData Revert(string ID)
		{
			var extract = Get(ID);
			extract.Points = extract.SourcePoints;
			return extract;
		}

		public TrackExtractData Remove(TrackRemovePointsRequest request)
		{
			var extract = Get(request.ID);

			List<GpxPointData> points = new List<GpxPointData>();
			for(var index = 0; index < extract.Points.Count; index++)
			{
				if (!request.Points.Contains(index)) points.Add(extract.Points[index]);
			}
			extract.Points = points;
			return extract;
		}

		public long Import(string uri)
		{
			//ConsolidateKeywords();
			var data = GenerateGPX(GpxSchemaVersion.Version1_1);
			File.WriteAllBytes(uri, data);
			return data.Length;
		}

		public byte[] Export(string format)
		{
			if (format.ToUpperInvariant() == "GPX")
				return GenerateGPX(GpxSchemaVersion.Version1);
			else if (format.ToUpperInvariant() == "KML")
				return GenerateKML();
			else
				return null;
		}


		private byte[] GenerateGPX(GpxSchemaVersion version)
		{
			var writer = new GpxFileWriter();

			writer.SetVersion(version);
			writer.WriteHeader(_track);

			if (version == GpxSchemaVersion.Version1_1) writer.WriteMetadata(_track.Timezone.TZID, _track.Country.Name);

			foreach (var track in List())
			{
				var t = PrepareTrackData(track);
				writer.WriteTrack(t);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}

		private byte[] GenerateKML()
		{
			var writer = new KmlFileWriter();
			writer.WriteHeader(_track.Name, _track.Description);

			foreach (var track in List())
			{
				var t = PrepareTrackData(track);
				writer.WriteTrack(t);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}

		/*
		private void ConsolidateKeywords()
		{
			var keywords = _track.Keywords;
			Dictionary<string, string> tags = new Dictionary<string, string>();
			if (!String.IsNullOrWhiteSpace(keywords))
			{
				var keys = keywords.Split(',');
				foreach (var key in keys)
				{
					if (!String.IsNullOrWhiteSpace(key))
					{
						var k = key.Trim().ToUpperInvariant();
						if (!tags.ContainsKey(k) && !k.StartsWith("GEOCOUNTRY") && !k.StartsWith("GEOTIMEZONE"))
						{
							tags.Add(k, key.Trim());
						}
					}
				}
			}

			var timezone = $"GEOTIMEZONE:{_track.Timezone.TZID}";
			tags.Add(timezone.ToUpperInvariant(), timezone);
			var country = $"GEOCOUNTRY:{_track.Country.ISO3}";
			tags.Add(country.ToUpperInvariant(), country);

			_track.Keywords = String.Join(", ", tags.Values);
		}
		*/

		private GpxTrackData PrepareTrackData(TrackExtractData track)
		{
			var t = new GpxTrackData();
			t.Name = track.Name;
			t.Description = track.Description;
			t.Points = track.Points;
			if (!String.IsNullOrWhiteSpace(track.SourceUri)) t.Source = Path.GetFileNameWithoutExtension(track.SourceUri);
			return t;
		}

		private void InitTrack(TrackExtractData extract)
		{
			// set initial name to source track
			if (String.IsNullOrWhiteSpace(_track.Name)) _track.Name = extract.Name;

			// set initial timestamp in UTC_track.Timestamp
			var point = extract.Points.FirstOrDefault();
			if (_track.Timestamp == null) _track.Timestamp = point.Timestamp;

			// auto select country and region if identifiable
			if (_track.Country == null)
			{
				var countries = GeoCountryInfo.ListByLocation(point);
				if (countries.Count() == 1) _track.Country = countries.First();
			}
			if (_track.Region == null)
			{
				var regions = GeoRegionInfo.ListByLocation(point);
				if (regions.Count() == 1)
				{
					var region = regions.First();
					if (_track.Country == null || _track.Country.IsSame(region.Country))
					{
						_track.Region = region;
						_track.Country = region.Country;
					}
				}
			}

			// set default for countries with single timezone
			if (_track.Timezone == null && _track.Country != null) _track.Timezone = new GeoLookupService(null).GuessTimezone(_track.Country);
		}

		private List<GpxPointData> FilterPoints(IEnumerable<GpxPointData> points, TrackFilterBase filter)
		{
			if (filter == null) return points.OrderBy(x => x.Timestamp).ToList();

			var query = points.AsQueryable();
			if (filter.StartTimestamp.HasValue) query = query.Where(x => (x.Timestamp ?? DateTime.MinValue) >= filter.StartTimestamp.Value);
			if (filter.FinishTimestamp.HasValue) query = query.Where(x => (x.Timestamp ?? DateTime.MaxValue) <= filter.FinishTimestamp.Value);
			if (filter.MinimumSatellite.HasValue) query = query.Where(x => (x.Sats ?? 0) >= filter.MinimumSatellite.Value);
			if (filter.MaximumVelocity.HasValue) query = query.Where(x => (x.Speed ?? 0) <= filter.MaximumVelocity.Value);
			if (filter.MaximumDilution.HasValue) query = query.Where(x => x.MaxDOP <= filter.MaximumDilution.Value);
			if (filter.MissingDilution) query = query.Where(x => x.HDOP.HasValue && x.VDOP.HasValue && x.PDOP.HasValue);
			return query.OrderBy(x => x.Timestamp).ToList();
		}
	}

	public class TrackData : IGpxFileHeader
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public DateTime? Timestamp { get; set; }

		public string Keywords { get; set; }

		public string Url { get; set; }
		public string UrlName { get; set; }

		public GeoTimezoneInfo Timezone { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
		public string Location { get; set; }
	}

	public class TrackExtractData
	{
		public string ID { get; set; }

		public string SourceUri { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public List<GpxPointData> Points { get; set; }

		public List<GpxPointData> SourcePoints { get; set; }
	}

	public class TrackUpdateRequest
	{
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

	public class TrackExtractUpdateRequest
	{
		public string ID { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
	}

	public class TrackRemovePointsRequest
	{
		public string ID { get; set; }
		public List<int> Points { get; set; }
	}

	public class TrackExtractCreateRequest : TrackFilterBase
	{
		// Pointer to Gpx source file
		public string Uri { get; set; }

		// Name to give extract
		public string Name { get; set; }
	}

	public class TrackFilterPointsRequest : TrackFilterBase
	{
		/// <summary>
		/// ID for existing track extract
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		/// Array of point indexes to remove
		/// </summary>
		public string Points { get; set; }
	}

	public abstract class TrackFilterBase
	{
		// Time range filters
		public DateTime? StartTimestamp { get; set; }
		public DateTime? FinishTimestamp { get; set; }

		// Data quality filters
		public int? MinimumSatellite { get; set; }
		public int? MaximumVelocity { get; set; }

		public int? MaximumDilution { get; set; }
		public bool MissingDilution { get; set; }
	}
}
