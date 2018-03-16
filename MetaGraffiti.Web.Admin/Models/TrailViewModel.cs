using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TrailViewModel : AdminViewModel
	{
		public IEnumerable<GeoCountryInfo> Countries { get; set; }
		public DateTime FirstDate { get; set; }
		public DateTime LastDate { get; set; }


		public TopoTrailInfo Trail { get; set; }

		public List<TopoTrailInfo> Trails { get; set; }

		public TrailReportRequest Filters { get; set; }

		public GeoCountryInfo Country { get; set; }

		public GeoRegionInfo Region { get; set; }



		public int GetCount(int year, int month)
		{
			return Trails.Count(x => x.LocalDate.Year == year && x.LocalDate.Month == month);

		}

		public string GetMonth(int? month, bool abbv = false)
		{
			if (!month.HasValue)
				return "";
			else
				return (abbv)
					? CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value)
					: CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value);
		}


		public static string GetTrailUrl() { return "/trail/"; }

		public static string GetReportUrl() { return "/trail/report/"; }

		public static string GetReportUrl(int year, int? month = null) { return $"/trail/report/?year={year}" + (month.HasValue ? $"&month={month}" :""); }

		public static string GetCountryUrl(string country, string region = "") { return $"/trail/country/{country}/" + (String.IsNullOrWhiteSpace(region) ? "" : $"?region={region}"); }

		public static string GetDisplayUrl(string id) { return $"/trail/display/{id}/"; }
	}
}