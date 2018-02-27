using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Gpx
{
	// http://www.topografix.com/gpx_manual.asp


	public class Gpx
	{
		public const string XmlNamespaceV1 = "http://www.topografix.com/GPX/1/0";
		public const string XmlNamespaceV1_1 = "http://www.topografix.com/GPX/1/1";
	}

	public enum GpxSchemaVersion
	{
		Version1 = 1,
		Version1_1 = 11,
	}

	public enum GpxPointTypes
	{
		/// <summary>
		/// Point marking a specific position
		/// </summary>
		WayPoint = 1,
		/// <summary>
		/// Point determining a specific route
		/// </summary>
		RoutePoint = 2,
		/// <summary>
		/// Point recorded along a track
		/// </summary>
		TrackPoint = 3,
	}

	/// <summary>
	/// Upper bounds to determine quality of position data
	/// </summary>
	public enum GpxDOPConfidence
	{
		/// <summary>
		/// Highest possible confidence level to be used for applications demanding the highest possible precision at all times.
		/// </summary>
		Ideal = 1,
		/// <summary>
		/// At this confidence level, positional measurements are considered accurate enough to meet all but the most sensitive applications.
		/// </summary>
		Excellent = 2,
		/// <summary>
		/// Represents a level that marks the minimum appropriate for making business decisions. Positional measurements could be used to make reliable in-route navigation suggestions to the user.
		/// </summary>
		Good = 5,
		/// <summary>
		/// Positional measurements could be used for calculations, but the fix quality could still be improved. A more open view of the sky is recommended.
		/// </summary>
		Moderate = 10,
		/// <summary>
		/// Represents a low confidence level. Positional measurements should be discarded or used only to indicate a very rough estimate of the current location.
		/// </summary>
		Fair = 20,
		/// <summary>
		/// At this level, measurements are inaccurate by as much as 300 meters with a 6-meter accurate device (50 DOP × 6 meters) and should be discarded.
		/// </summary>
		Poor = 100
	}
}
