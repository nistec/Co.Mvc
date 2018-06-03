//'use strict';

//============================================================================================ app_task_def

//app_task = {
//    init: function (TaskId, userInfo, taskModel) {

//        return new app_task_def(TaskId, userInfo, taskModel)
//    }
//};

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
        this.TaskId = dataModel.Id;
        this.TaskParentId = dataModel.PId;
        this.Model = dataModel;
        this.TaskModel = taskModel;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        this.Title = (this.TaskModel == 'E') ? 'כרטיס' : 'משימה';
        this.uploader;
        this.IsNew = (this.TaskId == 0);
        this.IsOwner =  (this.Model.UserId == this.UserInfo.UserId);
        this.Option = (dataModel.Option) ? dataModel.Option : 'e';
        this.IsEditable = (this.TaskId == 0) || ((this.Model.TaskStatus < 8) && (this.Model.AssignBy == this.UserInfo.UserId || this.Option == 'e'));
        this.EnableFormType = this.IsNew;
        this.ClientId = 0;
        this.ProjectId = 0;
        this.Tags;
        this.AssignTo;
        this.exp1_Inited = false;

        var slf = this;


        this.tabSettings();
        this.preLoad();
        this.loadControls();
        this.lazyLoad();
        this.loadEvents();

        return this;
    };
      
    app_task.prototype.tabSettings = function () {

        $("#hTitle-text").text(this.Title + ': ' + $("#TaskSubject").val());
        $("#hxp-title").text(this.Title + ': ' + this.TaskId);

        if (this.TaskId > 0) {
            $("#hxp-4").show();
            $("#hxp-5").show();
        }
        else {
            $("#hxp-4").hide();
            $("#hxp-5").hide();

        }
    };

    app_task.prototype.doCancel = function () {
        app.redirectTo("/System/TaskUser");
        return this;
    }
  
    app_task.prototype.doSubmit = function (act) {
        //e.preventDefault();

        if (!this.exp1_Inited)
            this.lazyLoad();

        var actionurl = $('#fcForm').attr('action');
        var status = $("#TaskStatus").val();
        var isnew = this.IsNew;

        var afterSubmit = function (slf, data) {

            if (isnew) {
                slf.TaskId = data.OutputId;
                $("#TaskId").val(data.OutputId);
                slf.tabSettings();
                $("#fcSubmit").val("סיום");
                slf.IsNew = slf.TaskId==0;
                app_messenger.Notify(data, 'info');//, "/System/TaskUser");
            }
            else {
                app_messenger.Notify(data, 'info', "/System/TaskUser");
                app.redirectTo('/System/TaskUser');
            }

            if (act == 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {
            //e.preventDefault();
 
            if (status == 0)
                $("#TaskStatus").val(1);
            else
                $("#TaskStatus").val(status);

            var value = $("#TaskBody").jqxEditor('val');
            var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }];
            var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);

            var validationResult = function (isValid) {
                if (isValid) {
                    $.ajax({
                        url: actionurl,
                        type: 'post',
                        dataType: 'json',
                        data: formData,
                        success: function (data) {
                            afterSubmit(slf, data);
                        },
                        error: function (jqXHR, status, error) {
                            app_messenger.Notify(error, 'error');
                        }
                    });
                }
            }
            $('#fcForm').jqxValidator('validate', validationResult);
            return this;
        };

        var actionurl = $('#fcForm').attr('action');
        status = 1;

        if (this.IsNew) {
            actionurl = '/System/UpdateNewTask';
            RunSubmit(this, status, actionurl)
        }
        else {
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
        $("#TaskModel").val(slf.TaskModel);

        if (theme === undefined)
            theme = 'nis_metro';


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
                    case "#exp-4":
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
                            slf.form.load(slf.Model, slf.UserInfo);
                        break;
                    case "#exp-5":
                        if (slf.TaskId == 0) {
                            event.preventDefault();
                            app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                            return false;
                        }

                        if (slf.uploader == null) {
                            slf.uploader = new app_media_uploader("#task-files");
                            slf.uploader.init(slf.TaskId, 't');
                            slf.uploader.show();
                        }

                        //if ($("#iframe-files").attr('src') === undefined)
                        //    var op = slf.Model.Option;
                        //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t&op=' + op, '100%', '350px', true);

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
    };

    app_task.prototype.loadControls = function (record) {

        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });
        $('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, allowKeyboardDelete: false, readonly: true, width: '150px', rtl: true });
        $('#CreatedDate').val(Date.now());

        if (this.IsNew) {
            app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
            app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
        }
        else {
            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', null, 0, 120, true, null, function (status, records) {
                if (record.Task_Type >= 0)
                    $("#Task_Type").val(record.Task_Type);
            });
            $("#Task_Type").jqxComboBox({ enableSelection: this.IsEditable });
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
            console.log('callback');
        });
        this.exp1_Inited = true;
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

            //$("#Form_Type").on('change', function (event) {
            //    var args = event.args;
            //    if (args) {
            //        //app_tasks_form_template.load(args.item.value);
            //        if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
            //            app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
            //                function (data) {
            //                    if (data.Status > 0)
            //                        $('#jqxgrid4').jqxGrid('source').dataBind();
            //                });
            //        }
            //    }
            //});
            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
                            function (data) {
                                if (data.Status > 0)
                                    $('#jqxgrid4').jqxGrid('source').dataBind();
                            });
                    });
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
                  { text: 'מועד רישום', datafield: 'ItemDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center' },
                  { text: 'נושא', datafield: 'ItemText', cellsalign: 'right', align: 'center' },
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
                width: 400, height: 240, resizable: false, autoOpen: false, cancelButton: $("#formCancel"), modalOpacity: 0.01, position: { x: 0, y: 0 }
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
                if (confirm('האם למחוק פעולה ' + id)) {
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


    //============================================================ app_task_form

    app_task.prototype.form = {

        wizardStep: 2,
        TaskId: 0,
        UserId: 0,
        Model: {},
        load: function (dataModel, userInfo) {
            this.TaskId = dataModel.PId;
            this.UserId = userInfo.UserId;
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
                editable: slf.Option == "e",
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
                slf.update(id, value);

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
                if (confirm('האם למחוק פעולה ' + id)) {
                    app_query.doPost(app.appPath() + "/System/TaskFormDelete", { 'id': id }, function (data) {
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

       
    app_task.setTaskButton = function (item, action, visible) {
        var name = 'עדכון';

        if (action == 'add')
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
             case 'form':
                tag = '#jqxgrid4'; break;
        }

        if (tag != null) {
            $(tag + '-window').hide();
            if (data != null && data.Status > 0)
                $(tag).jqxGrid('source').dataBind();
        }
    };


    return app_task;

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
                                                'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '/>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div id="divDisplayName" class="form-group">' +
                                            '<div class="field">בוצע ע"י:</div>' +
                                            '<input id="form-DisplayName" type="text" readonly="readonly" class="text-mid" />' +
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

    app_task_form.prototype.init = function (dataModel, userInfo) {

        this.TaskId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
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

                        //$('#StartTime').val(record['StartTime']);
                        $("#ItemDate").val(app.toLocalDateString(record.ItemDate));
                        $("#DoneDate").val(app.toLocalDateString(record.DoneDate));
                        if (record.DoneStatus == false) {
                            $("#divDoneDate").hide();
                            $("#divDisplayName").hide();
                        }
                        else {
                            $("#DoneStatus").prop('readonly', true)
                            $("#DoneComment").prop('readonly', true)
                            $("#form-Submit").hide();
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
            //$('#form-Submit').text("התחל");
        }
    }

    app_task_form.prototype.display = function () {
        this.$element.show();
    };

    app_task_form.prototype.doCancel = function () {

        app_task.triggerSubTaskCompleted('form');
    };

    app_task_form.prototype.doSubmit = function () {
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

    return app_task_form;

}());


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