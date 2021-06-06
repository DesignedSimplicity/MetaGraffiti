﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class ImgExifData : IImgMetaData
	{
		public int PixelsX { get; set; }
		public int PixelsY { get; set; }

		public DateTime? DateTaken { get; set; }

		public string Title { get; set; }
		public string Comment { get; set; }
		public string Copyright { get; set; }
		public string Description { get; set; }
		public string CameraMake { get; set; }
		public string CameraModel { get; set; }

		public decimal? ShutterSpeed { get; set; }
		public decimal? ExposureTime { get; set; }      // seconds
		public decimal? FocalLength { get; set; }       // on lens
		public decimal? FocalEquivalence { get; set; }  // at 35 equ
		public decimal? Aperture { get; set; }          // f1/a
		public decimal? FNumber { get; set; }           // f
		public decimal? Altitude { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }

		public int? Orientation { get; set; }
		public long? ISO { get; set; }
	}
}
