using ExifLib;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Foto;
using MetaGraffiti.Base.Modules.Foto.Data;
using MetaGraffiti.Base.Modules.Foto.Info;
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
			var reader = new ExifReader(file.FullName);
			var exif = ParseExif(reader);

			var image = new FotoImageInfo();
			image.Exif = exif;
			return image;
		}




		protected IFotoExif ParseExif(ExifReader reader)
		{
			var exif = new FotoExifData();

			object tag;

			reader.GetTagValue(ExifTags.XPTitle, out tag);
			if (tag != null && (tag is byte[]))
				exif.Title = System.Text.Encoding.Default.GetString((byte[])tag);
			else
				exif.Title = TypeConvert.ToString(tag).Trim();

			reader.GetTagValue(ExifTags.ImageDescription, out tag);
			if (tag != null && (tag is byte[]))
				exif.Description = System.Text.Encoding.Default.GetString((byte[])tag);
			else
				exif.Description = TypeConvert.ToString(tag).Trim();

			reader.GetTagValue(ExifTags.UserComment, out tag);
			if (tag != null && (tag is byte[]))
				exif.Comment = System.Text.Encoding.Default.GetString((byte[])tag);
			else
				exif.Comment = TypeConvert.ToString(tag).Trim();

			reader.GetTagValue(ExifTags.Copyright, out tag);
			if (tag != null && (tag is byte[]))
				exif.Copyright = System.Text.Encoding.Default.GetString((byte[])tag);
			else
				exif.Copyright = TypeConvert.ToString(tag).Trim();

			reader.GetTagValue(ExifTags.Make, out tag);
			exif.CameraMake = TypeConvert.ToString(tag).Trim();

			reader.GetTagValue(ExifTags.Model, out tag);
			exif.CameraModel = TypeConvert.ToString(tag).Trim();

			reader.GetTagValue(ExifTags.ApertureValue, out tag);
			exif.Aperture = TypeConvert.ToDecimal(tag);

			reader.GetTagValue(ExifTags.FNumber, out tag);
			exif.FNumber = TypeConvert.ToDecimal(tag);

			reader.GetTagValue(ExifTags.FocalLength, out tag);
			exif.FocalLength = TypeConvert.ToDecimal(tag);

			reader.GetTagValue(ExifTags.FocalLengthIn35mmFilm, out tag);
			exif.FocalEquivalence = TypeConvert.ToDecimal(tag);

			reader.GetTagValue(ExifTags.ExposureTime, out tag);
			exif.ExposureTime = TypeConvert.ToDecimal(tag);

			reader.GetTagValue(ExifTags.ShutterSpeedValue, out tag);
			exif.ShutterSpeed = TypeConvert.ToDecimal(tag);

			reader.GetTagValue(ExifTags.PhotographicSensitivity, out tag);
			exif.ISO = Convert.ToInt64(tag);

			reader.GetTagValue(ExifTags.PixelXDimension, out tag);
			exif.Width = TypeConvert.ToInt(tag);

			reader.GetTagValue(ExifTags.PixelYDimension, out tag);
			exif.Height = TypeConvert.ToInt(tag);

			reader.GetTagValue(ExifTags.Orientation, out tag);
			exif.Orientation = TypeConvert.ToInt(tag);

			reader.GetTagValue(ExifTags.DateTimeOriginal, out tag);
			if (tag != null)
				exif.DateTaken = ParseDateTime(tag);
			else
			{
				reader.GetTagValue(ExifTags.DateTime, out tag);
				if (tag != null) exif.DateTaken = ParseDateTime(tag);
			}

			reader.GetTagValue(ExifTags.GPSLatitude, out tag);
			if (tag != null)
			{
				double[] lat = (double[])tag;
				exif.Latitude = TypeConvert.ToDecimal(lat[0] + lat[1] / 60 + lat[2] / 3600);

				reader.GetTagValue<string>(ExifTags.GPSLatitudeRef, out string latref);
				if (latref == "S") exif.Latitude = 0 - exif.Latitude;
			}

			reader.GetTagValue(ExifTags.GPSLongitude, out tag);
			if (tag != null)
			{
				double[] lng = (double[])tag;
				exif.Longitude = TypeConvert.ToDecimal(lng[0] + lng[1] / 60 + lng[2] / 3600);

				reader.GetTagValue<string>(ExifTags.GPSLongitudeRef, out string lngref);
				if (lngref == "W") exif.Longitude = 0 - exif.Longitude;
			}

			reader.GetTagValue(ExifTags.GPSAltitude, out tag);
			if (tag != null) exif.Altitude = TypeConvert.ToDecimal(tag);

			// calculate missing data
			/*
			Camera = FotoGraffiti.GetCamera(CameraModel);
			if (Camera != FotoCamera.Unknown && FocalEquivalence == 0)
			{
				FocalEquivalence = FocalLength * FotoGraffiti.GetCropFactor(Camera);
			}
			*/
			/*if (Width == 0 || Height == 0)
			{
				Size s = FotoGraffiti.ReadJpegDimension(uri);
				Height = s.Height;
				Width = s.Width;
			}*/

			return exif;
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
