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
        $('nav .searchbar').css('margin-bottom', '0');
        $('.nav-home').css('display', 'block');
        $('.nav.navbar-nav > li > a.signup').css("background-color", "#73879C")
    } else {
        $('nav').css("background-color", "rgba(255,255,255,0)")
            .css("box-shadow", "0px 1px 2px 2px rgba(0, 0, 0, 0)");
        $('.navbar-right > li > a > i').css("color", "white");
        $('.navbar-right > li > a > span').css("color", "white");
        $('nav .searchbar').css('margin-bottom', '50px');
        $('.nav-home').css('display', 'none');
        $('.nav.navbar-nav > li > a.signup').css("background-color", "rgba(0, 0, 0, 0.1)")
    }
}

if ($('.main-header-container').length) {
    $(window).on("scroll load resize", function () {
        checkScroll();
    });
}

for (var i = 1; i <4 ; i++) {
    
    var name = $("#t-name-" + i).text().split(" ").join("");
    var id = "twitter-feed-" + i;
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
    
    
}

let AddChart = function (widgetId, labels, values, borderColor="#000", color="#000", darkColor="#000", chartType="line") {
    new Chart(document.getElementById("trending-graph"), {
        id: widgetId,
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                data: values,
                label: "Trending graph",
                borderColor: borderColor,
                backgroundColor: color,
                hoverBackgroundColor: darkColor,
                fill: false,
            }],
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
        }
    });
};


let getGraph = function(itemId, widgetId) {
    $.ajax({
        type: "GET",
        url: "/api/GetGraphs/" + itemId + "/" + widgetId,
        dataType: "json",
        success: data => AddChart(data[0].WidgetId, data[0].GraphValues.map(g => g.Value), data[0].GraphValues.map(g => g.NumberOfTimes)),
        fail: d => console.log(d)
    })}

