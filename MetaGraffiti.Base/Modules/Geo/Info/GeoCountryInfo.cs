using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Base.Common;
using MetaGraffiti.Base.Modules.Geo.Data;

namespace MetaGraffiti.Base.Modules.Geo.Info
{
	public class GeoCountryInfo
	{
		// ==================================================
		// Internals
		protected GeoCountryData _data;


		// ==================================================
		// Constructors
		public GeoCountryInfo(GeoCountryData data) { _data = data; }


		// ==================================================
		// Properties

		// --------------------------------------------------
		// Inferred
		public int CountryID => _data.CountryID;

		/*
		public double Latitude => _data.Center.Latitude;
		public double Longitude => _data.Center.Longitude;
		*/

		public IGeoLatLon Center => _data.Center;
		public IGeoPerimeter Bounds => _data.Bounds;
		public GeoContinents Continent => _data.Continent;

		public int Division => _data.Division;

		public string ISO2 => _data.ISO2;
		public string ISO3 => _data.ISO3;
		public string OC => _data.OC;

		public string Abbr10 => _data.Abbr10;
		public string Abbr15 => _data.Abbr15;
		public string Abbr30 => _data.Abbr30;

		public string Name => _data.Name;
		public string NameLocal => _data.NameLocal;
		public string NameLong => _data.NameLong;
		public string NameLocalLong => _data.NameLocalLong;

		// --------------------------------------------------
		// Derived
		public IEnumerable<GeoRegionInfo> Regions { get { return GeoRegionInfo.ListByCountry(CountryID); } }
		public bool HasRegions { get { return Regions != null && Regions.Any(); } }

		// --------------------------------------------------
		// Instance
		public GeoTimezoneInfo Timezone { get; set; }


		// ==================================================
		// Methods
		public bool IsSame(GeoCountryInfo country)
		{
			if (country == null) return false;
			return (CountryID == country.CountryID);
		}

		// ==================================================
		// Properties

		// --------------------------------------------------
		// Methods
		public static List<GeoCountryInfo> All { get { return Cache.ToInfo().ToList(); } }

		// --------------------------------------------------
		// Methods
		public static GeoCountryInfo ByID(int countryID)
		{
			return Cache.FirstOrDefault(x => x.CountryID == countryID).ToInfo();
		}

		public static GeoCountryInfo ByISO(string iso)
		{
			if (String.IsNullOrWhiteSpace(iso)) return null;

			if (iso.Length == 2)
				return Cache.FirstOrDefault(x => x.ISO2 == iso.ToUpperInvariant()).ToInfo();
			else if (iso.Length == 3)
				return Cache.FirstOrDefault(x => x.ISO3 == iso.ToUpperInvariant()).ToInfo();
			else
				return null;
		}

		public static GeoCountryInfo ByName(string name, bool deep = false)
		{
			if (String.IsNullOrWhiteSpace(name)) return null;

			var c = Cache.FirstOrDefault(x => String.Compare(x.Name, name, true) == 0);
			if (c == null && deep)
			{
				c = Cache.FirstOrDefault(x => String.Compare(x.NameLocal, name, true) == 0);
				if (c == null) c = Cache.FirstOrDefault(x => String.Compare(x.NameLong, name, true) == 0);
				if (c == null) c = Cache.FirstOrDefault(x => String.Compare(x.NameLocalLong, name, true) == 0);
			}

			return c.ToInfo();
		}

		public static GeoCountryInfo Find(string text)
		{
			if (String.IsNullOrWhiteSpace(text)) return null;

			text = text.Trim();

			if (text.Length == 2 || text.Length == 3) return ByISO(text);

			var c = Cache.FirstOrDefault(x => String.Compare(x.Name, text, true) == 0);
			if (c == null)
			{
				c = Cache.FirstOrDefault(x => String.Compare(x.NameLocal, text, true) == 0);
				if (c == null) c = Cache.FirstOrDefault(x => String.Compare(x.NameLong, text, true) == 0);
				if (c == null) c = Cache.FirstOrDefault(x => String.Compare(x.NameLocalLong, text, true) == 0);
			}

			if (c == null)
			{
				text = TextTranslate.StripAccents(text);
				c = Cache.FirstOrDefault(x => String.Compare(x.NameLocal, text, true) == 0);
				if (c == null) c = Cache.FirstOrDefault(x => String.Compare(x.NameLong, text, true) == 0);
				if (c == null) c = Cache.FirstOrDefault(x => String.Compare(x.NameLocalLong, text, true) == 0);
			}

			return c.ToInfo();
		}

		public static IEnumerable<GeoCountryInfo> ListByLocation(IGeoLatLon point)
		{
			return Cache.Where(x => x.Bounds.Contains(point)).ToInfo();
		}

		// --------------------------------------------------
		// Globals
		private static bool _initialized = false;
		private static List<GeoCountryData> _cache = new List<GeoCountryData>();
		private static IEnumerable<GeoCountryData> Cache { get { if (!_initialized) Initialize(); return _cache; } }

