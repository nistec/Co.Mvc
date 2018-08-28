
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



//=========================================================  bckup

/*
 var initLabelsGrid = function (tab, index, id) {

     var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
     nastedcontainer.rtl = true;
     var nastedsource = {
         datafields: [
             { name: 'Label', type: 'string' },
             { name: 'Val', type: 'string' },
             { name: 'AccountId', type: 'number' },
             { name: 'Modified', type: 'date' }
         ],
         datatype: "json",
         //id: 'UserId',
         type: 'POST',
         url: '/Admin/GetAccountsLabel',
         data: { 'id': id }
     }
     _slf.NContainer.LabelsGrid[index] = nastedcontainer;

     var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
     nastedcontainer.jqxGrid({
         source: nastedAdapter, width: '80%', height: 130, //showheader: true,
         localization: getLocalization('he'),
         rtl: true,
         columns: [
             //{
             //    text: 'קוד', dataField: 'LabelId', width: 100, cellsalign: 'right', align: 'center',
             //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
             //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="members_grid.usersEdit(' + value + ',' + index + ',' + row + ');" >הצג</a></div>'
             //    }
             //},
             { text: 'שדה', datafield: 'Label', width: 120, cellsalign: 'right', align: 'center' },
             {
                 text: 'פרטים', datafield: 'Val', width: 400, cellsalign: 'right', align: 'center',
                 cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                     if (value) {
                         if (value.match(/^http/i))
                             return '<div style = "margin:5px;" ><a href="' + value + '" target="_blank"> ' + value + '</a></div>';
                     }
                     return '<div style = "direction:rtl;text-align:right;margin:5px;" >' + value + '</div>';;
                 }
             },
             { text: 'מועד עדכון', datafield: 'Modified', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
         ]
     }).on('rowdoubleclick', function (event) {
         //var args = event.args;
         //var boundIndex = args.rowindex;
         //var visibleIndex = args.visibleindex;
         //var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex);
         //app_popup.memberEdit(id);
         doEditLabel();
     });
     
     var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;float:right;"></div>')
     var dialogContainer = $('<div id="label-dialog"></div>')

     var doEditLabel = function () {
         var rowindex = nastedcontainer.jqxGrid('getselectedrowindex');
         var data = nastedcontainer.jqxGrid('getrowdata', rowindex);

         var label_item = new app_account_label_item("#label-dialog");
         label_item.init(data, nastedAdapter, false);
         var dialog = _slf.NContainer.labelDialog = app_dialog.dialogDiv("#label-dialog", "פרטים נוספים");
         label_item.display(dialog);

     };

     var labelsCount = function () {

         var rows = nastedcontainer.jqxGrid('getrows');
         return rows == null ? 0 : rows.length;
     }


     var additem = $('<a class="btn-small w60" title="הוספת פרטים" href="#" >הוספה</a>')
         .click(function () {
             var dm = {'AccountId':id}
             var label_item = new app_account_label_item("#label-dialog");
             label_item.init(dm, nastedAdapter, false);
             var dialog = _slf.NContainer.labelDialog = app_dialog.dialogDiv("#label-dialog", "פרטים נוספים");
             label_item.display(dialog);
         });
     var updateitem = $('<a class="btn-small w60" title="עדכון פרטים" href="#" >עריכה</a>')
         .click(function () {
             doEditLabel();
         });
     var deleteitem = $('<a class="btn-small w60" title="מחיקת פרטים" href="#" >מחיקה</a>')
         .click(function () {

             var rowindex = nastedcontainer.jqxGrid('getselectedrowindex');
             var record = nastedcontainer.jqxGrid('getrowdata', rowindex);
             if (!record)
                 return;

             app_dialog.confirm("האם למחוק ? " + record.Label, function () {

                 app_query.doDataPost("/Admin/DeleteAccountsLabels", { 'AccountId': record.AccountId, 'Label': record.Label}, function (data) {
                     if (data.Status > 0) {
                             nastedAdapter.dataBind();
                     }
                     else
                         app_messenger.Post(data, 'error');
                 });

             });

         });
     var refreshitem = $('<a class="btn-small w60" title="רענון פרטים"  href="#" >רענון</a>')
         .click(function () {
             nastedAdapter.dataBind();
         });

     $(ctlContainer).append(dialogContainer);
     $(ctlContainer).append(additem);
     $(ctlContainer).append('<div style="height:5px"></div>');
     
         $(ctlContainer).append(updateitem);
         $(ctlContainer).append('<div style="height:5px"></div>');
         $(ctlContainer).append(deleteitem);
         $(ctlContainer).append('<div style="height:5px"></div>');
     
     $(ctlContainer).append(refreshitem);
     $(tab).append(ctlContainer);
     $(tab).append(nastedcontainer);

     //if (labelsCount() == 0) {
     //    updateitem.hide();
     //    deleteitem.hide();
     //}
 };
 */

