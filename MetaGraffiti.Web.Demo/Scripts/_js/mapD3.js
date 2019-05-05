/* TODO
	allow resolution change for hexgrid
	optimize transitions based on performance/brower
	auto redraw hexbin on zoom change in flat mode



	fix albersUSA zoom to shape method (center is not supported)
	fix drag zoom pan bounds at max extent (prevent clipping/dragging off screen)
	fix geo object reference and dependance and abstract out base country/region data
	create custom projection for antarctica and africa
	fix russian shape data or create custom projection
	fix isPlaceVisible to account for custom view ports
	enhance globe view to make background features semi-transparent
	fix mobile globe drag or add <^> rotate arrows in corners
*/


// SVG info
// https://www.dashingd3js.com/svg-paths-and-d3js
// https://github.com/mbostock/d3/wiki/SVG-Shapes



// ==================================================
// D3 Vector Map Projections
// ==================================================

var _mapD3 = new mapD3();

// constructs and sets defaults
function mapD3() {
	this.debug = false;				// toggles logging to console

	this.drag = true;				// toggles drag mode for globe
	this.dragDelay = 250;			// time in ms to cancel the click for a drag or zoom
	this.eventstart = null;			// time in ms when click for drag or zoom started


	this.tweenTime = 1000;			// time in ms for animated zoom tweens

	this.d3hexbin = null;
	this.d3hexrad = null;


	// config zoom behavior
	this.zoom = true;				// toggles mouse wheel zoom and drag
	this.zoomFade = 150;			// time in ms for faded zoom operations
	this.zoomPad = 0.9;			// safe zoom factor
	this.zoomMin = 0;				// min zoom level
	this.zoomMax = 0;				// max zoom level
	this.zoomStep = 2;				// click zoom factor

	// world, america, oceania data caches
	this.cacheData = [null, null, null];
	this.cacheShapes = [null, null, null];
	this.cacheFeatures = [null, null, null];

	// points, hexabin, hexagrid cache
	this.places = null;
	this.points = null;
	this.hexabin = null;
	this.hexagrid = null;

	this.topo = "";
	this.proj = "";

	this.width = 0;
	this.height = 0;

	// return self for chaining
	return this;
}

// initalizes object for rendering
mapD3.prototype.start = function (geo) {
	_mapD3.log("start", geo);

	// save geo object
	this.geo = geo;

	// do default settings
	this.drag = !isMobile();

	// read element dimensions less padding 
	this.width = $("#mapD3").width();
	this.height = $("#mapD3").height();

	// append svg object to page and attach click event
	this.d3map = d3.select("#mapD3")
		.append("svg")
		.attr("width", this.width)
		.attr("height", this.height)
		.style("pointer-events", "all")
		.on("click", this.doClick);

	// get refernces to svg objects
	this.d3svg = this.d3map.append("g");
	this.d3g = this.d3svg;

	// configure drag and attach event behaviors
	this.dragstart = null;
	this.dragmouse = null;
	this.dragpoint = null;
    if (this.drag) {
        /*
		this.dragcall = d3.behavior.drag()
			.on("dragstart", function () { _mapD3.doDrag(true); })
			.on("drag", function () { _mapD3.doDrag(false); })
			.on("dragend", function () {
				_mapD3.done = true;
				//_mapD3.onDone();
			});
		this.d3map.call(this.dragcall); // this needs to be on the base object
        */
        this.dragcall = d3.drag()
            .on("start", function () { _mapD3.doDrag(true); })
            .on("drag", function () { _mapD3.doDrag(false); })
            .on("end", function () {
                _mapD3.done = true;
                _mapD3.refreshZoom();
            });
        this.d3map.call(this.dragcall); // this needs to be on the base object
	}

	// configure zoom
	//this.zoomcall = d3.behavior.zoom().on("zoomstart", function () { _mapD3.clickStart(true); }).on("zoom", this.doZoom).on("zoomend", function () { _mapD3.refreshZoom(); _mapD3.refreshDone(); });
    this.zoomcall = d3.zoom()
        .on("start", this.zoomStart)
        .on("zoom", this.doZoom)
        .on("end", this.refreshZoom);

	//this.d3map.call(_mapD3.zoomcall).on("dblclick.zoom", null); // disable double click zoom

	// return self for chaining
	return this;
}

// displays a vector map using topo data and set projection based on map and mode (true for 3d)
// null/0/world: world map with zoom enabled
// 3d/globe: 3d orthographic projection with drag enabled
// asia/europe/south/north/africa/oceania: custom solo project of continent with zoom enabled
// us/840: albers projection of united states with zoom enabled
// nz/554: custom projection of new zealand with zoom enabled
// au/36: custom projection of australia with zoom enabled
// ??/###: custom solo project of single selected country
mapD3.prototype.show = function (map) {
	_mapD3.log("show", map);

	// update size if changed
	_mapD3.width = $("#mapD3").width();
	_mapD3.height = $("#mapD3").height();
	_mapD3.d3map.attr("width", _mapD3.width).attr("height", _mapD3.height);

	// check default/existing values
	var topo = "";
	var proj = "";
	if (map == null || map == 0 || map == "") {
		topo = (_mapD3.topo != null ? _mapD3.topo : "world");
		proj = (_mapD3.proj != null ? _mapD3.proj : "world");
	} else {
		topo = _mapD3.getTopoName(map);
		proj = _mapD3.getProjName(map);
	}

	// redraw on projection change
	_mapD3.topo = topo;
	_mapD3.proj = proj;
	_mapD3.startProjection();
	var data = _mapD3.data();
	if (data != null)
		_mapD3.draw();
	else
		d3.json("/content/_topo/" + topo + ".json", this.load);

	// return self for chaining
	return this;
}

