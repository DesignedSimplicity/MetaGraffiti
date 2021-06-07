using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public interface IGpxFileHeader
	{
		string Name { get; }
		string Keywords { get; }
		string Description { get; }

		string UrlLink { get; }
		string UrlText { get; }

		//DateTime? Timestamp { get; }
	}

	public interface IGpxTrack
	{
		string Name { get; }
		string Source { get; }
		string Description { get; }

		IList<IGpxPoint> Points { get; }
	}

	public interface IGpxPoint : IGeoPoint
	{
		int Segment { get; }
		int? Sats { get; }
		decimal? HDOP { get; }
		decimal? VDOP { get; }
		decimal? PDOP { get; }
		decimal? Speed { get; }
		decimal? Course { get; }
		string Source { get; }
	}


	public interface IImgMetaData
	{
		int Width { get; }
		int Height { get; }

		int PixelsX { get; }
		int PixelsY { get; }

		DateTime? DateTimeOriginal { get; }
		DateTime? DateTaken { get; }

		string Title { get; }
		string Comment { get; }
		string Copyright { get; }
		string Description { get; }
		string CameraMake { get; }
		string CameraModel { get; }

		decimal? ShutterSpeed { get; }
		decimal? ExposureTime { get; }      // seconds
		decimal? FocalLength { get; }       // on lens
		decimal? FocalEquivalence { get; }  // at 35 equ
		decimal? Aperture { get; }          // f1/a
		decimal? FNumber { get; }           // f
		decimal? Altitude { get; }
		decimal? Longitude { get; }
		decimal? Latitude { get; }

		int? Orientation { get; }
		long? ISO { get; }

		/*
		// Xmp:Aux
		string Lens { get; }
		string SerialNumber { get; }
		string LensSerialNumber { get; }
		string FlashCompensation { get; }
		*/
	}
}
