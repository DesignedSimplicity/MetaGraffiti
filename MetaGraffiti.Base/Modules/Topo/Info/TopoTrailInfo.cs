using System;
using System.Linq;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoTrailInfo2 : ITopoTrailInfo, ICacheEntity
	{
		public TopoTrailInfo2(GpxFileData data)
		{
			Name = data.Name;
			Description = data.Description;

			Keywords = data.Keywords;
			UrlText = data.UrlText;
			UrlLink = data.UrlLink;

			Timezone = GeoTimezoneInfo.Find(data.Extensions.Timezone);
			if (Timezone == null) Timezone = GeoTimezoneInfo.UTC;

			Country = GeoCountryInfo.Find(data.Extensions.Country);
			Region = GeoRegionInfo.Find(data.Extensions.Region);
			Location = data.Extensions.Location;

			foreach(var track in data.Tracks)
			{
				var t = new TopoTrackInfo2(this, track);
				_tracks.Add(t);
			}
		}

		public string Key { get; set; }
		public string Source { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public string Keywords { get; set; }
		public string UrlText { get; set; }
		public string UrlLink { get; set; }

		public string Location { get; set; }
		public GeoRegionInfo Region { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoTimezoneInfo Timezone { get; set; }


		public DateTime StartUTC => _tracks.Min(x => x.StartUTC);
		public DateTime StartLocal => Timezone.FromUTC(StartUTC);

		public DateTime FinishUTC => _tracks.Max(x => x.FinishUTC);
		public DateTime FinishLocal => Timezone.FromUTC(FinishUTC);


		private List<TopoTrackInfo2> _tracks = new List<TopoTrackInfo2>();
		public IEnumerable<ITopoTrackInfo> Tracks => _tracks.OrderBy(x => x.StartUTC);
		public IEnumerable<TopoTrackInfo2> TopoTracks => _tracks;

		public TopoStats Stats { get { return TopoStats.FromTrail(this); } }
	}

	public class TopoTrackInfo2 : ITopoTrackInfo
	{
		private GpxTrackData _track;
		private TopoTrailInfo2 _trail;
		private List<TopoPointInfo2> _points;

		public TopoTrackInfo2(TopoTrailInfo2 trail, GpxTrackData track)
		{
			_trail = trail;
			_track = track;
			_points = track.Points.Select(x => new TopoPointInfo2(this, x)).ToList();
		}

		public ITopoTrailInfo Trail => _trail;

		//public string Key => _track.Key;
		public string Source => _track.Source;

		public string Name => _track.Name;
		public string Description => _track.Description;


		public DateTime StartUTC { get { return Points.Min(x => x.Timestamp.Value); } }
		public DateTime StartLocal { get { return Trail.Timezone.FromUTC(StartUTC); } }

		public DateTime FinishUTC { get { return Points.Max(x => x.Timestamp.Value); } }
		public DateTime FinishLocal { get { return Trail.Timezone.FromUTC(FinishUTC); } }

		public TimeSpan ElapsedTime { get { return FinishUTC.Subtract(StartUTC); } }


		public CartoPlaceInfo StartPlace { get; set; }
		public CartoPlaceInfo FinishPlace { get; set; }

		public IEnumerable<ITopoPointInfo> Points => _points;

		public TopoStats Stats { get { return TopoStats.FromTrack(this); } }
	}

	public class TopoPointInfo2 : ITopoPointInfo
	{
		private ITopoTrackInfo _track;
		private IGeoPoint _point;

		public TopoPointInfo2(ITopoTrackInfo track, IGeoPoint point)
		{
			_track = track;
			_point = point;
		}

		public string Description { get; set; }

		public ITopoTrackInfo Track => _track;

		public double Latitude => _point.Latitude;

		public double Longitude => _point.Longitude;

		public double? Elevation => _point.Elevation;

		public DateTime? Timestamp => _point.Timestamp;

		public DateTime LocalTime => _track.Trail.Timezone.FromUTC(_point.Timestamp.Value);
	}







	public class TopoTrailInfo : ITrailInfo, ICacheEntity
	{
		public string Key { get; set; }
		public DateTime Date { get; set; }


		// ITrailData
		public string Name { get; set; }
		public string Source { get; set; }
		public string Description { get; set; }

		public string Keywords { get; set; }
		public string UrlText { get; set; }
		public string UrlLink { get; set; }


		public string Location { get; set; }
		public GeoRegionInfo Region { get; set; }
		public GeoCountryInfo Country { get; set; }
		public GeoTimezoneInfo Timezone { get; set; }












		public List<TopoTrackInfo> Tracks { get; private set; } = new List<TopoTrackInfo>();



		public List<CartoPlaceInfo> ViaPlaces { get; set; }




		public TimeSpan ElapsedTime { get { return TimeSpan.FromHours(Tracks.Sum(x => x.ElapsedTime.TotalHours)); } }
		public string ElapsedTimeText { get { return String.Format("{0:0} h {1:00} m", Math.Floor(ElapsedTime.TotalHours), ElapsedTime.Minutes); } }

		public GeoDistance Distance { get { return GeoDistance.FromKM(TotalKilometers); } }
		public double TotalKilometers { get { return Tracks.Sum(x => x.EstimatedDistance.KM); } }

		public string[] AutoTags { get { return Tracks.SelectMany(x => x.AutoTags).Distinct().ToArray(); } }
	}
}
