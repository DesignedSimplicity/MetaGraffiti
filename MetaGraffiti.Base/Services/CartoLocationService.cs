using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo.Info;

namespace MetaGraffiti.Base.Services
{
    public class CartoLocationService
    {
		private static Dictionary<string, GeoLocationInfo> _cache = new Dictionary<string, GeoLocationInfo>();


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
			var id = location.PlaceKey.ToUpperInvariant();
			if (_cache.ContainsKey(id))
				_cache[id] = location;
			else
				_cache.Add(id, location);
		}
    }
}
