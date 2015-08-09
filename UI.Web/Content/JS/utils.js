function showSuccessToast(msg, caption) {
    toastr.success(msg,caption, { "timeOut": "0", "extendedTimeOut": "0", "closeButton": true});
}

function showModalToast(msg, caption) {
    toastr.info(msg, caption, { "timeOut": "0", "extendedTimeOut": "0", "closeButton": true});
}

function showErrorToast(msg, caption) {
    toastr.error(msg, caption, { "timeOut": "0", "extendedTimeOut": "0", "closeButton": true});
}

function showWarningToast(msg, caption) {
    toastr.warning(msg, caption, { "timeOut": "0", "extendedTimeOut": "0", "closeButton": true });
}