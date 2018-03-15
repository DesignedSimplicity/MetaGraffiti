using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using Newtonsoft.Json.Linq;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TrackViewModel : AdminViewModel
	{
		public TrackInfo Track { get; set; }

		public List<TrackExtractInfo> Extracts { get; set; }

		public TrackExtractInfo SelectedExtract { get; set; }




		public DateTime StartTime(TrackExtractInfo track)
		{
			var timezone = Track.Timezone ?? GeoTimezoneInfo.UTC;
			var time = track.Points.First().Timestamp.Value;
			return timezone.FromUTC(time);
		}

		public DateTime FinishTime(TrackExtractInfo track)
		{
			var timezone = Track.Timezone ?? GeoTimezoneInfo.UTC;
			var time = track.Points.Last().Timestamp.Value;
			return timezone.FromUTC(time);
		}

		public decimal MaximumVelocity(TrackExtractInfo track)
		{
			return track.Points.Max(x => (x.Speed ?? 0));
		}

		public decimal MaximumDilution(TrackExtractInfo track)
		{
			return track.Points.Max(x => x.GetDOP());
		}

		public int MinimumSatellite(TrackExtractInfo track)
		{
			return track.Points.Min(x => (x.Sats ?? 0));
		}




		public HtmlString GetExtractJson()
		{
			if (SelectedExtract == null) return new HtmlString("{}");

			dynamic t = new JObject();
			t.id = SelectedExtract.ID;
			t.track = SelectedExtract.Name;
			t.points = new JArray();
			foreach (var point in SelectedExtract.Points)
			{
				dynamic p = new JObject();
				p.lat = point.Latitude;
				p.lng = point.Longitude;
				t.points.Add(p);
			}
			return new HtmlString(t.ToString());
		}

		public HtmlString GetTrackJson()
		{
			if (Extracts == null || Extracts.Count == 0) new HtmlString("[]");

			JArray list = new JArray();
			foreach (var track in Extracts)
			{
				dynamic t = new JObject();
				t.id = track.ID;
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

		public static string GetTrackUrl() { return "/track/"; }
		public static string GetResetUrl() { return "/track/reset/"; }
		public static string GetUpdateUrl() { return "/track/update/"; }
		public static string GetImportUrl() { return "/track/import/"; }
		public static string GetExportUrl(string format = "GPX") { return $"/track/export/?format={format}"; }
		public static string GetExtractUrl() { return "/track/extract/"; }
		public static string GetExtractUrl(string uri) { return $"/track/extract/?uri={HttpUtility.UrlEncode(uri)}"; }
		public static string GetDeleteUrl(string ID) { return $"/track/delete/{ID}"; }
		public static string GetEditUrl(string ID) { return $"/track/edit/{ID}"; }
		public static string GetSaveUrl() { return $"/track/save/"; }
		public static string GetFilterUrl() { return "/track/filter/"; }
		public static string GetRemoveUrl() { return "/track/remove/"; }
		public static string GetRevertUrl(string ID) { return $"/track/revert/{ID}"; }
	}
}