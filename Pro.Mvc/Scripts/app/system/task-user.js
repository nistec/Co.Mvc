
function tasks_active_toggleDiv(id) {
    $("#toHide" + id).toggle();
}
function tasks_active_add_comment1(id) {
    //$(".comment-add").empty();
    var state = $("#toComment" + id).toggle();

    var isVisible = $(state).is(":visible");
    if (isVisible) {
        $("#reminder-days" + id).focus();
        $("#comment-text" + id).focus();
    }
    //var isVisible = $("#toComment" + id).is(":visible");
    //var isHidden = $("#toComment" + id).is(":hidden");
}

function tasks_active_update_comment(id, model) {

    var comment = $("#comment-text" + id).val();
    if (comment.length == 0) {
        return app_messenger.Post("נא לציין הערה", 'error', true);
    }

    switch (model) {
        case 'T':
            app_query.doPost('/System/TaskCommentAddQs', { 'Task_Id': id, 'CommentText': comment }, tasks_active_update_confirm, id);
            break;
        case 'E':
            app_query.doPost('/System/TicketCommentAddQs', { 'Task_Id': id, 'CommentText': comment }, tasks_active_update_confirm, id);
            break;
        case 'R':
            app_query.doPost('/System/ReminderCommentAddQs', { 'Task_Id': id, 'CommentText': comment }, tasks_active_update_confirm, id);
            break;
        case 'C':
            break;
    }
}


function tasks_active_update_confirm(data, id) {
    //app_jqxnotify.Info(data, true);
    //dialog: function (data, template, showClose, callback, args) {

    app_messenger.Post(data);
    //Messenger().post("Your request has succeded!");
    $("#toComment" + id).hide();
    $("#comment-text" + id).val('');
}

