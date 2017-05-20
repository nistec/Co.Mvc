'use strict';
//============================================================ app_tasks_comment

/*
function app_tasks_comment_def(dataModel, userInfo) {

    //this.TaskId =  dataModel.PId;
    //this.CommentId =  dataModel.Id;
    //this.AccountId =  userInfo.AccountId;
    //this.UserInfo =  userInfo.UserRole;
    //this.AllowEdit =  (this.UserRole > 4) ? 1 : 0;

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
        
        $('#comment-Form').jqxValidator('validate',validationResult);
    };
    if (dataModel !== undefined) {
        this.init(dataModel, userInfo);
        this.load();
    }
};
*/

//============================================================ app_tasks_assign

function app_tasks_assign_def(dataModel, userInfo) {
    this.TaskId = dataModel.PId;
    this.AssignId = dataModel.Id;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

    $("#AccountId").val(this.AccountId);

    app_jqxcombos.createComboAdapter("User_Id", "DisplayName", "AssignedTo", '/System/GetUsersInTeamList', 0, 120, false);

    //$('#DueDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
    //$('#DueDate').val('');


    //app_jqx_list.taskTypeComboAdapter();
    //app_jqx_list.taskStatusComboAdapter();

    var input_rules = [
         {
             input: "#AssignedTo", message: 'חובה לציין העברה אל!', action: 'select', rule: function (input, commit) {
                 var index = $("#AssignedTo").jqxComboBox('getSelectedIndex');
                 return index >= 0;
             }
         },
         { input: '#AssignSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
    ];

    $('#fcForm').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });


    this.doCancel = function () {
        //parent.taskEnd(false);
        window.parent.triggerSubTaskCompleted('assign');
    }
    
    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var AssignedTo = $('#AssignedTo').val();
        //$('#UserId_AssignedTo').val(AssignedTo);

        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#fcForm').serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            window.parent.triggerSubTaskCompleted('assign',data);
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
        $('#fcForm').jqxValidator('validate', validationResult);
    };

    var slf = this;

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
            app_jqxform.loadDataForm("fcForm", record);
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    if (this.AssignId > 0) {
        this.viewAdapter.dataBind();
    }
    else {
        $('#Task_Id').val(this.TaskId);
        $('#UserId').val(userInfo.UserId);

    }
};


//============================================================ app_tasks_timer


function app_tasks_timer_def(dataModel, userInfo) {
    this.TaskId = dataModel.PId;
    this.TaskTimerId = dataModel.Id;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

    $("#AccountId").val(this.AccountId);

    $('#StartTime').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, formatString: "dd/MM/yyyy hh:mm", disabled: true });
    $('#StartTime').val('');
    $('#EndTime').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, formatString: "dd/MM/yyyy hh:mm", disabled: true });
    $('#EndTime').val('');

    var slf = this;

    var input_rules = [
          { input: '#Subject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
    ];

    $('#fcForm').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });


    //app_jqx_list.taskTypeComboAdapter();
    //app_jqx_list.taskStatusComboAdapter();

    //var input_rules = [
    //      { input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
    //      { input: '#TaskBody', message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: 'required' },
    //];

    //$('#fcForm').jqxValidator({
    //    rtl: true,
    //    hintType: 'label',
    //    animationDuration: 0,
    //    rules: input_rules
    //});


    this.doCancel = function () {
        //parent.taskEnd(false);
        window.parent.triggerSubTaskCompleted('timer');
    }

    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');

        //if (slf.TaskTimerId == 0) {
        //    //var d = new Date();
        //    //d.toISOString();       // "2013-12-08T17:55:38.130Z"
        //    //d.toLocaleDateString() // "8/12/2013" on my system
        //    //d.toLocaleString()     // "8/12/2013 18.55.38" on my system
        //    //d.toUTCString()        // "Sun, 08 Dec 2013 17:55:38 GMT"

        //    $("#StartTime").val(d.toISOString());
        //}
        //else {
        //    $("#EndTime").val(d.toISOString());

        //}


        

        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#fcForm').serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            window.parent.triggerSubTaskCompleted('timer',data);
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
        $('#fcForm').jqxValidator('validate', validationResult);
    };

  
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
            app_jqxform.loadDataForm("fcForm", record);
            if (record.TaskTimerId > 0) {
                if (app.isNull(record.EndTime, "") == "") {
                    $('#divEndTime').hide();
                    $('#divDuration').hide();
                    $('#fcSubmit').text("סיום");
                }
                else {
                    $('#fcSubmit').hide();
                    $('#Subject').prop('readonly',true);
                    
                }
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    if (this.TaskTimerId > 0) {
        this.viewAdapter.dataBind();
    }
    else {
        $('#Task_Id').val(this.TaskId);
        $('#UserId').val(userInfo.UserId);
        $('#fcSubmit').text("התחל");
     }
};


