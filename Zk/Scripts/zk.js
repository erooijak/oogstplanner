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
        // Display name, Race, and Crop count in the sowing table.
        actions.forEach(function (a) {
            $('#sowing').append( $(["<tr>",
                                "  <td>" + a.Crop.Name      + "</td>",
                                "  <td>" + a.Crop.Race      + "</td>",
                                "  <td>" + a.Crop.Category  + "</td>",
                                "  <td>" + a.CropCount      + "</td>",
                                "</tr>"
                               ].join("\n")));  
        });
    },

    fillHarvestingPattern: function(actions) {
        // Display name, Race, and Crop count in the sowing table.
        actions.forEach(function (a) {
            $('#harvesting').append( $(["<tr>",
                                "  <td>" + a.Crop.Name      + "</td>",
                                "  <td>" + a.Crop.Race      + "</td>",
                                "  <td>" + a.Crop.Category  + "</td>",
                                "  <td>" + a.CropCount      + "</td>",
                                "</tr>"
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