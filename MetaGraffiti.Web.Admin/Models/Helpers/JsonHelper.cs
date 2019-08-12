using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class JsonHelper
	{
		// ==================================================
		// Strings
		public static HtmlString GetJson(IEnumerable<string> list)
		{
			return new HtmlString(JsonConvert.SerializeObject(list));
		}


		// ==================================================
		// IGpxTrack
		public static HtmlString GetJson(IGpxTrack track)
		{
			var t = BuildTrack(track);
			return new HtmlString(t.ToString());
		}
		public static HtmlString GetJson(IEnumerable<IGpxTrack> tracks)
		{
			if (tracks == null || tracks.Count() == 0) new HtmlString("[]");

			JArray all = new JArray();
			foreach (var track in tracks)
			{
				dynamic t = BuildTrack(track);
				all.Add(t);
			}
			return new HtmlString(all.ToString());
		}
		private static dynamic BuildTrack(IGpxTrack track)
		{
			dynamic t = new JObject();
			t.track = track.Name;
			t.points = new JArray();
			foreach (var point in track.Points)
			{
				dynamic p = new JObject();
				p.lat = point.Latitude;
				p.lng = point.Longitude;
				t.points.Add(p);
			}
			return t;
		}


		// ==================================================
		// ITopoTrailInfo
		public static HtmlString GetJson(ITopoTrailInfo trail)
		{
			if (trail == null || trail.Tracks.Count() == 0) new HtmlString("[]");
			return new HtmlString(GetTrailJson(trail).ToString());
		}
		public static HtmlString GetJson(IEnumerable<ITopoTrailInfo> trails)
		{
			if (trails == null || trails.Count() == 0) new HtmlString("[]");

			JArray all = new JArray();
			foreach (var trail in trails)
			{
				dynamic t = new JObject();
				t.id = trail.Key;
				t.name = trail.Name;
				t.tracks = GetTrailJson(trail);
				all.Add(t);
			}
			return new HtmlString(all.ToString());
		}
		private static JArray GetTrailJson(ITopoTrailInfo trail)
		{
			JArray list = new JArray();
			foreach (var track in trail.Tracks)
			{
				dynamic t = new JObject();
				//t.id = track.ID;
				t.track = track.Name;
				t.points = new JArray();
				foreach (var point in track.TopoPoints)
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
			return new HtmlString(place == null ? "{}" : place.ToJson());
		}
		public static HtmlString GetJson(IEnumerable<CartoPlaceInfo> places)
		{
			if (places == null)
			{
				return new HtmlString("[]");
			}
			
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
}