var initCreditGrid2 = function (tab, index, id) {

    var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
    nastedcontainer.rtl = true;
    /*
    var nastedsource = {
        datafields: [
            { name: 'DisplayName', type: 'string' },
            { name: 'UserId', type: 'number' },
            { name: 'UserRole', type: 'number' },
            { name: 'RoleName', type: 'string' },
            { name: 'UserName', type: 'string' },
            { name: 'Email', type: 'string' },
            { name: 'Phone', type: 'string' },
            { name: 'AccountId', type: 'number' },
            { name: 'Lang', type: 'string' },
            { name: 'Evaluation', type: 'number' },
            { name: 'IsBlocked', type: 'bool' },
            { name: 'Creation', type: 'date' }
        ],
        datatype: "json",
        id: 'UserId',
        type: 'POST',
        url: '/Admin/GetAdminUsersProfile',
        data: { 'accountId': id }
    }
    _slf.NContainer.PricesGrid[index] = nastedcontainer;

    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
    nastedcontainer.jqxGrid({
        source: nastedAdapter, width: '80%', height: 130, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: 'קוד מחיר', dataField: 'UserId', width: 100, cellsalign: 'right', align: 'center',
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="members_grid.usersEdit(' + value + ',' + index + ',' + row + ');" >הצג</a></div>'
                }
            },
            //{ text: 'חשבון', datafield: 'AccountId', cellsalign: 'right', align: 'center' },
            //{ text: 'קוד תפקיד', datafield: 'UserRole',width: 60, cellsalign: 'right', align: 'center' },
            //{ text: 'תפקיד', datafield: 'RoleName', width: 90, cellsalign: 'right', align: 'center' },
            //{ text: 'שם משתמש', datafield: 'UserName', width: 120, cellsalign: 'right', align: 'center' },
            //{ text: 'פרטים', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
            //{ text: 'אימייל', datafield: 'Email', cellsalign: 'right', align: 'center' },
            //{ text: 'טלפון', datafield: 'Phone', width: 120, cellsalign: 'right', align: 'center' },
            ////{ text: 'נסיון', datafield: 'Evaluation', width: 60,cellsalign: 'right', align: 'center' },
            //{ text: 'חסום', datafield: 'IsBlocked', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
            { text: 'נוצר ב', datafield: 'Creation', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
        ]
    });
    */
    var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;float:right;"></div>')

    var additem = $('<a class="btn-tab" title="הוספת אשראי" href="#" >הוסף</a>')
        .click(function () {
            _slf.NContainer.creditDialog = app_dialog.dialogIframe(app.appPath() + "/Admin/_UserAdd?id=" + id, "580", "400", "אשראי");
        });
    var refreshitem = $('<a class="btn-tab" title="רענון אשראי"  href="#" >רענן</a>')
        .click(function () {
            nastedAdapter.dataBind();
        });

    $(ctlContainer).append(additem);
    $(ctlContainer).append('<div style="height:5px"></div>');
    $(ctlContainer).append(refreshitem);
    $(tab).append(ctlContainer);
    $(tab).append(nastedcontainer);

};

var initPricesGrid2 = function (tab, index, id) {

    var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
    nastedcontainer.rtl = true;
    /*
    var nastedsource = {
        datafields: [
            { name: 'DisplayName', type: 'string' },
            { name: 'UserId', type: 'number' },
            { name: 'UserRole', type: 'number' },
            { name: 'RoleName', type: 'string' },
            { name: 'UserName', type: 'string' },
            { name: 'Email', type: 'string' },
            { name: 'Phone', type: 'string' },
            { name: 'AccountId', type: 'number' },
            { name: 'Lang', type: 'string' },
            { name: 'Evaluation', type: 'number' },
            { name: 'IsBlocked', type: 'bool' },
            { name: 'Creation', type: 'date' }
        ],
        datatype: "json",
        id: 'UserId',
        type: 'POST',
        url: '/Admin/GetAdminUsersProfile',
        data: { 'accountId': id }
    }
    _slf.NContainer.PricesGrid[index] = nastedcontainer;

    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
    nastedcontainer.jqxGrid({
        source: nastedAdapter, width: '80%', height: 130, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: 'קוד מחיר', dataField: 'UserId', width: 100, cellsalign: 'right', align: 'center',
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="members_grid.usersEdit(' + value + ',' + index + ',' + row + ');" >הצג</a></div>'
                }
            },
            //{ text: 'חשבון', datafield: 'AccountId', cellsalign: 'right', align: 'center' },
            //{ text: 'קוד תפקיד', datafield: 'UserRole',width: 60, cellsalign: 'right', align: 'center' },
            //{ text: 'תפקיד', datafield: 'RoleName', width: 90, cellsalign: 'right', align: 'center' },
            //{ text: 'שם משתמש', datafield: 'UserName', width: 120, cellsalign: 'right', align: 'center' },
            //{ text: 'פרטים', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
            //{ text: 'אימייל', datafield: 'Email', cellsalign: 'right', align: 'center' },
            //{ text: 'טלפון', datafield: 'Phone', width: 120, cellsalign: 'right', align: 'center' },
            ////{ text: 'נסיון', datafield: 'Evaluation', width: 60,cellsalign: 'right', align: 'center' },
            //{ text: 'חסום', datafield: 'IsBlocked', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
            { text: 'נוצר ב', datafield: 'Creation', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
        ]
    });
    */
    var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;float:right;"></div>')

    var additem = $('<a class="btn-tab" title="הוספת פריט" href="#" >הוסף</a>')
        .click(function () {
            _slf.NContainer.pricesDialog = app_dialog.dialogIframe(app.appPath() + "/Admin/_UserAdd?id=" + id, "580", "400", "מחירון");
        });
    var refreshitem = $('<a class="btn-tab" title="רענון מחירים"  href="#" >רענן</a>')
        .click(function () {
            nastedAdapter.dataBind();
        });

    $(ctlContainer).append(additem);
    $(ctlContainer).append('<div style="height:5px"></div>');
    $(ctlContainer).append(refreshitem);
    $(tab).append(ctlContainer);
    $(tab).append(nastedcontainer);

};

