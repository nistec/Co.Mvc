
function tickets_edit_view_comment(row) {

    var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
    content = content.replace("\n", "<br/>");
    app_jqx.toolTipClick(".ticket-comment", '<p>'+content+'</p>', 350);
}
function triggerTicketCommentCompleted(data) {
    app_tickets_comment.end(data);
}
function triggerTicketAssignCompleted(data) {
    app_tickets_assign.end(data);
}
function triggerTicketTimerCompleted(data) {
    app_tickets_timer.end(data);
}
function triggerTicketFormCompleted(data) {
    app_tickets_form.end(data);
}

function setTicketButton(item,action, visible) {
    var name = 'עדכון';

    if (item == 'timer') {
        if (action == 'add')
            name = 'התחל';
        else if (action == 'update')
            name = 'סיום';
    }
    else if (action == 'add')
        $('#ticket-item-update').val('');
    else if (action == 'update')
        $('#ticket-item-update').val('עדכון');

    $('#ticket-item-update').html(name);

    if (visible !== undefined) {
        if (visible)
            $('#ticket-item-update').show();
        else
            $('#ticket-item-update').hide();
    }
}

//============================================================================================ app_ticket_def
(function ($) {


//var ticket_def;

app_ticket = {
    init: function (TicketId, userInfo, isdialog) {
        
        return new app_ticket_def(TicketId, userInfo, isdialog)
    }
};


//$('#ticket-item-update').click(function () {
//    var iframe = wizard.getIframe();
//    if (iframe && iframe.assign_def) {
//        iframe.assign_def.doSubmit();
//    }
//});
//$('#ticket-item-cancel').click(function () {
//    wizard.wizHome();
//});

function app_ticket_def(dataModel,userInfo,isdialog) {

    this.TicketId = dataModel.PId;
    this.Model = dataModel;
    this.UserInfo = userInfo;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    this.IsDialog = isdialog;
    this.uploader;
    var slf = this;

    $("#AccountId").val(this.AccountId);
    $("#hx-1").text("משימה: " + this.TicketId);

    $("#accordion").accordion({ heightStyle: "content", rtl: true, editable: true });
    $("#jqxExp-1").jqxExpander({ rtl: true, width: '300px', expanded: false });
    $("#ColorFlag").simplecolorpicker();
    $("#ColorFlag").on('change', function () {
        //$('select').simplecolorpicker('destroy');
        var color = $("#ColorFlag").val();
        $("#hTitle").css("color", color)
    });
    $("#TicketStatus").jqxDropDownList({ enableSelection: false });

    $('#TicketBody').jqxEditor({
        //height: '200px',
        //width: '100%',
        editable: dataModel.Option == 'e',
        rtl: true,
        tools: 'bold italic underline | color background | left center right'
        //theme: 'arctic'
        //stylesheets: ['editor.css']
    });

    $("#accordion").accordion({
        beforeActivate: function (event, ui) {
            switch(ui.newHeader.context.id){
                case"ui-id-1":
                    if ($("#jqxgrid1")[0].childElementCount == 0)
                        app_tickets_comment.load(slf.Model, slf.UserInfo);
                    break;
                case"ui-id-2":
                    if ($("#jqxgrid2")[0].childElementCount == 0)
                        app_tickets_assign.load(slf.Model, slf.UserInfo);
                    break;
                case"ui-id-3":
                    if ($("#jqxgrid3")[0].childElementCount == 0)
                        app_tickets_timer.load(slf.Model, slf.UserInfo);
                    break;
                case"ui-id-4":
                    if ($("#jqxgrid4")[0].childElementCount == 0)
                        app_tickets_form.load(slf.Model, slf.UserInfo);
                    break;
                case "ui-id-5":

                    if (slf.uploader == null) {
                        slf.uploader = new app_media_uploader("#task-files");
                        slf.uploader.init(slf.TaskId, 't');
                        slf.uploader.show();
                    }

                    //if ($("#iframe-files").attr('src') === undefined)
                    //    var op = slf.Model.Option;
                    //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TicketId + '&refType=t&op=' + op, '100%', '350px', true);
                    break;
            }
        }
    });

    //$("#accordion").accordion({ activate: function(event, ui) {
    //    alert(ui.newHeader.text());
    //}
    //});

    this.loadControls();

    this.doCancel = function () {
        app.redirectTo("/System/TicketUser");
        //app_messenger.Notify("הודעה");
        //parent.triggerCancelEdit();
    }
    this.doComment = function (id) {
        wizard.displayStep(2);
        $.ajax({
            type: 'GET',
            url: '/System/_TicketComment',
            data: { "id": id },
            success: function (data) {
                $('#divPartial2').html(data);
            }
        });
    }
    this.doAssign = function (id) {
        wizard.displayStep(3);
        $.ajax({
            type: 'GET',
            url: '/System/_TicketAssign',
            data: { "id": id },
            success: function (data) {
                $('#divPartial3').html(data);
            }
        });
    }

   
    this.doSubmit = function () {
        //e.preventDefault();

        var actionurl = $('#fcForm').attr('action');
        var status = $("#TicketStatus").val();

        if (status > 1 && status < 8)
        {
            if (confirm("האם לסיים משימה?") == false)
                return;
            else
                actionurl = '/System/TicketCompleted';
        }
        var value = $("#TicketBody").jqxEditor('val');
        var args = [{ key: 'TicketBody', value: app.htmlEscape(value)}];
        var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);

        var validationResult = function (isValid) {
            if (isValid) {
                //var formData = app.serialize('#fcForm');
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: formData,
                    success: function (data) {
                        app_messenger.Notify(data,'info', "/System/TicketUser");
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Notify(error,'error');
                    }
                });
            }
        }
        $('#fcForm').jqxValidator('validate', validationResult);
    };
       

    var view_source =
   {
    datatype: "json",
    //datafields: [
    //        { name: 'AccountId', type: 'number' },
    //        { name: 'AccountName', type: 'string' },
    //        { name: 'AccountType', type: 'string' },
    //        { name: 'AccountCategory', type: 'number' },
    //        //{ name: 'CompanyName', type: 'string' },
    //        //{ name: 'ContactName', type: 'string' },
    //        { name: 'Street', type: 'string' },
    //        { name: 'City', type: 'string' },
    //        { name: 'Phone1', type: 'string' },
    //        { name: 'Phone2', type: 'string' },
    //        //{ name: 'Mobile', type: 'string' },
    //        //{ name: 'Email', type: 'string' },
    //        { name: 'Fax', type: 'string' },
    //        { name: 'WebSite', type: 'string' },
    //        { name: 'ZipCode', type: 'string' },
    //        { name: 'Details', type: 'string' }
    //],
    id: 'TicketId',
    data: { 'id': slf.TicketId },
    type: 'POST',
    url: '/System/GetTicketEdit'
  };

    this.viewAdapter = new $.jqx.dataAdapter(view_source, {
        loadComplete: function (record) {

            slf.syncData(record);
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    if (this.TicketId > 0) {
        this.viewAdapter.dataBind();
    }
};

app_ticket_def.prototype.syncData = function (record) {

    if (record) {
        if (record.TicketStatus <= 0)
            record.TicketStatus = 1;
        

        app_jqxform.loadDataForm("fcForm", record);

        $("#TicketBody").jqxEditor('val', app.htmlUnescape(record.TicketBody));
        $("#CreatedDate").val(record.CreatedDate);
        $("#StartedDate").val(app.toLocalDateString(record.StartedDate));
        $("#EndedDate").val(app.toLocalDateString(record.EndedDate));
       
        $("#TicketSubject").val(record.TicketSubject);
        $("#hTitle").text("משימה: " + record.TicketSubject);
        $("#hTitle").css("color" , (record.ColorFlag||'#000'));
        
        if (record.TicketStatus > 1 && record.TicketStatus <8)
            $("#fcSubmit").val("סיום");
        else
            $("#fcSubmit").val("עדכון");


        //app_jqxcombos.selectCheckList("listCategory", record.Categories);

        //app_jqxcombos.initComboValue('City', 0);
    }
};

app_ticket_def.prototype.loadControls = function () {

    $('#DueDate').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, disabled: true });
    $('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, disabled: true });
    //$('#DueDate').val('');

    
    app_jqx_list.ticketTypeComboAdapter();
    app_jqx_list.ticketStatusComboAdapter();
    app_jqxcombos.createComboAdapter("ProjectId", "ProjectName", "Project_Id", '/System/GetProjectList', 0, 120, false);

    var input_rules = [
          //{ input: '#TicketSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' }
          //{ input: '#TicketBody', message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: 'required' },
          {
              input: "#TicketBody", message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: function (input, commit) {
                  //var value = $("#TicketBody").text();//.jqxEditor('val');
                  var value = $("#TicketBody").jqxEditor('val');
                  value = app.htmlText(value);//.replace(/(<([^>]+)>)/ig, "");
                  return value.length > 0;
              }
          }
    ];
 
    $('#fcForm').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

    //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid='+this.TicketId   +'&refType=t', '100%', '360px', true);
};

    //============================================================ app_tickets_comment

app_tickets_comment = {

    wizardStep: 2,
    TicketId: 0,
    Model: {},
    load: function (dataModel, userInfo) {
        this.TicketId = dataModel.PId;
        this.Model = dataModel;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TicketId);
        return this;
    },
    grid: function (ticketid) {
        var slf = this;

        var nastedsource = {
            datafields: [
                  { name: 'CommentId', type: 'number' },
                  { name: 'CommentDate', type: 'date' },
                  { name: 'CommentText', type: 'string' },
                  { name: 'ReminderDate', type: 'date' },
                  { name: 'Attachment', type: 'string' },
                  { name: 'DisplayName', type: 'string' },
                  { name: 'Ticket_Id', type: 'number' },
                  { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'CommentId',
            type: 'POST',
            url: '/System/GetTicketsCommentGrid',
            data: { 'pid': ticketid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        $("#jqxgrid1").jqxGrid({
            width: '100%',
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              {
                  text: 'מועד רישום', datafield: 'CommentDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
              { text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center'},
              { text: 'שם', datafield: 'DisplayName', cellsalign: 'right', align: 'center', hidden: app.IsMobile() }
              //{
              //    text: '...', datafield: 'CommentId', width: 120, cellsalign: 'right', align: 'center',
              //              cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
              //                  //var value = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;

              //                  //app_jqx.toolTipClick(null, value, 350);

              //                  //var drop = '<a class="drop-target drop-theme-hubspot-popovers">הצג</a>' +
              //                  //      '<div class="drop-content">' +
              //                  //          '<div class="drop-content-inner">' +
              //                  //              '<h3 class="title">Drop.js</h3>' +
              //                  //              '<p>Drop.js is a fast and capable dropdown library built on <a href="/tether/docs/welcome" target="_blank" style="color: inherit">Tether</a>.</p>' +
              //                  //          '</div>' +
              //                  //      '</div>';

              //                  return '<div class="ticket-comment" style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="tickets_edit_view_comment('+row+')">' + 'הצג' + '</a></div>';
              //              }
              //}
            ]
        });
        $('#jqxgrid1').on('rowdoubleclick', function (event) {
            slf.edit();
        });
        $('#jqxgrid1-add').click(function () {
            slf.add();
        });

        $('#jqxgrid1-edit').click(function () {
            slf.edit();
        });

        $('#jqxgrid1-remove').click(function () {
            slf.remove();
        });
        $('#jqxgrid1-refresh').click(function () {
            slf.refresh();
        });

       

    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    add: function () {
        setTicketButton('comment', 'add', true);
        wizard.appendIframe(2, app.appPath() + "/System/_TicketCommentAdd?pid=" + this.TicketId, "100%", "500px");
    },
    edit: function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            setTicketButton('comment', 'update', true);
            wizard.appendIframe(2, app.appPath() + "/System/_TicketCommentEdit?id=" + id, "100%", "500px");
        }
    },
    remove: function () {
        var id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק הערה ' + id)) {
                app_query.doPost(app.appPath() + "/System/TicketCommentDelete", { 'id': id });
                $('#jqxgrid1').jqxGrid('source').dataBind();
            }
        }
    },
    refresh: function () {
        $('#jqxgrid1').jqxGrid('source').dataBind();
    },
    cancel: function () {
        wizard.wizHome();
    },
    end: function (data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
            // app_dialog.alert(data.Message);
        }
    }

};


    //============================================================ app_tickets_action

app_tickets_action = {

    wizardStep: 2,
    TicketId: 0,
    UserId:0,
    Model: {},
    load: function (dataModel, userInfo) {
        this.TicketId = dataModel.PId;
        this.UserId = userInfo.UserId;
        this.Model = dataModel;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TicketId);
        return this;
    },
    grid: function (ticketid) {
        var slf = this;

        var nastedsource = {
            datafields: [
                  { name: 'ItemId', type: 'number' },
                  { name: 'ItemDate', type: 'date' },
                  { name: 'ItemText', type: 'string' },
                  { name: 'DoneDate', type: 'date' },
                  { name: 'DoneStatus', type: 'bool' },
                  { name: 'DisplayName', type: 'string' },
                  { name: 'Ticket_Id', type: 'number' },
                  { name: 'AssignBy', type: 'number' },
                  { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetTicketsFormGrid',
            data: { 'pid': ticketid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        $("#jqxgrid4").jqxGrid({
            width: '100%',
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              { text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
              { text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center' },
              { text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
              { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center' },
               //{ text: 'Product', columntype: 'dropdownlist', datafield: 'productname', width: 195 },
              { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' } 
              //{
              //    text: '...', datafield: 'ItemId', width: 120, cellsalign: 'right', align: 'center',
              //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
              //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
              //    }
              //}
            ]
        });
        $('#jqxgrid4').on('rowdoubleclick', function (event) {
            slf.edit();
        });
        $('#jqxgrid4-add').click(function () {
            slf.add();
        });

        $('#jqxgrid4-edit').click(function () {
            slf.edit();
        });

        $('#jqxgrid4-remove').click(function () {
            slf.remove();
        });
        $('#jqxgrid4-refresh').click(function () {
            slf.refresh();
        });

    },
    getRowData: function () {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return null;
        var data = $('#jqxGrid').jqxGrid('getrowdata', selectedrowindex);
        return data;
    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid4").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    add: function () {
        setTicketButton('form', 'add', true);
        wizard.appendIframe(2, app.appPath() + "/System/_TicketFormAdd?pid=" + this.TicketId, "100%", "500px");
    },
    edit: function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0){
            setTicketButton('form', 'update', true);
        wizard.appendIframe(2, app.appPath() + "/System/_TicketFormEdit?id=" + id, "100%", "500px");
       }
    },
    remove: function () {

        var data = this.getRowData();
        if (data == null)
            return;
        if (this.UserId != data.AssignBy)
        {
            app_dialog.Alert("לא ניתן למחוק פעולה שהוקצתה על ידי משתמש אחר");
            return;
        }
        var id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק פעולה ' + id)) {
                app_query.doPost(app.appPath() + "/System/TicketFormDelete", { 'id': id });
                $('#jqxgrid4').jqxGrid('source').dataBind();
            }
        }
    },
    refresh: function () {
        $('#jqxgrid4').jqxGrid('source').dataBind();
    },
    cancel: function () {
        wizard.wizHome();
    },
    end: function (data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
        }
    }
};
})(jQuery)

