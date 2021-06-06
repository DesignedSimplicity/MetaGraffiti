using MetaGraffiti.Base.Modules.Carto.Info;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Modules.Ortho.Data;
using MetaGraffiti.Base.Modules.Ortho.Info;
using MetaGraffiti.Base.Modules.Topo;
using MetaGraffiti.Base.Modules.Topo.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetaGraffiti.Web.Admin.Models
{
	public class OrthoTracksViewModel : OrthoViewModel
	{
		// ==================================================
		// Required
		public DirectoryInfo TrackSourceRoot { get; set; }
		public List<OrthoTrackImportModel> Sources { get; set; }


		// ==================================================
		// Optional
		public DirectoryInfo SelectedDirectory { get; set; }


		// ==================================================
		// Helpers
		public bool HasSources => Sources != null && Sources.Count > 0;

		public bool IsSelected(DirectoryInfo dir)
		{
			if (SelectedDirectory == null || dir == null) return false;

			return (String.Compare(SelectedDirectory.FullName.TrimEnd('\\'), dir.FullName.TrimEnd('\\'), true) == 0);
		}

		public bool IsAncestor(DirectoryInfo dir)
		{
			if (SelectedDirectory == null || dir == null) return false;

			var uri = SelectedDirectory.FullName.TrimEnd('\\') + '\\';
			return uri.StartsWith(dir.FullName.TrimEnd('\\') + '\\', StringComparison.InvariantCultureIgnoreCase);
		}
	}

	// ==================================================
	// Additional
	public class OrthoTrackImportModel
	{
		public FileInfo File { get; set; }
		public GpxFileInfo Data { get; set; }

		public ITopoTrailInfo Trail { get; set; }

		public TopoTrailInfo Preview { get; set; }

		public List<GeoRegionInfo> Regions { get; set; } = new List<GeoRegionInfo>();
		public List<CartoPlaceInfo> Places { get; set; } = new List<CartoPlaceInfo>();
		public List<CartoPlaceInfo> Nearby { get; set; } = new List<CartoPlaceInfo>();

		public bool ContainsPlace(CartoPlaceInfo place)
		{
			return Places.Any(x => x.Key == place.Key);
		}
		public bool IsStart(CartoPlaceInfo place)
		{
			foreach(var track in Preview.TopoTracks)
			{
				if (track.StartPlace?.Key == place.Key) return true;
			}
			return false;
		}
		public bool IsFinish(CartoPlaceInfo place)
		{
			foreach (var track in Preview.TopoTracks)
			{
				if (track.FinishPlace?.Key == place.Key) return true;
			}
			return false;
		}
	}
}