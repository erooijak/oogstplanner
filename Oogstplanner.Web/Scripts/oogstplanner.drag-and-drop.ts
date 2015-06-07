/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jquery.plugins.d.ts" />

enum ActionType { 
    SOWING = 0, 
    HARVESTING = 1 
}

class DragAndDrop { 

    selectedActionType : ActionType;
    recommendedMonths : string[];
    cropId : number;
    cropCount : number;
    growingTime : number;

    collectValues() {

        this.selectedActionType = $('.drag-and-drop-sentence-action-type').first().text() === "oogsten" 
            ? ActionType.HARVESTING : ActionType.SOWING;
        var cssClassActionTypeSubstring = this.selectedActionType === ActionType.HARVESTING ? 'harvest' : 'sow';
        this.recommendedMonths = $('#selected-crop-hidden-' + cssClassActionTypeSubstring + 'ingMonths').val().split(',');
        this.cropId = $('#selected-crop-hidden-id').val();
        this.cropCount = $('#selected-crop-count-number-field').val();
        this.growingTime = +$('#selected-crop-growingTime').text();
    }
        
    toggleHighlightOnRecommendedMonths() {

        // Loop over the recommended harvesting or sowing months and toggle the highlighting.
        $.each(this.recommendedMonths, function (i, month){
            if (month) { 
                $('div[data-month=' + month + ']').toggleClass('highlight');
            }
        });

    }

    toggleHighlightOnHover() {
        $('[data-month]').toggleClass('hover-highlight');
    }

}

$(function() {

    var dragged = new DragAndDrop();

    /* Provide draggable styling on mouse-over */
    $('#selected-crop-info').on('mouseover', function () {

        // Store all selected values in the dragged object so they can be retrieved on drop.
        dragged.collectValues();
    
        // Preload the images with same width as the responsive square elements.
        var squareWidth = $('.square').first().innerWidth();
        var imageHarvesting = $('<img>')
            .attr('src', '../Content/Images/Draggable/harvesting.png')
            .attr('width', squareWidth)
            .addClass('flopped');
        var imageSowing = $('<img>')
            .attr('src', '../Content/Images/Draggable/sowing.png')
            .attr('width', squareWidth)
            .addClass('flopped');

        $(this).draggable({
            
            helper: function () {
                var type = dragged.selectedActionType;
                var img = type == ActionType.HARVESTING ? imageHarvesting : imageSowing;
                return img;
            },

            start: function (e, ui) {     
                dragged.toggleHighlightOnHover();
                dragged.toggleHighlightOnRecommendedMonths();
            },

            stop: function (e, ui) {
                var draggedDiv = ui.helper;
                draggedDiv.remove();
                dragged.toggleHighlightOnHover();
                dragged.toggleHighlightOnRecommendedMonths();
            },

            zIndex: 999

        });
    });

    var oogstplanner = new Oogstplanner();

    /* Make month squares droppable */
    $('*[data-month]').droppable({
        hoverClass: 'droppable-hover',
        drop: function (event, ui) {

            var cropId = dragged.cropId;
            var month = $(event.target).data().month;
            var actionType =  dragged.selectedActionType;
            var cropCount = dragged.cropCount;

            oogstplanner.addFarmingAction(cropId, month, actionType, cropCount);

            oogstplanner.setHasActionAttributeValue(month, true);

            var monthNames : string[] = Util.getMonthNames();

            var indexCurrentMonth : number = monthNames.indexOf(month);
            var oppositeMonth = Util.getOppositeMonth(actionType, indexCurrentMonth, dragged.growingTime);

            oogstplanner.setHasActionAttributeValue(oppositeMonth, true);
        }
    });

});
