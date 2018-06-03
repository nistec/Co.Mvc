
//============================================================ app_tasks_act
var app_tasks_act = {

    tasks_active_toggleDiv: function (id) {
        $("#toHide" + id).toggle();
    },
    tasks_active_add_comment1: function (id) {
        //$(".comment-add").empty();
        var state = $("#toComment" + id).toggle();

        var isVisible = $(state).is(":visible");
        if (isVisible) {
            $("#reminder-days" + id).focus();
            $("#comment-text" + id).focus();
        }
        //var isVisible = $("#toComment" + id).is(":visible");
        //var isHidden = $("#toComment" + id).is(":hidden");
    },
    tasks_active_add_comment: function (id) {

        //$('#windowHeader').text("הערה למשימה: " + id);

        //$('#window').jqxWindow('open');
        ////app.redirectTo('/System/_TaskCommentAdd?pid=' + id);
        //var src = app.appPath() + "/System/_TaskCommentAdd?pid=" + id;

        //app_iframe.removeIframe("windowBody");
        ////this.displayStep(step);
        //app_iframe.appendIframe("windowBody", src, "350px", "250px", true);//, loaderTag);

        $("#CommentId").val(0);
        $("#Task_Id").val(id);
        $("#UserId").val(id);
        $("#CommentText").val('');
        $("#ReminderDate").val('');


        wizard.displayStep(2);
        //wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentAdd?pid=" + id, "100%", "500px");
    },
    tasks_active_update_comment: function (id, model) {

        var comment = $("#comment-text" + id).val();
        if (comment.length == 0) {
            return app_messenger.Post("נא לציין הערה", 'error', true);
        }

        switch (model) {
            case 'T':
            case 'N':
                app_query.doPost('/System/TaskCommentAddQs', { 'Task_Id': id, 'CommentText': comment }, app_tasks_act.tasks_active_update_confirm_callback, id);
                break;
            case 'E':
                app_query.doPost('/System/TicketCommentAddQs', { 'Task_Id': id, 'CommentText': comment }, app_tasks_act.tasks_active_update_confirm_callback, id);
                break;
            case 'R':
                app_query.doPost('/System/ReminderCommentAddQs', { 'Task_Id': id, 'CommentText': comment }, app_tasks_act.tasks_active_update_confirm_callback, id);
                break;
            case 'C':
                break;
        }
    },
    tasks_active_update_confirm_callback: function (data, id) {
        //app_jqxnotify.Info(data, true);
        //dialog: function (data, template, showClose, callback, args) {

        app_messenger.Post(data);
        //Messenger().post("Your request has succeded!");
        $("#toComment" + id).hide();
        $("#comment-text" + id).val('');
    },
    tasks_getStatus: function (arg_state) {
        if (arg_state == 'done')
            return 16;
        else if (arg_state == 'work')
            return 2;
        else if (arg_state == 'new')
            return 1;
        else if (arg_state == 'today')
            return 0;
        else
            return -1;
    },
    tasks_active_open_task: function (status, id, model) {

        switch (model) {
            case 'N':
            case 'T':
                if (status >= 8)
                    app.redirectTo('/System/TaskInfo?id=' + id);
                else if (status > 1 && status < 8)
                    app.redirectTo('/System/TaskEdit?id=' + id);
                else if (status == 0)//today
                    app.redirectTo('/System/TaskStart?id=' + id);
                else if (status <= 1) {
                    app_dialog.confirm("האם לאתחל משימה?", function () {
                        app.redirectTo('/System/TaskStart?id=' + id);
                    });

                    //if (confirm("האם לאתחל משימה?"))
                    //    app.redirectTo('/System/TaskStart?id=' + id);
                }

                break;
            case 'E':
                if (status >= 8)
                    app.redirectTo('/System/TicketInfo?id=' + id);
                else if (status > 1 && status < 8)
                    app.redirectTo('/System/TicketEdit?id=' + id);
                else if (status == 0) //today
                    app.redirectTo('/System/TicketStart?id=' + id);
                else if (status <= 1) {
                    app_dialog.confirm("האם לאתחל כרטיס?", function () {
                        app.redirectTo('/System/TicketStart?id=' + id);
                    });
                }
                //if (status >= 8)
                //    app.redirectTo('/System/TicketInfo?id=' + id);
                //else
                //    app.redirectTo('/System/TicketEdit?id=' + id);
                break;
            case 'R':
                if (status >= 8)
                    app.redirectTo('/System/ReminderInfo?id=' + id);
                else
                    app.redirectTo('/System/ReminderEdit?id=' + id);
                break;
            case 'C':
                break;
        }
        //return false;
    },
    tasks_active_open_task_info: function (status, id, model) {

        switch (model) {
            case 'N':
            case 'T':
                    app.redirectTo('/System/TaskInfo?id=' + id);
                break;
            case 'E':
                    app.redirectTo('/System/TicketInfo?id=' + id);
                break;
            case 'R':
                    app.redirectTo('/System/ReminderInfo?id=' + id);
                break;
            case 'C':
                break;
        }
    },
    tasks_active_task_expired: function (status, id, model) {

        var item = '';
        switch (model) {
            case 'N':
            case 'T':
                item = 'משימה';
                break;
            case 'E':
                item = 'כרטיס';
                break;
            case 'R':
                item = 'תזכורת';
                break;
            case 'C':
                break;
        }

        if (model !== 'R' && status !== 16) {
            app_dialog.alert('לא ניתן לסגור ' + item);
            return;
        }

        app_dialog.confirm('האם לסגור ' + item, function (id) {

            //alert('expired ' + id)

            app_query.doDataPost('/System/TaskExpired', { 'TaskId': id }, function (data){

                if (data.Status > 0) {
                    //app_tasks_active.reload();
                    app_tasks_active.remove(id,true);
                }
             });

        }, id);
      
    },
    getTaskModelName: function (model) {
        switch (model) {
            case 'N':
            case 'T':
                return 'משימה';
            case 'E':
                return 'כרטיס';
            case 'R':
                return 'תזכורות';
            case 'C':
                return 'יומן';
        }
        return 'NA'
    },
    getTaskModelIcon: function (model) {
        switch (model) {
            case 'N':
            case 'T':
                return '../../Images/icons/task-24.png';
            case 'E':
                return '../../Images/icons/event-gold-24.png';
            case 'R':
                return '../../Images/icons/comment-green-24.png';
            case 'C':
                return '../../Images/icons/vi-orange-24.png';
        }
        return '../../Images/icons/task-24.png'
    }
};

