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
            height: $('#responsive-square-elements').innerHeight()
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