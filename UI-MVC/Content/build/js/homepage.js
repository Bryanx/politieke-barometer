/* COLORS*/
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

/* ************* HOMEPAGE ************** */
/* Checking how much you have scrolled. if it is past 60% of the screen or 500px then show the navbar otherwise hide it */
function checkScroll() {

    if (($(window).height() * 0.6) > 500) {
        var startY = 500; //The point where the navbar changes in px
    } else {
        var startY = $(window).height() * 0.66; //The point where the navbar changes in px
    }

    if ($(window).scrollTop() > startY) {
        $('nav').css("background-color", "rgba(255,255,255,1)")
            .css("box-shadow", "0px 3px 3px -2px rgba(0, 0, 0, 0.05)");
        $('.navbar-right > li > a > i').css("color", "#73879C");
        $('.navbar-right > li > a > span').css("color", "#73879C");
        $('nav .searchbar').css('margin-top', '4px');
        $('.nav-home').css('display', 'block');
        $('.nav.navbar-nav > li > a.signup').css("background-color", "#73879C")
    } else {
        $('nav').css("background-color", "rgba(255,255,255,0)")
            .css("box-shadow", "0px 1px 2px 2px rgba(0, 0, 0, 0)");
        $('.navbar-right > li > a > i').css("color", "white");
        $('.navbar-right > li > a > span').css("color", "white");
        $('nav .searchbar').css('margin-top', '-50px');
        $('.nav-home').css('display', 'none');
        $('.nav.navbar-nav > li > a.signup').css("background-color", "rgba(0, 0, 0, 0.1)")
    }
}

if ($('.main-header-container').length) {
    $(window).on("scroll load resize", function () {
        checkScroll();
    });
}

/* ---------- Twitter feed ----------*/

let TwitterFeed = function (trendings) {
    $.each(trendings, (index,  value) => {

        let name = value.SocialMediaNames[0].Username[0] === "@" ? value.SocialMediaNames[0].Username.slice(1) : value.SocialMediaNames[0].Username;
        let nameId = "#t-name-" + (index + 1);
        let id = "twitter-feed-" + (index +1);
        // putting name above twitter feed
        $(nameId).append("" + value.Name + " ")
            .next()
            .append("" + value.TrendingPercentage.toFixed(2) + "%")
            .parent()
            .attr("href", "/Person/Details/" + value.ItemId);
        
        twttr.widgets.createTimeline(
            {
                sourceType: "profile",
                screenName: name
            },
            document.getElementById("" + id),
            {
                chrome: "noheader, noborder, nofooter",
                linkColor: primary_color,
                tweetLimit: 5
            }
        );
    });
}

/* ---------- Trending chart ----------*/
var charts = [];
let AddChart = function (name, widgetId, labels, values, chartType="line") {
    charts.push(new Chart(document.getElementById("trending-graph"), {
        id: widgetId,
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                data: values,
                label: name,
                borderColor: COLORS[values[2]],
                backgroundColor: COLORS[values[2]],
                hoverBackgroundColor: DARKCOLORS[values[2]],
                fill: false,
            }],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
        }
    }));
};


let getGraph = function(name, itemId, widgetId) {
    $.ajax({
        type: "GET",
        url: "/api/GetGraphs/" + itemId + "/" + widgetId,
        dataType: "json",
        success: data => {
            if (charts[0] == null) {
                AddChart(name, data[0].WidgetId, data[0].GraphValues.map(g => g.Value).slice(0, 10), data[0].GraphValues.map(g => g.NumberOfTimes).slice(0, 10));
            } else {
                AddDataSet(charts[0], name, data[0].GraphValues.map(g => g.NumberOfTimes).slice(0, 10))
            }
        },
        fail: d => console.log(d)
    })};
    
/*--------- Adding data for trending chart ----------*/    

let AddDataSet = function (chart, name, values) {
    var newColor = COLORS[values[3]]; // TEMPORARY FIX
    var hoverColor = DARKCOLORS[values[3]];
    var newDataset = {
        label: name,
        borderColor: newColor,
        backgroundColor: newColor,
        hoverBackgroundColor: hoverColor,
        data: values,
        fill: false
    };

    chart.config.data.datasets.push(newDataset);
    chart.update();
};

/*---------- getting top 3 trending ----------*/

var GetTopTrending = function (trendings){
    
    $.each(trendings, (index,  value) => {
        $.ajax({
            type: "GET",
            url: 'api/GetItemWidgets/' + value.ItemId,
            dataType: "json",
            success: data => getGraph(value.Name,  value.ItemId, data[0].WidgetId),
            error: (xhr) => alert(xhr.responseText)
        });
    });
};