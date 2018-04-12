using System.Web.Optimization;

namespace MetaGraffiti.Web.Admin
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundle/js")
						.Include("~/Scripts/jquery-{version}.js")
						.Include("~/Scripts/bootstrap.js")
						.Include("~/Scripts/typeahead.bundle.js")						
						.Include("~/Scripts/_graffiti/carto.js")
						.Include("~/Scripts/_graffiti/meta.js")
						.Include("~/Scripts/_graffiti/maps.js")
						.Include("~/Scripts/_graffiti/geo.js")
						.Include("~/Scripts/_graffiti/topo.js")
						.Include("~/Scripts/_graffiti/carto.js")
						);

			bundles.Add(new StyleBundle("~/bundle/css")
						.Include("~/Content/bootstrap.css")
						.Include("~/Content/bootstrap-responsive.css")
						.Include("~/Content/font-awesome.css")
						.Include("~/Content/flags.css")
						.Include("~/Content/_graffiti/common.css")
						.Include("~/Content/_graffiti/typeahead.css")
						);
		}
	}
}