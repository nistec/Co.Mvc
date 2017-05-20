
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

function redirectTo(url) {
    window.location.href = url;
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


var adminLink = function () {

    return '<a href="' + appPath() + '/Admin/Manager">מנהל מערכת</a>'
};

var defAccountLink = function () {

    return '<a href="' + appPath() + '/Admin/DefAccount">לקוחות</a>'
};


function pro_doUploadProc(url,uploadKey,tag) {
    var m = url;
    $.ajax({
        url: url,
        type: "POST",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: "{'uploadKey':'" + uploadKey + "'}",
        //data: { "uploadKey": "" + uploadKey + "" },
        success: function (data) {
            if (data) {
                //if (data.Status < 0)
                //    redirectTo('UploadProc?up=' + uploadKey);
                //else if (data.Status == 10)
                //    redirectTo('UploadProc?up=' + uploadKey);
                //else
                //{
                //    connectUploadedProc();

                //    $("#Comment").html(data.Message);
                //    //$('#dataTable').Html()
                //}

                $(tag).html(data.Message);
            }

        },
        error: function (jqXHR, status, error) {
            alert(error);
        }
    });
};