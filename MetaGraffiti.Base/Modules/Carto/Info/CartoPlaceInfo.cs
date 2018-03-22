using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Carto.Info
{
    public class CartoPlaceInfo : ICacheEntity
	{
		private CartoPlaceData _data;

		public string Key => _data.PlaceKey;

		public string Name => _data.Name;

		public CartoPlaceInfo(CartoPlaceData data)
		{
			_data = data;
		}
	}
}
