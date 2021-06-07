using MetaGraffiti.Base.Modules.Ortho;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Foto.Info
{
	public class FotoImageInfo
	{
		public IImgMetaData Exif { get; set; }

		public bool HasCoordinates => Exif != null && Exif.Latitude.HasValue && Exif.Longitude.HasValue;
	}
}
