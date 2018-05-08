$(document).ready(init);

var primary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--primary-color");
var secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--secondary-color");
var tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--tertiary-color");

//user widget element
var widgetElements = {
    addNodeboxGraph: function (id) {
        addCSSgrid(id);
        createNodebox(id);

    },
    //User widget element
    createUserWidget: function (id, title) {
        return "<div class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 class='graphTitle'>" + title + "</h2>" +
            "                    <ul class='nav navbar-right panel_toolbox'>" +
            "                       <li>" +
            "                            <a class='close-widget'>" +
            "                                <i id=" + id + " class='fa fa-close'></i>" +
            "                            </a>" +
            "                       </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-gear'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowLines'>Lines between points</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowXGrid'>X grid lines</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowYGrid'>Y grid lines</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowLogScale'>Logarithmic y-axes</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowLegend'>Legend</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-bar-chart'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='makeLineChart'>Line chart</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeBarChart'>Bar chart</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makePieChart'>Pie chart</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeDonutChart'>Donut chart</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-floppy-o'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='getJPGImage'>Download JPG image</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='getPNGImage'>Download PNG image</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                    </ul>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div style='position: relative; height: 75%;'> " +
            "                    <canvas id='graph" + id + "'></canvas>" +
            "                    <h2 class='no-graph-data text-center'>No data available.</h2>" +
            "               </div>" +
            "               <div class='graph-options'>" +
            "                   <input id=" + id + " type='text' class='form-control compareSearch' placeholder='Compare data with someone else.'>" +
            "               </div>" +
            "            </div>" +
            "        </div>";
    },
    //item widget element
    createItemWidget: function (id, title) {
        return "<div data-widget-id=" + id + " class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 class='graphTitle'>" + title + "</h2>" +
            "                    <ul class='nav navbar-right panel_toolbox'>" +
            "                   <li><a id=" + id + " class='addToDashboard'>" + Resources.Save + "</a></li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-gear'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowLines'>Lines between points</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowXGrid'>X grid lines</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowYGrid'>Y grid lines</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowLogScale'>Logarithmic y-axes</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='chartShowLegend'>Legend</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-bar-chart'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='makeLineChart'>Line chart</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeBarChart'>Bar chart</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makePieChart'>Pie chart</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeDonutChart'>Donut chart</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-floppy-o'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='getJPGImage'>Download JPG image</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='getPNGImage'>Download PNG image</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li><a data-widget-id=" + id + " class='dateChangeChart changeToWeek'>7d</a></li>" +
            "                   <li><a data-widget-id=" + id + " class='dateChangeChart changeToMonth'>1m</a></li>" +
            "                   <li><a data-widget-id=" + id + " class='dateChangeChart changeTo3Month'>3m</a></li>" +
            "                   <li><a data-widget-id=" + id + " class='dateChangeChart changeToYear'>1y</a></li>" +
            "                    </ul>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div style='position: relative; height: 75%;'> " +
            "                    <canvas id='graph" + id + "'></canvas>" +
            "                    <h2 class='no-graph-data text-center'>No data available.</h2>" +
            "               </div>" +
            "               <div class='graph-options'>" +
            "                   <input id=" + id + " type='text' class='form-control compareSearch' placeholder='Compare data with someone else.'>" +
            "               </div>" +
            "            </div>" +
            "        </div>";
    },
    //twitter widget
    createTwitterWidget: function (title) {
        return "<div class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 class='graphTitle'>" + title + "</h2>" +
            "                    <ul class='nav navbar-right panel_toolbox'>" +
            "                       <li><a class='addToDashboard'>" + Resources.Save + "</a></li>" +
            "                    </ul>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div class='scroll' style='position: relative; height: 88%;'> " +
            "                    <div id='twitter-feed'></div>" +
            "               </div>" +
            "            </div>" +
            "        </div>";
    },
    //related items widget
    createPeopleWidget: function (title) {
        return "<div class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 class='graphTitle'>" + title + "</h2>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div class='scroll' style='position: relative; height: 88%;'> " +
            "                    <div id='related-items'></div>" +
            "               </div>" +
            "            </div>" +
            "        </div>";
    },
    createPeopleItem: function (itemId, name, mentions) {
        return "<div class='org-member col-xs-6 col-sm-4 col-md-2'>" +
            "<img src='/Content/build/images/picture.jpg'>" +
            "<strong><a href='/Person/Details/" + itemId + "'> " + name + "</a></strong>" +
            "<div>" + Resources.Mentions + ": " + mentions + "</div>" +
            "</div>";
    },
};

