//app-model
String.prototype.jsonEscape = function () { return this.replace(/\\/g, "\\\\").replace(/\n/g, "\\n").replace(/\r/g, "\\r").replace(/\t/g, "\\t").replace(/\f/g, "\\f").replace(/"/g, "\\\"").replace(/'/g, "\\\'").replace(/\&/g, "\\&"); }

$.fn.hasClassStartsWith = function (prefix) {
    return $(this).is(`[class^=${prefix}]`);
};

var responsiveIframe = function () {
    // Find all iframes
    var $iframes = $("iframe");

    // Find &#x26; save the aspect ratio for all iframes
    $iframes.each(function () {
        $(this).data("ratio", this.height / this.width)
            // Remove the hardcoded width &#x26; height attributes
            .removeAttr("width")
            .removeAttr("height");
    });

    // Resize the iframes when the window is resized
    $(window).resize(function () {
        $iframes.each(function () {
            // Get the parent container&#x27;s width
            var width = $(this).parent().width();
            $(this).width(width)
                .height(width * $(this).data("ratio"));
        });
        // Resize to fix all iframes on page load.
    }).resize();
}

var config = {
    base: '',//'/party'
    debug: false,
    mobileWidth: '300px',
    defaultColor: '#46d6db',
    theme: 'nis_metro',
    domain:'localhost'//'.my-t.co.il'
};

var app = {
    globalID: 1,
    isMobileDevice: undefined,
    baseClassNames: {
        app: 'app',
        content: 'app-content',
        overlay: 'app-overlay'
    },
    defaultOptions: {
        content: '',
        appendLocation: 'body',
        className: '',
        css: {}
    },
    IsMobile: function () {

        if (this.isMobileDevice === undefined) {
            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
                || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4)))
                this.isMobileDevice = true;
            else
                this.isMobileDevice = false;
        }
        return this.isMobileDevice;
    },
    domainName: function () {
        var hostName = window.location.hostname;
        return hostName.substring(hostName.lastIndexOf(".", hostName.lastIndexOf(".") - 1) + 1);
    },
    appPath: function () {
        return window.location.protocol + "//" + window.location.host + config.base;
    },
    actionPath: function (action, conroller) {
        var link = window.location.protocol + "//" + window.location.host + config.base + "/" + conroller + "/" + action;
        return link;
    },
    redirectTo: function (url) {

        // similar behavior as an HTTP redirect
        //window.location.replace(url);

        // similar behavior as clicking on a link
        window.location.href = url;
    },
    goReferrer: function () {
        var ref = document.referrer;
        if (ref !== null && ref !== '') {
            if (ref.match(/:\/\/(.[^/]+)/)[1])
                window.location.href = ref;
        }
    },
    refresh: function () {
        location.reload();
    },
    getUrlPage: function (path) {
        //var path = window.location.pathname;
        var page = path.split("/").pop();
        return page;
    },
    isNull: function (value, valueIfnull) {
        if (value === undefined || value == null)
            return valueIfnull;// || '';
        return value;
    },
    toFloat: function (value, defaultVal) {
        var num = parseFloat(value);
        if (isNaN(num))
            return defaultVal
        return num;
    },
    toInt: function (value, defaultVal) {
        var num = parseInt(value, 10);
        if (isNaN(num))
            return defaultVal
        return num;
    },
    isInt: function (n) {
        return Number(n) === n && n % 1 === 0;
    },
    isFloat: function (n) {
        return n === Number(n) && n % 1 !== 0;
    },
    getFormInputs: function (forms) {
        var postData = [];

        $.each(forms, function (index, value) {
            var form = value
            $(form + ' input, ' + form + ' select, ' + form + ' textarea').each(
                function (index) {
                    var input = $(this);
                    postData.push(input.attr('name') + "=" + encodeURIComponent(input.val()));
                    //alert('Type: ' + input.attr('type') + 'Name: ' + input.attr('name') + 'Value: ' + input.val());
                }
            );
        });
        return postData.join("&");
    },
    serializeForms: function (forms) {
        var postData = [];

        $.each(forms, function (index, value) {
            var form = value
            $(form + ' input, ' + form + ' select, ' + form + ' textarea,' + form + 'hidden').each(
                function (index) {
                    var input = $(this);
                    var tag = input.attr('name');
                    if (tag !== undefined) {
                        var val = input.val();
                        if (input[0].type == "checkbox")
                            val = input[0].checked;
                        else if (val) {
                            if (typeof val === 'object')
                                val = val.value;
                        }
                        postData.push(tag + "=" + encodeURIComponent(val));
                    }
                    //alert('Type: ' + input.attr('type') + 'Name: ' + input.attr('name') + 'Value: ' + input.val());
                }
            );
        });
        return postData.join("&");
    },
    serialize: function (form) {
        var postData = [];
        $(form + ' input, ' + form + ' select, ' + form + ' textarea,' + form + 'hidden').each(
            function (index) {
                var input = $(this);
                var tag = input.attr('name');
                if (tag !== undefined)
                    var val = input.val();
                if (input[0].type == "checkbox")
                    val = input[0].checked;
                else if (val) {
                    if (typeof val === 'object')
                        val = val.value;
                }
                else
                    val = "";
                postData.push(tag + "=" + encodeURIComponent(val));
                //alert('Type: ' + input.attr('type') + 'Name: ' + input.attr('name') + 'Value: ' + input.val());
            }
        );
        return postData.join("&");
    },
    serializeEx: function (formInputs, exArgs) {
        var postData = [];
        //form + ' input, ' + form + ' select, ' + form + 'hidden'
        $(formInputs).each(function (index) {
            var input = $(this);
            var tag = input.attr('name');
            if (tag !== undefined) {
                var val = input.val();
                if (input[0].type == "checkbox")
                    val = input[0].checked;
                else if (val) {
                    if (typeof val === 'object')
                        val = val.value;
                }
                else
                    val = "";
                postData.push(tag + "=" + encodeURIComponent(val));
            }
        });
        if (exArgs) {
            for (var i = 0; i < exArgs.length; i++) {
                postData.push(exArgs[i].key + "=" + encodeURIComponent(exArgs[i].value));
            }
        }
        return postData.join("&");
    },
    serializeArrayToJson: function (form) {
        return JSON.stringify($(form).serializeArray());
    },
    serializeToJsonObject: function (form) {
        var unindexed_array = $(form).serializeArray();
        var indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    },
    htmlEncode: function (value) {
        return $('<div/>').text(value).html();
    },
    htmlDecode: function (value) {
        return $('<div/>').html(value).text();
    },
    htmlEscape: function (str) {
        return str

            //.replace(/{/g, '&#123;')
            //.replace(/}/g, '&#125;')
            //.replace('/', '&#47;')
            //.replace(/%/g, '&#37;')
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/\\/g, '\\\\')
            .replace(/'/g, '&apos;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    },
    htmlUnescape: function (str) {
        //console.log(str);
        return str
            //.replace(/&#123;/g, '{')
            //.replace(/&#125;/g, '}')
            //.replace(/&#47;/g, '/')
            //.replace(/&#37;/g, '%')
            .replace(/&quot;/g, '"')
            .replace(/\\\\/g, "\\")//
            .replace(/&apos;/g, "'")
            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>')
            .replace(/&amp;/g, '&');
    },
    htmlText: function (html) {
        //var text = jQuery(html).text();
        //value.replace(/(<([^>]+)>)/ig, "");
        var text = html.replace(/<(?:.|\n)*?>/gm, '');
        return text;
        //return html.replace(/<(?:.|\n)*?>/gm, '');
    },
    textToHtml: function (str) {
        if (!str)
            return "";
        str = str.replace(/(?:\r\n|\r|\n)/g, '<br>');
        return str;
    },
    requestQuery: function (name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    guid: function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    },
    UUIDv4: function () {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },
    UUID: function (v) {
        var d = new Date().getTime();
        if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
            d += performance.now(); //use high-precision timer if available
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    },
    //UUID_old:function(v) {
    //    if (v === undefined || v == 'v4')
    //        return app.guid();//Math.uuid() // RFC4122 v4 UUID
    ////"4FAC90E7-8CF1-4180-B47B-09C3A246CB67"
    //if (v == '62')
    //    return Math.uuid(17) // 17 digits, base 62 (0-9,a-Z,A-Z)
    ////"GaohlDbGYvOKd11p2"

    //if (v == '10')
    //    return Math.uuid(5, 10) // 5 digits, base 10
    ////"84274"

    //if (v == '16')
    //    return Math.uuid(8, 16) // 8 digits, base 16
    ////"19D954C3"
    //},
    jsonToHtml: function (json, appendTo) {
        var ul = $('<ul>').appendTo(appendTo);//('body');
        //var json = { items: ['item 1', 'item 2', 'item 3'] };
        $(json.items).each(function (index, item) {
            ul.append(
                $(document.createElement('li')).text(item)
            );
        });
    },
    arrayToHtml: function (array, appendTo) {
        var ul = $('<ul>').appendTo(appendTo);//('body');
        //var json = { items: ['item 1', 'item 2', 'item 3'] };
        for (var i = 0; i < array.length; i++) {
            ul.append(
                $(document.createElement('li')).text(array[i]['name'])
            );
            ul.append(
            $(document.createElement('li')).text(array[i]['value'])
            );
        }
    },
    parseJsonDate: function (value) {
        var reg = /^\/Date\((d|-|.*)\)[\/|\\]$/;
        if (typeof value === 'string') {
            //var strd = /\/Date\((\d*)\)\//.exec(value);
            var strd = reg.exec(value);
            return (strd) ? new Date(+strd[1]) : value;
        }
        return value;
        /*
        if (window.JSON && !window.JSON.dateParser) {
            var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*))(?:Z|(\+|-)([\d|:]*))?$/;
            var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

            JSON.dateParser = function (key, value) {
                if (typeof value === 'string') {
                    var a = reISO.exec(value);
                    if (a)
                        return new Date(value);
                    a = reMsAjax.exec(value);
                    if (a) {
                        var b = a[1].split(/[-+,.]/);
                        return new Date(b[0] ? +b[0] : 0 - +b[1]);
                    }
                }
                return value;
            };

        }
        */
        //value = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
        //return new Date(value);
    },
    jsonDateToString : function (value,addTime) {
        if (typeof value === 'string') {
            var strd = /^\/Date\((d|-|.*)\)[\/|\\]$/.exec(value);
            if (strd) {
                var d = new Date(+strd[1]);
                return app.dateToString(d,addTime);
            }
        }

        return value;
        //value = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
        //return new Date(value);
    },
    dateToString: function (d, addTime) {

        //if (d === typeof (Date)) {
                if (addTime)
                    return d.toLocaleString().replace(/[^T0-9\-/\:\. (am|pm)]/gi, "");
                else
                    return d.toLocaleDateString().replace(/[^T0-9\-/\:\. (am|pm)]/gi, "");
        //}

        //return d;
    },
    toLocalDateTimeString: function (strdate) {
        /*
        var d = new Date();
        d + '';                // "Sun Dec 08 2013 18:55:38 GMT+0100"
        d.toDateString();      // "Sun Dec 08 2013"
        d.toISOString();       // "2013-12-08T17:55:38.130Z"
        d.toLocaleDateString() // "8/12/2013" on my system
        d.toLocaleString()     // "8/12/2013 18.55.38" on my system
        d.toUTCString()        // "Sun, 08 Dec 2013 17:55:38 GMT"
        */
        return app.formatDateTimeString(strdate);

        //if (strdate === undefined || strdate == null || strdate == '')
        //    return '';
        //if (typeof strdate === 'string') {
        //    var strd = /\/Date\((\d*)\)\//.exec(strdate);
        //    if (strd)
        //        return app.dateToString(new Date(+strd[1]), true);
        //}
        //if (strdate === typeof (Date))
        //    return app.dateToString(strdate, true);
        //var d = new Date(strdate);
        //if (d.toString() == "NaN" || d.toString() == "Invalid Date") {
        //    //    if ($.jqx.dataFormat) {
        //    //        f = $.jqx.dataFormat.tryparsedate(new Date(value));
        //    //        return f;
        //    //   }
        //    return '';
        //}
        //return app.dateToString(d,true);
    },
    toLocalDateString: function (strdate) {
        /*
        var d = new Date();
        d + '';                // "Sun Dec 08 2013 18:55:38 GMT+0100"
        d.toDateString();      // "Sun Dec 08 2013"
        d.toISOString();       // "2013-12-08T17:55:38.130Z"
        d.toLocaleDateString() // "8/12/2013" on my system
        d.toLocaleString()     // "8/12/2013 18.55.38" on my system
        d.toUTCString()        // "Sun, 08 Dec 2013 17:55:38 GMT"
        */

        return app.formatDateTimeString(strdate, 'dd/mm/yyyy');

        //if (strdate === undefined || strdate == null || strdate == '')
        //    return '';
        //if (typeof strdate === 'string') {
        //    var strd = /\/Date\((\d*)\)\//.exec(strdate);
        //    if (strd)
        //        return app.dateToString(new Date(+strd[1]));
        //}
        //if (strdate === typeof (Date)) {
        //    return app.dateToString(strdate);
        //}
        //var d = new Date(strdate);
        //if (d.toString() == "NaN" || d.toString() == "Invalid Date") {
        //    //    if ($.jqx.dataFormat) {
        //    //        f = $.jqx.dataFormat.tryparsedate(new Date(value));
        //    //        return f;
        //    //   }
        //    return '';
        //}
        //return app.dateToString(d);
    },
    formatDateTimeIso: function (date, format) {
        return formatDateTimeString(date,'yyyy-mm-dd hh:mm:ss');
    },
    formatDateTimeString: function (date, format) {

        if (date === undefined || date == null || date == '')
            return '';
        if (format === undefined || format == null)
            format = 'dd/mm/yyyy hh:mm:ss';

        //if (date === typeof (Date)) {
        //    return date.format(format);
        //}
        if (date instanceof Date){// && typeof date.getMonth === 'function') {
            return date.format(format);
        }
        else {//if (typeof date === 'string') {

            var strdate = date.toString();
            if (strdate.indexOf('Date') != -1) {
                var jdate = app.parseJsonDate(date);
                if (jdate){// && jdate === typeof (Date)) {
                    return jdate.format(format);
                }
            }

            var d = new Date(date);
            return d.format(format);

            //if (d) {
            //    return d.format(format);
            //}
            //else {
            //    var jdate = app.parseJsonDate(date);
            //    if (jdate && jdate === typeof (Date)) {
            //        return jdate.format(format);
            //    }
            //    return jdate;
            //}
        }
    },
    formatDateString: function (date, format) {
        
        if (format === undefined || format == null)
            format = 'dd/mm/yyyy';
        return app.formatDateTimeString(date, format);


        //if (date === undefined || date == null || date == '')
        //    return null;

        //if (date === typeof (Date)) {
        //    return date.format(format);
        //}
        //else if (typeof date === 'string') {
        //    var strd = /\/Date\((\d*)\)\//.exec(date);
        //    if (strd) {
        //        var f = new Date(+strd[1]);
        //        return date.format(format);
        //    }
        //}

        //var f = new Date(value);
        //if (f.toString() == "NaN" || f.toString() == "Invalid Date") {
        //    if ($.jqx.dataFormat) {
        //        f = $.jqx.dataFormat.tryparsedate(new Date(value));
        //        return f;
        //    }
        //}

        //var f = new Date(parseInt(date.toString().replace("/Date(", "").replace(")/", ""), 10));
        //return f.format(format);

    },
    toggle: function (tag) {
        $(tag).toggle();
    },
    toYesNo: function (value) {
        if (value === undefined || value == null)
            return 'לא ידוע';
        return value ? 'כן' : 'לא';
    },
    NZ: function (value, match, valueIfMatch) {
        if (value === undefined || value == null)
            return valueIfMatch;
        if (value == match)
            return valueIfMatch;
        return value;
    },
    showIf: function (tag, condition) {
        if (condition)
            $(tag).show();
        else
            $(tag).hide();
    },
    hideOrData: function (tag, data, match) {
        if (data === undefined || data == null || data == match)
            $(tag).hide();
        else
            $(tag).text(data);
    },
    cancelBubble: function stopPropagation(evt) {
        if (typeof evt.stopPropagation == "function") {
            evt.stopPropagation();
        } else {
            evt.cancelBubble = true;
        }
    },
    disableSelect: function (tag) {
        $(tag+ " option").not(':selected').each(function (index) {
            $(this).prop('disabled', true);
        });
    }
   
};


