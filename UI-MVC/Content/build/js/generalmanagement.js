
$("#synchronizeForm").on("submit", function (event) {
    submitForm($(this), event, $("#synchronizeMessage"));
});

//Ajax call in generic function so every form can use the same code.
function submitForm($this, event, message) {
    $.ajax({
        type: $this.attr('method'),
        url: $this.attr('action'),
        data: $this.serialize(),
        succes: message
            .addClass('green')
            .html(Resources.Saved)
            .fadeOut(2000,
                function () {
                    $(this)
                        .removeClass()
                        .html("")
                        .css("display", "inline");
                })
    }).fail(() => message
        .addClass('red')
        .html(Resources.Failed)
        .fadeOut(2000,
            function () {
                $(this)
                    .removeClass()
                    .html("")
                    .css("display", "inline");
            }));
    event.preventDefault();
}

$("#addSourceForm").on("submit", function (event) {
    event.preventDefault();
    var $this = $(this);
    $.ajax({
        type: $this.attr('method'),
        url: $this.attr('action'),
        data: $this.serialize()
    }).fail(() => {/* ok */ })
        .done(function (data) {
            $("#datatable-responsive > tbody:last-child").append("<tr><td>" + $("#name").val() + "</td><td>" + $("#site").val() + "</td><td><btn class='sourceChange btn btn-danger btn-xs' id='" + data + "' title="+Resources.RemoveSource+">"+Resources.Remove+"</btn></td></tr>");
            $("#name").val("");
            $("#site").val("");
        });
});

$("#setSynchronizeForm").on("submit", function (event) {
    event.preventDefault();
    var $this = $(this);
    var interval=$("#interval").val();
    var settime = $("#settime").val().replace(':', '')
    $.ajax({
        type: "POST",
        url: "/api/Data/SetSynchronize/" + interval + "/" + settime
    }).fail(() => {/* ok */ })
});

$(document).on("click", ".sourceChange", (function (event) {
    event.preventDefault();
    var $this = $(this);
    var id = $this.attr('id');
    $.ajax({
        type: 'POST',
        url: '/api/Data/DeleteItem',
        data: JSON.stringify(id),
        contentType: 'application/json; charset=utf-8',
        dataType: "json"
    }).fail(() => {/* ok */ })
        .done(function () {
            $this.closest('tr').remove();
        });
}));
$(".removeItem").click(function() {
    var $this = $(this);
    var id = $this.data('source-id');
    var text = $this.html();
    $this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
    wto = setTimeout(function() {
            $.ajax({
                type: 'POST',
                url: '/api/Data/DeleteItem/' + id
            }).fail(() => { alert(Resources.CannotRemoveItem) })
                .done(function() {
                    if (text === Resources.Recover) $this.html(Resources.Remove);
                    else $this.html(Resources.Recover);
                    $this.toggleClass("btn-danger btn-dark");
                });
        },
        500);
});
$(".changeItem").change(function() {
    var $this = $(this);
    var id = document.getElementsByClassName('changeItem')[0].id
    var interval = document.getElementsByClassName('changeItem')[0].value;
    $this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
    wto = setTimeout(function() {
            $.ajax({
                type: 'POST',
                url: '/api/Data/ChangeDataSource/' + id + '/' + interval
            }).fail(() => { alert(Resources.CannotRemoveItem) })
        },
        500);
});

$(".synchronise").click(function() {
    var $this = $(this);
    var id = $this.data('source-id');
    var text = $this.html();
    $this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
    wto = setTimeout(function() {
            $.ajax({
                type: 'GET',
                url: '/api/Data/Synchronize/' + id
            }).fail(() => { alert(Resources.Failed) })
                .done(function() {
                    if (text === Resources.Recover) $this.html(Resources.Remove);
                    else $this.html(Resources.Recover);
                    $this.toggleClass("btn-danger btn-dark");
                });
        },
        500);
});

$("#sendPublicNotificatioForm").on("submit", function (event) {
    event.preventDefault();
    var $this = $(this);
    $.ajax({
        type: $this.attr('method'),
        url: $this.attr('action'),
        data: $this.serialize()
    }).fail(() => {/* ok */ })
        .done(function (data) {
            $("#SendPublicNotificationMessage").addClass('green')
                .html(Resources.Send)
                .fadeOut(2000,
                    function () {
                        $(this)
                            .removeClass()
                            .html("")
                            .css("display", "inline");
                    })
        });
});