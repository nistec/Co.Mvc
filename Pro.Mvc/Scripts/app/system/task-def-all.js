'use strict';

//============================================================================================ app_task_def

function app_task_demo() {
    var task = new app_task();
    task.doCancel();
    task.comment.end();
    app_task.triggerSubTaskCompleted();
    //app_task_trigger.
}


class app_task_base {

    constructor(dataModel, userInfo, taskModel) {

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
        this.AssignBy = (dataModel.AssignBy) ? dataModel.AssignBy : 0;
        this.TaskStatus = (dataModel.TaskStatus) ? dataModel.TaskStatus : 0;
        this.IsEditable = (this.TaskId == 0) || ((this.TaskStatus < 8) && (this.AssignBy == this.UserInfo.UserId || this.Option == 'e'));
        this.EnableFormType = this.IsNew;
        this.ClientId = 0;
        this.ProjectId = 0;
        this.Tags;
        this.AssignTo;
        this.SrcUserId = 0;
        this.exp1_Inited = false;
        this.Comment = null;
        this.Assign = null;
        this.Timer = null;
        this.Actions = null;
        this.FormTemplate = null;
        if (this.Option == 'g') {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }
    }

    doSettings(record,isInfo) {

        if (record.TaskStatus <= 0)
            record.TaskStatus = 1;

        this.AssignBy = record.AssignBy;
        this.TaskStatus = record.TaskStatus;
        this.IsEditable = (this.TaskId == 0) || ((this.TaskStatus < 8) && (this.AssignBy == this.UserInfo.UserId || this.Option == 'e'));

        this.ProjectId = record.Project_Id;
        this.ClientId = record.ClientId;
        this.Tags = record.Tags;
        this.AssignTo = record.AssignTo;
        this.SrcUserId = record.UserId;


        if (this.SrcUserId > 0)
            app_jqx_combo_async.userInputAdapter("#UserId", this.SrcUserId);
        if (isInfo) {
            //var model = this.Model.Data
            if (record.Comments == 0)
                $("#exp-1").hide();
            if (record.Assigns == 0)
                $("#exp-2").hide();
            if (record.Timers == 0)
                $("#exp-3").hide();
            if (record.Items == 0)
                $("#exp-4").hide();
            if (record.Files == 0)
                $("#exp-5").hide();
        }
    }

    _loadData(isInfo) {

        var _slf = this;
        var url = isInfo ? "/System/GetTaskInfo" : "/System/GetTaskEdit";
        var view_source =
            {
                datatype: "json",
                id: 'TaskId',
                data: { 'id': _slf.TaskId },
                type: 'POST',
                url: url
            };

        var viewAdapter = new $.jqx.dataAdapter(view_source, {
            loadComplete: function (record) {

                _slf.doSettings(record, isInfo);
                _slf.loadControls(record);
                //slf.loadEvents();
            },
            loadError: function (jqXHR, status, error) {
                app_dialog.alert("error loading task data");
            },
            beforeLoadComplete: function (records) {
            }
        });

        if (this.TaskId > 0) {
            viewAdapter.dataBind();
        }
    }
    
    sectionSettings(id) {

        var slf = this;
        switch (id) {
            case 1:
                if (this.TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                    return false;
                }
                if (this.Comment == null) {//($("#jqxgrid1")[0].childElementCount == 0) {
                    this.Comment = new app_task_comment_grid(this.TaskId, this.UserInfo, this.Option);
                    //this.Comment.load(this.TaskId, this.UserInfo, this.Option);
                }
                break;
            case 2:
                if (this.Assign == null) {//($("#jqxgrid2")[0].childElementCount == 0)
                    this.Assign = new app_task_assign_grid(this.TaskId, this.UserInfo, this.Option);
                }
                break;
            case 3:
                if (this.Timer == null)//($("#jqxgrid3")[0].childElementCount == 0)
                    this.Timer = new app_task_timer_grid(this.TaskId, this.UserInfo, this.Option);
                break;
            case 4:
                if (this.TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                    return false;
                }

                if (this.TaskModel == 'P') {
                    if (this.Actions == null)//($("#jqxgrid4")[0].childElementCount == 0)
                        this.Actions = new app_topic_form_grid(this.TaskId, this.UserInfo, this.Option);
                }
                else {
                    if (this.Actions == null)//($("#jqxgrid4")[0].childElementCount == 0)
                        this.Actions = new app_task_form_grid(this.TaskId, this.UserInfo, this.Option);


                    if (this.EnableFormType) {
                        var source = $('#Form_Type').jqxComboBox('source');
                        if (source == null)
                            app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetTaskFormTypeList', 0, 120, false);

                        //if (this.FormTemplate == null) {//($("#jqxgrid4")[0].childElementCount == 0)
                        //    this.FormTemplate = new app_task_form_template(); //app_task.form_template.load(this.TaskId, this.UserInfo);
                        //    this.FormTemplate.load(this.TaskId, this.UserInfo);
                        //}
                    }
                }
                break;
            case 5:
                if (this.TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                    return false;
                }

                if (this.uploader == null) {
                    this.uploader = new app_media_uploader("#task-files");
                    this.uploader.init(this.TaskId, 't', !this.IsEditable);
                    this.uploader.show();
                }

                //if ($("#iframe-files").attr('src') === undefined)
                //    var op = this.Model.Option;
                //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + this.TaskId + '&refType=t&op=' + op, '100%', '350px', true);

                break;
        }
    }

