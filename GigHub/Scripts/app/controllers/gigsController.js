var GigsController = function (attendanceService) {
    var button;

    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
     };


    var toggleAttendance = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default")) {
            attendanceService.createAttendance(gigId, done, fail);
        }
        else {
            attendanceService.deleteAttendance(gigId, done, fail);
        }

    };


    var done = function () {
        var text = (button.text().includes("Going?")) ? "Going" : "Going?";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
        if (text == "Going?")
            alertify.warning('You are not going to this event.').dismissOthers();
        else
            alertify.success('You are going to this event.').dismissOthers();
    }
    var fail = function (xhr) {
        alertify.alert(xhr.responseJSON);
    };

    return {
        init: init
    }
}(AttendanceService);
