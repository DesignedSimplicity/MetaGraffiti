using MetaGraffiti.Base.Modules.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Carto.Data
{
	public class CartoPlaceData : IGeoPoliticalData, ICartoPlaceBoundsData
	{
		public string PlaceKey { get; set; }
		public string PlaceType { get; set; }
		public string PlaceTags { get; set; } // TODO: CARTO: RC2 implement

		public string GoogleKey { get; set; } // TODO: REFACTOR: change this to GoogleID
		public string IconKey { get; set; } 

		public string Name { get; set; }
		public string LocalName { get; set; }
		public string DisplayAs { get; set; }

		public string Description { get; set; }
		public string Address { get; set; }
		public string Locality { get; set; }
		public string Postcode { get; set; }

		public string Subregions { get; set; }
		public string Sublocalities { get; set; }


		public string KnowAliases { get; set; } // TODO: CARTO: RC2 implement
		public string UrlLink { get; set; } // TODO: CARTO: RC2 implement
		public string UrlText { get; set; } // TODO: CARTO: RC2 implement

		// IGeoPoliticalData
		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }


		// ICartoPlaceBoundsData
		public double CenterLatitude { get; set; }
		public double CenterLongitude { get; set; }
		public double NorthLatitude { get; set; }
		public double SouthLatitude { get; set; }
		public double WestLongitude { get; set; }
		public double EastLongitude { get; set; }


		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
	}
}
