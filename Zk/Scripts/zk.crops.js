var cropsSuggestionEngine = new Bloodhound({
    datumTokenizer: function (datum) {
        return Bloodhound.tokenizers.whitespace(datum.Name);
    },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    prefetch: {
        url: "/Crop/All",
        filter: function (data) {
            return $.map(data, function(crop) {
                return {
                    value: crop.Name
                };
            });
        }
    }
});

$(function() {

    cropsSuggestionEngine.initialize();

    $('#select-crop .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
        name: 'Gewassen',
        displayKey: 'Name',
        source: cropsSuggestionEngine.ttAdapter(),
        templates: {
            empty: [
            '<div class="empty-message">',
            'unable to find any results that match the current query',
            '</div>'
            ].join('\n'),
            suggestion: Handlebars.compile('<p><strong>{{name}}</strong> in {{category}}</p>')
        }
    }); 

});