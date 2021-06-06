using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaGraffiti.Base.Modules.Foto.Data
{
	public class FotoSourceData
	{
		public FotoFile File { get; set; }

		
		
	}

	public class FotoExifData : IFotoExif
	{
		public Exception Error { get; set; }

		public string Camera { get; set; } //= FotoCamera.Unknown;
		public DateTime DateTaken { get; set; } //= DateTime.Now;
		public int Width { get; set; }
		public int Height { get; set; }
		public string Title { get; set; }
		public string Comment { get; set; }
		public string Copyright { get; set; }
		public string Description { get; set; }
		public string CameraMake { get; set; }
		public string CameraModel { get; set; }
		public decimal ShutterSpeed { get; set; }
		public decimal ExposureTime { get; set; }       // seconds
		public decimal FocalLength { get; set; }            // on lens
		public decimal FocalEquivalence { get; set; }   // at 35 equ
		public decimal Aperture { get; set; }           // f1/a
		public decimal FNumber { get; set; }            // f
		public decimal? Altitude { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public int Orientation { get; set; }
		public long ISO { get; set; }

		public bool Portrait { get { return !Landscape; } }
		public bool Landscape { get { return Orientation == 1; } }
		public decimal Megapixels { get { return Convert.ToDecimal(1.0 * Width * Height / 1024 / 1024); } }
	}
}