//============================================================ app_tasks_active
(function ($) {

    app_tasks_active = {

        TaskStatus: 0,
        taskToday: null,
        taskShare: null,
        quickReminder:null,
        grid: function (status) {
            var slf = this;
            this.TaskStatus = status;

            var source = {
                datafields: [
                       { name: "id", map: "TaskId", type: "number" },
                       { name: "status", map: "TaskState", type: "string" },
                       { name: "text", map: "TaskSubject", type: "string" },
                       { name: "content", map: "TaskBody", type: "string" },
                       { name: "tags", map: "Tags", type: "string" },//TaskTypeName
                       { name: "color", map: "ColorFlag", type: "string" },
                       { name: "resourceId", map: "TaskModel", type: "string" }
                ],
                datatype: "json",
                id: 'TaskId',
                type: 'POST',
                url: '/System/TaskUserKanban',
                data: { 'Status': status }
            }
            var dataAdapter = new $.jqx.dataAdapter(source, {
                loadComplete: function () {
                    // data is loaded.
                }
            });

            var resourcesAdapterFunc = function () {
                var resourcesSource =
                {
                    localData: [
                          { id: 'N', name: "לא משויך", image: '../../Images/icons/diagonal-red-24.png', common: true },
                          { id: 'T', name: "משימה", image: '../../Images/icons/task-24.png' },
                          { id: 'E', name: "כרטיס", image: '../../Images/icons/event-gold-24.png' },
                          { id: 'R', name: "תזכורת", image: '../../Images/icons/comment-green-24.png' },
                          { id: 'S', name: "יומן", image: '../../Images/icons/vi-orange-24.png' }
                    ],
                    dataType: "array",
                    dataFields: [
                         { name: "id", type: "string" },
                         { name: "name", type: "string" },
                         { name: "image", type: "string" },
                         //{ name: "content", type: "string" },
                         { name: "common", type: "boolean" }
                    ]
                };
                var resourcesDataAdapter = new $.jqx.dataAdapter(resourcesSource);
                return resourcesDataAdapter;
            }
            
            $('#kanban').jqxKanban({
                rtl: true,
                template: "<div class='jqx-kanban-item' id=''>"
                        + "<div class='jqx-kanban-item-color-status'></div>"
                        + "<div class='jqx-kanban-item-avatar'></div>"
                        + "<div class='jqx-icon jqx-icon-close-white jqx-kanban-item-template-content jqx-kanban-template-icon'></div>"
                        + "<div class='jqx-kanban-item-text'></div>"
                    + "<div class='jqx-kanban-item-footer'><div class='jqx-kanban-item-button' style='dislpay=inline;float:left'><i title='Open Task' class='fa fa-pencil-square'style='font-size:20px;color:dimgrey'></i></div><div class='jqx-kanban-item-button-display' style='dislpay=inline;float:left'><i title='View Task' class='fa fa-print' style='font-size:20px;color:dimgrey'></i></div><div class='jqx-kanban-item-button-expired' style='dislpay=inline;float:left'><i title='Expired Task' class='fa fa-close' style='font-size:20px;color:red'></i></div><div class='jqx-kanban-item-button-content panel-btn-2' title='Show Content' onclick='return toggleKanbanItem(this)' style='dislpay=inline;float:left'></div></div>"
                        + "<div style='display:none' class='jqx-kanban-item-details panel-areaB' ></div>"
                        + "</div>",
                itemRenderer: function (element, item, resource) {
                    //$(element).find(".jqx-kanban-item-color-status").html("<span style='line-height: 23px; margin-left: 5px; color:white;'>" + resource.name + "</span>");
                    //$(element).find(".jqx-kanban-item-footer").css("background-color", item.color);
                    $(element).find(".jqx-kanban-item-button-content").text(item.id);
                    $(element).find(".jqx-kanban-item-details").html(app.htmlUnescape(item.content));
                    var expiredEl = $(element).find(".jqx-kanban-item-button-expired");
                    if (item.status == 'done') {
                        $(element).find(".jqx-kanban-item-button").hide();
                        expiredEl.show();
                    }
                    else {
                        expiredEl.hide();
                    }
                },
                resources: resourcesAdapterFunc(),
                source: dataAdapter,
                width: '100%',
                height: '100%',
                columns: [
                { text: "משימות סגורות", dataField: "done" },
                { text: "משימות בטיפול", dataField: "work" },
                { text: "משימות ממתינות", dataField: "new" }
                //{ text: "משימות להיום", dataField: "today" }
                ]
            });

            $('#kanban').on('itemMoved', function (event) {
                //event.preventDefault();
                //event.stopPropagation();
                var args = event.args;
                var oldParentId = args.oldParentId;
                var newParentId = args.newParentId;
                var itemData = args.itemData;
                var itemId = itemData.id;
                var oldColumn = args.oldColumn;
                var newColumn = args.newColumn;
                var newstate = app_tasks_act.tasks_getStatus(newColumn.dataField);
                var oldstate = app_tasks_act.tasks_getStatus(oldColumn.dataField);
                if (oldColumn == newColumn) {
                    var status = app_tasks_act.tasks_getStatus(itemData.status);
                    app_tasks_act.tasks_active_open_task(status, itemData.id, itemData.resourceId);
                    return;
                }

                app_query.doDataPost('/System/TaskChangeState', { itemid: itemId, newstate: newstate, oldtate: oldstate }, function (data) {

                    if (data.Status < 0) {
                        event.stopPropagation();
                        //var newContent = { text: "Cookies", content: "Content", tags: "cookies", color: "lightgreen", resourceId: 1, className: "standard" };
                        //itemData.status = oldColumn.dataField;
                        //$('#kanban').jqxKanban('updateItem', itemId, itemData);
                    }

                });
            });
            
            $('#kanban').on('itemDisplayClicked', function (event) {
                event.preventDefault();
                event.stopPropagation();
                var args = event.args;
                var itemId = args.itemId;
                var attribute = args.attribute; // template, colorStatus, content, keyword, text, avatar
                var status = app_tasks_act.tasks_getStatus(args.item.status);

                var resourceId = args.item.resourceId;
                app_tasks_act.tasks_active_open_task_info(status, itemId, resourceId)

                //showKanbanItem(itemId,"sdaf asdf asdf asdf asdf asdf asdf asdf asdfas ");
                return false;
            });

            $('#kanban').on('itemExpiredClicked', function (event) {
                event.preventDefault();
                event.stopPropagation();
                var args = event.args;
                var itemId = args.itemId;
                //var attribute = args.attribute; // template, colorStatus, content, keyword, text, avatar
                var status = app_tasks_act.tasks_getStatus(args.item.status);
                //if (args.item.status !== 'done') {
                //    app_dialog.alert('לא ניתן לסגור');
                //    return;
                //}
                var resourceId = args.item.resourceId;
                app_tasks_act.tasks_active_task_expired(status, itemId, resourceId)

                //showKanbanItem(itemId,"sdaf asdf asdf asdf asdf asdf asdf asdf asdfas ");
                return false;
            });

            $('#kanban').on('itemAttrClicked', function (event) {
                event.preventDefault();
                event.stopPropagation();
                var args = event.args;
                var itemId = args.itemId;
                var attribute = args.attribute; // template, colorStatus, content, keyword, text, avatar
                var status = app_tasks_act.tasks_getStatus(args.item.status);
                //var arg_state = args.item.status;
                //if (arg_state == 'done')
                //    status = 16;
                //else if (arg_state == 'work')
                //    status = 2;
                //else if (arg_state == 'new')
                //    status = 1;
                //else if (arg_state == 'today')
                //    status = 0;
                var resourceId = args.item.resourceId;
                app_tasks_act.tasks_active_open_task(status, itemId, resourceId)
                return false;
            });

            $('#kanban').on('columnAttrClicked', function (event) {
                var args = event.args;
                var column = args.column;
                var cancelToggle = args.cancelToggle; // false by default. Set to true to cancel toggling dynamically.
                var attribute = args.attribute; // title, button

                //log.push("columnAttrClicked is raised");
                //updateLog();
            });

            $("#exp-today").jqxExpander({ rtl: true, width: '100%', theme: 'arctic', animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });
            $('#exp-today').on('expanding', function () {
                if (slf.taskToday == null) {
                    slf.taskToday = new app_tasks_today();
                    slf.taskToday.load();
                }
            });
            $("#exp-assign").jqxExpander({ rtl: true, width: '100%', theme: 'arctic', animationType: 'fade', expandAnimationDuration: 500, collapseAnimationDuration: 350, expanded: false });
            $('#exp-assign').on('expanding', function () {
                if (slf.taskShare == null) {
                    slf.taskShare = app_tasks_share.init();
                    //slf.taskShare.init();
                }
            });

            $('#quick-reminder').click(function (e) {
                e.preventDefault();

                if (slf.quickReminder !== null) {
                    slf.quickReminder.doCancel();
                    slf.quickReminder = null;
                }
                slf.quickReminder = new app_reminder_window("#QuickReminder-window");
                slf.quickReminder.init();
                //slf.quickReminder.show();
            });
        },
        load: function (userInfo) {
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            this.grid(255);
            return this;
        },
        remove: function (itemId, refreshToday) {
            $('#kanban').jqxKanban('removeItem', itemId);
            if (refreshToday) {
                if (this.taskToday != null) {
                    this.taskToday.reload();
                }
            }
        },
        reload: function () {
            app.refresh();

            //$('#kanban').jqxKanban('destroy'); 
            //$('#kanban').jqxKanban('source').dataBind();
            ////this.grid(255);

            //if (this.taskToday != null) {
            //    this.taskToday.reload();
            //}
            return this;
        }
    };
})(jQuery)


//============================================================ app_tasks_today


