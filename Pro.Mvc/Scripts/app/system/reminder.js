'use strict';


//============================================================ app_reminder

class app_reminder{

    constructor(dataModel, userInfo, taskModel) {

        this.$element = $("#accordion");
        //this.$element = document.querySelectorAll(element);

        //if (!this.$element.is('div')) {
        //    $.error('app_wiztabs should be applied to DIV element');
        //    return;
        //}
        //// ensure to use the `new` operator
        //if (!(this instanceof app_task))
        //    return new app_task(element, RemindId, userInfo, taskModel);

        this.RemindId = dataModel.Id;
        this.RemindParentId = dataModel.PId;
        this.Model = dataModel;
        this.RemindModel = RemindModel;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.Title = app_tasks.taskTitle(this.RemindModel);
        this.uploader;
        this.IsNew = (this.RemindId === 0);
        this.IsOwner = (this.Model.UserId === this.UserInfo.UserId);
        this.Option = (dataModel.Option) ? dataModel.Option : 'e';
        this.AssignBy = (dataModel.AssignByion) ? dataModel.AssignBy : 0;
        this.RemindStatus = (dataModel.RemindStatus) ? dataModel.RemindStatus : 0;
        this.IsEditable = (this.RemindId === 0) || ((this.RemindStatus < 8) && (this.AssignBy === this.UserInfo.UserId || this.Option === 'e'));
        this.EnableFormType = this.IsNew;
        this.ClientId = 0;
        this.ProjectId = 0;
        //this.Tags;
        //this.AssignTo;
        this.SrcUserId = 0;

    }

    init() {

        $("#hTitle-text").text(this.Title + ': ' + $("#RemindSubject").val());
        $("#hxp-title").text(this.Title + ': ' + this.RemindId);

        
        if (!this.IsEditable) {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }

        this.preLoad();

        if (this.RemindId > 0) {

            this._loadData();
            //this.loadControls(this.Model.Data);
            
        }
        else {
            this.loadControls();
        }
        this.loadEvents();
    }

    doSettings(record) {

        if (record.RemindStatus <= 0)
            record.RemindStatus = 1;

        this.AssignBy = record.AssignBy;
        this.RemindStatus = record.RemindStatus;
        this.IsEditable = (this.RemindId === 0) || ((this.RemindStatus < 8) && (this.AssignBy === this.UserInfo.UserId || this.Option === 'e'));

        this.ProjectId = record.Project_Id;
        this.ClientId = record.ClientId;
        this.Tags = record.Tags;
        //this.AssignTo = record.AssignTo;
        this.SrcUserId = record.UserId;


        $("#RemindSubject-div").hide();

        if (this.SrcUserId > 0)
            app_jqx_combo_async.userInputAdapter("#UserId", this.SrcUserId);
     }

    _loadData() {

        var _slf = this;
        var url = "/System/GetReminderEdit" ;
        var view_source =
            {
                datatype: "json",
                id: 'RemindId',
                data: { 'id': _slf.RemindId },
                type: 'POST',
                url: url
            };

        var viewAdapter = new $.jqx.dataAdapter(view_source, {
            loadComplete: function (record) {

                _slf.doSettings(record);
                _slf.loadControls(record);
                //slf.loadEvents();
            },
            loadError: function (jqXHR, status, error) {
                app_dialog.alert("error loading task data");
            },
            beforeLoadComplete: function (records) {
            }
        });

        if (this.RemindId > 0) {
            viewAdapter.dataBind();
        }
    }
    
    
    parentSettings(parentId) {
        $("#Remind_Parent").val(parentId);
        if (parentId > 0) {
            $("#Remind_Parent-group").show();
            $("#Remind_Parent-link").click(function () {
                app.redirectTo('/System/ReminderInfo?id=' + parentId);
            });
        }
        else {
            $("#Remind_Parent-group").hide();
        }
    }

