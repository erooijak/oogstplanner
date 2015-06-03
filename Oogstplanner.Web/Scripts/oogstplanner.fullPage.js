$(function () {
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
});
