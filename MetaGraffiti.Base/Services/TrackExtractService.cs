using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetaGraffiti.Base.Services
{
	public class TrackExtractService
	{
		// ==================================================
		// Internals
		private static BasicCacheService<TrackExtractData> _extracts = new BasicCacheService<TrackExtractData>();
		private static TrackGroupData _trackGroupData = new TrackGroupData();


		// ==================================================
		// Methods
		public TrackGroupData GetTrackGroup()
		{
			return _trackGroupData;
		}

		/// <summary>
		/// Resets current edit session
		/// </summary>
		public void ResetSession()
		{
			_trackGroupData = new TrackGroupData();
			_extracts = new BasicCacheService<TrackExtractData>();
		}

		/// <summary>
		/// Lists all track extracts in edit session
		/// </summary>
		public List<TrackExtractData> ListExtracts()
		{
			return _extracts.All.OrderBy(x => x.Points.First().Timestamp).ToList();
		}

		/// <summary>
		/// Gets a specific extract from edit session
		/// </summary>
		public TrackExtractData GetExtract(string ID)
		{
			return _extracts[ID.ToUpperInvariant()];
		}



		/// <summary>
		/// Reads track level data from file
		/// </summary>
		public TrackGroupData ReadTrail(string uri)
		{
			var track = new TrackGroupData();

			var reader = new GpxFileReader(uri);

			// load common properties
			var source = reader.ReadFile();
			track.Uri = uri;
			track.Data = source;
			track.Name = source.Name;
			track.Description = source.Description;
			track.Keywords = source.Keywords;
			track.UrlLink = source.UrlLink;
			track.UrlText = source.UrlText;

			// load custom properties
			var data = source.Extensions;
			track.Timezone = GeoTimezoneInfo.Find(data.Timezone);
			track.Country = GeoCountryInfo.Find(data.Country);
			track.Region = GeoRegionInfo.Find(data.Region);
			track.Location = data.Location;

			return track;
		}


		/// <summary>
		/// Creates an edit session for an existing trail file
		/// </summary>
		public void ModifyTrail(string uri)
		{
			_trackGroupData = ReadTrail(uri);

			// extract each track into edit session
			foreach (var track in _trackGroupData.Data.Tracks)
			{
				var request = new TrackExtractCreateRequest();

				request.Uri = uri;
				request.Name = track.Name;
				request.Description = track.Description;
				request.StartUTC = track.PointData.First().Timestamp;
				request.FinishUTC = track.PointData.Last().Timestamp;

				var extract = CreateExtract(request);
				extract.Source = track.Source;
			}
		}

		/// <summary>
		/// Updates the trail level metadata
		/// </summary>
		public TrackGroupData UpdateTrail(TrailUpdateRequest2 update)
		{
			_trackGroupData.Name = update.Name;
			_trackGroupData.Description = update.Description;

			_trackGroupData.Keywords = update.Keywords;

			// TODO: make sure url is prefixed with http/s
			_trackGroupData.UrlLink = update.Url;
			_trackGroupData.UrlText = update.UrlName;

			_trackGroupData.Country = Graffiti.Geo.SearchCountry(update.Country);
			_trackGroupData.Region = Graffiti.Geo.SearchRegion(update.Region, _trackGroupData.Country);
			_trackGroupData.Location = update.Location;

			_trackGroupData.Timezone = GeoTimezoneInfo.Find(update.Timezone);
			if (_trackGroupData.Timezone == null && _trackGroupData.Country != null) _trackGroupData.Timezone = Graffiti.Geo.GuessTimezone(_trackGroupData.Country);

			return _trackGroupData;
		}

		/// <summary>
		/// Updates the track file level metadata
		/// </summary>
		public TrackGroupData UpdateTrack(TrackUpdateRequest update)
		{
			_trackGroupData.Name = update.Name;
			_trackGroupData.Description = update.Description;
			_trackGroupData.Keywords = update.Keywords;

			_trackGroupData.UrlLink = update.Url; // TODO: make sure url is prefixed with http/s
			_trackGroupData.UrlText = update.UrlName;

			_trackGroupData.Country = Graffiti.Geo.SearchCountry(update.Country);
			_trackGroupData.Region = Graffiti.Geo.SearchRegion(update.Region, _trackGroupData.Country);
			_trackGroupData.Location = update.Location;

			_trackGroupData.Timezone = GeoTimezoneInfo.Find(update.Timezone);
			if (_trackGroupData.Timezone == null && _trackGroupData.Country != null) _trackGroupData.Timezone = Graffiti.Geo.GuessTimezone(_trackGroupData.Country);

			return _trackGroupData;
		}

		/// <summary>
		/// Prepares a track extract from all of the data in a given file
		/// </summary>
		public TrackExtractData PrepareExtract(string uri)
		{
			// load file and read data
			var file = new FileInfo(uri);
			var reader = new GpxFileReader(file.FullName);
			var source = reader.ReadFile();

			// create new extract entity
			var extract = new TrackExtractData();
			extract.ID = Graffiti.Crypto.GetNewHash();
			extract.Source = uri;

			// set default name value
			extract.Name = source.Name;
			if (String.IsNullOrWhiteSpace(extract.Name)) extract.Name = Path.GetFileNameWithoutExtension(file.Name);

			// prepare points and source points lists
			extract.SourcePoints = source.Tracks.SelectMany(x => x.PointData).ToList();
			extract.Points = extract.SourcePoints.ToList();

			return extract;
		}

		/// <summary>
		/// Creates a track extract from a specific set of data and adds it to the edit session
		/// </summary>
		public TrackExtractData CreateExtract(TrackExtractCreateRequest request)
		{
			var extract = PrepareExtract(request.Uri);

			// update name and description if provided
			if (!String.IsNullOrWhiteSpace(request.Name)) extract.Name = request.Name;
			if (!String.IsNullOrWhiteSpace(request.Description)) extract.Description = request.Description;

			// apply points filters
			extract.Points = FilterPoints(extract.SourcePoints, request);

			// initialize track metadata if required
			InitTrackMetadata(extract);

			// add to edits ession and return
			_extracts.Add(extract.ID, extract);
			return extract;
		}
		
		/// <summary>
		/// Exports edit session as new file for download
		/// </summary>
		public byte[] CreateTrackFile(string format)
		{
			if (format.ToUpperInvariant() == "GPX")
				return GenerateGPX(GpxSchemaVersion.Version1);
			else if (format.ToUpperInvariant() == "KML")
				return GenerateKML();
			else
				return null;
		}

		/// <summary>
		/// Imports edit session as new trail file
		/// </summary>
		public long WriteTrackFile(string uri)
		{
			var data = GenerateGPX(GpxSchemaVersion.Version1_1);
			File.WriteAllBytes(uri, data);
			return data.Length;
		}


		// ==================================================
		// Helpers
		private byte[] GenerateGPX(GpxSchemaVersion version)
		{
			var writer = new GpxFileWriter();

			writer.SetVersion(version);
			writer.WriteHeader(_trackGroupData);

			if (version == GpxSchemaVersion.Version1_1)
			{
				writer.WriteMetadata(_trackGroupData.Timezone.TZID, _trackGroupData.Country.Name, _trackGroupData.Region?.RegionName ?? "");
			}

			foreach (var track in ListExtracts())
			{
				var t = PrepareTrackData(track);
				writer.WriteTrack(t);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}

		private byte[] GenerateKML()
		{
			var writer = new KmlFileWriter();
			writer.WriteHeader(_trackGroupData.Name, _trackGroupData.Description);

			foreach (var track in ListExtracts())
			{
				writer.WriteTrack(track.Name, track.Description, track.Points);
			}

			return Encoding.ASCII.GetBytes(writer.GetXml());
		}

		private GpxTrackData PrepareTrackData(TrackExtractData track)
		{
			var t = new GpxTrackData();
			t.Name = track.Name;
			t.Description = track.Description;
			t.PointData = track.Points;
			if (!String.IsNullOrWhiteSpace(track.Source)) t.Source = Path.GetFileNameWithoutExtension(track.Source);
			return t;
		}

		
		private void InitTrackMetadata(TrackExtractData extract)
		{
			// set initial name to source track
			if (String.IsNullOrWhiteSpace(_trackGroupData.Name)) _trackGroupData.Name = extract.Name;

			// set initial timestamp in UTC_track.Timestamp
			var point = extract.Points.FirstOrDefault();
			if (_trackGroupData.Timestamp == null) _trackGroupData.Timestamp = point.Timestamp;

			// auto select country and region if identifiable
			if (_trackGroupData.Country == null)
			{
				var countries = GeoCountryInfo.ListByLocation(point);
				if (countries.Count() == 1) _trackGroupData.Country = countries.First();
			}
			if (_trackGroupData.Region == null)
			{
				var regions = GeoRegionInfo.ListByLocation(point);
				if (regions.Count() == 1)
				{
					var region = regions.First();
					if (_trackGroupData.Country == null || _trackGroupData.Country.IsSame(region.Country))
					{
						_trackGroupData.Region = region;
						_trackGroupData.Country = region.Country;
					}
				}
			}

			// set default for countries with single timezone
			if (_trackGroupData.Timezone == null && _trackGroupData.Country != null) _trackGroupData.Timezone = Graffiti.Geo.GuessTimezone(_trackGroupData.Country);
		}

		private List<GpxPointData> FilterPoints(IEnumerable<GpxPointData> points, TrackEditFilter filter)
		{
			if (filter == null) return points.OrderBy(x => x.Timestamp).ToList();

			var query = points.AsQueryable();
			if (filter.StartUTC.HasValue) query = query.Where(x => (x.Timestamp ?? DateTime.MinValue) >= filter.StartUTC.Value);
			if (filter.FinishUTC.HasValue) query = query.Where(x => (x.Timestamp ?? DateTime.MaxValue) <= filter.FinishUTC.Value);
			if (filter.MinimumSatellite.HasValue) query = query.Where(x => (x.Sats ?? 0) >= filter.MinimumSatellite.Value);
			if (filter.MaximumVelocity.HasValue) query = query.Where(x => (x.Speed ?? 0) <= filter.MaximumVelocity.Value);
			if (filter.MaximumDilution.HasValue) query = query.Where(x => x.MaxDOP <= filter.MaximumDilution.Value);
			if (filter.MissingDilution) query = query.Where(x => x.HDOP.HasValue && x.VDOP.HasValue && x.PDOP.HasValue);
			return query.OrderBy(x => x.Timestamp).ToList();
		}
	}


	public class TrailUpdateRequest2
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

	public class TrackGroupData : IGpxFileHeader
	{
		public string Uri { get; set; }

		public GpxFileData Data { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public DateTime? Timestamp { get; set; }

		public string Keywords { get; set; }

		public string UrlLink { get; set; }
		public string UrlText { get; set; }


		// Custom extension values
		public GeoTimezoneInfo Timezone { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
		public string Location { get; set; }
	}

	public class TrackExtractData
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string Source { get; set; }
		public string Description { get; set; }

		public List<GpxPointData> Points { get; set; }

		public List<GpxPointData> SourcePoints { get; set; }

		public IEnumerable<IGeoPoint> GeoPoints { get { return Points.AsEnumerable<IGeoPoint>(); } }
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

	public class TrackExtractCreateRequest : TrackEditFilter
	{
		// Pointer to Gpx source file
		public string Uri { get; set; }

		// Name to give extract
		public string Name { get; set; }

		// Description to give extract
		public string Description { get; set; }
	}
}
