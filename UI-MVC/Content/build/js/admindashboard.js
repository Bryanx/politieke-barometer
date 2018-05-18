var adminGridSelector = $("#admin-grid");
adminGridSelector.gridstack({
    resizable: {
        handles: "e, se, s, sw, w"
    }
});
var adminGrid = adminGridSelector.data("gridstack");

(() => {
    $.get({
        url: "/api/GetActivityWidgets",
        success: data => console.log(data)
    });
})($);
