using System;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class GpxPointData : GpxMetaData, IGpxPoint, IGeoPoint
	{
		public GpxPointTypes GpxPointType { get; set; }
		public int Segment { get; set; }

		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double? Elevation { get; set; }
		public DateTime? Timestamp { get; set; }

		public int? Sats { get; set; }
		public decimal? HDOP { get; set; }
		public decimal? VDOP { get; set; }
		public decimal? PDOP { get; set; }
		public decimal? Speed { get; set; }
		public decimal? Course { get; set; }

		public decimal? MagVariation;
		public decimal? GeoIDHeight;

		public string Symbol;
		public string Category;

		public string Fix;

		public TimeSpan? Decay;
		public string DpgsID;


		// TODO: deprecate
		public decimal GetDOP()
		{
			var h = HDOP ?? 0;
			var v = VDOP ?? 0;
			var p = PDOP ?? 0;
			var b = h;
			if (v > b) b = v;
			if (p > b) b = p;
			return b;
		}

		// TODO: deprecate
		public decimal MaxDOP
		{
			get
			{
				var h = HDOP ?? 11;
				var v = VDOP ?? 11;
				var p = PDOP ?? 11;
				var b = h;
				if (v > b) b = v;
				if (p > b) b = p;
				return b;
			}
		}
	}
}