var cropsSuggestionEngine = new Bloodhound({
    datumTokenizer: function (datum) {
        nameDatums = Bloodhound.tokenizers.whitespace(datum.name);
        raceDatums = Bloodhound.tokenizers.whitespace(datum.race);
        return nameDatums.concat(raceDatums);
    },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    prefetch: {
        url: "/Crop/All",
        filter: function (data) {
            return $.map(data, function (crop) {
                return {
                    id: crop.Id,
                    name: crop.Name.trim(),
                    race: crop.Race,
                    category: crop.Category,
                    growingTime: crop.GrowingTime,
                    areaPerCrop: crop.AreaPerCrop,
                    areaPerBag: crop.AreaPerBag,
                    pricePerBag: crop.PricePerBag,
                    harvestingMonths: crop.HarvestingMonths,
                    sowingMonths: crop.SowingMonths
                };
            });
        }
    }
});
$(function () {
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
            empty: ['<div class="empty-message">', 'Niets gevonden...', '</div>'].join('\n'),
            suggestion: Handlebars.compile('<p><strong>{{name}}</strong>' + '<span class="no-highlighting"> in {{category}}</span><br/>' + '<span class="no-highlighting">Ras:</span> {{race}}.<br/>' + '<i class="no-highlighting">Groeitijd: {{growingTime}} maanden.</i><br/></p>')
        }
    });
});
//# sourceMappingURL=oogstplanner.crops-suggestion-engine.js.map