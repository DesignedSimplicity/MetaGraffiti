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
		private static System.Object _lockBio = new System.Object();
		private static BioHealthService _bioHealthService;

		private static System.Object _lockCarto = new System.Object();
		private static CartoPlaceService _cartoPlaceService;

		private static System.Object _lockTrip = new System.Object();
		private static TripSheetService _tripSheetService;

		private static System.Object _lockTopo = new System.Object();
		private static TopoTrailService _topoTrailService;

		private static System.Object _lockDem = new System.Object();
		private static DemElevationService _demElevationService;

		private static GoogleApiService _googleApiService = new GoogleApiService(AutoConfig.GoogleMapsApiKey);
		private static GoogleLookupService _googleLookupService = new GoogleLookupService(_googleApiService);
		
		public static GoogleLookupService GoogleLookupService
		{
			get { return _googleLookupService; }
		}

		public static void ResetBioHealth()
		{
			// clear out existing cache if necessary
			if (_bioHealthService != null) _bioHealthService.ResetCache();

			// create and initalize service as needed
			var service = new BioHealthService();
			service.Init(AutoConfig.PolarSourceUri);

			// update shared static resource
			_bioHealthService = service;
		}

		public static BioHealthService BioHealthService
		{
			get
			{
				// unlocked check again current cache
				if (_bioHealthService != null) return _bioHealthService;

				lock (_lockBio)
				{
					// recheck after lock expires
					if (_bioHealthService != null) return _bioHealthService;

					ResetBioHealth();

					return _bioHealthService;
				}
			}
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

		public static DemElevationService DemElevationService
		{
			get
			{
				// unlocked check again current cache
				if (_demElevationService != null) return _demElevationService;

				lock (_lockDem)
				{
					// recheck after lock expires
					if (_demElevationService != null) return _demElevationService;

					// create and initalize service as needed
					var service = new DemElevationService();
					service.Init(AutoConfig.ElevationSourceUri);

					// update shared static resource
					_demElevationService = service;
					return _demElevationService;
				}
			}
		}
	}
}