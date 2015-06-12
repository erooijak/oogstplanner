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
        $.get('/zaaikalender/' + calendarId + '/aantallikes', function (count) {
            $('#amount-of-likes').text(count);
        })
        .then(function () { Liker.makeTextPluralWhenLikesNot1() })
        .fail(function() { Notification.error(); });
    }

    export function makeTextPluralWhenLikesNot1() {
        var text = parseInt( $('#amount-of-likes').text() ) == 1 ? 'iemand vindt' : 'mensen vinden';
        $('#people').text( text );  
    }
}

$(function() {

    Liker.makeTextPluralWhenLikesNot1();

    $(".like").on("click",function (e) {
        var calendarId : number = $(this).data('calendar-id');
        Liker.like(calendarId);
    });

});
