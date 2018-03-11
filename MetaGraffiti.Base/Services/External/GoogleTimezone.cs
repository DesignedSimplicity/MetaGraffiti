using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Services.External
{
	// https://developers.google.com/maps/documentation/timezone/start
	public class GoogleTimezoneResponse
	{
		public string Status { get; set; }
		public string TimeZoneId { get; set; }
		public string TimeZoneName { get; set; }
	}
}
