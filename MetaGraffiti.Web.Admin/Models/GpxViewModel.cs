using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Gpx.Data;
using MetaGraffiti.Base.Modules.Gpx.Info;
using MetaGraffiti.Web.Admin.Services;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GpxViewModel : AdminViewModel
	{
		public List<FileInfo> Files { get; set; }
		public List<GpxCache> Cache { get; set; }

		public List<GpxTrackData> Tracks { get; set; }

		private int _firstYear = 2011;

		// ==================================================
		// Properties
		public int? SelectedYear { get; set; }
		public int? SelectedMonth { get; set; }

		public GpxDisplayModel SelectedGpx { get; set; }

		public List<GpxCalendarModel> Calendar { get; set; }


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
		public string GetMonth(int? month, bool abbv = false)
		{
			if (!month.HasValue)
				return "";
			else
				return (abbv)
					? CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value)
					: CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);
		}

		public GpxCalendarModel GetCalendarEntry(int year, int month)
		{
			InitCalendar();
			return Calendar.FirstOrDefault(x => x.Year == year && x.Month == month);
		}

		public List<GpxCalendarModel> ListCalendarEntries(int? year, int? month)
		{
			InitCalendar();
			var list = Calendar.AsEnumerable();
			if (year.HasValue) list = list.Where(x => x.Year == year.Value);
			if (month.HasValue) list = list.Where(x => x.Month == month.Value);
			return list.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
		}

		public List<GpxCache> ListGpxFiles(int year, int? month)
		{
			var list = Cache.Where(x => x.IsCached && x.MetaData.Timestamp.Year == year);
			if (month.HasValue) list = list.Where(x => x.MetaData.Timestamp.Month == month.Value);
			return list.OrderBy(x => x.MetaData.Timestamp).ToList();
		}

		// ==================================================
		// Internal
		private void InitCalendar()
		{
			if (Calendar == null)
			{
				Calendar = new List<GpxCalendarModel>();
				lock (Calendar)
				{
					foreach (var file in Files)
					{
						// assume it is in \YEAR\MONTH folder
						if (file.Directory.Name.Length == 2 && file.Directory.Parent.Name.Length == 4)
						{
							var month = TypeConvert.ToInt(file.Directory.Name);
							var year = TypeConvert.ToInt(file.Directory.Parent.Name);
							var entry = Calendar.FirstOrDefault(x => x.Year == year && x.Month == month);
							if (entry == null)
							{
								entry = new GpxCalendarModel() { Month = month, Year = year };
								Calendar.Add(entry);
							}
							entry.Files++;
						}
					}
				}
			}
		}


		private static string GetGpxFileActionUrl(string action, string file)
		{
			return $"/gpx/{action}/?uri={file}";
		}

		public static string GetDisplayUrl(string file)
		{
			return GetGpxFileActionUrl("display", file);
		}

		public static string GetFilterUrl(string file)
		{
			return GetGpxFileActionUrl("filter", file);
		}

		public static string GetExtractUrl(string file)
		{
			return GetGpxFileActionUrl("extract", file);
		}

		public static string GetExportUrl(string file, string format = "gpx")
		{
			return GetGpxFileActionUrl("export", file) + $"&format={format}";
		}

		public static string GetReportUrl(int? year, int? month = null)
		{
			if (!year.HasValue) return "/gpx/";

			var url = $"/gpx/report/?year={year}";
			if (month.HasValue) url += $"&month={month}";

			return url;
		}

		public static string GetManageUrl()
		{
			return $"/gpx/manage/";
		}
	}

	public class GpxCalendarModel
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int Files { get; set; }
	}

	

	public class GpxDisplayModel
	{
		private GpxCache _cache;

		public GpxDisplayModel(GpxCache cache)
		{
			_cache = cache;
			Points = _cache.File.Points.ToList();
		}

		public List<GpxPointData> Points { get; set; }

		public GpxFileInfo File { get { return _cache.File; } }
		public GpxFilterData Filter { get { return _cache.Filter; } }
		public GpxCacheMetaData Data { get { return _cache.MetaData; } }


		public DateTime StartTime { get { return Points.First().Timestamp.Value; } }
		public DateTime StartTimeLocal { get { return Data.Timezone.FromUTC(StartTime); } }

		public DateTime FinishTime { get { return Points.Last().Timestamp.Value; } }
		public DateTime FinishTimeLocal { get { return Data.Timezone.FromUTC(FinishTime); } }
		

		public IGeoPerimeter Bounds { get { return new GeoPerimeter(Points.ToList<IGeoLatLon>()); } }


		public GeoDistance LinearDistance { get { return GeoDistance.BetweenPoints(Points, false); } }
		public GeoDistance ActualDistance { get { return GeoDistance.BetweenPoints(Points, true); } }
		public GeoDistance ElevationUp { get { return GeoDistance.ElevationBetweenPoints(Points, 1); } }
		public GeoDistance ElevationDown { get { return GeoDistance.ElevationBetweenPoints(Points, -1); } }
		public GeoDistance ElevationTotal { get { return GeoDistance.ElevationBetweenPoints(Points, 0); } }
		public TimeSpan ElapsedTime { get { return Points.Last().Timestamp.Value.Subtract(Points.First().Timestamp.Value); } }
		public string ElapsedTimeText { get { var ts = ElapsedTime; return String.Format("{0:0} hr{1} {2:0} min{3}", Math.Floor(ts.TotalHours), (Math.Floor(ts.TotalHours) == 1 ? "" : "s"), ts.Minutes, (ts.Minutes == 1 ? "" : "s")); } }

		/*
		public string Activites { get { return (IsWalking ? "Walk" : ""); } }
		public bool IsWalking { get { return !(AverageKMH > 5 && ActualDistance.KM > 15); } }

		public double AverageKMH { get { return (Convert.ToDouble(ActualDistance.KM * 60 * 60) / File.ElapsedTime.TotalSeconds); } }
		public string ElapsedTimeDisplay { get { return String.Format("{0:0}:{1:00}", Math.Floor(File.ElapsedTime.TotalHours), File.ElapsedTime.Minutes); } }
		*/

		public string GetDOPCss(decimal? dop)
		{
			var d = dop ?? 0;
			if (d == 0)
				return "danger";
			else if (d < 3)
				return "success";
			else if (d < 5)
				return "warning";
			else
				return "danger";
		}
	}
}