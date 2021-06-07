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

		public ImgFileData ReadFile()
		{
			try
			{
				var file = _uri.ToLowerInvariant();
				if (file.EndsWith(".jpg"))
				{
					MetaData = new JpgExifReader(_uri).ReadFile();
					return new ImgFileData(_uri, MetaData);
				}

				var xmp = Path.Combine(Path.GetDirectoryName(_uri), Path.GetFileNameWithoutExtension(_uri)) + ".xmp";
				if (File.Exists(xmp))
				{
					MetaData = new XmpDataReader(xmp).ReadFile();
					return new ImgFileData(_uri, MetaData);
				}

				if (file.EndsWith(".xmp"))
				{
					MetaData = new XmpDataReader(_uri).ReadFile();
					return new ImgFileData(_uri, MetaData);
				}

				using (var stream = File.OpenRead(_uri))
				{
					MetaData = ReadMagick(stream);
					return new ImgFileData(_uri, MetaData);
				}
			}
			catch (Exception ex)
			{
				Errors.Add(new Exception($"Error parsing: {_uri}", ex));
			}

			return new ImgFileData(_uri, MetaData);
		}

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

			data.Width = reader.Width;
			data.Height = reader.Height;

			//data.DateTaken = DateTime.MinValue;
			var iptc = reader.GetIptcProfile();
			var xmp = reader.GetXmpProfile();
			if (xmp != null)
			{
				XmlNodeReader xmlr = null;
				try
				{
					var s = Encoding.UTF8.GetString(xmp.ToByteArray());
					XmlDocument xml = new XmlDocument();
					xml.LoadXml(s);
					xmlr = new XmlNodeReader(xml.DocumentElement);
					while (xmlr.Read())
					{
						var n = xmlr.Name;
						var v = xmlr.Value;

						/*
						if (n == "rdf:Description")
						{
							data.Rating = TypeConvert.ToInt(xmlr.GetAttribute("xmp:Rating"));
							break;
						}*/
					}
				}
				catch { }
				if (xmlr != null) xmlr.Dispose();
			}

			var exif = reader.GetExifProfile();
			if (exif != null && exif.Values != null)
			{
				foreach (var e in exif.Values)
				{
					var tag = e.Tag.ToString();
					
					var log = $"Tag={tag} Value={e}";
					Console.WriteLine(log);
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
						case "DateTaken":
							data.DateTaken = ParseDateTime(e.ToString());
							break;
						case "DateTimeOriginal":
							data.DateTimeOriginal = ParseDateTime(e.ToString());
							break;
						case "GPSAltitude":
							data.Altitude = TypeConvert.ToDecimal(e.ToString());
							break;
						case "GPSLatitude":
							data.Latitude = ParseGPS(e.ToString());
							break;
						case "GPSLatitudeRef":
							latref = (ParseText(e.ToString()) == "S" ? -1 : 1);
							break;
						case "GPSLongitude":
							data.Longitude = ParseGPS(e.ToString());
							break;
						case "GPSLongitudeRef":
							lonref = (ParseText(e.ToString()) == "W" ? -1 : 1);
							break;
						/*
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

			// fix GPS signs
			data.Longitude = data.Longitude * lonref;
			data.Latitude = data.Latitude * latref;

			// backfill missing values
			if (data.Width == 0) data.Width = data.PixelsX;
			if (data.Height == 0) data.Height = data.PixelsY;
			if (data.DateTimeOriginal.HasValue)
				data.DateCreated = data.DateTimeOriginal.Value; 
			else if (data.DateTaken.HasValue)
				data.DateCreated = data.DateTaken.Value;
			else
				data.DateCreated = DateTime.MinValue;

			// return metadata
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

		protected Tuple<int, int> ReadJpegDimension(string uri)
		{
			int width = 0;
			int height = 0;
			bool found = false;
			bool eof = false;

			using (var stream = new FileStream(uri, FileMode.Open, FileAccess.Read))
			{
				using (var reader = new BinaryReader(stream))
				{
					try
					{
						while (!found || !eof)
						{
							// read 0xFF and the type
							reader.ReadByte();
							byte type = reader.ReadByte();

							// get length
							int len = 0;
							switch (type)
							{
								// start and end of the image
								case 0xD8:
								case 0xD9:
									len = 0;
									break;

								// restart interval
								case 0xDD:
									len = 2;
									break;

								// the next two bytes is the length
								default:
									int lenHi = reader.ReadByte();
									int lenLo = reader.ReadByte();
									len = (lenHi << 8 | lenLo) - 2;
									break;
							}

							if (len > 0)
							{
								byte[] data = reader.ReadBytes(len);
								if (type == 0xC0)
								{
									width = data[1] << 8 | data[2];
									height = data[3] << 8 | data[4];
									found = true;
								}

							}
							if (type == 0xD9) eof = true;
						}
					}
					catch { }
				}
			}
			return new Tuple<int, int>(width, height);
		}
	}
}
