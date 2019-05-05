function GeoGraffiti() {
	this._visited = [];
	this._planned = [];

	this._countries = [{ "continent": 1, "dlat": 4.556838, "dlng": 7.1864305, "id": 4, "key": "AF", "name": "Afghanistan", "lat": 33.939110, "lng": 67.709953 }, { "continent": 6, "dlat": 1.508176, "dlng": 0.8966675, "id": 8, "key": "AL", "name": "Albania", "lat": 41.153332, "lng": 20.168331 }, { "continent": 2, "dlat": 9.0608365, "dlng": 10.333333, "id": 12, "key": "DZ", "name": "Algeria", "lat": 27.225727, "lng": 2.492945 }, { "continent": 2, "dlat": 6.825580, "dlng": 6.207441, "id": 24, "key": "AO", "name": "Angola", "lat": -11.202692, "lng": 17.873887 }, { "continent": 5, "dlat": 15.000000, "dlng": 180.000000, "id": 10, "key": "AQ", "name": "Antarctica", "lat": -77.000000, "lng": 5.000000 }, { "continent": 4, "dlat": 16.6384505, "dlng": 9.9614395, "id": 32, "key": "AR", "name": "Argentina", "lat": -38.416097, "lng": -63.616672 }, { "continent": 6, "dlat": 1.2303745, "dlng": 1.5935055, "id": 51, "key": "AM", "name": "Armenia", "lat": 40.069099, "lng": 45.038189 }, { "continent": 7, "dlat": 17.215761, "dlng": 20.3573505, "id": 36, "key": "AU", "name": "Australia", "lat": -25.274398, "lng": 133.775136 }, { "continent": 6, "dlat": 1.3241365, "dlng": 3.8149515, "id": 40, "key": "AT", "name": "Austria", "lat": 47.516231, "lng": 14.550072 }, { "continent": 6, "dlat": 1.760175, "dlng": 2.801691, "id": 31, "key": "AZ", "name": "Azerbaijan", "lat": 40.143105, "lng": 47.576927 }, { "continent": 3, "dlat": 3.1756155, "dlng": 3.881439, "id": 44, "key": "BS", "name": "Bahamas", "lat": 25.034280, "lng": -77.396280 }, { "continent": 1, "dlat": 2.9399315, "dlng": 2.3357635, "id": 50, "key": "BD", "name": "Bangladesh", "lat": 23.684994, "lng": 90.356331 }, { "continent": 6, "dlat": 2.454930, "dlng": 4.7992415, "id": 112, "key": "BY", "name": "Belarus", "lat": 53.709807, "lng": 27.953389 }, { "continent": 6, "dlat": 1.0040655, "dlng": 1.931592, "id": 56, "key": "BE", "name": "Belgium", "lat": 50.503887, "lng": 4.469936 }, { "continent": 3, "dlat": 1.3051615, "dlng": 0.8679305, "id": 84, "key": "BZ", "name": "Belize", "lat": 17.189877, "lng": -88.497650 }, { "continent": 2, "dlat": 3.086490, "dlng": 1.5333375, "id": 204, "key": "BJ", "name": "Benin", "lat": 9.307690, "lng": 2.315834 }, { "continent": 1, "dlat": 0.8294045, "dlng": 1.6893795, "id": 64, "key": "BT", "name": "Bhutan", "lat": 27.514162, "lng": 90.433601 }, { "continent": 4, "dlat": 6.614383, "dlng": 6.0955935, "id": 68, "key": "BO", "name": "Bolivia", "lat": -16.290154, "lng": -63.588653 }, { "continent": 6, "dlat": 1.360110, "dlng": 1.9497845, "id": 70, "key": "BA", "name": "Bosnia and Herzegovina", "lat": 43.915886, "lng": 17.679076 }, { "continent": 2, "dlat": 4.564704, "dlng": 4.688199, "id": 72, "key": "BW", "name": "Botswana", "lat": -22.328474, "lng": 24.684866 }, { "continent": 4, "dlat": 19.5112955, "dlng": 19.594955, "id": 76, "key": "BR", "name": "Brazil", "lat": -14.235004, "lng": -51.925280 }, { "continent": 1, "dlat": 0.522329, "dlng": 0.6437495, "id": 96, "key": "BN", "name": "Brunei Darussalam", "lat": 4.535277, "lng": 114.727669 }, { "continent": 6, "dlat": 1.489839, "dlng": 3.1259595, "id": 100, "key": "BG", "name": "Bulgaria", "lat": 42.733883, "lng": 25.485830 }, { "continent": 2, "dlat": 2.8456115, "dlng": 3.9627015, "id": 854, "key": "BF", "name": "Burkina Faso", "lat": 12.238333, "lng": -1.561593 }, { "continent": 2, "dlat": 1.0796205, "dlng": 0.9245895, "id": 108, "key": "BI", "name": "Burundi", "lat": -3.373056, "lng": 29.918886 }, { "continent": 1, "dlat": 2.7066855, "dlng": 2.6470725, "id": 116, "key": "KH", "name": "Cambodia", "lat": 12.565679, "lng": 104.990963 }, { "continent": 2, "dlat": 5.713750, "dlng": 3.849822, "id": 120, "key": "CM", "name": "Cameroon", "lat": 7.369722, "lng": 12.354722 }, { "continent": 3, "dlat": 14.000000, "dlng": 46.000000, "id": 124, "key": "CA", "name": "Canada", "lat": 56.130366, "lng": -106.346771 }, { "continent": 2, "dlat": 4.3985495, "dlng": 6.5216035, "id": 140, "key": "CF", "name": "Central African Republic", "lat": 6.611111, "lng": 20.939444 }, { "continent": 2, "dlat": 8.003130, "dlng": 5.265001, "id": 148, "key": "TD", "name": "Chad", "lat": 15.454166, "lng": 18.732207 }, { "continent": 4, "dlat": 19.2407255, "dlng": 4.6392925, "id": 152, "key": "CL", "name": "Chile", "lat": -35.675147, "lng": -71.542969 }, { "continent": 1, "dlat": 17.7037265, "dlng": 30.636698, "id": 156, "key": "CN", "name": "China", "lat": 35.861660, "lng": 104.195397 }, { "continent": 4, "dlat": 8.3427835, "dlng": 6.0782135, "id": 170, "key": "CO", "name": "Colombia", "lat": 4.570868, "lng": -74.297333 }, { "continent": 2, "dlat": 4.3683695, "dlng": 3.747032, "id": 178, "key": "CG", "name": "Congo", "lat": -0.228021, "lng": 15.827659 }, { "continent": 2, "dlat": 4.3683695, "dlng": 3.747032, "id": 180, "key": "CD", "name": "Congo", "lat": -0.228021, "lng": 15.827659 }, { "continent": 3, "dlat": 1.5894915, "dlng": 1.701527, "id": 188, "key": "CR", "name": "Costa Rica", "lat": 9.748917, "lng": -83.753428 }, { "continent": 2, "dlat": 3.194504, "dlng": 3.0545135, "id": 384, "key": "CI", "name": "Côte d\u0027Ivoire", "lat": 7.539989, "lng": -5.547080 }, { "continent": 6, "dlat": 2.0814385, "dlng": 2.9791805, "id": 191, "key": "HR", "name": "Croatia", "lat": 45.100000, "lng": 15.200000 }, { "continent": 3, "dlat": 1.7254265, "dlng": 5.4695165, "id": 192, "key": "CU", "name": "Cuba", "lat": 21.521757, "lng": -77.781167 }, { "continent": 6, "dlat": 0.537448, "dlng": 1.1678965, "id": 196, "key": "CY", "name": "Cyprus", "lat": 35.126413, "lng": 33.429859 }, { "continent": 6, "dlat": 1.251955, "dlng": 3.3843235, "id": 203, "key": "CZ", "name": "Czech Republic", "lat": 49.817492, "lng": 15.472962 }, { "continent": 1, "dlat": 0.0086825, "dlng": 0.0160075, "id": 418, "key": "LA", "name": "Democratic Republic", "lat": 38.905448, "lng": -77.039316 }, { "continent": 6, "dlat": 1.596346, "dlng": 2.358755, "id": 208, "key": "DK", "name": "Denmark", "lat": 56.263920, "lng": 9.501785 }, { "continent": 2, "dlat": 0.8907255, "dlng": 0.8286255, "id": 262, "key": "DJ", "name": "Djibouti", "lat": 11.825138, "lng": 42.590275 }, { "continent": 3, "dlat": 1.230814, "dlng": 1.8420515, "id": 214, "key": "DO", "name": "Dominican Republic", "lat": 18.735693, "lng": -70.162651 }, { "continent": 4, "dlat": 3.2213845, "dlng": 2.948093, "id": 218, "key": "EC", "name": "Ecuador", "lat": -1.831239, "lng": -78.183406 }, { "continent": 2, "dlat": 4.835768, "dlng": 6.098885, "id": 818, "key": "EG", "name": "Egypt", "lat": 26.820553, "lng": 30.802498 }, { "continent": 3, "dlat": 0.6475625, "dlng": 1.2215295, "id": 222, "key": "SV", "name": "El Salvador", "lat": 13.794185, "lng": -88.896530 }, { "continent": 2, "dlat": 0.7311095, "dlng": 1.0157855, "id": 226, "key": "GQ", "name": "Equatorial Guinea", "lat": 1.650801, "lng": 10.267895 }, { "continent": 2, "dlat": 2.833243, "dlng": 3.354815, "id": 232, "key": "ER", "name": "Eritrea", "lat": 15.179384, "lng": 39.782334 }, { "continent": 6, "dlat": 1.0954835, "dlng": 3.222883, "id": 233, "key": "EE", "name": "Estonia", "lat": 58.595272, "lng": 25.013607 }, { "continent": 2, "dlat": 5.7450395, "dlng": 7.5011325, "id": 231, "key": "ET", "name": "Ethiopia", "lat": 9.145000, "lng": 40.489673 }, { "continent": 7, "dlat": 1.751214, "dlng": 0.000000, "id": 242, "key": "FJ", "name": "Fiji", "lat": -17.713371, "lng": 178.065032 }, { "continent": 6, "dlat": 5.177537, "dlng": 5.5198445, "id": 246, "key": "FI", "name": "Finland", "lat": 61.924110, "lng": 25.748151 }, { "continent": 6, "dlat": 4.873317, "dlng": 7.3500975, "id": 250, "key": "FR", "name": "France", "lat": 46.227638, "lng": 2.213749 }, { "continent": 4, "dlat": 1.823951, "dlng": 1.4604205, "id": 254, "key": "GF", "name": "French Guiana", "lat": 3.933889, "lng": -53.125782 }, { "continent": 5, "dlat": 0.642088, "dlng": 0.973792, "id": 260, "key": "TF", "name": "French Southern and Antarctic Lands", "lat": -49.280366, "lng": 69.348557 }, { "continent": 2, "dlat": 3.1382405, "dlng": 2.910752, "id": 266, "key": "GA", "name": "Gabon", "lat": -0.803689, "lng": 11.609444 }, { "continent": 2, "dlat": 0.3806035, "dlng": 1.5075105, "id": 270, "key": "GM", "name": "Gambia", "lat": 13.443182, "lng": -15.310139 }, { "continent": 6, "dlat": 2.322534, "dlng": 2.382189, "id": 268, "key": "GE", "name": "Georgia", "lat": 32.157435, "lng": -82.907123 }, { "continent": 6, "dlat": 3.894118, "dlng": 4.587777, "id": 276, "key": "DE", "name": "Germany", "lat": 51.165691, "lng": 10.451526 }, { "continent": 2, "dlat": 3.213897, "dlng": 2.230074, "id": 288, "key": "GH", "name": "Ghana", "lat": 7.946527, "lng": -1.023194 }, { "continent": 6, "dlat": 3.474018, "dlng": 4.436684, "id": 300, "key": "GR", "name": "Greece", "lat": 39.074208, "lng": 21.824312 }, { "continent": 3, "dlat": 11.916090, "dlng": 30.861372, "id": 304, "key": "GL", "name": "Greenland", "lat": 71.706936, "lng": -42.604303 }, { "continent": 3, "dlat": 2.037845, "dlng": 2.003110, "id": 320, "key": "GT", "name": "Guatemala", "lat": 15.783471, "lng": -90.230759 }, { "continent": 2, "dlat": 2.7418535, "dlng": 3.7201765, "id": 324, "key": "GN", "name": "Guinea", "lat": 9.945587, "lng": -9.696645 }, { "continent": 2, "dlat": 0.912376, "dlng": 1.5421155, "id": 624, "key": "GW", "name": "Guinea-Bissau", "lat": 11.803749, "lng": -15.180413 }, { "continent": 4, "dlat": 3.6917655, "dlng": 2.4618925, "id": 328, "key": "GY", "name": "Guyana", "lat": 4.860416, "lng": -58.930180 }, { "continent": 3, "dlat": 1.033768, "dlng": 1.429578, "id": 332, "key": "HT", "name": "Haiti", "lat": 18.971187, "lng": -72.285215 }, { "continent": 3, "dlat": 1.7661575, "dlng": 3.109536, "id": 340, "key": "HN", "name": "Honduras", "lat": 15.199999, "lng": -86.241905 }, { "continent": 1, "dlat": 0.204290, "dlng": 0.285683, "id": 344, "key": "HK", "name": "Hong Kong", "lat": 22.396428, "lng": 114.109497 }, { "continent": 6, "dlat": 1.424073, "dlng": 3.3919965, "id": 348, "key": "HU", "name": "Hungary", "lat": 47.162494, "lng": 19.503304 }, { "continent": 6, "dlat": 1.635108, "dlng": 5.525354, "id": 352, "key": "IS", "name": "Iceland", "lat": 64.963051, "lng": -19.020835 }, { "continent": 1, "dlat": 14.378601, "dlng": 14.616380, "id": 356, "key": "IN", "name": "India", "lat": 20.593684, "lng": 78.962880 }, { "continent": 1, "dlat": 8.455747, "dlng": 23.003799, "id": 360, "key": "ID", "name": "Indonesia", "lat": -0.789275, "lng": 113.921327 }, { "continent": 1, "dlat": 7.3611235, "dlng": 9.650723, "id": 364, "key": "IR", "name": "Iran", "lat": 32.427908, "lng": 53.688046 }, { "continent": 1, "dlat": 4.1598625, "dlng": 4.891157, "id": 368, "key": "IQ", "name": "Iraq", "lat": 33.223191, "lng": 43.679291 }, { "continent": 6, "dlat": 1.9822575, "dlng": 2.337370, "id": 372, "key": "IE", "name": "Ireland", "lat": 53.412910, "lng": -8.243890 }, { "continent": 1, "dlat": 1.9210795, "dlng": 0.8144285, "id": 376, "key": "IL", "name": "Israel", "lat": 31.046051, "lng": 34.851612 }, { "continent": 6, "dlat": 5.799540, "dlng": 5.9468905, "id": 380, "key": "IT", "name": "Italy", "lat": 41.871940, "lng": 12.567380 }, { "continent": 3, "dlat": 0.409793, "dlng": 1.0928435, "id": 388, "key": "JM", "name": "Jamaica", "lat": 18.109581, "lng": -77.297508 }, { "continent": 1, "dlat": 10.7370395, "dlng": 11.441860, "id": 392, "key": "JP", "name": "Japan", "lat": 36.204824, "lng": 138.252924 }, { "continent": 1, "dlat": 2.0948255, "dlng": 2.171409, "id": 400, "key": "JO", "name": "Jordan", "lat": 30.585164, "lng": 36.238414 }, { "continent": 6, "dlat": 7.4366995, "dlng": 20.410872, "id": 398, "key": "KZ", "name": "Kazakhstan", "lat": 48.019573, "lng": 66.923684 }, { "continent": 2, "dlat": 4.8565505, "dlng": 3.998505, "id": 404, "key": "KE", "name": "Kenya", "lat": -0.023559, "lng": 37.906193 }, { "continent": 1, "dlat": 0.789630, "dlng": 0.938709, "id": 414, "key": "KW", "name": "Kuwait", "lat": 29.311660, "lng": 47.481766 }, { "continent": 1, "dlat": 2.042551, "dlng": 5.4877805, "id": 417, "key": "KG", "name": "Kyrgyzstan", "lat": 41.204380, "lng": 74.766098 }, { "continent": 6, "dlat": 1.205396, "dlng": 3.639528, "id": 428, "key": "LV", "name": "Latvia", "lat": 56.879635, "lng": 24.603189 }, { "continent": 1, "dlat": 0.8185325, "dlng": 0.759971, "id": 422, "key": "LB", "name": "Lebanon", "lat": 33.854721, "lng": 35.862285 }, { "continent": 2, "dlat": 1.0523885, "dlng": 1.2222385, "id": 426, "key": "LS", "name": "Lesotho", "lat": -29.609988, "lng": 28.233608 }, { "continent": 2, "dlat": 2.1182865, "dlng": 2.052497, "id": 430, "key": "LR", "name": "Liberia", "lat": 6.428055, "lng": -9.429499 }, { "continent": 2, "dlat": 6.833179, "dlng": 7.877744, "id": 434, "key": "LY", "name": "Libya", "lat": 27.056776, "lng": 14.528040 }, { "continent": 6, "dlat": 1.276721, "dlng": 2.9406115, "id": 440, "key": "LT", "name": "Lithuania", "lat": 55.169438, "lng": 23.881275 }, { "continent": 6, "dlat": 0.3675205, "dlng": 0.3976505, "id": 442, "key": "LU", "name": "Luxembourg", "lat": 49.815273, "lng": 6.129583 }, { "continent": 6, "dlat": 0.759932, "dlng": 1.290835, "id": 807, "key": "MK", "name": "Macedonia", "lat": 41.608635, "lng": 21.745275 }, { "continent": 2, "dlat": 6.827304, "dlng": 3.649320, "id": 450, "key": "MG", "name": "Madagascar", "lat": -18.766947, "lng": 46.869107 }, { "continent": 2, "dlat": 3.8840625, "dlng": 1.6226385, "id": 454, "key": "MW", "name": "Malawi", "lat": -13.254308, "lng": 34.301525 }, { "continent": 1, "dlat": 3.132163, "dlng": 2.559862, "id": 458, "key": "MY", "name": "Malaysia", "lat": 4.210484, "lng": 101.975766 }, { "continent": 2, "dlat": 7.4261235, "dlng": 8.252775, "id": 466, "key": "ML", "name": "Mali", "lat": 17.570692, "lng": -3.996166 }, { "continent": 4, "dlat": 0.5810185, "dlng": 1.8157285, "id": 238, "key": "FK", "name": "Malvinas", "lat": -51.796253, "lng": -59.523613 }, { "continent": 2, "dlat": 6.2865855, "dlng": 6.1183995, "id": 478, "key": "MR", "name": "Mauritania", "lat": 21.007890, "lng": -10.940835 }, { "continent": 3, "dlat": 9.092107, "dlng": 15.826703, "id": 484, "key": "MX", "name": "Mexico", "lat": 23.634501, "lng": -102.552784 }, { "continent": 6, "dlat": 1.512520, "dlng": 1.7728415, "id": 498, "key": "MD", "name": "Moldova", "lat": 47.411631, "lng": 28.369885 }, { "continent": 1, "dlat": 5.283588, "dlng": 16.097164, "id": 496, "key": "MN", "name": "Mongolia", "lat": 46.862496, "lng": 103.846656 }, { "continent": 6, "dlat": 0.8545065, "dlng": 0.961986, "id": 499, "key": "ME", "name": "Montenegro", "lat": 42.708678, "lng": 19.374390 }, { "continent": 2, "dlat": 4.1279205, "dlng": 6.087958, "id": 504, "key": "MA", "name": "Morocco", "lat": 31.791702, "lng": -7.092620 }, { "continent": 2, "dlat": 8.198453, "dlng": 5.311786, "id": 508, "key": "MZ", "name": "Mozambique", "lat": -18.665695, "lng": 35.529562 }, { "continent": 1, "dlat": 9.4744015, "dlng": 4.4992315, "id": 104, "key": "MM", "name": "Myanmar (Burma)", "lat": 21.913965, "lng": 95.956223 }, { "continent": 2, "dlat": 6.0035765, "dlng": 6.7687525, "id": 516, "key": "NA", "name": "Namibia", "lat": -22.957640, "lng": 18.490410 }, { "continent": 1, "dlat": 2.049583, "dlng": 4.0735375, "id": 524, "key": "NP", "name": "Nepal", "lat": 28.394857, "lng": 84.124008 }, { "continent": 6, "dlat": 1.4626085, "dlng": 1.947770, "id": 528, "key": "NL", "name": "Netherlands", "lat": 52.132633, "lng": 5.291266 }, { "continent": 7, "dlat": 1.6712195, "dlng": 2.281980, "id": 540, "key": "NC", "name": "New Caledonia", "lat": -20.904305, "lng": 165.618042 }, { "continent": 7, "dlat": 6.8192835, "dlng": 6.3181995, "id": 554, "key": "NZ", "name": "New Zealand", "lat": -40.900557, "lng": 174.885971 }, { "continent": 3, "dlat": 2.1611105, "dlng": 2.5494985, "id": 558, "key": "NI", "name": "Nicaragua", "lat": 12.865416, "lng": -85.207229 }, { "continent": 2, "dlat": 5.903122, "dlng": 7.916183, "id": 562, "key": "NE", "name": "Niger", "lat": 17.607789, "lng": 8.081666 }, { "continent": 2, "dlat": 4.8078935, "dlng": 6.0005245, "id": 566, "key": "NG", "name": "Nigeria", "lat": 9.081999, "lng": 8.675277 }, { "continent": 1, "dlat": 2.669129, "dlng": 3.2506565, "id": 408, "key": "KP", "name": "North Korea", "lat": 40.339852, "lng": 127.510093 }, { "continent": 6, "dlat": 6.6072695, "dlng": 13.2615525, "id": 578, "key": "NO", "name": "Norway", "lat": 60.472024, "lng": 8.468946 }, { "continent": 1, "dlat": 4.877529, "dlng": 3.919698, "id": 512, "key": "OM", "name": "Oman", "lat": 21.512583, "lng": 55.923255 }, { "continent": 1, "dlat": 6.6947065, "dlng": 8.481347, "id": 586, "key": "PK", "name": "Pakistan", "lat": 30.375321, "lng": 69.345116 }, { "continent": 1, "dlat": 0.6047485, "dlng": 0.346889, "id": 275, "key": "PS", "name": "Palestine", "lat": 31.952162, "lng": 35.233154 }, { "continent": 3, "dlat": 1.2221115, "dlng": 2.9468765, "id": 591, "key": "PA", "name": "Panama", "lat": 8.537981, "lng": -80.782127 }, { "continent": 7, "dlat": 5.3932705, "dlng": 8.121877, "id": 598, "key": "PG", "name": "Papua New Guinea", "lat": -6.314993, "lng": 143.955550 }, { "continent": 4, "dlat": 4.150314, "dlng": 4.1897445, "id": 600, "key": "PY", "name": "Paraguay", "lat": -23.442503, "lng": -58.443832 }, { "continent": 4, "dlat": 9.1564015, "dlng": 6.3380875, "id": 604, "key": "PE", "name": "Peru", "lat": -9.189967, "lng": -75.015152 }, { "continent": 1, "dlat": 7.480290, "dlng": 4.9506105, "id": 608, "key": "PH", "name": "Philippines", "lat": 12.879721, "lng": 121.774017 }, { "continent": 6, "dlat": 2.9168935, "dlng": 5.0115145, "id": 616, "key": "PL", "name": "Poland", "lat": 51.919438, "lng": 19.145136 }, { "continent": 6, "dlat": 2.5970135, "dlng": 1.6634505, "id": 620, "key": "PT", "name": "Portugal", "lat": 39.399872, "lng": -8.224454 }, { "continent": 3, "dlat": 0.3172905, "dlng": 1.131565, "id": 630, "key": "PR", "name": "Puerto Rico", "lat": 18.220833, "lng": -66.590149 }, { "continent": 1, "dlat": 0.855987, "dlng": 0.4466025, "id": 634, "key": "QA", "name": "Qatar", "lat": 25.354826, "lng": 51.183884 }, { "continent": 6, "dlat": 2.3233275, "dlng": 4.747671, "id": 642, "key": "RO", "name": "Romania", "lat": 45.943161, "lng": 24.966760 }, { "continent": 1, "dlat": 15.000000, "dlng": 76.000000, "id": 643, "key": "RU", "name": "Russia", "lat": 61.524010, "lng": 105.318756 }, { "continent": 2, "dlat": 0.896134, "dlng": 1.018823, "id": 646, "key": "RW", "name": "Rwanda", "lat": -1.940278, "lng": 29.873888 }, { "continent": 1, "dlat": 7.887378, "dlng": 10.558851, "id": 682, "key": "SA", "name": "Saudi Arabia", "lat": 23.885942, "lng": 45.079162 }, { "continent": 2, "dlat": 2.1928825, "dlng": 3.0906205, "id": 686, "key": "SN", "name": "Senegal", "lat": 14.497401, "lng": -14.452362 }, { "continent": 6, "dlat": 1.979265, "dlng": 2.0838935, "id": 688, "key": "RS", "name": "Serbia", "lat": 44.016521, "lng": 21.005859 }, { "continent": 2, "dlat": 1.5504735, "dlng": 1.5151775, "id": 694, "key": "SL", "name": "Sierra Leone", "lat": 8.460555, "lng": -11.779889 }, { "continent": 1, "dlat": 0.152241, "dlng": 0.2400525, "id": 702, "key": "SG", "name": "Singapore", "lat": 1.352083, "lng": 103.819836 }, { "continent": 6, "dlat": 0.941323, "dlng": 2.8630495, "id": 703, "key": "SK", "name": "Slovakia", "lat": 48.669026, "lng": 19.699024 }, { "continent": 6, "dlat": 0.727493, "dlng": 1.610675, "id": 705, "key": "SI", "name": "Slovenia", "lat": 46.151241, "lng": 14.995463 }, { "continent": 7, "dlat": 2.637109, "dlng": 3.633322, "id": 90, "key": "SB", "name": "Solomon Islands", "lat": -9.645710, "lng": 160.156194 }, { "continent": 2, "dlat": 6.8253275, "dlng": 5.2093275, "id": 706, "key": "SO", "name": "Somalia", "lat": 5.152149, "lng": 46.199616 }, { "continent": 2, "dlat": 6.353876, "dlng": 8.2150795, "id": 710, "key": "ZA", "name": "South Africa", "lat": -30.559482, "lng": 22.937506 }, { "continent": 1, "dlat": 2.755411, "dlng": 2.488266, "id": 410, "key": "KR", "name": "South Korea", "lat": 35.907757, "lng": 127.766922 }, { "continent": 2, "dlat": 4.373704, "dlng": 6.254074, "id": 728, "key": "SS", "name": "South Sudan", "lat": 6.876991, "lng": 31.306978 }, { "continent": 6, "dlat": 5.035500, "dlng": 8.811000, "id": 724, "key": "ES", "name": "Spain", "lat": 40.463667, "lng": -3.749220 }, { "continent": 1, "dlat": 1.9583865, "dlng": 1.124898, "id": 144, "key": "LK", "name": "Sri Lanka", "lat": 7.873054, "lng": 80.771797 }, { "continent": 2, "dlat": 6.438849, "dlng": 8.384640, "id": 729, "key": "SD", "name": "Sudan", "lat": 12.862807, "lng": 30.217636 }, { "continent": 4, "dlat": 2.0859885, "dlng": 2.0597405, "id": 740, "key": "SR", "name": "Suriname", "lat": 3.919305, "lng": -56.027783 }, { "continent": 2, "dlat": 0.799422, "dlng": 0.671875, "id": 748, "key": "SZ", "name": "Swaziland", "lat": -26.522503, "lng": 31.465866 }, { "continent": 6, "dlat": 6.8616605, "dlng": 6.601419, "id": 752, "key": "SE", "name": "Sweden", "lat": 60.128161, "lng": 18.643501 }, { "continent": 6, "dlat": 0.995267, "dlng": 2.268130, "id": 756, "key": "CH", "name": "Switzerland", "lat": 46.818188, "lng": 8.227512 }, { "continent": 1, "dlat": 2.5047165, "dlng": 3.329857, "id": 760, "key": "SY", "name": "Syrian Arab Republic", "lat": 34.802075, "lng": 38.996815 }, { "continent": 1, "dlat": 1.7018725, "dlng": 0.989552, "id": 158, "key": "TW", "name": "Taiwan", "lat": 23.697810, "lng": 120.960515 }, { "continent": 1, "dlat": 2.186189, "dlng": 3.905972, "id": 762, "key": "TJ", "name": "Tajikistan", "lat": 38.861034, "lng": 71.276093 }, { "continent": 2, "dlat": 5.388428, "dlng": 5.5524825, "id": 834, "key": "TZ", "name": "Tanzania", "lat": -6.369028, "lng": 34.888822 }, { "continent": 1, "dlat": 7.426207, "dlng": 4.146708, "id": 764, "key": "TH", "name": "Thailand", "lat": 15.870032, "lng": 100.992541 }, { "continent": 1, "dlat": 0.667925, "dlng": 1.2049355, "id": 626, "key": "TL", "name": "Timor-Leste", "lat": -8.711485, "lng": 125.634764 }, { "continent": 2, "dlat": 2.513569, "dlng": 0.9765455, "id": 768, "key": "TG", "name": "Togo", "lat": 8.619543, "lng": 0.824782 }, { "continent": 3, "dlat": 0.400152, "dlng": 0.511057, "id": 780, "key": "TT", "name": "Trinidad and Tobago", "lat": 10.691803, "lng": -61.222503 }, { "continent": 2, "dlat": 3.5595495, "dlng": 2.038452, "id": 788, "key": "TN", "name": "Tunisia", "lat": 33.886917, "lng": 9.537499 }, { "continent": 6, "dlat": 3.149205, "dlng": 9.5772455, "id": 792, "key": "TR", "name": "Turkey", "lat": 38.963745, "lng": 35.243322 }, { "continent": 1, "dlat": 3.835042, "dlng": 7.129805, "id": 795, "key": "TM", "name": "Turkmenistan", "lat": 38.969719, "lng": 59.556278 }, { "continent": 2, "dlat": 2.8522705, "dlng": 2.729808, "id": 800, "key": "UG", "name": "Uganda", "lat": 1.373333, "lng": 32.290275 }, { "continent": 6, "dlat": 3.996559, "dlng": 9.045711, "id": 804, "key": "UA", "name": "Ukraine", "lat": 48.379433, "lng": 31.165580 }, { "continent": 1, "dlat": 1.7190705, "dlng": 2.440904, "id": 784, "key": "AE", "name": "United Arab Emirates", "lat": 23.424076, "lng": 53.847818 }, { "continent": 6, "dlat": 5.4947925, "dlng": 5.206033, "id": 826, "key": "GB", "name": "United Kingdom", "lat": 55.378051, "lng": -3.435973 }, { "continent": 3, "dlat": 11.780000, "dlng": 28.725000, "id": 840, "key": "US", "name": "United States", "lat": 37.090240, "lng": -95.712891 }, { "continent": 4, "dlat": 2.473102, "dlng": 2.680611, "id": 858, "key": "UY", "name": "Uruguay", "lat": -32.522779, "lng": -55.765835 }, { "continent": 1, "dlat": 4.208909, "dlng": 8.5753645, "id": 860, "key": "UZ", "name": "Uzbekistan", "lat": 41.377491, "lng": 64.585262 }, { "continent": 7, "dlat": 2.376824, "dlng": 1.0536525, "id": 548, "key": "VU", "name": "Vanuatu", "lat": -15.376706, "lng": 166.959158 }, { "continent": 4, "dlat": 5.7746095, "dlng": 6.772946, "id": 862, "key": "VE", "name": "Venezuela", "lat": 6.423750, "lng": -66.589730 }, { "continent": 1, "dlat": 7.490333, "dlng": 3.6622825, "id": 704, "key": "VN", "name": "Vietnam", "lat": 14.058324, "lng": 108.277199 }, { "continent": 2, "dlat": 3.448342, "dlng": 4.219121, "id": 732, "key": "EH", "name": "Western Sahara", "lat": 24.215527, "lng": -12.885834 }, { "continent": 1, "dlat": 3.445734, "dlng": 6.358750, "id": 887, "key": "YE", "name": "Yemen", "lat": 15.552727, "lng": 48.516388 }, { "continent": 2, "dlat": 4.9370675, "dlng": 5.852917, "id": 894, "key": "ZM", "name": "Zambia", "lat": -13.133897, "lng": 27.849332 }, { "continent": 2, "dlat": 3.407602, "dlng": 3.9154335, "id": 716, "key": "ZW", "name": "Zimbabwe", "lat": -19.015438, "lng": 29.154857 }];

	return this;
}

