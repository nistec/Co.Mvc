//'use strict';

function tasks_edit_view_comment(row) {

    var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
    content = content.replace("\n", "<br/>");
    app_jqx.toolTipClick(".task-comment", '<p>'+content+'</p>', 350);
}
function triggerTaskCommentCompleted(data) {
    app_tasks_comment.end(data);
}
function triggerTaskAssignCompleted(data) {
    app_tasks_assign.end(data);
}
function triggerTaskTimerCompleted(data) {
    app_tasks_timer.end(data);
}
function triggerTaskFormCompleted(data) {
    app_tasks_form.end(data);
}

function triggerSubTaskCompleted(type, data) {
    var tag;
    switch (type) {
        case 'comment':
            tag = '#jqxgrid1'; break;
        case 'assign':
            tag = '#jqxgrid2'; break;
        case 'timer':
            tag = '#jqxgrid3'; break;
        case 'form':
            tag = '#jqxgrid4'; break;
    }

    if (tag != null) {
        $(tag + '-window').hide();
        if (data != null && data.Status > 0)
            $(tag).jqxGrid('source').dataBind();
    }
}

function setTaskButton(item,action, visible) {
    var name = 'עדכון';

    if (item == 'timer') {
        if (action == 'add')
            name = 'התחל';
        else if (action == 'update')
            name = 'סיום';


    }
    else if (action == 'add')
        $('#task-item-update').val('');
    else if (action == 'update')
        $('#task-item-update').val('עדכון');

    $('#task-item-update').html(name);

    if (visible !== undefined) {
        if (visible)
            $('#task-item-update').show();
        else
            $('#task-item-update').hide();
    }
}

