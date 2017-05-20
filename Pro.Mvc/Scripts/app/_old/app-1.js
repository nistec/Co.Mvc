
var config = {
    base: ''//'/party'
};

var appPath = function () {
    return window.location.protocol + "//" + window.location.host + config.base ;
};

var actionPath = function (action, conroller) {
    var link = window.location.protocol + "//" + window.location.host + config.base + "/" +  conroller + "/" + action;
    return link;
};

function ObjectKeys(data) {
    var keys = Object.keys(data);
}

function memberEdit(id) {
    return popupIframe(appPath() + "/Common/_MemberEdit?id=" + id, "500", "600");
};
function managementEdit(id) {
    return popupIframe(appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600");
};

function accountNewsAdd(id) {
    return popupIframe(appPath() + "/Common/_AccountNewsEdit?id=" + id, "400", "550");
};

function accountNewsRemove(newsid, accid) {
    if (confirm("האם להסיר לקוח " + accid + " מקבוצת דיוור " + newsid)) {
        $.ajax({
            type: "POST",
            url: appPath() + "/Common/AccountNewsDelete",///Ws/CrmWs.asmx/AccountNewsDelete",
            data: "{ 'AccountId': '" + accid + "','NewsId': '" + newsid + "' }",
            contentType: "application/json; charset=utf-8",
            //dataType: "json",
            success: function (data) {
                dialogMessage('קבוצות דיוור', 'לקוח ' + accid + ' הוסר מקבוצת דיוור ' + newsid, true);
            },
            error: function (e) {
                alert(e);
            }
        });
    }
};

function setLinkHref(input, taglink, isEmail) {

    var val = $('#' + input).val();
    if (isEmail)
        val = 'mailto:' + val;
    if (val === undefined || val.length < 10) {
        $('a#' + taglink).attr('href', '');
        $('a#' + taglink).hide();
    }
    else {
        $('a#' + taglink).attr('href', val);
        $('a#' + taglink).show();
    }
};


function setInputValue(tag, value, asEmpty) {
    if (value != asEmpty)
        $("#" + tag).val(value);
}

function loadDataForm(form, record) {
    var arr = '';
    var types = '';
    $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
        var input = $(this);

        var tag = input.attr('name');
        if (tag !== undefined) {

            var value = record[tag];
            if (value !== undefined && value != null) {
                var str = value.toString();
                if (str.match(/Date/gi)) {
                    var d = formatJsonShortDate(value)
                    $("#" + tag).val(d);
                }
                else if (typeof value === 'boolean')
                    $("#" + tag).attr("checked", value);
                else
                    $("#" + tag).val(value);
            }
            else {
                $("#" + tag).val(null);
            }
        }
    });
}

function getSelectedComboValue(tag, defaultValue) {
    var item = $("#" + tag).jqxComboBox('getSelectedItem');
    if (!item)
        return defaultValue;
    return item.value;
};

var selectComboBoxValue = function (tag, value) {
    var item = $("#" + tag).jqxComboBox('getItemByValue', value);
    if (item)
        $("#" + tag).jqxComboBox('selectIndex', item.index);
};

function getSelectedDropDownValue(tag, defaultValue) {
    var item = $("#" + tag).jqxDropDownList('getSelectedItem');
    if (!item)
        return defaultValue;
    return item.value;
};

var selectDropDownValue = function (tag, value) {
    var item = $("#" + tag).jqxDropDownList('getItemByValue', value);
    if (item)
        $("#" + tag).jqxDropDownList('selectIndex', item.index);
};

var selectCheckListIndex = function (tag, value) {
    var item = $("#" + tag).jqxListBox('getItemByValue', value);
    if (item) {
        $("#" + tag).jqxListBox('checkIndex', item.index);
    }
};

var selectCheckList = function (tag, value) {
    if (value) {
        var items = value.toString().split(",");
        for (index = 0; index < items.length; ++index) {
            selectCheckListIndex(tag, items[index]);
        }
    }
};

