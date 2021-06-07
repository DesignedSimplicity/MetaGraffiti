using System;
using System.IO;

namespace MetaGraffiti.Web.Admin
{
	public class AutoConfig
	{
		private const string _configRoot = @"D:\Drives\Dropbox\Earth\Data\";
		private const string _imagesRoot = @"E:\_Meta\";
		public static string RootConfigUri { get { return _configRoot; } }

		public static string PolarSourceUri = Path.Combine(RootConfigUri, @"Bio\Polar\");
		public static string StravaSourceUri = Path.Combine(RootConfigUri, @"Bio\Strava\");

		public static string ElevationSourceUri = Path.Combine(RootConfigUri, @"Topo\Elevation\");
		public static string TrackSourceUri = Path.Combine(RootConfigUri, @"Topo\Tracks\");
		public static string TrailSourceUri = Path.Combine(RootConfigUri, @"Topo\Trails\");

		public static string PhotoSourceUri = Path.Combine(_imagesRoot, @"Photos\");
		public static string PanoSourceUri = Path.Combine(_imagesRoot, @"Panos\_ANDROID");

		public static string IconSourceUri = Path.Combine(_configRoot, @"Icono\");

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