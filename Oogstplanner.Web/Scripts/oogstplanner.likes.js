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
            Liker.makeLinkUnclickableWhenZeroLikes();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.like = like;
    function updateAmountOfLikes(calendarId) {
        Liker.getLikes(calendarId).done(function (count) { return ($('#amount-of-likes').text(count)); }).then(function () {
            Liker.makeTextPluralWhenLikesNot1();
            Liker.makeLinkUnclickableWhenZeroLikes();
        }).fail(function () {
            Notification.error();
        });
    }
    Liker.updateAmountOfLikes = updateAmountOfLikes;
    function getLikes(calendarId) {
        return $.get('/zaaikalender/' + calendarId + '/aantal-likes');
    }
    Liker.getLikes = getLikes;
    function makeLinkUnclickableWhenZeroLikes() {
        if (parseInt($('#amount-of-likes').text()) == 0) {
            $('.user-likes').addClass('disabled');
            Liker.disablePopover();
        }
        else {
            $('.user-likes').removeClass('disabled');
            Liker.enablePopover();
        }
    }
    Liker.makeLinkUnclickableWhenZeroLikes = makeLinkUnclickableWhenZeroLikes;
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
    function enablePopover() {
        $('.user-likes').webuiPopover({
            type: 'async',
            url: '/zaaikalender/' + $('.user-likes').data('calendar-id') + '/gebruikers-die-liken',
            title: '<span class="dark"> Mensen die dit leuk vinden<\/span>',
            cache: false,
            closeable: true,
            content: function (users) {
                var html = '<ul>';
                for (var i = 0; i < users.length; i++) {
                    var name = users[i];
                    html += "<li><a href='/gebruiker/" + name + "'>" + name + "</a></li>";
                }
                html += '</ul>';
                return html;
            }
        });
    }
    Liker.enablePopover = enablePopover;
    function disablePopover() {
        $('.user-likes').webuiPopover('destroy');
    }
    Liker.disablePopover = disablePopover;
})(Liker || (Liker = {}));
$(function () {
    Liker.makeTextPluralWhenLikesNot1();
    var calendarId = $('.user-likes').data('calendar-id');
    $('.like').on('click', function () {
        Liker.like(calendarId);
    });
    Liker.enablePopover();
    Liker.makeLinkUnclickableWhenZeroLikes();
});
//# sourceMappingURL=oogstplanner.likes.js.map