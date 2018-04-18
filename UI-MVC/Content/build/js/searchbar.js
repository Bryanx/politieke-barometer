searchlist = [];

//create autocomplete
function loadSuggestions() {
    $('.search-field').devbridgeAutocomplete({
        width: $('.searchbar').width()-2,
        lookup: searchlist,
        triggerSelectOnValidInput: false,
        maxHeight: 290,
        onSelect: function (suggestion) {
            window.location.href = "/Person/Details/" + suggestion.data;
        }
    });
}

//Load items
(() => {
    $.ajax({
        method: "GET",
        url: "/api/GetSearchItems",
        dataType: 'json'
    }).done(data => {
        searchlist = data;
        loadSuggestions();
    });
})(jQuery);

//Resizeable suggestions
$(window).on("resize", function () {
    $('.autocomplete-suggestions').css("width", $('.searchbar').width()-2);
});

