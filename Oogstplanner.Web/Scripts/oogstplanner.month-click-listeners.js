$(function () {
    $('[data-month]').each(function (i, monthElement) {
        var oogstplanner = new Scripts.Oogstplanner();
        $(monthElement).bind('click', function () {
            var month = $(monthElement).data('month');
            if (oogstplanner.getHasActionAttributeValue(month) === true) {
                oogstplanner.fillMonthCalendar(month);
            }
            else {
                alert("Deze maand heeft geen zaai- of oogstmomenten.");
            }
        });
    });
});
//# sourceMappingURL=oogstplanner.month-click-listeners.js.map