using System;
using System.IO;
using System.Reflection;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Tests
{
	public class TestsHelper
	{
		public static IGeoLatLon GetNYC()
		{
			return new GeoPosition(40.7128, -74.0060);
		}



		public static string GetAssemblyDirectory()
		{
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			return Path.GetDirectoryName(path);
		}

		public static string GetTestRootDirectory()
		{
			string path = GetAssemblyDirectory();
			var live = @"\.vs\MetaGraffiti.Base\lut\0\t\".ToLowerInvariant();
			if (path.ToLowerInvariant().Contains(live)) path = path.ToLowerInvariant().Replace(live, @"\");

			var length = path.Length;
			if (path.EndsWith(@"\bin\debug", StringComparison.InvariantCultureIgnoreCase))
				return path.Substring(0, length - 10);
			else if (path.EndsWith(@"\bin\release", StringComparison.InvariantCultureIgnoreCase))
				return path.Substring(0, length - 11);
			else
				return Path.Combine(path, @"..\..\");
		}

		public static string GetTestDataDirectory()
		{
			return Path.Combine(GetTestRootDirectory(), "Data");
		}

		public static string GetTestDataFileUri(string filename)
		{
			return Path.Combine(TestsHelper.GetTestDataDirectory(), filename);
		}
	}
}