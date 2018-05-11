

(() => {
    //When a languaged is clicked, submit the form.
    $(".culture").click(function () {
        $(this).parents("form").submit();
    });
})($)