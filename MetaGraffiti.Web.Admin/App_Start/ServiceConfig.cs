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
		private static GeoLookupService _geoLookupService = new GeoLookupService(_googleApiService);
		private static CartoPlaceService _cartoPlaceService;
		private static TripSheetService _tripSheetService;
		private static TrailDataService _trailDataService;

		public static GeoLookupService GeoLookupService
		{
			get { return _geoLookupService; }
		}

		public static TrailDataService TrailDataService
		{
			get
			{
				// unlocked check again current cache
				if (_trailDataService != null) return _trailDataService;

				lock (_lock)
				{
					// recheck after lock expires
					if (_trailDataService != null) return _trailDataService;

					// create and initalize service as needed
					var service = new TrailDataService();
					service.Init(AutoConfig.TrailSourceUri);

					// update shared static resource
					_trailDataService = service;
					return _trailDataService;
				}
			}
		}

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
					service.Init(AutoConfig.CartoPlaceData);

					// update shared static resource
					_cartoPlaceService = service;
					return _cartoPlaceService;
				}
			}
		}
	}
}