// loads and caches requested topo json data
mapD3.prototype.load = function (data) {
	_mapD3.log("load", data);

	_mapD3.data(data);
	if (data.objects.countries === undefined) {
		if (data.objects.states === undefined) {
			if (data.objects.subunits === undefined)
				_mapD3.error("load: no data found");
			else
				_mapD3.shapes(data.objects.subunits);
		}
		else
			_mapD3.shapes(data.objects.states);
	}
	else
		_mapD3.shapes(data.objects.countries);

	// render loaded data
	_mapD3.draw();
}

// renders topo data on svg layer
mapD3.prototype.draw = function () {
	_mapD3.log("draw");

	// clean up all shape objects and reset canvas
	_mapD3.d3g.selectAll(".geoshape").remove();
	//_mapD3.d3svg.attr("transform", "");

	// get expected data from cache
	var data = _mapD3.data();
	var shapes = _mapD3.shapes();

	// check feature cache or create data
	var features = _mapD3.features();
	if (features == null) {
		features = topojson.feature(data, shapes).features;
		_mapD3.features(features);
	}

	// cull features if set to solo country or continent projection
	//console.log(features);

	// show all features as default geoshapes on map with id = "g###"
	_mapD3.d3g.selectAll(".geoshape")
				.data(features)
				.enter()
				.insert("path")
				.attr("d", _mapD3.d3path)
				.attr("class", "geoshape")
				.attr("id", function (d) { return "g" + d.id; })
				.on("click", _mapD3.doClick)
				.on("mouseover", _mapD3.onMouseOver)
				.on("mouseout", _mapD3.onMouseOut)
				.each(function (d) { _mapD3.onStyle(d3.select(this), d); });

	// set done
	_mapD3.onDone();
};

// --------------------------------------------------
// Refresh projection translate, center/rotate, scale
// --------------------------------------------------

// starts d3 for the the current set projection
mapD3.prototype.startProjection = function () {
	_mapD3.log("startProjection");

	var height = _mapD3.height;
	var width = _mapD3.width;
	var proj = _mapD3.getDefaultProjection(_mapD3.proj);
    _mapD3.d3path = d3.geoPath().projection(proj);
    _mapD3.d3proj = proj;

	// get initial translate and scale
	_mapD3.defaultTranslate = _mapD3.viewTranslate = proj.translate();
	if (_mapD3.isGlobe())
		_mapD3.defaultCenter = _mapD3.viewCenter = proj.rotate();
	else if (!_mapD3.isAmerica())
		_mapD3.defaultCenter = _mapD3.viewCenter = proj.center();
	else
		_mapD3.defaultCenter = _mapD3.viewCenter = [0, 0];
	_mapD3.defaultScale = _mapD3.viewScale = proj.scale();

	// reset zoom translate and scale
	_mapD3.zoomMax = _mapD3.defaultScale;
	_mapD3.zoomMin = _mapD3.defaultScale * 10;
	//_mapD3.zoomcall.scaleExtent([_mapD3.zoomMin, _mapD3.zoomMax]);
	//_mapD3.zoomcall.scale(1);
	//_mapD3.zoomcall.translate([0, 0]);
}

// gets the default project for the current (or provided) projection name
mapD3.prototype.getDefaultProjection = function (proj) {
	_mapD3.log("getDefaultProjection", proj);

	if (proj == null || proj == "") proj = this.proj;
	var height = _mapD3.height;
	var width = _mapD3.width;
	var max = (width > height ? width : height);
	var min = (width < height ? width : height);

	var p = null;
	_mapD3.maxScale = height;
	if (_mapD3.isGlobe()) {
		_mapD3.maxScale = min / 2.1;
		p = d3.geoOrthographic().scale(_mapD3.maxScale).clipAngle(90).rotate([40, 0]);
	} else {
		switch (proj) {
			case "us":
				_mapD3.maxScale = width;
				p = d3.geo.albersUsa().scale(_mapD3.maxScale);
				break;
			case "nz":
				_mapD3.maxScale = width * 2;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([174, -41]);
				break;
			case "au":
				_mapD3.maxScale = width * 0.90;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([135, -29.5]);
				break;
			case "ru":
				_mapD3.maxScale = width * 0.37;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([100, 65]);
				break;
			case "oceania":
				_mapD3.maxScale = width * 0.75;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([146, -30]);
				break;
			case "europe":
				_mapD3.maxScale = width;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([5, 48.5]);
				break;
			case "south":
				_mapD3.maxScale = width * 0.45;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([-60, -30]);
				break;
			case "north":
				_mapD3.maxScale = width * 0.4;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([-100, 50]);
				break;
			case "asia":
				_mapD3.maxScale = width * 0.4;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([95, 40]);
				break;
			case "africa":
				_mapD3.maxScale = width * 0.6;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([25, 0]);
				break;
			default:
				_mapD3.maxScale = width * 0.158;
				p = d3.geo.mercator().scale(_mapD3.maxScale).center([0, 0]);
				break;
		}
	}

	// center custom projection
	return p.translate([width / 2, height / 2]).precision(1); //Math.SQRT1_2);
}

