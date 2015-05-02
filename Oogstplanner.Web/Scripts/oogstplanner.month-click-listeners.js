$(function() {

	/* Add click event listeners to the squares of the month. */
    $('[data-month]').each(function(i, monthElement) {
        
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

});