GeoGraffiti.prototype.setCountries = function (countries) {
    if (countries) GeoGraffiti._countries = countries;
    return GeoGraffiti.prototype._countries;
}

GeoGraffiti.prototype.getCountries = function (ids) {
	var countries = new Array();
	for (var i = 0; i < ids.length; i++) {
		countries.push(this.getCountry(ids[i]));
	}
	return countries;
}

GeoGraffiti.prototype.getCountry = function (id) {
	if (Number(id)) {
		for (i = 0; i < _geo._countries.length; i++) {
			if (_geo._countries[i].id === id) return _geo._countries[i];
		}
	}
	else if (id) {
		id = id.toLowerCase();
		for (i = 0; i < _geo._countries.length; i++) {
			if (_geo._countries[i].key.toLowerCase() === id) return _geo._countries[i];
		}
	}
	return this.getNullPlace();
}

GeoGraffiti.prototype.getCountryUrlPathKey = function (id) {
	var c = GeoGraffiti.prototype.getCountry(id);
	return c.name.toLowerCase().replace(" ", "-");
}

GeoGraffiti.prototype.getCountryBounds = function (id) {
	var c = GeoGraffiti.prototype.getCountry(id);
	return [[c.lng - c.dlng, c.lat - c.dlat], [c.lng + c.dlng, c.lat + c.dlat]]
}

