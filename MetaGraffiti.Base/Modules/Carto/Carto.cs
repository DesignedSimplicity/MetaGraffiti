using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Modules.Carto
{
	public enum CartoPlaceTypes
	{
		Country = 1,
		Region = 2,
	}




	public enum CartoLayerTypes
	{
		/// <summary>
		/// Reserved for Geo module
		/// </summary>
		Base = 1,
	}



	public class CartoLayer
	{
		public CartoLayerTypes LayerType { get; set; }

		public string LayerName { get; set; }
	}

	public class CartoMark
	{
		public CartoMark(CartoPoint point) { }

		public CartoPoint Point { get; set; }
	}

	public class CartoPoint : GeoPosition
	{
		public CartoPoint(GeoPosition location) : base(location.Latitude, location.Longitude, location.Elevation, location.Timestamp) { }

		public CartoLayer Layer { get; set; }
	}

	public class CartoArea
	{		
	}
}
