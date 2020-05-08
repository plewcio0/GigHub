var AttendanceService = function () {

    var createAttendance = function (id, done, fail) {
        $.post("/api/attendances", { gigId: id })
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

