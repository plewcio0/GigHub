var FollowingsController = function (followingService) {
    var button;
    var init = function () {
        $('.js-toggle-follow').click(toggleFollowing);
    };

    var toggleFollowing = function (e) {
        button = $(e.target);
        var id = button.attr("data-user-id")
        if (button.hasClass("btn-default")) {
            followingService.follow(id, done, fail);
        } else {
            followingService.unFollow(id, done, fail);
        };
    };

    var done = function () {
        var text = (button.text().includes("Follow")) ? "Unfollow" : "Follow";
        button.toggleClass("btn-default").toggleClass("btn-info").text(text);
        if (text == "Follow")
            alertify.warning('You are not following this artist.').dismissOthers();
        else
            alertify.success('You are following this artist.').dismissOthers();
    }

    var fail = function (xhr) {
        alertify.alert(xhr.responseJSON);
    }


    return {
        init: init
    };

}(FollowingService);