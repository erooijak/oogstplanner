$(function () {
    var oogstplanner = new Oogstplanner();
    $('.flowtype-area').flowtype({
        minimum: 100,
        maximum: 800,
        minFont: 8,
        maxFont: 60,
        fontRatio: 38
    });
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
    $('.container').fullpage({
        css3: true,
        scrollingSpeed: 350,
        autoScrolling: false,
        scrollBar: false,
        easing: 'easeInQuart',
        easingcss3: 'ease',
        loopHorizontal: false,
        scrollOverflow: false,
        touchSensitivity: 15,
        normalScrollElementTouchThreshold: 5,
        keyboardScrolling: false,
        verticalCentered: false,
        resize: true,
        responsive: 0,
        sectionSelector: '.container',
        slideSelector: '.slide',
    });
    $.fn.fullpage.setAllowScrolling(false);
    oogstplanner.makeNumericTextBoxesNumeric();
});
//# sourceMappingURL=oogstplanner.site-settings.js.map