using System;
using System.IO;

namespace MetaGraffiti.Web.Admin
{
	public class AutoConfig
	{
		private const string _configRoot = @"D:\Drives\Dropbox\Data\CODE\";
        public static string RootConfigUri { get { return _configRoot; } }

		public static string TrackSourceUri = Path.Combine(RootConfigUri, @"Gpx\Tracks\");
		public static string TrailSourceUri = Path.Combine(RootConfigUri, @"Gpx\Trails\");
		public static string IconSourceUri = Path.Combine(_configRoot, @"KnE\Icons\Places\");

		public static string CartoPlaceData = Path.Combine(RootConfigUri, @"KnE\CartoPlaceData.xlsx");
		public static string PlaceDataUri = Path.Combine(RootConfigUri, @"KnE\ConsolidatedTrips.xlsx");


		private static string _googleMapsApiKey = "";
		public static string GoogleMapsApiKey
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_googleMapsApiKey)) _googleMapsApiKey = File.ReadAllText(Path.Combine(_configRoot, "GoogleMapsApiKey.txt"));
				return _googleMapsApiKey;
			}
		}
	}
}