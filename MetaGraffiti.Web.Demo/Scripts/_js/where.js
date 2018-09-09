var _where = new where();

function where() {
	this.selectedCountryID = 0;
	this.defaultInfo = null;
	this.resizeWidth = 0;
	this.resize = 0;
	this.mode = "map";

    return this;
}

where.prototype.start = function (mode) {
	if (mode == null || mode == "" || mode == 0) mode = "map";
	this.mode = mode;

	// open layer event handlers
	mapOL.prototype.clickCountry = _where.gotoPlace;
	mapOL.prototype.clickPlace = _where.selectPlace;
	mapOL.prototype.hoverPlace = _where.hoverPlace;

	// d3 event handlers
	mapD3.prototype.onHoverCountry = _where.showPlaceInfo;
	mapD3.prototype.onHoverHexabin = _where.hoverHex;
	mapD3.prototype.onClick = function (d) {
		console.log("onClick");
		console.log(d);
		if (d != null && d.id > 0)
			_where.selectCountry(d.id);
		else
			_where.selectCountry(0);
	}
}

where.prototype.hoverHex = function (d) {
	//if (d.length > 0) _mapD3.hoverShape(d[0].country);
	_where.showPlaceList(d);
}

where.prototype.show = function () {
	if (_where.mode == "map") {
		$("#mapD3").hide();
		$("#mapOL").show();

		if (!_mapOL.isStarted()) _mapOL.start(_geo);
		_mapOL.show("bw");
		_where.drawMakers();
	} else {
		$("#mapOL").hide();
		$("#mapD3").show();

		/*
		var hash = window.location.hash;
		if (hash.length == 3) {
			var id = hash.substring(1);
			_where.selectCountry(id);
		}*/

		if (!_mapD3.isStarted()) _mapD3.start(_geo);
		if (_where.mode == "3d") {
			_mapD3.onDone = _where.startD3;
			_mapD3.show("globe");
		} else {
			_mapD3.onDone = _where.startD3;
			_mapD3.show("world");
		}
	}
}

where.prototype.startD3 = function () {
	_mapD3.onDone = _where.refreshPoints;

	var hash = window.location.hash;
	if (hash.length == 3) {
		var id = hash.substring(1);
		_where.selectCountry(id);
		_where.refreshPoints(places);
	} else {
		var places = _geo.getPlaces();
		_where.refreshPoints(places);
	}
}
/*
where.prototype.startD3 = function () {
	if (_where.mode == "3d")
		_mapD3.onDone = _where.refreshPoints;
	else
		_mapD3.onDone = _where.refreshHexabin;

	var hash = window.location.hash;
	if (hash.length == 3) {
		var id = hash.substring(1);
		_where.selectCountry(id);
		if (_where.mode == "3d") _where.refreshPoints(places);
	} else {
		var places = _geo.getPlaces();
		if (_where.mode == "3d")
			_where.refreshPoints(places);
		else
			_where.refreshHexabin(places);
	}
}
*/

where.prototype.drawMakers = function (places) {
	if (places != null) _geo.setPlaces(places);
	var places = _geo.getPlaces();

	// show flags for all visited countries
	var cv = _geo.getVisitedCountries();
	for (var i = 0; i < cv.length; i++) {
		if (cv[i].id != _where.selectedCountryID) _mapOL.addCountry(cv[i]);
	}
	var cp = _geo.getPlannedCountries();
	for (var i = 0; i < cp.length; i++) {
		if (cp[i].id != _where.selectedCountryID) _mapOL.addCountry(cp[i]);
	}

	// show all places if country selected
	if (_where.selectedCountryID > 0) {
		_mapOL.zoomToBounds(_geo.getCountryBounds(_where.selectedCountryID));
		for (var i = 0; i < places.length; i++) {
			var m = _mapOL.addPlace(places[i]);
		}
	}
}

