﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class FotoViewModel : AdminViewModel
	{
		// ==================================================
		// Required


		// ==================================================
		// Optional
		public string SelectedUri { get; set; }


		// ==================================================
		// Helpers


		// ==================================================
		// Navigation
		public static string GetFotoUrl() { return "/foto/"; }

		//public static string GetPreviewUrl(string uri) { return $"/foto/preview/?uri={HttpUtility.UrlEncode(uri)}"; }
		public static string GetImportUrl(string uri) { return $"/foto/import/?uri={HttpUtility.UrlEncode(uri)}"; }
		public static string GetThumbUrl(string uri, int size = 1080) { return $"/foto/thumb/?size={size}&uri={HttpUtility.UrlEncode(uri)}"; }
	}
}