// refresh projection translate, scale and center/rotate with animation
mapD3.prototype.refreshView = function (t, s, c, duration) {
	_mapD3.log("refreshView");

	// check if we are changing view
	if (c == null) c = _mapD3.center();
	if (s > _mapD3.zoomMin) s = _mapD3.zoomMin;
	var dT = _mapD3.changedTranslate(t);
	var dC = _mapD3.changedCenter(c);
	var dS = _mapD3.viewScale != s;
	if (dT || dS || dC) {
		// check animation mode
		if (duration > 1) {
			_mapD3.log("refreshView.tween", duration);
			// create tween for the duration specified
            d3.transition().duration(duration)
                .tween("animate", function () {
                    var iT = d3.interpolate(_mapD3.d3proj.translate(), t);
                    var iC = d3.interpolate(_mapD3.center(), c);
                    var iS = d3.interpolate(_mapD3.d3proj.scale(), s);
                    return function (i) {
                        _mapD3.refreshView(iT(i), iS(i), iC(i));
                    };
                })
                .transition(); //.each("end", function () { _mapD3.refreshDone(); });
		} else if (duration == 1) {
			_mapD3.log("refreshView.fade");
			// fade out, wait to refresh and then fade back in
			$("#mapD3").fadeOut(_mapD3.zoomFade);
			window.setTimeout(function () {
				_mapD3.refreshView(t, s, c, 0);
				$("#mapD3").fadeIn(_mapD3.zoomFade);
			}, _mapD3.zoomFade * 1.1);
		}
		else {
			_mapD3.log("refreshView.refresh");

			// refresh immediatly and update fps
			_mapD3.frameStart = new Date().getTime();

			// update current projection
            _mapD3.d3proj.translate(t);
            _mapD3.center(c);
			_mapD3.d3proj.scale(s);

			// redraw objects on canvas
			_mapD3.refreshPath();
			_mapD3.refreshZoom();

			// update fps calculation
			var ms = new Date().getTime() - _mapD3.frameStart;
			_mapD3.fps = 1000 / ms;

			// call done if not in animation mode
			if (_mapD3.tweenTime <= 1) _mapD3.refreshDone();
		}
	}
}



mapD3.prototype.center = function (c) {
	if (_mapD3.isGlobe()) {
		if (c == null) {
			c = _mapD3.viewCenter;
			return [-c[0], -c[1]];
		}
		else
			_mapD3.d3proj.rotate([-c[0], -c[1]]);
	}
	else if (!_mapD3.isAmerica()) {
		if (c == null)
			return _mapD3.d3proj.center();
		else
			_mapD3.d3proj.center(c);
	}
	else
		return [0, 0];
}

mapD3.prototype.changedTranslate = function (t) {
	return _mapD3.viewTranslate[0] != t[0] || _mapD3.viewTranslate[1] != t[1];
}

mapD3.prototype.changedCenter = function (c) {
	var cc = _mapD3.center();
	return cc[0] != c[0] || cc[1] != c[1];
}


mapD3.prototype.refreshDone = function () {
	_mapD3.log("refreshDone", _mapD3.fps);

	// redraw hexabin if they are configured
	if (_mapD3.hexabin != null) {
		_mapD3.drawHexabin(_mapD3.places);
	}

	_mapD3.done = true;
}

// updates (or clears) path/position for all objects on svg canvas
mapD3.prototype.refreshPath = function () {
	_mapD3.log("refreshPath");

	// redraw shapes
	window.onerror = null;
    var path = d3.geoPath().projection(_mapD3.d3proj);
	_mapD3.d3g.selectAll(".geoshape").attr("d", path);
	_mapD3.d3path = path;

	var places = _mapD3.places;
	var points = _mapD3.points;
	var hexabin = _mapD3.hexabin;
	var hexgrid = _mapD3.hexgrid;

	// redraw points if enabled
	if (points != null) {
		if (_mapD3.isGlobe()) _mapD3.viewCenter = _mapD3.d3proj.rotate();
		for (var i = 0; i < points.length; i++) {
			var p = _mapD3.d3proj([places[i].lng, places[i].lat]);
			points[i].attr("cx", p[0]).attr("cy", p[1]);
			var visible = _mapD3.isPlaceVisible(places[i]);
			points[i].style("opacity", (visible ? 1 : 0.1));
		}
	}

	// redraw hexes if enabled
	if (hexabin != null) {
		// update hexagons if there are present
		_mapD3.d3g.selectAll(".geohex").each(function (d) {
			var p = _mapD3.d3proj(d.center);
			d3.select(this).attr("transform", function (d) { return "translate(" + p[0] + "," + p[1] + ")"; });
		});
	}
}

