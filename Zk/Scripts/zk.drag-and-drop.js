$(function() {

    /* Provide draggable styling on mouse-over */
    $('#selected-crop-info').on('mouseover', function () {
        $(this).draggable({
            helper: 'clone',
            start: function(e, ui) {
                zk.toggleHighlightOnRecommendedMonths();
            },
            stop: function(e, ui) { 
                zk.toggleHighlightOnRecommendedMonths();
            }
        });
    });

    /* Make month squares droppable */
    $('*[data-month]').droppable({
        drop: function (event, ui) {
            var cropId = $('#selected-crop-hidden-id').val();
            var month = $(event.target).data().month;
        }
    });

});