//============================================================================================ app_cookies

app_cookies = {
    getItem: function (sKey) {
        if (!sKey || !this.hasItem(sKey)) { return null; }
        return unescape(document.cookie.replace(new RegExp("(?:^|.*;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*((?:[^;](?!;))*[^;]?).*"), "$1"));
    },
    setItemConfig: function (sKey, sValue, bSecure) {//config.domain
        app_cookies.setItem(sKey, sValue, 60 * 60 * 24 * 30, "/", app.domainName(), bSecure);
    },
    /**
    * app_cookies.setItem(sKey, sValue, vEnd, sPath, sDomain, bSecure)
    *
    * ;max-age=max-age-in-seconds (e.g., 60*60*24*365 for a year)
    * @argument sKey (String): the name of the cookie;
    * @argument sValue (String): the value of the cookie;
    * @optional argument vEnd (Number, String, Date Object or null): the max-age in seconds (e.g., 31536e3 for a year) or the
    *  expires date in GMTString format or in Date Object format; if not specified it will expire at the end of session; 
    * @optional argument sPath (String or null): e.g., "/", "/mydir"; if not specified, defaults to the current path of the current document location;
    * @optional argument sDomain (String or null): e.g., "example.com", ".example.com" (includes all subdomains) or "subdomain.example.com"; if not
    * specified, defaults to the host portion of the current document location;
    * @optional argument bSecure (Boolean or null): cookie will be transmitted only over secure protocol as https;
    * @return undefined;
    **/
    setItem1: function (sKey, sValue, sPath, sDomain, bSecure) {

        app_cookies.setItem(sKey, sValue, 60 * 60 * 24 * 1, sPath, sDomain, bSecure);
    },
    setItem30: function (sKey, sValue, sPath, sDomain, bSecure) {

        app_cookies.setItem(sKey, sValue, 60 * 60 * 24 * 30, sPath, sDomain, bSecure);
    },
    setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {

        if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/.test(sKey)) { return; }
        var sExpires = "";
        if (vEnd === undefined || vEnd == null)
            vEnd = 60 * 60 * 24 * 30;
        if (vEnd) {
            switch (typeof vEnd) {
                case "number": sExpires = "; max-age=" + vEnd; break;
                case "string": sExpires = "; expires=" + vEnd; break;
                case "object": if (vEnd.hasOwnProperty("toGMTString")) { sExpires = "; expires=" + vEnd.toGMTString(); } break;
            }
        }
        document.cookie = escape(sKey) + "=" + escape(sValue) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
    },
    removeItem: function (sKey) {
        if (!sKey || !this.hasItem(sKey)) { return; }
        var oExpDate = new Date();
        oExpDate.setDate(oExpDate.getDate() - 1);
        document.cookie = escape(sKey) + "=; expires=" + oExpDate.toGMTString() + "; path=/";
    },
    hasItem: function (sKey) { return (new RegExp("(?:^|;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie); },
    getEncItem: function (sKey) {
        if (!sKey || !this.hasItem(sKey)) { return null; }
        var val = unescape(document.cookie.replace(new RegExp("(?:^|.*;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*((?:[^;](?!;))*[^;]?).*"), "$1"));
        return val ? decodeURIComponent(val) : null;
    },
    setEncItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
        if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/.test(sKey)) { return; }
        var sExpires = "";
        if (vEnd) {
            switch (typeof vEnd) {
                case "number": sExpires = "; max-age=" + vEnd; break;
                case "string": sExpires = "; expires=" + vEnd; break;
                case "object": if (vEnd.hasOwnProperty("toGMTString")) { sExpires = "; expires=" + vEnd.toGMTString(); } break;
            }
        }
        var encVal = encodeURIComponent(sValue);
        document.cookie = escape(sKey) + "=" + escape(encVal) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
    },

    
};

