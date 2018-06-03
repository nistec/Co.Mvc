
/*

JQUERY: STOPWATCH & COUNTDOWN

This is a basic stopwatch & countdown plugin to run with jquery. Start timer, pause it, stop it or reset it. Same behaviour with the countdown besides you need to input the countdown value in seconds first. At the end of the countdown a callback function is invoked.

Any questions, suggestions? marc.fuehnen(at)gmail.com

*/
/*
elements:
tag-sw_timer
tag-sw_start
tag-sw_status
tag-sw_stop
tag-sw_reset
tag-sw_pause

*/

    (function ($) {
        $.extend({
            JStopwatch: {
                lastSw: 0,
                formatTimer: function (a) {
                    if (a < 10) {
                        a = '0' + a;
                    }
                    return a;
                },
                startTimer: function (tag,dir) {
                    var a;
                    // save type
                    $.JStopwatch.dir = dir;
                    $.JStopwatch.tag = tag+'-'+dir;
                    // get current date
                    $.JStopwatch.d1 = new Date();
                    switch ($.JStopwatch.state) {
                        case 'pause':
                            // resume timer
                            // get current timestamp (for calculations) and
                            // substract time difference between pause and now
                            $.JStopwatch.t1 = $.JStopwatch.d1.getTime() - $.JStopwatch.td;
                            break;
                        default:
                            // get current timestamp (for calculations)
                            $.JStopwatch.t1 = $.JStopwatch.d1.getTime();
                            // if countdown add ms based on seconds in textfield
                            if ($.JStopwatch.dir === 'cd') {
                                $.JStopwatch.t1 += parseInt($('#cd_seconds').val()) * 1000;
                            }
                            break;
                    }
                    // reset state
                    $.JStopwatch.state = 'alive';
                    $('#' + $.JStopwatch.tag + '_status').html('Running');

                    // start loop
                    $.JStopwatch.loopTimer();
                },
                pauseTimer: function () {
                    // save timestamp of pause
                    $.JStopwatch.dp = new Date();
                    $.JStopwatch.tp = $.JStopwatch.dp.getTime();

                    // save elapsed time (until pause)
                    $.JStopwatch.td = $.JStopwatch.tp - $.JStopwatch.t1;

                    // change button value
                    $('#' + $.JStopwatch.tag + '_start').val('Resume');

                    // set state
                    $.JStopwatch.state = 'pause';
                    $('#' + $.JStopwatch.tag + '_status').html('Paused');

                },

                stopTimer: function () {

                    // change button value
                    $('#' + $.JStopwatch.tag + '_start').val('Restart');
                                        // set state
                    $.JStopwatch.state = 'stop';
                    $('#' + $.JStopwatch.tag + '_status').html('Stopped');
                },

                resetTimer: function () {

                    // reset display
                    //$('#' + $.JStopwatch.dir + '_ms,#' + $.JStopwatch.dir + '_s,#' + $.JStopwatch.dir + '_m,#' + $.JStopwatch.dir + '_h').html('00');

                    $('#' + $.JStopwatch.tag + '_timer').html('00');

                    // change button value
                    $('#' + $.JStopwatch.tag + '_start').val('Start');

                    // set state
                    $.JStopwatch.state = 'reset';
                    $('#' + $.JStopwatch.tag + '_status').html('Reset & Idle again');

                },

                endTimer: function (callback) {

                    // change button value
                    $('#' + $.JStopwatch.tag + '_start').val('Restart');

                    // set state
                    $.JStopwatch.state = 'end';

                    // invoke callback
                    if (typeof callback === 'function') {
                        callback();
                    }

                },

                loopTimer: function () {

                    var td;
                    var d2, t2;

                    //var ms = 0;
                    var s = 0;
                    var m = 0;
                    var h = 0;
                    var sw = 0;

                    if ($.JStopwatch.state === 'alive') {

                        // get current date and convert it into 
                        // timestamp for calculations
                        d2 = new Date();
                        t2 = d2.getTime();

                        // calculate time difference between
                        // initial and current timestamp
                        if ($.JStopwatch.dir === 'sw') {
                            td = t2 - $.JStopwatch.t1;
                            // reversed if countdown
                        } else {
                            td = $.JStopwatch.t1 - t2;
                            if (td <= 0) {
                                // if time difference is 0 end countdown
                                $.JStopwatch.endTimer(function () {
                                    $.JStopwatch.resetTimer();
                                    $('#' + $.JStopwatch.tag + '_status').html('Ended & Reset');
                                });
                            }
                        }

                        // calculate milliseconds
                        ms = td % 1000;
                        if (ms < 1) {
                            ms = 0;
                        } else {
                            // calculate seconds
                            s = (td - ms) / 1000;
                            if (s < 1) {
                                s = 0;
                            } else {
                                // calculate minutes   
                                m = (s - (s % 60)) / 60;
                                if (m < 1) {
                                    m = 0;
                                } else {
                                    // calculate hours
                                    h = (m - (m % 60)) / 60;
                                    if (h < 1) {
                                        h = 0;
                                    }
                                }
                            }
                        }

                        // substract elapsed minutes & hours
                        ms = Math.round(ms / 100);
                        s = s - (m * 60);
                        m = m - (h * 60);

                        sw=(h*60*60)+(m*60)+s;
                        if ($.JStopwatch.lastSw !== sw) {

                            // update display
                            //$('#' + $.JStopwatch.dir + '_ms').html($.JStopwatch.formatTimer(ms));
                            //$('#' + $.JStopwatch.dir + '_s').html($.JStopwatch.formatTimer(s));
                            //$('#' + $.JStopwatch.dir + '_m').html($.JStopwatch.formatTimer(m));
                            //$('#' + $.JStopwatch.dir + '_h').html($.JStopwatch.formatTimer(h));

                            //sw_timer
                            $('#' + $.JStopwatch.tag + '_timer').html($.JStopwatch.formatTimer(h) + ':' + $.JStopwatch.formatTimer(m) + ':' + $.JStopwatch.formatTimer(s));
                            $.JStopwatch.lastSw = sw;
                        }

                        // loop
                        $.JStopwatch.t = setTimeout($.JStopwatch.loopTimer, 1);

                    } else {

                        // kill loop
                        clearTimeout($.JStopwatch.t);
                        return true;

                    }
                }
            }
        });
        /*
        $('#sw_start').live('click', function () {
            $.JStopwatch.startTimer('stopwatch','sw');//stopwatch
        });

        $('#cd_start').live('click', function () {
            $.JStopwatch.startTimer('cd');//countdown
        });

        $('#sw_stop,#cd_stop').live('click', function () {
            $.JStopwatch.stopTimer();
        });

        $('#sw_reset,#cd_reset').live('click', function () {
            $.JStopwatch.resetTimer();
        });

        $('#sw_pause,#cd_pause').live('click', function () {
            $.JStopwatch.pauseTimer();
        });
        */
    })(jQuery);


