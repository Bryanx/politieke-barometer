/**
 * When a colorpicker is changed, the page CSS updates automatically.
 */
var htmlselector = document.getElementsByTagName('html')[0];
var primary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--primary-color')
    .split(' ').join('');
var secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--secondary-color')
    .split(' ').join('');
var tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--tertiary-color')
    .split(' ').join('');
var bg_color = window.getComputedStyle(document.documentElement).getPropertyValue('--bg-color').split(' ')
    .join('');
var font_color = window.getComputedStyle(document.documentElement).getPropertyValue('--font-color').split(' ')
    .join('');

/*
 * Changes rgb string to hex
 */
function rgb2hex(rgb) {
    rgb = rgb.toString();
    rgb = rgb.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);
    return (rgb && rgb.length === 4)
        ? "#" +
        ("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
        ("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
        ("0" + parseInt(rgb[3], 10).toString(16)).slice(-2)
        : '';
}

/*
 * Updates the color field of the colorpicker and updates the CSS.
 */
function changeColor(variable, color, event) {
    color = rgb2hex(event.color);
    htmlselector.setAttribute("style", variable + ": " + color);
}

/*
 * When a colorpicker is changed css is updated.
 */
$('#cp-primary').colorpicker().on('changeColor', (e) => changeColor("--primary-color", primary_color, e));
$('#cp-secondary').colorpicker().on('changeColor', (e) => changeColor("--secondary-color", secondary_color, e));
$('#cp-tertiary').colorpicker().on('changeColor', (e) => changeColor("--tertiary-color", tertiary_color, e));
$('#cp-bg').colorpicker().on('changeColor', (e) => changeColor("--bg-color", bg_color, e));
$('#cp-font').colorpicker().on('changeColor', (e) => changeColor("--font-color", font_color, e));

/*
 * Initial values of colorpickers are set.
 */
(() => {
    $('#ip-primary').val(primary_color).trigger('change');
    $('#ip-secondary').val(secondary_color).trigger('change');
    $('#ip-tertiary').val(tertiary_color).trigger('change');
    $('#ip-bg').val(bg_color).trigger('change');
    $('#ip-font').val(font_color).trigger('change');
})($);

//Functions for ajax calls
//Customization
function putColor() {
    //Update dataforms
    formdata = $("#form_page_color").serializeArray().reduce(function (a, x) {
        a[x.name] = x.value;
        return a;
    }, {});
    
    let data = {
        PrimaryColor: formdata.PrimaryColor,
        PrimaryDarkerColor: shadeBlendConvert(-0.1, formdata.PrimaryColor),
        PrimaryDarkestColor: shadeBlendConvert(-0.2, formdata.PrimaryColor),
        SecondaryColor: formdata.SecondaryColor,
        SecondaryLighterColor: shadeBlendConvert(0.1, formdata.SecondaryColor),
        SecondaryDarkerColor: shadeBlendConvert(-0.1, formdata.SecondaryColor),
        SecondaryDarkestColor: shadeBlendConvert(-0.2, formdata.SecondaryColor),
        TertiaryColor: formdata.TertiaryColor,
        BackgroundColor: formdata.BackgroundColor,
        TextColor: formdata.TextColor
    };
    console.log(data);

    //Do ajax call
    $.ajax({
        type: "POST",
        url: "/api/Customization/PutColor",
        data: JSON.stringify(data),
        dataType: "application/json",
        contentType: "application/json",
    }).fail(() => {/* ok */ })
        .done(() => {/* ok */ })
}
function putAlias() {
    //Update dataforms
    formdata = $("#form_page_text").serializeArray().reduce(function (a, x) {
        a[x.name] = x.value;
        return a;
    }, {});

    let data = {
        PersonAlias: formdata.PersonAlias,
        PersonsAlias: formdata.PersonsAlias,
        OrganisationAlias: formdata.OrganisationAlias,
        OrganisationsAlias: formdata.OrganisationsAlias,
        ThemeAlias: formdata.ThemeAlias,
        ThemesAlias : formdata.ThemesAlias
    };

    //Do ajax-call
    $.ajax({
        type: "POST",
        url: "/api/Customization/PutAlias",
        data: JSON.stringify(data),
        dataType: "application/json",
        contentType: "application/json",
    }).fail(() => {/* ok */ })
        .done(() => {/* ok */ })
}
function putPrivacy() {
    //Update dataforms
    formdata = $("#form_page_privacy").serializeArray().reduce(function (a, x) {
        a[x.name] = x.value;
        return a;
    }, {});
    let data = {
        PrivacyTitle: formdata.PrivacyTitle,
        PrivacyText : formdata.PrivacyText
    };

    //Do ajax-call
    $.ajax({
        type: "POST",
        url: "/api/Customization/PutPrivacy/",
        data: JSON.stringify(data),
        dataType: "application/json",
        contentType: "application/json",
    }).fail(() => {/* ok */ })
        .done(() => {/* ok */ })
}
function putFAQ() {
    //Update dataforms
    formdata = $("#form_page_faq").serializeArray().reduce(function (a, x) {
        a[x.name] = x.value;
        return a;
    }, {});
    let data = {
        FAQAnswer: formdata.FAQAnswer,
        FAQQuestion: formdata.FAQQuestion
    };
    console.log(data);

    //Do ajax-call
    $.ajax({
        type: "POST",
        url: "/api/Customization/PutFAQ/",
        data: JSON.stringify(data),
        dataType: "application/json",
        contentType: "application/json",
    }).fail(() => {/* ok */ })
        .done(() => {/* ok */ })
}
function putContact() {
    //Update dataforms
    formdata = $("#form_page_contact").serializeArray().reduce(function (a, x) {
        a[x.name] = x.value;
        return a;
    }, {});
    let data = {
        StreetAndHousenumber: formdata.StreetAndHousenumber,
        Zipcode : formdata.Zipcode,
        City : formdata.City,
        Country : formdata.Country,
        Email : formdata.Email
    };

    //Do ajax-call
    $.ajax({
        type: "POST",
        url: "/api/Customization/PutContact/",
        data: JSON.stringify(data),
        dataType: "application/json",
        contentType: "application/json",
    }).fail(() => {/* ok */ })
        .done(() => {/* ok */ })
}

////Questions
////TODO: write an to create a question
//function putQuestion(id) {
//  $.ajax({
//    type: "POST",
//    url: "api/Customization/PutQuestion/" + id,
//    data: $("#form_page_faq").serialize(),
//    contentType: 'application/json; charset=utf-8',
//    dataType: "json"
//  }).fail(() => {/* ok */ })
//    .done(() => {/* ok */ })
//}
//function deleteQuestion(id) {
//  $.ajax({
//    type: "POST",
//    url: "api/Customization/DeleteQuestion/" + id,
//    data: $("#form_page_faq").serialize(),
//    contentType: 'application/json; charset=utf-8',
//    dataType: "json"
//  }).fail(() => {/* ok */ })
//    .done(() => {/* ok */ })
//}

//Eventlistners for ajax-calls
//Customization
$("#form_page_color").submit(function (event) {
    event.preventDefault();
    putColor();
});
$("#form_page_text").submit(function (event) {
    event.preventDefault();
    putAlias();
});
$("#form_page_privacy").submit(function (event) {
    event.preventDefault();
    putPrivacy();
});
$("#form_page_faq").submit(function (event) {
    event.preventDefault();
    putFAQ();
});
$("#form_page_contact").submit(function (event) {
    event.preventDefault();
    putContact();
});

////Questions
////TODO: write a listner to create a question
$("#form_page_faq").submit(function (event) {
    event.preventDefault();
    getSubplatformId(getCustomData);
});
$("#form_page_faq").submit(function (event) {
    event.preventDefault();
    getSubplatformId(getCustomData);
});


var wto;
$(".removeItem").click(function() {
    clearTimeout(wto);
    var $this = $(this);
    var id = $this.data('question-id');
    console.log(id);
    var text = $this.html();
    $this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
    wto = setTimeout(function() {
            $.ajax({
                type: 'POST',
                url: '/api/Subplatform/DeleteQuestion/' + id
            }).fail(() => { alert(Resources.CannotRemoveItem) })
                .done(function() {
                    $this.html(Resources.Deleted);
                    $this.toggleClass("btn-danger btn-dark");
                });
        },
        500);
});