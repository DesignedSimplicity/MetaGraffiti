using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Services.External
{
	// https://developers.google.com/maps/documentation/elevation/start
	// TOOD: https://developers.google.com/maps/documentation/javascript/examples/elevation-paths
	public class GoogleElevationResponse
	{
		public string Status { get; set; }
		public double Elevation { get; set; }
		public double Resolution { get; set; }
	}
}
