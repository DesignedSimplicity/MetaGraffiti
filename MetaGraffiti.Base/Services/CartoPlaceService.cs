using MetaGraffiti.Base.Modules.Carto.Info;
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
		public void InitCache(string uri)
		{

		}

		/// <summary>
		/// Lists all places in current cache
		/// </summary>
		public List<CartoPlaceInfo> ListPlaces()
		{
			return null;
		}


	}
}
