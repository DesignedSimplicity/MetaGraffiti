using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using MetaGraffiti.Base.Modules.Geo;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
	public class GpxFileWriter
	{
		private GpxSchemaVersion _version;
		private string _ns;
		private XmlDocument _xml;
		private const string _gpx = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><gpx version=""1.0"" creator=""GeoGraffiti - http://designed.com/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.topografix.com/GPX/1/0"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/0 http://www.topografix.com/GPX/1/0/gpx.xsd""></gpx>";

		public GpxFileWriter(string uri)
		{
		}

		public GpxFileWriter(Stream uri)
		{
		}

		public decimal Version { get; private set; }
		public string Creator { get; private set; }

		public void WriteHeader(string name, string description = null, DateTime? timestamp = null)
		{
			/*
			var header = new GpxFileHeader()
			{
				Name = name,
				Description = description,
				Timestamp = timestamp
			};
			WriteHeader(header);
			*/
		}

		public void WriteTrack(string name, string description = null)
		{
		}

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



		private void WriteXml(string uri)
		{
			var data = new GpxFileData();
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(_gpx);

			XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);
			ns.AddNamespace("gpx", _ns);

			//xml.DocumentElement.Attributes["creator"].InnerText = Creator;
			//xml.DocumentElement.Attributes["version"].InnerText = Version.ToString();
			if (data.Timestamp.HasValue)
			{
				var timestamp = xml.CreateElement("time", _ns);
				xml.DocumentElement.AppendChild(timestamp);
				timestamp.InnerText = data.Timestamp.Value.ToUniversalTime().ToString("s") + "Z";
			}
			if (!String.IsNullOrWhiteSpace(data.Name))
			{
				var name = xml.CreateElement("name", _ns);
				xml.DocumentElement.AppendChild(name);
				name.InnerText = data.Name;
			}
			if (!String.IsNullOrWhiteSpace(data.Description))
			{
				var desc = xml.CreateElement("desc", _ns);
				xml.DocumentElement.AppendChild(desc);
				desc.InnerText = data.Description;
			}

			foreach (var t in data.Tracks)
			{
				// create track
				var track = xml.CreateElement("trk", _ns);
				xml.DocumentElement.AppendChild(track);

				if (!String.IsNullOrWhiteSpace(t.Name))
				{
					var name = xml.CreateElement("name", _ns);
					track.AppendChild(name);
					name.InnerText = t.Name;
				}
				if (!String.IsNullOrWhiteSpace(t.Description))
				{
					var desc = xml.CreateElement("desc", _ns);
					track.AppendChild(desc);
					desc.InnerText = t.Description;
				}

				XmlElement tracksegment = null;
				var segment = -1;
				foreach (var p in t.Points.OrderBy(x => x.Segment).ThenBy(x => x.Timestamp))
				{
					if (segment != p.Segment)
					{
						// create new segments as needed
						tracksegment = xml.CreateElement("trkseg", _ns);
						track.AppendChild(tracksegment);
						segment = p.Segment;
					}

					// add points to segments
					var point = xml.CreateElement("trkpt", _ns);
					tracksegment.AppendChild(point);
					WritePoint(point, p);
				}
			}

			//todo- deal with routes
			xml.Save(uri);
		}

		private void WritePoint(XmlNode xml, GpxPointData p)
		{
			var lat = xml.OwnerDocument.CreateAttribute("lat");
			xml.Attributes.Append(lat);
			lat.Value = p.Latitude.ToString();

			var lon = xml.OwnerDocument.CreateAttribute("lon");
			xml.Attributes.Append(lon);
			lon.Value = p.Longitude.ToString();

			if (p.Timestamp.HasValue)
			{
				var time = xml.OwnerDocument.CreateElement("time", _ns);
				xml.AppendChild(time);
				time.InnerText = p.Timestamp.Value.ToUniversalTime().ToString("s") + "Z";
			}

			if (p.Elevation.HasValue)
			{
				var ele = xml.OwnerDocument.CreateElement("ele", _ns);
				xml.AppendChild(ele);
				ele.InnerText = p.Elevation.Value.ToString();
			}

			if (p.Course.HasValue)
			{
				var course = xml.OwnerDocument.CreateElement("course", _ns);
				xml.AppendChild(course);
				course.InnerText = p.Course.Value.ToString();
			}

			if (p.Speed.HasValue)
			{
				var speed = xml.OwnerDocument.CreateElement("speed", _ns);
				xml.AppendChild(speed);
				speed.InnerText = p.Speed.Value.ToString();
			}

			if (!String.IsNullOrWhiteSpace(p.Source))
			{
				var src = xml.OwnerDocument.CreateElement("src", _ns);
				xml.AppendChild(src);
				src.InnerText = p.Source;
			}

			if (p.Sats.HasValue)
			{
				var sat = xml.OwnerDocument.CreateElement("sat", _ns);
				xml.AppendChild(sat);
				sat.InnerText = p.Sats.Value.ToString();
			}

			if (p.HDOP.HasValue)
			{
				var hdop = xml.OwnerDocument.CreateElement("hdop", _ns);
				xml.AppendChild(hdop);
				hdop.InnerText = p.HDOP.Value.ToString();
			}

			if (p.VDOP.HasValue)
			{
				var vdop = xml.OwnerDocument.CreateElement("vdop", _ns);
				xml.AppendChild(vdop);
				vdop.InnerText = p.VDOP.Value.ToString();
			}

			if (p.PDOP.HasValue)
			{
				var pdop = xml.OwnerDocument.CreateElement("pdop", _ns);
				xml.AppendChild(pdop);
				pdop.InnerText = p.PDOP.Value.ToString();
			}
		}
	}
}