GeoGraffiti.prototype.getContinentName = function (id) {
	var asia = [4, 48, 50, 64, 86, 96, 104, 116, 144, 156, 158, 162, 166, 275, 344, 356, 360, 364, 368, 376, 392, 400, 408, 410, 414, 417, 418, 422, 446, 458, 462, 496, 512, 524, 586, 608, 626, 634, 643, 682, 702, 704, 760, 762, 764, 784, 795, 860, 887];
	var north = [28, 44, 52, 60, 84, 92, 124, 136, 188, 192, 212, 214, 222, 304, 308, 312, 320, 332, 340, 388, 474, 484, 500, 531, 533, 534, 535, 558, 591, 630, 652, 659, 660, 662, 663, 666, 670, 780, 796, 840, 850];
	var south = [32, 68, 76, 152, 170, 218, 238, 254, 328, 600, 604, 740, 858, 862];
	var europe = [8, 20, 31, 40, 51, 56, 70, 100, 112, 191, 196, 203, 208, 233, 234, 246, 248, 250, 268, 276, 292, 300, 336, 348, 352, 372, 380, 398, 428, 438, 440, 442, 470, 492, 498, 499, 528, 578, 616, 620, 642, 674, 688, 703, 705, 724, 744, 752, 756, 792, 804, 807, 826, 831, 832, 833];
	//var africa = [12, 24, 72, 108, 120, 132, 140, 148, 174, 175, 178, 180, 204, 226, 231, 232, 262, 266, 270, 288, 324, 384, 404, 426, 430, 434, 450, 454, 466, 478, 480, 504, 508, 516, 562, 566, 624, 638, 646, 654, 678, 686, 690, 694, 706, 710, 716, 728, 729, 732, 748, 768, 788, 800, 818, 834, 854, 894];

	if (id == 10)
		return "antarctica";
	else if (id == 36 || id == 554)
		return "oceania";
	else if (south.contains(id))
		return "south";
	else if (north.contains(id))
		return "north";
	else if (asia.contains(id))
		return "asia";
	else if (europe.contains(id))
		return "europe";
	else
		return "africa";
}

