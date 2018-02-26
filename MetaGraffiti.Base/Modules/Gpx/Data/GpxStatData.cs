using System;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
	public class GpxStats
	{
		public int Count;
		public decimal Min;
		public decimal Max;
		public decimal Avg;

		public GpxStats(decimal?[] dops)
		{
			var d = dops.Where(x => x.HasValue).Select(x => x.Value);
			Count = d.Count();
			if (Count > 0)
			{
				Min = d.Min();
				Max = d.Max();
				Avg = d.Average();
			}
		}

		public GpxStats(int?[] satellites)
		{
			var d = satellites.Where(x => x.HasValue).Select(x => x.Value);
			Count = d.Count();
			if (Count > 0)
			{
				Min = d.Min();
				Max = d.Max();
				Avg = Convert.ToDecimal(d.Average());
			}
		}
	}
}
