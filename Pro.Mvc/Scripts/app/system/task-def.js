'use strict';

//============================================================================================ app_task_def

function app_task_demo() {
    var task = new app_task();
    task.doCancel();
    task.comment.end();
    app_task.triggerSubTaskCompleted();
    //app_task_trigger.
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
            //status =  2;
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

            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', { 'model': this.TaskModel }, 0, 120, true, null, function (status, records) {
                if (record.Task_Type >= 0)
                    $("#Task_Type").val(record.Task_Type);
            });
            $("#Task_Type").jqxComboBox({ enableSelection: this.IsEditable });
            //app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);

            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);
            
        }
        else {
            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', { 'model': this.TaskModel }, 0, 120, true, "0");
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

