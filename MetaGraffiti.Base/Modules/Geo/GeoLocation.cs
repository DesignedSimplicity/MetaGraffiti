using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	public class GeoLocation : IGeoLocation
	{
		// ==================================================
		// Constructors
		public GeoLocation(double lat, double lon, double? elevation = null, DateTime? timestamp = null)
		{
			Init(lat, lon, elevation, timestamp);
		}

		public GeoLocation(decimal lat, decimal lon, decimal? elevation = null, DateTime? timestamp = null)
		{
			Init(Convert.ToDouble(lat), Convert.ToDouble(lon), Convert.ToDouble(elevation), timestamp);
		}

		// ==================================================
		// Properties
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double? Elevation { get; set; }
		public DateTime? Timestamp { get; set; }

		// ==================================================
		// Attributes
		public bool IsValidLocation => Latitude >= -90 && Latitude <= 90 && Longitude >= -180 && Longitude <= 180;
		public bool HasElevation => Elevation.HasValue;
		public bool HasTimestamp => Timestamp.HasValue && Timestamp.Value > DateTime.MinValue;
		public bool IsTimestampUTC => HasTimestamp && Timestamp.Value.Kind == DateTimeKind.Utc;

		// ==================================================
		// Internal
		private void Init(double lat, double lon, double? elevation, DateTime? timestamp)
		{
			Latitude = lat;
			Longitude = lon;
			Elevation = elevation;
			Timestamp = timestamp;
		}
	}
}
