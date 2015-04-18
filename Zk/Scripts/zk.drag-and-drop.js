var actionType = Object.freeze( { HARVESTING: 0, SOWING: 1 } );
var dragged = { 

    selectedActionType: null,
    recommendedMonths: null,
    cropId: null,
    cropCount: null,

    collectValues: function() {

        dragged.selectedActionType = $('.drag-and-drop-sentence-action-type').first().text() === "oogsten" 
            ? actionType.HARVESTING : actionType.SOWING;
        var cssClassActionTypeSubstring = dragged.selectedActionType === actionType.HARVESTING ? 'harvest' : 'sow';
        dragged.recommendedMonths = $('#selected-crop-hidden-' + cssClassActionTypeSubstring + 'ingMonths').val().split(',');
        dragged.cropId = $('#selected-crop-hidden-id').val();
        dragged.cropCount = $('#selected-crop-count-number-field').val();

    }

};

$(function() {

    /* Provide draggable styling on mouse-over */
    $('#selected-crop-info').on('mouseover', function () {

        // Store all selected values in the dragged object so they can be retrieved on drop.
        dragged.collectValues();
    
        // Preload the images with same width as the responsive square elements.
        var squareWidth = $('.square').first().innerWidth();
        var imageHarvesting = $('<img>').attr('src', '../Content/Images/Draggable/harvesting.png').attr('width', squareWidth);
        var imageSowing = $('<img>').attr('src', '../Content/Images/Draggable/sowing.png').attr('width', squareWidth);;

        $(this).draggable({
            
            helper: function() {
                var type = dragged.selectedActionType;
                var img = type == actionType.HARVESTING ? imageHarvesting : imageSowing;
                return img;
            },

            start: function(e, ui) {     
                zk.toggleHighlightOnHover();
                zk.toggleHighlightOnRecommendedMonths();
            },

            stop: function(e, ui) {
                var draggedDiv = ui.helper;
                draggedDiv.remove();
                zk.toggleHighlightOnHover();
                zk.toggleHighlightOnRecommendedMonths();
            }

        });
    });

    /* Make month squares droppable */
    $('*[data-month]').droppable({
        hoverClass: 'droppable-hover',
        drop: function (event, ui) {

            var cropId = dragged.cropId;
            var month = $(event.target).data().month;
            var actionType =  dragged.selectedActionType;
            var cropCount = dragged.cropCount;

            zk.addFarmingAction(cropId, month, actionType, cropCount);
        }
    });

});