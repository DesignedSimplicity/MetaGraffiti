using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Modules.Geo.Data;

namespace MetaGraffiti.Base.Modules.Geo.Info
{
	public class GeoRegionInfo
	{
		// ==================================================
		// Internals
		protected GeoRegionData _data;


		// ==================================================
		// Constructors
		public GeoRegionInfo(GeoRegionData data) { _data = data; }


		// ==================================================
		// Properties

		// --------------------------------------------------
		// Inferred
		public int RegionID => _data.RegionID;
		public int CountryID => _data.CountryID;

		public int RegionDiv => _data.RegionDiv;

		public string RegionISO => _data.RegionISO;
		public string RegionAbbr => _data.RegionAbbr;
		public string RegionName => _data.RegionName;
		public string RegionNameLocal => _data.RegionNameLocal;

		public IGeoLatLon Center => _data.Center;
		public IGeoPerimeter Bounds => _data.Bounds;

		// --------------------------------------------------
		// Derived
		public GeoCountryInfo Country { get { return GeoCountryInfo.ByID(CountryID); } }

		// --------------------------------------------------
		// Instance
		public GeoTimezoneInfo Timezone { get; set; }


		// ==================================================
		// Methods
		public bool IsSame(GeoRegionInfo region)
		{
			if (region == null) return false;
			return (RegionID == region.RegionID);
		}


		// ==================================================
		// Static

		// --------------------------------------------------
		// Properties
		public static List<GeoRegionInfo> All { get { return Cache.ToInfo().ToList(); } }

		// --------------------------------------------------
		// Methods
		public static GeoRegionInfo ByID(int regionID)
		{
			return Cache.FirstOrDefault(x => x.RegionID == regionID).ToInfo();
		}

		public static GeoRegionInfo ByISO(string iso)
		{
			if (String.IsNullOrWhiteSpace(iso)) return null;
			return Cache.FirstOrDefault(x => x.RegionISO == iso.ToUpperInvariant()).ToInfo();
		}

		public static GeoRegionInfo ByISO(string isoCountry, string isoRegion)
		{
			if (String.IsNullOrWhiteSpace(isoCountry) || String.IsNullOrWhiteSpace(isoRegion)) return null;
			return Cache.FirstOrDefault(x => x.RegionISO == $"{isoCountry}-{isoRegion}".ToUpperInvariant()).ToInfo();
		}

		public static GeoRegionInfo ByAbbr(string abbr)
		{
			if (String.IsNullOrWhiteSpace(abbr)) return null;
			return Cache.FirstOrDefault(x => x.RegionAbbr == abbr.ToUpperInvariant()).ToInfo();
		}

		public static GeoRegionInfo ByName(string name)
		{
			return Cache.FirstOrDefault(x => String.Compare(x.RegionName, name, true) == 0).ToInfo();
		}

		public static GeoRegionInfo Find(string text)
		{
			if (String.IsNullOrWhiteSpace(text)) return null;

			var r = ByISO(text);
			if (r != null) return r;

			r = ByAbbr(text);
			if (r != null) return r;

			r = ByName(text);
			return r;
		}

		public static IEnumerable<GeoRegionInfo> ListByCountry(int countryID)
		{
			return Cache.Where(x => x.CountryID == countryID).ToInfo();
		}

		public static IEnumerable<GeoRegionInfo> ListByLocation(IGeoLatLon point)
		{
			return Cache.Where(x => x.Bounds.Contains(point)).ToInfo();
		}

		// --------------------------------------------------
		// Globals
		private static bool _initialized = false;
		private static List<GeoRegionData> _cache = new List<GeoRegionData>();
		private static IEnumerable<GeoRegionData> Cache { get { if (!_initialized) Initialize(); return _cache; } }

