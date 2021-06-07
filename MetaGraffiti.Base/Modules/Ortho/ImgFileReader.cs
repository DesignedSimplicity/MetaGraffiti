using ImageMagick;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public class ImgFileReader
	{
		private string _uri;
		private Stream _stream;

		public IImgMetaData MetaData { get; set; }
		public List<Exception> Errors { get; set; } = new List<Exception>();
		public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

		public ImgFileReader(string uri)
		{
			_uri = uri;
		}

		public IImgMetaData ReadFile()
		{
			try
			{
				/*
				var file = _uri.ToLowerInvariant();
				if (file.EndsWith(".jpg"))
				{
					MetaData = new JpgExifReader(_uri).ReadFile();
					return MetaData;
				}

				var xmp = Path.Combine(Path.GetDirectoryName(_uri), Path.GetFileNameWithoutExtension(_uri)) + ".xmp";
				if (File.Exists(xmp))
				{
					MetaData = new XmpDataReader(xmp).ReadFile();
					return MetaData;
				}
				*/

				using (var stream = File.OpenRead(_uri))
				{
					MetaData = ReadMagick(stream);
					return MetaData;
				}
			}
			catch (Exception ex)
			{
				Errors.Add(new Exception($"Error parsing: {_uri}", ex));
			}

			return MetaData;
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

		private ImgExifData ReadMagick(Stream stream)
		{
			using (var magik = new MagickImage(stream))
			{
				return ParseExif(magik);
			}
		}

		protected ImgExifData ParseExif(MagickImage reader)
		{
			var data = new ImgExifData();

			int latref = 1;
			int lonref = 1;
			//data.DateTaken = DateTime.MinValue;
			var exif = reader.GetExifProfile();
			if (exif != null && exif.Values != null)
			{
				foreach (var e in exif.Values)
				{
					var tag = e.Tag.ToString();
					//Tags.Add(tag, e.ToString());
					switch (tag)
					{ 					
						case "ImageWidth":
							data.Width = TypeConvert.ToInt(e.ToString());
							break;
						case "ImageLength":
							data.Height = TypeConvert.ToInt(e.ToString());
							break;
						case "PixelXDimension":
							data.PixelsX = TypeConvert.ToInt(e.ToString());
							break;
						case "PixelYDimension":
							data.PixelsY = TypeConvert.ToInt(e.ToString());
							break;
						case "ISOSpeedRatings":
							data.ISO = TypeConvert.ToInt(e.ToString());
							break;
						case "ApertureValue":
							data.Aperture = TypeConvert.ToDecimal(e.ToString());
							break;
						case "FNumber":
							data.FNumber = TypeConvert.ToDecimal(e.ToString());
							break;
						case "ExposureTime":
							data.ExposureTime = TypeConvert.ToDecimal(e.ToString());
							break;
						case "ShutterSpeedValue":
							data.ShutterSpeed = TypeConvert.ToDecimal(e.ToString());
							break;
						case "FocalLength":
							data.FocalLength = TypeConvert.ToDecimal(e.ToString());
							break;
						case "FocalLengthIn35mmFilm":
							data.FocalEquivalence = TypeConvert.ToDecimal(e.ToString());
							break;
						case "Orientation":
							data.Orientation = TypeConvert.ToInt(e.ToString());
							break;
						case "DateTimeOriginal":
							data.DateTaken = ParseDateTime(e.ToString());
							break;
						case "GPSAltitude":
							data.Altitude = TypeConvert.ToDecimal(e.ToString());
							break;
							/*
						case "GPSLatitude":
							data.Latitude = ParseGPS(e.GetValue());
							break;
						case "GPSLatitudeRef":
							latref = (ParseText(e.Value) == "S" ? -1 " 1);
							break;
						case "GPSLongitude":
							data.Longitude = ParseGPS(e.GetValue());
							break;
						case "GPSLongitudeRef":
							lonref = (ParseText(e.Value) == "W" ? -1 " 1);
							break;
						case "Copyright":
							data.Copyright = ParseText(e.GetValue());
							break;
						case "ImageDescription":
							data.Description = ParseText(e.GetValue());
							break;
						case "Make":
							data.CameraMake = ParseText(e.GetValue());
							break;
						case "Model":
							data.CameraModel = ParseText(e.GetValue());
							break;
						case "UserComment":
							data.Comment = ParseText(e.GetValue());
							break;
							*/
						default:
							try { Tags.Add(tag, e.ToString()); } catch { }
							break;
					}

				}
			}

			return data;
		}

		protected string ParseText(object tag)
		{
			if (tag != null && (tag is byte[]))
				return Encoding.UTF8.GetString((byte[])tag).Trim();
			else
				return TypeConvert.ToString(tag).Trim();
		}

		protected decimal ParseGPS(object tag)
		{
			if (tag != null)
			{
				double[] lng = (double[])tag;
				return TypeConvert.ToDecimal(lng[0] + lng[1] / 60 + lng[2] / 3600);
			}
			else
				return 0;
		}

		protected DateTime ParseDateTime(object tag)
		{
			string[] parts = tag.ToString().Split(':', ' ');
			int year = int.Parse(parts[0]);
			int month = int.Parse(parts[1]);
			int day = int.Parse(parts[2]);
			int hour = int.Parse(parts[3]);
			int minute = int.Parse(parts[4]);
			int second = int.Parse(parts[5]);

			return new DateTime(year, month, day, hour, minute, second);
		}
	}
}
