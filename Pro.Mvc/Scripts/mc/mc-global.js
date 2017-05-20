﻿/*
 * Mc Global js framework
 * version: 1.4.2 (03/01/2008)
 * @requires jQuery v1.2 or later
 */

/////////////////////// html ////////////////////////

function toggleStyle(el) {
    if (el.className == "toogle_on") {
        el.className = "toogle_off";
    } else {
        el.className = "toogle_on";
    }
}

function getSelectedValueById(ddl, startIndex) {
    var el = document.getElementById(ddl);
    if (el)
        return getSelectedValue(el, startIndex);
    return "";
}

function getSelectedValue(ddl, startIndex) {
    if (!startIndex)
        startIndex = 0;
    if (ddl.selectedIndex < startIndex)
        return "";
    return ddl.options[ddl.selectedIndex].value;
};


function toggleCollapse(el) {
    var div = document.getElementById(el);
    var state = div.style.display;
    if (state == 'none') {
        div.style.display = "block";
    }
    else {
        div.style.display = "none";

    }
}


function htmlDecode(val) {

    return val.replace(/&gt;/g, ">").replace(/&lt;/g, "<").replace(/&quot;/g, "\"").replace(/&apos;/g, "'").replace(/&amp;/g, "&");
}

function htmlEncode(val) {
    if (!val) return "";
    return val.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&apos;");
}

function textToHtml(val) {
    return val.replace(/\r\n/g, "<BR>");
}

function htmlToText(val) {
    if (!val) return "";
    return val.replace(/&lt;BR&gt;/g, "\r\n").replace(/<BR>/g, "\r\n");
}
    
   function confirmSubmit(text)
    {
        var agree = confirm(text);

        if (agree)
            return true;
        else
            return false;
    }


function AddChars(count,chars){
	count.value = parseInt(count.value) + chars;
}

function uncheck_me(obj_id){

 var obj = document.getElementById(obj_id);
 obj.checked = false;
}


/////////////////////// validation ////////////////////////

function IsNumericKey(evt) {

 evt = (evt) ? evt : ((event) ? event : null);
 if (evt) {
     var key = evt.charCode//firefox
     if (!key) {
         key = evt.keyCode;
     }
     if (key < 48 || key > 57) {
         return false;
     }
 }
return true;
}

function IsDelKey(evt) {

    evt = (evt) ? evt : ((event) ? event : null);
    if (evt) {
        var key = evt.charCode//firefox
        if (!key) {
            key = evt.keyCode;
        }
        return key == 46;
    }
    return false;
}

// validates that the field value string has one or more characters in it
function isNotEmpty(elem) {
    var str = elem.value;
    var re = /.+/;
    if (!str.match(re)) {
        return false;
    } else {
        return true;
    }
}

//validates that the entry is a positive or negative number
function isNumeric(str, posetive, intgr) {
    if (isEmpty(str))
        return false;

    var re = /^[-]?\d*\.?\d*$/;
    if (posetive && intgr)
        re = /^\d*$/;
    if (intgr)
        re = /^[-]?\d*$/;
    if (posetive)
        re = /^\d*\.?\d*$/;
      
    str = str.toString();
    if (!str.match(re)) {
        return false;
    }
    return true;
}


//validates that the entry is a positive or negative number
function isNumberElement(elem) {
    if (isEmptyElement(elem))
        return false;
    return isNumeric(elem.value);
}
function isPercent(str) {
    if (!isNumeric(str))
        return false;
    return parseFloat(str) < 1.0;
}
function isPercentElement(elem) {
    if (isEmptyElement(elem))
        return false;
    return isPercent(elem.value);
}

function isMobile(str) {
    if (isEmpty(str))  
        return false;
    var re = /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/
    return re.test(str);
}

function isPhone(str,checkFree) {
    if (isEmpty(str))
        return false;
    var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
    var ok = re.test(str);
    if (!ok && checkFree)
        return isPhoneFree(str);
    else
        return ok;
}

function isPhoneFree(str) {
    if (isEmpty(str))
        return false;
    var re = /^(|\()(18|17)00(|[\)\/\.-])[0-9]{3}(|[\)\/\.-])[0-9]{3}$/
    return re.test(str);
}

// validates that the entry is formatted as an e-mail address
function isMobileElement(elem, showAlert) {
    var ok =elem? isMobile(elem.value):false;
    if (!ok && showAlert)
        alert("מספר טלפון נייד אינו תקין");
    return ok;
}

// returns true if the string is a valid email
function isEmail(str) {
    if (isEmpty(str)) return false;
    var re = /^[^\s()<>@,;:\/]+@\w[\w\.-]+\.[a-z]{2,}$/i
    return re.test(str);
}

// validates that the entry is formatted as an e-mail address
function isEmailElement(elem, showAlert) {
    var ok = elem ? isEmail(elem.value) : false;
    if (!ok && showAlert)
        alert("כתובת דואר אלקטרוני אינה תקינה");
    return ok;
}

