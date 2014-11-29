var zk = {
    
    toFarmingMonth: function() {
        $.fn.fullpage.moveSlideRight();
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
    },

    fillFarmingMonth: function(data) {

        // Display month on top of page
        $('#current-month').text(data.month);

        // TODO: Display pattern for edit
    }

};