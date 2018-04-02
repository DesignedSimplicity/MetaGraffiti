﻿using System;
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
		private static System.Object _lockCarto = new System.Object();
		private static CartoPlaceService _cartoPlaceService;

		private static System.Object _lockTrip = new System.Object();
		private static TripSheetService _tripSheetService;

		private static System.Object _lockTopo = new System.Object();
		private static TopoTrailService _topoTrailService;

		private static GoogleApiService _googleApiService = new GoogleApiService(AutoConfig.GoogleMapsApiKey);
		private static GeoLookupService _geoLookupService = new GeoLookupService(_googleApiService);
		
	

		public static GeoLookupService GeoLookupService
		{
			get { return _geoLookupService; }
		}

		public static void ResetTopoTrail()
		{
			// clear out existing cache if necessary
			if (_topoTrailService != null) _topoTrailService.ResetCache();

			// create and initalize service as needed
			var service = new TopoTrailService(CartoPlaceService);
			service.Init(AutoConfig.TrailSourceUri);

			// update shared static resource
			_topoTrailService = service;
		}

		public static TopoTrailService TopoTrailService
		{
			get
			{
				// unlocked check again current cache
				if (_topoTrailService != null) return _topoTrailService;

				lock (_lockTrip)
				{
					// recheck after lock expires
					if (_topoTrailService != null) return _topoTrailService;

					ResetTopoTrail();

					return _topoTrailService;
				}
			}
		}

		public static void ResetTripSheet()
		{
			// clear out existing cache if necessary
			if (_tripSheetService != null) _tripSheetService.ResetCache();

			// create and initalize service as needed
			var service = new TripSheetService();
			service.Init(AutoConfig.PlaceDataUri);

			// update shared static resource
			_tripSheetService = service;
		}

		public static TripSheetService TripSheetService
		{
			get
			{
				// unlocked check again current cache
				if (_tripSheetService != null) return _tripSheetService;

				lock (_lockTrip)
				{
					// recheck after lock expires
					if (_tripSheetService != null) return _tripSheetService;

					ResetTripSheet();

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

				lock (_lockCarto)
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