//============================================================================================ app_task_def
(function ($) {


//var task_def;

app_task = {
    init: function (TaskId, userInfo, taskModel) {
        
        return new app_task_def(TaskId, userInfo, taskModel)
    }
};


//$('#task-item-update').click(function () {
//    var iframe = wizard.getIframe();
//    if (iframe && iframe.assign_def) {
//        iframe.assign_def.doSubmit();
//    }
//});
//$('#task-item-cancel').click(function () {
//    wizard.wizHome();
//});

function app_task_def(dataModel,userInfo,taskModel) {

    this.TaskId = dataModel.PId;
    this.Model = dataModel;
    this.TaskModel = taskModel;
    this.UserInfo = userInfo;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    this.Title = (this.TaskModel =='E') ? 'סוגיה' : 'משימה';

    var slf = this;
    var exp1_Inited = false;
    $("#AccountId").val(this.AccountId);
    $("#hxp-0").text(this.Title + ': ' + this.TaskId);

    //$("#accordion").accordion({ heightStyle: "content", rtl: true, editable: true });

    $("#accordion").responsiveTabs({
        rotate: false,
        startCollapsed: 'accordion',
        collapsible: 'accordion',
        //setHash: true,
        //disabled: [3, 4],
        click: function (e, tab) {
            //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
        },
        activate: function (e, tab) {
            //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

            switch (tab.id) {
                case 1://"hxp-1"://"ui-id-1":
                    if (slf.TaskId == 0) {
                        event.preventDefault();
                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                        return false;
                    }
                    if ($("#jqxgrid1")[0].childElementCount == 0)
                        app_tasks_comment.load(slf.Model, slf.UserInfo);
                    break;
                case 2://"hxp-2"://"ui-id-2":
                    if ($("#jqxgrid2")[0].childElementCount == 0)
                        app_tasks_assign.load(slf.Model, slf.UserInfo);
                    break;
                case 3://"hxp-3"://"ui-id-3":
                    if ($("#jqxgrid3")[0].childElementCount == 0)
                        app_tasks_timer.load(slf.Model, slf.UserInfo);
                    break;
                case 4://"hxp-4"://"ui-id-4":
                    if (slf.TaskId == 0) {
                        event.preventDefault();
                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                        return false;
                    }
                    if ($("#jqxgrid4")[0].childElementCount == 0)
                        app_tasks_form.load(slf.Model, slf.UserInfo);
                    break;
                case 5://"hxp-5"://"ui-id-5":
                    if (slf.TaskId == 0) {
                        event.preventDefault();
                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                        return false;
                    }
                    if ($("#iframe-files").attr('src') === undefined)
                        var op = slf.Model.Option;
                    app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t&op=' + op, '100%', '350px', true);
                    break;
            }
        },
        activateState: function (e, state) {
            //console.log(state);
            //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
        }
    });

    $("#jqxExp-1").jqxExpander({ rtl: true, width: '100%', expanded: false });
    $('#jqxExp-1').on('expanding', function () {
        if(!exp1_Inited)
        {
            //app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
            app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');
        }

        exp1_Inited = true;
    });


    $("#ColorFlag").simplecolorpicker();
    $("#ColorFlag").on('change', function () {
        //$('select').simplecolorpicker('destroy');
        var color = $("#ColorFlag").val();
        $("#hTitle").css("color", color)
    });
    $("#TaskStatus").jqxDropDownList({ enableSelection: false, disabled: true ,rtl:true});

    $('#TaskBody').jqxEditor({
        //height: '200px',
        //width: '100%',
        editable: dataModel.Option == 'e',
        rtl: true,
        tools: 'bold italic underline | color background | left center right'
        //theme: 'arctic'
        //stylesheets: ['editor.css']
    });
    /*
    $("#accordion").accordion({
        beforeActivate: function (event, ui) {
            switch(ui.newHeader.context.id){
                case "hxp-1"://"ui-id-1":
                    if (slf.TaskId == 0) {
                        event.preventDefault();
                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                        return false;
                    }
                    if ($("#jqxgrid1")[0].childElementCount == 0)
                        app_tasks_comment.load(slf.Model, slf.UserInfo);
                    break;
                case "hxp-2"://"ui-id-2":
                    if ($("#jqxgrid2")[0].childElementCount == 0)
                        app_tasks_assign.load(slf.Model, slf.UserInfo);
                    break;
                case "hxp-3"://"ui-id-3":
                    if ($("#jqxgrid3")[0].childElementCount == 0)
                        app_tasks_timer.load(slf.Model, slf.UserInfo);
                    break;
                case "hxp-4"://"ui-id-4":
                    if (slf.TaskId == 0) {
                        event.preventDefault();
                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                        return false;
                    }
                    if ($("#jqxgrid4")[0].childElementCount == 0)
                        app_tasks_form.load(slf.Model, slf.UserInfo);
                    break;
                case "hxp-5"://"ui-id-5":
                    if (slf.TaskId == 0) {
                        event.preventDefault();
                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                        return false;
                    }
                    if ($("#iframe-files").attr('src') === undefined)
                        var op = slf.Model.Option;
                    app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t&op=' + op, '100%', '350px', true);
                    break;
            }
        }
    });
   */ 

    //$("#accordion").accordion({ activate: function(event, ui) {
    //    alert(ui.newHeader.text());
    //}
    //});

    this.loadControls();

    this.doCancel = function () {
        app.redirectTo("/System/TaskUser");
        //app_messenger.Notify("הודעה");
        //parent.triggerCancelEdit();
    }
    this.doComment = function (id) {
        wizard.displayStep(2);
        $.ajax({
            type: 'GET',
            url: '/System/_TaskComment',
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
            url: '/System/_TaskAssign',
            data: { "id": id },
            success: function (data) {
                $('#divPartial3').html(data);
            }
        });
    }

   
    this.doSubmit = function () {
        //e.preventDefault();
        var slf = this;
        var actionurl = $('#fcForm').attr('action');
        var status = $("#TaskStatus").val();

        if (status > 1 && status < 8) {
            app_dialog.confirmYesNoCancel("האם לסיים משימה?", function (res) {
                if (res == 'yes')
                    slf.RunSubmit(status, '/System/TaskCompleted')
                else if (res == 'no')
                    slf.RunSubmit(status, actionurl);

            });
        }
        else {
            slf.RunSubmit(status, actionurl);
        }
    };
       
    this.RunSubmit = function (status,actionurl) {
        //e.preventDefault();

        //var actionurl = $('#fcForm').attr('action');

        if(status==0)
            $("#TaskStatus").val(1);
        if (status == 1)
            $("#TaskStatus").val(2);

        //var clientId = $("#ClientId").val();
        //if (clientId > 0) {
        //    $('#ClientDetails').val($("#ClientId-display").val())
        //}

        //var clientDetails = $('#ClientDetails').val();
        //, { key: 'ClientDetails', value: clientDetails }
        var value = $("#TaskBody").jqxEditor('val');
        var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }];
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
                        app_messenger.Notify(data, 'info', "/System/TaskUser");
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Notify(error, 'error');
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
    id: 'TaskId',
    data: { 'id': slf.TaskId },
    type: 'POST',
    url: '/System/GetTaskEdit'
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

    if (this.TaskId > 0) {
        this.viewAdapter.dataBind();
    }
};

app_task_def.prototype.syncData = function (record) {

    if (record) {
        if (record.TaskStatus <= 0)
            record.TaskStatus = 1;
        

        app_jqxform.loadDataForm("fcForm", record);

        $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
        $("#CreatedDate").val(record.CreatedDate);
        $("#StartedDate").val(app.toLocalDateString(record.StartedDate));
        $("#EndedDate").val(app.toLocalDateString(record.EndedDate));
       
        $("#TaskSubject").val(record.TaskSubject);
        $("#hTitle").text(this.Title+ ": " + record.TaskSubject);
        $("#hTitle").css("color" , (record.ColorFlag||'#000'));
        
        if (record.TaskStatus > 1 && record.TaskStatus <8)
            $("#fcSubmit").val("סיום");
        else
            $("#fcSubmit").val("עדכון");
        var editable = false;
        if(record.TaskStatus <8 && record.AssignBy==record.UserId)
        {
            editable = true;
        }
       
        $('#DueDate').jqxDateTimeInput({ disabled: !editable, showCalendarButton: editable });
        $("#TaskBody").jqxEditor({ editable: editable });

        //app_jqxcombos.selectCheckList("listCategory", record.Categories);

        //app_jqxcombos.initComboValue('City', 0);
    }
};

app_task_def.prototype.loadControls = function () {

    $('#DueDate').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true });
    $('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, disabled: true });
    //$('#DueDate').val('');

    
    app_jqx_list.taskTypeComboAdapter();
    app_jqx_list.taskStatusComboAdapter();
    app_jqxcombos.createComboAdapter("ProjectId", "ProjectName", "Project_Id", '/System/GetProjectList', 0, 120, false);

    var input_rules = [
          //{ input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
          {
              input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'keyup, blur', rule: 'required', rule: function (input, commit) {
                  //var value = $('#DueDate').jqxDateTimeInput('value');
                  var text = $('#DueDate').jqxDateTimeInput('getText');
                  return text != null && text.length > 0;
              }
          },
          {
              input: "#TaskBody", message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: function (input, commit) {
                  //var value = $("#TaskBody").text();//.jqxEditor('val');
                  var value = $("#TaskBody").jqxEditor('val');
                  value = app.htmlText(value);//.replace(/(<([^>]+)>)/ig, "");
                  return value.length > 1;
              }
          }
    ];
 
    $('#fcForm').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

    //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid='+this.TaskId   +'&refType=t', '100%', '360px', true);
};

    //============================================================ app_tasks_comment

