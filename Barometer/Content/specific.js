$(document).ready(init);
// Highcharts won't automatically set its height and won't adjust
// its width, so we need to set height and call reflow.
function resizeChart(elem) {
    var $elem = $(elem);
    var $chart = $(elem).find('> div > div');
    $chart.find('> div').height($elem.height());
    Highcharts.charts[$chart.data('highchartsChart')].reflow();
}

function initializeChart() {
    Morris.Donut({
        element: 'graphHere',
        resize: true,
        data: [
            {label: "Friends", value: 30},
            {label: "Allies", value: 15},
            {label: "Enemies", value: 45},
            {label: "Neutral", value: 10}
        ]
    });
};

function init() {
    $('#grid').gridstack({
        resizable: {
            handles: 'e, se, s, sw, w'
        }
    });
    var grid = $('#grid').data('gridstack');
    for (var i = 0; i < $('.chart-container').length; i++) {
        // convert elements to gridstack widgets
        grid.makeWidget($('.chart-container')[i]);
    }

    $('.grid-stack').on('gsresizestop', function(event, elem) {
        var width = $(elem).attr('data-gs-width');
        console.log('width: ' + width + ' y: ' + $(elem).attr('data-gs-y'));
    });

    // $('.grid-stack').on('gsresizestop', function(event, elem) {
    //     // update chart width and height on resize
    //     resizeChart(elem);
    // });

    // Build the charts
    initializeChart();

    // set chart size on first load
    // var $elems = $('.chart-container');
    // for (var j = 0; j < $elems.length; j++) {
    //     resizeChart($elems[j]);
    // }
};