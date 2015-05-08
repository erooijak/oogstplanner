var Util;
(function (Util) {
    function toMain() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.toMain();
    }
    Util.toMain = toMain;
    function refreshPage() {
        window.location.reload();
    }
    Util.refreshPage = refreshPage;
    function resetValidation() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.resetValidation();
    }
    Util.resetValidation = resetValidation;
    function handleError(modelState) {
        for (var key in modelState) {
            if (modelState.hasOwnProperty(key)) {
                $("[name='" + modelState[key].key + "']").addClass('input-validation-error');
                $(".validation-summary-errors > ul").addClass('alert alert-danger').append('<li>' + modelState[key].errors + '</li>');
            }
        }
    }
    Util.handleError = handleError;
})(Util || (Util = {}));
//# sourceMappingURL=oogstplanner.utilities.js.map