		private static void Initialize()
		{
			lock (_cache)
			{
				// check if we are already initialized after a wait
				if (_initialized) return;

				#region United States
				_cache.Add(new GeoRegionData(840, 1001, 1, "US-DE", "DE", "Delaware", @"", 38.910832, -75.527669, 39.839483, -75.789147, 38.451018, -75.048345));
				_cache.Add(new GeoRegionData(840, 1002, 1, "US-PA", "PA", "Pennsylvania", @"", 41.203321, -77.194524, 42.269365, -80.519895, 39.719798, -74.689501));
				_cache.Add(new GeoRegionData(840, 1003, 1, "US-NJ", "NJ", "New Jersey", @"", 40.058323, -74.405661, 41.357423, -75.559792, 38.928625, -73.902439));
				_cache.Add(new GeoRegionData(840, 1004, 1, "US-GA", "GA", "Georgia", @"", 32.157435, -82.907123, 35.000658, -85.605164, 30.355590, -80.841339));
				_cache.Add(new GeoRegionData(840, 1005, 1, "US-CT", "CT", "Connecticut", @"", 41.60322, -73.087749, 42.050587, -73.727775, 40.985059, -71.787239));
				_cache.Add(new GeoRegionData(840, 1006, 1, "US-MA", "MA", "Massachusetts", @"", 42.40721, -71.382437, 42.886790, -73.508141, 41.239089, -69.927992));
				_cache.Add(new GeoRegionData(840, 1007, 1, "US-MD", "MD", "Maryland", @"", 39.045754, -76.641271, 39.723037, -79.487651, 37.912160, -75.049181));
				_cache.Add(new GeoRegionData(840, 1008, 1, "US-SC", "SC", "South Carolina", @"", 33.836081, -81.163724, 35.215540, -83.353258, 32.034559, -78.540817));
				_cache.Add(new GeoRegionData(840, 1009, 1, "US-NH", "NH", "New Hampshire", @"", 43.193851, -71.572395, 45.305476, -72.557185, 42.696985, -70.602661));
				_cache.Add(new GeoRegionData(840, 1010, 1, "US-VA", "VA", "Virginia", @"", 37.431573, -78.656894, 39.466012, -83.675415, 36.540758, -75.242157));
				_cache.Add(new GeoRegionData(840, 1011, 1, "US-NY", "NY", "New York", @"", 40.714352, -74.005973, 40.915255, -74.255734, 40.495914, -73.700272));
				_cache.Add(new GeoRegionData(840, 1012, 1, "US-NC", "NC", "North Carolina", @"", 35.759573, -79.019299, 36.588156, -84.321869, 33.840969, -75.460666));
				_cache.Add(new GeoRegionData(840, 1013, 1, "US-RI", "RI", "Rhode Island", @"", 41.580094, -71.477429, 42.018800, -71.892342, 41.146656, -71.120559));
				_cache.Add(new GeoRegionData(840, 1014, 1, "US-VT", "VT", "Vermont", @"", 44.558802, -72.577841, 45.016659, -73.430526, 42.726849, -71.465039));
				_cache.Add(new GeoRegionData(840, 1015, 1, "US-KY", "KY", "Kentucky", @"", 37.839333, -84.270017, 39.147458, -89.569328, 36.497220, -81.96497));
				_cache.Add(new GeoRegionData(840, 1016, 1, "US-TN", "TN", "Tennessee", @"", 35.517491, -86.580447, 36.677985, -90.310297, 34.982923, -81.6469));
				_cache.Add(new GeoRegionData(840, 1017, 1, "US-OH", "OH", "Ohio", @"", 40.417287, -82.907123, 41.977290, -84.820304, 38.403422, -80.5182));
				_cache.Add(new GeoRegionData(840, 1018, 1, "US-LA", "LA", "Louisiana", @"", 31.244823, -92.145024, 33.019544, -94.043352, 28.927209, -88.816467));
				_cache.Add(new GeoRegionData(840, 1019, 1, "US-IN", "IN", "Indiana", @"", 40.267194, -86.134901, 41.760697, -88.097892, 37.771741, -84.784662));
				_cache.Add(new GeoRegionData(840, 1020, 1, "US-MS", "MS", "Mississippi", @"", 32.354667, -89.398528, 34.996109, -91.652994, 30.174103, -88.09943));
				_cache.Add(new GeoRegionData(840, 1021, 1, "US-IL", "IL", "Illinois", @"", 40.633124, -89.398528, 42.508337, -91.511816, 36.973314, -87.495199));
				_cache.Add(new GeoRegionData(840, 1022, 1, "US-AL", "AL", "Alabama", @"", 32.318231, -86.902298, 35.007889, -88.473226, 30.194166, -84.888246));
				_cache.Add(new GeoRegionData(840, 1023, 1, "US-ME", "ME", "Maine", @"", 45.253783, -69.445468, 47.459686, -71.084334, 42.974704, -66.949606));
				_cache.Add(new GeoRegionData(840, 1024, 1, "US-MO", "MO", "Missouri", @"", 37.964252, -91.831833, 40.613640, -95.774704, 35.995682, -89.10446));
				_cache.Add(new GeoRegionData(840, 1025, 1, "US-AR", "AR", "Arkansas", @"", 35.20105, -91.831833, 36.499749, -94.617919, 33.004105, -89.646814));
				_cache.Add(new GeoRegionData(840, 1026, 1, "US-MI", "MI", "Michigan", @"", 44.314844, -85.602364, 48.262516, -90.418135, 41.696118, -82.413474));
				_cache.Add(new GeoRegionData(840, 1027, 1, "US-FL", "FL", "Florida", @"", 27.664827, -81.515753, 31.000968, -87.634896, 24.521404, -80.031137));
				_cache.Add(new GeoRegionData(840, 1028, 1, "US-TX", "TX", "Texas", @"", 31.968598, -99.901813, 36.501508, -106.645646, 25.837163, -93.508038));
				_cache.Add(new GeoRegionData(840, 1029, 1, "US-IA", "IA", "Iowa", @"", 41.878002, -93.097702, 43.501196, -96.639535, 40.375437, -90.142625));
				_cache.Add(new GeoRegionData(840, 1030, 1, "US-WI", "WI", "Wisconsin", @"", 43.784439, -88.787867, 47.080717, -92.889433, 42.491864, -86.763998));
				_cache.Add(new GeoRegionData(840, 1031, 1, "US-CA", "CA", "California", @"", 36.778261, -119.417932, 42.009516, -124.409619, 32.534232, -114.131211));
				_cache.Add(new GeoRegionData(840, 1032, 1, "US-MN", "MN", "Minnesota", @"", 46.729553, -94.685899, 49.384358, -97.239195, 43.499361, -89.491756));
				_cache.Add(new GeoRegionData(840, 1033, 1, "US-OR", "OR", "Oregon", @"", 43.804133, -120.554201, 46.292015, -124.612936, 41.991794, -116.463262));
				_cache.Add(new GeoRegionData(840, 1034, 1, "US-KS", "KS", "Kansas", @"", 39.011902, -98.484246, 40.004541, -102.051768, 36.993015, -94.588387));
				_cache.Add(new GeoRegionData(840, 1035, 1, "US-WV", "WV", "West Virginia", @"", 38.597626, -80.454902, 40.638801, -82.644413, 37.201539, -77.718982));
				_cache.Add(new GeoRegionData(840, 1036, 1, "US-NV", "NV", "Nevada", @"", 38.802609, -116.419389, 42.002207, -120.006472, 35.001857, -114.039647));
				_cache.Add(new GeoRegionData(840, 1037, 1, "US-NE", "NE", "Nebraska", @"", 41.492537, -99.901813, 43.001706, -104.053514, 39.999932, -95.30829));
				_cache.Add(new GeoRegionData(840, 1038, 1, "US-CO", "CO", "Colorado", @"", 39.55005, -105.782067, 41.003443, -109.060256, 36.992424, -102.040878));
				_cache.Add(new GeoRegionData(840, 1039, 1, "US-ND", "ND", "North Dakota", @"", 47.551492, -101.002011, 49.000692, -104.050040, 45.935071, -96.554491));
				_cache.Add(new GeoRegionData(840, 1040, 1, "US-SD", "SD", "South Dakota", @"", 43.969514, -99.901813, 45.945713, -104.057739, 42.479686, -96.436589));
				_cache.Add(new GeoRegionData(840, 1041, 1, "US-MT", "MT", "Montana", @"", 46.879682, -110.362565, 49.001390, -116.050003, 44.358208, -104.039563));
				_cache.Add(new GeoRegionData(840, 1042, 1, "US-WA", "WA", "Washington", @"", 47.443092, -120.761719, 49.062169, -124.892578, 45.824014, -116.630859));
				_cache.Add(new GeoRegionData(840, 1043, 1, "US-ID", "ID", "Idaho", @"", 44.068201, -114.742040, 49.001146, -117.241365, 41.988005, -111.043495));
				_cache.Add(new GeoRegionData(840, 1044, 1, "US-WY", "WY", "Wyoming", @"", 43.075967, -107.290283, 45.005904, -111.056889, 40.994746, -104.052244));
				_cache.Add(new GeoRegionData(840, 1045, 1, "US-UT", "UT", "Utah", @"", 39.32098, -111.093731, 42.001618, -114.052997, 36.997903, -109.041058));
				_cache.Add(new GeoRegionData(840, 1046, 1, "US-OK", "OK", "Oklahoma", @"", 35.46756, -97.516427, 35.674752, -97.833674, 35.290543, -97.12472));
				_cache.Add(new GeoRegionData(840, 1047, 1, "US-NM", "NM", "New Mexico", @"", 34.51994, -105.870090, 37.000293, -109.050173, 31.332172, -103.001964));
				_cache.Add(new GeoRegionData(840, 1048, 1, "US-AZ", "AZ", "Arizona", @"", 34.048928, -111.093731, 37.004259, -114.816590, 31.332177, -109.045223));
				_cache.Add(new GeoRegionData(840, 1049, 1, "US-AK", "AK", "Alaska", @"", 64.200841, -149.493673, 71.520727, -168.437594, 52.688363, -131.567476));
				_cache.Add(new GeoRegionData(840, 1050, 1, "US-HI", "HI", "Hawaii", @"", 19.896766, -155.582781, 22.370000, -160.530000, 18.550000, -154.48));
				_cache.Add(new GeoRegionData(840, 1051, 1, "US-DC", "DC", "District of Columbia", @"", 38.90723, -77.036464, 38.995548, -77.119759, 38.791644, -76.909393));
				#endregion

				#region Argentina
				_cache.Add(new GeoRegionData(32, 2001, 1, "AR-A", "A", "Salta", @"", -24.782932, -65.412155, -24.710187, -65.499115, -24.872471, -65.354611));
				_cache.Add(new GeoRegionData(32, 2002, 1, "AR-B", "B", "Buenos Aires", @"", -34.603723, -58.381593, -34.526546, -58.531452, -34.705158, -58.335144));
				_cache.Add(new GeoRegionData(32, 2003, 1, "AR-C", "C", "Capital Federal", @"", -34.603723, -58.381593, -34.526546, -58.531452, -34.705158, -58.335144));
				_cache.Add(new GeoRegionData(32, 2004, 1, "AR-D", "D", "San Luis", @"", -33.302220, -66.336797, -33.258356, -66.384365, -33.341238, -66.234525));
				_cache.Add(new GeoRegionData(32, 2005, 1, "AR-E", "E", "Entre Ríos", @"", -32.517564, -59.104175, -30.157686, -60.768061, -34.039127, -57.811914));
				_cache.Add(new GeoRegionData(32, 2006, 1, "AR-F", "F", "La Rioja", @"", -29.412800, -66.855980, -29.379902, -66.925365, -29.456885, -66.789069));
				_cache.Add(new GeoRegionData(32, 2007, 1, "AR-G", "G", "Santiago del Estero", @"", -27.784420, -64.267280, -27.742627, -64.311439, -27.855165, -64.221685));
				_cache.Add(new GeoRegionData(32, 2008, 1, "AR-H", "H", "Chaco", @"", -26.585765, -60.954007, -24.087868, -63.427358, -27.995535, -58.363626));
				_cache.Add(new GeoRegionData(32, 2009, 1, "AR-J", "J", "San Juan", @"", -31.527273, -68.521408, -31.489469, -68.644147, -31.601707, -68.463134));
				_cache.Add(new GeoRegionData(32, 2010, 1, "AR-K", "K", "Catamarca", @"", -28.468990, -65.778971, -28.420321, -65.824873, -28.510153, -65.728934));
				_cache.Add(new GeoRegionData(32, 2011, 1, "AR-L", "L", "La Pampa", @"", -37.895659, -65.095779, -34.992315, -68.295450, -39.316145, -63.386824));
				_cache.Add(new GeoRegionData(32, 2012, 1, "AR-M", "M", "Mendoza", @"", -32.890183, -68.844049, -32.809139, -68.900637, -33.043756, -68.726921));
				_cache.Add(new GeoRegionData(32, 2013, 1, "AR-N", "N", "Misiones", @"", -26.937714, -54.434213, -25.495489, -56.059504, -28.163359, -53.638557));
				_cache.Add(new GeoRegionData(32, 2014, 1, "AR-P", "P", "Formosa", @"", -26.185201, -58.175369, -26.124033, -58.247943, -26.220278, -58.141996));
				_cache.Add(new GeoRegionData(32, 2015, 1, "AR-Q", "Q", "Neuquén", @"", -38.952444, -68.064138, -38.893459, -68.192991, -38.986537, -68.014738));
				_cache.Add(new GeoRegionData(32, 2016, 1, "AR-R", "R", "Río Negro", @"", -40.734434, -66.617645, -37.572914, -71.902907, -42.002498, -62.791082));
				_cache.Add(new GeoRegionData(32, 2017, 1, "AR-S", "S", "Santa Fe", @"", 35.686975, -105.937799, 35.754008, -106.071886, 35.610977, -105.894186));
				_cache.Add(new GeoRegionData(32, 2018, 1, "AR-T", "T", "Tucumán", @"", -26.808284, -65.217590, -26.763602, -65.336328, -26.893568, -65.166769));
				_cache.Add(new GeoRegionData(32, 2019, 1, "AR-U", "U", "Chubut", @"", -43.684619, -69.274553, -41.991621, -72.197338, -46.002230, -63.583257));
				_cache.Add(new GeoRegionData(32, 2020, 1, "AR-V", "V", "Tierra del Fuego", @"", -53.867111, -69.297214, -52.456120, -72.023773, -54.960663, -68.604752));
				_cache.Add(new GeoRegionData(32, 2021, 1, "AR-W", "W", "Corrientes", @"", -27.471225, -58.839584, -27.438600, -58.857274, -27.532796, -58.747493));
				_cache.Add(new GeoRegionData(32, 2022, 1, "AR-X", "X", "Córdoba", @"", 37.888175, -4.779383, 37.927278, -4.822793, 37.855893, -4.746176));
				_cache.Add(new GeoRegionData(32, 2023, 1, "AR-Y", "Y", "Jujuy", @"", -24.185786, -65.299476, -24.150234, -65.376300, -24.255764, -65.233613));
				_cache.Add(new GeoRegionData(32, 2024, 1, "AR-Z", "Z", "Santa Cruz", @"", 36.974117, -122.030796, 37.010848, -122.075931, 36.948014, -121.986218));
				#endregion

				#region Austria
				_cache.Add(new GeoRegionData(40, 2025, 1, "AT-1", "1", "Burgenland", @"", 47.153716, 16.268879, 48.118790, 15.996320, 46.830470, 17.160399));
				_cache.Add(new GeoRegionData(40, 2026, 1, "AT-2", "2", "Kärnten", @"", 46.722203, 14.180588, 47.131310, 12.656390, 46.372300, 15.065140));
				_cache.Add(new GeoRegionData(40, 2027, 1, "AT-3", "3", "Niederösterreich", @"", 48.108077, 15.804955, 49.020620, 14.452130, 47.421980, 17.068470));
				_cache.Add(new GeoRegionData(40, 2028, 1, "AT-4", "4", "Oberösterreich", @"", 48.025854, 13.972366, 48.772690, 12.748950, 47.460980, 14.991290));
				_cache.Add(new GeoRegionData(40, 2029, 1, "AT-5", "5", "Salzburg", @"", 47.809490, 13.055010, 47.854310, 12.985980, 47.751310, 13.126879));
				_cache.Add(new GeoRegionData(40, 2030, 1, "AT-6", "6", "Steiermark", @"", 47.359344, 14.469982, 47.827890, 13.564170, 46.611630, 16.170140));
				_cache.Add(new GeoRegionData(40, 2031, 1, "AT-7", "7", "Tirol", @"", 47.253741, 11.601487, 47.743110, 10.098070, 46.651559, 12.966280));
				_cache.Add(new GeoRegionData(40, 2032, 1, "AT-8", "8", "Vorarlberg", @"", 47.249742, 9.979737, 47.596210, 9.530909, 46.840810, 10.236890));
				_cache.Add(new GeoRegionData(40, 2033, 1, "AT-9", "9", "Wien", @"", 48.208174, 16.373818, 48.323099, 16.182619, 48.118269, 16.577499));
				#endregion

				#region Australia
				_cache.Add(new GeoRegionData(36, 2034, 1, "AU-ACT", "ACT", "Australian Capital Territory", @"", -35.473467, 149.012367, -35.124512, 148.764097, -35.92053, 149.399284));
				_cache.Add(new GeoRegionData(36, 2035, 1, "AU-NSW", "NSW", "New South Wales", @"", -31.253218, 146.921099, -28.156192, 140.999212, -37.505031, 153.638673));
				_cache.Add(new GeoRegionData(36, 2036, 1, "AU-NT", "NT", "Northern Territory", @"", -19.49141, 132.55096, -10.96588, 129.000424, -26.016869, 137.999009));
				_cache.Add(new GeoRegionData(36, 2037, 1, "AU-QLD", "QLD", "Queensland", @"", -20.917573, 142.702795, -9.92973, 137.994574, -29.178587, 153.552919));
				_cache.Add(new GeoRegionData(36, 2038, 1, "AU-SA", "SA", "South Australia", @"", -30.000231, 136.209154, -25.996392, 129.000516, -38.06121, 141.00288));
				_cache.Add(new GeoRegionData(36, 2039, 1, "AU-TAS", "TAS", "Tasmania", @"", -41.365041, 146.62849, -39.550056, 143.805523, -43.677108, 148.518371));
				_cache.Add(new GeoRegionData(36, 2040, 1, "AU-VIC", "VIC", "Victoria", @"", -37.471307, 144.785153, -34.547238, 140.95, -39.102039, 150.04));
				_cache.Add(new GeoRegionData(36, 2041, 1, "AU-WA", "WA", "Western Australia", @"", -27.672816, 121.628309, -14.379746, 112.967031, -35.450265, 129.270741));
				#endregion

				#region Belgium
				_cache.Add(new GeoRegionData(56, 2042, 1, "BE-BRU", "BRU", "Brussels", @"Brussels Hoofdstedelijk Gewest, Bruxelles-Capitale", 50.850339, 4.35171, 50.91371, 4.3138, 50.79624, 4.436979));
				_cache.Add(new GeoRegionData(56, 2043, 2, "BE-VAN", "VAN", "Anvers", @"Antwerpen", 51.219215, 4.402881, 51.37743, 4.241398, 51.14334, 4.49784));
				_cache.Add(new GeoRegionData(56, 2044, 2, "BE-VBR", "VBR", "Brabant Flamand", @"Vlaams-Brabant", 50.881543, 4.564597, 51.04988, 3.88928, 50.68736, 5.18831));
				_cache.Add(new GeoRegionData(56, 2045, 2, "BE-VLI", "VLI", "Limbourg", @"Limburg", 50.620312, 5.941893, 50.6532, 5.89825, 50.57629, 5.983189));
				_cache.Add(new GeoRegionData(56, 2046, 2, "BE-VOV", "VOV", "Flandre Orientale", @"Oost-Vlaanderen", 51.03621, 3.737312, 51.348462, 3.33125, 50.72095, 4.3301));
				_cache.Add(new GeoRegionData(56, 2047, 2, "BE-VWV", "VWV", "Flandre Occidentale", @"West-Vlaanderen", 51.053602, 3.145794, 51.368657, 2.54494, 50.70816, 3.523299));
				_cache.Add(new GeoRegionData(56, 2048, 2, "BE-WBR", "WBR", "Brabant Wallon", @"Waals-Brabant", 50.633241, 4.524315, 50.80735, 4.09115, 50.52542, 5.02037));
				_cache.Add(new GeoRegionData(56, 2049, 2, "BE-WHT", "WHT", "Hainaut", @"Henegouwen", 50.525707, 4.062101, 50.81077, 2.842129, 49.94183, 4.617129));
				_cache.Add(new GeoRegionData(56, 2050, 2, "BE-WLG", "WLG", "Liège", @"Luik", 50.632557, 5.579666, 50.68819, 5.52307, 50.56109, 5.67511));
				_cache.Add(new GeoRegionData(56, 2051, 2, "BE-WLX", "WLX", "Luxembourg", @"Luxemburg", 49.815273, 6.129583, 50.18282, 5.735669, 49.447779, 6.53097));
				_cache.Add(new GeoRegionData(56, 2052, 2, "BE-WNA", "WNA", "Namur", @"Namen", 50.465328, 4.867665, 50.53122, 4.7229, 50.38738, 4.98398));
				#endregion

				#region Chile
				_cache.Add(new GeoRegionData(152, 2053, 1, "CL-AI", "AI", "Aysen del General Carlos Ibáñez del Campo", @"", -46.378345, -72.300762, -43.639976, -75.650233, -49.343969, -71.087501));
				_cache.Add(new GeoRegionData(152, 2054, 1, "CL-AN", "AN", "Antofagasta", @"", -23.650000, -70.400000, -23.059123, -70.629116, -25.401120, -68.118212));
				_cache.Add(new GeoRegionData(152, 2055, 1, "CL-AP", "AP", "Arica y Parinacota", @"", -18.594048, -69.478454, -17.500857, -70.379633, -19.230279, -68.916098));
				_cache.Add(new GeoRegionData(152, 2056, 1, "CL-AR", "AR", "La Araucanía", @"", -38.948921, -72.331113, -37.588049, -73.522871, -39.639724, -70.830112));
				_cache.Add(new GeoRegionData(152, 2057, 1, "CL-AT", "AT", "Atacama", @"", -27.566055, -70.050314, -25.289700, -71.593688, -29.537375, -68.266862));
				_cache.Add(new GeoRegionData(152, 2058, 1, "CL-BI", "BI", "Bío-Bío", @"", -36.977720, -72.331113, -36.008314, -73.969935, -38.491821, -70.988319));
				_cache.Add(new GeoRegionData(152, 2059, 1, "CL-CO", "CO", "Coquimbo", @"", -29.953300, -71.343600, -29.933159, -71.676863, -30.515065, -71.117463));
				_cache.Add(new GeoRegionData(152, 2060, 1, "CL-LI", "LI", "Libertador General Bernardo O'Higgins", @"", -34.575537, -71.002231, -33.853768, -72.071730, -35.006672, -70.012147));
				_cache.Add(new GeoRegionData(152, 2061, 1, "CL-LL", "LL", "Los Lagos", @"", 6.574722, 3.318611, 6.606201, 3.302912, 6.541857, 3.336951));
				_cache.Add(new GeoRegionData(152, 2062, 1, "CL-LR", "LR", "Los Ríos", @"", -40.231021, -72.331113, -39.287140, -73.727548, -40.681904, -71.582925));
				_cache.Add(new GeoRegionData(152, 2063, 1, "CL-MA", "MA", "Magallanes y de La Antártica Chilena", @"", -52.206431, -72.168500, -48.596600, -75.696786, -55.978583, -66.417835));
				_cache.Add(new GeoRegionData(152, 2064, 1, "CL-ML", "ML", "Maule", @"", 48.909203, 1.849405, 48.926705, 1.802338, 48.884958, 1.879348));
				_cache.Add(new GeoRegionData(152, 2065, 1, "CL-RM", "RM", "Región Metropolitana de Santiago", @"", -33.484335, -70.621679, -32.919451, -71.718694, -34.287880, -69.768994));
				_cache.Add(new GeoRegionData(152, 2066, 1, "CL-TA", "TA", "Tarapacá", @"", -20.202879, -69.287753, -18.938714, -70.288534, -21.633400, -68.406844));
				_cache.Add(new GeoRegionData(152, 2067, 1, "CL-VS", "VS", "Valparaíso", @"", -33.045646, -71.620361, -33.017816, -71.744452, -33.214974, -71.385611));
				#endregion

				#region China
				_cache.Add(new GeoRegionData(156, 2068, 1, "CN-11", "11", "Beijing", @"北京", 39.904030, 116.407526, 40.216496, 116.011934, 39.661271, 116.782983));
				_cache.Add(new GeoRegionData(156, 2069, 1, "CN-12", "12", "Tianjin", @"天津", 39.084158, 117.200983, 39.446832, 116.779174, 38.803230, 117.811889));
				_cache.Add(new GeoRegionData(156, 2070, 1, "CN-13", "13", "Hebei", @"河北", 38.037057, 114.468665, 42.619717, 113.465368, 36.048206, 119.854256));
				_cache.Add(new GeoRegionData(156, 2071, 1, "CN-14", "14", "Shanxi", @"山西", 37.873532, 112.562398, 40.737352, 110.230248, 34.581935, 114.568922));
				_cache.Add(new GeoRegionData(156, 2072, 1, "CN-15", "15", "Inner Mongolia", @"内蒙古", 40.817498, 111.765618, 53.337178, 97.172762, 37.406779, 126.075585));
				_cache.Add(new GeoRegionData(156, 2073, 1, "CN-21", "21", "Liaoning", @"辽宁", 41.835441, 123.429440, 43.490984, 118.845466, 38.722353, 125.791459));
				_cache.Add(new GeoRegionData(156, 2074, 1, "CN-22", "22", "Jilin", @"吉林", 43.837883, 126.549572, 44.004854, 126.385828, 43.704491, 126.750866));
				_cache.Add(new GeoRegionData(156, 2075, 1, "CN-23", "23", "Heilongjiang", @"黑龙江", 45.742347, 126.661669, 53.563623, 121.181753, 43.425798, 135.095669));
				_cache.Add(new GeoRegionData(156, 2076, 1, "CN-31", "31", "Shanghai", @"上海", 31.230393, 121.473704, 31.668896, 120.839706, 30.779801, 122.113798));
				_cache.Add(new GeoRegionData(156, 2077, 1, "CN-32", "32", "Jiangsu", @"江苏", 32.061707, 118.763232, 35.124513, 116.361960, 30.757840, 121.941977));
				_cache.Add(new GeoRegionData(156, 2078, 1, "CN-33", "33", "Zhejiang", @"浙江", 30.267447, 120.152792, 31.178782, 118.028279, 27.046733, 122.955665));
				_cache.Add(new GeoRegionData(156, 2079, 1, "CN-34", "34", "Anhui", @"安徽", 31.861184, 117.284923, 34.654233, 114.882751, 29.393037, 119.649144));
				_cache.Add(new GeoRegionData(156, 2080, 1, "CN-35", "35", "Fujian", @"福建", 26.099933, 119.296506, 28.312901, 115.852290, 23.521040, 120.725362));
				_cache.Add(new GeoRegionData(156, 2081, 1, "CN-36", "36", "Jiangxi", @"江西", 28.674432, 115.908915, 30.074946, 113.578186, 24.486252, 118.487106));
				_cache.Add(new GeoRegionData(156, 2082, 1, "CN-37", "37", "Shandong", @"山东", 36.668627, 117.020411, 38.401144, 114.824630, 34.377352, 122.712945));
				_cache.Add(new GeoRegionData(156, 2083, 1, "CN-41", "41", "Henan", @"河南", 34.765527, 113.753658, 36.366560, 110.360476, 31.382371, 116.652232));
				_cache.Add(new GeoRegionData(156, 2084, 1, "CN-42", "42", "Hubei", @"湖北", 30.545861, 114.341921, 33.275616, 108.366964, 29.029488, 116.134577));
				_cache.Add(new GeoRegionData(156, 2085, 1, "CN-43", "43", "Hunan", @"湖南", 28.112444, 112.983810, 30.126363, 108.790841, 24.636323, 114.261264));
				_cache.Add(new GeoRegionData(156, 2086, 1, "CN-44", "44", "Guangdong", @"广东", 23.132191, 113.266531, 25.516771, 109.668298, 20.221081, 117.318083));
				_cache.Add(new GeoRegionData(156, 2087, 1, "CN-45", "45", "Guangxi", @"广西", 22.815478, 108.327546, 26.385565, 104.450132, 20.894552, 112.061850));
				_cache.Add(new GeoRegionData(156, 2088, 1, "CN-46", "46", "Hainan", @"海南", 20.017378, 110.349229, 20.158830, 108.616342, 18.153485, 111.277975));
				_cache.Add(new GeoRegionData(156, 2089, 1, "CN-50", "50", "Chongqing", @"重庆", 29.563010, 106.551557, 29.740196, 106.283283, 29.369628, 106.813824));
				_cache.Add(new GeoRegionData(156, 2090, 1, "CN-51", "51", "Sichuan", @"四川", 30.651652, 104.075931, 34.313000, 97.348081, 26.045865, 108.546712));
				_cache.Add(new GeoRegionData(156, 2091, 1, "CN-52", "52", "Guizhou", @"贵州", 26.598194, 106.707410, 29.221275, 103.601438, 24.619919, 109.597755));
				_cache.Add(new GeoRegionData(156, 2092, 1, "CN-53", "53", "Yunnan", @"云南", 25.045350, 102.709938, 29.223327, 97.527978, 21.141769, 106.197722));
				_cache.Add(new GeoRegionData(156, 2093, 1, "CN-54", "54", "Tibet", @"西藏", 29.646923, 91.117212, 36.483334, 78.395544, 26.854815, 99.116241));
				_cache.Add(new GeoRegionData(156, 2094, 1, "CN-61", "61", "Shaanxi", @"陕西", 34.265472, 108.954239, 39.587010, 105.491949, 31.705059, 111.248054));
				_cache.Add(new GeoRegionData(156, 2095, 1, "CN-62", "62", "Gansu", @"甘肃", 36.059421, 103.826308, 42.795162, 92.339146, 32.594110, 108.712336));
				_cache.Add(new GeoRegionData(156, 2096, 1, "CN-63", "63", "Qinghai", @"青海", 36.620901, 101.780199, 39.208344, 89.404424, 31.598662, 103.070970));
				_cache.Add(new GeoRegionData(156, 2097, 1, "CN-64", "64", "Ningxia", @"宁夏", 38.471318, 106.258754, 39.381569, 104.287039, 35.238303, 107.659274));
				_cache.Add(new GeoRegionData(156, 2098, 1, "CN-65", "65", "Xinjiang", @"新疆", 43.793028, 87.627812, 49.182341, 73.502355, 34.334503, 96.386194));
				_cache.Add(new GeoRegionData(156, 2099, 1, "CN-91", "91", "Hong Kong", @"香港", 22.396428, 114.109497, 22.561968, 113.835079, 22.153388, 114.406947));
				_cache.Add(new GeoRegionData(156, 2100, 1, "CN-92", "92", "Macau", @"澳門", 22.198745, 113.543873, 22.217063, 113.527605, 22.109771, 113.598279));
				#endregion

				#region Germany
				_cache.Add(new GeoRegionData(276, 2101, 1, "DE-BB", "BB", "Brandenburg", @"Brandenburg", 52.412528, 12.531644, 52.541776, 12.361080, 52.311578, 12.725997));
				_cache.Add(new GeoRegionData(276, 2102, 1, "DE-BE", "BE", "Berlin", @"Berlin", 52.519171, 13.406091, 52.675454, 13.091166, 52.339629, 13.761117));
				_cache.Add(new GeoRegionData(276, 2103, 1, "DE-BW", "BW", "Baden-Württemberg", @"NULL", 48.661603, 9.350133, 49.791327, 7.511756, 47.532366, 10.495573));
				_cache.Add(new GeoRegionData(276, 2104, 1, "DE-BY", "BY", "Bavaria", @"NULL", 48.790447, 11.497889, 50.564714, 8.976349, 47.270111, 13.839637));
				_cache.Add(new GeoRegionData(276, 2105, 1, "DE-HB", "HB", "Bremen", @"Bremen", 53.079296, 8.801693, 53.228910, 8.481758, 53.011698, 8.990813));
				_cache.Add(new GeoRegionData(276, 2106, 1, "DE-HE", "HE", "Hesse", @"NULL", 50.652051, 9.162437, 51.657506, 7.772467, 49.395261, 10.236320));
				_cache.Add(new GeoRegionData(276, 2107, 1, "DE-HH", "HH", "Hamburg", @"Hamburg", 53.551084, 9.993681, 53.717145, 9.732151, 53.399999, 10.123492));
				_cache.Add(new GeoRegionData(276, 2108, 1, "DE-MV", "MV", "Mecklenburg-Western Pomerania", @"NULL", 53.612650, 12.429595, 54.684690, 10.593613, 53.110319, 14.412257));
				_cache.Add(new GeoRegionData(276, 2109, 1, "DE-NI", "NI", "Lower Saxony", @"NULL", 52.636703, 9.845076, 53.892248, 6.653897, 51.295067, 11.598205));
				_cache.Add(new GeoRegionData(276, 2110, 1, "DE-NW", "NW", "North Rhine-Westphalia", @"NULL", 51.433236, 7.661593, 52.531469, 5.866342, 50.322701, 9.461634));
				_cache.Add(new GeoRegionData(276, 2111, 1, "DE-RP", "RP", "Rhineland-Palatinate", @"NULL", 50.118346, 7.308952, 50.942305, 6.112265, 48.966418, 8.508313));
				_cache.Add(new GeoRegionData(276, 2112, 1, "DE-SH", "SH", "Schleswig-Holstein", @"NULL", 54.219367, 9.696116, 55.058347, 7.864961, 53.359806, 11.312920));
				_cache.Add(new GeoRegionData(276, 2113, 1, "DE-SL", "SL", "Saarland", @"Saarland", 49.396423, 7.022960, 49.639408, 6.357608, 49.111945, 7.404583));
				_cache.Add(new GeoRegionData(276, 2114, 1, "DE-SN", "SN", "Saxony", @"NULL", 51.104540, 13.201738, 51.684871, 11.871435, 50.171363, 15.041896));
				_cache.Add(new GeoRegionData(276, 2115, 1, "DE-ST", "ST", "Saxony", @"Sachsen-Anhalt", 51.104540, 13.201738, 51.684871, 11.871435, 50.171363, 15.041896));
				_cache.Add(new GeoRegionData(276, 2116, 1, "DE-TH", "TH", "Thuringia", @"NULL", 51.010989, 10.845346, 51.648935, 9.876984, 50.204346, 12.653932));
				#endregion

				#region Spain
				_cache.Add(new GeoRegionData(724, 2117, 2, "ES-A", "A", "Alicante", @"", 38.345996, -0.490685, 38.390932, -0.541629, 38.324748, -0.403419));
				_cache.Add(new GeoRegionData(724, 2118, 2, "ES-AB", "AB", "Albacete", @"", 38.994349, -1.858542, 39.012915, -1.886279, 38.971482, -1.834899));
				_cache.Add(new GeoRegionData(724, 2119, 2, "ES-AL", "AL", "Almería", @"", 36.834047, -2.463713, 36.865853, -2.485544, 36.817203, -2.428688));
				_cache.Add(new GeoRegionData(724, 2120, 2, "ES-AV", "AV", "Ávila", @"", 40.662594, -4.695817, 40.675949, -4.707720, 40.637822, -4.656667));
				_cache.Add(new GeoRegionData(724, 2121, 2, "ES-B", "B", "Barcelona", @"", 41.385063, 2.173403, 41.469576, 2.069525, 41.320004, 2.228009));
				_cache.Add(new GeoRegionData(724, 2122, 2, "ES-BA", "BA", "Badajoz", @"", 38.880138, -6.970166, 38.906992, -7.019638, 38.851767, -6.940695));
				_cache.Add(new GeoRegionData(724, 2123, 2, "ES-BI", "BI", "Vizcaya", @"", 43.220428, -2.698386, 43.457242, -3.449275, 42.981877, -2.412715));
				_cache.Add(new GeoRegionData(724, 2124, 2, "ES-BU", "BU", "Burgos", @"", 42.343992, -3.696906, 42.372134, -3.752635, 42.316117, -3.636750));
				_cache.Add(new GeoRegionData(724, 2125, 2, "ES-C", "C", "La Coruña", @"", 43.362343, -8.411540, 43.386407, -8.438259, 43.337341, -8.387108));
				_cache.Add(new GeoRegionData(724, 2126, 2, "ES-CA", "CA", "Cádiz", @"", 36.527061, -6.288596, 36.543206, -6.309599, 36.489566, -6.255334));
				_cache.Add(new GeoRegionData(724, 2127, 2, "ES-CC", "CC", "Cáceres", @"", 39.471329, -6.370961, 39.500104, -6.427048, 39.443169, -6.354389));
				_cache.Add(new GeoRegionData(724, 2128, 1, "ES-CE", "CE", "Ceuta", @"", 35.888383, -5.324635, 35.907028, -5.358712, 35.871060, -5.278339));
				_cache.Add(new GeoRegionData(724, 2129, 2, "ES-CO", "CO", "Córdoba", @"", 37.888175, -4.779383, 37.927278, -4.822793, 37.855893, -4.746176));
				_cache.Add(new GeoRegionData(724, 2130, 2, "ES-CR", "CR", "Ciudad Real", @"", 38.984829, -3.927377, 39.006216, -3.953380, 38.965870, -3.900595));
				_cache.Add(new GeoRegionData(724, 2131, 2, "ES-CS", "CS", "Castellón", @"", 39.984457, -0.044949, 40.004172, -0.079756, 39.970746, -0.016369));
				_cache.Add(new GeoRegionData(724, 2132, 2, "ES-CU", "CU", "Cuenca", @"", 40.070531, -2.136719, 40.083813, -2.180204, 40.040131, -2.115845));
				_cache.Add(new GeoRegionData(724, 2133, 2, "ES-GC", "GC", "Las Palmas", @"", 28.113154, -15.440883, 28.156340, -15.467486, 28.078381, -15.411748));
				_cache.Add(new GeoRegionData(724, 2134, 2, "ES-GI", "GI", "Girona", @"", 41.979400, 2.821426, 42.014208, 2.798378, 41.946296, 2.838956));
				_cache.Add(new GeoRegionData(724, 2135, 2, "ES-GR", "GR", "Granada", @"", 37.177336, -3.598557, 37.212464, -3.633835, 37.149427, -3.550571));
				_cache.Add(new GeoRegionData(724, 2136, 2, "ES-GU", "GU", "Guadalajara", @"", 20.673590, -103.343803, 20.743846, -103.407064, 20.603737, -103.263761));
				_cache.Add(new GeoRegionData(724, 2137, 2, "ES-H", "H", "Huelva", @"", 37.261421, -6.944722, 37.291320, -6.962591, 37.250415, -6.916788));
				_cache.Add(new GeoRegionData(724, 2138, 2, "ES-HU", "HU", "Huesca", @"", 42.137341, -0.410621, 42.146830, -0.423103, 42.124982, -0.388593));
				_cache.Add(new GeoRegionData(724, 2139, 2, "ES-J", "J", "Jaén", @"", 37.767826, -3.790845, 37.804088, -3.820125, 37.756100, -3.773604));
				_cache.Add(new GeoRegionData(724, 2140, 2, "ES-L", "L", "Lleida", @"", 41.617589, 0.620014, 41.639648, 0.590112, 41.597036, 0.649757));
				_cache.Add(new GeoRegionData(724, 2141, 2, "ES-LE", "LE", "León", @"", 21.129201, -101.672675, 21.213052, -101.766419, 21.030335, -101.561810));
				_cache.Add(new GeoRegionData(724, 2142, 2, "ES-LO", "LO", "La Rioja", @"", -29.412800, -66.855980, -29.379902, -66.925365, -29.456885, -66.789069));
				_cache.Add(new GeoRegionData(724, 2143, 2, "ES-LU", "LU", "Lugo", @"", 43.009738, -7.556758, 43.034798, -7.576188, 42.989768, -7.536812));
				_cache.Add(new GeoRegionData(724, 2144, 2, "ES-M", "M", "Madrid", @"", 40.416775, -3.703790, 40.563590, -3.834161, 40.312063, -3.524911));
				_cache.Add(new GeoRegionData(724, 2145, 2, "ES-MA", "MA", "Málaga", @"", 36.721261, -4.421265, 36.757552, -4.559037, 36.678891, -4.339496));
				_cache.Add(new GeoRegionData(724, 2146, 1, "ES-ML", "ML", "Melilla", @"", 35.292277, -2.938097, 35.303092, -2.965793, 35.269450, -2.923259));
				_cache.Add(new GeoRegionData(724, 2147, 2, "ES-MU", "MU", "Murcia", @"", 37.992331, -1.130457, 38.012258, -1.153380, 37.964976, -1.106699));
				_cache.Add(new GeoRegionData(724, 2148, 2, "ES-NA", "NA", "Navarra", @"", 42.695390, -1.676069, 43.314792, -2.500082, 41.909893, -0.723950));
				_cache.Add(new GeoRegionData(724, 2149, 2, "ES-O", "O", "Asturias", @"", 43.250439, -5.983257, 43.666532, -7.182488, 42.882542, -4.510594));
				_cache.Add(new GeoRegionData(724, 2150, 2, "ES-OR", "OR", "Ourense", @"", 42.335789, -7.863881, 42.358294, -7.880299, 42.318431, -7.847072));
				_cache.Add(new GeoRegionData(724, 2151, 2, "ES-P", "P", "Palencia", @"", 42.011121, -4.532032, 42.028919, -4.548122, 41.988712, -4.505785));
				_cache.Add(new GeoRegionData(724, 2152, 2, "ES-PM", "PM", "Islas Baleares", @"", 39.500000, 3.000000, 39.508609, 2.983992, 39.491389, 3.016007));
				_cache.Add(new GeoRegionData(724, 2153, 2, "ES-PO", "PO", "Pontevedra", @"", 42.429884, -8.644620, 42.451766, -8.664857, 42.415954, -8.613833));
				_cache.Add(new GeoRegionData(724, 2154, 2, "ES-S", "S", "Cantabria", @"", 43.182839, -3.987842, 43.513692, -4.851778, 42.758049, -3.149652));
				_cache.Add(new GeoRegionData(724, 2155, 2, "ES-SA", "SA", "Salamanca", @"", 40.970103, -5.663539, 40.985097, -5.707220, 40.941779, -5.631203));
				_cache.Add(new GeoRegionData(724, 2156, 2, "ES-SE", "SE", "Sevilla", @"", 37.388096, -5.982329, 37.435521, -6.021657, 37.315220, -5.888458));
				_cache.Add(new GeoRegionData(724, 2157, 2, "ES-SG", "SG", "Segovia", @"", 40.943562, -4.113002, 40.962501, -4.133586, 40.919998, -4.090817));
				_cache.Add(new GeoRegionData(724, 2158, 2, "ES-SO", "SO", "Soria", @"", 41.764430, -2.463772, 41.782282, -2.493806, 41.751960, -2.455352));
				_cache.Add(new GeoRegionData(724, 2159, 2, "ES-SS", "SS", "Guipúzcoa", @"", 43.075629, -2.223666, 43.395679, -2.602683, 42.895245, -1.729343));
				_cache.Add(new GeoRegionData(724, 2160, 2, "ES-T", "T", "Tarragona", @"", 41.119019, 1.245211, 41.135916, 1.187316, 41.109661, 1.289850));
				_cache.Add(new GeoRegionData(724, 2161, 2, "ES-TE", "TE", "Teruel", @"", 40.341839, -1.103675, 40.364295, -1.120992, 40.325724, -1.080937));
				_cache.Add(new GeoRegionData(724, 2162, 2, "ES-TF", "TF", "Santa Cruz de Tenerife", @"", 28.463629, -16.251846, 28.487616, -16.337004, 28.428024, -16.235664));
				_cache.Add(new GeoRegionData(724, 2163, 2, "ES-TO", "TO", "Toledo", @"", 41.663938, -83.555212, 41.732844, -83.694237, 41.580266, -83.454719));
				_cache.Add(new GeoRegionData(724, 2164, 2, "ES-V", "V", "Valencia", @"", 39.469907, -0.376288, 39.507322, -0.431544, 39.308248, -0.291477));
				_cache.Add(new GeoRegionData(724, 2165, 2, "ES-VA", "VA", "Valladolid", @"", 41.652251, -4.724532, 41.678095, -4.775489, 41.604284, -4.689443));
				_cache.Add(new GeoRegionData(724, 2166, 2, "ES-VI", "VI", "Álava", @"", 42.909998, -2.698386, 43.216969, -3.286766, 42.472255, -2.232687));
				_cache.Add(new GeoRegionData(724, 2167, 2, "ES-Z", "Z", "Zaragoza", @"", 41.648790, -0.889581, 41.689407, -0.947230, 41.613974, -0.842731));
				_cache.Add(new GeoRegionData(724, 2168, 2, "ES-ZA", "ZA", "Zamora", @"", 41.507326, -5.745313, 41.522601, -5.768415, 41.485018, -5.720656));
				#endregion

				#region France
				_cache.Add(new GeoRegionData(250, 2169, 2, "FR-01", "1", "Ain", @"", 70.637812, -160.000041, 70.641578, -160.023801, 70.634034, -159.973813));
				_cache.Add(new GeoRegionData(250, 2170, 2, "FR-02", "2", "Aisne", @"", 49.476919, 3.441736, 50.069495, 2.958276, 48.837212, 4.255678));
				_cache.Add(new GeoRegionData(250, 2171, 2, "FR-03", "3", "Allier", @"", 46.311555, 3.416765, 46.804293, 2.276795, 45.930732, 4.005739));
				_cache.Add(new GeoRegionData(250, 2172, 2, "FR-04", "4", "Alpes-de-Haute-Provence", @"", 44.077871, 6.237594, 44.659998, 5.496793, 43.668325, 6.969039));
				_cache.Add(new GeoRegionData(250, 2173, 2, "FR-05", "5", "Hautes-Alpes", @"", 44.600872, 6.322607, 45.126851, 5.418363, 44.186442, 7.077155));
				_cache.Add(new GeoRegionData(250, 2174, 2, "FR-06", "6", "Alpes-Maritimes", @"", 43.946679, 7.179026, 44.361154, 6.635411, 43.480302, 7.718993));
				_cache.Add(new GeoRegionData(250, 2175, 2, "FR-07", "7", "Ardèche", @"", 44.759629, 4.562442, 45.366200, 3.861100, 44.264341, 4.886470));
				_cache.Add(new GeoRegionData(250, 2176, 2, "FR-08", "8", "Ardennes", @"", 50.028871, 5.410097, 50.606108, 4.366665, 49.535765, 6.272531));
				_cache.Add(new GeoRegionData(250, 2177, 2, "FR-09", "9", "Ariège", @"", 42.932629, 1.443469, 43.316222, 0.825994, 42.571489, 2.175846));
				_cache.Add(new GeoRegionData(250, 2178, 2, "FR-10", "10", "Aube", @"", 48.156341, 4.373246, 48.716736, 3.383647, 47.923696, 4.864605));
				_cache.Add(new GeoRegionData(250, 2179, 2, "FR-11", "11", "Aude", @"", 43.072466, 2.381362, 43.460066, 1.688426, 42.648912, 3.240139));
				_cache.Add(new GeoRegionData(250, 2180, 2, "FR-12", "12", "Aveyron", @"", 44.217974, 2.618927, 44.941441, 1.839313, 43.690619, 3.451754));
				_cache.Add(new GeoRegionData(250, 2181, 2, "FR-13", "13", "Bouches-du-Rhône", @"", 43.591167, 5.310250, 43.924136, 4.230207, 43.157405, 5.813430));
				_cache.Add(new GeoRegionData(250, 2182, 2, "FR-14", "14", "Calvados", @"", 49.121331, -0.433057, 49.429918, -1.159777, 48.751681, 0.446476));
				_cache.Add(new GeoRegionData(250, 2183, 2, "FR-15", "15", "Cantal", @"", 45.119199, 2.632606, 45.483472, 2.062882, 44.615775, 3.371465));
				_cache.Add(new GeoRegionData(250, 2184, 2, "FR-16", "16", "Charente", @"", 45.751995, 0.153476, 46.140851, -0.463103, 45.191681, 0.947124));
				_cache.Add(new GeoRegionData(250, 2185, 2, "FR-17", "17", "Charente-Maritime", @"", 45.749490, -0.773318, 46.371485, -1.562668, 45.088749, 0.005972));
				_cache.Add(new GeoRegionData(250, 2186, 2, "FR-18", "18", "Cher", @"", 46.954005, 2.467190, 47.629116, 1.773677, 46.420476, 3.079745));
				_cache.Add(new GeoRegionData(250, 2187, 2, "FR-19", "19", "Corrèze", @"", 45.372114, 1.873739, 45.409959, 1.822327, 45.325842, 1.921707));
				_cache.Add(new GeoRegionData(250, 2188, 2, "FR-21", "21", "Côte-d'Or", @"", 47.512679, 4.635412, 48.031310, 4.065190, 46.899847, 5.518767));
				_cache.Add(new GeoRegionData(250, 2189, 2, "FR-22", "22", "Côtes-d'Armor", @"", 48.510810, -3.326367, 48.900936, -3.665906, 48.032568, -1.909064));
				_cache.Add(new GeoRegionData(250, 2190, 2, "FR-23", "23", "Creuse", @"", 46.037763, 2.062783, 46.455370, 1.372604, 45.663551, 2.611293));
				_cache.Add(new GeoRegionData(250, 2191, 2, "FR-24", "24", "Dordogne", @"", 45.146948, 0.757220, 45.714768, -0.041877, 44.570736, 1.448245));
				_cache.Add(new GeoRegionData(250, 2192, 2, "FR-25", "25", "Doubs", @"", 46.927600, 6.349001, 46.948530, 6.330692, 46.915786, 6.399583));
				_cache.Add(new GeoRegionData(250, 2193, 2, "FR-26", "26", "Drôme", @"", 44.731189, 5.226667, 45.343976, 4.646861, 44.115494, 5.830446));
				_cache.Add(new GeoRegionData(250, 2194, 2, "FR-27", "27", "Eure", @"", 49.118176, 0.958211, 49.482147, 0.296725, 48.666427, 1.803110));
				_cache.Add(new GeoRegionData(250, 2195, 2, "FR-28", "28", "Eure-et-Loir", @"", 48.552524, 1.198981, 48.941029, 0.755676, 47.953818, 1.994560));
				_cache.Add(new GeoRegionData(250, 2196, 2, "FR-29", "29", "Finistère", @"", 48.252024, -3.930052, 48.753500, -5.141256, 47.701296, -3.386618));
				_cache.Add(new GeoRegionData(250, 2197, 2, "FR-2A", "2A", "Corse-du-Sud", @"", 41.810263, 8.924534, 42.381519, 8.534677, 41.333570, 9.408043));
				_cache.Add(new GeoRegionData(250, 2198, 2, "FR-2B", "2B", "Haute-Corse", @"", 42.409787, 9.278558, 43.027678, 8.573306, 41.832168, 9.560067));
				_cache.Add(new GeoRegionData(250, 2199, 2, "FR-30", "30", "Gard", @"", 43.944699, 4.151376, 44.459663, 3.261869, 43.460159, 4.845564));
				_cache.Add(new GeoRegionData(250, 2200, 2, "FR-31", "31", "Haute-Garonne", @"", 43.401046, 1.135302, 43.921531, 0.441686, 42.689329, 2.048299));
				_cache.Add(new GeoRegionData(250, 2201, 2, "FR-32", "32", "Gers", @"", 43.636647, 0.450236, 44.080024, -0.282299, 43.310868, 1.203249));
				_cache.Add(new GeoRegionData(250, 2202, 2, "FR-33", "33", "Gironde", @"", 44.849665, -0.450236, 45.571979, -1.261424, 44.193901, 0.315137));
				_cache.Add(new GeoRegionData(250, 2203, 2, "FR-34", "34", "Hérault", @"", 43.591235, 3.258362, 43.972759, 2.539549, 43.210209, 4.194540));
				_cache.Add(new GeoRegionData(250, 2204, 2, "FR-35", "35", "Ille-et-Vilaine", @"", 48.229201, -1.530069, 48.721737, -2.289611, 47.631614, -1.015621));
				_cache.Add(new GeoRegionData(250, 2205, 2, "FR-36", "36", "Indre", @"", 46.661396, 1.448266, 47.277465, 0.867413, 46.346905, 2.204572));
				_cache.Add(new GeoRegionData(250, 2206, 2, "FR-37", "37", "Indre-et-Loire", @"", 47.289492, 0.816097, 47.709867, 0.052736, 46.736714, 1.366049));
				_cache.Add(new GeoRegionData(250, 2207, 2, "FR-38", "38", "Isère", @"", 44.995774, 5.929347, 45.883397, 4.742593, 44.695873, 6.359309));
				_cache.Add(new GeoRegionData(250, 2208, 2, "FR-39", "39", "Jura", @"", 46.762475, 5.672915, 47.305943, 5.251316, 46.260695, 6.207189));
				_cache.Add(new GeoRegionData(250, 2209, 2, "FR-40", "40", "Landes", @"", 43.941204, -0.753280, 44.532381, -1.523574, 43.487430, 0.136691));
				_cache.Add(new GeoRegionData(250, 2210, 2, "FR-41", "41", "Loir-et-Cher", @"", 47.676190, 1.415907, 48.133235, 0.580486, 47.186391, 2.247891));
				_cache.Add(new GeoRegionData(250, 2211, 2, "FR-42", "42", "Loire", @"", 45.984647, 4.052545, 46.276589, 3.688893, 45.231040, 4.760375));
				_cache.Add(new GeoRegionData(250, 2212, 2, "FR-43", "43", "Haute-Loire", @"", 45.082122, 3.926636, 45.427608, 3.082197, 44.743961, 4.490819));
				_cache.Add(new GeoRegionData(250, 2213, 2, "FR-44", "44", "Loire-Atlantique", @"", 47.278046, -1.815764, 47.835927, -2.559609, 46.860073, -0.923216));
				_cache.Add(new GeoRegionData(250, 2214, 2, "FR-45", "45", "Loiret", @"", 47.900771, 2.201817, 48.344954, 1.511450, 47.483027, 3.128409));
				_cache.Add(new GeoRegionData(250, 2215, 2, "FR-46", "46", "Lot", @"", 37.545600, -77.430143, 37.554447, -77.446150, 37.536753, -77.414135));
				_cache.Add(new GeoRegionData(250, 2216, 2, "FR-47", "47", "Lot-et-Garonne", @"", 44.247017, 0.450236, 44.765678, -0.140673, 43.972587, 1.078341));
				_cache.Add(new GeoRegionData(250, 2217, 2, "FR-48", "48", "Lozère", @"", 44.494203, 3.581269, 44.975761, 2.981197, 44.109590, 3.998366));
				_cache.Add(new GeoRegionData(250, 2218, 2, "FR-49", "49", "Maine-et-Loire", @"", 47.291354, -0.487785, 47.809978, -1.354187, 46.968882, 0.234549));
				_cache.Add(new GeoRegionData(250, 2219, 2, "FR-50", "50", "Manche", @"", 50.134664, -0.357056, 51.156164, -5.677063, 48.485980, 1.926622));
				_cache.Add(new GeoRegionData(250, 2220, 2, "FR-51", "51", "Marne", @"", 49.128754, 4.147544, 49.407828, 3.395865, 48.515251, 5.039668));
				_cache.Add(new GeoRegionData(250, 2221, 2, "FR-52", "52", "Haute-Marne", @"", 48.126096, 5.107132, 48.689322, 4.626602, 47.576566, 5.891043));
				_cache.Add(new GeoRegionData(250, 2222, 2, "FR-53", "53", "Mayenne", @"", 48.306123, -0.620935, 48.335396, -0.650275, 48.277843, -0.577747));
				_cache.Add(new GeoRegionData(250, 2223, 2, "FR-54", "54", "Meurthe-et-Moselle", @"", 48.799700, 6.094701, 49.563268, 5.426108, 48.348987, 7.123213));
				_cache.Add(new GeoRegionData(250, 2224, 2, "FR-55", "55", "Meuse", @"", 49.082431, 5.282399, 49.616864, 4.888376, 48.408991, 5.854205));
				_cache.Add(new GeoRegionData(250, 2225, 2, "FR-56", "56", "Morbihan", @"", 47.885292, -2.900186, 48.210889, -3.734914, 47.277863, -2.035338));
				_cache.Add(new GeoRegionData(250, 2226, 2, "FR-57", "57", "Moselle", @"", 49.098383, 6.552764, 49.515124, 5.891857, 48.526722, 7.640046));
				_cache.Add(new GeoRegionData(250, 2227, 2, "FR-58", "58", "Nièvre", @"", 47.238170, 3.529452, 47.588288, 2.844890, 46.651024, 4.231910));
				_cache.Add(new GeoRegionData(250, 2228, 2, "FR-59", "59", "Nord", @"", 18.444896, 30.158930, 22.176528, 25.000000, 16.511393, 32.637459));
				_cache.Add(new GeoRegionData(250, 2229, 2, "FR-60", "60", "Oise", @"", 49.421456, 2.414639, 49.763922, 1.688865, 49.060525, 3.166125));
				_cache.Add(new GeoRegionData(250, 2230, 2, "FR-61", "61", "Orne", @"", 48.638856, 0.084820, 48.972535, -0.860575, 48.179885, 0.976576));
				_cache.Add(new GeoRegionData(250, 2231, 2, "FR-62", "62", "Pas-de-Calais", @"", 50.573276, 2.324467, 51.006774, 1.555598, 50.019760, 3.188186));
				_cache.Add(new GeoRegionData(250, 2232, 2, "FR-63", "63", "Puy-de-Dôme", @"", 45.772466, 2.964578, 45.780248, 2.948570, 45.764682, 2.980585));
				_cache.Add(new GeoRegionData(250, 2233, 2, "FR-64", "64", "Pyrénées-Atlantiques", @"", 43.326994, -0.753280, 43.596724, -1.792325, 42.777531, 0.029807));
				_cache.Add(new GeoRegionData(250, 2234, 2, "FR-65", "65", "Hautes-Pyrénées", @"", 43.019392, 0.149498, 43.613334, -0.327160, 42.673305, 0.646119));
				_cache.Add(new GeoRegionData(250, 2235, 2, "FR-66", "66", "Pyrénées-Orientales", @"", 42.601291, 2.539603, 42.918539, 1.721635, 42.333014, 3.177833));
				_cache.Add(new GeoRegionData(250, 2236, 2, "FR-67", "67", "Bas-Rhin", @"", 48.634317, 7.525293, 49.077858, 6.940613, 48.120387, 8.233549));
				_cache.Add(new GeoRegionData(250, 2237, 2, "FR-68", "68", "Haut-Rhin", @"", 47.931504, 7.244109, 48.311198, 6.841026, 47.420261, 7.622121));
				_cache.Add(new GeoRegionData(250, 2238, 2, "FR-69", "69", "Rhône", @"", 45.735145, 4.610804, 46.306502, 4.243647, 45.454130, 5.160108));
				_cache.Add(new GeoRegionData(250, 2239, 2, "FR-70", "70", "Haute-Saône", @"", 47.756980, 6.155628, 48.024154, 5.366937, 47.252553, 6.824946));
				_cache.Add(new GeoRegionData(250, 2240, 2, "FR-71", "71", "Saône-et-Loire", @"", 46.582751, 4.486671, 47.155772, 3.622593, 46.156070, 5.465289));
				_cache.Add(new GeoRegionData(250, 2241, 2, "FR-72", "72", "Sarthe", @"", 47.921701, 0.165580, 48.485020, -0.448063, 47.568401, 0.916638));
				_cache.Add(new GeoRegionData(250, 2242, 2, "FR-73", "73", "Savoie", @"", 44.771079, 5.742806, 44.778999, 5.726798, 44.763157, 5.758813));
				_cache.Add(new GeoRegionData(250, 2243, 2, "FR-74", "74", "Haute-Savoie", @"", 46.175678, 6.538962, 46.408243, 5.805020, 45.681659, 7.045064));
				_cache.Add(new GeoRegionData(250, 2244, 2, "FR-75", "75", "Paris", @"", 48.856614, 2.352221, 48.902144, 2.224199, 48.815573, 2.469920));
				_cache.Add(new GeoRegionData(250, 2245, 2, "FR-76", "76", "Seine-Maritime", @"", 49.605418, 0.974843, 50.072097, 0.065575, 49.250643, 1.790588));
				_cache.Add(new GeoRegionData(250, 2246, 2, "FR-77", "77", "Seine-et-Marne", @"", 48.841082, 2.999366, 49.117897, 2.392326, 48.120081, 3.559006));
				_cache.Add(new GeoRegionData(250, 2247, 2, "FR-78", "78", "Yvelines", @"", 48.785093, 1.825657, 49.085448, 1.446170, 48.438556, 2.229126));
				_cache.Add(new GeoRegionData(250, 2248, 2, "FR-79", "79", "Deux-Sèvres", @"", 46.592654, -0.396284, 47.108548, -0.903680, 45.969669, 0.220405));
				_cache.Add(new GeoRegionData(250, 2249, 2, "FR-80", "80", "Somme", @"", 49.914518, 2.270709, 50.366321, 1.379662, 49.571623, 3.203045));
				_cache.Add(new GeoRegionData(250, 2250, 2, "FR-81", "81", "Tarn", @"", 43.926440, 1.988152, 44.201493, 1.535198, 43.382281, 2.937474));
				_cache.Add(new GeoRegionData(250, 2251, 2, "FR-82", "82", "Tarn-et-Garonne", @"", 44.012667, 1.289103, 44.393924, 0.737811, 43.767590, 2.000898));
				_cache.Add(new GeoRegionData(250, 2252, 2, "FR-83", "83", "Var", @"", 43.467646, 6.237594, 43.808881, 5.655900, 42.981998, 6.933446));
				_cache.Add(new GeoRegionData(250, 2253, 2, "FR-84", "84", "Vaucluse", @"", 44.056505, 5.143206, 44.431565, 4.649082, 43.658718, 5.757335));
				_cache.Add(new GeoRegionData(250, 2254, 2, "FR-85", "85", "Vendée", @"", 46.661396, -1.448266, 47.085008, -2.399889, 46.266539, -0.538134));
				_cache.Add(new GeoRegionData(250, 2255, 2, "FR-86", "86", "Vienne", @"", 48.208174, 16.373818, 48.323099, 16.182619, 48.118269, 16.577499));
				_cache.Add(new GeoRegionData(250, 2256, 2, "FR-87", "87", "Haute-Vienne", @"", 45.743517, 1.402548, 46.401586, 0.629250, 45.436630, 1.911078));
				_cache.Add(new GeoRegionData(250, 2257, 2, "FR-88", "88", "Vosges", @"", 48.144642, 6.335593, 48.513663, 5.393626, 47.813298, 7.198364));
				_cache.Add(new GeoRegionData(250, 2258, 2, "FR-89", "89", "Yonne", @"", 47.865272, 3.607982, 48.400061, 2.848432, 47.310363, 4.340296));
				_cache.Add(new GeoRegionData(250, 2259, 2, "FR-90", "90", "Territoire de Belfort", @"", 47.594657, 6.920771, 47.825113, 6.756256, 47.433383, 7.143381));
				_cache.Add(new GeoRegionData(250, 2260, 2, "FR-91", "91", "Essonne", @"", 48.458569, 2.156941, 48.776131, 1.914513, 48.284556, 2.585633));
				_cache.Add(new GeoRegionData(250, 2261, 2, "FR-92", "92", "Hauts-de-Seine", @"", 48.828508, 2.218806, 48.950961, 2.145702, 48.729351, 2.336941));
				_cache.Add(new GeoRegionData(250, 2262, 2, "FR-93", "93", "Seine-Saint-Denis", @"", 48.913745, 2.484572, 49.012329, 2.288310, 48.807248, 2.603291));
				_cache.Add(new GeoRegionData(250, 2263, 2, "FR-94", "94", "Val-de-Marne", @"", 48.793142, 2.474033, 48.861484, 2.308675, 48.687643, 2.615641));
				_cache.Add(new GeoRegionData(250, 2264, 2, "FR-95", "95", "Val-d'Oise", @"", 49.061590, 2.158135, 49.241504, 1.608733, 48.908674, 2.594979));
				_cache.Add(new GeoRegionData(250, 2265, 2, "FR-BL", "BL", "Saint-Barthélemy", @"", 17.900000, -62.833333, 17.957015, -62.878489, 17.870828, -62.789214));
				_cache.Add(new GeoRegionData(250, 2266, 2, "FR-GF", "GF", "Guyane", @"", 3.933889, -53.125782, 5.757189, -54.554437, 2.109287, -51.633596));
				_cache.Add(new GeoRegionData(250, 2267, 2, "FR-GP", "GP", "Guadeloupe", @"", 16.265000, -61.551000, 16.514251, -61.809081, 15.831938, -61.001672));
				_cache.Add(new GeoRegionData(250, 2268, 2, "FR-MF", "MF", "Saint-Martin", @"", 18.082550, -63.052251, 18.125133, -63.153326, 18.046575, -62.970392));
				_cache.Add(new GeoRegionData(250, 2269, 2, "FR-MQ", "MQ", "Martinique", @"", 14.641528, -61.024174, 14.878716, -61.229093, 14.388647, -60.810527));
				_cache.Add(new GeoRegionData(250, 2270, 2, "FR-NC", "NC", "Nouvelle-Calédonie", @"", -20.904305, 165.618042, -19.539508, 163.569721, -22.881947, 168.133681));
				_cache.Add(new GeoRegionData(250, 2271, 2, "FR-PF", "PF", "Polynésie Française", @"", -17.679742, -149.406843, -17.494411, -149.620887, -17.880432, -149.125156));
				_cache.Add(new GeoRegionData(250, 2272, 2, "FR-PM", "PM", "Saint-Pierre et Miquelon", @"", 46.885200, -56.315900, 47.144270, -56.405632, 46.749105, -56.118937));
				_cache.Add(new GeoRegionData(250, 2273, 2, "FR-RE", "RE", "La Réunion", @"", -21.115141, 55.536384, -20.871755, 55.216405, -21.389622, 55.836553));
				_cache.Add(new GeoRegionData(250, 2274, 2, "FR-TF", "TF", "French Southern and Antarctic Lands", @"", -49.280366, 69.348557, -48.449741, 68.609018, -49.733917, 70.556602));
				_cache.Add(new GeoRegionData(250, 2275, 2, "FR-WF", "WF", "Wallis et Futuna", @"", -14.293800, -178.116500, -14.236552, -178.186608, -14.362124, -177.992307));
				_cache.Add(new GeoRegionData(250, 2276, 2, "FR-YT", "YT", "Mayotte", @"", -12.827500, 45.166244, -12.636411, 45.018170, -13.006161, 45.300028));
				#endregion

				#region United Kingdom
				_cache.Add(new GeoRegionData(826, 2277, 2, "GB-ABD", "ABD", "Aberdeenshire", @"", 57.162142, -2.719416, 57.701204, -3.801648, 56.747185, -1.764452));
				_cache.Add(new GeoRegionData(826, 2278, 2, "GB-ABE", "ABE", "Aberdeen City", @"", 57.149717, -2.094278, 57.195650, -2.205892, 57.104151, -2.046181));
				_cache.Add(new GeoRegionData(826, 2279, 2, "GB-AGB", "AGB", "Argyll and Bute", @"", 56.370046, -5.031896, 56.704844, -7.112457, 55.274583, -4.559851));
				_cache.Add(new GeoRegionData(826, 2280, 2, "GB-AGY", "AGY", "Isle of Anglesey", @"", 53.269280, -4.321285, 53.435795, -4.700441, 53.126354, -4.019035));
				_cache.Add(new GeoRegionData(826, 2281, 2, "GB-ANS", "ANS", "Angus", @"", 56.796996, -2.920681, 56.986816, -3.407021, 56.463963, -2.426529));
				_cache.Add(new GeoRegionData(826, 2282, 3, "GB-ANT", "ANT", "Antrim", @"", 54.713630, -6.214280, 54.720075, -6.230287, 54.707183, -6.198272));
				_cache.Add(new GeoRegionData(826, 2283, 3, "GB-ARD", "ARD", "Ards", @"", 54.589964, -5.598497, 54.670089, -5.832820, 54.330950, -5.426790));
				_cache.Add(new GeoRegionData(826, 2284, 3, "GB-ARM", "ARM", "Armagh", @"", 54.348750, -6.651610, 54.355252, -6.667617, 54.342246, -6.635602));
				_cache.Add(new GeoRegionData(826, 2285, 3, "GB-BAS", "BAS", "Bath and North East Somerset", @"", 51.363629, -2.439998, 51.439535, -2.705954, 51.273101, -2.278543));
				_cache.Add(new GeoRegionData(826, 2286, 3, "GB-BBD", "BBD", "Blackburn with Darwen", @"", 53.689859, -2.467862, 53.781804, -2.564770, 53.616567, -2.362642));
				_cache.Add(new GeoRegionData(826, 2287, 3, "GB-BDF", "BDF", "Bedford", @"", 52.135972, -0.466654, 52.164402, -0.500009, 52.113782, -0.403153));
				_cache.Add(new GeoRegionData(826, 2288, 3, "GB-BDG", "BDG", "Barking and Dagenham", @"", 51.546482, 0.129349, 51.599436, 0.066648, 51.511380, 0.190189));
				_cache.Add(new GeoRegionData(826, 2289, 3, "GB-BEN", "BEN", "Brent", @"", 51.567280, -0.271056, 51.600370, -0.335584, 51.527654, -0.191483));
				_cache.Add(new GeoRegionData(826, 2290, 3, "GB-BEX", "BEX", "Bexley", @"", 51.439933, 0.154327, 51.449381, 0.113995, 51.423321, 0.177408));
				_cache.Add(new GeoRegionData(826, 2291, 3, "GB-BFS", "BFS", "Belfast City", @"", 54.597285, -5.930120, 54.659210, -6.045260, 54.530550, -5.808004));
				_cache.Add(new GeoRegionData(826, 2292, 2, "GB-BGE", "BGE", "Bridgend", @"", 51.504286, -3.576945, 51.533611, -3.671616, 51.482199, -3.525005));
				_cache.Add(new GeoRegionData(826, 2293, 2, "GB-BGW", "BGW", "Blaenau Gwent", @"", 51.787577, -3.204393, 51.825483, -3.310085, 51.681284, -3.106007));
				_cache.Add(new GeoRegionData(826, 2294, 3, "GB-BIR", "BIR", "Birmingham", @"", 33.520660, -86.802490, 33.678714, -87.122124, 33.383759, -86.578149));
				_cache.Add(new GeoRegionData(826, 2295, 2, "GB-BKM", "BKM", "Buckinghamshire", @"", 51.807220, -0.812766, 52.081521, -1.140696, 51.485481, -0.476616));
				_cache.Add(new GeoRegionData(826, 2296, 3, "GB-BLA", "BLA", "Ballymena", @"", 54.865380, -6.280380, 54.871800, -6.296387, 54.858958, -6.264372));
				_cache.Add(new GeoRegionData(826, 2297, 3, "GB-BLY", "BLY", "Ballymoney", @"", 55.072030, -6.516990, 55.078418, -6.532997, 55.065641, -6.500982));
				_cache.Add(new GeoRegionData(826, 2298, 3, "GB-BMH", "BMH", "Bournemouth", @"", 50.719164, -1.880769, 50.798891, -1.978819, 50.709255, -1.740733));
				_cache.Add(new GeoRegionData(826, 2299, 3, "GB-BNB", "BNB", "Banbridge", @"", 54.348970, -6.269780, 54.355472, -6.285787, 54.342466, -6.253772));
				_cache.Add(new GeoRegionData(826, 2300, 3, "GB-BNE", "BNE", "Barnet", @"", 51.644400, -0.199700, 51.686250, -0.256264, 51.635704, -0.119885));
				_cache.Add(new GeoRegionData(826, 2301, 3, "GB-BNH", "BNH", "The City of Brighton and Hove", @"", 50.835160, -0.126102, 50.892374, -0.245077, 50.799146, -0.016030));
				_cache.Add(new GeoRegionData(826, 2302, 3, "GB-BNS", "BNS", "Barnsley", @"", 53.552630, -1.479726, 53.601337, -1.528237, 53.527156, -1.368343));
				_cache.Add(new GeoRegionData(826, 2303, 3, "GB-BOL", "BOL", "Bolton", @"", 53.584441, -2.428619, 53.632951, -2.545628, 53.546657, -2.363008));
				_cache.Add(new GeoRegionData(826, 2304, 3, "GB-BPL", "BPL", "Blackpool", @"", 53.817505, -3.035674, 53.878239, -3.062461, 53.772578, -2.985879));
				_cache.Add(new GeoRegionData(826, 2305, 3, "GB-BRC", "BRC", "Bracknell Forest", @"", 51.407695, -0.729976, 51.468731, -0.837366, 51.331936, -0.630697));
				_cache.Add(new GeoRegionData(826, 2306, 3, "GB-BRD", "BRD", "Bradford", @"", 53.795984, -1.759398, 53.846849, -1.830908, 53.747144, -1.677812));
				_cache.Add(new GeoRegionData(826, 2307, 3, "GB-BRY", "BRY", "Bromley", @"", 51.406025, 0.013156, 51.435666, -0.014936, 51.354005, 0.069047));
				_cache.Add(new GeoRegionData(826, 2308, 2, "GB-BST", "BST", "City of Bristol", @"", 51.454513, -2.587910, 51.544432, -2.730516, 51.392545, -2.450902));
				_cache.Add(new GeoRegionData(826, 2309, 3, "GB-BUR", "BUR", "Bury", @"", 53.595024, -2.297151, 53.644733, -2.351879, 53.549733, -2.242539));
				_cache.Add(new GeoRegionData(826, 2310, 2, "GB-CAM", "CAM", "Cambridgeshire", @"", 52.276192, 0.096537, 52.739980, -0.499907, 52.005779, 0.514454));
				_cache.Add(new GeoRegionData(826, 2311, 2, "GB-CAY", "CAY", "Caerphilly", @"", 51.578829, -3.218134, 51.604063, -3.255585, 51.549683, -3.176560));
				_cache.Add(new GeoRegionData(826, 2312, 3, "GB-CBF", "CBF", "Central Bedfordshire", @"", 51.978006, -0.527598, 52.190903, -0.702181, 51.805087, -0.143957));
				_cache.Add(new GeoRegionData(826, 2313, 2, "GB-CGN", "CGN", "Ceredigion", @"", 52.381598, -3.922481, 52.562433, -4.696574, 52.026699, -3.658166));
				_cache.Add(new GeoRegionData(826, 2314, 3, "GB-CGV", "CGV", "Craigavon", @"", 54.450860, -6.393830, 54.457346, -6.409837, 54.444372, -6.377822));
				_cache.Add(new GeoRegionData(826, 2315, 3, "GB-CHE", "CHE", "Cheshire East", @"", 53.153011, -2.289809, 53.387445, -2.752928, 52.947149, -1.974793));
				_cache.Add(new GeoRegionData(826, 2316, 3, "GB-CHW", "CHW", "Cheshire West and Chester", @"", 53.230297, -2.715111, 53.344664, -3.114923, 52.982915, -2.346354));
				_cache.Add(new GeoRegionData(826, 2317, 3, "GB-CKF", "CKF", "Carrickfergus", @"", 54.713730, -5.808180, 54.720175, -5.824187, 54.707283, -5.792172));
				_cache.Add(new GeoRegionData(826, 2318, 3, "GB-CKT", "CKT", "Cookstown", @"", 40.049118, -74.562669, 40.055700, -74.570570, 40.041085, -74.552120));
				_cache.Add(new GeoRegionData(826, 2319, 3, "GB-CLD", "CLD", "Calderdale", @"", 53.716157, -1.858460, 53.722759, -1.874467, 53.709553, -1.842452));
				_cache.Add(new GeoRegionData(826, 2320, 2, "GB-CLK", "CLK", "Clackmannanshire", @"", 56.124139, -3.758379, 56.217234, -3.885090, 56.072424, -3.571412));
				_cache.Add(new GeoRegionData(826, 2321, 3, "GB-CLR", "CLR", "Coleraine", @"", 55.125290, -6.668420, 55.131669, -6.684427, 55.118909, -6.652412));
				_cache.Add(new GeoRegionData(826, 2322, 2, "GB-CMA", "CMA", "Cumbria", @"", 54.577232, -2.797483, 55.188981, -3.640200, 54.041892, -2.159018));
				_cache.Add(new GeoRegionData(826, 2323, 3, "GB-CMD", "CMD", "Camden", @"", 51.551705, -0.158825, 51.572978, -0.213501, 51.512652, -0.105349));
				_cache.Add(new GeoRegionData(826, 2324, 2, "GB-CMN", "CMN", "Carmarthenshire", @"", 51.859853, -4.260853, 52.142396, -4.723076, 51.654772, -3.647124));
				_cache.Add(new GeoRegionData(826, 2325, 2, "GB-CON", "CON", "Cornwall", @"", 50.503629, -4.652498, 50.931270, -5.722621, 49.955414, -4.166175));
				_cache.Add(new GeoRegionData(826, 2326, 3, "GB-COV", "COV", "Coventry", @"", 52.406822, -1.519693, 52.463472, -1.605885, 52.363910, -1.423950));
				_cache.Add(new GeoRegionData(826, 2327, 2, "GB-CRF", "CRF", "Cardiff", @"", 51.481581, -3.179090, 51.560906, -3.282381, 51.445744, -3.121518));
				_cache.Add(new GeoRegionData(826, 2328, 3, "GB-CRY", "CRY", "Croydon", @"", 51.376165, -0.098234, 51.405851, -0.153787, 51.329804, -0.010691));
				_cache.Add(new GeoRegionData(826, 2329, 3, "GB-CSR", "CSR", "Castlereagh", @"", 54.577180, -5.888210, 54.583646, -5.904217, 54.570712, -5.872202));
				_cache.Add(new GeoRegionData(826, 2330, 2, "GB-CWY", "CWY", "Conwy", @"", 53.282872, -3.829480, 53.293331, -3.861234, 53.254943, -3.819816));
				_cache.Add(new GeoRegionData(826, 2331, 3, "GB-DAL", "DAL", "Darlington", @"", 54.523610, -1.559458, 54.558784, -1.609591, 54.503605, -1.497105));
				_cache.Add(new GeoRegionData(826, 2332, 2, "GB-DBY", "DBY", "Derbyshire", @"", 53.122322, -1.513682, 53.540413, -2.034097, 52.696521, -1.166488));
				_cache.Add(new GeoRegionData(826, 2333, 2, "GB-DEN", "DEN", "Denbighshire", @"", 53.128308, -3.337108, 53.351621, -3.601270, 52.861955, -3.090296));
				_cache.Add(new GeoRegionData(826, 2334, 3, "GB-DER", "DER", "City of Derby", @"", 52.922530, -1.474618, 52.968135, -1.556857, 52.861037, -1.383078));
				_cache.Add(new GeoRegionData(826, 2335, 2, "GB-DEV", "DEV", "Devon", @"", 50.777213, -3.999461, 51.246198, -4.680656, 50.201896, -2.886640));
				_cache.Add(new GeoRegionData(826, 2336, 3, "GB-DGN", "DGN", "Dungannon and South Tyrone", @"", 54.460881, -6.824940, 54.582010, -7.366730, 54.325110, -6.575757));
				_cache.Add(new GeoRegionData(826, 2337, 2, "GB-DGY", "DGY", "Dumfries and Galloway", @"", 54.988285, -3.857783, 55.464050, -5.187639, 54.633238, -2.857363));
				_cache.Add(new GeoRegionData(826, 2338, 3, "GB-DNC", "DNC", "Doncaster", @"", 53.522820, -1.128462, 53.567153, -1.192751, 53.470540, -1.047295));
				_cache.Add(new GeoRegionData(826, 2339, 2, "GB-DND", "DND", "Dundee City", @"", 56.462018, -2.970721, 56.505597, -3.098024, 56.450921, -2.835641));
				_cache.Add(new GeoRegionData(826, 2340, 2, "GB-DOR", "DOR", "Dorset", @"", 50.739066, -2.338234, 51.080997, -2.961594, 50.513072, -1.681663));
				_cache.Add(new GeoRegionData(826, 2341, 3, "GB-DOW", "DOW", "Down", @"", 54.341291, -5.748485, 54.495680, -6.075909, 54.166989, -5.522689));
				_cache.Add(new GeoRegionData(826, 2342, 3, "GB-DRY", "DRY", "Derry City", @"", 54.996612, -7.308574, 55.043619, -7.360692, 54.969712, -7.262598));
				_cache.Add(new GeoRegionData(826, 2343, 3, "GB-DUD", "DUD", "Dudley", @"", 52.512255, -2.081112, 52.558173, -2.156784, 52.473544, -2.057186));
				_cache.Add(new GeoRegionData(826, 2344, 3, "GB-DUR", "DUR", "Durham", @"", 35.994032, -78.898619, 36.136928, -79.007650, 35.866724, -78.762172));
				_cache.Add(new GeoRegionData(826, 2345, 3, "GB-EAL", "EAL", "Ealing", @"", 51.525025, -0.341500, 51.559683, -0.419621, 51.490470, -0.245086));
				_cache.Add(new GeoRegionData(826, 2346, 2, "GB-EAY", "EAY", "East Ayrshire", @"", 55.459231, -4.333803, 55.763784, -4.638611, 55.138051, -3.957056));
				_cache.Add(new GeoRegionData(826, 2347, 2, "GB-EDH", "EDH", "City of Edinburgh", @"", 55.953252, -3.188267, 55.991984, -3.333018, 55.890422, -3.077748));
				_cache.Add(new GeoRegionData(826, 2348, 2, "GB-EDU", "EDU", "East Dunbartonshire", @"", 55.975521, -4.210514, 56.030296, -4.402052, 55.896088, -4.046871));
				_cache.Add(new GeoRegionData(826, 2349, 2, "GB-ELN", "ELN", "East Lothian", @"", 55.949338, -2.770446, 56.079289, -3.089280, 55.817320, -2.364235));
				_cache.Add(new GeoRegionData(826, 2350, 2, "GB-ELS", "ELS", "Outer Hebrides", @"", 58.243608, -6.667201, 58.518175, -7.732738, 56.778018, -6.135330));
				_cache.Add(new GeoRegionData(826, 2351, 3, "GB-ENF", "ENF", "Enfield", @"", 51.654465, -0.081814, 51.693754, -0.153915, 51.633741, -0.006705));
				_cache.Add(new GeoRegionData(826, 2352, 2, "GB-ERW", "ERW", "East Renfrewshire", @"", 55.747650, -4.351420, 55.823152, -4.550952, 55.675795, -4.218758));
				_cache.Add(new GeoRegionData(826, 2353, 3, "GB-ERY", "ERY", "East Riding of Yorkshire", @"", 53.821588, -0.718997, 54.176512, -1.103567, 53.572466, 0.147817));
				_cache.Add(new GeoRegionData(826, 2354, 2, "GB-ESS", "ESS", "Essex", @"", 51.765907, 0.667366, 52.092662, -0.019769, 51.507689, 1.296592));
				_cache.Add(new GeoRegionData(826, 2355, 2, "GB-ESX", "ESX", "East Sussex", @"", 50.928598, 0.276489, 51.147402, -0.135866, 50.733434, 0.867855));
				_cache.Add(new GeoRegionData(826, 2356, 2, "GB-FAL", "FAL", "Falkirk", @"", 56.001091, -3.783522, 56.026761, -3.847607, 55.983004, -3.750436));
				_cache.Add(new GeoRegionData(826, 2357, 3, "GB-FER", "FER", "Fermanagh", @"", 54.451352, -7.712501, 54.610350, -8.177540, 54.113100, -7.142280));
				_cache.Add(new GeoRegionData(826, 2358, 2, "GB-FIF", "FIF", "Fife", @"", 56.208207, -3.149517, 56.453114, -3.739918, 56.005883, -2.544331));
				_cache.Add(new GeoRegionData(826, 2359, 2, "GB-FLN", "FLN", "Flintshire", @"", 53.197787, -3.159979, 53.356240, -3.400597, 53.072125, -2.920275));
				_cache.Add(new GeoRegionData(826, 2360, 3, "GB-GAT", "GAT", "Gateshead", @"", 54.952680, -1.603411, 54.970534, -1.661696, 54.913672, -1.516398));
				_cache.Add(new GeoRegionData(826, 2361, 2, "GB-GLG", "GLG", "Glasgow City", @"", 55.864237, -4.251806, 55.929641, -4.393200, 55.781279, -4.071716));
				_cache.Add(new GeoRegionData(826, 2362, 2, "GB-GLS", "GLS", "Gloucestershire", @"", 51.746774, -2.225785, 52.112579, -2.687537, 51.577535, -1.615202));
				_cache.Add(new GeoRegionData(826, 2363, 3, "GB-GRE", "GRE", "Greenwich", @"", 51.483061, -0.004151, 51.490009, -0.020158, 51.476111, 0.011856));
				_cache.Add(new GeoRegionData(826, 2364, 2, "GB-GWN", "GWN", "Gwynedd", @"", 52.892893, -3.995846, 53.248342, -4.804320, 52.534638, -3.436765));
				_cache.Add(new GeoRegionData(826, 2365, 3, "GB-HAL", "HAL", "Halton", @"", 53.349027, -2.713691, 53.402537, -2.832457, 53.305020, -2.595222));
				_cache.Add(new GeoRegionData(826, 2366, 2, "GB-HAM", "HAM", "Hampshire", @"", 51.089520, -1.216844, 51.383915, -1.957277, 50.706016, -0.729387));
				_cache.Add(new GeoRegionData(826, 2367, 3, "GB-HAV", "HAV", "Havering", @"", 51.577924, 0.212082, 51.631734, 0.138156, 51.487277, 0.333995));
				_cache.Add(new GeoRegionData(826, 2368, 3, "GB-HCK", "HCK", "Hackney", @"", 53.156314, -1.575021, 53.163004, -1.591029, 53.149622, -1.559014));
				_cache.Add(new GeoRegionData(826, 2369, 2, "GB-HEF", "HEF", "Herefordshire", @"", 52.076516, -2.654418, 52.395471, -3.141914, 51.825944, -2.337966));
				_cache.Add(new GeoRegionData(826, 2370, 3, "GB-HIL", "HIL", "Hillingdon", @"", 51.535183, -0.448137, 51.631755, -0.510375, 51.453266, -0.375929));
				_cache.Add(new GeoRegionData(826, 2371, 2, "GB-HLD", "HLD", "Highland", @"", 39.859720, -95.269698, 39.864289, -95.274535, 39.855297, -95.255496));
				_cache.Add(new GeoRegionData(826, 2372, 3, "GB-HMF", "HMF", "Hammersmith and Fulham", @"", 51.499017, -0.229149, 51.532752, -0.255090, 51.463897, -0.177627));
				_cache.Add(new GeoRegionData(826, 2373, 3, "GB-HNS", "HNS", "Hounslow", @"", 51.460921, -0.373149, 51.500315, -0.416867, 51.445737, -0.345857));
				_cache.Add(new GeoRegionData(826, 2374, 3, "GB-HPL", "HPL", "Hartlepool", @"", 54.691745, -1.212926, 54.723330, -1.267925, 54.621658, -1.157729));
				_cache.Add(new GeoRegionData(826, 2375, 2, "GB-HRT", "HRT", "Hertfordshire", @"", 51.809782, -0.237674, 52.080536, -0.745789, 51.599618, 0.195566));
				_cache.Add(new GeoRegionData(826, 2376, 3, "GB-HRW", "HRW", "Harrow", @"", 51.580559, -0.341995, 51.630808, -0.386460, 51.553061, -0.279745));
				_cache.Add(new GeoRegionData(826, 2377, 3, "GB-HRY", "HRY", "Haringey", @"", 51.591169, -0.109425, 51.611214, -0.171285, 51.564635, -0.041447));
				_cache.Add(new GeoRegionData(826, 2378, 2, "GB-IOW", "IOW", "Isle of Wight", @"", 50.692717, -1.316710, 50.766738, -1.582977, 50.574176, -1.072790));
				_cache.Add(new GeoRegionData(826, 2379, 3, "GB-ISL", "ISL", "Islington", @"", 51.546506, -0.105805, 51.575508, -0.142536, 51.518540, -0.076357));
				_cache.Add(new GeoRegionData(826, 2380, 2, "GB-IVC", "IVC", "Inverclyde", @"", 55.911808, -4.735906, 55.963783, -4.898774, 55.837975, -4.597316));
				_cache.Add(new GeoRegionData(826, 2381, 3, "GB-KEC", "KEC", "Kensington and Chelsea", @"", 51.499080, -0.193825, 51.530352, -0.228727, 51.477221, -0.149843));
				_cache.Add(new GeoRegionData(826, 2382, 2, "GB-KEN", "KEN", "Kent", @"", 51.278707, 0.521725, 51.480310, 0.033519, 50.910528, 1.450999));
				_cache.Add(new GeoRegionData(826, 2383, 3, "GB-KHL", "KHL", "Kingston upon Hull", @"", 53.745670, -0.336741, 53.813250, -0.422575, 53.719495, -0.241396));
				_cache.Add(new GeoRegionData(826, 2384, 3, "GB-KIR", "KIR", "Kirklees", @"", 52.602053, 1.280326, 52.603310, 1.278804, 52.600612, 1.281773));
				_cache.Add(new GeoRegionData(826, 2385, 3, "GB-KTT", "KTT", "Kingston upon Thames", @"", 51.412330, -0.300689, 51.437290, -0.316854, 51.397720, -0.249543));
				_cache.Add(new GeoRegionData(826, 2386, 3, "GB-KWL", "KWL", "Knowsley", @"", 53.454594, -2.852907, 53.467191, -2.891589, 53.429681, -2.795111));
				_cache.Add(new GeoRegionData(826, 2387, 2, "GB-LAN", "LAN", "Lancashire", @"", 53.969008, -2.627690, 54.239557, -3.057182, 53.482765, -2.045072));
				_cache.Add(new GeoRegionData(826, 2388, 3, "GB-LBH", "LBH", "Lambeth", @"", 51.457147, -0.123068, 51.509871, -0.151231, 51.410991, -0.078306));
				_cache.Add(new GeoRegionData(826, 2389, 3, "GB-LCE", "LCE", "City of Leicester", @"", 52.636877, -1.139759, 52.691503, -1.215987, 52.580650, -1.046212));
				_cache.Add(new GeoRegionData(826, 2390, 3, "GB-LDS", "LDS", "Leeds", @"", 53.801279, -1.548567, 53.881202, -1.674091, 53.730802, -1.397373));
				_cache.Add(new GeoRegionData(826, 2391, 2, "GB-LEC", "LEC", "Leicestershire", @"", 52.740122, -1.140592, 52.977657, -1.597547, 52.392156, -0.664110));
				_cache.Add(new GeoRegionData(826, 2392, 3, "GB-LEW", "LEW", "Lewisham", @"", 51.461345, -0.011867, 51.468296, -0.027874, 51.454392, 0.004140));
				_cache.Add(new GeoRegionData(826, 2393, 2, "GB-LIN", "LIN", "Lincolnshire", @"", 53.217882, -0.199970, 53.616366, -0.820651, 52.640217, 0.356263));
				_cache.Add(new GeoRegionData(826, 2394, 3, "GB-LIV", "LIV", "Liverpool", @"", 53.408371, -2.991572, 53.503907, -3.008747, 53.336275, -2.812938));
				_cache.Add(new GeoRegionData(826, 2395, 3, "GB-LMV", "LMV", "Limavady", @"", 55.052220, -6.940350, 55.058611, -6.956357, 55.045827, -6.924342));
				_cache.Add(new GeoRegionData(826, 2396, 3, "GB-LND", "LND", "City of London", @"", 51.511213, -0.119824, 51.672343, -0.351468, 51.384940, 0.148271));
				_cache.Add(new GeoRegionData(826, 2397, 3, "GB-LRN", "LRN", "Larne", @"", 54.857500, -5.809860, 54.863922, -5.825867, 54.851076, -5.793852));
				_cache.Add(new GeoRegionData(826, 2398, 3, "GB-LSB", "LSB", "Lisburn", @"", 54.509720, -6.037400, 54.516197, -6.053407, 54.503241, -6.021392));
				_cache.Add(new GeoRegionData(826, 2399, 3, "GB-LUT", "LUT", "Luton", @"", 51.878670, -0.420025, 51.927738, -0.505948, 51.854473, -0.349923));
				_cache.Add(new GeoRegionData(826, 2400, 3, "GB-MAN", "MAN", "City of Manchester", @"", 53.479324, -2.248485, 53.544587, -2.300096, 53.399903, -2.147087));
				_cache.Add(new GeoRegionData(826, 2401, 3, "GB-MDB", "MDB", "Middlesbrough", @"", 54.574227, -1.234956, 54.591517, -1.279468, 54.517552, -1.166625));
				_cache.Add(new GeoRegionData(826, 2402, 3, "GB-MDW", "MDW", "Medway", @"", 51.408492, 0.587845, 51.487421, 0.397317, 51.327896, 0.723776));
				_cache.Add(new GeoRegionData(826, 2403, 3, "GB-MFT", "MFT", "Magherafelt", @"", 54.756390, -6.606360, 54.762828, -6.622367, 54.749950, -6.590352));
				_cache.Add(new GeoRegionData(826, 2404, 3, "GB-MIK", "MIK", "Milton Keynes", @"", 52.040622, -0.759417, 52.052697, -0.780704, 52.023579, -0.734583));
				_cache.Add(new GeoRegionData(826, 2405, 2, "GB-MLN", "MLN", "Midlothian", @"", 55.829224, -3.133842, 55.932023, -3.369039, 55.710349, -2.846238));
				_cache.Add(new GeoRegionData(826, 2406, 2, "GB-MON", "MON", "Monmouthshire", @"", 51.774928, -2.875924, 51.983136, -3.157344, 51.554020, -2.649806));
				_cache.Add(new GeoRegionData(826, 2407, 3, "GB-MRT", "MRT", "Merton", @"", 50.891854, -4.095316, 50.898891, -4.111323, 50.884815, -4.079308));
				_cache.Add(new GeoRegionData(826, 2408, 2, "GB-MRY", "MRY", "Moray", @"", 57.511548, -3.248377, 57.730045, -3.764745, 57.068453, -2.649822));
				_cache.Add(new GeoRegionData(826, 2409, 2, "GB-MTY", "MTY", "Merthyr Tydfil", @"", 51.748730, -3.381646, 51.769603, -3.400272, 51.732172, -3.356139));
				_cache.Add(new GeoRegionData(826, 2410, 3, "GB-MYL", "MYL", "Moyle", @"", 55.114938, -6.153783, 55.312940, -6.537390, 54.983930, -5.976800));
				_cache.Add(new GeoRegionData(826, 2411, 2, "GB-NAY", "NAY", "North Ayrshire", @"", 55.711390, -4.729983, 55.892613, -4.968088, 55.563075, -4.492474));
				_cache.Add(new GeoRegionData(826, 2412, 2, "GB-NBL", "NBL", "Northumberland", @"", 55.208254, -2.078413, 55.811086, -2.689785, 54.782370, -1.460316));
				_cache.Add(new GeoRegionData(826, 2413, 3, "GB-NDN", "NDN", "North Down", @"", 54.639742, -5.710525, 54.681719, -5.861379, 54.610950, -5.520369));
				_cache.Add(new GeoRegionData(826, 2414, 3, "GB-NEL", "NEL", "North East Lincolnshire", @"", 53.541855, -0.126373, 53.639978, -0.292110, 53.433575, 0.026638));
				_cache.Add(new GeoRegionData(826, 2415, 3, "GB-NET", "NET", "Newcastle upon Tyne", @"", 54.978252, -1.617780, 55.045304, -1.781081, 54.959439, -1.532605));
				_cache.Add(new GeoRegionData(826, 2416, 2, "GB-NFK", "NFK", "Norfolk", @"", 36.850768, -76.285872, 36.969454, -76.334232, 36.820757, -76.176904));
				_cache.Add(new GeoRegionData(826, 2417, 3, "GB-NGM", "NGM", "City of Nottingham", @"", 52.954783, -1.158108, 53.019045, -1.248289, 52.889000, -1.091833));
				_cache.Add(new GeoRegionData(826, 2418, 2, "GB-NLK", "NLK", "North Lanarkshire", @"", 55.828913, -3.922195, 56.031368, -4.194502, 55.734760, -3.711135));
				_cache.Add(new GeoRegionData(826, 2419, 3, "GB-NLN", "NLN", "North Lincolnshire", @"", 53.605559, -0.559658, 53.714714, -0.950005, 53.455043, -0.200681));
				_cache.Add(new GeoRegionData(826, 2420, 3, "GB-NSM", "NSM", "North Somerset", @"", 51.440965, -2.742652, 51.502681, -2.994972, 51.290618, -2.587200));
				_cache.Add(new GeoRegionData(826, 2421, 3, "GB-NTA", "NTA", "Newtownabbey", @"", 54.685280, -5.964500, 54.691729, -5.980507, 54.678829, -5.948492));
				_cache.Add(new GeoRegionData(826, 2422, 2, "GB-NTH", "NTH", "Northamptonshire", @"", 52.272994, -0.875551, 52.643600, -1.332346, 51.977281, -0.341608));
				_cache.Add(new GeoRegionData(826, 2423, 2, "GB-NTL", "NTL", "Neath Port Talbot", @"", 51.665173, -3.771648, 51.810337, -3.936316, 51.527163, -3.561870));
				_cache.Add(new GeoRegionData(826, 2424, 2, "GB-NTT", "NTT", "Nottinghamshire", @"", 53.128504, -0.903099, 53.502478, -1.344592, 52.789437, -0.666263));
				_cache.Add(new GeoRegionData(826, 2425, 3, "GB-NTY", "NTY", "North Tyneside", @"", 55.008000, -1.546000, 55.014398, -1.562007, 55.001600, -1.529992));
				_cache.Add(new GeoRegionData(826, 2426, 3, "GB-NWM", "NWM", "Newham", @"", 51.525516, 0.035216, 51.563988, -0.021264, 51.497706, 0.097676));
				_cache.Add(new GeoRegionData(826, 2427, 2, "GB-NWP", "NWP", "Newport", @"", 41.490102, -71.312828, 41.523208, -71.363399, 41.449012, -71.286137));
				_cache.Add(new GeoRegionData(826, 2428, 2, "GB-NYK", "NYK", "North Yorkshire", @"", 54.250359, -1.470855, 54.560122, -2.564738, 53.621094, -0.212495));
				_cache.Add(new GeoRegionData(826, 2429, 3, "GB-NYM", "NYM", "Newry and Mourne", @"", 54.127246, -6.508945, 54.274189, -6.669029, 54.022609, -5.871869));
				_cache.Add(new GeoRegionData(826, 2430, 3, "GB-OLD", "OLD", "Oldham", @"", 53.544545, -2.118732, 53.573324, -2.143764, 53.498962, -2.043590));
				_cache.Add(new GeoRegionData(826, 2431, 3, "GB-OMH", "OMH", "Omagh", @"", 54.596570, -7.306910, 54.603033, -7.322917, 54.590105, -7.290902));
				_cache.Add(new GeoRegionData(826, 2432, 2, "GB-ORK", "ORK", "Orkney Islands", @"", 59.042912, -3.154215, 59.392793, -3.436066, 58.673049, -2.370431));
				_cache.Add(new GeoRegionData(826, 2433, 2, "GB-OXF", "OXF", "Oxfordshire", @"", 51.761205, -1.246467, 52.168471, -1.719500, 51.459413, -0.870083));
				_cache.Add(new GeoRegionData(826, 2434, 2, "GB-PEM", "PEM", "Pembrokeshire", @"", 51.875707, -4.939193, 52.117605, -5.482705, 51.596078, -4.485517));
				_cache.Add(new GeoRegionData(826, 2435, 2, "GB-PKN", "PKN", "Perth and Kinross", @"", 56.591736, -3.855734, 56.948633, -4.720444, 56.132436, -3.051768));
				_cache.Add(new GeoRegionData(826, 2436, 3, "GB-PLY", "PLY", "City of Plymouth", @"", 50.375456, -4.142656, 50.444166, -4.205532, 50.333318, -4.019607));
				_cache.Add(new GeoRegionData(826, 2437, 3, "GB-POL", "POL", "Poole", @"", 50.715050, -1.987248, 50.764461, -2.054955, 50.681660, -1.891147));
				_cache.Add(new GeoRegionData(826, 2438, 3, "GB-POR", "POR", "City of Portsmouth", @"", 50.816666, -1.083333, 50.859309, -1.172402, 50.770393, -1.020640));
				_cache.Add(new GeoRegionData(826, 2439, 2, "GB-POW", "POW", "Powys", @"", 52.143086, -3.373682, 52.901560, -3.929353, 51.752745, -2.949634));
				_cache.Add(new GeoRegionData(826, 2440, 3, "GB-PTE", "PTE", "City of Peterborough", @"", 52.569498, -0.240529, 52.663770, -0.497663, 52.506094, -0.103242));
				_cache.Add(new GeoRegionData(826, 2441, 3, "GB-RCC", "RCC", "Redcar and Cleveland", @"", 54.539336, -1.031021, 54.647527, -1.201728, 54.487834, -0.788414));
				_cache.Add(new GeoRegionData(826, 2442, 3, "GB-RCH", "RCH", "Rochdale", @"", 53.614086, -2.161814, 53.670752, -2.271789, 53.578031, -2.111702));
				_cache.Add(new GeoRegionData(826, 2443, 2, "GB-RCT", "RCT", "Rhondda Cynon Taf", @"", 51.674497, -3.468708, 51.829450, -3.593687, 51.499039, -3.236280));
				_cache.Add(new GeoRegionData(826, 2444, 3, "GB-RDB", "RDB", "Redbridge", @"", 51.588610, 0.082397, 51.629022, 0.008678, 51.543568, 0.149485));
				_cache.Add(new GeoRegionData(826, 2445, 3, "GB-RDG", "RDG", "Reading", @"", 40.335648, -75.926874, 40.371134, -75.959873, 40.296420, -75.890512));
				_cache.Add(new GeoRegionData(826, 2446, 2, "GB-RFW", "RFW", "Renfrewshire", @"", 55.829858, -4.542838, 55.936296, -4.784011, 55.759532, -4.353106));
				_cache.Add(new GeoRegionData(826, 2447, 3, "GB-RIC", "RIC", "Richmond upon Thames", @"", 51.461311, -0.303742, 51.487199, -0.330339, 51.421302, -0.252335));
				_cache.Add(new GeoRegionData(826, 2448, 3, "GB-ROT", "ROT", "Rotherham", @"", 53.432603, -1.363500, 53.464485, -1.434339, 53.402481, -1.300166));
				_cache.Add(new GeoRegionData(826, 2449, 2, "GB-RUT", "RUT", "Rutland", @"", 52.658301, -0.639643, 52.759801, -0.821761, 52.524786, -0.428376));
				_cache.Add(new GeoRegionData(826, 2450, 3, "GB-SAW", "SAW", "Sandwell", @"", 52.536167, -2.010793, 52.569058, -2.097114, 52.460697, -1.918163));
				_cache.Add(new GeoRegionData(826, 2451, 2, "GB-SAY", "SAY", "South Ayrshire", @"", 55.270111, -4.652418, 55.601870, -5.124233, 54.997653, -4.398859));
				_cache.Add(new GeoRegionData(826, 2452, 2, "GB-SCB", "SCB", "Scottish Borders", @"", 55.548569, -2.786138, 55.946239, -3.539803, 55.108344, -2.034353));
				_cache.Add(new GeoRegionData(826, 2453, 2, "GB-SFK", "SFK", "Suffolk", @"", 52.187247, 0.970780, 52.549517, 0.339974, 51.932130, 1.763030));
				_cache.Add(new GeoRegionData(826, 2454, 3, "GB-SFT", "SFT", "Sefton", @"", 53.503445, -2.970359, 53.510081, -2.986366, 53.496808, -2.954351));
				_cache.Add(new GeoRegionData(826, 2455, 3, "GB-SGC", "SGC", "South Gloucestershire", @"", 51.531456, -2.454715, 51.677337, -2.698702, 51.415904, -2.252112));
				_cache.Add(new GeoRegionData(826, 2456, 3, "GB-SHF", "SHF", "Sheffield", @"", 53.381129, -1.470085, 53.486882, -1.663959, 53.304550, -1.334953));
				_cache.Add(new GeoRegionData(826, 2457, 3, "GB-SHN", "SHN", "St. Helens", @"", 45.859011, -122.815818, 45.884214, -122.855757, 45.833431, -122.789519));
				_cache.Add(new GeoRegionData(826, 2458, 2, "GB-SHR", "SHR", "Shropshire", @"", 52.586484, -2.703750, 52.998394, -3.235565, 52.306267, -2.232898));
				_cache.Add(new GeoRegionData(826, 2459, 3, "GB-SKP", "SKP", "Stockport", @"", 53.406754, -2.158843, 53.454879, -2.215528, 53.380150, -2.082595));
				_cache.Add(new GeoRegionData(826, 2460, 3, "GB-SLF", "SLF", "City of Salford", @"", 53.488465, -2.298296, 53.518428, -2.331082, 53.464860, -2.245138));
				_cache.Add(new GeoRegionData(826, 2461, 3, "GB-SLG", "SLG", "Slough", @"", 51.510538, -0.595040, 51.545842, -0.660165, 51.467984, -0.490044));
				_cache.Add(new GeoRegionData(826, 2462, 2, "GB-SLK", "SLK", "South Lanarkshire", @"", 55.524303, -3.703507, 55.844439, -4.282571, 55.290767, -3.396407));
				_cache.Add(new GeoRegionData(826, 2463, 3, "GB-SND", "SND", "Sunderland", @"", 54.906869, -1.383801, 54.944171, -1.466499, 54.847151, -1.345747));
				_cache.Add(new GeoRegionData(826, 2464, 3, "GB-SOL", "SOL", "Solihull", @"", 52.411811, -1.777610, 52.458301, -1.827838, 52.391844, -1.737395));
				_cache.Add(new GeoRegionData(826, 2465, 2, "GB-SOM", "SOM", "Somerset", @"", 51.058701, -2.949906, 51.329347, -3.839803, 50.820899, -2.244434));
				_cache.Add(new GeoRegionData(826, 2466, 3, "GB-SOS", "SOS", "Southend-on-Sea", @"", 51.545926, 0.707712, 51.576796, 0.622917, 51.520909, 0.820934));
				_cache.Add(new GeoRegionData(826, 2467, 2, "GB-SRY", "SRY", "Surrey", @"", 49.186495, -122.823134, 49.219932, -122.919669, 49.002082, -122.678207));
				_cache.Add(new GeoRegionData(826, 2468, 3, "GB-STB", "STB", "Strabane", @"", 54.827268, -7.463579, 54.833695, -7.479586, 54.820840, -7.447571));
				_cache.Add(new GeoRegionData(826, 2469, 3, "GB-STE", "STE", "City of Stoke-on-Trent", @"", 53.002668, -2.179404, 53.092546, -2.240246, 52.951331, -2.079240));
				_cache.Add(new GeoRegionData(826, 2470, 2, "GB-STG", "STG", "Stirling", @"", 56.116522, -3.936902, 56.145538, -3.975000, 56.091872, -3.904396));
				_cache.Add(new GeoRegionData(826, 2471, 3, "GB-STH", "STH", "City of Southampton", @"", 50.909700, -1.404350, 50.956135, -1.478998, 50.877371, -1.321987));
				_cache.Add(new GeoRegionData(826, 2472, 3, "GB-STN", "STN", "Sutton", @"", 51.361427, -0.193961, 51.388253, -0.240782, 51.329253, -0.169408));
				_cache.Add(new GeoRegionData(826, 2473, 2, "GB-STS", "STS", "Staffordshire", @"", 52.769795, -2.104524, 53.226223, -2.470841, 52.423243, -1.585467));
				_cache.Add(new GeoRegionData(826, 2474, 3, "GB-STT", "STT", "Stockton-on-Tees", @"", 54.570455, -1.328982, 54.645274, -1.436311, 54.476975, -1.153548));
				_cache.Add(new GeoRegionData(826, 2475, 3, "GB-STY", "STY", "South Tyneside", @"", 54.963669, -1.441863, 55.011342, -1.535512, 54.928412, -1.352390));
				_cache.Add(new GeoRegionData(826, 2476, 2, "GB-SWA", "SWA", "Swansea", @"", 51.621440, -3.943646, 51.639149, -3.967001, 51.611881, -3.928992));
				_cache.Add(new GeoRegionData(826, 2477, 3, "GB-SWD", "SWD", "Swindon", @"", 51.555773, -1.779717, 51.609129, -1.865137, 51.530334, -1.719472));
				_cache.Add(new GeoRegionData(826, 2478, 3, "GB-SWK", "SWK", "Southwark", @"", 51.502781, -0.087738, 51.509726, -0.103745, 51.495834, -0.071730));
				_cache.Add(new GeoRegionData(826, 2479, 3, "GB-TAM", "TAM", "Tameside", @"", 54.474100, -1.190638, 54.475525, -1.194679, 54.472827, -1.186730));
				_cache.Add(new GeoRegionData(826, 2480, 3, "GB-TFW", "TFW", "Telford and Wrekin", @"", 52.740991, -2.486858, 52.828372, -2.667364, 52.614543, -2.312211));
				_cache.Add(new GeoRegionData(826, 2481, 3, "GB-THR", "THR", "Thurrock", @"", 51.493455, 0.352919, 51.567819, 0.210460, 51.451002, 0.550786));
				_cache.Add(new GeoRegionData(826, 2482, 3, "GB-TOB", "TOB", "Torbay", @"", 50.461920, -3.525315, 50.502461, -3.582790, 50.451018, -3.480495));
				_cache.Add(new GeoRegionData(826, 2483, 2, "GB-TOF", "TOF", "Torfaen", @"", 51.665266, -3.039753, 51.796261, -3.143857, 51.606672, -2.958867));
				_cache.Add(new GeoRegionData(826, 2484, 3, "GB-TRF", "TRF", "Trafford", @"", 53.421513, -2.351726, 53.480369, -2.478333, 53.357408, -2.253039));
				_cache.Add(new GeoRegionData(826, 2485, 3, "GB-TWH", "TWH", "Tower Hamlets", @"", 51.520260, -0.029339, 51.544685, -0.080189, 51.484503, 0.009863));
				_cache.Add(new GeoRegionData(826, 2486, 2, "GB-VGL", "VGL", "Vale of Glamorgan", @"", 51.444358, -3.415116, 51.515343, -3.643908, 51.381342, -3.163787));
				_cache.Add(new GeoRegionData(826, 2487, 2, "GB-WAR", "WAR", "Warwickshire", @"", 52.267135, -1.467521, 52.687243, -1.962006, 51.955393, -1.172140));
				_cache.Add(new GeoRegionData(826, 2488, 3, "GB-WBK", "WBK", "West Berkshire", @"", 51.465986, -1.281401, 51.563711, -1.588088, 51.328978, -0.981745));
				_cache.Add(new GeoRegionData(826, 2489, 2, "GB-WDU", "WDU", "West Dunbartonshire", @"", 55.965064, -4.506359, 56.073138, -4.659908, 55.889151, -4.375489));
				_cache.Add(new GeoRegionData(826, 2490, 3, "GB-WFT", "WFT", "Waltham Forest", @"", 51.588638, -0.011762, 51.646526, -0.062274, 51.549925, 0.025712));
				_cache.Add(new GeoRegionData(826, 2491, 3, "GB-WGN", "WGN", "Wigan", @"", 53.545440, -2.637474, 53.573043, -2.702430, 53.501483, -2.600347));
				_cache.Add(new GeoRegionData(826, 2492, 2, "GB-WIL", "WIL", "Wiltshire", @"", 51.246271, -1.992212, 51.703141, -2.365598, 50.944992, -1.485726));
				_cache.Add(new GeoRegionData(826, 2493, 3, "GB-WKF", "WKF", "Wakefield", @"", 53.683298, -1.505924, 53.715547, -1.556621, 53.636011, -1.463839));
				_cache.Add(new GeoRegionData(826, 2494, 3, "GB-WLL", "WLL", "Walsall", @"", 52.586214, -1.982919, 52.638326, -2.040192, 52.550882, -1.887143));
				_cache.Add(new GeoRegionData(826, 2495, 2, "GB-WLN", "WLN", "West Lothian", @"", 55.907019, -3.551716, 56.002197, -3.831258, 55.770717, -3.386628));
				_cache.Add(new GeoRegionData(826, 2496, 3, "GB-WLV", "WLV", "City of Wolverhampton", @"", 52.586973, -2.128820, 52.647698, -2.212195, 52.543167, -2.050597));
				_cache.Add(new GeoRegionData(826, 2497, 3, "GB-WND", "WND", "Wandsworth", @"", 51.457071, -0.181782, 51.485892, -0.259113, 51.417752, -0.126363));
				_cache.Add(new GeoRegionData(826, 2498, 3, "GB-WNM", "WNM", "Windsor and Maidenhead", @"", 51.491705, -0.732175, 51.577825, -0.853926, 51.382872, -0.522784));
				_cache.Add(new GeoRegionData(826, 2499, 3, "GB-WOK", "WOK", "Wokingham", @"", 51.410457, -0.833861, 51.429805, -0.876588, 51.386660, -0.792522));
				_cache.Add(new GeoRegionData(826, 2500, 2, "GB-WOR", "WOR", "Worcestershire", @"", 52.254522, -2.266838, 52.455302, -2.663210, 51.966556, -1.757408));
				_cache.Add(new GeoRegionData(826, 2501, 3, "GB-WRL", "WRL", "Wirral", @"", 53.333333, -3.083333, 53.339995, -3.099340, 53.326669, -3.067325));
				_cache.Add(new GeoRegionData(826, 2502, 3, "GB-WRT", "WRT", "Warrington", @"", 53.390044, -2.596950, 53.436562, -2.697724, 53.353648, -2.449362));
				_cache.Add(new GeoRegionData(826, 2503, 2, "GB-WRX", "WRX", "Wrexham", @"", 53.043040, -2.992494, 53.073060, -3.030842, 53.029110, -2.945493));
				_cache.Add(new GeoRegionData(826, 2504, 3, "GB-WSM", "WSM", "City of Westminster", @"", 39.836652, -105.037204, 39.968186, -105.165542, 39.819150, -104.987761));
				_cache.Add(new GeoRegionData(826, 2505, 2, "GB-WSX", "WSX", "West Sussex", @"", 50.928014, -0.461707, 51.167302, -0.957597, 50.722467, 0.044525));
				_cache.Add(new GeoRegionData(826, 2506, 3, "GB-YOR", "YOR", "York", @"", 53.962300, -1.081884, 53.991258, -1.147269, 53.925934, -1.013913));
				_cache.Add(new GeoRegionData(826, 2507, 2, "GB-ZET", "ZET", "Shetland Islands", @"", 60.529650, -1.265940, 60.860761, -1.829708, 59.848673, -0.724540));

				#endregion

				#region Ireland
				_cache.Add(new GeoRegionData(372, 2508, 2, "IE-C", "C", "Cork", @"", 51.896891, -8.486315, 51.935860, -8.545520, 51.856150, -8.380579));
				_cache.Add(new GeoRegionData(372, 2509, 2, "IE-CE", "CE", "Clare", @"", 43.819470, -84.768628, 43.843899, -84.788412, 43.803541, -84.731962));
				_cache.Add(new GeoRegionData(372, 2510, 2, "IE-CN", "CN", "Cavan", @"", 53.990604, -7.362940, 53.997164, -7.378948, 53.984044, -7.346933));
				_cache.Add(new GeoRegionData(372, 2511, 2, "IE-CW", "CW", "Carlow", @"", 53.758684, 10.936471, 53.792815, 10.868787, 53.725392, 10.967167));
				_cache.Add(new GeoRegionData(372, 2512, 2, "IE-D", "D", "Dublin", @"", 53.349805, -6.260309, 53.425210, -6.450840, 53.223430, -6.052550));
				_cache.Add(new GeoRegionData(372, 2513, 2, "IE-DL", "DL", "Donegal", @"", 54.654197, -8.110546, 54.660651, -8.126553, 54.647741, -8.094538));
				_cache.Add(new GeoRegionData(372, 2514, 2, "IE-G", "G", "Galway", @"", 53.270558, -9.056667, 53.277231, -9.072675, 53.263885, -9.040660));
				_cache.Add(new GeoRegionData(372, 2515, 2, "IE-KE", "KE", "Kildare", @"", 53.158934, -6.909568, 53.182860, -6.922590, 53.151960, -6.882989));
				_cache.Add(new GeoRegionData(372, 2516, 2, "IE-KK", "KK", "Kilkenny", @"", 52.654145, -7.244787, 52.676980, -7.273159, 52.631110, -7.205470));
				_cache.Add(new GeoRegionData(372, 2517, 2, "IE-KY", "KY", "Kerry", @"", 52.154460, -9.566863, 52.591840, -10.618362, 51.688322, -9.118786));
				_cache.Add(new GeoRegionData(372, 2518, 2, "IE-LD", "LD", "Longford", @"", 39.169995, -97.331413, 39.176387, -97.332177, 39.169137, -97.325089));
				_cache.Add(new GeoRegionData(372, 2519, 2, "IE-LH", "LH", "Louth", @"", 53.365962, -0.007711, 53.388388, -0.029350, 53.345940, 0.029063));
				_cache.Add(new GeoRegionData(372, 2520, 2, "IE-LK", "LK", "Limerick", @"", 52.668020, -8.630497, 52.688640, -8.689940, 52.614389, -8.570339));
				_cache.Add(new GeoRegionData(372, 2521, 2, "IE-LM", "LM", "Leitrim", @"", 53.992600, -8.065585, 53.999159, -8.081592, 53.986040, -8.049577));
				_cache.Add(new GeoRegionData(372, 2522, 2, "IE-LS", "LS", "Laois", @"", 52.994295, -7.332300, 53.215652, -7.734649, 52.781269, -6.932115));
				_cache.Add(new GeoRegionData(372, 2523, 2, "IE-MH", "MH", "Meath", @"", 53.605548, -6.656416, 53.917666, -7.335520, 53.381866, -6.212610));
				_cache.Add(new GeoRegionData(372, 2524, 2, "IE-MN", "MN", "Monaghan", @"", 54.249204, -6.968313, 54.421390, -7.339506, 53.900679, -6.549728));
				_cache.Add(new GeoRegionData(372, 2525, 2, "IE-MO", "MO", "Mayo", @"", 53.934581, -9.351645, 54.345400, -10.251001, 53.471926, -8.582861));
				_cache.Add(new GeoRegionData(372, 2526, 2, "IE-OY", "OY", "Offaly", @"", 53.235687, -7.712222, 53.424279, -8.083872, 52.848171, -6.977707));
				_cache.Add(new GeoRegionData(372, 2527, 2, "IE-RN", "RN", "Roscommon", @"", 53.627590, -8.189095, 53.639420, -8.207130, 53.611769, -8.164180));
				_cache.Add(new GeoRegionData(372, 2528, 2, "IE-SO", "SO", "Sligo", @"", 54.276610, -8.476088, 54.287239, -8.519330, 54.249510, -8.442130));
				_cache.Add(new GeoRegionData(372, 2529, 2, "IE-TA", "TA", "Tipperary", @"", 52.473789, -8.161851, 53.167582, -8.479782, 52.202014, -7.372059));
				_cache.Add(new GeoRegionData(372, 2530, 2, "IE-WD", "WD", "Waterford", @"", 52.252270, -7.127206, 52.270199, -7.171190, 52.224810, -7.054869));
				_cache.Add(new GeoRegionData(372, 2531, 2, "IE-WH", "WH", "Westmeath", @"", 53.534530, -7.465321, 53.787959, -7.972953, 53.317962, -6.954784));
				_cache.Add(new GeoRegionData(372, 2532, 2, "IE-WW", "WW", "Wicklow", @"", 52.980820, -6.044589, 52.998920, -6.070040, 52.966100, -6.015459));
				_cache.Add(new GeoRegionData(372, 2533, 2, "IE-WX", "WX", "Wexford", @"", 52.336916, -6.463338, 52.347009, -6.499950, 52.320440, -6.446430));
				#endregion

				#region India
				_cache.Add(new GeoRegionData(356, 2534, 1, "IN-AN", "AN", "Andaman and Nicobar Islands", @"", 11.740086, 92.658640, 13.675834, 92.374991, 10.514224, 93.115812));
				_cache.Add(new GeoRegionData(356, 2535, 1, "IN-AP", "AP", "Andhra Pradesh", @"", 17.047762, 80.098186, 19.916715, 76.749786, 12.596836, 84.804129));
				_cache.Add(new GeoRegionData(356, 2536, 1, "IN-AR", "AR", "Arunachal Pradesh", @"", 28.217999, 94.727752, 29.453453, 91.558064, 26.642580, 97.403297));
				_cache.Add(new GeoRegionData(356, 2537, 1, "IN-AS", "AS", "Assam", @"", 26.200604, 92.937573, 27.968216, 89.685637, 24.138498, 96.013160));
				_cache.Add(new GeoRegionData(356, 2538, 1, "IN-BR", "BR", "Bihar", @"", 25.198009, 85.521896, 25.243604, 85.501548, 25.175641, 85.553779));
				_cache.Add(new GeoRegionData(356, 2539, 1, "IN-CH", "CH", "Chandigarh", @"", 30.733314, 76.779417, 30.795893, 76.679858, 30.656898, 76.852970));
				_cache.Add(new GeoRegionData(356, 2540, 1, "IN-CT", "CT", "Chhattisgarh", @"", 21.278656, 81.866144, 24.118709, 80.243900, 17.782531, 84.395998));
				_cache.Add(new GeoRegionData(356, 2541, 1, "IN-DD", "DD", "Daman and Diu", @"", 20.428283, 72.839731, 20.467070, 72.821406, 20.367219, 72.906796));
				_cache.Add(new GeoRegionData(356, 2542, 1, "IN-DL", "DL", "Delhi", @"", 28.635308, 77.224960, 28.889816, 76.839699, 28.401066, 77.341814));
				_cache.Add(new GeoRegionData(356, 2543, 1, "IN-DN", "DN", "Dadra and Nagar Haveli", @"", 20.180867, 73.016913, 20.362502, 72.921629, 20.052287, 73.225892));
				_cache.Add(new GeoRegionData(356, 2544, 1, "IN-GA", "GA", "Goa", @"", 15.299326, 74.123996, 15.799917, 73.689327, 14.894508, 74.340532));
				_cache.Add(new GeoRegionData(356, 2545, 1, "IN-GJ", "GJ", "Gujarat", @"", 22.258652, 71.192380, 24.705709, 68.162835, 20.127954, 74.476488));
				_cache.Add(new GeoRegionData(356, 2546, 1, "IN-HP", "HP", "Himachal Pradesh", @"", 31.836085, 77.169855, 33.257958, 75.587470, 30.382469, 79.003309));
				_cache.Add(new GeoRegionData(356, 2547, 1, "IN-HR", "HR", "Haryana", @"", 29.058775, 76.085601, 30.912864, 74.457616, 27.652993, 77.595448));
				_cache.Add(new GeoRegionData(356, 2548, 1, "IN-JH", "JH", "Jharkhand", @"", 23.610180, 85.279935, 25.328823, 83.323828, 21.972930, 87.947529));
				_cache.Add(new GeoRegionData(356, 2549, 1, "IN-JK", "JK", "Jammu and Kashmir", @"", 34.149087, 76.825965, 35.505427, 73.750507, 32.292269, 79.305850));
				_cache.Add(new GeoRegionData(356, 2550, 1, "IN-KA", "KA", "Karnataka", @"", 15.317277, 75.713888, 18.441168, 74.049613, 11.593352, 78.586010));
				_cache.Add(new GeoRegionData(356, 2551, 1, "IN-KL", "KL", "Kerala", @"", 10.850515, 76.271083, 12.788300, 74.864906, 8.294896, 77.395636));
				_cache.Add(new GeoRegionData(356, 2552, 1, "IN-LD", "LD", "Lakshadweep", @"", 10.076011, 73.630344, 10.098323, 73.618774, 10.053700, 73.641914));
				_cache.Add(new GeoRegionData(356, 2553, 1, "IN-MH", "MH", "Maharashtra", @"", 19.751479, 75.713888, 22.028441, 72.659363, 15.602412, 80.890924));
				_cache.Add(new GeoRegionData(356, 2554, 1, "IN-ML", "ML", "Meghalaya", @"", 25.467030, 91.366216, 26.120405, 89.815674, 25.033357, 92.802266));
				_cache.Add(new GeoRegionData(356, 2555, 1, "IN-MN", "MN", "Manipur", @"", 25.424094, 94.258653, 25.425146, 94.255807, 25.422317, 94.261558));
				_cache.Add(new GeoRegionData(356, 2556, 1, "IN-MP", "MP", "Madhya Pradesh", @"", 22.973422, 78.656894, 26.868108, 74.036248, 21.079913, 82.809674));
				_cache.Add(new GeoRegionData(356, 2557, 1, "IN-MZ", "MZ", "Mizoram", @"", 23.164543, 92.937573, 24.517435, 92.258479, 21.946661, 93.437561));
				_cache.Add(new GeoRegionData(356, 2558, 1, "IN-NL", "NL", "Nagaland", @"", 26.158435, 94.562442, 27.036123, 93.327578, 25.198068, 95.244715));
				_cache.Add(new GeoRegionData(356, 2559, 1, "IN-OR", "OR", "Orissa", @"", 20.951665, 85.098523, 22.570027, 81.388423, 17.811982, 87.483385));
				_cache.Add(new GeoRegionData(356, 2560, 1, "IN-PB", "PB", "Punjab", @"", 31.147130, 75.341217, 32.498915, 73.870887, 29.537147, 76.921758));
				_cache.Add(new GeoRegionData(356, 2561, 1, "IN-PY", "PY", "Puducherry", @"", 11.913859, 79.814472, 11.985004, 79.740164, 11.874154, 79.846037));
				_cache.Add(new GeoRegionData(356, 2562, 1, "IN-RJ", "RJ", "Rajasthan", @"", 27.023803, 74.217932, 30.195124, 69.483734, 23.063266, 78.263381));
				_cache.Add(new GeoRegionData(356, 2563, 1, "IN-SK", "SK", "Sikkim", @"", 27.532971, 88.512217, 28.128759, 88.006354, 27.079261, 88.910805));
				_cache.Add(new GeoRegionData(356, 2564, 1, "IN-TN", "TN", "Tamil Nadu", @"", 11.127122, 78.656894, 13.496666, 76.230554, 8.077606, 80.346451));
				_cache.Add(new GeoRegionData(356, 2565, 1, "IN-TR", "TR", "Tripura", @"", 23.940848, 91.988152, 24.533733, 91.150931, 22.929054, 92.336001));
				_cache.Add(new GeoRegionData(356, 2566, 1, "IN-UL", "UL", "Uttarakhand", @"", 30.066753, 79.019299, 31.454615, 77.578608, 28.710125, 81.055663));
				_cache.Add(new GeoRegionData(356, 2567, 1, "IN-UP", "UP", "Uttar Pradesh", @"", 27.570588, 80.098186, 30.411635, 77.092436, 23.870839, 84.674326));
				_cache.Add(new GeoRegionData(356, 2568, 1, "IN-WB", "WB", "West Bengal", @"", 22.986756, 87.854975, 27.220707, 85.820958, 21.528360, 89.880039));
				#endregion

				#region Italy
				_cache.Add(new GeoRegionData(380, 2569, 2, "IT-AG", "AG", "Agrigento", @"", 37.311089, 13.576547, 37.340272, 13.568948, 37.296011, 13.603402));
				_cache.Add(new GeoRegionData(380, 2570, 2, "IT-AL", "AL", "Alessandria", @"", 44.907272, 8.611679, 44.939749, 8.554022, 44.877314, 8.640493));
				_cache.Add(new GeoRegionData(380, 2571, 2, "IT-AN", "AN", "Ancona", @"", 43.615829, 13.518915, 43.631466, 13.420874, 43.532833, 13.549857));
				_cache.Add(new GeoRegionData(380, 2572, 2, "IT-AO", "AO", "Valle d'Aosta", @"", 45.738887, 7.426186, 45.987759, 6.801350, 45.466944, 7.939526));
				_cache.Add(new GeoRegionData(380, 2573, 2, "IT-AP", "AP", "Ascoli Piceno", @"", 42.853604, 13.574944, 42.868165, 13.552394, 42.837602, 13.665637));
				_cache.Add(new GeoRegionData(380, 2574, 2, "IT-AQ", "AQ", "L'Aquila", @"", 42.349847, 13.399509, 42.388588, 13.336641, 42.333162, 13.435549));
				_cache.Add(new GeoRegionData(380, 2575, 2, "IT-AR", "AR", "Arezzo", @"", 43.466967, 11.882307, 43.499573, 11.809633, 43.351436, 11.924063));
				_cache.Add(new GeoRegionData(380, 2576, 2, "IT-AT", "AT", "Asti", @"", 18.807868, 75.166483, 18.817186, 75.155196, 18.796996, 75.179958));
				_cache.Add(new GeoRegionData(380, 2577, 2, "IT-AV", "AV", "Avellino", @"", 40.914384, 14.790280, 40.945888, 14.758846, 40.898442, 14.831441));
				_cache.Add(new GeoRegionData(380, 2578, 2, "IT-BA", "BA", "Bari", @"", 41.117143, 16.871871, 41.169567, 16.730783, 41.053010, 17.033349));
				_cache.Add(new GeoRegionData(380, 2579, 2, "IT-BG", "BG", "Bergamo", @"", 45.698264, 9.677269, 45.723848, 9.620546, 45.662515, 9.713632));
				_cache.Add(new GeoRegionData(380, 2580, 2, "IT-BI", "BI", "Biella", @"", 45.562884, 8.058339, 45.598386, 8.016126, 45.542722, 8.093554));
				_cache.Add(new GeoRegionData(380, 2581, 2, "IT-BL", "BL", "Belluno", @"", 46.142463, 12.216708, 46.171553, 12.156942, 46.123724, 12.251399));
				_cache.Add(new GeoRegionData(380, 2582, 2, "IT-BN", "BN", "Benevento", @"", 41.129761, 14.782620, 41.147562, 14.735054, 41.098359, 14.810324));
				_cache.Add(new GeoRegionData(380, 2583, 2, "IT-BO", "BO", "Bologna", @"", 44.494887, 11.342616, 44.556198, 11.229654, 44.442037, 11.433716));
				_cache.Add(new GeoRegionData(380, 2584, 2, "IT-BR", "BR", "Brindisi", @"", 40.632727, 17.941761, 40.681425, 17.909445, 40.603034, 17.962463));
				_cache.Add(new GeoRegionData(380, 2585, 2, "IT-BS", "BS", "Brescia", @"", 45.541187, 10.219443, 45.590063, 10.147357, 45.495824, 10.299999));
				_cache.Add(new GeoRegionData(380, 2586, 2, "IT-BT", "BT", "Barletta-Andria-Trani", @"", 41.200454, 16.205148, 41.441710, 15.870251, 40.897719, 16.542249));
				_cache.Add(new GeoRegionData(380, 2587, 2, "IT-BZ", "BZ", "Bolzano", @"", 46.498295, 11.354758, 46.515434, 11.313667, 46.463038, 11.380647));
				_cache.Add(new GeoRegionData(380, 2588, 2, "IT-CA", "CA", "Cagliari", @"", 39.223841, 9.121661, 39.265703, 9.079536, 39.185105, 9.177219));
				_cache.Add(new GeoRegionData(380, 2589, 2, "IT-CB", "CB", "Campobasso", @"", 41.560254, 14.662716, 41.574161, 14.643634, 41.540825, 14.692414));
				_cache.Add(new GeoRegionData(380, 2590, 2, "IT-CE", "CE", "Caserta", @"", 41.072348, 14.331133, 41.112548, 14.298142, 41.048719, 14.378548));
				_cache.Add(new GeoRegionData(380, 2591, 2, "IT-CH", "CH", "Chieti", @"", 42.347886, 14.163584, 42.396239, 14.082862, 42.318100, 14.210622));
				_cache.Add(new GeoRegionData(380, 2592, 2, "IT-CI", "CI", "Carbonia-Iglesias", @"", 39.253465, 8.572101, 39.483066, 8.219648, 38.934491, 8.853128));
				_cache.Add(new GeoRegionData(380, 2593, 2, "IT-CL", "CL", "Caltanissetta", @"", 37.490111, 14.062892, 37.512063, 14.014004, 37.459473, 14.099692));
				_cache.Add(new GeoRegionData(380, 2594, 2, "IT-CN", "CN", "Cuneo", @"", 44.384476, 7.542671, 44.430961, 7.508458, 44.353203, 7.603918));
				_cache.Add(new GeoRegionData(380, 2595, 2, "IT-CO", "CO", "Como", @"", 45.808059, 9.085176, 45.842188, 9.033906, 45.758883, 9.136950));
				_cache.Add(new GeoRegionData(380, 2596, 2, "IT-CR", "CR", "Cremona", @"", 45.133249, 10.022651, 45.156956, 9.986330, 45.124589, 10.058867));
				_cache.Add(new GeoRegionData(380, 2597, 2, "IT-CS", "CS", "Cosenza", @"", 39.298262, 16.253735, 39.332610, 16.219560, 39.276989, 16.285057));
				_cache.Add(new GeoRegionData(380, 2598, 2, "IT-CT", "CT", "Catania", @"", 37.508038, 15.082851, 37.560876, 15.028614, 37.420546, 15.125929));
				_cache.Add(new GeoRegionData(380, 2599, 2, "IT-CZ", "CZ", "Catanzaro", @"", 38.909791, 16.587651, 38.949722, 16.553669, 38.815338, 16.648489));
				_cache.Add(new GeoRegionData(380, 2600, 2, "IT-EN", "EN", "Enna", @"", 37.565591, 14.275127, 37.570552, 14.262915, 37.545665, 14.305198));
				_cache.Add(new GeoRegionData(380, 2601, 2, "IT-FC", "FC", "Forlì-Cesena", @"", 44.139643, 12.246429, 44.214441, 12.187166, 44.083961, 12.337760));
				_cache.Add(new GeoRegionData(380, 2602, 2, "IT-FE", "FE", "Ferrara", @"", 44.838123, 11.619787, 44.888083, 11.548782, 44.797794, 11.664108));
				_cache.Add(new GeoRegionData(380, 2603, 2, "IT-FG", "FG", "Foggia", @"", 41.462198, 15.544630, 41.479509, 15.518324, 41.435967, 15.583576));
				_cache.Add(new GeoRegionData(380, 2604, 2, "IT-FI", "FI", "Firenze", @"", 43.771033, 11.248000, 43.832936, 11.154036, 43.726979, 11.331090));
				_cache.Add(new GeoRegionData(380, 2605, 2, "IT-FM", "FM", "Fermo", @"", 43.162818, 13.718320, 43.178585, 13.693860, 43.147896, 13.761712));
				_cache.Add(new GeoRegionData(380, 2606, 2, "IT-FR", "FR", "Frosinone", @"", 41.639600, 13.342634, 41.665580, 13.296769, 41.603524, 13.378590));
				_cache.Add(new GeoRegionData(380, 2607, 2, "IT-GE", "GE", "Genova", @"", 44.405649, 8.946256, 44.514882, 8.716091, 44.379125, 9.065573));
				_cache.Add(new GeoRegionData(380, 2608, 2, "IT-GO", "GO", "Gorizia", @"", 45.940181, 13.620175, 45.977405, 13.552211, 45.914997, 13.637898));
				_cache.Add(new GeoRegionData(380, 2609, 2, "IT-GR", "GR", "Grosseto", @"", 42.763525, 11.112363, 42.795991, 11.083981, 42.739771, 11.170989));
				_cache.Add(new GeoRegionData(380, 2610, 2, "IT-IM", "IM", "Imperia", @"", 43.890568, 8.034665, 43.922818, 7.959864, 43.864335, 8.063017));
				_cache.Add(new GeoRegionData(380, 2611, 2, "IT-IS", "IS", "Isernia", @"", 41.596041, 14.233161, 41.617443, 14.213550, 41.578224, 14.253852));
				_cache.Add(new GeoRegionData(380, 2612, 2, "IT-KR", "KR", "Crotone", @"", 39.080793, 17.127110, 39.119241, 17.072486, 39.037921, 17.158660));
				_cache.Add(new GeoRegionData(380, 2613, 2, "IT-LC", "LC", "Lecco", @"", 45.856569, 9.397670, 45.887055, 9.378138, 45.815082, 9.424340));
				_cache.Add(new GeoRegionData(380, 2614, 2, "IT-LE", "LE", "Lecce", @"", 40.351515, 18.175016, 40.377791, 18.119994, 40.330470, 18.228594));
				_cache.Add(new GeoRegionData(380, 2615, 2, "IT-LI", "LI", "Livorno", @"", 43.548473, 10.310567, 43.595893, 10.290590, 43.483205, 10.360103));
				_cache.Add(new GeoRegionData(380, 2616, 2, "IT-LO", "LO", "Lodi", @"", 45.313804, 9.501827, 45.328310, 9.475517, 45.294781, 9.522065));
				_cache.Add(new GeoRegionData(380, 2617, 2, "IT-LT", "LT", "Latina", @"", 41.467567, 12.903596, 41.486465, 12.844528, 41.437993, 12.941881));
				_cache.Add(new GeoRegionData(380, 2618, 2, "IT-LU", "LU", "Lucca", @"", 43.837621, 10.495060, 43.930715, 10.385155, 43.768702, 10.559524));
				_cache.Add(new GeoRegionData(380, 2619, 2, "IT-MB", "MB", "Monza e della Brianza", @"", 45.623599, 9.258801, 45.742733, 9.050523, 45.536646, 9.496672));
				_cache.Add(new GeoRegionData(380, 2620, 2, "IT-MC", "MC", "Macerata", @"", 43.298426, 13.453476, 43.313307, 13.416125, 43.278976, 13.487628));
				_cache.Add(new GeoRegionData(380, 2621, 2, "IT-ME", "ME", "Messina", @"", 38.193813, 15.554015, 38.301462, 15.465919, 38.054657, 15.652794));
				_cache.Add(new GeoRegionData(380, 2622, 2, "IT-MI", "MI", "Milano", @"", 45.465454, 9.186516, 45.535972, 9.065897, 45.392167, 9.277915));
				_cache.Add(new GeoRegionData(380, 2623, 2, "IT-MN", "MN", "Mantova", @"", 45.156416, 10.791375, 45.186306, 10.740419, 45.137279, 10.806081));
				_cache.Add(new GeoRegionData(380, 2624, 2, "IT-MO", "MO", "Modena", @"", 44.648836, 10.920086, 44.694198, 10.838720, 44.606156, 10.990284));
				_cache.Add(new GeoRegionData(380, 2625, 2, "IT-MS", "MS", "Massa-Carrara", @"", 44.079324, 10.097677, 44.092735, 10.018930, 44.026691, 10.108908));
				_cache.Add(new GeoRegionData(380, 2626, 2, "IT-MT", "MT", "Matera", @"", 40.666379, 16.604319, 40.690404, 16.574005, 40.638513, 16.628197));
				_cache.Add(new GeoRegionData(380, 2627, 2, "IT-NA", "NA", "Napoli", @"", 40.851774, 14.268124, 40.915934, 14.139489, 40.791933, 14.353714));
				_cache.Add(new GeoRegionData(380, 2628, 2, "IT-NO", "NO", "Novara", @"", 45.446930, 8.622161, 45.488490, 8.573948, 45.419167, 8.657613));
				_cache.Add(new GeoRegionData(380, 2629, 2, "IT-NU", "NU", "Nuoro", @"", 40.320242, 9.326437, 40.333433, 9.279806, 40.305326, 9.343346));
				_cache.Add(new GeoRegionData(380, 2630, 2, "IT-OG", "OG", "Ogliastra", @"", 39.841053, 9.456155, 40.224973, 9.272892, 39.549232, 9.735706));
				_cache.Add(new GeoRegionData(380, 2631, 2, "IT-OR", "OR", "Oristano", @"", 39.906182, 8.588386, 39.917004, 8.578592, 39.882148, 8.609466));
				_cache.Add(new GeoRegionData(380, 2632, 2, "IT-OT", "OT", "Olbia-Tempio", @"", 40.826838, 9.278558, 41.312573, 8.828220, 40.527273, 9.741358));
				_cache.Add(new GeoRegionData(380, 2633, 2, "IT-PA", "PA", "Palermo", @"", 38.115687, 13.361267, 38.219465, 13.267420, 38.061539, 13.447156));
				_cache.Add(new GeoRegionData(380, 2634, 2, "IT-PC", "PC", "Piacenza", @"", 45.047375, 9.686581, 45.069125, 9.635065, 45.015414, 9.761231));
				_cache.Add(new GeoRegionData(380, 2635, 2, "IT-PD", "PD", "Padova", @"", 45.406434, 11.876761, 45.457500, 11.809626, 45.355507, 11.972864));
				_cache.Add(new GeoRegionData(380, 2636, 2, "IT-PE", "PE", "Pescara", @"", 42.461790, 14.216089, 42.493871, 14.152871, 42.417160, 14.254354));
				_cache.Add(new GeoRegionData(380, 2637, 2, "IT-PG", "PG", "Perugia", @"", 43.110716, 12.390827, 43.155623, 12.283855, 43.024728, 12.464024));
				_cache.Add(new GeoRegionData(380, 2638, 2, "IT-PI", "PI", "Pisa", @"", 43.722838, 10.401688, 43.739481, 10.345375, 43.674020, 10.455402));
				_cache.Add(new GeoRegionData(380, 2639, 2, "IT-PN", "PN", "Pordenone", @"", 45.962639, 12.655136, 45.995856, 12.626335, 45.918426, 12.701357));
				_cache.Add(new GeoRegionData(380, 2640, 2, "IT-PO", "PO", "Prato", @"", 43.877704, 11.102228, 43.925211, 11.020264, 43.831412, 11.147486));
				_cache.Add(new GeoRegionData(380, 2641, 2, "IT-PR", "PR", "Parma", @"", 44.801485, 10.327903, 44.839502, 10.263433, 44.755404, 10.384297));
				_cache.Add(new GeoRegionData(380, 2642, 2, "IT-PT", "PT", "Pistoia", @"", 43.930347, 10.907858, 44.010107, 10.866161, 43.880461, 10.994604));
				_cache.Add(new GeoRegionData(380, 2643, 2, "IT-PU", "PU", "Pesaro e Urbino", @"", 43.613011, 12.713512, 43.969274, 12.185450, 43.416576, 13.172519));
				_cache.Add(new GeoRegionData(380, 2644, 2, "IT-PV", "PV", "Pavia", @"", 45.184724, 9.158206, 45.226200, 9.104418, 45.165594, 9.207372));
				_cache.Add(new GeoRegionData(380, 2645, 2, "IT-PZ", "PZ", "Potenza", @"", 40.640406, 15.805604, 40.665242, 15.772165, 40.617214, 15.845493));
				_cache.Add(new GeoRegionData(380, 2646, 2, "IT-RA", "RA", "Ravenna", @"", 44.418359, 12.203529, 44.440547, 12.166801, 44.384136, 12.234216));
				_cache.Add(new GeoRegionData(380, 2647, 2, "IT-RC", "RC", "Reggio di Calabria", @"", 38.111300, 15.647291, 38.204364, 15.630224, 37.991421, 15.713315));
				_cache.Add(new GeoRegionData(380, 2648, 2, "IT-RE", "RE", "Reggio Nell'Emilia", @"", 44.698993, 10.629685, 44.755048, 10.538017, 44.649793, 10.724518));
				_cache.Add(new GeoRegionData(380, 2649, 2, "IT-RG", "RG", "Ragusa", @"", 36.926927, 14.725512, 36.938700, 14.670508, 36.898667, 14.750067));
				_cache.Add(new GeoRegionData(380, 2650, 2, "IT-RI", "RI", "Rieti", @"", 42.404509, 12.856728, 42.439260, 12.827067, 42.392851, 12.904025));
				_cache.Add(new GeoRegionData(380, 2651, 2, "IT-RM", "RM", "Roma", @"", 41.892916, 12.482519, 42.050546, 12.341707, 41.766566, 12.730288));
				_cache.Add(new GeoRegionData(380, 2652, 2, "IT-RN", "RN", "Rimini", @"", 44.057081, 12.564600, 44.115545, 12.500603, 44.018350, 12.632253));
				_cache.Add(new GeoRegionData(380, 2653, 2, "IT-RO", "RO", "Rovigo", @"", 45.069811, 11.790215, 45.115636, 11.741528, 45.033458, 11.838665));
				_cache.Add(new GeoRegionData(380, 2654, 2, "IT-SA", "SA", "Salerno", @"", 40.682440, 14.768096, 40.712163, 14.733998, 40.637323, 14.851314));
				_cache.Add(new GeoRegionData(380, 2655, 2, "IT-SI", "SI", "Siena", @"", 43.318809, 11.330757, 43.351569, 11.291758, 43.280447, 11.363539));
				_cache.Add(new GeoRegionData(380, 2656, 2, "IT-SO", "SO", "Sondrio", @"", 46.169858, 9.878767, 46.177399, 9.844438, 46.158834, 9.898327));
				_cache.Add(new GeoRegionData(380, 2657, 2, "IT-SP", "SP", "La Spezia", @"", 44.102450, 9.824082, 44.136707, 9.785096, 44.077742, 9.888580));
				_cache.Add(new GeoRegionData(380, 2658, 2, "IT-SR", "SR", "Siracusa", @"", 37.075473, 15.286586, 37.105662, 15.240530, 37.052651, 15.301262));
				_cache.Add(new GeoRegionData(380, 2659, 2, "IT-SS", "SS", "Sassari", @"", 40.725926, 8.555682, 40.753741, 8.495269, 40.698524, 8.597026));
				_cache.Add(new GeoRegionData(380, 2660, 2, "IT-SV", "SV", "Savona", @"", 44.297560, 8.464500, 44.329340, 8.434786, 44.281010, 8.504227));
				_cache.Add(new GeoRegionData(380, 2661, 2, "IT-TA", "TA", "Taranto", @"", 40.465793, 17.247778, 40.499119, 17.201738, 40.379181, 17.299025));
				_cache.Add(new GeoRegionData(380, 2662, 2, "IT-TE", "TE", "Teramo", @"", 42.661143, 13.698663, 42.681171, 13.668671, 42.642805, 13.745285));
				_cache.Add(new GeoRegionData(380, 2663, 2, "IT-TN", "TN", "Trento", @"", 46.069692, 11.121088, 46.113482, 11.097499, 46.028131, 11.158018));
				_cache.Add(new GeoRegionData(380, 2664, 2, "IT-TO", "TO", "Torino", @"", 45.070982, 7.685676, 45.133501, 7.577850, 45.006776, 7.762328));
				_cache.Add(new GeoRegionData(380, 2665, 2, "IT-TP", "TP", "Trapani", @"", 38.017617, 12.537202, 38.034239, 12.491745, 37.991856, 12.585763));
				_cache.Add(new GeoRegionData(380, 2666, 2, "IT-TR", "TR", "Terni", @"", 42.563616, 12.642660, 42.601052, 12.568564, 42.523013, 12.687601));
				_cache.Add(new GeoRegionData(380, 2667, 2, "IT-TS", "TS", "Trieste", @"", 45.649535, 13.777971, 45.697797, 13.731867, 45.606843, 13.835456));
				_cache.Add(new GeoRegionData(380, 2668, 2, "IT-TV", "TV", "Treviso", @"", 45.666901, 12.243039, 45.703067, 12.182856, 45.637401, 12.287306));
				_cache.Add(new GeoRegionData(380, 2669, 2, "IT-UD", "UD", "Udine", @"", 46.071066, 13.234579, 46.106848, 13.196241, 46.011198, 13.292083));
				_cache.Add(new GeoRegionData(380, 2670, 2, "IT-VA", "VA", "Varese", @"", 45.820598, 8.825057, 45.864524, 8.775943, 45.780512, 8.863587));
				_cache.Add(new GeoRegionData(380, 2671, 2, "IT-VB", "VB", "Verbano-Cusio-Ossola", @"", 46.139968, 8.272464, 46.464435, 7.868422, 45.766913, 8.711094));
				_cache.Add(new GeoRegionData(380, 2672, 2, "IT-VC", "VC", "Vercelli", @"", 45.320227, 8.418573, 45.341982, 8.390465, 45.298516, 8.457671));
				_cache.Add(new GeoRegionData(380, 2673, 2, "IT-VE", "VE", "Venezia", @"", 45.440847, 12.315515, 45.449315, 12.303861, 45.424669, 12.367105));
				_cache.Add(new GeoRegionData(380, 2674, 2, "IT-VI", "VI", "Vicenza", @"", 45.545478, 11.535421, 45.603931, 11.491009, 45.496512, 11.614823));
				_cache.Add(new GeoRegionData(380, 2675, 2, "IT-VR", "VR", "Verona", @"", 45.438356, 10.991657, 45.481297, 10.914742, 45.374234, 11.070320));
				_cache.Add(new GeoRegionData(380, 2676, 2, "IT-VS", "VS", "Medio Campidano", @"", 39.531738, 8.704075, 39.782981, 8.379264, 39.374975, 9.073682));
				_cache.Add(new GeoRegionData(380, 2677, 2, "IT-VT", "VT", "Viterbo", @"", 42.420676, 12.107669, 42.440542, 12.079617, 42.404465, 12.160656));
				_cache.Add(new GeoRegionData(380, 2678, 2, "IT-VV", "VV", "Vibo Valentia", @"", 38.675777, 16.098348, 38.692919, 16.075639, 38.664571, 16.127784));
				#endregion

				#region Mongolia
				_cache.Add(new GeoRegionData(496, 2679, 2, "MN-035", "35", "Orhon", @"Орхон", 49.817220, 106.074909, 49.825968, 106.051282, 49.809742, 106.091752));
				_cache.Add(new GeoRegionData(496, 2680, 2, "MN-037", "37", "Darhan-Uul", @"Дархан-Уул", 49.448929, 106.232524, 49.825965, 105.815700, 49.084558, 106.806087));
				_cache.Add(new GeoRegionData(496, 2681, 2, "MN-039", "39", "Hentii", @"Хэнтий", 47.608120, 109.937285, 49.406742, 108.350123, 46.062024, 112.697171));
				_cache.Add(new GeoRegionData(496, 2682, 2, "MN-041", "41", "Huvsgul", @"Хөвсгөл", 51.122826, 100.549073, 51.621320, 100.149564, 50.421807, 100.805568));
				_cache.Add(new GeoRegionData(496, 2683, 2, "MN-043", "43", "Hovd", @"Ховд", 48.004166, 91.640555, 48.019381, 91.610870, 47.938081, 91.670265));
				_cache.Add(new GeoRegionData(496, 2684, 2, "MN-046", "46", "Uvs", @"Увс", 50.234961, 92.674555, 50.689280, 92.216148, 49.988295, 93.431854));
				_cache.Add(new GeoRegionData(496, 2685, 2, "MN-047", "47", "Tuv", @"Төв", 51.887266, 95.626017, 53.727431, 88.798533, 49.744511, 99.269666));
				_cache.Add(new GeoRegionData(496, 2686, 2, "MN-049", "49", "Selenge", @"Сэлэнгэ", 50.005927, 106.443410, 50.480400, 104.345383, 48.497168, 108.555588));
				_cache.Add(new GeoRegionData(496, 2687, 2, "MN-051", "51", "Syhbaatar", @"Сүхбаатар", 50.227150, 106.194801, 50.265753, 106.143379, 50.180415, 106.276588));
				_cache.Add(new GeoRegionData(496, 2688, 2, "MN-053", "53", "Umnugovi", @"Өмнөговь", 49.103020, 91.721642, 49.109444, 91.711463, 49.099386, 91.730260));
				_cache.Add(new GeoRegionData(496, 2689, 2, "MN-055", "55", "Uvurhangai", @"Өвөрхангай", 45.762439, 103.091703, 47.414307, 101.133332, 44.056532, 104.655877));
				_cache.Add(new GeoRegionData(496, 2690, 2, "MN-057", "57", "Zavhan", @"Завхан", 48.238814, 96.070301, 50.041237, 93.173905, 46.583037, 99.218587));
				_cache.Add(new GeoRegionData(496, 2691, 2, "MN-059", "59", "Dundgovi", @"Дундговь", 45.582278, 106.764420, 46.779331, 103.618634, 44.179281, 108.658953));
				_cache.Add(new GeoRegionData(496, 2692, 2, "MN-061", "61", "Dornod", @"Дорнод", 47.465815, 115.392711, 50.282908, 111.963234, 46.235500, 119.924301));
				_cache.Add(new GeoRegionData(496, 2693, 2, "MN-063", "63", "Dornogovi", @"Дорноговь", 43.965388, 109.177345, 46.628413, 107.636619, 42.387329, 112.015564));
				_cache.Add(new GeoRegionData(496, 2694, 2, "MN-064", "64", "Govisymber", @"Говьсүмбэр", 46.476275, 108.557062, 46.984510, 107.945448, 45.906124, 109.003989));
				_cache.Add(new GeoRegionData(496, 2695, 2, "MN-065", "65", "Govi-Altai", @"Говь-Алтай", 45.451122, 95.850576, 47.782393, 93.063152, 42.705204, 98.466612));
				_cache.Add(new GeoRegionData(496, 2696, 2, "MN-067", "67", "Bulgan", @"Булган", 48.811944, 103.533611, 48.866408, 103.453102, 48.774688, 103.587513));
				_cache.Add(new GeoRegionData(496, 2697, 2, "MN-069", "69", "Bayunhongor", @"Баянхонгор", 46.191666, 100.717777, 46.214288, 100.686435, 46.145350, 100.736818));
				_cache.Add(new GeoRegionData(496, 2698, 2, "MN-071", "71", "Bayun-Ulgii", @"Баян-Өлгий", 48.398325, 89.662591, 50.009587, 87.749664, 46.544199, 91.926869));
				_cache.Add(new GeoRegionData(496, 2699, 2, "MN-073", "73", "Arhangai", @"Архангай", 47.897110, 100.724016, 49.198343, 98.166168, 46.826294, 103.673478));
				_cache.Add(new GeoRegionData(496, 2700, 2, "MN-1", "1", "Ulaanbaatar", @"Улаанбаатар", 47.921378, 106.905540, 47.959122, 106.699442, 47.824177, 107.104300));
				#endregion

				#region // Mexico
				/*
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2701, 1, "MX-AGU", "AGU", "Aguascalientes", @"", 21.882776, -102.294584, 21.958078, -102.355560, 21.820095, -102.230141));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2702, 1, "MX-BCN", "BCN", "Baja California", @"", 30.840633, -115.283758, 32.718665, -117.300545, 27.999336, -112.761297));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2703, 1, "MX-BCS", "BCS", "Baja California Sur", @"", 26.044444, -111.666072, 28.000231, -115.213724, 22.871875, -109.413228));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2704, 1, "MX-CAM", "CAM", "Campeche", @"", 19.830125, -90.534908, 19.876340, -90.622375, 19.784844, -90.477794));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2705, 1, "MX-CHH", "CHH", "Chihuahua", @"", 28.635277, -106.088888, 28.772908, -106.167105, 28.558677, -105.961289));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2706, 1, "MX-CHP", "CHP", "Chiapas", @"", 16.322441, -91.791061, 16.343857, -91.823076, 16.301024, -91.759047));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2707, 1, "MX-COA", "COA", "Coahuila de Zaragoza", @"", 27.058676, -101.706829, 29.879919, -103.950664, 24.541498, -99.833164));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2708, 1, "MX-COL", "COL", "Colima", @"", 19.245226, -103.733455, 19.294302, -103.783819, 19.198799, -103.673497));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2709, 1, "MX-DIF", "DIF", "Distrito Federal", @"", 19.246469, -99.101349, 19.591852, -99.364170, 19.048220, -98.940085));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2710, 1, "MX-DUR", "DUR", "Durango", @"", 24.027720, -104.653175, 24.094566, -104.715184, 23.963354, -104.575401));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2711, 1, "MX-GRO", "GRO", "Guerrero", @"", 17.439192, -99.545097, 18.886354, -102.182889, 16.315536, -98.006992));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2712, 1, "MX-GUA", "GUA", "Guanajuato", @"", 21.018111, -101.258320, 21.051636, -101.280136, 20.998607, -101.235538));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2713, 1, "MX-HID", "HID", "Hidalgo", @"", 20.091096, -98.762387, 21.397893, -99.863953, 19.596589, -97.984731));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2714, 1, "MX-JAL", "JAL", "Jalisco", @"", 20.154019, -103.914399, 22.749799, -105.694591, 18.925343, -101.509952));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2715, 1, "MX-MEX", "MEX", "México", @"", 23.634501, -102.552784, 32.718762, -118.363977, 14.534548, -86.710571));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2716, 1, "MX-MIC", "MIC", "Michoacán de Ocampo", @"", 19.566519, -101.706829, 20.393876, -103.739068, 17.913164, -100.062147));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2717, 1, "MX-MOR", "MOR", "Morelos", @"", 18.681304, -99.101349, 19.132197, -99.493942, 18.331594, -98.632783));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2718, 1, "MX-NAY", "NAY", "Nayarit", @"", 21.751384, -104.845461, 23.083758, -105.759016, 20.602794, -103.721605));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2719, 1, "MX-NLE", "NLE", "Nuevo León", @"", 25.592172, -99.996194, 27.798582, -101.206182, 23.164995, -98.422119));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2720, 1, "MX-OAX", "OAX", "Oaxaca", @"", 17.059416, -96.721621, 17.133278, -96.780676, 17.022281, -96.676276));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2721, 1, "MX-PUE", "PUE", "Puebla", @"", 19.041296, -98.206199, 19.138197, -98.282430, 18.944380, -98.103501));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2722, 1, "MX-QUE", "QUE", "Querétaro Arteaga", @"", 20.607582, -100.080729, 21.669123, -100.596215, 20.017046, -99.046732));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2723, 1, "MX-ROO", "ROO", "Quintana Roo", @"", 19.181739, -88.479137, 21.587621, -89.314385, 17.892694, -86.710621));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2724, 1, "MX-SIN", "SIN", "Sinaloa", @"", 25.825701, -108.214302, 25.986288, -108.470420, 25.664895, -107.958183));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2725, 1, "MX-SLP", "SLP", "San Luís Potosí", @"", 22.156469, -100.985540, 22.209243, -101.040047, 22.062088, -100.874759));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2726, 1, "MX-SON", "SON", "Sonora", @"", 32.062199, -114.971922, 32.137816, -115.099981, 31.986519, -114.843862));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2727, 1, "MX-TAB", "TAB", "Tabasco", @"", 17.840917, -92.618927, 18.649632, -94.129665, 17.248707, -90.977023));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2728, 1, "MX-TAM", "TAM", "Tamaulipas", @"", 24.266940, -98.836275, 27.678790, -100.143896, 22.208256, -97.144260));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2729, 1, "MX-TLA", "TLA", "Tlaxcala", @"", 19.318152, -98.237514, 19.326459, -98.247962, 19.305666, -98.231671));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2730, 1, "MX-VER", "VER", "Veracruz Llave", @"", 19.260160, -96.578338, 22.471357, -98.680091, 17.135894, -93.607742));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2731, 1, "MX-YUC", "YUC", "Yucatán", @"", 20.709878, -89.094337, 21.623665, -90.406999, 19.537373, -87.533471));
				_all.Add(new GeoRegionInfo(new GeoRegionData(484, 2732, 1, "MX-ZAC", "ZAC", "Zacatecas", @"", 22.770924, -102.583253, 22.790116, -102.619694, 22.745158, -102.554983));
				*/
				#endregion

				#region Netherlands
				_cache.Add(new GeoRegionData(528, 2733, 1, "NL-DR", "DR", "Drenthe", @"", 52.947601, 6.623058, 53.203741, 6.119850, 52.612185, 7.092935));
				_cache.Add(new GeoRegionData(528, 2734, 1, "NL-FL", "FL", "Flevoland", @"", 52.527978, 5.595350, 52.844046, 5.121347, 52.252625, 6.011667));
				_cache.Add(new GeoRegionData(528, 2735, 1, "NL-FR", "FR", "Friesland", @"", 53.164164, 5.781754, 53.514597, 4.845565, 52.800692, 6.427476));
				_cache.Add(new GeoRegionData(528, 2736, 1, "NL-GE", "GE", "Gelderland", @"", 52.045155, 5.871823, 52.522008, 4.993880, 51.733607, 6.833041));
				_cache.Add(new GeoRegionData(528, 2737, 1, "NL-GR", "GR", "Groningen", @"", 53.219383, 6.566501, 53.264725, 6.462599, 53.178684, 6.666904));
				_cache.Add(new GeoRegionData(528, 2738, 1, "NL-LI", "LI", "Limburg", @"", 50.398600, 8.079578, 50.445846, 7.983616, 50.356889, 8.157307));
				_cache.Add(new GeoRegionData(528, 2739, 1, "NL-NB", "NB", "Noord-Brabant", @"", 51.482653, 5.232168, 51.830714, 4.190081, 51.220937, 6.047724));
				_cache.Add(new GeoRegionData(528, 2740, 1, "NL-NH", "NH", "Noord-Holland", @"", 52.520586, 4.788474, 53.183332, 4.493741, 52.165771, 5.328280));
				_cache.Add(new GeoRegionData(528, 2741, 1, "NL-OV", "OV", "Overijssel", @"", 52.438781, 6.501641, 52.854234, 5.777825, 52.118023, 7.072763));
				_cache.Add(new GeoRegionData(528, 2742, 1, "NL-UT", "UT", "Utrecht", @"", 52.091790, 5.114569, 52.135852, 5.016865, 52.026111, 5.195207));
				_cache.Add(new GeoRegionData(528, 2743, 1, "NL-ZE", "ZE", "Zeeland", @"", 51.494030, 3.849681, 51.758520, 3.357962, 51.200331, 4.279691));
				_cache.Add(new GeoRegionData(528, 2744, 1, "NL-ZH", "ZH", "Zuid-Holland", @"", 52.020797, 4.493783, 52.328274, 3.838909, 51.656067, 5.149262));
				#endregion

				#region New Zealand
				_cache.Add(new GeoRegionData(554, 2745, 1, "NZ-AUK", "AUK", "Auckland", @"", -36.848459, 174.763331, -36.016755, 174.154037, -37.206703, 175.560422));
				_cache.Add(new GeoRegionData(554, 2746, 1, "NZ-BOP", "BOP", "Bay of Plenty", @"", -37.682502, 176.188023, -37.264425, 175.851787, -38.936322, 178.107559));
				_cache.Add(new GeoRegionData(554, 2747, 1, "NZ-CAN", "CAN", "Canterbury", @"", -43.35701, 171.66687, -41.724693, 169.519043, -44.989327, 173.814697));
				_cache.Add(new GeoRegionData(554, 2748, 1, "NZ-GIS", "GIS", "Gisborne", @"", -38.662334, 178.017649, -37.528322, 177.080476, -39.003256, 178.602082));
				_cache.Add(new GeoRegionData(554, 2749, 1, "NZ-HKB", "HKB", "Hawke's Bay", @"", -39.771161, 176.741637, -38.175218, 175.830011, -40.440783, 178.002017));
				_cache.Add(new GeoRegionData(554, 2750, 1, "NZ-MBH", "MBH", "Marlborough", @"", -41.63122, 173.685607, -40.772872, 173.117065, -42.489568, 174.25415));
				_cache.Add(new GeoRegionData(554, 2751, 1, "NZ-MWT", "MWT", "Manawatu-Wanganui", @"", -39.727335, 175.437557, -38.487777, 174.762086, -40.782483, 176.632784));
				_cache.Add(new GeoRegionData(554, 2752, 1, "NZ-NSN", "NSN", "Nelson", @"", -41.270631, 173.283965, -40.489841, 171.669662, -41.997321, 174.421738));
				_cache.Add(new GeoRegionData(554, 2753, 1, "NZ-NTL", "NTL", "Northland", @"", -35.579546, 173.762405, -34.393331, 172.634665, -36.396875, 174.783209));
				_cache.Add(new GeoRegionData(554, 2754, 1, "NZ-OTA", "OTA", "Otago", @"", -44.828004, 169.634525, -43.788899, 168.116388, -46.638692, 171.155418));
				_cache.Add(new GeoRegionData(554, 2755, 1, "NZ-STL", "STL", "Southland", @"", -45.848915, 167.675538, -44.290561, 166.426128, -47.28995, 169.493939));
				_cache.Add(new GeoRegionData(554, 2756, 1, "NZ-TAS", "TAS", "Tasman", @"", -41.212212, 172.734714, -40.498029, 172.093419, -42.305597, 173.231885));
				_cache.Add(new GeoRegionData(554, 2757, 1, "NZ-TKI", "TKI", "Taranaki", @"", -39.353814, 174.438272, -38.706394, 173.751323, -39.869374, 174.976195));
				_cache.Add(new GeoRegionData(554, 2758, 1, "NZ-WGN", "WGN", "Wellington", @"", -41.28646, 174.776236, -40.782990, 174.613084, -41.610383, 176.279796));
				_cache.Add(new GeoRegionData(554, 2759, 1, "NZ-WKO", "WKO", "Waikato", @"", -38.059426, 175.437557, -36.428719, 174.613423, -39.300639, 176.663295));
				_cache.Add(new GeoRegionData(554, 2760, 1, "NZ-WTC", "WTC", "West Coast", @"", -42.406418, 171.691155, -41.167307, 168.052907, -44.774375, 172.480018));
				#endregion

				#region Russia
				_cache.Add(new GeoRegionData(643, 2761, 2, "RU-AD", "AD", "Adygea republic", @"Адыгея республика", 44.822915, 40.175446, 45.216840, 38.681853, 43.757678, 40.774497));
				_cache.Add(new GeoRegionData(643, 2762, 2, "RU-AL", "AL", "Altai republic", @"Алтай республика", 50.618192, 86.219930, 52.659519, 83.923075, 49.069900, 89.869532));
				_cache.Add(new GeoRegionData(643, 2763, 2, "RU-ALT", "ALT", "Altai krai", @"Алтайский край", 51.793629, 82.675859, 54.450860, 77.889155, 50.639513, 87.169088));
				_cache.Add(new GeoRegionData(643, 2764, 2, "RU-AMU", "AMU", "Amur oblast", @"Амурская область", 54.603506, 127.480172, 57.058533, 119.667950, 48.852223, 134.920254));
				_cache.Add(new GeoRegionData(643, 2765, 2, "RU-ARK", "ARK", "Arkhangelsk oblast", @"Архангельская область", 63.285280, 42.588419, 66.783100, 35.493293, 60.632123, 50.507104));
				_cache.Add(new GeoRegionData(643, 2766, 2, "RU-AST", "AST", "Astrakhan oblast", @"Астраханская область", 46.132116, 48.061011, 48.865344, 44.970761, 45.172556, 49.309981));
				_cache.Add(new GeoRegionData(643, 2767, 2, "RU-BA", "BA", "Bashkortostan republic", @"Башкортостан республика", 54.231217, 56.164525, 56.533519, 53.157997, 51.571226, 60.00295));
				_cache.Add(new GeoRegionData(643, 2768, 2, "RU-BEL", "BEL", "Belgorod oblast", @"Белгородская область", 50.710692, 37.753337, 51.432561, 35.328527, 49.796403, 39.275127));
				_cache.Add(new GeoRegionData(643, 2769, 2, "RU-BRY", "BRY", "Bryansk oblast", @"Брянская область", 53.040859, 33.269089, 54.036299, 31.242104, 51.844038, 35.321278));
				_cache.Add(new GeoRegionData(643, 2770, 2, "RU-BU", "BU", "Buryatia republic", @"Бурятия республика", 54.833114, 112.406052, 57.249440, 98.628668, 49.960000, 116.922295));
				_cache.Add(new GeoRegionData(643, 2771, 2, "RU-CE", "CE", "Chechen republic", @"Чеченская республика", 43.402330, 45.718746, 44.010791, 44.834384, 42.475582, 46.649418));
				_cache.Add(new GeoRegionData(643, 2772, 2, "RU-CHE", "CHE", "Chelyabinsk oblast", @"Челябинская область", 54.431942, 60.878896, 56.364883, 57.130071, 51.981232, 63.349445));
				_cache.Add(new GeoRegionData(643, 2773, 2, "RU-CHU", "CHU", "Chukotka AO", @"Чукотский АО", 65.629835, 171.695215, 71.611961, 157.732107, 61.809666, -168.999679));
				_cache.Add(new GeoRegionData(643, 2774, 2, "RU-CU", "CU", "Chuvashia republic", @"Чувашская республика", 55.559599, 46.928353, 56.329965, 45.910572, 54.626048, 48.416765));
				_cache.Add(new GeoRegionData(643, 2775, 2, "RU-DA", "DA", "Dagestan republic", @"Дагестан республика", 42.143188, 47.094979, 44.994961, 45.108820, 41.185747, 48.591326));
				_cache.Add(new GeoRegionData(643, 2776, 2, "RU-IN", "IN", "Ingushetia republic", @"Ингушетия республика", 43.405169, 44.820299, 43.610796, 44.476931, 42.614809, 45.190234));
				_cache.Add(new GeoRegionData(643, 2777, 2, "RU-IRK", "IRK", "Irkutsk oblast", @"Иркутская область", 56.132142, 103.948625, 64.316956, 95.657739, 51.137316, 119.130688));
				_cache.Add(new GeoRegionData(643, 2778, 2, "RU-IVA", "IVA", "Ivanovo oblast", @"Ивановская область", 57.105685, 41.483008, 57.742722, 39.377952, 56.350396, 43.305854));
				_cache.Add(new GeoRegionData(643, 2779, 2, "RU-KAM", "KAM", "Kamchatka krai", @"Камчатский край", 61.434398, 166.788413, 64.942804, 155.548205, 50.869097, 174.50079));
				_cache.Add(new GeoRegionData(643, 2780, 2, "RU-KB", "KB", "Kabardino-Balkar republic", @"Кабардино-Балкарская республика", 43.393246, 43.562849, 44.022814, 42.398120, 42.889616, 44.47055));
				_cache.Add(new GeoRegionData(643, 2781, 2, "RU-KC", "KC", "Karachay-Cherkess republic", @"Карачаево-Черкесская республика", 43.884514, 41.730394, 44.496979, 40.683370, 43.187489, 42.678032));
				_cache.Add(new GeoRegionData(643, 2782, 2, "RU-KDA", "KDA", "Krasnodar krai", @"Краснодарский край", 45.641528, 39.705597, 46.880237, 36.538387, 43.384864, 41.747644));
				_cache.Add(new GeoRegionData(643, 2783, 2, "RU-KEM", "KEM", "Kemerovo oblast", @"Кемеровская область", 54.757464, 87.405528, 56.835120, 84.450098, 52.160544, 89.399602));
				_cache.Add(new GeoRegionData(643, 2784, 2, "RU-KGD", "KGD", "Kaliningrad oblast", @"Калининградская область", 54.823529, 21.481616, 55.294446, 19.638742, 54.318845, 22.886888));
				_cache.Add(new GeoRegionData(643, 2785, 2, "RU-KGN", "KGN", "Kurgan oblast", @"Курганская область", 55.448154, 65.118097, 56.842082, 61.966103, 54.184864, 68.725487));
				_cache.Add(new GeoRegionData(643, 2786, 2, "RU-KHA", "KHA", "Khabarovsk krai", @"Хабаровский край", 50.588843, 135.000000, 62.524612, 130.388640, 46.634719, 147.203851));
				_cache.Add(new GeoRegionData(643, 2787, 1, "RU-KHM", "KHM", "KHM", @"", 25.670350, 94.109515, 25.700628, 94.087514, 25.648799, 94.126095));
				_cache.Add(new GeoRegionData(643, 2788, 2, "RU-KIR", "KIR", "Kirov oblast", @"Кировская область", 58.419852, 50.209724, 61.062916, 46.261811, 56.061146, 53.9431));
				_cache.Add(new GeoRegionData(643, 2789, 2, "RU-KK", "KK", "Khakassia republic", @"Хакасия республика", 53.045228, 90.398214, 55.431886, 87.875836, 51.284212, 91.924913));
				_cache.Add(new GeoRegionData(643, 2790, 2, "RU-KL", "KL", "Kalmykia republic", @"Калмыкия республика", 46.567684, 45.773161, 48.274318, 41.632715, 44.763935, 47.601117));
				_cache.Add(new GeoRegionData(643, 2791, 2, "RU-KLU", "KLU", "Kaluga oblast", @"Калужская область", 54.387266, 35.188909, 55.340263, 33.446577, 53.275996, 37.275648));
				_cache.Add(new GeoRegionData(643, 2792, 2, "RU-KO", "KO", "Komi republic", @"Коми республика", 63.863053, 54.831268, 68.422876, 45.404656, 59.196101, 66.252321));
				_cache.Add(new GeoRegionData(643, 2793, 2, "RU-KOS", "KOS", "Kostroma oblast", @"Костромская область", 58.550106, 43.954110, 59.620380, 40.399761, 57.277828, 47.644647));
				_cache.Add(new GeoRegionData(643, 2794, 2, "RU-KR", "KR", "Karelia republic", @"Карелия республика", 63.155870, 32.990555, 66.673264, 29.309960, 60.685767, 37.932051));
				_cache.Add(new GeoRegionData(643, 2795, 2, "RU-KRS", "KRS", "Kursk oblast", @"Курская область", 51.763402, 35.381181, 52.440509, 34.082188, 50.903424, 38.525757));
				_cache.Add(new GeoRegionData(643, 2796, 2, "RU-KYA", "KYA", "Krasnoyarsk krai", @"Красноярский край", 64.247975, 95.110417, 81.266309, 76.111751, 51.773457, 113.920995));
				_cache.Add(new GeoRegionData(643, 2797, 2, "RU-LEN", "LEN", "Leningrad oblast", @"Ленинградская область", 60.079320, 31.892664, 61.330116, 27.740038, 58.414717, 35.696131));
				_cache.Add(new GeoRegionData(643, 2798, 2, "RU-LIP", "LIP", "Lipetsk oblast", @"Липецкая область", 52.526470, 39.203226, 53.584863, 37.722447, 51.886779, 40.764882));
				_cache.Add(new GeoRegionData(643, 2799, 2, "RU-MAG", "MAG", "Magadan oblast", @"Магаданская область", 62.664341, 153.914990, 66.336092, 144.722207, 58.833461, 163.482784));
				_cache.Add(new GeoRegionData(643, 2800, 2, "RU-ME", "ME", "Mari El republic", @"Марий Эл республика", 56.438457, 47.960775, 57.343631, 45.619734, 55.830634, 50.200065));
				_cache.Add(new GeoRegionData(643, 2801, 2, "RU-MO", "MO", "Mordovia republic", @"Мордовия республика", 54.236944, 44.068396, 55.188243, 42.170382, 53.654297, 46.711374));
				_cache.Add(new GeoRegionData(643, 2802, 2, "RU-MOS", "MOS", "Moscow oblast", @"Московская область", 55.752168, 37.588643, 55.758447, 37.572636, 55.745889, 37.604651));
				_cache.Add(new GeoRegionData(643, 2803, 2, "RU-MOW", "MOW", "Moscow", @"Москва", 55.755826, 37.617300, 56.009657, 37.319328, 55.489927, 37.945661));
				_cache.Add(new GeoRegionData(643, 2804, 2, "RU-MUR", "MUR", "Murmansk oblast", @"Мурманская область", 67.844267, 35.088410, 69.954258, 28.415943, 66.044111, 41.401708));
				_cache.Add(new GeoRegionData(643, 2805, 1, "RU-NEN", "NEN", "NEN", @"Ненецкий автономный округ", 30.348879, -81.876811, 30.359009, -81.889073, 30.340743, -81.845009));
				_cache.Add(new GeoRegionData(643, 2806, 2, "RU-NGR", "NGR", "Novgorod oblast", @"Новгородская область", 58.242755, 32.566519, 59.445152, 29.623057, 56.917883, 36.219529));
				_cache.Add(new GeoRegionData(643, 2807, 2, "RU-NIZ", "NIZ", "Nizhny Novgorod oblast", @"Нижегородская область", 55.799515, 44.029676, 58.088995, 41.775117, 54.471866, 47.747376));
				_cache.Add(new GeoRegionData(643, 2808, 2, "RU-NVS", "NVS", "Novosibirsk oblast", @"Новосибирская область", 55.446713, 80.104392, 57.236193, 75.087855, 53.290919, 85.117563));
				_cache.Add(new GeoRegionData(643, 2809, 2, "RU-OMS", "OMS", "Omsk oblast", @"Омская область", 55.055466, 73.316734, 58.574149, 70.354737, 53.436968, 76.303768));
				_cache.Add(new GeoRegionData(643, 2810, 2, "RU-ORE", "ORE", "Orenburg oblast", @"Оренбургская область", 51.763402, 54.618818, 54.365830, 50.768394, 50.500679, 61.702417));
				_cache.Add(new GeoRegionData(643, 2811, 2, "RU-ORL", "ORL", "Oryol oblast", @"Орловская область", 52.745018, 36.484962, 53.637215, 34.791767, 51.936744, 38.064479));
				_cache.Add(new GeoRegionData(643, 2812, 2, "RU-PER", "PER", "Perm krai", @"Пермский край", 58.823192, 56.587248, 61.662844, 51.777268, 56.109045, 59.492162));
				_cache.Add(new GeoRegionData(643, 2813, 2, "RU-PNZ", "PNZ", "Penza oblast", @"Пензенская область", 53.141210, 44.094004, 54.028109, 42.091744, 52.304836, 46.985298));
				_cache.Add(new GeoRegionData(643, 2814, 2, "RU-PRI", "PRI", "Primorski krai", @"Приморский край", 45.052564, 135.000000, 48.458705, 130.400135, 42.294203, 139.021501));
				_cache.Add(new GeoRegionData(643, 2815, 2, "RU-PSK", "PSK", "Pskov oblast", @"Псковская область", 56.770859, 29.094009, 59.018851, 27.323293, 55.589601, 31.516264));
				_cache.Add(new GeoRegionData(643, 2816, 2, "RU-ROS", "ROS", "Rostov oblast", @"Ростовская область", 47.685324, 41.825895, 50.212328, 38.220563, 45.952120, 44.322544));
				_cache.Add(new GeoRegionData(643, 2817, 2, "RU-RYA", "RYA", "Ryazan oblast", @"Рязанская область", 54.387596, 41.259566, 55.366111, 38.665135, 53.312384, 42.694238));
				_cache.Add(new GeoRegionData(643, 2818, 2, "RU-SA", "SA", "Sakha", @"Саха (Якутия) республика", 66.761345, 124.123753, 76.757514, 105.529347, 55.490660, 162.858423));
				_cache.Add(new GeoRegionData(643, 2819, 2, "RU-SAK", "SAK", "Sakhalin oblast", @"Сахалинская область", 49.980784, 143.373812, 54.416034, 141.196450, 45.889285, 144.752776));
				_cache.Add(new GeoRegionData(643, 2820, 2, "RU-SAM", "SAM", "Samara oblast", @"Самарская область", 53.418383, 50.472552, 54.678024, 47.924687, 51.773957, 52.555451));
				_cache.Add(new GeoRegionData(643, 2821, 2, "RU-SAR", "SAR", "Saratov oblast", @"Саратовская область", 51.836926, 46.753939, 52.814547, 42.513297, 49.802167, 50.839508));
				_cache.Add(new GeoRegionData(643, 2822, 2, "RU-SE", "SE", "North Ossetia-Alania republic", @"Северная Осетия-Алания республика", 43.045130, 44.287097, 43.839755, 43.410435, 42.547022, 44.956779));
				_cache.Add(new GeoRegionData(643, 2823, 2, "RU-SMO", "SMO", "Smolensk oblast", @"Смоленская область", 54.988299, 32.667737, 56.070943, 30.748675, 53.414513, 35.392044));
				_cache.Add(new GeoRegionData(643, 2824, 2, "RU-SPE", "SPE", "Saint Petersburg", @"Санкт-Петербург", 59.934280, 30.335098, 60.089675, 30.090332, 59.745215, 30.559783));
				_cache.Add(new GeoRegionData(643, 2825, 2, "RU-STA", "STA", "Stavropol krai", @"Ставропольский край", 44.668099, 43.520214, 46.231539, 40.843062, 43.658068, 45.718975));
				_cache.Add(new GeoRegionData(643, 2826, 2, "RU-SVE", "SVE", "Sverdlovsk oblast", @"Свердловская область", 59.007735, 61.931622, 61.945903, 57.236015, 56.056233, 66.178652));
				_cache.Add(new GeoRegionData(643, 2827, 2, "RU-TA", "TA", "Tatarstan republic", @"Татарстан республика", 55.180236, 50.726394, 56.677216, 47.258647, 53.976026, 54.260289));
				_cache.Add(new GeoRegionData(643, 2828, 2, "RU-TAM", "TAM", "Tambov oblast", @"Тамбовская область", 52.641658, 41.421645, 53.822888, 39.917096, 51.589638, 43.244815));
				_cache.Add(new GeoRegionData(643, 2829, 2, "RU-TOM", "TOM", "Tomsk oblast", @"Томская область", 58.896988, 82.676550, 61.033530, 75.057459, 55.668640, 89.375326));
				_cache.Add(new GeoRegionData(643, 2830, 2, "RU-TUL", "TUL", "Tula oblast", @"Тульская область", 54.163768, 37.564950, 54.850572, 35.896326, 52.955600, 38.952968));
				_cache.Add(new GeoRegionData(643, 2831, 2, "RU-TVE", "TVE", "Tver oblast", @"Тверская область", 57.002165, 33.985314, 58.872110, 30.776834, 55.632269, 38.31839));
				_cache.Add(new GeoRegionData(643, 2832, 2, "RU-TY", "TY", "Tuva republic", @"Тыва республика", 51.887266, 95.626017, 53.727431, 88.798533, 49.744511, 99.269666));
				_cache.Add(new GeoRegionData(643, 2833, 2, "RU-TYU", "TYU", "Tyumens oblast", @"Тюменская область", 56.963438, 66.948278, 59.989689, 64.827799, 55.147014, 75.194198));
				_cache.Add(new GeoRegionData(643, 2834, 2, "RU-UD", "UD", "Udmurtia republic", @"Удмуртская республика", 57.067021, 53.027794, 58.545039, 51.122602, 55.864368, 54.427546));
				_cache.Add(new GeoRegionData(643, 2835, 2, "RU-ULY", "ULY", "Ulyanovsk oblast", @"Ульяновская область", 53.979335, 47.776242, 54.891972, 45.794927, 52.538088, 50.239164));
				_cache.Add(new GeoRegionData(643, 2836, 2, "RU-VGG", "VGG", "Volgograd oblast", @"Волгоградская область", 49.760452, 45.000000, 51.244303, 41.167563, 47.441459, 47.428742));
				_cache.Add(new GeoRegionData(643, 2837, 2, "RU-VLA", "VLA", "Vladimir oblast", @"Владимирская область", 56.155346, 40.592668, 56.816970, 38.272862, 55.103193, 42.977175));
				_cache.Add(new GeoRegionData(643, 2838, 2, "RU-VLG", "VLG", "Vologda oblast", @"Вологодская область", 59.870671, 40.655541, 61.607277, 34.716117, 58.483176, 47.157884));
				_cache.Add(new GeoRegionData(643, 2839, 2, "RU-VOR", "VOR", "Voronezh oblast", @"Воронежская область", 50.858971, 39.864437, 52.102429, 38.137080, 49.556056, 42.944786));
				_cache.Add(new GeoRegionData(643, 2840, 1, "RU-YAN", "YAN", "YAN", @"", 9.041163, 99.015828, 9.065535, 98.992138, 9.034737, 99.020695));
				_cache.Add(new GeoRegionData(643, 2841, 2, "RU-YAR", "YAR", "Yaroslavl oblast", @"Ярославская область", 57.899152, 38.838863, 58.950021, 37.324323, 56.540105, 41.178673));
				_cache.Add(new GeoRegionData(643, 2842, 1, "RU-YEV", "YEV", "YEV", @"", 68.304167, -133.482778, 68.313094, -133.510568, 68.300535, -133.449436));
				_cache.Add(new GeoRegionData(643, 2843, 2, "RU-ZAB", "ZAB", "ZAB", @"Забайкальский край", 52.179003, 20.503456, 52.185845, 20.487448, 52.172161, 20.519463));
				#endregion

				#region //Singapore
				/*
				_all.Add(new GeoRegionInfo(new GeoRegionData(702, 2844, 1, "SG-01", "1", "Central Singapore", @"", 1.341832, 103.860876, 1.277414, 103.823013, 1.394817, 103.862467));
				_all.Add(new GeoRegionInfo(new GeoRegionData(702, 2845, 1, "SG-02", "2", "North-East", @"", -20.903055, 27.455638, -21.571556, 27.21199, -20.473381, 28.013578));
				_all.Add(new GeoRegionInfo(new GeoRegionData(702, 2846, 1, "SG-03", "3", "North-West", @"", -26.663859, 25.283758, -28.112206, 22.629029, -24.636628, 28.298348));
				_all.Add(new GeoRegionInfo(new GeoRegionData(702, 2847, 1, "SG-04", "4", "South-East", @"", -24.936609, 25.804852, -25.470963, 25.543854, -24.511588, 26.181363));
				_all.Add(new GeoRegionInfo(new GeoRegionData(702, 2848, 1, "SG-05", "5", "South-West", @"", 44.466994, -73.17096, 44.405378, -73.233424, 44.500959, -73.134698)
				*/
				#endregion

				// set initialized flag
				_initialized = true;
			}
		}
	}
}