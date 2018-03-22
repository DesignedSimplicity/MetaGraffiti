using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Geo.Data;

namespace MetaGraffiti.Base.Services
{
	public class CartoLocationService
    {
		private static bool _inited = false;
		private static Dictionary<string, GeoLocationInfo> _cache = new Dictionary<string, GeoLocationInfo>();
		private static string[] _columns = {
			"PlaceID", "PlaceKey", "PlaceType", "GoogleKey",
			"Timezone", "Country", "Region",
			"Name", "LocalName", "DisplayAs", "Description",
			"Address", "Locality", "Postcode", "Subregions", "Sublocalities",
			"CenterLatitude", "CenterLongitude", "NorthLatitude", "SouthLatitude", "WestLongitude", "EastLongitude"
		};

		public void Init(string uri)
		{
			lock (_cache)
			{
				if (_inited) return;

				var reader = new XlsFileReader(uri);
				var file = reader.ReadFile();
				var sheet = file.Sheets[0];

				var index = IndexHeader(sheet);
				for (var row = 1; row < sheet.Rows.Count; row++)
				{
					var location = ParseRow(sheet.Rows[row], index);
					if (location != null)
						_cache.Add(location.ID.ToUpperInvariant(), location);
				}
				_inited = true;
			}
		}

		public void RemoveLocation(string key)
		{
			var id = key.ToUpperInvariant();
			_cache.Remove(id);
		}

		public bool ContainsLocation(GeoLocationInfo location)
		{
			var id = location.PlaceKey.ToUpperInvariant();
			return _cache.ContainsKey(id);
		}

		public GeoLocationInfo GetLocation(string key)
		{
			var id = key.ToUpperInvariant();
			return _cache[id];
		}

		public GeoLocationInfo FindLocation(string name, GeoCountryInfo country)
		{
			var search = _cache.Values.Where(x => x.Country.CountryID == country.CountryID);
			search = search.Where(x => String.Compare(x.Name, name, true) == 0);
			return search.FirstOrDefault();
		}

		public List<GeoLocationInfo> ListLocations(GeoCountryInfo country = null)
		{
			if (country == null)
				return _cache.Values.ToList();
			else
				return _cache.Values.Where(x => x.Country.CountryID == country.CountryID).ToList();
		}

		public void CacheLocation(GeoLocationInfo location)
		{
			var id = location.ID.ToUpperInvariant();
			if (_cache.ContainsKey(id))
				_cache[id] = location;
			else
				_cache.Add(id, location);
		}



		private GeoLocationInfo ParseRow(XlsRowData row, Dictionary<string, int> index)
		{
			var location = new GeoLocationData();

			// Identification
			//location.PlaceID =
			location.PlaceKey = TypeConvert.ToString(row.Cells[index["PlaceKey".ToUpperInvariant()]]);
			location.PlaceType = TypeConvert.ToString(row.Cells[index["PlaceType".ToUpperInvariant()]]);
			location.GoogleKey = TypeConvert.ToString(row.Cells[index["GoogleKey".ToUpperInvariant()]]);

			// Political Keys
			location.Timezone = TypeConvert.ToString(row.Cells[index["Timezone".ToUpperInvariant()]]);
			location.Country = TypeConvert.ToString(row.Cells[index["Country".ToUpperInvariant()]]);
			location.Region = TypeConvert.ToString(row.Cells[index["Region".ToUpperInvariant()]]);

			// Logical Name
			location.Name = TypeConvert.ToString(row.Cells[index["Name".ToUpperInvariant()]]);
			location.LocalName = TypeConvert.ToString(row.Cells[index["LocalName".ToUpperInvariant()]]);
			location.DisplayAs = TypeConvert.ToString(row.Cells[index["DisplayAs".ToUpperInvariant()]]);
			location.Description = TypeConvert.ToString(row.Cells[index["Description".ToUpperInvariant()]]);

			// Logical Location
			location.Address = TypeConvert.ToString(row.Cells[index["Address".ToUpperInvariant()]]);
			location.Locality = TypeConvert.ToString(row.Cells[index["Locality".ToUpperInvariant()]]);
			location.Postcode = TypeConvert.ToString(row.Cells[index["Postcode".ToUpperInvariant()]]);
			location.Subregions = TypeConvert.ToString(row.Cells[index["Subregions".ToUpperInvariant()]]);
			location.Sublocalities = TypeConvert.ToString(row.Cells[index["Sublocalities".ToUpperInvariant()]]);

			// Center
			var centerLatitude = TypeConvert.ToDouble(row.Cells[index["CenterLatitude".ToUpperInvariant()]]);
			var centerLongitude = TypeConvert.ToDouble(row.Cells[index["CenterLongitude".ToUpperInvariant()]]);
			location.Center = new GeoPosition(centerLatitude, centerLongitude);

			// Bounds
			var northLatitude = TypeConvert.ToDoubleNull(row.Cells[index["NorthLatitude".ToUpperInvariant()]]);
			var southLatitude = TypeConvert.ToDoubleNull(row.Cells[index["SouthLatitude".ToUpperInvariant()]]);
			var westLongitude = TypeConvert.ToDoubleNull(row.Cells[index["WestLongitude".ToUpperInvariant()]]);
			var eastLongitude = TypeConvert.ToDoubleNull(row.Cells[index["EastLongitude".ToUpperInvariant()]]);
			if (northLatitude.HasValue && southLatitude.HasValue && westLongitude.HasValue && eastLongitude.HasValue)
				location.Bounds = new GeoRectangle(northLatitude.Value, westLongitude.Value, southLatitude.Value, eastLongitude.Value);

			if (String.IsNullOrWhiteSpace(location.PlaceKey))
				return null;
			else
				return new GeoLocationInfo(location);
		}

		private Dictionary<string, int> IndexHeader(XlsSheetData sheet)
		{
			var indexes = new Dictionary<string, int>();
			foreach (var col in _columns)
			{
				indexes.Add(col.ToUpperInvariant(), -1);
			}

			var header = sheet.Rows[0];
			var columns = header.TextCells;
			for (int index = 0; index < columns.Length; index++)
			{
				var col = columns[index].ToUpperInvariant();
				if (indexes.ContainsKey(col))
					indexes[col] = index;
				else
					indexes.Add(col, index);
			}
			return indexes;
		}
	}
}
