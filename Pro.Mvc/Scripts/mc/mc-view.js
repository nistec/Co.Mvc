//mc-view version 20121026.1


/////////////////////// tag-form ////////////////////////

$(function () {
    $("form#tag-form-auto").submit(function (e) {
        if (!tagSubmitForm()) {
            return false;
        }
        e.preventDefault();

        //get the action-url of the form
        //var actionurl = e.currentTarget.action;
        var actionurl = window.location.protocol+"//"+window.location.host+'//view/TagForm';

        //var page = window.location.pathname.split("/").pop();
        //var relative = location.href.replace(page, 'TagForm').replace(location.search, '');

        $.ajax({
            url: actionurl,
            type: 'post',
            data: $("form#tag-form-auto").serialize(),
            success: function () {
                $("#popupAuto").show("slow");
                setTimeout(function () {
                    // then reload after in 2.5 second
                    window.location.reload();
                }, 2500);
            },
            error: function (request, status, error) {
                alert('אירעה שגיאה: ' + status);
            }
        });
    });
});

function tagSubmitForm(op) {

    var tagValidate = function(field) {
        var el = document.getElementById(field);
        var ok = 0;
        var msg = 'ok';

        if (el) {

            if (field == "tagid") {
                if (el.value == null || el.value.length == 0) {
                    msg = "חסרים נתונים לעדכון אנא פנה לתמיכה";
                    alert(msg);
                    return msg;
                }
            }
            var vld = el.getAttribute('data-validator');
            var req = el.getAttribute('data-required');
            var reg = el.getAttribute('data-regex');

            var lbl = document.getElementById(field + '-vld');
            if (lbl) {
                lbl.style.display = "none";
                lbl.title = msg;
            }

            //$('#' + field + '-vld').attr('display', 'none').attr('title', msg);

            var isreq = (req && req == "1");

            if (reg && reg.length > 0) {

                if (!reg.test(el.value)) {

                    var rvld = el.getAttribute('data-regex_vld');

                    if (rvld) {
                        msg = vld;
                    }
                    else {
                        msg = "נדרש ערך תקין";
                    }
                    ok = -1;
                }
            }
            else if (field.substring(0, 4) == "cli_" || field == "txtcli" || field == "txtphone") {
                if (isreq || isNotEmpty(el)) {
                    if (!isMobile(el.value)) {
                        msg = (vld) ? vld : "נדרש טלפון תקני";
                        ok = -1;
                    }
                }
            }
            else if (field.substring(0, 4) == "eml_" || field == "txtmail") {
                if (isreq || isNotEmpty(el)) {
                    if (!isEmail(el.value)) {
                        msg = (vld) ? vld : "נדרש דואל תקני";
                        ok = -1;
                    }
                }
            }
            else if (field.substring(0, 4) == "dat_") {
                if (isreq || isNotEmpty(el)) {
                    if (!isDate(el.value, true)) {
                        msg = (vld) ? vld : "נדרש תאריך תקני";
                        ok = -1;
                    }
                }
            }
            else if (isreq && (el.value == null || el.value.length == 0 || el.value == vld || el.value == "נדרש ערך תקין")) {
                if (vld) {
                    msg = vld;
                }
                else {
                    msg = "נדרש ערך תקין";
                }
                ok = -1;
            }

            if (ok < 0) {
                if (lbl) {
                    lbl.style.display = "inline";
                    lbl.title = msg;
                }
                //$('#' + field + '-vld').attr('display', 'inline').attr('title', msg);
                //el.value = msg;
            }
        }
        return msg;
    };

    var ivalid = 0;
    var answers = [];

    if (op && op == 1) {
        var msg;
        $('#WResult').empty();

        $.each($('.WField'), function () {
            msg = tagValidate($(this).attr('id'));
            if (msg != 'ok') {
                answers.push(msg);
            }
        });
        ivalid = answers.length > 0 ? -1 : 0;
        if (answers.length > 0) {
            var result = arrayToBulletList(answers);
            $('#WResult').html(result);
        }
        else {
            answers = "none";
        }
    }
    else {
        $.each($('.WField'), function () {
            msg = tagValidate($(this).attr('id'));
            if (msg != 'ok') {
                ivalid += -1;
                var el = document.getElementById($(this).attr('id'));
                el.value = msg;
                answers.push(msg);
            }
            //ivalid += (msg == 'ok') ? 0 : -1;
            //answers.push($(this).attr('id') + ';' + $(this).val().replace(/,/g, ' '));
        });

        if (answers.length > 0) {
            var msg = arrayToAlertMessage(answers);
            alert(msg);
        }
        else {
            answers = "none";
        }
    }

    if (ivalid < 0) {
        return false;
    }

    return true;
}
/*
function validateTagFields(field) {
    var el = document.getElementById(field);
    var ok = 0;
    var msg = 'ok';

    if (el) {

        if (field == "tagid") {
            if (el.value == null || el.value.length == 0) {
                msg = "חסרים נתונים לעדכון אנא פנה לתמיכה";
                alert(msg);
                return msg;
            }
        }
        var vld = el.getAttribute('data-validator');
        var req = el.getAttribute('data-required');
        var reg = el.getAttribute('data-regex');

        var lbl = document.getElementById(field + '-vld');
        if (lbl) {
            lbl.style.display = "none";
            lbl.title = msg;
        }

        //$('#' + field + '-vld').attr('display', 'none').attr('title', msg);

        var isreq = (req && req == "1");

        if (reg && reg.length > 0) {

            if (!reg.test(el.value)) {

                var rvld = el.getAttribute('data-regex_vld');

                if (rvld) {
                    msg = vld;
                }
                else {
                    msg = "נדרש ערך תקין";
                }
                ok = -1;
            }
        }
        else if (field.substring(0, 4) == "cli_" || field == "txtcli" || field == "txtphone") {
            if (isreq || isNotEmpty(el)) {
                if (!isMobile(el.value)) {
                    msg = (vld) ? vld : "נדרש טלפון תקני";
                    ok = -1;
                }
            }
        }
        else if (field.substring(0, 4) == "eml_" || field == "txtmail") {
            if (isreq || isNotEmpty(el)) {
                if (!isEmail(el.value)) {
                    msg = (vld) ? vld : "נדרש דואל תקני";
                    ok = -1;
                }
            }
        }
        else if (field.substring(0, 4) == "dat_") {
            if (isreq || isNotEmpty(el)) {
                if (!isDate(el.value,true)) {
                    msg = (vld) ? vld : "נדרש תאריך תקני";
                    ok = -1;
                }
            }
        }
        else if (isreq && (el.value == null || el.value.length == 0 || el.value == vld || el.value == "נדרש ערך תקין")) {
            if (vld) {
                msg = vld;
            }
            else {
                msg = "נדרש ערך תקין";
            }
            ok = -1;
        }

        if (ok < 0) {
            if (lbl) {
                lbl.style.display = "inline";
                lbl.title = msg;
            }
            //$('#' + field + '-vld').attr('display', 'inline').attr('title', msg);
            //el.value = msg;
        }
    }
    return msg;
}
*/
/////////////////////// form ////////////////////////

