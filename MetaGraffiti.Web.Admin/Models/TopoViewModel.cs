using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo;
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
		public DateTime FirstDate { get; set; }
		public DateTime LastDate { get; set; }


		// ==================================================
		// Optional
		public GeoCountryInfo SelectedCountry { get; set; }
		public GeoRegionInfo SelectedRegion { get; set; }
		public string SelectedSort { get; set; }
		public int? SelectedYear { get; set; }
		public int? SelectedMonth { get; set; }



		// ==================================================
		// Helpers
		public int GetTrailCount(GeoCountryInfo country) { return Trails.Count(x => x.Country.CountryID == country.CountryID); }
		public int GetTrailCount(int year, int month) { return Trails.Count(x => x.StartLocal.Year == year && x.StartLocal.Month == month); }
		public bool IsSortSelected(string sort)
		{
			if (String.IsNullOrWhiteSpace(sort) || String.IsNullOrWhiteSpace(SelectedSort)) return false;
			return (String.Compare(sort, SelectedSort, true) == 0);
		}
		public IEnumerable<TopoTrailInfo> ListTrailsSorted()
		{
			if (String.IsNullOrWhiteSpace(SelectedSort)) SelectedSort = "Newest";
			if (IsSortSelected("Region"))
				return Trails.OrderBy(x => (x.Region == null ? "" : x.Region.RegionName)).ThenBy(x => x.Name).ThenByDescending(x => x.StartLocal);
			else if (IsSortSelected("Name"))
				return Trails.OrderBy(x => x.Name).ThenByDescending(x => x.StartLocal);
			else if (IsSortSelected("Newest"))
				return Trails.OrderByDescending(x => x.StartLocal).ThenBy(x => x.Name);
			else if (IsSortSelected("Oldest"))
				return Trails.OrderBy(x => x.StartLocal).ThenBy(x => x.Name);
			else
				return Trails;
		}
		public IEnumerable<CartoPlaceInfo> ConsolidatePlaces(TopoTrailInfo trail)
		{
			var places = new List<CartoPlaceInfo>();
			foreach(var track in trail.TopoTracks)
			{
				var start = track.StartPlace;
				if (start != null && !places.Any(x => x.Key == start.Key)) places.Add(start);

				var finish = track.FinishPlace;
				if (finish != null && !places.Any(x => x.Key == finish.Key)) places.Add(finish);
			}
			return places;
		}



		// ==================================================
		// Navigation
		public static string GetTopoUrl() { return "/topo/"; }

		public static string GetRefreshUrl() { return "/topo/refresh/"; }
		public static string GetReportUrl() { return "/topo/report/"; }
		public static string GetReportUrl(int year, int? month = null) { return $"/topo/report/?year={year}" + (month.HasValue ? $"&month={month}" : ""); }
		public static string GetCountryUrl(GeoCountryInfo country) { return $"/topo/country/{country.ISO2}/"; }
		public static string GetCountryUrl(string country, string region = "", string sort = "") { return $"/topo/country/{country}/?region={region}&sort={sort}".Replace("?region=&", "?"); }
	}
}