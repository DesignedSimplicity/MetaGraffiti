using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class ImgFileData : IImgBasicData
	{
		public string Uri { get; private set; }

		public int Width { get; private set; }
		public int Height { get; private set; }

		public DateTime DateCreated { get; private set; }

		public ImgJpgData Thumb { get; set; }
		public ImgJpgData Image { get; set; }

		public IImgMetaData Metadata { get; set; }

		public ImgFileData(string uri, IImgBasicData metadata)
		{
			Uri = uri;
			
			Width = metadata.Width;
			Height = metadata.Height;
			DateCreated = metadata.DateCreated;
		}

		public ImgFileData(string uri, IImgMetaData metadata)
		{
			Uri = uri;
			Metadata = metadata;
			
			Width = metadata.Width;
			Height = metadata.Height;
			DateCreated = metadata.DateCreated;			
		}
	}
}