function onSubmitForm(op) {

    var ivalid = 0;
    var answers = [];

    if (op && op == 1) {
        var msg;
        $('#WResult').empty();

        $.each($('.WField'), function () {
            msg = validateFields($(this).attr('id'));
            if (msg != 'ok') {
                answers.push(msg);
            }
        });
        ivalid = answers.length > 0 ? -1 : 0;
        if (answers.length > 0) {
            var result = arrayToBulletList(answers);
            $('#WResult').html(result);
        }
        else {
            answers = "none";
        }
    }
    else {
        $.each($('.WField'), function () {
            msg = validateFields($(this).attr('id'));
            if (msg != 'ok') {
                ivalid += -1;
                var el = document.getElementById($(this).attr('id'));
                el.value = msg;
                answers.push(msg);
            }
            //ivalid += (msg == 'ok') ? 0 : -1;
            //answers.push($(this).attr('id') + ';' + $(this).val().replace(/,/g, ' '));
        });

        if (answers.length > 0) {
            var msg = arrayToAlertMessage(answers);
            alert(msg);
        }
        else {
            answers = "none";
        }
    }

    if (ivalid < 0) {
        return false;
    }

    return true;
}

function arrayToAlertMessage(arr) {
    var msg = '';
    $.each(arr, function (i) {
        msg += arr[i] + '\n';
    });
    return msg;
}

