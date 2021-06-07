using System;
using System.IO;
using System.Linq;
using System.Xml;
using ExifLib;
using ImageMagick;
using MetaGraffiti.Base.Modules.Ortho;
using MetaGraffiti.Base.Modules.Ortho.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaGraffiti.Base.Tests.Ortho
{
	
	[TestClass]
	public class ImgFileReaderTests
	{
		[TestMethod]
		public void JpgExifReader_ReadExifDataFromJpg()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.jpg");

			var reader = new JpgExifReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(3456, data.Width);
			Assert.AreEqual(4608, data.Height);
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateCreated.ToString("yyyy-MM-dd hh:mm:ss"));
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTimeOriginal.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void XmpDataReader_ReadXmpDataFromXmp()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.xmp");

			var reader = new XmpDataReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(3456, data.Width);
			Assert.AreEqual(4608, data.Height);
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateCreated.ToString("yyyy-MM-dd hh:mm:ss"));
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTimeOriginal.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		/* TODO:FOTO:FIX
		[TestMethod]
		public void ImgFileReader_ReadMetaDataFromJpg()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.jpg");

			var reader = new ImgFileReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(3456, data.Width);
			Assert.AreEqual(4608, data.Height);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateCreated.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void ImgFileReader_ReadMetaDataFromOrf()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.orf");

			var reader = new ImgFileReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(3456, data.Width);
			Assert.AreEqual(4608, data.Height);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateCreated.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void ImgFileReader_ReadMetaDataFromXmp()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.xmp");

			var reader = new ImgFileReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(3456, data.Width);
			Assert.AreEqual(4608, data.Height);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateCreated.ToString("yyyy-MM-dd hh:mm:ss"));
		}
		*/
	}
}
