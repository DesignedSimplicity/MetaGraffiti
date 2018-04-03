﻿using MetaGraffiti.Base.Modules;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Services
{
    public class TrackEditService
    {
		// ==================================================
		// Internals

		private static BasicCacheService<TrackEdit> _tracks = new BasicCacheService<TrackEdit>();


		// ==================================================
		// Methods
		
		/// <summary>
		/// Removes all current track edits from session
		/// </summary>
		public void Reset()
		{
			_tracks = new BasicCacheService<TrackEdit>();
		}

		/// <summary>
		/// Gets a specific track edit from session
		/// </summary>
		public TrackEdit GetTrack(string key)
		{
			return _tracks[key.ToLowerInvariant()];
		}

		/// <summary>
		/// Removes a specific track edit from session
		/// </summary>
		public TrackEdit RemoveTrack(string key)
		{
			var removed = _tracks[key.ToUpperInvariant()];
			_tracks.Remove(key.ToUpperInvariant());
			return removed;
		}

		/// <summary>
		/// Prepares a track edit from all of the data in a given file
		/// </summary>
		public TrackEdit PreviewTrack(string uri)
		{
			// load file and read data
			var file = new FileInfo(uri);
			var read = new GpxFileReader(file.FullName);
			var data = read.ReadFile();

			// create new extract entity
			var track = new TrackEdit();
			track.Key = Graffiti.Crypto.GetNewHash();
			track.Source = file.FullName;

			// set default name value
			track.Name = data.Name;
			if (String.IsNullOrWhiteSpace(track.Name)) track.Name = Path.GetFileNameWithoutExtension(file.Name);

			// prepare points and source points lists
			track.SourcePoints = data.Tracks.SelectMany(x => x.Points).ToList();
			track.Points = track.SourcePoints.ToList<IGpxPoint>();

			return track;
		}

		/// <summary>
		/// Creates a track extract from a specific set of data and adds it to the edit session
		/// </summary>
		public TrackEdit CreateTrack(TrackEditCreateRequest request)
		{
			var track = PreviewTrack(request.Uri);

			// update name and description if provided
			if (!String.IsNullOrWhiteSpace(request.Name)) track.Name = request.Name;
			if (!String.IsNullOrWhiteSpace(request.Description)) track.Description = request.Description;

			// apply points filters if provided
			track.Points = FilterPoints(track.SourcePoints, request);

			// add to edit session
			_tracks.Add(track);

			// return new track edit
			return track;
		}

		/// <summary>
		/// Filters tracks points with given criteria
		/// </summary>
		public TrackEdit ApplyFilter(TrackEditFilterRequest request)
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
		public TrackEdit RevertFilter(string key)
		{
			var track = GetTrack(key);
			track.Points = track.SourcePoints.ToList<IGpxPoint>();
			return track;
		}

		/// <summary>
		/// Removes one or more points from the list of filtered points
		/// </summary>
		public TrackEdit RemovePoints(TrackEditRemovePointsRequest request)
		{
			var track = GetTrack(request.Key);

			List<IGpxPoint> points = new List<IGpxPoint>();
			for (var index = 0; index < track.Points.Count; index++)
			{
				if (!request.Indexes.Contains(index)) points.Add(track.Points[index]);
			}

			track.Points = points;
			return track;
		}

		// ==================================================
		// Internals

		private List<IGpxPoint> FilterPoints(IEnumerable<IGpxPoint> points, TrackEditFilter filter)
		{
			if (filter == null) return points.OrderBy(x => x.Timestamp).ToList();

			var query = points.AsQueryable();
			if (filter.StartUTC.HasValue) query = query.Where(x => (x.Timestamp ?? DateTime.MinValue) >= filter.StartUTC.Value);
			if (filter.FinishUTC.HasValue) query = query.Where(x => (x.Timestamp ?? DateTime.MaxValue) <= filter.FinishUTC.Value);
			if (filter.MinimumSatellite.HasValue) query = query.Where(x => (x.Sats ?? 0) >= filter.MinimumSatellite.Value);
			if (filter.MaximumVelocity.HasValue) query = query.Where(x => (x.Speed ?? 0) <= filter.MaximumVelocity.Value);
			if (filter.MaximumDilution.HasValue) query = query.Where(x => GetMaxDOP(x) <= filter.MaximumDilution.Value);
			if (filter.MissingDilution) query = query.Where(x => x.HDOP.HasValue && x.VDOP.HasValue && x.PDOP.HasValue);
			return query.OrderBy(x => x.Timestamp).ToList();
		}

		private decimal GetMaxDOP(IGpxPoint point)
		{
			var h = point.HDOP ?? 0;
			var v = point.VDOP ?? 0;
			var p = point.PDOP ?? 0;
			var dop = h;
			if (v > dop) dop = v;
			if (p > dop) dop = p;
			return dop;
		}
	}

	public class TrackEdit : ICacheEntity, IGpxTrack
	{
		// Temporary key for edit session
		public string Key { get; set; }

		// Source file identifier
		public string Source { get; set; }

		// Required name for track
		public string Name { get; set; }

		// Optional description of track
		public string Description { get; set; }
		
		// Current set of edited points
		public List<IGpxPoint> Points { get; set; }

		// Origional set of raw data points
		public List<GpxPointData> SourcePoints { get; set; }
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
		// ID for existing track extract
		public string Key { get; set; }
	}

	public abstract class TrackEditFilter
	{
		// Time range filters
		public DateTime? StartUTC { get; set; }
		public DateTime? FinishUTC { get; set; }

		// Data quality filters
		public int? MinimumSatellite { get; set; }
		public int? MaximumVelocity { get; set; }

		public int? MaximumDilution { get; set; }
		public bool MissingDilution { get; set; }
	}

	public class TrackEditRemovePointsRequest
	{
		public string Key { get; set; }
		public List<int> Indexes { get; set; }
	}
}
