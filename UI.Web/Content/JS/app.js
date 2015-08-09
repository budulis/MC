var data = [];
var name;
var comments;
var cardNumber;
var loyaltyCardNumber;
$(function () {
    $("#submitOrder").click(function () {
        name = $("#name").val();
        comments = $("#comments").val();
        cardNumber = $("#cardNumber").val();
        loyaltyCardNumber = $("#loyaltyCardNumber").val();
        data = [];
        $(":checked").each(function () {
            data.push($(this).val());
        });
        $.ajax({
            url: "/Order",
            type: "POST",
            data: "{\"customerName\":\"" + name + "\",\"comments\":\"" + comments + "\",\"cardNumber\":\"" + cardNumber + "\",\"loyaltyCardNumber\":\"" + loyaltyCardNumber + "\",\"productIDs\":" + JSON.stringify(data) + "}",
            contentType: 'application/json; charset=utf-8',
        }).done(function (data, statusText, xhr) {
            if (xhr.status === 202) {
                var orderid = xhr.getResponseHeader("location");
                showSuccessToast("Order is placed");
                subscribe_for_status_updates(orderid);
            }
        }).fail(function (xhr, statusText, errorThrown) {
            if (xhr.status === 400) {
                showWarningToast(xhr.statusText,"Validation error");
            } else {
                var orderid = xhr.getResponseHeader("location");
                showErrorToast("Id:" + orderid, "Error occured");
            }
        });;
    });
});

function setRecieptData(msg) {
    $(".AmountCharged").text(msg.AmountCharged);
    $(".Comments").text(msg.Comments);
    $(".CustomerName").text(msg.CustomerName);
    $(".Date").text(msg.Date);
    $(".CardNumber").text(msg.CardNumber);
    $(".LoyaltyCardNumber").text(msg.LoyaltyCardNumber);
    $(".Products").text(msg.Products);
}