// app_cookies.setItem("test1", "Hello world!");
// app_cookies.setItem("test2", "Hello world!", new Date(2020, 5, 12));
// app_cookies.setItem("test3", "Hello world!", new Date(2027, 2, 3), "/blog");
// app_cookies.setItem("test4", "Hello world!", "Sun, 06 Nov 2022 21:43:15 GMT");
// app_cookies.setItem("test5", "Hello world!", "Tue, 06 Dec 2022 13:11:07 GMT", "/home");
// app_cookies.setItem("test6", "Hello world!", 150);
// app_cookies.setItem("test7", "Hello world!", 245, "/content");
// app_cookies.setItem("test8", "Hello world!", null, null, "example.com");
// app_cookies.setItem("test9", "Hello world!", null, null, null, true);

// alert(app_cookies.getItem("test1"));

//============================================================================================ app_perms

var app_perms = {
    isReadonly: function (option) {
        return option ? /^[^aef]/i.test(option) : false; 
        //return option === 'r' || option === 'g';
    },
    isEditable: function (option, addAlso) {
        return option ? /^[aef]/i.test(option) : false; 
        //return (option === 'e') || (option === 'a' && addAlso);
    },
    isAddable: function (option) {
        return (option.indexOf('a') != -1);
    },
    isDelable: function (option) {
        return (option.indexOf('d') != -1);
        //return option === 'd';
    },
    //getPerm: function (option, perms) {
    //    //g-e-a-d-f
    //    if (perms === undefined || perms == null)
    //        return false;
    //    if (perms === 'f')
    //        return true;

    //    switch (option) {
    //        case 'e':
    //            return perms === 'e';
    //        case 'a':
    //            return perms === 'a';
    //        case 'd':
    //            return perms === 'd';
    //        case 'g':
    //        case 'r':
    //            return perms === 'g';
    //        default:
    //            return (option.indexOf(perms) != -1);
    //    }
    //},
    readonlyForm: function (form, readonly) {

        if (readonly === undefined || readonly === null)
            readonly = true;
        $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
            var input = $(this);
            var type = input.attr('type');
            if (type !== 'button' && type !== 'submit' && type !== 'reset')//(type == 'check' || type == 'radio' || type == 'text' || type == 'number' || type == 'date')
            {
                input.prop('readonly', readonly);
            }
        });
    },
    //readonlyOptions: function (inputs, option) {
    //    var readonly = option ? app_perms.isReadonly(option) : true;
    //    $.each(inputs, function (index, value) {
    //        $('#' + value).prop('readonly', readonly);
    //    });
    //},
    readonlyItems: function (inputs, readonly, option) {

        if (readonly === undefined || readonly === null || readonly === 'NA')
            readonly = option ? app_perms.isReadonly(option) : true;
        $.each(inputs, function (index, value) {
            $('#' + value).prop('readonly', readonly);
        });
    },
    //pasiveItems: function (inputs, option, perms) {
    //    var perm = app_perms.getPerm(option, perms);
    //    $.each(inputs, function (index, value) {
    //        $('#' + form + '#' + value).removeClass('pasive');
    //    });
    //},
    readonlyInput: function (tag, readonly) {
        if (readonly === undefined || readonly === null)
            readonly = true;
        $(tag).prop('readonly', readonly);
    },
    disabledInput: function (tag, disabled) {
        if (disabled === undefined || disabled === null)
            disabled = true;
        $(tag).prop('disabled', disabled);
    }

    //readonlyInput: function (tag, option, perms) {
    //    //var perm = app_perms.getPerm(option, perms);
    //    var readonly = option != perms;
    //    $(tag).prop('readonly', app_perms.isReadonly(option));
    //},
    //disabledInput: function (tag, option) {
    //    //var perm = app_perms.isReadonly(option, perms);
    //    $(tag).prop('disabled', app_perms.isReadonly(option));
    //}
}

//============================================================================================ app_global

var app_global = {

    documentWidth: function () {
        var myWidth = 0, myHeight = 0;
        if (typeof (window.innerWidth) == 'number') {
            //Non-IE
            myWidth = window.innerWidth;
            myHeight = window.innerHeight;
        } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
            //IE 6+ in 'standards compliant mode'
            myWidth = document.documentElement.clientWidth;
            myHeight = document.documentElement.clientHeight;
        } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
            //IE 4 compatible
            myWidth = document.body.clientWidth;
            myHeight = document.body.clientHeight;
        }
        return [myWidth, myHeight]
        //window.alert('Width = ' + myWidth);
        //window.alert('Height = ' + myHeight);
    },
    documentScroll: function () {
        var scrOfX = 0, scrOfY = 0;
        if (typeof (window.pageYOffset) == 'number') {
            //Netscape compliant
            scrOfY = window.pageYOffset;
            scrOfX = window.pageXOffset;
        } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
            //DOM compliant
            scrOfY = document.body.scrollTop;
            scrOfX = document.body.scrollLeft;
        } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
            //IE6 standards compliant mode
            scrOfY = document.documentElement.scrollTop;
            scrOfX = document.documentElement.scrollLeft;
        }
        return [scrOfX, scrOfY];
    }
}

//============================================================================================ app_messenger

var app_messenger = {
    //'messenger-fixed messenger-on-bottom messenger-on-right'
    //'messenger-fixed messenger-on-top'
    Init: function (location, theme) {
        if (typeof location === 'undefined') { location = "messenger-fixed messenger-on-top"; }
        else if (location == 'bottom-right') { location = "messenger-fixed messenger-on-bottom messenger-on-right"; }
        else if (location == 'top') { location = "messenger-fixed messenger-on-top"; }
        if (typeof theme === 'undefined') { theme = "block"; }

        Messenger.options = {
            extraClasses: location,
            theme: theme
        }
    },
    //Alert: function (message, type, showClose) {
    //    if (typeof showClose === 'undefined') { showClose = false; }
    //    if (typeof type === 'undefined') { type = "info"; }
    //    Messenger().post({
    //        message: message,
    //        type: type,
    //        showCloseButton: showClose,
    //        hideOnNavigate: !showClose
    //    });
    //    //hideAfter: 10,
    //    //hideOnNavigate: true
    //    //Messenger().post(message);
    //},
    Notify: function (data, type, redirectTo) {
        if (typeof type === 'undefined') { type = "info"; }
        
        var msg = Messenger().post({
            message: (typeof data === 'string') ? data : data.Message,
            type: type,
            showCloseButton: (redirectTo)? true:false,
            actions: {
                click: {
                    label: 'אישור',
                    action: function () {
                        //if (redirectTo) {
                        //    if (typeof data === 'object') {
                        //        if (data.Status >= 0)
                        //            app.redirectTo(redirectTo);
                        //    }
                        //    else
                        //        app.redirectTo(redirectTo);
                        //}
                        return msg.cancel();
                    }
                }
            }
        });

        if (redirectTo) {
            if (typeof data === 'object') {
                if (data.Status >= 0)
                    app.redirectTo(redirectTo);
            }
            else
                app.redirectTo(redirectTo);
        }
        //return msg.cancel();
    },
    Post: function (data, type, showClose) {
        //if (typeof showClose === 'undefined') { showClose = false; }
        if (typeof type === 'undefined') { type = "info"; }
        if (typeof showClose === 'undefined') {
            showClose = true;
        }
        //var message = (typeof data === 'string') ? data : data.Message;
        Messenger().post({
            message: (typeof data === 'string') ? data : data.Message,
            type: type,
            showCloseButton: showClose
            //actions: {
            //    click: {
            //        label: 'אישור',
            //        action: function () {
            //            //if (callback && type != "error") {
            //            //    (args) ? callback(data, args) : callback(data);
            //            //}
            //            return msg.cancel();
            //        }
            //    }
            //}
        });
        //msg.on('action:click', function () {
        //    return msg;//app_dialog.alert('Hey, you retried!');
        //});

        //msg.on('action:retry', function () {
        //    returnapp_dialog.alert('Hey, you retried!');
        //});

        //Messenger().post(message);
    },
    Error: function (message, showClose) {
        if (typeof showClose === 'undefined') {
            showClose = true;
        }
        Messenger().post({
            message: message,
            type: "error",
            showCloseButton: showClose
        });
    },
    Dialog: function (data, callback, args) {
        var type = (data.Status > 0) ? 'success' : 'error';
        var msg= Messenger().post({
            message: data.Message,
            type: type,
            showCloseButton: false,
            actions: {
                cancel: {
                    label: 'אישור',
                    action: function () {
                        if (callback && type != "error") {
                            (args) ? callback(data, args) : callback(data);
                        }
                        return msg.cancel();
                    }
                }
            }
        });


        //Messenger().post(message);
    }
/*
    Retry: function (data, type, showClose, callback, args) {
        if (typeof showClose === 'undefined') { showClose = false; }
        if (typeof type === 'undefined') { type = "info"; }
        var message = (typeof data === 'string') ? data : data.Message;
 
        if (data.Status>0)

        var msg = Messenger().post({
            message: data.Message,
            type: type,
            showCloseButton: showClose,
            actions: {
                retry: {
                        label: 'נסה שוב',
                        phrase: 'Retrying TIME',
                        auto: true,
                        delay: 10,
                        action: function () {


                        },
                cancel: {
                    label: 'אישור',
                    action: function () {
                        if (callback && type != "error") {
                            (args) ? callback(data,args) : callback(data);
                        }
                        return msg.cancel();
                    }
                }
            }
        });

*/


/*
        msg = Messenger().post({
            message: message,
            type: type,
            showCloseButton: showClose,
            actions: {
                cancel: {
                    label: 'ביטול',
                    action: function () {
                        return msg.update({
                            events: {
                                'success click': function () {
                                    if (callback && template != "error") {
                                         (args) ? callback(args) : callback(data);
                                    }
                                }
                            }

                            //message: 'Thermonuclear war averted',
                            //type: 'success',
                            //actions: false
                        });
                    }
                }
            }
        });
    }*/
};

//============================================================================================ app_model

