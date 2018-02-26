using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	public class GeoPoint : IGeoPoint
	{
		public GeoPoint(double lat, double lon) { Latitude = lat; Longitude = lon; }
		public GeoPoint(decimal lat, decimal lon) { Latitude = Convert.ToDouble(lat); Longitude = Convert.ToDouble(lon); }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}

	public class GeoLocation : GeoPoint, IGeoLocation
	{
		public GeoLocation(double lat, double lon, double? elevation = null, DateTime? timestamp = null) : base(lat, lon) { Elevation = elevation; Timestamp = timestamp; }
		public GeoLocation(decimal lat, decimal lon, decimal? elevation = null, DateTime? timestamp = null) : base(lat, lon) { Elevation = Convert.ToDouble(elevation); Timestamp = timestamp; }
		public double? Elevation { get; set; }
		public DateTime? Timestamp { get; set; }
	}
}
