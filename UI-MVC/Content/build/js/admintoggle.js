
var wto;
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
                        $(".item-name").html($this.val());
                    });
            },
            1000);
    });

$("#itemForm").on("submit",
    function(event) {
        event.preventDefault();
        let form = $(this);
        $.ajax({
            method: "GET",
            url: "/api/GetItemWithDetails/" + form.data("item-id"),
            success: data => {
                formdata = form.serializeArray().reduce(function(a, x) {
                        a[x.name] = x.value;
                        return a;
                    },
                    {});
                data.District = formdata.District;
                data.Site = formdata.Site;
                data.DateOfBirth = formdata.DateOfBirth;
                data.Position = formdata.Position;
                submitForm($(this), event, $("#accountSettingsMessage"), data);
            }
        });
    });

$(".item-input").on('input propertychange paste', () => $("#itemForm").submit());
$(document).on("click", ".subscribeItem", (e) => ajaxToggleSubscribe(e));
$("#fileUpload").on("change", () => $("#formFileUpload").submit());
$("#uploadPicture").click(() => $("#fileUpload").click());

(() => {
    $("#viewAsUser").click(function() {
        $("#userview").toggle();
        $("#adminview").toggle();
    });
})(jQuery);