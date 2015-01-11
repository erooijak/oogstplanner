$(function() {

	/* Add click event listeners to the squares of the month. */
    $('[data-month]').each(function(i, monthElement) {
        
        $(monthElement).bind('click', function() {
            
            var month = $(monthElement).data('month');

            // Add a call to the edit method of a farming month on a click on a specific month.
            // The data is loaded in partial view and switched to the page on success.
            // Note: In the call to the Edit method a query string parameter is necessary because normal 
            //       parameter binding does not seem to work in our version of Mono.
            $.get('/FarmingAction/Edit?month=' + month, function(data) {
                zk.fillFarmingMonth(month, data);
            })
              .done(function() { zk.toFarmingMonth(); })
              .fail(function() { alert('TODO: Error handling'); });
        });
    });

});