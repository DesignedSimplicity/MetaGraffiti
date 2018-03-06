using System;
using System.IO;

namespace MetaGraffiti.Web.Admin
{
	public class AutoConfig
	{
		private const string _configRoot = @"C:\Code\";

		public static string RootConfigUri { get { return _configRoot; } }


		private static string _googleMapsApiKey = "";
		public static string GoogleMapsApiKey
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_googleMapsApiKey)) _googleMapsApiKey = File.ReadAllText(Path.Combine(_configRoot, "GoogleMapsApiKey.txt"));
				return _googleMapsApiKey;
			}
		}

		private static string[] _visitedCountries = null;
		public static string[] VisitedCountries
		{
			get
			{
				if (_visitedCountries == null)
				{
					_visitedCountries = File.ReadAllText(Path.Combine(_configRoot, "KnE", "VisitedCountries.txt")).Split(',');
				}
				return _visitedCountries;
			}
		}
	}
}