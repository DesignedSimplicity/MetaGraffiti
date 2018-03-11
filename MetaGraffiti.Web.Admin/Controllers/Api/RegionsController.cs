using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

		public IHttpActionResult GetRegion(string id)
		{
			var r = GeoRegionInfo.Find(id);
			if (r == null) throw new HttpResponseException(HttpStatusCode.NotFound);

			var d = MapRegion(r);
			return Ok(d);
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
