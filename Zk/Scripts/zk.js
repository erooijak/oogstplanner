var zk = {
    
    toMonthCalendar: function() {
        $.fn.fullpage.moveSlideRight();
        $(window).scrollTop(0);
        this.makeNumericTextBoxesNumeric();
        this.makeCropPluralWhenCropCountIsBiggerThan1();
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
        $(window).scrollTop(0);
        this.makeNumericTextBoxesNumeric();
    },

    fillMonthCalendar: function(data) {
        $('#_MonthCalendar').html(data);
    },

    addFarmingAction: function(cropId, month, actionType, cropCount) {
        $.post('/Calendar/AddFarmingAction', { cropId: cropId, month: month, actionType: actionType, cropCount: cropCount } );
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
                    if ( (e.keyCode === 46 || e.keyCode === 8) && $(this).val().length === 1 ) {
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
        $('.crop-count-crop-word').prev('input').change(function() {
            $(this).next('span').text( $(this).val() == 1 ? 'plant' : 'planten' );
        });

        // Ensure text is correct on load by triggering change event.
        $('.crop-count-crop-word').prev('input').trigger('change');
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

        // Get the css class sub string based on action type text and collected recommended harvesting or sowing months.
        var cssClassActionTypeSubstring = $('.drag-and-drop-sentence-action-type:first').text() === 'oogsten' ? 'harvest' : 'sow';
        var recommendedMonths = $('#selected-crop-hidden-' + cssClassActionTypeSubstring + 'ingMonths').val().split(',');

        // Loop over the recommended harvesting or sowing months and toggle the highlighting.
        $.each(recommendedMonths, function(i, month){
            $('div[data-month=' + month + ']').toggleClass('highlight');
        });

    },

    toggleHighlightOnHover: function() {
        $('[data-month]').toggleClass('hover-highlight');
    },

    // Helper functions:
    capitaliseFirstLetter: function(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    },

    // Returns the elements from an array (arr) where the type (type) has the specified value (val).
    getData: function(arr, type, val) {
        return arr.filter(function (el) {
            return el[type] === val;
        });
    }

};