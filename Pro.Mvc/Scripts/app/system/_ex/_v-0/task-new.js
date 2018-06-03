
//============================================================================================ app_task_def

var app_task = (function () {

    var app_task = function (taskParentId,userInfo, taskModel) {

        this.$element = $("#accordion");
        this.TaskId = 0;
        this.TaskParentId = taskParentId;
        this.TaskModel = taskModel;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.Title = 'משימה';//(this.TaskModel == 'E') ? 'כרטיס' : 'משימה';
        this.uploader;

        if (this.TaskModel == 'R')
            this.Title = 'תזכורת';
        else if (this.TaskModel == 'E')
            this.Title = 'סוגיה';
        else //if (this.TaskModel == 'T')
            this.Title = 'משימה';

        var exp1_Inited = false;
        var slf = this;

        $("#hxp-1").hide();
        $("#hxp-2").hide();
        $("#hxp-3").hide();
        $("#hxp-4").hide();
        $("#hxp-5").hide();

        


        var loadElements = function () {
            $("#Task_Parent").val(slf.TaskParentId);
            $("#AccountId").val(slf.AccountId);
            //$("#hxp-title").text(slf.Title + ': ' + slf.TaskId);

            if (theme === undefined)
                theme = 'nis_metro';


            //$("#accordion").accordion({
            //    beforeActivate: function (event, ui) {
            //        switch (ui.newHeader.context.id) {
            //            //case "hxp-1"://"ui-id-1":
            //            //    if (slf.TaskId == 0) {
            //            //        event.preventDefault();
            //            //        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
            //            //        return false;
            //            //    }
            //            //    if ($("#jqxgrid1")[0].childElementCount == 0)
            //            //        app_tasks_comment.load(slf.Model, slf.UserInfo);
            //            //    break;
            //            case "hxp-4"://"ui-id-1":
            //                if (slf.TaskId == 0) {
            //                    event.preventDefault();
            //                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
            //                    return false;
            //                }
            //                var source = $('#Form_Type').jqxComboBox('source');
            //                if (source == null)
            //                    app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetTaskFormTypeList', 0, 120, false);
            //                if ($("#jqxgrid4")[0].childElementCount == 0)
            //                    app_tasks_form_template.load(slf.TaskId, slf.UserInfo);
            //                break;
            //            case "hxp-5"://"ui-id-2":
            //                if (slf.TaskId == 0) {
            //                    event.preventDefault();
            //                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
            //                    return false;
            //                }
            //                if (slf.uploader == null) {
            //                    slf.uploader = new app_media_uploader("#task-files");
            //                    slf.uploader.init(slf.TaskId, 't');
            //                    slf.uploader.show();
            //                }

            //                //if ($("#iframe-files").attr('src') === undefined)
            //                //    app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t', '100%', '350px', true);
            //                break;
            //        }
            //    }
            //});

            $("#accordion").jcxTabs({
                rotate: false,
                startCollapsed: 'accordion',
                collapsible: 'accordion',
                click: function (e, tab) {
                    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
                },
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
                                slf.comment.load(slf.Model, slf.UserInfo);
                            break;
                        case 2:
                            if ($("#jqxgrid2")[0].childElementCount == 0)
                                slf.assign.load(slf.Model, slf.UserInfo);
                            break;
                        case 3:
                            if ($("#jqxgrid3")[0].childElementCount == 0)
                                slf.timer.load(slf.Model, slf.UserInfo);
                            break;
                        case 4:
                            if (slf.TaskId == 0) {
                                event.preventDefault();
                                app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                                return false;
                            }
                            if ($("#jqxgrid4")[0].childElementCount == 0)
                                slf.form.load(slf.Model, slf.UserInfo);
                            break;
                        case 5:
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
                            break;
                    }

                },
                activateState: function (e, state) {
                    //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
                }
            });


            $("#jqxExp-1").jqxExpander({ rtl: true, width: '80%', expanded: false });
            $('#jqxExp-1').on('expanding', function () {

                if (!exp1_Inited) {

                    app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', null, function () {

                    });
                    app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', null, function () {

                    });

                    //app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
                    //app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');
                }
                //else {
                //    if ($('#ClientId').val() != '' && $('#ClientId-display').val() == '') {
                //        app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
                //    }
                //    if ($('#Project_Id').val() != '' && $('#Project_Id-display').val() == '') {
                //        app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');
                //    }
                //}

                exp1_Inited = true;
            });


            $("#ColorFlag").simplecolorpicker();
            $("#ColorFlag").on('change', function () {
                //$('select').simplecolorpicker('destroy');
                var color = $("#ColorFlag").val();
                $("#hTitle").css("color", color)
            });
            $("#TaskStatus").jqxDropDownList({ enableSelection: false, disabled: true, rtl: true });

            $('#TaskBody').jqxEditor({
                height: '300px',
                //width: '100%',
                //editable: dataModel.Option == 'e',
                rtl: true,
                tools: 'bold italic underline | color background | left center right'
                //theme: 'arctic'
                //stylesheets: ['editor.css']
            });
        }

        loadElements();

        this.loadControls();

        return this;

    }
    app_task.prototype.afterUpdate = function () {
        $("#hxp-1").show();
        $("#hxp-2").show();
        $("#hxp-3").show();
        $("#hxp-4").show();
        $("#hxp-5").show();
    };
    app_task.prototype.doCancel = function () {
        app.redirectTo("/System/TaskUser");
        //app_messenger.Notify("הודעה");
        //parent.triggerCancelEdit();
    };

    app_task.prototype.doComment = function (id) {
        wizard.displayStep(2);
        $.ajax({
            type: 'GET',
            url: '/System/_TaskComment',
            data: { "id": id },
            success: function (data) {
                $('#divPartial2').html(data);
            }
        });
    };

    app_task.prototype.doAssign = function (id) {
        wizard.displayStep(3);
        $.ajax({
            type: 'GET',
            url: '/System/_TaskAssign',
            data: { "id": id },
            success: function (data) {
                $('#divPartial3').html(data);
            }
        });
    };

    app_task.prototype.doSubmit = function (act) {
        //e.preventDefault();

        //var status = $("#TaskStatus").val();
        //if (status > 1 && status < 8)
        //{
        //    if (confirm("האם לסיים משימה?") == false)
        //        return;
        //}
        var value = $("#TaskBody").jqxEditor('val');
        //var clientDetails = $('#ClientDetails').val();
        //, { key: 'ClientDetails', value: clientDetails }
        var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }];
        var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);
        var actionurl = $('#fcForm').attr('action');
        var slf = this;

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
                            $("#hTitle-text").text('משימה: ' + data.OutputId);
                            slf.afterUpdate();
                            //$(".hxp").show();
                            app_messenger.Notify(data, 'info');//, "/System/TaskUser");
                        }
                        if (act == 'plus') {
                            app.refresh();
                        }
                    },
                    error: function (jqXHR, status, error) {
                        app_messenger.Notify(error, 'error');
                    }
                });
            }
        }
        $('#fcForm').jqxValidator('validate', validationResult);
    };

    app_task.prototype.loadControls = function () {

        var slf = this;

        $('#DueDate').jqxDateTimeInput({ width: '150px', rtl: true });
        $('#DueDate ').jqxDateTimeInput('setMinDate', new Date());
        //$('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, width: '150px', rtl: true, disabled: true });
        //$('#DueDate').val('');


        //app_jqx_list.taskTypeComboAdapter();
        //app_jqx_list.taskStatusComboAdapter();
        //$("#TaskStatus").jqxDropDownList({ enableSelection: false });
        app_jqxcombos.createComboAdapter("PropId", "PropName", "Task_Type", '/System/GetTaskTypeList', 0, 120, false);
        app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "AssignTo", '/System/GetUserTeamList', 0, 120, false);
        app_jqxcombos.createComboAdapter("ProjectId", "ProjectName", "Project_Id", '/System/GetProjectList', 0, 120, false);
        $("#Project_Id").jqxComboBox({ showArrow: false, autoComplete: true });

        //$("#accordion").accordion({ heightStyle: "content", rtl: true, editable: true });
        //$("#jqxExp-1").jqxExpander({ rtl: true, width: '240px', expanded: false });
        $("#ColorFlag").simplecolorpicker();
        $("#ColorFlag").on('change', function () {
            //$('select').simplecolorpicker('destroy');
            var color = $("#ColorFlag").val();
            $("#hTitle").css("color", color)
        });
        $("#Form_Type").on('change', function (event) {
            var args = event.args;
            if (args) {
                //app_tasks_form_template.load(args.item.value);
                if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
                    app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
                        function (data) {
                            if (data.Status > 0)
                                $('#jqxgrid4').jqxGrid('source').dataBind();
                        });
                }
            }
        });
        $("#jqxgrid4-check_template").on('change', function (event) {

            if (this.checked) {//($('#jqxgrid4-check_template').is(":checked")) {

                var rows = $('#jqxgrid4').jqxGrid('getrows');
                if (rows && rows.length > 0)
                    $("#divFormTemplate").show();
                else {
                    app_dialog.alert("לא נמצאו רשומות ליצירת תבנית");
                    this.checked = false;
                }
            }
            else
                $("#divFormTemplate").hide();
        });
        $('#jqxgrid4-save_template').click(function () {
            var name = $('#jqxgrid4-template_name').val();
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


        $("#AssignTo").on('change', function (event) {
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


        $('#TaskBody').jqxEditor({
            //height: '200px',
            //width: '100%',
            editable: true,//dataModel.Option == 'e',
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });


        //$("#accordion").accordion({
        //    beforeActivate: function (event, ui) {
        //        switch (ui.newHeader.context.id) {
        //            //case "hxp-1"://"ui-id-1":
        //            //    if (slf.TaskId == 0) {
        //            //        event.preventDefault();
        //            //        app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
        //            //        return false;
        //            //    }
        //            //    if ($("#jqxgrid1")[0].childElementCount == 0)
        //            //        app_tasks_comment.load(slf.Model, slf.UserInfo);
        //            //    break;
        //            case "hxp-4"://"ui-id-1":
        //                if (slf.TaskId == 0) {
        //                    event.preventDefault();
        //                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
        //                    return false;
        //                }
        //                var source = $('#Form_Type').jqxComboBox('source');
        //                if (source == null)
        //                    app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetTaskFormTypeList', 0, 120, false);
        //                if ($("#jqxgrid4")[0].childElementCount == 0)
        //                    app_tasks_form_template.load(slf.TaskId, slf.UserInfo);
        //                break;
        //            case "hxp-5"://"ui-id-2":
        //                if (slf.TaskId == 0) {
        //                    event.preventDefault();
        //                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
        //                    return false;
        //                }
        //                if (slf.uploader == null) {
        //                    slf.uploader = new app_media_uploader("#task-files");
        //                    slf.uploader.init(slf.TaskId, 't');
        //                    slf.uploader.show();
        //                }

        //                //if ($("#iframe-files").attr('src') === undefined)
        //                //    app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + slf.TaskId + '&refType=t', '100%', '350px', true);
        //                break;
        //        }
        //    }
        //});



        //var input_rules = [
        //    { input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
        //     //{ input: '#AssignTo', message: 'חובה לציין משימה עבור!', action: 'keyup, blur', rule: 'required' },
        //     {
        //         input: "#AssignTo", message: 'חובה לציין משימה עבור!', action: 'select', rule: function (input, commit) {
        //             var index = $("#AssignTo").jqxComboBox('getSelectedIndex');
        //             return index >= 0;
        //         }
        //     },
        //    {
        //        input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'keyup, blur', rule: 'required', rule: function (input, commit) {
        //            //var value = $('#DueDate').jqxDateTimeInput('value');
        //            var text = $('#DueDate').jqxDateTimeInput('getText');
        //            return text != null && text.length > 0;
        //        }
        //    },
        //    {
        //        input: "#TaskBody", message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: function (input, commit) {
        //            //var value = $("#TaskBody").text();//.jqxEditor('val');
        //            var value = $("#TaskBody").jqxEditor('val');
        //            value = app.htmlText(value);//.replace(/(<([^>]+)>)/ig, "");
        //            return value.length > 1;
        //        }
        //    }
        //];

        var input_rules = [
              { input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'none', rule: 'required' },
               //{ input: '#AssignTo', message: 'חובה לציין משימה עבור!', action: 'keyup, blur', rule: 'required' },
               {
                   input: "#AssignTo", message: 'חובה לציין משימה עבור!', action: 'none', rule: function (input, commit) {
                       var index = $("#AssignTo").jqxComboBox('getSelectedIndex');
                       return index >= 0;
                   }
               },
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

        //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid='+this.TaskId   +'&refType=t', '100%', '360px', true);
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
    
    //============================================================ app_tasks trigger
    
    app_task.tasks_edit_view_comment = function (row) {

        var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
        content = content.replace("\n", "<br/>");
        app_jqx.toolTipClick(".task-comment", '<p>' + content + '</p>', 350);
    };
    app_task.triggerTaskCommentCompleted = function (data) {
        app_tasks_comment.end(data);
    };
    app_task.triggerTaskAssignCompleted = function (data) {
        app_tasks_assign.end(data);
    };
    app_task.triggerTaskTimerCompleted = function (data) {
        app_tasks_timer.end(data);
    };
    app_task.triggerTaskFormCompleted = function (data) {
        app_tasks_form.end(data);
    };


    return app_task;
}());



//============================================================ app_tasks_files
//app_tasks_files = {

//    app_iframe.attachIframe ('task-files', '/Media/_MediaFiles', '100%', '300px', true);
//}