var renderCheckList = function (tag, dest) {
    var val = $("#" + tag).val();
    $("#" + dest).val(val);
};


var getDataCheckedList = function (records, field) {
    var length = records.length;
    var newsType = '';
    for (var i = 0; i < length; i++) {
        var record = records[i];
        var val = record[field];
        newsType += val + ',';
    }
    if (newsType.length > 0)
        newsType = newsType.substring(0, newsType.length - 1);
    return newsType;
}

var createComboAdapter = function (valueMember, displayMember, tag, url, width, dropDownHeight, async) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url, async);
    var autoDropDownHeight = true;
    if (typeof width === 'undefined' || width == 0) { width = 240; }
    if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
        dropDownHeight = 200;
    else
        autoDropDownHeight= false;

    $("#" + tag).jqxComboBox(
    {
        rtl: true,
        source: srcAdapter,
        width: width,
        dropDownHeight: dropDownHeight,
        autoDropDownHeight: autoDropDownHeight,
        displayMember: displayMember,
        valueMember: valueMember
    });
    return srcAdapter;
};

var createDropDownAdapter = function (valueMember, displayMember, tag, url, width, dropDownHeight, async) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url, async);
    var autoDropDownHeight = true;
    if (typeof width === 'undefined' || width == 0) { width = 240; }
    if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
        dropDownHeight = 200;
    else
        autoDropDownHeight= false;

    $("#" + tag).jqxDropDownList(
    {
        rtl: true,
        source: srcAdapter,
        width: width,
        dropDownHeight: dropDownHeight,
        autoDropDownHeight: autoDropDownHeight,
        placeHolder: 'נא לבחור',
        displayMember: displayMember,
        valueMember: valueMember
    });
    return srcAdapter;
};
var createListAdapter = function (valueMember, displayMember, tag, url, width, height, async) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url, async);
    $("#" + tag).jqxListBox(
    {
        rtl: true,
        source: srcAdapter,
        width: width,
        height: height,
        displayMember: displayMember,
        valueMember: valueMember
    });
    return srcAdapter;
};
var createtDataAdapter = function (valueMember, displayMember, url, async) {
    if (typeof async === 'undefined') { async = true; }
    var source =
            {
                dataType: "json",
                async: async,
                dataFields: [
                    { name: valueMember },
                    { name: displayMember }
                ],
                type: 'POST',
                url: url
            };
    var srcAdapter = new $.jqx.dataAdapter(source);
    return srcAdapter;
};

function appendIframe(div, src, width, height, scrolling) {
    var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
    $("#" + div).append(iframe);
    iframe.attr({
        scrolling: scrolling,
        width: width,
        height: height,
        src: src
    });
};

function listBoxToInput(list, input, checkbox) {
    //$('#' + list).on('change', function (event) {
    var items = $("#" + list).jqxListBox('getSelectedItems');
    var checked = true;
    var values = "";
    if (items && items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            values += items[i].value;
            if (i < items.length - 1) values += ",";
        }
        checked = false;
    }
    if (checkbox)
        $("#" + checkbox).prop('checked', checked);
    $("#" + input).val(values);
};
function listCheckBoxToInput(list, input, checkbox) {
    //$('#' + list).on('checkChange', function (event) {
    var items = $("#" + list).jqxListBox('getCheckedItems');
    var checked = true;
    var values = "";
    if (items && items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            values += items[i].value;
            if (i < items.length - 1) values += ",";
        }
        checked = false;
    }
    if (checkbox)
        $("#" + checkbox).prop('checked', checked);
    $("#" + input).val(values);
};
function comboBoxToInput(list, input, checkbox) {
    //$('#' + list).on('change', function (event) {
    var item = $("#" + list).jqxComboBox('getSelectedItem');
    var checked = true;
    var value = "";
    if (item) {
        value = item.value;
        checked = false;
    }
    if (checkbox)
        $("#" + checkbox).prop('checked', checked);
    $("#" + input).val(value);
};
///////////////////////////////  jq ///////////////////////////

