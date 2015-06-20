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

    $('#search-button').on('click', function () { 
        window.location.href='/gemeenschap/zoek/' + $('#search-input').val() 
    });

    oogstplanner.makeNumericTextBoxesNumeric();

});
