using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public interface IGpxFileHeader
	{
		string Name { get; }
		string Description { get; }
		DateTime? Timestamp { get; }

		string UrlLink { get; }
		string UrlText { get; }
		string Keywords { get; }
	}

	public class GpxFileData : IGpxFileHeader
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime? Timestamp { get; set; }

		// additional attributes
		public string UrlLink { get; set; }
		public string UrlText { get; set; }
		public string Keywords { get; set; }
		/*
		public string Author { get; set; }
		public string Email { get; set; }
		public DateTime? Created { get; set; }
		*/

		public GpxExtensionData Extensions { get; set; }

		public List<GpxTrackData> Tracks { get; set; }
		public List<GpxRouteData> Routes { get; set; }
		public List<GpxPointData> Waypoints { get; set; }
	}

	public class GpxExtensionData
	{
		public string Timezone { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string Location { get; set; }
	}
}