    lazyLoad() {

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', this.ClientId, function () {

        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', this.ProjectId, function () {

        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 225, 0, true, this.Tags, function () {

        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, this.AssignTo, function () {
            $("#AssignTo").jqxComboBox({ disabled: true });
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
            //$("#ShareType").jqxDropDownList({ enableSelection: false });
            app.disableSelect("#ShareType");
        }
    }

    doCancel() {

        app.redirectTo(app_task_base.getReferrer());
        //return this;
    }

    static getReferrer() {

        var referrer = document.referrer;
        if (referrer) {

            if (referrer.match(/System\/TaskUser/gi))
                return "/System/TaskUser";
            else if (referrer.match(/System\/ReportTasks/gi))
                return "/System/ReportTasks";
            else if (referrer.match(/System\/ReportSubTask/gi))
                return "/System/ReportSubTask";
            else if (referrer.match(/System\/ReportTopics/gi))
                return "/System/ReportTopics";
            else {
                return "/System/TaskUser";
            }
        }
        else {
            return "/System/TaskUser";
        }
        //return this;
    }

    loadControls(record) {

    }

    //============================================================ app_tasks global

    static tasks_edit_view_comment(row) {

        var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
        content = content.replace("\n", "<br/>");
        app_jqx.toolTipClick(".task-comment", '<p>' + content + '</p>', 350);
    };

    static setTaskButton(item, action, visible) {
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

    triggerTaskCommentCompleted(data) {
        this.Comment.end(data);
    };
    triggerTaskAssignCompleted(data) {
        this.Assign.end(data);
    };
    triggerTaskTimerCompleted(data) {
        this.Assign.end(data);
    };
    triggerTaskFormCompleted(data) {
        this.Actions.end(data);
    };
    static triggerSubTaskCompleted(type, data) {
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
}


//============================================================ app_task

class app_task extends app_task_base {

    constructor(dataModel, userInfo, taskModel) {
        
        super(dataModel, userInfo, taskModel);
       
    }

    init(isInfo) {

        if (isInfo) {
            $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
            $("#hxp-title").text(this.Title + ': ' + this.TaskId);
        }
        else {

            this.tabSettings();
        }

        this.preLoad();

        if (this.TaskId > 0) {
            //this.syncData(dataModel.Data);
            //this.loadControls(this.Model.Data);
            this._loadData(isInfo);
        }
        else {
            this.loadControls();
        }
        this.loadEvents();
    }

    tabSettings() {
        var _slf = this;
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

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            //click: function (e, tab) {
            //    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            //},
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                _slf.sectionSettings(tab.id);
                return false;
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });
    }

    //initInfo() {
        
    //    $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
    //    $("#hxp-title").text(this.Title + ': ' + this.TaskId);

    //    //var model = this.Model.Data
    //    //if (model.Comments == 0)
    //    //    $("#exp-1").hide();
    //    //if (model.Assigns == 0)
    //    //    $("#exp-2").hide();
    //    //if (model.Timers == 0)
    //    //    $("#exp-3").hide();
    //    //if (model.Items == 0)
    //    //    $("#exp-4").hide();
    //    //if (model.Files == 0)
    //    //    $("#exp-5").hide();

    //    //if (this.SrcUserId > 0)
    //    //    app_jqx_combo_async.userInputAdapter("#UserId", this.SrcUserId);

    //    this.preLoad();
    //    this._loadData();
    //    //this.loadControls(model);
    //    this.loadEvents();

    //}

    parentSettings(parentId) {
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
    }

    doSubmit(act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = this.TaskStatus;// app_jqx.getInputAutoValue("#TaskStatus", 1);
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
                app_messenger.Notify(data, 'info', app_task_base.getReferrer());
            }

            if (act == 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {
            
            //app_jqx.setInputAutoValue("#TaskStatus", status);
            app_tasks.setTaskStatus("#TaskStatus", status);

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


            //return this;
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
        //return this;
    }

    preLoad() {

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
         if (theme === undefined)
            theme = 'nis_metro';

        app_tasks.setColorFlag();
        app_tasks.setShareType();

        //$("#jqxExp-1").jqxExpander({ rtl: true, width: '80%', expanded: false });
        //$('#jqxExp-1').on('expanding', function () {

        //    if (!slf.exp1_Inited) {
        //        slf.lazyLoad();
        //    }
        //});

        $('#a-jqxExp-1').on('click', function (e) {
            if (!slf.exp1_Inited) {
                slf.lazyLoad();
            }
            $('#jqxExp-box').slideToggle();
            return false;
        });


        $("#ColorFlag").simplecolorpicker();
        $("#ColorFlag").on('change', function () {
            //$('select').simplecolorpicker('destroy');
            var color = $("#ColorFlag").val();
            $("#hTitle").css("background-color", color)
        });

        $('#TaskBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });
        $('#TaskBody-btn-view').on('click', function () {

            if ($('#TaskBody-div').hasClass("editor-view")) {
                $('#TaskBody-div').removeClass("editor-view");
                $('#TaskBody').jqxEditor('height', '300px');
                $('#TaskBody').css('height', '305px');
            }
            else {
                $('#TaskBody-div').addClass("editor-view");
                $('#TaskBody').css('height', '805px');
                $('#TaskBody').jqxEditor('height', '800px');
            }
        });
    }

    loadControls(record) {


        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

        //$("#ShareType").jqxDropDownList({ autoDropDownHeight: true, width: 120, height: 25 ,rtl:true});
        //$("#ShareType").on('change', function (event) {

        //    var index=$(this).val();

        //        //var args = event.args;
        //        //var index = args.item.index;
        //        var disableAssign = (index !== 3)
        //        $("#AssignTo").jqxComboBox({ disabled: disableAssign }); 
        //        if (index === 3) {
        //            //$("#AssignTo").jqxComboBox('open');
        //        }
        //        else {
        //            $("#AssignTo").jqxComboBox('uncheckAll'); 
        //        }
        //});

        

        $("#ShareType").change(function () {
            var index = $('option:selected', this).val();

            if (index === "3") {
                $("#AssignTo").jqxComboBox({ disabled: false });
                //$("#AssignTo").jqxComboBox('open');
            }
            else {
                $("#AssignTo").jqxComboBox('uncheckAll');
                $("#AssignTo").jqxComboBox({ disabled: true });
            }
        });

        if (record) {
           

            //this.doSettings(record);
            app_form.loadDataForm("fcForm", record, ["Project_Id", "ClientId", "Tags", "AssignTo"]);
            
            $("#AssignTo").jqxComboBox({ disabled: record.ShareType !== 3 });
            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
            
            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });
            this.parentSettings(record.Task_Parent);

            if (this.Option !== 'g' && record.TaskStatus > 1 && record.TaskStatus < 8)
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
            //app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);

            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);
            
        }
        else {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');

        }
        //return this;
    }

    loadEvents() {

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
                slf.doSubmit('plus');
            });

            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    //app_tasks_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", {
                            'TaskId': slf.TaskId, 'FormId': args.item.value
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
                app_query.doDataPost("/System/TaskFormTemplateCreate", {
                    'id': slf.TaskId, 'name': name
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
                slf.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    slf.doSubmit('plus');
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
    }
}

//============================================================ app_topic

class app_topic extends app_task_base {

    constructor(dataModel, userInfo, taskModel) {

        super(dataModel, userInfo, taskModel);
        this.EnableFormType = false;
    }

    init(isInfo) {

        if (isInfo) {
            $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
            $("#hxp-title").text(this.Title + ': ' + this.TaskId);
        }
        else {
            this.tabSettings();
        }

        this.preLoad();

        if (this.TaskId > 0) {
            this._loadData(isInfo);
            //this.loadControls(this.Model.Data);
        }
        else {
            this.loadControls();
        }
        this.loadEvents();

        //return this;
    };
   
    tabSettings() {
        var _slf = this;
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

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            click: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            },
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                switch (tab.selector) {
                    case "#exp-1":
                        _slf.sectionSettings(1);
                        break;
                    case "#exp-4":
                        _slf.sectionSettings(4);
                        break;
                    case "#exp-5":
                        _slf.sectionSettings(5);
                        break;
                }
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });
    };
 
    //initInfo() {

    //    $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
    //    $("#hxp-title").text(this.Title + ': ' + this.TaskId);

    //    var model = this.Model.Data
    //    if (model.Comments == 0)
    //        $("#exp-1").hide();
    //    if (model.Assigns == 0)
    //        $("#exp-2").hide();
    //    if (model.Timers == 0)
    //        $("#exp-3").hide();
    //    if (model.Items == 0)
    //        $("#exp-4").hide();
    //    if (model.Files == 0)
    //        $("#exp-5").hide();

    //    this.preLoad();
    //    this.loadControls(model);
    //    this.loadEvents();
    //};

    doCancel() {
        app.redirectTo(app_task_base.getReferrer());
        //return this;
    };

    doSubmit(act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = this.TaskStatus;// app_jqx.getInputAutoValue("#TaskStatus", 1);
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
                app_messenger.Notify(data, 'info', app_task_base.getReferrer());
            }

            if (act == 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {
            

            //app_jqx.setInputAutoValue("#TaskStatus", status);
            app_tasks.setTaskStatus("#TaskStatus", status);

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

    preLoad() {

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


        $("#AccountId").val(slf.AccountId);
        if (theme === undefined)
            theme = 'nis_metro';

        app_tasks.setColorFlag();

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

   
        $('#TaskBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });
        $('#TaskBody-btn-view').on('click', function () {

            if ($('#TaskBody-div').hasClass("editor-view")) {
                $('#TaskBody-div').removeClass("editor-view");
                $('#TaskBody').jqxEditor('height', '300px');
                $('#TaskBody').css('height', '305px');
            }
            else {
                $('#TaskBody-div').addClass("editor-view");
                $('#TaskBody').css('height', '805px');
                $('#TaskBody').jqxEditor('height', '800px');
            }
        });
    }

    loadControls(record) {

        console.log('controls');

        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

        if (record) {

            app_form.loadDataForm("fcForm", record, ["TaskStatus", "Project_Id", "ClientId", "Tags", "AssignTo"]);

            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));

            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });

            if (this.Option !== 'g' && record.TaskStatus > 1 && record.TaskStatus < 8)
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
            //app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);
            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);
        }
        else {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');//

        }
        //return this;
    }

    loadEvents() {

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
                slf.doSubmit('plus');
            });

            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    //app_tasks_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
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
                app_query.doDataPost("/System/TaskFormTemplateCreate", { 'id': slf.TaskId, 'name': name }, function (data) {
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
                slf.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    slf.doSubmit('plus');
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
    }

}

