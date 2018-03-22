using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Crypto;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Services.External;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Carto.Info
{
    public class CartoPlaceInfo : ICacheEntity
	{
		private CartoPlaceData _data;

		public string Key => _data.PlaceKey;

		public string GoogleKey => _data.GoogleKey;
		public string PlaceType => _data.PlaceType;

		public string Name => _data.Name;
		public string LocalName => _data.LocalName;
		public string DisplayAs => _data.DisplayAs;
		public string Description => _data.Description;
		
		public string Locality => _data.Locality;

		public string Subregions => _data.Subregions;
		public string Address => _data.Address;
		public string Postcode => _data.Postcode;


		public GeoCountryInfo Country => GeoCountryInfo.Find(_data.Country);
		public GeoRegionInfo Region => GeoRegionInfo.Find(_data.Region);


		public IGeoLatLon Center => new GeoPosition(_data.CenterLatitude, _data.CenterLongitude);
		public IGeoPerimeter Bounds => new GeoRectangle(_data.NorthLatitude, _data.WestLongitude, _data.SouthLatitude, _data.EastLongitude);


		public string ToJson()
		{
			dynamic json = new JObject();
			json.name = Name;

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



		public CartoPlaceInfo(CartoPlaceData data)
		{
			_data = data;
		}

		public CartoPlaceInfo(GoogleLocationResult result)
		{
			_data = new CartoPlaceData();

			_data.PlaceKey = result.PlaceID;
			_data.GoogleKey = result.PlaceID;
			
			_data.PlaceType = result.TypedNameSource; // TODO: make better map to place type

			_data.Name = TextTranslate.StripAccents(result.ShortName);
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
		}
	}
}
