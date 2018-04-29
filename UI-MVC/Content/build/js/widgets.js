$(document).ready(init);

var counter = 0;
var primary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--primary-color');
var secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--secondary-color');
var tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--tertiary-color');


/**
 * The basic HTML structure of a widget
 */
function createUserWidget(id, title) {
    return '<div class="chart-container">' +
        '            <div class="x_panel grid-stack-item-content bg-white no-scrollbar">' +
        '                <div class="x_title">' +
        '                    <h2 class="graphTitle">' + title + '</h2>' +
        '                    <ul class="nav navbar-right panel_toolbox">' +
        '                       <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>' +
        '                       <li class="dropdown">' +
        '                       <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>' +
        '                       <ul class="dropdown-menu" role="menu">' +
        '                           <li><a href="#">Settings 1</a></li>' +
        '                           <li><a href="#">Settings 2</a></li>' +
        '                       </ul>' +
        '                       </li>' +
        '                       <li>' +
        '                            <a class="close-widget">' +
        '                                <i id=' + id + ' class="fa fa-close"></i>' +
        '                            </a>' +
        '                       </li>' +
        '                    </ul>' +
        '                    <div class="clearfix"></div>' +
        '                </div>' +
        '                <div style="position: relative; height: 85%;"><canvas id="graph' + id + '"></canvas></div>' +
        '            </div>' +
        '        </div>'
}

function createItemWidget(id, title) {
    return '<div data-widget-id=' + id + ' class="chart-container">' +
        '            <div class="x_panel grid-stack-item-content bg-white no-scrollbar">' +
        '                <div class="x_title">' +
        '                    <h2 class="graphTitle">' + title + '</h2>' +
        '                    <ul class="nav navbar-right panel_toolbox">' +
        '                       <li>' +
        '                            <button class="addToDashboard btn btn-dark">Add to dashboard</button>' +
        '                       </li>' +
        '                       <li class="dropdown">' +
        '                       <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-gear"></i></a>' +
        '                       <ul class="dropdown-menu" role="menu">' +
        '                           <li><a id=' + id + ' class="toggleChartType">Bar chart</a></li>' +
        '                           <li><a id=' + id + ' class="chartShowLines">Lines between points</a></li>' +
        '                           <li><a id=' + id + ' class="chartShowXGrid">X grid lines</a></li>' +
        '                           <li><a id=' + id + ' class="chartShowYGrid">Y grid lines</a></li>' +
        '                           <li><a id=' + id + ' class="chartShowLogScale">Logarithmic y-axes</a></li>' +
        '                           <li><a id=' + id + ' class="chartShowLegend">Legend</a></li>' +
        '                       </ul>' +
        '                       </li>' +
        '                    </ul>' +
        '                    <div class="clearfix"></div>' +
        '                </div>' +
        '                <div style="position: relative; height: 85%;"><canvas id="graph' + id + '"></canvas></div>' +
        '            </div>' +
        '        </div>'
}

var gridselector = $('#grid');
gridselector.gridstack({
    resizable: {
        handles: 'e, se, s, sw, w'
    }
});

var grid = gridselector.data('gridstack');

function addCSSgrid(id) {
    let elem = $('#' + id);
    elem.css('width', 'auto');
    elem.css('height', '85%');
}

function noWidgetsAvailable() {
    $('.no-widgets').show();
}

function showSaveMessage() {
    $('#notificationMessage')
        .addClass('bg-success')
        .html('<i class="fa fa-check-circle"></i> ' + Resources.Saved)
        .show()
        .delay(3000)
        .slideUp("slow", "swing", function () {
            $(this).removeClass().empty();
        });
}

function showErrorMessage() {
    $('#notificationMessage')
        .addClass('bg-error')
        .html('<i class="fa fa-times-circle"></i> ' + Resources.Failed)
        .show()
        .delay(3000)
        .slideUp("slow", "swing", function () {
            $(this).removeClass().empty();
        });
}

var charts = [];

function ToggleChartType(e) {
    let id = e.target.id;
    let chart = charts.find(c => c.config.id == id);
    var ctx = document.getElementById("graph"+id).getContext("2d");
    var temp = jQuery.extend(true, {}, chart.config);

    //change chart type
    let type = chart.config.type;
    type = type === 'line' ? 'bar' : 'line';
    
    //Remove chart from charts and delete it.
    charts.splice(charts.findIndex(c => c.config.id == id), 1);
    chart.destroy();

    temp.type = type; // The new chart type
    charts.push(new Chart(ctx, temp));
    
    //change text on button
    let text = $('.toggleChartType').html();
    text = text === 'Bar chart' ? 'Line chart' : 'Bar chart';
    $('.toggleChartType').html(text);
}

function ShowLines(e) {
    let chart = charts.find(c => c.config.id == e.target.id);
    chart.options.showLines = !chart.options.showLines;
    chart.update();
}

function ShowXGrid(e) {
    let chart = charts.find(c => c.config.id == e.target.id);
    chart.options.scales.xAxes[0].gridLines.display = !chart.options.scales.xAxes[0].gridLines.display;
    chart.update();
}

