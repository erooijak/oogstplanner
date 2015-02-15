$(function () {
    
    /* Initially hide the selected crop info box */
    $('#selected-crop-info').hide();

    /* Set information in the selected-crop box*/
    $('#crop-selection-box').on('typeahead:selected', function(e, datum) { 
        $('#selected-crop-info').show();
        /* TODO: Call server to get an image via datum.id (cropId) */
        $('#selected-crop-name').text(datum.name);
        $('#selected-crop-race').text(datum.race);
        $('#selected-crop-category').text(datum.category);
        $('#selected-crop-growingTime').text(datum.growingTime);
        $('#selected-crop-areaPerCrop').text(datum.areaPerCrop);
        $('#selected-crop-pricePerBag').text(datum.pricePerBag);
    })
        
    /* Display action type in the drag and drop explanation sentence.*/
    $('input[name=action-type-radios]').on('change', function() {
        var actionType = this.value;
        $('#drag-and-drop-sentence-action-type').text(actionType.toLowerCase());
    });

});