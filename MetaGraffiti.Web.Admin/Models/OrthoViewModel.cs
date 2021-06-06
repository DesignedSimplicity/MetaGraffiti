using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Web.Admin.Models
{
	public class OrthoViewModel : AdminViewModel
	{
		// ==================================================
		// Required
		public List<int> Years { get; set; }


		// ==================================================
		// Optional
		public int? SelectedYear { get; set; }


		// ==================================================
		// Helpers
		public bool IsSelected(int year)
		{
			return (SelectedYear ?? -1) == year;
		}

		// ==================================================
		// Navigation
		public static string GetOrthoUrl() { return $"/ortho/"; }
		public static string GetResetUrl() { return $"/ortho/reset/"; }
		public static string GetSheetsUrl(string sheet = "") { return $"/ortho/sheets/{sheet}/".Replace("//", "/"); }
		public static string GetPlacesUrl(int? year = null) { return $"/ortho/places/?year={year}"; }
		public static string GetPlacesUrl(string country) { return $"/ortho/places/?country={country}"; }
		public static string GetBrowseTracksUrl(string path = "") { return $"/ortho/tracks/?path={path}"; }
		public static string GetBrowseImagesUrl(string path = "") { return $"/ortho/images/?path={path}"; }
	}
}