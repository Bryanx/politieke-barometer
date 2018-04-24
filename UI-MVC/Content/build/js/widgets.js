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

function addLineChart(id) {
    addCSSgrid(id);
    Morris.Line({
        // ID of the element in which to draw the chart.
        element: id,
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: [
            {year: '2008', value: 20},
            {year: '2009', value: 10},
            {year: '2010', value: 5},
            {year: '2011', value: 5},
            {year: '2012', value: 20}
        ],
        // The name of the data record attribute that contains x-values.
        xkey: 'year',
        // A list of names of data record attributes that contain y-values.
        ykeys: ['value'],
        // Labels for the ykeys -- will be displayed when you hover over the
        // chart.
        labels: ['Value'],
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

function addNodeboxGraph(id) {
    addCSSgrid(id);
    createNodebox(id);
    
}


/**
 * Nodebox api
 */

function createNodebox(id) {
    let nodeId = "canvas" + id;
    let node = document.createElement('canvas');
    node.id = nodeId;
    node.width = document.getElementById(id).clientWidth;
    node.height = document.getElementById(id).clientHeight;
    document.getElementById(id).appendChild(node);
    
    var canvas = {
        userId: 'AnthonyT',
        projectId: 'tutorial',
        functionId: 'circle_graph',
        canvasId: "canvas" + id,
        autoplay: true
    };

    // Initialize the NodeBox player object
    ndbx.embed(canvas, function(err, player) {
        if (err) {
            throw new Error(err);
        } else {
            window.player = player;
        }
    });
    
}


/**
 * The basic HTML structure of a widget
 */
function createWidget(id, title) {
    return '<div class="chart-container">' +
        '            <div class="x_panel grid-stack-item-content bg-white no-scrollbar">' +
        '                <div class="x_title">' +
        '                    <h2 class="graphTitle">' + title + '</h2>' +
        '                    <ul class="nav navbar-right panel_toolbox">' +
        '                        <li>' +
        '                            <a class="collapse-link">' +
        '                                <i class="fa fa-chevron-up"></i>' +
        '                            </a>' +
        '                        </li>' +
        '                        <li class="dropdown">' +
        '                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">' +
        '                                <i class="fa fa-wrench"></i>' +
        '                            </a>' +
        '                            <ul class="dropdown-menu" role="menu">' +
        '                                <li>' +
        '                                    <a href="#">Settings 1</a>' +
        '                                </li>' +
        '                                <li>' +
        '                                    <a href="#">Settings 2</a>' +
        '                                </li>' +
        '                            </ul>' +
        '                        </li>' +
        '                        <li>' +
        '                            <a id="close-widget">' +
        '                                <i class="fa fa-close"></i>' +
        '                            </a>' +
        '                        </li>' +
        '                    </ul>' +
        '                    <div class="clearfix"></div>' +
        '                </div>' +
        '                <div id="' + id + '"></div>' +
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


function loadGrid(data) {
    for (var i = 0; i < data.length; i++) {
        grid.addWidget(createWidget('grafiek' + counter, data[i].Title),
            data.x, data[i].y, data[i].width, data[i].height, true, 4, 12, 4);
        if (data[i].Graph != null) {
            switch (data[i].Graph.Type) {
                case "donut" :
                    addPieChart('grafiek' + counter);
                    break;
                case "line" :
                    addLineChart('grafiek' + counter);
                    break;
                case "bar" :
                    addBarChart('grafiek' + counter);
                    break;
                case "nodebox" :
                    addNodeboxGraph('grafiek' + counter);
                    break;
            }
        }
        counter++;
    }
}

var count = 0;

function saveGrid() {
    var titles = $('.graphTitle');
    grid.serializedData = _.map($('.grid-stack > .grid-stack-item:visible'), function (el) {
        el = $(el);
        var node = el.data('_gridstack_node');
        return {
            Id: node._id,
            Title: $(titles[count++]).html(),
            X: node.x,
            Y: node.y,
            Width: node.width,
            Height: node.height
        };
    }, this);
    var JSONdata = JSON.stringify(grid.serializedData, null, '    ');
    console.log(grid.serializedData);
    return false;
}

function init() {
    //close widget
    $(document).on('click', '#close-widget', function (e) {
        e.preventDefault();
        let el = $(this).closest('.grid-stack-item');
        $('#grid').data('gridstack').removeWidget(el);
    });

    //add widget with graph
    this.btnAddLineChart = function () {
        grid.addWidget(createWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addLineChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);
    this.btnAddPieChart = function () {
        grid.addWidget(createWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addPieChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);
    this.btnAddBarChart = function () {
        grid.addWidget(createWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addBarChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);
    this.btnAddNodebox = function () {
        grid.addWidget(createWidget('grafiek' + counter), 0, 0, 6, 6, true, 4, 12, 4);
        addNodeboxGraph('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);

    //handlers
    $('#btnAddLine').click(this.btnAddLineChart);
    $('#btnAddPie').click(this.btnAddPieChart);
    $('#btnAddBar').click(this.btnAddBarChart);
    $('#btnAddNodebox').click(this.btnAddNodebox);
    
}

//TODO: Add persistence and import functionality
// $(function () {
//         this.clearGrid = function () {
//             this.grid.removeAll();
//             return false;
//         }.bind(this);
//
//         $('#save-grid').click(this.saveGrid);
//         $('#load-grid').click(this.loadGrid);
//         $('#clear-grid').click(this.clearGrid);
//
//         this.loadGrid();
//     };
// });
    

