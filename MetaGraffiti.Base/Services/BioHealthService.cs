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
		public string Uri { get; set; }
		public DateTime Timestamp { get; set; }
		public TcxFileData Data { get; set; }
	}

	public class BioHealthService
	{
		// ==================================================
		// Internals
		private static string _rootUri = "";
		private static object _init = false;
		private static BasicCacheService<PolarTrainingInfo> _polar;

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
						var filename = Path.GetFileNameWithoutExtension(file.Name);

						// cache file list for later
						var training = new PolarTrainingInfo();
						training.Uri = file.FullName;
						training.Key = filename.ToUpperInvariant();

						// parse out datetime
						var index = filename.Length - 19;
						var parts = filename.Substring(index).Split('_');
						var time = parts[1].Replace("-", ":"); // + "Z";
						training.Timestamp = DateTime.Parse(parts[0] + " " + time);

						// add to partial cache
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

		public List<PolarTrainingInfo> GetTrainingInfo(DateTime start, DateTime finish)
		{
			var list = _polar.All.Where(x => x.Timestamp >= start && x.Timestamp <= finish).ToList();
			foreach(var item in list)
			{
				// load data if not yet cached
				if (item.Data == null)
				{
					item.Data = LoadTrainingData(item.Uri);
				}
			}
			return list;
		}

		// ==================================================
		// Helpers
		private TcxFileData LoadTrainingData(string uri)
		{
			using (var f = File.OpenRead(uri))
			{
				using (var zip = new ZipArchive(f, ZipArchiveMode.Read))
				{
					var entry = zip.Entries.First();
					using (var stream = entry.Open())
					{
						// load tcx data
						var reader = new TcxFileReader(stream);
						var data = reader.ReadFile();
						return data;
					}
				}
			}
		}
	}
}