GeoGraffiti.prototype.getContinentBounds = function (continent) {
	switch (continent.toLowerCase()) {
		case "asia":
			return [[31, -2], [146, 65]];
		case "north":
			return [[-150, 66], [-54, 16]];
		case "south":
			return [[-80, -54], [-34, 9]];
		case "europe":
			return [[-11, 34], [20, 57]];
		case "africa":
			return [[-16, -35], [50, 35]];
		case "oceania":
			return [[113, -47], [179, -10]];
		case "antarctica":
			//return [[-155, -80], [155, -70]];  //82.5070406727076, -84.96864137533176
			return [[60, -60], [110, -110]];
		default:
			return [[-180, -90], [180, 90]];
	}
}

GeoGraffiti.prototype.getContinentCenter = function (continent) {
	var b = _geo.getContinentBounds(continent);
	var bx = (b[0][0] + b[1][0]) / 2;
	var by = (b[0][1] + b[1][1]) / 2;
	return [bx, by];
}

GeoGraffiti.prototype.setVisitedCountries = function (ids) {
	_geo._visited = ids;
}

GeoGraffiti.prototype.getVisitedCountries = function () {
	return _geo.getCountries(_geo._visited);
}

GeoGraffiti.prototype.isVisitedCountry = function (id) {
	return _geo._visited.contains(id);
}

