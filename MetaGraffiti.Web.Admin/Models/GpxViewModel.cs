using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GpxViewModel : AdminViewModel
	{
		private int _firstYear = 2011;
		private string _rootUri = @"E:\Annuals\_GPS";

		// ==================================================
		// Properties
		public int? SelectedYear { get; set; }
		public int? SelectedMonth { get; set; }
		public string SelectedFile { get; set; }

		public List<GpxCalendarModel> Calendar { get; set; } = new List<GpxCalendarModel>();


		// ==================================================
		// Attributes
		public List<int> Years
		{
			get
			{
				var years = new List<int>();
				for (int year = DateTime.Now.Year; year >= _firstYear; year--)
				{
					years.Add(year);
				}
				return years;
			}
		}


		// ==================================================
		// Methods
		public string GetMonth(int? month)
		{
			return month.HasValue
				? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value)
				: "";
		}

		public GpxCalendarModel GetCalendarEntry(int year, int month)
		{
			return Calendar.FirstOrDefault(x => x.Year == year && x.Month == month);
		}

		public List<GpxCalendarModel> GetCalendarEntries(int? year, int? month)
		{
			var list = Calendar.AsEnumerable();
			if (year.HasValue) list = list.Where(x => x.Year == year.Value);
			if (month.HasValue) list = list.Where(x => x.Month == month.Value);
			return list.OrderByDescending(x => x.Year).ThenBy(x => x.Month).ToList();
		}


		// ==================================================
		// Constructors
		public GpxViewModel()
		{
			foreach(var year in Years)
			{
				for (var month = 1; month <= 12; month++)
				{
					var uri = Path.Combine(_rootUri, year.ToString(), month.ToString("00"));
					var dir = new DirectoryInfo(uri);
					var cal = new GpxCalendarModel()
					{
						Year = year,
						Month = month,
					};
					if (dir.Exists) cal.Files = dir.GetFiles("*.gpx").Select(x => Path.GetFileNameWithoutExtension(x.Name)).OrderBy(x => x).ToList();
					Calendar.Add(cal);
				}
			}
		}
	}

	public class GpxCalendarModel
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public List<string> Files { get; set; } = new List<string>();
	}
}