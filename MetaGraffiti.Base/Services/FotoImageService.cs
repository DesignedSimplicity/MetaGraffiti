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
				Resize(magick, size, size);
				using (var stream = new MemoryStream())
				{
					magick.Write(stream);
					stream.Seek(0, SeekOrigin.Begin);
					return stream.ToArray();
				}
			}
		}


		/// <summary>
		/// Make sure it fills both width and height
		/// </summary>
		/// <param name="magick"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		private void ResizeFill(MagickImage magick, int width, int height)
		{
			int w = width;
			int h = Convert.ToInt32(Math.Ceiling(100M * width * magick.Height / magick.Width / 100M));
			if (h < height)
			{
				h = height;
				w = Convert.ToInt32(Math.Ceiling(100M * height * magick.Width / magick.Height / 100M));
			}
			magick.Scale(w, h);
		}

		/// <summary>
		/// Make sure it fits in both width and height
		/// </summary>
		/// <param name="magick"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		private void ResizeFit(MagickImage magick, int width = 0, int height = 0)
		{
			if (width < 1) width = magick.Width;
			if (height < 1) height = magick.Height;
			int w = width;
			int h = Convert.ToInt32(Math.Ceiling(100M * height * width / magick.Width / 100M));
			if (h > height)
			{
				h = height;
				w = Convert.ToInt32(Math.Ceiling(100M * height * width / magick.Height / 100M));
			}

			magick.Scale(w, h);
		}

		/// <summary>
		/// Make sure it fits in both width and height
		/// </summary>
		/// <param name="magick"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fill"></param>
		private void Resize(MagickImage magick, int width = 0, int height = 0, bool fill = false)
		{
			if (width < 1) width = magick.Width;
			if (height < 1) height = magick.Height;

			int w = width;
			int h = Convert.ToInt32(Math.Ceiling(100M * height * width / magick.Width / 100M));
			if (fill && h < height)
			{
				h = height;
				w = Convert.ToInt32(Math.Ceiling(100M * height * magick.Width / magick.Height / 100M));
			}
			else
			{
				h = height;
				w = Convert.ToInt32(Math.Ceiling(100M * height * width / magick.Height / 100M));
			}

			magick.Scale(width, height);
		}
	}
}
