$(function() {

	/* Add click event listeners to the squares of the month. */

    // The months that correspond to the CSS classes of the squares.
    var months = [ 'january', 
                   'february', 
                   'march', 
                   'april', 
                   'may', 
                   'june', 
                   'july', 
                   'august', 
                   'september', 
                   'october', 
                   'november', 
                   'december' ]

    // Add a call to the edit method of a farming month on a click on a specific month.
    months.forEach(function(month) {
        $('.' + month).bind('click', function() {
            console.log('Calling controller for month: ' + month);

            // Note: In the call to the Edit method a query string parameter is necessary because normal 
            //       parameter binding does not work in Mono.
            $.getJSON('/FarmingMonth/Edit?month=' + month, function( data ) {
                console.log('Called controller for month: ' + month);
                console.log(data);
            });
        });
    });
});