$(function() {

    /* Provide draggable styling on mouse-over */
    $('#selected-crop-info').on('mouseover', function () {
        $(this).draggable({
            helper: 'clone',
            start: function(event, ui) { 
                $('div[data-month=maart]').toggleClass('activeDroppable'); /*TODO*/
            },
            stop: function(event, ui) { 
                $('div[data-month=maart]').toggleClass('activeDroppable'); /*TODO*/
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