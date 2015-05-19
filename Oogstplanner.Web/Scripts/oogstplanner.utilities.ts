module Util {

    export function toMain() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.toMain();
    }

    export function refreshPage() {
        window.location.reload();
    }

    export function resetValidation() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.resetValidation();
    }

    export function handleError(modelState : any) {
        for (var key in modelState) {
            if (modelState.hasOwnProperty(key)) {
                $("[name='" + modelState[key].key + "']").addClass('input-validation-error');
                $(".validation-summary-errors > ul")
                    .addClass('alert alert-danger')
                    .append('<li>' + modelState[key].errors + '</li>');
            }   
        }
    }

    export function convertToDisplayMonth(monthName : string) : string {

        var monthNames : string[] = this.getMonthNames();
        var displayMonthNames : string[] = this.getDisplayMonthNames();

        var monthDictionary = {};
        for (var i = 0; i < monthNames.length; i += 1) {
            var key = monthNames[i];
            var val = displayMonthNames[i]
            monthDictionary[key] = val;
        }

        var displayMonthName = monthDictionary[monthName];

        return displayMonthName;
    }

    export function getMonthNames() : string[] {
        return $('#month-names').val().split(',');
    }

    export function getDisplayMonthNames() : string[] {
        return $('#display-month-names').val().split(',');
    }

    export function getOppositeMonth(actionType : ActionType, indexCurrentMonth : number, growingTime : number) {

        var monthNames = this.getMonthNames();
        var monthCount = monthNames.length; // 12

        return actionType === ActionType.HARVESTING 
            ? monthNames[monthCount - indexCurrentMonth - (growingTime % monthCount)]
            : monthNames[(indexCurrentMonth + growingTime) % monthCount]
    }
} 
