var _mapDiv = null;
var _mapCoder = null;
var _mapGoogle = null;

var _mapColors = [
	'#007bff',
	'#28a745',
	'#17a2b8',
	'#ffc107',
	'#dc3545',

	/*
	'#b8daff',
	'#c3e6cb',
	'#bee5eb',
	'#ffeeba',
	'#f5c6cb',
	*/
	'#007bff',
	'#28a745',
	'#17a2b8',
	'#ffc107',
	'#dc3545',
];

var _infoColors = [
	'#b8daff',
	'#c3e6cb',
	'#bee5eb',
	'#ffeeba',
	'#f5c6cb',
];

// initalizes the global google map objects
function initMap() {
	_mapDiv = document.getElementById('map');
	_mapCoder = new google.maps.Geocoder;
	_mapGoogle = new google.maps.Map(_mapDiv, {
		zoom: 5,
		center: new google.maps.LatLng(0, 0),
		mapTypeId: google.maps.MapTypeId.TERRAIN,
		keyboardShortcuts: false
		//gestureHandling: 'greedy'
	});
}

// returns a color to use for a given inded
function getMapColor(index) {
	if (index === undefined) return '#ff0000';
	return _mapColors[index % 10];
}

function getInfoColor(index) {
	if (index === undefined) return _infoColors[0];
	return _infoColors[index % 5];
}

// attempts to fix the entire world in the view port
function showWorld() {
	_mapGoogle.panTo(new google.maps.LatLng(0, 0));
	_mapGoogle.setZoom(Math.ceil(Math.log2($(_mapDiv).width())) - 8);
}

// draws a rectangle with the given bounds, color and fill weight
function drawBounds(bounds, color, fill) {
	if (color === null) color = '#ff0000';
	if (fill === null) fill = 0.2;
	var rectangle = new google.maps.Rectangle({
		bounds: bounds,
		geodesic: false,
		strokeColor: color,
		strokeOpacity: 0.8,
		strokeWeight: 2,
		fillColor: color,
		fillOpacity: fill,
		map: _mapGoogle
	});

	return rectangle;
}

function showBounds(bounds, color, fill) {
	_mapGoogle.fitBounds(bounds);

	return drawBounds(bounds, color, fill);
}

function fitBounds(bounds) {
	_mapGoogle.fitBounds(bounds);
}

function editBounds(bounds, color, fill) {
	if (color == null) color = '#ff0000';
	if (fill == null) fill = 0.2;
	var rectangle = new google.maps.Rectangle({
		bounds: bounds,
		editable: true,
		//draggable: true,
		geodesic: false,
		strokeColor: color,
		strokeOpacity: 0.8,
		strokeWeight: 2,
		fillColor: color,
		fillOpacity: fill,
		map: _mapGoogle
	});

	_mapGoogle.fitBounds(bounds);

	return rectangle;
}


function showTrack(path, color) {
	if (color == null) color = '#ff0000';
	var track = new google.maps.Polyline({
		path: path,
		geodesic: true,
		strokeColor: color,
		strokeOpacity: 0.8,
		strokeWeight: 4,
		map: _mapGoogle
	});

	var bounds = new google.maps.LatLngBounds();
	for (var i = 0; i < path.length; i++) {
		bounds.extend(new google.maps.LatLng(path[i].lat, path[i].lng));
	}
	_mapGoogle.fitBounds(bounds);

	return track;
}

function gotoPoint(lat, lng) {
	var position = new google.maps.LatLng(lat, lng);
	_mapGoogle.panTo(position);
	_mapGoogle.setZoom(15);
}

// displays a marker on the map
function markPoint(lat, lng, title, number) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		title: title,
	});

	if (number) marker.setIcon('http://maps.google.com/mapfiles/kml/paddle/' + number + '.png');

	marker.setMap(_mapGoogle);

	return marker;
}

// displays a marker for a place on the map, adds click navigation if url is provided which takes the placekey
function markPlace(place, url) {
	var marker = markPoint(place.center.lat, place.center.lng, place.name);
	marker.placeKey = place.key;
	if (url) {
		marker.addListener('click', function () {
			window.location = url.replace('{key}', marker.placeKey);
		});
	}
	return marker;
}

function dragPoint(lat, lng, title, number) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		draggable: true,
		title: title,
	});

	if (number) marker.setIcon('http://maps.google.com/mapfiles/kml/paddle/' + number + '.png');

	marker.setMap(_mapGoogle);

	return marker;
}

function markTrack(lat, lng, number) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		map: _mapGoogle,
		icon: 'http://maps.google.com/mapfiles/kml/paddle/' + number + '.png'
	});

	return marker;
}


function markStart(lat, lng) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		map: _mapGoogle,
		icon: 'http://maps.google.com/mapfiles/marker_greenA.png'
	});

	return marker;
}

function markStop(lat, lng) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		map: _mapGoogle,
		icon: 'http://maps.google.com/mapfiles/marker_greenB.png'
	});

	return marker;
}

function lookupLocation(lat, lng, set) {
	_mapCoder.geocode({ 'location': { lat: lat, lng: lng } }, function (results, status) {
		if (status === 'OK') {
			if (results[1]) {
				set(results[1].formatted_address);
			} else {
				set('No results found');
			}
		} else {
			set('Geocoder failed due to: ' + status);
		}
	});
}