function tasks_getStatus(arg_state) {
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
}
function tasks_active_open_task(status, id,model) {

    switch (model) {
        case 'T':
            if (status > 1 && status < 8)
                app.redirectTo('/System/TaskEdit?id=' + id);
            else if (status <= 1) {
                app_dialog.confirm("האם לאתחל משימה?", function () {
                    app.redirectTo('/System/TaskStart?id=' + id);
                });

                //if (confirm("האם לאתחל משימה?"))
                //    app.redirectTo('/System/TaskStart?id=' + id);
            }
            else if (status >= 8)
                app.redirectTo('/System/TaskInfo?id=' + id);
            break;
        case 'E':
            if (status >= 8)
                app.redirectTo('/System/TicketInfo?id=' + id);
            else
                app.redirectTo('/System/TicketEdit?id=' + id);
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
}

//function tasks_active_edit_task(id) {
//    app.redirectTo('/System/TaskEdit?id=' + id);
//}
//function tasks_active_view_task(id) {
//    app.redirectTo('/System/TaskInfo?id=' + id);
//}
//function tasks_active_start_task(id) {
//    app.redirectTo('/System/TaskStart?id=' + id);
//}
function getStatusFilter() {
    var val = $("#StatusFilter").val();
    return val;
}
function getStatusFilterName(status) {
    switch(status)
    {
        case 1:
            return 'משימות ממתינות';
        case 2:
            return 'משימות פעילות';
        case 16:
            return 'משימות סגורות';
        case 255:
            return 'משימות להיום';
    }
    return 'משימות'
}

function getTaskModelName(model) {
    switch(model)
    {
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
}



function tasks_active_add_comment(id) {
    
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
}
//function triggerCommentCompleted(data) {
//    //app_tasks_comment.end(data);
//    app_messenger.Post(data);
//    if (data.Status > 0)
//        wizard.wizHome();
//}
function _createWindow() {
    var jqxWidget = $('#jqxWidget');
    var offset = jqxWidget.offset();
    $('#window').jqxWindow({
        //rtl: true,
        autoOpen: false, 
        showCloseButton: true,
        closeButtonSize: 20,
        position: 'center',//{ x: offset.left + 50, y: offset.top + 50 },
        //showCollapseButton: true,
        maxHeight: 400, maxWidth: 400, minHeight: 200, minWidth: 200, height: 300, width: 400,
        initContent: function () {
            //$('#tab').jqxTabs({ height: '100%', width: '100%' });
            $('#window').jqxWindow('focus');
        }
    });
};

//============================================================ app_tasks_active
(function ($) {

app_tasks_active = {

    TaskStatus: 0,

    grid: function (status) {
        var slf = this;
        this.TaskStatus = status;

        var source = {
              datafields : [
                     { name: "id", map: "TaskId", type: "number" },
                     { name: "status", map: "TaskState", type: "string" },
                     { name: "text", map: "TaskSubject", type: "string" },
                     { name: "tags", map: "TaskTypeName", type: "string" },
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
        //var dataAdapter = new $.jqx.dataAdapter(source);

        //var allRowsSelected = function () {
        //    var selection = $("#dataTable").jqxDataTable('getSelection');
        //    var rows = $("#dataTable").jqxDataTable('getRows');
        //    if (selection.length == 0) {
        //        return false;
        //    }
        //    if (rows.length != selection.length) {
        //        return null;
        //    }
        //    return true;
        //}

        var updatingSelection = false;
        var updatingSelectionFromDataTable = false;


        //function expandersActions(eventType, eventTarget, expanderNumber) {
        //    var oldHeight, newHeight, heightDifference, currentSplitterHeight;
        //    if (eventType === 'expanding') {
        //        oldHeight = eventTarget.clientHeight;
        //        $('.toHide' + expanderNumber).css('display', 'none');
        //        $('#expander' + expanderNumber).jqxExpander('refresh');
        //        newHeight = eventTarget.clientHeight + $('#expander' + expanderNumber + ' div.jqx-widget-content').height();
        //        heightDifference = newHeight - oldHeight;
        //        currentSplitterHeight = $('#mainSplitter').jqxSplitter('height');
        //        $('#mainSplitter').jqxSplitter({ height: currentSplitterHeight + heightDifference + 7 });
        //    } else {
        //        oldHeight = eventTarget.clientHeight;
        //        $('.toHide' + expanderNumber).css('display', 'block');
        //        $('#expander' + expanderNumber).jqxExpander('refresh');
        //        newHeight = eventTarget.clientHeight - $('#expander' + expanderNumber + ' div.jqx-widget-content').height();
        //        heightDifference = oldHeight - newHeight;
        //        currentSplitterHeight = $('#mainSplitter').jqxSplitter('height');
        //        $('#mainSplitter').jqxSplitter({ height: currentSplitterHeight - heightDifference - 7 });
        //    }
        //}

        //$('.expander').on('expanding', function (e) {
        //    expandersActions('expanding', e.target, 1);
        //});

        var resourcesAdapterFunc = function () {
            var resourcesSource =
            {
                localData: [
                      { id: '', name: "לא משויך", image: '../../Images/icons/settings-24.png', common: true },
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
                     { name: "common", type: "boolean" }
                ]
            };
            var resourcesDataAdapter = new $.jqx.dataAdapter(resourcesSource);
            return resourcesDataAdapter;
        }
 
        $('#kanban').jqxKanban({rtl:true,
            resources: resourcesAdapterFunc(),
            source: dataAdapter,
            width: '100%',
            height: '100%',
            columns: [
            { text: "משימות סגורות", dataField: "done" },
            { text: "משימות בטיפול", dataField: "work" },
            { text: "משימות ממתינות", dataField: "new" },
            { text: "משימות להיום", dataField: "today" }
            ]
        });

        $('#kanban').on('itemAttrClicked', function (event) {
            var args = event.args;
            var itemId = args.itemId;
            var attribute = args.attribute; // template, colorStatus, content, keyword, text, avatar
            var status = tasks_getStatus(args.item.status);
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
            tasks_active_open_task(status, itemId, resourceId)

        });
        $('#kanban').on('itemMoved', function (event) {
            var args = event.args;
            var itemId = args.itemId;
            var oldParentId = args.oldParentId;
            var newParentId = args.newParentId;
            var itemData = args.itemData;
            var oldColumn = args.oldColumn;
            var newColumn = args.newColumn;
            var newstate=tasks_getStatus(newColumn.dataField);
            var oldstate = tasks_getStatus(oldColumn.dataField);
            app_query.doDataPost('/System/TaskChangeState', {itemid:itemId,newstate:newstate,oldtate:oldstate}, function(data){
            
                if(data.Status < 0)
                {
                    //var newContent = { text: "Cookies", content: "Content", tags: "cookies", color: "lightgreen", resourceId: 1, className: "standard" };
                    itemData.status = oldColumn;
                    $('#jqxKanban').jqxKanban('updateItem', itemId, itemData);
                }

            });
        });

        function updateItem()
        {
            var itemId = '12354';
            var newContent = { text: "Cookies", content: "Content", tags: "cookies", color: "lightgreen", resourceId: 1, className: "standard" };
            $('#jqxKanban').jqxKanban('updateItem', itemId, newContent);
        }

        

/*
        $("#dataTable").jqxDataTable(
        {
            width: '100%',
            rtl: true,
            //editable:true ,
            pageable: true,
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
            //showStatusbar:false,
            //showToolbar: false,
            //renderToolbar: function (toolbar) {
            //    var container = $("<div style='margin: 5px;'></div>");
            //    var span = $("<span style='float: left; margin-top: 5px; margin-right: 4px;'>איתור:</span>");
            //    var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 23px; float: left; width: 223px;' />");
            //    toolbar.append(container);
            //    container.append(input);
            //    container.append(span);


            //    var group=$(
            //    '<div id="Status-group" class="switch-group" style="float: right; margin-top: 5px;">' +
            //        '<div class="switch-item" id="Status-Open" data-val="1">בהמתנה</div>'+
            //        '<div class="switch-item" id="Status-Started" data-val="2">בעבודה</div>'+
            //        '<div class="switch-item" id="Status-Close" data-val="16">שנסגרו</div>'+
            //    '</div>').jqxButtonGroup({ mode: 'radio', rtl: true })
            //    .on('buttonclick', function (event) {
            //        var val = $(event.args.button).data('val');
            //        app_tasks_active.reload(val);
            //    });


            //    container.append(group);


            //},
            columns: [
                  {
                      text: '...', align: 'center', dataField: 'TaskHex', cellsalign: 'center', width: 40, filterable: false,
                      cellsRenderer: function (row, column, value, rowData) {
                          //var value = $("#dataTable").jqxDataTable('getCellValue', 0, 'firstName');
                          // var status = slf.TaskStatus;

                          //var method = '';
                          //if (status <= 1)

                          //    function tasks_active_edit_task(id) {
                          //        app.redirectTo('/System/TaskEdit?id=' + id);
                          //    }
                          //function tasks_active_view_task(id) {
                          //    app.redirectTo('/System/TaskInfo?id=' + id);
                          //}
                          //function tasks_active_start_task(id) {
                          //    app.redirectTo('/System/TaskStart?id=' + id);
                          //}
                          //var taskid = "'" + rowData.TaskId + "'";
                          
                          //var part1 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a href="#" title="הצג משימה" onclick="tasks_active_open_task(' + slf.TaskStatus + ',' + rowData.TaskId + ')">...</a></div>' +
                          //    '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a href="#" title="הצג הכל" onclick="tasks_active_toggleDiv(' + rowData.TaskId + ')">{}</a></div>';
                       
                          var part1 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a href="#" title="הצג הכל" onclick="tasks_active_toggleDiv(' + rowData.TaskId + ')"><i class="fa fa-plus-square-o"></i></a></div>';

                          var part2 = '';

                          if (rowData.TaskModel!='T' ||  (rowData.TaskStatus > 1 && rowData.TaskStatus < 8))
                              var part2 = '<div style="text-align:center;margin: 5px;border: 1px solid #33842a;background-color:' + value + '"><a class="popoverBtn" href="#" title="הוספת הערה" onclick="tasks_active_add_comment(' + rowData.TaskId + ',' + "'"+rowData.TaskModel+"'" + ')"><i class="fa fa-comment-o"></i></a></div>';

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
                          
                       var tsk_modelname = getTaskModelName(rowData.TaskModel);

                        var expColumn=
                        '<div class="expanderContainer rtl">'+
                            '<div class="icon icon-news"></div>'+
                            '<div class="expander">'+
                                '<div><div style="color:' + (rowData.ColorFlag || '#000') + '">' + tsk_modelname + ':<a href="#" title="' + tsk_modelname + '" onclick="tasks_active_open_task(' + rowData.TaskStatus + ',' + rowData.TaskId + ',' + "'" + rowData.TaskModel + "'" + ')">' + rowData.TaskSubject + '</a></div>' +
                                '<div class="rtl"><spn> נוצר ע"י: </span>' + rowData.DisplayName + '<span class="glyphicon glyphicon-calendar glyphicon-custom1"></span><span class="glyphicon glyphicon-time glyphicon-custom2"> </span><spn> ב: </span><span title="נוצר בתאריך">' + rowData.CreatedDate.toLocaleDateString() + '</span><spn> לביצוע: </span><span title="מועד לביצוע">' + app.toLocalDateString(rowData.DueDate) + '</span></div>' +
                                '<div id="toHide' + rowData.TaskId + '" style="display:none" class="toHide1 toHide">' +
                                        '<div><b>מועד לביצוע:</b>' + app.toLocalDateString(rowData.DueDate) + '</div>' +
                                        '<div><b>מועד התחלה:</b>' + app.toLocalDateString(rowData.StartedDate) + '</div>' +
                                        '<div><b>מועד סיום:</b>' + app.toLocalDateString(rowData.EndedDate) + '</div>' +
                                        '<div><b>נוצר ע"י:</b>' + (rowData.AssignByName || '') + '</div>' +
                                '</div>' +
                                    '<div id="toComment' + rowData.TaskId + '" style="display:none;">' +
                                        '<textarea tabindex="1" id="comment-text' + rowData.TaskId + '" class="comment-add" style="width:99%;height:40px"></textarea>' +
                                        '<div><span>תזכורת בעוד: </span><input tabindex="2" type="number" value="0" style="width:40px" id="reminder-days' + rowData.TaskId + '" /><span>ימים</span>' +
                                        '<button tabindex="3" onclick="tasks_active_update_comment(' + rowData.TaskId + ')">אישור</button></div>' +
                                    '</div>'+
                                '</div>'+
                                '<div><div class="newsTextContainer">' + app.htmlText(app.htmlUnescape(rowData.TaskBody))  + '</div></div>' +
                            '</div>'+
                        '</div>';

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
        //$("#dataTable").on('rowSelect', function (event) {
        //    updatingSelectionFromDataTable = true;
        //    if (!updatingSelection && $("#checkbox").length > 0) {
        //        $("#checkbox").jqxCheckBox({ checked: allRowsSelected() });
        //    }
        //    updatingSelectionFromDataTable = false;
        //});

        //$("#dataTable").on('rowUnselect', function (event) {
        //    updatingSelectionFromDataTable = true;
        //    if (!updatingSelection && $("#checkbox").length > 0) {
        //        $("#checkbox").jqxCheckBox({ checked: allRowsSelected() });
        //    }
        //    updatingSelectionFromDataTable = false;
        //});
 
    },
    getrowId: function () {

        //var selectedrowindex = $("#dataTable").jqxDataTable('getselectedrowindex');
        //if (selectedrowindex < 0)
        //    return -1;
        //var id = $("#dataTable").jqxDataTable('getrowid', selectedrowindex);
        //return id;
        return 0;
    },
    load: function (userInfo) {
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(255);
        //$('#hTitle').text(getStatusFilterName(255));
        return this;
    },
    reload: function (status) {
        //var src = $('#dataTable').jqxDataTable('source');
        //src._source.data = { 'Status': status };
        //$('#dataTable').jqxDataTable('source').dataBind();
        //$('#hTitle').text(getStatusFilterName(status));
    },
    add: function () {
        wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentAdd?id=" + this.TaskId, "100%", "500px");
    },
    edit: function () {
        var id = this.getrowId();
        if (id > 0)
            wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "500px");
    },
    remove: function () {
        //var id = this.getrowId();
        //if (id > 0) {
        //    if (confirm('האם למחוק הערה ' + id)) {
        //        app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id });
        //        $('#dataTable').jqxDataTable('source').dataBind();
        //    }
        //}
    },
    refresh: function () {
        //$('#dataTable').jqxDataTable('source').dataBind();
    },
    cancel: function () {
        //wizard.wizHome();
    },
    end: function (data) {
        //wizard.wizHome();
        ////wizard.removeIframe(2);
        //if (data && data.Status > 0) {
        //    this.refresh();
        //    app_jqxnotify.Info(data, true);//notificationInfo(data.Message, true);
        //    // app_dialog.alert(data.Message);
        //}
    }

};

})(jQuery)