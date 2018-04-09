using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Ortho.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Carto.Data
{
    public class CartoPlaceSheetData
    {
		private static string[] _columns = {
			"PlaceKey", "PlaceType", "PlaceTags",
			"GoogleKey", "IconKey", 
			"Timezone", "Country", "Region",

			"Name", "LocalName", "DisplayAs", "Description",
			"Address", "Locality", "Postcode",
			"Subregions", "Sublocalities",

			"CenterLatitude", "CenterLongitude",
			"NorthLatitude", "SouthLatitude",
			"WestLongitude", "EastLongitude",

			"Created", "Updated"
		};


		private XlsSheetData _sheet;
		protected Dictionary<string, int> ColumnIndex { get; private set; } = new Dictionary<string, int>();
		protected List<string> MissingColumns { get; private set; } = new List<string>();
		protected List<string> UnknownColumns { get; private set; } = new List<string>();


		public CartoPlaceSheetData(XlsSheetData sheet)
		{
			_sheet = sheet;
			IndexColumns();
		}

		private object _lock = new System.Object();
		private List<CartoPlaceData> _rows = null;
		public List<CartoPlaceData> Rows
		{
			get
			{
				lock (_lock)
				{
					if (_rows == null) ParseRowData();
					return _rows;
				}
			}
		}

		private void ParseRowData()
		{
			_rows = new List<CartoPlaceData>();

			// ship first row
			var rows = _sheet.Rows.ToList();
			for(int index = 1; index < rows.Count(); index++)
			{
				var row = rows[index];
				var place = new CartoPlaceData();

				place.PlaceKey = ExtractText(row, "PlaceKey");

				if (!String.IsNullOrWhiteSpace(place.PlaceKey))
				{
					place.PlaceType = ExtractText(row, "PlaceType");
					place.PlaceTags = ExtractText(row, "PlaceTags");

					place.GoogleKey = ExtractText(row, "GoogleKey");
					place.IconKey = ExtractText(row, "IconKey");

					place.Timezone = ExtractText(row, "Timezone");
					place.Country = ExtractText(row, "Country");
					place.Region = ExtractText(row, "Region");

					place.Name = ExtractText(row, "Name");
					place.LocalName = ExtractText(row, "LocalName");
					place.DisplayAs = ExtractText(row, "DisplayAs");
					place.Description = ExtractText(row, "Description");

					place.Address = ExtractText(row, "Address");
					place.Locality = ExtractText(row, "Locality");
					place.Postcode = ExtractText(row, "Postcode");
					place.Subregions = ExtractText(row, "Subregions");
					place.Sublocalities = ExtractText(row, "Sublocalities");

					place.CenterLatitude = ExtractDouble(row, "CenterLatitude");
					place.CenterLongitude = ExtractDouble(row, "CenterLongitude");
					place.NorthLatitude = ExtractDouble(row, "NorthLatitude");
					place.SouthLatitude = ExtractDouble(row, "SouthLatitude");
					place.WestLongitude = ExtractDouble(row, "WestLongitude");
					place.EastLongitude = ExtractDouble(row, "EastLongitude");

					place.Created = ExtractDateTime(row, "Created");
					place.Updated = ExtractDateTime(row, "Updated");

					_rows.Add(place);
				}
			}
		}


		private string ExtractText(XlsRowData row, string column)
		{
			if (ColumnIndex.ContainsKey(column))
			{
				var index = ColumnIndex[column];
				return row.TextCells[index];
			}
			else
				return String.Empty;
		}

		private double ExtractDouble(XlsRowData row, string column)
		{
			if (ColumnIndex.ContainsKey(column))
			{
				var index = ColumnIndex[column];
				return TypeConvert.ToDouble(row.Cells[index]);
			}
			else
				return 0;
		}

		private DateTime? ExtractDateTime(XlsRowData row, string column)
		{
			if (ColumnIndex.ContainsKey(column))
			{
				var index = ColumnIndex[column];
				return TypeConvert.ToDateTimeNull(row.Cells[index]);
			}
			else
				return null;
		}

		private void IndexColumns()
		{
			// assume first row is header with names
			var header = _sheet.Rows[0];
			for (var index = 0; index < header.Cells.Length; index++)
			{
				var name = header.TextCells[index];
				if (!String.IsNullOrWhiteSpace(name))
				{
					var column = _columns.FirstOrDefault(x => String.Compare(x, name, true) == 0);
					if (column == null)
						UnknownColumns.Add(column); // unknown column
					else
						ColumnIndex.Add(column, index);
				}
			}

			// figure out which columns are missing
			foreach(var col in _columns)
			{
				if (!ColumnIndex.ContainsKey(col)) MissingColumns.Add(col);
			}
		}
	}
}