app_tasks_comment = {

    wizardStep: 2,
    TaskId: 0,
    Model: {},
    UInfo: null,
    Control: null,
    load: function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.Model = dataModel;
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TaskId);
        return this;
    },
    grid: function (taskid) {
        var slf = this;

        var nastedsource = {
            datafields: [
                  { name: 'CommentId', type: 'number' },
                  { name: 'CommentDate', type: 'date' },
                  { name: 'CommentText', type: 'string' },
                  { name: 'ReminderDate', type: 'date' },
                  { name: 'Attachment', type: 'string' },
                  { name: 'DisplayName', type: 'string' },
                  { name: 'Task_Id', type: 'number' },
                  { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'CommentId',
            type: 'POST',
            url: '/System/GetTasksCommentGrid',
            data: { 'pid': taskid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        //var nastedAdapter = new $.jqx.dataAdapter(nastedsource, {
        //    loadComplete: function (record) {
        //        $("#hxp-1").text("הערות : " + record.length);
        //    },
        //    loadError: function (jqXHR, status, error) {
        //    },
        //    beforeLoadComplete: function (records) {
        //    }
        //});


        $("#jqxgrid1").jqxGrid({scrollbarsize: 1,
            width: '100%',
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              {
                  text: 'מועד רישום', datafield: 'CommentDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
              },
              { text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center' },
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

              //                  return '<div class="task-comment" style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="tasks_edit_view_comment('+row+')">' + 'הצג' + '</a></div>';
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
    showControl: function (id, option, action) {

        var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_tasks_comment_control("#jqxgrid1-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    add: function () {
        //setTaskButton('comment', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_TaskCommentAdd?pid=" + this.TaskId, "100%", "280px");
        this.showControl(0, 'a');

    },
    edit: function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('comment', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "350px");

            this.showControl(id, 'e');
        }
    },
    remove: function () {
        var id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק הערה ' + id)) {
                app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid1').jqxGrid('source').dataBind();
                });
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

    //============================================================ app_tasks_assign

app_tasks_assign = {
    wizardStep: 3,
    Model: {},
    load: function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.Model = dataModel;
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TaskId);
        return this;
    },
    grid: function (taskid) {
        var slf = this;

        var nastedsource = {
            datafields: [
                  { name: 'AssignId', type: 'number' },
                  { name: 'AssignedBy', type: 'number' },
                  { name: 'AssignedTo', type: 'number' },
                  { name: 'Task_Id', type: 'number' },
                  { name: 'AssignDate', type: 'date' },
                  { name: 'AssignSubject', type: 'string' },
                  { name: 'AssignedByName', type: 'string' },
                  { name: 'AssignedToName', type: 'string' }
            ],
            datatype: "json",
            id: 'AssignId',
            type: 'POST',
            url: '/System/GetTasksAssignGrid',
            data: { 'pid': taskid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        $("#jqxgrid2").jqxGrid({
            width: '100%',
            autoheight: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              { text: 'מועד רישום', datafield: 'AssignDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
              { text: 'עבר מ', datafield: 'AssignedByName', width: 120, cellsalign: 'right', align: 'center' },
              { text: 'עבר ל', datafield: 'AssignedToName', width: 120, cellsalign: 'right', align: 'center' },
              { text: 'נושא', datafield: 'AssignSubject', cellsalign: 'right', align: 'center' }
             //{
             //    text: 'מספר רישום', datafield: 'AssignId', width: 120, cellsalign: 'right', align: 'center',
             //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
             //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
             //    }
             //}
            ]
        });
        //$('#jqxgrid2-add').click(function () {
        //    slf.taskDialog = app_dialog.dialogIframe(app.appPath() + "/System/_TaskAssignAdd?id=" + slf.TaskId, "580", "400", "הערות");
        //});
        //$('#jqxgrid2-refresh').click(function () {
        //    $('#jqxgrid1').jqxGrid('source').dataBind();
        //});

        //$('#jqxgrid2').on('rowdoubleclick', function (event) {
        //    slf.view();
        //});
        $('#jqxgrid2-add').click(function () {
            slf.add();
        });

        //$('#jqxgrid2-edit').click(function () {
        //    slf.view();
        //});

        $('#jqxgrid2-remove').click(function () {
            slf.remove();
        });
        $('#jqxgrid2-refresh').click(function () {
            slf.refresh();
        });

        

    },
    UInfo: null,
    Control: null,
    showControl: function (id, option, action) {

        var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_tasks_assign_control("#jqxgrid2-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    },
    getrowId:function(){

        var selectedrowindex = $("#jqxgrid2").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid2").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    add: function () {
        //setTaskButton('assign', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskAssignAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid2-window", app.appPath() + "/System/_TaskAssignAdd?pid=" + this.TaskId, "100%", "280px");
        this.showControl(0, 'a');
    },
    edit: function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('assign', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskAssignEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid2-window", app.appPath() + "/System/_TaskAssignEdit?id=" + id, "100%", "280px");
            this.showControl(id, 'e');
        }
    },
    //view: function () {
    //    var id = this.getrowId();
    //    if (id > 0) {
    //        $('#task-item-update').hide();
    //        wizard.appendIframe(2, app.appPath() + "/System/_TaskAssignView?id=" + id, "100%", "500px");
    //    }
    //},
    remove: function () {
        var id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק העברה ' + id)) {
                app_query.doPost(app.appPath() + "/System/TaskAssignDelete", { 'id': id }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid2').jqxGrid('source').dataBind();
                });

                
            }
        }
    },
    refresh: function () {
        $('#jqxgrid2').jqxGrid('source').dataBind();
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
}

//============================================================ app_tasks_timer

app_tasks_timer = {
    wizardStep: 4,
    Model: {},
    load: function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.Model = dataModel;
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TaskId);
        return this;
    },
    grid: function (taskid) {
        var slf = this;
        var nastedsource = {
            datafields: [
                  { name: 'TaskTimerId', type: 'number' },
                  { name: 'SubIndex', type: 'number' },
                  { name: 'Duration', type: 'number' },
                  { name: 'DurationView', type: 'string' },
                  { name: 'Task_Id', type: 'number' },
                  { name: 'StartTime', type: 'date' },
                  { name: 'EndTime', type: 'date' },
                  { name: 'Subject', type: 'string' },
                  { name: 'UserId', type: 'number' },
                  { name: 'DisplayName', type: 'string' }
            ],
            datatype: "json",
            id: 'TaskTimerId',
            type: 'POST',
            url: '/System/GetTasksTimerGrid',
            data: { 'pid': taskid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
        //var nastedAdapter = new $.jqx.dataAdapter(nastedsource, {
        //    loadComplete: function (record) {
        //        $("#hxp-3").text("מעקב זמנים : " + record.length);
        //    },
        //    loadError: function (jqXHR, status, error) {
        //    },
        //    beforeLoadComplete: function (records) {
        //    }
        //});

        $("#jqxgrid3").jqxGrid({
            width: '100%',
            autoheight: true,
            //showstatusbar: true,
            //statusbarheight: 50,
            //showaggregates: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              { text: 'מס-סידורי', datafield: 'SubIndex', width: 120, cellsalign: 'right', align: 'center' },
              { text: 'מועד התחלה', datafield: 'StartTime', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
              { text: 'מועד סיום', datafield: 'EndTime', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
              {
                  text: 'משך זמן', datafield: 'DurationView', width: 120, cellsalign: 'right', align: 'center'
                  //aggregates: ['sum']
              },
              { text: 'נושא', datafield: 'Subject', cellsalign: 'right', align: 'center' }
             //{
             //    text: 'מספר רישום', datafield: 'TaskTimerId', width: 120, cellsalign: 'right', align: 'center',
             //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
             //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
             //    }
             //}
            ]
        });
        $('#jqxgrid3').on('rowdoubleclick', function (event) {
            slf.edit();
        });
        $('#jqxgrid3-add').click(function () {
            slf.add();
        });

        $('#jqxgrid3-edit').click(function () {
            slf.edit();
        });

        $('#jqxgrid3-remove').click(function () {
            slf.remove();
        });
        $('#jqxgrid3-refresh').click(function () {
            slf.refresh();
        });

        $('#task-timer-update').click(function () {
            var iframe = wizard.getIframe();
            if (iframe && iframe.timer_def) {
                iframe.timer_def.doSubmit();
            }
        });
        $('#task-timer-cancel').click(function () {
            wizard.wizHome();
        });

        //$('#jqxgrid3-add').click(function () {
        //    slf.taskDialog = app_dialog.dialogIframe(app.appPath() + "/System/_TaskTimerAdd?id=" + slf.TaskId, "580", "400", "מעקב זמנים");
        //});
        //$('#jqxgrid3-refresh').click(function () {
        //    $('#jqxgrid3').jqxGrid('source').dataBind();
        //});
    },
    getrowId: function () {
        var selectedrowindex = $("#jqxgrid3").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid3").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    UInfo: null,
    Control: null,
    showControl: function (id, option, action) {

        var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_tasks_timer_control("#jqxgrid3-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    },
    add: function () {
        //setTaskButton('timer', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskTimerAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid3-window", app.appPath() + "/System/_TaskTimerAdd?pid=" + this.TaskId, "100%", "280px");
        this.showControl(0, 'a');
    },
    edit: function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('timer', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskTimerEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid3-window", app.appPath() + "/System/_TaskTimerEdit?id=" + id, "100%", "450px" );
            this.showControl(id, 'e');
        }
    },
    remove: function () {
        var id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק מעקב זמנים ' + id)) {
                app_query.doPost(app.appPath() + "/System/TaskTimerDelete", { 'id': id }, function (data) {
                    if(data.Status>0)
                        $('#jqxgrid3').jqxGrid('source').dataBind();

                });
            }
        }
    },
    refresh: function () {
        $('#jqxgrid3').jqxGrid('source').dataBind();
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

}

    //============================================================ app_tasks_form

app_tasks_form = {

    wizardStep: 2,
    TaskId: 0,
    UserId:0,
    Model: {},
    load: function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.UserId = userInfo.UserId;
        this.Model = dataModel;
        this.UInfo=userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TaskId);
        return this;
    },
    grid: function (taskid) {
        var slf = this;

        var nastedsource = {
            datafields: [
                  { name: 'ItemId', type: 'number' },
                  { name: 'ItemDate', type: 'date' },
                  { name: 'ItemText', type: 'string' },
                  { name: 'DoneDate', type: 'date' },
                  { name: 'DoneStatus', type: 'bool' },
                  { name: 'DisplayName', type: 'string' },
                  { name: 'Task_Id', type: 'number' },
                  { name: 'AssignBy', type: 'number' },
                  { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetTasksFormGrid',
            data: { 'pid': taskid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        $("#jqxgrid4").jqxGrid({
            width: '100%',
            editable: slf.Model.Option == "e",
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              { text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false },
              { text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center', editable: false },
              { text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false },
              { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center' },
               //{ text: 'Product', columntype: 'dropdownlist', datafield: 'productname', width: 195 },
              { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center', editable: false }
              //{
              //    text: '...', datafield: 'ItemId', width: 120, cellsalign: 'right', align: 'center',
              //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
              //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
              //    }
              //}
            ]
        });
        $("#jqxgrid4").on('cellvaluechanged', function (event) {
            // event arguments.
            var args = event.args;
            // column data field.
            var datafield = event.args.datafield;
            // row's bound index.
            var rowBoundIndex = args.rowindex;
            // new cell value.
            var value = args.newvalue;
            // old cell value.
            var oldvalue = args.oldvalue;

            var id = args.owner.rows.records[args.rowindex].bounddata.ItemId;
            slf.update(id,value);

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
        var data = $('#jqxgrid4').jqxGrid('getrowdata', selectedrowindex);
        return data;
    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid4").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    UInfo: null,
    Control: null,
    showControl: function (id, option, action) {

        var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_tasks_form_control("#jqxgrid4-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    },
    add: function () {
        //setTaskButton('form', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskFormAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormAdd?pid=" + this.TaskId, "100%", "220px", true);
        this.showControl(0, 'a');
    },
    edit: function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0){
            //setTaskButton('form', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskFormEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormEdit?id=" + id, "100%", "480px", true);
            this.showControl(id, 'e');
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
                app_query.doPost(app.appPath() + "/System/TaskFormDelete", { 'id': id }, function (data) {
                    if(data.Status>0)
                        $('#jqxgrid4').jqxGrid('source').dataBind();
                });
                
            }
        }
    },
    update: function (id, value) {

        $.ajax({
            url: '/System/TaskFormChecked',
            type: 'post',
            dataType: 'json',
            data: { 'id': id, 'done': value },
            success: function (data) {
                if (data.Status > 0) {
                    window.parent.triggerSubTaskCompleted('form', data);
                }
                else
                    app_messenger.Post(data, 'error');
            },
            error: function (jqXHR, status, error) {
                app_messenger.Post(error, 'error');
            }
        });
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

//============================================================ app_tasks_comment_control

var app_tasks_comment_control = function (tagWindow) {
    this.comment_sync,
    this.init = function (dataModel, userInfo) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";

        var html = '<div id="comment-Window" class="container" style="margin:5px">' +
                          '<hr style="width:100%;border:solid 1px #15C8D8">' +
                          '<div id="comment-Header">' +
                              '<span id="comment-Title" style="font-weight: bold;">מעקב טיפול</span>' +
                          '</div>' +
                          '<div id="comment-Body">' +
                              '<form class="fcForm" id="comment-Form" method="post" action="/System/TaskCommentUpdate">' +
                                  '<div style="direction: rtl; text-align: right">' +
                                      '<input type="hidden" id="CommentId" name="CommentId" value="0" />' +
                                      '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                      '<input type="hidden" id="UserId" name="UserId" value="" />' +
                                      '<div style="height:5px"></div>' +
                                      '<div id="tab-content" class="tab-content" dir="rtl">' +
                                            '<div class="form-group">' +
                                              '<div class="field">הערה:</div>' +
                                              '<textarea id="CommentText" name="CommentText" class="text-content-mid"></textarea>' +
                                            '</div>' +
                                            '<div class="form-group">' +
                                                '<div class="field">תזכורת ל:</div>' +
                                                '<div id="ReminderDate" name="ReminderDate"></div>' +
                                            '</div>' +
                                            '<div class="form-group' + pasive + '">' +
                                                '<div class="field">נוצר ב:</div>' +
                                                '<input id="CommentDate" name="CommentDate" type="text" readonly="readonly" class="text-mid" />' +
                                            '</div>' +
                                            '<div class="form-group' + pasive + '">' +
                                                '<div class="field">נוצר ע"י":</div>' +
                                                '<input id="DisplayName" name="DisplayName" type="text" readonly="readonly" class="text-mid" />' +
                                            '</div>' +
                                    '</div>' +
                                    '<div>' +
                                        '<a id="comment-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
                                        '<a id="comment-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
                                    '</div>' +
                                '</div>' +
                            '</form>' +
                        '</div>' +
                    '</div>';

        $(tagWindow).html(html).hide();

        var slf = this;
        $('#comment-Submit').on('click', function (e) {
            e.preventDefault();
            slf.comment_sync.doSubmit();
        });
        $('#comment-Cancel').on('click', function (e) {
            slf.comment_sync.doCancel();
        });
        if (this.comment_sync == null)
            this.comment_sync = new app_tasks_comment_sync();

        this.comment_sync.init(dataModel, userInfo);
        this.comment_sync.load();

    },
    this.display = function () {
        $(tagWindow).show();
    }
};
var app_tasks_comment_sync = function (dataModel, userInfo) {

        var slf = this;

        this.init = function (dataModel, userInfo) {
            this.TaskId = dataModel.PId;
            this.CommentId = dataModel.Id || 0;
            this.AccountId = userInfo.AccountId;
            this.UserInfo = userInfo;
            this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;

            $('#ReminderDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
            $('#ReminderDate').val('');

            var input_rules = [
              { input: '#CommentText', message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: 'required' },
            ];

            $('#comment-Form').jqxValidator({
                rtl: true,
                hintType: 'label',
                animationDuration: 0,
                rules: input_rules
            });
        },
        this.load = function () {
            if (this.CommentId > 0) {
                if (this.viewAdapter == null) {
                    var view_source =
                    {
                        datatype: "json",
                        id: 'CommentId',
                        data: { 'taskid': slf.TaskId, 'id': slf.CommentId },
                        type: 'POST',
                        url: '/System/GetTaskCommentEdit'
                    };

                    this.viewAdapter = new $.jqx.dataAdapter(view_source, {
                        loadComplete: function (record) {
                            app_jqxform.loadDataForm("comment-Form", record);
                            if (record.CommentDate > 0)
                                $('#CommentDate').val(record.CommentDate.toLocaleDateString());
                        },
                        loadError: function (jqXHR, status, error) {
                        },
                        beforeLoadComplete: function (records) {
                        }
                    });
                }
                else {
                    this.viewAdapter._source.data = { 'taskid': slf.TaskId, 'id': slf.CommentId };
                }
                this.viewAdapter.dataBind();
            }
            else {
                $('#Task_Id').val(this.TaskId);
                $('#UserId').val(this.UserInfo.UserId);

            }
        },

        this.doCancel = function () {

            window.parent.triggerSubTaskCompleted('comment');
        },

        this.doSubmit = function () {
            //e.preventDefault();
            var actionurl = $('#comment-Form').attr('action');
            var data = $('#comment-Form').serialize();// app.serialize('#comment-Form');// 
            
            var validationResult = function (isValid) {
                if (isValid) {
                    $.ajax({
                        url: actionurl,
                        type: 'post',
                        dataType: 'json',
                        data: data,
                        success: function (data) {
    
                            if (data.Status > 0) {
                                window.parent.triggerSubTaskCompleted('comment',data);
    
                                //if (slf.IsDialog) {
                                //    window.parent.triggerCommentCompleted(data.OutputId);
                                //    //$('#fcForm').reset();
                                //}
                                //else {
                                //    app.refresh();
                                //}
                                //$('#TaskId').val(data.OutputId);
                            }
                            else
                                app_messenger.Post(data,'error');
                        },
                        error: function (jqXHR, status, error) {
                            app_messenger.Post(error, 'error'); 
                        }
                    });
                }
            }
           
            $('#comment-Form').jqxValidator('validate', validationResult);

        };
        if (dataModel !== undefined) {
            this.init(dataModel, userInfo);
            this.load();
        }
    }

    //============================================================ app_tasks_assign_control

var app_tasks_assign_control = function (tagWindow) {
    this.assign_sync,
    this.init = function (dataModel, userInfo) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";

        var html = '<div id="assign-Window" class="container" style="margin:5px">' +
                    '<hr style="width:100%;border:solid 1px #15C8D8">' +
                    '<div id="assign-Header">' +
                        '<span id="assign-Title" style="font-weight: bold;">העברות</span>' +
                    '</div>' +
                    '<div id="assign-Body">' +
                        '<form class="fcForm" id="assign-Form" method="post" action="/System/TaskAssignAdd">' +
                            '<div style="direction: rtl; text-align: right">' +
                                '<input type="hidden" id="AssignId" name="AssignId" value="0" />' +
                                '<input type="hidden" id="AssignedBy" name="AssignedBy" value="0" />' +
                                '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                '<input type="hidden" id="UserId" name="UserId" value="" />' +
                                '<div style="height:5px"></div>' +
                                '<div id="tab-content" class="tab-content" dir="rtl">' +
                                    '<div class="form-group' + pasive + '>' +
                                        '<div class="field">העבר אל:</div>' +
                                        '<div id="AssignedTo" name="AssignedTo"></div>' +
                                    '</div>' +
                                    '<div class="form-group">' +
                                        '<div class="field">נושא:</div>' +
                                        '<textarea id="AssignSubject" name="AssignSubject" class="text-content-mid"></textarea>' +
                                    '</div>'+
                                    '<div class="form-group' + pasive + '">'+
                                        '<div class="field">הועבר מ:</div>' +
                                        '<textarea id="AssignedByName" name="AssignedByName" class="text-content-mid"></textarea>'+
                                    '</div>'+
                                    '<div class="form-group' + pasive + '" : null)">' +
                                        '<div class="field">הועבר אל:</div>' +
                                        '<textarea id="AssignedToName" name="AssignedToName" class="text-content-mid"></textarea>' +
                                    '</div>' +
                                    '<div class="form-group' + pasive + '">' +
                                        '<div class="field">מועד העברה:</div>' +
                                        '<div id="AssignDate" name="AsignDate"></div>' +
                                    '</div>' +
                            '</div>' +
                        '<div>' +
                            '<a id="assign-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
                            '<a id="assign-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
                        '</div>' +
                     '</div>' +
                 '</form>' +
            '</div>' +
         '</div>';


        $(tagWindow).html(html).hide();

        var slf = this;
        $('#assign-Submit').on('click', function (e) {
            e.preventDefault();
            slf.assign_sync.doSubmit();
        });
        $('#assign-Cancel').on('click', function (e) {
            slf.assign_sync.doCancel();
        });
        if (this.assign_sync == null)
            this.assign_sync = new app_tasks_assign_sync();

        this.assign_sync.init(dataModel, userInfo);
        this.assign_sync.load();

    },
    this.display = function () {
        $(tagWindow).show();
    }
};
var app_tasks_assign_sync = function (dataModel, userInfo) {

    var slf = this;

    this.init = function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.AssignId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;

        //$("#AccountId").val(this.AccountId);

        app_jqxcombos.createComboAdapter("User_Id", "DisplayName", "AssignedTo", '/System/GetUsersInTeamList', 0, 120, false);

        var input_rules = [
                 {
                     input: "#AssignedTo", message: 'חובה לציין העברה אל!', action: 'select', rule: function (input, commit) {
                         var index = $("#AssignedTo").jqxComboBox('getSelectedIndex');
                         return index >= 0;
                     }
                 },
                 { input: '#AssignSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
        ];

        $('#assign-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    },
    this.load = function () {
        if (this.AssignId > 0) {
            if (this.viewAdapter == null) {
                var view_source =
                {
                    datatype: "json",
                    id: 'AssignId',
                    data: { 'id': slf.AssignId },
                    type: 'POST',
                    url: '/System/GetTaskAssignEdit'
                };

                 this.viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("assign-Form", record);
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            }
            else {
                this.viewAdapter._source.data = { 'id': slf.AssignId };
            }
            this.viewAdapter.dataBind();
        }
        else {
            $("#assign-Form input[name=Task_Id]").val(this.TaskId);
            $("#assign-Form input[name=UserId]").val(this.UserInfo.UserId);
        }
    },

    this.doCancel = function () {

        window.parent.triggerSubTaskCompleted('assign');
    },

    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#assign-Form').attr('action');
        //var AssignedTo = $('#AssignedTo').val();
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#assign-Form').serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            window.parent.triggerSubTaskCompleted('assign', data);
                        }
                        else
                            app_messenger.Post(data, 'error');
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Post(error, 'error');
                    }
                });
            }
        }
        $('#assign-Form').jqxValidator('validate', validationResult);
    };
    if (dataModel !== undefined) {
        this.init(dataModel, userInfo);
        this.load();
    }
}

    //============================================================ app_tasks_timer_control

var app_tasks_timer_control = function (tagWindow) {
    this.timer_sync,
    this.init = function (dataModel, userInfo) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";

        var html = '<div id="timer-Window" class="container" style="margin:5px">' +
                    '<hr style="width:100%;border:solid 1px #15C8D8">' +
                    '<div id="timer-Header">' +
                        '<span id="timer-Title" style="font-weight: bold;">מעקב זמנים</span>' +
                    '</div>' +
                    '<div id="timer-Body">'+
                        '<form class="fcForm" id="timer-Form" method="post" action="/System/TaskTimerUpdate">'+
                            '<div style="direction: rtl; text-align: right">'+
                                '<input type="hidden" id="TaskTimerId" name="TaskTimerId" value="0" />'+
                                '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />'+
                                '<input type="hidden" id="UserId" name="UserId" value="" />' +
                                '<div style="height:5px"></div>'+
                                '<div id="tab-content" class="tab-content" dir="rtl">'+
                                    '<div class="form-group' + pasive + '">'+
                                        '<div class="field">סדר:</div>'+
                                       '<input id="SubIndex" name="SubIndex" type="number" readonly="readonly" class="text-mid" />'+
                                    '</div>'+
                                    '<div class="form-group">'+
                                        '<div class="field">נושא:</div>'+
                                        '<textarea id="Subject" name="Subject" class="text-content-mid">' + '</textarea>' +
                                    '</div>'+
                                    '<div class="form-group' + pasive + '">'+
                                        '<div class="field">מועד התחלה:</div>'+
                                        '<div id="StartTime" name="StartTime">' + '</div>' +
                                    '</div>'+
                                    '<div id="divEndTime" class="form-group' + pasive + '">'+
                                         '<div class="field">מועד סיום:</div>'+
                                         '<div id="EndTime" name="EndTime">' + '</div>' +
                                    '</div>'+
                                    '<div id="divDuration" class="form-group' + pasive + '">'+
                                         '<div class="field">משך זמן:</div>'+
                                         '<input id="Duration" name="Duration" type="number" readonly="readonly" class="text-mid" />'+
                                    '</div>'+
                                    '<div class="form-group' + pasive + '">'+
                                         '<div class="field">נוצר ע"י:</div>'+
                                         '<input id="DisplayName" name="DisplayName" type="text" readonly="readonly" class="text-mid" />' +
                                    '</div>'+
                               '</div>'+
                               '<div>'+
                                    '<a id="timer-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> '+
                                    '<a id="timer-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>'+
                              '</div>'+
                       '</div>'+
              '</form>'+
           '</div>'+
        '</div>';

        $(tagWindow).html(html).hide();

        var slf = this;
        $('#timer-Submit').on('click', function (e) {
            e.preventDefault();
            slf.timer_sync.doSubmit();
        });
        $('#timer-Cancel').on('click', function (e) {
            slf.timer_sync.doCancel();
        });
        if (this.timer_sync == null)
            this.timer_sync = new app_tasks_timer_sync();

        this.timer_sync.init(dataModel, userInfo);
        this.timer_sync.load();

    },
    this.display = function () {
        $(tagWindow).show();
    }
};
var app_tasks_timer_sync = function (dataModel, userInfo) {

    var slf = this;

    this.init = function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.TaskTimerId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        $('#StartTime').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, formatString: "dd/MM/yyyy hh:mm", disabled: true });
        $('#StartTime').val('');
        $('#EndTime').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, formatString: "dd/MM/yyyy hh:mm", disabled: true });
        $('#EndTime').val('');

        var input_rules = [
              { input: '#Subject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
        ];

        $('#timer-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    },
    this.load = function () {
        if (this.TaskTimerId > 0) {
            if (this.viewAdapter == null) {
                var view_source =
                {
                    datatype: "json",
                    id: 'TaskTimerId',
                    data: { 'id': slf.TaskTimerId },
                    type: 'POST',
                    url: '/System/GetTaskTimerEdit'
                };

                this.viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("timer-Form", record);
                        $("#timer-Form input[id=DisplayName]").val(record.DisplayName);
                             //$('#StartTime').val(record.StartTime);
                            if (app.isNull(record.EndTime, "") == "") {
                                $('#divEndTime').hide();
                                $('#divDuration').hide();
                                $('#timer-Submit').text("סיום");
                            }
                            //else {
                            //    $('#timer-Submit').hide();
                            //    $('#timer-form > #Subject').prop('readonly', true);
                            //    var sub= app_jqxform.findInputInForm ("timer-Form", "Subject");//, attrName, inputType) {
                            //    sub.prop('readonly', true);
                            //}
                        
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            }
            else {
                this.viewAdapter._source.data = { 'id': slf.TaskTimerId };
            }
            this.viewAdapter.dataBind();
        }
        else {
            $("#timer-Form input[name=Task_Id]").val(this.TaskId);
            $("#timer-Form input[name=UserId]").val(this.UserInfo.UserId);
            $('#timer-Submit').text("התחל");

        }
    },
    this.doCancel = function () {

        window.parent.triggerSubTaskCompleted('timer');
    },

    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#timer-Form').attr('action');
        //var AssignedTo = $('#AssignedTo').val();
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#timer-Form').serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            window.parent.triggerSubTaskCompleted('timer', data);
                        }
                        else
                            app_messenger.Post(data, 'error');
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Post(error, 'error');
                    }
                });
            }
        }
        $('#timer-Form').jqxValidator('validate', validationResult);
    };
    if (dataModel !== undefined) {
        this.init(dataModel, userInfo);
        this.load();
    }
}

    //============================================================ app_tasks_form_control

