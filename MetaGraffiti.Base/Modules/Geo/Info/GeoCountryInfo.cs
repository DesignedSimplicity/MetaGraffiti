using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Modules.Geo.Info
{
	public class GeoCountryInfo
	{
		// ==================================================
		// Constructors
		public GeoCountryInfo(GeoContinents continent, int id, int division, string iso2, string iso3, string oc, string name, string nameLong, string local, string localLong, string abbr10, string abbr15, string abbr30, double latCenter, double lonCenter, double latNorthWest, double lonNorthWest, double latSouthEast, double lonSouthEast)
		{
			Continent = continent;
			CountryID = id;
			Division = division;

			ISO2 = iso2.ToUpperInvariant();
			ISO3 = iso3.ToUpperInvariant();
			OC = oc.ToUpperInvariant();

			Name = name;
			NameLong = nameLong;
			NameLocal = local;
			NameLocalLong = localLong;

			Abbr10 = abbr10;
			Abbr15 = abbr15;
			Abbr30 = abbr30;

			Center = new GeoLocation(latCenter, lonCenter);
			Bounds = new GeoRectangle(latNorthWest, lonNorthWest, latSouthEast, lonSouthEast);
		}

		// ==================================================
		// Properties
		public int CountryID { get; private set; }

		public IGeoLatLon Center { get; private set; }
		public GeoRectangle Bounds { get; private set; }
		public GeoContinents Continent { get; private set; }

		public int Division { get; private set; }

		public string ISO2 { get; private set; }
		public string ISO3 { get; private set; }
		public string OC { get; private set; }

		public string Abbr10 { get; private set; }
		public string Abbr15 { get; private set; }
		public string Abbr30 { get; private set; }

		public string Name { get; private set; }
		public string NameLocal { get; private set; }
		public string NameLong { get; private set; }
		public string NameLocalLong { get; private set; }

		public IEnumerable<GeoRegionInfo> Regions { get { return GeoRegionInfo.ListByCountry(CountryID); } }
		public bool HasRegions { get { return Regions != null && Regions.Any(); } }

		// ==================================================
		// Static Factory
		public static GeoCountryInfo ByID(int countryID)
		{
			return All.FirstOrDefault(x => x.CountryID == countryID);
		}

		public static GeoCountryInfo ByISO(string iso)
		{
			if (String.IsNullOrWhiteSpace(iso)) return null;

			if (iso.Length == 2)
				return All.FirstOrDefault(x => x.ISO2 == iso.ToUpperInvariant());
			else if (iso.Length == 3)
				return All.FirstOrDefault(x => x.ISO3 == iso.ToUpperInvariant());
			else
				return null;
		}

		public static GeoCountryInfo ByName(string name, bool deep = false)
		{
			if (String.IsNullOrWhiteSpace(name)) return null;

			var c = All.FirstOrDefault(x => String.Compare(x.Name, name, true) == 0);
			if (c == null && deep)
			{
				c = All.FirstOrDefault(x => String.Compare(x.NameLocal, name, true) == 0);
				if (c == null) c = All.FirstOrDefault(x => String.Compare(x.NameLong, name, true) == 0);
				if (c == null) c = All.FirstOrDefault(x => String.Compare(x.NameLocalLong, name, true) == 0);
			}

			return c;
		}

		public static IEnumerable<GeoCountryInfo> ListByLocation(IGeoLatLon point)
		{
			return All.Where(x => x.Bounds.Contains(point));
		}

		// ==================================================
		// Static Globals
		public static List<GeoCountryInfo> All { get { if (_all == null) Init(); return _all; } }

		private static List<GeoCountryInfo> _all;

		private static void Init()
		{
			_all = new List<GeoCountryInfo>
			{
				new GeoCountryInfo(GeoContinents.Asia, 4, 2, "AF", "AFG", "AF", "Afghanistan", "Islamic Republic of Afghanistan", "Afghanestan", "Jomhuri-ye Eslami-ye Afghanestan", "AFG", "Afghanistan", "Afghanistan", 33.93911, 67.709953, 29.3772, 60.517, 38.490876, 74.889861),
				new GeoCountryInfo(GeoContinents.Europe, 8, 2, "AL", "ALB", "72", "Albania", "Republic of Albania", "Shqiperia", "Republika e Shqiperise", "Albania", "Albania", "Albania", 41.153332, 20.168331, 39.644729, 19.263904, 42.661081, 21.057239),
				new GeoCountryInfo(GeoContinents.Antarctica, 10, 2, "AQ", "ATA", "", "Antarctica", "Antarctica", "", "", "Antarctica", "Antarctica", "Antarctica", -77, 0, -61.465019, -110.566406, -82.221655, -5.097656),
				new GeoCountryInfo(GeoContinents.Africa, 12, 2, "DZ", "DZA", "AL", "Algeria", "People's Democratic Republic of Algeria", "Al Jaza'ir", "Al Jumhuriyah al Jaza'iriyah ad Dimuqratiyah ash Sha'biyah", "Algeria", "Algeria", "Algeria", 27.225727, 2.492945, 18.968147, -8.666667, 37.08982, 11.999999),
				new GeoCountryInfo(GeoContinents.Oceania, 16, 2, "AS", "ASM", "", "American Samoa", "Territory of American Samoa", "", "", "AS", "American Samoa", "American Samoa", -14.305941, -170.6962, -14.382477, -170.846822, -14.229404, -170.545578),
				new GeoCountryInfo(GeoContinents.Europe, 20, 2, "AD", "AND", "AD", "Andorra", "Principality of Andorra", "Andorra", "Principat d'Andorra", "Andorra", "Andorra", "Andorra", 42.506285, 1.521801, 42.428748, 1.408705, 42.655791, 1.786639),
				new GeoCountryInfo(GeoContinents.Africa, 24, 2, "AO", "AGO", "AN", "Angola", "Republic of Angola", "Angola", "Republica de Angola", "Angola", "Angola", "Angola", -11.202692, 17.873887, -18.039104, 11.669562, -4.387944, 24.084444),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 28, 2, "AG", "ATG", "AG", "Antigua and Barbuda", "Antigua and Barbuda", "", "", "ATG", "Antigua/Barbuda", "Antigua and Barbuda", 17.060816, -61.796428, 16.997382, -61.90982, 17.176495, -61.657419),
				new GeoCountryInfo(GeoContinents.Europe, 31, 2, "AZ", "AZE", "AZ", "Azerbaijan", "Republic of Azerbaijan", "Azarbaycan", "Azarbaycan Respublikasi", "Azerbaijan", "Azerbaijan", "Azerbaijan", 40.143105, 47.576927, 38.39199, 44.764683, 41.91234, 50.368065),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 32, 2, "AR", "ARG", "8", "Argentina", "Argentine Republic", "Argentina", "Republica Argentina", "Argentina", "Argentina", "Argentina", -38.416097, -63.616672, -55.057714, -73.56036, -21.780813, -53.637481),
				new GeoCountryInfo(GeoContinents.Oceania, 36, 1, "AU", "AUS", "9", "Australia", "Commonwealth of Australia", "", "", "Australia", "Australia", "Australia", -25.274398, 133.775136, -43.658327, 112.923972, -9.226805, 153.638673),
				new GeoCountryInfo(GeoContinents.Europe, 40, 1, "AT", "AUT", "10", "Austria", "Republic of Austria", "Oesterreich", "Republik Oesterreich", "Austria", "Austria", "Austria", 47.516231, 14.550072, 46.372335, 9.530783, 49.020608, 17.160686),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 44, 2, "BS", "BHS", "BH", "Bahamas", "Bahamas", "", "", "Bahamas", "Bahamas", "Bahamas", 25.03428, -77.39628, 20.912131, -80.474946, 27.263362, -72.712068),
				new GeoCountryInfo(GeoContinents.Asia, 48, 2, "BH", "BHR", "BB", "Bahrain", "Kingdom of Bahrain", "Al Bahrayn", "Mamlakat al Bahrayn", "Bahrain", "Bahrain", "Bahrain", 26.0667, 50.5577, 25.57984, 50.37815, 26.326528, 50.822863),
				new GeoCountryInfo(GeoContinents.Asia, 50, 2, "BD", "BGD", "BG", "Bangladesh", "People's Republic of Bangladesh", "Banladesh", "Gana Prajatantri Banladesh", "Bangladesh", "Bangladesh", "Bangladesh", 23.684994, 90.356331, 20.75438, 88.008588, 26.634243, 92.680115),
				new GeoCountryInfo(GeoContinents.Europe, 51, 2, "AM", "ARM", "AM", "Armenia", "Republic of Armenia", "Hayastan", "Hayastani Hanrapetut'yun", "Armenia", "Armenia", "Armenia", 40.069099, 45.038189, 38.840244, 43.447211, 41.300993, 46.634222),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 52, 2, "BB", "BRB", "BA", "Barbados", "", "", "", "Barbados", "Barbados", "Barbados", 13.193887, -59.543198, 13.044999, -59.65103, 13.335126, -59.420097),
				new GeoCountryInfo(GeoContinents.Europe, 56, 1, "BE", "BEL", "11", "Belgium", "Kingdom of Belgium", "Belgique/Belgie", "Royaume de Belgique/Koninkrijk Belgie", "Belgium", "Belgium", "Belgium", 50.503887, 4.469936, 49.497013, 2.54494, 51.505144, 6.408124),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 60, 2, "BM", "BMU", "", "Bermuda", "", "", "", "Bermuda", "Bermuda", "Bermuda", 32.3078, -64.7505, 32.24705, -64.886788, 32.391305, -64.647377),
				new GeoCountryInfo(GeoContinents.Asia, 64, 2, "BT", "BTN", "BT", "Bhutan", "Kingdom of Bhutan", "Druk Yul", "Druk Gyalkhap", "Bhutan", "Bhutan", "Bhutan", 27.514162, 90.433601, 26.702016, 88.746473, 28.360825, 92.125232),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 68, 2, "BO", "BOL", "13", "Bolivia", "Plurinational State of Bolivia", "", "", "Bolivia", "Bolivia", "Bolivia", -16.290154, -63.588653, -22.898089, -69.64499, -9.669323, -57.453803),
				new GeoCountryInfo(GeoContinents.Europe, 70, 2, "BA", "BIH", "BC", "Bosnia and Herzegovina", "", "Bosna i Hercegovina", "", "BIH", "Bosnia/Herzegov", "Bosnia and Herzegovina", 43.915886, 17.679076, 42.556406, 15.722366, 45.276626, 19.621935),
				new GeoCountryInfo(GeoContinents.Africa, 72, 2, "BW", "BWA", "BW", "Botswana", "Republic of Botswana", "Botswana", "Republic of Botswana", "Botswana", "Botswana", "Botswana", -22.328474, 24.684866, -26.907545, 19.998905, -17.778137, 29.375303),
				new GeoCountryInfo(GeoContinents.Antarctica, 74, 2, "BV", "BVT", "", "Bouvet Island", "Bouvet Island", "", "", "BVT", "Bouvet Island", "Bouvet Island", -54.420791, 3.346449, -54.451543, 3.285149, -54.400322, 3.487975),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 76, 2, "BR", "BRA", "14", "Brazil", "Federative Republic of Brazil", "Brasil", "Republica Federativa do Brasil", "Brazil", "Brazil", "Brazil", -14.235004, -51.92528, -33.75099, -73.982817, 5.271601, -34.792907),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 84, 2, "BZ", "BLZ", "BZ", "Belize", "Belize", "", "", "Belize", "Belize", "Belize", 17.189877, -88.49765, 15.885618, -89.227587, 18.495941, -87.491726),
				new GeoCountryInfo(GeoContinents.Asia, 86, 2, "IO", "IOT", "", "British Indian Ocean Territory", "British Indian Ocean Territory", "", "", "BIOT", "BIOT", "British Indian Ocean Territory", -7.334755, 72.424232, -7.44407, 72.353768, -7.22544, 72.494696),
				new GeoCountryInfo(GeoContinents.Oceania, 90, 2, "SB", "SLB", "SO", "Solomon Islands", "Solomon Islands", "Solomon Islands", "", "SLB", "Solomon Islands", "Solomon Islands", -9.64571, 160.156194, -11.863458, 155.48624, -6.58924, 162.752884),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 92, 2, "VG", "VGB", "", "British Virgin Islands", "British Virgin Islands", "", "", "VGB", "Virgin Is", "British Virgin Islands", 18.420695, -64.639968, 18.306332, -64.85046, 18.529889, -64.320402),
				new GeoCountryInfo(GeoContinents.Asia, 96, 2, "BN", "BRN", "", "Brunei Darussalam", "Brunei Darussalam", "", "", "BRN", "BRN", "Brunei Darussalam", 4.535277, 114.727669, 4.002508, 114.076063, 5.047166, 115.363562),
				new GeoCountryInfo(GeoContinents.Europe, 100, 2, "BG", "BGR", "15", "Bulgaria", "Republic of Bulgaria", "Balgariya", "Republika Balgariya", "Bulgaria", "Bulgaria", "Bulgaria", 42.733883, 25.48583, 41.235446, 22.357344, 44.215124, 28.609263),
				new GeoCountryInfo(GeoContinents.Asia, 104, 2, "MM", "MMR", "12", "Myanmar (Burma)", "Myanmar", "Myanmar", "", "Myanmar", "Burma (Myanmar)", "Burma (Myanmar)", 21.913965, 95.956223, 9.599032, 92.171808, 28.547835, 101.170271),
				new GeoCountryInfo(GeoContinents.Africa, 108, 2, "BI", "BDI", "BI", "Burundi", "Republic of Burundi", "Burundi", "Republique du Burundi/Republika y'u Burundi", "Burundi", "Burundi", "Burundi", -3.373056, 29.918886, -4.469228, 29.000993, -2.309987, 30.850172),
				new GeoCountryInfo(GeoContinents.Europe, 112, 2, "BY", "BLR", "", "Belarus", "Republic of Belarus", "Byelarus'", "Respublika Byelarus'", "Belarus", "Belarus", "Belarus", 53.709807, 27.953389, 51.262011, 23.178337, 56.171871, 32.77682),
				new GeoCountryInfo(GeoContinents.Asia, 116, 2, "KH", "KHM", "KH", "Cambodia", "Kingdom of Cambodia", "Kampuchea", "Preahreacheanachakr Kampuchea", "Cambodia", "Cambodia", "Cambodia", 12.565679, 104.990963, 9.276808, 102.333542, 14.690179, 107.627687),
				new GeoCountryInfo(GeoContinents.Africa, 120, 2, "CM", "CMR", "17", "Cameroon", "Republic of Cameroon", "Cameroun/Cameroon", "Republique du Cameroun/Republic of Cameroon", "Cameroon", "Cameroon", "Cameroon", 7.369722, 12.354722, 1.655899, 8.494763, 13.083399, 16.194407),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 124, 2, "CA", "CAN", "18", "Canada", "Canada", "", "", "Canada", "Canada", "Canada", 56.130366, -106.346771, 42, -142, 70, -50),
				new GeoCountryInfo(GeoContinents.Africa, 132, 2, "CV", "CPV", "CV", "Cape Verde", "Republic of Cape Verde", "Cabo Verde", "Republica de Cabo Verde", "Cape Verde", "Cape Verde", "Cape Verde", 15.121728, -23.605081, 14.899668, -23.781329, 15.343788, -23.428833),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 136, 2, "KY", "CYM", "", "Cayman Islands", "Cayman Islands", "", "", "CYM", "Cayman Islands", "Cayman Islands", 19.3133, -81.2546, 19.262839, -81.420063, 19.396557, -81.083848),
				new GeoCountryInfo(GeoContinents.Africa, 140, 2, "CF", "CAF", "CF", "Central African Republic", "Central African Republic", "", "Republique Centrafricaine", "CAR", "Cent Africa Rep", "Central African Republic", 6.611111, 20.939444, 2.220857, 14.415098, 11.017956, 27.458305),
				new GeoCountryInfo(GeoContinents.Asia, 144, 2, "LK", "LKA", "19", "Sri Lanka", "Democratic Socialist Republic of Sri Lanka", "Shri Lamka/Ilankai", "Shri Lamka Prajatantrika Samajaya di Janarajaya/Ilankai Jananayaka Choshalichak Kutiyarachu", "Sri Lanka", "Sri Lanka", "Sri Lanka", 7.873054, 80.771797, 5.919077, 79.628906, 9.83585, 81.878702),
				new GeoCountryInfo(GeoContinents.Africa, 148, 2, "TD", "TCD", "TD", "Chad", "Republic of Chad", "Tchad/Tshad", "Republique du Tchad/Jumhuriyat Tshad", "Chad", "Chad", "Chad", 15.454166, 18.732207, 7.442975, 13.469999, 23.449235, 24.000001),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 152, 1, "CL", "CHL", "20", "Chile", "Republic of Chile", "Chile", "Republica de Chile", "Chile", "Chile", "Chile", -35.675147, -71.542969, -55.97978, -75.696786, -17.498329, -66.418201),
				new GeoCountryInfo(GeoContinents.Asia, 156, 1, "CN", "CHN", "76", "China", "People's Republic of China", "Zhongguo", "Zhonghua Renmin Gongheguo", "PRC", "China", "China", 35.86166, 104.195397, 18.153521, 73.499413, 53.560974, 134.772809),
				new GeoCountryInfo(GeoContinents.Asia, 158, 2, "TW", "TWN", "", "Taiwan", "Taiwan, Province of China", "", "", "Taiwan", "Taiwan", "Taiwan", 23.69781, 120.960515, 21.896695, 120.027801, 25.30044, 122.006905),
				new GeoCountryInfo(GeoContinents.Asia, 162, 2, "CX", "CXR", "", "Christmas Island", "Territory of Christmas Island", "", "", "CXR", "CXR", "Christmas Island", -10.447525, 105.690449, -10.570087, 105.533316, -10.412374, 105.712647),
				new GeoCountryInfo(GeoContinents.Asia, 166, 2, "CC", "CCK", "", "Cocos (Keeling) Islands", "Territory of Cocos (Keeling) Islands", "", "", "CCK", "CCK", "Cocos (Keeling) Islands", -12.170873, 96.841739, -12.207169, 96.816723, -12.13439, 96.866755),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 170, 2, "CO", "COL", "22", "Colombia", "Republic of Colombia", "Colombia", "Republica de Colombia", "Colombia", "Colombia", "Colombia", 4.570868, -74.297333, -4.22711, -79.00835, 12.458457, -66.851923),
				new GeoCountryInfo(GeoContinents.Africa, 174, 2, "KM", "COM", "", "Comoros", "Union of the Comoros", "Comores", "Union des Comores", "Comoros", "Comoros", "Comoros", -11.6455, 43.3333, -11.939322, 43.219421, -11.364639, 43.525772),
				new GeoCountryInfo(GeoContinents.Africa, 175, 2, "YT", "MYT", "", "Mayotte", "Territorial Collectivity of Mayotte", "", "", "Mayotte", "Mayotte", "Mayotte", -12.8275, 45.166244, -13.006161, 45.01817, -12.636537, 45.300177),
				new GeoCountryInfo(GeoContinents.Africa, 178, 2, "CG", "COG", "", "Congo", "Congo", "", "", "Congo", "Congo", "Congo", -0.228021, 15.827659, -5.028948, 11.149547, 3.707791, 18.643611),
				new GeoCountryInfo(GeoContinents.Africa, 180, 2, "CD", "COD", "ZR", "Congo", "The Democratic Republic of the Congo", "", "", "Congo", "Congo", "Congo", -0.228021, 15.827659, -5.028948, 11.149547, 3.707791, 18.643611),
				new GeoCountryInfo(GeoContinents.Oceania, 184, 2, "CK", "COK", "", "Cook Islands", "Cook Islands", "", "", "COK", "Cook Islands", "Cook Islands", -21.236736, -159.777671, -21.273064, -159.831347, -21.198695, -159.723746),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 188, 2, "CR", "CRI", "CR", "Costa Rica", "Republic of Costa Rica", "Costa Rica", "Republica de Costa Rica", "Costa Rica", "Costa Rica", "Costa Rica", 9.748917, -83.753428, 8.040697, -85.955711, 11.21968, -82.552657),
				new GeoCountryInfo(GeoContinents.Europe, 191, 2, "HR", "HRV", "", "Croatia", "Republic of Croatia", "Hrvatska", "Republika Hrvatska", "Croatia", "Croatia", "Croatia", 45.1, 15.2, 42.392346, 13.489691, 46.555223, 19.448052),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 192, 2, "CU", "CUB", "CU", "Cuba", "Republic of Cuba", "Cuba", "Republica de Cuba", "Cuba", "Cuba", "Cuba", 21.521757, -77.781167, 19.825899, -85.071256, 23.276752, -74.132223),
				new GeoCountryInfo(GeoContinents.Europe, 196, 2, "CY", "CYP", "CY", "Cyprus", "Republic of Cyprus", "Kypros/Kibris", "Kypriaki Dimokratia/Kibris Cumhuriyeti", "Cyprus", "Cyprus", "Cyprus", 35.126413, 33.429859, 34.632303, 32.268707, 35.707199, 34.6045),
				new GeoCountryInfo(GeoContinents.Europe, 203, 2, "CZ", "CZE", "CZ", "Czech Republic", "Czech Republic", "Cesko", "Ceska Republika", "CZE", "Czech Republic", "Czech Republic", 49.817492, 15.472962, 48.551808, 12.090589, 51.055718, 18.859236),
				new GeoCountryInfo(GeoContinents.Africa, 204, 2, "BJ", "BEN", "", "Benin", "Republic of Benin", "Benin", "Republique du Benin", "Benin", "Benin", "Benin", 9.30769, 2.315834, 6.235631, 0.776667, 12.408611, 3.843342),
				new GeoCountryInfo(GeoContinents.Europe, 208, 2, "DK", "DNK", "26", "Denmark", "Kingdom of Denmark", "Danmark", "Kongeriget Danmark", "Denmark", "Denmark", "Denmark", 56.26392, 9.501785, 54.559121, 8.07224, 57.751813, 12.78975),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 212, 2, "DM", "DMA", "", "Dominica", "Commonwealth of Dominica", "", "", "Dominica", "Dominica", "Dominica", 15.414999, -61.370976, 15.207682, -61.47983, 15.640063, -61.240303),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 214, 2, "DO", "DOM", "70", "Dominican Republic", "Dominican Republic", "La Dominicana", "Republica Dominicana", "DOM", "Dominican Rep", "Dominican Republic", 18.735693, -70.162651, 17.47009, -72.007509, 19.931718, -68.323406),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 218, 2, "EC", "ECU", "28", "Ecuador", "Republic of Ecuador", "Ecuador", "Republica del Ecuador", "Ecuador", "Ecuador", "Ecuador", -1.831239, -78.183406, -5.014351, -81.08498, 1.428418, -75.188794),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 222, 2, "SV", "SLV", "75", "El Salvador", "Republic of El Salvador", "El Salvador", "Republica de El Salvador", "SLV", "El Salvador", "El Salvador", 13.794185, -88.89653, 13.155431, -90.12681, 14.450556, -87.683751),
				new GeoCountryInfo(GeoContinents.Africa, 226, 2, "GQ", "GNQ", "GQ", "Equatorial Guinea", "Republic of Equatorial Guinea", "Guinea Ecuatorial/Guinee equatoriale", "Republica de Guinea Ecuatorial/Republique de Guinee equatoriale", "GNQ", "Equator Guinea", "Equatorial Guinea", 1.650801, 10.267895, 0.887196, 9.301729, 2.349415, 11.3333),
				new GeoCountryInfo(GeoContinents.Africa, 231, 2, "ET", "ETH", "", "Ethiopia", "Federal Democratic Republic of Ethiopia", "Ityop'iya", "Ityop'iya Federalawi Demokrasiyawi Ripeblik", "FDRE", "Ethiopia", "Ethiopia", 9.145, 40.489673, 3.404135, 32.997734, 14.894214, 47.999999),
				new GeoCountryInfo(GeoContinents.Africa, 232, 2, "ER", "ERI", "ER", "Eritrea", "State of Eritrea", "Ertra", "Hagere Ertra", "Eritrea", "Eritrea", "Eritrea", 15.179384, 39.782334, 12.354723, 36.433347, 18.021209, 43.142977),
				new GeoCountryInfo(GeoContinents.Europe, 233, 2, "EE", "EST", "ES", "Estonia", "Republic of Estonia", "Eesti", "Eesti Vabariik", "Estonia", "Estonia", "Estonia", 58.595272, 25.013607, 57.509316, 21.764372, 59.700283, 28.210138),
				new GeoCountryInfo(GeoContinents.Europe, 234, 2, "FO", "FRO", "", "Faroe Islands", "", "Foroyar", "", "FRO", "Faeroe Islands", "Faeroe Islands", 61.892635, -6.911806, 61.390905, -7.691905, 62.394099, -6.251564),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 238, 2, "FK", "FLK", "", "Malvinas", "Falkland Islands (Islas Malvinas)", "", "", "Malvinas", "Falkland Is", "Falkland Islands", -51.796253, -59.523613, -52.395296, -61.347571, -51.233259, -57.716114),
				new GeoCountryInfo(GeoContinents.Antarctica, 239, 2, "GS", "SGS", "", "South Georgia", "South Georgia and the South Sandwich Islands", "", "", "SGSSI", "South Georgia", "South Georgia", 32.157435, -82.907123, 30.35559, -85.605164, 35.000658, -80.840786),
				new GeoCountryInfo(GeoContinents.Oceania, 242, 2, "FJ", "FJI", "", "Fiji", "Republic of the Fiji Islands", "Fiji/Viti", "Republic of the Fiji Islands/Matanitu ko Viti", "Fiji", "Fiji", "Fiji", -17.713371, 178.065032, -19.216151, 176.909494, -15.713723, 176.909494),
				new GeoCountryInfo(GeoContinents.Europe, 246, 2, "FI", "FIN", "34", "Finland", "Republic of Finland", "Suomi/Finland", "Suomen tasavalta/Republiken Finland", "Finland", "Finland", "Finland", 61.92411, 25.748151, 59.737038, 20.54741, 70.092112, 31.587099),
				new GeoCountryInfo(GeoContinents.Europe, 248, 2, "AX", "ALA", "", "Aland Islands", "Aland Islands", "", "Åland Islands", "ALA", "Aland Islands", "Aland Islands", 60.338548, 20.271258, 59.734261, 19.278988, 60.664982, 21.478265),
				new GeoCountryInfo(GeoContinents.Europe, 250, 1, "FR", "FRA", "35", "France", "French Republic", "France", "Republique francaise", "France", "France", "France", 46.227638, 2.213749, 41.342327, -5.140402, 51.088961, 9.559793),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 254, 2, "GF", "GUF", "", "French Guiana", "Department of Guiana", "Guyane", "", "GUF", "French Guiana", "French Guiana", 3.933889, -53.125782, 2.109287, -54.554437, 5.757189, -51.633596),
				new GeoCountryInfo(GeoContinents.Oceania, 258, 2, "PF", "PYF", "", "French Polynesia", "Overseas Lands of French Polynesia", "Polynesie Francaise", "Pays d'outre-mer de la Polynesie Francaise", "PYF", "French Poly", "French Polynesia", -17.679742, -149.406843, -17.880432, -149.620887, -17.494411, -149.125156),
				new GeoCountryInfo(GeoContinents.Antarctica, 260, 2, "TF", "ATF", "", "French Southern and Antarctic Lands", "French Southern and Antartic Lands", "French Southern Territories", "", "ATF", "ATF", "French Southern Territories", -49.280366, 69.348557, -49.733917, 68.609018, -48.449741, 70.556602),
				new GeoCountryInfo(GeoContinents.Africa, 262, 2, "DJ", "DJI", "DJ", "Djibouti", "Republic of Djibouti", "Djibouti/Jibuti", "Republique de Djibouti/Jumhuriyat Jibuti", "Djibouti", "Djibouti", "Djibouti", 11.825138, 42.590275, 10.931944, 41.759722, 12.713395, 43.416973),
				new GeoCountryInfo(GeoContinents.Africa, 266, 2, "GA", "GAB", "GA", "Gabon", "Gabonese Republic", "Gabon", "Republique gabonaise", "Gabon", "Gabon", "Gabon", -0.803689, 11.609444, -3.958372, 8.699052, 2.318109, 14.520556),
				new GeoCountryInfo(GeoContinents.Europe, 268, 2, "GE", "GEO", "GE", "Georgia", "", "Sak'art'velo", "", "Georgia", "Georgia", "Georgia", 32.157435, -82.907123, 30.35559, -85.605164, 35.000658, -80.840786),
				new GeoCountryInfo(GeoContinents.Africa, 270, 2, "GM", "GMB", "GM", "Gambia", "Gambia", "", "", "Gambia", "Gambia", "Gambia", 13.443182, -15.310139, 13.065182, -16.813631, 13.826389, -13.79861),
				new GeoCountryInfo(GeoContinents.Asia, 275, 2, "PS", "PSE", "", "Palestine", "State of Palestine", "", "", "Palestine", "Palestine", "Palestine", 31.952162, 35.233154, 31.342602, 34.880274, 32.552099, 35.574052),
				new GeoCountryInfo(GeoContinents.Europe, 276, 1, "DE", "DEU", "6", "Germany", "Federal Republic of Germany", "Deutschland", "Bundesrepublik Deutschland", "Germany", "Germany", "Germany", 51.165691, 10.451526, 47.270111, 5.866342, 55.058347, 15.041896),
				new GeoCountryInfo(GeoContinents.Africa, 288, 2, "GH", "GHA", "GH", "Ghana", "Republic of Ghana", "", "", "Ghana", "Ghana", "Ghana", 7.946527, -1.023194, 4.738873, -3.260786, 11.166667, 1.199362),
				new GeoCountryInfo(GeoContinents.Europe, 292, 2, "GI", "GIB", "", "Gibraltar", "Gibraltar", "", "", "Gibraltar", "Gibraltar", "Gibraltar", 36.140751, -5.353585, 36.108834, -5.367415, 36.155118, -5.338419),
				new GeoCountryInfo(GeoContinents.Oceania, 296, 2, "KI", "KIR", "", "Kiribati", "Republic of Kiribati", "Kiribati", "Republic of Kiribati", "Kiribati", "Kiribati", "Kiribati", 1.871114, -157.360669, 1.692371, -157.561493, 2.048331, -157.158347),
				new GeoCountryInfo(GeoContinents.Europe, 300, 2, "GR", "GRC", "36", "Greece", "Hellenic Republic", "Ellas or Ellada", "Elliniki Dhimokratia", "Greece", "Greece", "Greece", 39.074208, 21.824312, 34.801021, 19.373587, 41.749057, 28.246955),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 304, 2, "GL", "GRL", "", "Greenland", "Greenland", "Kalaallit Nunaat", "", "Greenland", "Greenland", "Greenland", 71.706936, -42.604303, 59.777401, -73.035063, 83.609581, -11.312319),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 308, 2, "GD", "GRD", "GR", "Grenada", "Grenada", "", "", "Grenada", "Grenada", "Grenada", 12.1165, -61.679, 11.984872, -61.802727, 12.235087, -61.58472),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 312, 2, "GP", "GLP", "", "Guadeloupe", "Department of Guadeloupe", "Guadeloupe", "Departement de la Guadeloupe", "Guadeloupe", "Guadeloupe", "Guadeloupe", 16.265, -61.551, 15.831938, -61.809081, 16.514251, -61.001672),
				new GeoCountryInfo(GeoContinents.Oceania, 316, 2, "GU", "GUM", "", "Guam", "Territory of Guam", "Guahan", "Guahan", "Guam", "Guam", "Guam", 13.444304, 144.793731, 13.24619, 144.61838, 13.654224, 144.956536),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 320, 2, "GT", "GTM", "37", "Guatemala", "Republic of Guatemala", "Guatemala", "Republica de Guatemala", "Guatemala", "Guatemala", "Guatemala", 15.783471, -90.230759, 13.740021, -92.231835, 17.815711, -88.225615),
				new GeoCountryInfo(GeoContinents.Africa, 324, 2, "GN", "GIN", "GU", "Guinea", "Republic of Guinea", "Guinee", "Republique de Guinee", "Guinea", "Guinea", "Guinea", 9.945587, -9.696645, 7.190909, -15.078206, 12.674616, -7.637853),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 328, 2, "GY", "GUY", "GY", "Guyana", "Cooperative Republic of Guyana", "", "", "Guyana", "Guyana", "Guyana", 4.860416, -58.93018, 1.164724, -61.414905, 8.548255, -56.49112),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 332, 2, "HT", "HTI", "38", "Haiti", "Republic of Haiti", "Haiti/Ayiti", "Republique d'Haiti/Repiblik d' Ayiti", "Haiti", "Haiti", "Haiti", 18.971187, -72.285215, 18.022078, -74.48091, 20.089614, -71.621754),
				new GeoCountryInfo(GeoContinents.Antarctica, 334, 2, "HM", "HMD", "", "Heard Island and McDonald Islands", "Territory of Heard Island and McDonald Islands", "", "", "HIMI", "HIMI", "HIMI", -53.08181, 73.504158, -53.191547, 73.25124, -52.961616, 73.776083),
				new GeoCountryInfo(GeoContinents.Europe, 336, 2, "VA", "VAT", "", "Vatican", "Holy See (Vatican City State)", "", "", "Vatican", "Holy See", "Holy See", 41.902916, 12.453389, 41.900197, 12.445687, 41.907561, 12.458479),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 340, 2, "HN", "HND", "HO", "Honduras", "Republic of Honduras", "Honduras", "Republica de Honduras", "Honduras", "Honduras", "Honduras", 15.199999, -86.241905, 12.984224, -89.355148, 16.516539, -83.136076),
				new GeoCountryInfo(GeoContinents.Asia, 344, 2, "HK", "HKG", "", "Hong Kong", "Hong Kong Special Administrative Region", "Xianggang", "Xianggang Tebie Xingzhengqu", "HK", "Hong Kong", "Hong Kong", 22.396428, 114.109497, 22.153388, 113.835079, 22.561968, 114.406445),
				new GeoCountryInfo(GeoContinents.Europe, 348, 2, "HU", "HUN", "HU", "Hungary", "Republic of Hungary", "Magyarorszag", "Magyar Koztarsasag", "Hungary", "Hungary", "Hungary", 47.162494, 19.503304, 45.737088, 16.113386, 48.585234, 22.897379),
				new GeoCountryInfo(GeoContinents.Europe, 352, 2, "IS", "ISL", "46", "Iceland", "Republic of Iceland", "Island", "Lydveldid Island", "Iceland", "Iceland", "Iceland", 64.963051, -19.020835, 63.296102, -24.546523, 66.566318, -13.495815),
				new GeoCountryInfo(GeoContinents.Asia, 356, 1, "IN", "IND", "41", "India", "Republic of India", "India/Bharat", "Republic of India/Bharatiya Ganarajya", "India", "India", "India", 20.593684, 78.96288, 6.747138, 68.162795, 35.50434, 97.395555),
				new GeoCountryInfo(GeoContinents.Asia, 360, 2, "ID", "IDN", "42", "Indonesia", "Republic of Indonesia", "Indonesia", "Republik Indonesia", "Indonesia", "Indonesia", "Indonesia", -0.789275, 113.921327, -11.004673, 95.011064, 5.906821, 141.018662),
				new GeoCountryInfo(GeoContinents.Asia, 364, 2, "IR", "IRN", "44", "Iran", "Islamic Republic of Iran", "", "", "Iran", "Iran", "Iran", 32.427908, 53.688046, 25.059428, 44.03189, 39.781675, 63.333336),
				new GeoCountryInfo(GeoContinents.Asia, 368, 2, "IQ", "IRQ", "43", "Iraq", "Republic of Iraq", "Al Iraq", "Al Jumhuriyah al-Iraqiyah", "Iraq", "Iraq", "Iraq", 33.223191, 43.679291, 29.061207, 38.793602, 37.380932, 48.575916),
				new GeoCountryInfo(GeoContinents.Europe, 372, 1, "IE", "IRL", "45", "Ireland", "Ireland", "Eire", "", "Ireland", "Ireland", "Ireland", 53.41291, -8.24389, 51.422195, -10.66945, 55.38671, -5.99471),
				new GeoCountryInfo(GeoContinents.Asia, 376, 2, "IL", "ISR", "47", "Israel", "State of Israel", "Yisra'el", "Medinat Yisra'el", "Israel", "Israel", "Israel", 31.046051, 34.851612, 29.490646, 34.267387, 33.332805, 35.896244),
				new GeoCountryInfo(GeoContinents.Europe, 380, 1, "IT", "ITA", "48", "Italy", "Italian Republic", "Italia", "Repubblica Italiana", "Italy", "Italy", "Italy", 41.87194, 12.56738, 35.49292, 6.62672, 47.092, 18.520501),
				new GeoCountryInfo(GeoContinents.Africa, 384, 2, "CI", "CIV", "IC", "Côte d'Ivoire", "Republic of Cote d'Ivoire", "Cote d'Ivoire", "Republique de Cote d'Ivoire", "CIV", "C?te d'Ivoire", "C?te d'Ivoire", 7.539989, -5.54708, 4.351007, -8.602058, 10.740015, -2.493031),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 388, 2, "JM", "JAM", "JA", "Jamaica", "Jamaica", "", "", "Jamaica", "Jamaica", "Jamaica", 18.109581, -77.297508, 17.705724, -78.368846, 18.52531, -76.183159),
				new GeoCountryInfo(GeoContinents.Asia, 392, 2, "JP", "JPN", "49", "Japan", "Japan", "Nihon/Nippon", "Nihon-koku/Nippon-koku", "Japan", "Japan", "Japan", 36.204824, 138.252924, 24.048692, 122.93383, 45.522771, 145.81755),
				new GeoCountryInfo(GeoContinents.Europe, 398, 2, "KZ", "KAZ", "KZ", "Kazakhstan", "Republic of Kazakhstan", "Qazaqstan", "Qazaqstan Respublikasy", "Kazakhstan", "Kazakhstan", "Kazakhstan", 48.019573, 66.923684, 40.568584, 46.493671, 55.441983, 87.315415),
				new GeoCountryInfo(GeoContinents.Asia, 400, 2, "JO", "JOR", "50", "Jordan", "Hashemite Kingdom of Jordan", "Al Urdun", "Al Mamlakah al Urduniyah al Hashimiyah", "Jordan", "Jordan", "Jordan", 30.585164, 36.238414, 29.185036, 34.958336, 33.374687, 39.301154),
				new GeoCountryInfo(GeoContinents.Africa, 404, 2, "KE", "KEN", "KE", "Kenya", "Republic of Kenya", "Kenya", "Republic of Kenya/Jamhuri ya Kenya", "Kenya", "Kenya", "Kenya", -0.023559, 37.906193, -4.679681, 33.909821, 5.03342, 41.906831),
				new GeoCountryInfo(GeoContinents.Asia, 408, 2, "KP", "PRK", "KR", "North Korea", "Democratic People's Republic of Korea", "", "", "PRK", "North Korea", "North Korea", 40.339852, 127.510093, 37.673332, 124.173552, 43.01159, 130.674865),
				new GeoCountryInfo(GeoContinents.Asia, 410, 2, "KR", "KOR", "", "South Korea", "Republic of Korea", "", "", "KOR", "South Korea", "South Korea", 35.907757, 127.766922, 33.106109, 124.608139, 38.616931, 129.584671),
				new GeoCountryInfo(GeoContinents.Asia, 414, 2, "KW", "KWT", "KU", "Kuwait", "State of Kuwait", "Al Kuwayt", "Dawlat al Kuwayt", "Kuwait", "Kuwait", "Kuwait", 29.31166, 47.481766, 28.524446, 46.553039, 30.103706, 48.430457),
				new GeoCountryInfo(GeoContinents.Asia, 417, 2, "KG", "KGZ", "", "Kyrgyzstan", "Kyrgyz Republic", "Kyrgyzstan", "Kyrgyz Respublikasy", "Kyrgyzstan", "Kyrgyz Republic", "Kyrgyz Republic", 41.20438, 74.766098, 39.180254, 69.250998, 43.265356, 80.226559),
				new GeoCountryInfo(GeoContinents.Asia, 418, 2, "LA", "LAO", "", "Democratic Republic", "Lao People's Democratic Republic", "", "", "LAO", "Laos", "Laos", 38.905448, -77.039316, 38.896765, -77.055324, 38.91413, -77.023309),
				new GeoCountryInfo(GeoContinents.Asia, 422, 2, "LB", "LBN", "52", "Lebanon", "Lebanese Republic", "Lubnan", "Al Jumhuriyah al Lubnaniyah", "Lebanon", "Lebanon", "Lebanon", 33.854721, 35.862285, 33.055025, 35.103778, 34.69209, 36.62372),
				new GeoCountryInfo(GeoContinents.Africa, 426, 2, "LS", "LSO", "LS", "Lesotho", "Kingdom of Lesotho", "Lesotho", "Kingdom of Lesotho", "Lesotho", "Lesotho", "Lesotho", -29.609988, 28.233608, -30.675578, 27.011231, -28.570801, 29.455708),
				new GeoCountryInfo(GeoContinents.Europe, 428, 2, "LV", "LVA", "LA", "Latvia", "Republic of Latvia", "Latvija", "Latvijas Republika", "Latvia", "Latvia", "Latvia", 56.879635, 24.603189, 55.674776, 20.962346, 58.085568, 28.241402),
				new GeoCountryInfo(GeoContinents.Africa, 430, 2, "LR", "LBR", "54", "Liberia", "Republic of Liberia", "", "", "Liberia", "Liberia", "Liberia", 6.428055, -9.429499, 4.315413, -11.474248, 8.551986, -7.369254),
				new GeoCountryInfo(GeoContinents.Africa, 434, 2, "LY", "LBY", "53", "Libya", "Great Socialist People's Libyan Arab Jamahiriya", "", "Al Jumahiriyah al Arabiyah al Libiyah ash Shabiyah al Ishtirakiyah al Uzma", "Libya", "Libya", "Libya", 27.056776, 14.52804, 19.500429, 9.391466, 33.166787, 25.146954),
				new GeoCountryInfo(GeoContinents.Europe, 438, 2, "LI", "LIE", "", "Liechtenstein", "Principality of Liechtenstein", "Liechtenstein", "Fuerstentum Liechtenstein", "LIE", "Liechtenstein", "Liechtenstein", 47.166, 9.555373, 47.04829, 9.47162, 47.270546, 9.63565),
				new GeoCountryInfo(GeoContinents.Europe, 440, 2, "LT", "LTU", "LT", "Lithuania", "Republic of Lithuania", "Lietuva", "Lietuvos Respublika", "Lithuania", "Lithuania", "Lithuania", 55.169438, 23.881275, 53.896878, 20.954368, 56.45032, 26.835591),
				new GeoCountryInfo(GeoContinents.Europe, 442, 2, "LU", "LUX", "LU", "Luxembourg", "Grand Duchy of Luxembourg", "Luxembourg", "Grand Duche de Luxembourg", "Luxembourg", "Luxembourg", "Luxembourg", 49.815273, 6.129583, 49.447779, 5.735669, 50.18282, 6.53097),
				new GeoCountryInfo(GeoContinents.Asia, 446, 2, "MO", "MAC", "", "Macau", "Macao", "", "", "Macao", "Macao", "Macao", 22.198745, 113.543873, 22.109771, 113.527605, 22.217063, 113.598279),
				new GeoCountryInfo(GeoContinents.Africa, 450, 2, "MG", "MDG", "", "Madagascar", "Republic of Madagascar", "Madagascar/Madagasikara", "Republique de Madagascar/Repoblikan'i Madagasikara", "Madagascar", "Madagascar", "Madagascar", -18.766947, 46.869107, -25.606571, 43.185139, -11.951963, 50.483779),
				new GeoCountryInfo(GeoContinents.Africa, 454, 2, "MW", "MWI", "MW", "Malawi", "Republic of Malawi", "Malawi", "Dziko la Malawi", "Malawi", "Malawi", "Malawi", -13.254308, 34.301525, -17.135278, 32.678889, -9.367153, 35.924166),
				new GeoCountryInfo(GeoContinents.Asia, 458, 2, "MY", "MYS", "MS", "Malaysia", "Malaysia", "Malaysia", "", "Malaysia", "Malaysia", "Malaysia", 4.210484, 101.975766, 0.461421, 99.422836, 6.725747, 104.54256),
				new GeoCountryInfo(GeoContinents.Asia, 462, 2, "MV", "MDV", "MV", "Maldives", "Republic of Maldives", "Dhivehi Raajje", "Dhivehi Raajjeyge Jumhooriyyaa", "Maldives", "Maldives", "Maldives", 1.977246, 73.536103, 1.821331, 73.486261, 2.133162, 73.585945),
				new GeoCountryInfo(GeoContinents.Africa, 466, 2, "ML", "MLI", "MI", "Mali", "Republic of Mali", "Mali", "Republique de Mali", "Mali", "Mali", "Mali", 17.570692, -3.996166, 10.147811, -12.238884, 25.000058, 4.266666),
				new GeoCountryInfo(GeoContinents.Europe, 470, 2, "MT", "MLT", "ML", "Malta", "Republic of Malta", "Malta", "Repubblika ta' Malta", "Malta", "Malta", "Malta", 35.937496, 14.375416, 35.805811, 14.183349, 36.082146, 14.5755),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 474, 2, "MQ", "MTQ", "", "Martinique", "Department of Martinique", "Martinique", "Departement de la Martinique", "Martinique", "Martinique", "Martinique", 14.641528, -61.024174, 14.388647, -61.229093, 14.87845, -60.810527),
				new GeoCountryInfo(GeoContinents.Africa, 478, 2, "MR", "MRT", "MU", "Mauritania", "Islamic Republic of Mauritania", "Muritaniyah", "Al Jumhuriyah al Islamiyah al Muritaniyah", "Mauritania", "Mauritania", "Mauritania", 21.00789, -10.940835, 14.721273, -17.070133, 27.294444, -4.833334),
				new GeoCountryInfo(GeoContinents.Africa, 480, 2, "MU", "MUS", "MA", "Mauritius", "Republic of Mauritius", "Mauritius", "Republic of Mauritius", "Mauritius", "Mauritius", "Mauritius", -20.348404, 57.552152, -21.637, 55.7666, -18.7763, 59.5844),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 484, 1, "MX", "MEX", "57", "Mexico", "United Mexican States", "Mexico", "Estados Unidos Mexicanos", "Mexico", "Mexico", "Mexico", 23.634501, -102.552784, 14.534548, -118.363977, 32.718762, -86.710571),
				new GeoCountryInfo(GeoContinents.Europe, 492, 2, "MC", "MCO", "MO", "Monaco", "Principality of Monaco", "Monaco", "Principaute de Monaco", "Monaco", "Monaco", "Monaco", 43.738417, 7.424615, 43.724742, 7.409104, 43.751902, 7.439811),
				new GeoCountryInfo(GeoContinents.Asia, 496, 1, "MN", "MNG", "MN", "Mongolia", "Mongolia", "Mongol Uls", "", "Mongolia", "Mongolia", "Mongolia", 46.862496, 103.846656, 41.58152, 87.73762, 52.148696, 119.931948),
				new GeoCountryInfo(GeoContinents.Europe, 498, 2, "MD", "MDA", "MD", "Moldova", "Republic of Moldova", "", "", "Moldova", "Moldova", "Moldova", 47.411631, 28.369885, 45.466904, 26.616855, 48.491944, 30.162538),
				new GeoCountryInfo(GeoContinents.Europe, 499, 2, "ME", "MNE", "", "Montenegro", "Montenegro", "", "", "Montenegro", "Montenegro", "Montenegro", 42.708678, 19.37439, 41.84973, 18.433792, 43.558743, 20.357764),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 500, 2, "MS", "MSR", "", "Montserrat", "Montserrat", "", "", "Montserrat", "Montserrat", "Montserrat", 16.742498, -62.187366, 16.674695, -62.241517, 16.824051, -62.144175),
				new GeoCountryInfo(GeoContinents.Africa, 504, 2, "MA", "MAR", "56", "Morocco", "Kingdom of Morocco", "Al Maghrib", "Al Mamlakah al Maghribiyah", "Morocco", "Morocco", "Morocco", 31.791702, -7.09262, 27.666666, -13.172891, 35.922507, -0.996975),
				new GeoCountryInfo(GeoContinents.Africa, 508, 2, "MZ", "MOZ", "MZ", "Mozambique", "Republic of Mozambique", "Mocambique", "Republica de Mocambique", "Mozambique", "Mozambique", "Mozambique", -18.665695, 35.529562, -26.868108, 30.215549, -10.471202, 40.839121),
				new GeoCountryInfo(GeoContinents.Asia, 512, 2, "OM", "OMN", "OM", "Oman", "Sultanate of Oman", "Uman", "Saltanat Uman", "Oman", "Oman", "Oman", 21.512583, 55.923255, 16.650336, 52.000001, 26.405394, 59.839397),
				new GeoCountryInfo(GeoContinents.Africa, 516, 2, "NA", "NAM", "", "Namibia", "Republic of Namibia", "Namibia", "Republic of Namibia", "Namibia", "Namibia", "Namibia", -22.95764, 18.49041, -28.970638, 11.724246, -16.963485, 25.261751),
				new GeoCountryInfo(GeoContinents.Oceania, 520, 2, "NR", "NRU", "NR", "Nauru", "Republic of Nauru", "Nauru", "Republic of Nauru", "Nauru", "Nauru", "Nauru", -0.522778, 166.931503, -0.554189, 166.909548, -0.502639, 166.958928),
				new GeoCountryInfo(GeoContinents.Asia, 524, 2, "NP", "NPL", "NP", "Nepal", "Federal Democratic Republic of Nepal", "Nepal", "Sanghiya Loktantrik Ganatantra Nepal", "Nepal", "Nepal", "Nepal", 28.394857, 84.124008, 26.347779, 80.052222, 30.446945, 88.199297),
				new GeoCountryInfo(GeoContinents.Europe, 528, 1, "NL", "NLD", "", "Netherlands", "Kingdom of the Netherlands", "Nederland", "Koninkrijk der Nederlanden", "NLD", "Netherlands", "Netherlands", 52.132633, 5.291266, 50.750383, 3.3316, 53.6756, 7.22714),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 531, 2, "CW", "CUW", "", "Curaçao", "Curaçao", "", "", "Curaçao", "Curaçao", "Curaçao", 12.16957, -68.99002, 12.034326, -69.162454, 12.392435, -68.737335),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 533, 2, "AW", "ABW", "AW", "Aruba", "Aruba", "", "", "Aruba", "Aruba", "Aruba", 12.52111, -69.968338, 12.411765, -70.066025, 12.623378, -69.86588),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 534, 2, "SX", "SXM", "", "Sint Maarten", "Sint Maarten (Dutch)", "", "", "SXM", "Sint Maarten", "Sint Maarten", 18.027304, -63.05008, 18.005101, -63.137978, 18.125133, -62.970392),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 535, 2, "BQ", "BES", "", "Bonaire", "Caribbean Netherlands", "Bonaire, Sint Eustatius and Saba", "", "Bonaire", "Bonaire", "Bonaire", 12.178361, -68.238533, 12.024504, -68.420963, 12.312239, -68.195402),
				new GeoCountryInfo(GeoContinents.Oceania, 540, 2, "NC", "NCL", "59", "New Caledonia", "Territory of New Caledonia and Dependencies", "Nouvelle-Caledonie", "Territoire des Nouvelle-Caledonie et Dependances", "NCL", "New Caledonia", "New Caledonia", -20.904305, 165.618042, -22.881947, 163.569721, -19.539508, 168.133681),
				new GeoCountryInfo(GeoContinents.Oceania, 548, 2, "VU", "VUT", "VU", "Vanuatu", "Republic of Vanuatu", "Vanuatu", "Ripablik blong Vanuatu", "Vanuatu", "Vanuatu", "Vanuatu", -15.376706, 166.959158, -17.826103, 166.541758, -13.072455, 168.649063),
				new GeoCountryInfo(GeoContinents.Oceania, 554, 1, "NZ", "NZL", "61", "New Zealand", "New Zealand", "", "", "NZ", "New Zealand", "New Zealand", -40.900557, 174.885971, -47.768124, 166.426136, -34.129557, 179.062535),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 558, 2, "NI", "NIC", "NC", "Nicaragua", "Republic of Nicaragua", "Nicaragua", "Republica de Nicaragua", "Nicaragua", "Nicaragua", "Nicaragua", 12.865416, -85.207229, 10.708054, -87.691068, 15.030275, -82.592071),
				new GeoCountryInfo(GeoContinents.Africa, 562, 2, "NE", "NER", "NE", "Niger", "Republic of Niger", "Niger", "Republique du Niger", "Niger", "Niger", "Niger", 17.607789, 8.081666, 11.693756, 0.166667, 23.5, 15.999033),
				new GeoCountryInfo(GeoContinents.Africa, 566, 2, "NG", "NGA", "NI", "Nigeria", "Federal Republic of Nigeria", "", "", "Nigeria", "Nigeria", "Nigeria", 9.081999, 8.675277, 4.269857, 2.676932, 13.885644, 14.677981),
				new GeoCountryInfo(GeoContinents.Oceania, 570, 2, "NU", "NIU", "", "Niue", "Niue", "", "", "Niue", "Niue", "Niue", -19.054445, -169.867233, -19.155566, -169.949848, -18.95297, -169.774324),
				new GeoCountryInfo(GeoContinents.Oceania, 574, 2, "NF", "NFK", "", "Norfolk Island", "Territory of Norfolk Island", "", "", "NFK", "Norfolk Island", "Norfolk Island", -29.040835, 167.954712, -29.07514, 167.916219, -28.995388, 167.996926),
				new GeoCountryInfo(GeoContinents.Europe, 578, 2, "NO", "NOR", "58", "Norway", "Kingdom of Norway", "Norge", "Kongeriket Norge", "Norway", "Norway", "Norway", 60.472024, 8.468946, 57.970937, 4.626684, 71.185476, 31.149789),
				new GeoCountryInfo(GeoContinents.Oceania, 580, 2, "MP", "MNP", "", "Northern Mariana Islands", "Commonwealth of the Northern Mariana Islands", "", "", "CNMI", "N Mariana Is", "Northern Mariana Islands", 15.0979, 145.6739, 14.922522, 145.582344, 15.290259, 145.830551),
				new GeoCountryInfo(GeoContinents.Oceania, 581, 2, "UM", "UMI", "", "United States Minor Outlying Islands", "United States Minor Outlying Islands", "", "", "UMI", "UMI", "UMI", 5.88062, -162.07721, 5.865629, -162.101639, 5.895611, -162.051493),
				new GeoCountryInfo(GeoContinents.Oceania, 583, 2, "FM", "FSM", "", "Micronesia", "", "Micronesia", "Federated States of Micronesia", "FSM", "Micronesia, F ", "", 1, 8.857145, 2, 7.629661, 3, 7.011341),
				new GeoCountryInfo(GeoContinents.Oceania, 584, 2, "MH", "MHL", "MH", "Marshall Islands", "Republic of the Marshall Islands", "Marshall Islands", "Republic of the Marshall Islands", "RMI", "Marshall Is", "Marshall Islands", 7.131474, 171.184478, 7.052192, 171.027163, 7.169534, 171.38497),
				new GeoCountryInfo(GeoContinents.Oceania, 585, 2, "PW", "PLW", "", "Palau", "Republic of Palau", "Belau", "Beluu er a Belau", "Palau", "Palau", "Palau", 7.51498, 134.58252, 2.99912, 131.062032, 7.759268, 134.649191),
				new GeoCountryInfo(GeoContinents.Asia, 586, 2, "PK", "PAK", "62", "Pakistan", "Islamic Republic of Pakistan", "Pakistan", "Jamhuryat Islami Pakistan", "Pakistan", "Pakistan", "Pakistan", 30.375321, 69.345116, 23.694694, 60.872972, 37.084107, 77.835666),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 591, 2, "PA", "PAN", "PA", "Panama", "Republic of Panama", "Panama", "Republica de Panama", "Panama", "Panama", "Panama", 8.537981, -80.782127, 7.203556, -83.052241, 9.647779, -77.158488),
				new GeoCountryInfo(GeoContinents.Oceania, 598, 2, "PG", "PNG", "PG", "Papua New Guinea", "Independent State of Papua New Guinea", "Papuaniugini", "", "PNG", "PNG", "Papua New Guinea", -6.314993, 143.95555, -11.65786, 140.841969, -0.871319, 157.085723),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 600, 2, "PY", "PRY", "PY", "Paraguay", "Republic of Paraguay", "Paraguay", "Republica del Paraguay", "Paraguay", "Paraguay", "Paraguay", -23.442503, -58.443832, -27.588334, -62.638051, -19.287706, -54.258562),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 604, 2, "PE", "PER", "65", "Peru", "Republic of Peru", "Peru", "Republica del Peru", "Peru", "Peru", "Peru", -9.189967, -75.015152, -18.35158, -81.328504, -0.038777, -68.652329),
				new GeoCountryInfo(GeoContinents.Asia, 608, 2, "PH", "PHL", "66", "Philippines", "Republic of the Philippines", "Pilipinas", "Republika ng Pilipinas", "PHL", "Philippines", "Philippines", 12.879721, 121.774017, 4.613444, 116.703162, 19.574024, 126.604383),
				new GeoCountryInfo(GeoContinents.Oceania, 612, 2, "PN", "PCN", "", "Pitcairn", "Pitcairn Islands", "", "", "Pitcairn", "Pitcairn", "Pitcairn", -24.37649, -128.324367, -24.422495, -128.357047, -24.330485, -128.291687),
				new GeoCountryInfo(GeoContinents.Europe, 616, 2, "PL", "POL", "67", "Poland", "Republic of Poland", "Polska", "Rzeczpospolita Polska", "Poland", "Poland", "Poland", 51.919438, 19.145136, 49.002025, 14.122864, 54.835812, 24.145893),
				new GeoCountryInfo(GeoContinents.Europe, 620, 2, "PT", "PRT", "68", "Portugal", "Portuguese Republic", "Portugal", "Republica Portuguesa", "Portugal", "Portugal", "Portugal", 39.399872, -8.224454, 36.960177, -9.51711, 42.154204, -6.190209),
				new GeoCountryInfo(GeoContinents.Africa, 624, 2, "GW", "GNB", "GN", "Guinea-Bissau", "Republic of Guinea-Bissau", "Guine-Bissau", "Republica da Guine-Bissau", "GNB", "Guinea-Bissau", "Guinea-Bissau", 11.803749, -15.180413, 10.85997, -16.711735, 12.684722, -13.627504),
				new GeoCountryInfo(GeoContinents.Asia, 626, 2, "TL", "TLS", "", "Timor-Leste", "Timor-Leste", "", "", "TLS", "Timor-Leste", "Timor-Leste", -8.711485, 125.634764, -9.462656, 124.931763, -8.126806, 127.341634),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 630, 2, "PR", "PRI", "", "Puerto Rico", "Commonwealth of Puerto Rico", "", "", "PRI", "Puerto Rico", "Puerto Rico", 18.220833, -66.590149, 17.881428, -67.484239, 18.516009, -65.221109),
				new GeoCountryInfo(GeoContinents.Asia, 634, 2, "QA", "QAT", "QA", "Qatar", "State of Qatar", "Qatar", "Dawlat Qatar", "Qatar", "Qatar", "Qatar", 25.354826, 51.183884, 24.471118, 50.750055, 26.183092, 51.64326),
				new GeoCountryInfo(GeoContinents.Africa, 638, 2, "RE", "REU", "", "Réunion", "Department of Reunion", "Ile de la Reunion", "", "Réunion", "R?union", "R?union", -21.115141, 55.536384, -21.389622, 55.216405, -20.871755, 55.836553),
				new GeoCountryInfo(GeoContinents.Europe, 642, 2, "RO", "ROU", "73", "Romania", "Romania", "Romania", "", "Romania", "Romania", "Romania", 45.943161, 24.96676, 43.618619, 20.261759, 48.265274, 29.757101),
				new GeoCountryInfo(GeoContinents.Asia, 643, 1, "RU", "RUS", "RU", "Russia", "Russian Federation", "", "", "Russia", "Russia", "Russian Federation", 61.52401, 105.318756, 40, 27, 70, 179),
				new GeoCountryInfo(GeoContinents.Africa, 646, 2, "RW", "RWA", "RW", "Rwanda", "Republic of Rwanda", "Rwanda", "Republika y'u Rwanda", "Rwanda", "Rwanda", "Rwanda", -1.940278, 29.873888, -2.839839, 28.861754, -1.047571, 30.8994),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 652, 2, "BL", "BLM", "", "Saint Barthélemy", "Saint Barthélemy", "", "", "BLM", "BLM", "Saint Barthélemy", 17.9, -62.833333, 17.870828, -62.878489, 17.957015, -62.789214),
				new GeoCountryInfo(GeoContinents.Africa, 654, 2, "SH", "SHN", "", "Saint Helena", "Saint Helena, Ascension and Tristan da Cunha", "", "", "SHN", "St Helena", "St Helena", 38.505242, -122.470386, 38.486474, -122.495042, -15.904009, -5.633969),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 659, 2, "KN", "KNA", "KN", "Saint Kitts and Nevis", "Federation of Saint Kitts and Nevis", "", "", "KNA", "Scotland", "Scotland", 17.357822, -62.782998, 17.094157, -62.864617, 17.418201, -62.539694),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 660, 2, "AI", "AIA", "", "Anguilla", "Anguilla", "", "", "Anguilla", "Anguilla", "Anguilla", 18.220554, -63.068615, 18.149946, -63.190832, 18.296594, -62.922434),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 662, 2, "LC", "LCA", "LC", "Saint Lucia", "Saint Lucia", "", "", "LCA", "St Lucia", "St Lucia", 13.909444, -60.978893, 13.708117, -61.079671, 14.110932, -60.873098),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 663, 2, "MF", "MAF", "", "Saint Martin", "Saint Martin (French)", "", "", "MAF", "Saint Martin", "Saint Martin", 18.08255, -63.052251, 18.046575, -63.153326, 18.125133, -62.970392),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 666, 2, "PM", "SPM", "", "Saint Pierre and Miquelon", "Territorial Collectivity of Saint Pierre and Miquelon", "Saint-Pierre et Miquelon", "Departement de Saint-Pierre et Miquelon", "SPM", "St Pierre/Mique", "St Pierre and Miquelon", 46.8852, -56.3159, 46.749105, -56.405632, 47.14427, -56.118937),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 670, 2, "VC", "VCT", "", "Saint Vincent and the Grenadines", "Saint Vincent and the Grenadines", "", "", "VCT", "St Vinc/Grenad", "St Vincent and the Grenadines", 13.253383, -61.196251, 13.122553, -61.279077, 13.384213, -61.113424),
				new GeoCountryInfo(GeoContinents.Europe, 674, 2, "SM", "SMR", "SR", "San Marino", "Republic of San Marino", "San Marino", "Repubblica di San Marino", "San Marino", "San Marino", "San Marino", 43.94236, 12.457777, 43.89368, 12.403482, 43.992075, 12.516704),
				new GeoCountryInfo(GeoContinents.Africa, 678, 2, "ST", "STP", "", "Sao Tome and Principe", "Sao Tome and Principe", "São Tomé and Príncipe", "", "STP", "STP", "Sao Tome and Principe", 0.18636, 6.613081, -0.014004, 6.460475, 0.413394, 6.760652),
				new GeoCountryInfo(GeoContinents.Asia, 682, 2, "SA", "SAU", "SA", "Saudi Arabia", "Kingdom of Saudi Arabia", "Al Arabiyah as Suudiyah", "Al Mamlakah al Arabiyah as Suudiyah", "SAU", "Saudi Arabia", "Saudi Arabia", 23.885942, 45.079162, 16.379528, 34.548997, 32.154284, 55.666699),
				new GeoCountryInfo(GeoContinents.Africa, 686, 2, "SN", "SEN", "SE", "Senegal", "Republic of Senegal", "Senegal", "Republique du Senegal", "Senegal", "Senegal", "Senegal", 14.497401, -14.452362, 12.307289, -17.529848, 16.693054, -11.348607),
				new GeoCountryInfo(GeoContinents.Europe, 688, 2, "RS", "SRB", "", "Serbia", "Serbia", "", "", "Serbia", "Serbia", "Serbia", 44.016521, 21.005859, 42.231502, 18.838522, 46.190032, 23.006309),
				new GeoCountryInfo(GeoContinents.Africa, 690, 2, "SC", "SYC", "SC", "Seychelles", "Republic of Seychelles", "Seychelles", "Republic of Seychelles", "Seychelles", "Seychelles", "Seychelles", -4.679574, 55.491977, -4.806792, 55.359914, -4.560981, 55.539169),
				new GeoCountryInfo(GeoContinents.Africa, 694, 2, "SL", "SLE", "SL", "Sierra Leone", "Republic of Sierra Leone", "Sierra Leone", "Republic of Sierra Leone", "SLE", "Sierra Leone", "Sierra Leone", 8.460555, -11.779889, 6.899025, -13.302006, 9.999972, -10.271651),
				new GeoCountryInfo(GeoContinents.Asia, 702, 1, "SG", "SGP", "SI", "Singapore", "Republic of Singapore", "Singapore", "Republic of Singapore", "Singapore", "Singapore", "Singapore", 1.352083, 103.819836, 1.166398, 103.605575, 1.47088, 104.08568),
				new GeoCountryInfo(GeoContinents.Europe, 703, 2, "SK", "SVK", "", "Slovakia", "Slovak Republic", "Slovensko", "Slovenska Republika", "Slovakia", "Slovakia", "Slovakia", 48.669026, 19.699024, 47.731159, 16.833182, 49.613805, 22.559281),
				new GeoCountryInfo(GeoContinents.Asia, 704, 2, "VN", "VNM", "94", "Vietnam", "Viet Nam", "", "", "Vietnam", "Viet Nam", "Viet Nam", 14.058324, 108.277199, 8.412729, 102.14441, 23.393395, 109.468975),
				new GeoCountryInfo(GeoContinents.Europe, 705, 2, "SI", "SVN", "SN", "Slovenia", "Republic of Slovenia", "Slovenija", "Republika Slovenija", "Slovenia", "Slovenia", "Slovenia", 46.151241, 14.995463, 45.421673, 13.375335, 46.876659, 16.596685),
				new GeoCountryInfo(GeoContinents.Africa, 706, 2, "SO", "SOM", "SM", "Somalia", "Somalia", "Soomaaliya", "Jamhuuriyada Demuqraadiga Soomaaliyeed", "Somalia", "Somalia", "Somalia", 5.152149, 46.199616, -1.662041, 40.994373, 11.988614, 51.413028),
				new GeoCountryInfo(GeoContinents.Africa, 710, 2, "ZA", "ZAF", "91", "South Africa", "Republic of South Africa", "", "", "RSA", "South Africa", "South Africa", -30.559482, 22.937506, -34.833138, 16.460832, -22.125386, 32.890991),
				new GeoCountryInfo(GeoContinents.Africa, 716, 2, "ZW", "ZWE", "", "Zimbabwe", "Republic of Zimbabwe", "", "", "Zimbabwe", "Zimbabwe", "Zimbabwe", -19.015438, 29.154857, -22.424523, 25.237368, -15.609319, 33.068235),
				new GeoCountryInfo(GeoContinents.Europe, 724, 1, "ES", "ESP", "29", "Spain", "Kingdom of Spain", "Espana", "Reino de Espana", "Spain", "Spain", "Spain", 40.463667, -3.74922, 35.173, -12.524, 45.244, 5.098),
				new GeoCountryInfo(GeoContinents.Africa, 728, 2, "SS", "SSD", "", "South Sudan", "South Sudan", "", "", "SSD", "South Sudan", "South Sudan", 6.876991, 31.306978, 3.48898, 23.440849, 12.236388, 35.948997),
				new GeoCountryInfo(GeoContinents.Africa, 729, 2, "SD", "SDN", "", "Sudan", "Republic of the Sudan", "As-Sudan", "Jumhuriyat as-Sudan", "Sudan", "Sudan", "Sudan", 12.862807, 30.217636, 9.34722, 21.814939, 22.224918, 38.584219),
				new GeoCountryInfo(GeoContinents.Africa, 732, 2, "EH", "ESH", "", "Western Sahara", "Western Sahara", "", "", "ESH", "Western Sahara", "Western Sahara", 24.215527, -12.885834, 20.770018, -17.104908, 27.666702, -8.666666),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 740, 2, "SR", "SUR", "79", "Suriname", "Republic of Suriname", "Suriname", "Republiek Suriname", "Suriname", "Suriname", "Suriname", 3.919305, -56.027783, 1.837306, -58.070505, 6.009283, -53.951024),
				new GeoCountryInfo(GeoContinents.Europe, 744, 2, "SJ", "SJM", "", "Svalbard and Jan Mayen", "Svalbard and Jan Mayen", "", "", "SJM", "SJM", "Svalbard and Jan Mayen", 77.553604, 23.670272, 76.436279, 10.490722, 80.834053, 33.497093),
				new GeoCountryInfo(GeoContinents.Africa, 748, 2, "SZ", "SWZ", "SZ", "Swaziland", "Kingdom of Swaziland", "eSwatini", "Umbuso weSwatini", "Swaziland", "Swaziland", "Swaziland", -26.522503, 31.465866, -27.317363, 30.791094, -25.718519, 32.134844),
				new GeoCountryInfo(GeoContinents.Europe, 752, 2, "SE", "SWE", "77", "Sweden", "Kingdom of Sweden", "Sverige", "Konungariket Sverige", "Sweden", "Sweden", "Sweden", 60.128161, 18.643501, 55.336702, 10.963186, 69.060023, 24.166024),
				new GeoCountryInfo(GeoContinents.Europe, 756, 2, "CH", "CHE", "78", "Switzerland", "Swiss Confederation", "Schweiz (German); Suisse (French); Svizzera (Italian); Svizra (Romansh)", "Schweizerische Eidgenossenschaft (German); Confederation Suisse (French); Confederazione Svizzera (Italian); Confederaziun Svizra (Romansh)", "CHE", "Switzerland", "Switzerland", 46.818188, 8.227512, 45.81792, 5.95608, 47.808454, 10.49234),
				new GeoCountryInfo(GeoContinents.Asia, 760, 2, "SY", "SYR", "80", "Syrian Arab Republic", "Syrian Arab Republic", "", "", "SYR", "Syria", "Syria", 34.802075, 38.996815, 32.311136, 35.716595, 37.320569, 42.376309),
				new GeoCountryInfo(GeoContinents.Asia, 762, 2, "TJ", "TJK", "TJ", "Tajikistan", "Republic of Tajikistan", "Tojikiston", "Jumhurii Tojikiston", "Tajikistan", "Tajikistan", "Tajikistan", 38.861034, 71.276093, 36.671989, 67.342012, 41.044367, 75.153956),
				new GeoCountryInfo(GeoContinents.Asia, 764, 2, "TH", "THA", "86", "Thailand", "Kingdom of Thailand", "Prathet Thai", "Ratcha Anachak Thai", "Thailand", "Thailand", "Thailand", 15.870032, 100.992541, 5.612729, 97.343396, 20.465143, 105.636812),
				new GeoCountryInfo(GeoContinents.Africa, 768, 2, "TG", "TGO", "87", "Togo", "Togolese Republic", "", "Republique togolaise", "Togo", "Togo", "Togo", 8.619543, 0.824782, 6.112357, -0.144041, 11.139495, 1.80905),
				new GeoCountryInfo(GeoContinents.Oceania, 772, 2, "TK", "TKL", "", "Tokelau", "Tokelau", "", "", "Tokelau", "Tokelau", "Tokelau", -9.2002, -171.8484, -9.233011, -171.870632, -9.100656, -171.76579),
				new GeoCountryInfo(GeoContinents.Oceania, 776, 2, "TO", "TON", "TN", "Tonga", "Kingdom of Tonga", "Tonga", "Pule'anga Tonga", "Tonga", "Tonga", "Tonga", -21.178986, -175.198242, -21.47346, -175.356497, -21.010185, -174.900594),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 780, 2, "TT", "TTO", "TT", "Trinidad and Tobago", "Republic of Trinidad and Tobago", "", "", "TTO", "Trinidad/Tobago", "Trinidad and Tobago", 10.691803, -61.222503, 10.043169, -61.931068, 10.843473, -60.908954),
				new GeoCountryInfo(GeoContinents.Asia, 784, 2, "AE", "ARE", "", "United Arab Emirates", "United Arab Emirates", "", "Al Imarat al Arabiyah al Muttahidah", "UAE", "Unit Arab Emir", "United Arab Emirates", 23.424076, 53.847818, 22.631513, 51.49977, 26.069654, 56.381578),
				new GeoCountryInfo(GeoContinents.Africa, 788, 2, "TN", "TUN", "88", "Tunisia", "Tunisian Republic", "Tunis", "Al Jumhuriyah at Tunisiyah", "Tunisia", "Tunisia", "Tunisia", 33.886917, 9.537499, 30.228033, 7.522313, 37.347132, 11.599217),
				new GeoCountryInfo(GeoContinents.Europe, 792, 2, "TR", "TUR", "89", "Turkey", "Republic of Turkey", "Turkiye", "Turkiye Cumhuriyeti", "Turkey", "Turkey", "Turkey", 38.963745, 35.243322, 35.80768, 25.663637, 42.10609, 44.818128),
				new GeoCountryInfo(GeoContinents.Asia, 795, 2, "TM", "TKM", "TM", "Turkmenistan", "Turkmenistan", "Turkmenistan", "", "TKM", "Turkmenistan", "Turkmenistan", 38.969719, 59.556278, 35.12876, 52.447743, 42.798844, 66.707353),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 796, 2, "TC", "TCA", "", "Turks and Caicos Islands", "Turks and Caicos Islands", "", "", "TCI", "Turks/Caicos Is", "Turks and Caicos Islands", 21.694025, -71.797928, 21.454027, -72.482471, 21.96235, -71.461763),
				new GeoCountryInfo(GeoContinents.Oceania, 798, 2, "TV", "TUV", "TV", "Tuvalu", "Tuvalu", "Tuvalu", "", "Tuvalu", "Tuvalu", "Tuvalu", -10.728071, 179.472656, -10.800933, 179.390258, -10.655209, 179.555053),
				new GeoCountryInfo(GeoContinents.Africa, 800, 2, "UG", "UGA", "UG", "Uganda", "Republic of Uganda", "", "", "Uganda", "Uganda", "Uganda", 1.373333, 32.290275, -1.481541, 29.573433, 4.223, 35.033049),
				new GeoCountryInfo(GeoContinents.Europe, 804, 2, "UA", "UKR", "UR", "Ukraine", "Ukraine", "Ukrayina", "", "Ukraine", "Ukraine", "Ukraine", 48.379433, 31.16558, 44.386463, 22.137158, 52.379581, 40.22858),
				new GeoCountryInfo(GeoContinents.Europe, 807, 2, "MK", "MKD", "", "Macedonia", "The Former Yugoslav Republic of Macedonia", "Macedonia (FYROM)", "", "Macedonia", "Macedonia", "Macedonia", 41.608635, 21.745275, 40.853782, 20.452423, 42.373646, 23.034093),
				new GeoCountryInfo(GeoContinents.Africa, 818, 2, "EG", "EGY", "27", "Egypt", "Arab Republic of Egypt", "Misr", "Jumhuriyat Misr al-Arabiyah", "Egypt", "Egypt", "Egypt", 26.820553, 30.802498, 21.999999, 24.696774, 31.671535, 36.894544),
				new GeoCountryInfo(GeoContinents.Europe, 826, 1, "GB", "GBR", "74", "United Kingdom", "United Kingdom of Great Britain and Northern Ireland", "", "", "UK", "United Kingdom", "United Kingdom", 55.378051, -3.435973, 49.866968, -8.649357, 60.856553, 1.762709),
				new GeoCountryInfo(GeoContinents.Europe, 831, 2, "GG", "GGY", "", "Guernsey", "Bailiwick of Guernsey", "", "", "Guernsey", "Guernsey", "Guernsey", 49.465691, -2.585278, 49.416719, -2.67574, 49.50941, -2.497839),
				new GeoCountryInfo(GeoContinents.Europe, 832, 2, "JE", "JEY", "", "Jersey", "Bailiwick of Jersey", "", "", "Jersey", "Jersey", "Jersey", 49.214439, -2.13125, 49.160121, -2.254801, 49.262131, -2.009796),
				new GeoCountryInfo(GeoContinents.Europe, 833, 2, "IM", "IMN", "", "Isle of Man", "Isle of Man", "", "", "IMN", "Isle of Man", "Isle of Man", 54.236107, -4.548056, 54.04464, -4.83018, 54.418089, -4.308823),
				new GeoCountryInfo(GeoContinents.Africa, 834, 2, "TZ", "TZA", "ZA", "Tanzania", "United Republic of Tanzania", "", "", "Tanzania", "Tanzania", "Tanzania", -6.369028, 34.888822, -11.761253, 29.34, -0.984397, 40.444965),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 840, 1, "US", "USA", "33", "United States", "United States of America", "", "", "America", "USA", "United States of America", 37.09024, -95.712891, 25.82, -124.39, 49.38, -66.94),
				new GeoCountryInfo(GeoContinents.NorthAmerica, 850, 2, "VI", "VIR", "", "U.S. Virgin Islands", "United States Virgin Islands", "US Virgin Islands", "", "VIR", "VIR", "US Virgin Islands", 18.335765, -64.896335, 18.273932, -65.085456, 18.41295, -64.648869),
				new GeoCountryInfo(GeoContinents.Africa, 854, 2, "BF", "BFA", "BF", "Burkina Faso", "Burkina Faso", "Burkina Faso", "", "BFA", "Burkina Faso", "Burkina Faso", 12.238333, -1.561593, 9.393888, -5.521111, 15.085111, 2.404292),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 858, 2, "UY", "URY", "92", "Uruguay", "Oriental Republic of Uruguay", "Uruguay", "Republica Oriental del Uruguay", "Uruguay", "Uruguay", "Uruguay", -32.522779, -55.765835, -35.031418, -58.43915, -30.085214, -53.077928),
				new GeoCountryInfo(GeoContinents.Asia, 860, 2, "UZ", "UZB", "UZ", "Uzbekistan", "Republic of Uzbekistan", "Ozbekiston", "Ozbekiston Respublikasi", "Uzbekistan", "Uzbekistan", "Uzbekistan", 41.377491, 64.585262, 37.172257, 55.998217, 45.590075, 73.148946),
				new GeoCountryInfo(GeoContinents.SouthAmerica, 862, 2, "VE", "VEN", "93", "Venezuela", "Bolivarian Republic of Venezuela", "", "", "Venezuela", "Venezuela", "Venezuela", 6.42375, -66.58973, 0.647529, -73.351558, 12.196748, -59.805666),
				new GeoCountryInfo(GeoContinents.Oceania, 876, 2, "WF", "WLF", "", "Wallis and Futuna", "Territory of the Wallis and Futuna Islands", "Wallis et Futuna", "Territoire des Iles Wallis et Futuna", "WLF", "Wallis/Futuna", "Wallis and Futuna", -14.2938, -178.1165, -14.362124, -178.186608, -14.236552, -177.992307),
				new GeoCountryInfo(GeoContinents.Oceania, 882, 2, "WS", "WSM", "", "Samoa", "Independent State of Samoa", "Samoa", "Malo Sa'oloto Tuto'atasi o Samoa", "Samoa", "Samoa", "Samoa", -13.759029, -172.104629, -14.076588, -172.803195, -13.434402, -171.405859),
				new GeoCountryInfo(GeoContinents.Asia, 887, 2, "YE", "YEM", "YM", "Yemen", "Republic of Yemen", "Al Yaman", "Al Jumhuriyah al Yamaniyah", "Yemen", "Yemen", "Yemen", 15.552727, 48.516388, 12.108165, 41.816055, 18.999633, 54.533555),
				new GeoCountryInfo(GeoContinents.Africa, 894, 2, "ZM", "ZMB", "ZM", "Zambia", "Republic of Zambia", "", "", "Zambia", "Zambia", "Zambia", -13.133897, 27.849332, -18.077418, 21.996387, -8.203283, 33.702221)
			};

		}
	}
}