GeoGraffiti.prototype.setPlannedCountries = function (ids) {
	_geo._planned = ids;
}

GeoGraffiti.prototype.getPlannedCountries = function () {
	return _geo.getCountries(_geo._planned);
}

GeoGraffiti.prototype.isPlannedCountry = function (id) {
	return _geo._planned.contains(id);
}



/*
GeoGraffiti.prototype.setCountryPlaces = function (id, places) {
    var c = GeoGraffiti.prototype.getCountry(id);
    if (c != null) c.places = places;
}

GeoGraffiti.prototype.getCountryPlaces = function (id) {
    var c = GeoGraffiti.prototype.getCountry(id);
    return c == null ? null : c.places;
}
*/

GeoGraffiti.prototype.setRegions = function (regions) {
    if (regions != null) GeoGraffiti._regions = regions;
    return GeoGraffiti.prototype._regions;
}

GeoGraffiti.prototype.getRegion = function (id) {
    if (Number(id)) {
        for (i = 0; i < GeoGraffiti._regions.length; i++) {
            if (GeoGraffiti._regions[i].id == id) return GeoGraffiti._regions[i];
        }
    }
    else {
        id = id.toLowerCase();
        for (i = 0; i < GeoGraffiti._regions.length; i++) {
            if (GeoGraffiti._regions[i].key.toLowerCase() == id) return GeoGraffiti._regions[i];
        }
    }
    return this.getNullPlace();
}

