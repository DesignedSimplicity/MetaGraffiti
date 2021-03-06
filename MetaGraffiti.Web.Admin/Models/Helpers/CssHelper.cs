﻿using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class CssHelper
	{
		/// <summary>
		/// Marks field as vaild or invalid
		/// </summary>
		public static string GetFieldValidCss(bool valid)
		{
			return (valid ? "is-valid" : "is-invalid");
		}

		/// <summary>
		/// Highlights invalid fields only
		/// </summary>
		public static string GetFieldInvalidCss(bool valid)
		{
			return (valid ? "" : "is-invalid");
		}


		public static string GetAreaCss(AdminAreas area)
		{
			switch (area)
			{
				//case AdminAreas.Geo: return "info";
				//case AdminAreas.Trail: return "topo-color";
				//case AdminAreas.Track: return "topo-color";
				//case AdminAreas.Carto: return "primary";
				//case AdminAreas.Place: return "carto-color";
				//case AdminAreas.Ortho: return "secondary";
				default: return area.ToString().ToLowerInvariant() + "-color";
			}
		}

		public static string GetSortCss(bool selected)
		{
			return (selected ? "text-light" : "text-secondary");
		}

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


		public static string GetGpxDOPCss(decimal? dop)
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

		public static string GetGpxSpeedCss(decimal? speed)
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

		public static string GetGpxSatsCss(int? sats)
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
	}
}