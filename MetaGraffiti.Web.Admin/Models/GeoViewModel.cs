using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GeoViewModel
	{
		public List<GeoTimezoneData> Timezones { get { return GeoTimezoneData.All; } }
		public List<GeoCountryData> Countries { get { return GeoCountryData.All; } }
		public List<GeoRegionData> Regions { get { return GeoRegionData.All; } }
	}
}