where.prototype.refreshPoints = function (places) {
	if (places != null) _geo.setPlaces(places);
	var places = _geo.getPlaces();
	if (places == null || places.length == 0)
		d3.json("/content/_topo/data/places.json", _where.refreshPoints);
	else
		_mapD3.drawPoints(places);
}

where.prototype.refreshHexabin = function (places) {
	if (places != null) _geo.setPlaces(places);
	var places = _geo.getPlaces();
	if (places == null || places.length == 0)
		d3.json("/content/_topo/data/places.json", _where.refreshHexabin);
	else
		_mapD3.drawHexabin(places);
}

// updates info bar with selected place details
where.prototype.showPlaceInfo = function (id) {
	// check if initial/default value is safed
	if (_where.defaultInfo == null) _where.defaultInfo = $("#info").html();

	if (id == null || id == 0)
		$("#info").html(_where.defaultInfo); // reset to default
	else {
		// show country or place template
		var list = "";
		var place = null;
		var icon = "";
		if (id < 1000) {
			place = _geo.getCountry(id);
			icon = "flags/" + place.key;

			var cities = _geo.getPlaces();
			for (var i = 0; i < cities.length; i++) {
				var city = cities[i];
				if (city.country == id) {
					list += "<span>" + city.name + "<br /></span>";
				}
			}

		} else {
			place = _geo.getPlace(id);
			icon = "icons/places/" + _geo.getPlaceTypeName(place.type);
		}
		var html = "<div><img src='https://www.kevinandearth.com/images/" + icon + ".png' /><span>" + place.name + "</span></div>";
		html += "<div id=info2>" + list + "</div>";
		$("#info").html(html);
	}
}

where.prototype.showPlaceList = function (ids) {
	// check if initial/default value is safed
	if (_where.defaultInfo == null) _where.defaultInfo = $("#info").html();

	if (ids == null || ids.length == 0)
		$("#info").html(_where.defaultInfo); // reset to default
	else {
		// show country or place template
		var html = "";
		var places = [];
		for (var i = 0; i < ids.length; i++) {
			var place = _geo.getPlace(ids[i].id);
			places.push(place);
			//html += place.name + ", ";
		}
		places.sort(function (a, b) { return a.name - b.name; });
		for (var i = 0; i < places.length; i++) {
			html += places[i].name + ", ";
		}

		if (html.length > 2) html = html.substring(0, html.length - 2);
		html = "<span class='h3'>" + html + "</span>";
		$("#info").html(html);
	}
}

where.prototype.selectContinent = function (continent) {
	// show contintent info

	// unmark all selected
	$("NAV .geonav").removeClass("selected");
	$("#mapD3 .geoshape").removeClass("selected");

	// zoom active map to continent bounds
	if (_mapD3.isStarted()) _mapD3.zoomToBounds(_geo.getContinentBounds(continent));
	if (_mapOL.isStarted()) _mapOL.zoomToBounds(_geo.getContinentBounds(continent));
}

where.prototype.selectCountry = function (id) {
	console.log("where.selectCountry");

	// get country and update id
	var c = _geo.getCountry(id);
	_where.selectedCountryID = c.id;
	_where.selectedCountry = c;
	id = c.id;

	// show country info
	_where.showPlaceInfo(id);

	// mark geonav, geoshape and geomaker as selected
	$("NAV .geonav").removeClass("selected");
	if (id > 0) $("NAV #geo" + id).addClass("selected");

	// zoom active map to country bounds
	if (_mapD3.isStarted()) {
		_mapD3.selectShape(id);

		// zoom to selected shape by id
		var d = _mapD3.getShapeByID(id);
		_mapD3.zoomTo(d);
		
		// download country specific data if needed
		/*
		if (!_mapD3.isGlobe()) {
			if (id > 0)
				d3.json("/json/places/" + id, _where.refreshHexabin);
			else
				d3.json("/json/cities", _where.refreshHexabin);
		}
		*/

		// update where buttons with selected country when in D3 mode
		$("#where1D").attr("href", "/where/" + c.name.replace(' ', '-'));
		$("#where2D").attr("href", "/where/2D" + (_where.mode == "2d" ? "" : "#" + c.key));
		$("#where3D").attr("href", "/where/3D" + (_where.mode == "3d" ? "" : "#" + c.key));
		window.location.hash = c.key;
	}
	if (_mapOL.isStarted()) {
		_mapOL.zoomToBounds(_geo.getCountryBounds(id));
	}
}

