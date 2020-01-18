﻿using System;
using System.IO;

namespace MetaGraffiti.Web.Admin
{
	public class AutoConfig
	{
		private const string _configRoot = @"D:\Drives\Dropbox\Earth\Data\";
        public static string RootConfigUri { get { return _configRoot; } }

		public static string TrackSourceUri = Path.Combine(RootConfigUri, @"Topo\Tracks\");
		public static string TrailSourceUri = Path.Combine(RootConfigUri, @"Topo\Trails\");
		public static string IconSourceUri = Path.Combine(_configRoot, @"Icono\Places\");

		public static string CartoPlaceData = Path.Combine(RootConfigUri, @"Carto\CartoPlaceData.xlsx");
		public static string PlaceDataUri = Path.Combine(RootConfigUri, @"Carto\AnnualTravelLog.xlsx");


		private static string _googleMapsApiKey = "";
		public static string GoogleMapsApiKey
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_googleMapsApiKey)) _googleMapsApiKey = File.ReadAllText(Path.Combine(_configRoot, "Crypto", "GoogleMapsApiKey.txt"));
				return _googleMapsApiKey;
			}
		}
	}
}