// updates the view scale and tranlsation after a manual zoom
mapD3.prototype.refreshZoom = function () {
	_mapD3.log("refreshZoom");

	// update cached view settings
	var proj = _mapD3.d3proj;
	_mapD3.viewScale = proj.scale();
	if (_mapD3.isGlobe())
		_mapD3.viewCenter = proj.rotate();
	else if (!_mapD3.isAmerica())
		_mapD3.viewCenter = proj.center();
	else
		_mapD3.viewCenter = [0, 0];
	_mapD3.viewTranslate = proj.translate();

	// update zoom event
	//_mapD3.zoomcall.scale(1);
	//_mapD3.zoomcall.translate([0, 0]);
}


// --------------------------------------------------
// Zoom and drag implementation
// --------------------------------------------------

// handles startdrag and drag events for globe projection
mapD3.prototype.doDrag = function (start) {
	// Adapted from http://mbostock.github.io/d3/talk/20111018/azimuthal.html and updated for d3 v3
	// need to add this one to make flat world map pan/zoom http://bl.ocks.org/patricksurry/6621971
	if (_mapD3.drag && _mapD3.isGlobe()) {
		var mouse = [d3.event.sourceEvent.pageX, d3.event.sourceEvent.pageY];
		var touch = false;
		if (d3.event.sourceEvent.changedTouches != null && d3.event.sourceEvent.changedTouches.length > 0) {
			touch = true;
			mouse = [d3.event.sourceEvent.changedTouches[0].pageX, d3.event.sourceEvent.changedTouches[0].pageY];
			//d3.event.stopPropagation();
		}

		if (start) {
			//console.log("dragstart");

			_mapD3.clickStart(true);
			_mapD3.dragmouse = mouse;
			_mapD3.dragpoint = _mapD3.d3proj.rotate();

		} else {
			//console.log("drag");
			var x = mouse[0] - _mapD3.dragmouse[0];
			var y = mouse[1] - _mapD3.dragmouse[1];
			var point = [_mapD3.dragpoint[0] + (mouse[0] - _mapD3.dragmouse[0]) / 4, _mapD3.dragpoint[1] + (_mapD3.dragmouse[1] - mouse[1]) / 4];
			/*
			if (touch) {
				var r = _mapD3.d3proj.rotate();
				if (x < 10) x = 0;
				if (y < 10) y = 0;
				r[0] += (x < 0 ? -1 : 1) * 45;
				r[1] += (y < 0 ? -1 : 1) * 45;
				_mapD3.d3proj.rotate(r);
			} else*/
			_mapD3.d3proj.rotate([point[0], point[1]]);
			_mapD3.refreshPath();
			//d3.event.stopPropagation();
		}
	}
}

// handles zoom call event when in flat projection
mapD3.prototype.doZoom = function () {
	if (_mapD3.zoom) {
		_mapD3.log("doZoom");

		// decide if we moved or need to reset
		var refresh = false;
		var reset = true;

		var p = _mapD3.d3proj;
		var s = _mapD3.viewScale;
		var t = _mapD3.viewTranslate;

		// check the resulting scale and refresh projection if in bounds
		var toS = s * d3.event.scale;
		if (toS >= _mapD3.zoomMax) {
			p.scale(toS);
			reset = false;
			refresh = true;
		}

		// check the translation if scale is valid
		if (!_mapD3.isGlobe() && toS > _mapD3.defaultScale) {
			var tx = t[0] * d3.event.scale + d3.event.translate[0];
			var ty = t[1] * d3.event.scale + d3.event.translate[1];
			p.translate([tx, ty]);
			refresh = true;
		}

		// check zoom range and refresh or reset
		if (reset) {
			_mapD3.zoomcall.scale(1);
			_mapD3.zoomcall.translate([0, 0]);

			var proj = _mapD3.getDefaultProjection();
			_mapD3.viewTranslate = proj.translate();
			if (_mapD3.isGlobe())
				proj.rotate(_mapD3.viewCenter);
			else
				_mapD3.viewCenter = proj.center();
			_mapD3.viewScale = proj.scale();
			_mapD3.d3proj = proj;

			_mapD3.refreshPath();
		} else if (refresh) {
			_mapD3.refreshPath();
		}
	}
}

// executes the optimal zoom for a given projection
mapD3.prototype.zoomTo = function (d, duration) {
	if (_mapD3.isGlobe())
		_mapD3.zoomToLevel(d, duration);
	else
		_mapD3.zoomToShape(d, duration);
}
	

// center map and zoom to a specific level
mapD3.prototype.zoomToLevel = function (d, duration) {
	_mapD3.log("zoomToLevel", d);
	if (d == null)
		_mapD3.zoomReset(duration);
	else {
		var toC = d3.geoCentroid(d);
		var toT = _mapD3.defaultTranslate;
		var toS = _mapD3.defaultScale * _mapD3.zoomStep;

		// set view, refresh path, update zoom
		_mapD3.refreshView(toT, toS, toC, (duration === undefined ? _mapD3.tweenTime : duration));
	}
}