//============================================================ app_doc

class app_doc extends app_task_base {

    constructor(dataModel, userInfo, taskModel) {

        super(dataModel, userInfo, taskModel);
        this.EnableFormType = false;
    }

    init(isInfo) {

        if (isInfo) {
            $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
            $("#hxp-title").text(this.Title + ': ' + this.TaskId);
        }
        else {
            this.tabSettings();
        }

        this.preLoad();

        if (this.TaskId > 0) {
            this._loadData(isInfo);
            //this.loadControls(this.Model.Data);
        }
        else {
            this.loadControls();
        }
        this.loadEvents();

        //return this;
    };

    tabSettings() {
        var _slf = this;
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

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            click: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            },
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                switch (tab.selector) {
                    case "#exp-1":
                        _slf.sectionSettings(1);
                        break;
                    case "#exp-4":
                        _slf.sectionSettings(4);
                        break;
                    case "#exp-5":
                        _slf.sectionSettings(5);
                        break;
                }
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });
    };

    //initInfo() {

    //    $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
    //    $("#hxp-title").text(this.Title + ': ' + this.TaskId);

    //    var model = this.Model.Data
    //    if (model.Comments == 0)
    //        $("#exp-1").hide();
    //    if (model.Assigns == 0)
    //        $("#exp-2").hide();
    //    if (model.Timers == 0)
    //        $("#exp-3").hide();
    //    if (model.Items == 0)
    //        $("#exp-4").hide();
    //    if (model.Files == 0)
    //        $("#exp-5").hide();

    //    this.preLoad();
    //    this.loadControls(model);
    //    this.loadEvents();
    //};

    doCancel() {
        app.redirectTo(app_task_base.getReferrer());
        //return this;
    };

    doSubmit(act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = this.TaskStatus;// app_jqx.getInputAutoValue("#TaskStatus", 1);
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
                app_messenger.Notify(data, 'info', app_task_base.getReferrer());
            }

            if (act == 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {


            //app_jqx.setInputAutoValue("#TaskStatus", status);
            app_tasks.setTaskStatus("#TaskStatus", status);

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

    preLoad() {

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


        $("#AccountId").val(slf.AccountId);
        if (theme === undefined)
            theme = 'nis_metro';

        app_tasks.setColorFlag();

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


        $('#TaskBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });
        $('#TaskBody-btn-view').on('click', function () {

            if ($('#TaskBody-div').hasClass("editor-view")) {
                $('#TaskBody-div').removeClass("editor-view");
                $('#TaskBody').jqxEditor('height', '300px');
                $('#TaskBody').css('height', '305px');
            }
            else {
                $('#TaskBody-div').addClass("editor-view");
                $('#TaskBody').css('height', '805px');
                $('#TaskBody').jqxEditor('height', '800px');
            }
        });
    }

    loadControls(record) {

        console.log('controls');

        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

        if (record) {

            app_form.loadDataForm("fcForm", record, ["TaskStatus", "Project_Id", "ClientId", "Tags", "AssignTo"]);

            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));

            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });

            if (this.Option !== 'g' && record.TaskStatus > 1 && record.TaskStatus < 8)
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
            //app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);
            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);
        }
        else {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');//

        }
        //return this;
    }

    loadEvents() {

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
                slf.doSubmit('plus');
            });

            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    //app_tasks_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
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
                app_query.doDataPost("/System/TaskFormTemplateCreate", { 'id': slf.TaskId, 'name': name }, function (data) {
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
                slf.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    slf.doSubmit('plus');
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
    }

}

//============================================================ app_ticket

class app_ticket extends app_task_base {

    constructor(dataModel, userInfo, taskModel) {

        super(dataModel, userInfo, taskModel);
        this.EnableFormType = false;
    }

    init(isInfo) {

        var slf = this;

        if (isInfo) {
            $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
            $("#hxp-title").text(this.Title + ': ' + this.TaskId);
        }
        else {
            this.tabSettings();
        }

        this.preLoad();

        if (this.TaskId > 0) {
            this._loadData(isInfo);
            //this.loadControls(this.Model.Data);
        }
        else {
            this.loadControls();
        }
        this.loadEvents();

        //return this;
    };

    tabSettings(isNew) {

        var _slf = this;

        $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
        $("#hxp-title").text(this.Title + ': ' + this.TaskId);

        if (this.IsNew) {
            $("#hxp-1").hide();
            $("#hxp-2").hide();
            $("#hxp-3").hide();
            $("#hxp-4").hide();
            $("#hxp-5").show();
        }
        else {
            $("#hxp-1").show();
            $("#hxp-2").show();
            $("#hxp-3").hide();
            $("#hxp-4").show();
            $("#hxp-5").show();
        }

        if (!this.IsEditable) {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            //click: function (e, tab) {
            //    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            //},
            activate: function (e, tab) {

                switch (tab.selector) {
                    case "#exp-1":
                        _slf.sectionSettings(1);
                        break;
                    case "#exp-2":
                        _slf.sectionSettings(2);
                        break;
                    case "#exp-4":
                        _slf.sectionSettings(4);
                        break;
                    case "#exp-5":
                        _slf.sectionSettings(5);
                        break;
                }
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');
                //slf.sectionSettings(tab.id);
                return false;
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });
    };

    //initInfo() {
    //    $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
    //    $("#hxp-title").text(this.Title + ': ' + this.TaskId);

    //    var model = this.Model.Data
    //    if (model.Comments == 0)
    //        $("#exp-1").hide();
    //    if (model.Assigns == 0)
    //        $("#exp-2").hide();
    //    //if (model.Timers == 0)
    //    //    $("#exp-3").hide();
    //    if (model.Items == 0)
    //        $("#exp-4").hide();
    //    if (model.Files == 0)
    //        $("#exp-5").hide();

    //    this.preLoad();
    //    this.loadControls(model);
    //    this.loadEvents();
    //};

    doCancel() {
        app.redirectTo(app_task_base.getReferrer());
        //return this;
    };

    doSubmit(act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = this.TaskStatus;// app_jqx.getInputAutoValue("#TaskStatus", 1);
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
                app_messenger.Notify(data, 'info', app_task_base.getReferrer());
            }

            if (act == 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {
            
            //app_jqx.setInputAutoValue("#TaskStatus", status);
            app_tasks.setTaskStatus("#TaskStatus", status);

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

    preLoad() {

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


        $("#AccountId").val(slf.AccountId);
        if (theme === undefined)
            theme = 'nis_metro';

        app_tasks.setColorFlag();

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            click: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            },
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                switch (tab.selector) {
                    case "#exp-1":
                        slf.sectionSettings(1);
                        break;
                    case "#exp-4":
                        slf.sectionSettings(4);
                        break;
                    case "#exp-5":
                        slf.sectionSettings(5);
                        break;
                }
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
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
        
        $('#TaskBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });
        $('#TaskBody-btn-view').on('click', function () {

            if ($('#TaskBody-div').hasClass("editor-view")) {
                $('#TaskBody-div').removeClass("editor-view");
                $('#TaskBody').jqxEditor('height', '300px');
                $('#TaskBody').css('height', '305px');
            }
            else {
                $('#TaskBody-div').addClass("editor-view");
                $('#TaskBody').css('height', '805px');
                $('#TaskBody').jqxEditor('height', '800px');
            }
        });
       
    }

    loadControls(record) {

        console.log('controls');

        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

        if (record) {

            app_form.loadDataForm("fcForm", record, ["Project_Id", "ClientId", "Tags", "AssignTo"]);

            $("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
            $("#TaskSubject").val(record.TaskSubject);
            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });

            if (this.Option !== 'g' && record.TaskStatus > 1 && record.TaskStatus < 8)
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
            //app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);
            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);
        }
        else {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');//

        }
        //return this;
    }

