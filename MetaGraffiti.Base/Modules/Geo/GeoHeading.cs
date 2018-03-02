using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo
{
	/// <summary>
	/// //Heading: the direction in which a vessel is pointing
	/// </summary>
	public class GeoHeading
	{
		// ==================================================
		// Constructors

		public GeoHeading(IGeoLocation from, IGeoLocation to)
		{
			From = from;
			To = to;
		}

		// ==================================================
		// Properties

		public IGeoLocation To { get; protected set; }

		public IGeoLocation From { get; protected set; }

		// ==================================================
		// Attributes

		public GeoDirection Direction { get { return GeoDirection.FromPoints(From, To); } }

		public GeoDistance Distance { get { return GeoDistance.BetweenPoints(From, To, true); } }

		public TimeSpan ElapsedTime
		{
			get
			{
				if (From.Timestamp.HasValue && To.Timestamp.HasValue)
					return To.Timestamp.Value.Subtract(From.Timestamp.Value);
				else
					return new TimeSpan();
			}
		}


		// ==================================================
		// Static Factory

		public static GeoHeading FromPositions(IGeoLocation a, IGeoLocation b)
		{
			return new GeoHeading(a, b);
		}

		public static List<GeoHeading> FromPositions(IList<IGeoLocation> points)
		{
			var l = new List<GeoHeading>();
			var p0 = points.FirstOrDefault();
			foreach (var p in points)
			{
				if (p0 != p)
				{
					var h = GeoHeading.FromPositions(p0, p);
					l.Add(h);
					p0 = p;
				}
			}

			return l;
		}
	}
}
