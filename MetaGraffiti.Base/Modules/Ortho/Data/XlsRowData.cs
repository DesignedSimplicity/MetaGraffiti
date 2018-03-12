using System;
using System.Collections.Generic;
using System.Text;

using MetaGraffiti.Base.Common;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class XlsRowData
    {
		public object[] Cells { get; set; }

		public string[] TextCells
		{
			get
			{
				var text = new List<string>();
				foreach(var cell in Cells)
				{
					text.Add(TypeConvert.ToString(cell));
				}
				return text.ToArray();
			}
		}
	}
}
