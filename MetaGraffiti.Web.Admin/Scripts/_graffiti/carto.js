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

function drawCartoPlaces(scale, types) {
	if (!scale) scale = 1;
	if (!types) types = false;
	for (var index = 0; index < _cartoPlaces.length; index++) {
		var place = _cartoPlaces[index];
		var color = getMapColor(index);

		var icon = getPlaceIcon(color, scale);
		if (types) icon = getCartoPlaceTypeIcon(place.type, scale);
		place.marker = markPlace(place, icon, _cartoPlaceUrl);
		var center = new google.maps.LatLng(place.center.lat, place.center.lng);
		_cartoPlacesBounds.extend(center);
	}
	return _cartoPlacesBounds;
}


function fitCartoPlaces() {
	fitBounds(_cartoPlacesBounds);
}



function getCartoPlaceTypeIcon(type, scale) {
	if (!scale) scale = 1;
	var icon = {
		url: "/icon/placetype/" + type,
		size: new google.maps.Size(24, 24),
		anchor: new google.maps.Point(12, 24),
	};
	return icon;
}


function initCartoPlaceTypeLookup(field) {
	$(field).typeahead({
		minLength: 1,
		highlight: true,
	}, {
			name: "PlaceTypes",
			source: new Bloodhound({
				prefetch: "/json/placetypes/",
				queryTokenizer: Bloodhound.tokenizers.whitespace,
				datumTokenizer: Bloodhound.tokenizers.whitespace
			})
		});
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