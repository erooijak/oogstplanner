/// <reference path="typings/jquery.d.ts" />

module Liker {
    
    export function like(calendarId : number) {
        var that = this;
        $.post('/zaaikalender/like', { calendarId: calendarId })
        .done(function() { 
            Notification.informational("Liken succesvol.", "U vindt de zaaikalender leuk.");
            Liker.updateAmountOfLikes(calendarId);
        })
        .then(Liker.makeLikesTextPluralWhenNot1())
        .fail(function() { Notification.error(); });
    }

    export function unLike(calendarId : number) {
        var that = this;
        $.post('/zaaikalender/unlike', { calendarId: calendarId })
        .done(function () { 
            Notification.informational("Unliken succesvol.", "U vindt de zaaikalender niet leuk.");
            Liker.updateAmountOfLikes(calendarId);
            Liker.makeLikesTextPluralWhenNot1();
        })
        .then(function () { Liker.makeLikesTextPluralWhenNot1() })
        .fail(function() { Notification.error(); });
    }

    export function updateAmountOfLikes(calendarId : number) {
        $.get('/zaaikalender/' + calendarId + '/aantallikes', function (count) {
            $('#amount-of-likes').text(count);
        })
        .then(function () { Liker.makeLikesTextPluralWhenNot1() })
        .fail(function() { Notification.error(); });
    }

    export function makeLikesTextPluralWhenNot1() {
        var text = parseInt( $('#amount-of-likes').text() ) == 1 ? 'like' : 'likes';
        $('#likes-verb').text( text );  
    }
}

$(function() {

    Liker.makeLikesTextPluralWhenNot1();

    $(".like").on("click",function (e) {
        var calendarId : number = $(this).data('calendar-id');
        Liker.like(calendarId);
    });
    $(".unlike").on("click",function (e) {
        var calendarId : number = $(this).data('calendar-id');
        Liker.unLike(calendarId);
    });
});
