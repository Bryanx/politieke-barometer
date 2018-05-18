var adminGridSelector = $("#admin-grid");
adminGridSelector.gridstack({
    resizable: {
        handles: "e, se, s, sw, w"
    }
});
var adminGrid = adminGridSelector.data("gridstack");

//create activity widget
var activityWidget = function (id, title) {
    return "<div data-widget-id=" + id + " class='chart-container'>" +
        "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
        "                <div class='x_title'>" +
        "                    <h2 data-widget-id=" + id + " class='graphTitle'>" + title + "</h2>" +
        "                    <div class='clearfix'></div>" +
        "                </div>" +
        "                <div style='position: relative; height: 75%;'> " +
        "                    <canvas id='graph" + id + "'></canvas>" +
        "                    <h2 class='no-graph-data text-center'>" + Resources.NoDataAvailable + "</h2>" +
        "               </div>" +
        "               <div class='graph-options'>" +
        "                   <button data-widget-id=" + id + " class='btn btn-danger removeData' id='removeData" + id + "'>" + Resources.RemoveData + "</button>" +
        "               </div>" +
        "            </div>" +
        "        </div>";
};

(() => {
    $.get({
        url: "/api/GetActivityWidgets",
        success: widgets => {
            let rowSpan = 12;
            widgets.forEach(widget => {
                adminGrid.addWidget(activityWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber,
                    rowSpan,  widget.ColumnSpan,  true, 6, 12, 6, 6, widget.WidgetId);
                adminGrid.movable(".grid-stack-item", false);
                adminGrid.resizable(".grid-stack-item", false);
                rowSpan = 6;
            });
        }
    });
})($);
