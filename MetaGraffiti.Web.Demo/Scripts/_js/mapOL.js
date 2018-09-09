// ==================================================
// OpenLayers OpenStreetMap
// ==================================================

var _mapOL = new mapOL();

function mapOL() {
	this.showPopup = true;
	this.places = [];
	this.defaultExtents = [[-135.7031249999989, 69.65708627301332], [180.70312499999886, -66.51326044310828]];

    return this;
}

mapOL.prototype.start = function (geo, style) {
    this.geo = geo;
    //console.log("mapOL.start");

    if (this.map != null) this.map.destroy();
    this.zoomControl = new OpenLayers.Control.Zoom();
    this.map = new OpenLayers.Map("mapOL", {
        controls: [
            this.zoomControl,
            new OpenLayers.Control.Navigation(),
            new OpenLayers.Control.ArgParser()
        ],
        maxExtent: new OpenLayers.Bounds(-180, -90, 180, 90)
    });
    this.projTo = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
    this.projFrom = new OpenLayers.Projection("EPSG:4326");   // Google Maps

    // init base layer
    this.tiles = this.getMapTiles(style);
    this.map.addLayer(this.tiles);

    // init countries layer
    this.countries = new OpenLayers.Layer.Markers("Countries");
    this.map.addLayer(this.countries);

	// init cuntry flag icon
    this.flagIconSize = new OpenLayers.Size(32, 32);
    this.flagIconOffset = new OpenLayers.Pixel(-this.flagIconSize.w / 2, -this.flagIconSize.h);

    // init markers layer
    this.markers = new OpenLayers.Layer.Markers("Markers");
    this.map.addLayer(this.markers);

    // init default icon
    this.mapIconSize = new OpenLayers.Size(24, 24);
    this.mapIconOffset = new OpenLayers.Pixel(-this.mapIconSize.w / 2, -this.mapIconSize.h);
    this.mapIconDefault = new OpenLayers.Icon("/images/pins/starG.png", this.mapIconSize, this.mapIconOffset);
    this.mapIconSelected = new OpenLayers.Icon("/images/pins/starO.png", this.mapIconSize, this.mapIconOffset);
};

mapOL.prototype.show = function (style, id) {
    if (style != null) this.setMapStyle(style);
    if (id != null) {
    }
    _mapOL.zoomToBounds(_mapOL.defaultExtents);
}

mapOL.prototype.zoomTo = function (lng, lat, zoom) {
    if (lng == null || lat == null)
        _mapOL.map.zoomToMaxExtent();
    else {
        var xy = _mapOL.getTranform(lng, lat);
        _mapOL.map.setCenter(xy, zoom);
    }
};

mapOL.prototype.moveTo = function (lng, lat) {
    var xy = _mapOL.getTranform(lng, lat);
    _mapOL.map.moveTo(xy);
};

mapOL.prototype.zoomToBounds = function (b) {
	if (b == null)
		_mapOL.map.zoomToMaxExtent();
	else
		_mapOL.zoomToExtent(b[0][0], b[0][1], b[1][0], b[1][1]);
};

mapOL.prototype.zoomToExtent = function (minlng, minlat, maxlng, maxlat) {
    if (minlng == null || minlat == null || maxlng == null || maxlat == null)
        _mapOL.map.zoomToMaxExtent();
    else {
        var sw = _mapOL.getTranform(minlng, minlat);
        var ne = _mapOL.getTranform(maxlng, maxlat);
        var view = new OpenLayers.Bounds(sw.lon, sw.lat, ne.lon, ne.lat); // left, bottom, right, top
        var zoom = _mapOL.map.getZoomForExtent(view, false);
        _mapOL.map.moveTo(view.getCenterLonLat(), zoom);
    }
};

mapOL.prototype.addCountry = function (country) {
	var html = "";
    var point = this.getTranform(country.lng, country.lat);
    var marker = new OpenLayers.Marker(point, new OpenLayers.Icon("/images/flags/" + country.key + ".png", this.flagIconSize, this.flagIconOffset));
    marker.id = country.id;
    marker.key = country.key;
    marker.name = country.name;
    marker.country = country.country;
    this.countries.addMarker(marker);
    this.addMarkerEvents(marker);
};

mapOL.prototype.addCountries = function (countries) {
    for (var i = 0; i < countries.length; i++) {
        this.addCountry(countries[i]);
    }
};

mapOL.prototype.showCountries = function (show) {
    this.countries.setVisibility(show);
}


