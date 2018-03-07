using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Carto.Data
{
	public class CartoPlaceData
	{
		public string Name { get; set; }

		public string Area { get; set; }

		public string Region { get; set; }

		public string Country { get; set; }

		public bool IsSamePlace(CartoPlaceData place)
		{
			return String.Compare(Country, place.Country, true) == 0
				&& String.Compare(Region, place.Region, true) == 0
				&& String.Compare(Area, place.Area, true) == 0
				&& String.Compare(Name, place.Name, true) == 0;
		}
	}
}
