using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public enum AdminArea { Home, Geo, Gpx }

	public class AdminViewModel
	{
		public AdminArea Area
		{
			get
			{
				var path = UrlPath.ToLowerInvariant();
				if (path.StartsWith("/geo"))
					return AdminArea.Geo;
				else if (path.StartsWith("/gpx"))
					return AdminArea.Gpx;
				else
					return AdminArea.Home;
			}
		}

		private string _pageTitle = String.Empty;
		public string PageTitle
		{
			get
			{
				return String.IsNullOrWhiteSpace(_pageTitle)
					? UrlPath.Trim('/').Replace(@"/", " : ")
					: _pageTitle;
			}
			set { _pageTitle = value; }
		}

		private string _pageName = String.Empty;
		public string PageName
		{
			get
			{
				if (_pageName != String.Empty)
					return _pageName;

				var path = UrlPath.Trim('/');
				var index = path.IndexOf('/');
				return index > 0
					? path.Substring(index + 1, 1).ToUpperInvariant() + path.Substring(index + 2)
					: "";
			}
			set { _pageName = value; }
		}

		public bool IsHome => Area == AdminArea.Home;

		public bool IsAreaPage => !IsHome && !String.IsNullOrWhiteSpace(PageName);
		

		public string UrlPath
		{
			get { return HttpContext.Current.Request.Url.AbsolutePath; }
		}
	}
}