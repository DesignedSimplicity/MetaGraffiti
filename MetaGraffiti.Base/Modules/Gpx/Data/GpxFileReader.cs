﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using MetaGraffiti.Base.Common;

namespace MetaGraffiti.Base.Modules.Gpx.Data
{
	public class GpxFileReader
	{
		private GpxSchemaVersion _version;
		private XmlNamespaceManager _ns;
		private XmlDocument _xml;

		// ==================================================
		// Constructors
		public GpxFileReader(string uri)
		{
			_xml = new XmlDocument();
			_xml.Load(uri);
			InitXml();
		}

		public GpxFileReader(Stream stream)
		{
			_xml = new XmlDocument();
			_xml.Load(stream);
			InitXml();			
		}

		// ==================================================
		// Properties
		public decimal Version { get; private set; }
		public string Creator { get; private set; }

		/// <summary>
		/// Reads all file data into object
		/// </summary>
		public GpxFileData ReadFile()
		{
			var file = new GpxFileData();
			file.Header = ReadHeader();
			file.Tracks = ReadTracks();
			file.Routes = ReadRoutes();
			file.Waypoints = ReadWaypoints();
			return file;
		}

		// ==================================================
		// Methods

		/// <summary>
		/// Reads the file header data
		/// </summary>
		public GpxFileHeader ReadHeader()
		{
			var header = new GpxFileHeader();

			// built in attributes
			Creator = SafeConvert.ToString(_xml.DocumentElement.Attributes["creator"].InnerText);
			Version = SafeConvert.ToDecimal(_xml.DocumentElement.Attributes["version"].InnerText);

			// common top elements
			header.Name = ReadString(_xml.DocumentElement, "name");
			header.Description = ReadString(_xml.DocumentElement, "desc");
			header.Timestamp = ReadDateTime(_xml.DocumentElement, "time");

			// TODO: support other optional elements

			return header;
		}

		/// <summary>
		/// Reads all track data in file
		/// </summary>
		public List<GpxTrackData> ReadTracks()
		{
			var tracks = new List<GpxTrackData>();
			foreach (XmlNode xt in _xml.DocumentElement.SelectNodes("gpx:trk", _ns))
			{
				var t = new GpxTrackData();
				PopulateMetaData(xt, t);
				tracks.Add(t);

				int segment = 0;
				t.Points = new List<GpxPointData>();
				var xtss = xt.SelectNodes("gpx:trkseg", _ns);
				if ((xtss?.Count ?? 0) > 0)
				{
					foreach (XmlNode xts in xtss)
					{
						segment++;
						foreach (XmlNode xtp in xts.SelectNodes("gpx:trkpt", _ns))
						{
							var p = ReadPoint(xtp, GpxPointTypes.TrackPoint);
							p.Segment = segment;
							t.Points.Add(p);
						}
					}
				}
				else
				{
					// no track segments, just use points directly
					foreach (XmlNode xtp in xt.SelectNodes("gpx:trkpt", _ns))
					{
						var p = ReadPoint(xtp, GpxPointTypes.TrackPoint);
						t.Points.Add(p);
					}
				}
			}
			return tracks;
		}

		/// <summary>
		/// Reads all route data in file
		/// </summary>
		public List<GpxRouteData> ReadRoutes()
		{
			var routes = new List<GpxRouteData>();
			foreach (XmlNode xr in _xml.DocumentElement.SelectNodes("gpx:rte", _ns))
			{
				var r = new GpxRouteData();
				PopulateMetaData(xr, r);
				routes.Add(r);

				r.Points = new List<GpxPointData>();
				foreach (XmlNode xtp in xr.SelectNodes("gpx:rtept ", _ns))
				{
					var p = ReadPoint(xtp, GpxPointTypes.RoutePoint);
					r.Points.Add(p);
				}
			}
			return routes;
		}

		/// <summary>
		/// Reads all waypoint data in file
		/// </summary>
		public List<GpxPointData> ReadWaypoints()
		{
			var points = new List<GpxPointData>();
			foreach (XmlNode xp in _xml.DocumentElement.SelectNodes("gpx:wpt", _ns))
			{
				var p = ReadPoint(xp, GpxPointTypes.WayPoint);
				PopulateMetaData(xp, p);
				points.Add(p);
			}
			return points;
		}


		// ==================================================
		// Internal

		private void PopulateMetaData(XmlNode node, GpxMetaData meta)
		{
			meta.Name = ReadString(node, "name");
			meta.Comment = ReadString(node, "cmt");
			meta.Description = ReadString(node, "desc");

			meta.Source = ReadString(node, "src");
			meta.Url = ReadString(node, "url");
			meta.UrlText = ReadString(node, "urlname");
		}

		private GpxPointData ReadPoint(XmlNode node, GpxPointTypes pointType)
		{
			var p = new GpxPointData();
			p.GpxPointType = pointType;

			PopulateMetaData(node, p);
			
			p.Latitude = SafeConvert.ToDouble(node.Attributes["lat"].Value);
			p.Longitude = SafeConvert.ToDouble(node.Attributes["lon"].Value);
			p.Timestamp = ReadDateTime(node, "time");
			p.Elevation = ReadDouble(node, "ele");
			p.Course = ReadDecimal(node, "course");
			p.Speed = ReadDecimal(node, "speed");
			p.Source = ReadString(node, "src");
			p.Sats = ReadInteger(node, "sat");

			p.HDOP = ReadDecimal(node, "hdop");
			p.VDOP = ReadDecimal(node, "vdop");
			p.PDOP = ReadDecimal(node, "pdop");

			return p;
		}

		private double? ReadDouble(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToDoubleNull(n.InnerText);
		}

		private decimal? ReadDecimal(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToDecimalNull(n.InnerText);
		}

		private int? ReadInteger(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToIntNull(n.InnerText);
		}

		private string ReadString(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToString(n.InnerText);
		}

		private DateTime? ReadDateTime(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return SafeConvert.ToUTCDateTimeNull(n.InnerText);
		}

		private void InitXml()
		{
			// check for file version
			_ns = new XmlNamespaceManager(_xml.NameTable);
			if (_xml.DocumentElement.FirstChild.Name == "metadata")
			{
				_version = GpxSchemaVersion.Version1_1;
				_ns.AddNamespace("gpx", Gpx.XmlNamespaceV1_1);
			}
			else
			{
				_version = GpxSchemaVersion.Version1;
				_ns.AddNamespace("gpx", Gpx.XmlNamespaceV1);
			}
		}
	}
}
