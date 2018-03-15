using System;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class GpxPointData : GpxMetaData, IGeoPoint
	{
		public GpxPointTypes GpxPointType;
		public int Segment;

		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double? Elevation { get; set; }
		public DateTime? Timestamp { get; set; }

		public decimal? Course;
		public decimal? Speed;

		public decimal? MagVariation;
		public decimal? GeoIDHeight;

		public string Symbol;
		public string Category;

		public string Fix;
		public int? Sats;
		public decimal? HDOP;
		public decimal? VDOP;
		public decimal? PDOP;

		public TimeSpan? Decay;
		public string DpgsID;

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