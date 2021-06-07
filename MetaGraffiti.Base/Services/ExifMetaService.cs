using ExifLib;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Foto;
using MetaGraffiti.Base.Modules.Foto.Data;
using MetaGraffiti.Base.Modules.Foto.Info;
using MetaGraffiti.Base.Modules.Ortho;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaGraffiti.Base.Services
{
	public class ExifMetaService
	{
		public IList<FotoImageInfo> ListImages(string uri)
		{
			var images = new List<FotoImageInfo>();
			var dir = new DirectoryInfo(uri);
			foreach (var file in dir.EnumerateFiles("*.jpg"))
			{
				var image = LoadImage(file);
				images.Add(image);
			}
			return images;
		}

		public FotoImageInfo LoadImage(FileInfo file)
		{
			var reader = new ImgFileReader(file.FullName);

			var image = new FotoImageInfo();
			image.Exif = reader.ReadFile().Metadata;
			return image;
		}
	}
}