    doSubmit(act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = app_jqx.getInputAutoValue("#RemindStatus", 1);// $("#RemindStatus").val();
        var isnew = this.IsNew;

        if (!this.exp1_Inited) {
            this.lazyLoad();
        }
        
        var afterSubmit = function (slf, data) {

            //if (isnew) {
            //    slf.RemindId = data.OutputId;
            //    $("#RemindId").val(data.OutputId);
            //    slf.tabSettings();
            //    $("#fcSubmit").val("עדכון");
            //    slf.IsNew = slf.RemindId == 0;
            //    app_messenger.Notify(data, 'info');
            //}
            //else {
            //    app_messenger.Notify(data, 'info', "/System/TaskUser");
            //}

            //if (act == 'plus') {
            //    app.refresh();
            //}

            if (slf.RemindId === 0) {
                slf.RemindId = data.OutputId;
                $("#RemindId").val(data.OutputId);
                $("#hxp-0").text('תזכורת: ' + data.OutputId);
                //$(".hxp").show();
            }
            app_messenger.Notify(data, 'info');
            if (act === 'plus') {
                app.refresh();
            }
            else {//if (act == 'finished') {
                app.redirectTo('/System/TaskUser');
            }

        }

        var RunSubmit = function (slf, status, actionurl) {
            //e.preventDefault();
            //var slf = this;
            //var actionurl = $('#fcForm').attr('action');

            //if (status == 0)
            //    $("#RemindStatus").val(1);
            //if (status == 1)
            //    $("#RemindStatus").val(2);


            //$("#RemindStatus").val(status);
            app_jqx.setInputAutoValue("#RemindStatus", status);

            //var clientId = $("#ClientId").val();
            //if (clientId > 0) {
            //    $('#ClientDetails').val($("#ClientId-display").val())
            //}

            //var clientDetails = $('#ClientDetails').val();
            //, { key: 'ClientDetails', value: clientDetails }
            var value = $("#Message").jqxEditor('val');
            var AssignTo = app_jqxcombos.getComboCheckedValues("AssignTo");
            var args = [{ key: 'RemindBody', value: app.htmlEscape(value) }, { key: 'AssignTo', value: AssignTo }];
            var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);

            app_query.doFormSubmit("#fcForm", actionurl, formData, function (data) {

                afterSubmit(slf, data);
            });


            //return this;
        };

        //var actionurl = $('#fcForm').attr('action');

