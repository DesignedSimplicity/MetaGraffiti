using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Topo.Info;
using Newtonsoft.Json.Linq;

namespace MetaGraffiti.Web.Admin.Models
{
	public class JsonHelper
	{
		// ==================================================
		// TopoTrailInfo
		public static HtmlString GetJson(TopoTrailInfo trail)
		{
			if (trail == null || trail.Tracks.Count == 0) new HtmlString("[]");
			return new HtmlString(GetTrailJson(trail).ToString());
		}
		public static HtmlString GetJson(IEnumerable<TopoTrailInfo> trails)
		{
			if (trails == null || trails.Count() == 0) new HtmlString("[]");

			JArray all = new JArray();
			foreach(var trail in trails)
			{
				dynamic t = new JObject();
				t.id = trail.ID;
				t.name = trail.Name;
				t.tracks = GetTrailJson(trail);
				all.Add(t);
			}
			return new HtmlString(all.ToString());
		}
		private static JArray GetTrailJson(TopoTrailInfo trail)
		{
			JArray list = new JArray();
			foreach (var track in trail.Tracks)
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
			return list;
		}


		// ==================================================
		// CartoPlaceInfo
		public static HtmlString GetJson(CartoPlaceInfo place)
		{
			return new HtmlString(place.ToJson());
		}
		public static HtmlString GetJson(IEnumerable<CartoPlaceInfo> places)
		{
			var json = "";
			foreach (var place in places)
			{
				json += place.ToJson() + ",";
			}
			return new HtmlString("[" + json + "]");
		}


		// ==================================================
		// GeoRegionInfo
		public static HtmlString GetJson(IEnumerable<GeoRegionInfo> regions)
		{
			var json = "";
			foreach (var region in regions)
			{
				json += region.ToJson() + ",";
			}
			return new HtmlString("[" + json + "]");
		}

		// ==================================================
		// GeoCountryInfo
		public static HtmlString GetJson(GeoCountryInfo country)
		{
			return new HtmlString(country.ToJson());
		}
		public static HtmlString GetJson(IEnumerable<GeoCountryInfo> countries)
		{
			var json = "";
			foreach (var country in countries)
			{
				json += country.ToJson() + ",";
			}
			return new HtmlString("[" + json + "]");
		}
	}


	public class CssHelper
	{
		public static string GetContinentCss(GeoContinents continent)
		{
			switch (continent)
			{
				case GeoContinents.Asia:
					return "warning";
				case GeoContinents.Africa:
					return "dark";
				case GeoContinents.NorthAmerica:
					return "primary";
				case GeoContinents.SouthAmerica:
					return "success";
				case GeoContinents.Antarctica:
					return "light";
				case GeoContinents.Europe:
					return "secondary";
				case GeoContinents.Oceania:
					return "info";
				default:
					return "danger";
			}
		}


		public static string GetTagCss(string tag)
		{
			if (String.IsNullOrWhiteSpace(tag)) return "";

			switch (tag.ToUpperInvariant())
			{
				case "WALK": return "primary";
				case "BIKE": return "info";
				case "FAST": return "warning";
				case "STOPS": return "dark";
				case "LOOP": return "success";
				case "BAD": return "danger";
				case "SHORT": return "secondary";
				default: return "";
			}
		}
	}
}