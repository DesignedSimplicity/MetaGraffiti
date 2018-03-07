﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public enum AdminAreas { Home, Geo, Gpx, Xls }

	public class AdminViewModel
	{
		public AdminAreas Area
		{
			get
			{
				var path = UrlPath.ToLowerInvariant();
				foreach(AdminAreas area in Enum.GetValues(typeof(AdminAreas)))
				{
					if (path.StartsWith("/" + area.ToString().ToLowerInvariant())) return area;
				}
				return AdminAreas.Home;
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

				if (index <= 1) return "";
				
				var name = path.Substring(index + 1, 1).ToUpperInvariant() + path.Substring(index + 2);
				return name.Replace("/", @" \ ");
			}
			set { _pageName = value; }
		}

		public bool IsHome => Area == AdminAreas.Home;

		public bool IsAreaPage => !IsHome && !String.IsNullOrWhiteSpace(PageName);
		

		public string UrlPath
		{
			get { return HttpContext.Current.Request.Url.AbsolutePath; }
		}
	}
}