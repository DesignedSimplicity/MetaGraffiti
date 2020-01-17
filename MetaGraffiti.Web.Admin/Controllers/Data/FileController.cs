using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers.Data
{
    public class FileController : Controller
    {
        public ActionResult Gpx(string id)
        {
			var topoTrailService = ServiceConfig.TopoTrailService;
			var path = topoTrailService.GetTrailUri(id);
			return new FilePathResult(path, "text/gpx");
		}
    }
}