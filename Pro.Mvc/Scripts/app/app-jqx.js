/*
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
*/

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

var selectCheckList = function (tag, value, output) {
    if (value) {
        var items = value.toString().split(",");
        for (index = 0; index < items.length; ++index) {
            selectCheckListIndex(tag, items[index]);
        }
        if (output)
            $("#" + output).val(value);
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
var createListAdapter = function (valueMember, displayMember, tagList, url, width, height, async, output) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url, async);
    $("#" + tagList).jqxListBox(
    {
        rtl: true,
        source: srcAdapter,
        width: width,
        height: height,
        displayMember: displayMember,
        valueMember: valueMember
    });
    if (output) {
        $("#" + tagList).on('change', function (event) {
            listBoxToInput(tagList, output);
        });
    }
    return srcAdapter;
};
var createCheckListAdapter = function (valueMember, displayMember, tagList, url, width, height, async, output) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url, async);
    $("#" + tagList).jqxListBox(
    {
        rtl: true,
        source: srcAdapter,
        width: width,
        checkboxes: true,
        height: height,
        displayMember: displayMember,
        valueMember: valueMember
    });
    if (output) {
        $("#" + tagList).on('checkChange', function (event) {
            listCheckBoxToInput(tagList, output);
        });
    }
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

//debug
function debugObjectKeys(obj) {

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