function arrayToBulletList(arr) {
    var cList = '<ul>';
    $.each(arr, function (i) {
        var li = '<li style="color:red">' + arr[i] + '</li>';
        cList += li;
    });
    cList += '</ul>';
    return cList;
}

function arrayToBulletLinkList(arr) {
    var cList = $('ul.mylist')
    $.each(arr, function (i) {
        var li = $('<li/>').addClass('ui-menu-item').attr('role', 'menuitem').appendTo(cList);
        var aaa = $('<a/>').addClass('ui-all').text(arr[i]).appendTo(li);
    });

    return cList;
}

function validateFields(field) {
    var el = document.getElementById(field);
    var ok = 0;
    var msg = 'ok';

    if (el) {

        if (field == "accountid" || field == "registryid") {
            if (el.value == null || el.value.length == 0) {
                msg = "חסרים נתונים לעדכון אנא פנה לתמיכה";
                alert(msg);
                return msg;
            }
        }
        var vld = el.getAttribute('data-validator');
        var req = el.getAttribute('data-required');
        var reg = el.getAttribute('data-regex');

        var lbl = document.getElementById(field + '-vld');
        if (lbl) {
            lbl.style.display = "none";
            lbl.title = msg;
        }

        //$('#' + field + '-vld').attr('display', 'none').attr('title', msg);

        var isreq = (req && req == "1");

        if (reg && reg.length > 0) {

            if (!reg.test(el.value)) {

                var rvld = el.getAttribute('data-regex_vld');

                if (rvld) {
                    msg = vld;
                }
                else {
                    msg = "נדרש ערך תקין";
                }
                ok = -1;
            }
        }
        else if (field == "txtcli" || field == "txtphone") {
            if (isreq || isNotEmpty(el)) {
                if (!isMobile(el.value)) {
                    msg = (vld) ? vld : "נדרש טלפון תקני";
                    ok = -1;
                }
            }
        }
        else if (field == "txtmail") {
            if (isreq || isNotEmpty(el)) {
                if (!isEmail(el.value)) {
                    msg = (vld) ? vld : "נדרש דואל תקני";
                    ok = -1;
                }
            }
        }
        else if (isreq && (el.value == null || el.value.length == 0 || el.value == vld || el.value == "נדרש ערך תקין")) {
            if (vld) {
                msg = vld;
            }
            else {
                msg = "נדרש ערך תקין";
            }
            ok = -1;
        }

        if (ok < 0) {
            if (lbl) {
                lbl.style.display = "inline";
                lbl.title = msg;
            }
            //$('#' + field + '-vld').attr('display', 'inline').attr('title', msg);
            //el.value = msg;
        }
    }
    return msg;
}

/////////////////////  tag functions   ////////////////////////////////

function tagRedirect(key, target) {
    if (key) {
        var src = getQueryVariable(key);
        if (src) {
            var location = window.location.protocol +
                '//' + window.location.host +'/tagLink.aspx?src=' + src;
            if (target && target == 'href')
                window.location.href = location;
            window.location.replace(location);
        }
    }
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
    console.log('Query variable %s not found', variable);
    return undefined;
}

/////////////////////  extend   ////////////////////////////////

$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});

function img_track(src, alt, title) {
    //var img = IsIE ? new Image() : document.createElement('img');
    var img = document.createElement('img');
    img.src = src;
    img.setAttribute('width', '0');
    img.setAttribute('height', '0');
    if (alt != null) img.alt = alt;
    if (title != null) img.title = title;

    document.body.appendChild(img);
    document.body.removeChild(img);
}
