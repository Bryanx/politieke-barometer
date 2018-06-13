$(document).ready(init);

//colors
var primary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--primary-color");
var secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--secondary-color");
var tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue("--tertiary-color");

var charts = [];
var widgets = [];
var itempage = false;
var orgpage = false;
var dashboardpage = false;
var addWidgetToDashboard;
var updateWidgets;
var deleteWidget;

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
            "                    <h2 class='graphTitle userWidgetTitle'>" + title + "<i class='fa fa-edit edit-title'></i></h2>" +
            "                    <input class='form-control title-input' data-widget-id=" + id + " type='text' value='"+title+"' placeholder='Give your graph a title.'>" +
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
            "                           <li><a data-widget-id=" + id + " class='makeLineChart'>"+Resources.LineChart+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeBarChart'>"+Resources.BarChart+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makePieChart'>"+Resources.PieChart+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeDonutChart'>"+Resources.DonutChart+"</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-floppy-o'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='getJPGImage'>"+Resources.DownloadJPGImage+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='getPNGImage'>"+Resources.DownloadPNGImage+"</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                    </ul>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div style='position: relative; height: 75%;'> " +
            "                    <div id='loadingSymbol" + id + "'></div>" +
            "                    <canvas id='graph" + id + "'></canvas>" +
            "                    <h2 class='no-graph-data text-center'>"+Resources.NoDataAvailable+"</h2>" +
            "               </div>" +
            "               <div class='graph-options'>" +
            "                   <input id=" + id + " type='text' class='form-control compareSearch' placeholder='"+Resources.CompareDataPlaceholder+"'>" +
            "                   <button data-widget-id=" + id + " class='btn btn-danger removeData' id='removeData"+id+"'>"+Resources.RemoveData+"</button>" +
            "               </div>" +
            "            </div>" +
            "        </div>";
    },
    //item widget element
    createItemWidget: function (id, title) {
        return "<div data-widget-id=" + id + " class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 data-widget-id=" + id + " class='graphTitle'>" + title + "</h2>" +
            "                    <ul class='nav navbar-right panel_toolbox'>" +
            "                   <li><a id=" + id + " class='addToDashboard'>" + Resources.Save +" "+ Resources.To.toString().toLowerCase() +" "+ Resources.Dashboard.toString().toLowerCase() + "</a></li>" +
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
            "                           <li><a data-widget-id=" + id + " class='makeLineChart'>"+Resources.LineChart+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeBarChart'>"+Resources.BarChart+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makePieChart'>"+Resources.PieChart+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='makeDonutChart'>"+Resources.DonutChart+"</a></li>" +
            "                       </ul>" +
            "                   </li>" +
            "                   <li class='dropdown'>" +
            "                       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-expanded='false'><i class='fa fa-floppy-o'></i></a>" +
            "                       <ul class='dropdown-menu' role='menu'>" +
            "                           <li><a data-widget-id=" + id + " class='getJPGImage'>"+Resources.DownloadJPGImage+"</a></li>" +
            "                           <li><a data-widget-id=" + id + " class='getPNGImage'>"+Resources.DownloadPNGImage+"</a></li>" +
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
            "                    <div id='loadingSymbol" + id + "'></div>" +
            "                    <canvas id='graph" + id + "'></canvas>" +
            "                    <h2 class='no-graph-data text-center'>"+Resources.NoDataAvailable+"</h2>" +
            "               </div>" +
            "               <div class='graph-options'>" +
            "                   <input id=" + id + " type='text' class='form-control compareSearch' placeholder='"+Resources.CompareDataPlaceholder+"'>" +
            "                   <button data-widget-id=" + id + " class='btn btn-danger removeData' id='removeData"+id+"'>"+Resources.RemoveData+"</button>" +
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
    //facebook widget
    createFacebookWidget: function (title, url) {
        return "<div class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 class='graphTitle'>" + title + "</h2>" +
            "                    <ul class='nav navbar-right panel_toolbox'>" +
            "                       <li><a class='addToDashboard'>" + Resources.Save + "</a></li>" +
            "                    </ul>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div class='no-scrollbar' style='position: relative; height: 88%;'> " +
            "               <div class='fb-page' data-href='"+url+"' data-tabs='timeline' " +
            "                data-small-header='true' data-adapt-container-width='false' data-hide-cover='true' " +
            "                data-show-facepile='false'><blockquote cite='https://www.facebook.com/facebook' class='fb-xfbml-parse-ignore'> " +
            "               <a href='https://www.facebook.com/facebook'>Facebook</a></blockquote></div> " +
            "               </div>" +
            "            </div>" +
            "        </div>";
    },
    //stories widget
    createStoriesWidget: function (title) {
        return "<div class='chart-container'>" +
            "            <div class='x_panel grid-stack-item-content bg-white no-scrollbar'>" +
            "                <div class='x_title'>" +
            "                    <h2 class='graphTitle'>" + title + "</h2>" +
            // "                    <ul class='nav navbar-right panel_toolbox'>" +
            // "                       <li><a class='addToDashboard'>" + Resources.Save + "</a></li>" +
            // "                    </ul>" +
            "                    <div class='clearfix'></div>" +
            "                </div>" +
            "                <div style='position: relative; height: 88%;'> " +
            "                    <ul id='stories'></ul>" +
            "               </div>" +
            "            </div>" +
            "        </div>";
    }
};

