var zk = {
    
    toFarmingMonth: function() {
        $.fn.fullpage.moveSlideRight();
    },
    
    toMain: function() {
        $.fn.fullpage.moveSlideLeft();
        this.emptyFarmingMonth();
    },

    fillFarmingMonth: function(data) {

        var currentMonth = data.month;

        // Display month on top of page
        $('#current-month').text(currentMonth);

        // Add data to hidden formfield
        $('input[name="Month"]').val(currentMonth);

        var farmingActions = data.farmingActions;

        // Display all crops that need to be sowed and harvested.
        var sowingActions = this.getData(farmingActions, "Action", "Sowing");
        var harvestingActions = this.getData(farmingActions, "Action", "Harvesting");

        this.fillSowingPattern(sowingActions);
        this.fillHarvestingPattern(harvestingActions);
       
    },

    fillSowingPattern: function(actions) {
        // Display name, Race, and Crop count in the sowing area.
        actions.forEach(function (a) {
            $('#sowing').append( $([
                                "  <span><input type='text'" 
                                    + "class='cropcount input form-control'" 
                                    + "name='CropCount'" 
                                    + "value='" + a.CropCount 
                                    + "'></input> stuks</span>",
                                "  <span>" + a.Crop.Name      + "</span>",
                                "  <span>" + a.Crop.Race      + "</span></br>"
                               ].join("\n")));  
        });
    },

    fillHarvestingPattern: function(actions) {
        // Display name, Race, and Crop count in the harvesting area.
        actions.forEach(function (a) {
            $('#harvesting').append( $([
                                "  <span><input type='text'" 
                                    + "class='cropcount input form-control'" 
                                    + "name='CropCount'" 
                                    + "value='" + a.CropCount 
                                    + "'></input> stuks</span>",
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