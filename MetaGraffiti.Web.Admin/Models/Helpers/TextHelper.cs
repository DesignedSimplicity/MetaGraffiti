using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TextHelper
	{
		public static string GetMonth(int? month, bool abbv = false)
		{
			if (!month.HasValue)
				return "";
			else
				return (abbv)
					? CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value)
					: CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);
		}

		public static string GetElapsedTime(TimeSpan elapsed)
		{
			return String.Format("{0:0} h {1:00} m", Math.Floor(elapsed.TotalHours), elapsed.Minutes);
		}

		public static string GetTotalDays(TimeSpan elapsed)
		{
			if (elapsed.TotalDays < 1)
				return "1 day";
			else
				return String.Format("{0:0} days", Math.Ceiling(elapsed.TotalDays));
		}
		public static string GetTotalDays(DateTime start, DateTime finish)
		{
			var days = (finish.DayOfYear - start.DayOfYear) + 1;
			if (days == 0)
				return "1 day";
			else
				return $"{days} days";
		}
	}
}