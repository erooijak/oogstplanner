﻿/// <reference path="typings/jquery.d.ts" />

module Scripts {

    export function toMain() {
        var oogstplanner = new Oogstplanner();
        oogstplanner.toMain();
    }

    export class Oogstplanner {
    	
        month : string;

        toMonthCalendar() {
            $.fn.fullpage.moveSlideRight();
            $(window).scrollTop(0);
            this.makeNumericTextBoxesNumeric();
            this.makeCropPluralWhenCropCountIsBiggerThan1();

            // Remove the crop-selection-box since the side is visible on the month calendar...
            $('#crop-selection-box').hide();
        }

        toMain() {
                $.fn.fullpage.moveSlideLeft();
                $(window).scrollTop(0);

                $('#crop-selection-box').show();
        }

        fillMonthCalendar(month : string) {

            // Store the month in the object so we can use it and do not have to get it from the page.
            this.month = month;
            var that = this;
            $.get('/Calendar/Month?month=' + month, function(data) {
                $('#_MonthCalendar').html(data);
            })
            .done(function() { that.toMonthCalendar(); that.bindFarmingActionRemoveFunctionToDeleteButton(); })
            .fail(function() { alert('TODO: Error handling'); });
        }

        addFarmingAction(cropId : number, month : string, actionType : ActionType, cropCount : number) {
            $.post('/Calendar/AddFarmingAction', { cropId: cropId, month: month, actionType: actionType, cropCount: cropCount } );
            this.setHasActionAttributeValue(month, true);
        }

        setHasActionAttributeValue(month : string, value : boolean) {
            $('[data-month=' + month + ']').data( "hasAction", value);
        }

        getHasActionAttributeValue(month : string) {
            return $('[data-month=' + month + ']').data('hasAction');
        }

        removeFarmingAction(id : number) {
            var that = this;
            $.post('/Calendar/RemoveFarmingAction', { id: id }, function (response) {
                if (response.success === true) { 
                    that.fillMonthCalendar(that.month);
                    that.setHasActionAttributeValue(that.month, false);
                    alert("Het gewas is succesvol verwijderd.");
                    that.toMain();
                }
                else { 
                    alert("TODO: Error handling");
                }
            });
        }

        resizeCropSelectionBox() {
            $('#crop-selection-box').css({
                height: $('#month-overview-responsive-square-elements').innerHeight()
            });
        }

        resizeLoginArea() {
            
            var windowHeight = $(window).innerHeight();
            var topHeight = $('#top').innerHeight();
            var padding = windowHeight * 0.025 + 20;	

            $('#login').css({ 
                height: windowHeight - topHeight - padding
            });

        }
       
        resetValidation() {

            // Removes validation from input-fields
            $('.input-validation-error').empty();

            // Removes validation message after input-fields
            $('.field-validation-error').empty();

            // Removes validation summary 
            $('.validation-summary-errors').empty();

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
            $('.form-group').each(function() {
                var input = $(this).find('input:first');
                var span = $(this).find('.crop-count-crop-word:first');
                input.change(function() {
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
                that.removeFarmingAction(farmingActionId);

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
}
