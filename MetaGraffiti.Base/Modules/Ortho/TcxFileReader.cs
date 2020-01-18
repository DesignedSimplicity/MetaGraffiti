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
	public class TcxFileReader
	{
		// ==================================================
		// Internal
		private XmlNamespaceManager _ns;
		private XmlDocument _xml;


		// ==================================================
		// Constructors

		public TcxFileReader(string uri)
		{
			_xml = new XmlDocument();
			_xml.Load(uri);
			InitXml();
		}

		public TcxFileReader(Stream stream)
		{
			_xml = new XmlDocument();
			_xml.Load(stream);
			InitXml();
		}


		// ==================================================
		// Properties



		// ==================================================
		// Methods

		/// <summary>
		/// Reads all file data into object
		/// </summary>
		public TcxFileData ReadFile()
		{
			var data = ReadData();
			return data;
		}

		/// <summary>
		/// Reads the file header data
		/// </summary>
		public TcxFileData ReadData()
		{
			var data = new TcxFileData();

			// built in attributes

			// common top elements
			data.Name = "";

			data.Activities = ReadActivities();

			return data;
		}

		/// <summary>
		/// Reads all track data in file
		/// </summary>
		public List<TcxActivityData> ReadActivities()
		{
			var activities = new List<TcxActivityData>();
			foreach (XmlNode xa in _xml.DocumentElement.FirstChild.SelectNodes("tc:Activity", _ns))
			{
				var a = new TcxActivityData();
				a.Id = ReadString(xa, "Id");
				activities.Add(a);

				// extract training data
				var xt = xa.SelectSingleNode(@"tc:Training", _ns);
				var xtp = xt.SelectSingleNode(@"tc:Plan", _ns);
				a.TrainingName = ReadString(xtp, @"Name");

				// read laps
				a.Laps = new List<TcxLapData>();
				foreach (XmlNode xl in xa.SelectNodes("tc:Lap", _ns))
				{
					// extra lap data
					var l = new TcxLapData();
					a.Laps.Add(l);

					// read general lap info
					l.TotalTimeSeconds = ReadDecimal(xl, "TotalTimeSeconds") ?? 0;
					l.DistanceMeters = ReadDecimal(xl, "DistanceMeters") ?? 0;
					l.MaximumSpeed = ReadDecimal(xl, "MaximumSpeed") ?? 0;
					l.Calories = ReadInteger(xl, "Calories") ?? 0;
					l.Intensity = ReadString(xl, "Intensity");

					// read heart rate parameters
					var xavg = xl.SelectSingleNode("tc:AverageHeartRateBpm", _ns);
					l.AverageHeartRateBpm = ReadInteger(xavg, "Value") ?? 0;
					var xmax = xl.SelectSingleNode("tc:MaximumHeartRateBpm", _ns);
					l.MaximumHeartRateBpm = ReadInteger(xmax, "Value") ?? 0;

					l.Tracks = new List<TcxTrackData>();
					foreach (XmlNode xp in xl.SelectSingleNode("tc:Track", _ns).SelectNodes("tc:Trackpoint", _ns))
					{
						// position data
						var t = new TcxTrackData();
						l.Tracks.Add(t);

						// read expected data
						t.Timestamp = ReadDateTime(xp, "Time").Value;
						t.SensorState = ReadString(xp, "SensorState");
						t.DistanceMeters = ReadDecimal(xp, "DistanceMeters");
						
						var xhr = xp.SelectSingleNode("tc:HeartRateBpm", _ns);
						if (xhr != null)
						{
							t.HeartRateBpm = ReadInteger(xhr, "Value");
						}

						var xpos = xp.SelectSingleNode("tc:Position", _ns);
						if (xpos != null)
						{
							t.Latitude = ReadDouble(xpos, "LatitudeDegrees");
							t.Longitude = ReadDouble(xpos, "LongitudeDegrees");
						}
					}
				}
			}
			return activities;
		}

		// ==================================================
		// Internal

		private double? ReadDouble(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("tc:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToDoubleNull(n.InnerText);
		}

		private decimal? ReadDecimal(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("tc:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToDecimalNull(n.InnerText);
		}

		private int? ReadInteger(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("tc:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToIntNull(n.InnerText);
		}

		private string ReadString(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("tc:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToString(n.InnerText);
		}

		private DateTime? ReadDateTime(XmlNode node, string name)
		{
			var n = node.SelectSingleNode("tc:" + name, _ns);
			if (n == null)
				return null;
			else
				return TypeConvert.ToUTCDateTimeNull(n.InnerText);
		}

		private void InitXml()
		{
			// check for file version
			_ns = new XmlNamespaceManager(_xml.NameTable);
			_ns.AddNamespace("tc", @"http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2");
		}
	}
}
