var Scripts;
(function (Scripts) {
    function toMain() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.toMain();
    }
    Scripts.toMain = toMain;
    function refreshPage() {
        window.location.reload();
    }
    Scripts.refreshPage = refreshPage;
    function resetValidation() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.resetValidation();
    }
    Scripts.resetValidation = resetValidation;
    function handleError(modelState) {
        for (var key in modelState) {
            if (modelState.hasOwnProperty(key)) {
                $("[name='" + modelState[key].key + "']").addClass('input-validation-error');
                $(".validation-summary-errors > ul").addClass('alert alert-danger').append('<li>' + modelState[key].errors + '</li>');
            }
        }
    }
    Scripts.handleError = handleError;
    var Oogstplanner = (function () {
        function Oogstplanner() {
        }
        Oogstplanner.prototype.toMonthCalendar = function () {
            $.fn.fullpage.moveSlideRight();
            $(window).scrollTop(0);
            this.makeNumericTextBoxesNumeric();
            this.makeCropPluralWhenCropCountIsBiggerThan1();
            $('#crop-selection-box').hide();
        };
        Oogstplanner.prototype.toMain = function () {
            $.fn.fullpage.moveSlideLeft();
            $(window).scrollTop(0);
            $('#crop-selection-box').show();
        };
        Oogstplanner.prototype.fillMonthCalendar = function (month) {
            this.month = month;
            var that = this;
            $.get('/Calendar/Month?month=' + month, function (data) {
                $('#_MonthCalendar').html(data);
            }).done(function () {
                that.toMonthCalendar();
                that.bindFarmingActionRemoveFunctionToDeleteButton();
            }).fail(function () {
                alert('TODO: Error handling');
            });
        };
        Oogstplanner.prototype.addFarmingAction = function (cropId, month, actionType, cropCount) {
            $.post('/Calendar/AddFarmingAction', { cropId: cropId, month: month, actionType: actionType, cropCount: cropCount });
            this.setHasActionAttributeValue(month, true);
        };
        Oogstplanner.prototype.setHasActionAttributeValue = function (month, value) {
            $('[data-month=' + month + ']').data("hasAction", value);
        };
        Oogstplanner.prototype.getHasActionAttributeValue = function (month) {
            return $('[data-month=' + month + ']').data('hasAction');
        };
        Oogstplanner.prototype.removeFarmingAction = function (id) {
            var that = this;
            $.post('/Calendar/RemoveFarmingAction', { id: id }, function (response) {
                if (response.success === true) {
                    that.fillMonthCalendar(that.month);
                    alert("Het gewas is succesvol verwijderd.");
                    var monthHasNoActions = $('.farmingMonth').children().length === 0;
                    if (monthHasNoActions) {
                        that.setHasActionAttributeValue(that.month, false);
                        that.toMain();
                    }
                    ;
                }
                else {
                    alert("TODO: Error handling");
                }
            });
        };
        Oogstplanner.prototype.resizeCropSelectionBox = function () {
            $('#crop-selection-box').css({
                height: $('#month-overview-responsive-square-elements').innerHeight()
            });
        };
        Oogstplanner.prototype.resizeLoginArea = function () {
            var windowHeight = $(window).innerHeight();
            var topHeight = $('#top').innerHeight();
            var padding = windowHeight * 0.025 + 20;
            $('#login').css({
                height: windowHeight - topHeight - padding
            });
        };
        Oogstplanner.prototype.resetValidation = function () {
            $('.input-validation-error').removeClass('input-validation-error');
            $('.field-validation-error').removeClass('field-validation-error');
            $('.validation-summary-errors > ul').empty();
            $('.validation-summary-errors > ul').removeClass('alert alert-danger');
        };
        Oogstplanner.prototype.makeNumericTextBoxesNumeric = function () {
            $(".numeric-text-box").keydown(function (e) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 || (e.keyCode === 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 40)) {
                    if ((e.keyCode === 46 || e.keyCode === 8) && $(this).val().length === 1) {
                        e.preventDefault();
                    }
                    return;
                }
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        };
        Oogstplanner.prototype.makeCropPluralWhenCropCountIsBiggerThan1 = function () {
            $('.form-group').each(function () {
                var input = $(this).find('input:first');
                var span = $(this).find('.crop-count-crop-word:first');
                input.change(function () {
                    span.text($(this).val() == 1 ? 'plant' : 'planten');
                });
                input.trigger('change');
            });
        };
        Oogstplanner.prototype.bindFarmingActionRemoveFunctionToDeleteButton = function () {
            var that = this;
            $('.remove-farming-action').bind('click', function (e) {
                e.preventDefault();
                var farmingActionId = this.id;
                that.removeFarmingAction(farmingActionId);
            });
        };
        Oogstplanner.prototype.showSignupBox = function () {
            $('#loginbox').hide();
            $('#signupbox').show();
            this.resetValidation();
        };
        Oogstplanner.prototype.showLoginBox = function () {
            $('#signupbox').hide();
            $('#loginbox').show();
            this.resetValidation();
        };
        return Oogstplanner;
    })();
    Scripts.Oogstplanner = Oogstplanner;
})(Scripts || (Scripts = {}));
//# sourceMappingURL=oogstplanner.js.map