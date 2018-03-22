using MetaGraffiti.Base.Modules.Carto.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class JsonViewModel
	{
		public static HtmlString GetJson(IEnumerable<CartoPlaceInfo> places)
		{
			var json = "";
			foreach (var place in places)
			{
				json += place.ToJson() + ",";
			}
			return new HtmlString("[" + json + "]");
		}
	}
}