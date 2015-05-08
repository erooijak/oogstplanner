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
} 