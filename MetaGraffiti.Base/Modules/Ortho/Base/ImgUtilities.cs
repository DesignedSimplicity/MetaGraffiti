using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Base
{
	// TODO:FOTO:MOVE
	public class ImgUtilities
	{
		private readonly static List<int> _swapWH = new List<int> { 6 };

		public static void FixExifData(ImgExifData data)
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
	}
}
