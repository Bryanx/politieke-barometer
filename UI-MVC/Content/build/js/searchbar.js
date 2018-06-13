var searchlist = [];

//create autocomplete
function loadSuggestions() {
  $('.search-field').devbridgeAutocomplete({
    width: "auto",
    lookup: searchlist,
    triggerSelectOnValidInput: false,
    maxHeight: 290,
    onSelect: function (suggestion) {
      //window.location.href = "/Person/Details/" + suggestion.data;
      checkItemType(suggestion);    
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

//Check item type
function checkItemType(id) {
  console.log("test");
  $.ajax({
    type: "GET",
    url: "/api/checkItemType/" + id.data,
    contentType: 'application/json; charset=utf-8',
    dataType: "json",
  }).fail(() => {/* ok */ })
    .done(function (data) {
      window.location.href = "/" + data + "/Details/" + id.data;
    })
};

