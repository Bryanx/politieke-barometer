/* COLORS*/
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
        $('.navbar-right > li > a > i').css("text-shadow", 'none');
        $('.navbar-right > li > a > span').css("color", "#73879C");
        $('nav .searchbar').css('margin-top', '4px');
        $('.nav-home').css('display', 'block');
        $('.nav.navbar-nav > li > a.signup').css("background-color", "#73879C");
        $('.nav.navbar-nav > li > a.signup').hover(()=> $('.nav.navbar-nav > li > a.signup').css("border", "2px solid #73879C"));
    } else {
        $('nav').css("background-color", "rgba(255,255,255,0)")
            .css("box-shadow", "0px 1px 2px 2px rgba(0, 0, 0, 0)");
        $('.navbar-right > li > a > i').css("color", "white");
        $('.navbar-right > li > a > i').css("text-shadow", '0 1px 5px '+primary_color);
        $('.navbar-right > li > a > span').css("color", "white");
        $('nav .searchbar').css('margin-top', '-50px');
        $('.nav-home').css('display', 'none');
        $('.nav.navbar-nav > li > a.signup').css("background-color", "rgba(0, 0, 0, 0.1)")
        $('.nav.navbar-nav > li > a.signup').hover(()=> $('.nav.navbar-nav > li > a.signup').css("border", "2px solid #fff"));
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

        let nameId = "#t-name-" + (index + 1);
        if (value.SocialMediaNames[0] == null || value.SocialMediaNames.length == 0){
            $(nameId).append("<p>Geen twitterprofiel gevonden voor " + value.Name + "</p>")
        } else {
            let name = value.SocialMediaNames[0].Username[0] === "@" ? value.SocialMediaNames[0].Username.slice(1) : value.SocialMediaNames[0].Username;
            let id = "twitter-feed-" + (index +1);

            // putting name above twitter feed
            $(nameId).append("" + value.Name + " ")
                .next()
                .append("" + value.TrendingPercentage.toFixed(2) + "%" + " trending")
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
                    tweetLimit: 20
                }
            );
        }
    });
};

/* ---------- Trending chart ----------*/
var charts = [];
let AddChart = function (name, widgetId, labels, values, itemType, chartType="line") {
    let el = "trending-" + itemType + "-graph";
    let c = new Chart(document.getElementById(el), {
        id: widgetId,
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                data: values,
                label: name,
                borderColor: "rgb(30, 143, 190)",
                backgroundColor: "rgb(30, 143, 190)",
                hoverBackgroundColor: "rgb(116, 135, 155)",
                fill: false,
            }],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
        }
    });
    charts.push(c);
        
};


let getGraph = function(name, itemId, widgetId, itemType) {
    $.ajax({
        type: "GET",
        url: "/api/GetGraphs/" + itemId + "/" + widgetId,
        dataType: "json",
        success: data => {
            let d = $('#' + (itemType - 1)).data("widgetId");
            $('#' + (itemType - 1)).data("widgetId", d + " " + itemId);
            switch (itemType){
                case 1:
                    charts.find(c => c.config.id === 0)  === undefined ? AddChart(name, 0, data[0].GraphValues.map(g => g.Value).reverse().slice(-14), data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14), itemType) : AddDataSet(charts.find(c => c.config.id === 0), name, data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14), itemType);
                    break;
                case 2:
                    charts.find(c => c.config.id === 1)  === undefined ? AddChart(name, 1, data[0].GraphValues.map(g => g.Value).reverse().slice(-14), data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14), itemType) : AddDataSet( charts.find(c => c.config.id === 1), name, data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14), itemType);
                    break;
                case 3:
                    charts.find(c => c.config.id === 2)  === undefined ? AddChart(name, 2, data[0].GraphValues.map(g => g.Value).reverse().slice(-14), data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14), itemType) : AddDataSet( charts.find(c => c.config.id === 2), name, data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14), itemType);
                    break;
            }
        },
        fail: d => console.log(d)
    })};
    
/*--------- Adding data for trending chart ----------*/    

let AddDataSet = function (chart, name, values) {
    let random = getRandomColor(chart.config.data.datasets); // Gets random color
    let newDataset = {
        label: name,
        borderColor: COLORS[random],
        backgroundColor: COLORS[random],
        hoverBackgroundColor: DARKCOLORS[random],
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
            success: data => getGraph((value.Name + " " + value.TrendingPercentage.toFixed(0) + "%"),  value.ItemId, data[0].WidgetId, value.ItemType),
        });
    });
};

/*---------- Weekly Review ----------*/

let WeeklyReview = function (name, itemId, widgetId, itemType) {
   
    $.ajax({
        type: "GET",
        url: "/api/GetGraphs/" + itemId + "/" + widgetId,
        dataType: "json",
        success: data => {
            let random = Math.floor(Math.random()*(COLORS.length -1)); // Gets random color
            let el = "weeklyReview-" + itemType;
            let d = $('#' + (itemType+2)).data("widgetId");
            $('#' + (itemType+2)).data("widgetId", d + " " + itemId);
            let c = new Chart(document.getElementById(el), {
                id: (itemType+2),
                type: "line",
                data: {
                    labels: data[0].GraphValues.map(g => g.Value).reverse().slice(-14),
                    datasets: [{
                        data: data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14),
                        label: name,
                        borderColor: COLORS[random],
                        backgroundColor: COLORS[random],
                        hoverBackgroundColor: DARKCOLORS[random],
                        fill: false,
                    }],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                }
            });
            charts.push(c)
        },
        fail: d => console.log(d)
    });
};

let WeeklyReviewAddData = function (name, itemId, widgetId, itemType) {

    $.ajax({
        type: "GET",
        url: "/api/GetGraphs/" + itemId + "/" + widgetId,
        dataType: "json",
        success: data => {
            let c = findingChart(itemType+2);
            AddDataSet(c,  name,  data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14));
        },
        fail: d => console.log(d)
    });
};

/*---------- finding chart ---------- */
let findingChart = function (id) {
  return charts.find(chart => chart.config.id == id);  
};

/*----- Saving widget to dashboard -----*/
let makingJSON = function (e){
    let c = findingChart(e.target.id);
    let list = $('#' + e.target.id).data("widgetId").toString().split(" ");
    let ids = [];
        $.each(list, function( index, value ) {
        if (value !== "") ids.push(parseInt(value)  ) 
    });
       let json = {
        ItemIds: ids, 
        GraphType: c.config.type,
        PropertyTag: "Number of mentions"
    };
    addWidgetToDashboard(json);
    
};


$(document).on("click", ".makeJSON", (e) => makingJSON(e));


(() => {
    $.get({
        url: '/api/GetTopTrendingItems/1',
        dataType: "json",
        success: data => {
            GetTopTrending(data);
            TwitterFeed(data);
        }
    });

    $.get({
        url: '/api/GetTopTrendingItems/2',
        dataType: "json",
        success: data => {
            GetTopTrending(data);
        }
    });

    $.get({
        url: '/api/GetTopTrendingItems/3',
        dataType: "json",
        success: data => {
            GetTopTrending(data);
        }
    });

})($);