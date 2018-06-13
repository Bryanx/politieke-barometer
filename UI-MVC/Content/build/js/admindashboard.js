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

var AddTimeChart = function (widget, labels, label, values, borderColor, color, darkColor, chartType) {
    let chart = new Chart(document.getElementById("graph" + widget.WidgetId), {
        id: widget.WidgetId,
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                data: values,
                label: label,
                borderColor: borderColor,
                backgroundColor: color,
                fill: false,
            }],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                xAxes: [{
                    type: 'time',
                    display: true,
                    time: {
                        format: "DD-MM",
                        round: 'day'
                    }
                }],
            },
        }
    });
};


(() => {
    let COLORS = [
        'rgb(255, 99, 132)',
        'rgb(255, 159, 64)',
        'rgb(255, 205, 86)',
        'rgb(75, 192, 192)',
        'rgb(54, 162, 235)',
        'rgb(153, 102, 255)',
        'rgb(207, 81, 171)'
    ];
    $.get({
        url: "/api/GetActivityWidgets",
        success: widgets => {
            let rowSpan = 12;
            $.each(widgets, (i, widget) => {
                adminGrid.addWidget(activityWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber,
                    rowSpan,  widget.ColumnSpan,  true, 6, 12, 6, 6, widget.WidgetId);
                adminGrid.movable(".grid-stack-item", false);
                adminGrid.resizable(".grid-stack-item", false);
                rowSpan = 6;
                i += 3;
                let labels = widget.WidgetDatas[0].GraphValues.map(g => g.Value);
                let values = widget.WidgetDatas[0].GraphValues.map(g => g.NumberOfTimes);
                AddTimeChart(widget, labels, widget.Title, values, COLORS[i], COLORS[i], COLORS[i], "line");
            });
        }
    });
})($);
