using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class XlsSheetData
    {
		public string SheetName { get; set; }

		public List<XlsColumnData> Columns { get; set; }

		public List<XlsRowData> Rows { get; set; }
	}
}
