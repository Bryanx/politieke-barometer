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
        '                        <li>' +
        '                            <a class="close-widget">' +
        '                                <i id=' + id + ' class="fa fa-close"></i>' +
        '                            </a>' +
        '                        </li>' +
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
        '                    <div class="pull-right">' +
        '                       <button class="addToDashboard btn btn-dark btn-xs">Add to dashboard</button>' +
        '                   </div>' +
        '                    <div class="clearfix"></div>' +
        '                </div>' +
        '                <button id=' + id + ' class="chartShowLines btn btn-default btn-xs">Show lines</button>' +
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

function ShowLines(e) {
    var id = e.target.id;
    let chart = charts.find(c => c.config.id == id);
    if (chart.options.showLines) chart.options.showLines = false;
    else chart.options.showLines = true;
    chart.legend.options.enabled = true;
    chart.update();
    console.log(chart);
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
                fill: false
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
            //UserWidget
            if (widget.DashboardId !== undefined) {
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
    $(document).on('click', '.chartShowLines', (e) => ShowLines(e));

    //dashboard handlers
    $('#btnAddLine').click(this.btnAddLineChart);
    $('#btnAddPie').click(this.btnAddPieChart);
    $('#btnAddBar').click(this.btnAddBarChart);
    $(document).on('click', '.close-widget', (e) => this.deleteWidget(e));
    $('.grid-stack').on('change', (event, items) => this.updateWidgets(items));

    //itempage handlers
    $(document).on('click', '.addToDashboard', () => this.createWidget());
}

