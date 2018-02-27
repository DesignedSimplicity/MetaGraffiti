using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MetaGraffiti.Web.Admin
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "GpxView",
				url: "Gpx/{year}/{month}/{file}",
				defaults: new { controller = "Gpx", action = "View" }
			);

			routes.MapRoute(
				name: "GpxList",
				url: "Gpx/{year}/{month}",
				defaults: new { controller = "Gpx", action = "List", month = UrlParameter.Optional }
			);



			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
