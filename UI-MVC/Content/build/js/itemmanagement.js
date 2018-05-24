
(() => {
    $('#datatable-buttons').dataTable({
        "sScrollY": "700px",
        "paging": true,
        dom: "lBfrtip",
        buttons: [
            'copy', 'excelHtml5', 'pdfHtml5', 'csvHtml5'
        ]
    });

    var wto;
    $(".removeItem").click(function() {
        clearTimeout(wto);
        var $this = $(this);
        var id = $this.data('item-id');
        var text = $this.html();
        $this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
        wto = setTimeout(function() {
                $.ajax({
                    type: 'POST',
                    url: '/api/Admin/ToggleDeleteItem/' + id
                }).fail(() => { alert(Resources.CannotRemoveItem) })
                    .done(function() {
                        if (text === Resources.Recover) $this.html(Resources.Remove);
                        else $this.html(Resources.Recover);
                        $this.toggleClass("btn-danger btn-dark");
                    });
            },
            500);
    });

    $(".renameItem").on('input propertychange paste',
        function() {
            clearTimeout(wto);
            var $this = $(this);
            var id = $this.data('item-id');
            $this.addClass("loadMessage");
            wto = setTimeout(function() {
                    $.ajax({
                        type: 'POST',
                        url: '/api/Admin/RenameItem/' + id + '/' + $this.val()
                    }).fail(() => { /**/ })
                        .done(function() {
                            $this.removeClass("loadMessage");
                        });
                },
                1000);
        });
})($);
$(document).ready(function () {
    $('#fileUpload').change(function () {
        $('#btnSubmit').click();
    });
    $('#themeFileUpload').change(function () {
        $('#btnThemeSubmit').click();
    });
});