//gridstack
var mentionsGridselector = $("#mentions-grid");
mentionsGridselector.gridstack({
    resizable: {
        handles: "e, se, s, sw, w"
    }
});
var mentionsGrid = mentionsGridselector.data("gridstack");
var gridselector = $("#grid");
gridselector.gridstack({
    resizable: {
        handles: "e, se, s, sw, w"
    }
});
var grid = gridselector.data("gridstack");
var socialGridselector = $("#social-grid");
socialGridselector.gridstack({
    resizable: {
        handles: "e, se, s, sw, w"
    }
});
var socialGrid = socialGridselector.data("gridstack");

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
    parent.appendChild(node);
}

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

function loadGraphs(itemId, widget) {

    var widgetId = widget.WidgetId;
    var COLORS = [
        'rgb(255, 99, 132)',
        'rgb(255, 159, 64)',
        'rgb(255, 205, 86)',
        'rgb(75, 192, 192)',
        'rgb(54, 162, 235)',
        'rgb(153, 102, 255)',
        'rgb(207, 81, 171)'
    ];
    var DARKCOLORS = [
        'rgb(235, 69, 102)',
        'rgb(235, 129, 34)',
        'rgb(235, 175, 46)',
        'rgb(55, 162, 162)',
        'rgb(34, 132, 205)',
        'rgb(123, 72, 225)',
        'rgb(177, 51, 151)'
    ];

    //Converts possible labels to resource items
    function convertLabelsToResource(labels, itemName) {
        labels.forEach((label, index, labels) => {
            switch (label) {
                case "m": label = Resources.Male; break;
                case "f": label = Resources.Female; break;
                case "25+": label = Resources.Above25; break;
                case "25-": label = Resources.Below25; break;
                case "I": label = Resources.Introvert; break;
                case "E": label = Resources.Extrovert; break;
                case "+": label = Resources.HigherEducated; break;
                case "-": label = Resources.LowerEducated; break;
                default: label = Resources.Unknown; break;
            }
            if (itempage) labels[index] = label;
            else labels[index] = itemName + " - " + label;
        });
        return labels;
    }
    
    //Converts a keyvalue to a translated string
    function ConvertKeyValueToResource(KeyValue) {
        if (KeyValue === "Number of mentions") return Resources.NumberOfMentionsFull;
        else if (KeyValue === "Age") return Resources.AgeDistributionOfMentions;
        else if (KeyValue === "Gender") return Resources.GenderDistributionOfMentions;
        else if (KeyValue === "Education") return Resources.EducationDistributionOfMentions;
        else if (KeyValue === "Personality") return Resources.PersonalityDistributionOfMentions;
        else return Resources.Unknown;
    }
    
    //Retrieves a chart by data-id.
    let FindChartByEvent = function (e) {
        let widgetId = $(e.target).data("widget-id");
        return charts.find(c => c.config.id == widgetId);
    };

    //Change widget title
    let changeItemWidgetTitle = function (data, title = null) {
        $(".graphTitle").each(function () {
            let widgetId = $(this).data("widget-id");
            if (widgetId === data.WidgetId) {
                if (title == null) {
                    title = ConvertKeyValueToResource(data.KeyValue);
                    $(this).html(title);
                }
                $(this).html(title);
            }
        });
    };

    //Change widget title by user
    function ChangeUserWidgetTitle(e) {
        $this = $(e.target);
        let newTitle = $this.val();
        let widgetId = $this.data('widget-id');
        $userWidgetTitle = $this.parent().find(".userWidgetTitle");
        $this.addClass("loadMessage");
        $.ajax({
            type: "POST",
            url: "api/WidgetApi/" + widgetId + "/" + newTitle,
            success: () => {
                $this.removeClass("loadMessage");
                $userWidgetTitle.html(newTitle + "<i class='fa fa-edit edit-title'></i></h2>");
                $userWidgetTitle.show();
                $this.hide();
            },
        })
    }

    //Shows title input tag and hides title tag
    let ShowTitleInput = function (e) {
        $titleinput = $(e.target).parent().parent().find(".title-input");
        $(e.target).parents(".userWidgetTitle").hide();
        $titleinput.show();
        $titleinput.focus();
    };
    //Removes the last added dataset
    let RemoveData = function (e) {
        let widget = widgets.find(w => w.WidgetId === $(e.target).data("widget-id"));
        if (widget.ItemIds.length !== 1) {
            let chart = FindChartByEvent(e);
            chart.config.data.datasets.splice(widget.ItemIds.length-1, 1);
            chart.update();
            widget.ItemIds.splice(widget.ItemIds.length-1, 1);
            if (widget.ItemIds.length < 2) $("#removeData"+widget.WidgetId).hide();
            if (dashboardpage) updateWidgets(widgets);
        }
    };
    
    //Retrieves an image of the graph
    let AddImageUrl = function (id) {
        let chart = charts.find(c => c.config.id == id);
        $(".getPNGImage").attr("href", chart.toBase64Image())
            .attr("download", "graph.png");
        $(".getJPGImage").attr("href", chart.toBase64Image())
            .attr("download", "graph.jpg");
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
        let widget = $("#graph" + WidgetId).parents(".grid-stack-item-content");
        $(widget).find(".graph-options").hide();
        $(widget).find(".panel_toolbox").hide();
        $("#graph" + WidgetId).hide();
    };

    //Get a new color that has not yet been added to the graph
    function getRandomColor(datasets) {
        if (datasets.length >= COLORS.length) return COLORS.length-1;
        setColors = [];
        datasets.forEach(d => setColors.push(COLORS.indexOf(d.backgroundColor)));
        let newColor = setColors[0];
        while (setColors.includes(newColor)) {
            newColor = Math.floor((Math.random() * COLORS.length));
        }
        return newColor;
    }

    //Add data to graph.
    let AddDataSet = function (chart, name, values, widgetId, keyValue = null) {
        let colorNumber = getRandomColor(chart.config.data.datasets);
        let newColor = COLORS[colorNumber];
        let borderColor = newColor;
        let hoverColor = DARKCOLORS[colorNumber];
        if (chart.config.type === "doughnut" || chart.config.type === "pie") {
            let firstDataset = chart.config.data.datasets[0];
            borderColor = firstDataset.borderColor;
            newColor = firstDataset.backgroundColor;
            hoverColor = firstDataset.hoverBackgroundColor;
            chart.config.options.tooltips.callbacks.label = function (tooltipItem,data) {
                let dslabels = data.labels[tooltipItem.index];
                let dataset = data.datasets[tooltipItem.datasetIndex];
                let dslabelamt = dataset.data[tooltipItem.index];
                if (tooltipItem.datasetIndex === 0) return dslabels + data.datasets[tooltipItem.datasetIndex].label + ': ' + dslabelamt;
                return data.datasets[tooltipItem.datasetIndex].label + ' : ' + dslabelamt;
            };
            // name = convertLabelsToResource(labels, name.split(" - ")[0]);
        } else {
            name = name + " - " + ConvertKeyValueToResource(keyValue);
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
        $("#removeData"+widgetId).show();
    };

    //Load graph data
    let LoadGraphDataSet = function (suggestion, $this) {
        let name = suggestion.value;
        let itemId = suggestion.data;
        let widgetId = $this[0].id;
        let chart = charts.find(c => c.config.id == widgetId);
        let widgetToUpdate = widgets.find(w => w.WidgetId == widgetId);
        $.ajax({
            type: "GET",
            url: "/api/GetGraphs/" + itemId + "/" + widgetId,
            dataType: "json",
            success: data => {
                if (data !== undefined && !widgetToUpdate.ItemIds.includes(itemId)) {
                    widgetToUpdate.ItemIds.push(itemId);
                    AddDataSet(chart, name, data[0].GraphValues.map(g => g.NumberOfTimes), widgetId, data[0].KeyValue);
                    if (dashboardpage) updateWidgets(widgets);
                }
            },
            fail: d => alert(d)
        })
    };

    //Toggles the charttype: bar/line chart
    let ChangeChartType = function (e, type) {
        let widgetId = $(e.target).data("widget-id");
        let chart = charts.find(c => c.config.id == widgetId);

        var ctx = document.getElementById("graph" + widgetId).getContext("2d");
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
            case 1:
                return "line";
            case 2:
                return "bar";
            case 3:
                return "pie";
            case 4:
                return "doughnut";
        }
    };

    //Create a new chart with time on xAxes
    let AddTimeChart = function (widget, labels, label, values, borderColor, color, darkColor, chartType) {
        charts.push(new Chart(document.getElementById("graph" + widgetId), {
            id: widgetId,
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
                animation: {
                    //when the graph is done loading, JPG/PNG download is possible:
                    onComplete: function () {
                        AddImageUrl(widgetId);
                    }
                },
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
        }));
    };
    
    //Create a new chart
    let AddChart = function (widget, labels, label, values, borderColor, color, darkColor, chartType) {
        charts.push(new Chart(document.getElementById("graph" + widgetId), {
            id: widgetId,
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
                animation: {
                    //when the graph is done loading, JPG/PNG download is possible:
                    onComplete: function () {
                        AddImageUrl(widgetId);
                    }
                }
            }
        }));
    };

    //Settings for a pie/donut chart
    let AddCircleChart = function (widget, chartData, chartType = "pie") {
        let labels = chartData[0].GraphValues.map(g => g.Value);
        let values = chartData[0].GraphValues.map(g => g.NumberOfTimes);
        labels = convertLabelsToResource(labels, chartData[0].ItemName);
        let borderColor = "#fff";
        let color = [];
        let darkColor = [];
        let r = Math.floor((Math.random() * 4));
        $.each(values, () => {
            color.push(COLORS[r]);
            darkColor.push(COLORS[r]);
            r++;
        });
        $(".dateChangeChart").each(function () {
            if ($(this).data("widget-id") == widget.WidgetId) {
                $(this).hide();
            }
        });
        AddChart(widget, labels, "", values, borderColor, color, darkColor, chartType);
    };

    //Settings for a line/bar chart
    let AddAreaChart = function (widget, chartData, chartType = "line") {
        let labels = chartData[0].GraphValues.map(g => g.Value);
        let values = chartData[0].GraphValues.map(g => g.NumberOfTimes);
        let colorNumber = Math.floor((Math.random() * COLORS.length));
        let color = COLORS[colorNumber];
        let darkColor = DARKCOLORS[colorNumber];
        let borderColor = COLORS[colorNumber];
        let legendLabel = chartData[0].ItemName + " - " + ConvertKeyValueToResource(chartData[0].KeyValue);
        if (chartData[0].KeyValue === "Number of mentions") {
            AddTimeChart(widget, labels, legendLabel, values, borderColor, color, darkColor, chartType, chartData[0].KeyValue);
        } else {
            AddChart(widget, labels, legendLabel, values, borderColor, color, darkColor, chartType, chartData[0].KeyValue);
        }
    };

    //Moves the graph data to the appropriate method.
    let loadGraphHandler = function (widget, data) {
        if (data !== undefined) {
            let chartType = ConvertToChartType(widget.GraphType);
            if (chartType === "line" || chartType === "bar") {
                AddAreaChart(widget, data, chartType);
            } else if (chartType === "pie" || chartType === "doughnut") {
                AddCircleChart(widget, data, chartType);
            }
        } else {
            displayNoGraphData(widgetId);
        }
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
            let widgetId = widget.WidgetId;
            $.each(itemids, (index, itemId) => {
                $("#loadingSymbol"+widgetId).html("<i class='graphLoading fa fa-circle-o-notch fa-spin'></i>");
                $.ajax({
                    type: "GET",
                    url: "/api/GetGraphs/" + itemId + "/" + widgetId,
                    dataType: "json",
                    success: data => {
                        $("#loadingSymbol"+widgetId).empty();
                        if (!widget.ItemIds.includes(itemId)) widget.ItemIds.push(itemId);
                        let chart = charts.find(c => c.config.id == widgetId);
                        if (chart == undefined) {
                            if (itempage) changeItemWidgetTitle(data[0]);
                            loadGraphHandler(widget, data);
                        } else {
                            let values = data[0].GraphValues.map(g => g.NumberOfTimes);
                            AddDataSet(chart, data[0].ItemName, values, widgetId, data[0].KeyValue);
                        }
                    }
                });
            });
        } else {
            if (!dashboardpage) displayNoGraphData(widget.WidgetId);
        }
    };

    //Loads the graph data.
    $(() => ajaxLoadGraphs(widget));

    //Compare search.
    function addAutocomplete(){
        if (searchlist.length > 0){
            $('.compareSearch').devbridgeAutocomplete({
                width: "auto",
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

    //Change widget title
    $(document).on("mouseenter", ".userWidgetTitle", (e) => $(e.target).children(".edit-title").show());
    $(document).on("mouseleave", ".userWidgetTitle", (e) => $(e.target).children(".edit-title").hide());
    $(document).on("mouseleave", ".edit-title", (e) => $(e.target).hide());
    $(document).on("click", ".edit-title", (e) => ShowTitleInput(e));
    $(document).on("keypress", ".title-input", (e) => {if (e.which === 13) ChangeUserWidgetTitle(e)});
    //Graph handlers
    $(document).on("click", ".makeLineChart", (e) => ChangeChartType(e, "line"));
    $(document).on("click", ".makeBarChart", (e) => ChangeChartType(e, "bar"));
    $(document).on("click", ".makePieChart", (e) => ChangeChartType(e, "pie"));
    $(document).on("click", ".makeDonutChart", (e) => ChangeChartType(e, "doughnut"));
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
    $(document).on("click", ".removeData", (e) => RemoveData(e).unbind()); //unbind may give an error. this can be ignored.
}

function loadWidgets(url, itemId, onlyLoadLastWidget = false) {
    //Creates a widget with stories for item page
    function loadStories(itemId, rowSpan) {
        socialGrid.addWidget(widgetElements.createStoriesWidget(Resources.LatestStories), 0, 0, rowSpan, 6, true, 4, 12, 5, 12, -2);
        socialGrid.movable(".grid-stack-item", false);
        socialGrid.resizable(".grid-stack-item", false);
        $.ajax({
            method : "GET",
            url : "/api/GetStories/" + itemId,
            success : data => {
                $.each(data,  (index, url) => {
                    $("#stories").append("<li class='story-url'><i class='fa fa-caret-right'></i><a href='"+url+"'>" + url + "</a></li>");
                });
            }
        });
    }
    
    //Loads a social widgets.
    let loadSocialWidget = function(data) {
        let widgetAmount = data.SocialMediaNames.length;
        let rowSpan = 6;
        if (widgetAmount === 2) rowSpan = 4; 
        $.each(data.SocialMediaNames, (index, value) => {
            if (value.Source.Name === "Twitter") {
                socialGrid.addWidget(widgetElements.createTwitterWidget("Twitter feed"), 1, 1, rowSpan, 6, true, 4, 12, 4, 12, 1);
                socialGrid.movable(".grid-stack-item", false);
                socialGrid.resizable(".grid-stack-item", false);
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
            if (value.Source.Name === "Facebook") {
                let url = value.Username;
                socialGrid.addWidget(widgetElements.createFacebookWidget("Facebook feed", url), 1, 1, rowSpan, 6, true, 4, 12, 4, 12, 1);
                socialGrid.movable(".grid-stack-item", false);
                socialGrid.resizable(".grid-stack-item", false);
            }
        });
        if (itempage) loadStories(itemId, rowSpan);
    };
    
    //Retrieves item for social widget..
    let loadItemForSocialWidget = function (itemId) {
        $.ajax({
            method: "GET",
            url: "/api/GetItemWithDetails/" + itemId,
            success: data => loadSocialWidget(data)
        });
    };

    //Adds a widget to the gridstack grid.
    let addWidgetToGrid = function(widget, itemId, index) {
        //UserWidget
        if (widget.DashboardId !== -1) {
            grid.addWidget(widgetElements.createUserWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                true, 4, 12, 5, 12, widget.WidgetId);
            //ItemWidget
        } else {
            if (index === 0) {
                mentionsGrid.addWidget(widgetElements.createItemWidget(widget.WidgetId, ""), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                    true, 4, 12, 4, 12, widget.WidgetId);
                mentionsGrid.movable(".grid-stack-item", false);
                mentionsGrid.resizable(".grid-stack-item", false);
            } else {
                grid.addWidget(widgetElements.createItemWidget(widget.WidgetId, ""), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                    true, 4, 12, 4, 12, widget.WidgetId);
                grid.movable(".grid-stack-item", false);
                grid.resizable(".grid-stack-item", false);
            }
        }
        //if widgettype == graphtype
        if (widget.WidgetType === 1) loadGraphs(itemId, widget);
        widgets.push(widget);
    };

    //Puts all the widgets on the grid.
    let loadGrid = function (data, itemId) {
        if (data != null && data.length) {
            $.each(data, (index, widget) => {
                if (!onlyLoadLastWidget) addWidgetToGrid(widget, itemId, index);
                else if (index === data.length-1) addWidgetToGrid(widget, itemId, index);
            });
        } else {
            if (dashboardpage) noWidgetsAvailable();
        }
        if (itempage && !orgpage) loadItemForSocialWidget(itemId);
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
    itempage = $(".item-page").length;
    orgpage = $(".organisation-page").length;
    dashboardpage = $(".dashboard-page").length;

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
        let serializedWidget = {
            WidgetId: widget.id ? widget.id : widget.WidgetId,
            GraphType: widget.GraphType ? widget.GraphType : 0,
            ItemIds: widget.ItemIds ? widget.ItemIds : []
        };
        $.ajax({
            type: "POST",
            url: "/api/MoveWidget/",
            data:  JSON.stringify(serializedWidget),
            dataType: "application/json",
            contentType: "application/json",
            success: () => showSaveMessage(),
        }).fail((e) => {
            let response = JSON.parse(e.responseText);
            if (response.Message.toLowerCase().includes("authorization")) {
                $("#loginmodal").modal("show");
            } else {
                showErrorMessage();
            }
        })
    };
    
    //Adds a new widget to the current dashboard.
    addWidgetToDashboard = function (json) {
        json.GraphType = convertChartTypeToGraphType(json.GraphType);
        json = JSON.stringify(json);
        $modal = $(".makeGraphModal");
        $nowidgets = $(".no-widgets");
        $.ajax({
            type: "POST",
            url: "/api/newWidget/",
            data: json,
            dataType: "application/json",
            contentType: "application/json",
            success: () => {
                if ($modal.length) $modal.modal("hide");
                if ($nowidgets.length) $nowidgets.hide();
                if (dashboardpage) {
                    if (widgets.length < 1) loadWidgets("api/WidgetApi/GetUserWidgets", "");
                    else loadWidgets("api/WidgetApi/GetUserWidgets", "", true);
                }
                showSaveMessage();
            }
        }).fail((e) => {
            let response = JSON.parse(e.responseText);
            if (response.Message.toLowerCase().includes("authorization")) {
                $("#loginmodal").modal("show");
            } else {
                showErrorMessage();
            }
        })
    };
    
    //Updates given widgets (wto works as a buffer for a lot
    //TODO: only update if widget has changes.
    var wto;
    updateWidgets = function (widgets) {
        clearTimeout(wto);
        wto = setTimeout(function () {
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
        }, 1000);
    };
    
    //Removes a widget
    deleteWidget = function (e) {
        let widget = (e.target).closest(".grid-stack-item");
        let widgetId = e.target.id;
        if (widgetId.length) {
            $.ajax({
                type: "POST",
                url: "/api/WidgetApi/Delete/" + widgetId,
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
    
    //start loading widgets
    if (dashboardpage) loadWidgets("api/WidgetApi/GetUserWidgets", "");
}

