using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Services.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
//using AlloyTemplates;
using System.Web;

namespace MetaGraffiti.Web.Core.Models
{
	public enum AdminAreas { Home, Topo, Carto, Ortho, Geo }

	public class AdminViewModel
	{
		public string AreaCss { get { return CssHelper.GetAreaCss(Area); } }

		public string ConfirmMessage { get; set; }
		public bool HasConfirmation { get { return !String.IsNullOrWhiteSpace(ConfirmMessage); } }

		public List<string> ErrorMessages { get; set; } = new List<string>();
		public bool HasError { get { return ErrorMessages.Count > 0; } }
		public void AddValidationErrors(IEnumerable<ValidationServiceResponseError> errors)
		{
			foreach (var error in errors)
			{
				ErrorMessages.Add(error.Message);
			}
		}


		public AdminAreas[] Areas
		{
			get
			{
				return (AdminAreas[])Enum.GetValues(typeof(AdminAreas));
			}
		}

		public AdminAreas Area
		{
			get
			{
				var path = UrlPath.ToLowerInvariant();
				if (path.StartsWith("/place/"))
					return AdminAreas.Carto;
				else if (path.StartsWith("/trail/"))
					return AdminAreas.Topo;
				else if (path.StartsWith("/track/"))
					return AdminAreas.Topo;

				foreach (AdminAreas area in Enum.GetValues(typeof(AdminAreas)))
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
					? TextMutate.ToTitleCase(UrlPath.ToLowerInvariant().Trim('/').Replace(@"/", " : "))
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
				return TextMutate.ToTitleCase(HttpUtility.UrlDecode(name.ToLowerInvariant().Replace("/", @" \ ")));
			}
			set { _pageName = value; }
		}

		public bool IsHome => Area == AdminAreas.Home;

		public bool IsAreaPage => !IsHome && !String.IsNullOrWhiteSpace(PageName);
		

		public string UrlPath
		{
			get { return AlloyTemplates.HttpContextHelper.Current.Request.Path; }//TODO-FIX get { return HttpContextHelper.Current.Request.Path; }// .Url.AbsolutePath; }
		}
	}
}