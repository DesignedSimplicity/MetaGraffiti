using MetaGraffiti.Base.Modules.Carto.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class CartoPlaceFormModel
	{
		public CartoPlaceFormModel(CartoPlaceInfo place) { Place = place; }

		public CartoPlaceInfo Place { get; set; }

		public bool IsPreview { get { return Place.GoogleKey == Place.Key; } }
	}
}