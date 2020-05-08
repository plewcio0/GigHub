var GigsController = function () {
    var button;

    var init = function () {
        $('.js-toggle-attendance').click(toggleAttendance);
    };


    var toggleAttendance = function (e) {
        button = $(e.target);
        if (button.hasClass("btn-default")) {
            createAttendance();
        }
        else {
            deleteAttendance();
        }

    };

    var createAttendance = function () {
        $.post("/api/attendances", { gigId: button.attr("data-gig-id") })
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function () {
        $.ajax({
            url: "/api/attendances/" + button.attr("data-gig-id"),
            method: "delete"
        })
            .done(done)
            .fail(fail);
    };
    var done = function () {
        var text = (button.text() == "Going" ? "Going?" : "Going");
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
        if (text == "Going")
            alertify.success('You are going to this event.').dismissOthers();
        else
            alertify.warning('You are not going to this event.').dismissOthers();
    }
    var fail = function (xhr) {
        alertify.alert(xhr.responseJSON.message);
    };

    return {
        init: init
    }
}();
