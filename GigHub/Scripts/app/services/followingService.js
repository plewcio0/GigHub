var FollowingService = function () {

    var follow = function (id, done, fail) {
        $.post("/api/followings", { followeeId: id })
            .done(done)
            .fail(fail)
    }

    var unFollow = function (id, done, fail) {
        $.ajax({
            url: "/api/followings/" + id,
            method: "delete"
        })
            .done(done)
            .fail(fail)
    }

    return {
        follow: follow,
        unFollow: unFollow
    }

}();