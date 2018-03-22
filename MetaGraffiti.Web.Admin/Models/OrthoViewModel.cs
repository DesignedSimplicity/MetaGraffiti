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
	public class OrthoViewModel : AdminViewModel
	{
		public List<XlsSheetData> Sheets { get; set; }
		public string SelectedSheet { get; set; }

		public bool IsSelected(XlsSheetData sheet)
		{
			return (String.Compare(sheet.SheetName, SelectedSheet, true) == 0);
		}
	}
}