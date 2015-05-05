$(function() {

	/* Add click event listeners to the squares of the month for entering the month calendar detail view. */
    $('[data-month]').each(function(i, monthElement) {
        
        var oogstplanner = new Scripts.Oogstplanner();

        $(monthElement).bind('click', function() {
            var month = $(monthElement).data('month');

            if (oogstplanner.getHasActionAttributeValue(month) === true) {
                oogstplanner.fillMonthCalendar(month);
            }
            else { 
                alert("Deze maand heeft geen zaai- of oogstmomenten.");
            }

        });

    });

    /* Add click listener to login or register button which shows modal popup. */
    $('#registerOrLogin').on('click', function() {
        BootstrapDialog.show({
            title: 'Registreren of inloggen bij de Oogstplanner',
            message: $('<div></div>').load('/Account/LoginOrRegisterModal')
        });
    });

});