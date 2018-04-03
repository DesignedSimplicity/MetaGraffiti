using System;
using System.Collections.Generic;
using System.Text;


using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Modules.Geo.Data
{
	public class GeoLocationData
	{
		public string RawData { get; set; }

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

		public string Premise { get; set; }

		public string Subregions { get; set; }
		public string Sublocalities { get; set; }


		public IGeoCoordinate Center { get; set; }
		public IGeoPerimeter Bounds { get; set; }
	}
}
