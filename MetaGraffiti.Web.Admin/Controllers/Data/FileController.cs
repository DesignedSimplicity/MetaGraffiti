using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers.Data
{
    public class FileController : Controller
    {
        public FileResult Gpx(string id)
        {
			var topoTrailService = ServiceConfig.TopoTrailService;
			var path = topoTrailService.GetTrailUri(id);
			return new PhysicalFileResult(path, "text/gpx");
		}
    }
}