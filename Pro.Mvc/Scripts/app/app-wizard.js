/*
<div id="wizard">
    <ul>
        <li><a href="#tab-1">1</a></li>
        <li><a href="#tab-2">2</a></li>
        <li><a href="#tab-3">3</a></li>
    </ul>
    <div id="tab-1" class="ui-tabs-panel">
        <h3>נמענים</h3>
        <div>
            <div class="form-group">
                סיווגים:
                <div id="listCategory"></div>
                <input type="hidden" id="Category" name="Category" />
                <input type="checkbox" id="allCategory" name="allCategory" /><span>  </span><span>שלח לכל המנויים</span>
            </div>
            <div class="form-group">
                סה"כ נמענים:<input type="number" id="TotalCount" readonly class="text-short" style="border:none" />
                <a id="btnShow" href="#" class="btn-tab">הצג נמענים</a>
                <br />
            </div>
        </div>
    </div>
    <div id="wizard-steps">
    <a href="#" class="btn-tab prev-tab ">הקודם</a>
    <a href="#" class="btn-tab  next-tab">הבא</a>
    <a id="submit" href="#" class="btn-tab  end-tab">סיום</a>
    <input type="text" id="wizard-validation" readonly />
</div>
*/


//wizard ====================================

function app_wizard(showSteps, showBorder, height) {
    var slf = this;
    this.ShowSteps = showSteps;
    this.$tabs = $('#wizard').tabs();

    if (height)
        $('#wizard').css('height', height);
    if (!showBorder) {
        $('#wizard').css('border', 'none');
        //$(".ui-tabs-panel").css('padding', '0');
    }
    if (showSteps) {
        //$('.next-tab').hide();
        //$('#wizard-validation').hide();

        $('.prev-tab').hide();
        $('.end-tab').hide();
    }

    $('.next-tab').click(function () {
        var totalSize = $(".ui-tabs-panel").length;
        var active = $('#wizard').tabs("option", "active");

        if (!wizard_validate_step(active)) {
            return;
        }

        var next = active + 1;

        if (slf.ShowSteps) {
            if (next >= totalSize - 1) {
                $('.next-tab').hide();
                $('.end-tab').show();
            }
            $('.prev-tab').show();
        }

        
        slf.displayStep(next, totalSize);
        return false;
    });
    $('.prev-tab').click(function () {

        var active = $('#wizard').tabs("option", "active");
        var prev = active - 1;

        if (slf.ShowSteps) {
            if (prev <= 0) {
                $('.prev-tab').hide();
            }
            $('.end-tab').hide();
            $('.next-tab').show();
            $('#wizard-validation').val("");
        }

        //$('#wizard').tabs({ active: prev });
        slf.displayStep(prev);
        //$tabs.tabs('select', $(this).attr("rel"));
        return false;
    });
    this.displayStep = function (step, totalSize) {

        $('#wizard').tabs({ active: step });
        
        if (totalSize === undefined)
            totalSize = $(".ui-tabs-panel").length;
        //totalSize++;
        step++;
        $('#wizard-validation').val("שלב " + step + " מתוך " + totalSize);
    }
    this.displayWizardValidation = function (message) {
        $('#wizard-validation').val(message);
        return false;
    }
    //var wizard_validate_step = function (step) {

    //    return false;
    //}
    //displayStep(0);

    //end wizard =====================================

};

