'use strict';

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
        this.IsInfo = dataModel.IsInfo;
        this.TaskModel = taskModel;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.Title = app_tasks.taskTitle(this.TaskModel);
        this.uploader = undefined;
        this.IsNew = (this.TaskId == 0);
        this.IsOwner = (this.Model.UserId == this.UserInfo.UserId);
        this.Option = (dataModel.Option) ? dataModel.Option : 'e';
        this.AssignBy = (dataModel.AssignBy) ? dataModel.AssignBy : 0;
        this.TaskStatus = (dataModel.TaskStatus) ? dataModel.TaskStatus : 0;
        
        this.IsEditable = (!this.IsInfo) &&((this.TaskId == 0) || ((this.TaskStatus < 8) && (this.AssignBy == this.UserInfo.UserId || this.Option == 'e')));
        this.EnableFormType = this.IsNew;
        this.Record = null;
        //this.ClientId = 0;
        //this.ProjectId = 0;
        //this.Tags = undefined;
        //this.AssignTo = undefined;
        //this.SrcUserId = 0;
        this.PostLoaded = false;
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

    doSettings() {

        var record = this.Record;

        if (record.TaskStatus <= 0)
            record.TaskStatus = 1;

        this.AssignBy = record.AssignBy;
        this.TaskStatus = record.TaskStatus;
        this.IsEditable = (!this.IsInfo) && ((this.TaskId == 0) || ((this.TaskStatus < 8) && (this.AssignBy == this.UserInfo.UserId || this.Option == 'e')));

        //this.ProjectId = record.Project_Id;
        //this.ClientId = record.ClientId;
        //this.Tags = record.Tags;
        //this.AssignTo = record.AssignTo;
        //this.SrcUserId = record.UserId;


        if (record.UserId > 0)
            app_jqx_combo_async.userInputAdapter("#UserId", record.UserId);
        if (this.IsInfo) {
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

    _loadData() {

        var _slf = this;
        var url = _slf.IsInfo ? "/System/GetTaskInfo" : "/System/GetTaskEdit";
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
                _slf.Record = record;
                _slf.doSettings();
                _slf.loadControls();
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
                if (this.IsInfo) {
                    if ($("#jqxgrid1").is(':empty'))
                        app_repeater.task_comments_adapter("#jqxgrid1", this.TaskId);
                }
                else {
                    if (this.Comment == null) {//($("#jqxgrid1")[0].childElementCount == 0) {
                        this.Comment = new app_task_comment_grid(this.TaskId, this.UserInfo, this.Option);
                        //this.Comment.load(this.TaskId, this.UserInfo, this.Option);
                    }
                }

                break;
            case 2:
                if (this.IsInfo) {
                    if ($("#jqxgrid2").is(':empty'))
                        app_repeater.task_assign_adapter("#jqxgrid2", this.TaskId);
                }
                else {
                    if (this.Assign == null) {//($("#jqxgrid2")[0].childElementCount == 0)
                        this.Assign = new app_task_assign_grid(this.TaskId, this.UserInfo, this.Option);
                    }
                }
                break;
            case 3:
                if (this.IsInfo) {
                    if ($("#jqxgrid3").is(':empty'))
                        app_repeater.task_timer_adapter("#jqxgrid3", this.TaskId);
                }
                else {
                    if (this.Timer == null)//($("#jqxgrid3")[0].childElementCount == 0)
                        this.Timer = new app_task_timer_grid(this.TaskId, this.UserInfo, this.Option);
                }
                break;
            case 4:
                if (this.TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                    return false;
                }

                if (this.IsInfo) {

                    if (this.TaskModel == 'D') {
                        if (this.Actions == null)//($("#jqxgrid4")[0].childElementCount == 0)
                            this.Actions = new app_doc_form_grid(this.TaskId, this.UserInfo, this.Option);
                    }
                    else if (this.TaskModel == 'P') {

                        if ($("#jqxgrid4").is(':empty'))
                            app_repeater.topic_form_adapter("#jqxgrid4", this.TaskId);
                    }
                    else {
                        if ($("#jqxgrid4").is(':empty'))
                            app_repeater.task_form_adapter("#jqxgrid4", this.TaskId);
                    }
                }
                else {
                    if (this.TaskModel == 'D') {
                        if (this.Actions == null)//($("#jqxgrid4")[0].childElementCount == 0)
                            this.Actions = new app_doc_form_grid(this.TaskId, this.UserInfo, this.Option);
                    }
                    else if (this.TaskModel == 'P') {

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
                            //    this.FormTemplate = new app_task_form_template(); //app_task_base.form_template.load(this.TaskId, this.UserInfo);
                            //    this.FormTemplate.load(this.TaskId, this.UserInfo);
                            //}
                        }
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

    preLoad() {

        var slf = this;

        this.parentSettings(slf.TaskParentId);
  
        $("#AccountId").val(slf.AccountId);

        //app_tasks.setColorFlag();
        app_tasks.setShareType();

        $('#a-jqxExp-1').on('click', function (e) {
            if (!slf.PostLoaded) {
                slf.postLoad();
            }
            $('#jqxExp-box').slideToggle();
            return false;
        });

        app_features.editorTag("#TaskBody", slf.IsEditable);

        if (this.IsEditable) {
            app_features.colorFlag("#ColorFlag", "#hTitle");
        }

        //$("#jqxWidget").slideDown('slow');
        $("#jqxWidget").toggleClass('box-slide');

    }

    postLoad(waitAll) {

        //waitAll = true;

        if (this.PostLoaded)
            return;

        var _slf = this;
        var clientId, projectId, tags, assignTo;
        if (this.Record) {
            clientId = this.Record.ClientId;
            projectId = this.Record.ProjectId;
            tags = this.Record.Tags;
            assignTo = this.Record.AssignTo;
        }
        var promise1 = [0, 0, 0, 0];

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', clientId, function () {
            promise1[0] = 1;
        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', projectId, function () {
            promise1[1] = 1;
        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 225, 0, true, tags, function () {
            promise1[2] = 1;
        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, assignTo, function () {
            promise1[3] = 1;
            $("#AssignTo").jqxComboBox({ disabled: true });
        });

        function afterPostLoaded() {

            _slf.PostLoaded = true;

            if (!_slf.IsEditable) {
                //$("#ClientId").prop("readonly", true);
                //$("#Project_Id").prop("readonly", true);
                $("#Tags").jqxComboBox({ disabled: true });
                $("#AssignTo").jqxComboBox({ disabled: true });
                //$("#ShareType").jqxDropDownList({ enableSelection: false });
                app.disableSelect("#ShareType");
            }

        }

        if (waitAll) {
            
            function waitUntil() {

                async function promiseCompleted() {
                    var promiseMax = 4;
                    var sum = promise1.reduce(function (acc, val) { return acc + val; }, 0);
                    if (sum == promiseMax) {
                        afterPostLoaded();
                    }
                    return sum == promiseMax;
                }

                async function delay() {
                    return new Promise(resolve => setTimeout(resolve, 300));
                }

                async function processArray(array) {
                    var maxloop = 10;
                    var counter = 0;
                    while (! await promiseCompleted()) {
                        await delay();
                        counter++;
                        if (counter >= maxloop) {
                            break;
                        }
                    }
                    console.log('Done');
                }

                processArray(promise1);
            }

            waitUntil();
        }
        else {

            afterPostLoaded();
        }


        //$("#AssignTo").keydown(function (event) {
        //   if (event.which == 8) {
        //       event.preventDefault();
        //   }
        //});

        //app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
        //app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');

        
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

    parentSettings(parentId) {
        $("#Task_Parent").val(parentId);
        if (parentId > 0) {
            $("#Task_Parent-group").show();
            $("#Task_Parent-link").click(function () {
                app_open.modelEdit(parentId, this.TaskModel);
                //app.redirectTo('/System/TaskInfo?id=' + parentId);
            });
        }
        else {
            $("#Task_Parent-group").hide();
        }
    }

    loadControls() {

        var record = this.Record;

        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

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
            app_form.loadDataForm("fcForm", record, ["TaskStatus"], this.IsInfo);// ["Project_Id", "ClientId", "Tags", "AssignTo"]);//,"Task_Type"]);

            $("#AssignTo").jqxComboBox({ disabled: record.ShareType !== 3 });
            //$("#TaskBody").jqxEditor('val', app.htmlUnescape(record.TaskBody));
            $('#TaskBody').css('text-align', app_style.langAlign(record.Lang))

            $("#hTitle-text").text(this.Title + ": " + record.TaskSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

            this.parentSettings(record.Task_Parent);

            if (this.Option !== 'g' && record.TaskStatus > 1 && record.TaskStatus < 8)
                $("#fcEnd").show();
            else
                $("#fcEnd").hide();
            
            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);

        }
        else {
            app_select_loader.loadTag("Task_Type", "Task_Type", 4);
            app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');
        }
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
            autoheight: true,
            autorowheight: true,
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

        app_task_base.triggerSubTaskCompleted('comment');
    };

    doSubmit() {
        //e.preventDefault();
        var actionUrl = $('#comment-Form').attr('action');
        var formData = $('#comment-Form').serialize();

        app_query.doFormSubmit('#comment-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {
                app_task_base.triggerSubTaskCompleted('comment', data);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };

    //return app_task_comment;
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
            autorowheight: true,
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
            app_task_base.triggerSubTaskCompleted('form');

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
        this.showControl(0, 'a', 'add');
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

        appimport.init(function (data) {

            if (data.result.Status > 0) {

                _slf.refresh();
            }

            $("#jqxgrid4-import_window").hide();
            $("#jqxgrid4-bar").show();
            app_task_base.triggerSubTaskCompleted('form');

        });

        this.Import = appimport;



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
            app_dialog.alert("לא ניתן למחוק פעולה שהוקצתה על ידי משתמש אחר");
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
                    app_task_base.triggerSubTaskCompleted('form', data);
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
                        slf.Mode = 'started';
                        $("#form-Submit").text('עדכון');
                        $("#form-Done-group").show();

                        if (record.DoneStatus == false) {
                            //slf.Mode = 'started';
                            $("#divDoneDate").hide();
                            $("#divDisplayName").hide();
                        }
                        else {
                            //$("#DoneStatus").prop('disabled', true)
                            slf.Mode = 'done';
                            app_perms.readonlyInput("#ItemText");
                            $("#DoneComment").prop('readonly', true)
                            $("#form-Submit").hide();
                            //$("#form-Start").hide();
                        }
                    }
                    else {
                        //$("#form-Start").show();
                        slf.Mode = 'start';
                        $("#form-Submit").text('התחל');
                        $("#form-Done-group").hide();

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
        app_task_base.triggerSubTaskCompleted('form');
    };
    doSubmit() {
        //e.preventDefault();
        var formData = $("#form-Form").serialize();
        var actionurl = $('#form-Form').attr('action');
        app_query.doFormSubmit("#form-Form", actionurl, null, function (data) {

            if (data.Status > 0) {
                app_task_base.triggerSubTaskCompleted('form', data);
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
                app_task_base.triggerSubTaskCompleted('form', data);
            }
        });
    };

    //return app_task_form;

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
            //width: '100%',
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
        //    app_dialog.alert("לא ניתן למחוק פעולה שהוקצתה על ידי משתמש אחר");
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
        //    app_task_base.triggerSubTaskCompleted('form');
        //});

        $('#fileupload-import').fileupload({
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
        $('#progress-import .progress-bar').css(
            'width',
            progress + '%'
        );
    };

    resetPprogress() {
        $('#progress-import .progress-bar').css(
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
    //            //       app_dialog.alert(data.Message);
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


