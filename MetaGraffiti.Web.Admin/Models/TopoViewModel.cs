using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TopoViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public IEnumerable<GeoCountryInfo> Countries { get; set; }
		public List<TopoTrailInfo> Trails { get; set; }


		// ==================================================
		// Optional
		public TopoTrailInfo SelectedTrail { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
		public string SelectedSort { get; set; }


		// ==================================================
		// Helpers
		public DateTime FirstDate { get { return Trails.Min(x => x.LocalDate); } }
		public DateTime LastDate { get { return Trails.Max(x => x.LocalDate); } }
		public int GetTrailCount(GeoCountryInfo country) { return Trails.Count(x => x.Country.CountryID == country.CountryID); }
		public int GetTrailCount(int year, int month) { return Trails.Count(x => x.LocalDate.Year == year && x.LocalDate.Month == month); }
		public bool IsSortSelected(string sort)
		{
			if (String.IsNullOrWhiteSpace(sort) || String.IsNullOrWhiteSpace(SelectedSort)) return false;
			return (String.Compare(sort, SelectedSort, true) == 0);
		}


		// ==================================================
		// Navigation
		public static string GetTopoUrl() { return "/topo/"; }
		public static string GetTrailUrl(TopoTrailInfo trail) { return $"/topo/trail/{trail.ID}"; }
		public static string GetRefreshUrl() { return "/topo/refresh/"; }
		public static string GetReportUrl() { return "/topo/report/"; }
		public static string GetReportUrl(int year, int? month = null) { return $"/topo/report/?year={year}" + (month.HasValue ? $"&month={month}" : ""); }
		public static string GetCountryUrl(GeoCountryInfo country) { return $"/topo/country/{country.ISO2}/"; }
		public static string GetCountryUrl(string country, string region = "", string sort = "") { return $"/topo/country/{country}/?region={region}&sort={sort}".Replace("?region=&", "?"); }
	}
}