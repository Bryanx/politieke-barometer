searchlist = [];

function loadSuggestions() {
    $('.search-field').devbridgeAutocomplete({
        width: $('.searchbar').width()-2,
        lookup: searchlist,
        triggerSelectOnValidInput: false,
        onSelect: function (suggestion) {
            window.location.href = "/Person/Details/" + suggestion.data;
        }
    });
}

$(window).on("resize", function () {
    $('.autocomplete-suggestions').css("width", $('.searchbar').width()-2);
});

(() => {

    $.ajax({
        method: "GET",
        url: "/api/GetSearchItems",
        dataType: 'json'
    }).done(data => {
        searchlist = data;
        loadSuggestions(data)
    });
})(jQuery);

