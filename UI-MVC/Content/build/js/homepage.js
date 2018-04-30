function checkScroll() {

    if (($(window).height() * 0.6) > 500) {
        var startY = 500; //The point where the navbar changes in px
    } else {
        var startY = $(window).height() * 0.66; //The point where the navbar changes in px
    }

    if ($(window).scrollTop() > startY) {
        $('.nav_menu').css("background-color", "rgba(255,255,255,1)")
            .css("box-shadow", "0px 1px 2px 2px rgba(0, 0, 0, 0.1)");
        $('.navbar-right > li > a > i').css("color", "#73879C");
        $('.navbar-right > li > a > span').css("color", "#73879C");
        $('div.nav_menu .searchbar').css('margin-bottom', '0');
        $('.nav-home').css('transition', '1s ease-in-out')
            .css('display', 'block');
    } else {
        $('.nav_menu').css("background-color", "rgba(255,255,255,0)")
            .css("box-shadow", "0px 1px 2px 2px rgba(0, 0, 0, 0)");
        $('.navbar-right > li > a > i').css("color", "white");
        $('.navbar-right > li > a > span').css("color", "white");
        $('div.nav_menu .searchbar').css('margin-bottom', '50px');
        $('.nav-home').css('transition', '1s ease-in-out')
            .css('display', 'none');
    }
}

if ($('.nav_menu').length > 0) {
    $(window).on("scroll load resize", function () {
        checkScroll();
    });
}