using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Xls.Data
{
    public class XlsFileData
    {
		public string FileName { get; set; }

		public long FileSize { get; set; }

		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }

		public List<XlsSheetData> Sheets { get; set; }
    }
}
