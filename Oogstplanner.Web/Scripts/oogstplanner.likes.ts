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
        .then(function () { Liker.makeTextPluralWhenLikesNot1() })
        .fail(function() { Notification.error(); });
    }

    export function updateAmountOfLikes(calendarId : number) {
        $.get('/zaaikalender/' + calendarId + '/aantal-likes', function (count) {
            $('#amount-of-likes').text(count);
        })
        .then(function () { Liker.makeTextPluralWhenLikesNot1() })
        .fail(function() { Notification.error(); });
    }

    export function showUserList(calendarId : number) {
        
        // TODO: Seperate command and query (more places, find out how the scope works.
        $.get('/zaaikalender/' + calendarId + '/gebruikers-die-liken', function (users : string[]) {
            for (var i = 0; i < users.length; i++) {
                alert(users[i]);
            };
        })
        .fail(function() { Notification.error(); });
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

    $(".like").on("click", function (e) {
        var calendarId : number = $(this).data('calendar-id');
        Liker.like(calendarId);
    });

    $(".user-likes").on("click", function (e) {
        var calendarId : number = $(this).data('calendar-id');
        Liker.showUserList(calendarId);
    });

});
