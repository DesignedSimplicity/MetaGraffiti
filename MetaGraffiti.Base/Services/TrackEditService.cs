using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Services
{
    public class TrackEditService
    {
		private static TopoTrailInfo2 _existing;

		private static ITopoTrailData _trail;
		private static List<TrackExtractData2> _extracts;

		public ITopoTrailData GetTrail()
		{
			return (_trail == null ? new TopoTrailInfo2() : _trail);
		}

		public List<GpxTrackData> ListTracks()
		{
			var tracks = new List<GpxTrackData>();
			var index = 0;
			foreach(var extract in _extracts)
			{
				var track = new GpxTrackData();
				track.Number = index++;
				track.Source = extract.Source;

				track.Name = extract.Name;
				track.Description = extract.Description;
				
				track.Points = extract.Points;

				tracks.Add(track);
			}
			return tracks;
		}

		public void UpdateTrail(ITopoTrailData data)
		{
			_trail = data;
		}


		public void ModifyTrail(TopoTrailInfo2 trail)
		{
			_existing = trail;
			_trail = trail;
			_extracts = _existing.TopoTracks.Select(x => ExtractTrack(x)).ToList();
		}

		public TopoTrailInfo2 PrepareTrail()
		{
			var trail = new TopoTrailInfo2(_trail, ListTracks());

			trail.Key = _existing?.Key;
			trail.Source = _existing?.Source;

			return trail;
		}

		


		private TrackExtractData2 ExtractTrack(TopoTrackInfo2 track)
		{
			var extract = new TrackExtractData2();

			extract.Track = track.TrackData;
			extract.Source = track.Source;

			extract.Name = track.Name;
			extract.Description = track.Description;

			extract.Points = track.TrackData.Points.ToList();

			return extract;
		}



	}

	public class TrackExtractData2
	{
		public GpxTrackData Track { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public string Source { get; set; }
		public List<GpxPointData> Points { get; set; }
	}
}
