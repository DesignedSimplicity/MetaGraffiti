using ImageMagick;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public class ImgFileReader
	{
		private string _uri;
		private Stream _stream;

		public List<Exception> Errors { get; set; } = new List<Exception>();

		public ImgFileReader(string uri)
		{
			_uri = uri;
		}

		public IImgMetaData ReadFile()
		{
			var file = _uri.ToLowerInvariant();
			if (file.EndsWith(".jpg"))
			{
				return new JpgExifReader(_uri).ReadFile();
			} 

			var xmp = Path.Combine(Path.GetDirectoryName(_uri), Path.GetFileNameWithoutExtension(_uri)) + ".xmp";
			if (File.Exists(xmp))
			{ 
				return new XmpDataReader(xmp).ReadFile();
			}

			return null;
		}

		/*
		public ImgMetaReader(Stream stream)
		{
			_stream = stream;
		}

		public ImgXmpData ReadFile()
		{
			// read from stream
			if (_stream != null)
			{
				return ReadMagick(_stream);
			}

			// fall back to uri
			using (var stream = File.OpenRead(_uri))
			{
				return ReadMagick(stream);
			}
		}
		*/

		private ImgXmpData ReadMagick(Stream stream)
		{
			var data = new ImgXmpData();
			using (var magik = new MagickImage(stream))
			{
				var xmp = magik.GetXmpProfile();
				if (xmp != null)
				{
					using (var reader = xmp.CreateReader())
					{
					}
				}

				//var exif = magik.GetExifProfile();

				/*
				using (var reader = xmp.CreateReader())
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(reader);

					var lens = doc.SelectSingleNode("//*:xmpmeta/*:Description/*:Lens");

					//var txt = doc.OuterXml;
					Assert.IsNotNull(lens.InnerText);
				}
				*/

				//Assert.AreEqual("Hiking", d.Activities.First().TrainingName);
			}
			return data;
		}
	}
}