        if (this.IsNew) {
            status = 1;
            //actionurl = '/System/UpdateNewTask';
            RunSubmit(this, status, actionurl)
        }
        else {
            status = 2;
            //update
            RunSubmit(this, status, actionurl);

        }
        //return this;
    }

    preLoad() {

        var slf = this;


        this.parentSettings(slf.RemindParentId);
  
        $("#AccountId").val(slf.AccountId);

        //$("#hTitle-text").text(slf.Title + ': ' + slf.RemindId);

        //$("#hxp-title").text(slf.Title + ': ' + slf.RemindId);

        if (theme === undefined)
            theme = 'nis_metro';

        $('#RemindBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });
        $('#RemindBody-btn-view').on('click', function () {

            if ($('#RemindBody-div').hasClass("editor-view")) {
                $('#RemindBody-div').removeClass("editor-view");
                $('#RemindBody').jqxEditor('height', '300px');
                $('#RemindBody').css('height', '305px');
            }
            else {
                $('#RemindBody-div').addClass("editor-view");
                $('#RemindBody').css('height', '805px');
                $('#RemindBody').jqxEditor('height', '800px');
            }
        });

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
      
    }

    loadControls(record) {


        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });
        //$('#CreatedDate').jqxDateTimeInput({ showCalendarButton: false, allowKeyboardDelete: false, readonly: true, width: '150px', rtl: true });
        

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

        //$("#ShareType").change(function () {
        //    var index = $('option:selected', this).val();

        //    if (index === "3") {
        //        $("#AssignTo").jqxComboBox({ disabled: false });
        //        //$("#AssignTo").jqxComboBox('open');
        //    }
        //    else {
        //        $("#AssignTo").jqxComboBox('uncheckAll');
        //        $("#AssignTo").jqxComboBox({ disabled: true });
        //    }
        //});

        if (record) {
            //if (record.RemindStatus <= 0)
            //    record.RemindStatus = 1;

            //this.taskSettings(record);

            app_form.loadDataForm("fcForm", record, ["RemindStatus", "Project_Id", "ClientId", "Tags", "AssignTo"]);

            //$("#ShareType").val(record.ShareType);

            //$("#AssignTo").jqxComboBox({ disabled: record.ShareType !== 3 });
            app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, record.AssignTo, function () {
                $("#AssignTo").jqxComboBox({ disabled: true });
            });
            $("#RemindBody").jqxEditor('val', app.htmlUnescape(record.RemindBody));
            //$("#CreatedDate").val(record.CreatedDate);
            //$("#StartedDate").val(app.jsonDateToString(record.StartedDate, true));
            //$("#EndedDate").val(app.jsonDateToString(record.EndedDate, true));

            $("#RemindSubject").val(record.RemindSubject);
            $("#hTitle-text").text(this.Title + ": " + record.RemindSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || '#000'));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });
            this.parentSettings(record.Remind_Parent);

            //if (record.RemindStatus > 1 && record.RemindStatus < 8)
            //    $("#fcEnd").show();//$("#fcSubmit").val("סיום");
            //else
            //    $("#fcEnd").hide();//$("#fcSubmit").val("עדכון");


            var align = app_style.langAlign(record.Lang);
            $('#RemindBody').css('text-align', align)

            //app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Remind_Type", '/System/GetRemindTypeList', null, 0, 120, true, null, function (status, records) {
            //    if (record.Remind_Type >= 0)
            //        $("#Remind_Type").val(record.Remind_Type);
            //});
            //$("#Remind_Type").jqxComboBox({ enableSelection: this.IsEditable });

            app_jqx_combo_async.taskStatusInputAdapter("#RemindStatus", record.RemindStatus);


        }
        else {
            app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, null, function () {
               
            });
            //app_jqxcombos.createComboAdapter("PropId", "PropName", "Remind_Type", '/System/GetRemindTypeList', 0, 120, false);
            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
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

                        app_query.doDataPost("/System/RemindFormByTemplate", {
                            'RemindId': slf.RemindId, 'FormId': args.item.value
                        },
                            function (data) {
                                if (data.Status > 0)
                                    $('#jqxgrid4').jqxGrid('source').dataBind();
                            });
                    });

                    //if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
                    //    app_query.doDataPost("/System/RemindFormByTemplate", { 'RemindId': slf.RemindId, 'FormId': args.item.value },
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
                if (name === null || name === '') {
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

                //app_query.doPost("/System/TaskFormTemplateCreate", { 'id': slf.RemindId, 'name': name }, function (data) {
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
                    if (item.label.charAt(0) === '@') {
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

            $("#Remind_Child-link").click(function () {

                app_dialog.confirm("האם ליצור תת משימה", function () {
                    app.redirectTo('/System/ReminderNew?pid=' + slf.RemindId);
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
            //{ input: '#RemindSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
            {
                input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'none', rule: function (input, commit) {
                    //var value = $('#DueDate').jqxDateTimeInput('value');
                    var text = $('#DueDate').jqxDateTimeInput('getText');
                    return text !== null && text.length > 0;
                }
            },
            {
                input: "#RemindBody", message: 'חובה לציין תוכן!', action: 'none', rule: function (input, commit) {
                    //var value = $("#RemindBody").text();//.jqxEditor('val');
                    var value = $("#RemindBody").jqxEditor('val');
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

    lazyLoad() {

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', this.ClientId, function () {

        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', this.ProjectId, function () {

        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 225, 0, true, this.Tags, function () {

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
            //app.disableSelect("#ShareType");
        }
    }

    doCancel() {
        app.redirectTo("/System/TaskUser");
        //return this;
    }

    //============================================================ app_tasks global

    static tasks_edit_view_comment(row) {

        var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
        content = content.replace("\n", "<br/>");
        app_jqx.toolTipClick(".task-comment", '<p>' + content + '</p>', 350);
    };

    static setRemindButton(item, action, visible) {
        var name = 'עדכון';

        if (item === 'timer') {
            if (action === 'add')
                name = 'התחל';
            else if (action === 'update')
                name = 'סיום';


        }
        else if (action === 'add')
            $('#task-item-update').val('');
        else if (action === 'update')
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

    triggerRemindCommentCompleted(data) {
        this.Comment.end(data);
    };
    triggerRemindAssignCompleted(data) {
        this.Assign.end(data);
    };
    triggerRemindTimerCompleted(data) {
        this.Assign.end(data);
    };
    triggerRemindFormCompleted(data) {
        this.Actions.end(data);
    };
    static triggerSubRemindCompleted(type, data) {
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

        if (tag !== null) {
            $(tag + '-window').hide();
            $(tag + '-bar').show();
            if (data !== null && data.Status > 0)
                $(tag).jqxGrid('source').dataBind();
        }
    };

}


