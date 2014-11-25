$(function() {

	/* Using flowtype.js to adjust the size of the names of the seasons and months */
    $('.slide').flowtype({
       minimum   : 200,
       maximum   : 1200,
       minFont   : 12,
       maxFont   : 80,
       fontRatio : 30
    });

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
        verticalCentered: true,
        resize : true,
        responsive: 0,

        // Custom selectors
        sectionSelector: '.container',
        slideSelector: '.slide',
    });

});