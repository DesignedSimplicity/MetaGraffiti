using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;

namespace MetaGraffiti.Web.Admin
{
	public class ServiceConfig
	{
		private static System.Object _lock = new System.Object();

		private static GoogleApiService _googleApiService = new GoogleApiService(AutoConfig.GoogleMapsApiKey);
		private static TripSheetService _tripSheetService;
		private static CartoPlaceService _cartoPlaceService;

		private static BasicCacheService<GpxCache> _gpxSourceCache;
		private static BasicCacheService<GpxCache> _gpxTrackCache;

		public static TripSheetService TripSheetService
		{
			get
			{
				// unlocked check again current cache
				if (_tripSheetService != null) return _tripSheetService;

				lock (_lock)
				{
					// recheck after lock expires
					if (_tripSheetService != null) return _tripSheetService;

					// create and initalize service as needed
					var service = new TripSheetService();
					service.Init(AutoConfig.PlaceDataUri);

					// update shared static resource
					_tripSheetService = service;
					return _tripSheetService;
				}
			}
		}

		public static CartoPlaceService CartoPlaceService
		{
			get
			{
				// unlocked check again current cache
				if (_cartoPlaceService != null) return _cartoPlaceService;

				lock (_lock)
				{
					// recheck after lock expires
					if (_cartoPlaceService != null) return _cartoPlaceService;

					// create and initalize service as needed
					var service = new CartoPlaceService(_googleApiService);
					service.Init(AutoConfig.CartoDataUri);

					// update shared static resource
					_cartoPlaceService = service;
					return _cartoPlaceService;
				}
			}
		}




		public static GpxCacheService_DEPRECATED GpxSourceService
		{
			get
			{
				// unlocked check again current cache
				if (_gpxSourceCache != null) return new GpxCacheService_DEPRECATED(_gpxSourceCache);

				lock (_lock)
				{
					// recheck after lock expires
					if (_gpxSourceCache != null) return new GpxCacheService_DEPRECATED(_gpxSourceCache);

					// create if not already exists
					var cache = new BasicCacheService<GpxCache>();
					var service = new GpxCacheService_DEPRECATED(cache);

					// recursively load all gpx files in source directory
					service.LoadDirectory(Path.Combine(AutoConfig.RootConfigUri, @"GPX\Source"), true);

					// update shared static resource
					_gpxSourceCache = cache;
					return service;
				}
			}
		}

		public static GpxCacheService_DEPRECATED GpxTrackService
		{
			get
			{
				// unlocked check again current cache
				if (_gpxTrackCache != null) return new GpxCacheService_DEPRECATED(_gpxTrackCache);

				lock (_lock)
				{
					// recheck after lock expires
					if (_gpxTrackCache != null) return new GpxCacheService_DEPRECATED(_gpxTrackCache);

					// create if not already exists
					var cache = new BasicCacheService<GpxCache>();
					var service = new GpxCacheService_DEPRECATED(cache);

					// load files in each country subdirectory
					foreach (var dir in Directory.GetDirectories(Path.Combine(AutoConfig.RootConfigUri, @"GPX\Tracks")))
					{
						service.LoadDirectory(dir);
					}

					// update shared static resource
					_gpxTrackCache = cache;
					return service;
				}
			}
		}
	}
}