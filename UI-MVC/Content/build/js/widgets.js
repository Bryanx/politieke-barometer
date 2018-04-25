$(document).ready(init);

var primary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--primary-color");
var secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--secondary-color");
var tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--tertiary-color");

//user widget element
function createUserWidget(id, title) {
    return "<div class='chart-container'>" +
        "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
        "                <div class='x_title'>" +
        "                    <h2 class='graphTitle'>" + title + "</h2>" +
        "                    <ul class='nav navbar-right panel_toolbox'>" +
        "                       <li><a class='collapse-link'><i class='fa fa-chevron-up'></i></a></li>" +
        "                       <li class='dropdown'>" +
        "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-wrench'></i></a>" +
        "                       <ul class='dropdown-menu' role='menu'>" +
        "                           <li><a href='#'>Settings 1</a></li>" +
        "                           <li><a href='#'>Settings 2</a></li>" +
        "                       </ul>" +
        "                       </li>" +
        "                       <li>" +
        "                            <a class='close-widget'>" +
        "                                <i id=' + id + ' class='fa fa-close'></i>" +
        "                            </a>" +
        "                       </li>" +
        "                    </ul>" +
        "                    <div class='clearfix'></div>" +
        "                </div>" +
        "                <div style='position: relative; height: 85%;'><canvas id='graph" + id + "'></canvas></div>" +
        "            </div>" +
        "        </div>";
}

//item widget element
function createItemWidget(id, title) {
    let element = "<div data-widget-id=" + id + " class='chart-container'>" +
        "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
        "                <div class='x_title'>" +
        "                    <h2 class='graphTitle'>" + title + "</h2>" +
        "                    <ul class='nav navbar-right panel_toolbox'>" +
        "                       <li class='dropdown'>" +
        "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-gear'></i></a>" +
        "                       <ul class='dropdown-menu' role='menu'>" +
        "                           <li><a data-widget-id=" + id + " class='toggleChartType'>Bar chart</a></li>" +
        "                           <li><a data-widget-id=" + id + " class='chartShowLines'>Lines between points</a></li>" +
        "                           <li><a data-widget-id=" + id + " class='chartShowXGrid'>X grid lines</a></li>" +
        "                           <li><a data-widget-id=" + id + " class='chartShowYGrid'>Y grid lines</a></li>" +
        "                           <li><a data-widget-id=" + id + " class='chartShowLogScale'>Logarithmic y-axes</a></li>" +
        "                           <li><a data-widget-id=" + id + " class='chartShowLegend'>Legend</a></li>" +
        "                       </ul>" +
        "                       </li>" +
        "                    </ul>" +
        "                    <div class='clearfix'></div>" +
        "                </div>" +
        "                <div style='position: relative; height: 80%;'> " +
        "                    <canvas id='graph" + id + "'></canvas>" +
        "                    <h2 class='no-graph-data text-center'>No data available.</h2>" +
        "               </div>" +
        "               <div class='graph-options'>" +
        "                   <input id=" + id + " type='text' class='form-control compareSearch' placeholder='Compare data with someone else.'>" +
        "                   <button class='btn btn-success addToDashboard'>Add to dashboard</button>" +
        "               </div>" +
        "            </div>" +
        "        </div>";
    return element;
}

var gridselector = $("#grid");
gridselector.gridstack({
    resizable: {
        handles: "e, se, s, sw, w"
    }
});
var grid = gridselector.data("gridstack");

function noWidgetsAvailable() {
    $(".no-widgets").show();
}

