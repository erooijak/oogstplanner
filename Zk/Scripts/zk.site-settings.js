$(function() {

	/* Using flowtype.js to adjust the size of the fonts */
    $('.flowtype-area').flowtype({
       minimum   : 200,
       maximum   : 1200,
       minFont   : 12,
       maxFont   : 80,
       fontRatio : 30
    });

    /* Keep login screen full width initially and on resize */
    zk.resizeLoginArea();
    $(window).resize(function() { zk.resizeLoginArea(); });

    /* Bind event listeners to hide and show register or signin box when links are clicked. */
    $('#register-link').click(function() { zk.showSignupBox(); });
    $('#signin-link').click(function() { zk.showLoginBox(); });

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

});