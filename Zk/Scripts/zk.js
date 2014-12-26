var zk = {
    
    toFarmingMonth: function() {
        $.fn.fullpage.moveSlideRight();
        $(window).scrollTop(0);
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
        $(window).scrollTop(0);
    },

    fillFarmingMonth: function(month, data) {
        $('#_FarmingMonth').html(data);
    },

    resizeLoginArea: function() {
        
        var windowHeight = $(window).innerHeight();
        var topHeight = $('#top').innerHeight();
        var padding = windowHeight * 0.025;

        $('#login').css({ 
            height: windowHeight - topHeight - padding
        });

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