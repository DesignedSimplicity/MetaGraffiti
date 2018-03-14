using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Services;

namespace MetaGraffiti.Web.Admin
{
	public class ServiceConfig
	{
		private static System.Object _lock = new System.Object();

		private static BasicCacheService<GpxCache> _gpxSourceCache;
		private static BasicCacheService<GpxCache> _gpxTrackCache;

		public static GpxCacheService GpxSourceService
		{
			get
			{
				// unlocked check again current cache
				if (_gpxSourceCache != null) return new GpxCacheService(_gpxSourceCache);

				lock (_lock)
				{
					// recheck after lock expires
					if (_gpxSourceCache != null) return new GpxCacheService(_gpxSourceCache);

					// create new if not already exists
					var cache = new BasicCacheService<GpxCache>();
					var service = new GpxCacheService(cache);
					service.LoadDirectory(Path.Combine(AutoConfig.RootConfigUri, @"GPX\Source"), true);

					// update shared static resource
					_gpxSourceCache = cache;
					return service;
				}
			}
		}

		public static GpxCacheService GpxTrackService
		{
			get
			{
				// unlocked check again current cache
				if (_gpxTrackCache != null) return new GpxCacheService(_gpxTrackCache);

				lock (_lock)
				{
					// recheck after lock expires
					if (_gpxTrackCache != null) return new GpxCacheService(_gpxTrackCache);

					// create new if not already exists
					var cache = new BasicCacheService<GpxCache>();
					var service = new GpxCacheService(cache);
					service.LoadDirectory(Path.Combine(AutoConfig.RootConfigUri, @"GPX\Tracks"));

					// update shared static resource
					_gpxTrackCache = cache;
					return service;
				}
			}
		}
	}
}