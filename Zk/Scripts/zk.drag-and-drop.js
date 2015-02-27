$(function() {

    /* Provide draggable styling on mouse-over */
    $('#selected-crop-info').on('mouseover', function () {
        $(this).draggable({
            helper: 'clone',
            start: function(e, ui) {
                zk.toggleHighlightOnHover();
                zk.toggleHighlightOnRecommendedMonths();
            },
            stop: function(e, ui) { 
                zk.toggleHighlightOnHover();
                zk.toggleHighlightOnRecommendedMonths();
            }
        });
    });

    /* Make month squares droppable */
    $('*[data-month]').droppable({
        hoverClass: 'droppable-hover',
        drop: function (event, ui) {
            console.log($('#selected-crop-hidden-id').val());
            console.log($(event.target).data().month);
            console.log($('.drag-and-drop-sentence-action-type').first().text());
            var cropId = $('#selected-crop-hidden-id').val();
            var month = $(event.target).data().month;
        }
    });

});