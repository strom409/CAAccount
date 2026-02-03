function bootstrapModalShow(selector) {
    var modalElement = document.querySelector(selector);
    if (modalElement) {
        var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
        modal.show();
    }
}
function bootstrapModalHide(selector) {
    var modalElement = document.querySelector(selector);
    if (modalElement) {
        var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
        modal.hide();
    }
}
