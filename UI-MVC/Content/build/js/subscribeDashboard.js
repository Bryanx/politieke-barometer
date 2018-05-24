
(() => {
    var wto;
    $('.subscribeItem').click(function () {
        clearTimeout(wto);
        var $this = $(this);
        var id = $this.data('item-id');
        if (res.subscriptionAmount === 1) {
            $('.subscriptions').empty();
            res.alertcount = 0;
            updateAlertCount();
        } else {
            $this.parent().parent().empty();
        }
        wto = setTimeout(function () {
                $.ajax({
                    type: 'POST',
                    url: '/api/ToggleSubscribe/' + id
                }).fail(() => { /* ok */ })
                    .done(function () {
                        loadAlerts();
                        res.subscriptionAmount--;
                    });
            },
            500);
    });
})($);