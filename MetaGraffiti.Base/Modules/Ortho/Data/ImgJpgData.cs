using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class ImgJpgData : IImgBasicData
	{
		public int Width { get; }
		public int Height { get; }

		public DateTime DateCreated { get; }

		public byte[] JpgData { get; set; }
	}
}
