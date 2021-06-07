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
	public class JpgFileReader
	{
		private string _uri;
		private Stream _stream;

		public List<Exception> Errors = new List<Exception>();

		public JpgFileReader(string uri)
		{
			_uri = uri;
		}

		public JpgFileReader(Stream stream)
		{
			_stream = stream;
		}

		public JpgFileData ReadFile()
		{
			var jpg = new JpgFileData();

			jpg.File = ReadFileData();
			jpg.Dimenions = ReadDimension();

			try
			{
				if (_stream != null)
					jpg.Exif = new JpgExifReader(_stream).ReadFile();
				else
					jpg.Exif = new JpgExifReader(_uri).ReadFile();
			}
			catch (Exception ex)
			{
				Errors.Add(ex);
			}

			return jpg;
		}

		public IImgFileData ReadFileData()
		{
			try
			{
				if (!String.IsNullOrWhiteSpace(_uri))
				{
					var file = new FileInfo(_uri);
					return new JpegFileData()
					{
						FileSize = file.Length,
						FileName = file.Name,
						DateCreated = file.CreationTime,
						DateUpdated = file.LastWriteTimeUtc,
					};
				}
				else if (_stream != null)
				{
					return new JpegFileData()
					{
						FileSize = _stream.Length,
						FileName = null,
						DateCreated = DateTime.MinValue,
						DateUpdated = null,
					};
				}
			}
			catch (Exception ex)
			{
				Errors.Add(ex);
			}

			return new JpegFileData()
			{
				FileSize = -1,
				FileName = null,
				DateCreated = DateTime.MinValue,
				DateUpdated = null,
			};
		}

		public IImgDimenions ReadDimension()
		{
			if (_stream != null)
				return ReadDimension(_stream);
			else
			{
				using (var stream = File.OpenRead(_uri))
				{
					return ReadDimension(stream);
				}
			}
		}

		private IImgDimenions ReadDimension(Stream stream)
		{
			int width = 0;
			int height = 0;
			bool found = false;
			bool eof = false;

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
				catch (Exception ex)
				{
					Errors.Add(ex);
				}
			}
			return new JpegDimensions()
			{
				Width = width,
				Height = height,
			};
		}

		private class JpegDimensions : IImgDimenions
		{
			public int Width { get; set; }
			public int Height { get; set; }
		}

		private class JpegFileData : IImgFileData
		{
			public long FileSize { get; set; }			
			public string FileName { get; set; }
			public DateTime DateCreated { get; set; }
			public DateTime? DateUpdated { get; set; }
		}
	}
}
