$(function() {

	/* Add click event listeners to the squares of the month. */
    $('[data-month]').each(function(i, monthElement) {
        
        $(monthElement).bind('click', function() {
            
            var month = $(monthElement).data('month');

            $.get('/Calendar/Month?month=' + month, function(data) {
                zk.fillMonthCalendar(data);
            })
              .done(function() { zk.toMonthCalendar(); })
              .fail(function() { alert('TODO: Error handling'); });
        });
    });

});