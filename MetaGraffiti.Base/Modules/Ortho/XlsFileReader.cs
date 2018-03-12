using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ExcelDataReader;

using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Ortho
{
    public class XlsFileReader
    {
		private string _uri = null;

		// ==================================================
		// Constructors
		public XlsFileReader(string uri) { _uri = uri; }

		/// <summary>
		/// Reads all file data into object
		/// </summary>
		public XlsFileData ReadFile()
		{
			var data = ReadHeader();
			data.Sheets = ReadSheets();
			return data;
		}

		private XlsFileData ReadHeader()
		{
			var file = new FileInfo(_uri);

			return new XlsFileData()
			{
				FileName = file.Name,
				FileSize = file.Length,
				Created = file.CreationTime,
				Updated = file.LastWriteTime
			};
		}

		private List<XlsSheetData> ReadSheets()
		{
			var sheets = new List<XlsSheetData>();
			using (var stream = File.Open(_uri, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
				{
					do
					{
						// read each sheet
						var sheet = ReadSheet(reader);
						sheets.Add(sheet);
					} while (reader.NextResult());
				}
			}
			return sheets;
		}

		private XlsSheetData ReadSheet(IExcelDataReader reader)
		{
			var cols = new List<XlsColumnData>();
			var rows = new List<XlsRowData>();
			var sheet = new XlsSheetData()
			{
				SheetName = reader.Name,
				Columns = cols,
				Rows = rows
			};

			//var header = reader.HeaderFooter;
			var count = reader.FieldCount;
			for (var col = 0; col < count; col++)
			{
				var data = typeof(object);
				var name = col.ToString();
				cols.Add(new XlsColumnData()
				{
					Name = name,
					DataType = data
				});
			}

			while (reader.Read())
			{
				var row = new XlsRowData();
				var cells = new object[count];
				for (var col = 0; col < count; col++)
				{
					cells[col] = reader.GetValue(col);
				}
				row.Cells = cells;
				rows.Add(row);
			}

			return sheet;
		}
	}
}
