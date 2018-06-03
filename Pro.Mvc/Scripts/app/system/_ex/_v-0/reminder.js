
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
//============================================================================================ app_task_def
(function ($) {


//var task_def;

app_reminder = {
    init: function (dataModel, userInfo) {
        
        return new app_reminder_def(dataModel, userInfo)
    }
};


function app_reminder_def(dataModel, userInfo) {
    this.TaskId = dataModel.Id;
    this.TaskParentId = dataModel.PId;
    this.TaskModel = 'R';
    this.UserInfo = userInfo;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.Title = 'תזכורת';//(this.TaskModel == 'E') ? 'כרטיס' : 'משימה';
    this.ProjectId =0;
    this.ClientId = 0;
    this.Tags;

    //if (this.TaskModel == 'R')
    //    this.Title = 'תזכורת';
    //else if (this.TaskModel == 'E')
    //    this.Title = 'כרטיס';
    //else //if (this.TaskModel == 'T')
    //    this.Title = 'משימה';

    //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    //this.IsDialog = isdialog;

    $("#AccountId").val(this.AccountId);
    $("#hxp-0").text(this.Title+  ": " + this.TaskId);
    
    var slf = this;

    $("#Task_Parent").val(slf.TaskParentId);
    if (slf.TaskParentId > 0) {
        $("#Task_Parent-group").show();
        $("#Task_Parent-link").click(function () {
            app.redirectTo('/System/TaskInfo?id=' + slf.TaskParentId);
        });
    }
    else {
        $("#Task_Parent-group").hide();
    }


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
    
    this.doSubmit = function (act) {
        //e.preventDefault();

        var status = $("#TaskStatus").val();
        //if (status > 1 && status < 8)
        //{
        //    if (confirm("האם לסיים משימה?") == false)
        //        return;
        //}

        if (act == 'finished')
            $("#TaskStatus").val(16);
        else if(status==0)
            $("#TaskStatus").val(1);

        var value = $("#TaskBody").jqxEditor('val');
        //var clientDetails = $('#ClientDetails').val();
        //, { key: 'ClientDetails', value: clientDetails }
        var AssignTo = app_jqxcombos.getComboCheckedValues("AssignTo");
        var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }, { key: 'AssignTo', value: AssignTo }];
        var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);
        var actionurl = $('#fcForm').attr('action');

        var validationResult = function (isValid) {
            if (isValid) {
                //var formData = app.serialize('#fcForm');
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: formData,
                    success: function (data) {
                        if (slf.TaskId == 0) {
                            slf.TaskId = data.OutputId;
                            $("#TaskId").val(data.OutputId);
                            $("#hxp-0").text('תזכורת: '+data.OutputId);
                            //$(".hxp").show();
                            app_messenger.Notify(data, 'info');
                        }
                        if (act == 'plus') {
                            app.refresh();
                        }
                        else {//if (act == 'finished') {
                            app.redirectTo('/System/TaskUser');
                        }
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Notify(error,'error');
                    }
                });
            }
        }
        $('#fcForm').jqxValidator('validate', validationResult);
    };


    if(this.TaskId>0)
    {
        $("#TaskStatus-group").show();
        this.syncData(dataModel.Result);
    }
    else
    {
        $("#TaskStatus-group").hide();
    }
};

