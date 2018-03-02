var _mapDiv = null;
var _mapCoder = null;
var _mapGoogle = null;

function initMap() {
	_mapDiv = document.getElementById('map');
	_mapCoder = new google.maps.Geocoder;
	_mapGoogle = new google.maps.Map(_mapDiv, {
		zoom: 5,
		center: new google.maps.LatLng(0, 0),
		mapTypeId: google.maps.MapTypeId.TERRAIN,
		//gestureHandling: 'greedy'
	});
}

function showTrack(coordinates) {
	var track = new google.maps.Polyline({
		path: coordinates,
		geodesic: true,
		strokeColor: '#ff0000',
		strokeOpacity: 0.8,
		strokeWeight: 3
	});

	track.setMap(_mapGoogle);

	var bounds = new google.maps.LatLngBounds();
	for (var i = 0; i < coordinates.length; i++) {
		bounds.extend(new google.maps.LatLng(coordinates[i].lat, coordinates[i].lng));
	}
	_mapGoogle.fitBounds(bounds);

	return track;
}

function gotoPoint(lat, lng) {
	var position = new google.maps.LatLng(lat, lng);
	_mapGoogle.panTo(position);
	_mapGoogle.setZoom(15);
}


function markPoint(lat, lng, title) {
	var marker = new google.maps.Marker({
		position: new google.maps.LatLng(lat, lng),
		map: _mapGoogle,
		title: title
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