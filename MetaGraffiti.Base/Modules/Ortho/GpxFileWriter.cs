using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Ortho
{
	public class GpxFileWriter
	{
		// ==================================================
		// Internal

		private const string _gpxTemplateV1 = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><gpx version=""1.0"" creator=""MetaGraffiti"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.topografix.com/GPX/1/0"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/0 http://www.topografix.com/GPX/1/0/gpx.xsd""></gpx>";
		private const string _gpxTemplateV1_1 = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><gpx version=""1.1"" creator=""MetaGraffiti"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.topografix.com/GPX/1/1"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd""></gpx>";

		private XmlDocument _xml = null;
		private XmlNode _root = null;


		// ==================================================
		// Properties

		public GpxSchemaVersion Version { get; private set; } = GpxSchemaVersion.Version1;
		public string Creator { get; private set; } = "MetaGraffiti - https://github.com/DesignedSimplicity/MetaGraffiti";
		public string Namespace { get { return Version == GpxSchemaVersion.Version1 ? OrthoConstants.GpxNamespaceV1 : OrthoConstants.GpxNamespaceV1_1; } }


		// ==================================================
		// Methods

		public void SetVersion(GpxSchemaVersion version)
		{
			if (_xml != null)
				throw new Exception("Header already written.");
			else
				Version = version;
		}

		public void WriteHeader(IGpxFileHeader header)
		{
			_xml = new XmlDocument();
			_xml.LoadXml(Version == GpxSchemaVersion.Version1 ? _gpxTemplateV1 : _gpxTemplateV1_1);

			XmlNamespaceManager ns = new XmlNamespaceManager(_xml.NameTable);
			ns.AddNamespace("gpx", Namespace);

			_xml.DocumentElement.Attributes["creator"].InnerText = Creator;
			_xml.DocumentElement.Attributes["version"].InnerText = Version == GpxSchemaVersion.Version1 ? "1.0" : "1.1";

			_root = _xml.DocumentElement;
			if (Version == GpxSchemaVersion.Version1_1)
			{
				_root = _xml.CreateElement("metadata", Namespace);
				_xml.DocumentElement.AppendChild(_root);
			}
			/*
			if (header.Timestamp.HasValue)
			{
				var timestampNode = _xml.CreateElement("time", Namespace);
				_root.AppendChild(timestampNode);
				timestampNode.InnerText = header.Timestamp.Value.ToString("s") + "Z";
			}
			*/
			if (!String.IsNullOrWhiteSpace(header.Name))
			{
				var nameNode = _xml.CreateElement("name", Namespace);
				_root.AppendChild(nameNode);
				nameNode.InnerText = header.Name;
			}
			if (!String.IsNullOrWhiteSpace(header.Description))
			{
				var descriptionNode = _xml.CreateElement("desc", Namespace);
				_root.AppendChild(descriptionNode);
				descriptionNode.InnerText = header.Description;
			}
			if (!String.IsNullOrWhiteSpace(header.Keywords))
			{
				var keywordsNode = _xml.CreateElement("keywords", Namespace);
				_root.AppendChild(keywordsNode);
				keywordsNode.InnerText = header.Keywords;
			}
			if (!String.IsNullOrWhiteSpace(header.UrlLink))
			{
				var urlNode = _xml.CreateElement("url", Namespace);
				_root.AppendChild(urlNode);
				urlNode.InnerText = header.UrlLink;
			}
			if (!String.IsNullOrWhiteSpace(header.UrlText))
			{
				var urlNameNode = _xml.CreateElement("urlname", Namespace);
				_root.AppendChild(urlNameNode);
				urlNameNode.InnerText = header.UrlText;
			}
		}

		public void WriteMetadata(GpxExtensionData data)
		{
			var extensions = _xml.CreateElement("extensions");
			_root.AppendChild(extensions);

			var timezoneNode = _xml.CreateElement("timezone");
			extensions.AppendChild(timezoneNode);
			timezoneNode.InnerText = data.Timezone;

			var countryNode = _xml.CreateElement("country");
			extensions.AppendChild(countryNode);
			countryNode.InnerText = data.Country;

			if (!String.IsNullOrWhiteSpace(data.Region))
			{
				var regionNode = _xml.CreateElement("region");
				extensions.AppendChild(regionNode);
				regionNode.InnerText = data.Region;
			}

			if (!String.IsNullOrWhiteSpace(data.Location))
			{
				var locationNode = _xml.CreateElement("location");
				extensions.AppendChild(locationNode);
				locationNode.InnerText = data.Location;
			}
		}

		public void WriteMetadata(string timezone, string country, string region = "")
		{
			WriteMetadata(new GpxExtensionData()
			{
				Timezone = timezone,
				Country = country,
				Region = region
			});
		}

		public void WriteHeader(string name, string description = null, DateTime? timestamp = null)
		{
			WriteHeader(new GpxFileData()
			{
				Name = name,
				Description = description,
				Timestamp = timestamp
			});
		}

		public void WriteTrack(IGpxTrack track)
		{
			// create header if needed
			if (_xml == null) WriteHeader(track.Name, track.Description, track.Points.First().Timestamp.Value);

			// create track
			var trk = _xml.CreateElement("trk", Namespace);
			_xml.DocumentElement.AppendChild(trk);

			if (!String.IsNullOrWhiteSpace(track.Name))
			{
				var nameNode = _xml.CreateElement("name", Namespace);
				trk.AppendChild(nameNode);
				nameNode.InnerText = track.Name;
			}
			if (!String.IsNullOrWhiteSpace(track.Description))
			{
				var descriptionNode = _xml.CreateElement("desc", Namespace);
				trk.AppendChild(descriptionNode);
				descriptionNode.InnerText = track.Description;
			}
			if (!String.IsNullOrWhiteSpace(track.Source))
			{
				var sourceNode = _xml.CreateElement("src", Namespace);
				trk.AppendChild(sourceNode);
				sourceNode.InnerText = track.Source;
			}

			XmlElement tracksegmentNode = null;
			var segment = -1;
			foreach (var p in track.Points.OrderBy(x => x.Segment).ThenBy(x => x.Timestamp))
			{
				if (segment != p.Segment)
				{
					// create new segments as needed
					tracksegmentNode = _xml.CreateElement("trkseg", Namespace);
					trk.AppendChild(tracksegmentNode);
					segment = p.Segment;
				}

				// add points to segments
				var pointNode = _xml.CreateElement("trkpt", Namespace);
				tracksegmentNode.AppendChild(pointNode);
				SetPoint(pointNode, p);
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
			return XElement.Parse(_xml.OuterXml).ToString();
		}


		// ==================================================
		// Internal

		private void SetPoint(XmlNode node, IGpxPoint p)
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