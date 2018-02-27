using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo;
using MetaGraffiti.Base.Modules.Gpx;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
    public class GpxXmlData
    {
		private const string _ns = "http://www.topografix.com/GPX/1/0";
		private const string _gpx = @"<?xml version=""1.0"" encoding=""UTF-8"" ?><gpx version=""1.0"" creator=""GeoGraffiti - http://designed.com/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.topografix.com/GPX/1/0"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/0 http://www.topografix.com/GPX/1/0/gpx.xsd""></gpx>";

		// required
		public decimal Version { get; set; }
		public string Creator { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime? Timestamp { get; set; }

		public List<GpxRouteData> Routes { get; set; }
		public List<GpxTrackData> Tracks { get; set; }

		// optional
		/*
		public string Author;
		public string Email;
		public string Url;
		public string UrlName;
		public string Keywords;
		public DateTime? Created;
		*/

		//==================================================
		//Instance Methods
		//==================================================
		public void ReadXml(string uri)
		{
			try
			{
				XmlDocument xml = new XmlDocument();
				xml.Load(uri);

				XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);
				ns.AddNamespace("gpx", _ns);

				// built in attributes
				Creator = SafeConvert.ToString(xml.DocumentElement.Attributes["creator"].InnerText);
				Version = SafeConvert.ToDecimal(xml.DocumentElement.Attributes["version"].InnerText);

				// common top elements
				Name = ReadString(xml.DocumentElement, ns, "name");
				Description = ReadString(xml.DocumentElement, ns, "desc");
				Timestamp = ReadDateTime(xml.DocumentElement, ns, "time");

				// read all tracks, segments and points
				Tracks = new List<GpxTrackData>();
				foreach (XmlNode xt in xml.DocumentElement.SelectNodes("gpx:trk", ns))
				{
					int segment = 0;
					var t = new GpxTrackData();
					Tracks.Add(t);

					t.Name = ReadString(xt, ns, "name");
					t.Description = ReadString(xt, ns, "desc");

					t.Points = new List<GpxPointData>();
					foreach (XmlNode xts in xt.SelectNodes("gpx:trkseg", ns))
					{
						segment++;
						foreach (XmlNode xtp in xts.SelectNodes("gpx:trkpt", ns))
						{
							var p = ReadPoint(xtp, ns);
							p.Segment = segment;
							t.Points.Add(p);
						}
					}
				}

				// read all routes and points
				Routes = new List<GpxRouteData>();
				foreach (XmlNode xr in xml.DocumentElement.SelectNodes("gpx:rte", ns))
				{
					var r = new GpxRouteData();
					r.Points = new List<GpxPointData>();
					foreach (XmlNode xrp in xr.SelectNodes("gpx:rtept", ns))
					{
						var p = ReadPoint(xrp, ns);
						r.Points.Add(p);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Error parsing XML in {0}", uri), ex);
			}
		}

		public void WriteXml(string uri)
		{
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(_gpx);

			XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);
			ns.AddNamespace("gpx", _ns);

			//xml.DocumentElement.Attributes["creator"].InnerText = Creator;
			//xml.DocumentElement.Attributes["version"].InnerText = Version.ToString();
			if (Timestamp.HasValue)
			{
				var timestamp = xml.CreateElement("time", _ns);
				xml.DocumentElement.AppendChild(timestamp);
				timestamp.InnerText = Timestamp.Value.ToUniversalTime().ToString("s") + "Z";
			}
			if (!String.IsNullOrWhiteSpace(Name))
			{
				var name = xml.CreateElement("name", _ns);
				xml.DocumentElement.AppendChild(name);
				name.InnerText = Name;
			}
			if (!String.IsNullOrWhiteSpace(Description))
			{
				var desc = xml.CreateElement("desc", _ns);
				xml.DocumentElement.AppendChild(desc);
				desc.InnerText = Description;
			}

			foreach (var t in Tracks)
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

			//TODO: save routes as required
			if (Routes.Count > 0) throw new NotImplementedException("Savings routes is not supported");

			xml.Save(uri);
		}


		//==================================================
		#region Private Helpers

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

		private GpxPointData ReadPoint(XmlNode xml, XmlNamespaceManager ns)
		{
			var p = new GpxPointData();
			p.Latitude = SafeConvert.ToDouble(xml.Attributes["lat"].Value);
			p.Longitude = SafeConvert.ToDouble(xml.Attributes["lon"].Value);
			p.Timestamp = ReadDateTime(xml, ns, "time");
			p.Elevation = ReadDouble(xml, ns, "ele");
			p.Course = ReadDecimal(xml, ns, "course");
			p.Speed = ReadDecimal(xml, ns, "speed");
			p.Source = ReadString(xml, ns, "src");
			p.Sats = ReadInteger(xml, ns, "sat");

			p.HDOP = ReadDecimal(xml, ns, "hdop");
			p.VDOP = ReadDecimal(xml, ns, "vdop");
			p.PDOP = ReadDecimal(xml, ns, "pdop");

			return p;
		}

		private double? ReadDouble(XmlNode xml, XmlNamespaceManager ns, string node)
		{
			var n = xml.SelectSingleNode("gpx:" + node, ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToDoubleNull(n.InnerText);
		}

		private decimal? ReadDecimal(XmlNode xml, XmlNamespaceManager ns, string node)
		{
			var n = xml.SelectSingleNode("gpx:" + node, ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToDecimalNull(n.InnerText);
		}

		private int? ReadInteger(XmlNode xml, XmlNamespaceManager ns, string node)
		{
			var n = xml.SelectSingleNode("gpx:" + node, ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToIntNull(n.InnerText);
		}

		private string ReadString(XmlNode xml, XmlNamespaceManager ns, string node)
		{
			var n = xml.SelectSingleNode("gpx:" + node, ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToString(n.InnerText);
		}

		private DateTime? ReadDateTime(XmlNode xml, XmlNamespaceManager ns, string node)
		{
			var n = xml.SelectSingleNode("gpx:" + node, ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToUTCDateTimeNull(n.InnerText);
		}

		#endregion
		//==================================================
	}
}
