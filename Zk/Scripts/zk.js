var zk = {
    
    toFarmingMonth: function() {
        $.fn.fullpage.moveSlideRight();
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
        this.emptyFarmingMonth();
    },

    fillFarmingMonth: function(data) {

        // Display month on top of page
        $('#current-month').text(data.month);

        // Display all crops that need to be sowed and harvested.
        var sowingActions = this.getData(data.farmingActions, "Action", "Sowing");
        var harvestingActions = this.getData(data.farmingActions, "Action", "Harvesting");

        this.fillSowingPattern(sowingActions);
        this.fillHarvestingPattern(harvestingActions);
       
    },

    fillSowingPattern: function(actions) {
        // Display name, Race, and Crop count in the sowing area.
        actions.forEach(function (a) {
            $('#sowing').append( $([
                                "  <span>" + a.CropCount      + " stuks</span>",
                                "  <span>" + a.Crop.Name      + "</span>",
                                "  <span>" + a.Crop.Race      + "</span></br>"
                               ].join("\n")));  
        });
    },

    fillHarvestingPattern: function(actions) {
        // Display name, Race, and Crop count in the harvesting area.
        actions.forEach(function (a) {
            $('#harvesting').append( $([
                                "  <span>" + a.CropCount      + " stuks</span>",
                                "  <span>" + a.Crop.Name      + "</span>",
                                "  <span>" + a.Crop.Race      + "</span></br>"
                               ].join("\n")));  
        });
    },

    emptyFarmingMonth: function() {
        $('#sowing').empty();
        $('#harvesting').empty();
    },

    // Helper functions:
    //   Returns the elements from an array (arr) where the type (type) has the specified value (val).
    getData: function(arr, type, val) {
        return arr.filter(function (el) {
            return el[type] === val;
        });
    }

};