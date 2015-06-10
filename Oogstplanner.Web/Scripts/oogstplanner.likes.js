var Liker;
(function (Liker) {
    function like(calendarId) {
        var that = this;
        $.post('/zaaikalender/like', { calendarId: calendarId }).done(function () {
            Notification.informational("Liken succesvol.", "U vindt de zaaikalender leuk.");
            Liker.updateAmountOfLikes(calendarId);
        }).then(function () {
            Liker.makeTextPluralWhenLikesNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.like = like;
    function unLike(calendarId) {
        var that = this;
        $.post('/zaaikalender/unlike', { calendarId: calendarId }).done(function () {
            Notification.informational("Unliken succesvol.", "U vindt de zaaikalender niet leuk.");
            Liker.updateAmountOfLikes(calendarId);
            Liker.makeTextPluralWhenLikesNot1();
        }).then(function () {
            Liker.makeTextPluralWhenLikesNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.unLike = unLike;
    function updateAmountOfLikes(calendarId) {
        $.get('/zaaikalender/' + calendarId + '/aantallikes', function (count) {
            $('#amount-of-likes').text(count);
        }).then(function () {
            Liker.makeTextPluralWhenLikesNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.updateAmountOfLikes = updateAmountOfLikes;
    function makeTextPluralWhenLikesNot1() {
        var text = parseInt($('#amount-of-likes').text()) == 1 ? 'iemand vindt' : 'mensen vinden';
        $('#people').text(text);
    }
    Liker.makeTextPluralWhenLikesNot1 = makeTextPluralWhenLikesNot1;
})(Liker || (Liker = {}));
$(function () {
    Liker.makeTextPluralWhenLikesNot1();
    $(".like").on("click", function (e) {
        var calendarId = $(this).data('calendar-id');
        Liker.like(calendarId);
    });
    $(".unlike").on("click", function (e) {
        var calendarId = $(this).data('calendar-id');
        Liker.unLike(calendarId);
    });
});
//# sourceMappingURL=oogstplanner.likes.js.map