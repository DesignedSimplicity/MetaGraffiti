﻿using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Topo.Info;
using Newtonsoft.Json.Linq;

namespace MetaGraffiti.Web.Admin.Models
{
	public class IconHelper
	{
		public static HtmlString GetFontAwesomeIcon(string faName)
		{
			return new HtmlString($"<i class='{faName}'></i>");
		}

		public static HtmlString GetPlaceTypeIconSvg(string placeType)
		{
			string path = GetPlaceTypeIconUri(placeType);
			return new HtmlString(File.ReadAllText(path));
		}
		public static string GetPlaceTypeIconUri(string placeType)
		{
			var path = Path.Combine(AutoConfig.IconSourceUri, "Places", placeType + ".svg");
			if (File.Exists(path))
				return path;
			else
				return Path.Combine(AutoConfig.IconSourceUri, "Places", "place.svg");
		}


		// ==================================================
		// Alert Messages
		public static HtmlString ConfirmationMessageIcon { get { return GetFontAwesomeIcon("fas fa-thumbs-up"); } }
		public static HtmlString ErrorMessageIcon { get { return GetFontAwesomeIcon("fas fa-exclamation-triangle"); } }


		// ==================================================
		// Form Actions
		public static HtmlString AddIcon { get { return GetFontAwesomeIcon("fas fa-plus"); } }
		public static HtmlString SaveIcon { get { return GetFontAwesomeIcon("fas fa-save"); } }
		public static HtmlString EditIcon { get { return GetFontAwesomeIcon("fas fa-pencil-alt"); } }
		public static HtmlString UndoIcon { get { return GetFontAwesomeIcon("fas fa-undo"); } }
		public static HtmlString RemoveIcon { get { return GetFontAwesomeIcon("fas fa-times"); } }
		public static HtmlString CancelIcon { get { return GetFontAwesomeIcon("fas fa-ban"); } }


		// ==================================================
		// Other Actions
		public static HtmlString SearchIcon { get { return GetFontAwesomeIcon("fas fa-search"); } }
		public static HtmlString FilterIcon { get { return GetFontAwesomeIcon("fas fa-filter"); } }
		public static HtmlString LookupIcon { get { return GetFontAwesomeIcon("fas fa-magnet"); } }
		public static HtmlString GeocodeIcon { get { return GetFontAwesomeIcon("fas fa-crosshairs"); } }
		public static HtmlString ImportIcon { get { return GetFontAwesomeIcon("fas fa-database"); } }
		public static HtmlString ExportIcon { get { return GetFontAwesomeIcon("fas fa-download"); } }
		public static HtmlString RecycleIcon { get { return GetFontAwesomeIcon("fas fa-recycle"); } }
		public static HtmlString ExtractIcon { get { return GetFontAwesomeIcon("fas fa-edit"); } }
		public static HtmlString PreviewIcon { get { return GetFontAwesomeIcon("fas fa-eye"); } }
		public static HtmlString ReportIcon { get { return GetFontAwesomeIcon("fas fa-list"); } }
		public static HtmlString MapIcon { get { return GetFontAwesomeIcon("fas fa-map"); } }
		public static HtmlString RenderIcon { get { return GetFontAwesomeIcon("far fa-newspaper"); } } //fa-seedling


		// ==================================================
		// Object Types
		public static HtmlString ListIcon { get { return GetFontAwesomeIcon("fas fa-list"); } }
		public static HtmlString BoundsIcon { get { return GetFontAwesomeIcon("fas fa-expand"); } }
		public static HtmlString StartIcon { get { return GetFontAwesomeIcon("fas fa-sign-out-alt"); } }
		public static HtmlString FinishIcon { get { return GetFontAwesomeIcon("fas fa-sign-in-alt"); } }
		public static HtmlString ElevationIcon { get { return GetFontAwesomeIcon("fas fa-chart-area"); } }


		// ==================================================
		// Entity Types
		public static HtmlString FotoIcon { get { return GetFontAwesomeIcon("fas fa-camera"); } }
		public static HtmlString TrailIcon { get { return GetFontAwesomeIcon("fas fa-map-signs"); } }
		public static HtmlString TrackIcon { get { return GetFontAwesomeIcon("fas fa-paw"); } }
		public static HtmlString PlaceIcon { get { return GetFontAwesomeIcon("fas fa-map-marker"); } }
		public static HtmlString SheetIcon { get { return GetFontAwesomeIcon("fas fa-sitemap"); } }


		// ==================================================
		// Namespace Types
		public static HtmlString BioIcon { get { return GetFontAwesomeIcon("fas fa-heartbeat"); } }
		public static HtmlString GeoIcon { get { return GetFontAwesomeIcon("fas fa-globe"); } }
		public static HtmlString TopoIcon { get { return GetFontAwesomeIcon("fas fa-code-branch"); } }
		public static HtmlString CartoIcon { get { return GetFontAwesomeIcon("fas fa-map"); } }
		public static HtmlString OrthoIcon { get { return GetFontAwesomeIcon("fas fa-sitemap"); } }

	}

	public class FlagHelper
	{
		public static HtmlString GetFlagIcon(GeoCountryInfo country)
		{
			var css = "flag flag-" + country.ISO2.ToLowerInvariant();
			return new HtmlString($"<img src='/images/sprites/blank.gif' class='flag flag-" + css + "' />");
		}
	}

	// TODO: migrate to helper folder and IconHelper name
	public class SvgHelper
	{
		public static HtmlString GetIcon(string name)
		{
			var root = HttpContext.Current.Server.MapPath(@"\Images\Icons");
			if (name.Contains("/")) root = @"C:\Code\KnE\Icons\";
			string path = Path.Combine(root, name + ".svg");
			if (File.Exists(path))
				return new HtmlString(File.ReadAllText(path));
			else
				return new HtmlString("");
		}

		public static HtmlString GetPlaceTypeIcon(string placeType)
		{
			string path = Path.Combine(AutoConfig.IconSourceUri, "places", placeType + ".svg");
			if (File.Exists(path))
				return new HtmlString(File.ReadAllText(path));
			else
				return new HtmlString("");
		}
	}
}