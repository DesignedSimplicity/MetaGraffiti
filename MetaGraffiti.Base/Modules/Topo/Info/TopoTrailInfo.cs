using System;
using System.Linq;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public interface ITrailInfo
	{
		string Uri { get; }
		string Name { get; }
		string Description { get; }
		string Keywords { get; }
		string Url { get; }
		string UrlName { get; }

		string Location { get; }
		GeoRegionInfo Region { get; }
		GeoCountryInfo Country { get; }
		GeoTimezoneInfo Timezone { get; }

		/*
		TimeSpan ElapsedTime { get; }
		GeoDistance Distance { get; }
		*/
	}

	public class TopoTrailInfo : ITrailInfo
	{
		public string ID { get; set; }

		public string Uri { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Keywords { get; set; }

		public string Url { get; set; }
		public string UrlName { get; set; }


		public DateTime LocalDate { get; set; }

		public GeoTimezoneInfo Timezone { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoRegionInfo Region { get; set; }
		public string Location { get; set; }


		public List<TopoTrackInfo> Tracks { get; private set; } = new List<TopoTrackInfo>();



		public List<CartoPlaceInfo> ViaPlaces { get; set; }




		public TimeSpan ElapsedTime { get { return TimeSpan.FromHours(Tracks.Sum(x => x.ElapsedTime.TotalHours)); } }
		public string ElapsedTimeText { get { return String.Format("{0:0} h {1:00} m", Math.Floor(ElapsedTime.TotalHours), ElapsedTime.Minutes); } }

		public GeoDistance Distance { get { return GeoDistance.FromKM(TotalKilometers); } }
		public double TotalKilometers { get { return Tracks.Sum(x => x.EstimatedDistance.KM); } }

		public string[] AutoTags { get { return Tracks.SelectMany(x => x.AutoTags).Distinct().ToArray(); } }
	}
}
