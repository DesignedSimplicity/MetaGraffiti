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
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTaken.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void XmpDataReader_ReadXmpDataFromXmp()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.xmp");

			var reader = new XmpDataReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTaken.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void ImgFileReader_ReadMetaDataFromJpg()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.jpg");

			var reader = new ImgFileReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTaken.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void ImgFileReader_ReadMetaDataFromOrf()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.orf");

			var reader = new ImgFileReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTaken.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		[TestMethod]
		public void ImgFileReader_ReadMetaDataFromXmp()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.xmp");

			var reader = new ImgFileReader(uri);
			var data = reader.ReadFile();

			Assert.IsNotNull(data);
			Assert.AreEqual(4608, data.PixelsX);
			Assert.AreEqual(3456, data.PixelsY);
			Assert.AreEqual("2019-01-05 11:13:32", data.DateTaken.Value.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		/*
		[TestMethod]
		public void ImgFileReader_ReadXmpFromOrf()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.orf");

			using (var magik = new MagickImage(uri))
			{
				var xmp = magik.GetXmpProfile();

				using (var reader = xmp.CreateReader())
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(reader);

					var lens = doc.SelectSingleNode("//*:xmpmeta/*:Description/*:Lens");

					//var txt = doc.OuterXml;
					Assert.IsNotNull(lens.InnerText);
				}

				//Assert.AreEqual("Hiking", d.Activities.First().TrainingName);
			}
		}

		[TestMethod]
		public void ImgFileReader_ReadXmpFromXmp()
		{
			var uri = TestsHelper.GetTestDataFileUri(@"Img\Example.xmp");

			using (var magik = new MagickImage(uri))
			{
				var xmp = magik.GetXmpProfile();

				using (var reader = xmp.CreateReader())
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(reader);

					var txt = doc.OuterXml;
					Assert.IsNotNull(txt);
				}

				//Assert.AreEqual("Hiking", d.Activities.First().TrainingName);
			}
		}
		*/
	}
}
