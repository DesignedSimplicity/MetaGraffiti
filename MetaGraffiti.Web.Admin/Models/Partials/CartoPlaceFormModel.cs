using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Web.Admin.Models
{
	public class CartoPlaceFormModel
	{
		public CartoPlaceFormModel(CartoPlaceInfo place, CartoPlaceData data = null)
		{
			Place = place;
			Data = data ?? place.GetData();
			Political = new GeoPolitical(Data);
		}

		public CartoPlaceData Data { get; private set; }
		public CartoPlaceInfo Place { get; private set; }
		public GeoPolitical Political { get; private set; }

		public bool IsPreview { get { return Place.GoogleKey == Place.Key; } }
		public string Timezone { get { return Political.Timezone?.Name ?? Data.Timezone; } }
		public string Country { get { return Political.Country?.Name ?? Data.Country; } }
		public string Region { get { return Political.Region?.RegionName ?? Data.Region; } }
	}
}