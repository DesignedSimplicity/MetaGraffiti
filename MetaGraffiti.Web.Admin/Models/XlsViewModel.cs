using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MetaGraffiti.Base.Modules.Carto;
using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Xls;
using MetaGraffiti.Base.Modules.Xls.Data;

namespace MetaGraffiti.Web.Admin.Models
{
	public class XlsViewModel : AdminViewModel
	{
		public List<XlsSheetData> Sheets { get; set; }

		public int RawPlaceCount { get; set; }
		public List<CartoPlaceData> Places { get; set; }
	}
}