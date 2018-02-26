using System;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
	public class GpxPointData : GpxMetaData, IGeoLocation
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

		public decimal MaxDOP
		{
			get
			{
				var h = HDOP ?? Decimal.MaxValue;
				var v = VDOP ?? Decimal.MaxValue;
				var p = PDOP ?? Decimal.MaxValue;
				var b = h;
				if (v > b) b = v;
				if (p > b) b = p;
				return b;
			}
		}
	}

	/*
	<trkpt lat="-41.2420763" lon="174.78955016">
		<ele>193.0</ele>
		<time>2016-01-16T22:48:25Z</time>
		<speed>0.0</speed>
		<src>gps</src>
		<sat>21</sat>
		<hdop>1.0</hdop>
		<vdop>0.9</vdop>
		<pdop>1.3</pdop>
		<geoidheight>17.0</geoidheight>
	</trkpt>
	*/
	public class GpxTrackData : GpxRouteData
	{
	}

	/*
    <rtept lon="9.860624216140083" lat="54.9328621088893">
        <ele>0.0</ele>
        <name>Position 1</name>
    </rtept>
	*/
	public class GpxRouteData : GpxMetaData
	{
		public int Number;
		public IList<GpxPointData> Points;
	}
}
