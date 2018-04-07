using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MetaGraffiti.Base.Modules;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Controllers.Api
{
    public class RegionsController : ApiController
    {
		/*
		public IEnumerable<GeoRegionInfo> GetAllRegions()
		{
			return GeoRegionInfo.All;
		}
		*/

		[HttpGet]
		public IHttpActionResult GetRegion(string id)
		{
			var region = GeoRegionInfo.Find(id);
			return ReturnRegion(region);
		}

		[HttpGet]
		public IHttpActionResult FindRegionByPoint(double lat, double lng)
		{
			var regions = Graffiti.Geo.NearbyRegions(new GeoPosition(lat, lng));
			return ReturnRegions(regions);
		}

		private IHttpActionResult ReturnRegion(GeoRegionInfo region)
		{
			if (region == null) throw new HttpResponseException(HttpStatusCode.NotFound);

			var d = MapRegion(region);
			return Ok(d);
		}
		private IHttpActionResult ReturnRegions(IEnumerable<GeoRegionInfo> regions)
		{
			if (regions == null || regions.Count() == 0) throw new HttpResponseException(HttpStatusCode.NotFound);

			var a = new List<dynamic>();
			foreach (var region in regions)
			{
				a.Add(MapRegion(region));
			}
			return Ok(a);
		}

		private dynamic MapRegion(GeoRegionInfo r)
		{
			dynamic d = new ExpandoObject();
			d.RegionID = r.RegionID;
			d.RegionISO = r.RegionISO;
			d.RegionAbbr = r.RegionAbbr;
			d.RegionName = r.RegionName;
			return d;
		}
	}
}
