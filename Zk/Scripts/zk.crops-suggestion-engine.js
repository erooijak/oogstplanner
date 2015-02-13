﻿/* Create a bloodhound typeahead functionality using bloodhound.js */
var cropsSuggestionEngine = new Bloodhound({
    datumTokenizer: function(datum) {
        x = Bloodhound.tokenizers.whitespace(datum.name);
        y = Bloodhound.tokenizers.whitespace(datum.category);
        z = Bloodhound.tokenizers.whitespace(datum.race);
        return x.concat(y).concat(z);
    },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    prefetch: {
        url: "/Crop/All",
        filter: function(data) {
            return $.map(data, function(crop) {
                return {
                    id: crop.Id,
                    name: crop.Name,
                    race: crop.Race,
                    category: crop.Category,
                    growingTime: crop.GrowingTime,
                    areaPerCrop: crop.AreaPerCrop,
                    areaPerBag: crop.AreaPerBag,
                    pricePerBag: crop.PricePerBag
                };
            });
        }
    }
});

/* Bind the typeahead function to all select-crop input fields. */
$(function() {

    cropsSuggestionEngine.initialize();

    $('#select-crop .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
        name: 'Gewassen',
        displayKey: 'name',
        source: cropsSuggestionEngine.ttAdapter(),
        templates: {
            empty: ['<div class="empty-message">','Niets gevonden...','</div>'].join('\n'),
            suggestion: Handlebars.compile('<p><strong>{{name}}</strong> in {{category}}<br/>'
                                         + '<span class="no-highlighting">Ras:</span> {{race}}.<br/>'
                                         + '<i class="no-highlighting">Groeitijd: {{growingTime}} maanden.</i><br/>'
                                         + '<input type="hidden" name="selected-crop-id" value={{id}}></p>')
        }
    }); 

});