mapOL.prototype.addPlace = function (place) {
    var html = "";
    var type = this.geo.getPlaceTypeName(place.type);
    var point = this.getTranform(place.lng, place.lat);
    var flags = "g";
    //if (place.flags == 2) flags = "G"; else if (place.flags == 1) flags = "O";
    var marker = new OpenLayers.Marker(point, new OpenLayers.Icon("/images/icons/places/" + type + flags + ".png", this.mapIconSize, this.mapIconOffset));
    marker.id = place.id;
    marker.key = place.key;
    marker.name = place.name;
    marker.country = place.country;
    this.markers.addMarker(marker);
    this.addMarkerEvents(marker);
    this.places.push(marker);
    return marker;
};

mapOL.prototype.addMarkerEvents = function (marker) {
    marker.events.register("click", marker, function () {
        _mapOL.clickMarker(marker);
    });
    marker.events.register("touchstart", marker, function () {
        _mapOL.clickMarker(marker);
    });
    marker.events.register("mouseover", marker, function () {
        _mapOL.hoverMarker(marker, true);
    });
    marker.events.register("mouseout", marker, function () {
        _mapOL.hoverMarker(marker, false);
    });
};

mapOL.prototype.addMarker = function (lng, lat, key, name, icon, html) {
    var marker;
    var point = this.getTranform(lng, lat);
    if (icon == null || icon == "")
        marker = new OpenLayers.Marker(point, this.mapIconDefault.clone());
    else {
        marker = new OpenLayers.Marker(point, new OpenLayers.Icon("/images/" + icon + ".png", this.mapIconSize, this.mapIconOffset));
    }
    marker.key = key;
    marker.name = name;
    this.markers.addMarker(marker);
    this.addMarkerEvents(marker);
};

mapOL.prototype.clickCountry = function (id) { }
mapOL.prototype.clickPlace = function (id) { }

mapOL.prototype.clickMarker = function (marker) {
    if (marker.id < 1000)
        mapOL.prototype.clickCountry(marker.id);
    else
        mapOL.prototype.clickPlace(marker.id);
};

mapOL.prototype.hoverPlace = function (id) { }

mapOL.prototype.hoverMarker = function (marker, over) {
    // remove prior popup
    if (_mapOL.popUp != null) _mapOL.map.removePopup(_mapOL.popUp);

    // reset prior marker hover image
    if (_mapOL.hoveredMarker != null && _mapOL.hoveredMarker.icon.url.toLowerCase().indexOf("/places/") > 0) _mapOL.hoveredMarker.setUrl(_mapOL.hoveredMarker.icon.url.toLowerCase().replace("o.png", "g.png"));

    _mapOL.hoveredMarker = marker;
    if (marker != null && over) {
    	// update marker hover image
        if (_mapOL.hoveredMarker.icon.url.toLowerCase().indexOf("/places/") > 0) _mapOL.hoveredMarker.setUrl(_mapOL.hoveredMarker.icon.url.toLowerCase().replace("g.png", "o.png"));
        
        // show popup if enabled
        if (_mapOL.showPopup) {
            var name = marker.name;
            var size = _mapOL.measurePopup(name);
            _mapOL.popUp = new OpenLayers.Popup("p" + marker.key, marker.lonlat, size, name);
            _mapOL.popUp.backgroundColor = "#000";
            _mapOL.popUp.setOpacity(0.8);
            _mapOL.map.addPopup(_mapOL.popUp);
        }

        // call external hover event
        mapOL.prototype.hoverPlace(marker.id);
    }
};

mapOL.prototype.selectMarkerByID = function (id) {
	for (var i = 0; i < _mapOL.places.length; i++) {
		var marker = _mapOL.places[i];

		var icon = marker.icon.url.toLowerCase();
		if (icon.indexOf("/places/") > 0) {
			if (marker.key == id.toString()) {
				if (icon.indexOf("g.png") > 0) {
					_mapOL.markers.removeMarker(marker);
					marker.setUrl(marker.icon.url.toLowerCase().replace("g.png", "o.png"));
					_mapOL.markers.addMarker(marker);
				}
			} else {
				if (icon.indexOf("o.png") > 0) marker.setUrl(marker.icon.url.toLowerCase().replace("o.png", "g.png"));
			}
		}
	}
}

function fix(d, o) {
	return d; //Math.round(d * 995) / 1000;
}

/* Internal Methods */
mapOL.prototype.getViewport = function() {
	var ex = _mapOL.map.getExtent();
	var min = _mapOL.getTranformReverse(ex.left, ex.top);
	var max = _mapOL.getTranformReverse(ex.right, ex.bottom);
	var b = [[min.lon, min.lat], [max.lon, max.lat]];
	//console.log(fix(min.lon, 0.01) + ", " + fix(min.lat, 0.01) + ", " + fix(max.lon, -0.01) + ", " + fix(max.lat, -0.01));
	return b;
	/*
	var p = new OpenLayers.Pixel(0, 0);
	var b = _mapOL.map.getLonLatFromPixel(p);
	//console.log(b);*/
}