var initUsersGrid2 = function (tab, index, id) {

    var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
    nastedcontainer.rtl = true;
    var nastedsource = {
        datafields: [
            { name: 'DisplayName', type: 'string' },
            { name: 'UserId', type: 'number' },
            { name: 'UserRole', type: 'number' },
            { name: 'RoleName', type: 'string' },
            { name: 'UserName', type: 'string' },
            { name: 'Email', type: 'string' },
            { name: 'Phone', type: 'string' },
            { name: 'AccountId', type: 'number' },
            { name: 'Lang', type: 'string' },
            { name: 'Evaluation', type: 'number' },
            { name: 'IsBlocked', type: 'bool' },
            { name: 'Creation', type: 'date' }
        ],
        datatype: "json",
        id: 'UserId',
        type: 'POST',
        url: '/Admin/GetAdminUsersProfile',
        data: { 'accountId': id }
    }
    _slf.NContainer.UsersGrid[index] = nastedcontainer;

    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
    nastedcontainer.jqxGrid({
        source: nastedAdapter, width: '80%', height: 130, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: 'קוד משתמש', dataField: 'UserId', width: 100, cellsalign: 'right', align: 'center',
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="members_grid.usersEdit(' + value + ',' + index + ',' + row + ');" >הצג</a></div>'
                }
            },
            //{ text: 'חשבון', datafield: 'AccountId', cellsalign: 'right', align: 'center' },
            //{ text: 'קוד תפקיד', datafield: 'UserRole',width: 60, cellsalign: 'right', align: 'center' },
            { text: 'תפקיד', datafield: 'RoleName', width: 90, cellsalign: 'right', align: 'center' },
            { text: 'שם משתמש', datafield: 'UserName', width: 120, cellsalign: 'right', align: 'center' },
            { text: 'פרטים', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
            { text: 'אימייל', datafield: 'Email', cellsalign: 'right', align: 'center' },
            { text: 'טלפון', datafield: 'Phone', width: 120, cellsalign: 'right', align: 'center' },
            //{ text: 'נסיון', datafield: 'Evaluation', width: 60,cellsalign: 'right', align: 'center' },
            { text: 'חסום', datafield: 'IsBlocked', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
            { text: 'נוצר ב', datafield: 'Creation', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
        ]
    });

    //var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;float:right;"></div>')
    //var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;text-align:right"></div>');
    var ctlContainer = $('<div style="margin:2px;direction:rtl;text-align:right"></div>');

    var additem = $('<a class="btn-small w60" style="margin:0 2px" title="הוספת משתמש" href="#" >הוסף</a>')
        .click(function () {
            _slf.NContainer.usersDialog = app_dialog.dialogIframe(app.appPath() + "/Admin/_UserAdd?id=" + id, "580", "400", "משתמשים");
        });
    var refreshitem = $('<a class="btn-small w60" style="margin:0 2px" title="רענון משתמשים"  href="#" >רענן</a>')
        .click(function () {
            nastedAdapter.dataBind();
        });

    $(ctlContainer).append(additem);
    //$(ctlContainer).append('<div style="height:5px"></div>');
    $(ctlContainer).append(refreshitem);
    $(ctlContainer).append('<br/>');
    $(tab).append(ctlContainer);
    $(tab).append(nastedcontainer);

};