GeoGraffiti.prototype.getRegionBounds = function (id) {
	var r = GeoGraffiti.prototype.getRegion(id);
	return [[r.lng - r.dlng, r.lat - r.dlat], [r.lng + r.dlng, r.lat + r.dlat]]
}






GeoGraffiti.prototype.setPlaces = function (places) {
    if (places) _geo._places = places;
    return _geo._places;
};

GeoGraffiti.prototype.getPlaces = function () {
    return _geo._places;
};

GeoGraffiti.prototype.getPlace = function (id) {
    var count = _geo._places ? _geo._places.length : 0;
    for (i = 0; i < count; i++) {
        if (_geo._places[i].id === id) return _geo._places[i];
    }
    return _geo.getNullPlace();
};


GeoGraffiti.prototype.getPlaceTypeName = function (type) {
    switch (type) {
        case 1: return "Country";
        case 2: return "Region";
        case 3: return "City";
        case 4: return "Town";
        case 6: return "Address";
        case 11: return "Nature";
        case 12: return "Interest";
        case 13: return "Business";
        case 14: return "Airport";
        case 15: return "Park";
        default: return "Default";
    }
}







/* Internal Methods */
GeoGraffiti.prototype.getNullPlace = function () {
    return { "id": 0, "type": 0, "flags": 0, "key": "", "name": "", "lat": null, "lng": null };
};





// calulates the distance in km between two points
GeoGraffiti.prototype.calculateDistance = function (lon1, lat1, lon2, lat2) {
    var radlat1 = Math.PI * lat1 / 180;
    var radlat2 = Math.PI * lat2 / 180;
    var radlon1 = Math.PI * lon1 / 180;
    var radlon2 = Math.PI * lon2 / 180;
    var theta = lon1 - lon2;
    var radtheta = Math.PI * theta / 180;
	var dist = Math.sin(radlat1) * Math.sin(radlat2) + Math.cos(radlat1) * Math.cos(radlat2) * Math.cos(radtheta);
    dist = Math.acos(dist);
    dist = dist * 180 / Math.PI;
    dist = dist * 60 * 1.1515;

	return dist * 1.609344; // km
	//return dist * 0.8684; // miles
}