using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Web.Admin.Models
{
	public class GeoViewModel : AdminViewModel
	{
		public GeoCountryInfo SelectedCountry { get; set; }

		public List<GeoTimezoneInfo> Timezones { get { return GeoTimezoneInfo.All; } }
		public List<GeoCountryInfo> Countries { get { return GeoCountryInfo.All; } }
		public List<GeoRegionInfo> Regions { get { return GeoRegionInfo.All; } }
	}
}