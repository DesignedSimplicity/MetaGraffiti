using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class ImgXmpData : ImgExifData
	{
		// aux
		public string Lens { get; set; }
		public string SerialNumber { get; set; }
		public string LensSerialNumber { get; set; }
		public string FlashCompensation { get; set; }
	}
}