// zoom and center to shape fill the entire window with padding
mapD3.prototype.zoomToShape = function (d, duration) {
	_mapD3.log("zoomToShape", d);
	if (d == null)
		_mapD3.zoomReset(duration);
	else {
		// resize by width and height
		var proj = _mapD3.getDefaultProjection();
		if (_mapD3.isGlobe()) proj = proj.clipAngle(180); // reset clip angle for projection
		var b = _mapD3.d3path.projection(proj).bounds(d);
		/*var bw = b[0][0] - b[1][0];
		var bh = b[0][1] - b[1][1];
		if (bw < 0) bw = -bw;
		if (bh < 0) bh = -bh;
		var mw = _mapD3.width / bw;
		var mh = _mapD3.height / bh;*/
		var zs = _mapD3.zoomGetForBounds(b);

		var toC = d3.geo.centroid(d);
		var toT = _mapD3.defaultTranslate;
		var toS = _mapD3.defaultScale * zs * _mapD3.zoomPad;
		if (_mapD3.isGlobe()) toS = toS * _mapD3.zoomPad; // double padding for globe

		// set view, refresh path, update zoom
		_mapD3.refreshView(toT, toS, toC, (duration === undefined ? _mapD3.tweenTime : duration));
	}
}

// center map and zoom to a custom projection
mapD3.prototype.zoomToCustom = function (d, duration) {
	_mapD3.log("zoomToCustom", d);

	if (d == null)
		_mapD3.zoomReset(duration);
	else {
		var c = _mapD3.geo.getContinentName(d.id);
		var b = _mapD3.geo.getContinentBounds(c);		
		var zs = _mapD3.zoomGetForBounds(b);

		var toC = d3.geo.centroid(d);
		var toT = _mapD3.defaultTranslate;
		var toS = _mapD3.defaultScale / 4 * zs;
		//if (_mapD3.isGlobe()) toS = toS * _mapD3.zoomPad; // add padding

		// set view, refresh path, update zoom
		_mapD3.refreshView(toT, toS, toC, (duration === undefined ? _mapD3.tweenTime : duration));


		/*
		// get continent name by country id
		var proj = _mapD3.geo.getContinentName(d.id);

		// resize by width and height
		var p = _mapD3.getDefaultProjection(proj);

		var toC = d3.geo.centroid(d);
		var toT = _mapD3.defaultTranslate;
		var toS = p.scale();
		if (_mapD3.isGlobe()) toS = toS * 2; // double zoom

		// set view, refresh path, update zoom
		_mapD3.refreshView(toT, toS, toC, (duration === undefined ? _mapD3.tweenTime : duration));
		*/
	}
}

// zoom and center bounds fill the entire window with padding
mapD3.prototype.zoomToBounds = function (b, duration) {
	_mapD3.log("zoomToBounds", b);
	if (b == null)
		_mapD3.zoomReset(duration);
	else {
		// resize by width and height
		/*var bw = b[0][0] - b[1][0];
		var bh = b[0][1] - b[1][1];
		if (bw < 0) bw = -bw;
		if (bh < 0) bh = -bh;
		var mw = _mapD3.width / bw;
		var mh = _mapD3.height / bh;
		var zs = (mw < mh ? mw : mh);*/
		var zs = _mapD3.zoomGetForBounds(b);
		
		var cx = (b[0][0] + b[1][0]) / 2;
		var cy = (b[0][1] + b[1][1]) / 2;

		var toC = [cx, cy];
		var toT = _mapD3.defaultTranslate;
		var toS = _mapD3.defaultScale / 4 * zs;
		if (_mapD3.isGlobe()) toS = toS * _mapD3.zoomPad; // add padding

		// set view, refresh path, update zoom
		_mapD3.refreshView(toT, toS, toC, (duration === undefined ? _mapD3.tweenTime : duration));
	}
}

// resets the zoom to the default for the current projection
mapD3.prototype.zoomReset = function (duration) {
	_mapD3.log("zoomReset");

	if (_mapD3.isGlobe()) {
		if (_mapD3.tweenTime > 0) {
			var r = _mapD3.d3proj.rotate();
			r[0] = -r[0]; r[1] = -r[1];
			_mapD3.refreshView(_mapD3.d3proj.translate(), _mapD3.defaultScale, r, (duration === undefined ? _mapD3.tweenTime : duration));
		}
		else {
			var toS = _mapD3.d3proj.scale(_mapD3.viewScale);
			_mapD3.refreshPath();
		}
	}
	else {
		if (_mapD3.tweenTime > 0) {
			var p = _mapD3.getDefaultProjection();
			_mapD3.refreshView(p.translate(), p.scale(), p.center(), (duration === undefined ? _mapD3.tweenTime : duration));
		}
		else {
			_mapD3.startProjection();
			_mapD3.refreshPath();
		}
	}
}


// calculates the desired zoom based on the bounds
mapD3.prototype.zoomGetForBounds = function (b) {
	var bw = b[0][0] - b[1][0];
	var bh = b[0][1] - b[1][1];
	if (bw < 0) bw = -bw;
	if (bh < 0) bh = -bh;
	var mw = _mapD3.width / bw;
	var mh = _mapD3.height / bh;
	return (mw < mh ? mw : mh);
}


// --------------------------------------------------
// Draw points, hexagons and hexgrid
// --------------------------------------------------

// renders points as points on map
mapD3.prototype.drawPoints = function (places) {
	//console.log("drawPoints");
	_mapD3.d3g.selectAll(".geodot").remove();

	_mapD3.places = places;
	_mapD3.points = new Array();

	for (i = 0; i < places.length; i++) {
		var p = _mapD3.d3proj([places[i].lng, places[i].lat]);
		var point = _mapD3.d3g.append("circle")
			.on("click", _mapD3.doClickCircle)
			.attr("class", "geodot")
			.attr("country", function (d) { return places[i].country; })
			.attr("cx", p[0])
			.attr("cy", p[1])
			.attr("r", 2);

		point.append("svg:title").text(places[i].name);

		var visible = _mapD3.isPlaceVisible(places[i]);
		if (!visible) point.style("opacity", 0.1);

		_mapD3.points.push(point);
	}
}