var app_tasks_today = function () {

    this.TaskStatus = 0;
    this.DataAdapter = null;
    this.grid = function (status) {
        var slf = this;
        this.TaskStatus = status;

        var source = {
            datatype: "json",
            id: 'TaskId',
            type: 'POST',
            url: '/System/TaskUserToday',
            data: { 'Status': status }
        }
        this.DataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function (records) {
                // data is loaded.
                //var records = dataAdapter.records;
                var html = "<table border='0'>";

                var length = records ? records.length : 0;
                if (length == 0) {
                    var msg = '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';

                    html += "<tr>";
                    html += "<td>" + msg +"</td>";
                    html += "</tr>";
                }
                else {
                    for (var i = 0; i < length; i++) {
                        var rowData = records[i];
                        // col 1
                        var src = app_tasks_act.getTaskModelIcon(rowData.TaskModel);
                        var part1 = '<div style="text-align:center;margin: 5px;border: 0;"><a href="#"  title="הצג הכל" onclick="app_tasks_act.tasks_active_toggleDiv(' + rowData.TaskId + ')"><img style="border: 0;" src="' + src + '" alt="" width="20" height="20" /></a></div>';
                        var part2 = '<div style="text-align:center;margin: 5px;border: 0;"><a href="#" title="Open " onclick=" app_tasks_act.tasks_active_open_task(' + rowData.TaskStatus + ',' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-pencil-square" style="font-size:20px;"></i></a></div>'
                        var part3 = '<div style="text-align:center;margin: 5px;border: 0;"><a href="#" title="Expired " onclick=" app_tasks_act.tasks_active_task_expired(' + rowData.TaskStatus + ',' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-close" style="font-size:20px;color:red"></i></a></div>'

                        var col1 = part1 + part2;
                        if (rowData.TaskModel == 'R')
                            col1 += part3;

                        //col 2
                        var container = '<div style="width: 100%; height: 100%;">'

                        //var reminderitem = $('<div id="date-reminder' + rowData.TaskId + '" style="margin:10px"></div>').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });

                        var tsk_modelname = app_tasks_act.getTaskModelName(rowData.TaskModel);
                        var created = app.formatDateString(rowData.CreatedDate);
                        var dueDate = app.formatDateString(rowData.DueDate);
                        var startedDate = app.formatDateString(rowData.StartedDate);
                        var body = rowData.TaskBody && rowData.TaskBody != null ? rowData.TaskBody : '';

                        var expColumn =
                            '<div class="expanderContainer rtl">' +
                            '<div class="icon icon-news"></div>' +
                            '<div class="expander">' +
                            '<div><div style="color:' + (rowData.ColorFlag || '#000') + '">' + tsk_modelname + ': ' + rowData.TaskSubject + '</div>' +
                            '<div class="rtl"><spn> נוצר ע"י: </span>' + rowData.DisplayName + '<span class="glyphicon glyphicon-calendar glyphicon-custom1"></span><span class="glyphicon glyphicon-time glyphicon-custom2"> </span><spn> ב: </span><span title="נוצר בתאריך">' + created + '</span></div>' +
                            '<div id="toHide' + rowData.TaskId + '" style="display:none" class="toHide1 toHide">' +
                            '<div><b>מועד לביצוע:</b>' + dueDate + '</div>' +
                            '<div><b>מועד התחלה:</b>' + startedDate + '</div>' +
                            '<div><b>נוצר ע"י:</b>' + (rowData.AssignByName || '') + '</div>' +
                            '</div>' +
                            '<div id="toComment' + rowData.TaskId + '" style="display:none;">' +
                            '<textarea tabindex="1" id="comment-text' + rowData.TaskId + '" class="comment-add" style="width:99%;height:40px"></textarea>' +
                            '<div><span>תזכורת בעוד: </span><input tabindex="2" type="number" value="0" style="width:40px" id="reminder-days' + rowData.TaskId + '" /><span>ימים</span>' +
                            '<button tabindex="3" onclick="app_tasks_act.tasks_active_update_comment(' + rowData.TaskId + ')">אישור</button></div>' +
                            '</div>' +
                            '</div>' +
                            '<div><div class="newsTextContainer">' + app.htmlText(app.htmlUnescape(body)) + '</div></div>' +
                            '</div>' +
                            '</div>';
                        container += expColumn;
                        container += "</div>";


                        html += "<tr>";
                        html += "<td>" + col1 + "</td>";
                        html += "<td>" + container + "</td>";
                        html += "</tr>";
                    }
                }
                html += "</table>";
                $("#taskToday").html(html);
            }
        });
        this.DataAdapter.dataBind();
        /*
        $("#taskToday").jqxGrid(
            {
                width: '100%',
                //height: "400px",
                rtl: true,
                autoheight: true,
                altrows: true,
                //editable:true ,
                pageable: false,
                pageSize: 10,
                pagerButtonsCount: 5,
                showHeader: false,
                enableBrowserSelection: true,
                //enableHover: false,
                //selectionMode: "singleRow",
                localization: getLocalization('he'),
                theme: 'energyblue',
                //sortable: true,
                //height: 700,
                autoRowHeight: true,
                //filterable: true,
                //filterMode: 'simple',
                //filterMode: 'advanced',
                source: dataAdapter,

                columns: [
                    {
                        text: '...', align: 'center', dataField: 'TaskHex', cellsalign: 'center', width: '10%', filterable: false,
                        cellsRenderer: function (row, column, value, rowData) {

                            //var part1 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a href="#" title="הצג הכל" onclick="tasks_active_toggleDiv(' + rowData.TaskId + ')"><i class="fa fa-plus-square-o"></i></a></div>';

                            var src = app_tasks_act.getTaskModelIcon(rowData.TaskModel);
                            var part1 = '<div style="text-align:center;margin: 5px;border: 0;background-color:' + value + '"><a href="#"  title="הצג הכל" onclick="app_tasks_act.tasks_active_toggleDiv(' + rowData.TaskId + ')"><img style="border: 0;" src="' + src + '" alt="" width="20" height="20" /></a></div>';
                            var part2 = '<div><a href="#" title="Open " onclick=" app_tasks_act.tasks_active_open_task(' + rowData.TaskStatus + ',' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-pencil-square"></i></a></div>'


                            //if (rowData.TaskModel != 'T' || (rowData.TaskStatus > 1 && rowData.TaskStatus < 8))
                            //    var part2 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a class="popoverBtn" href="#" title="הוספת הערה" onclick="app_tasks_act.tasks_active_add_comment(' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-comment-o"></i></a></div>';

                            return part1 + part2;
                        }
                    },
                    {
                        text: 'משימה', align: 'center', dataField: 'DueDate', width: '90%', filterable: true,
                        // row - row's index.
                        // column - column's data field.
                        // value - cell's value.
                        // rowData - rendered row's object.
                        cellsRenderer: function (row, column, value, rowData) {

                            var container = '<div style="width: 100%; height: 100%;">'

                            //var reminderitem = $('<div id="date-reminder' + rowData.TaskId + '" style="margin:10px"></div>').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });

                            var tsk_modelname = app_tasks_act.getTaskModelName(rowData.TaskModel);
                            var created = app.formatDateString(rowData.CreatedDate);
                            var dueDate = app.formatDateString(rowData.DueDate);
                            var startedDate = app.formatDateString(rowData.StartedDate);
                            var body = rowData.TaskBody && rowData.TaskBody != null ? rowData.TaskBody : '';

                            var expColumn =
                                '<div class="expanderContainer rtl">' +
                                '<div class="icon icon-news"></div>' +
                                '<div class="expander">' +
                                '<div><div style="color:' + (rowData.ColorFlag || '#000') + '">' + tsk_modelname + ': ' + rowData.TaskSubject + '</div>' +
                                '<div class="rtl"><spn> נוצר ע"י: </span>' + rowData.DisplayName + '<span class="glyphicon glyphicon-calendar glyphicon-custom1"></span><span class="glyphicon glyphicon-time glyphicon-custom2"> </span><spn> ב: </span><span title="נוצר בתאריך">' + created + '</span></div>' +
                                '<div id="toHide' + rowData.TaskId + '" style="display:none" class="toHide1 toHide">' +
                                '<div><b>מועד לביצוע:</b>' + dueDate + '</div>' +
                                '<div><b>מועד התחלה:</b>' + startedDate + '</div>' +
                                '<div><b>נוצר ע"י:</b>' + (rowData.AssignByName || '') + '</div>' +
                                '</div>' +
                                '<div id="toComment' + rowData.TaskId + '" style="display:none;">' +
                                '<textarea tabindex="1" id="comment-text' + rowData.TaskId + '" class="comment-add" style="width:99%;height:40px"></textarea>' +
                                '<div><span>תזכורת בעוד: </span><input tabindex="2" type="number" value="0" style="width:40px" id="reminder-days' + rowData.TaskId + '" /><span>ימים</span>' +
                                '<button tabindex="3" onclick="app_tasks_act.tasks_active_update_comment(' + rowData.TaskId + ')">אישור</button></div>' +
                                '</div>' +
                                '</div>' +
                                '<div><div class="newsTextContainer">' + app.htmlText(app.htmlUnescape(body)) + '</div></div>' +
                                '</div>' +
                                '</div>';

                            //<span title="מועד לביצוע">' + dueDate + '</span>
                            //'<div><b>מועד סיום:</b>' + app.toLocalDateString(rowData.EndedDate) + '</div>' +

                            //container += leftcolumn;
                            //container += rightcolumn;

                            container += expColumn;
                            container += "</div>";

                            return container;
                        }
                    }
                    //{ text: 'נושא', align: 'center', dataField: 'TaskSubject', filterable: true, hidden: true }
                ]
            });
        */

    };
    this.getrowId = function () {

        var selectedrowindex = $("#taskToday").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#taskToday").jqxGrid('getrowid', selectedrowindex);
        return id;
    };
    this.load = function () {
        //userInfo
        //this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(255);
        return this;
    };
    this.reload = function (status) {
        //var src = $('#dataTable').jqxDataTable('source');
        //src._source.data = { 'Status': status };
       // $('#taskToday').jqxGrid('source').dataBind();
        this.grid(255);
    };
    this.remove = function () {
        //var id = this.getrowId();
        //if (id > 0) {
        //    if (confirm('האם למחוק הערה ' + id)) {
        //        app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id });
        //        $('#dataTable').jqxDataTable('source').dataBind();
        //    }
        //}
    };
    this.refresh = function () {
        //$('#taskToday').jqxGrid('source').dataBind();
        this.grid(255);
    };

};


