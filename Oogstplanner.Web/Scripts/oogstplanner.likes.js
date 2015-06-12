var Liker;
(function (Liker) {
    function like(calendarId) {
        var that = this;
        $.post('/zaaikalender/like', { calendarId: calendarId }).done(function (data) {
            if (data.wasUnlike) {
                Notification.informational("Unliken succesvol.", "U vindt de zaaikalender niet leuk.");
            }
            else {
                Notification.informational("Liken succesvol.", "U vindt de zaaikalender leuk.");
            }
            Liker.updateAmountOfLikes(calendarId);
        }).then(function () {
            Liker.makeTextPluralWhenLikesNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.like = like;
    function updateAmountOfLikes(calendarId) {
        $.get('/zaaikalender/' + calendarId + '/aantal-likes', function (count) {
            $('#amount-of-likes').text(count);
        }).then(function () {
            Liker.makeTextPluralWhenLikesNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.updateAmountOfLikes = updateAmountOfLikes;
    function showUserList(calendarId) {
        $.get('/zaaikalender/' + calendarId + '/gebruikers-die-liken', function (users) {
            for (var i = 0; i < users.length; i++) {
                alert(users[i]);
            }
            ;
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.showUserList = showUserList;
    function makeTextPluralWhenLikesNot1() {
        var peopleSingleOrPlural;
        var verbSingleOrPlural;
        if (parseInt($('#amount-of-likes').text()) == 1) {
            peopleSingleOrPlural = 'iemand';
            verbSingleOrPlural = 'vindt';
        }
        else {
            peopleSingleOrPlural = 'mensen';
            verbSingleOrPlural = 'vinden';
        }
        $('#people-single-or-plural').text(peopleSingleOrPlural);
        $('#people-single-or-plural-verb').text(verbSingleOrPlural);
    }
    Liker.makeTextPluralWhenLikesNot1 = makeTextPluralWhenLikesNot1;
})(Liker || (Liker = {}));
$(function () {
    Liker.makeTextPluralWhenLikesNot1();
    $(".like").on("click", function (e) {
        var calendarId = $(this).data('calendar-id');
        Liker.like(calendarId);
    });
    $(".user-likes").on("click", function (e) {
        var calendarId = $(this).data('calendar-id');
        Liker.showUserList(calendarId);
    });
});
//# sourceMappingURL=oogstplanner.likes.js.map