// renders places as hexabins on map
mapD3.prototype.drawHexabin = function (places) {
	//console.log("drawHexabin");

	_mapD3.places = places;
	_mapD3.d3g.selectAll(".geohex").remove();

	// get only visisble hexes
	var hexes = new Array();
	for (var i = 0; i < places.length; i++) {
		if (!_mapD3.isGlobe() || _mapD3.isPlaceVisible(places[i])) {
			var p = _mapD3.d3proj([places[i].lng, places[i].lat]);
			if (p == null) {
				places[i].x = 0;
				places[i].y = 0;
			}
			else {
				places[i].x = p[0]; //lon
				places[i].y = p[1]; //lat
			}
			hexes.push(places[i]);
		}
	}

	var radius = 10;// * _mapD3.d3scale;
	var domain = 4;// * _mapD3.d3scale;
	var range = 10;// * _mapD3.d3scale;

	_mapD3.d3hexrad = d3.scale.sqrt()
		.domain([0, domain])
		.range([0, range])
		.clamp(true);

	_mapD3.d3hexbin = d3.hexbin()
		.size([_mapD3.width, _mapD3.height])
		.radius(radius)
		.x(function (d) { return d.x; })//_mapD3.d3proj([d.lng, d.lat])[0]; })
		.y(function (d) { return d.y; });//_mapD3.d3proj([d.lng, d.lat])[1]; });


	var d3hexset = _mapD3.d3hexbin(hexes);//.sort(function (a, b) { return b.length - a.length; });
	//var max = d3.max(d3hexset, function (d) { //console.log(d); return d.length; });

	_mapD3.d3g.selectAll(".geohex")
		.data(d3hexset)
		.enter().append("path")
		//.on("click", _mapD3.doClick)
		.on("mouseout", _mapD3.onMouseOut)
		.on("mouseover", _mapD3.onMouseOver)
		.attr("class", "geohex")
		.attr("transform", function (d) {
			// log center point for easy transformation
			d.center = _mapD3.d3proj.invert([d.x, d.y]);
			return "translate(" + d.x + "," + d.y + ")";
		})
		.attr("d", function (d) { return _mapD3.d3hexbin.hexagon(_mapD3.d3hexrad(d.length)); })
		.append("svg:title").text(function (d) {
			var text = "";
			for (var j = 0; j < d.length; j++) {
				text += d[j].name + "\r\n";
			}
			return text;
		});


	_mapD3.hexabin = d3hexset;
};




mapD3.prototype.drawHexgrid = function (places) {
	//console.log("drawHexgrid START = " + _mapD3.clickStart(true));
	var radius = 5;// * _mapD3.d3scale;
	var range = 12;

	var hexes = new Array();
	var h = Math.sqrt(3) * radius;
	var w = 2 * radius;
	var s = 3 / w;

	var sx = w;
	var sy = h;
	var ex = _mapD3.width - w;
	var ey = _mapD3.height - h;

	var rows = 0;
	var dx = 0;
	for (var y = sy - h; y < (ey + h) ; y += h) {
		for (var x = sx - w; x < (ex + w) ; x += w) {
			var d = new Array();
			d[0] = x + dx;
			d[1] = y;
			d.name = "";
			hexes.push(d);
			//console.log(x + "," + y);
			/*
			var off = $("#mapD3").offset();
			var e = document.elementFromPoint(d[0] + off.left, d[1] + off.top);
			if (e != null) {
				//console.log(e);
				if (e.nodeName != "rect") hexes.push(d);

				if (e.nodeName == "rect") {
					_mapD3.d3svg.append("circle")
				   .attr("class", "geodot")
				   .attr("cx", d[0])
				   .attr("cy", d[1])
				   .attr("r", _mapD3.d3scale * 1.5);
				}
			}*/


		}
		rows++;
		if ((rows % 2) == 1) {
			dx = h / 2;
		}
		else {
			dx = 0;
		}
		//console.log(rows);
	}
	//console.log("drawHexgrid COPIED = " + _mapD3.clickStart());

	places.forEach(function (d) {
		var p = _mapD3.d3proj([d.lng, d.lat]);
		if (p != null) {
			d[0] = p[0]; //lon
			d[1] = p[1]; //lat
			hexes.push(d);
		}
	});
	//console.log("drawHexgrid PROJECTED = " + _mapD3.clickStart());

	d3hexrad = d3.scale.linear()
		.domain([0, radius])
		.range([0, range]);

	d3hexbin = d3.hexbin()
		//.size([_mapD3.width, _mapD3.height])
		.radius(radius * 1.155);
	//console.log("drawHexgrid SETUP = " + _mapD3.clickStart());

	var d3hexset = d3hexbin(hexes).sort(function (a, b) { return b.length - a.length; });
	//console.log("drawHexgrid SORTED = " + _mapD3.clickStart());

	var color = d3.scale.linear().domain([1, 3]).range([0, 15]).clamp(true);

	_mapD3.d3g.selectAll(".geogrid")//.remove()
		.data(d3hexset)
		.enter().append("path")
		//.on("click", _mapD3.onClickHex)
		.attr("d", function (d) { return d3hexbin.hexagon(); })//d3hexbin.hexagon(d3hexrad(d.length)); })
		.attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; })
		.attr("class", "geogrid")
		//.attr("stroke-width", "1px")
		.attr("fill", "#f00")
		.attr("opacity", function (d) {
			if (d.length == 1)
				return "0.1";
			else {
				return "1";
			}
		})
		.attr("fill-rule", "evenodd")
		.style("fill", function (d) {
			if (d.length == 1)
				return "none";
			else {
				var c = color(d.length);
				var i = Math.round(Number(c), 0);
				//var h = String.fromCharCode(97 + i - 1);
				var h = i.toString();
				if (i == 10)
					h = "a";
				else if (i == 11)
					h = "b";
				else if (i == 12)
					h = "c";
				else if (i == 13)
					h = "d";
				else if (i == 14)
					h = "e";
				else if (i == 15)
					h = "f";
				return "#00" + h;
			}
		})
		.append("svg:title").text(function (d) {
			var text = "";
			for (var j = 0; j < d.length; j++) {
				var name = d[j].name;
				if (name.length > 0) text += name + "\r\n";
			}
			return text;
		});

	//console.log("drawHexgrid RENDERED = " + _mapD3.clickStart());
};







