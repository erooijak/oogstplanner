/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jquery.plugins.d.ts" />
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
        .then(() => { Liker.makeTextPluralWhenLikesNot1(); Liker.makeLinkUnclickableWhenZeroLikes(); })
        .fail(() => { Notification.error(); });
    }

    export function updateAmountOfLikes(calendarId : number) {
        Liker.getLikes(calendarId).done((count : number) =>( $('#amount-of-likes').text(count)))
        .then(() => { Liker.makeTextPluralWhenLikesNot1(); Liker.makeLinkUnclickableWhenZeroLikes(); })
        .fail(() => { Notification.error(); });
    }

    export function getLikes(calendarId : number) : JQueryPromise<number> {
        return $.get('/zaaikalender/' + calendarId + '/aantal-likes');
    }

    export function makeLinkUnclickableWhenZeroLikes() {
        if (parseInt( $('#amount-of-likes').text() ) == 0) {
            $('.user-likes').addClass('disabled');
            Liker.disablePopover();
        } else {
            $('.user-likes').removeClass('disabled');
            Liker.enablePopover();
        }
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

    export function enablePopover() {
        $('.user-likes').webuiPopover({   
            type: 'async',
            url: '/zaaikalender/' + $('.user-likes').data('calendar-id') + '/gebruikers-die-liken',
            title: '<span class="dark"> Mensen die dit leuk vinden<\/span>',
            cache: false,
            closeable: true,
            content: function(users){
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

    export function disablePopover() {
        $('.user-likes').webuiPopover('destroy');
    }
}

$(function() {

    Liker.makeTextPluralWhenLikesNot1();

    var calendarId : number = $('.user-likes').data('calendar-id');

    $('.like').on('click', function () {
        Liker.like(calendarId);
    });

    Liker.enablePopover();
    Liker.makeLinkUnclickableWhenZeroLikes();

});
