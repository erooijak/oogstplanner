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

    // Add a call to the edit method of a farming month to clicks on a specific month.
    months.forEach(function(month) {
        $('.' + month).bind("click", function() {
            $.getJSON('/FarmingMonth/Edit/' + month, function( data ) {
                console.log(data);
            });
        });
    });
});