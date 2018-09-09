using System.Web;
using System.Web.Optimization;

namespace MetaGraffiti.Web.Demo
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));






			bundles.Add(new ScriptBundle("~/js/base").Include(
				"~/Scripts/_js/common.js",
				"~/Scripts/_js/graffiti.js",
				"~/Scripts/_js/mapD3.js",
				"~/Scripts/_js/mapOL.js",
				"~/Scripts/_js/where.js"
				));

			bundles.Add(new ScriptBundle("~/js/maps").Include(
				"~/Scripts/_js/map/OpenLayers.js",
				"~/Scripts/_js/map/d3.v3.min.js",
				"~/Scripts/_js/map/d3.geo.projection.v0.min.js",
				"~/Scripts/_js/map/d3.hexbin.v0.min.js",
				"~/Scripts/_js/map/topojson.v1.min.js"
				));

			bundles.Add(new StyleBundle("~/css/base").Include(
				"~/Content/_css/normalize.css",
				"~/Content/_css/common.css",
				"~/Content/_css/main.css",
				"~/Content/_css/nav.css",
				"~/Content/_css/mapD3.css",
				"~/Content/_css/mapOL.css"
				));
		}
	}
}
