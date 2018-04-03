using System;
using System.Collections.Generic;
using System.Linq;
using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Topo.Info
{
	public class TopoTrackInfo : ITopoTrackInfo
	{
		private GpxTrackData _track;
		private TopoTrailInfo _trail;
		private List<TopoPointInfo> _points;

		public TopoTrackInfo(TopoTrailInfo trail, GpxTrackData track)
		{
			_trail = trail;
			_track = track;
			_points = track.Points.Select(x => new TopoPointInfo(this, x)).ToList();
		}

		public ITopoTrailInfo Trail => _trail;

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
		public string[] Tags { get { return TopoTags.FromPoints(_track.Points); } }
	}
}