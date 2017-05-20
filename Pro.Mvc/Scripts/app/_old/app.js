
function accountDisplay(val, title, divResult) {
    $.ajax({
        type: "POST",
        url: "/Ws/CrmWs.asmx/GetAccountDetails",
        data: "{ 'id': '" + val + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            dialogMessage(title, data.d);
        },
        error: function (e) {
            if (divResult)
                $("#" + divResult).html("WebSerivce unreachable");
        }
    });
}

function accountEdit(id, acctype) {
    return popupIframe("/Common/_AccountEdit?id=" + id + "&acctype=" + acctype, "500", "580");
};

function accountNewsAdd(id) {
    return popupIframe("/Common/_AccountNewsEdit?id=" + id, "400", "550");
};

function accountNewsRemove(newsid, accid) {
    if (confirm("האם להסיר לקוח " + accid + " מקבוצת דיוור " + newsid)) {
        $.ajax({
            type: "POST",
            url: "/Ws/CrmWs.asmx/AccountNewsDelete",
            data: "{ 'AccountId': '" + accid + "','NewsId': '" + newsid + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dialogMessage('קבוצות דיוור', 'לקוח ' + accid + ' הוסר מקבוצת דיוור ' + newsid, true);
            },
            error: function (e) {
                alert(e);
            }
        });
    }
};
function accountContacts(id) {
    popupIframe("/Common/_AccountContacts?id=" + id , "600", "320");
};
function contactDisplay(val, title, divResult) {
    $.ajax({
        type: "POST",
        url: "/Ws/CrmWs.asmx/GetContactDetails",//"/Ws/CrmWs.asmx/GetContactDetails",
        data: "{ 'id': '" + val + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            dialogMessage(title, data.d);
        },
        error: function (e) {
            if (divResult)
                $("#" + divResult).html("WebSerivce unreachable");
        }
    });
}

function contactEdit(id, accid, op) {
    if (op == "d")
       return dialogIframe("/Common/_ContactEdit?id=" + id + "&accid=" + accid + "&op=" + op, "300", "250", "אנשי קשר");
    else
      return  popupIframe("/Common/_ContactEdit?id=" + id + "&accid=" + accid+ "&op=p", "320", "320");
};
//function contactEditDialog(id, accid) {
//    //popupAjax("/Common/_ContactEdit?id=" + id + "&accid=" + accid);
//    //popupIframe("/Common/_ContactEdit?id=" + id + "&accid=" + accid, "320", "300");
//    dialogIframe("/Common/_ContactEdit?id=" + id + "&accid=" + accid, "300", "250", "אנשי קשר");
//};
function contactDelete(id, async) {
    if (typeof async === 'undefined') { async = true; }

    if (confirm("האם למחוק איש קשר " + id)) {
        $.ajax({
            async: async,
            type: "POST",
            url: "/Ws/CrmWs.asmx/ContactDelete",
            data: "{ 'id': '" + id + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dialogMessage('אנשי קשר', 'איש קשר ' + id + 'הוסר מהמערכת ',true);
            },
            error: function (e) {
                alert(e);
            }
        });
    }
};

function mediaEditor(bid, pid, pt) {
    popupIframe("/Building/_Media?bid=" + bid + "&pid=" + pid + "&pt=" + pt, "900", "520");
};

function setInputValue(tag, value, asEmpty) {
    if (value != asEmpty)
        $("#" + tag).val(value);
}
function loadDataForm(form, record) {
    var arr = '';
    var types = '';
    //$('#form > input').each(function () {
    //$('#' + form + '.text').each(function () {
    //var formChildren = $("#tag-form > *").each(
    //function (index) {
    //    var input = $(this);
    //    arr += input.attr('name') + ',';
    //    //alert('Type: ' + input.attr('type') + 'Name: ' + input.attr('name') + 'Value: ' + input.val());
    //});
    //alert(arr);
    //arr = '';
    $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
        var input = $(this);

        var tag = input.attr('name');
        if (tag !== undefined) {

            //var type = input.attr('type');
            //types += type + ',';
            //var datatype = input.attr('data-type');
            var value = record[tag];
            //arr += tag + ',' + type + '\n';
            //if (datatype == "combo")
            //    selectComboBoxValue(tag, value);
            //if (datatype == "checklist")
            //    selectCheckList(tag, value);
            //else {

                $("#" + tag).val(value);

            //}
        }
        //arr += input.attr('name') + ',';
        //alert('Type: ' + input.attr('type') + 'Name: ' + input.attr('name') + 'Value: ' + input.val());
    });
    //alert(types);
    //alert(arr);
    //arr = '';
    //$.each($('.text'), function () {
    //    var tag = $(this).attr('name');
    //    var type = $(this).attr('data-type');
    //    var value = record[tag];
    //    if (type == "combo")
    //        selectComboBoxValue(tag, value);
    //    else if (type == "checklist")
    //        selectCheckList(tag, value);
    //    else
    //        $("#" + tag).val(value);
    //    arr += tag + ',';
    //});
    //alert(arr);
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

var createComboAdapter = function (valueMember, displayMember, tag, url, width, dropDownHeight) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url);
    var autoDropDownHeight = true;
    if (typeof width === 'undefined') { width = 240; }
    if (typeof dropDownHeight === 'undefined')
        dropDownHeight = 200;
    else
        autoDropDownHeight: false;

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

var createDropDownAdapter = function (valueMember, displayMember, tag, url, width, dropDownHeight) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url);
    var autoDropDownHeight = true;
    if (typeof width === 'undefined') { width = 240; }
    if (typeof dropDownHeight === 'undefined')
        dropDownHeight = 200;
    else
        autoDropDownHeight: false;

    $("#" + tag).jqxDropDownList(
    {
        rtl: true,
        source: srcAdapter,
        width: width,
        dropDownHeight: dropDownHeight,
        autoDropDownHeight: autoDropDownHeight,
        placeHolder:'נא לבחור',
        displayMember: displayMember,
        valueMember: valueMember
    });
    return srcAdapter;
};
var createListAdapter = function (valueMember, displayMember, tag, url, width, height) {
    var srcAdapter = createtDataAdapter(valueMember, displayMember, url);
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
var createtDataAdapter = function (valueMember, displayMember, url) {
    var source =
            {
                dataType: "json",
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
    $("#"+div).append(iframe);
    iframe.attr({
        scrolling:scrolling,
        width: width,
        height: height,
        src: src
    });
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

var adminLink=function(){
  return  '<a href="/Admin/Manager">מנהל מערכת</a>'
};

//$("#print").jqxButton();

/*
print grid
$("#print").click(function () {
    var gridContent = $("#jqxgrid").jqxGrid('exportdata', 'html');
    var newWindow = window.open('', '', 'width=800, height=500'),
    document = newWindow.document.open(),
    pageContent =
        '<!DOCTYPE html>\n' +
        '<html>\n' +
        '<head>\n' +
        '<meta charset="utf-8" />\n' +
        '<title>jQWidgets Grid</title>\n' +
        '</head>\n' +
        '<body>\n' + gridContent + '\n</body>\n</html>';
    document.write(pageContent);
    document.close();
    newWindow.print();
});
*/