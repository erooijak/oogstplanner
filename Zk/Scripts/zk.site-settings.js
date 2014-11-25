$(function() {

    /* Using fullpage.js to enable pretty page switching. */
    $('#fullpage').fullpage({
        slidesNavigation: false,
        keyboardScrolling: false
    });

    /* Using flowtype.js on the page sections to adjust the font size of the names of the seasons and months */
    $('.flow-type').flowtype({
       minimum   : 200,
       maximum   : 1200,
       minFont   : 12,
       maxFont   : 80,
       fontRatio : 30
    });

});