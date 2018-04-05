using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Carto.Data;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using MetaGraffiti.Base.Services.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Carto.Info
{
    public class CartoPlaceInfo : ICacheEntity
	{
		private CartoPlaceData _data;
		private string _raw;

		public string Key => _data.PlaceKey;

		public string GoogleKey => _data.GoogleKey;
		public string PlaceType => _data.PlaceType;

		public string Name { get { return _data.Name; } set { _data.Name = value; } }
		public string LocalName => _data.LocalName;
		public string DisplayAs => _data.DisplayAs;
		public string Description => _data.Description;
		
		public string Locality => _data.Locality;

		public string Subregions => _data.Subregions;
		public string Address => _data.Address;
		public string Postcode => _data.Postcode;


		public GeoCountryInfo Country { get { return GeoCountryInfo.Find(_data.Country); } set { _data.Country = (value == null ? "" : value.ISO2); } }
		public GeoRegionInfo Region { get { return GeoRegionInfo.Find(_data.Region); } set { _data.Region = (value == null ? "" : value.RegionISO); } }

		public GeoTimezoneInfo Timezone => null; // TODO: fix timezone


		public IGeoLatLon Center => new GeoPosition(_data.CenterLatitude, _data.CenterLongitude);
		public IGeoPerimeter Bounds => new GeoRectangle(_data.NorthLatitude, _data.WestLongitude, _data.SouthLatitude, _data.EastLongitude);



		// TODO: move this to extensions or somewhere else
		public string ToJson()
		{
			dynamic json = new JObject();
			json.name = Name;
			json.key = Key;

			json.center = new JObject();
			json.center.lat = _data.CenterLatitude;
			json.center.lng = _data.CenterLongitude;

			json.bounds = new JObject();
			json.bounds.north = _data.NorthLatitude;
			json.bounds.south = _data.SouthLatitude;
			json.bounds.east = _data.EastLongitude;
			json.bounds.west = _data.WestLongitude;

			return json.ToString();
		}


		public string GetRawData()
		{
			return _raw;
		}


		public CartoPlaceInfo(CartoPlaceData data)
		{
			_data = data;
		}

		public CartoPlaceInfo(GoogleLocationResult result)
		{
			_raw = result.RawJson;
			_data = new CartoPlaceData();

			_data.PlaceKey = result.PlaceID;
			_data.GoogleKey = result.PlaceID;
			
			_data.PlaceType = result.TypedNameSource;

			_data.Name = TextMutate.StripAccents(result.ShortName);
			_data.LocalName = (_data.Name == result.LongName ? "" : result.LongName);
			_data.DisplayAs = (_data.Name == result.TypedName ? "" : result.TypedName);
			_data.Description = result.ColloquialArea;

			//data.Timezone = // TODO: determine timezone
			_data.Country = result.Country;
			_data.Region = result.Region;

			_data.Subregions = "";
			if (!String.IsNullOrWhiteSpace(result.Region2))
				_data.Subregions += result.Region2;
			if (!String.IsNullOrWhiteSpace(result.Region3))
				_data.Subregions += @" \ " + result.Region3;
			if (!String.IsNullOrWhiteSpace(result.Region4))
				_data.Subregions += @" \ " + result.Region4;
			if (!String.IsNullOrWhiteSpace(result.Region5))
				_data.Subregions += @" \ " + result.Region5;

			var address = $"{result.StreeNumber} {result.Route}";
			_data.Address = (String.IsNullOrWhiteSpace(address) ? result.Intersection : address);
			_data.Postcode = result.PostalCode;

			/*
			_data.Premise = result.Premise;
			if (!String.IsNullOrWhiteSpace(result.SubPremise)) _data.Premise += @" \ " + result.SubPremise;
			*/

			_data.Locality = result.Locality;
			_data.Sublocalities = result.SubLocality;

			_data.CenterLatitude = result.Center.Latitude;
			_data.CenterLongitude = result.Center.Longitude;

			_data.NorthLatitude = result.Bounds.NorthWest.Latitude;
			_data.WestLongitude = result.Bounds.NorthWest.Longitude;
			_data.SouthLatitude = result.Bounds.SouthEast.Latitude;
			_data.EastLongitude = result.Bounds.SouthEast.Longitude;

			// update place type based on current data
			MapPlaceType();
		}


		private void MapPlaceType()
		{
			if (_data.Name.EndsWith(" Hut"))
				_data.PlaceType = "Hut";
			else if (_data.Name.EndsWith(" Park"))
				_data.PlaceType = "Park";
			else if (_data.PlaceType == "NaturalFeature")
				_data.PlaceType = "Nature";
			else if (_data.PlaceType == "Locality" && _data.Name == _data.Locality)
				_data.PlaceType = "City";
		}
	}
}
