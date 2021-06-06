using MetaGraffiti.Web.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class IconController : Controller
    {
		public ActionResult PlaceType(string id, bool svg = false, string color = "")
        {
			var path = IconHelper.GetPlaceTypeIconUri(id);
			var svgContent = System.IO.File.ReadAllText(path);
			
			if (svg)
			{
				/*
				if (argb.HasValue)
				{
					svgContent = svgContent.Replace("<svg", "svg fill='" + argb + "'");
				}
				*/
				return Content(svgContent, "image/svg+xml; charset=utf-8");
			}
			else
			{
				using (var stream = new MemoryStream())
				{
					var svgDocument = SvgDocument.Open(path);
					if (!String.IsNullOrEmpty(color))
					{
						svgDocument.Fill = new SvgColourServer(System.Drawing.ColorTranslator.FromHtml("#" + color));
					}
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
}