		private static void Initialize()
		{
			lock (_cache)
			{
				// check if we are already initialized after a wait
				if (_initialized) return;

				#region Countries

				_cache.Add(new GeoCountryData(GeoContinents.Asia, 4, 2, "AF", "AFG", "AF", "Afghanistan", "Islamic Republic of Afghanistan", "Afghanestan", "Jomhuri-ye Eslami-ye Afghanestan", "AFG", "Afghanistan", "Afghanistan", 33.939110, 67.709953, 38.490876, 60.517000, 29.377200, 74.889861));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 8, 2, "AL", "ALB", "72", "Albania", "Republic of Albania", "Shqiperia", "Republika e Shqiperise", "Albania", "Albania", "Albania", 41.153332, 20.168331, 42.661081, 19.263904, 39.644729, 21.057239));
				_cache.Add(new GeoCountryData(GeoContinents.Antarctica, 10, 2, "AQ", "ATA", "", "Antarctica", "Antarctica", "", "", "Antarctica", "Antarctica", "Antarctica", -77.000000, 0.000000, -61.465019, -110.566406, -82.221655, -5.097656));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 12, 2, "DZ", "DZA", "AL", "Algeria", "People's Democratic Republic of Algeria", "Al Jaza'ir", "Al Jumhuriyah al Jaza'iriyah ad Dimuqratiyah ash Sha'biyah", "Algeria", "Algeria", "Algeria", 27.225727, 2.492945, 37.089820, -8.666667, 18.968147, 11.999999));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 16, 2, "AS", "ASM", "", "American Samoa", "Territory of American Samoa", "", "", "AS", "American Samoa", "American Samoa", -14.305941, -170.696200, -14.229404, -170.846822, -14.382477, -170.545578));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 20, 2, "AD", "AND", "AD", "Andorra", "Principality of Andorra", "Andorra", "Principat d'Andorra", "Andorra", "Andorra", "Andorra", 42.506285, 1.521801, 42.655791, 1.408705, 42.428748, 1.786639));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 24, 2, "AO", "AGO", "AN", "Angola", "Republic of Angola", "Angola", "Republica de Angola", "Angola", "Angola", "Angola", -11.202692, 17.873887, -4.387944, 11.669562, -18.039104, 24.084444));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 28, 2, "AG", "ATG", "AG", "Antigua and Barbuda", "Antigua and Barbuda", "", "", "ATG", "Antigua/Barbuda", "Antigua and Barbuda", 17.060816, -61.796428, 17.176495, -61.909820, 16.997382, -61.657419));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 31, 2, "AZ", "AZE", "AZ", "Azerbaijan", "Republic of Azerbaijan", "Azarbaycan", "Azarbaycan Respublikasi", "Azerbaijan", "Azerbaijan", "Azerbaijan", 40.143105, 47.576927, 41.912340, 44.764683, 38.391990, 50.368065));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 32, 2, "AR", "ARG", "8", "Argentina", "Argentine Republic", "Argentina", "Republica Argentina", "Argentina", "Argentina", "Argentina", -38.416097, -63.616672, -21.780813, -73.560360, -55.057714, -53.637481));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 36, 1, "AU", "AUS", "9", "Australia", "Commonwealth of Australia", "", "", "Australia", "Australia", "Australia", -25.274398, 133.775136, -9.226805, 112.923972, -43.658327, 153.638673));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 40, 1, "AT", "AUT", "10", "Austria", "Republic of Austria", "Oesterreich", "Republik Oesterreich", "Austria", "Austria", "Austria", 47.516231, 14.550072, 49.020608, 9.530783, 46.372335, 17.160686));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 44, 2, "BS", "BHS", "BH", "Bahamas", "Bahamas", "", "", "Bahamas", "Bahamas", "Bahamas", 25.034280, -77.396280, 27.263362, -80.474946, 20.912131, -72.712068));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 48, 2, "BH", "BHR", "BB", "Bahrain", "Kingdom of Bahrain", "Al Bahrayn", "Mamlakat al Bahrayn", "Bahrain", "Bahrain", "Bahrain", 26.066700, 50.557700, 26.326528, 50.378150, 25.579840, 50.822863));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 50, 2, "BD", "BGD", "BG", "Bangladesh", "People's Republic of Bangladesh", "Banladesh", "Gana Prajatantri Banladesh", "Bangladesh", "Bangladesh", "Bangladesh", 23.684994, 90.356331, 26.634243, 88.008588, 20.754380, 92.680115));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 51, 2, "AM", "ARM", "AM", "Armenia", "Republic of Armenia", "Hayastan", "Hayastani Hanrapetut'yun", "Armenia", "Armenia", "Armenia", 40.069099, 45.038189, 41.300993, 43.447211, 38.840244, 46.634222));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 52, 2, "BB", "BRB", "BA", "Barbados", "", "", "", "Barbados", "Barbados", "Barbados", 13.193887, -59.543198, 13.335126, -59.651030, 13.044999, -59.420097));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 56, 1, "BE", "BEL", "11", "Belgium", "Kingdom of Belgium", "Belgique/Belgie", "Royaume de Belgique/Koninkrijk Belgie", "Belgium", "Belgium", "Belgium", 50.503887, 4.469936, 51.505144, 2.544940, 49.497013, 6.408124));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 60, 2, "BM", "BMU", "", "Bermuda", "", "", "", "Bermuda", "Bermuda", "Bermuda", 32.307800, -64.750500, 32.391305, -64.886788, 32.247050, -64.647377));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 64, 2, "BT", "BTN", "BT", "Bhutan", "Kingdom of Bhutan", "Druk Yul", "Druk Gyalkhap", "Bhutan", "Bhutan", "Bhutan", 27.514162, 90.433601, 28.360825, 88.746473, 26.702016, 92.125232));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 68, 2, "BO", "BOL", "13", "Bolivia", "Plurinational State of Bolivia", "", "", "Bolivia", "Bolivia", "Bolivia", -16.290154, -63.588653, -9.669323, -69.644990, -22.898089, -57.453803));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 70, 2, "BA", "BIH", "BC", "Bosnia and Herzegovina", "", "Bosna i Hercegovina", "", "BIH", "Bosnia/Herzegov", "Bosnia and Herzegovina", 43.915886, 17.679076, 45.276626, 15.722366, 42.556406, 19.621935));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 72, 2, "BW", "BWA", "BW", "Botswana", "Republic of Botswana", "Botswana", "Republic of Botswana", "Botswana", "Botswana", "Botswana", -22.328474, 24.684866, -17.778137, 19.998905, -26.907545, 29.375303));
				_cache.Add(new GeoCountryData(GeoContinents.Antarctica, 74, 2, "BV", "BVT", "", "Bouvet Island", "Bouvet Island", "", "", "BVT", "Bouvet Island", "Bouvet Island", -54.420791, 3.346449, -54.400322, 3.285149, -54.451543, 3.487975));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 76, 2, "BR", "BRA", "14", "Brazil", "Federative Republic of Brazil", "Brasil", "Republica Federativa do Brasil", "Brazil", "Brazil", "Brazil", -14.235004, -54.322394, 4.587802, -74.056850, -33.868150, -34.587939));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 84, 2, "BZ", "BLZ", "BZ", "Belize", "Belize", "", "", "Belize", "Belize", "Belize", 17.189877, -88.497650, 18.495941, -89.227587, 15.885618, -87.491726));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 86, 2, "IO", "IOT", "", "British Indian Ocean Territory", "British Indian Ocean Territory", "", "", "BIOT", "BIOT", "British Indian Ocean Territory", -7.334755, 72.424232, -7.225440, 72.353768, -7.444070, 72.494696));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 90, 2, "SB", "SLB", "SO", "Solomon Islands", "Solomon Islands", "Solomon Islands", "", "SLB", "Solomon Islands", "Solomon Islands", -9.645710, 160.156194, -6.589240, 155.486240, -11.863458, 162.752884));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 92, 2, "VG", "VGB", "", "British Virgin Islands", "British Virgin Islands", "", "", "VGB", "Virgin Is", "British Virgin Islands", 18.420695, -64.639968, 18.529889, -64.850460, 18.306332, -64.320402));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 96, 2, "BN", "BRN", "", "Brunei Darussalam", "Brunei Darussalam", "", "", "BRN", "BRN", "Brunei Darussalam", 4.535277, 114.727669, 5.047166, 114.076063, 4.002508, 115.363562));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 100, 2, "BG", "BGR", "15", "Bulgaria", "Republic of Bulgaria", "Balgariya", "Republika Balgariya", "Bulgaria", "Bulgaria", "Bulgaria", 42.733883, 25.485830, 44.215124, 22.357344, 41.235446, 28.609263));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 104, 2, "MM", "MMR", "12", "Myanmar (Burma)", "Myanmar", "Myanmar", "", "Myanmar", "Burma (Myanmar)", "Burma (Myanmar)", 21.913965, 95.956223, 28.547835, 92.171808, 9.599032, 101.170271));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 108, 2, "BI", "BDI", "BI", "Burundi", "Republic of Burundi", "Burundi", "Republique du Burundi/Republika y'u Burundi", "Burundi", "Burundi", "Burundi", -3.373056, 29.918886, -2.309987, 29.000993, -4.469228, 30.850172));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 112, 2, "BY", "BLR", "", "Belarus", "Republic of Belarus", "Byelarus'", "Respublika Byelarus'", "Belarus", "Belarus", "Belarus", 53.709807, 27.953389, 56.171871, 23.178337, 51.262011, 32.776820));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 116, 2, "KH", "KHM", "KH", "Cambodia", "Kingdom of Cambodia", "Kampuchea", "Preahreacheanachakr Kampuchea", "Cambodia", "Cambodia", "Cambodia", 12.565679, 104.990963, 14.690179, 102.333542, 9.276808, 107.627687));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 120, 2, "CM", "CMR", "17", "Cameroon", "Republic of Cameroon", "Cameroun/Cameroon", "Republique du Cameroun/Republic of Cameroon", "Cameroon", "Cameroon", "Cameroon", 7.369722, 12.354722, 13.083399, 8.494763, 1.655899, 16.194407));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 124, 2, "CA", "CAN", "18", "Canada", "Canada", "", "", "Canada", "Canada", "Canada", 56.130366, -106.346771, 70.000000, -142.000000, 42.000000, -50.000000));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 132, 2, "CV", "CPV", "CV", "Cape Verde", "Republic of Cape Verde", "Cabo Verde", "Republica de Cabo Verde", "Cape Verde", "Cape Verde", "Cape Verde", 15.121728, -23.605081, 15.343788, -23.781329, 14.899668, -23.428833));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 136, 2, "KY", "CYM", "", "Cayman Islands", "Cayman Islands", "", "", "CYM", "Cayman Islands", "Cayman Islands", 19.313300, -81.254600, 19.396557, -81.420063, 19.262839, -81.083848));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 140, 2, "CF", "CAF", "CF", "Central African Republic", "Central African Republic", "", "Republique Centrafricaine", "CAR", "Cent Africa Rep", "Central African Republic", 6.611111, 20.939444, 11.017956, 14.415098, 2.220857, 27.458305));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 144, 2, "LK", "LKA", "19", "Sri Lanka", "Democratic Socialist Republic of Sri Lanka", "Shri Lamka/Ilankai", "Shri Lamka Prajatantrika Samajaya di Janarajaya/Ilankai Jananayaka Choshalichak Kutiyarachu", "Sri Lanka", "Sri Lanka", "Sri Lanka", 7.873054, 80.771797, 9.835850, 79.628906, 5.919077, 81.878702));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 148, 2, "TD", "TCD", "TD", "Chad", "Republic of Chad", "Tchad/Tshad", "Republique du Tchad/Jumhuriyat Tshad", "Chad", "Chad", "Chad", 15.454166, 18.732207, 23.449235, 13.469999, 7.442975, 24.000001));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 152, 1, "CL", "CHL", "20", "Chile", "Republic of Chile", "Chile", "Republica de Chile", "Chile", "Chile", "Chile", -35.675147, -71.542969, -17.498329, -75.696786, -55.979780, -66.418201));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 156, 1, "CN", "CHN", "76", "China", "People's Republic of China", "Zhongguo", "Zhonghua Renmin Gongheguo", "PRC", "China", "China", 35.861660, 104.195397, 53.560974, 73.499413, 18.153521, 134.772809));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 158, 2, "TW", "TWN", "", "Taiwan", "Taiwan; Province of China", "", "", "Taiwan", "Taiwan", "Taiwan", 23.697810, 120.960515, 25.300440, 120.027801, 21.896695, 122.006905));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 162, 2, "CX", "CXR", "", "Christmas Island", "Territory of Christmas Island", "", "", "CXR", "CXR", "Christmas Island", -10.447525, 105.690449, -10.412374, 105.533316, -10.570087, 105.712647));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 166, 2, "CC", "CCK", "", "Cocos (Keeling) Islands", "Territory of Cocos (Keeling) Islands", "", "", "CCK", "CCK", "Cocos (Keeling) Islands", -12.170873, 96.841739, -12.134390, 96.816723, -12.207169, 96.866755));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 170, 2, "CO", "COL", "22", "Colombia", "Republic of Colombia", "Colombia", "Republica de Colombia", "Colombia", "Colombia", "Colombia", 4.570868, -74.297333, 12.458457, -79.008350, -4.227110, -66.851923));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 174, 2, "KM", "COM", "", "Comoros", "Union of the Comoros", "Comores", "Union des Comores", "Comoros", "Comoros", "Comoros", -11.645500, 43.333300, -11.364639, 43.219421, -11.939322, 43.525772));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 175, 2, "YT", "MYT", "", "Mayotte", "Territorial Collectivity of Mayotte", "", "", "Mayotte", "Mayotte", "Mayotte", -12.827500, 45.166244, -12.636537, 45.018170, -13.006161, 45.300177));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 178, 2, "CG", "COG", "", "Congo", "Congo", "", "", "Congo", "Congo", "Congo", -0.228021, 15.827659, 3.707791, 11.149547, -5.028948, 18.643611));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 180, 2, "CD", "COD", "ZR", "Congo", "The Democratic Republic of the Congo", "", "", "Congo", "Congo", "Congo", -0.228021, 15.827659, 3.707791, 11.149547, -5.028948, 18.643611));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 184, 2, "CK", "COK", "", "Cook Islands", "Cook Islands", "", "", "COK", "Cook Islands", "Cook Islands", -21.236736, -159.777671, -21.198695, -159.831347, -21.273064, -159.723746));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 188, 2, "CR", "CRI", "CR", "Costa Rica", "Republic of Costa Rica", "Costa Rica", "Republica de Costa Rica", "Costa Rica", "Costa Rica", "Costa Rica", 9.748917, -83.753428, 11.219680, -85.955711, 8.040697, -82.552657));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 191, 2, "HR", "HRV", "", "Croatia", "Republic of Croatia", "Hrvatska", "Republika Hrvatska", "Croatia", "Croatia", "Croatia", 45.100000, 15.200000, 46.555223, 13.489691, 42.392346, 19.448052));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 192, 2, "CU", "CUB", "CU", "Cuba", "Republic of Cuba", "Cuba", "Republica de Cuba", "Cuba", "Cuba", "Cuba", 21.521757, -77.781167, 23.276752, -85.071256, 19.825899, -74.132223));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 196, 2, "CY", "CYP", "CY", "Cyprus", "Republic of Cyprus", "Kypros/Kibris", "Kypriaki Dimokratia/Kibris Cumhuriyeti", "Cyprus", "Cyprus", "Cyprus", 35.126413, 33.429859, 35.707199, 32.268707, 34.632303, 34.604500));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 203, 2, "CZ", "CZE", "CZ", "Czech Republic", "Czech Republic", "Cesko", "Ceska Republika", "CZE", "Czech Republic", "Czech Republic", 49.817492, 15.472962, 51.055718, 12.090589, 48.551808, 18.859236));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 204, 2, "BJ", "BEN", "", "Benin", "Republic of Benin", "Benin", "Republique du Benin", "Benin", "Benin", "Benin", 9.307690, 2.315834, 12.408611, 0.776667, 6.235631, 3.843342));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 208, 2, "DK", "DNK", "26", "Denmark", "Kingdom of Denmark", "Danmark", "Kongeriget Danmark", "Denmark", "Denmark", "Denmark", 56.263920, 9.501785, 57.751813, 8.072240, 54.559121, 12.789750));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 212, 2, "DM", "DMA", "", "Dominica", "Commonwealth of Dominica", "", "", "Dominica", "Dominica", "Dominica", 15.414999, -61.370976, 15.640063, -61.479830, 15.207682, -61.240303));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 214, 2, "DO", "DOM", "70", "Dominican Republic", "Dominican Republic", "La Dominicana", "Republica Dominicana", "DOM", "Dominican Rep", "Dominican Republic", 18.735693, -70.162651, 19.931718, -72.007509, 17.470090, -68.323406));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 218, 2, "EC", "ECU", "28", "Ecuador", "Republic of Ecuador", "Ecuador", "Republica del Ecuador", "Ecuador", "Ecuador", "Ecuador", -1.831239, -78.183406, 1.428418, -81.084980, -5.014351, -75.188794));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 222, 2, "SV", "SLV", "75", "El Salvador", "Republic of El Salvador", "El Salvador", "Republica de El Salvador", "SLV", "El Salvador", "El Salvador", 13.794185, -88.896530, 14.450556, -90.126810, 13.155431, -87.683751));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 226, 2, "GQ", "GNQ", "GQ", "Equatorial Guinea", "Republic of Equatorial Guinea", "Guinea Ecuatorial/Guinee equatoriale", "Republica de Guinea Ecuatorial/Republique de Guinee equatoriale", "GNQ", "Equator Guinea", "Equatorial Guinea", 1.650801, 10.267895, 2.349415, 9.301729, 0.887196, 11.333300));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 231, 2, "ET", "ETH", "", "Ethiopia", "Federal Democratic Republic of Ethiopia", "Ityop'iya", "Ityop'iya Federalawi Demokrasiyawi Ripeblik", "FDRE", "Ethiopia", "Ethiopia", 9.145000, 40.489673, 14.894214, 32.997734, 3.404135, 47.999999));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 232, 2, "ER", "ERI", "ER", "Eritrea", "State of Eritrea", "Ertra", "Hagere Ertra", "Eritrea", "Eritrea", "Eritrea", 15.179384, 39.782334, 18.021209, 36.433347, 12.354723, 43.142977));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 233, 2, "EE", "EST", "ES", "Estonia", "Republic of Estonia", "Eesti", "Eesti Vabariik", "Estonia", "Estonia", "Estonia", 58.595272, 25.013607, 59.700283, 21.764372, 57.509316, 28.210138));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 234, 2, "FO", "FRO", "", "Faroe Islands", "", "Foroyar", "", "FRO", "Faeroe Islands", "Faeroe Islands", 61.892635, -6.911806, 62.394099, -7.691905, 61.390905, -6.251564));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 238, 2, "FK", "FLK", "", "Malvinas", "Falkland Islands (Islas Malvinas)", "", "", "Malvinas", "Falkland Is", "Falkland Islands", -51.796253, -59.523613, -51.233259, -61.347571, -52.395296, -57.716114));
				_cache.Add(new GeoCountryData(GeoContinents.Antarctica, 239, 2, "GS", "SGS", "", "South Georgia", "South Georgia and the South Sandwich Islands", "", "", "SGSSI", "South Georgia", "South Georgia", 32.157435, -82.907123, 35.000658, -85.605164, 30.355590, -80.840786));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 242, 2, "FJ", "FJI", "", "Fiji", "Republic of the Fiji Islands", "Fiji/Viti", "Republic of the Fiji Islands/Matanitu ko Viti", "Fiji", "Fiji", "Fiji", -17.713371, 178.065032, -15.713723, 176.909494, -19.216151, 179.22057));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 246, 2, "FI", "FIN", "34", "Finland", "Republic of Finland", "Suomi/Finland", "Suomen tasavalta/Republiken Finland", "Finland", "Finland", "Finland", 61.924110, 25.748151, 70.092112, 20.547410, 59.737038, 31.587099));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 248, 2, "AX", "ALA", "", "Aland Islands", "Aland Islands", "", "Åland Islands", "ALA", "Aland Islands", "Aland Islands", 60.338548, 20.271258, 60.664982, 19.278988, 59.734261, 21.478265));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 250, 1, "FR", "FRA", "35", "France", "French Republic", "France", "Republique francaise", "France", "France", "France", 46.227638, 2.213749, 51.088961, -5.140402, 41.342327, 9.559793));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 254, 2, "GF", "GUF", "", "French Guiana", "Department of Guiana", "Guyane", "", "GUF", "French Guiana", "French Guiana", 3.933889, -53.125782, 5.757189, -54.554437, 2.109287, -51.633596));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 258, 2, "PF", "PYF", "", "French Polynesia", "Overseas Lands of French Polynesia", "Polynesie Francaise", "Pays d'outre-mer de la Polynesie Francaise", "PYF", "French Poly", "French Polynesia", -17.679742, -149.406843, -17.494411, -149.620887, -17.880432, -149.125156));
				_cache.Add(new GeoCountryData(GeoContinents.Antarctica, 260, 2, "TF", "ATF", "", "French Southern and Antarctic Lands", "French Southern and Antartic Lands", "French Southern Territories", "", "ATF", "ATF", "French Southern Territories", -49.280366, 69.348557, -48.449741, 68.609018, -49.733917, 70.556602));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 262, 2, "DJ", "DJI", "DJ", "Djibouti", "Republic of Djibouti", "Djibouti/Jibuti", "Republique de Djibouti/Jumhuriyat Jibuti", "Djibouti", "Djibouti", "Djibouti", 11.825138, 42.590275, 12.713395, 41.759722, 10.931944, 43.416973));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 266, 2, "GA", "GAB", "GA", "Gabon", "Gabonese Republic", "Gabon", "Republique gabonaise", "Gabon", "Gabon", "Gabon", -0.803689, 11.609444, 2.318109, 8.699052, -3.958372, 14.520556));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 268, 2, "GE", "GEO", "GE", "Georgia", "", "Sak'art'velo", "", "Georgia", "Georgia", "Georgia", 32.157435, -82.907123, 35.000658, -85.605164, 30.355590, -80.840786));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 270, 2, "GM", "GMB", "GM", "Gambia", "Gambia", "", "", "Gambia", "Gambia", "Gambia", 13.443182, -15.310139, 13.826389, -16.813631, 13.065182, -13.798610));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 275, 2, "PS", "PSE", "", "Palestine", "State of Palestine", "", "", "Palestine", "Palestine", "Palestine", 31.952162, 35.233154, 32.552099, 34.880274, 31.342602, 35.574052));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 276, 1, "DE", "DEU", "6", "Germany", "Federal Republic of Germany", "Deutschland", "Bundesrepublik Deutschland", "Germany", "Germany", "Germany", 51.165691, 10.451526, 55.058347, 5.866342, 47.270111, 15.041896));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 288, 2, "GH", "GHA", "GH", "Ghana", "Republic of Ghana", "", "", "Ghana", "Ghana", "Ghana", 7.946527, -1.023194, 11.166667, -3.260786, 4.738873, 1.199362));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 292, 2, "GI", "GIB", "", "Gibraltar", "Gibraltar", "", "", "Gibraltar", "Gibraltar", "Gibraltar", 36.140751, -5.353585, 36.155118, -5.367415, 36.108834, -5.338419));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 296, 2, "KI", "KIR", "", "Kiribati", "Republic of Kiribati", "Kiribati", "Republic of Kiribati", "Kiribati", "Kiribati", "Kiribati", 1.871114, -157.360669, 2.048331, -157.561493, 1.692371, -157.158347));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 300, 2, "GR", "GRC", "36", "Greece", "Hellenic Republic", "Ellas or Ellada", "Elliniki Dhimokratia", "Greece", "Greece", "Greece", 39.074208, 21.824312, 41.749057, 19.373587, 34.801021, 28.246955));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 304, 2, "GL", "GRL", "", "Greenland", "Greenland", "Kalaallit Nunaat", "", "Greenland", "Greenland", "Greenland", 71.706936, -42.604303, 83.609581, -73.035063, 59.777401, -11.312319));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 308, 2, "GD", "GRD", "GR", "Grenada", "Grenada", "", "", "Grenada", "Grenada", "Grenada", 12.116500, -61.679000, 12.235087, -61.802727, 11.984872, -61.584720));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 312, 2, "GP", "GLP", "", "Guadeloupe", "Department of Guadeloupe", "Guadeloupe", "Departement de la Guadeloupe", "Guadeloupe", "Guadeloupe", "Guadeloupe", 16.265000, -61.551000, 16.514251, -61.809081, 15.831938, -61.001672));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 316, 2, "GU", "GUM", "", "Guam", "Territory of Guam", "Guahan", "Guahan", "Guam", "Guam", "Guam", 13.444304, 144.793731, 13.654224, 144.618380, 13.246190, 144.956536));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 320, 2, "GT", "GTM", "37", "Guatemala", "Republic of Guatemala", "Guatemala", "Republica de Guatemala", "Guatemala", "Guatemala", "Guatemala", 15.783471, -90.230759, 17.815711, -92.231835, 13.740021, -88.225615));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 324, 2, "GN", "GIN", "GU", "Guinea", "Republic of Guinea", "Guinee", "Republique de Guinee", "Guinea", "Guinea", "Guinea", 9.945587, -9.696645, 12.674616, -15.078206, 7.190909, -7.637853));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 328, 2, "GY", "GUY", "GY", "Guyana", "Cooperative Republic of Guyana", "", "", "Guyana", "Guyana", "Guyana", 4.860416, -58.930180, 8.548255, -61.414905, 1.164724, -56.491120));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 332, 2, "HT", "HTI", "38", "Haiti", "Republic of Haiti", "Haiti/Ayiti", "Republique d'Haiti/Repiblik d' Ayiti", "Haiti", "Haiti", "Haiti", 18.971187, -72.285215, 20.089614, -74.480910, 18.022078, -71.621754));
				_cache.Add(new GeoCountryData(GeoContinents.Antarctica, 334, 2, "HM", "HMD", "", "Heard Island and McDonald Islands", "Territory of Heard Island and McDonald Islands", "", "", "HIMI", "HIMI", "HIMI", -53.081810, 73.504158, -52.961616, 73.251240, -53.191547, 73.776083));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 336, 2, "VA", "VAT", "", "Vatican", "Holy See (Vatican City State)", "", "", "Vatican", "Holy See", "Holy See", 41.902916, 12.453389, 41.907561, 12.445687, 41.900197, 12.458479));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 340, 2, "HN", "HND", "HO", "Honduras", "Republic of Honduras", "Honduras", "Republica de Honduras", "Honduras", "Honduras", "Honduras", 15.199999, -86.241905, 16.516539, -89.355148, 12.984224, -83.136076));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 344, 2, "HK", "HKG", "", "Hong Kong", "Hong Kong Special Administrative Region", "Xianggang", "Xianggang Tebie Xingzhengqu", "HK", "Hong Kong", "Hong Kong", 22.396428, 114.109497, 22.561968, 113.835079, 22.153388, 114.406445));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 348, 2, "HU", "HUN", "HU", "Hungary", "Republic of Hungary", "Magyarorszag", "Magyar Koztarsasag", "Hungary", "Hungary", "Hungary", 47.162494, 19.503304, 48.585234, 16.113386, 45.737088, 22.897379));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 352, 2, "IS", "ISL", "46", "Iceland", "Republic of Iceland", "Island", "Lydveldid Island", "Iceland", "Iceland", "Iceland", 64.963051, -19.020835, 66.566318, -24.546523, 63.296102, -13.495815));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 356, 1, "IN", "IND", "41", "India", "Republic of India", "India/Bharat", "Republic of India/Bharatiya Ganarajya", "India", "India", "India", 20.593684, 78.962880, 35.504340, 68.162795, 6.747138, 97.395555));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 360, 2, "ID", "IDN", "42", "Indonesia", "Republic of Indonesia", "Indonesia", "Republik Indonesia", "Indonesia", "Indonesia", "Indonesia", -0.789275, 113.921327, 5.906821, 95.011064, -11.004673, 141.018662));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 364, 2, "IR", "IRN", "44", "Iran", "Islamic Republic of Iran", "", "", "Iran", "Iran", "Iran", 32.427908, 53.688046, 39.781675, 44.031890, 25.059428, 63.333336));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 368, 2, "IQ", "IRQ", "43", "Iraq", "Republic of Iraq", "Al Iraq", "Al Jumhuriyah al-Iraqiyah", "Iraq", "Iraq", "Iraq", 33.223191, 43.679291, 37.380932, 38.793602, 29.061207, 48.575916));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 372, 1, "IE", "IRL", "45", "Ireland", "Ireland", "Eire", "", "Ireland", "Ireland", "Ireland", 53.412910, -8.243890, 55.386710, -10.669450, 51.422195, -5.994710));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 376, 2, "IL", "ISR", "47", "Israel", "State of Israel", "Yisra'el", "Medinat Yisra'el", "Israel", "Israel", "Israel", 31.046051, 34.851612, 33.332805, 34.267387, 29.490646, 35.896244));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 380, 1, "IT", "ITA", "48", "Italy", "Italian Republic", "Italia", "Repubblica Italiana", "Italy", "Italy", "Italy", 41.871940, 12.567380, 47.092000, 6.626720, 35.492920, 18.520501));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 384, 2, "CI", "CIV", "IC", "Côte d'Ivoire", "Republic of Cote d'Ivoire", "Cote d'Ivoire", "Republique de Cote d'Ivoire", "CIV", "C?te d'Ivoire", "C?te d'Ivoire", 7.539989, -5.547080, 10.740015, -8.602058, 4.351007, -2.493031));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 388, 2, "JM", "JAM", "JA", "Jamaica", "Jamaica", "", "", "Jamaica", "Jamaica", "Jamaica", 18.109581, -77.297508, 18.525310, -78.368846, 17.705724, -76.183159));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 392, 2, "JP", "JPN", "49", "Japan", "Japan", "Nihon/Nippon", "Nihon-koku/Nippon-koku", "Japan", "Japan", "Japan", 36.204824, 138.252924, 45.522771, 122.933830, 24.048692, 145.817550));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 398, 2, "KZ", "KAZ", "KZ", "Kazakhstan", "Republic of Kazakhstan", "Qazaqstan", "Qazaqstan Respublikasy", "Kazakhstan", "Kazakhstan", "Kazakhstan", 48.019573, 66.923684, 55.441983, 46.493671, 40.568584, 87.315415));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 400, 2, "JO", "JOR", "50", "Jordan", "Hashemite Kingdom of Jordan", "Al Urdun", "Al Mamlakah al Urduniyah al Hashimiyah", "Jordan", "Jordan", "Jordan", 30.585164, 36.238414, 33.374687, 34.958336, 29.185036, 39.301154));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 404, 2, "KE", "KEN", "KE", "Kenya", "Republic of Kenya", "Kenya", "Republic of Kenya/Jamhuri ya Kenya", "Kenya", "Kenya", "Kenya", -0.023559, 37.906193, 5.033420, 33.909821, -4.679681, 41.906831));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 408, 2, "KP", "PRK", "KR", "North Korea", "Democratic People's Republic of Korea", "", "", "PRK", "North Korea", "North Korea", 40.339852, 127.510093, 43.011590, 124.173552, 37.673332, 130.674865));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 410, 2, "KR", "KOR", "", "South Korea", "Republic of Korea", "", "", "KOR", "South Korea", "South Korea", 35.907757, 127.766922, 38.616931, 124.608139, 33.106109, 129.584671));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 414, 2, "KW", "KWT", "KU", "Kuwait", "State of Kuwait", "Al Kuwayt", "Dawlat al Kuwayt", "Kuwait", "Kuwait", "Kuwait", 29.311660, 47.481766, 30.103706, 46.553039, 28.524446, 48.430457));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 417, 2, "KG", "KGZ", "", "Kyrgyzstan", "Kyrgyz Republic", "Kyrgyzstan", "Kyrgyz Respublikasy", "Kyrgyzstan", "Kyrgyz Republic", "Kyrgyz Republic", 41.204380, 74.766098, 43.265356, 69.250998, 39.180254, 80.226559));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 418, 2, "LA", "LAO", "", "Democratic Republic", "Lao People's Democratic Republic", "", "", "LAO", "Laos", "Laos", 38.905448, -77.039316, 38.914130, -77.055324, 38.896765, -77.023309));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 422, 2, "LB", "LBN", "52", "Lebanon", "Lebanese Republic", "Lubnan", "Al Jumhuriyah al Lubnaniyah", "Lebanon", "Lebanon", "Lebanon", 33.854721, 35.862285, 34.692090, 35.103778, 33.055025, 36.623720));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 426, 2, "LS", "LSO", "LS", "Lesotho", "Kingdom of Lesotho", "Lesotho", "Kingdom of Lesotho", "Lesotho", "Lesotho", "Lesotho", -29.609988, 28.233608, -28.570801, 27.011231, -30.675578, 29.455708));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 428, 2, "LV", "LVA", "LA", "Latvia", "Republic of Latvia", "Latvija", "Latvijas Republika", "Latvia", "Latvia", "Latvia", 56.879635, 24.603189, 58.085568, 20.962346, 55.674776, 28.241402));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 430, 2, "LR", "LBR", "54", "Liberia", "Republic of Liberia", "", "", "Liberia", "Liberia", "Liberia", 6.428055, -9.429499, 8.551986, -11.474248, 4.315413, -7.369254));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 434, 2, "LY", "LBY", "53", "Libya", "Great Socialist People's Libyan Arab Jamahiriya", "", "Al Jumahiriyah al Arabiyah al Libiyah ash Shabiyah al Ishtirakiyah al Uzma", "Libya", "Libya", "Libya", 27.056776, 14.528040, 33.166787, 9.391466, 19.500429, 25.146954));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 438, 2, "LI", "LIE", "", "Liechtenstein", "Principality of Liechtenstein", "Liechtenstein", "Fuerstentum Liechtenstein", "LIE", "Liechtenstein", "Liechtenstein", 47.166000, 9.555373, 47.270546, 9.471620, 47.048290, 9.635650));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 440, 2, "LT", "LTU", "LT", "Lithuania", "Republic of Lithuania", "Lietuva", "Lietuvos Respublika", "Lithuania", "Lithuania", "Lithuania", 55.169438, 23.881275, 56.450320, 20.954368, 53.896878, 26.835591));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 442, 2, "LU", "LUX", "LU", "Luxembourg", "Grand Duchy of Luxembourg", "Luxembourg", "Grand Duche de Luxembourg", "Luxembourg", "Luxembourg", "Luxembourg", 49.815273, 6.129583, 50.182820, 5.735669, 49.447779, 6.530970));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 446, 2, "MO", "MAC", "", "Macau", "Macao", "", "", "Macao", "Macao", "Macao", 22.198745, 113.543873, 22.217063, 113.527605, 22.109771, 113.598279));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 450, 2, "MG", "MDG", "", "Madagascar", "Republic of Madagascar", "Madagascar/Madagasikara", "Republique de Madagascar/Repoblikan'i Madagasikara", "Madagascar", "Madagascar", "Madagascar", -18.766947, 46.869107, -11.951963, 43.185139, -25.606571, 50.483779));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 454, 2, "MW", "MWI", "MW", "Malawi", "Republic of Malawi", "Malawi", "Dziko la Malawi", "Malawi", "Malawi", "Malawi", -13.254308, 34.301525, -9.367153, 32.678889, -17.135278, 35.924166));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 458, 2, "MY", "MYS", "MS", "Malaysia", "Malaysia", "Malaysia", "", "Malaysia", "Malaysia", "Malaysia", 4.210484, 101.975766, 6.725747, 99.422836, 0.461421, 104.542560));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 462, 2, "MV", "MDV", "MV", "Maldives", "Republic of Maldives", "Dhivehi Raajje", "Dhivehi Raajjeyge Jumhooriyyaa", "Maldives", "Maldives", "Maldives", 1.977246, 73.536103, 2.133162, 73.486261, 1.821331, 73.585945));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 466, 2, "ML", "MLI", "MI", "Mali", "Republic of Mali", "Mali", "Republique de Mali", "Mali", "Mali", "Mali", 17.570692, -3.996166, 25.000058, -12.238884, 10.147811, 4.266666));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 470, 2, "MT", "MLT", "ML", "Malta", "Republic of Malta", "Malta", "Repubblika ta' Malta", "Malta", "Malta", "Malta", 35.937496, 14.375416, 36.082146, 14.183349, 35.805811, 14.575500));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 474, 2, "MQ", "MTQ", "", "Martinique", "Department of Martinique", "Martinique", "Departement de la Martinique", "Martinique", "Martinique", "Martinique", 14.641528, -61.024174, 14.878450, -61.229093, 14.388647, -60.810527));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 478, 2, "MR", "MRT", "MU", "Mauritania", "Islamic Republic of Mauritania", "Muritaniyah", "Al Jumhuriyah al Islamiyah al Muritaniyah", "Mauritania", "Mauritania", "Mauritania", 21.007890, -10.940835, 27.294444, -17.070133, 14.721273, -4.833334));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 480, 2, "MU", "MUS", "MA", "Mauritius", "Republic of Mauritius", "Mauritius", "Republic of Mauritius", "Mauritius", "Mauritius", "Mauritius", -20.348404, 57.552152, -18.776300, 55.766600, -21.637000, 59.584400));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 484, 1, "MX", "MEX", "57", "Mexico", "United Mexican States", "Mexico", "Estados Unidos Mexicanos", "Mexico", "Mexico", "Mexico", 23.634501, -102.552784, 32.718762, -118.363977, 14.534548, -86.710571));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 492, 2, "MC", "MCO", "MO", "Monaco", "Principality of Monaco", "Monaco", "Principaute de Monaco", "Monaco", "Monaco", "Monaco", 43.738417, 7.424615, 43.751902, 7.409104, 43.724742, 7.439811));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 496, 1, "MN", "MNG", "MN", "Mongolia", "Mongolia", "Mongol Uls", "", "Mongolia", "Mongolia", "Mongolia", 46.862496, 103.846656, 52.148696, 87.737620, 41.581520, 119.931948));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 498, 2, "MD", "MDA", "MD", "Moldova", "Republic of Moldova", "", "", "Moldova", "Moldova", "Moldova", 47.411631, 28.369885, 48.491944, 26.616855, 45.466904, 30.162538));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 499, 2, "ME", "MNE", "", "Montenegro", "Montenegro", "", "", "Montenegro", "Montenegro", "Montenegro", 42.708678, 19.374390, 43.558743, 18.433792, 41.849730, 20.357764));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 500, 2, "MS", "MSR", "", "Montserrat", "Montserrat", "", "", "Montserrat", "Montserrat", "Montserrat", 16.742498, -62.187366, 16.824051, -62.241517, 16.674695, -62.144175));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 504, 2, "MA", "MAR", "56", "Morocco", "Kingdom of Morocco", "Al Maghrib", "Al Mamlakah al Maghribiyah", "Morocco", "Morocco", "Morocco", 31.791702, -7.092620, 35.922507, -13.172891, 27.666666, -0.996975));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 508, 2, "MZ", "MOZ", "MZ", "Mozambique", "Republic of Mozambique", "Mocambique", "Republica de Mocambique", "Mozambique", "Mozambique", "Mozambique", -18.665695, 35.529562, -10.471202, 30.215549, -26.868108, 40.839121));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 512, 2, "OM", "OMN", "OM", "Oman", "Sultanate of Oman", "Uman", "Saltanat Uman", "Oman", "Oman", "Oman", 21.512583, 55.923255, 26.405394, 52.000001, 16.650336, 59.839397));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 516, 2, "NA", "NAM", "", "Namibia", "Republic of Namibia", "Namibia", "Republic of Namibia", "Namibia", "Namibia", "Namibia", -22.957640, 18.490410, -16.963485, 11.724246, -28.970638, 25.261751));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 520, 2, "NR", "NRU", "NR", "Nauru", "Republic of Nauru", "Nauru", "Republic of Nauru", "Nauru", "Nauru", "Nauru", -0.522778, 166.931503, -0.502639, 166.909548, -0.554189, 166.958928));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 524, 2, "NP", "NPL", "NP", "Nepal", "Federal Democratic Republic of Nepal", "Nepal", "Sanghiya Loktantrik Ganatantra Nepal", "Nepal", "Nepal", "Nepal", 28.394857, 84.124008, 30.446945, 80.052222, 26.347779, 88.199297));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 528, 1, "NL", "NLD", "", "Netherlands", "Kingdom of the Netherlands", "Nederland", "Koninkrijk der Nederlanden", "NLD", "Netherlands", "Netherlands", 52.132633, 5.291266, 53.675600, 3.331600, 50.750383, 7.227140));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 531, 2, "CW", "CUW", "", "Curaçao", "Curaçao", "", "", "Curaçao", "Curaçao", "Curaçao", 12.169570, -68.990020, 12.392435, -69.162454, 12.034326, -68.737335));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 533, 2, "AW", "ABW", "AW", "Aruba", "Aruba", "", "", "Aruba", "Aruba", "Aruba", 12.521110, -69.968338, 12.623378, -70.066025, 12.411765, -69.865880));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 534, 2, "SX", "SXM", "", "Sint Maarten", "Sint Maarten (Dutch)", "", "", "SXM", "Sint Maarten", "Sint Maarten", 18.027304, -63.050080, 18.125133, -63.137978, 18.005101, -62.970392));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 535, 2, "BQ", "BES", "", "Bonaire", "Caribbean Netherlands", "Bonaire; Sint Eustatius and Saba", "", "Bonaire", "Bonaire", "Bonaire", 12.178361, -68.238533, 12.312239, -68.420963, 12.024504, -68.195402));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 540, 2, "NC", "NCL", "59", "New Caledonia", "Territory of New Caledonia and Dependencies", "Nouvelle-Caledonie", "Territoire des Nouvelle-Caledonie et Dependances", "NCL", "New Caledonia", "New Caledonia", -20.904305, 165.618042, -19.539508, 163.569721, -22.881947, 168.133681));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 548, 2, "VU", "VUT", "VU", "Vanuatu", "Republic of Vanuatu", "Vanuatu", "Ripablik blong Vanuatu", "Vanuatu", "Vanuatu", "Vanuatu", -15.376706, 166.959158, -13.072455, 166.541758, -17.826103, 168.649063));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 554, 1, "NZ", "NZL", "61", "New Zealand", "New Zealand", "", "", "NZ", "New Zealand", "New Zealand", -40.900557, 174.885971, -34.129557, 166.426136, -47.768124, 179.062535));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 558, 2, "NI", "NIC", "NC", "Nicaragua", "Republic of Nicaragua", "Nicaragua", "Republica de Nicaragua", "Nicaragua", "Nicaragua", "Nicaragua", 12.865416, -85.207229, 15.030275, -87.691068, 10.708054, -82.592071));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 562, 2, "NE", "NER", "NE", "Niger", "Republic of Niger", "Niger", "Republique du Niger", "Niger", "Niger", "Niger", 17.607789, 8.081666, 23.500000, 0.166667, 11.693756, 15.999033));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 566, 2, "NG", "NGA", "NI", "Nigeria", "Federal Republic of Nigeria", "", "", "Nigeria", "Nigeria", "Nigeria", 9.081999, 8.675277, 13.885644, 2.676932, 4.269857, 14.677981));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 570, 2, "NU", "NIU", "", "Niue", "Niue", "", "", "Niue", "Niue", "Niue", -19.054445, -169.867233, -18.952970, -169.949848, -19.155566, -169.774324));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 574, 2, "NF", "NFK", "", "Norfolk Island", "Territory of Norfolk Island", "", "", "NFK", "Norfolk Island", "Norfolk Island", -29.040835, 167.954712, -28.995388, 167.916219, -29.075140, 167.996926));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 578, 2, "NO", "NOR", "58", "Norway", "Kingdom of Norway", "Norge", "Kongeriket Norge", "Norway", "Norway", "Norway", 60.472024, 8.468946, 71.185476, 4.626684, 57.970937, 31.149789));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 580, 2, "MP", "MNP", "", "Northern Mariana Islands", "Commonwealth of the Northern Mariana Islands", "", "", "CNMI", "N Mariana Is", "Northern Mariana Islands", 15.097900, 145.673900, 15.290259, 145.582344, 14.922522, 145.830551));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 581, 2, "UM", "UMI", "", "United States Minor Outlying Islands", "United States Minor Outlying Islands", "", "", "UMI", "UMI", "UMI", 5.880620, -162.077210, 5.895611, -162.101639, 5.865629, -162.051493));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 583, 2, "FM", "FSM", "", "Micronesia", "", "Micronesia", "Federated States of Micronesia", "FSM", "Micronesia", "", 1.000000, 8.857145, 3.000000, 7.011341, 2.000000, 7.629661));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 584, 2, "MH", "MHL", "MH", "Marshall Islands", "Republic of the Marshall Islands", "Marshall Islands", "Republic of the Marshall Islands", "RMI", "Marshall Is", "Marshall Islands", 7.131474, 171.184478, 7.169534, 171.027163, 7.052192, 171.384970));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 585, 2, "PW", "PLW", "", "Palau", "Republic of Palau", "Belau", "Beluu er a Belau", "Palau", "Palau", "Palau", 7.514980, 134.582520, 7.759268, 131.062032, 2.999120, 134.649191));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 586, 2, "PK", "PAK", "62", "Pakistan", "Islamic Republic of Pakistan", "Pakistan", "Jamhuryat Islami Pakistan", "Pakistan", "Pakistan", "Pakistan", 30.375321, 69.345116, 37.084107, 60.872972, 23.694694, 77.835666));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 591, 2, "PA", "PAN", "PA", "Panama", "Republic of Panama", "Panama", "Republica de Panama", "Panama", "Panama", "Panama", 8.537981, -80.782127, 9.647779, -83.052241, 7.203556, -77.158488));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 598, 2, "PG", "PNG", "PG", "Papua New Guinea", "Independent State of Papua New Guinea", "Papuaniugini", "", "PNG", "PNG", "Papua New Guinea", -6.314993, 143.955550, -0.871319, 140.841969, -11.657860, 157.085723));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 600, 2, "PY", "PRY", "PY", "Paraguay", "Republic of Paraguay", "Paraguay", "Republica del Paraguay", "Paraguay", "Paraguay", "Paraguay", -23.442503, -58.443832, -19.287706, -62.638051, -27.588334, -54.258562));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 604, 2, "PE", "PER", "65", "Peru", "Republic of Peru", "Peru", "Republica del Peru", "Peru", "Peru", "Peru", -9.189967, -75.015152, -0.038777, -81.328504, -18.351580, -68.652329));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 608, 2, "PH", "PHL", "66", "Philippines", "Republic of the Philippines", "Pilipinas", "Republika ng Pilipinas", "PHL", "Philippines", "Philippines", 12.879721, 121.774017, 19.574024, 116.703162, 4.613444, 126.604383));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 612, 2, "PN", "PCN", "", "Pitcairn", "Pitcairn Islands", "", "", "Pitcairn", "Pitcairn", "Pitcairn", -24.376490, -128.324367, -24.330485, -128.357047, -24.422495, -128.291687));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 616, 2, "PL", "POL", "67", "Poland", "Republic of Poland", "Polska", "Rzeczpospolita Polska", "Poland", "Poland", "Poland", 51.919438, 19.145136, 54.835812, 14.122864, 49.002025, 24.145893));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 620, 2, "PT", "PRT", "68", "Portugal", "Portuguese Republic", "Portugal", "Republica Portuguesa", "Portugal", "Portugal", "Portugal", 39.399872, -8.224454, 42.154204, -9.517110, 36.960177, -6.190209));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 624, 2, "GW", "GNB", "GN", "Guinea-Bissau", "Republic of Guinea-Bissau", "Guine-Bissau", "Republica da Guine-Bissau", "GNB", "Guinea-Bissau", "Guinea-Bissau", 11.803749, -15.180413, 12.684722, -16.711735, 10.859970, -13.627504));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 626, 2, "TL", "TLS", "", "Timor-Leste", "Timor-Leste", "", "", "TLS", "Timor-Leste", "Timor-Leste", -8.711485, 125.634764, -8.126806, 124.931763, -9.462656, 127.341634));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 630, 2, "PR", "PRI", "", "Puerto Rico", "Commonwealth of Puerto Rico", "", "", "PRI", "Puerto Rico", "Puerto Rico", 18.220833, -66.590149, 18.516009, -67.484239, 17.881428, -65.221109));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 634, 2, "QA", "QAT", "QA", "Qatar", "State of Qatar", "Qatar", "Dawlat Qatar", "Qatar", "Qatar", "Qatar", 25.354826, 51.183884, 26.183092, 50.750055, 24.471118, 51.643260));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 638, 2, "RE", "REU", "", "Réunion", "Department of Reunion", "Ile de la Reunion", "", "Réunion", "R?union", "R?union", -21.115141, 55.536384, -20.871755, 55.216405, -21.389622, 55.836553));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 642, 2, "RO", "ROU", "73", "Romania", "Romania", "Romania", "", "Romania", "Romania", "Romania", 45.943161, 24.966760, 48.265274, 20.261759, 43.618619, 29.757101));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 643, 1, "RU", "RUS", "RU", "Russia", "Russian Federation", "", "", "Russia", "Russia", "Russian Federation", 61.524010, 105.318756, 70.000000, 27.000000, 40.000000, 179.000000));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 646, 2, "RW", "RWA", "RW", "Rwanda", "Republic of Rwanda", "Rwanda", "Republika y'u Rwanda", "Rwanda", "Rwanda", "Rwanda", -1.940278, 29.873888, -1.047571, 28.861754, -2.839839, 30.899400));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 652, 2, "BL", "BLM", "", "Saint Barthélemy", "Saint Barthélemy", "", "", "BLM", "BLM", "Saint Barthélemy", 17.900000, -62.833333, 17.957015, -62.878489, 17.870828, -62.789214));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 654, 2, "SH", "SHN", "", "Saint Helena", "Saint Helena; Ascension and Tristan da Cunha", "", "", "SHN", "St Helena", "St Helena", 38.505242, -122.470386, 38.486474, -122.495042, -15.904009, -5.633969));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 659, 2, "KN", "KNA", "KN", "Saint Kitts and Nevis", "Federation of Saint Kitts and Nevis", "", "", "KNA", "Scotland", "Scotland", 17.357822, -62.782998, 17.418201, -62.864617, 17.094157, -62.539694));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 660, 2, "AI", "AIA", "", "Anguilla", "Anguilla", "", "", "Anguilla", "Anguilla", "Anguilla", 18.220554, -63.068615, 18.296594, -63.190832, 18.149946, -62.922434));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 662, 2, "LC", "LCA", "LC", "Saint Lucia", "Saint Lucia", "", "", "LCA", "St Lucia", "St Lucia", 13.909444, -60.978893, 14.110932, -61.079671, 13.708117, -60.873098));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 663, 2, "MF", "MAF", "", "Saint Martin", "Saint Martin (French)", "", "", "MAF", "Saint Martin", "Saint Martin", 18.082550, -63.052251, 18.125133, -63.153326, 18.046575, -62.970392));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 666, 2, "PM", "SPM", "", "Saint Pierre and Miquelon", "Territorial Collectivity of Saint Pierre and Miquelon", "Saint-Pierre et Miquelon", "Departement de Saint-Pierre et Miquelon", "SPM", "St Pierre/Mique", "St Pierre and Miquelon", 46.885200, -56.315900, 47.144270, -56.405632, 46.749105, -56.118937));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 670, 2, "VC", "VCT", "", "Saint Vincent and the Grenadines", "Saint Vincent and the Grenadines", "", "", "VCT", "St Vinc/Grenad", "St Vincent and the Grenadines", 13.253383, -61.196251, 13.384213, -61.279077, 13.122553, -61.113424));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 674, 2, "SM", "SMR", "SR", "San Marino", "Republic of San Marino", "San Marino", "Repubblica di San Marino", "San Marino", "San Marino", "San Marino", 43.942360, 12.457777, 43.992075, 12.403482, 43.893680, 12.516704));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 678, 2, "ST", "STP", "", "Sao Tome and Principe", "Sao Tome and Principe", "São Tomé and Príncipe", "", "STP", "STP", "Sao Tome and Principe", 0.186360, 6.613081, 0.413394, 6.460475, -0.014004, 6.760652));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 682, 2, "SA", "SAU", "SA", "Saudi Arabia", "Kingdom of Saudi Arabia", "Al Arabiyah as Suudiyah", "Al Mamlakah al Arabiyah as Suudiyah", "SAU", "Saudi Arabia", "Saudi Arabia", 23.885942, 45.079162, 32.154284, 34.548997, 16.379528, 55.666699));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 686, 2, "SN", "SEN", "SE", "Senegal", "Republic of Senegal", "Senegal", "Republique du Senegal", "Senegal", "Senegal", "Senegal", 14.497401, -14.452362, 16.693054, -17.529848, 12.307289, -11.348607));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 688, 2, "RS", "SRB", "", "Serbia", "Serbia", "", "", "Serbia", "Serbia", "Serbia", 44.016521, 21.005859, 46.190032, 18.838522, 42.231502, 23.006309));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 690, 2, "SC", "SYC", "SC", "Seychelles", "Republic of Seychelles", "Seychelles", "Republic of Seychelles", "Seychelles", "Seychelles", "Seychelles", -4.679574, 55.491977, -4.560981, 55.359914, -4.806792, 55.539169));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 694, 2, "SL", "SLE", "SL", "Sierra Leone", "Republic of Sierra Leone", "Sierra Leone", "Republic of Sierra Leone", "SLE", "Sierra Leone", "Sierra Leone", 8.460555, -11.779889, 9.999972, -13.302006, 6.899025, -10.271651));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 702, 1, "SG", "SGP", "SI", "Singapore", "Republic of Singapore", "Singapore", "Republic of Singapore", "Singapore", "Singapore", "Singapore", 1.352083, 103.819836, 1.470880, 103.605575, 1.166398, 104.085680));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 703, 2, "SK", "SVK", "", "Slovakia", "Slovak Republic", "Slovensko", "Slovenska Republika", "Slovakia", "Slovakia", "Slovakia", 48.669026, 19.699024, 49.613805, 16.833182, 47.731159, 22.559281));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 704, 2, "VN", "VNM", "94", "Vietnam", "Viet Nam", "", "", "Vietnam", "Viet Nam", "Viet Nam", 14.058324, 108.277199, 23.393395, 102.144410, 8.412729, 109.468975));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 705, 2, "SI", "SVN", "SN", "Slovenia", "Republic of Slovenia", "Slovenija", "Republika Slovenija", "Slovenia", "Slovenia", "Slovenia", 46.151241, 14.995463, 46.876659, 13.375335, 45.421673, 16.596685));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 706, 2, "SO", "SOM", "SM", "Somalia", "Somalia", "Soomaaliya", "Jamhuuriyada Demuqraadiga Soomaaliyeed", "Somalia", "Somalia", "Somalia", 5.152149, 46.199616, 11.988614, 40.994373, -1.662041, 51.413028));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 710, 2, "ZA", "ZAF", "91", "South Africa", "Republic of South Africa", "", "", "RSA", "South Africa", "South Africa", -30.559482, 22.937506, -22.125386, 16.460832, -34.833138, 32.890991));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 716, 2, "ZW", "ZWE", "", "Zimbabwe", "Republic of Zimbabwe", "", "", "Zimbabwe", "Zimbabwe", "Zimbabwe", -19.015438, 29.154857, -15.609319, 25.237368, -22.424523, 33.068235));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 724, 1, "ES", "ESP", "29", "Spain", "Kingdom of Spain", "Espana", "Reino de Espana", "Spain", "Spain", "Spain", 40.463667, -3.749220, 45.244000, -12.524000, 35.173000, 5.098000));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 728, 2, "SS", "SSD", "", "South Sudan", "South Sudan", "", "", "SSD", "South Sudan", "South Sudan", 6.876991, 31.306978, 12.236388, 23.440849, 3.488980, 35.948997));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 729, 2, "SD", "SDN", "", "Sudan", "Republic of the Sudan", "As-Sudan", "Jumhuriyat as-Sudan", "Sudan", "Sudan", "Sudan", 12.862807, 30.217636, 22.224918, 21.814939, 9.347220, 38.584219));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 732, 2, "EH", "ESH", "", "Western Sahara", "Western Sahara", "", "", "ESH", "Western Sahara", "Western Sahara", 24.215527, -12.885834, 27.666702, -17.104908, 20.770018, -8.666666));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 740, 2, "SR", "SUR", "79", "Suriname", "Republic of Suriname", "Suriname", "Republiek Suriname", "Suriname", "Suriname", "Suriname", 3.919305, -56.027783, 6.009283, -58.070505, 1.837306, -53.951024));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 744, 2, "SJ", "SJM", "", "Svalbard and Jan Mayen", "Svalbard and Jan Mayen", "", "", "SJM", "SJM", "Svalbard and Jan Mayen", 77.553604, 23.670272, 80.834053, 10.490722, 76.436279, 33.497093));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 748, 2, "SZ", "SWZ", "SZ", "Swaziland", "Kingdom of Swaziland", "eSwatini", "Umbuso weSwatini", "Swaziland", "Swaziland", "Swaziland", -26.522503, 31.465866, -25.718519, 30.791094, -27.317363, 32.134844));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 752, 2, "SE", "SWE", "77", "Sweden", "Kingdom of Sweden", "Sverige", "Konungariket Sverige", "Sweden", "Sweden", "Sweden", 60.128161, 18.643501, 69.060023, 10.963186, 55.336702, 24.166024));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 756, 2, "CH", "CHE", "78", "Switzerland", "Swiss Confederation", "Schweiz (German); Suisse (French); Svizzera (Italian); Svizra (Romansh)", "Schweizerische Eidgenossenschaft (German); Confederation Suisse (French); Confederazione Svizzera (Italian); Confederaziun Svizra (Romansh)", "CHE", "Switzerland", "Switzerland", 46.818188, 8.227512, 47.808454, 5.956080, 45.817920, 10.492340));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 760, 2, "SY", "SYR", "80", "Syrian Arab Republic", "Syrian Arab Republic", "", "", "SYR", "Syria", "Syria", 34.802075, 38.996815, 37.320569, 35.716595, 32.311136, 42.376309));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 762, 2, "TJ", "TJK", "TJ", "Tajikistan", "Republic of Tajikistan", "Tojikiston", "Jumhurii Tojikiston", "Tajikistan", "Tajikistan", "Tajikistan", 38.861034, 71.276093, 41.044367, 67.342012, 36.671989, 75.153956));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 764, 2, "TH", "THA", "86", "Thailand", "Kingdom of Thailand", "Prathet Thai", "Ratcha Anachak Thai", "Thailand", "Thailand", "Thailand", 15.870032, 100.992541, 20.465143, 97.343396, 5.612729, 105.636812));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 768, 2, "TG", "TGO", "87", "Togo", "Togolese Republic", "", "Republique togolaise", "Togo", "Togo", "Togo", 8.619543, 0.824782, 11.139495, -0.144041, 6.112357, 1.809050));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 772, 2, "TK", "TKL", "", "Tokelau", "Tokelau", "", "", "Tokelau", "Tokelau", "Tokelau", -9.200200, -171.848400, -9.100656, -171.870632, -9.233011, -171.765790));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 776, 2, "TO", "TON", "TN", "Tonga", "Kingdom of Tonga", "Tonga", "Pule'anga Tonga", "Tonga", "Tonga", "Tonga", -21.178986, -175.198242, -21.010185, -175.356497, -21.473460, -174.900594));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 780, 2, "TT", "TTO", "TT", "Trinidad and Tobago", "Republic of Trinidad and Tobago", "", "", "TTO", "Trinidad/Tobago", "Trinidad and Tobago", 10.691803, -61.222503, 10.843473, -61.931068, 10.043169, -60.908954));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 784, 2, "AE", "ARE", "", "United Arab Emirates", "United Arab Emirates", "", "Al Imarat al Arabiyah al Muttahidah", "UAE", "Unit Arab Emir", "United Arab Emirates", 23.424076, 53.847818, 26.069654, 51.499770, 22.631513, 56.381578));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 788, 2, "TN", "TUN", "88", "Tunisia", "Tunisian Republic", "Tunis", "Al Jumhuriyah at Tunisiyah", "Tunisia", "Tunisia", "Tunisia", 33.886917, 9.537499, 37.347132, 7.522313, 30.228033, 11.599217));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 792, 2, "TR", "TUR", "89", "Turkey", "Republic of Turkey", "Turkiye", "Turkiye Cumhuriyeti", "Turkey", "Turkey", "Turkey", 38.963745, 35.243322, 42.106090, 25.663637, 35.807680, 44.818128));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 795, 2, "TM", "TKM", "TM", "Turkmenistan", "Turkmenistan", "Turkmenistan", "", "TKM", "Turkmenistan", "Turkmenistan", 38.969719, 59.556278, 42.798844, 52.447743, 35.128760, 66.707353));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 796, 2, "TC", "TCA", "", "Turks and Caicos Islands", "Turks and Caicos Islands", "", "", "TCI", "Turks/Caicos Is", "Turks and Caicos Islands", 21.694025, -71.797928, 21.962350, -72.482471, 21.454027, -71.461763));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 798, 2, "TV", "TUV", "TV", "Tuvalu", "Tuvalu", "Tuvalu", "", "Tuvalu", "Tuvalu", "Tuvalu", -10.728071, 179.472656, -10.655209, 179.390258, -10.800933, 179.555053));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 800, 2, "UG", "UGA", "UG", "Uganda", "Republic of Uganda", "", "", "Uganda", "Uganda", "Uganda", 1.373333, 32.290275, 4.223000, 29.573433, -1.481541, 35.033049));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 804, 2, "UA", "UKR", "UR", "Ukraine", "Ukraine", "Ukrayina", "", "Ukraine", "Ukraine", "Ukraine", 48.379433, 31.165580, 52.379581, 22.137158, 44.386463, 40.228580));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 807, 2, "MK", "MKD", "", "Macedonia", "The Former Yugoslav Republic of Macedonia", "Macedonia (FYROM)", "", "Macedonia", "Macedonia", "Macedonia", 41.608635, 21.745275, 42.373646, 20.452423, 40.853782, 23.034093));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 818, 2, "EG", "EGY", "27", "Egypt", "Arab Republic of Egypt", "Misr", "Jumhuriyat Misr al-Arabiyah", "Egypt", "Egypt", "Egypt", 26.820553, 30.802498, 31.671535, 24.696774, 21.999999, 36.894544));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 826, 1, "GB", "GBR", "74", "United Kingdom", "United Kingdom of Great Britain and Northern Ireland", "", "", "UK", "United Kingdom", "United Kingdom", 55.378051, -3.435973, 60.856553, -8.649357, 49.866968, 1.762709));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 831, 2, "GG", "GGY", "", "Guernsey", "Bailiwick of Guernsey", "", "", "Guernsey", "Guernsey", "Guernsey", 49.465691, -2.585278, 49.509410, -2.675740, 49.416719, -2.497839));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 832, 2, "JE", "JEY", "", "Jersey", "Bailiwick of Jersey", "", "", "Jersey", "Jersey", "Jersey", 49.214439, -2.131250, 49.262131, -2.254801, 49.160121, -2.009796));
				_cache.Add(new GeoCountryData(GeoContinents.Europe, 833, 2, "IM", "IMN", "", "Isle of Man", "Isle of Man", "", "", "IMN", "Isle of Man", "Isle of Man", 54.236107, -4.548056, 54.418089, -4.830180, 54.044640, -4.308823));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 834, 2, "TZ", "TZA", "ZA", "Tanzania", "United Republic of Tanzania", "", "", "Tanzania", "Tanzania", "Tanzania", -6.369028, 34.888822, -0.984397, 29.340000, -11.761253, 40.444965));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 840, 1, "US", "USA", "33", "United States", "United States of America", "", "", "America", "USA", "United States of America", 37.090240, -95.712891, 49.380000, -124.390000, 25.820000, -66.940000));
				_cache.Add(new GeoCountryData(GeoContinents.NorthAmerica, 850, 2, "VI", "VIR", "", "U.S. Virgin Islands", "United States Virgin Islands", "US Virgin Islands", "", "VIR", "VIR", "US Virgin Islands", 18.335765, -64.896335, 18.412950, -65.085456, 18.273932, -64.648869));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 854, 2, "BF", "BFA", "BF", "Burkina Faso", "Burkina Faso", "Burkina Faso", "", "BFA", "Burkina Faso", "Burkina Faso", 12.238333, -1.561593, 15.085111, -5.521111, 9.393888, 2.404292));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 858, 2, "UY", "URY", "92", "Uruguay", "Oriental Republic of Uruguay", "Uruguay", "Republica Oriental del Uruguay", "Uruguay", "Uruguay", "Uruguay", -32.522779, -55.765835, -30.085214, -58.439150, -35.031418, -53.077928));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 860, 2, "UZ", "UZB", "UZ", "Uzbekistan", "Republic of Uzbekistan", "Ozbekiston", "Ozbekiston Respublikasi", "Uzbekistan", "Uzbekistan", "Uzbekistan", 41.377491, 64.585262, 45.590075, 55.998217, 37.172257, 73.148946));
				_cache.Add(new GeoCountryData(GeoContinents.SouthAmerica, 862, 2, "VE", "VEN", "93", "Venezuela", "Bolivarian Republic of Venezuela", "", "", "Venezuela", "Venezuela", "Venezuela", 6.423750, -66.589730, 12.196748, -73.351558, 0.647529, -59.805666));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 876, 2, "WF", "WLF", "", "Wallis and Futuna", "Territory of the Wallis and Futuna Islands", "Wallis et Futuna", "Territoire des Iles Wallis et Futuna", "WLF", "Wallis/Futuna", "Wallis and Futuna", -14.293800, -178.116500, -14.236552, -178.186608, -14.362124, -177.992307));
				_cache.Add(new GeoCountryData(GeoContinents.Oceania, 882, 2, "WS", "WSM", "", "Samoa", "Independent State of Samoa", "Samoa", "Malo Sa'oloto Tuto'atasi o Samoa", "Samoa", "Samoa", "Samoa", -13.759029, -172.104629, -13.434402, -172.803195, -14.076588, -171.405859));
				_cache.Add(new GeoCountryData(GeoContinents.Asia, 887, 2, "YE", "YEM", "YM", "Yemen", "Republic of Yemen", "Al Yaman", "Al Jumhuriyah al Yamaniyah", "Yemen", "Yemen", "Yemen", 15.552727, 48.516388, 18.999633, 41.816055, 12.108165, 54.533555));
				_cache.Add(new GeoCountryData(GeoContinents.Africa, 894, 2, "ZM", "ZMB", "ZM", "Zambia", "Republic of Zambia", "", "", "Zambia", "Zambia", "Zambia", -13.133897, 27.849332, -8.203283, 21.996387, -18.077418, 33.702221));

				#endregion

				// set initialized flag
				_initialized = true;
			}
		}
	}
}
