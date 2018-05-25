(() => {
    $('#datatable-buttons').dataTable({
        "paging": true,
        dom: "lBfrtip",
        buttons: [
            'copy', 'excelHtml5', 'pdfHtml5', 'csvHtml5'
        ]
    });

})($);

$('.accountActivityChange').click(function() {
    var $this = $(this);
    var id = $this.data('item-id');
    $.ajax({
        type: 'POST',
        url: '/api/Admin/ToggleAccountActivity/' + id
    }).fail(() => {/* ok */ })
        .done(function () {
            $this.toggleClass("btn-danger btn-success")
                .toggleText(Resources.Recover, Resources.Remove)
                .parent().parent().toggleClass("greyed-out");
        });
});

function changeRole(id, htmlId) {
    $.ajax({
        type: "POST",
        url: "/api/Admin/ChangeRole/" + id,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: JSON.stringify($("#dd" + htmlId + " option:selected").text())
    }).fail(() => {/* ok */ })
        .done(function () {
            $("#td2" + htmlId).text($("#dd" + htmlId + " option:selected").text());
        });
    event.preventDefault();
}