function loadGraphs(itemId, widget) {

    //for performance reasons charts are stored in a local variable.
    var charts = [];
    var widgetId = widget.WidgetId;
    var COLORS = [
        '#4dc9f6',
        '#f67019',
        '#f53794',
        '#537bc4',
        '#acc236',
        '#166a8f',
        '#00a950',
        '#58595b',
        '#8549ba'
    ];

    //When no data is available this function shows a message.
    let displayNoGraphData = function (WidgetId) {
        $(".no-graph-data").css("display", "flex");
        $("#graph"+WidgetId).hide();
    };

    //Add data to graph.
    function AddDataSet(chart, name, values) {
        var newColor = COLORS[chart.config.data.datasets.length];
        var newDataset = {
            label: name,
            borderColor: newColor,
            data: values,
            fill: false
        };
        
        chart.config.data.datasets.push(newDataset);
        chart.update();
    }

    //Load graph data
    let LoadGraphDataSet = function (suggestion, $this) {
        let name = suggestion.value;
        let widgetId = $this[0].id;
        let chart = charts.find(c => c.config.id == widgetId);
        let itemId = suggestion.data;
        $.ajax({
            type: "GET",
            url: "/api/GetGraphs/" + itemId + "/" + itemId,
            dataType: "json",
            success: data => {
                if (data !== undefined) {
                    AddDataSet(chart, name, Object.values(data));
                }
            },
            fail: d => console.log(d)
        })
    };
    
    //Toggles the charttype: bar/line chart
    let ToggleChartType = function (e) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);
        var ctx = document.getElementById("graph"+widgetId).getContext("2d");
        var temp = jQuery.extend(true, {}, chart.config);

        //change chart type
        let type = chart.config.type;
        type = type === "line" ? "bar" : "line";

        //Remove chart from charts and delete it.
        charts.splice(charts.findIndex(c => c.config.id == widgetId), 1);
        chart.destroy();

        temp.type = type; // The new chart type
        charts.push(new Chart(ctx, temp));

        //change text on button
        let text = $(".toggleChartType").html();
        text = text === "Bar chart" ? "Line chart" : "Bar chart";
        $(".toggleChartType").html(text);
    };

    //Toggles lines between points on a line graph.
    let ShowLines = function (e) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);
        chart.options.showLines = !chart.options.showLines;
        chart.update();
    };

    //Toggles the horizontal lines on a graph.
    let ShowXGrid = function (e) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);
        chart.options.scales.xAxes[0].gridLines.display = !chart.options.scales.xAxes[0].gridLines.display;
        chart.update();
    };

    //Toggles the vertical lines on a graph.
    let ShowYGrid = function (e) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);
        chart.options.scales.yAxes[0].gridLines.display = !chart.options.scales.yAxes[0].gridLines.display;
        chart.update();
    };

    //Transforms graph type to logarithmic or linear.
    let ShowLogScale = function (e) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);
        let type = chart.options.scales.yAxes[0].type;
        type = type === "linear" ? "logarithmic" : "linear";
        chart.options.scales.yAxes[0].type = type;
        let text = $(".chartShowLogScale").html();
        text = text === "Linear y-axes" ? "Logarithmic y-axes" : "Linear y-axes";
        $(".chartShowLogScale").html(text);
        chart.update();
    };

    //Shows the legend of a graph.
    let ShowLegend = function (e) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);
        chart.options.legend.display = !chart.options.legend.display;
        chart.update();
    };

    //Adds a linechart to a widget.
    let addLineChartJS = function (widget, chartData) {
        charts.push(new Chart(document.getElementById("graph"+widgetId), {
            id: widgetId,
            type: "line",
            data: {
                labels: Object.keys(chartData),
                datasets: [{
                    data: Object.values(chartData),
                    label: widget.Title,
                    borderColor: COLORS[0],
                    fill: false,
                }],
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        }));
    };

    //Retrieves the graph data from api.
    let ajaxLoadGraphs = function (widget) {
        $.ajax({
            type: "GET",
            url: "/api/GetGraphs/" + itemId + "/" + widgetId,
            dataType: "json",
            success: data2 => {
                if (data2 !== undefined) {
                    addLineChartJS(widget, data2);
                } else {
                    displayNoGraphData(widgetId);
                }
            }
        })
    };
    
    //Graph handlers
    $(document).on("click", ".toggleChartType", (e) => ToggleChartType(e));
    $(document).on("click", ".chartShowLines", (e) => ShowLines(e));
    $(document).on("click", ".chartShowXGrid", (e) => ShowXGrid(e));
    $(document).on("click", ".chartShowYGrid", (e) => ShowYGrid(e));
    $(document).on("click", ".chartShowLogScale", (e) => ShowLogScale(e));
    $(document).on("click", ".chartShowLegend", (e) => ShowLegend(e));
    $(document).on("click", ".addGraphData", (e) => AddGraphData(e));

    //Loads the graph data.
    $(() => ajaxLoadGraphs(widget));

    //Compare search
    $('.compareSearch').devbridgeAutocomplete({
        width: 400,
        lookup: searchlist,
        triggerSelectOnValidInput: false,
        maxHeight: 200,
        formatResult: function (suggestion) {
            return "<div class='compareSuggestion'>" + suggestion.value + "</div>";
        },
        onSelect: function (suggestion) {
            LoadGraphDataSet(suggestion, $(this))
        }
    });
    $('.compareSearch').keyup(()=> {
        $($('.compareSuggestion')[0]).parent().parent().css("margin-left", "0");
    });

}

