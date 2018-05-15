
(() => {
    //When a languaged is clicked, submit the form.
    $(".culture").click(function () {
        $(this).parents("form").submit();
    });
    $(".avatar-view").css("height", $(".avatar-view").width());
    $(".related-img-view").css("height", $(".related-img-view").width());
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
