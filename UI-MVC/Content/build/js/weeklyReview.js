var charts = [];

// Adding Chart
let AddChart = function (name, widgetId, labels, values, chartType="line") {
    let el = "user-weekly-" + widgetId;
    charts.push(new Chart(document.getElementById(el), {
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
    }));
};


var GetGraph = function(name, itemId, widgetId) {
    $.ajax({
        type: "GET",
        url: "/api/GetGraphs/" + itemId + "/" + widgetId,
        dataType: "json",
        success: data => {
            if (charts.each(charts => charts.config.id === widgetId)) {
                AddDataSet(charts.where(chart => chart.config.id === widgetId), name, data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14));
            } else {
                AddChart(name, data[0].WidgetId, data[0].GraphValues.map(g => g.Value).reverse().slice(-14), data[0].GraphValues.map(g => g.NumberOfTimes).reverse().slice(-14))
            }
        },
        fail: d => console.log(d)
    })
};