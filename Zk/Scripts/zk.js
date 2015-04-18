var zk = {
	
    month: null,

    toMonthCalendar: function() {
        $.fn.fullpage.moveSlideRight();
        $(window).scrollTop(0);
        this.makeNumericTextBoxesNumeric();
        this.makeCropPluralWhenCropCountIsBiggerThan1();

        // Remove the crop-selection-box since the side is visible on the month calendar...
        $('#crop-selection-box').hide();
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
        $(window).scrollTop(0);

        $('#crop-selection-box').show();
    },

    fillMonthCalendar: function(month) {

        // Store the month in the object so we can use it and do not have to get it from the page.
        this.month = month;

        $.get('/Calendar/Month?month=' + month, function(data) {
            $('#_MonthCalendar').html(data);
        })
        .done(function() { zk.toMonthCalendar(); zk.bindFarmingActionRemoveFunctionToDeleteButton(); })
        .fail(function() { alert('TODO: Error handling'); });
    },

    addFarmingAction: function(cropId, month, actionType, cropCount) {
        $.post('/Calendar/AddFarmingAction', { cropId: cropId, month: month, actionType: actionType, cropCount: cropCount } );
    },

    removeFarmingAction: function(id) {
        var that = this;
        $.post('/Calendar/RemoveFarmingAction', { id: id }, function (response) {
            if (response.success === true) { that.fillMonthCalendar(that.month); alert("Het gewas is succesvol verwijderd.") }
            else { alert("TODO: Error handling") }
        });
    },

    resizeCropSelectionBox: function() {
        $('#crop-selection-box').css({
            height: $('#month-overview-responsive-square-elements').innerHeight()
        });
    },

    resizeLoginArea: function() {
        
        var windowHeight = $(window).innerHeight();
        var topHeight = $('#top').innerHeight();
        var padding = windowHeight * 0.025 + 20;	

        $('#login').css({ 
            height: windowHeight - topHeight - padding
        });

    },

    resetValidation: function() {

        // Removes validation from input-fields
        $('.input-validation-error').empty();

        // Removes validation message after input-fields
        $('.field-validation-error').empty();

        // Removes validation summary 
        $('.validation-summary-errors').empty();

    },

    makeNumericTextBoxesNumeric: function() {
        $(".numeric-text-box").keydown(function (e) {
            
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||

                // Allow: Ctrl+A.
                (e.keyCode === 65 && e.ctrlKey === true) || 

                // Allow: home, end, left, right, down, up.
                (e.keyCode >= 35 && e.keyCode <= 40)) {

                    // If field would become empty by backspace or delete disable the move.
                    if ( (e.keyCode === 46 || e.keyCode === 8) && $(this).val().length === 1) {
                        
                        // TODO: If field would become larger than 999 disable the move.

                        e.preventDefault();
                    }

                    // Otherwise let it happen, don't do anything.
                    return;
            }

            // Ensure that it is a number and stop the keypress.
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }

        });
    },

    makeCropPluralWhenCropCountIsBiggerThan1: function() {

        // Every crop count input field needs a span label with crop or crops depending on the count.
        $('.form-group').each(function() {
            var input = $(this).find('input:first');
            var span = $(this).find('.crop-count-crop-word:first');
            input.change(function() {
                span.text( $(this).val() == 1 ? 'plant' : 'planten' );
            });      

            // Ensure text is correct on load by triggering change event.
            input.trigger('change');
          
        });

    },

    bindFarmingActionRemoveFunctionToDeleteButton: function() {

        var that = this;

        $('.remove-farming-action').bind('click', function (e) {
            e.preventDefault();

            // farming action id is stored in the id of the remove-farming-action element.
            var farmingActionId = this.id;
            that.removeFarmingAction(farmingActionId);

        });

    },

    showSignupBox: function() {
        $('#loginbox').hide(); 
        $('#signupbox').show(); 
        this.resetValidation();
    },

    showLoginBox: function() {
        $('#signupbox').hide(); 
        $('#loginbox').show(); 
        this.resetValidation();
    },

    toggleHighlightOnRecommendedMonths: function() {

        // Loop over the recommended harvesting or sowing months and toggle the highlighting.
        $.each(dragged.recommendedMonths, function(i, month){
            if (month) { 
                $('div[data-month=' + month + ']').toggleClass('highlight');
            }
        });

    },

    toggleHighlightOnHover: function() {
        $('[data-month]').toggleClass('hover-highlight');
    }

};