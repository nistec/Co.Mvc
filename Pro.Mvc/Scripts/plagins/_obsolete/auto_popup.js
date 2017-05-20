
jQuery(function ($) {

    $("a.popupAuto").click(function () {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup
        }, 500); // .5 second

        setTimeout(function () { // then close popup, deley in 2 second
            disablePopup(); // function show popup
        }, 2000);

        return false;
    });

    $.fn.showPopup = function() {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup
        }, 500); // .5 second
        
        setTimeout(function () { // then close popup, deley in 2 second
            disablePopup(); // function show popup
        }, 2400);

        return false;
    };

    
    /* event for close the popup */
    $("div.popupClose").hover(
					function () {
					    $('span.ecs_tooltip').show();
					},
					function () {
					    $('span.ecs_tooltip').hide();
					}
				);

    $("div.popupClose").click(function () {
        disablePopup();  // function close pop up
    });

    $(this).keyup(function (event) {
        if (event.which == 27) { // 27 is 'Ecs' in the keyboard
            disablePopup();  // function close pop up
        }
    });

    $("div#popupBackground").click(function () {
        disablePopup();  // function close pop up
    });

    $('a.livebox').click(function () {
        alert('Hello World!');
        return false;
    });

    /************** start: functions. **************/
    function loading() {
        $("div.popupLoader").show();
    }
    function closeloading() {
        $("div.popupLoader").fadeOut('normal');
    }

    var popupStatus = 0; // set value

    function loadPopup() {
        if (popupStatus == 0) { // if value is 0, show popup
            closeloading(); // fadeout loading
            $("#popupAuto").fadeIn(0500); // fadein popup div
            $("#popupBackground").css("opacity", "0.7"); // css opacity, supports IE7, IE8
            $("#popupBackground").fadeIn(0001);
            popupStatus = 1; // and set value to 1
        }
    }

    function disablePopup() {
        if (popupStatus == 1) { // if value is 1, close popup
            $("#popupAuto").fadeOut("normal");
            $("#popupBackground").fadeOut("normal");
            popupStatus = 0;  // and set value to 0
        }
    }

    /************** end: functions. **************/
}); // jQuery End
