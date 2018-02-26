using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo.Data
{
	public class GeoTimezoneData
	{
		// ==================================================
		// Constructors
		private GeoTimezoneData(int geotzid, string key, string tzid, string wintz)
		{
			TimezoneType = GeoTimezoneTypes.Windows;
			BaseTimezone = this;
			GeoTZID = geotzid;

			Key = key;
			TZID = tzid;
			Name = WinTZ = wintz;

			TimeZone = TimeZoneInfo.FindSystemTimeZoneById(wintz);
			DisplayAs = TimeZone.DisplayName;
			OffsetUTC = OffsetDST = TimeZone.BaseUtcOffset;
			if (TimeZone.SupportsDaylightSavingTime) OffsetDST.Add(new TimeSpan(1, 0, 0));
		}

		private GeoTimezoneData(int geotzid, string tzid, string wintz)
		{
			TimezoneType = GeoTimezoneTypes.Olson;
			BaseTimezone = GeoTimezoneData.BySystem(wintz);
			GeoTZID = geotzid;

			Key = Name = TZID = tzid;
			WinTZ = wintz;

			TimeZone = TimeZoneInfo.FindSystemTimeZoneById(wintz);
			DisplayAs = tzid.Replace("_", " ");
			OffsetUTC = OffsetDST = TimeZone.BaseUtcOffset;
			if (TimeZone.SupportsDaylightSavingTime) OffsetDST.Add(new TimeSpan(1, 0, 0));
		}

		// ==================================================
		// Properties
		public GeoTimezoneTypes TimezoneType { get; private set; } // windows vs olson timezone
		public GeoTimezoneData BaseTimezone { get; private set; } // points to windows timezone
		public int GeoTZID { get; private set; } // generated ID for core timezones +/- UTC DST #

		public string Key { get; private set; } // Geo level unique id
		public string Name { get; private set; } // default display name
		public string TZID { get; private set; } // Olson TZID
		public string WinTZ { get; private set; } // Windows system TimeZoneInfo.Id
		public string DisplayAs { get; private set; } // formatted display friendly name

		public TimeSpan OffsetUTC { get; private set; }
		public TimeSpan OffsetDST { get; private set; }
		public TimeZoneInfo TimeZone { get; private set; }

		public bool HasDST { get { return OffsetUTC.TotalMinutes != OffsetDST.TotalSeconds; } }
		public bool IsDefault { get { return TimezoneType == GeoTimezoneTypes.Windows && (GeoTZID == 0 || GeoTZID == 1 || GeoTZID.ToString().EndsWith("0")); } }

		// ==================================================
		// Methods
		public DateTime FromUTC(DateTime utc)
		{
			return TimeZoneInfo.ConvertTime(utc, TimeZone);
		}

		public DateTime ToUTC(DateTime utc)
		{
			return TimeZoneInfo.ConvertTime(utc, TimeZoneInfo.FindSystemTimeZoneById("UTC"));
		}

		// ==================================================
		// Static Factory
		public static GeoTimezoneData LocalTimezone { get { return BySystem(TimeZoneInfo.Local.Id); } }

		public static List<GeoTimezoneData> Default
		{
			get { if (_windows == null) Init(); return _windows.Where(x => x.IsDefault).ToList(); }
		}

		public static List<GeoTimezoneData> Windows
		{
			get { if (_windows == null) Init(); return _windows; }
		}

		public static List<GeoTimezoneData> Olson
		{
			get { if (_olson == null) Init(); return _olson; }
		}

		public static GeoTimezoneData BySystem(string winTZ)
		{
			return Windows.FirstOrDefault(x => String.Compare(x.WinTZ, winTZ, true) == 0);
		}

		public static GeoTimezoneData ByTZID(string tzid)
		{
			return Olson.FirstOrDefault(x => String.Compare(x.TZID, tzid, true) == 0);
		}

		public static GeoTimezoneData ByKey(string key)
		{
			var t = Windows.FirstOrDefault(x => String.Compare(x.Key, key, true) == 0);
			if (t == null) t = Olson.FirstOrDefault(x => String.Compare(x.Key, key, true) == 0);
			return t;
		}

		// ==================================================
		// Static Globals
		public static List<GeoTimezoneData> All { get { if (_all == null) { Init(); } return _all.Values.ToList(); } }

		private static Dictionary<int, GeoTimezoneData> _all;
		private static List<GeoTimezoneData> _windows;
		private static List<GeoTimezoneData> _olson;

		private static void Init()
		{
			// init global cache
			_all = new Dictionary<int, GeoTimezoneData>();

			// --------------------------------------------------
			#region Windows System Time Zones
			_windows = new List<GeoTimezoneData>
			{
				new GeoTimezoneData(1120000000, "Dateline", "Etc/GMT+12", "Dateline Standard Time"),
				new GeoTimezoneData(1110000000, "UTC-11", "Etc/GMT+11", "UTC-11"),
				new GeoTimezoneData(1100010000, "Aleutian", "America/Adak", "Aleutian Standard Time"),
				new GeoTimezoneData(1100000000, "Hawaiian", "Pacific/Honolulu", "Hawaiian Standard Time"),
				new GeoTimezoneData(1093000000, "Marquesas", "Pacific/Marquesas", "Marquesas Standard Time"),
				new GeoTimezoneData(1090010000, "Alaskan", "America/Anchorage", "Alaskan Standard Time"),
				new GeoTimezoneData(1090000000, "UTC-09", "Etc/GMT+9", "UTC-09"),
				new GeoTimezoneData(1080010000, "Pacific (Mexico)", "America/Tijuana", "Pacific Standard Time (Mexico)"),
				new GeoTimezoneData(1080000000, "UTC-08", "Etc/GMT+8", "UTC-08"),
				new GeoTimezoneData(1080010001, "Pacific", "America/Los_Angeles", "Pacific Standard Time"),
				new GeoTimezoneData(1070000000, "US Mountain", "America/Phoenix", "US Mountain Standard Time"),
				new GeoTimezoneData(1070010000, "Mountain (Mexico)", "America/Chihuahua", "Mountain Standard Time (Mexico)"),
				new GeoTimezoneData(1070010001, "Mountain", "America/Denver", "Mountain Standard Time"),
				new GeoTimezoneData(1060000000, "Central America", "America/Guatemala", "Central America Standard Time"),
				new GeoTimezoneData(1060010000, "Central", "America/Chicago", "Central Standard Time"),
				new GeoTimezoneData(1060010001, "Easter Island", "Pacific/Easter", "Easter Island Standard Time"),
				new GeoTimezoneData(1060010002, "Central (Mexico)", "America/Mexico_City", "Central Standard Time (Mexico)"),
				new GeoTimezoneData(1060000001, "Canada Central", "America/Regina", "Canada Central Standard Time"),
				new GeoTimezoneData(1050000000, "SA Pacific", "America/Bogota", "SA Pacific Standard Time"),
				new GeoTimezoneData(1050010000, "Eastern (Mexico)", "America/Cancun", "Eastern Standard Time (Mexico)"),
				new GeoTimezoneData(1050010001, "Eastern", "America/New_York", "Eastern Standard Time"),
				new GeoTimezoneData(1050010002, "Haiti", "America/Port-au-Prince", "Haiti Standard Time"),
				new GeoTimezoneData(1050010003, "Cuba", "America/Havana", "Cuba Standard Time"),
				new GeoTimezoneData(1050010004, "US Eastern", "America/Indianapolis", "US Eastern Standard Time"),
				new GeoTimezoneData(1040010000, "Paraguay", "America/Asuncion", "Paraguay Standard Time"),
				new GeoTimezoneData(1040010001, "Atlantic", "America/Halifax", "Atlantic Standard Time"),
				new GeoTimezoneData(1040010002, "Venezuela", "America/Caracas", "Venezuela Standard Time"),
				new GeoTimezoneData(1040010003, "Central Brazilian", "America/Cuiaba", "Central Brazilian Standard Time"),
				new GeoTimezoneData(1040000000, "SA Western", "America/La_Paz", "SA Western Standard Time"),
				new GeoTimezoneData(1040010004, "Pacific SA", "America/Santiago", "Pacific SA Standard Time"),
				new GeoTimezoneData(1040010005, "Turks And Caicos", "America/Grand_Turk", "Turks And Caicos Standard Time"),
				new GeoTimezoneData(1033010000, "Newfoundland", "America/St_Johns", "Newfoundland Standard Time"),
				new GeoTimezoneData(1030010000, "Tocantins", "America/Araguaina", "Tocantins Standard Time"),
				new GeoTimezoneData(1030010001, "E. South America", "America/Sao_Paulo", "E. South America Standard Time"),
				new GeoTimezoneData(1030000000, "SA Eastern", "America/Cayenne", "SA Eastern Standard Time"),
				new GeoTimezoneData(1030010002, "Argentina", "America/Buenos_Aires", "Argentina Standard Time"),
				new GeoTimezoneData(1030010003, "Greenland", "America/Godthab", "Greenland Standard Time"),
				new GeoTimezoneData(1030010004, "Montevideo", "America/Montevideo", "Montevideo Standard Time"),
				new GeoTimezoneData(1030010005, "Saint Pierre", "America/Miquelon", "Saint Pierre Standard Time"),
				new GeoTimezoneData(1030010006, "Bahia", "America/Bahia", "Bahia Standard Time"),
				new GeoTimezoneData(1020000000, "UTC-02", "Etc/GMT+2", "UTC-02"),
				new GeoTimezoneData(1020010000, "Mid-Atlantic", "Atlantic/South_Georgia", "Mid-Atlantic Standard Time"),
				new GeoTimezoneData(1010010000, "Azores", "Atlantic/Azores", "Azores Standard Time"),
				new GeoTimezoneData(1010000000, "Cape Verde", "Atlantic/Cape_Verde", "Cape Verde Standard Time"),
				new GeoTimezoneData(0, "UTC", "Etc/GMT", "UTC"),
				new GeoTimezoneData(10000, "Morocco", "Africa/Casablanca", "Morocco Standard Time"),
				new GeoTimezoneData(10001, "GMT", "Europe/London", "GMT Standard Time"),
				new GeoTimezoneData(1, "Greenwich", "Atlantic/Reykjavik", "Greenwich Standard Time"),
				new GeoTimezoneData(10010000, "W. Europe", "Europe/Berlin", "W. Europe Standard Time"),
				new GeoTimezoneData(10010001, "Central Europe", "Europe/Budapest", "Central Europe Standard Time"),
				new GeoTimezoneData(10010002, "Romance", "Europe/Paris", "Romance Standard Time"),
				new GeoTimezoneData(10010003, "Central European", "Europe/Warsaw", "Central European Standard Time"),
				new GeoTimezoneData(10000000, "W. Central Africa", "Africa/Lagos", "W. Central Africa Standard Time"),
				new GeoTimezoneData(10010004, "Namibia", "Africa/Windhoek", "Namibia Standard Time"),
				new GeoTimezoneData(20010000, "Jordan", "Asia/Amman", "Jordan Standard Time"),
				new GeoTimezoneData(20010001, "GTB", "Europe/Bucharest", "GTB Standard Time"),
				new GeoTimezoneData(20010002, "Middle East", "Asia/Beirut", "Middle East Standard Time"),
				new GeoTimezoneData(20010003, "Egypt", "Africa/Cairo", "Egypt Standard Time"),
				new GeoTimezoneData(20010004, "E. Europe", "Europe/Chisinau", "E. Europe Standard Time"),
				new GeoTimezoneData(20010005, "Syria", "Asia/Damascus", "Syria Standard Time"),
				new GeoTimezoneData(20010006, "West Bank", "Asia/Hebron", "West Bank Standard Time"),
				new GeoTimezoneData(20000000, "South Africa", "Africa/Johannesburg", "South Africa Standard Time"),
				new GeoTimezoneData(20010007, "FLE", "Europe/Kiev", "FLE Standard Time"),
				new GeoTimezoneData(20010008, "Israel", "Asia/Jerusalem", "Israel Standard Time"),
				new GeoTimezoneData(20010009, "Kaliningrad", "Europe/Kaliningrad", "Kaliningrad Standard Time"),
				new GeoTimezoneData(20010010, "Libya", "Africa/Tripoli", "Libya Standard Time"),
				new GeoTimezoneData(30010000, "Arabic", "Asia/Baghdad", "Arabic Standard Time"),
				new GeoTimezoneData(30010001, "Turkey", "Europe/Istanbul", "Turkey Standard Time"),
				new GeoTimezoneData(30000000, "Arab", "Asia/Riyadh", "Arab Standard Time"),
				new GeoTimezoneData(30010002, "Belarus", "Europe/Minsk", "Belarus Standard Time"),
				new GeoTimezoneData(30010003, "Russian", "Europe/Moscow", "Russian Standard Time"),
				new GeoTimezoneData(30000001, "E. Africa", "Africa/Nairobi", "E. Africa Standard Time"),
				new GeoTimezoneData(33010000, "Iran", "Asia/Tehran", "Iran Standard Time"),
				new GeoTimezoneData(40000000, "Arabian", "Asia/Dubai", "Arabian Standard Time"),
				new GeoTimezoneData(40010000, "Astrakhan", "Europe/Astrakhan", "Astrakhan Standard Time"),
				new GeoTimezoneData(40010001, "Azerbaijan", "Asia/Baku", "Azerbaijan Standard Time"),
				new GeoTimezoneData(40010002, "Russia Time Zone 3", "Europe/Samara", "Russia Time Zone 3"),
				new GeoTimezoneData(40010003, "Mauritius", "Indian/Mauritius", "Mauritius Standard Time"),
				new GeoTimezoneData(40000001, "Georgian", "Asia/Tbilisi", "Georgian Standard Time"),
				new GeoTimezoneData(40010004, "Caucasus", "Asia/Yerevan", "Caucasus Standard Time"),
				new GeoTimezoneData(43000000, "Afghanistan", "Asia/Kabul", "Afghanistan Standard Time"),
				new GeoTimezoneData(50000000, "West Asia", "Asia/Tashkent", "West Asia Standard Time"),
				new GeoTimezoneData(50010000, "Ekaterinburg", "Asia/Yekaterinburg", "Ekaterinburg Standard Time"),
				new GeoTimezoneData(50010001, "Pakistan", "Asia/Karachi", "Pakistan Standard Time"),
				new GeoTimezoneData(53000000, "India", "Asia/Calcutta", "India Standard Time"),
				new GeoTimezoneData(53000001, "Sri Lanka", "Asia/Colombo", "Sri Lanka Standard Time"),
				new GeoTimezoneData(54500000, "Nepal", "Asia/Katmandu", "Nepal Standard Time"),
				new GeoTimezoneData(60000000, "Central Asia", "Asia/Almaty", "Central Asia Standard Time"),
				new GeoTimezoneData(60010000, "Bangladesh", "Asia/Dhaka", "Bangladesh Standard Time"),
				new GeoTimezoneData(60010001, "Omsk", "Asia/Omsk", "Omsk Standard Time"),
				new GeoTimezoneData(63000000, "Myanmar", "Asia/Rangoon", "Myanmar Standard Time"),
				new GeoTimezoneData(70000000, "SE Asia", "Asia/Bangkok", "SE Asia Standard Time"),
				new GeoTimezoneData(70010000, "Altai", "Asia/Barnaul", "Altai Standard Time"),
				new GeoTimezoneData(70010001, "W. Mongolia", "Asia/Hovd", "W. Mongolia Standard Time"),
				new GeoTimezoneData(70010002, "North Asia", "Asia/Krasnoyarsk", "North Asia Standard Time"),
				new GeoTimezoneData(70010003, "N. Central Asia", "Asia/Novosibirsk", "N. Central Asia Standard Time"),
				new GeoTimezoneData(70010004, "Tomsk", "Asia/Tomsk", "Tomsk Standard Time"),
				new GeoTimezoneData(80000000, "China", "Asia/Shanghai", "China Standard Time"),
				new GeoTimezoneData(80010000, "North Asia East", "Asia/Irkutsk", "North Asia East Standard Time"),
				new GeoTimezoneData(80000001, "Singapore", "Asia/Singapore", "Singapore Standard Time"),
				new GeoTimezoneData(80010001, "W. Australia", "Australia/Perth", "W. Australia Standard Time"),
				new GeoTimezoneData(80000002, "Taipei", "Asia/Taipei", "Taipei Standard Time"),
				new GeoTimezoneData(80010002, "Ulaanbaatar", "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time"),
				new GeoTimezoneData(83010000, "North Korea", "Asia/Pyongyang", "North Korea Standard Time"),
				new GeoTimezoneData(84500000, "Aus Central W.", "Australia/Eucla", "Aus Central W. Standard Time"),
				new GeoTimezoneData(90010000, "Transbaikal", "Asia/Chita", "Transbaikal Standard Time"),
				new GeoTimezoneData(90000000, "Tokyo", "Asia/Tokyo", "Tokyo Standard Time"),
				new GeoTimezoneData(90000001, "Korea", "Asia/Seoul", "Korea Standard Time"),
				new GeoTimezoneData(90010001, "Yakutsk", "Asia/Yakutsk", "Yakutsk Standard Time"),
				new GeoTimezoneData(93010000, "Cen. Australia", "Australia/Adelaide", "Cen. Australia Standard Time"),
				new GeoTimezoneData(93000000, "AUS Central", "Australia/Darwin", "AUS Central Standard Time"),
				new GeoTimezoneData(100000000, "E. Australia", "Australia/Brisbane", "E. Australia Standard Time"),
				new GeoTimezoneData(100010000, "AUS Eastern", "Australia/Sydney", "AUS Eastern Standard Time"),
				new GeoTimezoneData(100000001, "West Pacific", "Pacific/Port_Moresby", "West Pacific Standard Time"),
				new GeoTimezoneData(100010001, "Tasmania", "Australia/Hobart", "Tasmania Standard Time"),
				new GeoTimezoneData(100010002, "Vladivostok", "Asia/Vladivostok", "Vladivostok Standard Time"),
				new GeoTimezoneData(103010000, "Lord Howe", "Australia/Lord_Howe", "Lord Howe Standard Time"),
				new GeoTimezoneData(110010000, "Bougainville", "Pacific/Bougainville", "Bougainville Standard Time"),
				new GeoTimezoneData(110010001, "Russia Time Zone 10", "Asia/Srednekolymsk", "Russia Time Zone 10"),
				new GeoTimezoneData(110010002, "Magadan", "Asia/Magadan", "Magadan Standard Time"),
				new GeoTimezoneData(110010003, "Norfolk", "Pacific/Norfolk", "Norfolk Standard Time"),
				new GeoTimezoneData(110010004, "Sakhalin", "Asia/Sakhalin", "Sakhalin Standard Time"),
				new GeoTimezoneData(110000000, "Central Pacific", "Pacific/Guadalcanal", "Central Pacific Standard Time"),
				new GeoTimezoneData(120010000, "Russia Time Zone 11", "Asia/Kamchatka", "Russia Time Zone 11"),
				new GeoTimezoneData(120010001, "New Zealand", "Pacific/Auckland", "New Zealand Standard Time"),
				new GeoTimezoneData(120000000, "UTC+12", "Etc/GMT-12", "UTC+12"),
				new GeoTimezoneData(120010002, "Fiji", "Pacific/Fiji", "Fiji Standard Time"),
				new GeoTimezoneData(120010003, "Kamchatka", "Asia/Kamchatka", "Kamchatka Standard Time"),
				new GeoTimezoneData(124510000, "Chatham Islands", "Pacific/Chatham", "Chatham Islands Standard Time"),
				new GeoTimezoneData(130010000, "Tonga", "Pacific/Tongatapu", "Tonga Standard Time"),
				new GeoTimezoneData(130010001, "Samoa", "Pacific/Apia", "Samoa Standard Time"),
				new GeoTimezoneData(140000000, "Line Islands", "Pacific/Kiritimati", "Line Islands Standard Time"),

				new GeoTimezoneData(1030010007, "Magallanes Standard Time", "America/Punta_Arenas", "Magallanes Standard Time"),
				new GeoTimezoneData(40000005, "Saratov Standard Time", "Europe/Saratov", "Saratov Standard Time"),
				new GeoTimezoneData(10000001, "Sudan Standard Time", "Africa/Sudan", "Sudan Standard Time"),
				new GeoTimezoneData(130000001, "UTC+13", "Etc/GMT-13", "UTC+13")
			};
			#endregion
			// --------------------------------------------------

			// add to global lookup
			foreach (var g in _windows)
			{
				_all.Add(g.GeoTZID, g);
			}
			
			// --------------------------------------------------
			#region Olson Based Time Zones
			_olson = new List<GeoTimezoneData>
			{
				new GeoTimezoneData(2000000000, "Etc/GMT", "UTC"),
				new GeoTimezoneData(2000000001, "Etc/GMT+1", "Cape Verde Standard Time"),
				new GeoTimezoneData(2000000002, "Etc/GMT+2", "UTC-02"),
				new GeoTimezoneData(2000000003, "Etc/GMT+3", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000004, "Etc/GMT+4", "SA Western Standard Time"),
				new GeoTimezoneData(2000000005, "Etc/GMT+5", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000006, "Etc/GMT+6", "Central America Standard Time"),
				new GeoTimezoneData(2000000007, "Etc/GMT+7", "US Mountain Standard Time"),
				new GeoTimezoneData(2000000008, "Etc/GMT+8", "UTC-08"),
				new GeoTimezoneData(2000000009, "Etc/GMT+9", "UTC-09"),
				new GeoTimezoneData(2000000010, "Etc/GMT+10", "Hawaiian Standard Time"),
				new GeoTimezoneData(2000000011, "Etc/GMT+11", "UTC-11"),
				new GeoTimezoneData(2000000012, "Etc/GMT+12", "Dateline Standard Time"),
				new GeoTimezoneData(2000000013, "Etc/GMT-1", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000014, "Etc/GMT-2", "South Africa Standard Time"),
				new GeoTimezoneData(2000000015, "Etc/GMT-3", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000016, "Etc/GMT-4", "Arabian Standard Time"),
				new GeoTimezoneData(2000000017, "Etc/GMT-5", "West Asia Standard Time"),
				new GeoTimezoneData(2000000018, "Etc/GMT-6", "Central Asia Standard Time"),
				new GeoTimezoneData(2000000019, "Etc/GMT-7", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000020, "Etc/GMT-8", "Singapore Standard Time"),
				new GeoTimezoneData(2000000021, "Etc/GMT-9", "Tokyo Standard Time"),
				new GeoTimezoneData(2000000022, "Etc/GMT-10", "West Pacific Standard Time"),
				new GeoTimezoneData(2000000023, "Etc/GMT-11", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000024, "Etc/GMT-12", "UTC+12"),
				new GeoTimezoneData(2000000025, "Etc/GMT-13", "Tonga Standard Time"),
				new GeoTimezoneData(2000000026, "EST5EDT", "Eastern Standard Time"),
				new GeoTimezoneData(2000000027, "CST6CDT", "Central Standard Time"),
				new GeoTimezoneData(2000000028, "PST8PDT", "Pacific Standard Time"),
				new GeoTimezoneData(2000000029, "Africa/Abidjan", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000030, "Africa/Accra", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000031, "Africa/Addis_Ababa", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000032, "Africa/Algiers", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000033, "Africa/Asmera", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000034, "Africa/Bamako", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000035, "Africa/Bangui", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000036, "Africa/Banjul", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000037, "Africa/Bissau", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000038, "Africa/Blantyre", "South Africa Standard Time"),
				new GeoTimezoneData(2000000039, "Africa/Brazzaville", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000040, "Africa/Bujumbura", "South Africa Standard Time"),
				new GeoTimezoneData(2000000041, "Africa/Cairo", "Egypt Standard Time"),
				new GeoTimezoneData(2000000042, "Africa/Casablanca", "Morocco Standard Time"),
				new GeoTimezoneData(2000000043, "Africa/Ceuta", "Romance Standard Time"),
				new GeoTimezoneData(2000000044, "Africa/Conakry", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000045, "Africa/Dakar", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000046, "Africa/Dar_es_Salaam", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000047, "Africa/Djibouti", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000048, "Africa/Douala", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000049, "Africa/El_Aaiun", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000050, "Africa/Freetown", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000051, "Africa/Gaborone", "South Africa Standard Time"),
				new GeoTimezoneData(2000000052, "Africa/Harare", "South Africa Standard Time"),
				new GeoTimezoneData(2000000053, "Africa/Johannesburg", "South Africa Standard Time"),
				new GeoTimezoneData(2000000054, "Africa/Juba", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000055, "Africa/Kampala", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000056, "Africa/Khartoum", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000057, "Africa/Kigali", "South Africa Standard Time"),
				new GeoTimezoneData(2000000058, "Africa/Kinshasa", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000059, "Africa/Lagos", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000060, "Africa/Libreville", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000061, "Africa/Lome", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000062, "Africa/Luanda", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000063, "Africa/Lubumbashi", "South Africa Standard Time"),
				new GeoTimezoneData(2000000064, "Africa/Lusaka", "South Africa Standard Time"),
				new GeoTimezoneData(2000000065, "Africa/Malabo", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000066, "Africa/Maputo", "South Africa Standard Time"),
				new GeoTimezoneData(2000000067, "Africa/Maseru", "South Africa Standard Time"),
				new GeoTimezoneData(2000000068, "Africa/Mbabane", "South Africa Standard Time"),
				new GeoTimezoneData(2000000069, "Africa/Mogadishu", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000070, "Africa/Monrovia", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000071, "Africa/Nairobi", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000072, "Africa/Ndjamena", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000073, "Africa/Niamey", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000074, "Africa/Nouakchott", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000075, "Africa/Ouagadougou", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000076, "Africa/Porto-Novo", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000077, "Africa/Sao_Tome", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000078, "Africa/Tripoli", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000079, "Africa/Tunis", "W. Central Africa Standard Time"),
				new GeoTimezoneData(2000000080, "Africa/Windhoek", "Namibia Standard Time"),
				new GeoTimezoneData(2000000081, "America/Adak", "Aleutian Standard Time"),
				new GeoTimezoneData(2000000082, "America/Anchorage", "Alaskan Standard Time"),
				new GeoTimezoneData(2000000083, "America/Anguilla", "SA Western Standard Time"),
				new GeoTimezoneData(2000000084, "America/Antigua", "SA Western Standard Time"),
				new GeoTimezoneData(2000000085, "America/Araguaina", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000086, "America/Argentina/La_Rioja", "Argentina Standard Time"),
				new GeoTimezoneData(2000000087, "America/Argentina/Rio_Gallegos", "Argentina Standard Time"),
				new GeoTimezoneData(2000000088, "America/Argentina/Salta", "Argentina Standard Time"),
				new GeoTimezoneData(2000000089, "America/Argentina/San_Juan", "Argentina Standard Time"),
				new GeoTimezoneData(2000000090, "America/Argentina/San_Luis", "Argentina Standard Time"),
				new GeoTimezoneData(2000000091, "America/Argentina/Tucuman", "Argentina Standard Time"),
				new GeoTimezoneData(2000000092, "America/Argentina/Ushuaia", "Argentina Standard Time"),
				new GeoTimezoneData(2000000093, "America/Aruba", "SA Western Standard Time"),
				new GeoTimezoneData(2000000094, "America/Asuncion", "Paraguay Standard Time"),
				new GeoTimezoneData(2000000095, "America/Bahia", "Bahia Standard Time"),
				new GeoTimezoneData(2000000096, "America/Bahia_Banderas", "Central Standard Time (Mexico)"),
				new GeoTimezoneData(2000000097, "America/Barbados", "SA Western Standard Time"),
				new GeoTimezoneData(2000000098, "America/Belem", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000099, "America/Belize", "Central America Standard Time"),
				new GeoTimezoneData(2000000100, "America/Blanc-Sablon", "SA Western Standard Time"),
				new GeoTimezoneData(2000000101, "America/Boa_Vista", "SA Western Standard Time"),
				new GeoTimezoneData(2000000102, "America/Bogota", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000103, "America/Boise", "Mountain Standard Time"),
				new GeoTimezoneData(2000000104, "America/Buenos_Aires", "Argentina Standard Time"),
				new GeoTimezoneData(2000000105, "America/Cambridge_Bay", "Mountain Standard Time"),
				new GeoTimezoneData(2000000106, "America/Campo_Grande", "Central Brazilian Standard Time"),
				new GeoTimezoneData(2000000107, "America/Cancun", "Central Standard Time (Mexico)"),
				new GeoTimezoneData(2000000108, "America/Caracas", "Venezuela Standard Time"),
				new GeoTimezoneData(2000000109, "America/Catamarca", "Argentina Standard Time"),
				new GeoTimezoneData(2000000110, "America/Cayenne", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000111, "America/Cayman", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000112, "America/Chicago", "Central Standard Time"),
				new GeoTimezoneData(2000000113, "America/Chihuahua", "Mountain Standard Time (Mexico)"),
				new GeoTimezoneData(2000000114, "America/Coral_Harbour", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000115, "America/Cordoba", "Argentina Standard Time"),
				new GeoTimezoneData(2000000116, "America/Costa_Rica", "Central America Standard Time"),
				new GeoTimezoneData(2000000117, "America/Creston", "US Mountain Standard Time"),
				new GeoTimezoneData(2000000118, "America/Cuiaba", "Central Brazilian Standard Time"),
				new GeoTimezoneData(2000000119, "America/Curacao", "SA Western Standard Time"),
				new GeoTimezoneData(2000000120, "America/Danmarkshavn", "UTC"),
				new GeoTimezoneData(2000000121, "America/Dawson", "Pacific Standard Time"),
				new GeoTimezoneData(2000000122, "America/Dawson_Creek", "US Mountain Standard Time"),
				new GeoTimezoneData(2000000123, "America/Denver", "Mountain Standard Time"),
				new GeoTimezoneData(2000000124, "America/Detroit", "Eastern Standard Time"),
				new GeoTimezoneData(2000000125, "America/Dominica", "SA Western Standard Time"),
				new GeoTimezoneData(2000000126, "America/Edmonton", "Mountain Standard Time"),
				new GeoTimezoneData(2000000127, "America/Eirunepe", "SA Western Standard Time"),
				new GeoTimezoneData(2000000128, "America/El_Salvador", "Central America Standard Time"),
				new GeoTimezoneData(2000000129, "America/Fortaleza", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000130, "America/Glace_Bay", "Atlantic Standard Time"),
				new GeoTimezoneData(2000000131, "America/Godthab", "Greenland Standard Time"),
				new GeoTimezoneData(2000000132, "America/Goose_Bay", "Atlantic Standard Time"),
				new GeoTimezoneData(2000000133, "America/Grand_Turk", "Eastern Standard Time"),
				new GeoTimezoneData(2000000134, "America/Grenada", "SA Western Standard Time"),
				new GeoTimezoneData(2000000135, "America/Guadeloupe", "SA Western Standard Time"),
				new GeoTimezoneData(2000000136, "America/Guatemala", "Central America Standard Time"),
				new GeoTimezoneData(2000000137, "America/Guayaquil", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000138, "America/Guyana", "SA Western Standard Time"),
				new GeoTimezoneData(2000000139, "America/Halifax", "Atlantic Standard Time"),
				new GeoTimezoneData(2000000140, "America/Havana", "Cuba Standard Time"),
				new GeoTimezoneData(2000000141, "America/Hermosillo", "US Mountain Standard Time"),
				new GeoTimezoneData(2000000142, "America/Indiana/Knox", "Central Standard Time"),
				new GeoTimezoneData(2000000143, "America/Indiana/Marengo", "US Eastern Standard Time"),
				new GeoTimezoneData(2000000144, "America/Indiana/Petersburg", "Eastern Standard Time"),
				new GeoTimezoneData(2000000145, "America/Indiana/Tell_City", "Central Standard Time"),
				new GeoTimezoneData(2000000146, "America/Indiana/Vevay", "US Eastern Standard Time"),
				new GeoTimezoneData(2000000147, "America/Indiana/Vincennes", "Eastern Standard Time"),
				new GeoTimezoneData(2000000148, "America/Indiana/Winamac", "Eastern Standard Time"),
				new GeoTimezoneData(2000000149, "America/Indianapolis", "US Eastern Standard Time"),
				new GeoTimezoneData(2000000150, "America/Inuvik", "Mountain Standard Time"),
				new GeoTimezoneData(2000000151, "America/Iqaluit", "Eastern Standard Time"),
				new GeoTimezoneData(2000000152, "America/Jamaica", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000153, "America/Jujuy", "Argentina Standard Time"),
				new GeoTimezoneData(2000000154, "America/Juneau", "Alaskan Standard Time"),
				new GeoTimezoneData(2000000155, "America/Kentucky/Monticello", "Eastern Standard Time"),
				new GeoTimezoneData(2000000156, "America/Kralendijk", "SA Western Standard Time"),
				new GeoTimezoneData(2000000157, "America/La_Paz", "SA Western Standard Time"),
				new GeoTimezoneData(2000000158, "America/Lima", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000159, "America/Los_Angeles", "Pacific Standard Time"),
				new GeoTimezoneData(2000000160, "America/Louisville", "Eastern Standard Time"),
				new GeoTimezoneData(2000000161, "America/Lower_Princes", "SA Western Standard Time"),
				new GeoTimezoneData(2000000162, "America/Maceio", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000163, "America/Managua", "Central America Standard Time"),
				new GeoTimezoneData(2000000164, "America/Manaus", "SA Western Standard Time"),
				new GeoTimezoneData(2000000165, "America/Marigot", "SA Western Standard Time"),
				new GeoTimezoneData(2000000166, "America/Martinique", "SA Western Standard Time"),
				new GeoTimezoneData(2000000167, "America/Matamoros", "Central Standard Time"),
				new GeoTimezoneData(2000000168, "America/Mazatlan", "Mountain Standard Time (Mexico)"),
				new GeoTimezoneData(2000000169, "America/Mendoza", "Argentina Standard Time"),
				new GeoTimezoneData(2000000170, "America/Menominee", "Central Standard Time"),
				new GeoTimezoneData(2000000171, "America/Merida", "Central Standard Time (Mexico)"),
				new GeoTimezoneData(2000000172, "America/Mexico_City", "Central Standard Time (Mexico)"),
				new GeoTimezoneData(2000000173, "America/Miquelon", "Saint Pierre Standard Time"),
				new GeoTimezoneData(2000000174, "America/Moncton", "Atlantic Standard Time"),
				new GeoTimezoneData(2000000175, "America/Monterrey", "Central Standard Time (Mexico)"),
				new GeoTimezoneData(2000000176, "America/Montevideo", "Montevideo Standard Time"),
				new GeoTimezoneData(2000000177, "America/Montreal", "Eastern Standard Time"),
				new GeoTimezoneData(2000000178, "America/Montserrat", "SA Western Standard Time"),
				new GeoTimezoneData(2000000179, "America/Nassau", "Eastern Standard Time"),
				new GeoTimezoneData(2000000180, "America/New_York", "Eastern Standard Time"),
				new GeoTimezoneData(2000000181, "America/Nipigon", "Eastern Standard Time"),
				new GeoTimezoneData(2000000182, "America/Nome", "Alaskan Standard Time"),
				new GeoTimezoneData(2000000183, "America/Noronha", "UTC-02"),
				new GeoTimezoneData(2000000184, "America/North_Dakota/Beulah", "Central Standard Time"),
				new GeoTimezoneData(2000000185, "America/North_Dakota/Center", "Central Standard Time"),
				new GeoTimezoneData(2000000186, "America/North_Dakota/New_Salem", "Central Standard Time"),
				new GeoTimezoneData(2000000187, "America/Ojinaga", "Mountain Standard Time"),
				new GeoTimezoneData(2000000188, "America/Panama", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000189, "America/Pangnirtung", "Eastern Standard Time"),
				new GeoTimezoneData(2000000190, "America/Paramaribo", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000191, "America/Phoenix", "US Mountain Standard Time"),
				new GeoTimezoneData(2000000192, "America/Port_of_Spain", "SA Western Standard Time"),
				new GeoTimezoneData(2000000193, "America/Port-au-Prince", "SA Pacific Standard Time"),
				new GeoTimezoneData(2000000194, "America/Porto_Velho", "SA Western Standard Time"),
				new GeoTimezoneData(2000000195, "America/Puerto_Rico", "SA Western Standard Time"),
				new GeoTimezoneData(2000000196, "America/Rainy_River", "Central Standard Time"),
				new GeoTimezoneData(2000000197, "America/Rankin_Inlet", "Central Standard Time"),
				new GeoTimezoneData(2000000198, "America/Recife", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000199, "America/Regina", "Canada Central Standard Time"),
				new GeoTimezoneData(2000000200, "America/Resolute", "Central Standard Time"),
				new GeoTimezoneData(2000000201, "America/Rio_Branco", "SA Western Standard Time"),
				new GeoTimezoneData(2000000202, "America/Santa_Isabel", "Pacific Standard Time (Mexico)"),
				new GeoTimezoneData(2000000203, "America/Santarem", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000204, "America/Santiago", "Pacific SA Standard Time"),
				new GeoTimezoneData(2000000205, "America/Santo_Domingo", "SA Western Standard Time"),
				new GeoTimezoneData(2000000206, "America/Sao_Paulo", "E. South America Standard Time"),
				new GeoTimezoneData(2000000207, "America/Scoresbysund", "Azores Standard Time"),
				new GeoTimezoneData(2000000208, "America/Shiprock", "Mountain Standard Time"),
				new GeoTimezoneData(2000000209, "America/Sitka", "Alaskan Standard Time"),
				new GeoTimezoneData(2000000210, "America/St_Barthelemy", "SA Western Standard Time"),
				new GeoTimezoneData(2000000211, "America/St_Johns", "Newfoundland Standard Time"),
				new GeoTimezoneData(2000000212, "America/St_Kitts", "SA Western Standard Time"),
				new GeoTimezoneData(2000000213, "America/St_Lucia", "SA Western Standard Time"),
				new GeoTimezoneData(2000000214, "America/St_Thomas", "SA Western Standard Time"),
				new GeoTimezoneData(2000000215, "America/St_Vincent", "SA Western Standard Time"),
				new GeoTimezoneData(2000000216, "America/Swift_Current", "Canada Central Standard Time"),
				new GeoTimezoneData(2000000217, "America/Tegucigalpa", "Central America Standard Time"),
				new GeoTimezoneData(2000000218, "America/Thule", "Atlantic Standard Time"),
				new GeoTimezoneData(2000000219, "America/Thunder_Bay", "Eastern Standard Time"),
				new GeoTimezoneData(2000000220, "America/Tijuana", "Pacific Standard Time"),
				new GeoTimezoneData(2000000221, "America/Toronto", "Eastern Standard Time"),
				new GeoTimezoneData(2000000222, "America/Tortola", "SA Western Standard Time"),
				new GeoTimezoneData(2000000223, "America/Vancouver", "Pacific Standard Time"),
				new GeoTimezoneData(2000000224, "America/Whitehorse", "Pacific Standard Time"),
				new GeoTimezoneData(2000000225, "America/Winnipeg", "Central Standard Time"),
				new GeoTimezoneData(2000000226, "America/Yakutat", "Alaskan Standard Time"),
				new GeoTimezoneData(2000000227, "America/Yellowknife", "Mountain Standard Time"),
				new GeoTimezoneData(2000000228, "Antarctica/Casey", "W. Australia Standard Time"),
				new GeoTimezoneData(2000000229, "Antarctica/Davis", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000230, "Antarctica/DumontDUrville", "West Pacific Standard Time"),
				new GeoTimezoneData(2000000231, "Antarctica/Macquarie", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000232, "Antarctica/Mawson", "West Asia Standard Time"),
				new GeoTimezoneData(2000000233, "Antarctica/McMurdo", "New Zealand Standard Time"),
				new GeoTimezoneData(2000000234, "Antarctica/Palmer", "Pacific SA Standard Time"),
				new GeoTimezoneData(2000000235, "Antarctica/Rothera", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000236, "Antarctica/South_Pole", "New Zealand Standard Time"),
				new GeoTimezoneData(2000000237, "Antarctica/Syowa", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000238, "Antarctica/Vostok", "Central Asia Standard Time"),
				new GeoTimezoneData(2000000239, "Arctic/Longyearbyen", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000240, "Asia/Aden", "Arab Standard Time"),
				new GeoTimezoneData(2000000241, "Asia/Almaty", "Central Asia Standard Time"),
				new GeoTimezoneData(2000000242, "Asia/Amman", "Jordan Standard Time"),
				new GeoTimezoneData(2000000243, "Asia/Anadyr", "Magadan Standard Time"),
				new GeoTimezoneData(2000000244, "Asia/Aqtau", "West Asia Standard Time"),
				new GeoTimezoneData(2000000245, "Asia/Aqtobe", "West Asia Standard Time"),
				new GeoTimezoneData(2000000246, "Asia/Ashgabat", "West Asia Standard Time"),
				new GeoTimezoneData(2000000247, "Asia/Baghdad", "Arabic Standard Time"),
				new GeoTimezoneData(2000000248, "Asia/Bahrain", "Arab Standard Time"),
				new GeoTimezoneData(2000000249, "Asia/Baku", "Azerbaijan Standard Time"),
				new GeoTimezoneData(2000000250, "Asia/Bangkok", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000251, "Asia/Barnaul", "Altai Standard Time"),
				new GeoTimezoneData(2000000252, "Asia/Beirut", "Middle East Standard Time"),
				new GeoTimezoneData(2000000253, "Asia/Bishkek", "Central Asia Standard Time"),
				new GeoTimezoneData(2000000254, "Asia/Brunei", "Singapore Standard Time"),
				new GeoTimezoneData(2000000255, "Asia/Calcutta", "India Standard Time"),
				new GeoTimezoneData(2000000256, "Asia/Chita", "Transbaikal Standard Time"),
				new GeoTimezoneData(2000000257, "Asia/Choibalsan", "Ulaanbaatar Standard Time"),
				new GeoTimezoneData(2000000258, "Asia/Chongqing", "China Standard Time"),
				new GeoTimezoneData(2000000259, "Asia/Colombo", "Sri Lanka Standard Time"),
				new GeoTimezoneData(2000000260, "Asia/Damascus", "Syria Standard Time"),
				new GeoTimezoneData(2000000261, "Asia/Dhaka", "Bangladesh Standard Time"),
				new GeoTimezoneData(2000000262, "Asia/Dili", "Tokyo Standard Time"),
				new GeoTimezoneData(2000000263, "Asia/Dubai", "Arabian Standard Time"),
				new GeoTimezoneData(2000000264, "Asia/Dushanbe", "West Asia Standard Time"),
				new GeoTimezoneData(2000000265, "Asia/Gaza", "Egypt Standard Time"),
				new GeoTimezoneData(2000000266, "Asia/Harbin", "China Standard Time"),
				new GeoTimezoneData(2000000267, "Asia/Hebron", "Egypt Standard Time"),
				new GeoTimezoneData(2000000268, "Asia/Hong_Kong", "China Standard Time"),
				new GeoTimezoneData(2000000269, "Asia/Hovd", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000270, "Asia/Irkutsk", "North Asia East Standard Time"),
				new GeoTimezoneData(2000000271, "Asia/Jakarta", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000272, "Asia/Jayapura", "Tokyo Standard Time"),
				new GeoTimezoneData(2000000273, "Asia/Jerusalem", "Israel Standard Time"),
				new GeoTimezoneData(2000000274, "Asia/Kabul", "Afghanistan Standard Time"),
				new GeoTimezoneData(2000000275, "Asia/Kamchatka", "Magadan Standard Time"),
				new GeoTimezoneData(2000000276, "Asia/Karachi", "Pakistan Standard Time"),
				new GeoTimezoneData(2000000277, "Asia/Kashgar", "China Standard Time"),
				new GeoTimezoneData(2000000278, "Asia/Katmandu", "Nepal Standard Time"),
				new GeoTimezoneData(2000000279, "Asia/Krasnoyarsk", "North Asia Standard Time"),
				new GeoTimezoneData(2000000280, "Asia/Kuala_Lumpur", "Singapore Standard Time"),
				new GeoTimezoneData(2000000281, "Asia/Kuching", "Singapore Standard Time"),
				new GeoTimezoneData(2000000282, "Asia/Kuwait", "Arab Standard Time"),
				new GeoTimezoneData(2000000283, "Asia/Macau", "China Standard Time"),
				new GeoTimezoneData(2000000284, "Asia/Magadan", "Magadan Standard Time"),
				new GeoTimezoneData(2000000285, "Asia/Makassar", "Singapore Standard Time"),
				new GeoTimezoneData(2000000286, "Asia/Manila", "Singapore Standard Time"),
				new GeoTimezoneData(2000000287, "Asia/Muscat", "Arabian Standard Time"),
				new GeoTimezoneData(2000000288, "Asia/Nicosia", "E. Europe Standard Time"),
				new GeoTimezoneData(2000000289, "Asia/Novokuznetsk", "N. Central Asia Standard Time"),
				new GeoTimezoneData(2000000290, "Asia/Novosibirsk", "N. Central Asia Standard Time"),
				new GeoTimezoneData(2000000291, "Asia/Omsk", "N. Central Asia Standard Time"),
				new GeoTimezoneData(2000000292, "Asia/Oral", "West Asia Standard Time"),
				new GeoTimezoneData(2000000293, "Asia/Phnom_Penh", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000294, "Asia/Pontianak", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000295, "Asia/Pyongyang", "Korea Standard Time"),
				new GeoTimezoneData(2000000296, "Asia/Qatar", "Arab Standard Time"),
				new GeoTimezoneData(2000000297, "Asia/Qyzylorda", "Central Asia Standard Time"),
				new GeoTimezoneData(2000000298, "Asia/Rangoon", "Myanmar Standard Time"),
				new GeoTimezoneData(2000000299, "Asia/Riyadh", "Arab Standard Time"),
				new GeoTimezoneData(2000000300, "Asia/Saigon", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000301, "Asia/Sakhalin", "Vladivostok Standard Time"),
				new GeoTimezoneData(2000000302, "Asia/Samarkand", "West Asia Standard Time"),
				new GeoTimezoneData(2000000303, "Asia/Seoul", "Korea Standard Time"),
				new GeoTimezoneData(2000000304, "Asia/Shanghai", "China Standard Time"),
				new GeoTimezoneData(2000000305, "Asia/Singapore", "Singapore Standard Time"),
				new GeoTimezoneData(2000000306, "Asia/Srednekolymsk", "Russia Time Zone 10"),
				new GeoTimezoneData(2000000307, "Asia/Taipei", "Taipei Standard Time"),
				new GeoTimezoneData(2000000308, "Asia/Tashkent", "West Asia Standard Time"),
				new GeoTimezoneData(2000000309, "Asia/Tbilisi", "Georgian Standard Time"),
				new GeoTimezoneData(2000000310, "Asia/Tehran", "Iran Standard Time"),
				new GeoTimezoneData(2000000311, "Asia/Thimphu", "Bangladesh Standard Time"),
				new GeoTimezoneData(2000000312, "Asia/Tokyo", "Tokyo Standard Time"),
				new GeoTimezoneData(2000000313, "Asia/Tomsk", "Tomsk Standard Time"),
				new GeoTimezoneData(2000000314, "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time"),
				new GeoTimezoneData(2000000315, "Asia/Urumqi", "China Standard Time"),
				new GeoTimezoneData(2000000316, "Asia/Vientiane", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000317, "Asia/Vladivostok", "Vladivostok Standard Time"),
				new GeoTimezoneData(2000000318, "Asia/Yakutsk", "Yakutsk Standard Time"),
				new GeoTimezoneData(2000000319, "Asia/Yekaterinburg", "Ekaterinburg Standard Time"),
				new GeoTimezoneData(2000000320, "Asia/Yerevan", "Caucasus Standard Time"),
				new GeoTimezoneData(2000000321, "Atlantic/Azores", "Azores Standard Time"),
				new GeoTimezoneData(2000000322, "Atlantic/Bermuda", "Atlantic Standard Time"),
				new GeoTimezoneData(2000000323, "Atlantic/Canary", "GMT Standard Time"),
				new GeoTimezoneData(2000000324, "Atlantic/Cape_Verde", "Cape Verde Standard Time"),
				new GeoTimezoneData(2000000325, "Atlantic/Faeroe", "GMT Standard Time"),
				new GeoTimezoneData(2000000326, "Atlantic/Madeira", "GMT Standard Time"),
				new GeoTimezoneData(2000000327, "Atlantic/Reykjavik", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000328, "Atlantic/South_Georgia", "UTC-02"),
				new GeoTimezoneData(2000000329, "Atlantic/St_Helena", "Greenwich Standard Time"),
				new GeoTimezoneData(2000000330, "Atlantic/Stanley", "SA Eastern Standard Time"),
				new GeoTimezoneData(2000000331, "Australia/Adelaide", "Cen. Australia Standard Time"),
				new GeoTimezoneData(2000000332, "Australia/Brisbane", "E. Australia Standard Time"),
				new GeoTimezoneData(2000000333, "Australia/Broken_Hill", "Cen. Australia Standard Time"),
				new GeoTimezoneData(2000000334, "Australia/Currie", "Tasmania Standard Time"),
				new GeoTimezoneData(2000000335, "Australia/Darwin", "AUS Central Standard Time"),
				new GeoTimezoneData(2000000336, "Australia/Eucla", "Aus Central W. Standard Time"),
				new GeoTimezoneData(2000000337, "Australia/Hobart", "Tasmania Standard Time"),
				new GeoTimezoneData(2000000338, "Australia/Lindeman", "E. Australia Standard Time"),
				new GeoTimezoneData(2000000339, "Australia/Lord_Howe", "Lord Howe Standard Time"),
				new GeoTimezoneData(2000000340, "Australia/Melbourne", "AUS Eastern Standard Time"),
				new GeoTimezoneData(2000000341, "Australia/Perth", "W. Australia Standard Time"),
				new GeoTimezoneData(2000000342, "Australia/Sydney", "AUS Eastern Standard Time"),
				new GeoTimezoneData(2000000343, "Europe/Amsterdam", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000344, "Europe/Andorra", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000345, "Europe/Astrakhan", "Astrakhan Standard Time"),
				new GeoTimezoneData(2000000346, "Europe/Athens", "GTB Standard Time"),
				new GeoTimezoneData(2000000347, "Europe/Belgrade", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000348, "Europe/Berlin", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000349, "Europe/Bratislava", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000350, "Europe/Brussels", "Romance Standard Time"),
				new GeoTimezoneData(2000000351, "Europe/Bucharest", "GTB Standard Time"),
				new GeoTimezoneData(2000000352, "Europe/Budapest", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000353, "Europe/Chisinau", "GTB Standard Time"),
				new GeoTimezoneData(2000000354, "Europe/Copenhagen", "Romance Standard Time"),
				new GeoTimezoneData(2000000355, "Europe/Dublin", "GMT Standard Time"),
				new GeoTimezoneData(2000000356, "Europe/Gibraltar", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000357, "Europe/Guernsey", "GMT Standard Time"),
				new GeoTimezoneData(2000000358, "Europe/Helsinki", "FLE Standard Time"),
				new GeoTimezoneData(2000000359, "Europe/Isle_of_Man", "GMT Standard Time"),
				new GeoTimezoneData(2000000360, "Europe/Istanbul", "Turkey Standard Time"),
				new GeoTimezoneData(2000000361, "Europe/Jersey", "GMT Standard Time"),
				new GeoTimezoneData(2000000362, "Europe/Kaliningrad", "Kaliningrad Standard Time"),
				new GeoTimezoneData(2000000363, "Europe/Kiev", "FLE Standard Time"),
				new GeoTimezoneData(2000000364, "Europe/Lisbon", "GMT Standard Time"),
				new GeoTimezoneData(2000000365, "Europe/Ljubljana", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000366, "Europe/London", "GMT Standard Time"),
				new GeoTimezoneData(2000000367, "Europe/Luxembourg", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000368, "Europe/Madrid", "Romance Standard Time"),
				new GeoTimezoneData(2000000369, "Europe/Malta", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000370, "Europe/Mariehamn", "FLE Standard Time"),
				new GeoTimezoneData(2000000371, "Europe/Minsk", "Kaliningrad Standard Time"),
				new GeoTimezoneData(2000000372, "Europe/Monaco", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000373, "Europe/Moscow", "Russian Standard Time"),
				new GeoTimezoneData(2000000374, "Europe/Oslo", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000375, "Europe/Paris", "Romance Standard Time"),
				new GeoTimezoneData(2000000376, "Europe/Podgorica", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000377, "Europe/Prague", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000378, "Europe/Riga", "FLE Standard Time"),
				new GeoTimezoneData(2000000379, "Europe/Rome", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000380, "Europe/Samara", "Russian Standard Time"),
				new GeoTimezoneData(2000000381, "Europe/San_Marino", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000382, "Europe/Sarajevo", "Central European Standard Time"),
				new GeoTimezoneData(2000000383, "Europe/Simferopol", "FLE Standard Time"),
				new GeoTimezoneData(2000000384, "Europe/Skopje", "Central European Standard Time"),
				new GeoTimezoneData(2000000385, "Europe/Sofia", "FLE Standard Time"),
				new GeoTimezoneData(2000000386, "Europe/Stockholm", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000387, "Europe/Tallinn", "FLE Standard Time"),
				new GeoTimezoneData(2000000388, "Europe/Tirane", "Central Europe Standard Time"),
				new GeoTimezoneData(2000000389, "Europe/Uzhgorod", "FLE Standard Time"),
				new GeoTimezoneData(2000000390, "Europe/Vaduz", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000391, "Europe/Vatican", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000392, "Europe/Vienna", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000393, "Europe/Vilnius", "FLE Standard Time"),
				new GeoTimezoneData(2000000394, "Europe/Volgograd", "Russian Standard Time"),
				new GeoTimezoneData(2000000395, "Europe/Warsaw", "Central European Standard Time"),
				new GeoTimezoneData(2000000396, "Europe/Zagreb", "Central European Standard Time"),
				new GeoTimezoneData(2000000397, "Europe/Zaporozhye", "FLE Standard Time"),
				new GeoTimezoneData(2000000398, "Europe/Zurich", "W. Europe Standard Time"),
				new GeoTimezoneData(2000000399, "Indian/Antananarivo", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000400, "Indian/Chagos", "Central Asia Standard Time"),
				new GeoTimezoneData(2000000401, "Indian/Christmas", "SE Asia Standard Time"),
				new GeoTimezoneData(2000000402, "Indian/Cocos", "Myanmar Standard Time"),
				new GeoTimezoneData(2000000403, "Indian/Comoro", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000404, "Indian/Kerguelen", "West Asia Standard Time"),
				new GeoTimezoneData(2000000405, "Indian/Mahe", "Mauritius Standard Time"),
				new GeoTimezoneData(2000000406, "Indian/Maldives", "West Asia Standard Time"),
				new GeoTimezoneData(2000000407, "Indian/Mauritius", "Mauritius Standard Time"),
				new GeoTimezoneData(2000000408, "Indian/Mayotte", "E. Africa Standard Time"),
				new GeoTimezoneData(2000000409, "Indian/Reunion", "Mauritius Standard Time"),
				new GeoTimezoneData(2000000410, "MST7MDT", "Mountain Standard Time"),
				new GeoTimezoneData(2000000411, "Pacific/Apia", "Samoa Standard Time"),
				new GeoTimezoneData(2000000412, "Pacific/Auckland", "New Zealand Standard Time"),
				new GeoTimezoneData(2000000413, "Pacific/Bougainville", "Bougainville Standard Time"),
				new GeoTimezoneData(2000000414, "Pacific/Chatham", "Chatham Islands Standard Time"),
				new GeoTimezoneData(2000000415, "Pacific/Easter", "Easter Island Standard Time"),
				new GeoTimezoneData(2000000416, "Pacific/Efate", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000417, "Pacific/Enderbury", "Tonga Standard Time"),
				new GeoTimezoneData(2000000418, "Pacific/Fakaofo", "Tonga Standard Time"),
				new GeoTimezoneData(2000000419, "Pacific/Fiji", "Fiji Standard Time"),
				new GeoTimezoneData(2000000420, "Pacific/Funafuti", "UTC+12"),
				new GeoTimezoneData(2000000421, "Pacific/Galapagos", "Central America Standard Time"),
				new GeoTimezoneData(2000000422, "Pacific/Guadalcanal", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000423, "Pacific/Guam", "West Pacific Standard Time"),
				new GeoTimezoneData(2000000424, "Pacific/Honolulu", "Hawaiian Standard Time"),
				new GeoTimezoneData(2000000425, "Pacific/Johnston", "Hawaiian Standard Time"),
				new GeoTimezoneData(2000000426, "Pacific/Kiritimati", "Line Islands Standard Time"),
				new GeoTimezoneData(2000000427, "Pacific/Kosrae", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000428, "Pacific/Kwajalein", "UTC+12"),
				new GeoTimezoneData(2000000429, "Pacific/Majuro", "UTC+12"),
				new GeoTimezoneData(2000000430, "Pacific/Marquesas", "Marquesas Standard Time"),
				new GeoTimezoneData(2000000431, "Pacific/Midway", "UTC-11"),
				new GeoTimezoneData(2000000432, "Pacific/Nauru", "UTC+12"),
				new GeoTimezoneData(2000000433, "Pacific/Niue", "UTC-11"),
				new GeoTimezoneData(2000000434, "Pacific/Norfolk", "Norfolk Standard Time"),
				new GeoTimezoneData(2000000435, "Pacific/Noumea", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000436, "Pacific/Pago_Pago", "UTC-11"),
				new GeoTimezoneData(2000000437, "Pacific/Palau", "Tokyo Standard Time"),
				new GeoTimezoneData(2000000438, "Pacific/Ponape", "Central Pacific Standard Time"),
				new GeoTimezoneData(2000000439, "Pacific/Port_Moresby", "West Pacific Standard Time"),
				new GeoTimezoneData(2000000440, "Pacific/Rarotonga", "Hawaiian Standard Time"),
				new GeoTimezoneData(2000000441, "Pacific/Saipan", "West Pacific Standard Time"),
				new GeoTimezoneData(2000000442, "Pacific/Tahiti", "Hawaiian Standard Time"),
				new GeoTimezoneData(2000000443, "Pacific/Tarawa", "UTC+12"),
				new GeoTimezoneData(2000000444, "Pacific/Tongatapu", "Tonga Standard Time"),
				new GeoTimezoneData(2000000445, "Pacific/Truk", "West Pacific Standard Time"),
				new GeoTimezoneData(2000000446, "Pacific/Wake", "UTC+12"),
				new GeoTimezoneData(2000000447, "Pacific/Wallis", "UTC+12"),

				// Windows 10 Creators Update
				new GeoTimezoneData(2000000448, "America/Punta_Arenas", "Magallanes Standard Time"),
				new GeoTimezoneData(2000000449, "Europe/Saratov", "Georgian Standard Time"),
				new GeoTimezoneData(2000000450, "Africa/Sudan", "Sudan Standard Time"),
				new GeoTimezoneData(2000000451, "Etc/GMT-13", "UTC+13")
			};
			#endregion
			// --------------------------------------------------

			// add to global lookup
			foreach (var g in _olson)
			{
				_all.Add(g.GeoTZID, g);
			}
		}

		/*
		/// <summary>
		/// GeoTimezoneID
		/// 4294967296 = int32 max
		/// 0 UTC+
		/// 1 UTC -
		/// _HHMM - hours : mins UTC +/-
		/// _____0 - no DST
		/// _____1 - DST +1 hour
		/// _____2 - DST something special
		/// ______XXXX - 0000-9999 index number of timezones at this UTC+DST offset combination
		/// </summary>
		/// <param name="utc"></param>
		/// <param name="dst"></param>
		/// <returns></returns>
		private static int BuildGeoTZID(TimeSpan utc, bool dst = false)
		{
			var sid = "";
			sid += (utc.TotalMinutes < 0 ? "1" : "0");
			sid += Math.Abs(utc.Hours).ToString("00");
			sid += Math.Abs(utc.Minutes).ToString("00");
			sid += (dst ? "1" : "0");
			sid += "0000";

			var count = 0;
			var id = Convert.ToInt32(sid);
			while (_all.ContainsKey(id))
			{
				id++;
				count++;
				if (count == 9999) throw new Exception("Cannot create valid unique GeoTimezone ID");
			}
			return id;
		}*/
	}
}