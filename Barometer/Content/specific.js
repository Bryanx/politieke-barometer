$(document).ready(init);

var counter = 0;

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

function addPieChart(id) {
    addCSSgrid(id);
    Morris.Donut({
        element: id,
        resize: true,
        data: [
            {label: "Friends", value: 30},
            {label: "Allies", value: 15},
            {label: "Enemies", value: 45},
            {label: "Neutral", value: 10}
        ]
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
        barColors: ["#26B99A", "#34495E", "#ACADAC", "#3498DB"],
        ykeys: ["licensed", "sorned"],
        labels: ["Licensed", "SORN"],
        hideHover: "auto",
        xLabelAngle: 60,
        resize: !0
    });
}

function createWidget(id) {
    return '<div class="chart-container" data-gs-width="4" data-gs-height="4" data-gs-y="0" data-gs-x="0">' +
        '            <div class="x_panel grid-stack-item-content bg-white no-scrollbar">' +
        '                <div class="x_title">' +
        '                    <h2 id="graphTitle">Titel</h2>' +
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

function init() {
    let gridselector = $('#grid');
    gridselector.gridstack({
        resizable: {
            handles: 'e, se, s, sw, w'
        }
    });
    this.grid = gridselector.data('gridstack');

    //close widget
    $(document).on('click', '#close-widget', function (e) {
        e.preventDefault();
        let el = $(this).closest('.grid-stack-item');
        $('#grid').data('gridstack').removeWidget(el);
    });

    //add widget with graph
    this.btnAddLineChart = function () {
        this.grid.addWidget(createWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addLineChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);
    this.btnAddPieChart = function () {
        this.grid.addWidget(createWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addPieChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);  
    this.btnAddBarChart = function () {
        this.grid.addWidget(createWidget('grafiek' + counter), 0, 0, 4, 4, true, 4, 12, 4);
        addBarChart('grafiek' + counter);
        counter++;
        return false;
    }.bind(this);

    //handlers
    $('#btnAddLine').click(this.btnAddLineChart);
    $('#btnAddPie').click(this.btnAddPieChart);
    $('#btnAddBar').click(this.btnAddBarChart);

}

//TODO: Add persistence and import functionality
// $(function () {
//     new function () {
//         this.serializedData = [
//             {x: 0, y: 0, width: 2, height: 2},
//             {x: 3, y: 1, width: 1, height: 2},
//             {x: 4, y: 1, width: 1, height: 1},
//             {x: 2, y: 3, width: 3, height: 1},
//             {x: 1, y: 4, width: 1, height: 1},
//             {x: 1, y: 3, width: 1, height: 1},
//             {x: 2, y: 4, width: 1, height: 1},
//             {x: 2, y: 5, width: 1, height: 1}
//         ];
//
//         this.grid = $('.grid-stack').data('gridstack');
//
//         this.loadGrid = function () {
//             this.grid.removeAll();
//             var items = GridStackUI.Utils.sort(this.serializedData);
//             _.each(items, function (node) {
//                 this.grid.addWidget($('<div><div class="grid-stack-item-content" /><div/>'),
//                     node.x, node.y, node.width, node.height);
//             }, this);
//             return false;
//         }.bind(this);
//
//         this.saveGrid = function () {
//             this.serializedData = _.map($('.grid-stack > .grid-stack-item:visible'), function (el) {
//                 el = $(el);
//                 var node = el.data('_gridstack_node');
//                 return {
//                     x: node.x,
//                     y: node.y,
//                     width: node.width,
//                     height: node.height
//                 };
//             }, this);
//             $('#saved-data').val(JSON.stringify(this.serializedData, null, '    '));
//             return false;
//         }.bind(this);
//
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
    

