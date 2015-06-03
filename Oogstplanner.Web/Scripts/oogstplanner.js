var Oogstplanner = (function () {
    function Oogstplanner() {
    }
    Oogstplanner.prototype.toMonthCalendar = function () {
        $.fn.fullpage.moveSlideRight();
        $(window).scrollTop(0);
        this.makeNumericTextBoxesNumeric();
        this.makeCropPluralWhenCropCountIsBiggerThan1();
    };
    Oogstplanner.prototype.toMain = function () {
        $.fn.fullpage.moveSlideLeft();
        $(window).scrollTop(0);
        this.setHasActionAttributes();
    };
    Oogstplanner.prototype.fillMonthCalendar = function (month) {
        this.month = month;
        var that = this;
        $.get('/zaaikalender/' + month, function (data) {
            $('#_MonthCalendar').html(data);
        }).done(function () {
            that.toMonthCalendar();
            that.bindFarmingActionRemoveFunctionToDeleteButton();
        }).fail(function () {
            Notification.error();
        });
    };
    Oogstplanner.prototype.addFarmingAction = function (cropId, month, actionType, cropCount) {
        $.post('/zaaikalender/toevoegen', { cropId: cropId, month: month, actionType: actionType, cropCount: cropCount });
    };
    Oogstplanner.prototype.setHasActionAttributeValue = function (monthName, value) {
        $('[data-month=' + monthName + ']').data("hasAction", value);
    };
    Oogstplanner.prototype.getHasActionAttributeValue = function (monthName) {
        return $('[data-month=' + monthName + ']').data('hasAction');
    };
    Oogstplanner.prototype.resetHasActionAttributes = function () {
        var that = this;
        $('[data-month]').each(function (i, monthSquare) {
            var monthName = $(monthSquare).data('month');
            that.setHasActionAttributeValue(monthName, false);
        });
    };
    Oogstplanner.prototype.setHasActionAttributes = function () {
        this.resetHasActionAttributes();
        var that = this;
        $.get('/zaaikalender/actievemaanden', function (monthNames) {
            for (var i = 0; i < monthNames.length; i++) {
                var monthName = monthNames[i];
                that.setHasActionAttributeValue(monthName, true);
            }
        });
    };
    Oogstplanner.prototype.removeFarmingAction = function (id) {
        var that = this;
        $.post('/zaaikalender/verwijder', { id: id }, function (response) {
            if (response.success === true) {
                that.fillMonthCalendar(that.month);
                var monthHasNoActions = $('.farmingMonth').children().length === 2;
                if (monthHasNoActions) {
                    that.toMain();
                }
            }
            else {
                Notification.error();
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
            Notification.confirmation('Weet u zeker dat u dit gewas volledig wilt verwijderen uit uw kalender?', function () { return that.removeFarmingAction(farmingActionId); });
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
//# sourceMappingURL=oogstplanner.js.map