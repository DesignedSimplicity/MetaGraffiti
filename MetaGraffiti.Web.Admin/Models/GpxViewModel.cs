using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Gpx.Data;
using MetaGraffiti.Base.Modules.Gpx.Info;

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

		public GpxFileModel SelectedGpx { get; set; }



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
					if (dir.Exists) cal.Files = dir.GetFiles("*.gpx").Select(x => x.FullName).OrderBy(x => x).ToList();
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

	public class GpxFileModel
	{
		public GpxFileModel(GpxFileInfo file) { File = file; GuessTimezone(); }

		public GpxFileInfo File { get; private set; }

		public string Name { get { return (String.IsNullOrWhiteSpace(File.Name) ? Path.GetFileNameWithoutExtension(File.Uri) : File.Name); } }

		public IGeoPerimeter Bounds { get { return new GeoPerimeter(FilteredPoints.ToList<IGeoLatLon>()); } }

		private GeoTimezoneInfo _timezone;
		public GeoTimezoneInfo Timezone { get { if (_timezone == null) _timezone = GuessTimezone(); return _timezone; } }

		public DateTime Timestamp { get { return File.Points.First().Timestamp.Value; } }

		public DateTime StartTime { get { return (Timezone == null ? File.Points.First().Timestamp.Value : Timezone.FromUTC(File.Points.First().Timestamp.Value)); } }
		public DateTime FinishTime { get { return (Timezone == null ? File.Points.Last().Timestamp.Value : Timezone.FromUTC(File.Points.Last().Timestamp.Value)); } }

		public int? FilterGPS { get; set; }
		public decimal? FilterDOP { get; set; }
		public DateTime? FilterStart { get; set; }
		public DateTime? FilterFinish { get; set; }

		public GeoDistance FilteredLinearDistance { get { return GeoDistance.BetweenPoints(FilteredPoints, false); } }
		public GeoDistance FilteredActualDistance { get { return GeoDistance.BetweenPoints(FilteredPoints, true); } }
		public GeoDistance FilteredElevationUp { get { return GeoDistance.ElevationBetweenPoints(FilteredPoints, 1); } }
		public GeoDistance FilteredElevationDown { get { return GeoDistance.ElevationBetweenPoints(FilteredPoints, -1); } }
		public GeoDistance FilteredElevationTotal { get { return GeoDistance.ElevationBetweenPoints(FilteredPoints, 0); } }
		public string FilteredElapsedTime { get { return ""; } } // var ts = FilteredPoints.Last().Timestamp.Value.Subtract(FilteredPoints.First().Timestamp.Value); return String.Format("{0:0} hour{1} {2:0} min{3}", Math.Floor(ts.TotalHours), (Math.Floor(ts.TotalHours) == 1 ? "" : "s"), ts.Minutes, (ts.Minutes == 1 ? "" : "s")); } }

		private List<GpxPointData> _filtered = null;
		public List<GpxPointData> FilteredPoints
		{
			get
			{
				if (_filtered == null)
				{
					var p = File.Points;
					if (FilterStart.HasValue)
					{
						//var s = FilterStart.Value.ToUniversalTime();
						var s = Timezone.ToUTC(FilterStart.Value);
						p = p.Where(x => x.Timestamp >= s);
					}
					if (FilterFinish.HasValue)
					{
						var f = FilterFinish.Value.ToUniversalTime();
						p = p.Where(x => x.Timestamp <= f);
					}
					if (FilterDOP.HasValue && FilterDOP.Value > 0) p = p.Where(x => x.MaxDOP <= FilterDOP.Value);
					if (FilterGPS.HasValue && FilterGPS.Value > 0) p = p.Where(x => x.Sats >= FilterGPS.Value);
					_filtered = p.ToList();
				}
				return _filtered;
			}
		}


		public IEnumerable<GeoCountryInfo> Countries { get { return GeoCountryInfo.ListByLocation(File.Points.First()); } }
		public IEnumerable<GeoRegionInfo> Regions { get { return GeoRegionInfo.ListByLocation(File.Points.First()); } }

		public string LocationText
		{
			get
			{
				if (Regions.Count() == 1)
					return String.Format("{0}, {1}", Regions.First().RegionName, Regions.First().Country.Name);
				else if (Countries.Count() == 1)
					return Countries.First().Name;
				else
					return "";
			}
		}

		public string Activites { get { return (IsWalking ? "Walk" : ""); } }
		public bool IsWalking { get { return !(AverageKMH > 5 && FilteredActualDistance.KM > 15); } }

		public double AverageKMH { get { return (Convert.ToDouble(FilteredActualDistance.KM * 60 * 60) / File.ElapsedTime.TotalSeconds); } }
		public string ElapsedTimeDisplay { get { return String.Format("{0:0}:{1:00}", Math.Floor(File.ElapsedTime.TotalHours), File.ElapsedTime.Minutes); } }


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


		private GeoTimezoneInfo GuessTimezone()
		{
			if (Countries.Any(x => x.ISO2 == "NZ"))
				return GeoTimezoneInfo.ByKey("New Zealand");
			else if (Countries.Any(x => x.ISO2 == "JP"))
				return GeoTimezoneInfo.ByKey("Tokyo");
			else if (Countries.Any(x => x.ISO2 == "SG"))
				return GeoTimezoneInfo.ByKey("Singapore");
			else if (Countries.Any(x => x.Continent == GeoContinents.Europe))
				return GeoTimezoneInfo.ByKey("W. Europe");
			else if (Regions.Any(x => x.RegionName == "Tasmania"))
				return GeoTimezoneInfo.ByKey("Tasmania");
			else if (Regions.Any(x => x.RegionName == "Western Australia"))
				return GeoTimezoneInfo.ByKey("W. Australia");
			else if (Regions.Any(x => x.RegionName == "New South Wales"))
				return GeoTimezoneInfo.ByKey("AUS Eastern");
			else if (Regions.Any(x => x.RegionName == "Queensland"))
				return GeoTimezoneInfo.ByKey("AUS Eastern");
			else if (Countries.Count() == 1)
			{
				var c = Countries.First();
				if (c.ISO2 == "AR")
					return GeoTimezoneInfo.ByKey("Argentina");
				else if (c.ISO2 == "CL")
					return GeoTimezoneInfo.ByKey("Pacific SA");
			}
			else if (Regions.Select(x => x.Country).Distinct().Count() == 1)
			{
				var c = Regions.First().Country;
				if (c.ISO2 == "AR")
					return GeoTimezoneInfo.ByKey("Argentina");
				else if (c.ISO2 == "CL")
					return GeoTimezoneInfo.ByKey("Pacific SA");
			}
			
			// default here
			return GeoTimezoneInfo.LocalTimezone;
		}
	}
}