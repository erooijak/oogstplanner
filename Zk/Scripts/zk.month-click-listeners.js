$(function() {

	/* Add click event listeners to the squares of the month. */

    // The months (in Dutch) that correspond to the CSS classes of the squares.
    var months = [ 'januari', 
                   'februari', 
                   'maart', 
                   'april', 
                   'mei', 
                   'juni', 
                   'juli', 
                   'augustus', 
                   'september', 
                   'oktober', 
                   'november', 
                   'december' ]

    // Add a call to the edit method of a farming month on a click on a specific month.
    months.forEach(function(month) {

        $('.' + month).bind('click', function() {

            // Note: In the call to the Edit method a query string parameter is necessary because normal 
            //       parameter binding does not seem to work in our version of Mono.
            $.getJSON('/FarmingMonth/Edit?month=' + month, function(data) {

                // Switch to the farming month view and display the month and data.
                zk.toFarmingMonth();
                zk.fillFarmingMonth(data);
            });
        });
    });
});