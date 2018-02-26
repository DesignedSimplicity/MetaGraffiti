using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Gpx.Data
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
	public class GpxBase
	{
		public string Name;
		public string Comment;
		public string Description;

		public string Source;
		public string Url;
		public string UrlText;
	}
}