var app_tasks_today_2 = function () {

    this.TaskStatus = 0;
    this.grid = function (status) {
        var slf = this;
        this.TaskStatus = status;

        var source = {
            datatype: "json",
            id: 'TaskId',
            type: 'POST',
            url: '/System/TaskUserToday',
            data: { 'Status': status }
        }
        var dataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function () {
                // data is loaded.
            }
        });

        $("#taskToday").jqxDataTable(
                {
                    width: '100%',
                    height: "400px",
                    rtl: true,
                    //editable:true ,
                    pageable: false,
                    pageSize: 10,
                    pagerButtonsCount: 5,
                    showHeader: false,
                    enableBrowserSelection: true,
                    //enableHover: false,
                    //selectionMode: "singleRow",
                    localization: getLocalization('he'),
                    theme: 'energyblue',
                    //sortable: true,
                    //height: 700,
                    autoRowHeight: true,
                    //filterable: true,
                    //filterMode: 'simple',
                    //filterMode: 'advanced',
                    source: dataAdapter,

                    columns: [
                          {
                              text: '...', align: 'center', dataField: 'TaskHex', cellsalign: 'center', width: 40, filterable: false,
                              cellsRenderer: function (row, column, value, rowData) {

                                  //var part1 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a href="#" title="הצג הכל" onclick="tasks_active_toggleDiv(' + rowData.TaskId + ')"><i class="fa fa-plus-square-o"></i></a></div>';

                                  var src = app_tasks_act.getTaskModelIcon(rowData.TaskModel);
                                  var part1 = '<div style="text-align:center;margin: 5px;border: 0;background-color:' + value + '"><a href="#"  title="הצג הכל" onclick="app_tasks_act.tasks_active_toggleDiv(' + rowData.TaskId + ')"><img style="border: 0;" src="' + src + '" alt="" width="20" height="20" /></a></div>';
                                  var part2 = '<div><a href="#" title="Open " onclick=" app_tasks_act.tasks_active_open_task(' + rowData.TaskStatus + ',' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-pencil-square"></i></a></div>'


                                  //if (rowData.TaskModel != 'T' || (rowData.TaskStatus > 1 && rowData.TaskStatus < 8))
                                  //    var part2 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a class="popoverBtn" href="#" title="הוספת הערה" onclick="app_tasks_act.tasks_active_add_comment(' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-comment-o"></i></a></div>';

                                  return part1 + part2;
                              }
                          },
                          {
                              text: 'משימה', align: 'center', dataField: 'DueDate', filterable: true,
                              // row - row's index.
                              // column - column's data field.
                              // value - cell's value.
                              // rowData - rendered row's object.
                              cellsRenderer: function (row, column, value, rowData) {

                                  var container = '<div style="width: 100%; height: 100%;">'

                                  //var reminderitem = $('<div id="date-reminder' + rowData.TaskId + '" style="margin:10px"></div>').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });

                                  var tsk_modelname = app_tasks_act.getTaskModelName(rowData.TaskModel);
                                  var created = app.formatDateString(rowData.CreatedDate);
                                  var dueDate = app.formatDateString(rowData.DueDate);
                                  var startedDate = app.formatDateString(rowData.StartedDate);
                                  var body = rowData.TaskBody && rowData.TaskBody != null ? rowData.TaskBody : '';

                                  var expColumn =
                                  '<div class="expanderContainer rtl">' +
                                      '<div class="icon icon-news"></div>' +
                                      '<div class="expander">' +
                                          '<div><div style="color:' + (rowData.ColorFlag || '#000') + '">' + tsk_modelname + ': ' + rowData.TaskSubject + '</div>' +
                                          '<div class="rtl"><spn> נוצר ע"י: </span>' + rowData.DisplayName + '<span class="glyphicon glyphicon-calendar glyphicon-custom1"></span><span class="glyphicon glyphicon-time glyphicon-custom2"> </span><spn> ב: </span><span title="נוצר בתאריך">' + created + '</span></div>' +
                                          '<div id="toHide' + rowData.TaskId + '" style="display:none" class="toHide1 toHide">' +
                                                  '<div><b>מועד לביצוע:</b>' + dueDate + '</div>' +
                                                  '<div><b>מועד התחלה:</b>' + startedDate + '</div>' +
                                                  '<div><b>נוצר ע"י:</b>' + (rowData.AssignByName || '') + '</div>' +
                                          '</div>' +
                                              '<div id="toComment' + rowData.TaskId + '" style="display:none;">' +
                                                  '<textarea tabindex="1" id="comment-text' + rowData.TaskId + '" class="comment-add" style="width:99%;height:40px"></textarea>' +
                                                  '<div><span>תזכורת בעוד: </span><input tabindex="2" type="number" value="0" style="width:40px" id="reminder-days' + rowData.TaskId + '" /><span>ימים</span>' +
                                                  '<button tabindex="3" onclick="app_tasks_act.tasks_active_update_comment(' + rowData.TaskId + ')">אישור</button></div>' +
                                              '</div>' +
                                          '</div>' +
                                          '<div><div class="newsTextContainer">' + app.htmlText(app.htmlUnescape(body)) + '</div></div>' +
                                      '</div>' +
                                  '</div>';

                                  //<span title="מועד לביצוע">' + dueDate + '</span>
                                  //'<div><b>מועד סיום:</b>' + app.toLocalDateString(rowData.EndedDate) + '</div>' +

                                  //container += leftcolumn;
                                  //container += rightcolumn;

                                  container += expColumn;
                                  container += "</div>";

                                  return container;
                              }
                          }
                           //{ text: 'נושא', align: 'center', dataField: 'TaskSubject', filterable: true, hidden: true }
                    ]
                });


    };
    this.getrowId = function () {

        var selectedrowindex = $("#taskToday").jqxDataTable('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#taskToday").jqxDataTable('getrowid', selectedrowindex);
        return id;
    };
    this.load = function () {
        //userInfo
        //this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(255);
        return this;
    };
    this.reload = function (status) {
        //var src = $('#dataTable').jqxDataTable('source');
        //src._source.data = { 'Status': status };
        $('#taskToday').jqxDataTable('source').dataBind();
    };
    this.remove = function () {
        //var id = this.getrowId();
        //if (id > 0) {
        //    if (confirm('האם למחוק הערה ' + id)) {
        //        app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id });
        //        $('#dataTable').jqxDataTable('source').dataBind();
        //    }
        //}
    };
    this.refresh = function () {
        $('#taskToday').jqxDataTable('source').dataBind();
    };

};

