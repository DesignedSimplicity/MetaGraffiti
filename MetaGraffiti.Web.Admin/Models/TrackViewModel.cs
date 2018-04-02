using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Newtonsoft.Json.Linq;

using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;

namespace MetaGraffiti.Web.Admin.Models
{
	public class TrackViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public TrackGroupData TrackGroup { get; set; }
		public List<TrackExtractData> TrackExtracts { get; set; }


		// ==================================================
		// Optional


		// ==================================================
		// Helpers
		public string GetDOPCss(decimal? dop)
		{
			var d = dop ?? 0;
			if (d == 0)
				return "secondary";
			else if (d < 3)
				return "success";
			else if (d < 5)
				return "warning";
			else
				return "danger";
		}

		public string GetSpeedCss(decimal? speed)
		{
			var s = speed ?? 0;
			if (s == 0)
				return "danger";
			if (s <= 2)
				return "success";
			else if (s <= 4)
				return "warning";
			else
				return "primary";
		}

		public string GetSatsCss(int? sats)
		{
			var s = sats ?? 0;
			if (s == 0)
				return "secondary";
			else if (s > 10)
				return "success";
			else if (s > 5)
				return "warning";
			else
				return "danger";
		}


		// ==================================================
		// Navigation

		public static string GetPreviewUrl(string uri) { return $"/track/preview/?uri={HttpUtility.UrlEncode(uri)}"; }
		public static string GetPreviewUrl(string uri, DateTime? start, DateTime? finish) { return $"/track/preview/?uri={HttpUtility.UrlEncode(uri)}&start={start}&finish={finish}"; }





		// ==================================================
		// ==================================================
		// ==================================================
		// ==================================================

	

		

		

		public TrackExtractData SelectedExtract { get; set; }
		public TrackFileModel SelectedSource { get; set; }

		public DateTime? SelectedStart { get; set; }
		public DateTime? SelectedFinish { get; set; }




		public DateTime StartTime(TrackExtractData track)
		{
			var timezone = TrackGroup.Timezone ?? GeoTimezoneInfo.UTC;
			var time = track.Points.FirstOrDefault()?.Timestamp.Value ?? DateTime.MinValue;
			return timezone.FromUTC(time);
		}

		public DateTime FinishTime(TrackExtractData track)
		{
			var timezone = TrackGroup.Timezone ?? GeoTimezoneInfo.UTC;
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

		

		public bool IsTimezoneValid { get { return TrackGroup.Timezone != null && TrackGroup.Timezone.Key != "UTC;"; } }
		public bool IsCountryValid { get { return TrackGroup.Country != null; } }
		public bool IsRegionValid { get { return IsCountryValid && TrackGroup.Country.HasRegions && TrackGroup.Region != null; } }


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
			if (TrackExtracts == null || TrackExtracts.Count == 0) new HtmlString("[]");

			JArray list = new JArray();
			foreach (var track in TrackExtracts)
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

		public static string GetManageUrl() { return "/track/manage/"; }
		public static string GetResetUrl() { return "/track/reset/"; }
		public static string GetUpdateUrl() { return "/track/update/"; }
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

	public class TrackFileModel
	{
		public string Uri { get; set; }
		public string FileName { get; set; }
		public string Directory { get; set; }

		public TrackGroupData Metadata { get; set; }

		public TimeSpan Elapsed { get; set; }
		public double Distance { get; set; }

		public bool IsLoop { get; set; }
		public bool IsBike { get; set; }
		public bool IsWalk { get; set; }
		public bool IsFast { get; set; }
		public bool IsShort { get; set; }
		public bool IsBad { get; set; }

		public TopoTrailInfo Trail { get; set; }

		public List<GeoRegionInfo> Regions { get; set; } = new List<GeoRegionInfo>();
		public List<CartoPlaceInfo> Places { get; set; } = new List<CartoPlaceInfo>();
		public List<CartoPlaceInfo> NearbyPlaces { get; set; } = new List<CartoPlaceInfo>();
		public CartoPlaceInfo StartPlace;
		public CartoPlaceInfo FinishPlace;



		public bool ContainsPlace(CartoPlaceInfo place)
		{
			return Places.Any(x => x.Key == place.Key);
		}
		public bool IsStart(CartoPlaceInfo place)
		{
			return StartPlace != null && place.Key == StartPlace.Key;
		}
		public bool IsFinish(CartoPlaceInfo place)
		{
			return StartPlace != null && place.Key == StartPlace.Key;
		}



		public string GetDistanceCss(double distance)
		{
			if (distance < 1)
				return "danger";
			else if (distance > 50)
				return "primary";
			else
				return "success";
		}


		public string LocationText
		{
			get
			{
				var text = Metadata.Country.Name;
				if (Metadata.Region != null) text += @" \ " + Metadata.Region.RegionName;
				return text;
			}
		}

		public DateTime LocalTime
		{
			get
			{
				if (Metadata.Timezone != null && Metadata.Timestamp.HasValue)
					return Metadata.Timezone.FromUTC(Metadata.Timestamp.Value);
				else
					return Metadata.Timestamp ?? DateTime.MinValue;
			}
		}
	}
}