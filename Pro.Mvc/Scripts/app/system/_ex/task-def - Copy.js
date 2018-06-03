//'use strict';

//============================================================================================ app_task_def

function app_task_demo()
{
    var task = new app_task();
   
    task.doCancel();
    task.comment.end();
    app_task.triggerSubTaskCompleted();
    //app_task_trigger.
}

var app_task = (function () {

    var app_task = function (dataModel, userInfo, taskModel) {

        this.$element = $("#accordion");
        //this.$element = document.querySelectorAll(element);

        //if (!this.$element.is('div')) {
        //    $.error('app_wiztabs should be applied to DIV element');
        //    return;
        //}
        //// ensure to use the `new` operator
        //if (!(this instanceof app_task))
        //    return new app_task(element, TaskId, userInfo, taskModel);

        this.TaskId = dataModel.Id;
        this.TaskParentId = dataModel.PId;
        this.Model = dataModel;
        this.TaskModel = taskModel;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.Title = app_tasks.taskTitle(this.TaskModel);
        this.uploader;
        this.IsNew = (this.TaskId == 0);
        this.IsOwner = (this.Model.UserId == this.UserInfo.UserId);
        this.Option = (dataModel.Option) ? dataModel.Option : 'e';
        this.AssignBy = (dataModel.AssignByion) ? dataModel.AssignBy : 0;
        this.TaskStatus = (dataModel.TaskStatus) ? dataModel.TaskStatus : 0;
        this.IsEditable = (this.TaskId == 0) || ((this.TaskStatus < 8) && (this.AssignBy == this.UserInfo.UserId || this.Option == 'e'));
        this.EnableFormType = this.IsNew;
        this.ClientId = 0;
        this.ProjectId = 0;
        this.Tags;
        this.AssignTo;
        this.SrcUserId=0;
        this.exp1_Inited = false;
        var slf = this;

        this.tabSettings();

        var loadData = function () {

            var view_source =
            {
                datatype: "json",
                id: 'TaskId',
                data: { 'id': slf.TaskId },
                type: 'POST',
                url: '/System/GetTaskEdit'
            };

            var viewAdapter = new $.jqx.dataAdapter(view_source, {
                loadComplete: function (record) {

                    //slf.syncData(record);
                    slf.loadControls(record);
                    slf.loadEvents();
                },
                loadError: function (jqXHR, status, error) {
                    app_dialog.alert("error loading task data");
                },
                beforeLoadComplete: function (records) {
                }
            });

            if (slf.TaskId > 0) {
                viewAdapter.dataBind();
            }
        };

        this.preLoad();

        if (this.TaskId > 0) {
            //this.syncData(dataModel.Data);
            this.loadControls(dataModel.Data);
        }
        else {
            this.loadControls();
        }
        this.loadEvents();

        return this;
    };
      
    app_task.prototype.taskSettings = function (record) {

        this.AssignBy = record.AssignBy;
        this.TaskStatus = record.TaskStatus;
        this.IsEditable = (this.TaskId == 0) || ((this.TaskStatus < 8) && (this.AssignBy == this.UserInfo.UserId || this.Option == 'e'));

        this.ProjectId = record.Project_Id;
        this.ClientId = record.ClientId;
        this.Tags = record.Tags;
        this.AssignTo = record.AssignTo;
        this.SrcUserId = record.UserId;

    }

    app_task.prototype.tabSettings = function () {

        $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
        $("#hxp-title").text(this.Title + ': ' + this.TaskId);

        if (this.TaskId > 0) {
            $("#hxp-1").show();
            $("#hxp-2").show();
            $("#hxp-3").show();
            $("#hxp-4").show();
            $("#hxp-5").show();
        }
        else {
            $("#hxp-1").hide();
            $("#hxp-2").hide();
            $("#hxp-3").hide();
            $("#hxp-4").hide();
            $("#hxp-5").hide();
        }

        if (!this.IsEditable) {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }
    };

    app_task.prototype.sectionSettings = function (id) {

        var slf = this;
        switch (id) {
            case 1:
                if (slf.TaskId == 0) {
                    event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                    return false;
                }
                if ($("#jqxgrid1")[0].childElementCount == 0)
                    slf.comment.load(slf.TaskId, slf.UserInfo, slf.Option);
                break;
            case 2:
                if ($("#jqxgrid2")[0].childElementCount == 0)
                    slf.assign.load(slf.TaskId, slf.UserInfo, slf.Option);
                break;
            case 3:
                if ($("#jqxgrid3")[0].childElementCount == 0)
                    slf.timer.load(slf.TaskId, slf.UserInfo, slf.Option);
                break;
            case 4:
                if (slf.TaskId == 0) {
                    event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                    return false;
                }
                if (slf.EnableFormType) {
                    var source = $('#Form_Type').jqxComboBox('source');
                    if (source == null)
                        app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetTaskFormTypeList', 0, 120, false);
                    if ($("#jqxgrid4")[0].childElementCount == 0)
                        app_task.form_template.load(slf.TaskId, slf.UserInfo);

                }
                if ($("#jqxgrid4")[0].childElementCount == 0)
                    slf.form.load(slf.TaskId, slf.UserInfo, slf.Option);
                break;
            case 5:
                if (slf.TaskId == 0) {
                    event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                    return false;
                }

                if (slf.uploader == null) {
                    slf.uploader = new app_media_uploader("#task-files");
                    slf.uploader.init(slf.TaskId, 't', !slf.IsEditable);
                    slf.uploader.show();
                }

                //if ($("#iframe-files").attr('src') === undefined)
                //    var op = slf.Model.Option;
                //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t&op=' + op, '100%', '350px', true);

                break;
        }
    };

    app_task.prototype.initInfo = function () {
        var model = this.Model.Data
        if (model.Comments == 0)
            $("#exp-1").hide();
        if (model.Assigns == 0)
            $("#exp-2").hide();
        if (model.Timers == 0)
            $("#exp-3").hide();
        if (model.Items == 0)
            $("#exp-4").hide();
        if (model.Files == 0)
            $("#exp-5").hide();

        if (this.SrcUserId > 0)
            app_jqx_combo_async.userInputAdapter("#UserId", this.SrcUserId);

    };

    app_task.prototype.parentSettings = function (parentId) {
        $("#Task_Parent").val(parentId);
        if (parentId > 0) {
            $("#Task_Parent-group").show();
            $("#Task_Parent-link").click(function () {
                app.redirectTo('/System/TaskInfo?id=' + parentId);
            });
        }
        else {
            $("#Task_Parent-group").hide();
        }
    };

    app_task.prototype.doCancel = function () {
        app.redirectTo("/System/TaskUser");
        return this;
    };

        //app_task.prototype.doComment = function (id) {
        //    wizard.displayStep(2);
        //    $.ajax({
        //        type: 'GET',
        //        url: '/System/_TaskComment',
        //        data: { "id": id },
        //        success: function (data) {
        //            $('#divPartial2').html(data);
        //        }
        //    });
        //    return this;
        //};

        //app_task.prototype.doAssign = function (id) {
        //    wizard.displayStep(3);
        //    $.ajax({
        //        type: 'GET',
        //        url: '/System/_TaskAssign',
        //        data: { "id": id },
        //        success: function (data) {
        //            $('#divPartial3').html(data);
        //        }
        //    });
        //    return this;
        //};

    app_task.prototype.doSubmit = function (act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = app_jqx.getInputAutoValue("#TaskStatus",1);// $("#TaskStatus").val();
        var isnew = this.IsNew;

        var afterSubmit = function (slf, data) {

            if (isnew) {
                slf.TaskId = data.OutputId;
                $("#TaskId").val(data.OutputId);
                slf.tabSettings();
                $("#fcSubmit").val("עדכון");
                slf.IsNew = slf.TaskId == 0;
                app_messenger.Notify(data, 'info');
            }
            else {
                app_messenger.Notify(data, 'info', "/System/TaskUser");
        }

            if (act == 'plus') {
                app.refresh();
        }
    }

        var RunSubmit = function (slf, status, actionurl) {
            //e.preventDefault();
            //var slf = this;
            //var actionurl = $('#fcForm').attr('action');

            //if (status == 0)
            //    $("#TaskStatus").val(1);
            //if (status == 1)
            //    $("#TaskStatus").val(2);


            //$("#TaskStatus").val(status);
            app_jqx.setInputAutoValue("#TaskStatus", status);

            //var clientId = $("#ClientId").val();
            //if (clientId > 0) {
            //    $('#ClientDetails').val($("#ClientId-display").val())
            //}

            //var clientDetails = $('#ClientDetails').val();
            //, { key: 'ClientDetails', value: clientDetails }
            var value = $("#TaskBody").jqxEditor('val');
            var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }];
            var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);

            app_query.doFormSubmit("#fcForm", actionurl, formData, function (data) {

                afterSubmit(slf, data);
            });


            //var validationResult = function (isValid) {
            //    if (isValid) {
            //        //var formData = app.serialize('#fcForm');
            //        $.ajax({
            //            url: actionurl,
            //            type: 'post',
            //            dataType: 'json',
            //            data: formData,
            //            success: function (data) {

            //                afterSubmit(slf,data);

            //                //if (slf.TaskId == 0) {
            //                //    slf.TaskId = data.OutputId;
            //                //    $("#TaskId").val(data.OutputId);
            //                //    $("#hTitle-text").text('משימה: ' + data.OutputId);
            //                //    slf.afterUpdate();
            //                //    //$(".hxp").show();
            //                //    app_messenger.Notify(data, 'info');//, "/System/TaskUser");
            //                //}
            //                //else {
            //                //    app_messenger.Notify(data, 'info', "/System/TaskUser");
            //                //}

            //                //if (act == 'plus') {
            //                //    app.refresh();
            //                //}

            //            },
            //            error: function (jqXHR, status, error) {
            //                app_messenger.Notify(error, 'error');
            //            }
            //        });
            //    }
            //}
            //$('#fcForm').jqxValidator('validate', validationResult);
            return this;
    };

        var actionurl = $('#fcForm').attr('action');

        if (this.IsNew) {
            status = 1;
            actionurl = '/System/UpdateNewTask';
            RunSubmit(this, status, actionurl)
        }
        else {
            status = 2;
            actionurl = '/System/UpdateTask';
            if (status > 1 && status < 8 && act == 'end') {
                app_dialog.confirmYesNoCancel("האם לסיים משימה?", function (res) {
                    if (res == 'yes')
                        RunSubmit(this, status, '/System/TaskCompleted')
                    else if (res == 'no')//update
                        RunSubmit(this, status, actionurl);

                });
            }
            else {//update
                RunSubmit(this, status, actionurl);
        }
    }
        return this;
    };

    app_task.prototype.preLoad = function () {

        var slf = this;

        this.parentSettings(slf.TaskParentId);
        //    $("#Task_Parent").val(slf.TaskParentId);

        //    if (slf.TaskParentId > 0) {
        //        $("#Task_Parent-group").show();
        //        $("#Task_Parent-link").click(function () {
        //            app.redirectTo('/System/TaskInfo?id=' + slf.TaskParentId);
        //        });
        //    }
        //    else {
        //        $("#Task_Parent-group").hide();
        //}


        $("#AccountId").val(slf.AccountId);
        //$("#hTitle-text").text(slf.Title + ': ' + slf.TaskId);

        //$("#hxp-title").text(slf.Title + ': ' + slf.TaskId);

        if (theme === undefined)
            theme = 'nis_metro';


        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            //click: function (e, tab) {
            //    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            //},
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                switch (tab.id) {
                    case 1:
                        if (slf.TaskId == 0) {
                            event.preventDefault();
                            app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                            return false;
                        }
                        if ($("#jqxgrid1")[0].childElementCount == 0)
                            slf.comment.load(slf.TaskId, slf.UserInfo, slf.Option);
                        break;
                    case 2:
                        if ($("#jqxgrid2")[0].childElementCount == 0)
                            slf.assign.load(slf.TaskId, slf.UserInfo, slf.Option);
                        break;
                    case 3:
                        if ($("#jqxgrid3")[0].childElementCount == 0)
                            slf.timer.load(slf.TaskId, slf.UserInfo, slf.Option);
                        break;
                    case 4:
                        if (slf.TaskId == 0) {
                            event.preventDefault();
                            app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                            return false;
                        }
                        if (slf.EnableFormType) {
                            var source = $('#Form_Type').jqxComboBox('source');
                            if (source == null)
                                app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetTaskFormTypeList', 0, 120, false);
                            if ($("#jqxgrid4")[0].childElementCount == 0)
                                app_task.form_template.load(slf.TaskId, slf.UserInfo);

                        }
                        if ($("#jqxgrid4")[0].childElementCount == 0)
                            slf.form.load(slf.TaskId, slf.UserInfo, slf.Option);
                        break;
                    case 5:
                        if (slf.TaskId == 0) {
                            event.preventDefault();
                            app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                            return false;
                        }

                        if (slf.uploader == null) {
                            slf.uploader = new app_media_uploader("#task-files");
                            slf.uploader.init(slf.TaskId, 't', !slf.IsEditable);
                            slf.uploader.show();
                        }

                        //if ($("#iframe-files").attr('src') === undefined)
                        //    var op = slf.Model.Option;
                        //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t&op=' + op, '100%', '350px', true);

                        break;
                }
                return false;
            }
            //activateState: function (e, state) {
            //    //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            //}
        });


        $("#jqxExp-1").jqxExpander({ rtl: true, width: '80%', expanded: false });
        $('#jqxExp-1').on('expanding', function () {

            if (!slf.exp1_Inited) {
                slf.lazyLoad();
            }
        });


        $("#ColorFlag").simplecolorpicker();
        $("#ColorFlag").on('change', function () {
            //$('select').simplecolorpicker('destroy');
            var color = $("#ColorFlag").val();
            $("#hTitle").css("background-color", color)
        });
        //$("#TaskStatus").jqxDropDownList({ enableSelection: false, rtl: true });

        //$('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });
        //$("#Task_Type").jqxComboBox({ disabled: !this.IsEditable });
        //$("#TaskSubject").prop('disabled', !this.IsEditable);
        //$("#AssignTo").prop('disabled', !this.IsEditable);
        //$("#Task_Type").prop('disabled', !this.IsEditable);

        $('#TaskBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });
    };

    app_task.prototype.loadControls = function (record) {


        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });
        $('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, allowKeyboardDelete: false, readonly: true, width: '150px', rtl: true });

        if (record) {
            if (record.TaskStatus <= 0)
                record.TaskStatus = 1;

            this.taskSettings(record);

            app_jqxform.loadDataForm("fcForm", record, true, ["TaskStatus", "Project_Id", "ClientId", "Tags", "AssignTo"]);

            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
            $("#CreatedDate").val(record.CreatedDate);
            $("#StartedDate").val(app.jsonDateToString(record.StartedDate, true));
            $("#EndedDate").val(app.jsonDateToString(record.EndedDate, true));

            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || '#000'));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });
            this.parentSettings(record.Task_Parent);
            
            if (record.TaskStatus > 1 && record.TaskStatus < 8)
                $("#fcEnd").show();//$("#fcSubmit").val("סיום");
            else
                $("#fcEnd").hide();//$("#fcSubmit").val("עדכון");


            var align = app_style.langAlign(record.Lang);
            $('#TaskBody').css('text-align', align)

            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', null, 0, 120, true, null, function (status, records) {
                if (record.Task_Type >= 0)
                    $("#Task_Type").val(record.Task_Type);
            });
            $("#Task_Type").jqxComboBox({ enableSelection: this.IsEditable });
            //app_jqx_combo_async.taskStatusComboAdapter("#TaskStatus", record.TaskStatus);
            app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);
            

        }
        else {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            $('#CreatedDate').val(Date.now());

    }
        return this;
    };

    app_task.prototype.syncData_Obs = function (record) {

        if (record) {
            if (record.TaskStatus <= 0)
                record.TaskStatus = 1;

            //var parsed = JSON.parse(data, function (key, value) {
            //    if (typeof value === 'string') {
            //        var d = /\/Date\((\d*)\)\//.exec(value);
            //        return (d) ? new Date(+d[1]) : value;
            //    }
            //    return value;
            //});

            var parseJsonDate = function (value) {

                if (typeof value === 'string') {
                    var d = /\/Date\((\d*)\)\//.exec(value);
                    return (d) ? new Date(+d[1]) : value;
            }

                //value = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
                //return new Date(value);
        }

            app_jqxform.loadDataForm("fcForm", record, true, ["TaskStatus", "Project_Id", "ClientId", "Tags", "AssignTo"]);
            this.ProjectId = record.Project_Id;
            this.ClientId = record.ClientId;
            this.Tags = record.Tags;
            this.AssignTo = record.AssignTo;

            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
            $("#CreatedDate").val(record.CreatedDate);
            $("#StartedDate").val(app.jsonDateToString(record.StartedDate, true));
            $("#EndedDate").val(app.jsonDateToString(record.EndedDate, true));

            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || '#000'));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });

            if (record.TaskStatus > 1 && record.TaskStatus < 8)
                $("#fcEnd").show();//$("#fcSubmit").val("סיום");
            else
                $("#fcEnd").hide();//$("#fcSubmit").val("עדכון");


            var align = app_style.langAlign(record.Lang);
            $('#TaskBody').css('text-align', align)

            //var editable = false;
            //if (record.TaskStatus < 8 && record.AssignBy == record.UserId) {
            //    editable = true;
            //}

            //$("#TaskBody").jqxEditor({ editable: this.IsEditable });

            //app_jqxcombos.selectCheckList("listCategory", record.Categories);

            //app_jqxcombos.initComboValue('City', 0);
    }
        return this;
    };

    app_task.prototype.lazyLoad = function () {

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', this.ClientId, function () { 

        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', this.ProjectId, function () { 

        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 300, 0, true, this.Tags, function () { 

        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 300, 0, null, this.AssignTo, function () { 

        });
        //$("#AssignTo").keydown(function (event) {
        //   if (event.which == 8) {
        //       event.preventDefault();
        //   }
        //});

        //app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
        //app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');

        this.exp1_Inited = true;

        if (!this.IsEditable) {
            $("#ClientId").prop("readonly", true);
            $("#Project_Id").prop("readonly", true);
            $("#Tags").jqxComboBox({ enableSelection: false });
            $("#AssignTo").jqxComboBox({ enableSelection: false });
    }
    };

    app_task.prototype.loadControls_Obs = function (record) {

        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });
        $('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, allowKeyboardDelete: false, readonly: true, width: '150px', rtl: true });
        //$('#CreatedDate').val(Date.now());

        //$('#DueDate').val('');

        if (this.IsNew) {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
        }
        else {
            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', null, 0, 120, true, null, function (status, records) {
                if (record.Task_Type >= 0)
                    $("#Task_Type").val(record.Task_Type);
            });
            //app_jqx_combo_async.taskTypeComboAdapter("#Task_Type", record.Task_Type);
            $("#Task_Type").jqxComboBox({ enableSelection: this.IsEditable });
            //app_jqx_adapter.createComboAdapterAsync("UserTeamId", "DisplayName", "#AssignTo", '/System/GetUserTeamList', null, 0, 120, true, value, callback)
            //app_jqx_list.taskTypeComboAdapter();
            app_jqx_combo_async.taskStatusComboAdapter("#TaskStatus", record.TaskStatus);
    }



        //app_jqx_list.taskStatusComboAdapter();
        //app_jqxcombos.createComboAdapter("ProjectId", "ProjectName", "Project_Id", '/System/GetProjectList', 0, 120, false);






        //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid='+this.TaskId   +'&refType=t', '100%', '360px', true);
        return this;
    };

    app_task.prototype.loadEvents = function () {

        var slf = this;

        $('#reset').on('click', function () {
            location.reload();
        });

        $('#fcClear').on('click', function (e) {
            //app_messenger.Post("הודעה");
            $('#fcForm')[0].reset();
            $('#fcForm').jqxValidator('hide');
        });
        $('#fcNew').on('click', function (e) {
            app.refresh();
        });
        $('#fcCancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#fcSubmit').on('click', function (e) {
            slf.doSubmit('update');
        });

        if (this.IsNew) {

            $('#fcSubmit-plus').on('click', function (e) {
                task_def.doSubmit('plus');
            });

            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >=0) {
                    //app_tasks_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value
                        },
                            function (data) {
                                if (data.Status > 0)
                                    $('#jqxgrid4').jqxGrid('source').dataBind();
                        });
                    });

                    //if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
                    //    app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
                    //        function (data) {
                    //            if (data.Status > 0)
                    //                $('#jqxgrid4').jqxGrid('source').dataBind();
                    //        });
                    //}
            }
                $("#Form_Type").jqxComboBox({ selectedIndex: -1 });

            });
            $("#form_template-check").on('change', function (event) {

                if (this.checked) {//($('#form_template-check').is(":checked")) {

                    var rows = $('#jqxgrid4').jqxGrid('getrows');
                    if (rows && rows.length > 0)
                        $("#form_template-div").show();
                    else {
                        app_dialog.alert("לא נמצאו רשומות ליצירת תבנית");
                        this.checked = false;
                }
                }
                else
                    $("#form_template-div").hide();
            });
            $('#form_template-save').click(function () {
                var name = $('#form_template-input').val();
                if (name == null || name == '') {
                    app_dialog.alert("נא לציין שם תבנית");
                    return;
            }
                name = "'" + name + "'";
                app_query.doDataPost("/System/TaskFormTemplateCreate", { 'id': slf.TaskId, 'name': name
                }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid4').jqxGrid('source').dataBind();
                });

                //app_query.doPost("/System/TaskFormTemplateCreate", { 'id': slf.TaskId, 'name': name }, function (data) {
                //    if (data.Status > 0) {
                //        app_messenger.Post(data);
                //        $('#jqxgrid4').jqxGrid('source').dataBind();
                //    }
                //    else
                //        app_messenger.Post(data, 'error');
                //});
            });
            $("#IntendedTo").on('change', function (event) {
                var args = event.args;
                if (args) {
                    var item = args.item;
                    if (item.label.charAt(0) == '@') {
                        $('#UserId').val(0);
                        $('#TeamId').val(item.value);
                    }
                    else {
                        $('#UserId').val(item.value);
                        $('#TeamId').val(0);
                }

            }
            });
        }
        else {

            $("#Task_Child-link").click(function () {

                app_dialog.confirm("האם ליצור תת משימה", function () {
                    app.redirectTo('/System/TaskNew?pid=' + slf.TaskId);
                });
            });

            $('#fcEnd').on('click', function (e) {
                task_def.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    task_def.doSubmit('plus');
            //});
            $('#task-item-update').click(function () {
                var iframe = wizard.getIframe();
                if (iframe && iframe.triggerSubmit) {
                    iframe.triggerSubmit();
            }
            });
            $('#task-item-cancel').click(function () {
                wizard.wizHome();
            });
    }

        var input_rules = [
       //{ input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
       {
               input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'none', rule: 'required', rule: function (input, commit) {
                   //var value = $('#DueDate').jqxDateTimeInput('value');
               var text = $('#DueDate').jqxDateTimeInput('getText');
               return text != null && text.length > 0;
       }
        },
       {
               input: "#TaskBody", message: 'חובה לציין תוכן!', action: 'none', rule: function (input, commit) {
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

        //============================================================ app_tasks_form

    app_task.form_template = {

            wizardStep: 2,
            FormId: 0,
            TaskId: 0,
            UserId: 0,
            load: function (taskId, userInfo) {
            this.TaskId = taskId;
            this.UserId = userInfo.UserId;
            this.grid(this.TaskId);
            return this;
    },
            load_template: function (formId) {
            this.FormId = formId;
            this.grid(formId);
            return this;
    },
            grid: function (taskid) {
            var slf = this;

            var nastedsource = {
                    datafields: [
                          { name: 'ItemId', type: 'number'
                    },
                          { name: 'ItemDate', type: 'date'
                    },
                          { name: 'ItemText', type: 'string'
                    },
                          { name: 'DoneDate', type: 'date'
                    },
                          { name: 'DoneStatus', type: 'bool'
                    },
                          { name: 'DisplayName', type: 'string'
                    },
                          { name: 'Task_Id', type: 'number'
                    },
                          { name: 'AssignBy', type: 'number'
                    },
                          { name: 'UserId', type: 'number' }
            ],
                    datatype: "json",
                    id: 'ItemId',
                    type: 'POST',
                    url: '/System/GetTasksFormGrid',
                    data: { 'pid': taskid
            }
            }
            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

            $("#jqxgrid4").jqxGrid({
                    width: '100%',
                //editable: true,
                //selectionmode: 'singlerow',
                //editmode: 'selectedrow',
                    autoheight: true,
                    enabletooltips: true,
                    localization: getLocalization('he'),
                    source: nastedAdapter, width: '99%', height: 130,
                    columnsresize: true,
                    rtl: true,
                    columns: [
                      { text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                    },
                      { text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center'
                    },
                      //{ text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
                      //{ text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center' },
                      { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' }
            ]
            });
                //$("#jqxgrid4").on('cellbeginedit', function (event) {
                //    var args = event.args;
                //    if (args.datafield === "firstname") {
                //        rowValues = "";
                //    }
                //    rowValues += args.value.toString() + "    ";
                //    if (args.datafield === "price") {
                //        $("#cellbegineditevent").text("Begin Row Edit: " + (1 + args.rowindex) + ", Data: " + rowValues);
                //    }
                //});
                //$("#jqxgrid4").on('cellendedit', function (event) {
                //    var args = event.args;
                //    if (args.datafield === "firstname") {
                //        rowValues = "";
                //    }
                //    rowValues += args.value.toString() + "    ";
                //    if (args.datafield === "price") {
                //        $("#cellbegineditevent").text("End Row Edit: " + (1 + args.rowindex) + ", Data: " + rowValues);
                //    }
                //});

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

                //================= popup Update ================.

                //$('#jqxgrid4').on('rowdoubleclick', function (event) {
                //    var args = event.args;
                //    var boundIndex = args.rowindex;
                //    var visibleIndex = args.visibleindex;
                //    doRowEdit(boundIndex);
                //});
            $("#jqxgrid4-window").jqxWindow({
                    width: 400, height: 240, resizable: false, autoOpen: false, cancelButton: $("#formCancel"), modalOpacity: 0.01, position: { x: 0, y: 0
            }
            });
                //$("#jqxgrid4-window").jqxWindow({
                //    width: 400, height:240, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#formCancel"), modalOpacity: 0.01
                //});
                //$("#popupWindow").on('open', function () {
                //    $("#ItemText").jqxInput('selectAll');
                //});
            $('#fcTaskForm').jqxValidator({
                    hintType: 'label',
                    animationDuration: 0,
                    rules: [
                          { input: '#ItemText', message: 'נדרש נושא!', action: 'keyup, blur', rule: 'required' }
            ]
            });
                //$('#popupWindow').on('validationSuccess', function (event) {
                //    var row = {
                //        ItemText: $("#ItemText").val(),
                //        ItemId: $("#ItemId").val(),
                //        TaskId: $("#TaskId").val()
                //    };
                //    if (ItemId == 0) {
                //        $('#jqxgrid4').jqxGrid('addrow', null, row);
                //    }
                //    else if (editrow >= 0) {
                //        var rowID = $('#jqxgrid4').jqxGrid('getrowid', editrow);
                //        $('#jqxgrid4').jqxGrid('updaterow', rowID, row);
                //    }
                //    $("#popupWindow").jqxWindow('hide');
                //});
            $("#formSave").click(function () {
                slf.update();
                //$('#popupWindow').jqxValidator('validate');
            });
                //================= end popup Update ================.
    },
            doRowEdit: function (selectedrowindex) {
            $("#Task_Id").val(this.TaskId);

            var rowscount = $("#jqxgrid4").jqxGrid('getdatainformation').rowscount;
            if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                $("#jqxgrid4").jqxGrid('ensurerowvisible', selectedrowindex);
                editrow = selectedrowindex;
                var dataRecord = $("#jqxgrid4").jqxGrid('getrowdata', editrow);

                $("#ItemText").val(dataRecord.ItemText);
                $("#ItemId").val(dataRecord.ItemId);
                //app_jqx.openWindow("#popupWindow", "#jqxgrid4");
                app_jqx.displayWindow("#jqxgrid4-window", "#jqxgrid4-add");
            }
            else {
                $("#ItemText").val('');
                $("#ItemId").val(0);
                //app_jqx.openWindow("#popupWindow", "#jqxgrid4");
                app_jqx.displayWindow("#jqxgrid4-window", "#jqxgrid4-add");
            }
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
            add: function () {
            $("#Task_Id").val(this.TaskId);
            $("#ItemText").val('');
            $("#ItemId").val(0);
                //app_jqx.openWindow("#popupWindow", "#jqxgrid4");
            app_jqx.displayWindow("#jqxgrid4-window", "#jqxgrid4-add");

                //this.doRowEdit(0);
                //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormAdd?pid=" + this.TaskId, "100%", "220px", true);
    },
            edit: function () {
            var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
            if (selectedrowindex < 0)
                    return;
            this.doRowEdit(selectedrowindex);
                //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormEdit?id=" + id, "100%", "480px", true);
    },
            remove: function () {

            var data = this.getRowData();
            if (data == null)
                    return;
                //if (this.UserId != data.AssignBy)
                //{
                //    app_dialog.Alert("לא ניתן למחוק פעולה שהוקצתה על ידי משתמש אחר");
                //    return;
                //}
            var id = this.getrowId();
            if (id > 0) {
                if (confirm('האם למחוק פעולה ' +id)) {
                    app_query.doPost(app.appPath() + "/System/TaskFormDelete", { 'id': id });
                    $('#jqxgrid4').jqxGrid('source').dataBind();
            }
            }
    },
            update: function () {

            app_query.doFormPost('#fcTaskForm', function (data) {
                if (data.Status > 0) {
                    app_messenger.Post(data);
                    $('#jqxgrid4').jqxGrid('source').dataBind();
                    app_jqx.closeWindow("#jqxgrid4-window");
                }
                else
                    app_messenger.Post(data, 'error');
            });
    },
            refresh: function () {
            $('#jqxgrid4').jqxGrid('source').dataBind();
    },
            cancel: function () {
                //wizard.wizHome();
    },
            end: function (data) {

            app_messenger.Post(data);
            if (data && data.Status > 0) {
                this.refresh();
            }
    }


    };

        //============================================================ app_task_comment

    app_task.prototype.comment = {

            wizardStep: 2,
            TaskId: 0,
            Option: 'e',
        //Model: {},
            UInfo: null,
            Control: null,
            load: function (taskId, userInfo, option) {
            this.TaskId = taskId;// dataModel.Id;
            this.Option = (option) ? option : 'e';
                //this.Model = dataModel;
            this.UInfo = userInfo;
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            this.grid(this.TaskId);
            return this;
    },
            grid: function (taskid) {
            var slf = this;

            var nastedsource = {
                    datafields: [
                          { name: 'CommentId', type: 'number'
                    },
                          { name: 'CommentDate', type: 'date'
                    },
                          { name: 'CommentText', type: 'string'
                    },
                          { name: 'ReminderDate', type: 'date'
                    },
                          { name: 'Attachment', type: 'string'
                    },
                          { name: 'DisplayName', type: 'string'
                    },
                          { name: 'Task_Id', type: 'number'
                    },
                          { name: 'UserId', type: 'number' }
            ],
                    datatype: "json",
                    id: 'CommentId',
                    type: 'POST',
                    url: '/System/GetTasksCommentGrid',
                    data: { 'pid': taskid
            }
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
                    scrollbarsize: 1,
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
                      { text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center'
                    },
                      { text: 'שם', datafield: 'DisplayName', cellsalign: 'right', align: 'center', width: 100, hidden: app.IsMobile() }
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

            var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action
            };

            if (this.Control == null) {
                this.Control = new app_task_comment("#jqxgrid1-window");//app_task_comment_control("#jqxgrid1-window");
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
            if (this.Option != "e")
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
                if (confirm('האם למחוק הערה ' +id)) {
                    app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id
                    }, function (data) {
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

        //============================================================ app_task_assign

    app_task.prototype.assign = {
            wizardStep: 3,
            Option: 'e',
        //Model: {},
            load: function (taskId, userInfo, option) {
            this.TaskId = taskId;//dataModel.Id;
                //this.Model = dataModel;
            this.Option = (option) ? option : 'e';
            this.UInfo = userInfo;
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            this.grid(this.TaskId);
            return this;
    },
            grid: function (taskid) {
            var slf = this;

            var nastedsource = {
                    datafields: [
                          { name: 'AssignId', type: 'number'
                    },
                          { name: 'AssignedBy', type: 'number'
                    },
                          { name: 'AssignedTo', type: 'number'
                    },
                          { name: 'Task_Id', type: 'number'
                    },
                          { name: 'AssignDate', type: 'date'
                    },
                          { name: 'AssignSubject', type: 'string'
                    },
                          { name: 'AssignedByName', type: 'string'
                    },
                          { name: 'AssignedToName', type: 'string' }
            ],
                    datatype: "json",
                    id: 'AssignId',
                    type: 'POST',
                    url: '/System/GetTasksAssignGrid',
                    data: { 'pid': taskid
            }
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
                      { text: 'מועד רישום', datafield: 'AssignDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                    },
                      { text: 'עבר מ', datafield: 'AssignedByName', width: 120, cellsalign: 'right', align: 'center'
                    },
                      { text: 'עבר ל', datafield: 'AssignedToName', width: 120, cellsalign: 'right', align: 'center'
                    },
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

            var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action
            };

            if (this.Control == null) {
                this.Control = new app_task_assign("#jqxgrid2-window");
            }
            this.Control.init(data_model, this.UInfo);
            this.Control.display();
    },
            getrowId: function () {

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
            if (this.Option != "e")
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
                if (confirm('האם למחוק העברה ' +id)) {
                    app_query.doPost(app.appPath() + "/System/TaskAssignDelete", { 'id': id
                    }, function (data) {
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
    };

        //============================================================ app_task_timer

    app_task.prototype.timer = {
            wizardStep: 4,
        //Model: {},
            Option: 'e',
            load: function (taskId, userInfo, option) {
            this.TaskId = taskId;//dataModel.Id;
                //this.Model = dataModel;
            this.Option = (option) ? option : 'e';
            this.UInfo = userInfo;
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            this.grid(this.TaskId);
            return this;
    },
            grid: function (taskid) {
            var slf = this;
            var nastedsource = {
                    datafields: [
                          { name: 'TaskTimerId', type: 'number'
                    },
                          { name: 'SubIndex', type: 'number'
                    },
                          { name: 'Duration', type: 'number'
                    },
                          { name: 'DurationView', type: 'string'
                    },
                          { name: 'Task_Id', type: 'number'
                    },
                          { name: 'StartTime', type: 'date'
                    },
                          { name: 'EndTime', type: 'date'
                    },
                          { name: 'Subject', type: 'string'
                    },
                          { name: 'UserId', type: 'number'
                    },
                          { name: 'DisplayName', type: 'string' }
            ],
                    datatype: "json",
                    id: 'TaskTimerId',
                    type: 'POST',
                    url: '/System/GetTasksTimerGrid',
                    data: { 'pid': taskid
            }
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
                      { text: 'מס-סידורי', datafield: 'SubIndex', width: 120, cellsalign: 'right', align: 'center'
                    },
                      { text: 'מועד התחלה', datafield: 'StartTime', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                    },
                      { text: 'מועד סיום', datafield: 'EndTime', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                    },
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

            var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action
            };

            if (this.Control == null) {
                this.Control = new app_task_timer("#jqxgrid3-window");
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
            if (this.Option != "e")
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
                if (confirm('האם למחוק מעקב זמנים ' +id)) {
                    app_query.doPost(app.appPath() + "/System/TaskTimerDelete", { 'id': id
                    }, function (data) {
                        if (data.Status > 0)
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

    };

        //============================================================ app_task_form

    app_task.prototype.form = {

            wizardStep: 2,
            TaskId: 0,
            UserId: 0,
            isMobile: false,
            Option: 'e',
        //Model: {},
            load: function (taskId, userInfo, option) {
            this.TaskId = taskId;//dataModel.Id;
            this.UserId = userInfo.UserId;
                //this.Model = dataModel;
            this.Option = (option) ? option : 'e';
            this.UInfo = userInfo;
            this.isMobile = app.IsMobile();
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            this.grid(this.TaskId);
            return this;
    },
            grid: function (taskid) {
            var slf = this;

            var nastedsource = {
                    datafields: [
                          { name: 'ItemId', type: 'number'
                    },
                          { name: 'ItemDate', type: 'date'
                    },
                          { name: 'ItemText', type: 'string'
                    },
                          { name: 'DoneDate', type: 'date'
                    },
                          { name: 'StartDate', type: 'date'
                    },
                          { name: 'DoneStatus', type: 'bool'
                    },
                          { name: 'DisplayName', type: 'string'
                    },
                          { name: 'Task_Id', type: 'number'
                    },
                          { name: 'AssignBy', type: 'number'
                    },
                          { name: 'UserId', type: 'number' }
            ],
                    datatype: "json",
                    id: 'ItemId',
                    type: 'POST',
                    url: '/System/GetTasksFormGrid',
                    data: { 'pid': taskid
            }
            }
            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

            $("#jqxgrid4").jqxGrid({
                    width: '100%',
                    editable: slf.Option == "e",
                    autoheight: true,
                    enabletooltips: true,
                    localization: getLocalization('he'),
                    source: nastedAdapter, width: '99%', height: 130,
                    columnsresize: true,
                    rtl: true,
                    columns: [
                      { text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile
                    },
                      { text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center', editable: false
                    },
                      { text: 'מועד התחלה', datafield: 'StartDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile
                    },
                      { text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile
                    },
                      { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center'
                    },
                      { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center', editable: false, hidden: slf.isMobile }
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
                slf.update(id, value);

            });
            $('#jqxgrid4').on('rowselect', function (event) {
                // event arguments.
                var args = event.args;
                // row's bound index.
                var rowBoundIndex = args.rowindex;
                // row's data. The row's data object or null(when all rows are being selected or unselected with a single action). If you have a datafield called "firstName", to access the row's firstName, use var firstName = rowData.firstName;
                var rowData = args.row;

                if (rowData) {
                    var editable = (rowData.AssignBy == slf.UserId);
                    app.showIf("#jqxgrid4-remove", editable);
                    //app.showIf("#jqxgrid4-edit",editable);
            }

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

            var data_model = { PId: this.TaskId, Id: id, Option: option, Action: action
            };

            if (this.Control == null) {
                this.Control = new app_task_form("#jqxgrid4-window");
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
            if (this.Option != "e")
                    return;
            var id = this.getrowId();
            if (id > 0) {

                //if (rowData) {
                //    var editable = (rowData.AssignBy == slf.UserId);
                //    app.showIf("#jqxgrid4-remove", editable);
                //    app.showIf("#jqxgrid4-edit", editable);
                //}

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
            if (this.UserId != data.AssignBy) {
                app_dialog.Alert("לא ניתן למחוק פעולה שהוקצתה על ידי משתמש אחר");
                return;
            }
            var id = this.getrowId();
            if (id > 0) {
                if (confirm('האם למחוק פעולה ' +id)) {
                    app_query.doPost(app.appPath() + "/System/TaskFormDelete", { 'id': id
                    }, function (data) {
                        if (data.Status > 0)
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
                    data: { 'id': id, 'done': value
            },
                    success: function (data) {
                    if (data.Status > 0) {
                        app_task.triggerSubTaskCompleted('form', data);
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


        //============================================================ app_tasks global

    app_task.tasks_edit_view_comment = function (row) {

        var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
        content = content.replace("\n", "<br/>");
        app_jqx.toolTipClick(".task-comment", '<p>' + content + '</p>', 350);
    };

    app_task.setTaskButton = function (item, action, visible) {
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
    };

        //============================================================ app_tasks trigger

    app_task.triggerTaskCommentCompleted = function (data) {
        app_task_comment.end(data);
    };
    app_task.triggerTaskAssignCompleted = function (data) {
        app_task_assign.end(data);
    };
    app_task.triggerTaskTimerCompleted = function (data) {
        app_task_timer.end(data);
    };
    app_task.triggerTaskFormCompleted = function (data) {
        app_task_form.end(data);
    };
    app_task.triggerSubTaskCompleted = function (type, data) {
        var tag;
        switch (type) {
            case 'comment':
                tag = '#jqxgrid1'; break;
            case 'assign':
                tag = '#jqxgrid2'; break;
            case 'timer':
                tag = '#jqxgrid3'; break;
            case 'form':
                tag = '#jqxgrid4';
                break;
    }

        if (tag != null) {
            $(tag + '-window').hide();
            $(tag + '-bar').show();
            if (data != null && data.Status > 0)
                $(tag).jqxGrid('source').dataBind();
    }
    };


    return app_task;

}());

//============================================================ app_task_comment_control

var app_task_comment = (function () {

    var app_task_comment = function ($element) {
        this.$element = $($element);
        var slf = this;

        return this;
    }

    app_task_comment.tag = function ($element,option) {

        var pasive = option == "a" ? " pasive" : "";

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

        $element.html(html).hide();

    };

    app_task_comment.prototype.init = function (dataModel, userInfo,readonly) {
        this.TaskId = dataModel.PId;
        this.CommentId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf=this;

        app_task_comment.tag(this.$element,dataModel.Option);

        $('#ReminderDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        //$('#ReminderDate').val('');

        $('#comment-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#comment-Cancel').on('click', function (e) {
            slf.doCancel();
        });

        var input_rules = [
          { input: '#CommentText', message: 'חובה לציין תוכן!', action: 'none', rule: 'required' },
        ];

        $('#comment-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });

        var load = function (slf) {

            //if (this.viewAdapter == null) {
                var view_source =
                {
                    datatype: "json",
                    id: 'CommentId',
                    data: { 'taskid': slf.TaskId, 'id': slf.CommentId },
                    type: 'POST',
                    url: '/System/GetTaskCommentEdit'
                };

                var viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("comment-Form", record);
                        if (record.CommentDate > 0)
                            $('#CommentDate').val(record.CommentDate.toLocaleDateString());
                        $("#comment-Title").text("הערה: " + slf.CommentId);
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'taskid': slf.TaskId, 'id': slf.CommentId };
            //}
            viewAdapter.dataBind();
        };

        if (this.CommentId > 0) {
            load(this);
        } else {
            $('#comment-Form input[name=Task_Id]').val(this.TaskId);
            $('#comment-Form input[name=UserId]').val(this.UserInfo.UserId);
            $("#comment-Title").text("הערה: " + "חדשה");
        }
    };

    app_task_comment.prototype.display = function () {
        this.$element.show();
        $("#jqxgrid1-bar").hide()
    };

    app_task_comment.prototype.doCancel = function () {

        app_task.triggerSubTaskCompleted('comment');
    };

    app_task_comment.prototype.doSubmit = function () {
        //e.preventDefault();
        var actionUrl = $('#comment-Form').attr('action');
        var formData = $('#comment-Form').serialize();
      
        app_query.doFormSubmit('#comment-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {
                app_task.triggerSubTaskCompleted('comment', data);
            }
            else
                app_messenger.Post(data, 'error');
        });

        //var validationResult = function (isValid) {
        //    if (isValid) {
        //        $.ajax({
        //            url: actionurl,
        //            type: 'post',
        //            dataType: 'json',
        //            data: formData,
        //            success: function (data) {

        //                if (data.Status > 0) {
        //                    app_task.triggerSubTaskCompleted('comment', data);

        //                    //if (slf.IsDialog) {
        //                    //    window.parent.triggerCommentCompleted(data.OutputId);
        //                    //    //$('#fcForm').reset();
        //                    //}
        //                    //else {
        //                    //    app.refresh();
        //                    //}
        //                    //$('#TaskId').val(data.OutputId);
        //                }
        //                else
        //                    app_messenger.Post(data, 'error');
        //            },
        //            error: function (jqXHR, status, error) {
        //                app_messenger.Post(error, 'error');
        //            }
        //        });
        //    }
        //}

        //$('#comment-Form').jqxValidator('validate', validationResult);

    };

    return app_task_comment;
}());

//============================================================ app_task_assign_control

var app_task_assign = (function () {

    var app_task_assign = function ($element) {
        this.$element = $($element);
        var slf = this;
        return this;
    }

    app_task_assign.tag = function ($element,option) {
        var pasive = option == "a" ? " pasive" : "";

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
                                    '</div>' +
                                    '<div class="form-group' + pasive + '">' +
                                        '<div class="field">הועבר מ:</div>' +
                                        '<textarea id="AssignedByName" name="AssignedByName" class="text-content-mid"></textarea>' +
                                    '</div>' +
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

        $element.html(html);
    };

    app_task_assign.prototype.init = function (dataModel, userInfo) {

        this.TaskId = dataModel.PId;
        this.AssignId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        var slf = this;

        //$("#AccountId").val(this.AccountId);
        app_task_assign.tag(slf.$element,dataModel.Option);

        app_jqxcombos.createComboAdapter("User_Id", "DisplayName", "AssignedTo", '/System/GetUsersInTeamList', 0, 120, false);

        var input_rules = [
                 {
                     input: "#AssignedTo", message: 'חובה לציין העברה אל!', action: 'none', rule: function (input, commit) {
                         var index = $("#AssignedTo").jqxComboBox('getSelectedIndex');
                         return index >= 0;
                     }
                 },
                 { input: '#AssignSubject', message: 'חובה לציין נושא!', action: 'none', rule: 'required' },
        ];

        $('#assign-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });


        $('#assign-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#assign-Cancel').on('click', function (e) {
            slf.doCancel();
        });

        var load = function (slf) {

                var view_source =
                {
                    datatype: "json",
                    id: 'AssignId',
                    data: { 'id': slf.AssignId },
                    type: 'POST',
                    url: '/System/GetTaskAssignEdit'
                };

                var viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("assign-Form", record);
                        $("#assign-Title").text("העברה: " + slf.AssignId);
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'id': slf.AssignId };
            //}
            viewAdapter.dataBind();
        };

        if (this.AssignId > 0) {
            load(this);
        }
        else {
            $("#assign-Form input[name=Task_Id]").val(this.TaskId);
            $("#assign-Form input[name=UserId]").val(this.UserInfo.UserId);
            $("#assign-Title").text("העברה: " + "חדשה");

        }
    };

    app_task_assign.prototype.display = function () {
        this.$element.show();
        $("#jqxgrid2-bar").hide()
    };

    app_task_assign.prototype.doCancel = function () {

        app_task.triggerSubTaskCompleted('assign');
    };

    app_task_assign.prototype.doSubmit = function () {
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
                            app_task.triggerSubTaskCompleted('assign', data);
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

    return app_task_assign;
    }());

//============================================================ app_task_timer_control

var app_task_timer = (function () {

    var app_task_timer = function ($element) {
        this.$element = $($element);
        this.timerState = 'idle';//idle|started|stoped
        var slf = this;
        return this;
    }

    app_task_timer.tag = function ($element, option) {
        var pasive = option == "a" ? " pasive" : "";

        var html = '<div id="timer-Window" class="container" style="margin:5px">' +
                    '<hr style="width:100%;border:solid 1px #15C8D8">' +
                    '<div id="timer-Header">' +
                        '<span id="timer-Title" style="font-weight: bold;">מעקב זמנים</span>' +
                    '</div>' +
                    '<div id="timer-Body">' +
                        '<form class="fcForm" id="timer-Form" method="post" action="/System/TaskTimerUpdate">' +
                            '<div style="direction: rtl; text-align: right">' +
                                '<input type="hidden" id="TaskTimerId" name="TaskTimerId" value="0" />' +
                                '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                '<input type="hidden" id="UserId" name="UserId" value="" />' +
                                '<div style="height:5px"></div>' +
                                '<div id="tab-content" class="tab-content" dir="rtl">' +
                                    '<div class="form-group' + pasive + '">' +
                                        '<label class="field">מס-סידורי: </label>' +
                                       '<input id="SubIndex" name="SubIndex" type="number" readonly="readonly" class="text-small label" />' +
                                    '</div>' +
                                    '<div class="form-group">' +
                                        '<div class="field">נושא:</div>' +
                                        '<textarea id="Subject" name="Subject" class="text-content-mid">' + '</textarea>' +
                                    '</div>' +
                                    '<div class="form-group' + pasive + '">' +
                                        '<div class="field">מועד התחלה:</div>' +
                                        '<div id="StartTime" name="StartTime">' + '</div>' +
                                    '</div>' +
                                    '<div id="divEndTime" class="form-group' + pasive + '">' +
                                         '<div class="field">מועד סיום:</div>' +
                                         '<div id="EndTime" name="EndTime">' + '</div>' +
                                    '</div>' +
                                    '<div id="divDuration" class="form-group' + pasive + '">' +
                                         '<div class="field">משך זמן:</div>' +
                                         '<input id="Duration" name="Duration" type="number" readonly="readonly" class="text-mid" />' +
                                    '</div>' +
                                    '<div class="form-group' + pasive + '">' +
                                         '<div class="field">נוצר ע"י:</div>' +
                                         '<input id="DisplayName" type="text" disabled="disabled" class="text-mid" />' +
                                    '</div>' +
                               '</div>' +
                               '<div>' +
                                    '<a id="timer-sw_start" class="btn-default btn7 w-60" href="#">התחל</a> ' +
                                    '<a id="timer-Submit" class="btn-default btn7 w-60" href="#">סיום</a> ' +
                                    '<a id="timer-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
                                    '<label> : </label><span id="timer-sw_timer" class="panel-area-header"></span>' +
                              '</div>' +
                              '<div id="timer-sw_status" class="pasive"></div>' +
                       '</div>' +
              '</form>' +
           '</div>' +
        '</div>';

        $element.html(html);
    };

    app_task_timer.prototype.init = function (dataModel, userInfo, readonly) {

        this.TaskId = dataModel.PId;
        this.TaskTimerId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        app_task_timer.tag(this.$element, dataModel.Option);

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        $('#StartTime').jqxDateTimeInput({ showCalendarButton: false, allowKeyboardDelete: false, readonly: true, width: '150px', rtl: true, formatString: "dd/MM/yyyy hh:mm"});
        //$('#StartTime').val('');
        $('#EndTime').jqxDateTimeInput({ showCalendarButton: false, allowKeyboardDelete: false, readonly: true, width: '150px', rtl: true, formatString: "dd/MM/yyyy hh:mm" });
        //$('#EndTime').val('');

        $("#timer-Submit").hide();
        //$("#timer-Cancel").hide();


        var input_rules = [
              { input: '#Subject', message: 'חובה לציין נושא!', action: 'none', rule: 'required' },
        ];

        $('#timer-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });

        $('#timer-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#timer-Cancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#timer-sw_start').on('click', function (e) {
            e.preventDefault();
            slf.doStart();
        });
        var load = function (slf) {

           // if (this.viewAdapter == null) {
                var view_source =
                {
                    datatype: "json",
                    id: 'TaskTimerId',
                    data: { 'id': slf.TaskTimerId },
                    type: 'POST',
                    url: '/System/GetTaskTimerEdit'
                };

                var viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("timer-Form", record, true, ["EndTime"]);
                        $("#timer-Form input[id=DisplayName]").val(record.DisplayName);
                        $("#timer-Title").text("מד-זמן: " + slf.TaskTimerId);
                        //$('#StartTime').val(record.StartTime);
                        if (app.isNull(record.EndTime, "") == "") {
                            $('#divEndTime').hide();
                            $('#divDuration').hide();
                            $('#timer-Submit').text("סיום");
                            //$('#timer-sw_start').text("המשך");
                        }
                        else {
                            $('#EndTime').val(record.EndTime);
                        //    $('#timer-Submit').hide();
                        //    $('#timer-form > #Subject').prop('readonly', true);
                        //    var sub= app_jqxform.findInputInForm ("timer-Form", "Subject");//, attrName, inputType) {
                        //    sub.prop('readonly', true);
                        }

                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'id': slf.TaskTimerId };
            //}
            viewAdapter.dataBind();

        };

        if (this.TaskTimerId > 0) {
            load(this);
        }
        else {

            $("#timer-Form input[name=Task_Id]").val(this.TaskId);
            $("#timer-Form input[name=UserId]").val(this.UserInfo.UserId);
            //$('#timer-Submit').text("התחל");
            $("#timer-Submit").hide();
            $("#timer-Cancel").show();
            $("#timer-Title").text("מד-זמן: " + "חדש");
        }

    };

    app_task_timer.prototype.display = function () {
        this.$element.show();
        $("#jqxgrid3-bar").hide()
    };

    app_task_timer.prototype.doCancel = function () {
        this.timerState = 'idle';
        app_task.triggerSubTaskCompleted('timer');
    };

    app_task_timer.prototype.doStart = function () {
        //e.preventDefault();
        var slf = this;

        if (this.timerState == 'started') {
            $.JStopwatch.stopTimer();
            this.timerState = 'stoped';
            $("#timer-sw_start").text("התחל");
            $("#timer-Submit").show();
            $("#timer-Cancel").show();
            return;
        }
        else if (this.timerState == 'stoped') {
            $.JStopwatch.startTimer('timer', 'sw');
            this.timerState = 'started';
            $("#timer-sw_start").text("עצור");
            $("#timer-Submit").hide();
            $("#timer-Cancel").hide();
            return;
        }
        else if (this.timerState=='idle'){
            if (this.TaskTimerId>0) {
                $.JStopwatch.startTimer('timer', 'sw');
                this.timerState = 'started';
                $("#timer-sw_start").text("עצור");
                $("#timer-Submit").hide();
                $("#timer-Cancel").hide();
                return;
            }
        }

        var actionurl = $('#timer-Form').attr('action');
        var formData = $('#timer-Form').serialize();
        //var AssignedTo = $('#AssignedTo').val();

        app_query.doFormSubmit('#timer-Form', actionurl, formData, function (data) {

            if (data.Status > 0) {

                slf.TaskTimerId = data.OutputId;
                $("#TaskTimerId").val(data.OutputId);
                $("#timer-Title").text("מד-זמן: " + slf.TaskTimerId);
                $('#jqxgrid3').jqxGrid('source').dataBind();

                $.JStopwatch.startTimer('timer', 'sw');//stopwatch
                slf.timerState = 'started';
                $("#timer-sw_start").text("עצור");
                $("#timer-Submit").hide();
                $("#timer-Cancel").hide();
            }
            else
                app_messenger.Post(data, 'error');

        });

        //app_query.doDataPost ('#timer-Form', data, callback, args) {
    };

    app_task_timer.prototype.doSubmit = function () {
        //e.preventDefault();

        //var actionurl = $('#timer-Form').attr('action');

        app_query.doFormSubmit("#timer-Form", null, null, function (data) {

            if (data.Status > 0) {
                this.timerState = 'idle';
                app_task.triggerSubTaskCompleted('timer', data);
            }
            else
                app_messenger.Post(data, 'error');
        });


        ////var AssignedTo = $('#AssignedTo').val();
        //var validationResult = function (isValid) {
        //    if (isValid) {
        //        $.ajax({
        //            url: actionurl,
        //            type: 'post',
        //            dataType: 'json',
        //            data: $('#timer-Form').serialize(),
        //            success: function (data) {
        //                if (data.Status > 0) {
        //                    app_task.triggerSubTaskCompleted('timer', data);
        //                }
        //                else
        //                    app_messenger.Post(data, 'error');
        //            },
        //            error: function (jqXHR, status, error) {
        //                app_messenger.Post(error, 'error');
        //            }
        //        });
        //    }
        //}
        //$('#timer-Form').jqxValidator('validate', validationResult);
    };

    return app_task_timer;

}());

//============================================================ app_task_form_control

var app_task_form = (function () {

    var app_task_form = function ($element) {
        this.$element = $($element);
        var slf = this;
        return this;
    }

    app_task_form.tag = function ($element, dataModel) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";

        var html = '<div id="form-Window" class="container" style="margin:5px">' +
                    '<hr style="width:100%;border:solid 1px #15C8D8">' +
                    '<div id="form-Header">' +
                        '<span id="form-Title" style="font-weight: bold;">פעולות</span>' +
                    '</div>' +
                    '<div id="form-Body">' +
                        '<form class="fcForm" id="form-Form" method="post" action="/System/TaskFormUpdate">' +
                            '<div style="direction: rtl; text-align: right">' +
                                '<input type="hidden" id="ItemId" name="ItemId" value="0" />' +
                                '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                '<input type="hidden" id="form-UserId" name="UserId" value="" />' +
                                '<input type="hidden" id="AssignBy" name="AssignBy" value="" />' +
                                '<input type="hidden" id="Duration" name="Duration" value="" />' +
                                '<div style="height:5px"></div>' +
                                '<div id="tab-content" class="tab-content" dir="rtl">' +
                                    '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">לביצוע</div>' +
                                    '<div class="form-group">' +
                                        '<div class="field">משימה:</div>' +
                                        '<textarea id="ItemText" name="ItemText" class="text-content"' + (dataModel.Option != "a" ? " readonly=\"true\"" : "") + '></textarea>' +
                                    '</div>' +
                                    '<div class="form-group' + (dataModel.Id == 0 ? " pasive" : "") + '">' +
                                        '<div class="form-group">' +
                                            '<div class="field">נוצר בתאריך:</div>' +
                                            '<input id="ItemDate" name="ItemDate" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                        '<div class="form-group">' +
                                            '<div class="field">מועד התחלה:</div>' +
                                            '<input id="StartDate" name="StartDate" type="text" readonly="readonly" class="text-mid" />' +
                                            '<a id="form-Start" href="#" class="btn-bar"><i class="fa fa-chevron-left"></i>התחל</a> ' +
                                        '</div>' +
                                        '<div id="form-Done-group" class="form-group">' +
                                        '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">סיום ביצוע</div>' +
                                        '<div id="divDoneDate" class="form-group">' +
                                            '<div class="field">מועד סיום:</div>' +
                                            '<input id="DoneDate" name="DoneDate" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                        '<div id="form-Done" class="form-group">' +
                                            '<div class="field"><a id="form-comment-toggle" href="#" ><i class="fa fa-plus-square-o"></i></a><span>הערות ביצוע:</span></div>' +
                                            '<div id="form-Comment">' +
                                            '<textarea id="DoneComment" name="DoneComment" class="text-content"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '></textarea>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div class="form-group">' +
                                            '<div class="field">' +
                                                'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '/>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div id="divDisplayName" class="form-group">' +
                                            '<div class="field">בוצע ע"י:</div>' +
                                            '<input id="form-DisplayName" name="DisplayName" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
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
        $element.html(html);
    };

    app_task_form.prototype.init = function (dataModel, userInfo,readonly) {

        this.TaskId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;
        app_task_form.tag(slf.$element, dataModel);

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        var input_rules = [
             { input: '#ItemText', message: 'חובה לציין נושא!', action: 'none', rule: 'required' },
        ];

        $('#form-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });

        $('#form-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#form-Cancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#form-Start').on('click', function (e) {
            slf.doStart();
        });
        //$('#form-comment-toggle').on('click', function (e) {
        //    $("#form-Comment").toggle();
        //});

        var load = function (slf) {

            //if (this.viewAdapter == null) {
            var view_source =
            {
                datatype: "json",
                id: 'ItemId',
                data: { 'id': slf.ItemId },
                type: 'POST',
                url: '/System/GetTaskFormEdit'
            };

            var viewAdapter = new $.jqx.dataAdapter(view_source, {
                loadComplete: function (record) {
                    app_jqxform.loadDataForm("form-Form", record);
                    // $("form#form-Form [name=UserId]").val(record.UserId);

                    //var itemDate = app.toLocalDateString(record.ItemDate);
                    $("#ItemText").prop("readonly",slf.ReadOnly);

                    $('#Duration').val(record['Duration']);
                    $("#ItemDate").val(app.toLocalDateString(record.ItemDate));
                    $('#StartDate').val(app.toLocalDateString(record.StartDate));
                    $("#DoneDate").val(app.toLocalDateTimeString(record.DoneDate));
                    //$("#form-Comment").hide();
                    $("#form-Title").text("פעולה: " + slf.ItemId);

                    if (record.StartDate) {
                        $("#form-Start").hide();
                        $("#form-Done-group").show();
                    }
                    else {
                        $("#form-Start").show();
                        $("#form-Done-group").hide();

                    }
                    if (record.DoneStatus == false) {
                        $("#divDoneDate").hide();
                        $("#divDisplayName").hide();
                    }
                    else {
                        //$("#DoneStatus").prop('disabled', true)
                        $("#DoneComment").prop('readonly', true)
                        $("#form-Submit").hide();
                        $("#form-Start").hide();
                    }
                },
                loadError: function (jqXHR, status, error) {
                },
                beforeLoadComplete: function (records) {
                }
            });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'id': slf.ItemId };
            //}
            viewAdapter.dataBind();

        };

        if (this.ItemId > 0) {
            load(this);
        }
        else {
            $("#form-Form input[name=Task_Id]").val(this.TaskId);
            $("#form-Form input[name=UserId]").val(this.UserInfo.UserId);
            $("#form-Title").text("פעולה: " + "חדשה");
            //$('#form-Submit').text("התחל");
        }
    };

    app_task_form.prototype.display = function () {
        this.$element.show();
        $("#jqxgrid4-bar").hide();
    };

    app_task_form.prototype.doCancel = function () {
        $("#jqxgrid4-bar").show();
        app_task.triggerSubTaskCompleted('form');
    };

    app_task_form.prototype.doSubmit = function () {
        //e.preventDefault();
        var formData = $("#form-Form").serialize();
        var actionurl = $('#form-Form').attr('action');
        app_query.doFormSubmit("#form-Form", actionurl, null, function (data) {

            if (data.Status > 0) {
                app_task.triggerSubTaskCompleted('form', data);
            }
            else
                app_messenger.Post(data, 'error');
        });

        //var started = $("#StartDate").val();

        //if (started) {

        //    app_query.doFormSubmit("#form-Form", actionurl, null, function (data) {

        //        if (data.Status > 0) {
        //            app_task.triggerSubTaskCompleted('form', data);
        //        }
        //        else
        //            app_messenger.Post(data, 'error');
        //    });
        //}
        //else {

        //    app_query.doDataPost("/System/TaskFormStart", { 'id': this.ItemId }, function (data) {
        //        if (data.Status > 0) {
        //            $('#StartDate').val(app.toLocalDateTimeString(Date.now()));
        //            $("#jqxgrid4").jqxGrid('source').dataBind();
        //            //app_task.triggerSubTaskCompleted('form', data);
        //        }
        //    });
        //}

        ////var AssignedTo = $('#AssignedTo').val();
        //var validationResult = function (isValid) {
        //    if (isValid) {
        //        $.ajax({
        //            url: actionurl,
        //            type: 'post',
        //            dataType: 'json',
        //            data: $('#form-Form').serialize(),
        //            success: function (data) {
        //                if (data.Status > 0) {
        //                    app_task.triggerSubTaskCompleted('form', data);
        //                }
        //                else
        //                    app_messenger.Post(data, 'error');
        //            },
        //            error: function (jqXHR, status, error) {
        //                app_messenger.Post(error, 'error');
        //            }
        //        });
        //    }
        //}
        //$('#form-Form').jqxValidator('validate', validationResult);
    };

    app_task_form.prototype.doStart = function () {
        //e.preventDefault();

        app_query.doDataPost("/System/TaskFormStart", { 'id': this.ItemId }, function (data) {
            if (data.Status > 0) {
                $('#StartDate').val(app.toLocalDateString(Date.now()));
                $("#form-Done-group").show();
                $("#form-Start").hide();
                $("#jqxgrid4").jqxGrid('source').dataBind();
                //app_task.triggerSubTaskCompleted('form', data);
            }
        });
    };

    return app_task_form;

}());



/*
var app_task_trigger = (function () {

    var app_task_trigger = function () { }

    app_task_trigger.triggerTaskCommentCompleted=function(data) {
        app_task_comment.end(data);
    };
    app_task_trigger.triggerTaskAssignCompleted = function (data) {
        app_task_assign.end(data);
    };
    app_task_trigger.triggerTaskTimerCompleted=function(data) {
        app_task_timer.end(data);
    };
    app_task_trigger.triggerTaskFormCompleted=function(data) {
        app_task_form.end(data);
    };
    app_task_trigger.triggerSubTaskCompleted=function(type, data) {
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
    };
 
    return app_task_trigger;

}());
*/




//var loadElements = function () {
//    $("#Task_Parent").val(slf.TaskParentId);
//    if (slf.TaskParentId > 0)
//    {
//        $("#Task_Parent-group").show();
//        $("#Task_Parent-link").click(function () {
//            app.redirectTo('/System/TaskInfo?id=' + slf.TaskParentId);
//        });
//    }
//    else
//    {
//        $("#Task_Parent-group").hide();
//    }


//    $("#AccountId").val(slf.AccountId);
//    //$("#hTitle-text").text(slf.Title + ': ' + slf.TaskId);

//    //$("#hxp-title").text(slf.Title + ': ' + slf.TaskId);

//    if (theme === undefined)
//        theme = 'nis_metro';


//    $("#accordion").jcxTabs({
//        rotate: false,
//        startCollapsed: 'accordion',
//        collapsible: 'accordion',
//        click: function (e, tab) {
//            //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
//        },
//        activate: function (e, tab) {
//            //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

//            switch (tab.id)
//            {
//                case 1:
//                    if (slf.TaskId == 0) {
//                        event.preventDefault();
//                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
//                        return false;
//                    }
//                    if ($("#jqxgrid1")[0].childElementCount == 0)
//                        slf.comment.load(slf.Model, slf.UserInfo);
//                    break;
//                case 2:
//                    if ($("#jqxgrid2")[0].childElementCount == 0)
//                        slf.assign.load(slf.Model, slf.UserInfo);
//                    break;
//                case 3:
//                    if ($("#jqxgrid3")[0].childElementCount == 0)
//                        slf.timer.load(slf.Model, slf.UserInfo);
//                    break;
//                case 4:
//                    if (slf.TaskId == 0) {
//                        event.preventDefault();
//                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
//                        return false;
//                    }
//                    if (slf.IsNew) {
//                        var source = $('#Form_Type').jqxComboBox('source');
//                        if (source == null)
//                            app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetTaskFormTypeList', 0, 120, false);
//                        if ($("#jqxgrid4")[0].childElementCount == 0)
//                            app_task.form_template.load(slf.TaskId, slf.UserInfo);

//                    }
//                    if ($("#jqxgrid4")[0].childElementCount == 0)
//                        slf.form.load(slf.Model, slf.UserInfo);
//                    break;
//                case 5:
//                    if (slf.TaskId == 0) {
//                        event.preventDefault();
//                        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
//                        return false;
//                    }

//                    if (slf.uploader == null) {
//                        slf.uploader = new app_media_uploader("#task-files");
//                        slf.uploader.init(slf.TaskId, 't');
//                        slf.uploader.show();
//                    }

//                    //if ($("#iframe-files").attr('src') === undefined)
//                    //    var op = slf.Model.Option;
//                    //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t&op=' + op, '100%', '350px', true);

//                    break;
//            }

//        },
//        activateState: function (e, state) {
//            //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
//        }
//    });

//    //$("#exp-0").jqxExpander({ rtl: true, width: '100%', theme: theme, animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: true });
//    //$("#exp-1").jqxExpander({ rtl: true, width: '100%', theme: theme, animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });
//    //$("#exp-2").jqxExpander({ rtl: true, width: '100%', theme: theme, animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });
//    //$("#exp-3").jqxExpander({ rtl: true, width: '100%', theme: theme, animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });
//    //$("#exp-4").jqxExpander({ rtl: true, width: '100%', theme: theme, animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });
//    //$("#exp-5").jqxExpander({ rtl: true, width: '100%', theme: theme, animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });



//    $("#jqxExp-1").jqxExpander({ rtl: true, width: '80%', expanded: false });
//    $('#jqxExp-1').on('expanding', function () {

//        if (!exp1_Inited) {


//            app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', slf.ClientId, function () {

//            });
//            app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', slf.ProjectId, function () {

//            });
//            //app_jqx_adapter.createComboArrayAsync("Tags", "#Tags", '/System/GetTagsList', null, 300, 0, true, slf.Tags, function () {

//            //});
//            app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 300, 0, true, slf.Tags, function () {

//            });
//            app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 300, 0, null, slf.AssignTo, function () {
//                console.log('callback');
//            });
//             //$("#AssignTo").keydown(function (event) {
//             //   if (event.which == 8) {
//             //       event.preventDefault();
//             //   }
//             //});

//            //app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
//            //app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');
//        }
//        //else {
//        //    if ($('#ClientId').val() != '' && $('#ClientId-display').val() == '') {
//        //        app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
//        //    }
//        //    if ($('#Project_Id').val() != '' && $('#Project_Id-display').val() == '') {
//        //        app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');
//        //    }
//        //}

//        exp1_Inited = true;
//    });


//    $("#ColorFlag").simplecolorpicker();
//    $("#ColorFlag").on('change', function () {
//        //$('select').simplecolorpicker('destroy');
//        var color = $("#ColorFlag").val();
//        $("#hTitle").css("background-color", color)
//    });
//    $("#TaskStatus").jqxDropDownList({ enableSelection: false, disabled: true, rtl: true });
//    //$('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });
//    //$("#Task_Type").jqxComboBox({ disabled: !this.IsEditable });
//    //$("#TaskSubject").prop('disabled', !this.IsEditable);
//    //$("#AssignTo").prop('disabled', !this.IsEditable);
//    //$("#Task_Type").prop('disabled', !this.IsEditable);

//    $('#TaskBody').jqxEditor({
//        height: '300px',
//        //width: '100%',
//        editable: slf.IsEditable,
//        rtl: true,
//        tools: 'bold italic underline | color background | left center right'
//        //theme: 'arctic'
//        //stylesheets: ['editor.css']
//    });

//}
