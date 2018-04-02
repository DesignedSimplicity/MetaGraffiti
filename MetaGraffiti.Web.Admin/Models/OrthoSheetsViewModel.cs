using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaGraffiti.Web.Admin.Models
{
	public class OrthoSheetsViewModel : OrthoViewModel
	{
		public List<XlsSheetData> Sheets { get; set; }

		public string SelectedSheet { get; set; }

		public bool IsSelected(XlsSheetData sheet)
		{
			return (String.Compare(sheet.SheetName, SelectedSheet, true) == 0);
		}
	}
}