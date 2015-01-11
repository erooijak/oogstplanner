$(function() {

	/* Add click event listeners to the squares of the month. */
    $('[data-month]').each(function(i, monthElement) {
        
        $(monthElement).bind('click', function() {
            
            var month = $(monthElement).data('month');

            $.get('/FarmingAction/Edit?month=' + month, function(data) {
                zk.fillFarmingMonth(month, data);
            })
              .done(function() { zk.toFarmingMonth(); })
              .fail(function() { alert('TODO: Error handling'); });
        });
    });

});