function createNodebox(id) {
    let node = document.getElementById('graph' + id);
    let parent = node.parentElement;

    parent.removeChild(node);
    node = document.createElement('canvas');
    node.id = 'graph' + id;
    node.width = parent.clientWidth;
    node.height = parent.clientHeight;
    parent.appendChild(node);

    parent.parentElement.parentElement.onchange = function () {
        nodeboxSize(node, parent, id)
    };

    let canvas = {
        userId: 'AnthonyT',
        projectId: 'tutorial',
        functionId: 'circle_graph',
        canvasId: 'graph' + id,
        autoplay: true
    };

    // Initialize the NodeBox player object
    ndbx.embed(canvas, function (err, player) {
        if (err) {
            throw new Error(err);
        } else {
            window.player = player;
        }
    });

};

function nodeboxSize(node, parent, id) {
    parent.removeChild(node);
    node = document.createElement('canvas');
    node.id = 'graph' + id;
    node.width = parent.clientWidth;
    node.height = parent.clientHeight;
    console.log("height: " + parent.clientHeight + "\nwidth: " + parent.clientWidth);
    parent.appendChild(node);
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

let convertChartTypeToGraphType = function(chartType) {
    switch (chartType) {
        case "line": return 1;
        case "bar": return 2;
        case "pie": return 3;
        case "donut": return 4;
        case "doughnut": return 4;
    }
};

var charts = [];
var widgets = [];
var itempage = false;
var orgpage = false;
var dashboardpage = false;
var updateWidgets;

function loadGraphs(itemId, widget) {

    var widgetId = widget.WidgetId;
    var COLORS = [
        'rgb(255, 99, 132)',
        'rgb(255, 159, 64)',
        'rgb(255, 205, 86)',
        'rgb(75, 192, 192)',
        'rgb(54, 162, 235)',
        'rgb(153, 102, 255)',
        'rgb(201, 203, 207)'
    ];
    var DARKCOLORS = [
        'rgb(235, 69, 102)',
        'rgb(235, 129, 34)',
        'rgb(235, 175, 46)',
        'rgb(55, 162, 162)',
        'rgb(34, 132, 205)',
        'rgb(123, 72, 225)',
        'rgb(171, 173, 177)'
    ];
    
    //Retrieves a chart by data-id.
    let FindChartByEvent = function(e) {
        let widgetId = $(e.target).data("widget-id");
        return charts.find(c => c.config.id == widgetId);
    };
    
    //Retrieves an image of the graph
    let AddImageUrl = function (id) {
        let chart = charts.find(c => c.config.id == id);
        $(".getPNGImage").attr("href", chart.toBase64Image())
            .attr("download","graph.png");        
        $(".getJPGImage").attr("href", chart.toBase64Image())
            .attr("download","graph.jpg");
    };
    
    //Time is changed on the x-axes accordingly.
    let ChangeTime = function (e, value, date) {
        let chart = FindChartByEvent(e);
        chart.options.scales = {
            xAxes: [{
                type: 'time',
                id: 'x-axes-0',
                time: {
                    format: "DD-MM",
                    min: moment().add(value, date).format('DD-MM')
                }
            }],
        };
        chart.update();
    };
    
    //When no data is available this function shows a message.
    let displayNoGraphData = function (WidgetId) {
        $(".no-graph-data").css("display", "flex");
        let widget = $("#graph"+WidgetId).parents(".grid-stack-item-content");
        $(widget).find(".graph-options").hide();
        $(widget).find(".panel_toolbox").hide();
        $("#graph"+WidgetId).hide();
    };

    //Add data to graph.
    let AddDataSet = function (chart, name, values) {
        let newColor = COLORS[chart.config.data.datasets.length];
        let borderColor = newColor;
        let hoverColor = DARKCOLORS[chart.config.data.datasets.length];
        
        if (chart.config.type === "doughnut" || chart.config.type === "pie") {
            let firstDataset = chart.config.data.datasets[0];
            borderColor = firstDataset.borderColor;
            newColor = firstDataset.backgroundColor;
            hoverColor = firstDataset.hoverBackgroundColor;
        }
        
        var newDataset = {
            label: name,
            borderColor: borderColor,
            backgroundColor: newColor,
            hoverBackgroundColor: hoverColor,
            data: values,
            fill: false
        };
        
        chart.config.data.datasets.push(newDataset);
        chart.update();
    };

    //Load graph data
    let LoadGraphDataSet = function (suggestion, $this) {
        let name = suggestion.value;
        let widgetId = $this[0].id;
        let chart = charts.find(c => c.config.id == widgetId);
        let itemId = suggestion.data;
        $.ajax({
            type: "GET",
            url: "/api/GetGraphs/" + itemId + "/" + widgetId,
            dataType: "json",
            success: data => {
                if (data !== undefined) {
                    if (!widget.ItemIds.includes("" + itemId)) {
                        widget.ItemIds.push(itemId);
                        if (dashboardpage) updateWidgets(widgets);
                    }
                    AddDataSet(chart, name, data[0].GraphValues.map(g => g.NumberOfTimes));
                }
            },
            fail: d => alert(d)
        })
    };
    
    //Toggles the charttype: bar/line chart
    let ChangeChartType = function (e, type) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);

        var ctx = document.getElementById("graph"+widgetId).getContext("2d");
        var temp = jQuery.extend(true, {}, chart.config);

        //Remove chart from charts and delete it.
        charts.splice(charts.findIndex(c => c.config.id == widgetId), 1);
        chart.destroy();

        temp.type = type; // The new chart type
        charts.push(new Chart(ctx, temp));
        
        //change text on button
        widgets.filter(w => w.WidgetId === widgetId)[0].GraphType = convertChartTypeToGraphType(type);
        if (dashboardpage) updateWidgets(widgets);
    };

    //Toggles lines between points on a line graph.
    let ShowLines = function (e) {
        let chart = FindChartByEvent(e);
        chart.options.showLines = !chart.options.showLines;
        chart.update();
    };

    //Toggles the horizontal lines on a graph.
    let ShowXGrid = function (e) {
        let chart = FindChartByEvent(e);
        chart.options.scales.xAxes[0].gridLines.display = !chart.options.scales.xAxes[0].gridLines.display;
        chart.update();
    };

    //Toggles the vertical lines on a graph.
    let ShowYGrid = function (e) {
        let chart = FindChartByEvent(e);
        chart.options.scales.yAxes[0].gridLines.display = !chart.options.scales.yAxes[0].gridLines.display;
        chart.update();
    };

    //Transforms graph type to logarithmic or linear.
    let ShowLogScale = function (e) {
        let chart = FindChartByEvent(e);
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
        let chart = FindChartByEvent(e);
        chart.options.legend.display = !chart.options.legend.display;
        chart.update();
    };
    
    //Converts a graphtype to appropriate string
    let ConvertToChartType = function (graphType) {
        switch (graphType) {
            case 1: return "line";
            case 2: return "bar";
            case 3: return "pie";
            case 4: return "doughnut";
        }
    };
    
    //Create a new chart
    let AddChart = function (widget, labels, values, borderColor, color, darkColor, chartType) {
        charts.push(new Chart(document.getElementById("graph"+widgetId), {
            id: widgetId,
            type: chartType,
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    label: widget.Title,
                    borderColor: borderColor,
                    backgroundColor: color,
                    fill: false,
                }],
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    onComplete: function () {
                        AddImageUrl(widgetId);
                    }
                }
            }
        }));
    };

    //Settings for a piechart
    let AddPieChart = function (widget, chartData, chartType="pie") {
        let labels = chartData[0].GraphValues.map(g => g.Value);
        let values = chartData[0].GraphValues.map(g => g.NumberOfTimes);
        let borderColor = "#fff";
        let color = [];
        let darkColor = [];
        let r = Math.floor((Math.random() * 4));
        $.each(values, () => {
            color.push(COLORS[r]);
            darkColor.push(COLORS[r]);
            r++;
        });
        if (chartData[0].KeyValue === "Gender") {
            labels = [Resources.Male, Resources.Female];
        }
        $(".dateChangeChart").each(function () {
            if ($(this).data("widget-id") == widget.WidgetId) {
                $(this).hide();
            }
        });
        AddChart(widget, labels, values, borderColor, color, darkColor, chartType);
    };

    //Settings for a linechart
    let AddLineChart = function (widget, chartData, chartType="line") {
        let labels = chartData[0].GraphValues.map(g => g.Value);
        let values = chartData[0].GraphValues.map(g => g.NumberOfTimes);
        let colorNumber = Math.floor((Math.random() * 6));
        let color = COLORS[colorNumber];
        let darkColor = DARKCOLORS[colorNumber];
        let borderColor = COLORS[colorNumber];
        AddChart(widget, labels, values, borderColor, color, darkColor, chartType);
    };

    //Settings for a barchart
    let AddBarChart = function (widget, chartData) {
        AddLineChart(widget, chartData, "bar");
    };

    //Settings for a donutchart
    let AddDoughnutChart = function (widget, chartData) {
        AddPieChart(widget,  chartData, "doughnut");
    };
    
    //Moves the graph data to the appropriate method.
    let loadGraphHandler = function (widget, data) {
        if (data !== undefined) {
            let chartType = ConvertToChartType(widget.GraphType);
            switch (chartType) {
                case "line": AddLineChart(widget, data);break;
                case "bar": AddBarChart(widget, data);break;
                case "pie": AddPieChart(widget, data);break;
                case "doughnut": AddDoughnutChart(widget, data);
            }
        } else {
            displayNoGraphData(widgetId);
        }
    };

    //Moves the graph data to the appropriate method.
    let AddDataSetWithName = function(itemId, chart, title, values) {
        $.ajax({
            method: "GET",
            url: "/api/GetItemWithDetails/" + itemId,
            success: data => AddDataSet(chart, data.Name + " " + title.toLowerCase(), values)
        });
    };
    
    //Retrieves the graph data from api.
    let ajaxLoadGraphs = function (widget) {
        if (widget.ItemIds != null && widget.ItemIds.length) {
            itemids = [];
            if (!itemId.length) {
                itemids.push.apply(itemids, widget.ItemIds);
            } else {
                itemids.push(itemId);
            }
            $.each(itemids, (index, itemId) => {
                $.ajax({
                    type: "GET",
                    url: "/api/GetGraphs/" + itemId + "/" + widget.WidgetId,
                    dataType: "json",
                    success: data => {
                        if (!widget.ItemIds.includes("" + itemId)) {
                            widget.ItemIds.push(itemId);
                        }
                        let chart = charts.find(c => c.config.id == widget.WidgetId);
                        if (chart == undefined) {
                            loadGraphHandler(widget, data)
                        } else {
                            AddDataSetWithName(itemId, chart, data[0].KeyValue, data[0].GraphValues.map(g => g.NumberOfTimes));
                        }
                    }
                });
            });
        } else {
            displayNoGraphData(widget.WidgetId);
        }
    };
    
    //Graph handlers
    $(document).on("click", ".makeLineChart", (e) => ChangeChartType(e, "line"));
    $(document).on("click", ".makeBarChart", (e) => ChangeChartType(e, "bar"));
    $(document).on("click", ".makePieChart", (e) => ChangeChartType(e, "pie"));
    $(document).on("click", ".makeDonutChart", (e) => ChangeChartType(e, "doughnut"));
    $(document).on("click", ".chartShowLines", (e) => ShowLines(e));
    $(document).on("click", ".chartShowXGrid", (e) => ShowXGrid(e));
    $(document).on("click", ".chartShowYGrid", (e) => ShowYGrid(e));
    $(document).on("click", ".chartShowLogScale", (e) => ShowLogScale(e));
    $(document).on("click", ".chartShowLegend", (e) => ShowLegend(e));
    $(document).on("click", ".changeToWeek", (e) => ChangeTime(e, -7, 'day'));
    $(document).on("click", ".changeToMonth", (e) => ChangeTime(e, -1, 'month'));
    $(document).on("click", ".changeTo3Month", (e) => ChangeTime(e, -3, 'month'));
    $(document).on("click", ".changeToYear", (e) => ChangeTime(e, -12, 'month'));

    //Loads the graph data.
    $(() => ajaxLoadGraphs(widget));

    //Compare search.
    function addAutocomplete(){
        if (searchlist.length > 0){
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
        } else {
            //Keep trying to get the searchlist 
            setTimeout(addAutocomplete, 100);
        }
    }
    
    addAutocomplete();
    
    $('.compareSearch').keyup(()=> {
        $($('.compareSuggestion')[0]).parent().parent().css("margin-left", "0");
    });

}

