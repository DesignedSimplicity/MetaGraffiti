using System;
using System.Linq;
using System.Collections.Generic;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Services.Internal;
using MetaGraffiti.Base.Modules.Ortho;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoTrailInfo : ITopoTrailInfo, ICacheEntity, IGpxFileHeader
	{
		public TopoTrailInfo()
		{
		}

		public TopoTrailInfo(GpxFileData data)
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

			foreach (var track in data.Tracks)
			{
				var t = new TopoTrackInfo(this, track);
				_tracks.Add(t);
			}
		}

		public TopoTrailInfo(ITopoTrailData data, List<GpxTrackData> tracks)
		{
			Name = data.Name;
			Description = data.Description;

			Keywords = data.Keywords;
			UrlText = data.UrlText;
			UrlLink = data.UrlLink;

			Timezone = data.Timezone;
			if (Timezone == null) Timezone = GeoTimezoneInfo.UTC;

			Country = data.Country;
			Region = data.Region;
			Location = data.Location;

			foreach (var track in tracks)
			{
				var t = new TopoTrackInfo(this, track);
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


		private List<TopoTrackInfo> _tracks = new List<TopoTrackInfo>();
		public IEnumerable<TopoTrackInfo> TopoTracks => _tracks.OrderBy(x => x.StartUTC);
		public IEnumerable<ITopoTrackInfo> Tracks => TopoTracks;

		public TopoStats Stats { get { return TopoStats.FromTrail(this); } }


		public IEnumerable<CartoPlaceInfo> Places
		{
			get
			{
				var places = new List<CartoPlaceInfo>();
				foreach (var track in TopoTracks)
				{
					var start = track.StartPlace;
					if (start != null && !places.Any(x => x.Key == start.Key)) places.Add(start);

					var finish = track.FinishPlace;
					if (finish != null && !places.Any(x => x.Key == finish.Key)) places.Add(finish);
				}
				return places;
			}
		}

		public void AddTrack_TO_DEPRECATE(TopoTrackInfo track)
		{
			_tracks.Add(track);
		}
	}
}
