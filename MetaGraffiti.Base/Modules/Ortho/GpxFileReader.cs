using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Ortho.Data;

namespace MetaGraffiti.Base.Modules.Ortho
{
	/// <summary>
	/// Parses a GPX file into a set of data classes
	/// </summary>
	public class GpxFileReader
	{
		// ==================================================
		// Internal

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


		// ==================================================
		// Methods

		/// <summary>
		/// Reads all file data into object
		/// </summary>
		public GpxFileData ReadFile()
		{
			var data = ReadHeader();
			data.Tracks = ReadTracks();
			data.Routes = ReadRoutes();
			data.Waypoints = ReadWaypoints();
			data.Extensions = ReadExtension();
			return data;
		}

		/// <summary>
		/// Reads the file header data
		/// </summary>
		public GpxFileData ReadHeader()
		{
			var data = new GpxFileData();

			// built in attributes
			Creator = TypeConvert.ToString(_xml.DocumentElement.Attributes["creator"].InnerText);
			Version = TypeConvert.ToDecimal(_xml.DocumentElement.Attributes["version"].InnerText);

			// common top elements
			data.Name = ReadString(GetMetaDataNode(), "name");
			data.Description = ReadString(GetMetaDataNode(), "desc");
			data.Timestamp = ReadDateTime(GetMetaDataNode(), "time");

			// optional top elements
			data.UrlLink = ReadString(GetMetaDataNode(), "url");
			data.UrlText = ReadString(GetMetaDataNode(), "urlname");
			data.Keywords = ReadString(GetMetaDataNode(), "keywords");

			// TODO: support other optional elements

			return data;
		}

		/// <summary>
		/// Extracts additional custom data if exists
		/// </summary>
		public GpxExtensionData ReadExtension()
		{
			var data = new GpxExtensionData();
			data.Timezone = ReadExtension("timezone");
			data.Country = ReadExtension("country");
			data.Region = ReadExtension("region");
			data.Location = ReadExtension("location");
			return data;
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
				t.PointData = new List<GpxPointData>();
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
							t.PointData.Add(p);
						}
					}
				}
				else
				{
					// no track segments, just use points directly
					foreach (XmlNode xtp in xt.SelectNodes("gpx:trkpt", _ns))
					{
						var p = ReadPoint(xtp, GpxPointTypes.TrackPoint);
						t.PointData.Add(p);
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

				r.PointData = new List<GpxPointData>();
				foreach (XmlNode xtp in xr.SelectNodes("gpx:rtept ", _ns))
				{
					var p = ReadPoint(xtp, GpxPointTypes.RoutePoint);
					r.PointData.Add(p);
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

		private XmlNode GetMetaDataNode()
		{
			if (_version == GpxSchemaVersion.Version1_1)
				return _xml.DocumentElement.FirstChild;
			else
				return (XmlNode)_xml.DocumentElement;
		}

		private void PopulateMetaData(XmlNode node, GpxMetaData meta)
		{
			meta.Name = ReadString(node, "name");
			meta.Comment = ReadString(node, "cmt");
			meta.Description = ReadString(node, "desc");

			meta.Source = ReadString(node, "src");
			meta.Url = ReadString(node, "url");
			meta.UrlName = ReadString(node, "urlname");
		}

		private GpxPointData ReadPoint(XmlNode node, GpxPointTypes pointType)
		{
			var p = new GpxPointData();
			p.GpxPointType = pointType;

			PopulateMetaData(node, p);
			
			p.Latitude = TypeConvert.ToDouble(node.Attributes["lat"].Value);
			p.Longitude = TypeConvert.ToDouble(node.Attributes["lon"].Value);
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

		private string ReadExtension(string name)
		{
			var ex = GetMetaDataNode().SelectSingleNode("extensions");
			if (ex == null) return null;

			var n = ex.SelectSingleNode(name);
			if (n == null)
				return null;
			else
				return TypeConvert.ToString(n.InnerText);
		}

		private double? ReadDouble(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToDoubleNull(n.InnerText);
		}

		private decimal? ReadDecimal(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToDecimalNull(n.InnerText);
		}

		private int? ReadInteger(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToIntNull(n.InnerText);
		}

		private string ReadString(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToString(n.InnerText);
		}

		private DateTime? ReadDateTime(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("gpx:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToUTCDateTimeNull(n.InnerText);
		}

		private void InitXml()
		{
			// check for file version
			_ns = new XmlNamespaceManager(_xml.NameTable);
			if (_xml.DocumentElement.FirstChild.Name == "metadata")
			{
				_version = GpxSchemaVersion.Version1_1;
				_ns.AddNamespace("gpx", OrthoConstants.GpxNamespaceV1_1);
			}
			else
			{
				_version = GpxSchemaVersion.Version1;
				_ns.AddNamespace("gpx", OrthoConstants.GpxNamespaceV1);
			}
		}
	}
}
