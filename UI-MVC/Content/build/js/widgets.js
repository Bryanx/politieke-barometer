$(document).ready(init);

var counter = 0;
var primary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--primary-color');
var secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--secondary-color');
var tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--tertiary-color');


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

function addLineChart(widgetId, chartData) {
    var ret = [];
    $.each(chartData, (k, v) => {
        ret.push({
            day: k,
            mentions: v
        })
    });
    console.log(ret);
    var id = "graph" + widgetId;
    addCSSgrid(id);
    Morris.Line({
        // ID of the element in which to draw the chart.
        element: id,
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: ret,
        // The name of the data record attribute that contains x-values.
        xkey: 'day',
        // A list of names of data record attributes that contain y-values.
        ykeys: ['mentions'],
        // Labels for the ykeys -- will be displayed when you hover over the
        // chart.
        labels: ['Mentions'],
        resize: true
    });
}

function addPieChart(id, labels, values) {
    addCSSgrid(id);
    Morris.Donut({
        element: id,
        resize: true,
        data: [
            {label: labels[0], value: values[0]},
            {label: labels[1], value: values[1]},
            {label: labels[2], value: values[2]},
            {label: labels[3], value: values[3]}
        ],
        colors: [primary_color, secondary_color, '#ACADAC', tertiary_color]
    });
}

function addBarChart(id) {
    addCSSgrid(id);
    Morris.Bar({
        element: id,
        data: [{
            period: "2016-10-01",
            licensed: 807,
            sorned: 660
        }, {
            period: "2016-09-30",
            licensed: 1251,
            sorned: 729
        }, {
            period: "2016-09-29",
            licensed: 1769,
            sorned: 1018
        }, {
            period: "2016-09-20",
            licensed: 2246,
            sorned: 1461
        }, {
            period: "2016-09-19",
            licensed: 2657,
            sorned: 1967
        }, {
            period: "2016-09-18",
            licensed: 3148,
            sorned: 2627
        }, {
            period: "2016-09-17",
            licensed: 3471,
            sorned: 3740
        }, {
            period: "2016-09-16",
            licensed: 2871,
            sorned: 2216
        }, {
            period: "2016-09-15",
            licensed: 2401,
            sorned: 1656
        }, {
            period: "2016-09-10",
            licensed: 2115,
            sorned: 1022
        }],
        xkey: "period",
        barColors: [primary_color, "#34495E", "#ACADAC", "#3498DB"],
        ykeys: ["licensed", "sorned"],
        labels: ["Licensed", "SORN"],
        hideHover: "auto",
        xLabelAngle: 60,
        resize: !0
    });
}

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
        '                <div id="' + id + '"></div>' +
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
        '                <div id="graph' + id + '"></div>' +
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

            // if (widget.Graph != null) {
                $.ajax({
                    type: 'GET',
                    url: '/api/GetGraphs/' + itemId +'/' + widget.WidgetId,
                    dataType: 'json',
                    success: data2 => addLineChart(widget.WidgetId, data2)
                }).fail();
            // }
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

    //dashboard handlers
    $('#btnAddLine').click(this.btnAddLineChart);
    $('#btnAddPie').click(this.btnAddPieChart);
    $('#btnAddBar').click(this.btnAddBarChart);
    $(document).on('click', '.close-widget', (e) => this.deleteWidget(e));
    $('.grid-stack').on('change', (event, items) => this.updateWidgets(items));

    //itempage handlers
    $(document).on('click', '.addToDashboard', () => this.createWidget());
}