function loadWidgets(url, itemId) {
    //Loads a social widget.
    let loadSocialWidget = function(data) {
        $.each(data.SocialMediaNames, (index, value) => {
            if (value.Source.Name === "Twitter") {
                grid.addWidget(widgetElements.createTwitterWidget("Twitter feed"), 1, 1, 6, 6, true, 4, 12, 4, 12, 1);
                grid.movable(".grid-stack-item", false);
                grid.resizable(".grid-stack-item", false);
                twttr.widgets.createTimeline(
                    {
                        sourceType: "profile",
                        screenName: value.Username.replace("@",""),
                    },
                    document.getElementById("twitter-feed"),
                    {
                        chrome: "noheader, noborder, nofooter",
                        linkColor: primary_color,
                        tweetLimit: 5
                    }
                );
            }
        });
    };
    
    //Retrieves item for social widget..
    let loadItemForSocialWidget = function (itemId) {
        $.ajax({
            method: "GET",
            url: "/api/GetItemWithDetails/" + itemId,
            success: data => loadSocialWidget(data)
        });
    };

    //Loads a MorePeopleFromOrg widget
    let loadPeopleWidget = function (data, title, widgetHeight) {
        grid.addWidget(widgetElements.createPeopleWidget(title), 1, 1, 12, widgetHeight, true, 4, 12, 4, 12, 1);
        grid.movable(".grid-stack-item", false);
        grid.resizable(".grid-stack-item", false);
        $.each(data, (index, value) => {
           $("#related-items").append(widgetElements.createPeopleItem(value.ItemId, value.Name, value.NumberOfMentions)); 
        });
    };
    
    //Loads more people from the same organisation
    let loadPeopleFromOrg = function (itemId, url, title, widgetHeight) {
        $.ajax({
            method: "GET",
            url: url + itemId,
            success: data => loadPeopleWidget(data, title, widgetHeight)
        });
    };
    
    //Puts the widgets on the grid.
    let loadGrid = function (data, itemId) {
        itempage = false;
        orgpage = $(".organisation-page").length;
        dashboardpage = $(".dashboard-page").length;
        if (data != null && data.length) {
            $.each(data, (index, widget) => {
                //UserWidget
                if (widget.DashboardId !== -1) {
                    grid.addWidget(widgetElements.createUserWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                        false, 4, 12, 4, 12, widget.WidgetId);
                    //ItemWidget
                } else {
                    grid.addWidget(widgetElements.createItemWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                        true, 4, 12, 4, 12, widget.WidgetId);
                    grid.movable(".grid-stack-item", false);
                    grid.resizable(".grid-stack-item", false);
                    itempage = true;
                }
                //if widgettype == graphtype
                if (widget.WidgetType === 0) {
                    loadGraphs(itemId, widget);
                }
                widgets.push(widget);
            });
        } else {
            noWidgetsAvailable();
        }
        if (itempage && !orgpage) loadItemForSocialWidget(itemId);
        if (orgpage) loadPeopleFromOrg(itemId,"/api/GetPeopleFromOrg/", "Members", 6);
        if (itempage) loadPeopleFromOrg(itemId,"/api/GetMorePeopleFromOrg/", "People from the same organisation", 4);
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

    this.btnAddNodebox = function () {
        grid.addWidget(widgetElements.createUserWidget('grafiek' + counter), 0, 0, 6, 6, true, 4, 12, 4, 12);
        widgetElements.addNodeboxGraph('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);

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
    let moveWidget = function (e) {
        let widget = widgets.find(w => w.WidgetId == e.target.id);
        $.ajax({
            type: "POST",
            url: "/api/MoveWidget/" + e.target.id,
            data:  {itemIds: widget.ItemIds},
            success: () => showSaveMessage()
        }).fail(() => showErrorMessage());
    };
    
    //Updates given widgets on resize
    updateWidgets = function (widgets) {
        let serializedItems = [];
        $.each(widgets, function (index, widget) {
            serializedItems.push({
                WidgetId: widget.id ? widget.id : widget.WidgetId,
                Title: "widget", //unused title
                RowNumber: widget.x ? widget.x : widget.RowNumber,
                ColumnNumber: widget.y ? widget.y : widget.ColumnNumber,
                RowSpan: widget.width ? widget.width : widget.RowSpan,
                ColumnSpan: widget.height ? widget.height : widget.ColumnSpan,
                WidgetType: 0,
                DashboardId: 0,
                GraphType: widget.GraphType ? widget.GraphType : 0,
                ItemIds: widget.ItemIds ? widget.ItemIds : []
            });
        });
        $.ajax({
            type: "POST",
            url: "/api/UpdateWidget/",
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
        let widget = (e.target).closest(".grid-stack-item");
        let widgetId = e.target.id;
        if (widgetId.length) {
            $.ajax({
                type: "DELETE",
                url: "/api/Widget/Delete/" + widgetId,
                dataType: "json",
                error: (xhr) => {
                    alert($.parseJSON(xhr.responseText).Message);
                    showErrorMessage();
                },
                success: () => {
                    gridselector.data("gridstack").removeWidget(widget);
                    if (!$(".grid-stack-item").length) noWidgetsAvailable();
                    showSaveMessage();
                }
            })
        }
    };
    
    //dashboard handlers
    $(document).on("click", ".close-widget", (e) => deleteWidget(e));
    $('#btnAddNodebox').click(this.btnAddNodebox);

    //persist widget state if changed. (only for dashboard widgets)
        $(".grid-stack").on("change", (event, items) => {
            if (dashboardpage) updateWidgets(items);
        });

    //itempage handlers
    $(document).on("click", ".addToDashboard", (e) => moveWidget(e));
}

