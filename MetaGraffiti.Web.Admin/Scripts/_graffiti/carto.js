var _cartoPlaces = null;			// a reference to the array of places
var _cartoPlacesBounds = null;		// the bounds for all carto places

var _cartoPlaceUrl = "";			// a url with a {place.key} placeholder
var _cartoPlaceBounds = null;		// bounds of the selected carto place



function initCartoPlaces(places, url) {
	_cartoPlaces = places;
	_cartoPlacesBounds = new google.maps.LatLngBounds();

	_cartoPlaceUrl = url;
	_cartoPlaceBounds = null;
}

function drawCartoPlaces(scale) {
	if (!scale) scale = 1;
	for (var index = 0; index < _cartoPlaces.length; index++) {
		var place = _cartoPlaces[index];
		var color = getMapColor(index);

		place.marker = markPlace(place, getPlaceIcon(color, scale), _cartoPlaceUrl);
		var center = new google.maps.LatLng(place.center.lat, place.center.lng);
		_cartoPlacesBounds.extend(center);
	}
	return _cartoPlacesBounds;
}

function fitCartoPlaces() {
	fitBounds(_cartoPlacesBounds);
}






function bounceCartoPlaceMarker(index) {
	for (var i = 0; i < _cartoPlaces.length; i++) {
		if (i == index) {
			_cartoPlaces[i].marker.setAnimation(google.maps.Animation.BOUNCE);
		} else {
			_cartoPlaces[i].marker.setAnimation(null);
		}
	}
}

function drawCartoPlaceBounds(index) {
	if (_cartoPlaceBounds) _cartoPlaceBounds.setMap(null);

	if (index >= 0) {
		var place = _cartoPlaces[index];
		var color = getMapColor(index);
		_cartoPlaceBounds = drawBounds(place.bounds, color);
	}
}

function fitCartoPlaceBounds(index) {
	var place = _cartoPlaces[index];
	fitBounds(place.bounds);
}



function actionCartoPlaceBounceMarkerAndDrawBounds() {
	var index = $(this).index();
	drawCartoPlaceBounds(index);
	bounceCartoPlaceMarker(index);
}

function actionCartoPlaceToFitBounds() {
	var index = $(this).index();
	fitCartoPlaceBounds(index);
}