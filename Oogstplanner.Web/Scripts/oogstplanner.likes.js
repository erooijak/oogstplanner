var Liker;
(function (Liker) {
    function like(calendarId) {
        var that = this;
        $.post('/zaaikalender/like', { calendarId: calendarId }).done(function () {
            Notification.informational("Liken succesvol.", "U vindt de zaaikalender leuk.");
            Liker.updateAmountOfLikes(calendarId);
        }).then(Liker.makeLikesTextPluralWhenNot1()).fail(function () {
            Notification.error();
        });
    }
    Liker.like = like;
    function unLike(calendarId) {
        var that = this;
        $.post('/zaaikalender/unlike', { calendarId: calendarId }).done(function () {
            Notification.informational("Unliken succesvol.", "U vindt de zaaikalender niet leuk.");
            Liker.updateAmountOfLikes(calendarId);
            Liker.makeLikesTextPluralWhenNot1();
        }).then(function () {
            Liker.makeLikesTextPluralWhenNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.unLike = unLike;
    function updateAmountOfLikes(calendarId) {
        $.get('/zaaikalender/' + calendarId + '/aantallikes', function (count) {
            $('#amount-of-likes').text(count);
        }).then(function () {
            Liker.makeLikesTextPluralWhenNot1();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.updateAmountOfLikes = updateAmountOfLikes;
    function makeLikesTextPluralWhenNot1() {
        var text = parseInt($('#amount-of-likes').text()) == 1 ? 'like' : 'likes';
        $('#likes-verb').text(text);
    }
    Liker.makeLikesTextPluralWhenNot1 = makeLikesTextPluralWhenNot1;
})(Liker || (Liker = {}));
$(function () {
    Liker.makeLikesTextPluralWhenNot1();
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