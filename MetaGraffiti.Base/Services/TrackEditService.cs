﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetaGraffiti.Base.Modules;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services.Internal;

namespace MetaGraffiti.Base.Services
{
    public class TrackEditService
    {
		// ==================================================
		// Internals

		private static BasicCacheService<TrackEditData> _tracks = new BasicCacheService<TrackEditData>();


		// ==================================================
		// Properties

		public bool HasEdits
		{
			get { return _tracks.Count > 0; }
		}


		// ==================================================
		// Methods

		/// <summary>
		/// Lists all tracks in current edit session
		/// </summary>
		public List<TrackEditData> ListTracks()
		{
			return _tracks.All.OrderBy(x => x.Points.First().Timestamp.Value).ToList();
		}

		/// <summary>
		/// Removes all current track edits from session
		/// </summary>
		public List<TrackEditData> RemoveAll()
		{
			var all = _tracks.All;
			_tracks = new BasicCacheService<TrackEditData>();
			return all;
		}

		/// <summary>
		/// Gets a specific track edit from session
		/// </summary>
		public TrackEditData GetTrack(string key)
		{
			return _tracks[key.ToUpperInvariant()];
		}

		/// <summary>
		/// Removes a specific track edit from session
		/// </summary>
		public TrackEditData RemoveTrack(string key)
		{
			var removed = _tracks[key.ToUpperInvariant()];
			_tracks.Remove(key.ToUpperInvariant());
			return removed;
		}

		/// <summary>
		/// Prepares a single track edit from all of the data in a given file
		/// </summary>
		public TrackEditData PreviewTrack(string uri)
		{
			// load file and read data
			var file = new FileInfo(uri);
			var read = new GpxFileReader(file.FullName);
			var data = read.ReadFile();

			// create new extract entity
			var track = new TrackEditData();
			track.Key = Graffiti.Crypto.GetNewHash();
			track.Source = file.FullName;

			// set default name value
			track.Name = data.Name;
			if (String.IsNullOrWhiteSpace(track.Name)) track.Name = Path.GetFileNameWithoutExtension(file.Name);

			// prepare points and source points lists
			track.DataPoints = data.Tracks.SelectMany(x => x.PointData).ToList();
			track.Points = track.DataPoints.ToList<IGpxPoint>();

			return track;
		}

		/// <summary>
		/// Creates a single track edit from a specific set of file data and adds it to the edit session
		/// </summary>
		public TrackEditData CreateTrack(TrackEditCreateRequest request)
		{
			var track = PreviewTrack(request.Uri);

			// update name and description if provided
			if (!String.IsNullOrWhiteSpace(request.Name)) track.Name = request.Name;
			if (!String.IsNullOrWhiteSpace(request.Description)) track.Description = request.Description;

			// apply points filters if provided
			track.Points = FilterPoints(track.DataPoints, request);

			// add to edit session
			_tracks.Add(track);

			// return new track edit
			return track;
		}

		/// <summary>
		/// Creates track edits for each track in the file data
		/// </summary>
		public List<TrackEditData> CreateTracks(string uri)
		{
			// load file and read data
			var file = new FileInfo(uri);
			var read = new GpxFileReader(file.FullName);
			var data = read.ReadFile();

			var tracks = new List<TrackEditData>();
			foreach (var t in data.Tracks)
			{
				// create new extract entity
				var track = new TrackEditData();
				track.Key = Graffiti.Crypto.GetNewHash();
				track.Source = t.Source;

				track.Name = t.Name;
				track.Description = t.Description;

				track.DataPoints = t.PointData;
				track.Points = t.Points;

				// default missing properties
				if (String.IsNullOrWhiteSpace(track.Source)) track.Source = file.FullName;
				if (String.IsNullOrWhiteSpace(track.Name)) track.Source = Path.GetFileNameWithoutExtension(file.Name);

				// add to edit session
				_tracks.Add(track);
				tracks.Add(track);
			}

			return tracks;
		}

		/// <summary>
		/// Filters tracks points with given criteria
		/// </summary>
		public TrackEditData ApplyFilter(TrackEditFilterRequest request)
		{
			var track = GetTrack(request.Key);

			// TODO: need better way to address when filter excludes everything
			var points = FilterPoints(track.Points, request);			
			if (points.Count == 0) return null;
			track.Points = points;

			return track;
		}

		/// <summary>
		/// Reverts the set of points to the origional data
		/// </summary>
		public TrackEditData RevertFilter(string key)
		{
			var track = GetTrack(key);
			track.Points = track.DataPoints.ToList<IGpxPoint>();
			return track;
		}

