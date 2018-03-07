using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Carto;
using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Xls;
using MetaGraffiti.Base.Modules.Xls.Data;

namespace MetaGraffiti.Base.Services
{
	public class CartoDataService
	{
		private static object _lock = true;
		private static XlsFileData _source;

		public void Reset()
		{
			_source = null;
		}

		public void Init(string uri)
		{
			lock (_lock)
			{
				if (_source == null)
				{
					var reader = new XlsFileReader(uri);
					_source = reader.ReadFile();
				}
			}
		}

		public List<XlsSheetData> ListSheets()
		{
			return _source.Sheets;
		}

		public List<string> ListRawPlaces(int year)
		{
			var places = new List<string>();
			var sheet = _source.Sheets.Where(x => x.SheetName == year.ToString()).First();
			var rows = sheet.Rows.Select(x => x.Cells.First());
			foreach (var row in rows)
			{
				var place = TypeConvert.ToString(row);
				if (!String.IsNullOrWhiteSpace(place))
				{
					place = place.Trim();
					if (place.ToUpperInvariant() != "FROM")
					{
						if (!places.Any(x => String.Compare(x, place, true) == 0)) places.Add(place);
					}
				}
			}
			return places;
		}

		public List<CartoPlaceData> ListPlaces(int year)
		{
			var places = new List<CartoPlaceData>();
			foreach (var p in ListRawPlaces(year))
			{
				var place = ParsePlace(p);
				if (place != null) places.Add(place);
			}
			return places;
		}


		public CartoPlaceData ParsePlace(string place)
		{
			var p = new CartoPlaceData();
			p.Name = place;

			var index = place.LastIndexOf(',');
			if (index > 0)
			{
				var name = place.Substring(0, index).Trim();
				var countryCode = place.Substring(index + 1).Trim();

				var country = GeoCountryInfo.ByISO(countryCode);
				if (country != null)
				{
					p.Name = name;
					p.Country = country.Name;

					index = name.LastIndexOf(',');
					if (index > 0)
					{
						var area = name.Substring(index + 1).Trim();
						name = name.Substring(0, index).Trim();
						if (country.HasRegions)
						{
							var region = GeoRegionInfo.ByISO(country.ISO2, area);
							if (region != null)
							{
								p.Region = region.RegionName;
								p.Name = name;
							}
							else
							{
								p.Area = area;
								p.Name = name;
							}
						}
						else
						{
							p.Area = area;
							p.Name = name;
						}
					}
				}
			}

			return p;
		}
	}
}
