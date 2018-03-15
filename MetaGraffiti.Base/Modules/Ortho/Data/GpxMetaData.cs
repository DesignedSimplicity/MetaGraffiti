using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	/*
	<name>GPS track name</name>
	<cmt>GPS track comment</cmt>
	<desc>Description of the track</desc>
	<src>Source of the track data</src>
	<url>URL associated with the track</url>
	<urlname>Text to display on the url hyperlink</urlname>
	<number>GPS track number</number>
	*/

	public class GpxMetaData
	{
		public string Name { get; set; }
		public string Comment { get; set; }
		public string Description { get; set; }

		public string Source { get; set; }
		public string Url { get; set; }
		public string UrlName { get; set; }

		public GpxMetaData CopyMetaData()
		{
			var data = new GpxMetaData();
			CopyMetaDataTo(data);
			return data;
		}

		public void CopyMetaDataTo(GpxMetaData dest)
		{
			dest.Name = Name;
			dest.Comment = Comment;
			dest.Description = Description;
			dest.Source = Source;
			dest.Url = Url;
			dest.UrlName = UrlName;
		}
	}
}