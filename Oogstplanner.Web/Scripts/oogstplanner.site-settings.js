$(function() {

	/* Using flowtype.js to adjust the size of the fonts */
    $('.flowtype-area').flowtype({
       minimum   : 100,
       maximum   : 800,
       minFont   : 8,
       maxFont   : 60,
       fontRatio : 38
    });

    /* Keep login screen full width initially and on resize */
    oogstplanner.resizeLoginArea();
    $(window).resize(function() { oogstplanner.resizeLoginArea(); });

    /* And keep crop-selection-box as big as the responsive square elements initially and on resize */
    oogstplanner.resizeCropSelectionBox();
    $(window).resize(function() { oogstplanner.resizeCropSelectionBox(); });

    /* Bind event listeners to hide and show register or signin box when links are clicked. */
    $('#register-link').click(function() { oogstplanner.showSignupBox(); });
    $('#signin-link').click(function() { oogstplanner.showLoginBox(); });

    /* Initialize fullPage.js sliders */
    $('.container').fullpage({

        // Scrolling
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

        // Accessibility
        keyboardScrolling: false,

        // Design
        verticalCentered: false,
        resize : true,
        responsive: 0,

        // Custom selectors
        sectionSelector: '.container',
        slideSelector: '.slide',
    });

    /* Disable scrolling on touch. */
    $.fn.fullpage.setAllowScrolling(false);

    /* Only allow numeric inputs in numeric fields */
    oogstplanner.makeNumericTextBoxesNumeric();

});