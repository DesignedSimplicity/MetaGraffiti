using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public abstract class ImgMetaReader
	{
		private readonly static List<int> _swapWH = new List<int> { 6 };

		protected void FixExifData(ImgExifData data)
		{
			// adjust orientation
			if (data.Width == 0) data.Width = data.PixelsX;
			if (data.Height == 0) data.Height = data.PixelsY;
			if (data.Orientation.HasValue && _swapWH.Contains(data.Orientation.Value))
			{
				var swap = data.Width;
				data.Width = data.Height;
				data.Height = swap;
			}

			// backfill missing values
			if (data.DateTimeOriginal.HasValue)
				data.DateCreated = data.DateTimeOriginal.Value;
			else if (data.DateTaken.HasValue)
				data.DateCreated = data.DateTaken.Value;
			else
				data.DateCreated = DateTime.MinValue;
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
		protected DateTime ParseDateTimeISO8601(string text)
		{
			return DateTime.Parse(text.Replace("T", " ")); //, "yyyy-MM-dd hh:mm:ss");
		}

	}
}
