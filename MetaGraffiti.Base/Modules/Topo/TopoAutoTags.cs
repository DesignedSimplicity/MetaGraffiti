using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Topo
{
    public class TopoAutoTags
    {
		public static string[] FromPoints(IEnumerable<GpxPointData> points)
		{
			var tags = new List<string>();
			var count = points.Count();

			// is closed loop
			var loop = GeoDistance.BetweenPoints(points.First(), points.Last());
			if (loop.Meters <= 200) tags.Add("Loop");

			// average speed
			var km = GeoDistance.BetweenPoints(points).KM;
			var time = points.Max(x => x.Timestamp.Value).Subtract(points.Min(x => x.Timestamp.Value));
			var kmh = km / time.TotalHours;
			if (kmh <= 5) // km/h
				tags.Add("Walk");
			else if (kmh <= 15) // km/h
				tags.Add("Bike");

			// max speed (of data)
			var max = points.Max(x => x.Speed ?? 0);
			if (max > 5) tags.Add("Fast"); // m/s

			// number of stops
			var stops = points.Count(x => x.Speed == 0);
			if ((stops / count) >= 0.01) tags.Add("Stops"); // 1%

			// few options
			if (count < 30) tags.Add("Short");

			// bad data points
			var bad = max > 33; // m/s
			if (!bad) bad = points.Average(x => (x.Sats ?? 0)) <= 5;
			if (!bad) bad = points.Count(x => !x.HDOP.HasValue) >= 20; // TODO: fix this threshold for old files without DOP data
			if (bad) tags.Add("Bad");

			return tags.ToArray();
		}
	}
}
