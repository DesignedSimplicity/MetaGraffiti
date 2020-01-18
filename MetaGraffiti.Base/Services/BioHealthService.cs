using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services.Internal;

namespace MetaGraffiti.Base.Services
{
	public class PolarTrainingInfo : ICacheEntity
	{
		public string Key { get; set; }
		public TcxFileData Data { get; set; }
	}

	public class BioHealthService
	{
		// ==================================================
		// Internals
		private static string _rootUri = "";
		private static object _init = false;
		private static BasicCacheService<PolarTrainingInfo> _polar;
		private CartoPlaceService _cartoPlaceService;

		public BioHealthService()
		{
		}


		// ==================================================
		// Methods

		/// <summary>
		/// Initializes repository with all local GPX files
		/// </summary>
		public void Init(string uri)
		{
			lock (_init) // TODO: use internal init for basic cache
			{
				if (Convert.ToBoolean(_init)) return;

				_polar = new BasicCacheService<PolarTrainingInfo>();
				var root = new DirectoryInfo(uri);
				_rootUri = root.FullName;
				foreach (var year in root.EnumerateDirectories())
				{
					foreach (var file in year.EnumerateFiles("*.zip"))
					{
						var training = LoadTraining(file);
						_polar.Add(training);
					}
				}
				_init = true;
			}
		}

		/// <summary>
		/// Clears the GPX file cache
		/// </summary>
		public void ResetCache()
		{
			_polar = null;
			_init = false;
		}


		// ==================================================
		// Helpers
		private PolarTrainingInfo LoadTraining(FileInfo file)
		{
			var train = new PolarTrainingInfo();

			using (var f = File.OpenRead(file.FullName))
			{
				using (var zip = new ZipArchive(f, ZipArchiveMode.Read))
				{
					foreach (var entry in zip.Entries)
					{
						using (var stream = entry.Open())
						{
							// load tcx data
							var reader = new TcxFileReader(stream);
							var data = reader.ReadFile();
							train.Data = data;

							// setup trail details
							var filename = Path.GetFileNameWithoutExtension(file.Name);
							train.Key = filename.ToUpperInvariant();
						}
					}
				}
			}

			return train;
		}
	}
}
