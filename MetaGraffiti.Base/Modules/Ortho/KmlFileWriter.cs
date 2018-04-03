using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;

using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Ortho
{
    public class KmlFileWriter
    {
		private const string _kmlTemplate = @"<?xml version=""1.0"" encoding=""UTF-8""?><kml xmlns=""http://www.opengis.net/kml/2.2""  xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom""></kml>";

		private XmlDocument _xml = null;

		public string Namespace { get { return "http://www.google.com/kml/ext/2.2"; } }

		public void WriteHeader(string name, string description = null)
		{
			_xml = new XmlDocument();
			_xml.LoadXml(_kmlTemplate);

			XmlNamespaceManager ns = new XmlNamespaceManager(_xml.NameTable);
			ns.AddNamespace("gx", Namespace);

			var doc = _xml.CreateElement("Document", null);
			_xml.DocumentElement.AppendChild(doc);

			if (!String.IsNullOrWhiteSpace(name))
			{
				var nameNode = _xml.CreateElement("name", null);
				doc.AppendChild(nameNode);
				nameNode.InnerText = name;
			}
			if (!String.IsNullOrWhiteSpace(description))
			{
				var descriptionNode = _xml.CreateElement("description", null);
				doc.AppendChild(descriptionNode);
				descriptionNode.InnerText = description;
			}
		}

		public void WriteTrack(GpxTrackData track)
		{
			// create header if needed
			if (_xml == null) WriteHeader(track.Name, track.Description);

			// create placemark
			var pm = _xml.CreateElement("Placemark", null);
			_xml.DocumentElement.FirstChild.AppendChild(pm);

			// add details
			if (!String.IsNullOrWhiteSpace(track.Name))
			{
				var nameNode = _xml.CreateElement("name", null);
				pm.AppendChild(nameNode);
				nameNode.InnerText = track.Name;
			}
			if (!String.IsNullOrWhiteSpace(track.Description))
			{
				var descriptionNode = _xml.CreateElement("description", null);
				pm.AppendChild(descriptionNode);
				descriptionNode.InnerText = track.Description;
			}

			// create track
			var trackNode = _xml.CreateElement("gx:Track", Namespace);
			pm.AppendChild(trackNode);

			// add points
			foreach (var p in track.PointData.OrderBy(x => x.Segment).ThenBy(x => x.Timestamp))
			{
				var whenNode = _xml.CreateElement("when", null);
				trackNode.AppendChild(whenNode);
				whenNode.InnerText = p.Timestamp.Value.ToUniversalTime().ToString("s") + "Z";

				var pointNode = _xml.CreateElement("gx:coord", Namespace);
				trackNode.AppendChild(pointNode);
				pointNode.InnerText = $"{p.Longitude} {p.Latitude} {p.Elevation}".TrimEnd();
			}
		}

		public void WriteTrack(string name, string description, IEnumerable<GpxPointData> points)
		{
			WriteTrack(new GpxTrackData()
			{
				Name = name,
				Description = description,
				PointData = points.ToList()
			});
		}

		public string GetXml()
		{
			return _xml.OuterXml;
		}
	}
}