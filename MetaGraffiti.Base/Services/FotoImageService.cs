using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetaGraffiti.Base.Services
{
	public class FotoImageService
	{
		public byte[] GetThumb(string uri, int size)
		{
			using (var magick = new MagickImage(uri))
			{
				magick.Scale(size, size);
				using (var stream = new MemoryStream())
				{
					magick.Write(stream);
					stream.Seek(0, SeekOrigin.Begin);
					return stream.ToArray();
				}
			}
		}		
	}
}
