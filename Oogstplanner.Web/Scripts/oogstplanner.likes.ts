/// <reference path="typings/jquery.d.ts" />

module Liker {
    
    export function like(calendarId : number) {
        var that = this;
        $.post('/zaaikalender/like', { calendarId: calendarId })
        .done(function(data) { 
            if (data.wasUnlike) {
                Notification.informational("Unliken succesvol.", "U vindt de zaaikalender niet leuk.");
            }
            else {
                Notification.informational("Liken succesvol.", "U vindt de zaaikalender leuk.");
            }
            Liker.updateAmountOfLikes(calendarId);
        })
        .then(() => { Liker.makeTextPluralWhenLikesNot1() })
        .fail(() => { Notification.error(); });
    }

    export function updateAmountOfLikes(calendarId : number) {
        Liker.getLikes(calendarId).done((count : number) =>( $('#amount-of-likes').text(count)))
        .then(() => { Liker.makeTextPluralWhenLikesNot1() })
        .fail(() => { Notification.error(); });
    }

    export function showUserList(calendarId : number) {

        Liker.getUserList(calendarId).done((users) => {
            for (var i = 0; i < users.length; i++) {
                alert(users[i]);
            };
        })
        .fail(() => { Notification.error(); });
    }

    export function getLikes(calendarId : number) : JQueryPromise<number> {
        return $.get('/zaaikalender/' + calendarId + '/aantal-likes');
    }

    export function getUserList(calendarId : number) : JQueryPromise<string[]> {

        return $.get('/zaaikalender/' + calendarId + '/gebruikers-die-liken');
    }

    export function makeTextPluralWhenLikesNot1() {

        var peopleSingleOrPlural : string;
        var verbSingleOrPlural : string;

        if (parseInt( $('#amount-of-likes').text() ) == 1) {
            peopleSingleOrPlural = 'iemand';
            verbSingleOrPlural = 'vindt';
        } else {
            peopleSingleOrPlural = 'mensen';
            verbSingleOrPlural = 'vinden';
        }

        $('#people-single-or-plural').text(peopleSingleOrPlural);  
        $('#people-single-or-plural-verb').text(verbSingleOrPlural);  
    }
}

$(function() {

    Liker.makeTextPluralWhenLikesNot1();

    $(".like").on("click", function () {
        var calendarId : number = $(this).data('calendar-id');
        Liker.like(calendarId);
    });

    $(".user-likes").on("click", function () {
        var calendarId : number = $(this).data('calendar-id');
        Liker.showUserList(calendarId);
    });

});