		/// <summary>
		/// Removes one or more points from the list of filtered points
		/// </summary>
		public TrackEditData RemovePoints(TrackEditRemovePointsRequest request)
		{
			var track = GetTrack(request.Key);

			if (request.Points == null) return track;

			List<IGpxPoint> points = new List<IGpxPoint>();
			for (var index = 0; index < track.Points.Count; index++)
			{
				if (!request.Points.Contains(index)) points.Add(track.Points[index]);
			}

			track.Points = points;
			return track;
		}

		/// <summary>
		/// Generates a GPX file from the current set of tracks
		/// </summary>
		/// <returns></returns>
		public byte[] GenerateGPX(string name = "")
		{
			var writer = new GpxFileWriter();
			writer.WriteHeader(name);
			foreach (var track in ListTracks())
			{
				writer.WriteTrack(track);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}

		/// <summary>
		/// Generates a KML file from the current set of tracks
		/// </summary>
		/// <returns></returns>
		public byte[] GenerateKML(string name = "")
		{
			var writer = new KmlFileWriter();
			writer.WriteHeader(name);
			foreach (var track in ListTracks())
			{
				writer.WriteTrack(track);
			}
			return Encoding.ASCII.GetBytes(writer.GetXml());
		}


		// ==================================================
		// Helpers

		public decimal GetMaxDOP(IGpxPoint point)
		{
			var h = point.HDOP ?? 0;
			var v = point.VDOP ?? 0;
			var p = point.PDOP ?? 0;
			var dop = h;
			if (v > dop) dop = v;
			if (p > dop) dop = p;
			return dop;
		}


		// ==================================================
		// Internals

		private List<IGpxPoint> FilterPoints(IEnumerable<IGpxPoint> points, TrackEditFilter filter)
		{
			if (filter == null) return points.OrderBy(x => x.Timestamp).ToList();

			var query = points.AsQueryable();
			if (filter.StartUTC.HasValue)
			{
				var start = filter.StartUTC.Value.AddMilliseconds(-1);
				query = query.Where(x => (x.Timestamp ?? DateTime.MinValue) >= start);
			}
			if (filter.FinishUTC.HasValue)
			{
				var finish = filter.FinishUTC.Value.AddSeconds(1).AddMilliseconds(-1);
				query = query.Where(x => (x.Timestamp ?? DateTime.MaxValue) <= finish);
			}
			if (filter.MinimumSatellite.HasValue) query = query.Where(x => (x.Sats ?? 0) >= filter.MinimumSatellite.Value);
			if (filter.MaximumVelocity.HasValue) query = query.Where(x => (x.Speed ?? 999) <= filter.MaximumVelocity.Value);
			if (filter.MaximumDilution.HasValue) query = query.Where(x => GetMaxDOP(x) <= filter.MaximumDilution.Value);
			if (filter.MissingDilution) query = query.Where(x => x.HDOP.HasValue && x.VDOP.HasValue && x.PDOP.HasValue);
			return query.OrderBy(x => x.Timestamp).ToList();
		}
	}

	public class TrackEditData : ICacheEntity, IGpxTrack
	{
		// Track ID for edit session
		public string Key { get; set; }

		// Source file identifier
		public string Source { get; set; }

		// Required name for track
		public string Name { get; set; }

		// Optional description of track
		public string Description { get; set; }
		
		// Current set of edited points
		public IList<IGpxPoint> Points { get; set; }

		// Origional set of raw data points
		public List<GpxPointData> DataPoints { get; set; }
	}

	public class TrackEditCreateRequest : TrackEditFilter
	{
		// Pointer to Gpx source file
		public string Uri { get; set; }

		// Name to give extract
		public string Name { get; set; }

		// Description to give extract
		public string Description { get; set; }
	}

	public class TrackEditFilterRequest : TrackEditFilter
	{
		// Track ID for edit session
		public string Key { get; set; }
	}

	public class TrackEditFilter
	{
		// Time range filters
		public DateTime? StartUTC { get; set; }
		public DateTime? FinishUTC { get; set; }

		// Data quality filters
		public int? MinimumSatellite { get; set; }
		public decimal? MaximumVelocity { get; set; }

		public decimal? MaximumDilution { get; set; }
		public bool MissingDilution { get; set; }
	}

	public class TrackEditRemovePointsRequest
	{
		// Track ID for edit session
		public string Key { get; set; }

		// List of points to remove by index
		public List<int> Points { get; set; }
	}
}