var AttendanceService = function () {

    var createAttendance = function (id, done, fail) {
        $.post("/api/attendances", { gigId:  id})
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function (id, done, fail) {
        $.ajax({
            url: "/api/attendances/" + id,
            method: "delete"
        })
            .done(done)
            .fail(fail);
    };
    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendance
    };
}();

var GigsController = function (attendanceService) {
    var button;

    var init = function () {
        $('.js-toggle-attendance').click(toggleAttendance);
    };


    var toggleAttendance = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default")) {
            attendanceService.createAttendance(gigId,done,fail);
        }
        else {
            attendanceService.deleteAttendance(gigId, done, fail);
        }

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
}(AttendanceService);
