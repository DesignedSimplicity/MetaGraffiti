using System;
using System.IO;

namespace MetaGraffiti.Web.Admin
{
	public class AutoConfig
	{
		private const string _configRoot = @"C:\Code\";
		private static string _googleMapsApiKey = "";

		public static string RootConfigUri { get { return _configRoot; } }

		public static string GoogleMapsApiKey
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_googleMapsApiKey))
					_googleMapsApiKey = File.ReadAllText(Path.Combine(_configRoot, "GoogleMapsApiKey.txt"));
				return _googleMapsApiKey;
			}
		}

	}
}