// returns true if the string is a valid email
function isUrl(str) {
    if (isEmpty(str)) return false;
    var re = /(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
    return re.test(str);
}

// returns true if the string is empty
function isEmpty(str){
    return (str == null) || (str.length == 0);
}

function isEmptyLessThen(str,len){
  return (str == null) || (str.length < len);
}

function isEmptyElement(elem) {

    if (!elem) return false;
    return isEmpty(elem.value);
}

// returns true if the string only contains characters A-Z or a-z
function isAlpha(str){
  var re = /[^a-zA-Z]/g
  if (re.test(str)) return false;
  return true;
}
// returns true if the string only contains characters 0-9
function isDigits(str){
  var re = /[\D]/g
  if (re.test(str)) return false;
  return true;
}
// returns true if the string only contains characters A-Z, a-z or 0-9
function isAlphaNumeric(str){
  var re = /[^a-zA-Z0-9]/g
  if (re.test(str)) return false;
  return true;
}
// returns true if the string's length equals "len"
function isLength(str, len){
  return str.length == len;
}
// returns true if the string's length is between "min" and "max"
function isLengthBetween(str, min, max){
  return (str.length >= min)&&(str.length <= max);
}

// returns true if the string is a valid date formatted as...
// mm dd yyyy, mm/dd/yyyy, mm.dd.yyyy, mm-dd-yyyy
//^(\d{1,2})[\s\.\/-](\d{1,2})[\s\.\/-](\d{4})[\s](\d{1,2})[\:](\d{1,2})+$
//^(\d{4})[\-](\d{1,2})[\-](\d{1,2})[\sT](\d{1,2})[\:](\d{1,2})+$
//^((\d{1,2})[\s\.\/-](\d{1,2})[\s\.\/-](\d{4})[\s]|(\d{4})[\-](\d{1,2})[\-](\d{1,2})[\sT])(\d{1,2})[\:](\d{1,2})$
function isDate(str,isfull) {
    var re = /^(\d{1,2})[\s\.\/-](\d{1,2})[\s\.\/-](\d{4})$/
    if (isfull)
        re = /^(\d{2})[\s\.\/-](\d{2})[\s\.\/-](\d{4})$/

    if (!re.test(str)) return false;
    var result = str.match(re);

    var sm = result[2];
    var sd = result[1];
    var y = parseInt(result[3]);


    if (sm.length > 1 && sm.charAt(0) == "0")
        sm = sm.charAt(1);
    if (sd.length > 1 && sd.charAt(0) == "0")
        sd = sd.charAt(1);

    /*
    var m = parseInt(result[2]);
    var d = parseInt(result[1]);
    var y = parseInt(result[3]);
    */
    m = parseInt(sm);
    d = parseInt(sd);


    if (m < 1 || m > 12 || y < 1900 || y > 2100) return false;
    var days = 30;
    if (m == 2) {
        days = ((y % 4) == 0) ? 29 : 28;
    } else if (m == 4 || m == 6 || m == 9 || m == 11) {
        days = 30;
    } else {
        days = 31;
    }
    return (d >= 1 && d <= days);
}


function isTime(str) {
    var re = /^(\d{1,2})[\s\:](\d{1,2})$/
    if (!re.test(str)) return false;
    var result = str.match(re);

    var sm = result[2];
    var sh = result[1];

    if (sm.length > 1 && sm.charAt(0) == "0")
        sm = sm.charAt(1);
    if (sh.length > 1 && sd.charAt(0) == "0")
        sh = sh.charAt(1);

    m = parseInt(sm);
    h = parseInt(sh);


    if (m < 1 || m > 59) return false;
    if (h < 1 || h > 23) return false;

    return true;
}

// returns true if "str1" is the same as the "str2"
function isMatch(str1, str2){
  return str1 == str2;
}

// returns true if the string contains only whitespace
// cannot check a password type input for whitespace
function isWhitespace(str){ // NOT USED IN FORM VALIDATION
  var re = /[\S]/g
  if (re.test(str)) return false;
  return true;
}

// removes any whitespace from the string and returns the result
// the value of "replacement" will be used to replace the whitespace (optional)
function stripWhitespace(str, replacement){// NOT USED IN FORM VALIDATION
  if (replacement == null) replacement = '';
  var result = str;
  var re = /\s/g
  if(str.search(re) != -1){
    result = str.replace(re, replacement);
  }
  return result;
}

/////////////// Ajax //////////////////////////////////

function doPostBackAsync(eventName, eventArgs) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    if (!Array.contains(prm._asyncPostBackControlIDs, eventName)) {
        prm._asyncPostBackControlIDs.push(eventName);
    }

    if (!Array.contains(prm._asyncPostBackControlClientIDs, eventName)) {
        prm._asyncPostBackControlClientIDs.push(eventName);
    }

    __doPostBack(eventName, eventArgs);
}

function LoadAjaxHtml(url, target) {
    //document.getElementById(target).innerHTML = ' Fetching data...';
    if (window.XMLHttpRequest) {
        req = new XMLHttpRequest();
    } else if (window.ActiveXObject) {
        req = new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (req != undefined) {
        req.onreadystatechange = function() { LoadAjaxHtmlDone(url, target); };
        req.open("GET", url, true);
        req.send("");
    }
}

function LoadAjaxHtmlDone(url, target) {
    if (req.readyState == 4) { // only if req is "loaded"
        if (req.status == 200) { // only if "OK"
            var html = req.responseText;
            document.getElementById(target).innerHTML = html;
        } else {
            document.getElementById(target).innerHTML = " Mc Error:\n" + req.status + "\n" + req.statusText;
        }
    }
}

var xmlHttp = null;

function ajaxRequest(url, param, target) {
    if (window.XMLHttpRequest) {
        xmlHttp = new XMLHttpRequest();
    } else if (window.ActiveXObject) {
        xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (xmlHttp != undefined) {
        xmlHttp.onreadystatechange = function () { ProcessRequest(url, target); };
        xmlHttp.open("GET", url + '?' + param, true);
        xmlHttp.send("");
    }
}

function ProcessRequest(url, target) {
    if (!target) return;
    if (xmlHttp.readyState == 4) { // only if req is "loaded"
        if (xmlHttp.status == 200) { // only if "OK"
            var html = xmlHttp.responseText;
            document.getElementById(target).innerHTML = html;
        } else {
            document.getElementById(target).innerHTML = " Mc Error:\n" + req.status + "\n" + req.statusText;
        }
    }
}

function getRequestParam(name, valueIfnull) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return valueIfnull;
    else
        return results[1];
}
