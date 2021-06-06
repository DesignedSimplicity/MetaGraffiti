var _dirty = false;

function initDirty() {
	$("input, textarea").change(function () {
		_dirty = true;
	});
}

$(document).ready(function () {
	$(".js-confirm").click(function (e) {
		e.preventDefault();
		var text = $(this).text();
		if (confirm(text + "?")) {
			window.location = $(this).attr("href");
		}
	});

	$(".js-confirm-dirty").click(function (e) {
		if (_dirty) {
			e.preventDefault();
			if (confirm("Discard changes?")) window.location = $(this).attr("href");
		}
	});

});



function initActionRows(hover, click) {
	$(".js-mappable-row").each(function (index) {
		var color = getMapColor(index);
		$(this).children().eq(0).css('background-color', color);
	});

	if (hover) $(".js-mappable-row").hover(hover);
	if (click) $(".js-mappable-row").click(click);
}


// ==================================================
// Colors


var _mapColors = [
	'#007bff',
	'#28a745',
	'#ffc107',
	'#17a2b8',
	'#dc3545',
];

var _infoColors = [
	'#b8daff',
	'#c3e6cb',
	'#ffeeba',
	'#bee5eb',
	'#f5c6cb',
];

// returns a color to use for a given index
function getMapColor(index) {
	if (index === undefined) return '#ff0000';
	return _mapColors[index % 5];
}

// returns a lighter color to use for a given index
function getInfoColor(index) {
	if (index === undefined) return _infoColors[0];
	return _infoColors[index % 5];
}