using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Services.External;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Services
{
	public class CartoPlaceService
	{
		// ==================================================
		// Internals
		private GoogleApiService _google = null;
		private static bool _cached = false;
		private static BasicCacheService<CartoPlaceInfo> _cache = new BasicCacheService<CartoPlaceInfo>();


		// ==================================================
		// Constructors
		public CartoPlaceService(GoogleApiService google)
		{
			_google = google;
		}


		// ==================================================
		// Methods

		/// <summary>
		/// Loads cached places from local storage
		/// </summary>
		public void InitPlaces(string uri)
		{
			lock (_cache)
			{
				if (_cached) return;

				var reader = new XlsFileReader(uri);
				var file = reader.ReadFile();

				var sheet = new CartoPlaceSheetData(file.Sheets[0]);
				foreach(var row in sheet.Rows)
				{
					var info = new CartoPlaceInfo(row);
					_cache.Add(info);
				}				
				
				_cached = true;
			}
		}

		/// <summary>
		/// Lists all places in current cache
		/// </summary>
		public List<CartoPlaceInfo> ListPlaces()
		{
			return _cache.All;
		}


	}
}
