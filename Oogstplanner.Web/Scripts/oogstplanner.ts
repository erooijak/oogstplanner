/// <reference path="typings/jquery.d.ts" />

class Oogstplanner {
	
    month : string;
    hasActionSymbolClass = 'bg-watering-can';

    toMonthCalendar() {
        
        // We hide and show the elements, otherwise the corners of elements 
        // can be visible in the other fullpage slide. 
        // Note: needs to be called before fullpage page switch.

        $('#crop-selection-box').hide();
        $('#_MonthCalendar').show();

        $.fn.fullpage.moveSlideRight();
        $(window).scrollTop(0);
    }

    toMain() {

        // We hide and show the elements, otherwise the corners of elements 
        // can be visible in the other fullpage slide.
        // Note: needs to be called before fullpage page switch.
        $('#crop-selection-box').show(); 
        $('#_MonthCalendar').hide();

        $.fn.fullpage.moveSlideLeft();
        $(window).scrollTop(0);

        // Recalculate which months have actions because things might have changed.
        this.setHasActionAttributes();
    }

    fillMonthCalendar(month : string) {

        // Store the month in the object so we can use it and do not have to get it from the page.
        this.month = month;
        var that = this;
        $.get('/zaaikalender/' + month, function (data) {
            $('#_MonthCalendar').html(data);
        })
        .done(() => { 
            that.bindFarmingActionRemoveFunctionToDeleteButton(); 
            that.makeNumericTextBoxesNumeric();
            that.makeCropPluralWhenCropCountIsBiggerThan1();
        })
        .fail(() =>  { Notification.error(); });
    }

    addFarmingAction(cropId : number, month : string, actionType : ActionType, cropCount : number) {
        $.post('/zaaikalender/toevoegen', { cropId: cropId, month: month, actionType: actionType, cropCount: cropCount } );
    }

    setHasActionAttributeValue(monthName : string, value : boolean) {
        $('[data-month=' + monthName + ']').data("hasAction", value);
    }

    setHasActionSymbol(monthName) {
        $('[data-month=' + monthName + ']').addClass(this.hasActionSymbolClass);
    }

    getHasActionAttributeValue(monthName : string) {
        return $('[data-month=' + monthName + ']').data('hasAction');
    }

    resetHasActionAttributes() {
        var that = this
        $('[data-month]').each(function (i, monthSquare) {
            var monthName = $(monthSquare).data('month');
            that.setHasActionAttributeValue(monthName, false);
            $(monthSquare).removeClass(that.hasActionSymbolClass);
        });
    }

    setHasActionAttributes() {
        this.resetHasActionAttributes();
        var that = this
        $.get('/zaaikalender/actievemaanden', function (monthNames) {
            for (var i = 0; i < monthNames.length; i++) {
                var monthName = monthNames[i];
                that.setHasActionAttributeValue(monthName, true);
                that.setHasActionSymbol(monthName);
            }
        });
    }

    removeFarmingAction(id : number) {
        var that = this;
        $.post('/zaaikalender/verwijder', { id: id }, function (response) {
            if (response.success === true) { 
                that.fillMonthCalendar(that.month);
            
                var amountOfElementsLeftWhenLastIsRemoved = 1
                var monthHasNoActionsLeft = 
                    $('#harvesting').find('.form-inline').length 
                    + $('#sowing').find('.form-inline').length === amountOfElementsLeftWhenLastIsRemoved;

                if (monthHasNoActionsLeft)
                {
                    that.toMain();
                }

            }
            else { 
                Notification.error();
            }
        });
    }

    bindRemoveAccount() : void {
        var that = this;

        $('#remove-account').bind('click', function (e) {
            e.preventDefault();
            Notification.confirmation('Weet u zeker dat u uw account, alle data in uw zaaikalender, en al uw verkregen en gegeven likes volledig wilt verwijderen?', 
                () => { that.removeAccount(); });            
        });
    }

    removeAccount() : void {
        $.post('/gebruiker/verwijderen')
        .done(() => { Notification.informational('Account verwijderd', 'Uw account is succesvol verwijderd.'); Util.refreshPage();  })
        .fail(() => Notification.error());
    }

