// ==================================================
// Globals

var _mapDiv = null;
var _mapCoder = null;
var _mapGoogle = null;



// ==================================================
// Initialization

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

// attempts to fix the entire world in the viewport
function showWorld() {
	_mapGoogle.panTo(new google.maps.LatLng(0, 0));
	_mapGoogle.setZoom(Math.ceil(Math.log2($(_mapDiv).width())) - 8);
}

// pans and zooms to a given point
function gotoPoint(lat, lng) {
	var position = new google.maps.LatLng(lat, lng);
	_mapGoogle.panTo(position);
	_mapGoogle.setZoom(15);
}

// gets a basic marker
function getMarker(lat, lng, title, icon) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		title: title,
		icon: icon,
	});

	return marker;
}




// ==================================================
// Bounds

// calculates the bounds of a path (points with lat lng)
function calcBounds(path) {
	var bounds = new google.maps.LatLngBounds();
	for (var i = 0; i < path.length; i++) {
		bounds.extend(new google.maps.LatLng(path[i].lat, path[i].lng));
	}
	return bounds;
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

// draws a bounds that is editable and fits to viewport
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

// draws a rectangle and pans/zooms to fit in viewport
function showBounds(bounds, color, fill) {
	_mapGoogle.fitBounds(bounds);

	return drawBounds(bounds, color, fill);
}

// fits bounds in viewport
function fitBounds(bounds) {
	_mapGoogle.fitBounds(bounds);
}


// ==================================================
// Points

// displays a marker on the map with an optional title and custom numbered marker
function markPoint(lat, lng, title, icon) {
	var marker = getMarker(lat, lng, title, icon);
	marker.setMap(_mapGoogle);
	return marker;
}

// marks a point with the start icon
function markStart(lat, lng, title) {
	return markPoint(lat, lng, title, "https://maps.gstatic.com/intl/en_ALL/mapfiles/dd-start.png");
}

// marks a point with the finish icon
function markFinish(lat, lng, title) {
	return markPoint(lat, lng, title, "https://maps.gstatic.com/intl/en_ALL/mapfiles/dd-end.png");
}

// marks a point with the stop icon
function markStop(lat, lng, title) {
	return markPoint(lat, lng, title, "https://maps.gstatic.com/intl/en_ALL/mapfiles/dd-stop.png");
}

// creates a draggle point with the default place icon
function dragPoint(lat, lng, title, icon) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		map: _mapGoogle,
		title: title,
		icon: icon,
		draggable: true,
	});

	return marker;
}


// ==================================================
// Places

// displays a marker for a place on the map, adds click navigation if url is provided which takes the placekey
function markPlace(place, icon, url) {
	var marker = getMarker(place.center.lat, place.center.lng, place.name, icon);
	marker.setMap(_mapGoogle);

	marker.placeKey = place.key;
	if (url) {
		marker.addListener('click', function () {
			window.location = url.replace('{key}', marker.placeKey);
		});
	}

	return marker;
}









// ==================================================
// Tracks

// draws a track and zooms/pans it into the viewport
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

	var bounds = calcBounds(path);
	_mapGoogle.fitBounds(bounds);

	return track;
}


// ==================================================
// Icons

function getPlaceIcon(color, scale) {
	if (!color) color = "#000";
	var icon = {
		path: "M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z",
		scale: scale,
		anchor: new google.maps.Point(12, 24),
		fillOpacity: 1,
		fillColor: color,
		strokeColor: "#000",
	};
	return icon;
}

function getNumberIcon(number) {
	if (!number) number = 0;
	if (number < 1 || number > 10) number = "red-circle";
	return "http://maps.google.com/mapfiles/kml/paddle/" + number + ".png";
}






// ==================================================
// TODO

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

