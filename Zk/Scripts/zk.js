var zk = {
    
    toFarmingMonth: function() {
        $.fn.fullpage.moveSlideRight();
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
    },

    fillFarmingMonth: function(month) {

        // Display month on top of page
        $('#current-month').text(month.name);

        // TODO: Display pattern for edit
    }

};