// ***********************************************************************************************************************************








mapD3.prototype.onDone = function () {
}




// --------------------------------------------------
// Feature Event Handlers
// --------------------------------------------------

// returns true if the place is in the current view port
mapD3.prototype.isPlaceVisible = function (place) {
	if (_mapD3.isGlobe())
		return (_mapD3.geo.calculateDistance(-_mapD3.viewCenter[0], -_mapD3.viewCenter[1], place.lng, place.lat) <= 10000);
	else
		return true; // assume true for world perspective
}

// gets the specified feature given an id
mapD3.prototype.getShapeByID = function (id) {
	return _mapD3.features().filter(function (d) { return d.id == id; })[0];
}

// sets the desired css style on the shape by id
mapD3.prototype.selectShape = function (id) {
	_mapD3.d3g.selectAll(".geoshape").each(function (d) {
		var p = d3.select(this);
		_mapD3.setCssStyle(p, "s", d.id == id);
	});
}

mapD3.prototype.hoverShape = function (id) {
	_mapD3.d3g.selectAll(".geoshape").each(function (d) {
		var p = d3.select(this);
		_mapD3.setCssStyle(p, "h", d.id == id);
	});
}


// gets the css class based on input parameters
mapD3.prototype.getCssStyle = function (flags) {
	if (flags > 0)
		return "visited";
	else
		return "unknown";
}

// sets the desired css style on the shape instance
mapD3.prototype.setCssStyle = function (p, state, on) {
	p.classed("unknown" + state, on && p.classed("unknown"));
	p.classed("visited" + state, on && p.classed("visited"));
	p.classed("planned" + state, on && p.classed("planned"));
}

// sets the desired css style on the shape by id
mapD3.prototype.setCssStyleByID = function (id, state, on) {
	_mapD3.d3g.selectAll(".geoshape").each(function (d) {
		//console.log(d.id);
		if (d.id == id) {
			var p = d3.select(this);
			_mapD3.setCssStyle(p, state, on);
		}
	});
}

// styles geo shape objects when added to svg
mapD3.prototype.onStyle = function (p, d) {
	if (_mapD3.topo == "america" && mapD3.prototype.onStyleAmerica)
		_mapD3.onStyleAmerica(p, d);
	else if (_mapD3.topo == "oceania" && mapD3.prototype.onStyleOceania)
		_mapD3.onStyleOceania(p, d);
	else if (_mapD3.topo == "world")
		_mapD3.onStyleCountry(p, d);
	else if (mapD3.prototype.onStyleRegion)
		_mapD3.onStyleRegion(p, d);
};

// default style for country shapes
mapD3.prototype.onStyleCountry = function (p, d) {
	var c = _mapD3.geo.getCountry(d.id);
	p.append("svg:title").text(c.name);

	if (_mapD3.geo.isVisitedCountry(c.id))
		p.classed("visited", true);
	else if (_mapD3.geo.isPlannedCountry(c.id))
		p.classed("planned", true);
	else
		p.classed("unknown", true);
};

// special styles for united states
mapD3.prototype.onStyleAmerica = function (p, d) {
	var r = _mapD3.geo.getRegion(d.id);
	p.classed(_mapD3.getCssStyle(r.flags), true);
	p.append("svg:title").text(r.name);
}

// special styles for austrila and new zealand
mapD3.prototype.onStyleOceania = function (p, d) {
	var name = "";
	if (d.id == "NZN")
		name = "North Island";
	else if (d.id == "NZS")
		name = "South Island";
	else if (d.id == "AUZ")
		name = "Australia";
	else if (d.id == "AUA")
		name = "Tasmania";
	p.classed("visited", true);
	p.append("svg:title").text(name);
}


// --------------------------------------------------
// Mouse Event Handlers
// --------------------------------------------------

