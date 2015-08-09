var hub = $.connection.notificationHub;
var connected = false;

hub.client.notify = function (msg) {
    var data = jQuery.parseJSON(msg);

    if (data.type === 0)
        showSuccessToast(data.msg);
    if (data.type === 1)
        showErrorToast(data.msg);
    if (data.type === 2)
        setRecieptData(data.msg);
};


function subscribe_for_status_updates(orderid) {
    $.connection.hub.start()
        .done(function (h) {
            connected = true;
            subscribe(orderid);
        });
}
function subscribe(ordeId) {
    hub.server.subscribe(ordeId);
}



