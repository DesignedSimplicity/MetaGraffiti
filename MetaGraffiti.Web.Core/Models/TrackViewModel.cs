using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Topo.Info;
using MetaGraffiti.Base.Services;

namespace MetaGraffiti.Web.Core.Models
{
	public class TrackViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public List<TrackEditData> Tracks { get; set; }


		// ==================================================
		// Optional
		public TrackEditModel EditTrack { get; set; }


		// ==================================================
		// Helpers
		public DateTime GetTrackDate(TrackEditData edit)
		{
			//TODO: BUG: fix this to use timezone for local time display
			//return edit.Timezone.FromUTC(edit.Points.First().Timestamp.Value);
			return edit.Points.First().Timestamp.Value;
		}


		// ==================================================
		// Navigation
		public static string GetManageUrl() { return "/track/manage/"; }
		public static string GetExtractUrl() { return "/track/extract/"; }
		public static string GetModifyUrl(string key) { return $"/track/modify/{key}/"; }
		public static string GetExtractUrl(string source) { return $"/track/extract/?source={source}"; }
		public static string GetPreviewUrl(string source) { return $"/track/preview/?source={source}"; }
		public static string GetFilterUrl() { return "/track/filter/"; }
		public static string GetRemoveUrl() { return "/track/remove/"; }
		public static string GetRevertUrl(string key) { return $"/track/revert/{key}/"; }
		public static string GetDeleteUrl(string key) { return $"/track/delete/{key}/"; }
		public static string GetResetUrl() { return "/track/reset/"; }
		public static string GetExportUrl(string format = "GPX") { return $"/track/export/?format={format}"; }
	}


	// ==================================================
	// Partials
	public class TrackEditModel
	{
		public FileInfo File { get; set; }
		public TrackEditData Track { get; set; }
		public TopoTrackInfo TopoTrack { get; set; }

		public TrackEditFilter Filters { get; set; }

		public List<CartoPlaceInfo> NearbyPlaces { get; set; }
		public List<CartoPlaceInfo> ContainedPlaces { get; set; }
	}
}