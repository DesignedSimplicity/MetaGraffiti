using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Carto;
using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Xls;
using MetaGraffiti.Base.Modules.Xls.Data;

namespace MetaGraffiti.Web.Admin.Models
{
	public class XlsViewModel : AdminViewModel
	{
		public int[] Years = { 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017 };

		public List<XlsSheetData> Sheets { get; set; }
		public string SelectedSheet { get; set; }


		public int RawCount { get; set; }
		public List<CartoPlaceData> Places { get; set; }

		public IEnumerable<CartoPlaceData> ListPlacesOrdered() { return Places.OrderBy(x => x.Country).ThenBy(x => x.Region).ThenBy(x => x.Area).ThenBy(x => x.Name); }

		public List<GeoCountryInfo> Countries { get; set; }


		public bool IsSelected(XlsSheetData sheet)
		{
			return (String.Compare(sheet.SheetName, SelectedSheet, true) == 0);
		}
	}
}