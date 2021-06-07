using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Ortho.Base;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XmpCore;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public class XmpDataReader
	{
		private string _uri;
		private Stream _stream;

		public List<Exception> Errors { get; set; } = new List<Exception>();

		public XmpDataReader(string uri)
		{
			_uri = uri;
		}

		public XmpDataReader(Stream stream)
		{
			_stream = stream;
		}

		public ImgXmpData ReadFile()
		{
			// read from stream
			if (_stream != null)
			{
				var xmp = XmpMetaFactory.Parse(_stream);
				return ParseData(xmp);
			}

			// fall back to uri
			using (var stream = File.OpenRead(_uri))
			{
				var xmp = XmpMetaFactory.Parse(stream);
				return ParseData(xmp);
			}
		}

		private ImgXmpData ParseData(IXmpMeta reader)
		{
			var data = new ImgXmpData();

			try
			{
				foreach (var property in reader.Properties)
				{
					switch (property.Path)
					{
						/*
						case "aux:Lens":
							data.Lens = property.Value;
							break;
						case "aux:SerialNumber":
							data.SerialNumber = property.Value;
							break;
						case "aux:LensSerialNumber":
							data.LensSerialNumber = property.Value;
							break;
						case "aux:FlashCompensation":
							data.FlashCompensation = property.Value;
							break;
						*/
						case "tiff:Make":
							data.CameraMake = property.Value;
							break;
						case "tiff:Model":
							data.CameraModel = property.Value;
							break;

						case "tiff:Orientation":
							data.Orientation = TypeConvert.ToInt(property.Value);
							break;
						case "tiff:ImageWidth":
							data.Width = TypeConvert.ToInt(property.Value);
							break;
						case "tiff:ImageLenght":
							data.Height = TypeConvert.ToInt(property.Value);
							break;

						case "exif:PixelXDimension":
							data.PixelsX = TypeConvert.ToInt(property.Value);
							break;
						case "exif:PixelYDimension":
							data.PixelsY = TypeConvert.ToInt(property.Value);
							break;

						case "exif:DateTaken":
							data.DateTaken = ParseDateTime(property.Value);
							break;
						case "exif:DateTimeOriginal":
							data.DateTimeOriginal = ParseDateTime(property.Value);
							break;
					}

					/*
					   tiff:Orientation="6"
					   exif:ExifVersion="0230"
					   exif:ExposureTime="1/125"
					   exif:ShutterSpeedValue="6965784/1000000"
					   exif:ExposureProgram="3"
					   exif:SensitivityType="1"
					   exif:ExposureBiasValue="0/10"
					   exif:MaxApertureValue="2/1"
					   exif:MeteringMode="5"
					   exif:LightSource="0"
					   exif:FocalLength="15/2"
					   exif:FileSource="3"
					   exif:FocalLengthIn35mmFilm="15"
					   exif:CustomRendered="0"
					   exif:ExposureMode="0"
					   exif:WhiteBalance="0"
					   exif:SceneCaptureType="0"
					   exif:GainControl="2"
					   exif:Contrast="0"
					   exif:Saturation="0"
					   exif:Sharpness="0"
					   exif:DigitalZoomRatio="100/100"
					   exif:FocalPlaneXResolution="87196351/32768"
					   exif:FocalPlaneYResolution="87196351/32768"
					   exif:FocalPlaneResolutionUnit="3"
					   exif:DateTimeOriginal="2019-01-05T11:13:32"
					   exif:PixelXDimension="4608"
					   exif:PixelYDimension="3456"
					*/
				}
			}
			catch (Exception ex)
			{
				this.Errors.Add(ex);
			}

			ImgUtilities.FixExifData(data);

			// return metadata
			return data;
		}

		protected DateTime ParseDateTime(string text)
		{
			return DateTime.Parse(text.Replace("T", " ")); //, "yyyy-MM-dd hh:mm:ss");
		}
	}
}
