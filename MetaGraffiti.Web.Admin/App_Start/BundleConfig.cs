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
						.Include("~/Scripts/_graffiti/common.js")
						.Include("~/Scripts/_graffiti/googleMaps.js")
						);

			bundles.Add(new StyleBundle("~/bundle/css")
						.Include("~/Content/bootstrap.css")
						.Include("~/Content/bootstrap-responsive.css")
						.Include("~/Content/_graffiti/common.css")
						);
		}
	}
}