function checkTagObject(obj) {

    for (var o in obj) {
        var i = o;
    }

    // Visit non-inherited enumerable keys
    Object.keys(obj).forEach(function (key) {
        console.log(key, obj[key]);
    });

    for (var key in validation_messages) {
        if (validation_messages.hasOwnProperty(key)) {
            var obj = validation_messages[key];
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    //alert(prop + " = " + obj[prop]);
                    console.log(prop + " = " + obj[prop]);
                }
            }
        }
    }
}

var adminLink = function () {
    return '<a href="/Admin/Manager">מנהל מערכת</a>'
};

//======================== upload


function getImgUrl(baseUrl,mediaType, picUrl) {
    if (mediaType == 'img')
        return baseUrl+'/Uploads/img' + '/' + picUrl;
    else if (mediaType == 'doc')
        return baseUrl + '/Uploads/doc' + '/' + picUrl;
    else
        return picUrl;
};

function getImgTumb(baseUrl, picUrl) {
    var mediaType = getMediaType(picUrl);

    var imgurl;
    var imgtumb;
    if (mediaType == 'img') {
        imgurl = baseUrl+'/Uploads/img' + '/' + picUrl;
        //imgtumb = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>').colorbox({ rel: 'group1' });
        //imgtumb = '<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>';
        imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>'

    }
    else if (mediaType == 'doc') {
        imgurl = baseUrl + '/Images/doc.jpg';
        imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
    }
    else if (mediaType == 'link') {
        imgurl = baseUrl + '/Images/link.jpg';
        imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
    }
    else {
        imgurl = baseUrl + '/Images/none.jpg';
        imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');

    }
    return imgtumb;
};
function getImgTag(baseUrl, mediaType, picUrl) {

    var imgurl;
    var imgtag;

    if (mediaType == 'img') {
        imgurl = baseUrl + '/Uploads/img' + '/' + picUrl;
        imgtag = $('<div style="margin: 10px;"><b>תמונה:</b><br/></div><div style="margin: 10px;overflow:auto;"><img src="' + imgurl + '"/></div>');
    }
    else if (mediaType == 'doc') {
        imgurl = baseUrl + '/Uploads/doc' + '/' + picUrl;
        imgtag = $('<div style="margin: 10px;"><b>מסמך:</b><br/></div><div><a href="' + imgurl + '">לצפיה</a></div>');
    }
    else if (mediaType == 'link') {
        imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">לצפיה</a></div>');
    }
    else {
        imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">לצפיה</a></div>');

    }
    return imgtag;

    //return '<div style="margin: 10px;overflow:auto;"><img src="' + getImgUrl(mediaType,picUrl) + '"/></div>';
};

function getFileExtension(filename) {
    return filename.split('.').pop();
};
function getMediaType(filename) {
    if (filename.substr(0, 4) == 'http')
        return 'link';
    var extension = getFileExtension(filename);
    switch (extension.toLowerCase()) {
        case 'jpg':
        case 'jpeg':
        case 'png':
        case 'gif':
        case 'tif':
            return 'img';
        case 'pdf':
        case 'doc':
        case 'docx':
        case 'xls':
        case 'xlsx':
        case 'txt':
            return 'doc';

    }
    return "none";
};

function generateUUIDv4() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
};

function generateUUID(v) {
    if (v == 'v4')
        return Math.uuid() // RFC4122 v4 UUID
    //"4FAC90E7-8CF1-4180-B47B-09C3A246CB67"
    if (v == '62')
        return Math.uuid(17) // 17 digits, base 62 (0-9,a-Z,A-Z)
    //"GaohlDbGYvOKd11p2"

    if (v == '10')
        return Math.uuid(5, 10) // 5 digits, base 10
    //"84274"

    if (v == '16')
        return Math.uuid(8, 16) // 8 digits, base 16
    //"19D954C3"
};