var app_tasks_form_control = function (tagWindow) {
    this.form_sync,
    this.init = function (dataModel, userInfo) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";

        var html = '<div id="form-Window" class="container" style="margin:5px">' +
                    '<hr style="width:100%;border:solid 1px #15C8D8">' +
                    '<div id="form-Header">' +
                        '<span id="form-Title" style="font-weight: bold;">פעולות לביצוע</span>' +
                    '</div>' +
                    '<div id="form-Body">' +
                        '<form class="fcForm" id="form-Form" method="post" action="/System/TaskFormUpdate">' +
                            '<div style="direction: rtl; text-align: right">' +
                                '<input type="hidden" id="ItemId" name="ItemId" value="0" />' +
                                '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                '<input type="hidden" id="form-UserId" name="UserId" value="" />' +
                                '<input type="hidden" id="AssignBy" name="AssignBy" value="" />' +
                                '<div style="height:5px"></div>' +
                                '<div id="tab-content" class="tab-content" dir="rtl">' +
                                    '<div id="fcTitle" style="font-weight: bold;">לביצוע</div>' +
                                    '<div class="form-group">' +
                                        '<div class="field">נושא:</div>' +
                                        '<textarea id="ItemText" name="ItemText" class="text-content"' + (dataModel.Option != "a" ? " readonly=\"true\"" : "") + '></textarea>' +
                                    '</div>' +
                                    '<div class="form-group' + (dataModel.Id == 0 ? " pasive" : "") + '">' +
                                        '<div class="form-group">' +
                                            '<div class="field">נוצר בתאריך:</div>' +
                                            '<input id="ItemDate" name="ItemDate" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                        '<div id="fcTitle" style="font-weight: bold;">בוצע</div>' +
                                        '<div id="divDoneDate" class="form-group">' +
                                            '<div class="field">מועד סיום:</div>' +
                                            '<input id="DoneDate" name="DoneDate" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                        '<div class="form-group">' +
                                            '<div class="field">הערות ביצוע:</div>' +
                                            '<textarea id="DoneComment" name="DoneComment" class="text-content"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '></textarea>' +
                                        '</div>' +
                                        '<div class="form-group">' +
                                            '<div class="field">' +
                                                'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option != "e" ? " disabled=\"true\"" : "") + '/>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div id="divDisplayName" class="form-group">' +
                                            '<div class="field">בוצע ע"י:</div>' +
                                            '<input id="form-DisplayName" name="DisplayName" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                    '</div>' +
                                '</div>' +
                                '<div>' +
                                    '<a id="form-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
                                    '<a id="form-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
                                '</div>' +
                            '</div>' +
                        '</form>' +
                    '</div>' +
        '</div>';

        $(tagWindow).html(html).hide();

        var slf = this;
        $('#form-Submit').on('click', function (e) {
            e.preventDefault();
            slf.form_sync.doSubmit();
        });
        $('#form-Cancel').on('click', function (e) {
            slf.form_sync.doCancel();
        });
        if (this.form_sync == null)
            this.form_sync = new app_tasks_form_sync();

        this.form_sync.init(dataModel, userInfo);
        this.form_sync.load();

    },
    this.display = function () {
        $(tagWindow).show();
    }
};
var app_tasks_form_sync = function (dataModel, userInfo) {

    var slf = this;

    this.init = function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        var input_rules = [
             { input: '#ItemText', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
        ];

        $('#form-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    },
    this.load = function () {
        if (this.ItemId > 0) {
            if (this.viewAdapter == null) {
                var view_source =
                {
                    datatype: "json",
                    id: 'ItemId',
                    data: { 'id': slf.ItemId },
                    type: 'POST',
                    url: '/System/GetTaskFormEdit'
                };

                this.viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("form-Form", record);
                       // $("form#form-Form [name=UserId]").val(record.UserId);
                                                
                        //$('#StartTime').val(record['StartTime']);
                        $("#ItemDate").val(app.toLocalDateString(record.ItemDate));
                        $("#DoneDate").val(app.toLocalDateString(record.DoneDate));
                        if (record.DoneStatus == false) {
                            $("#divDoneDate").hide();
                            $("#divDisplayName").hide();
                        }
                        else {
                            $("#DoneStatus").prop('disabled', true)
                            $("#DoneComment").prop('readonly', true)
                            $("#form-Submit").hide();
                        }
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            }
            else {
                this.viewAdapter._source.data = { 'id': slf.ItemId };
            }
            this.viewAdapter.dataBind();
        }
        else {
            $("#form-Form input[name=Task_Id]").val(this.TaskId);
            $("#form-Form input[name=UserId]").val(this.UserInfo.UserId);
            //$('#form-Submit').text("התחל");

        }
    },
    this.doCancel = function () {

        window.parent.triggerSubTaskCompleted('form');
    },

    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#form-Form').attr('action');
        //var AssignedTo = $('#AssignedTo').val();
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#form-Form').serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            window.parent.triggerSubTaskCompleted('form', data);
                        }
                        else
                            app_messenger.Post(data, 'error');
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Post(error, 'error');
                    }
                });
            }
        }
        $('#form-Form').jqxValidator('validate', validationResult);
    };
    if (dataModel !== undefined) {
        this.init(dataModel, userInfo);
        this.load();
    }
}

})(jQuery)




//============================================================ app_tasks_files
//app_tasks_files = {

//    app_iframe.attachIframe ('task-files', '/Media/_MediaFiles', '100%', '300px', true);
//}