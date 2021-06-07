using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class JpgFileData
	{
		public IImgDimenions Dimenions { get; set; }
		public IImgFileData File { get; set; }
		public ImgExifData Exif { get; set; }
	}
}
