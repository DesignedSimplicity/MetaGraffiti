using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public enum AdminArea { Home, Geo }

	public class AdminViewModel
	{
		public AdminArea Area {
			get
			{
				var path = UrlPath.ToLowerInvariant();
				if (path.StartsWith("/geo"))
					return AdminArea.Geo;
				else
					return AdminArea.Home;
			}
		}
		public string PageName
		{
			get
			{
				var path = UrlPath.Trim('/');
				var index = path.IndexOf('/');
				return index > 0
					? path.Substring(index + 1, 1).ToUpperInvariant() + path.Substring(index + 2)
					: "";				
			}
		}

		public bool IsHome => Area == AdminArea.Home;

		public bool IsAreaPage => !IsHome && !String.IsNullOrWhiteSpace(PageName);
		

		public string UrlPath
		{
			get { return HttpContext.Current.Request.Url.AbsolutePath; }
		}
	}
}