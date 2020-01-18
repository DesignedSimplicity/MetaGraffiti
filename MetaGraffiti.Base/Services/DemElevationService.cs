using System;
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
	public class DemElevationService
	{
		// ==================================================
		// Internals
		private static string _rootUri = "";
		private static object _init = false;
		private static BasicCacheService<TopoTrailInfo> _elevations;

		public DemElevationService()
		{
		}


		// ==================================================
		// Methods

		/// <summary>
		/// Initializes repository with all local GPX files with DEM data
		/// </summary>
		public void Init(string uri)
		{
			lock (_init) // TODO: use internal init for basic cache
			{
				if (Convert.ToBoolean(_init)) return;
				var root = new DirectoryInfo(uri);
				_rootUri = root.FullName;
				_init = true;
			}
		}

		/// <summary>
		/// Returns the DEM elevation data is available for the given trail
		/// </summary>
		public BasicServiceResponse<TopoTrailInfo> GetElevationData(TopoTrailInfo trail)
		{
			// check if DEM file exists
			var name = trail.Key + " DEM.gpx";
			var uri = Path.Combine(_rootUri, name);
			if (File.Exists(uri))
			{
				// load file since it exists
				var elevation = LoadTrail(uri);

				// do some crude validation
				if (elevation.Tracks.Count() != trail.Tracks.Count())
				{
					return new BasicServiceResponse<TopoTrailInfo>("Track counts do not match");
				}
				if (elevation.Stats.PointCount != trail.Stats.PointCount)
				{
					return new BasicServiceResponse<TopoTrailInfo>("Point counts do not match");
				}
				return new BasicServiceResponse<TopoTrailInfo>(elevation);
			}
			else
			{
				return new BasicServiceResponse<TopoTrailInfo>(true, "No DEM data is available");
			}			
		}

		// ==================================================
		// Helpers
		private TopoTrailInfo LoadTrail(string uri)
		{
			// load gpx data into trail
			var reader = new GpxFileReader(uri);
			var data = reader.ReadFile();
			var trail = new TopoTrailInfo(data);

			// setup trail details
			var filename = Path.GetFileNameWithoutExtension(uri);
			trail.Key = filename.ToUpperInvariant();
			trail.Source = uri;

			return trail;
		}

	}

}