mapOL.prototype.getTranform = function (lng, lat) {
	return new OpenLayers.LonLat(lng, lat).transform(_mapOL.projFrom, _mapOL.projTo);
};

mapOL.prototype.getTranformReverse = function (lng, lat) {
	return new OpenLayers.LonLat(lng, lat).transform(_mapOL.projTo, _mapOL.projFrom);
};

mapOL.prototype.setMapStyle = function (style) {
    var tiles = _mapOL.getMapTiles(style);
    _mapOL.map.addLayer(tiles);
    if (_mapOL.tiles != null) _mapOL.map.removeLayer(_mapOL.tiles, tiles);
    _mapOL.tiles = tiles;
}

mapOL.prototype.getMapTiles = function (style) {
    switch (style) {
        case "bw":
        case "mono":
            return new OpenLayers.Layer.OSM("BlackAndWhite", [
                "http://a.tile.stamen.com/toner/${z}/${x}/${y}.png",
                "http://b.tile.stamen.com/toner/${z}/${x}/${y}.png",
                "http://c.tile.stamen.com/toner/${z}/${x}/${y}.png",
                "http://d.tile.stamen.com/toner/${z}/${x}/${y}.png"]);
        case "mq":
        case "map":
            return new OpenLayers.Layer.OSM("MapQuest", [
                "http://otile1.mqcdn.com/tiles/1.0.0/osm/${z}/${x}/${y}.png",
                "http://otile2.mqcdn.com/tiles/1.0.0/osm/${z}/${x}/${y}.png",
                "http://otile3.mqcdn.com/tiles/1.0.0/osm/${z}/${x}/${y}.png",
                "http://otile4.mqcdn.com/tiles/1.0.0/osm/${z}/${x}/${y}.png"]); // MapQuest     otile[1234].mqcdn.com/tiles/1.0.0/osm/zoom/x/y.jpg  0-19
        case "sat":
        case "aerial":
            return new OpenLayers.Layer.OSM("MapQuest Aerial", [
                "http://otile1.mqcdn.com/tiles/1.0.0/sat/${z}/${x}/${y}.png",
                "http://otile2.mqcdn.com/tiles/1.0.0/sat/${z}/${x}/${y}.png",
                "http://otile3.mqcdn.com/tiles/1.0.0/sat/${z}/${x}/${y}.png",
                "http://otile4.mqcdn.com/tiles/1.0.0/sat/${z}/${x}/${y}.png"]); // MapQuest Open Aerial     otile[1234].mqcdn.com/tiles/1.0.0/sat/zoom/x/y.jpg  0-11 globally, 12+ in the U.S.
        case "cycle":
            return new OpenLayers.Layer.OSM("OpenCycleMap", [
                "http://a.tile.opencyclemap.org/cycle/${z}/${x}/${y}.png",
                "http://b.tile.opencyclemap.org/cycle/${z}/${x}/${y}.png",
                "http://c.tile.opencyclemap.org/cycle/${z}/${x}/${y}.png"]); // OpenCycleMap    [abc].tile.opencyclemap.org/cycle/zoom/x/y.png  0-18
        case "tera":
        case "terra":
        case "terrain":
            return new OpenLayers.Layer.OSM("Migurski", ["http://tile.stamen.com/terrain-background/${z}/${x}/${y}.png"]); // Migurski's Terrain    tile.stamen.com/terrain-background/zoom/x/y.jpg 4-18, US-only (for now)
        default:
            return new OpenLayers.Layer.OSM(); // OSM 'standard' style	http://[abc].tile.openstreetmap.org/zoom/x/y.png   	 0-19
    }
    //CloudMade (Web style)	 [abc].tile.cloudmade.com/your_CloudMade_API_key/1/256/zoom/x/y.png	 0-18
    //CloudMade (NoNames style)	 [abc].tile.cloudmade.com/your_CloudMade_API_key/3/256/zoom/x/y.png	 0-18
    //CloudMade (Fine line style)	 [abc].tile.cloudmade.com/your_CloudMade_API_key/2/256/zoom/x/y.png	 0-18
};

// dynamic text length calculation
mapOL.prototype.measurePopup = function (html, css) {
    var o = $('<div>' + html + '</div>')
            .css({ 'position': 'absolute', 'float': 'left', 'white-space': 'nowrap', 'visibility': 'hidden' })
            .addClass(css)
            .hide()
            .appendTo($("#mapOL"));
    var s = new Array();
    var w = o.width();
    var h = o.height();
    o.remove();
    return new OpenLayers.Size(w, h);
};

// has the start method been called yet
mapOL.prototype.isStarted = function () {
    return _mapOL.geo != null;
}


mapOL.log = function (log) {
    if (mapOL.logging) console.log(log);
};