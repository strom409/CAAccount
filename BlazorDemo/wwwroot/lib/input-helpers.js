window.isNumberKey = function (evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    // Only allow digits (0–9)
    if (charCode < 48 || charCode > 57) {
        evt.preventDefault();
        return false;
    }