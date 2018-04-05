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
}
