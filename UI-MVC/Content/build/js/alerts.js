//global variables
var res = {
    alertLoader: $("<div />", { class: "loadingAlerts" }),
    alertcount: 0,
    subscriptionAmount: $('.subscription').length
};

//Alerts:
//Updates the alert count icon
function updateAlertCount() {
    if (res.alertcount <= 0) {
        $('#alertCount').hide();
    } else {
        $('#alertCount').show();
        $('#alertCount').html(res.alertcount);
    }
}

function removeAlert(alertId) {
    $.ajax(`/api/User/Alert/${alertId}/Delete`,
        {
            type: 'DELETE',
            dataType: 'json'
        })
        .done(() => loadAlerts())
        .fail(() => alert('Cannot change alert.'));
}

//When an alert is clicked, this function is called.
//It sets the property isRead of the Alert to true.
function markAsRead(id) {
    var alert = $(`#alert${id}`);
    if (!alert.hasClass('alertRead')) {
        alert.addClass('alertRead');
        res.alertcount = res.alertcount - 1;
        updateAlertCount();
    }
    $.ajax(`/api/User/Alert/${id}/Read`,
        {
            type: 'PUT',
            dataType: 'json'
        })
        .done(() => { /*ok*/ })
        .fail(() => alert('Cannot change alert.'));
}

//Generates the HTML for one alert item in the dropdown alert menu
function generateAlertHTML(alert) {
    var read = "";
    if (alert.IsRead) {
        read = "alertRead";
    }

    var alertItem = $("<li />",
        {
            "class": `alertMessage${alert.AlertId} ${read}`,
            onclick: `markAsRead(${alert.AlertId})`
        });

    var alertCloseButton = $("<div />",
        {
            "class": "alertClose",
            title: Resources.RemoveAlert
        }).append($("<a />",
        {
            "class": "fa fa-close",
            onclick: `removeAlert(${alert.AlertId})`

        }));
    var alertBody = $("<a />",
        {
            href: `/Person/Details/${alert.AlertId}`
        });
    var alertIcon = $("<div />",
        {
            "class": "alertIcon"
        }).append($("<i />",
        {
            "class": "fa fa-user"
        }));
    var alertContent = $("<span />",
        {
            "class": "font_16",
            html: `<strong>${alert.Name}</strong> ${Resources.IsNowTrending}`
        });
    var alertTime = $("<span />",
        {
            "class": "alertTime",
            html: `<i class="font_11 fa fa-clock-o"></i> ${jQuery.timeago(alert.TimeStamp)}`
        });
    alertBody.append(alertIcon).append(alertContent).append(alertTime)
    alertItem.append(alertCloseButton).append(alertBody);
    return alertItem;
}


//Inserts the HTML for the alerts inside the alert dropdownlist in the top-navbar
function InsertAlerts(alertData) {
    $('#alertMenu').empty();
    var counter = 0;
    $.each(alertData,
        (key, value) => {
            var alertItem = generateAlertHTML(value);

            //Add alert to alert dropdownlist
            $('#alertMenu').append(alertItem);

            if (!value.IsRead) {
                counter++;
            }
        });

    //Update the alertCounter
    res.alertcount = counter;
    updateAlertCount();
}

//Ajax GET request: loads all alerts and calls InsertAlerts() method to add HTML to the page.
function loadAlerts() {
    //show loading gif
    $(".loadingAlerts").css("display", "block");
    $.ajax({
        type: 'GET',
        url: '/api/User/GetAlerts',
        dataType: 'json',
        error: function(xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    })
        .done(data => {
            //hide loading gif
            $("#alertMenu").find(res.alertLoader).remove();
            //There are alerts available
            if (data != undefined) {
                var alertData = data;
                console.log(alertData);
                InsertAlerts(data);
                $('#alertMenu').append(
                    "<li id=\"seeAllAlerts\"><div class=\"text-center\"><a><strong>"+Resources.ShowAllAlerts+"</strong></a></div></li>");
            } else { //No available alerts
                $('#alertMenu').empty();
                $('#alertMenu').append("<li class=\"noAlertsAvailable\"><i class=\"fa fa-bell\"></i></br>" +
                    `<strong>${Resources.YouHaveNoAlerts}</strong></br>` +
                    `${Resources.NoAlertsMessage}</li>`);
            }
        });
}


(() => {
    loadAlerts();
    //Everytime the alert button is clicked, they are reloaded.
    $('#alertDropdown').click(loadAlerts);
})(jQuery);