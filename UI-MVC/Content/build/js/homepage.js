
function checkScroll(){
    let startY = $('.nav_menu').height() * 2; //The point where the navbar changes in px

    if($(window).scrollTop() > startY){
        $('.nav_menu').addClass("scrolled");
    }else{
        $('.nav_menu').removeClass("scrolled");
    }
}

if($('.nav_menu').length > 0){
    $(window).on("scroll load resize", function(){
        checkScroll();
    });
}