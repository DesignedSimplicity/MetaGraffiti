using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Foto
{
	interface IFotoFile
	{

	}

	public interface IFotoExif
	{
		string Camera { get; set; } //= FotoCamera.Unknown;
		DateTime DateTaken { get; set; } //= DateTime.Now;
		int Width { get; set; }
		int Height { get; set; }
		string Title { get; set; }
		string Comment { get; set; }
		string Copyright { get; set; }
		string Description { get; set; }
		string CameraMake { get; set; }
		string CameraModel { get; set; }
		decimal ShutterSpeed { get; set; }
		decimal ExposureTime { get; set; }      // seconds
		decimal FocalLength { get; set; }           // on lens
		decimal FocalEquivalence { get; set; }  // at 35 equ
		decimal Aperture { get; set; }          // f1/a
		decimal FNumber { get; set; }           // f
		decimal? Altitude { get; set; }
		decimal? Longitude { get; set; }
		decimal? Latitude { get; set; }
		int Orientation { get; set; }
		long ISO { get; set; }

		bool Portrait { get; }
		bool Landscape { get; }
		decimal Megapixels { get; }
		Exception Error { get; set; }
	}
}
