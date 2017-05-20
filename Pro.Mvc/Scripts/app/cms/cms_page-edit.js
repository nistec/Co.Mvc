
//============================================================================================ app_cms_content_def
function app_cms_head_edit(accountId) {


    if (accountId === undefined) {
        return;
    }
    var AccountId=accountId;
    var slf = this;

    $("#updateHeader").on('click', function () {

        var contant = app.htmlEscape($('#editorHeader').val());
        var data = {
            AccountId: $('#AccountId').val(),
            PageType: 'Head',
            Section: 'Header',
            Content: encodeURIComponent(contant)
        };
        sendCommand(data);
    });

    $("#updateFooter").on('click', function () {

        var contant = app.htmlEscape($('#editorFooter').val());
        var data = {
            AccountId: $('#AccountId').val(),
            PageType: 'Head',
            Section: 'Footer',
            Content: encodeURIComponent(contant)
        };
        sendCommand(data);
    });

    var sendCommand = function (rowdata) {
        console.log(rowdata);
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/Cms/CmsContentUpdate',
            data: rowdata,
            success: function (data, status, xhr) {
               app_dialog.alert(data.Message);
                if (data.Status > 0) {
                    dataAdapter.dataBind();
                }
            },
            error: function () {
               app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
            }
        });
    };


    var source =
    {
        dataType: "json",
        //datafields:
        //[
        //    { name: 'AccountId', type: 'number' },
        //    { name: 'PageType', type: 'string' },
        //    { name: 'Section', type: 'string' },
        //    { name: 'Content', type: 'string' }
        //],
        id: 'AccountId',
        data: { 'accountId': AccountId },
        type: 'POST',
        url: '/Cms/GetCmsHead'
    };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        loadComplete: function (record) {
            if (record) {
                $('#editorHeader').val(app.htmlUnescape(record.Header));
                $('#editorFooter').val(app.htmlUnescape(record.Footer));
                $('#AccountId').val(record.AccountId);
                $('#listDesign').val(record.DesignType);
                $('#DesignType').val(record.DesignType);
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    dataAdapter.dataBind();

    return this;

};

function app_cms_pages(accountId) {

    this. AccountId = accountId;

    this.getExtId = function (accountId, pageType, section) {

        return accountId.toString() + '-' + pageType + '-' + section;
    };

    this.preview = function (folder, pageType) {

        app_popup.cmsPreview(folder, pageType);
    };
}

function app_cms_html_edit(extid) {


    if (extid === undefined) {
        return;
    }
    var pageName;
    var pageCategory;
    var topMenu;
    var footerMenu;

    // create editor.
    $('#editor').jqxEditor({
        height: '500px',
        width: '800px',
        rtl: true
        //theme: 'arctic'
        //stylesheets: ['editor.css']
    });

    $("#btnUpdtate").on('click', function () {

        var contant = app.htmlEscape($('#editor').val());
        var data = {
            AccountId: $('#AccountId').val(),
            PageType: $('#PageType').val(),
            Section: $('#Section').val(),
            Content: encodeURIComponent(contant)
        };
        sendCommand(data);
    });

    $("#btnCancel").on('click', function () {
        //window.location.href = "CmsContentGrid";
        dialogIframClose();
    });

    
    var sendCommand = function (rowdata) {
        console.log(rowdata);
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/Cms/CmsContentUpdate',
            data: rowdata,
            success: function (data, status, xhr) {
               app_dialog.alert(data.Message);
                if (data.Status > 0) {
                    dataAdapter.dataBind();
                    //alert('עודכן בהצלחה');
                    window.parent.triggerCmsComplete();
                }
            },
            error: function () {
               app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
            }
        });
    };


 var source =
 {
     dataType: "json",
     datafields:
     [
         { name: 'AccountId', type: 'number' },
         { name: 'PageType', type: 'string' },
         { name: 'Section', type: 'string' },
         { name: 'Content', type: 'string' }
     ],
     id: 'Field',
     data: { 'extid': extid },
     type: 'POST',
     url: '/Cms/GetCmsContent'
 };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        loadComplete: function (record) {
            if (record) {
                $('#editor').val(app.htmlUnescape(record.Content));
                $('#AccountId').val(record.AccountId);
                $('#PageType').val(record.PageType);
                $('#Section').val(record.Section);
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    dataAdapter.dataBind();

    return this;

};


function app_cms_text_edit(extid) {


    if (extid === undefined) {
        return;
    }
    var pageName;
    var pageCategory;
    var topMenu;
    var footerMenu;

    // create editor.
    $('#editor').css({
        height: '500px',
    width: '800px'
    //rtl: true
    });

    $("#btnUpdtate").on('click', function () {
       
        var contant = app.htmlEscape($('#editor').val());
        var data = {
            AccountId: $('#AccountId').val(),
            PageType: $('#PageType').val(),
            Section: $('#Section').val(),
            Content: encodeURIComponent(contant)
        };
        sendCommand(data);
    });

    $("#btnCancel").on('click', function () {
        //window.location.href = "CmsContentGrid";
        dialogIframClose();
    });


    var sendCommand = function (rowdata) {
        console.log(rowdata);
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/Cms/CmsContentUpdate',
            data: rowdata,
            success: function (data, status, xhr) {
               app_dialog.alert(data.Message);
                if (data.Status > 0) {
                    dataAdapter.dataBind();
                    //alert('עודכן בהצלחה');
                    window.parent.triggerCmsComplete();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
               app_dialog.alert(' אירעה שגיאה, לא עודכנו נתונים ' + errorThrown);
            }
        });
    };


    var source =
    {
        dataType: "json",
        datafields:
        [
            { name: 'AccountId', type: 'number' },
            { name: 'PageType', type: 'string' },
            { name: 'Section', type: 'string' },
            { name: 'Content', type: 'string' }
        ],
        id: 'Field',
        data: { 'extid': extid },
        type: 'POST',
        url: '/Cms/GetCmsContent'
    };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        loadComplete: function (record) {
            if (record) {
                $('#editor').val(app.htmlUnescape(record.Content));
                $('#AccountId').val(record.AccountId);
                $('#PageType').val(record.PageType);
                $('#Section').val(record.Section);
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    dataAdapter.dataBind();

    return this;

};