var app_tasks_assign_2 = function () {

    this.TaskStatus = 0;
    this.grid = function (status) {
        var slf = this;
        this.TaskStatus = status;

        var source = {
            datatype: "json",
            id: 'TaskId',
            type: 'POST',
            url: '/System/TaskUserAssign',
            data: { 'Status': status }
        }
        var dataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function () {
                // data is loaded.
            }
        });

        $("#taskAssign").jqxDataTable(
                {
                    width: '100%',
                    height: "400px",
                    rtl: true,
                    //editable:true ,
                    pageable: false,
                    pageSize: 10,
                    pagerButtonsCount: 5,
                    showHeader: false,
                    enableBrowserSelection: true,
                    //enableHover: false,
                    //selectionMode: "singleRow",
                    localization: getLocalization('he'),
                    theme: 'energyblue',
                    //sortable: true,
                    //height: 700,
                    autoRowHeight: true,
                    //filterable: true,
                    //filterMode: 'simple',
                    //filterMode: 'advanced',
                    source: dataAdapter,

                    columns: [
                          {
                              text: '...', align: 'center', dataField: 'TaskHex', cellsalign: 'center', width: 40, filterable: false,
                              cellsRenderer: function (row, column, value, rowData) {

                                  //var part1 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a href="#" title="הצג הכל" onclick="tasks_active_toggleDiv(' + rowData.TaskId + ')"><i class="fa fa-plus-square-o"></i></a></div>';

                                  var src = app_tasks_act.getTaskModelIcon(rowData.TaskModel);
                                  var part1 = '<div style="text-align:center;margin: 5px;border: 0;background-color:' + value + '"><a href="#"  title="הצג הכל" onclick="app_tasks_act.tasks_active_toggleDiv(' + rowData.TaskId + ')"><img style="border: 0;" src="' + src + '" alt="" width="20" height="20" /></a></div>';
                                  var part2 = '<div><a href="#" title="Open " onclick=" app_tasks_act.tasks_active_open_task(' + rowData.TaskStatus + ',' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-pencil-square"></i></a></div>'


                                  //if (rowData.TaskModel != 'T' || (rowData.TaskStatus > 1 && rowData.TaskStatus < 8))
                                  //    var part2 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a class="popoverBtn" href="#" title="הוספת הערה" onclick="app_tasks_act.tasks_active_add_comment(' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')"><i class="fa fa-comment-o"></i></a></div>';

                                  return part1 + part2;
                              }
                          },
                          {
                              text: 'משימה', align: 'center', dataField: 'DueDate', filterable: true,
                              // row - row's index.
                              // column - column's data field.
                              // value - cell's value.
                              // rowData - rendered row's object.
                              cellsRenderer: function (row, column, value, rowData) {


                                  var container = '<div style="width: 100%; height: 100%;">'


                                  //var reminderitem = $('<div id="date-reminder' + rowData.TaskId + '" style="margin:10px"></div>').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });

                                  var tsk_modelname = app_tasks_act.getTaskModelName(rowData.TaskModel);
                                  var created = app.formatDateString(rowData.CreatedDate);
                                  var dueDate = app.formatDateString(rowData.DueDate);
                                  var startedDate = app.formatDateString(rowData.StartedDate);
                                  var body = rowData.TaskBody && rowData.TaskBody != null ? rowData.TaskBody : '';

                                  var expColumn =
                                  '<div class="expanderContainer rtl">' +
                                      '<div class="icon icon-news"></div>' +
                                      '<div class="expander">' +
                                          '<div><div style="color:' + (rowData.ColorFlag || '#000') + '">' + tsk_modelname + ': ' + rowData.TaskSubject + '</div>' +
                                          '<div class="rtl"><spn> נוצר ע"י: </span>' + rowData.DisplayName + '<span class="glyphicon glyphicon-calendar glyphicon-custom1"></span><span class="glyphicon glyphicon-time glyphicon-custom2"> </span><spn> ב: </span><span title="נוצר בתאריך">' + created + '</span></div>' +
                                          '<div id="toHide' + rowData.TaskId + '" style="display:none" class="toHide1 toHide">' +
                                                  '<div><b>מועד לביצוע:</b>' + dueDate + '</div>' +
                                                  '<div><b>מועד התחלה:</b>' + startedDate + '</div>' +
                                                  '<div><b>נוצר ע"י:</b>' + (rowData.AssignByName || '') + '</div>' +
                                          '</div>' +
                                              '<div id="toComment' + rowData.TaskId + '" style="display:none;">' +
                                                  '<textarea tabindex="1" id="comment-text' + rowData.TaskId + '" class="comment-add" style="width:99%;height:40px"></textarea>' +
                                                  '<div><span>תזכורת בעוד: </span><input tabindex="2" type="number" value="0" style="width:40px" id="reminder-days' + rowData.TaskId + '" /><span>ימים</span>' +
                                                  '<button tabindex="3" onclick="app_tasks_act.tasks_active_update_comment(' + rowData.TaskId + ')">אישור</button></div>' +
                                              '</div>' +
                                          '</div>' +
                                          '<div><div class="newsTextContainer">' + app.htmlText(app.htmlUnescape(body)) + '</div></div>' +
                                      '</div>' +
                                  '</div>';

                                  //<span title="מועד לביצוע">' + dueDate + '</span>
                                  //'<div><b>מועד סיום:</b>' + app.toLocalDateString(rowData.EndedDate) + '</div>' +

                                  //container += leftcolumn;
                                  //container += rightcolumn;

                                  container += expColumn;
                                  container += "</div>";

                                  return container;
                              }
                          }
                           //{ text: 'נושא', align: 'center', dataField: 'TaskSubject', filterable: true, hidden: true }
                    ]
                });


    };
    this.getrowId = function () {

        var selectedrowindex = $("#taskAssign").jqxDataTable('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#taskAssign").jqxDataTable('getrowid', selectedrowindex);
        return id;
    };
    this.load = function () {
        //userInfo
        //this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(255);
        return this;
    };
    this.reload = function (status) {
        //var src = $('#dataTable').jqxDataTable('source');
        //src._source.data = { 'Status': status };
        $('#taskAssign').jqxDataTable('source').dataBind();
    };
    this.remove = function () {
        //var id = this.getrowId();
        //if (id > 0) {
        //    if (confirm('האם למחוק הערה ' + id)) {
        //        app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id });
        //        $('#dataTable').jqxDataTable('source').dataBind();
        //    }
        //}
    };
    this.refresh = function () {
        $('#taskAssign').jqxDataTable('source').dataBind();
    };

};


//============================================================================================ app_tasks_share

    //var task_grid;
    //var wizard;