app_reminder_def.prototype.loadControls = function () {

    var slf = this;
    var exp1_Inited = false;
    $('#DueDate').jqxDateTimeInput({ width: '150px', rtl: true });
    $('#DueDate ').jqxDateTimeInput('setMinDate', new Date());
    //$('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, disabled: true });
    //$('#DueDate').val('');

    
    //app_jqx_list.taskTypeComboAdapter();
    app_jqx_list.taskStatusComboAdapter();
    $("#TaskStatus").jqxDropDownList({ enableSelection: false, disabled: true, rtl: true });

    app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
    app_jqxcombos.createComboCheckAdapter("UserTeamId", "DisplayName", "AssignTo", '/System/GetUserTeamList', 350, 0, false);
    app_jqxcombos.createComboAdapter("ProjectId", "ProjectName", "Project_Id", '/System/GetProjectList', 0, 120, false);
    $("#Project_Id").jqxComboBox({ showArrow: false, autoComplete: true });
    $("#RemindPlatform").jqxDropDownList({autoDropDownHeight: true, width:160, rtl:true });
    

    $("#accordion").accordion({ heightStyle: "content", rtl: true, editable: true });
    $("#jqxExp-1").jqxExpander({ rtl: true, width: '80%', expanded: false });
    $('#jqxExp-1').on('expanding', function () {
        if (!exp1_Inited) {

            app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', slf.ClientId, function () {

            });
            app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', slf.ProjectId, function () {

            });
            app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 300, 0, true, slf.Tags, function () {

            });

            //app_jqx_adapter.createComboArrayAsync("Tags", "#Tags", '/System/GetTagsList', null, 300, 0, true, slf.Tags, function () {

            //});

            //var AssignTo = app_jqxcombos.getComboCheckedValues("Tags");

            ////app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
            //app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');
        }

        exp1_Inited = true;
    });

    
    $("#hTitle").css("background-color", "#46d6db");
    //$("#ColorFlag").simplecolorpicker();
    //$("#ColorFlag").on('change', function () {
    //    //$('select').simplecolorpicker('destroy');
    //    var color = $("#ColorFlag").val();
    //    $("#hTitle").css("background-color", color)
    //});
   
   
    //$("#AssignTo").on('change', function (event) {
    //    var args = event.args;
    //    if (args) {
    //        var item = args.item;
    //        if (item.label.charAt(0) == '@') {
    //            $('#UserId').val(0);
    //            $('#TeamId').val(item.value);
    //        }
    //        else {
    //            $('#UserId').val(item.value);
    //            $('#TeamId').val(0);
    //        }

    //    }
    //});
   

    $('#TaskBody').jqxEditor({
        //height: '200px',
        //width: '100%',
        editable: true,//dataModel.Option == 'e',
        rtl: true,
        tools: 'bold italic underline | color background | left center right'
        //theme: 'arctic'
        //stylesheets: ['editor.css']
    });
   

    var input_rules = [
          { input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
           //{ input: '#AssignTo', message: 'חובה לציין משימה עבור!', action: 'keyup, blur', rule: 'required' },
           //{
           //    input: "#AssignTo", message: 'חובה לציין תזכורת עבור!', action: 'select', rule: function (input, commit) {
           //        var index = $("#AssignTo").jqxComboBox('getSelectedIndex');
           //        return index >=0;
           //    }
           //},
          {
              input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'keyup, blur', rule: 'required', rule: function (input, commit) {
                  //var value = $('#DueDate').jqxDateTimeInput('value');
                  var text = $('#DueDate').jqxDateTimeInput('getText');
                  return text!=null && text.length > 0;
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


};

app_reminder_def.prototype.syncData = function (record) {

    var slf = this;
  //  var view_source =
  //{
  //    datatype: "json",
  //    id: 'TaskId',
  //    data: { 'id': slf.TaskId },
  //    type: 'POST',
  //    url: '/System/GetTaskEdit'
  //};

  //  this.viewAdapter = new $.jqx.dataAdapter(view_source, {
  //      loadComplete: function (record) {

  //          slf.loadData (record);
  //      },
  //      loadError: function (jqXHR, status, error) {
  //      },
  //      beforeLoadComplete: function (records) {
  //      }
  //  });

  //  if (this.TaskId > 0) {
  //      this.viewAdapter.dataBind();
  //  }

   

        if (record) {
            if (record.TaskStatus <= 0)
                record.TaskStatus = 1;


            app_jqxform.loadDataForm("fcForm", record, true, ["TaskStatus", "Project_Id", "ClientId"]);
            $("#TaskStatus").val(record.TaskStatus);
            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
            $("#CreatedDate").val(record.CreatedDate);
            $("#StartedDate").val(app.toLocalDateString(record.StartedDate));
            $("#EndedDate").val(app.toLocalDateString(record.EndedDate));

            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle").text(this.Title + ": " + record.TaskSubject);
            //$("#hTitle").css("background-color", (record.ColorFlag || '#46d6db'));

            this.ProjectId = record.Project_Id;
            this.ClientId = record.ClientId;
            this.Tags = record.Tags;

            app.showIf('#fcSubmit', record.TaskStatus < 8);
            app.showIf('#fcFinished', record.TaskStatus >= 1 && record.TaskStatus < 8);

            //if (record.TaskStatus > 1 && record.TaskStatus < 8)
            //    $("#fcSubmit").val("סיום");
            //else
            //    $("#fcSubmit").val("עדכון");

            var editable = false;
            if (record.TaskStatus < 8 && record.AssignBy == record.UserId) {
                editable = true;
            }

            $('#DueDate').jqxDateTimeInput({ disabled: !editable });
            $("#TaskBody").jqxEditor({ editable: editable });

            var align = app_style.langAlign(record.Lang);
            $('#TaskBody').css('text-align', align)

            app_jqxcombos.selectComboCheckBoxValues("AssignTo", record.AssignTo);

            //app_jqxcombos.selectCheckList("listCategory", record.Categories);

            //app_jqxcombos.initComboValue('City', 0);
        }
    };


})(jQuery)

