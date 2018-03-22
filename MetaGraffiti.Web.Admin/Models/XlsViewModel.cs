using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Web.Admin.Models
{
	public class XlsViewModel : AdminViewModel
	{
		public int[] Years = { 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017 };

		public List<XlsSheetData> Sheets { get; set; }
		public string SelectedSheet { get; set; }


		public int RawCount { get; set; }
		public List<GeoLocationData> Places { get; set; }

		public List<XlsRowModel> SelectedRows { get; set; }

		public IEnumerable<GeoLocationData> ListPlacesOrdered() { return Places.OrderBy(x => x.Country).ThenBy(x => x.Region).ThenBy(x => x.Locality).ThenBy(x => x.Name); }

		public IEnumerable<XlsRowModel> ListRowsOrdered() { return SelectedRows.OrderBy(x => x.Place.Country).ThenBy(x => x.Place.Region).ThenBy(x => x.Place.Locality).ThenBy(x => x.Place.Name); }

		public List<GeoCountryInfo> Countries { get; set; }


		public string GetRowCss(XlsRowModel row)
		{
			if (row.Place.Country == null)
				return "table-danger";
			else if (row.Location != null)
				return "table-success";
			else
				return "";
		}
	}

	public class XlsRowModel
	{
		public GeoLocationData Place { get; set; }
		public CartoPlaceInfo Location { get; set; }
	}
}