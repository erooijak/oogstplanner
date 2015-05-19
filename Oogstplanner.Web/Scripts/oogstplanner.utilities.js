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
    function convertToDisplayMonth(monthName) {
        var monthNames = this.getMonthNames();
        var displayMonthNames = this.getDisplayMonthNames();
        var monthDictionary = {};
        for (var i = 0; i < monthNames.length; i += 1) {
            var key = monthNames[i];
            var val = displayMonthNames[i];
            monthDictionary[key] = val;
        }
        var displayMonthName = monthDictionary[monthName];
        return displayMonthName;
    }
    Util.convertToDisplayMonth = convertToDisplayMonth;
    function getMonthNames() {
        return $('#month-names').val().split(',');
    }
    Util.getMonthNames = getMonthNames;
    function getDisplayMonthNames() {
        return $('#display-month-names').val().split(',');
    }
    Util.getDisplayMonthNames = getDisplayMonthNames;
    function getOppositeMonth(actionType, indexCurrentMonth, growingTime) {
        var monthNames = this.getMonthNames();
        var monthCount = monthNames.length;
        return actionType === ActionType.HARVESTING ? monthNames[monthCount - indexCurrentMonth - (growingTime % monthCount)] : monthNames[(indexCurrentMonth + growingTime) % monthCount];
    }
    Util.getOppositeMonth = getOppositeMonth;
})(Util || (Util = {}));
//# sourceMappingURL=oogstplanner.utilities.js.map