//============================================================ app_tasks_form


function app_tasks_form_def(dataModel, userInfo) {
    this.TaskId = dataModel.PId;
    this.ItemId = dataModel.Id;
    this.UserId = userInfo.UserId;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

    $("#AccountId").val(this.AccountId);

    //$('#DoneDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
    //$('#DoneDate').val('');
    //$('#ItemDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
    //$('#ItemDate').val('');

    this.doCancel = function () {
        //parent.taskEnd(false);
        window.parent.triggerSubTaskCompleted('form');
    }

    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');

        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#fcForm').serialize(),
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
        $('#fcForm').jqxValidator('validate', validationResult);
    };

    var slf = this;

    var input_rules = [
         { input: '#ItemText', message: 'חובה לציין נןשא!', action: 'keyup, blur', rule: 'required' },
    ];

    $('#fcForm').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });


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
            app_jqxform.loadDataForm("fcForm", record);
            $("#ItemDate").val(app.toLocalDateString(record.ItemDate));
            $("#DoneDate").val(app.toLocalDateString(record.DoneDate));
            if (record.DoneStatus == false) {
                $("#divDoneDate").hide();
                $("#divDisplayName").hide();
            }
            else {
                $("#DoneStatus").prop('disabled', true)
                $("#DoneComment").prop('readonly', true)
                $("#fcSubmit").hide();
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    if (this.ItemId > 0) {
        this.viewAdapter.dataBind();
    }
    else {
        $('#Task_Id').val(this.TaskId);
        $('#UserId').val(this.UserId);

    }
};
/*
app_task_Comment_def=function(tagWindow)
{
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
            slf.comment_def.doSubmit();
        });
        $('#comment-Cancel').on('click', function (e) {
            slf.comment_def.doCancel();
        });
        if (this.comment_sync == null)
            this.comment_sync = new app_tasks_comment_def.sync();
        
        this.comment_sync.init(dataModel, userInfo);
        this.comment_sync.load();
        
    },
    this.display=function(){
        $(tagWindow).show();
    }
}

app_tasks_comment_def.sync=function(dataModel, userInfo) {

    //this.TaskId =  dataModel.PId;
    //this.CommentId =  dataModel.Id;
    //this.AccountId =  userInfo.AccountId;
    //this.UserInfo =  userInfo.UserRole;
    //this.AllowEdit =  (this.UserRole > 4) ? 1 : 0;

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
};

app_task_Comment_def.prototype.control=function () {

    wizardStep= 2,
    TaskId= 0,
    Model= {},
    UInfo=null,
    Dialog=null,
    load= function (dataModel, userInfo) {
        this.TaskId = dataModel.PId;
        this.Model = dataModel;
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(this.TaskId);
        return this;
    },
    grid= function (taskid) {
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
    showDialog=function(id,option,action){

        var data_model = { PId: this.TaskId, Id: id, Option:  option , Action:  action };
        

        if (this.Dialog == null) {
            this.Dialog = new app_task_Comment_control("#jqxgrid1-window");
        }
        this.Dialog.init(data_model, this.UInfo);
        this.Dialog.display();
    },
    getrowId= function () {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    add= function () {
        //setTaskButton('comment', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_TaskCommentAdd?pid=" + this.TaskId, "100%", "280px");
        this.showDialog(0,'a');
        
    },
    edit= function () {
        if (this.Model.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('comment', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "500px");
            
            //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "350px");

            this.showDialog(id,'e');
        }
    },
    remove= function () {
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
    refresh= function () {
        $('#jqxgrid1').jqxGrid('source').dataBind();
    },
    cancel= function () {
        wizard.wizHome();
    },
    end= function (data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
            // app_dialog.alert(data.Message);
        }
    }

};

*/