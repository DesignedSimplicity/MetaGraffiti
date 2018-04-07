using MetaGraffiti.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class DataHelper
	{
		public static List<string> PlaceTypes
		{
			get { return ServiceConfig.CartoPlaceService.ListPlaceTypes(); }
		}
	}
}