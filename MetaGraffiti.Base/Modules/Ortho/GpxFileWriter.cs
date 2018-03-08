using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public class GpxFileWriter
	{
		private const string _gpxTemplateV1 = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><gpx version=""1.0"" creator=""MetaGraffiti"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.topografix.com/GPX/1/0"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/0 http://www.topografix.com/GPX/1/0/gpx.xsd""></gpx>";

		private XmlDocument _xml = null;

		public GpxSchemaVersion Version { get; private set; } = GpxSchemaVersion.Version1;
		public string Creator { get; private set; } = "MetaGraffiti - https://github.com/DesignedSimplicity/MetaGraffiti";
		public string Namespace { get { return Version == GpxSchemaVersion.Version1 ? Gpx.XmlNamespaceV1 : Gpx.XmlNamespaceV1_1; } }

		public void WriteHeader(string name, string description = null, DateTime? timestamp = null)
		{
			_xml = new XmlDocument();
			_xml.LoadXml(_gpxTemplateV1);

			XmlNamespaceManager ns = new XmlNamespaceManager(_xml.NameTable);
			ns.AddNamespace("gpx", Namespace);

			_xml.DocumentElement.Attributes["creator"].InnerText = Creator;
			_xml.DocumentElement.Attributes["version"].InnerText = Version == GpxSchemaVersion.Version1 ? "1.0" : "1.1";
			if (timestamp.HasValue)
			{
				var timestampNode = _xml.CreateElement("time", Namespace);
				_xml.DocumentElement.AppendChild(timestampNode);
				timestampNode.InnerText = timestamp.Value.ToString("s") + "Z";
			}
			if (!String.IsNullOrWhiteSpace(name))
			{
				var nameNode = _xml.CreateElement("name", Namespace);
				_xml.DocumentElement.AppendChild(nameNode);
				nameNode.InnerText = name;
			}
			if (!String.IsNullOrWhiteSpace(description))
			{
				var descriptionNode = _xml.CreateElement("desc", Namespace);
				_xml.DocumentElement.AppendChild(descriptionNode);
				descriptionNode.InnerText = description;
			}
		}

		public void WriteTrack(string name, string description, IEnumerable<GpxPointData> points)
		{
			// create header if needed
			if (_xml == null) WriteHeader(name, description, points.First().Timestamp.Value);
			
			// create track
			var track = _xml.CreateElement("trk", Namespace);
			_xml.DocumentElement.AppendChild(track);

				if (!String.IsNullOrWhiteSpace(name))
				{
					var nameNode = _xml.CreateElement("name", Namespace);
					track.AppendChild(nameNode);
					nameNode.InnerText = name;
				}
				if (!String.IsNullOrWhiteSpace(description))
				{
					var descriptionNode = _xml.CreateElement("desc", Namespace);
					track.AppendChild(descriptionNode);
					descriptionNode.InnerText = description;
				}

				XmlElement tracksegmentNode = null;
				var segment = -1;
				foreach (var p in points.OrderBy(x => x.Segment).ThenBy(x => x.Timestamp))
				{
					if (segment != p.Segment)
					{
						// create new segments as needed
						tracksegmentNode = _xml.CreateElement("trkseg", Namespace);
						track.AppendChild(tracksegmentNode);
						segment = p.Segment;
					}

					// add points to segments
					var pointNode = _xml.CreateElement("trkpt", Namespace);
					tracksegmentNode.AppendChild(pointNode);
					SetPoint(pointNode, p);
				}
		}

		public string GetXml()
		{
			return _xml.OuterXml;
		}



		/*
		public void WriteTrack(GpxTrackData track)
		{
		}

		public void WriteRoute(GpxRouteData route)
		{
		}

		public void WritePoint(GpxPointData point)
		{
		}

		public void WritePoint(IGeoLocation point)
		{
		}

		public void WritePoint(double latitude, double longitude, double? elevation, DateTime? timestamp)
		{
		}

		public void WritePoints(IEnumerable<GpxPointData> points)
		{
		}

		public void WritePoints(IEnumerable<IGeoLocation> points)
		{
		}

		public void WriteWaypoint(GpxPointData waypoint)
		{
		}

		public void WriteWaypoints(IEnumerable<GpxPointData> waypoints)
		{
		}
		*/


		private void SetPoint(XmlNode node, GpxPointData p)
		{
			var lat = node.OwnerDocument.CreateAttribute("lat");
			node.Attributes.Append(lat);
			lat.Value = p.Latitude.ToString();

			var lon = node.OwnerDocument.CreateAttribute("lon");
			node.Attributes.Append(lon);
			lon.Value = p.Longitude.ToString();

			if (p.Timestamp.HasValue)
			{
				var time = node.OwnerDocument.CreateElement("time", Namespace);
				node.AppendChild(time);
				time.InnerText = p.Timestamp.Value.ToUniversalTime().ToString("s") + "Z";
			}

			if (p.Elevation.HasValue)
			{
				var ele = node.OwnerDocument.CreateElement("ele", Namespace);
				node.AppendChild(ele);
				ele.InnerText = p.Elevation.Value.ToString();
			}

			if (p.Course.HasValue)
			{
				var course = node.OwnerDocument.CreateElement("course", Namespace);
				node.AppendChild(course);
				course.InnerText = p.Course.Value.ToString();
			}

			if (p.Speed.HasValue)
			{
				var speed = node.OwnerDocument.CreateElement("speed", Namespace);
				node.AppendChild(speed);
				speed.InnerText = p.Speed.Value.ToString();
			}

			if (!String.IsNullOrWhiteSpace(p.Source))
			{
				var src = node.OwnerDocument.CreateElement("src", Namespace);
				node.AppendChild(src);
				src.InnerText = p.Source;
			}

			if (p.Sats.HasValue)
			{
				var sat = node.OwnerDocument.CreateElement("sat", Namespace);
				node.AppendChild(sat);
				sat.InnerText = p.Sats.Value.ToString();
			}

			if (p.HDOP.HasValue)
			{
				var hdop = node.OwnerDocument.CreateElement("hdop", Namespace);
				node.AppendChild(hdop);
				hdop.InnerText = p.HDOP.Value.ToString();
			}

			if (p.VDOP.HasValue)
			{
				var vdop = node.OwnerDocument.CreateElement("vdop", Namespace);
				node.AppendChild(vdop);
				vdop.InnerText = p.VDOP.Value.ToString();
			}

			if (p.PDOP.HasValue)
			{
				var pdop = node.OwnerDocument.CreateElement("pdop", Namespace);
				node.AppendChild(pdop);
				pdop.InnerText = p.PDOP.Value.ToString();
			}
		}		
	}
}
