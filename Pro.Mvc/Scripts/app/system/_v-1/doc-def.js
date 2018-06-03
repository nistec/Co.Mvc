'use strict';

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
                slf.IsNew = slf.TaskId === 0;
                app_messenger.Notify(data, 'info');
            }
            else {
                app_messenger.Notify(data, 'info', app_task_base.getReferrer());
            }

            if (act === 'plus') {
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

        if (this.IsNew) {
            status = 1;
            actionurl = '/System/UpdateNewTask';
            RunSubmit(this, status, actionurl)
        }
        else {
            status = 2;
            actionurl = '/System/UpdateTask';
            if (status > 1 && status < 8 && act === 'end') {
                app_dialog.confirmYesNoCancel("האם לסיים משימה?", function (res) {
                    if (res === 'yes')
                        RunSubmit(this, status, '/System/TaskCompleted')
                    else if (res === 'no')//update
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

            app_form.loadDataForm("fcForm", record, ["TaskStatus", "Project_Id", "ClientId", "Tags", "AssignTo","Folder"]);

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

            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', { 'model': this.TaskModel }, 0, 120, true, null, function (status, records) {
                if (record.Task_Type >= 0)
                    $("#Task_Type").val(record.Task_Type);
            });
            $("#Task_Type").jqxComboBox({ enableSelection: this.IsEditable });
            //app_jqx_combo_async.taskStatusInputAdapter("#TaskStatus", record.TaskStatus);
            app_tasks.setTaskStatus("#TaskStatus", record.TaskStatus);

            //app_jqx_combo_async.docFolderInputAdapter("#Folder", record.Folder, function (status, records) {

            //});
        }
        else {
            app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Task_Type", '/System/GetTaskTypeList', { 'model': this.TaskModel }, 0, 120,true,"0");

            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');//

        }

        app_jqx_combo_async.docFolderInputAdapter("#Folder", record ? record.Folder : null);

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
                if (name === null || name === '') {
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
                input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'none', rule: function (input, commit) {
                    //var value = $('#DueDate').jqxDateTimeInput('value');
                    var text = $('#DueDate').jqxDateTimeInput('getText');
                    return text !== null && text.length > 0;
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

//============================================================ app_doc_form

class app_doc_form_grid extends app_task_form_grid {

    constructor(taskId, userInfo, option) {

        super(taskId, userInfo, option);
    }

    init() {
        var slf = this;
        var taskid = this.TaskId;

        var docsource = {
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

        var docAdapter = new $.jqx.dataAdapter(docsource);

        $("#jqxgrid4").jqxGrid({
            editable: slf.Option === "e",
            autoheight: true,
            autorowheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: docAdapter,
            width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
                //{
                //    text: 'מס', dataField: 'ItemTask', filterable: false, width: 90, cellsalign: 'right', align: 'center',
                //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                //        var editlink = '';
                //        //var asb = $('#jqxgrid').jqxGrid('getrowdata', row).AssignBy;
                //        //if (slf.UserId == asb)
                //        //    editlink = '<label> </label><a href="#" onclick="app_task.taskTopicEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';
                //        if (value > 0)
                //            return '<div style="text-align:center">' + value + '<a href="#" onclick="app_tasks.taskEdit(' + value + ',\"D\")" ><label> </label><i class="fa fa-plus-square-o"></i></a>' + editlink + '</div>';
                //        else
                //            return '';
                //    }
                //},
                //{ text: 'מועד לביצוע', datafield: 'ItemDueDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
                { text: 'נושא', datafield: 'ItemSubject', width: 200, cellsalign: 'right', align: 'center', editable: false },
                { text: 'תאור', datafield: 'ItemText', cellsalign: 'right', align: 'center', editable: false },
                //{ text: 'מועד התחלה', datafield: 'StartDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
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
                var editable = (rowData.AssignBy === slf.UserId);
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

        if (this.Control === null) {
            this.Control = new app_doc_form_item("#jqxgrid4-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
}


class app_doc_form_item extends app_task_form_item {

    constructor($element) {
       super($element);
    }

    tag($element, dataModel) {
        var pasive = dataModel.Option === "a" ? " pasive" : "";

        var html = '<div id="form-Window" class="container" style="margin:5px">' +
            '<hr style="width:100%;border:solid 1px #15C8D8">' +
            '<div id="form-Header">' +
            '<span id="form-Title" style="font-weight: bold;">סיכומים</span>' +
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
            //'<div class="form-group">' +
            //'<div class="field">משימה עבור:</div>' +
            //'<div id="ItemAssignTo" name="ItemAssignTo"></div>' +
            //'</div>' +
            '<div class="form-group">' +
            '<div class="field">נושא:</div>' +
            '<input id="ItemSubject" name="ItemSubject" type="text" style="width:90%" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">תאור:</div>' +
            '<textarea id="ItemText" name="ItemText" class="text-content"' + (dataModel.Option !== "a" ? " readonly=\"true\"" : "") + '></textarea>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">מועד לביצוע:</div>' +
            '<div id="ItemDueDate" name="ItemDueDate"></div>' +
            '</div>' +
            '<div class="form-group' + (dataModel.Id === 0 ? " pasive" : "") + '">' +
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
            '<textarea id="DoneComment" name="DoneComment" class="text-content"' + (dataModel.Option !== "e" ? " readonly=\"true\"" : "") + '></textarea>' +
            '</div>' +
            '</div>' +
            '<div class="form-group pasive">' +
            '<div class="field">' +
            'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option !== "e" ? " readonly=\"true\"" : "") + '/>' +
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
            //'<a id="form-Start" class="btn-default btn7 w-60" title="צור משימה" href="#">משימה</a>' +
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
        //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "ItemAssignTo", '/System/GetUserTeamList', 0, 120, false);

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
