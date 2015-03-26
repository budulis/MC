function showInfoToast(msg,caption) {
    toastr.success(msg,caption, { "timeOut": "0", "extendedTimeOut": "0", "closeButton": true, "positionClass": "toast-bottom-full-width" });
}

function showErrorToast(msg, caption) {
    toastr.error(msg, caption, { "timeOut": "0", "extendedTimeOut": "0", "closeButton": true, "positionClass": "toast-top-center" });
}