    loadEvents() {

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
                slf.doSubmit('plus');
            });

            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    //app_tasks_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
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
                app_query.doDataPost("/System/TaskFormTemplateCreate", { 'id': slf.TaskId, 'name': name }, function (data) {
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
                slf.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    slf.doSubmit('plus');
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
    }

}


//============================================================ app_task_comment

class app_task_comment_grid {

    constructor(taskId, userInfo, option) {
        this.wizardStep = 2;
        this.TaskId = taskId;
        this.Option = (option) ? option : 'e';
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.Control = null;
        this.init();
    }
    init() {
        var slf = this;
        var taskid = this.TaskId;

        var nastedsource = {
            datafields: [
                {
                    name: 'CommentId', type: 'number'
                },
                {
                    name: 'CommentDate', type: 'date'
                },
                {
                    name: 'CommentText', type: 'string'
                },
                {
                    name: 'ReminderDate', type: 'date'
                },
                {
                    name: 'Attachment', type: 'string'
                },
                {
                    name: 'DisplayName', type: 'string'
                },
                {
                    name: 'Task_Id', type: 'number'
                },
                { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'CommentId',
            type: 'POST',
            url: '/System/GetTasksCommentGrid',
            data: {
                'pid': taskid
            }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        //var nastedAdapter = new $.jqx.dataAdapter(nastedsource, {
        //    loadComplete (record) {
        //        $("#hxp-1").text("הערות : " + record.length);
        //    },
        //    loadError (jqXHR, status, error) {
        //    },
        //    beforeLoadComplete (records) {
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
                {
                    text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center'
                },
                { text: 'שם', datafield: 'DisplayName', cellsalign: 'right', align: 'center', width: 100, hidden: app.IsMobile() }
                //{
                //    text: '...', datafield: 'CommentId', width: 120, cellsalign: 'right', align: 'center',
                //              cellsrenderer (row, columnfield, value, defaulthtml, columnproperties) {
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
    }
    showControl(id, option, action) {

        var data_model = {
            PId: this.TaskId, Id: id, Option: option, Action: action
        };

        if (this.Control == null) {
            this.Control = new app_task_comment_item("#jqxgrid1-window");//app_task_comment_control("#jqxgrid1-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
    getrowId() {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    }
    add() {
        //setTaskButton('comment', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_TaskCommentAdd?pid=" + this.TaskId, "100%", "280px");
        this.showControl(0, 'a');

    }
    edit() {
        if (this.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('comment', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "350px");

            this.showControl(id, 'e');
        }
    }
    remove() {
        var id = this.getrowId();
        if (id > 0) {
            app_dialog.confirm('האם למחוק הערה ' + id, function () {
                app_query.doPost(app.appPath() + "/System/TaskCommentDelete", {
                    'id': id
                }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid1').jqxGrid('source').dataBind();
                });
            });

        }
    }
    refresh() {
        $('#jqxgrid1').jqxGrid('source').dataBind();
    }
    cancel() {
        wizard.wizHome();
    }
    end(data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
            // app_dialog.alert(data.Message);
        }
    }
};

class app_task_comment_item {

    constructor($element) {
        this.$element = $($element);
        //var slf = this;
        //return this;
    }

    tag($element, option) {

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

    init(dataModel, userInfo, readonly) {
        this.TaskId = dataModel.PId;
        this.CommentId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(this.$element, dataModel.Option);

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

    display() {
        this.$element.show();
        $("#jqxgrid1-bar").hide()
    };

    doCancel() {

        app_task.triggerSubTaskCompleted('comment');
    };

    doSubmit() {
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
    };

    //return app_task_comment;
}

//============================================================ app_task_assign

class app_task_assign_grid {

    constructor(taskId, userInfo, option) {
        this.wizardStep = 3;
        this.TaskId = taskId;//dataModel.Id;
        //this.Model = dataModel;
        this.Option = (option) ? option : 'e';
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.Control = null;
        this.init();
        //return this;
    }
    init() {
        var slf = this;
        var taskid = this.TaskId;
        var nastedsource = {
            datafields: [
                {
                    name: 'AssignId', type: 'number'
                },
                {
                    name: 'AssignedBy', type: 'number'
                },
                {
                    name: 'AssignedTo', type: 'number'
                },
                {
                    name: 'Task_Id', type: 'number'
                },
                {
                    name: 'AssignDate', type: 'date'
                },
                {
                    name: 'AssignSubject', type: 'string'
                },
                {
                    name: 'AssignedByName', type: 'string'
                },
                { name: 'AssignedToName', type: 'string' }
            ],
            datatype: "json",
            id: 'AssignId',
            type: 'POST',
            url: '/System/GetTasksAssignGrid',
            data: {
                'pid': taskid
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
                {
                    text: 'מועד רישום', datafield: 'AssignDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                },
                {
                    text: 'עבר מ', datafield: 'AssignedByName', width: 120, cellsalign: 'right', align: 'center'
                },
                {
                    text: 'עבר ל', datafield: 'AssignedToName', width: 120, cellsalign: 'right', align: 'center'
                },
                { text: 'נושא', datafield: 'AssignSubject', cellsalign: 'right', align: 'center' }
                //{
                //    text: 'מספר רישום', datafield: 'AssignId', width: 120, cellsalign: 'right', align: 'center',
                //    cellsrenderer (row, columnfield, value, defaulthtml, columnproperties) {
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



    }

    showControl(id, option, action) {

        var data_model = {
            PId: this.TaskId, Id: id, Option: option, Action: action
        };

        if (this.Control == null) {
            this.Control = new app_task_assign_item("#jqxgrid2-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }

    getrowId() {

        var selectedrowindex = $("#jqxgrid2").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid2").jqxGrid('getrowid', selectedrowindex);
        return id;
    };
    add() {
        //setTaskButton('assign', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskAssignAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid2-window", app.appPath() + "/System/_TaskAssignAdd?pid=" + this.TaskId, "100%", "280px");
        this.showControl(0, 'a');
    };
    edit() {
        if (this.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('assign', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskAssignEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid2-window", app.appPath() + "/System/_TaskAssignEdit?id=" + id, "100%", "280px");
            this.showControl(id, 'e');
        }
    }
    //view () {
    //    var id = this.getrowId();
    //    if (id > 0) {
    //        $('#task-item-update').hide();
    //        wizard.appendIframe(2, app.appPath() + "/System/_TaskAssignView?id=" + id, "100%", "500px");
    //    }
    //},
    remove() {
        var id = this.getrowId();
        if (id > 0) {
            app_dialog.confirm('האם למחוק העברה ' + id, function () {
                app_query.doPost(app.appPath() + "/System/TaskAssignDelete", {
                    'id': id
                }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid2').jqxGrid('source').dataBind();
                });
            });
        }
    }
    refresh() {
        $('#jqxgrid2').jqxGrid('source').dataBind();
    }
    cancel() {
        wizard.wizHome();
    }
    end(data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
        }
    }
};

class app_task_assign_item {

    constructor($element) {
        this.$element = $($element);
        //var slf = this;
        //return this;
    }

    tag($element, option) {
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

    init(dataModel, userInfo) {

        this.TaskId = dataModel.PId;
        this.AssignId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        var slf = this;

        //$("#AccountId").val(this.AccountId);
        this.tag(slf.$element, dataModel.Option);

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

    display() {
        this.$element.show();
        $("#jqxgrid2-bar").hide()
    };

    doCancel() {

        app_task.triggerSubTaskCompleted('assign');
    };

    doSubmit() {
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
                            app.redirectTo(app_task_base.getReferrer());
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

    //return app_task_assign;
}

//============================================================ app_task_timer

class app_task_timer_grid {

    constructor(taskId, userInfo, option) {
        this.wizardStep = 4;
        this.TaskId = taskId;//dataModel.Id;
        //this.Model = dataModel;
        this.Option = (option) ? option : 'e';
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.Control = null;
        this.init();
        //return this;
    }
    init() {
        var slf = this;
        var taskid = this.TaskId;

        var nastedsource = {
            datafields: [
                {
                    name: 'TaskTimerId', type: 'number'
                },
                {
                    name: 'SubIndex', type: 'number'
                },
                {
                    name: 'Duration', type: 'number'
                },
                {
                    name: 'DurationView', type: 'string'
                },
                {
                    name: 'Task_Id', type: 'number'
                },
                {
                    name: 'StartTime', type: 'date'
                },
                {
                    name: 'EndTime', type: 'date'
                },
                {
                    name: 'Subject', type: 'string'
                },
                {
                    name: 'UserId', type: 'number'
                },
                { name: 'DisplayName', type: 'string' }
            ],
            datatype: "json",
            id: 'TaskTimerId',
            type: 'POST',
            url: '/System/GetTasksTimerGrid',
            data: {
                'pid': taskid
            }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
        //var nastedAdapter = new $.jqx.dataAdapter(nastedsource, {
        //    loadComplete (record) {
        //        $("#hxp-3").text("מעקב זמנים : " + record.length);
        //    },
        //    loadError (jqXHR, status, error) {
        //    },
        //    beforeLoadComplete (records) {
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
                {
                    text: 'מס-סידורי', datafield: 'SubIndex', width: 120, cellsalign: 'right', align: 'center'
                },
                {
                    text: 'מועד התחלה', datafield: 'StartTime', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                },
                {
                    text: 'מועד סיום', datafield: 'EndTime', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                },
                {
                    text: 'משך זמן', datafield: 'DurationView', width: 120, cellsalign: 'right', align: 'center'
                    //aggregates: ['sum']
                },
                { text: 'נושא', datafield: 'Subject', cellsalign: 'right', align: 'center' }
                //{
                //    text: 'מספר רישום', datafield: 'TaskTimerId', width: 120, cellsalign: 'right', align: 'center',
                //    cellsrenderer (row, columnfield, value, defaulthtml, columnproperties) {
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
    }
    getrowId() {
        var selectedrowindex = $("#jqxgrid3").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid3").jqxGrid('getrowid', selectedrowindex);
        return id;
    }
    showControl(id, option, action) {

        var data_model = {
            PId: this.TaskId, Id: id, Option: option, Action: action
        };

        if (this.Control == null) {
            this.Control = new app_task_timer_item("#jqxgrid3-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
    add() {
        //setTaskButton('timer', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskTimerAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid3-window", app.appPath() + "/System/_TaskTimerAdd?pid=" + this.TaskId, "100%", "280px");
        this.showControl(0, 'a');
    }
    edit() {
        if (this.Option != "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setTaskButton('timer', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_TaskTimerEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid3-window", app.appPath() + "/System/_TaskTimerEdit?id=" + id, "100%", "450px" );
            this.showControl(id, 'e');
        }
    }
    remove() {
        var id = this.getrowId();
        if (id > 0) {
            app_dialog.confirm('האם למחוק מעקב זמנים ' + id, function () {
                app_query.doPost(app.appPath() + "/System/TaskTimerDelete", {
                    'id': id
                }, function (data) {
                    if (data.Status > 0) {
                        $('#jqxgrid3').jqxGrid('source').dataBind();
                        //app_messenger.Post("רשומה נמחקה");
                    }
                });
            });
        }
    }
    refresh() {
        $('#jqxgrid3').jqxGrid('source').dataBind();
    }
    cancel() {
        wizard.wizHome();
    }
    end(data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
        }
    }

};

class app_task_timer_item {

    constructor($element) {
        this.$element = $($element);
        this.timerState = 'idle';//idle|started|stoped
        //var slf = this;
        //return this;
    }

    tag($element, option) {
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
            '<input type="text" id="StartTime" name="StartTime" readonly="readonly" data-type="datetime" />' +
            '</div>' +
            '<div id="divEndTime" class="form-group' + pasive + '">' +
            '<div class="field">מועד סיום:</div>' +
            '<input type="text" id="EndTime" name="EndTime" readonly="readonly" data-type="datetime" />' +
            '</div>' +
            '<div id="divDuration" class="form-group' + pasive + '">' +
            '<div class="field">משך זמן:</div>' +
            '<input id="Duration" name="Duration" type="number" readonly="readonly" class="text-mid" />' +
            '</div>' +
            '<div class="form-group' + pasive + '">' +
            '<div class="field">נוצר ע"י:</div>' +
            '<input id="DisplayName" type="text" disabled="disabled" class="text-mid" data-field="DisplayName" />' +
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

    init(dataModel, userInfo, readonly) {

        this.TaskId = dataModel.PId;
        this.TaskTimerId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(this.$element, dataModel.Option);

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

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
                    app_form.loadDataForm("timer-Form", record);

                    //app_jqxform.loadDataForm("timer-Form", record, true, ["EndTime"]);
                    //$("#timer-Form input[id=DisplayName]").val(record.DisplayName);
                    $("#timer-Title").text("מד-זמן: " + slf.TaskTimerId);
                    ////$('#StartTime').val(record.StartTime);
                    //$('#StartTime').val(app.formatDateTimeString(record.StartTime));
                    if (app.isNull(record.EndTime, "") == "") {
                        $('#divEndTime').hide();
                        $('#divDuration').hide();
                        $('#timer-Submit').text("סיום");
                        //$('#timer-sw_start').text("המשך");
                    }
                    //else {
                    //    //$('#EndTime').val(record.EndTime);
                    //    $('#EndTime').val(app.formatDateTimeString(record.EndTime));


                    //    //    $('#timer-Submit').hide();
                    //    //    $('#timer-form > #Subject').prop('readonly', true);
                    //    //    var sub= app_jqxform.findInputInForm ("timer-Form", "Subject");//, attrName, inputType) {
                    //    //    sub.prop('readonly', true);
                    //}

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

    display() {
        this.$element.show();
        $("#jqxgrid3-bar").hide()
    };

    doCancel() {
        this.timerState = 'idle';
        app_task.triggerSubTaskCompleted('timer');
    };

    doStart() {
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
        else if (this.timerState == 'idle') {
            if (this.TaskTimerId > 0) {
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

    doSubmit() {
        //e.preventDefault();

        //var actionurl = $('#timer-Form').attr('action');
        var _slf = this;
        app_query.doFormSubmit("#timer-Form", null, null, function (data) {

            if (data.Status > 0) {
                app_task.triggerSubTaskCompleted('timer', data);
                _slf.timerState = 'idle';
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

    //return app_task_timer;

}

//============================================================ app_task_form

class app_task_form_grid {

    constructor(taskId, userInfo, option) {
        this.wizardStep = 2;
        this.TaskId = taskId;//dataModel.Id;
        this.UserId = userInfo.UserId;
        this.Option = (option) ? option : 'e';
        this.UInfo = userInfo;
        this.isMobile = app.IsMobile();
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.Control = null;
        this.Import = null;
        this.init();
        //return this;
    }
    init() {
        var slf = this;
        var taskid = this.TaskId

        var actionSource = {
            datafields: [
                { name: 'ItemId', type: 'number' },
                { name: 'ItemDate', type: 'date' },
                { name: 'ItemText', type: 'string' },
                { name: 'DoneDate', type: 'date' },
                { name: 'StartDate', type: 'date' },
                { name: 'DoneStatus', type: 'bool' },
                { name: 'DisplayName', type: 'string' },
                { name: 'Task_Id', type: 'number' },
                { name: 'AssignBy', type: 'number' },
                //{ name: 'ItemDueDate', type: 'date' },
                //{ name: 'ItemAssignTo', type: 'number' },
                //{ name: 'ItemTask', type: 'number' },
                //{ name: 'ItemSubject', type: 'string' },
                { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetTasksFormGrid',
            data: { 'pid': taskid }
        }

        var actiondAdapter = new $.jqx.dataAdapter(actionSource);


        $("#jqxgrid4").jqxGrid({
            width: '100%',
            height: 130,
            editable: slf.Option == "e",
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: actiondAdapter, 
            columnsresize: true,
            rtl: true,
            columns: [
                {
                    text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile
                },
                {
                    text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center', editable: false
                },
                {
                    text: 'מועד התחלה', datafield: 'StartDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile
                },
                {
                    text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile
                },
                {
                    text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center'
                },
                { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center', editable: false, hidden: slf.isMobile }
                //{
                //    text: '...', datafield: 'ItemId', width: 120, cellsalign: 'right', align: 'center',
                //    cellsrenderer (row, columnfield, value, defaulthtml, columnproperties) {
                //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
                //    }
                //}
            ]
        });

        //console.log('jqxgrid4');

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

        $('#jqxgrid4-import').click(function () {
            $("#jqxgrid4-bar").hide();
            $('#jqxgrid4-import_window').show();
            slf.doImport();
        });
        $('#importCancel').click(function () {
            $("#jqxgrid4-import_window").hide();
            $("#jqxgrid4-bar").show();
            app_task.triggerSubTaskCompleted('form');

        });
    }
    getRowData() {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return null;
        var data = $('#jqxgrid4').jqxGrid('getrowdata', selectedrowindex);
        return data;
    }
    getrowId() {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid4").jqxGrid('getrowid', selectedrowindex);
        return id;
    }
    showControl(id, option, action) {

        var data_model = {
            PId: this.TaskId, Id: id, Option: option, Action: action
        };

        if (this.Control == null) {
            this.Control = new app_task_form_item("#jqxgrid4-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
    add() {
        //setTaskButton('form', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskFormAdd?pid=" + this.TaskId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormAdd?pid=" + this.TaskId, "100%", "220px", true);
        this.showControl(0, 'a','add');
    }
    edit() {
        if (this.Option != "e")
            return;
        var rowData = this.getRowData();
        if (rowData) {
            var opt = (rowData.AssignBy == this.UserId) ? 'e' : 'r';

            var act = 'edit';
            if (rowData.StartDate == null)
                act = 'start';
            else if (rowData.DoneStatus)
                act = 'done';

            this.showControl(rowData.ItemId, opt, act);
        }
      
        //var id = this.getrowId();
        //if (id > 0) {

        //    //if (rowData) {
        //    //    var editable = (rowData.AssignBy == slf.UserId);
        //    //    app.showIf("#jqxgrid4-remove", editable);
        //    //    app.showIf("#jqxgrid4-edit", editable);
        //    //}

        //    //setTaskButton('form', 'update', true);
        //    //wizard.appendIframe(2, app.appPath() + "/System/_TaskFormEdit?id=" + id, "100%", "500px");

        //    //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormEdit?id=" + id, "100%", "480px", true);
        //    this.showControl(id, 'e');
        //}
    }
    doImport() {

    var _slf = this;
    this.Import = null;

        var appimport = new app_task_form_import(this.TaskId, this.UserId, 0);
    
        appimport.init(function(data) {

        if (data.result.Status > 0) {

            _slf.refresh();
        }

        $("#jqxgrid4-import_window").hide();
        $("#jqxgrid4-bar").show();
        app_task.triggerSubTaskCompleted('form');

    });

        this.Import = appimport ;

       

        //var args = event.args;


        //    app_dialog.confirm("האם ליצור משימות לביצוע מקובץ?", function () {

        //        app_query.doDataPost("/System/TaskFormByTemplate", {
        //            'TaskId': slf.TaskId, 'FormId': args.item.value
        //        },
        //            function (data) {
        //                if (data.Status > 0)
        //                    $('#jqxgrid4').jqxGrid('source').dataBind();
        //            });
        //    });

            //if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
            //    app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
            //        function (data) {
            //            if (data.Status > 0)
            //                $('#jqxgrid4').jqxGrid('source').dataBind();
            //        });
            //}
        //}
    }
    remove() {

        var data = this.getRowData();
        if (data == null)
            return;
        if (this.UserId != data.AssignBy) {
            app_dialog.Alert("לא ניתן למחוק פעולה שהוקצתה על ידי משתמש אחר");
            return;
        }
        var id = data.ItemId;// this.getrowId();
        if (id > 0) {
            app_dialog.confirm('האם למחוק פעולה ' + id, function () {
                app_query.doPost(app.appPath() + "/System/TaskFormDelete", {
                    'id': id
                }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid4').jqxGrid('source').dataBind();
                });
            });
        }
    }
    update(id, value) {

        $.ajax({
            url: '/System/TaskFormChecked',
            type: 'post',
            dataType: 'json',
            data: {
                'id': id, 'done': value
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
    }
    refresh() {
        $('#jqxgrid4').jqxGrid('source').dataBind();
    }
    cancel() {
        wizard.wizHome();
    }
    end(data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
        }
    }
};

class app_task_form_item {

    constructor($element) {
        this.$element = $($element);
        //var slf = this;
        //return this;
    }

    tag($element, dataModel) {
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
            '<textarea id="ItemText" name="ItemText" class="text-content"' + (dataModel.Option != "a" ? " readonly=\"readonly\"" : "") + '></textarea>' +
            '</div>' +
            '<div class="form-group' + (dataModel.Id == 0 ? " pasive" : "") + '">' +
            '<div class="form-group">' +
            '<div class="field">נוצר בתאריך:</div>' +
            '<input id="ItemDate" name="ItemDate" type="text" readonly="readonly" class="text-mid" data-type="date" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">מועד התחלה:</div>' +
            '<input id="StartDate" name="StartDate" type="text" readonly="readonly" class="text-mid" data-type="datetime" />' +
            '<a id="form-Start" href="#" class="btn-bar pasive"><i class="fa fa-chevron-left"></i>התחל</a> ' +
            '</div>' +
            '<div id="form-Done-group" class="form-group">' +
            '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">סיום ביצוע</div>' +
            '<div id="divDoneDate" class="form-group">' +
            '<div class="field">מועד סיום:</div>' +
            '<input id="DoneDate" name="DoneDate" type="text" readonly="readonly" class="text-mid" data-type="datetime" />' +
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
            '<input id="DisplayName" type="text" readonly="readonly" class="text-mid" data-field="DisplayName" />' +
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

        app_perms.readonlyItems(["ItemText"], dataModel.Option != "a" || this.ReadOnly);
        app_perms.readonlyItems(["DoneComment", "DoneStatus"], dataModel.Option != "e" || this.ReadOnly);
        //app_perms.readonlyInput("#DoneComment", dataModel.Option != "e" || this.ReadOnly);
        //app_perms.readonlyInput("#DoneStatus", dataModel.Option != "e" || this.ReadOnly);
    };

    init(dataModel, userInfo, readonly) {

        this.TaskId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        this.Mode = dataModel.Action || 'edit';//edit|start|done   //this.ItemId==0 ? 'submit' : 'start';
        var slf = this;
        this.tag(slf.$element, dataModel);

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
            switch (slf.Mode) {
                case "start":
                    slf.doStart(); break;
                default:
                    slf.doSubmit(); break;
            }
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
                    app_form.loadDataForm("form-Form", record);

                    //app_jqxform.loadDataForm("form-Form", record);
                    // $("form#form-Form [name=UserId]").val(record.UserId);

                    //var itemDate = app.toLocalDateString(record.ItemDate);
                    $("#ItemText").prop("readonly", slf.ReadOnly);

                    //$('#Duration').val(record['Duration']);
                    //$("#ItemDate").val(app.formatDateTimeString(record.ItemDate));//(app.toLocalDateString(record.ItemDate));
                    //$('#StartDate').val(app.formatDateTimeString(record.StartDate));//app.toLocalDateTimeString(record.StartDate));
                    //$("#DoneDate").val(app.formatDateTimeString(record.DoneDate));//app.toLocalDateTimeString(record.DoneDate));

                    //$("#form-Comment").hide();
                    $("#form-Title").text("פעולה: " + slf.ItemId);

                    if (record.StartDate) {
                        //$("#form-Start").hide();
                        app_perms.readonlyInput("#ItemText");
                        slf.Mode == 'started';
                        $("#form-Submit").text('עדכון');
                        $("#form-Done-group").show();
                    }
                    else {
                        //$("#form-Start").show();
                        slf.Mode == 'start';
                        $("#form-Submit").text('התחל');
                        $("#form-Done-group").hide();

                    }
                    if (record.DoneStatus == false) {
                        slf.Mode == 'started';
                        $("#divDoneDate").hide();
                        $("#divDisplayName").hide();
                    }
                    else {
                        //$("#DoneStatus").prop('disabled', true)
                        slf.Mode == 'done';
                        app_perms.readonlyInput("#ItemText");
                        $("#DoneComment").prop('readonly', true)
                        $("#form-Submit").hide();
                        //$("#form-Start").hide();
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

    display() {
        this.$element.show();
        $("#jqxgrid4-bar").hide();
    };

    doCancel() {
        $("#jqxgrid4-bar").show();
        app_task.triggerSubTaskCompleted('form');
    };
    doSubmit() {
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

    };

    doStart() {
        //e.preventDefault();

        app_query.doDataPost("/System/TaskFormStart", { 'id': this.ItemId }, function (data) {
            if (data.Status > 0) {
                $('#StartDate').val(app.toLocalDateTimeString(Date.now()));
                $("#form-Done-group").show();
                $("#form-Start").hide();
                $("#jqxgrid4").jqxGrid('source').dataBind();
                app_task.triggerSubTaskCompleted('form', data);
            }
        });
    };

    //return app_task_form;

}


//============================================================ app_topic_form

class app_topic_form_grid extends app_task_form_grid {

    constructor(taskId, userInfo, option) {

        super(taskId, userInfo, option);
    }
    init() {
        var slf = this;
        var taskid = this.TaskId;

        var topicsource = {
            datafields: [
                { name: 'ItemId', type: 'number' },
                { name: 'ItemDate', type: 'date' },
                { name: 'ItemText', type: 'string' },
                { name: 'DoneDate', type: 'date' },
                { name: 'StartDate', type: 'date' },
                { name: 'DoneStatus', type: 'bool' },
                { name: 'DisplayName', type: 'string' },
                { name: 'Task_Id', type: 'number' },
                { name: 'AssignBy', type: 'number' },
                { name: 'ItemDueDate', type: 'date' },
                { name: 'ItemAssignTo', type: 'number' },
                { name: 'ItemTask', type: 'number' },
                { name: 'ItemSubject', type: 'string' },
                { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetTasksFormGrid',
            data: { 'pid': taskid }
        };

        var topicAdapter = new $.jqx.dataAdapter(topicsource);

        $("#jqxgrid4").jqxGrid({
            width: '100%',
            editable: slf.Option == "e",
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: topicAdapter,
            width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
                {
                    text: 'משימה', dataField: 'ItemTask', filterable: false, width: 90, cellsalign: 'right', align: 'center',
                    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                        var editlink = '';
                        //var asb = $('#jqxgrid').jqxGrid('getrowdata', row).AssignBy;
                        //if (slf.UserId == asb)
                        //    editlink = '<label> </label><a href="#" onclick="app_task.taskTopicEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';
                        if (value > 0)
                            return '<div style="text-align:center">' + value + '<a href="#" onclick="app_tasks.taskEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>' + editlink + '</div>';
                        else
                            return '';
                    }
                },
                { text: 'מועד לביצוע', datafield: 'ItemDueDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
                { text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center', editable: false },
                { text: 'מועד התחלה', datafield: 'StartDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
                { text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
                { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center' },
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

    }
    showControl(id, option, action) {

        var data_model = {
            PId: this.TaskId, Id: id, Option: option, Action: action
        };

        if (this.Control == null) {
            this.Control = new app_topic_form_item("#jqxgrid4-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
}



class app_topic_form_item extends app_task_form_item {

    constructor($element) {
        super($element);
    }

    tag($element, dataModel) {
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
            '<div class="field">משימה עבור:</div>' +
            '<div id="ItemAssignTo" name="ItemAssignTo"></div>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">נושא:</div>' +
            '<input id="ItemSubject" name="ItemSubject" type="text" style="width:90%" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">משימה:</div>' +
            '<textarea id="ItemText" name="ItemText" class="text-content"' + (dataModel.Option != "a" ? " readonly=\"true\"" : "") + '></textarea>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">מועד לביצוע:</div>' +
            '<div id="ItemDueDate" name="ItemDueDate"></div>' +
            '</div>' +
            '<div class="form-group' + (dataModel.Id == 0 ? " pasive" : "") + '">' +
            '<!--<div class="form-group">' +
            '<div class="field">נוצר בתאריך:</div>' +
            '<input id="ItemDate" name="ItemDate" type="text" readonly="readonly" class="text-mid" data-type="date" />' +
            '</div>-->' +
            '<div class="form-group">' +
            '<div class="field">יצירת משימה:</div>' +
            '<!--<input id="StartDate" name="StartDate" type="text" readonly="readonly" class="text-mid" />-->' +
            '<!--<a id="form-Start" href="#" class="btn-bar"><i class="fa fa-chevron-left"></i>צור משימה</a>--> ' +
            '</div>' +
            '<div id="form-Done-group" class="form-group pasive">' +
            '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">סיום ביצוע</div>' +
            '<div id="divDoneDate" class="form-group">' +
            '<div class="field">מועד סיום:</div>' +
            '<input id="DoneDate" name="DoneDate" type="text" readonly="readonly" class="text-mid" data-type="datetime" />' +
            '</div>' +
            '<div id="form-Done" class="form-group pasive">' +
            '<div class="field"><a id="form-comment-toggle" href="#" ><i class="fa fa-plus-square-o"></i></a><span>הערות ביצוע:</span></div>' +
            '<div id="form-Comment">' +
            '<textarea id="DoneComment" name="DoneComment" class="text-content"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '></textarea>' +
            '</div>' +
            '</div>' +
            '<div class="form-group pasive">' +
            '<div class="field">' +
            'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '/>' +
            '</div>' +
            '</div>' +
            '<div id="divDisplayName" class="form-group">' +
            '<div class="field">בוצע ע"י:</div>' +
            '<input id="DisplayName" type="text" readonly="readonly" class="text-mid" data-field="DisplayName"/>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div>' +
            '<a id="form-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="form-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a> ' +
            '<a id="form-Start" class="btn-default btn7 w-60" title="צור משימה" href="#">משימה</a>' +
            '</div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';
        $element.html(html);
    };

    init(dataModel, userInfo, readonly) {

        this.TaskId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(slf.$element, dataModel);

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        var input_rules = [
            { input: '#ItemText', message: 'חובה לציין משימה!', action: 'none', rule: 'required' },
        ];

        $('#ItemDueDate').jqxDateTimeInput({ showCalendarButton: true, readonly: false, width: '150px', rtl: true });
        app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "ItemAssignTo", '/System/GetUserTeamList', 0, 120, false);

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
                    app_form.loadDataForm("form-Form", record);

                    //app_jqxform.loadDataForm("form-Form", record);
                    // $("form#form-Form [name=UserId]").val(record.UserId);

                    //var itemDate = app.toLocalDateString(record.ItemDate);
                    $("#ItemText").prop("readonly", slf.ReadOnly);

                    //$('#Duration').val(record['Duration']);
                    //$("#ItemDate").val(app.toLocalDateString(record.ItemDate));
                    //$('#StartDate').val(app.toLocalDateString(record.StartDate));
                    //$("#DoneDate").val(app.toLocalDateTimeString(record.DoneDate));
                    //$("#form-Comment").hide();
                    $("#form-Title").text("פעולה: " + slf.ItemId);

                    if (record.StartDate || record.ItemTask > 0) {
                        $("#form-Start").hide();
                        //$("#form-Done-group").show();
                    }
                    else {
                        $("#form-Start").show();
                        //$("#form-Done-group").hide();

                    }
                    //if (record.DoneStatus == false) {
                    //    $("#divDoneDate").hide();
                    //    $("#divDisplayName").hide();
                    //}
                    //else {
                    //    //$("#DoneStatus").prop('disabled', true)
                    //    $("#DoneComment").prop('readonly', true)
                    //    $("#form-Submit").hide();
                    //    $("#form-Start").hide();
                    //}
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

    doStart() {
        //e.preventDefault();

        app_query.doDataPost("/System/TopicTaskFormStart", { 'id': this.TaskId, 'itemId': this.ItemId }, function (data) {
            if (data.Status > 0) {
                app_form.setDateTimeNow('#StartDate');
                //$('#StartDate').val(app.toLocalDateString(Date.now()));
                //$("#form-Done-group").show();
                $("#form-Start").hide();
                $("#jqxgrid4").jqxGrid('source').dataBind();
                //app_task.triggerSubTaskCompleted('form', data);
            }
        });
    };
}



//============================================================ app_task_form_template

class app_task_form_template {

    constructor() {
        this.wizardStep = 2;
        this.FormId = 0;
        this.TaskId = 0;
        this.UserId = 0;
    }

    load(taskId, userInfo) {
        this.TaskId = taskId;
        this.UserId = userInfo.UserId;
        this.grid(this.TaskId);
        //return this;
    }
    load_template(formId) {
        this.FormId = formId;
        this.grid(formId);
        //return this;
    }
    grid(taskid) {
        var slf = this;

        var nastedsource = {
            datafields: [
                {
                    name: 'ItemId', type: 'number'
                },
                {
                    name: 'ItemDate', type: 'date'
                },
                {
                    name: 'ItemText', type: 'string'
                },
                {
                    name: 'DoneDate', type: 'date'
                },
                {
                    name: 'DoneStatus', type: 'bool'
                },
                {
                    name: 'DisplayName', type: 'string'
                },
                {
                    name: 'Task_Id', type: 'number'
                },
                {
                    name: 'AssignBy', type: 'number'
                },
                { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetTasksFormGrid',
            data: {
                'pid': taskid
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
                {
                    text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                },
                {
                    text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center'
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
            width: 400, height: 240, resizable: false, autoOpen: false, cancelButton: $("#formCancel"), modalOpacity: 0.01, position: {
                x: 0, y: 0
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
    }
    doRowEdit(selectedrowindex) {
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
    }
    getRowData() {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return null;
        var data = $('#jqxgrid4').jqxGrid('getrowdata', selectedrowindex);
        return data;
    }
    getrowId() {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid4").jqxGrid('getrowid', selectedrowindex);
        return id;
    }
    add() {
        $("#Task_Id").val(this.TaskId);
        $("#ItemText").val('');
        $("#ItemId").val(0);
        //app_jqx.openWindow("#popupWindow", "#jqxgrid4");
        app_jqx.displayWindow("#jqxgrid4-window", "#jqxgrid4-add");

        //this.doRowEdit(0);
        //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormAdd?pid=" + this.TaskId, "100%", "220px", true);
    }
    edit() {
        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return;
        this.doRowEdit(selectedrowindex);
        //app_iframe.appendEmbed("jqxgrid4-window", app.appPath() + "/System/_TaskFormEdit?id=" + id, "100%", "480px", true);
    }
    remove() {

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
            app_dialog.confirm('האם למחוק פעולה ' + id, function () {
                app_query.doPost(app.appPath() + "/System/TaskFormDelete", { 'id': id });
                $('#jqxgrid4').jqxGrid('source').dataBind();
            });
        }
    }
    update() {

        app_query.doFormPost('#fcTaskForm', function (data) {
            if (data.Status > 0) {
                app_messenger.Post(data);
                $('#jqxgrid4').jqxGrid('source').dataBind();
                app_jqx.closeWindow("#jqxgrid4-window");
            }
            else
                app_messenger.Post(data, 'error');
        });
    }
    refresh() {
        $('#jqxgrid4').jqxGrid('source').dataBind();
    }
    cancel() {
        //wizard.wizHome();
    }
    end(data) {

        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
        }
    }
};



//============================================================ app_task_form_import

class app_task_form_import {

    constructor(taskId, assignBy, assignTo) {
        this.Id = taskId;
        this.AssignBy = assignBy;
        this.AssignTo = assignTo || 0;
        this.UploadKey = '';
        
    }

    init(callback) {

        this.Callback = callback;
        var _slf = this;
        var wait_message = 'נא לבחור קובץ לטעינה ולהמתין לסיום.';

        _slf.resetPprogress();
        $("#loader").hide();
        $("#import-message").html('<strong>' + wait_message + '</strong>');

        //$('#importUpload').click(function () {
        //    _slf.doUploadSync();
        //});

        //$('#importCancel').click(function () {
        //    $("#jqxgrid4-bar").show();
        //    app_task.triggerSubTaskCompleted('form');
        //});

        $('#fileupload').fileupload({
            maxFileSize: 10000000,
            url: '/Media/TaskUpload',
            formData: {
                param1: _slf.Id,
                param2: _slf.AssignBy,
                param3: _slf.AssignTo
            },
            dataType: 'json',
            done: function (e, data) {
                $("#loader").hide();
                $("#import-message").html('<strong>' + data.result.Message + '</strong>');

                //connectUploadedGrid();
                if (data.result.Status > 0) {
                    //wizard.next();
                    //$('#btnUpload').show();
                    if (_slf.Callback)
                        _slf.Callback(data);
                }
                //$.each(data.result.files, function (index, file) {
                //    $('<p/>').text(file.name).appendTo('#files');
                //});
            },
            error: function (jqXHR, status, error) {
                $("#loader").hide();
                app_dialog.alert(error);
            },
            beforeSend: function (e, data) {
                $("#import-message").html(wait_message);
                //wizard.showHint(wait_message);
            },
            progressall: function (e, data) {
                _slf.doPprogress(data);
                $("#loader").show();
            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled')
            .bind('fileuploadsubmit', function (e, data) {
                $("#loader").hide();
                _slf.resetPprogress();
                //fileuuid = generateUUID('16');
                //data.formData = {
                //    param1: $('#uploadBid').val(),
                //    param2: $('#uploadUid').val(),
                //    param3: $('#uploadPt').val()
                //};
            });

        //console.log('import is set');

    }
    //}).prop('disabled', !$.support.fileInput)
    //    .parent().addClass($.support.fileInput ? undefined : 'disabled');

    doPprogress(data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progress .progress-bar').css(
            'width',
            progress + '%'
        );
    };

    resetPprogress() {
        $('#progress .progress-bar').css(
            'width', '0%'
        );
    };

    //doUploadSync() {

    //    var callback = this.Callback;

    //    $.ajax({
    //        url: '/Media/ExecUploadTaskAsync',
    //        type: "POST",
    //        dataType: 'json',
    //        //contentType: "application/json; charset=utf-8",
    //        data: { 'id': this.Id, 'uploadKey': '' + this.UploadKey + '', 'updateExists': false },
    //        success: function (data) {

    //            if (callback)
    //                callback(data);

    //            //if (data) {
    //            //    if (data.Status < 0)
    //            //        alert(data.Message);
    //            //    else {

    //            //        //redirectTo('UploadProc?uk='+uploadKey);
    //            //        wizard.upload(uploadKey);

    //            //        //$("#final").html(data.Message);

    //            //    };
    //            //}

    //        },
    //        error: function (jqXHR, status, error) {
    //            app_dialog.alert(error);
    //        }
    //    });
    //}

}
