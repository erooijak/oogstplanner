var zk = {
    
    toMonthCalendar: function() {
        $.fn.fullpage.moveSlideRight();
        $(window).scrollTop(0);
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
        $(window).scrollTop(0);
    },

    fillMonthCalendar: function(data) {
        $('#_MonthCalendar').html(data);
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

    showSignupBox: function() {
        $('#loginbox').hide(); 
        $('#signupbox').show(); 
        this.resetValidation();
    },

    showLoginBox: function() {
        $('#signupbox').hide(); 
        $('#loginbox').show(); 
        this.resetValidation()
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