function loadWidgets(url, itemId) {
    //Puts the widgets on the grid.
    let loadGrid = function (data, itemId) {
        if (data != null && data.length) {
            $.each(data, (index, widget) => {
                //UserWidget
                if (widget.DashboardId !== -1) {
                    grid.addWidget(createUserWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                        false, 4, 12, 4, 12, widget.WidgetId);
                    //ItemWidget
                } else {
                    grid.addWidget(createItemWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                        true, 4, 12, 4, 12, widget.WidgetId);
                    grid.movable(".grid-stack-item", false);
                    grid.resizable(".grid-stack-item", false);
                }
                //if widgettype == graphtype
                if (widget.WidgetType === 0) {
                    loadGraphs(itemId, widget);
                }
            });
        } else {
            noWidgetsAvailable();
        }
    };
    
    //Loads the widgets via api call.
    let ajaxCallLoadGrid = function () {
        $.ajax({
            type: "GET",
            url: url + itemId,
            dataType: "json",
            success: data => loadGrid(data, itemId),
            error: (xhr) => alert(xhr.responseText)
        })
    };
    
    $(() => ajaxCallLoadGrid());
}

function init() {
    "use strict";
    
    //Shows an animated save message
    function showSaveMessage() {
        $("#notificationMessage")
            .addClass("bg-success")
            .html("<i class='fa fa-check-circle'></i> " + Resources.Saved)
            .show()
            .delay(3000)
            .slideUp("slow", "swing", function () {
                $(this).removeClass().empty();
            });
    }

    //Shows an animated error message
    function showErrorMessage() {
        $("#notificationMessage")
            .addClass("bg-error")
            .html("<i class='fa fa-times-circle'></i> " + Resources.Failed)
            .show()
            .delay(3000)
            .slideUp("slow", "swing", function () {
                $(this).removeClass().empty();
            });
    }

    //Moves a widget from item page to dashboard page
    let moveWidget = function () {
        let widgetId = $(".addToDashboard").parents(".chart-container").data("widget-id");
        $.ajax({
            type: "POST",
            url: "/api/MoveWidget/" + widgetId,
            dataType: "json",
            success: () => showSaveMessage()
        }).fail(() => showErrorMessage());
    };

    //Updates given widgets on resize
    let updateWidgets = function (items) {
        let serializedItems = [];
        $.each(items, function (index, item) {
            serializedItems.push({
                WidgetId: item.id,
                Title: "widget", //unused title
                RowNumber: item.x,
                ColumnNumber: item.y,
                RowSpan: item.width,
                ColumnSpan: item.height,
                WidgetType: 0,
                DashboardId: 0
            });
        });
        $.ajax({
            type: "PUT",
            url: "/api/Widget/",
            data: JSON.stringify(serializedItems),
            dataType: "application/json",
            contentType: "application/json",
            success: () => showSaveMessage(),
            error: (xhr) => {
                alert($.parseJSON(xhr.responseText).Message);
                showErrorMessage();
            }
        })
    };
    
    //Removes a widget
    let deleteWidget = function (e) {
        let el = (e.target).closest(".grid-stack-item");
        gridselector.data("gridstack").removeWidget(el);
        $.ajax({
            type: "DELETE",
            url: "/api/Widget/Delete/" + e.target.id,
            dataType: "json",
            error: (xhr) => {
                alert($.parseJSON(xhr.responseText).Message);
                showErrorMessage();
            },
            success: () => {
                if (!$(".grid-stack-item").length) noWidgetsAvailable();
                showSaveMessage();
            }
        })
    };
    
    //dashboard handlers
    $(document).on("click", ".close-widget", (e) => deleteWidget(e));
    $(".grid-stack").on("change", (event, items) => updateWidgets(items));

    //itempage handlers
    $(document).on("click", ".addToDashboard", () => moveWidget());
}

