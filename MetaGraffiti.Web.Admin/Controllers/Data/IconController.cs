using MetaGraffiti.Web.Admin.Models;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class IconController : Controller
    {
        public ActionResult PlaceType(string id)
        {
			var root = AutoConfig.IconSourceUri;
			var path = Path.Combine(root, id + ".svg");
			if (!System.IO.File.Exists(path)) path = Path.Combine(root, "place.svg");

			using (var stream = new MemoryStream())
			{
				var svgDocument = SvgDocument.Open(path);
				using (var bitmap = svgDocument.Draw())
				{
					bitmap.Save(stream, ImageFormat.Png);
					stream.Position = 0;
					var bytes = stream.ToArray();
					return new FileContentResult(bytes, "image/png");
				}
			}
		}
    }
}