var app_model = {

    postModel: function (url, postData, callback) {
        $.ajax({
            async: false,
            url: url,
            type: "POST",
            dataType: 'json',
            //contentType: "application/json; charset=utf-8",
            //contentType: "application/x-www-form-urlencoded;charset=utf-8",
            data: postData,
            success: function (data) {
                if (callback)
                callback(data);
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
        //return false;
    }
};

//============================================================================================ app_query

var app_query = {

 
    doFormSubmit: function (formTag, actionUrl, formData, callback) {
        //e.preventDefault();
        if (actionUrl === undefined || actionUrl == null)
            actionUrl = $(formTag).attr('action');

        if (formData === undefined || formData == null)
            formData = $(formTag).serialize();// app.serialize(formTag);

        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionUrl,
                    type: 'post',
                    dataType: 'json',
                    data: formData,
                    success: function (data) {
                        if (callback)
                            callback(data);
                        else
                            app_messenger.Post(data);
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Notify(error, 'error');
                    }
                });
            }
            else {
                //not valid
            }
        }
        $(formTag).jqxValidator('validate', validationResult);

        return this;
    },
    doFormPost: function (formTag, callback, preSubmit, validatorTag) {
        if (validatorTag)
            $(validatorTag).empty();
        var actionurl = $(formTag).attr('action');
        if (preSubmit)
            preSubmit();
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $(formTag).serialize(),
                    success: function (data) {
                        if (callback)
                            callback(data);
                        else
                            app_dialog.alert(data.Message);
                    },
                    error: function (jqXHR, status, error) {
                        app_dialog.alert(error);
                    }
                });
            }
            else {
                if (validatorTag)
                    $(validatorTag).text("חסרים פרטים הכרחיים לעדכון");
            }
        }
        $(formTag).jqxValidator('validate', validationResult);
    },
    doDataPost: function (url, data, callback, args) {
        $.ajax({
            url: url,
            type: 'post',
            dataType: 'json',
            data: data,
            success: function (data) {
                if (data.Status > 0)
                    app_messenger.Post(data);
                else
                    app_messenger.Post(data, 'error');
                if (callback) {
                    if (args)
                        callback(data, args);
                    else
                        callback(data);
                }
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
    },
    doPost: function (url, data, callback,args) {
        $.ajax({
            url: url,
            type: 'post',
            dataType: 'json',
            data: data,
            success: function (data) {
                console.log(data);
                if (callback) {
                    if (args)
                        callback(data, args);
                    else
                        callback(data);
                }
                else if (data && data.Link)
                    app.redirectTo(data.Link);
                else if (data && data.Target.search(/input#/i) > -1)
                    $(data.Target).val(data.Message);
                else if (data && data.Target.search(/#/i) > -1)
                    $(data.Target).html(data.Message);
                else if (data && data.Message)
                    app_dialog.alert(data.Message);
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
    },
    doGet: function (url, data, callback) {
        $.ajax({
            url: url,
            type: 'get',
            dataType: 'json',
            data: data,
            success: function (data) {
                console.log(data);
                if (callback)
                    callback(data);
                //if (data && data.Link)
                //    app.redirectTo(data.Link);
                //else if (data && data.Target.search(/input#/i) > -1)
                //    $(data.Target).val(data.Message);
                //else if (data && data.Target.search(/#/i) > -1)
                //    $(data.Target).html(data.Message);
                //else if (data && data.Message)
                //    app_dialog.alert(data.Message);
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
    },
    doLookup: function (url, data, callback) {
        $.ajax({
            url: url,
            type: 'post',
            dataType: 'json',
            data: data,
            success: function (data) {
                console.log(data);
                if (callback) {
                    callback(data);//.Content);
                }
            },
            error: function (jqXHR, status, error) {
                //app_dialog.alert(error);
                app_messenger.Error(error);
            }
        });
    },
    doDataAdapter: function (url, data, callback) {
        $.ajax({
            url: url,
            type: 'post',
            dataType: 'json',
            data: data,
            success: function (data) {
                console.log(data);
                if (callback) {
                    callback(data);//.Content);
                }
            },
            error: function (jqXHR, status, error) {
                //app_dialog.alert(error);
                app_messenger.Error(error);
            }
        });
    }

};

//============================================================================================ app_validation

var app_validation = {
    notForMobile: function () {
        var ismobile = app.IsMobile();
        if (ismobile)
        {
            app_dialog.alert("This item not compatible for mobile");
        }
        return ismobile;
    },
    phone: function (value) {
        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
        return value ? re.test(value) : true;
    },
    phoneInput: function (input, commit) {
        var val = input.val();
        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
        return val ? re.test(val) : true;
    },
    memberId: function (str) {
 

        // DEFINE RETURN VALUES
        var ELEGAL_INPUT = -1;
        var NOT_VALID = -2;
        var VALID = 1;

        if (config.debug)
            return VALID;

        // Just in case -> convert to string
        var IDnum = String(str);

        // Validate correct input
        if ((IDnum.length > 9) || (IDnum.length < 5))
            return ELEGAL_INPUT;
        if (isNaN(IDnum))
            return ELEGAL_INPUT;

        // The number is too short - add leading 0000
        if (IDnum.length < 9) {
            while (IDnum.length < 9) {
                IDnum = '0' + IDnum;
            }
        }
        // CHECK THE ID NUMBER
        var mone = 0, incNum;
        for (var i = 0; i < 9; i++) {
            incNum = Number(IDnum.charAt(i));
            incNum *= (i % 2) + 1;
            if (incNum > 9)
                incNum -= 9;
            mone += incNum;
        }
        if (mone % 10 == 0)
            return VALID;
        else
            return  NOT_VALID;
    },
    signupValididty: function (AccountId, MemberId, CellPhone, Email, ExId,funk) {

        $.ajax({
            url: "/Registry/SignupValidity",
            type: 'POST',
            dataType: 'json',
            data: { 'AccountId': AccountId, 'MemberId': MemberId, 'CellPhone': CellPhone, 'Email': Email, 'ExId': ExId },
            success: function (data) {
                funk(data);
            }
        });
    }
};

//============================================================================================ app_dialog

var app_dialog = {

    //mode=auto|modal
    /*
    alert: function (msg, callback, args) {

        vex.dialog.alert({
            message: msg,
            className: 'vex-theme-default',
            callback: function (value) {
                if (callback)
                    callback(value, args);
                //console.log(value)
            }
        });
    },
    notify: function (msg, callback, args) {

        var d=vex.dialog.open({
            message: msg,
            className: 'vex-theme-default',
            //input: [
            //    '<input name="username" type="text" placeholder="Username" required />',
            //    '<input name="password" type="password" placeholder="Password" required />'
            //].join(''),
            buttons: [
                $.extend({}, vex.dialog.buttons.YES, { text: 'אישור' }),
                // $.extend({}, vex.dialog.buttons.NO, { text: 'Back' })
            ],
            callback: function (data) {
                if (callback) {
                    callback(args);
                }
                //if (!data) {
                //    console.log('Cancelled')
                //} else {
                //    console.log('Username', data.username, 'Password', data.password)
                //}
            }
        });

        setTimeout(function () {
            vex.close(d);
        }, 2000);
    },
    confirm: function (message, callback, args) {

        vex.dialog.confirm({
            message: message,
            className: 'vex-theme-default',
            callback: function (value) {
                if (callback)
                    callback(value,args);
                //console.log(value)
            }
        })
    },
    */
    
    alert: function (msg,callback,args) {
        //font: normal normal normal 10px/1.5 Arial, Helvetica, sans-serif;
        var d = $('<div id="alert-message" title="..." style="direction:rtl;">' +
            '<div style="margin-right: 20px;margin-top:10px;">' +
            '<p>' + msg + '</p></div></div>').dialog({
                modal: true,
                show: 'fade',
                hide: 'fade',
                dialogClass: 'ui-dialog-osx',
                buttons: [
                    {
                        text: "אישור",
                        "class": 'btn-dialog',
                        click: function () {
                            $(this).dialog("close");
                            if (callback) {
                                callback(args);
                            }
                        }
                    }
                ],
            });
    },
    notify: function (msg, callback, args) {
        var d = $('<div id="alert-message" title="..." style="direction:rtl;">' +
             '<div style="margin-right: 20px;margin-top:10px;">' +
             '<p>' + msg + '</p></div></div>').dialog({
                 modal: true,
                 show: 'fade',
                 hide: 'fade',
                 dialogClass: 'ui-dialog-osx',
                 buttons: [
                     {
                         text: "אישור",
                         "class": 'btn-dialog',
                         click: function () {
                             $(this).dialog("close");
                             if (callback) {
                                 callback(args);
                             }
                         }
                     }
                 ],
             });
        setTimeout(function () {
            d.dialog("close");
        }, 2000);
    },
    confirm: function (message, callback, args, onCancel) {
        var divmessage = $('<div class="rtl">' + message + '</div>');
        var dialog = $("<div class='bdialog'></div>").append(divmessage).appendTo("body").dialog({
            //var dialog = $('<div id="alert-message" title="..." style="direction:rtl;">' +
            //    '<div style="margin-right: 20px;margin-top:10px;">' +
            //    '<p>' + message + '</p></div></div>').dialog({
            resizable: false,
            height: "auto",
            width: 350,
            modal: true,
            dialogClass: 'ui-dialog-osx',
            buttons: {
                "אישור": function () {
                    //res = true;
                    $(this).dialog("close");
                    if (callback)
                        callback(args);
                },
                "ביטול": function () {
                    $(this).dialog("close");
                    if (onCancel)
                        onCancel(args);
                }
            }
        });
    },
    
    popMessage: function (caption, msg, mode, callback, args) {
        var modal = false;
        var auto = false;
        if (mode == "auto")
            auto = true;
        else if (mode == "modal")
            modal = true;
        //font: normal normal normal 10px/1.5 Arial, Helvetica, sans-serif;
        var d = $('<div id="pop-message" title="' + caption + '" style="direction:rtl;">' +
            '<div style="margin-right: 20px;margin-top:10px;">' +
            '<p>' + msg + '</p></div></div>').dialog({
                modal: true,
                show: 'blind',
                hide: 'blind',
                dialogClass: 'ui-dialog-osx',
                buttons: [
                    {
                        text: "אישור",
                        "class": 'btn-dialog',
                        click: function () {
                            $(this).dialog("close");
                            if (callback) {
                                callback(args);
                            }
                        }
                    }
                ],
            });
        if (auto) {
            setTimeout(function () {
                d.dialog("close");
            }, 2000);
        }
    },

    notifyMessage: function (caption, msg, mode, callback, args) {

        var modal = false;
        var auto = false;
        if (mode == "auto")
            auto = true;
        else if (mode == "modal")
            modal = true;

        var d = $('<div id="notify-message" title="' + caption + '" style="direction:rtl;">' +
            '<div style="margin-right: 20px;margin-top:10px;">' +
            '<p>' + msg + '</p></div></div>').dialog({
                modal: modal,
                draggable: false,
                resizable: false,
                position: ['center', 'top'],
                show: 'blind',
                hide: 'blind',
                width: '80%',
                dialogClass: 'ui-dialog-osx',
                buttons: [
                    {
                        text: "אישור",
                        "class": 'btn-dialog',
                        click: function () {
                            $(this).dialog("close");
                            if (callback) {
                                callback(args);
                            }
                        }
                    }
                ],
            });
        if (auto) {
            setTimeout(function () {
                d.dialog("close");
            }, 2000);
        }
    },

    dialogMessage: function (caption, msg, auto, modal, callback, args) {
        if (typeof auto === 'undefined') { auto = false; }
        if (typeof modal === 'undefined' || auto == true) { modal = false; }
        //;font: normal normal normal 10px/1.5 Arial, Helvetica, sans-serif;
        var d = $('<div id="dialog-message" title="' + caption + '" style="direction:rtl">' +
            '<div style="margin-right: 20px;margin-top:10px;">' +
            '<p>' + msg + '</p></div></div>').dialog({
                modal: modal,
                draggable: false,
                resizable: false,
                position: ['center', 'top'],
                show: 'blind',
                hide: 'blind',
                width: '80%',
                dialogClass: 'ui-dialog-osx',
                buttons: [
                    {
                        text: "אישור",
                        "class": 'btn-dialog',
                        click: function () {
                            $(this).dialog("close");
                            if (callback) {
                                callback(args);
                            }
                        }
                    }
                ],
            });
        if (auto) {
            setTimeout(function () {
                d.dialog("close");
            }, 2000);
        }
    },

    dialogForm: function (caption, href, modal) {
        if (typeof modal === 'undefined' || auto == true) { modal = false; }

        var d = $('<div id="dialog-form" title="' + caption + '" style="direction:rtl;"></div>');
        //font: normal normal normal 10px/1.5 Arial, Helvetica, sans-serif;
        d.dialog({
            autoOpen: false,
            modal: modal,
            draggable: false,
            resizable: false,
            position: ['center', 'top'],
            show: 'blind',
            hide: 'blind',
            width: '80%',
            dialogClass: 'ui-dialog-osx',
            buttons: {
                "class": 'btn-dialog',
                "אישור": function () {
                    $(this).dialog("close");
                }
            }
        });

        d.html("");
        d.dialog("option", "title", "Loading...").dialog("open");
        d.load(href, function () {
            //$(this).dialog("option", "title", $(this).find("h1").text());
            //$(this).find("h1").remove();
        });
    },

    dialogDiv: function (tag, title, scrolling) {
        if (!scrolling)
            scrolling = 'no';
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        var container = $(tag);
        var dialog = $("<div class='bdialog'></div>").append(container).appendTo("body").dialog({
            autoOpen: false,
            modal: true,
            resizable: false,
            width: "auto",
            height: "auto",
            title: title,
            dialogClass: 'ui-dialog-osx'
            //my: "center",
            //at: "center",
            //of: window
            //create: function (event, ui) {
            //    $(".ui-widget-header").hide();
            //},
            //close: function () {
            //    iframe.attr("src", "");
            //}
        });

        dialog.dialog("open");
        return dialog;
    },
    dialogIframe: function (src, width, height, title, scrolling) {
        if (!scrolling)
            scrolling = 'no';
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        var iframe = $('<iframe scrolling="' + scrolling + '" frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
        var dialog = $("<div class='bdialog'></div>").append(iframe).appendTo("body").dialog({
            autoOpen: false,
            modal: true,
            resizable: false,
            width: "auto",
            height: "auto",
            title: title,
            dialogClass: 'ui-dialog-osx',
            //create: function (event, ui) {
            //    $(".ui-widget-header").hide();
            //},
            close: function () {
                iframe.attr("src", "");
            }
        });

        iframe.attr({
            width: width,
            height: height,
            src: src
        });
        dialog.dialog("open");
        return dialog;
        //dialog.dialog("option", "title", title).dialog("open");
        //$(".ui-dialog-titlebar").hide();
    },

    dialogIframClose: function () {
        var d = $('.bdialog');
        if (d)
            d.dialog('close');
    },

    dialogClose: function (d) {
        if (d)
            d.dialog('close');
    },

    popupDialogClose: function () {
        var d = $('.bdialog');
        if (d)
            d.dialog('close');
    },
    confirmYesNoCancel: function (message, callback, args) {
        var divmessage = $('<div class="rtl">' + message + '</div>');
        var dialog = $("<div class='bdialog'></div>").append(divmessage).appendTo("body").dialog({
            resizable: false,
            height: "auto",
            width: 350,
            modal: true,
            dialogClass: 'ui-dialog-osx',
            buttons: {
                "כן": function () {
                    $(this).dialog("close");
                    if (callback)
                        callback('yes',args);
                },
                "לא": function () {
                    $(this).dialog("close");
                    if (callback)
                        callback('no',args);
                },
                "ביטול": function () {
                    $(this).dialog("close");
                    if (callback)
                        callback('cancel', args);
                }
            }
        });
    },
    doConfirm: function (message, postUrl, args) {
        app_dialog.confirm(message,
            callBack = function (args) {
                app_query.doPost(postUrl, args);
            }, args);
    },
    dialogProgress: function (title, input, ismodal, progress) {
        var progressTimer;
        //progressLabel = $('.progress-label');
        //progressbar = $('#progressbar');
        progressLabel = $('<div class="progress-label center">Wait...</div>');
        progressbar = $('<div id="progressbar"></div>');
        dialogButtons = [{
            text: "ביטול",
            click: closeProgress
        }];

        //dialog = $("#dialog").dialog({
            var dialog = $("<div class='bdialog' title='" + title + "'></div>").append(progressLabel).append(progressbar).appendTo("body").dialog({
            autoOpen: false,
            modal: ismodal,
            closeOnEscape: false,
            resizable: false,
            buttons: dialogButtons,
            open: function (event, ui) {
                $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                if (progress)
                    progressTimer = setTimeout(progress, 2000);
            },
            beforeClose: function () {
                if (input)
                    $(input).attr("disabled", false);
                //.button("option", {
                //    disabled: false,
                //    label: "התחל"
                //});
            }
        });
        function closeProgress() {
            clearTimeout(progressTimer);
            dialog
              .dialog("option", "buttons", dialogButtons)
              .dialog("close");
            progressbar.progressbar({ value: false });
            progressLabel
              .text("התחל...");
            if (input)
                $(input).trigger("focus");
        };
        progressbar.progressbar({
            value: false,
            change: function () {
                progressLabel.text(progressbar.progressbar("value") + "%");
            },
            complete: function () {
                progressLabel.text("התהליך הסתיים");
                dialog.dialog("option", "buttons", [{
                    text: "סגירה",
                    click: closeProgress
                }]);
                $(".ui-dialog button").last().trigger("focus");
            }
        });
        dialog.dialog("open");

        
        //<div id="dialog" title="Send broadcast">
        //        <div class="progress-label">Wait...</div>
        //        <div id="progressbar"></div>
        //</div>
        
        //function progress() {
        //    var val = progressbar.progressbar("value") || 0;
        //    progressbar.progressbar("value", val + Math.floor(Math.random() * 3));
        //    if (val <= 99) {
        //        progressTimer = setTimeout(progress, 50);
        //    }
        //}

    },
 
};

//============================================================================================ app_iframe

var app_iframe = {
    panelClose: function (tag, doempty) {
        tag = tag.replace("#", "");
        $("#" + tag).hide();
        if (doempty)
            $("#" + tag).empty();
    },
    panelSwitchClose: function (parentName, isSwitch, doempty) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";
        $("#" + tag).hide();
        if (isSwitch)
            $("#" + parentName).show();
        if (doempty)
            $("#" + tag).empty();
    },
    appendPanelSwitch: function (parentName, src, width, height, scroll, title) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        $("#" + parentName).hide();

        //app_iframe.showPanel("#" + tag, src, width, height, scroll, title);
        $("#" + tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
          .on("click", function (e) {
              e.preventDefault();
              $("#" + tag).hide();
              $("#" + parentName).show();
              $("#" + tag).empty();
          });
        panel.append(close);
        $("#" + tag).append(panel);

        app_iframe.appendIframe(tag, src, width, height, scroll);

        $("#" + tag).show();
    },
    appendPanel: function (parentName, src, width, height, scroll, title) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        app_iframe.showPanel("#" + tag, src, width, height, scroll, title);

    },
    showPanel: function (tag, src, width, height, scroll, title) {
        $(tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
          .on("click", function (e) {
              e.preventDefault();
              $(tag).hide();
              $(tag).empty();
          });
        panel.append(close);
        $(tag).append(panel);

        app_iframe.appendIframe(tag, src, width, height, scroll);

        $(tag).show();
    },
    appendIframe: function (div, src, width, height, scrolling,loaderTag) {
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        //$("#" + div).html('<div class="divWait">המתן...</div>');
        
        var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
        if (loaderTag) {
            var loader = $(loaderTag);//"#wiz-loader1");
            iframe.ready(function () {
                //loader.addClass('active');
                app_jqx.loaderOpen();
            })
            iframe.load(function () {
                //loader.removeClass('active');
                app_jqx.loaderClose();
            });
        }
        div = div.replace("#", "");
        $("#" + div).append(iframe);
        iframe.attr({
            scrolling: scrolling,
            width: width,
            height: height,
            src: src
        });
        //$("#" + div).find('.divWait').remove();
    },
    appendDocPanelSwitch: function (parentName, content, width, height, scroll, title) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        $("#" + parentName).hide();

        //app_iframe.showPanel("#" + tag, src, width, height, scroll, title);
        $("#" + tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
            .on("click", function (e) {
                e.preventDefault();
                $("#" + tag).hide();
                $("#" + parentName).show();
                $("#" + tag).empty();
            });
        panel.append(close);
        $("#" + tag).append(panel);

        app_iframe.appendDocIframe(tag, content, width, height, scroll);

        $("#" + tag).show();
    },
    appendDocIframe: function (tag, content, width, height, scrolling, loaderTag) {
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        //$("#" + div).html('<div class="divWait">המתן...</div>');
        var iframe_content = '<html><head></head><body>' + content + '</body></html>';

        var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
        if (loaderTag) {
            var loader = $(loaderTag);//"#wiz-loader1");
            iframe.ready(function () {
                //loader.addClass('active');
                app_jqx.loaderOpen();
            })
            iframe.load(function () {
                //loader.removeClass('active');
                app_jqx.loaderClose();
            });
        }
        tag = tag.replace("#", "");
        $("#" + tag).append(iframe);

        iframe.attr({
            scrolling: scrolling,
            width: width,
            height: height
            //src: src
        });

        // if jQuery is available, you may use the get(0) function to obtain the DOM object like this:
        //   var iframe = $('iframe#target_iframe_id').get(0);

        var iframe = $(iframe).get(0);
        var iframedoc = iframe[0].document;
        if (iframe[0].contentDocument)
            iframedoc = iframe[0].contentDocument;
        else if (iframe[0].contentWindow)
            iframedoc = iframe[0].contentWindow.document;

        if (iframedoc) {
            // Put the content in the iframe
            iframedoc.open();
            iframedoc.writeln(iframe_content);
            iframedoc.close();
        } else {
            //just in case of browsers that don't support the above 3 properties.
            //fortunately we don't come across such case so far.
            app_dialog.alert('Cannot inject dynamic contents into iframe.');
        }
        //$("#" + div).find('.divWait').remove();
    },
    responsiveIframe: function (tag) {

        // Remove the hardcoded width &#x26; height attributes
        $(tag).data("ratio", this.height / this.width)
            .removeAttr("width")
            .removeAttr("height");

        // Resize the iframes when the window is resized
        $(window).resize(function () {
            // Get the parent container&#x27;s width
            var width = $(tag).parent().width();
            $(tag).width(width).height(width * $(tag).data("ratio"));

            // Resize to fix all iframes on page load.
        }).resize();
    },
    loadIframe: function (div, src, width, height, scrolling) {
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
        $("#" + div).load(iframe);
        iframe.attr({
            scrolling: scrolling,
            width: width,
            height: height,
            src: src
        });
    },
    clearIframe: function (iframeTag,hideParent) {
        
        var iframe = $(iframeTag);
        iframe.attr({
            width: width,
            height: 0,
            src: null
        });
        if (hideParent) {
            var parent = iframe.parent().get(0);
            if(parent)
                parent.hide();
        }

    },
    attachIframe: function (tag, src, width, height, scrolling) {
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        var iframe = $("#" + tag)
        iframe.attr({
            scrolling: scrolling,
            width: width,
            height: height,
            src: src
        });
    },
    changeIframe: function (iframe, src) {
        if (iframe)
            iframe.attr({
                src: src
            });
    },
    removeIframe: function (div,iframeTag) {
        
        var div = $("#" + div);
        if (div.length > 0) {
            if (iframeTag) {
                var iframe = div.find(iframeTag);
                if (iframe)
                    iframe.remove();
            }
            else {
                div.children().remove();
            }
        }
    },
    loadAjax: function (div, src, width, height) {
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        var container = $("#" + div);

        container.load(src, '', function (response, status, xhr) {
            if (status == 'error') {
                var msg = "Sorry but there was an error: ";
                $(".content").html(msg + xhr.status + " " + xhr.statusText);
            }
        });
    },
    appendObject: function (div, src, width, height) {

        if (app.IsMobile()) {
            width = config.mobileWidth;
        }

        var container = $("#" + div);
        if (container.length > 0) {
            container.children().remove();
        }
        container.css('height', height);
        container.addClass('nis-loader');
        container.show();

        var iframe = $('<object type="text/html"></object>');
        //iframe.ready(function () {
        //    container.addClass('nis-loader');
        //})
        iframe.load(function () {
            container.removeClass('nis-loader');
        });

        iframe.attr({
            //type: "text/html",
            width: width,
            height: height,
            data: src
        });
        container.prepend(iframe).fadeIn('slow');
    },
    appendEmbed: function (div, src, width, height) {
        if (app.IsMobile()) {
            width = config.mobileWidth;
        }
        var container = $("#" + div);

        //if (container.is(':visible'))
        //{
        //    container.children().remove();
        //    container.hide();
        //    return;
        //}
        if (container.length > 0) {
            container.children().remove();
        }
        container.show();

        var iframe = $('<embed/>');
        iframe.ready(function () {
            container.addClass('nis-loader');
        })
        iframe.load(function () {
            container.removeClass('nis-loader');
        });

        container.prepend(iframe).fadeIn(999);
        iframe.attr({
            //type: "text/html",
            width: width,
            height: height,
            src: src
        });
    },
    removeEmbed: function (div, embedTag) {

        var div = $("#" + div);
        if (div.length > 0) {
            if (embedTag) {
                var iframe = div.find(embedTag);
                if (iframe)
                    iframe.remove();
            }
            else {
                div.children().remove();
            }
        }
    }

    //(function(d){
    //    var iframe = d.body.appendChild(d.createElement('iframe')),
    //    doc = iframe.contentWindow.document;

    //    // style the iframe with some CSS
    //    iframe.style.cssText = "position:absolute;width:200px;height:100px;left:0px;";
  
    //    doc.open().write('<body onload="' + 
    //    'var d = document;d.getElementsByTagName(\'head\')[0].' + 
    //    'appendChild(d.createElement(\'script\')).src' + 
    //    '=\'\/path\/to\/file\'">');
  
    //    doc.close(); //iframe onload event happens

    //})(document);
};

class panel_dialog_base {

    constructor($element,tagPrefix ,title) {
        this.$element = $($element);
        this.Adapter = null;
        this.Title = title;
        this.TagPrefix = tagPrefix;
    }

    loadHtml($element, readonly,callBack) {

        //var pasive = readonly ? " pasive" : "";

        //var html = '<div id="user-Window" class="container" style="margin:5px;width:400px">' +

        //    '<div>' +
        //    '<a id="user-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
        //    '<a id="user-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
        //    '</div>' +
        //    '</div>';

        //if ($element == null)
        //    return html;
    };

    inputRules() {

        //$('#user-Form').jqxValidator({
        //    rtl: true,
        //    hintType: 'label',
        //    animationDuration: 0,
        //    rules: input_rules
        //});
        //$('#user-Form').jqxValidator('hide');
    }

    loadData(actModel, record) {


    }

    init(actModel, record, dataAdapter, readonly) {
        this.AccountId = actModel.AccountId;
        this.Adapter = dataAdapter;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.loadHtml(this.$element, readonly, function (html) {

            app_panel.appendPanelAfter(slf.$element.selector, html, slf.Title);

        });


        this.loadData(actModel, record);

        $('#' + this.TagPrefix + '-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#' + this.TagPrefix + '-Cancel').on('click', function (e) {
            slf.doClose();
            return false;
        });

        this.inputRules();
    };

    display() {
        app_panel.panelAfterShow(this.$element.selector);
    }

    doClose() {
        app_panel.panelAfterClose(this.$element.selector);
    };

    doSubmit() {
        var slf = this;
        //e.preventDefault();
        var actionUrl = $('#' + this.TagPrefix + '-Form').attr('action');
        var formData = $('#' + this.TagPrefix + '-Form').serialize();

        app_query.doFormSubmit('#' + this.TagPrefix + '-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {

                if (slf.Adapter)
                    slf.Adapter.dataBind();
                slf.doClose();
                //if (slf.Dialog)
                //    app_dialog.dialogClose(slf.Dialog);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };

    doClear() {
        app_form.clearDataForm('#' + this.TagPrefix + '-Form');
    }
}


var app_panel = {
    panelClose: function (tag, doempty) {
        tag = tag.replace("#", "");
        $("#" + tag).hide();
        if (doempty)
            $("#" + tag).empty();
    },
    panelSwitchClose: function (parentName, isSwitch, doempty) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";
        $("#" + tag).hide();
        if (isSwitch)
            $("#" + parentName).show();
        if (doempty)
            $("#" + tag).empty();
    },
    appendPanelAfter: function (parentName, content, title) {//width, height, scroll,
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";

        if (content.startsWith("#")) {
            content = $(content).html();
        }

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        $("#" + tag).hide();

        //$("#" + parentName).hide();

        //app_iframe.showPanel("#" + tag, src, width, height, scroll, title);
        $("#" + tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
            .on("click", function (e) {
                e.preventDefault();
                $("#" + tag).hide();
                $("#" + parentName).show();
                $("#" + tag).empty();
            });
        panel.append(close);
        $("#" + tag).append(panel);
        $("#" + tag).append(content);

        //app_iframe.appendIframe(tag, src, width, height, scroll);

        //$("#" + tag).show();

        return $("#" + tag);
    },
    panelAfterShow: function (tagParent,tagAfter) {

        $(tagParent).hide();
        if (tagAfter === undefined || tagAfter == null) {
            tagAfter = tagParent + "-panel";
        }
        $(tagAfter).show();
    },
    panelAfterClose: function (tagParent, doempty) {
        if (doempty === undefined)
            doempty = true;
        var tagAfter = tagParent + "-panel";
        $(tagAfter).hide();
        $(tagParent).show();
        if (doempty)
            $(tagAfter).empty();
    },
    appendPanelSwitch: function (parentName, content, width, height, scroll, title) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";

        if (content.startsWith("#")) {
            content = $(content).html();
        }

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        $("#" + parentName).hide();

        //app_iframe.showPanel("#" + tag, src, width, height, scroll, title);
        $("#" + tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
            .on("click", function (e) {
                e.preventDefault();
                $("#" + tag).hide();
                $("#" + parentName).show();
                $("#" + tag).empty();
            });
        panel.append(close);
        $("#" + tag).append(panel);
        $("#" + tag).append(content);
        //app_iframe.appendIframe(tag, src, width, height, scroll);

        $("#" + tag).show();
    },
    appendPanel: function (parentName, content, width, height, scroll, title) {
        parentName = parentName.replace("#", "");
        var tag = parentName + "-panel";

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        $("#" + tag).append(content);

        //app_iframe.showPanel("#" + tag, src, width, height, scroll, title);

    },
    appendPanelPop: function (parentName, content, title) {
        parentName = parentName.replace("#", "");
        //$("#" + parentName).empty();

        var tag = parentName + "-panel";

        if ($("#" + tag).length == 0) {
            $("#" + parentName).after('<div id="' + tag + '" class="panel-window"></div>');
        }
        else {
            $("#" + tag).empty();
        }

        $("#" + tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
            .on("click", function (e) {
                e.preventDefault();
                //$("#" + tag).hide();
                $("#" + tag).empty();
            });
        panel.append(close);
        $("#" + tag).append(panel);
        $("#" + tag).append(content);
    },
    showPanel: function (tag, content, width, height, scroll, title) {
        $(tag).empty();
        var panel = $('<div class="panel-header"></div>');
        panel.append('<span style="float:right">' + title + '</span>');
        var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
            .on("click", function (e) {
                e.preventDefault();
                $(tag).hide();
                $(tag).empty();
            });
        panel.append(close);
        $(tag).append(panel);
        $(tag).append(content);

        //app_iframe.appendIframe(tag, src, width, height, scroll);

        $(tag).show();
    },

}
//============================================================================================ app_form

var app_form = {

    loadDataForm: function (form, record, exclude,readonly) {

        $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
            var input = $(this);
            var tag = input.attr('name');
            var datatype = input.attr("data-type");
            var datafield = input.attr("data-field");
            var datatrigger = input.attr("data-trigger");
            
            var tagId = input.attr('id');
            var dataselect2 = input.attr("data-select2-id");

            //var type = input.prop('tagName');

            

            if (datafield !== undefined && datafield !== null)
                tag = datafield;

            if (tag !== undefined) {
                var isexclude = false;
                if (exclude) {
                    isexclude = (exclude.indexOf(tag)) >= 0;
                }

                var value = record[tag];
                if (!isexclude) {

                    if (datatype === undefined) {
                        if (dataselect2 !== undefined && dataselect2 == tagId)
                            datatype = "select2";
                        else if (input.length > 0) {
                            var parent = input[0].parentElement;
                            if (parent) {
                                if ($(parent).hasClass("jqx-editor"))
                                    datatype = "jqx-editor";
                                else if ($(parent).hasClass("jqx-combobox"))
                                    datatype = "jqx-combobox";
                                else if ($(parent).hasClass("jqx-widget"))
                                    datatype = "jqx-widget";
                            }
                        }
                    }

                    if (value !== undefined && value != null) {
                        //typeof date.getMonth === 'function'
                        //if (datatype === undefined && value instanceof Date) {
                        //    if (input.hasClass("jqx-input-content"))
                        //        datatype = "jqx-date";
                        //    else
                        //        datatype = "date";
                        //}

                        switch (datatype) {
                            case "datetime":
                                input.val(app.formatDateTimeString(value)); break;
                            case "date":
                                input.val(app.formatDateString(value)); break;
                            case "bool":
                                input.attr("checked", value); break;
                            case "select-input":
                                $('#' + tag).val(value);
                                $('form#' + form + ' [name=' + tag + ']').val(value);
                                break;
                            case "html-editor":
                                $("#" + tagId).val(app.htmlUnescape(value));
                                break;
                            case "jqx-editor":
                                $("#" + tagId).jqxEditor('val', app.htmlUnescape(value));
                                break;
                            //case "jqx-date":
                            //    $('form#' + form + ' [name=' + tag + ']').val(value);
                            //    if (readonly)
                            //        $("#" + app_form.getJqxId(tagId)).jqxDateTimeInput({ showCalendarButton: false, readonly: true });
                            //    break;
                            case "jqx-combobox":
                                //$('form#' + form + '#' + tag).val(value);
                                $('form#' + form + ' [name=' + tag + ']').val(value);
                                if (readonly)
                                    $('#' + tag).jqxComboBox({ enableSelection: false });
                                break;
                            case "jqx-widget":
                            case "jqx-dropdwon":
                                //$('form#' + form + '#' + tag).val(value);
                                $('form#' + form + ' [name=' + tag + ']').val(value);
                                //if (readonly)
                                //    $('#' + tag).jqxDropDownList({ enableSelection: false });
                                break;
                             case "lookup":
                                app_lookup.setInput(form, tag, value);
                                break;
                            case "select2":
                                $('#' + tagId).select2('val', value);
                                if (readonly)
                                    $('#' + tagId).prop("disabled", true);

                                //$('#' + tag).val(value).trigger("change.select2");
                                break;
                            case "select-loader":
                                var dataargs = input.attr("data-args");
                                app_select_loader.loadTag(tag,tagId, dataargs, value, readonly);
                                break;
                            default:

                                var str = value.toString();
                                if (str.match(/Date/gi)) {
                                    var d = formatJsonShortDate(value)
                                    app_form.setInputValue(input, form, tagId, tag, d, readonly);
                                }
                                //else if (typeof value === 'boolean')
                                //    input.attr("checked", value); 
                                //else if (tagId === tag)
                                //    $('#' + tagId).val(value);
                                else
                                    app_form.setInputValue(input, form, tagId, tag, value, readonly);
                                break;
                        }
                        if (datatrigger)
                            input.trigger(datatrigger);
                    }
                    else {
                        app_form.setInputValue(input, form, tagId, tag, null, readonly); //$('#' + tag).val(null);
                    }
                    
                }
            }
        });
    },
    setInputValue: function (input, form, id, tag, value, readonly) {

        if (input.hasClassStartsWith("jqx-")) {
        //if (id == 'input' + tag) {//for jqx
            if (typeof value === 'boolean')
                $('form#' + form + ' [name=' + tag + ']').attr("checked", value);
            else
                $('form#' + form + ' [name=' + tag + ']').val(value);
        }
        else if (typeof value === 'boolean')
            input.attr("checked", value); 
        else
            input.val(value);

        if (readonly) {
            if (input.is('select'))
                input.prop("disabled", true);
            else
                input.prop("readonly", true);
        }
    },
    getJqxId: function (tagId) {
        return tagId.replace(/^(input)/, "");
    },
    setReadonly(input, readonly) {
        if (readonly)
            input.prop("readonly", true);
    },
    setDateNow: function (tag) {
        $(tag).val(app.toLocalDateString(Date.now()));
    },
    setDateTimeNow: function (tag) {
        $(tag).val(app.toLocalDateTimeString(Date.now()));
    },
    getCheckedBox: function (classname) {
        var selected = $("." + classname + ":checked");
        if (!selected.val()) {
            return null;
        }
        else {
            var box = {
                selectedValue: selected.val(),
                selectedName: selected.siblings().text()
            };
            return box;
        }
    },
    getCheckedValue: function (classname) {
        var selected = $("." + classname + ":checked");
        if (!selected.val())
            return null;
        else
            return selected.val();
    },
    onRadioChange: function (selector,formname) {
        if ($(selector).is(':checked')) {
            var action = $(selector).val();
            $("#" + formname).attr('action', action);
        }
    },
    radioSelectedValue: function (name) {
        var selected = $("input[type='radio'][name='" + name + "']:checked");
        if (selected.length > 0) 
            return selected.val();
        else
            return null;
    },
    doProgress: function (show, input) {
        if (show) {
            if (input !== undefined)
                $(input).attr("disabled", "disabled");

            $("#progressbar").progressbar({ value: false }).show();
        }
        else {
            $("#progressbar").hide();
        }
    },
    clearDataForm: function (form) {
        $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
            var input = $(this);
            var type = input.attr('type');
            if (type == 'check' || type == 'radio')
                input.checked = false;
            else
                input.val('');
        });
    },
    doFormPost: function (formTag, callback, preSubmit, validatorTag) {
        if (validatorTag)
            $(validatorTag).empty();
        var actionurl = $(formTag).attr('action');
        if (preSubmit)
            preSubmit();
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $(formTag).serialize(),
                    success: function (data) {
                        if (callback)
                            callback(data);
                        else
                            app_messenger.Post(data.Message);
                    },
                    error: function (jqXHR, status, error) {
                        app_dialog.alert(error);
                    }
                });
            }
            else {
                if (validatorTag)
                    $(validatorTag).text("חסרים פרטים הכרחיים לעדכון");
            }
        }
        $(formTag).jqxValidator('validate', validationResult);
    },
};

var app_control = {

    select2Ajax1: function (tag, width, option, url, data, selectValue, callback, onChange) {
        if (width === undefined || width === 0)
            width = 200;

        var valueField = "id";
        var labelField = "text";

        if (option === undefined || option == null)
            option = { disable_search_threshold: 0 };
        $.extend(option, { width: width, rtl: true});

        $(tag).empty()
        var $select = $(tag);
        

        $.ajax({
            type: "post",
            dataType: 'json',
            url: url,
            data: data,
            success: function (data) {
                // Parse the returned json data
                //var opts = $.parseJSON(data);
                $.each(data, function (i, d) {
                    if (d[valueField] == selectValue)
                        $select.append($("<option></option>")//.css('padding', '28px')
                            .attr("value", d[valueField]).attr("selected", true)
                            .text(d[labelField]));
                    else
                    $select.append($("<option></option>")//.css('padding', '28px')
                        .attr("value", d[valueField]).attr("disabled", false)
                        .text(d[labelField]));
                });

                if (onChange) {
                    $(tag).on("change", function (e) {
                        onChange(e);
                        //log("change " + JSON.stringify({ val: e.val, added: e.added, removed: e.removed }));
                    });
                }

                $(tag).chosen(option);
                

                if (selectValue) {
                    $select.val(selectValue);
                }

                if (callback)
                    callback(data);

                
            }
        });

        //return $select;
    },
    //select3Tags: function (tag, width, placeholder, maximumSelection, tags, callback, onChange) {
    select2Ajax: function (tag, width, option, url, data, selectValue, readonly, callback, onChange) {
        if (width === undefined || width === 0)
            width = 200;

        if (option === undefined || option == null)
            option = { minimumResultsForSearch: - 1 };

       // $.extend(option, { 'width': width });

        var $select = $(tag).select2();

        //$(tag).attr("readonly", true);

        //var $select=$(tag).select2({
        //    width: width,
        //    placeholder: "בחירת סוג"
        //    //allowClear: true,
        //    //multiple:false,
        //    //maximumSelectionSize:1,
        //    //minimumResultsForSearch: -1,
        //    //dropdownCssClass: 'no-search'
        //    //selectOnClose: true
        //});

        $.ajax({
            type: "post",
            dataType: 'json',
            url: url,
            data: data,
            success: function (data) {
                //[{ id: 0, text: 'story' }, { id: 1, text: 'bug' }, { id: 2, text: 'task' }]
                $.extend(option, { 'data': data, 'width': width });
                $(tag).select2(option);

                //$(tag).select2({ data: data });
                //$(tag).select2("readonly",true);
               

                if (onChange) {
                    $(tag).on("change", function (e) {
                        onChange(e);
                        //log("change " + JSON.stringify({ val: e.val, added: e.added, removed: e.removed }));
                    });
                }

                if (selectValue) {
                    $(tag).select2('val', selectValue);
                    //$(tag).val(selectValue);//.trigger('change');
                   // var v=$(tag).val();
                }
                if (readonly)
                    $(tag).prop("disabled", true);

                if (callback) {
                    callback(data);
                }
            }
        });
        //return $select;
    },
    select2Tags: function (tag, width, placeholder, maximumSelection, tags, callback, onChange) {
        if (width === undefined || width === 0)
            width = 200;

        var $select =$(tag).select2({
            width: width,
            placeholder: placeholder,
            allowClear: true,
            tokenSeparators: [",", " "],
            maximumInputLength: maximumSelection,
            tags: tags//["red", "green", "blue"]
        });

        if (callback) {
            $(tag).on("change", function (e) {
                callback(e);
                //log("change " + JSON.stringify({ val: e.val, added: e.added, removed: e.removed }));
            });
        }
        return $select;
     },
    select2Data: function (tag, width, placeholder, data, selectValue, onChange) {
        if (width === undefined || width === 0)
            width = 200;

        var $select =$(tag).select2({
            width: width,
            placeholder: placeholder,
            allowClear: true,
            data: data//[{ id: 0, text: 'story' }, { id: 1, text: 'bug' }, { id: 2, text: 'task' }]
        });

        if (onChange) {
            $(tag).on("change", function (e) {
                onChange(e);
                //log("change " + JSON.stringify({ val: e.val, added: e.added, removed: e.removed }));
            });
        }
        if (selectValue) {
            $select.val(selectValue);
        }
        return $select;
    },
    selectTag: function (tag, width) {
        if (width === undefined)
            width = 200;

        //$(tag).selectmenu({
        //    width: width,
        //    open: function (event, ui) {
        //        $(this).selectmenu("menuWidget").hide().slideDown("fast");
        //    },
        //    close: function (event, ui) {
        //        var $menuWidget = $(this).selectmenu("menuWidget");
        //        $menuWidget.parent().show();
        //        $menuWidget.slideUp("fast");
        //    }
        //});
        $(tag).css('width', width);
    },
    fillSelect: function (tag, url, data, valueField, labelField, selectValue,callback) {

        $(tag).empty()
        var $dropDown = $(tag);
        $.ajax({
            type: "post",
            dataType: 'json',
            url: url,
            data: data,
            success: function (data) {
                // Parse the returned json data
                //var opts = $.parseJSON(data);
                $.each(data, function (i, d) {
                    $dropDown.append($("<option></option>").css('padding', '28px')
                        .attr("value", d[valueField])
                        .text(d[labelField]));
                });
                if (selectValue) {
                    $dropDown.val(selectValue);
                }

                if (callback)
                    callback(data);
            }
        });
    },
    appendSelectRecords: function (tag, records, valueField, labelField) {

        // var selectValues = { "0": "ללא", "1": "צוות", "2": "פרטי", "3": "לפי בחירה" };
        $(tag).empty();
        var $dropDown = $(tag);
        $.each(selectValues, function (i, record) {
            $dropDown.append($("<option></option>")
                .attr("value", record[valueField])
                .text(record[labelField]));
        });
    },
    appendSelectOptions: function (tag, optionValues) {

       // var selectValues = { "0": "ללא", "1": "צוות", "2": "פרטי", "3": "לפי בחירה" };
        $(tag).empty();
        var $dropDown = $(tag);
        $.each(optionValues, function (key, value) {
            $dropDown.append($("<option></option>")
                .attr("value", key)
                .text(value));
        });
    },
    datepicker: function (selector, yearRange,formValidator) {

        $(selector).datepicker({
            regional: ["he"],
            isRTL: true,
            //yearRange: "1925:1999",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'dd/mm/yy'
        });
        //getter
        //var yearRange = $(".selector").datepicker("option", "yearRange");
        // Setter
        if (yearRange) {
            $(selector).datepicker("option", "yearRange", yearRange);
        }
        if (formValidator !==undefined) {
            $(selector).datepicker({
                onSelect: function (dateText, inst) {
                    $(formValidator).jqxValidator('validateInput', '' + selector + '');
                }
            });
        }
    },

    datepickerBirthday: function (selector, yearRange, formValidator) {
        $(selector).datepicker({
            regional: ["he"],
            isRTL: true,
            yearRange: "1925:1999",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'dd/mm/yy',
            onSelect: function (dateText, inst) {
                $(formValidator).jqxValidator('validateInput', '' + selector + '');
            }
        });
    }
}



//============================================================================================ app_popup

//var app_popup = {

//    memberEdit: function (id) {
//        //return popupIframe(app.appPath() + "/Common/_MemberEdit?id=" + id, "500", "600");

//        return app_dialog.dialogIframe(app.appPath() + "/Common/_MemberEdit?id=" + id, "500", "600","מנוי "+ id);
//    },
//    managementEdit: function (id) {
//        //return popupIframe(app.appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600");
//        return app_dialog.dialogIframe(app.appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600", "מנוי " + id);
//    },
//    cmsHtmlEditor: function (extid) {
//        if (app_validation.notForMobile())
//            return;
//        //return popupIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600");

//        return app_dialog.dialogIframe("/Cms/CmsHtmlEdit?extid=" + extid, "850", "600", "Cms Html Editor")
//    },
//    cmsTextEditor: function (extid) {
//        if (app_validation.notForMobile())
//            return;
//        //return popupIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600");

//        return app_dialog.dialogIframe("/Cms/CmsTextEdit?extid=" + extid, "850", "600", "Cms Text Editor")
//    },
//    cmsPageSettings: function (pageType) {
//        if (app_validation.notForMobile())
//            return;
//        return app_dialog.dialogIframe("/Cms/CmsPageSettings?pageType=" + pageType, "850", "600", "Cms Text Editor")
//    },
//    cmsPreview: function (folder, pageType) {
//        if (app_validation.notForMobile())
//            return;
//        var path = "/Preview/" + pageType + "/" + folder;
//        return app_dialog.dialogIframe(path, "850", "600", "Cms Preview",true)
//    },
//    batchMessageView: function (id) {

//        return app_dialog.dialogIframe(app.appPath() + "/Common/_BatchMessageView?id=" + id, "400", "400", "נוסח הודעה " + id);
//    },
//    gridView: function (src,title) {
//        return app_dialog.dialogIframe(app.appPath() + src, "850", "600", title);
//    }
//}

//============================================================================================ app_const

//var app_const = {

//    adminLink: '<a href="/Admin/Manager">מנהל מערכת</a>',
//    accountsLink: '<a href="/Admin/DefAccount">ניהול לקוחות</a>'
//};

//============================================================================================ app_menu

//var app_menu = {

//     activeLayoutMenu: function (li) {
//        //$("#cssmenu>ul>li.active").removeClass("active");
//        //$("#cssmenu>ul>li#" + li).addClass("active");

//         $("#mainnav>ul>li.active").removeClass("active");
//         $("#mainnav>ul>li#" + li).addClass("active");

//    },

//    printObject: function (obj) {
//        //debugObjectKeys(obj);
//        var o = obj;
//    },

//    breadcrumbs: function (section, page, lang) {

//        var breadcrumbs = $(".breadcrumbs");
//        breadcrumbs.text('');
//        var b = $('<div style="text-align:left;direction:ltr;"></div>')

//        if (lang === undefined || lang == 'en') {
//            b.append($('<a href="/home/index">Home</a>'));
//            b.append($('<span> >> </span>'));
//            b.append($('<a href="/home/main">Main</a>'));
//            b.append($('<span> >> </span>'));

//            b.append('' + section + ' >> ' + page + ' |  ');
//            b.append('<a href="javascript:parent.history.back()">Back</a>');

//            //var path = document.referrer;
//            //var page = app.getUrlPage(path);
//            //b.append($('<a href="' + path + '">' + page.split('?')[0] + '</a>'));
//            //b.append($('<span> >> </span>'));
//            //var curPage = app.getUrlPage(location.href);
//            //b.append($('<span> ' + curPage.split('?')[0] + ' </span>'));
//        }
//        else {
//            b.append($('<a href="/home/index">דף הבית</a>'));
//            b.append($('<span> >> </span>'));
//            b.append($('<a href="/home/main">ראשי</a>'));
//            b.append($('<span> >> </span>'));
//            b.append('' + section + ' >> ' + page + ' |  ');
//            b.append('<a href="javascript:parent.history.back()">חזרה</a>');

//        }
//        b.appendTo(breadcrumbs);
//    }
//};

//============================================================================================ app_rout

//    var app_rout = {

//    isAllowEdit: function (allowEdit) {
//        if (allowEdit == 0) {
//            app_dialog.alert('You have no permission for this action.');
//        }
//    },

//    redirectToMembers: function () {
//        app.redirectTo("/Main/Members");
//    },
//};

//============================================================================================ app_members

//var app_members = {

//    displayMemberFields: function () {

//        $.ajax({
//            url: '/Common/GetMemberFieldsView',
//            type: 'post',
//            dataType: 'json',
//            //data: { },
//            success: function (data) {
//                if (data) {
//                    $("#ExType").val(data.ExType);

//                    if (data.ExEnum1 == "")
//                        $("#divEnum1").hide();
//                    else
//                        $("#lblEnum1").text(data.ExEnum1);

//                    if (data.ExEnum2 == "")
//                        $("#divEnum2").hide();
//                    else
//                        $("#lblEnum2").text(data.ExEnum2);

//                    if (data.ExEnum3 == "")
//                        $("#divEnum3").hide();
//                    else
//                        $("#lblEnum3").text(data.ExEnum3);

//                    if (data.ExField1 == "")
//                        $("#divField1").hide();
//                    else
//                        $("#lblExField1").text(data.ExField1);

//                    if (data.ExField2 == "")
//                        $("#divField2").hide();
//                    else
//                        $("#lblExField2").text(data.ExField2);

//                    if (data.ExField3 == "")
//                        $("#divField3").hide();
//                    else
//                        $("#lblExField3").text(data.ExField3);

//                    if (data.ExId == "")
//                        $("#divExId").hide();
//                    else
//                        $("#lblExId").text(data.ExId);

//                }
//            },
//            error: function (jqXHR, status, error) {
//                app_dialog.alert(error);
//            }
//        });
//    }
//};


var app_debug = {

  
}