// starts the time for click events
mapD3.prototype.clickStart = function (start) {
	var now = new Date().getTime();
	//console.log("clickStart(" + now + ")");
	if (start) this.eventstart = now;
	return (now - this.eventstart);
}

mapD3.prototype.onClick = function (d) {

}

// click event handler for shapes and base layer
mapD3.prototype.doClick = function (d) {
	//console.log("doClick(" + d + ")");
	if (!_mapD3.drag || !_mapD3.zoom || _mapD3.clickStart() < _mapD3.dragDelay) {
		_mapD3.onClick(d);
	}

	// hacky fix for default svg click bubble in firefox
	if (!(d == null || d === undefined)) d3.event.stopPropagation();
}

// click event handler for points as circles
mapD3.prototype.doClickCircle = function () {
	var c = $(this).attr("country");
	if (c != null && c != "") {
		var d = _mapD3.getShapeByID(Number(c));
		//console.log(d);
		_mapD3.doClick(d);
	}
}

mapD3.prototype.doClickHex = function () {
	//console.log("doClickHex");
	var d = $(this);
	_mapD3.doClick(d);
}




// default mouse over for geoshape
mapD3.prototype.onMouseOver = function (d) {
	var id = d.id;
	if (d.length > 0) {		
		_mapD3.onHoverHexabin(d);
	} else if (id > 0 && id < 1000) {
		var p = d3.select(this);
		if (_mapD3.hoverPrev != null && p != _mapD3.hoverPrev) {
			_mapD3.setCssStyle(_mapD3.hoverPrev, "h", 0);
		}
		_mapD3.setCssStyle(p, "h", 1);
		_mapD3.onHoverCountry(id, true);
		_mapD3.hoverPrev = p;
	}
}

// default mouse out for geoshape
mapD3.prototype.onMouseOut = function (d) {
	if (d.length > 0) {
		_mapD3.onHoverHexabin(d);
	}
}

mapD3.prototype.onHoverHexabin = function (d) {
}

mapD3.prototype.onClickHex = function (d) {
	//console.log("onClickHex");
}

mapD3.prototype.onSelectPosition = function (lat, lng, country, places) {
}

// special click handler for when a country is selected
mapD3.prototype.onSelectCountry = function (id) {
}

// special click handler for when a country is selected
mapD3.prototype.onHoverCountry = function (id, on) {
}






// --------------------------------------------------
// Internal Helper Methods
// --------------------------------------------------

// has the start method been called yet
mapD3.prototype.isStarted = function () {
	return _mapD3.geo != null;
}

// easy way to check the topo file
mapD3.prototype.isWorld = function () {
	return _mapD3.proj == "world";
}

// easy way to check the projection
mapD3.prototype.isGlobe = function () {
	return _mapD3.proj == "globe";
}

// easy way to check the projection
mapD3.prototype.isAmerica = function () {
	return _mapD3.topo == "america";
}

// returns the safe topo name from topo or proj
mapD3.prototype.getTopoName = function (proj) {
	switch (proj) {
		case "us":
		case "840":
		case "america":
			return "america";
		case "au":
		case "36":
		case "nz":
		case "554":
		case "oceania":
			return "oceania";
		default:
			return "world";
	}
}

// returns the safe projection name from map or proj
mapD3.prototype.getProjName = function (map) {
	switch (map.toString().toLowerCase()) {
		case "world":
		case "globe":
		case "asia":
		case "north":
		case "south":
		case "europe":
		case "africa":
		case "oceania":
			return map.toLowerCase();
		case "au":
		case "36":
			return "au";
		case "nz":
		case "554":
			return "nz";
		case "ru":
		case "643":
		case "russia":
			return "ru";
		case "us":
		case "usa":
		case "840":
		case "america":
		case "united states":
			return "us";
			break;
		default:
			return "world";
	}
}

// maps topo name to cache index
mapD3.prototype.getCacheKey = function () {
	switch (_mapD3.topo) {
		case "america":
			return 1;
		case "oceania":
			return 2;
		default:
			return 0;
	}
}

// gets/sets current shapes for topo in cache
mapD3.prototype.data = function (set) {
	if (set == null)
		return _mapD3.cacheData[_mapD3.getCacheKey()];
	else
		_mapD3.cacheData[_mapD3.getCacheKey()] = set;
}

// gets/sets current shapes for topo in cache
mapD3.prototype.shapes = function (set) {
	if (set == null)
		return _mapD3.cacheShapes[_mapD3.getCacheKey()];
	else
		_mapD3.cacheShapes[_mapD3.getCacheKey()] = set;
}

// gets/sets current features for topo in cache
mapD3.prototype.features = function (set) {
	if (set == null)
		return _mapD3.cacheFeatures[_mapD3.getCacheKey()];
	else
		_mapD3.cacheFeatures[_mapD3.getCacheKey()] = set;
}

// shows an error message regardless of debug mode
mapD3.prototype.error = function (msg) {
	//console.log("mapD3.ERROR." + msg);
}

// logs to console if debugging enabled
mapD3.prototype.log = function (msg, obj) {
	if (_mapD3.debug) {
		if (obj === undefined)
			console.log("mapD3." + msg);
		else
			console.log("mapD3." + msg + "(" + obj + ")");
	}
}