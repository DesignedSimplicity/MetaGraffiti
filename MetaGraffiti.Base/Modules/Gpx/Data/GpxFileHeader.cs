﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
    public class GpxFileHeader
    {
		// primary attributes
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime? Timestamp { get; set; }

		// additional attributes
		/*
		public string Author { get; set; }
		public string Email { get; set; }
		public string Url { get; set; }
		public string UrlName { get; set; }
		public string Keywords { get; set; }
		public DateTime? Created { get; set; }
		*/
	}
}
