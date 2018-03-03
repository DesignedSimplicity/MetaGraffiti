using System;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
	public class GpxStatData
	{
		public int Count;
		public decimal Min;
		public decimal Max;
		public decimal Avg;

		public GpxStatData(decimal?[] dops)
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

		public GpxStatData(int?[] satellites)
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