app_tasks_share = {

    //accType: 0,
    NScustomers: {},
    dataAdapter: {},
    AllowEdit: 0,
    //TaskStatus: 0,
    IsMobile: false,
    source:
        {
            datatype: "json",
            //async: false,
            datafields: [
               { name: 'TaskId', type: 'number' },
               { name: 'TaskSubject', type: 'string' },
               { name: 'TaskBody', type: 'string' },
               { name: 'Task_Type', type: 'number' },
               { name: 'Task_Parent', type: 'number' },
               { name: 'Project_Id', type: 'number' },
               { name: 'CreatedDate', type: 'date' },
               { name: 'DueDate', type: 'date' },
               { name: 'StartedDate', type: 'date' },
               { name: 'EndedDate', type: 'date' },
               { name: 'LastUpdate', type: 'date' },
               { name: 'LastAct', type: 'string' },
               { name: 'TaskEstimateDays', type: 'number' },
               { name: 'AccountId', type: 'number' },
               { name: 'UserId', type: 'number' },
               { name: 'DisplayName', type: 'string' },
               { name: 'AssignByName', type: 'string' },
               { name: 'TaskStatus', type: 'number' },
               { name: 'StatusName', type: 'string' },
               { name: 'TaskTypeName', type: 'string' },
               { name: 'ProjectName', type: 'string' },
               { name: 'IsShare', type: 'boolean' },
               { name: 'ShareType', type: 'number' },
               { name: 'TotalTimeView', type: 'string' }
               //{ name: 'TotalRows', type: 'number' }
            ],
            id: 'TaskId',
            type: 'POST',
            url: '/System/TaskUserShare',
            //data: { 'Status': status },
            pagenum: 0,
            pagesize: 20
        },
    edit: function (id) {
        app.redirectTo('/System/TaskEdit?id=' + id);

    },
    end: function (refresh) {
        wizard.displayStep(1);
        $('#divPartial').html('');
        if (refresh)
            $('#jqxgrid').jqxGrid('source').dataBind();
    },
    getTotalRows: function (data) {
        if (data) {
            return dataTotalRows(data);
        }
        return 0;
    },
    init: function (status) {
        this.IsMobile = app.IsMobile();
        //Model, userInfo, 
        //if (status === undefined)
        //    status = 255;
        //this.TaskStatus = status;

        this.NScustomers.nastedCategoriesGrids = new Array();

        this.AllowEdit = 0;// userInfo.UserRole > 4 ? 1 : 0;
        //this.source.data['Status'] = status;
        //this.source.data['AccountId'] = Model.AccountId;
        //this.source.data['UserId'] = Model.UserId;

        this.dataAdapter = new $.jqx.dataAdapter(this.source, {
            loadComplete: function (data) {
            },
            loadError: function (xhr, status, error) {
                app_dialog.alert(' status: ' + status + '\n error ' + error)
            }
        });

        this.grid();

        return this;
    },
    grid: function () {
        var slf = this;
        var subjectWidth = slf.IsMobile ? 250 : 800;

        var initTaskFormGrid = function (tab, index, id) {

            var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
            nastedcontainer.rtl = true;
            var nastedsource = {
                datafields: [
                      { name: 'ItemId', type: 'number' },
                      { name: 'UserId', type: 'number' },
                      { name: 'DisplayName', type: 'string' },
                      { name: 'AssignBy', type: 'number' },
                      { name: 'Task_Id', type: 'number' },
                      { name: 'ItemDate', type: 'date' },
                      { name: 'DoneDate', type: 'date' },
                      { name: 'ItemText', type: 'string' },
                      { name: 'DoneComment', type: 'string' },
                      { name: 'DoneStatus', type: 'bool' }
                ],
                datatype: "json",
                id: 'ItemId',
                type: 'POST',
                url: '/System/GetTasksFormGrid',
                data: { 'pid': id }
            }
            slf.NScustomers.nastedCategoriesGrids[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                localization: getLocalization('he'),
                source: nastedAdapter, width: '99%', height: 130,
                columnsresize: true,
                rtl: true,
                columns: [
                  { text: 'מועד רישום', datafield: 'ItemDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'תאור', datafield: 'ItemText', cellsalign: 'right', align: 'center' },
                  { text: 'מועד ביצוע', datafield: 'DoneDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'סיום', datafield: 'DoneComment', cellsalign: 'right', align: 'center' },
                  { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center' },
                  { text: 'מבצע', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' }
                ]
            });

            $(tab).append(nastedcontainer);

        };

        var initTaskAssignmentsGrid = function (tab, index, id) {

            var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
            nastedcontainer.rtl = true;
            var nastedsource = {
                datafields: [
                      { name: 'AssignId', type: 'number' },
                      { name: 'UserId_AssignedBy', type: 'number' },
                      { name: 'UserId_AssignedTo', type: 'number' },
                      { name: 'Task_Id', type: 'number' },
                      { name: 'AsignDate', type: 'date' },
                      { name: 'AssignSubject', type: 'string' },
                      { name: 'AssignedByName', type: 'string' },
                      { name: 'AssignedToName', type: 'string' }
                ],
                datatype: "json",
                id: 'AssignId',
                type: 'POST',
                url: '/System/GetTasksAssignGrid',
                data: { 'pid': id }
            }
            slf.NScustomers.nastedCategoriesGrids[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                localization: getLocalization('he'),
                source: nastedAdapter, width: '99%', height: 130,
                columnsresize: true,
                rtl: true,
                columns: [
                  { text: 'מועד רישום', datafield: 'AsignDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'עבר מ', datafield: 'AssignedByName', width: 120, cellsalign: 'right', align: 'center' },
                  { text: 'עבר ל', datafield: 'AssignedToName', width: 120, cellsalign: 'right', align: 'center' },
                  { text: 'נושא', datafield: 'AssignSubject', cellsalign: 'right', align: 'center' }
                ]
            });
            $(tab).append(nastedcontainer);

        };

        var initTaskCommentsGrid = function (tab, index, id) {

            var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
            nastedcontainer.rtl = true;
            var nastedsource = {
                datafields: [
                      { name: 'CommentId', type: 'number' },
                      { name: 'CommentDate', type: 'date' },
                      { name: 'CommentText', type: 'string' },
                      { name: 'Attachment', type: 'string' },
                      { name: 'DisplayName', type: 'string' },
                      { name: 'Task_Id', type: 'number' },
                      { name: 'UserId', type: 'number' }
                ],
                datatype: "json",
                id: 'CommentId',
                type: 'POST',
                url: '/System/GetTasksCommentGrid',
                data: { 'pid': id }
            }
            slf.NScustomers.nastedCategoriesGrids[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                localization: getLocalization('he'),
                source: nastedAdapter, width: '99%', height: 130,
                columnsresize: true,
                rtl: true,
                columns: [
                  { text: 'מועד רישום', datafield: 'CommentDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center' },
                  { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' }
                ]
            });
            $(tab).append(nastedcontainer);

        };

        var initTaskTimersGrid = function (tab, index, id) {

            var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
            nastedcontainer.rtl = true;
            var nastedsource = {
                datafields: [
                      { name: 'TaskTimerId', type: 'number' },
                      { name: 'Task_Id', type: 'number' },
                      { name: 'SubIndex', type: 'number' },
                      { name: 'StartTime', type: 'date' },
                      { name: 'EndTime', type: 'date' },
                      { name: 'Duration', type: 'string' },
                      { name: 'Subject', type: 'string' },
                      { name: 'DisplayName', type: 'string' },
                      { name: 'UserId', type: 'number' }
                ],
                datatype: "json",
                id: 'TaskTimerId',
                type: 'POST',
                url: '/System/GetTasksTimerGrid',
                data: { 'pid': id }
            }
            slf.NScustomers.nastedCategoriesGrids[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                source: nastedAdapter, width: '98%', height: 130,
                rtl: true,
                columns: [
                  { text: 'מועד התחלה', datafield: 'StartTime', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'מועד סיום', datafield: 'EndTime', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'משך', datafield: 'Duration', width: 100, cellsalign: 'right', align: 'center' },
                  { text: 'נושא', datafield: 'Subject', cellsalign: 'right', align: 'center' },
                  { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' }
                ]
            });
            $(tab).append(nastedcontainer);

        };

        var initrowdetails = function (index, parentElement, gridElement, datarecord) {

            slf.NScustomers.currentIndex = index;

            var tabsdiv = $($(parentElement).children()[0]);
            if (tabsdiv != null) {
                var tabinfo = tabsdiv.find('.information');
                var tabnotes = tabsdiv.find('.body');
                var tabcomments = tabsdiv.find('.comments');
                var tabhistory = tabsdiv.find('.history');
                var tabatimers = tabsdiv.find('.timers');
                var tabform = tabsdiv.find('.form');

                var title = tabsdiv.find('.title');
                title.text('משימה: ' + datarecord.TaskId);

                //tab notes
                var notescontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"><span>' + app.htmlUnescape(datarecord.TaskBody) + '</span></div>');
                notescontainer.rtl = true;
                $(tabnotes).append(notescontainer);

                //fill info
                var container = $('<div style="margin: 5px;text-align:right;"></div>')
                container.rtl = true;
                //container.appendTo($(tabinfo));

                var leftcolumn = $('<div style="float: left; width: 45%;direction:rtl;"></div>');
                var rightcolumn = $('<div style="float: right; width: 40%;direction:rtl;"></div>');
                container.append(leftcolumn);
                container.append(rightcolumn);

                var divLeft = $(
                    "<div style='margin: 10px;'><b>מטפל נוכחי:</b> " + (datarecord.DisplayName || '') + "</div>" +
                    "<div style='margin: 10px;'><b>יוצר המשימה:</b> " + (datarecord.AssignByName || '') + "</div>" +
                    "<div style='margin: 10px;'><b>מועד עדכון:</b> " + datarecord.CreatedDate.toLocaleDateString() + "</div>" +
                    "<div style='margin: 10px;'><b>פרוייקט:</b> " + (datarecord.ProjectName || '') + "</div>");

                divLeft.rtl = true;
                var divRight = $("<div style='margin: 10px;'><b>לקוח\\מנוי:</b> " + (datarecord.ClientDetails || '') + "</div>" +
                    "<div style='margin: 10px;'><b>סוג משימה:</b> " + (datarecord.TaskTypeName || '') + "</div>" +
                    "<div style='margin: 10px;'><b>מועד התחלה:</b> " + app.toLocalDateString(datarecord.StartedDate) + "</div>" +
                    "<div style='margin: 10px;'><b>מועד סיום:</b> " + app.toLocalDateString(datarecord.EndedDate) + "</div>");

                divRight.rtl = true;
                $(leftcolumn).append(divLeft);
                $(rightcolumn).append(divRight);

                $(tabinfo).append(container);

                var rcdid = datarecord.TaskId;//parseInt(datarecord.TaskId);

                initTaskCommentsGrid(tabcomments, index, rcdid);
                initTaskTimersGrid(tabatimers, index, rcdid);
                initTaskAssignmentsGrid(tabhistory, index, rcdid);
                initTaskFormGrid(tabform, index, rcdid);

                $(tabsdiv).jqxTabs({ width: '95%', height: 170, rtl: true });
            }
        };

        var renderstatusbar = function (statusbar) {
            // appends buttons to the status bar.
            var container = $("<div style='overflow: hidden; position: relative; margin: 5px;float:right;'></div>");
            var addButton = $("<div style='float: left; margin-left: 5px;' title='הוספת מנוי חדש' ><img src='../scripts/app/images/add.gif'><span style='margin-left: 4px; position: relative;'>הוסף</span></div>");
            var editButton = $("<div style='float: left; margin-left: 5px;' title='הצג את הרשומה המסומנת' ><img src='../scripts/app/images/edit.gif'><span style='margin-left: 4px; position: relative;'>הצג</span></div>");
            var deleteButton = $("<div style='float: left; margin-left: 5px;' title='מחק את הרשומה המסומנת'><img src='../scripts/app/images/delete.gif'><span style='margin-left: 4px; position: relative;'>מחיקה</span></div>");
            var reloadButton = $("<div style='float: left; margin-left: 5px;' title='רענון'><img src='../scripts/app/images/refresh.gif'><span style='margin-left: 4px; position: relative;'>רענון</span></div>");
            var clearFilterButton = $("<div style='float: left; margin-left: 5px;' title='הסר סינון' ><img src='../scripts/app/images/filterRemove.gif'><span style='margin-left: 4px; position: relative;'>הסר סינון</span></div>");
            var queryButton = $("<div style='float: left; margin-left: 5px;' title='איתור' ><img src='../scripts/app/images/search.gif'><span style='margin-left: 4px; position: relative;'>איתור</span></div>");
            container.append(reloadButton);
            container.append(clearFilterButton);
            container.append(queryButton);
            if (slf.AllowEdit == 1) {
                container.append(deleteButton);
            }
            container.append(editButton);
            container.append(addButton);
            statusbar.append(container);
            addButton.jqxButton({ width: 70, height: 20 });
            editButton.jqxButton({ width: 70, height: 20 });
            deleteButton.jqxButton({ width: 70, height: 20 });
            reloadButton.jqxButton({ width: 70, height: 20 });
            clearFilterButton.jqxButton({ width: 70, height: 20 });
            queryButton.jqxButton({ width: 70, height: 20 });
            //searchButton.jqxButton({ width: 50, height: 20 });
            // add new row.
            addButton.click(function (event) {
                //var datarow = generatedata(1);
                //$("#jqxgrid").jqxGrid('addrow', null, datarow[0]);
                app_popup.memberEdit(0);
            });
            editButton.click(function (event) {
                //var datarow = generatedata(1);
                //$("#jqxgrid").jqxGrid('addrow', null, datarow[0]);
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                if (selectedrowindex < 0)
                    return;
                var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                app_popup.memberEdit(id);
            });
            // delete selected row.
            deleteButton.click(function (event) {
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                if (selectedrowindex < 0)
                    return;
                var rcdid = $('#jqxgrid').jqxGrid('getrowdata', selectedrowindex).RecordId;
                //if (confirm('האם למחוק את המנוי ' + memid)) {
                app_tasks_grid.memberDelete(rcdid);
                //}
                //$("#jqxgrid").jqxGrid('deleterow', id);
            });
            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
            queryButton.click(function (event) {
                app.redirectTo('/Main/TasksQuery');
            });
        };

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
        {
            width: '100%',
            autoheight: true,
            autorowheight: true,
            altrows: true,
            //enabletooltips: true,
            rtl: true,
            source: slf.dataAdapter,
            localization: getLocalization('he'),
            //virtualmode: true,
            rendergridrows: function (obj) {
                return slf.dataAdapter.records;
            },
            columnsresize: true,
            scrollmode: 'logical',
            pageable: false,
            pagermode: 'simple',
            sortable: true,
            rowdetails: true,
            rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li>תוכן</li><li class='title'></li><li>הערות</li><li>העברות</li><li>מד-זמן</li><li>פעולות</li></ul><div class='body'></div><div class='information'></div><div class='comments'></div><div class='history'></div><div class='timers'></div><div class='form'></div></div>", rowdetailsheight: 200 },
            initrowdetails: initrowdetails,
            columns: [
              {
                  text: 'מס', dataField: 'TaskId', filterable: false, width: 100, cellsalign: 'right', align: 'center',
                  cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                      return '<div style="text-align:center">' + value + '<a href="#" onclick="app_tasks_share.taskEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a></div>';
                  }
              },
              {
                  text: '  נושא  ', dataField: 'TaskSubject', cellsalign: 'right', align: 'center', width: subjectWidth,
                  filtertype: "custom",
                  createfilterpanel: function (datafield, filterPanel) {
                      app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                  }
              },
              {
                  text: 'סטאטוס', dataField: 'StatusName', width: 100, cellsalign: 'right', align: 'center', width: 120,
                  filtertype: "custom",
                  createfilterpanel: function (datafield, filterPanel) {
                      app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                  }
              },
              {
                  text: ' מבצע ', dataField: 'DisplayName', cellsalign: 'right', align: 'center', width: 120,
                  filtertype: "custom",
                  createfilterpanel: function (datafield, filterPanel) {
                      app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                  }
              },
               {
                   text: 'נוצר ע"י', dataField: 'AssignByName', cellsalign: 'right', align: 'center', width: 120,
                   filtertype: "custom",
                   createfilterpanel: function (datafield, filterPanel) {
                       app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                   }
               },
              {
                  text: 'נוצר ב', type: 'date', dataField: 'CreatedDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center',
                  filtertype: "custom",
                  createfilterpanel: function (datafield, filterPanel) {
                      app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                  }
              },
            {
                text: 'מועד לביצוע', type: 'date', dataField: 'DueDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center',
                filtertype: "custom",
                createfilterpanel: function (datafield, filterPanel) {
                    app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                }
            },
                {
                    text: 'מועד התחלה', type: 'date', dataField: 'StartedDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', hidden: true,
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                },
                {
                    text: 'מועד סיום', type: 'date', dataField: 'EndedDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', hidden: true,
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                },
                 {
                     text: 'משך', dataField: 'TotalTimeView', cellsalign: 'right', align: 'center', hidden: true,
                     filtertype: "custom",
                     createfilterpanel: function (datafield, filterPanel) {
                         app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                     }
                 },
                {
                    text: 'מועד התחלה משוער', type: 'date', dataField: 'EstimateStartTime', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', hidden: true,
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                }
            ]
            });

        $("#jqxgrid").on("pagechanged", function (event) {
            var args = event.args;
            var pagenum = args.pagenum;
            var pagesize = args.pagesize;

            $.jqx.cookie.cookie("jqxGrid_jqxWidget", pagenum);
        });
        $('#jqxgrid').on('rowdoubleclick', function (event) {
            var args = event.args;
            var boundIndex = args.rowindex;
            var visibleIndex = args.visibleindex;
            var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex);
            return taskEdit(id);
        });

    },
    taskEdit: function (id) {
        app.redirectTo('/System/TaskEdit?id=' + id);
        //wizard.displayStep(2);
        //$.ajax({
        //    type: 'GET',
        //    url: '/System/TaskEdit',
        //    data: { "id": id },
        //    success: function (data) {
        //        $('#divPartial').html(data);
        //    }
        //});
    },
    taskDelete: function (rcdid) {
        if (!confirm('האם למחוק את המשימה ' + rcdid)) {
            return;
        };
        $.ajax({
            type: "POST",
            url: '/System/DeleteTask',
            data: { 'TaskId': rcdid },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                app_dialog.alert(data.Message);
                $('#jqxgrid').jqxGrid('source').dataBind();
            },
            completed: function (data) {
                $('#jqxgrid').jqxGrid('source').dataBind();
            },
            error: function (e) {
                app_dialog.alert(e);
            }
        });
    },

    categoriesRefresh: function () {
        try {
            var i = this.NScustomers.currentIndex;
            var g = this.NScustomers.nastedCategoriesGrids[i];
            g.jqxGrid('source').dataBind();
        }
        catch (e) {
            app_dialog.alert(e);
        }
    },

    commentDelete: function (id, rcdid) {
        //accountNewsRemove(id, memid);
        var slf = this;
        if (confirm("האם להסיר הערה " + rcdid + " ממשימה " + id)) {
            $.ajax({
                type: "POST",
                url: '/System/DeleteTaskComment',
                data: { 'TaskId': rcdid, 'CommentId': id },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    slf.categoriesRefresh();
                    if (data.Status > 0)
                        dialogMessage('הערות', 'משימה ' + rcdid + ' הוסר ממשימה ' + id, true);
                    else
                        app_dialog.alert(data.Message);
                },
                error: function (e) {
                    app_dialog.alert(e);
                }
            });
        }
        this.categoriesRefresh();
    },


    reload: function () {
        //this.source.data['assignMe'] = $("#chkAssignBy").is(':checked');
        //this.source.data['state'] = $('#TaskState').val();
        $('#jqxgrid').jqxGrid('source').dataBind();
    }
};




