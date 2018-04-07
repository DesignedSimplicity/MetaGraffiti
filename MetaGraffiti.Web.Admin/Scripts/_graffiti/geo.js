// looks up a region given the point and updates the field
// TODO: use the field value for search if no point is specified
function lookupRegion(field, point) {
	$(field).val("...").addClass("is-invalid").removeClass("is-valid");
	$.getJSON("/api/regions/point/?lat=" + point.lat + "&lng=" + point.lng)
		.done(function (data) {
			if (data.length == 1)
				$(field).val(data[0].RegionName).addClass("is-valid").removeClass("is-invalid");
			else {
				var regions = "";
				for (var i = 0; i < data.length; i++) {
					if (regions.length > 0) regions += " | ";
					regions += data[i].RegionName;
				}
				$(field).val(regions).addClass("is-invalid").removeClass("is-valid");
			}
		})
		.fail(function (jqXHR, textStatus, err) {
			$(field).val("").addClass("is-invalid").removeClass("is-valid");
		});
}


// looks up a timezone given the point and updates the field
// TODO: use the field value for search if no point is specified
function lookupTimezone(field, point) {
	$(field).val("...").addClass("is-invalid").removeClass("is-valid");
	$.getJSON("/api/timezones/point/?lat=" + point.lat + "&lng=" + point.lng)
		.done(function (data) {
			processTimezone(field, data);
		})
		.fail(function (jqXHR, textStatus, err) {
			processTimezone(field);
		});
}

function lookupTimezoneByLocation(field, name) {
	$(field).val("...").addClass("is-invalid").removeClass("is-valid");
	$.getJSON("/api/timezones/location/?name=" + name)
		.done(function (data) {
			processTimezone(field, data);
		})
		.fail(function (jqXHR, textStatus, err) {
			processTimezone(field);
		});
}

function processTimezone(field, data) {
	if (data)
		$(field).val(data.tzid).addClass("is-valid").removeClass("is-invalid");
	else
		$(field).val("").addClass("is-invalid").removeClass("is-valid");
}