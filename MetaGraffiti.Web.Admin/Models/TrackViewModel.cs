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
		public TrackData Track { get; set; }

		public List<TrackExtractData> Extracts { get; set; }

		public TrackExtractData SelectedExtract { get; set; }




		public DateTime StartTime(TrackExtractData track)
		{
			var timezone = Track.Timezone ?? GeoTimezoneInfo.UTC;
			var time = track.Points.FirstOrDefault()?.Timestamp.Value ?? DateTime.MinValue;
			return timezone.FromUTC(time);
		}

		public DateTime FinishTime(TrackExtractData track)
		{
			var timezone = Track.Timezone ?? GeoTimezoneInfo.UTC;
			var time = track.Points.LastOrDefault()?.Timestamp.Value ?? DateTime.MaxValue;
			return timezone.FromUTC(time);
		}

		public decimal MaximumVelocity(TrackExtractData track)
		{
			return track.Points.Max(x => x.Speed ?? 0);
		}

		public decimal MaximumDilution(TrackExtractData track)
		{
			return track.Points.Max(x => x.GetDOP());
		}

		public int MinimumSatellite(TrackExtractData track)
		{
			return track.Points.Min(x => (x.Sats ?? 0));
		}

		public double TotalDistance(TrackExtractData track)
		{
			return GeoDistance.BetweenPoints(track.Points, true).KM;
		}

		public string ElapsedTime(TrackExtractData track)
		{
			var ts = FinishTime(track).Subtract(StartTime(track));
			return String.Format("{0:0} hr{1} {2:0} min{3}", Math.Floor(ts.TotalHours), (Math.Floor(ts.TotalHours) == 1 ? "" : "s"), ts.Minutes, (ts.Minutes == 1 ? "" : "s"));
		}

		public string GetDOPCss(decimal? dop)
		{
			var d = dop ?? 99;
			if (d < 3)
				return "success";
			else if (d < 5)
				return "warning";
			else
				return "danger";
		}

		public string GetSpeedCss(decimal? speed)
		{
			var s = speed ?? 9;
			if (s < 1)
				return "success";
			else if (s < 2)
				return "warning";
			else
				return "danger";
		}

		public string GetSatsCss(int? sats)
		{
			var s = sats ?? 0;
			if (s > 10)
				return "success";
			else if (s > 5)
				return "warning";
			else
				return "danger";
		}

		// TODO: consolidate JSON
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

		// TODO: consolidate JSON
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