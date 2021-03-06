var Notification;
(function (Notification) {
    function informational(header, body) {
        noty({
            text: '<h3>' + header + '</h3><br/><br/><h4>' + body + '</h4>',
            type: 'information',
            layout: 'topCenter',
            timeout: 8000,
            animation: {
                open: 'animated flipInX',
                close: 'animated flipOutX'
            }
        });
    }
    Notification.informational = informational;
    function error() {
        noty({
            text: '<h3>Er is iets fout gegaan...</h3>',
            type: 'error',
            layout: 'topCenter',
            timeout: 8000,
            animation: {
                open: 'animated wobble',
                close: 'animated bounceOut'
            }
        });
    }
    Notification.error = error;
    function confirmation(message, successCallback) {
        noty({
            text: '<h3>' + message + '</h3>',
            type: 'confirm',
            layout: 'topCenter',
            timeout: false,
            animation: {
                open: 'animated flipInX',
                close: 'animated flipOutX'
            },
            buttons: [
                { addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                    successCallback();
                    $noty.close();
                } },
                { addClass: 'btn btn-danger', text: 'Annuleren', onClick: function ($noty) {
                    $noty.close();
                } }
            ]
        });
    }
    Notification.confirmation = confirmation;
})(Notification || (Notification = {}));
//# sourceMappingURL=oogstplanner.notifications.js.map