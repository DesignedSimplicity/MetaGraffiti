using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Foto.Info
{
	public class FotoImageInfo
	{
		public IFotoExif Exif { get; set; }

		public bool HasCoordinates => Exif != null && Exif.Latitude.HasValue && Exif.Longitude.HasValue;
	}
}
