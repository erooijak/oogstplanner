$(function() {

	/* Add click event listeners to the squares of the month for entering the month calendar detail view. */
    $('[data-month]').each(function(i, monthSquare) {
        var oogstplanner = new Oogstplanner();

        $(monthSquare).bind('click', function() {
            
            var monthName = $(monthSquare).data('month');

            if (oogstplanner.getHasActionAttributeValue(monthName) === true) {
                oogstplanner.fillMonthCalendar(monthName);
            }
            else { 
                alert("Deze maand heeft geen zaai- of oogstmomenten.");
            }

        });

    });

    /* Add click listener to login or register button which shows modal popup. */
    $('#registerOrLogin').on('click', function() {
        BootstrapDialog.show({
            title: 'De oogstplanner gemeenschap',
            message: $('<div></div>').load('/Account/LoginOrRegisterModal')
        });
    });

});
