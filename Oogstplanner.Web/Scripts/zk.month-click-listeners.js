$(function() {

	/* Add click event listeners to the squares of the month. */
    $('[data-month]').each(function(i, monthElement) {
        
        $(monthElement).bind('click', function() {
            var month = $(monthElement).data('month');
            zk.fillMonthCalendar(month);
        });

    });

});