    // TODO: refactor code below
    resizeCropSelectionBox() {

        // media query event handler
        if (matchMedia) {
            var mq = window.matchMedia("(max-width: 991px)");
            mq.addListener(WidthChange);
            WidthChange(mq);
        }

        // media query change
        function WidthChange(mq) {
            
            if (mq.matches && Util.isMobile()) {
                // window width is less than 991px, and user is on mobile, then check if landscape
                if (window.matchMedia("(orientation: landscape)").matches) {
                    $('#crop-selection-box').css({
                        height: $('#month-overview-responsive-square-elements').innerHeight() / 2
                    });
                }
                else
                {
                    $('#crop-selection-box').css({
                        height: $('#month-overview-responsive-square-elements').innerHeight()
                    })
                }
            }
            else if (mq.matches) {
                $('#crop-selection-box').css({
                    height: $('#month-overview-responsive-square-elements').innerHeight() / 2
                });
            }
            else {
                // window width is more than 991px
                $('#crop-selection-box').css({
                    height: $('#month-overview-responsive-square-elements').innerHeight()
                });
            }

        }
    }

    resizeLoginArea() {
        
        var rowHeight = $('#main-row').innerHeight();

        $('#login').css({ 
            height: rowHeight
        });

    }

    changeLengthFaqLinkText() {
        
        /* Change link in upper screen based on window width so menu stays in one row. */
        if ($(window).width() > 750 && $(window).width() < 850 ) {
            $("a[href='/veelgesteldevragen']").first().text("FAQ")
        } else { 
            $("a[href='/veelgesteldevragen']").first().text("Veelgestelde vragen")
        }
    }
        
    resetValidation() {

        // Removes validation from input-fields
        $('.input-validation-error').removeClass('input-validation-error');

        // Removes validation message after input-fields
        $('.field-validation-error').removeClass('field-validation-error');

        // Removes validation summary 
        $('.validation-summary-errors > ul').empty();
        $('.validation-summary-errors > ul').removeClass('alert alert-danger');

    }

    makeNumericTextBoxesNumeric() {
        $(".numeric-text-box").keydown(function (e) {
            
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||

                // Allow: Ctrl+A.
                (e.keyCode === 65 && e.ctrlKey === true) || 

                // Allow: home, end, left, right, down, up.
                (e.keyCode >= 35 && e.keyCode <= 40)) {

                    // If field would become empty by backspace or delete disable the move.
                    if ( (e.keyCode === 46 || e.keyCode === 8) && $(this).val().length === 1) {
                        
                        // TODO: If field would become larger than 999 disable the move.

                        e.preventDefault();
                    }

                    // Otherwise let it happen, don't do anything.
                    return;
            }

            // Ensure that it is a number and stop the keypress.
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }

        });
    }

    makeCropPluralWhenCropCountIsBiggerThan1() {

        // Every crop count input field needs a span label with crop or crops depending on the count.

        // Below works for double form:
        $('.form-inline').each(function() {
            var input = $(this).find('input:first');
            var span = $(this).find('.crop-count-crop-word:first'); 
            input.on('change', function() {
                span.text( $(this).val() == 1 ? 'plant' : 'planten' );
            });      

            // Ensure text is correct on load by triggering change event.
            input.trigger('change');
          
        });

    }

    bindFarmingActionRemoveFunctionToDeleteButton() {

        var that = this;

        $('.remove-farming-action').bind('click', function (e) {
            e.preventDefault();

            // farming action id is stored in the id of the remove-farming-action element.
            var farmingActionId = this.id;

            Notification.confirmation('Weet u zeker dat u dit gewas volledig wilt verwijderen uit uw kalender?', () => that.removeFarmingAction(farmingActionId));

        });

    }

    showSignupBox() {
        $('#loginbox').hide(); 
        $('#signupbox').show(); 
        this.resetValidation();
    }

    showLoginBox() {
        $('#signupbox').hide(); 
        $('#loginbox').show(); 
        this.resetValidation();
    }        
}  