function ShowYGrid(e) {
    let chart = charts.find(c => c.config.id == e.target.id);
    chart.options.scales.yAxes[0].gridLines.display = !chart.options.scales.yAxes[0].gridLines.display;
    chart.update();
}

function ShowLogScale(e) {
    let id = e.target.id;
    let chart = charts.find(c => c.config.id == id);
    let type = chart.options.scales.yAxes[0].type;
    type = type === 'linear' ? 'logarithmic' : 'linear';
    chart.options.scales.yAxes[0].type = type;
    let text = $('.chartShowLogScale').html();
    text = text === 'Linear y-axes' ? 'Logarithmic y-axes' : 'Linear y-axes';
    $('.chartShowLogScale').html(text);
    chart.update();
}

function ShowLegend(e) {
    let chart = charts.find(c => c.config.id == e.target.id);
    chart.options.legend.display = !chart.options.legend.display;
    chart.update();
}

function addLineChartJS(widgetId, chartData) {
    charts.push(new Chart(document.getElementById("graph"+widgetId), {
        id: widgetId,
        type: 'line',
        data: {
            labels: Object.keys(chartData),
            datasets: [{
                data: Object.values(chartData),
                label: Resources.Mentions,
                borderColor: "#3e95cd",
                fill: false,
                borderColor: "purple",
                backgroundColor: "purple"
            }],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    }));
}


function loadGrid(data, itemId) {
    if (data != null && data.length) {
        $.each(data, (index, widget) => {
            //UserWidgets
            console.log(data);
            if (widget.DashboardId !== -1) {
                grid.addWidget(createUserWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                    false, 4, 12, 4, 12, widget.WidgetId);
                //ItemWidget
            } else {
                grid.addWidget(createItemWidget(widget.WidgetId, widget.Title), widget.RowNumber, widget.ColumnNumber, widget.RowSpan, widget.ColumnSpan,
                    true, 4, 12, 4, 12, widget.WidgetId);
                grid.movable('.grid-stack-item', false);
                grid.resizable('.grid-stack-item', false);
            }

            $.ajax({
                type: 'GET',
                url: '/api/GetGraphs/' + itemId + '/' + widget.WidgetId,
                dataType: 'json',
                success: data2 => addLineChartJS(widget.WidgetId, data2)
            }).fail();
            counter++;
        });
    } else {
        noWidgetsAvailable();
    }
}

function loadWidgets(url, itemId) {
    $.ajax({
        type: 'GET',
        url: url + itemId,
        dataType: 'json',
        success: data => loadGrid(data, itemId),
        error: (xhr) => alert(xhr.responseText)
    })
}

var count = 0;

function init() {
    //add widget with graph
    this.btnAddLineChart = function () {
        grid.addWidget(createUserWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addLineChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);

    this.btnAddPieChart = function () {
        grid.addWidget(createUserWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addPieChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);

    this.btnAddBarChart = function () {
        grid.addWidget(createUserWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addBarChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);

    //CRUD:
    this.createWidget = function () {
        let widgetId = $('.addToDashboard').parents(".chart-container").data("widget-id");
        $.ajax({
            type: 'POST',
            url: '/api/MoveWidget/' + widgetId,
            dataType: 'json',
            success: () => showSaveMessage()
        }).fail(() => showFailedMessage());
    };

    this.updateWidgets = function (items) {
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
            type: 'PUT',
            url: '/api/Widget/',
            data: JSON.stringify(serializedItems),
            dataType: 'application/json',
            contentType: 'application/json',
            success: () => showSaveMessage(),
            error: (xhr) => {
                alert($.parseJSON(xhr.responseText).Message);
                showErrorMessage();
            }
        })
    };

    this.deleteWidget = function (e) {
        let el = (e.target).closest('.grid-stack-item');
        gridselector.data('gridstack').removeWidget(el);
        $.ajax({
            type: 'DELETE',
            url: '/api/Widget/Delete/' + e.target.id,
            dataType: 'json',
            error: (xhr) => {
                alert($.parseJSON(xhr.responseText).Message);
                showErrorMessage();
            },
            success: () => {
                if (!$('.grid-stack-item').length) noWidgetsAvailable();
                showSaveMessage();
            }
        })
    };

    //Graph handlers
    $(document).on('click', '.toggleChartType', (e) => ToggleChartType(e));
    $(document).on('click', '.chartShowLines', (e) => ShowLines(e));
    $(document).on('click', '.chartShowXGrid', (e) => ShowXGrid(e));
    $(document).on('click', '.chartShowYGrid', (e) => ShowYGrid(e));
    $(document).on('click', '.chartShowLogScale', (e) => ShowLogScale(e));
    $(document).on('click', '.chartShowLegend', (e) => ShowLegend(e));

    //dashboard handlers
    $('#btnAddLine').click(this.btnAddLineChart);
    $('#btnAddPie').click(this.btnAddPieChart);
    $('#btnAddBar').click(this.btnAddBarChart);
    $(document).on('click', '.close-widget', (e) => this.deleteWidget(e));
    $('.grid-stack').on('change', (event, items) => this.updateWidgets(items));

    //itempage handlers
    $(document).on('click', '.addToDashboard', () => this.createWidget());
}

