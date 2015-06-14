$(function () {
    var oogstplanner = new Oogstplanner();

    oogstplanner.resizeLoginArea();

    $(window).resize(function () {
        oogstplanner.resizeLoginArea();
    });

    oogstplanner.resizeCropSelectionBox();

    $(window).resize(function () {
        oogstplanner.resizeCropSelectionBox();
    });

    $('#register-link').click(function () {
        oogstplanner.showSignupBox();
    });

    $('#signin-link').click(function () {
        oogstplanner.showLoginBox();
    });

    oogstplanner.makeNumericTextBoxesNumeric();

});
