
//Generic ajax toggle button
var wto;
function ajaxToggleSubscribe(e) {
    $this = $(e.target);
    clearTimeout(wto);
    var id = $this.data('item-id');
    var text = $this.html();
    $this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
    wto = setTimeout(function () {
            $.ajax({
                type: 'POST',
                url: '/api/ToggleSubscribe/' + id,
                success: (e) => {
                    if (e !== undefined && e.Message.toLowerCase().includes("authorization")) {
                        $("#loginmodal").modal("show");
                        $this.html(Resources.Subscribe);
                    } else {
                        if (text === Resources.Subscribe) $this.html(Resources.Unsubscribe);
                        else $this.html(Resources.Subscribe);
                        $this.toggleClass("btn-danger btn-success");
                    }
                }
            })
        },
        500);
}

window.addEventListener('resize', function () {
    $avatar = $(".avatar-view");
    $relatedimg = $(".related-img-view");
    if ($avatar.length) $avatar.css("height", $avatar.width());
    if ($relatedimg.length) $relatedimg.css("height", $relatedimg.width());
});

(() => {
    //When a languaged is clicked, submit the form.
    $(".culture").click(function () {
        $(this).parents("form").submit();
    });
    $(".avatar-view").css("height", $(".avatar-view").width());
    $(".related-img-view").css("height", $(".related-img-view").width());

    $(document).on("click", ".subscribeItem", (e) => ajaxToggleSubscribe(e));
})($)
window.addEventListener('resize', function () {
    $(".avatar-view").css("height", $(".avatar-view").width());
    $(".related-img-view").css("height", $(".related-img-view").width());
});



function convertWizardGraphTypeToChartType(GraphType) {
    switch (GraphType) {
        case 'Line chart' : return "line";
        case 'Bar chart' : return "bar";
        case 'Pie chart' : return "pie";
        case 'Donut chart' : return "donut";
        default : return "line";
    }
}