function app_wiztabs() {

    var slf = this;

    this.getCurrent = function () {
        return $("#wiz-container").find('.active');
    }
    this.getIframe = function () {
        var iframe = this.getCurrent().find('.wiz-partial').find("iframe");
        if (iframe && iframe.length>0) {
            return iframe[0].contentWindow;
        }
        return null;
        //$(wizard.getCurrent().find('.wiz-partial')[0].childNodes['0'])[0].contentWindow.def.doSubmit

        //return cur.find("iframe")[0].contentWindow;
    }
    //this.doIframeCallback = function (callback) {
    //    var iframe = this.getIframe();
    //    if (iframe && iframe.def && callback) {
    //        iframe.def.callback();
    //    }
    //}
    this.wizHome = function () {
        this.displayStep(1);
    }
    this.displayStep = function (step) {
        var current = $("#wiz-container").find('.active');
        current.removeClass('active')
        $("#wiz-" + step).addClass('active');
    }
    this.changeIframe = function (src) {
        var iframe = this.getCurrent().find('.wiz-partial').find("iframe");
        app_iframe.changeIframe(iframe,src);
    }
    this.loadIframe = function (step, src, width, height, scrolling) {
        //var part = $("#divPartial" + step);
        //if (part.length > 0)
        //    part.children().remove();
        app_iframe.removeIframe("divPartial" + step);
        this.displayStep(step);
        app_iframe.loadIframe("divPartial" + step, src, width, height, scrolling);
    }
    this.appendIframe = function (step, src, width, height, scrolling,loaderTag) {
        //var part = $("#divPartial" + step);
        //if (part.length > 0)
        //    part.children().remove();
        app_iframe.removeIframe("divPartial" + step);
        this.displayStep(step);
        app_iframe.appendIframe("divPartial" + step, src, width, height, scrolling,loaderTag);
    }
    this.attachIframe = function (step, src, width, height, scrolling) {
        this.displayStep(step);
        app_iframe.attachIframe("divPartial" + step, src, width, height, scrolling);
    }
    this.removeIframe = function (step) {
        var part = (step) ? $("#divPartial" + step) : this.getCurrent().find('.wiz-partial');
        if (part.length > 0)
            part.children().remove();
    }
    this.closeIframe = function () {

        this.displayStep(1);
    }

    this.displayWizardValidation = function (message) {
        $('#wizard-validation').val(message);
        return false;
    }
    //var wizard_validate_step = function (step) {

    //    return false;
    //}
    //displayStep(0);

    //end wizard =====================================
};


/*
(function ($) {

    $.fn.wiztabs = function (options) {

        //var getOptions = {
        //    corectionValue: 0
        //}
        //$.extend(getOptions, options); 

        var
            slf = $(this)
        , _window = $(window)
        , _document = $(document)
        ;

        init();
        function init() {
        }
    }
})(jQuery)
*/


(function ($) {

    //$.fn.wiztabs=function(options){ 

    //var getOptions = {
    //    corectionValue: 0
    //}
    //$.extend(getOptions, options); 

    //var
    //	slf = $(this)
    //,	_window = $(window)
    //,	_document = $(document)
    //;

    //init();
    //function init() {
    //}

    var wiztabs = function (element, options) {
        this.$element = $(element);
        if (!this.$element.is('div')) {
            $.error('app_wiztabs should be applied to DIV element');
            return;
        }
        this.options = $.extend({}, $.fn.wiztabs.defaults, options, this.$element.data());
        this.init();
    };
    wiztabs.prototype = {
        constructor: wiztabs,
        init: function () {
            this.map = {
                //key   regexp    moment.method
                day: ['D', 'date'],
                month: ['M', 'month'],
                year: ['Y', 'year'],
                hour: ['[Hh]', 'hours'],
                minute: ['m', 'minutes'],
                second: ['s', 'seconds'],
                ampm: ['[Aa]', '']
            };
        },
        getCurrent: function () {
            return $("#wiz-container").find('.active');
        },
        displayStep: function (step) {
            var current = $("#wiz-container").find('.active');
            current.removeClass('active')
            $("#wiz-" + step).addClass('active');
        },
        attachIframe: function (step, src, width, height, scrolling) {
            this.displayStep(step);
            app_iframe.attachIframe("wiz-" + step, src, width, height, scrolling);
        },
        closeIframe: function () {
            this.displayStep(1);
        },

        displayWizardValidation: function (message) {
            $('#wizard-validation').val(message);
            return false;
        }
    };

    $.fn.wiztabs = function (option) {
        var
        slf = $(this)
       , _window = $(window)
       , _document = $(document)
        ;
        //args.shift();

        //getValue returns date as string / object (not jQuery object)
        if (option === 'getValue' && this.length && (d = this.eq(0).data('wiztabs'))) {
            return d.getValue.apply(d, args);
        }

        return this.each(function () {
            var $this = $(this),
            data = $this.data('wiztabs'),
            options = typeof option == 'object' && option;
            if (!data) {
                $this.data('wiztabs', (data = new Combodate(this, options)));
            }
            if (typeof option == 'string' && typeof data[option] == 'function') {
                data[option].apply(data, args);
            }
        });
    };
})(jQuery)



/*


Type 1:

(function($) {
    $.fn.jPluginName = {

        },

        $.fn.jPluginName.defaults = {

        }
})(jQuery);


Type 2:

(function($) {
    $.jPluginName = {

        }
})(jQuery);


Type 3:

(function($){

    //Attach this new method to jQuery
    $.fn.extend({ 

        var defaults = {  
        }  

        var options =  $.extend(defaults, options);  

        //This is where you write your plugin's name
        pluginname: function() {

            //Iterate over the current set of matched elements
            return this.each(function() {

                //code to be inserted here

            });
        }
    }); 
})(jQuery);

*/