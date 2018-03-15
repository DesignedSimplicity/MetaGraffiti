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

	$(".js-check-dirty").click(function (e) {
		if (_dirty) {
			e.preventDefault();
			if (confirm("Discard changes?")) window.location = $(this).attr("href");
		}
	});

});