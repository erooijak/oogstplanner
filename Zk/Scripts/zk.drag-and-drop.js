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
            var cropId = $('#selected-crop-hidden-id').val();
            var month = $(event.target).data().month;
            var actionType = $('.drag-and-drop-sentence-action-type').first().text() === "oogsten" ? "harvesting" : "sowing";
            var cropCount = $('#selected-crop-count-number-field').val();

            zk.addFarmingAction(cropId, month, actionType, cropCount);
        }
    });

});