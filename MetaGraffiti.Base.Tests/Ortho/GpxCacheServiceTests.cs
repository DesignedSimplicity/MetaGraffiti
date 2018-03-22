using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Info;
using MetaGraffiti.Base.Services;

namespace MetaGraffiti.Base.Tests.Ortho
{
	// TODO: deprecate
	[TestClass]
	public class GpxCacheServiceTests
	{
		private BasicCacheService<GpxCache> _cache = new BasicCacheService<GpxCache>();

		public GpxCacheService GetService()
		{
			_cache = new BasicCacheService<GpxCache>();
			return new GpxCacheService(_cache);
		}

		[TestMethod]
		public void GpxCacheService_LoadFileTest()
		{
			var service = GetService();

			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Example1.gpx");
			var gpx = service.LoadFile(uri);

			Assert.IsNotNull(gpx);
			Assert.AreEqual(uri.ToLowerInvariant(), gpx.File.Uri.ToLowerInvariant());

			Assert.IsTrue(_cache.Contains(uri.ToLowerInvariant()));
		}

		[TestMethod]
		public void GpxCacheService_LoadDirectoryTest()
		{
			var service = GetService();

			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Example1.gpx");
			var dir = TestsHelper.GetTestDataDirectory(@"Gpx");
			var list = service.LoadDirectory(dir);

			Assert.IsNotNull(list);
			Assert.IsTrue(list.Count > 2);

			Assert.IsTrue(_cache.Contains(uri.ToLowerInvariant()));
		}

		[TestMethod]
		public void GpxCacheService_LoadDirectoryRecursiveTest()
		{
			var service = GetService();

			var uri = TestsHelper.GetTestDataFileUri(@"Gpx\Example1.gpx");
			var dir = TestsHelper.GetTestDataDirectory();
			var list = service.LoadDirectory(dir, true);

			Assert.IsNotNull(list);
			Assert.IsTrue(list.Count > 2);

			Assert.IsTrue(_cache.Contains(uri.ToLowerInvariant()));
		}
	}
}
