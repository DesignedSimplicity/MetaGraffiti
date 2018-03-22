using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Carto.Data
{
	public class CartoPlaceData
	{
		public string PlaceKey { get; set; }
		public string PlaceType { get; set; }

		public string GoogleKey { get; set; }
		public string IconKey { get; set; }

		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }

		public string Name { get; set; }
		public string LocalName { get; set; }
		public string DisplayAs { get; set; }
		public string Description { get; set; }

		public string Address { get; set; }
		public string Locality { get; set; }
		public string Postcode { get; set; }

		public string Subregions { get; set; }
		public string Sublocalities { get; set; }

		public double CenterLatitude { get; set; }
		public double CenterLongitude { get; set; }
		public double NorthLatitude { get; set; }
		public double SouthLatitude { get; set; }
		public double WestLongitude { get; set; }
		public double EastLongitude { get; set; }

	}
}