//============================================================================================ app_reminder_window

var app_reminder_window = function (tagWindow) {

    this.tagDiv = tagWindow;
    this.Option = "a",

        this.init = function () {

            var slf = this;

            var html = (function () {/*
         <div id="accordion" style="margin: 0 auto; display: block; direction: rtl">
			<div id="exp-0" class="panel-area">
				<div id="hxp-0" class="panel-area-title">תזכורת</div>
				<div class="expander-entry" style="margin:10px">
					<form class="fcForm-reminder" id="fcForm-reminder" method="post" action="/System/ReminderUpdate">
						<input type="hidden" id="TaskId" name="TaskId" value="0" />
						<input type="hidden" id="TaskModel" name="TaskModel" value="R" />
						<input type="hidden" id="AccountId" name="AccountId" value="" />
						<input type="hidden" id="UserId" name="UserId" value="" />
						<input type="hidden" id="TeamId" name="TeamId" value="" />
						<input type="hidden" id="Task_Type" name="Task_Type" value="0" />
						<div id="tab-content" class="tab-content" dir="rtl">
							<div class="form-group">
								<div class="field">
									אל:
								</div>
								<div id="AssignTo"></div>
							</div>
							<div class="form-group">
								<div class="field">
									נושא:
								</div>
								<input id="TaskSubject" name="TaskSubject" type="text" style="width:80%;max-width:600px;" />
							</div>
							<div class="form-group">
								<div class="field">
									תיאור:
								</div>
								<textarea id="TaskBody" name="TaskBody"  style="background-color:#fff"></textarea>
							</div>
							 <div class="form-group">
								<div class="field">
									תאריך לביצוע:
								</div>
								<div id="DueDate" name="DueDate"></div>
							</div>
							<div class="form-group pasive">
								<div class="field">
									אופן התזכור:
								</div>
								<select id="RemindPlatform">
									<option selected value="0">מערכת</option>
									<option value="1">מסרון</option>
									<option value="2">דואר אלקטרוני</option>
								</select>
							</div>
							<div class="form-group pasive">
								<div class="field">
									צבע :
								</div>
								<select id="ColorFlag" name="ColorFlag">
									<option value="#46d6db">Turquoise</option>
									<option value="#7bd148">Green</option>
									<option value="#5484ed">Bold blue</option>
									<option value="#a4bdfc">Blue</option>
									<option value="#7ae7bf">Light green</option>
									<option value="#51b749">Bold green</option>
									<option value="#fbd75b">Yellow</option>
									<option value="#ffb878">Orange</option>
									<option value="#ff887c">Red</option>
									<option value="#dc2127">Bold red</option>
									<option value="#dbadff">Purple</option>
								</select>
							</div>
							<div id="TaskStatus-group" class="form-group pasive">
								<div class="field">
									סטאטוס :
								</div>
								<input type="text" id="TaskStatus" name="TaskStatus" class="text-normal" readonly />
							</div>
							<div class="form-group pasive">
								<div class="field">
									נוצר ב:
								</div>
								<input type="text" id="CreatedDate" name="CreatedDate" class="text-mid label" readonly="readonly" data-type="datetime" />
							</div>
							<div id="jqxExp-1" class="panel-area rtl pasive">
								<div class="panel-area-title">
									<a id="a-jqxExp-1" href="#">פרטים נוספים</a>
								</div>
								<div id="jqxExp-box" style="display:none">
										<div id="Task_Parent-group" class="form-group">
											<div class="field">
												משימת אב:
											</div>
											<input type="text" id="Task_Parent" name="Task_Parent" readonly /><a id="Task_Parent-link" href="#" title="הצג משימת אב"><i class="fa fa-search-plus"></i></a>
										</div>
										<div class="form-group">
											<div class="field">
												פרוייקט:
											</div>
											<input type="text" id="Project_Id" name="Project_Id" /><i class="fa fa-search"></i>
										</div>
										<div class="form-group">
											<div class="field">
												לקוח\מנוי:
											</div>
											<input type="text" id="ClientId" name="ClientId" /><i class="fa fa-search"></i>
										</div>
										<div class="form-group">
											<div class="field">
												תגיות:
											</div>
											<div id="Tags" name="Tags"></div>
										</div>
									</div>
							</div>
							<div style="height: 5px"></div>
							<div class="panel-areaB">
								<input id="fcSubmit" class="btn-default btn7 w-60" type="button" value="עדכון" />
								<input id="fcEnd" class="btn-default btn7 w-60" style="display:none" type="button" value="סיום" />
								<input id="fcCancel" class="btn-default btn7 w-60" type="button" value="x" />
							</div>
						</div>
					</form>
				</div>
			</div>
    </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

            if (this.Option == "a") {
                html = html.replace('form-group active', 'form-group pasive')
            }

            var container = $(html);

            $(slf.tagDiv).empty();
            $(slf.tagDiv).append(container);

            this.loadControls();

            $(slf.tagDiv).show();

            return this;
        };

    this.loadControls = function () {

        var slf = this;

        //this.parentSettings(slf.TaskParentId);

        //$("#AccountId").val(slf.AccountId);

        if (theme === undefined)
            theme = 'nis_metro';

        $('#TaskBody').jqxEditor({
            height: '140px',
            //width: '100%',
            editable: true,
            rtl: true,
            tools: 'bold italic underline | color background | left center right',
            theme: theme
            //stylesheets: ['editor.css']
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


        $('#DueDate').jqxDateTimeInput({ showCalendarButton: true, readonly: true, width: '150px', rtl: true });

        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, null, function () {

        });
        app_form.setDateTimeNow('#CreatedDate');

        //$('#reset').on('click', function () {
        //    location.reload();
        //});

        //$('#fcClear').on('click', function (e) {
        //    //app_messenger.Post("הודעה");
        //    $('#fcForm')[0].reset();
        //    $('#fcForm').jqxValidator('hide');
        //});
        //$('#fcNew').on('click', function (e) {
        //    app.refresh();
        //});

        $('#fcCancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#fcSubmit').on('click', function (e) {
            slf.doSubmit('update');
        });


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

        $('#fcForm-reminder').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', this.ClientId, function () {

        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', this.ProjectId, function () {

        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 225, 0, true, this.Tags, function () {

        });

        this.exp1_Inited = true;

        //if (!this.IsEditable) {
        //    $("#ClientId").prop("readonly", true);
        //    $("#Project_Id").prop("readonly", true);
        //    $("#Tags").jqxComboBox({ enableSelection: false });
        //    $("#AssignTo").jqxComboBox({ enableSelection: false });
        //    //$("#ShareType").jqxDropDownList({ enableSelection: false });
        //    //app.disableSelect("#ShareType");
        //}

    }
    this.doSubmit = function (act) {
        //e.preventDefault();
        var actionurl = $('#fcForm-reminder').attr('action');
        var status = app_jqx.getInputAutoValue("#TaskStatus", 1);// $("#TaskStatus").val();
        var isnew = this.IsNew;

        if (!this.exp1_Inited) {
            this.lazyLoad();
        }

        var afterSubmit = function (slf, data) {

            if (slf.TaskId === 0) {
                slf.TaskId = data.OutputId;
                $("#TaskId").val(data.OutputId);
                $("#hxp-0").text('תזכורת: ' + data.OutputId);
                //$(".hxp").show();
            }
            app_messenger.Notify(data, 'info');
            //$('#TaskBody').jqxEditor('destroy');
            
            if (act === 'plus') {
                app.refresh();
            }
            else {//if (act == 'finished') {
                app.redirectTo('/System/TaskUser');
            }
        }

        var RunSubmit = function (slf, status, actionurl) {

            app_jqx.setInputAutoValue("#TaskStatus", status);

            var value = $("#TaskBody").jqxEditor('val');
            var AssignTo = app_jqxcombos.getComboCheckedValues("AssignTo");
            var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }, { key: 'AssignTo', value: AssignTo }];
            var formData = app.serializeEx('#fcForm-reminder input, #fcForm-reminder select, #fcForm-reminder hidden', args);

            app_query.doFormSubmit("#fcForm-reminder", actionurl, formData, function (data) {

                afterSubmit(slf, data);
            });

        };

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
    };

    this.doCancel = function () {
        //$('#TaskBody').jqxEditor('destroy'); 
        $(this.tagDiv).hide();
        $(this.tagDiv).empty();
    }

    //this.show = function () {
    //    $(this.tagDiv).show();
    //};
    //this.hide = function () {
    //    $(slf.tagDiv).hide();
    //};
    //this.doRefresh = function () {
    //    this.appMedia.doRefresh();
    //};
    //this.doRemove = function () {
    //    this.appMedia.doRemove();
    //};
    //this.updatePanel = function (item) {
    //    this.appMedia.updatePanel(item);
    //};
}
