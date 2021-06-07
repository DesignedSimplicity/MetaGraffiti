using ExifLib;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public class JpgExifReader : ImgMetaReader
	{
		private string _uri;
		private Stream _stream;

		public List<Exception> Errors { get; set; } = new List<Exception>();

		public JpgExifReader(string uri)
		{
			_uri = uri;
		}

		public JpgExifReader(Stream stream)
		{
			_stream = stream;
		}

		public ImgExifData ReadFile()
		{
			// read from stream
			if (_stream != null)
			{
				using (var reader = new ExifReader(_stream))
				{
					var exif = ParseData(reader);
					return exif;
				}
			}

			// fall back to uri
			using (var reader = new ExifReader(_uri))
			{
				var exif = ParseData(reader);
				return exif;
			}
		}

		protected ImgExifData ParseData(ExifReader reader)
		{
			var data = new ImgExifData();
			try
			{
				object tag;
				
				reader.GetTagValue(ExifTags.Make, out tag);
				data.CameraMake = TypeConvert.ToString(tag).Trim();

				reader.GetTagValue(ExifTags.Model, out tag);
				data.CameraModel = TypeConvert.ToString(tag).Trim();

				/*
				reader.GetTagValue(ExifTags.ApertureValue, out tag);
				if (tag != null) data.Aperture = TypeConvert.ToDecimal(tag);

				reader.GetTagValue(ExifTags.FNumber, out tag);
				if (tag != null) data.FNumber = TypeConvert.ToDecimal(tag);

				reader.GetTagValue(ExifTags.FocalLength, out tag);
				if (tag != null) data.FocalLength = TypeConvert.ToDecimal(tag);

				reader.GetTagValue(ExifTags.FocalLengthIn35mmFilm, out tag);
				if (tag != null) data.FocalEquivalence = TypeConvert.ToDecimal(tag);

				reader.GetTagValue(ExifTags.ExposureTime, out tag);
				if (tag != null) data.ExposureTime = TypeConvert.ToDecimal(tag);

				reader.GetTagValue(ExifTags.ShutterSpeedValue, out tag);
				if (tag != null) data.ShutterSpeed = TypeConvert.ToDecimal(tag);

				reader.GetTagValue(ExifTags.PhotographicSensitivity, out tag);
				if (tag != null) data.ISO = Convert.ToInt64(tag);

				reader.GetTagValue(ExifTags.XPTitle, out tag);
				if (tag != null && (tag is byte[]))
					data.Title = Encoding.Default.GetString((byte[])tag);
				else
					data.Title = TypeConvert.ToString(tag).Trim();

				reader.GetTagValue(ExifTags.ImageDescription, out tag);
				if (tag != null && (tag is byte[]))
					data.Description = Encoding.Default.GetString((byte[])tag);
				else
					data.Description = TypeConvert.ToString(tag).Trim();

				reader.GetTagValue(ExifTags.UserComment, out tag);
				if (tag != null && (tag is byte[]))
					data.Comment = Encoding.Default.GetString((byte[])tag);
				else
					data.Comment = TypeConvert.ToString(tag).Trim();

				reader.GetTagValue(ExifTags.Copyright, out tag);
				if (tag != null && (tag is byte[]))
					data.Copyright = Encoding.Default.GetString((byte[])tag);
				else
					data.Copyright = TypeConvert.ToString(tag).Trim();
				*/

				reader.GetTagValue(ExifTags.PixelXDimension, out tag);
				data.PixelsX = TypeConvert.ToInt(tag);

				reader.GetTagValue(ExifTags.PixelYDimension, out tag);
				data.PixelsY = TypeConvert.ToInt(tag);

				reader.GetTagValue(ExifTags.ImageWidth, out tag);
				data.Width = TypeConvert.ToInt(tag);

				reader.GetTagValue(ExifTags.ImageLength, out tag);
				data.Height = TypeConvert.ToInt(tag);

				reader.GetTagValue(ExifTags.Orientation, out tag);
				if (tag != null) data.Orientation = TypeConvert.ToInt(tag);

				reader.GetTagValue(ExifTags.DateTimeOriginal, out tag);
				if (tag != null) data.DateTimeOriginal = base.ParseDateTime(tag);
				
				reader.GetTagValue(ExifTags.DateTime, out tag);
				if (tag != null) data.DateTaken = base.ParseDateTime(tag);

				reader.GetTagValue(ExifTags.GPSLatitude, out tag);
				if (tag != null)
				{
					double[] lat = (double[])tag;
					data.Latitude = TypeConvert.ToDecimal(lat[0] + lat[1] / 60 + lat[2] / 3600);

					reader.GetTagValue<string>(ExifTags.GPSLatitudeRef, out string latref);
					if (latref == "S") data.Latitude = 0 - data.Latitude;
				}

				reader.GetTagValue(ExifTags.GPSLongitude, out tag);
				if (tag != null)
				{
					double[] lng = (double[])tag;
					data.Longitude = TypeConvert.ToDecimal(lng[0] + lng[1] / 60 + lng[2] / 3600);

					reader.GetTagValue<string>(ExifTags.GPSLongitudeRef, out string lngref);
					if (lngref == "W") data.Longitude = 0 - data.Longitude;
				}

				reader.GetTagValue(ExifTags.GPSAltitude, out tag);
				if (tag != null) data.Altitude = TypeConvert.ToDecimal(tag);

			}
			catch (Exception ex)
			{
				this.Errors.Add(ex);
			}

			base.FixExifData(data);

			// return metadata
			return data;
		}
	}
}
