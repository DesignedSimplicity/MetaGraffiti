using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;
using Newtonsoft.Json.Linq;
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


		public int? SelectedYear { get; set; }
		public int? SelectedMonth { get; set; }


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

		// TODO: consolidate JSON
		public HtmlString GetTrackJson()
		{
			if (Trail == null || Trail.Tracks.Count == 0) new HtmlString("[]");

			JArray list = new JArray();
			foreach (var track in Trail.Tracks)
			{
				dynamic t = new JObject();
				//t.id = track.ID;
				t.track = track.Name;
				t.points = new JArray();
				foreach (var point in track.Points)
				{
					dynamic p = new JObject();
					p.lat = point.Latitude;
					p.lng = point.Longitude;
					t.points.Add(p);
				}
				list.Add(t);
			}
			return new HtmlString(list.ToString());
		}


		public static string GetTrailUrl() { return "/trail/"; }

		public static string GetRefreshUrl() { return "/trail/refresh/"; }

		public static string GetImportUrl() { return "/trail/import/"; }

		public static string GetModifyUrl() { return "/trail/modify/"; }

		public static string GetReportUrl() { return "/trail/report/"; }

		public static string GetReportUrl(int year, int? month = null) { return $"/trail/report/?year={year}" + (month.HasValue ? $"&month={month}" : ""); }

		public static string GetCountryUrl(string country, string region = "") { return $"/trail/country/{country}/" + (String.IsNullOrWhiteSpace(region) ? "" : $"?region={region}"); }

		public static string GetDisplayUrl(string id) { return $"/trail/display/{id}/"; }
	}


	public class TopoTrailModel
	{
		public TopoTrailInfo Trail { get; set; }


		public CartoPlaceInfo StartPlace { get; set; }
		public CartoPlaceInfo FinishPlace { get; set; }
		public List<CartoPlaceInfo> ViaPlaces { get; set; }


		/// <summary>
		/// Places partially contained within the trail bounds
		/// </summary>
		public List<CartoPlaceInfo> NearbyPlaces { get; set; }

		/// <summary>
		/// Places which contain part of the trail bounds
		/// </summary>
		public List<CartoPlaceInfo> ContaingPlaces { get; set; }
	}
}