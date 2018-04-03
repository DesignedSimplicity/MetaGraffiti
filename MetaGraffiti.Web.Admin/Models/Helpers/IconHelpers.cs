using MetaGraffiti.Base.Modules.Carto.Info;
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
	}
}