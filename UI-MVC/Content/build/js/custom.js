
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