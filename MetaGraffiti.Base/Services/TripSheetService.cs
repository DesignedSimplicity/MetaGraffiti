using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Geo.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Services
{
	public class TripSheetService
	{
		private object _lock = true;
		private XlsFileData _source;

		public void Reset()
		{
			_source = null;
		}

		public void Load(string uri)
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

		public List<int> ListYears()
		{
			var list = new List<int>();
			foreach (var sheet in _source.Sheets)
			{
				var year = TypeConvert.ToInt(sheet.SheetName);
				if (year > 0) list.Add(year);
			}
			return list.OrderBy(x => x).ToList();
		}

		public List<GeoCountryInfo> ListCountries()
		{
			var list = new List<GeoCountryInfo>();

			foreach(var c in ListPlaces().Select(x => x.Country).Distinct())
			{
				var country = GeoCountryInfo.Find(c);
				if (country != null && !list.Any(x => x.CountryID == country.CountryID)) list.Add(country);
			}
			return list.OrderBy(x => x.Name).ToList();
		}


		public List<XlsSheetData> ListSheets()
		{
			return _source.Sheets;
		}

		public XlsSheetData GetSheet(string name)
		{
			return _source.Sheets.FirstOrDefault(x => String.Compare(x.SheetName, name, true) == 0);
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


		

		public List<CartoPlaceData> ListPlaces(int? year = null, string country = "")
		{
			var places = new List<CartoPlaceData>();

			var c = GeoCountryInfo.Find(country);
			var hasCountry = c != null;
			foreach (var sheet in _source.Sheets)
			{
				var y = TypeConvert.ToInt(sheet.SheetName);				
				var isYear = (y > 0);
				var hasYear = year.HasValue;
				var thisYear = y == (year ?? -1);				
				if (isYear && (!hasYear || thisYear))
				{
					foreach (var p in ListRawPlaces(y))
					{
						var place = ParsePlace(p);
						if (place != null && !places.Any(x => IsSamePlace(x, place)))
						{
							if (!hasCountry)
								places.Add(place);
							else
							{
								var pc = GeoCountryInfo.Find(place.Country);
								if (pc != null && pc.CountryID == c.CountryID) places.Add(place);
							}
						}
					}
				}
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
								p.Locality = area;
								p.Name = name;
							}
						}
						else
						{
							p.Locality = area;
							p.Name = name;
						}
					}
				}
			}

			return p;
		}

		private bool IsSamePlace(CartoPlaceData p1, CartoPlaceData p2)
		{
			return String.Compare(p1.Country, p2.Country, true) == 0
				&& String.Compare(p1.Region, p2.Region, true) == 0
				//&& String.Compare(p1.Locality, p2.Locality, true) == 0
				&& String.Compare(p1.Name, p2.Name, true) == 0;
		}
	}
}