where.prototype.hoverPlace = function (id) {
	// show place info and update url
	_where.showPlaceInfo(id);

	// select the marker on the map
	if (_mapOL.isStarted()) _mapOL.selectMarkerByID(id);
}

where.prototype.selectPlace = function (id) {
	if (id < 1000)
		_where.selectCountry(id);
	else if (id < 10000)
		_where.selectRegion(id);
	else {
		// show place info and update url
		window.location.hash = id;
		_where.hoverPlace(id);

		// clear any selected navigation items
		$(".geonav").removeClass("selected");

		// highlight and expand selected item
		var nav = $("#geo" + id);
		nav.addClass("selected");
		if (!nav.is(":visible")) nav.show();

		$(".navtoggle UL").hide();
		var toggle = nav.parent().parent();
		if (toggle.parent().is(".navtoggle")) toggle.show();
	}
}

where.prototype.selectRegion = function (id) {
	window.location.hash = id;
	var b = _geo.getRegionBounds(id);
	console.log("selectRegion(" + b + ")");
	_mapOL.zoomToBounds(b);
}

where.prototype.gotoPlace = function (id) {
	if (_where.mode == "map" && id > 0 && id < 1000)
		window.location = "/where/" + _geo.getCountryUrlPathKey(id);
	else
		_where.selectPlace(id);
}

// extract place id from geonav element and call selectPlace
where.prototype.onClickGeoNav = function (event) {
	event.preventDefault();
	var gid = this.id;
	if (gid.length > 3 && gid.substring(0, 3).toLowerCase() == "geo") {
		var id = gid.substring(3);
		if (id > 0) _where.gotoPlace(id);
	}
}

where.prototype.onHoverGeoNav = function (event) {
	event.preventDefault();
	var gid = this.id;
	if (gid.length > 3 && gid.substring(0, 3).toLowerCase() == "geo") {
		var id = gid.substring(3);
		if (id > 0) {
			if (_mapD3.isStarted()) _mapD3.hoverShape(id);
		}
	}
}


where.prototype.zoomToRegion = function (region) { }
where.prototype.zoomToPlace = function (place) { }



where.prototype.onResize = function () {
	var w = $(window).width();
	if (_where.resizeWidth != w) {
		_where.resizeWidth = w;
		if ((w > 480 && w < 1200) || (w >= 1900)) {
			if (_where.resize > 0) window.clearTimeout(_where.resize);
			_where.resize = window.setTimeout(function () { _where.doResize(); }, 250);
			if (_mapD3.isStarted()) $("#mapD3").hide();
			if (_mapOL.isStarted()) $("#mapOL").hide();
		}
	}
}

where.prototype.doResize = function () {
	_where.resize = 0;
	var w = $(window).width();
	
	if (_mapD3.isStarted()) {
		$("#mapD3").show();
		_mapD3.show();
	}
	if (_mapOL.isStarted()) {
		$("#mapOL").show();
		_mapOL.show();
	}
}























$(document).ready(function () {
    $("LI.navtoggle").click(function () { navtoggleClick(this) });
    $("LI.navtoggle").on("touchstart", function () { navtoggleClick(this) });
});

function navtoggleClick(nav) {
    var sub = $(nav).children("ul").first();
    if (!sub.is(":visible")) {
        // hide all others
        $(".navtoggle").filter(function () { return this != nav; }).children("ul").hide();

        // show this one
        sub.fadeIn(500);
    }
}