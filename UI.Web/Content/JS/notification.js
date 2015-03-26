/////////////////////////////////////////////////////////////////////////
// SignalR specific code
/////////////////////////////////////////////////////////////////////////

var hub = $.connection.notificationHub;
var connected = false;

hub.client.notify = function (msg) {
    var data = jQuery.parseJSON(msg);

    if (data.type === 0)
        showInfoToast(data.msg);
    if (data.type === 1)
        showErrorToast(data.msg);
};


function subscribe_for_status_updates(orderid) {
    $.connection.hub.start()
        .done(function (h) {
            connected = true;
            subscribe(orderid);
        });
}
//move subscription to server side to avoid notification message race 
function subscribe(ordeId) {
    hub.server.subscribe(ordeId);
}

