$(document).ready(function () {

	$(".js-confirm-delete").click(function (e) {
		e.preventDefault();
		if (confirm("Delete?")) {
			window.location = $(this).attr("href");
		}
	});

});