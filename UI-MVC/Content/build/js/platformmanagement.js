
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

$("#addSubPlatformForm").on("submit", function (event) {
    event.preventDefault();
    var $this = $(this);
    $.ajax({
        type: $this.attr('method'),
        url: $this.attr('action'),
        data: $this.serialize()
    }).fail(() => {/* ok */ })
        .done(function (data) {
            $("#datatable-responsive > tbody:last-child").append("<tr><td>" + $("#name").val() + "</td><td>0</td><td>0</td></tr>");
            $("#name").val("");
        });
});