var ActionType;
(function (ActionType) {
    ActionType[ActionType["SOWING"] = 0] = "SOWING";
    ActionType[ActionType["HARVESTING"] = 1] = "HARVESTING";
})(ActionType || (ActionType = {}));
var DragAndDrop = (function () {
    function DragAndDrop() {
    }
    DragAndDrop.prototype.collectValues = function () {
        this.selectedActionType = $('.drag-and-drop-sentence-action-type').first().text() === "oogsten" ? 1 /* HARVESTING */ : 0 /* SOWING */;
        var cssClassActionTypeSubstring = this.selectedActionType === 1 /* HARVESTING */ ? 'harvest' : 'sow';
        this.recommendedMonths = $('#selected-crop-hidden-' + cssClassActionTypeSubstring + 'ingMonths').val().split(',');
        this.cropId = $('#selected-crop-hidden-id').val();
        this.cropCount = $('#selected-crop-count-number-field').val();
    };
    DragAndDrop.prototype.toggleHighlightOnRecommendedMonths = function () {
        $.each(this.recommendedMonths, function (i, month) {
            if (month) {
                $('div[data-month=' + month + ']').toggleClass('highlight');
            }
        });
    };
    DragAndDrop.prototype.toggleHighlightOnHover = function () {
        $('[data-month]').toggleClass('hover-highlight');
    };
    return DragAndDrop;
})();
$(function () {
    var dragged = new DragAndDrop();
    $('#selected-crop-info').on('mouseover', function () {
        dragged.collectValues();
        var squareWidth = $('.square').first().innerWidth();
        var imageHarvesting = $('<img>').attr('src', '../Content/Images/Draggable/harvesting.png').attr('width', squareWidth);
        var imageSowing = $('<img>').attr('src', '../Content/Images/Draggable/sowing.png').attr('width', squareWidth);
        ;
        $(this).draggable({
            helper: function () {
                var type = dragged.selectedActionType;
                var img = type == 1 /* HARVESTING */ ? imageHarvesting : imageSowing;
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
            }
        });
    });
    var oogstplanner = new Scripts.Oogstplanner();
    $('*[data-month]').droppable({
        hoverClass: 'droppable-hover',
        drop: function (event, ui) {
            var cropId = dragged.cropId;
            var month = $(event.target).data().month;
            var actionType = dragged.selectedActionType;
            var cropCount = dragged.cropCount;
            oogstplanner.addFarmingAction(cropId, month, actionType, cropCount);
        }
    });
});
//# sourceMappingURL=oogstplanner.drag-and-drop.js.map