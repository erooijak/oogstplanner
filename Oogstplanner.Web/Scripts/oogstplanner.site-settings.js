$(function () {
    var oogstplanner = new Oogstplanner();

    oogstplanner.resizeLoginArea();
    oogstplanner.changeLengthFaqLinkText()
    oogstplanner.resizeCropSelectionBox();

    $(window).resize(function () {
        oogstplanner.resizeLoginArea();
        oogstplanner.resizeCropSelectionBox();
        oogstplanner.changeLengthFaqLinkText();
    });

    oogstplanner.makeNumericTextBoxesNumeric();

});
