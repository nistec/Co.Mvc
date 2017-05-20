

//============================================================ app_tasks_comment
(function ($) {

app_tasks_action = {

    wizardStep: 2,
    TaskId: 0,
    taskDialog: {},
    grid: function (taskid) {
        var slf = this;

        var nastedsource = {
            //datafields: [
            //      { name: 'ActionId', type: 'number' },
            //      { name: 'CommentDate', type: 'date' },
            //      { name: 'CommentText', type: 'string' },
            //      { name: 'ReminderDate', type: 'date' },
            //      { name: 'Attachment', type: 'string' },
            //      { name: 'DisplayName', type: 'string' },
            //      { name: 'Task_Id', type: 'number' },
            //      { name: 'UserId', type: 'number' }
            //],
            datatype: "json",
            id: 'ActionId',
            type: 'POST',
            url: '/System/GetTasksActionGrid',
            data: { 'id': taskid }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        $("#jqxgrid1").jqxGrid({
            width: '100%',
            autoheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
              { text: 'מועד רישום', datafield: 'CommentDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
              { text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center' },
              { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' },
              {
                  text: '...', datafield: 'CommentId', width: 120, cellsalign: 'right', align: 'center',
                  cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                      return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
                  }
              }
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

        $('#task-item-update').click(function () {
            var iframe = wizard.getIframe();
            if (iframe && iframe.triggerSubmit) {
                iframe.triggerSubmit();
            }
        });
        $('#task-item-cancel').click(function () {
            wizard.wizHome();
        });

    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    load: function (id, userInfo) {
        this.TaskId = id;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid(id);
        return this;
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
        var id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק הערה ' + id)) {
                app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id });
                $('#jqxgrid1').jqxGrid('source').dataBind();
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
        if (data && data.Status > 0) {
            this.refresh();
            app_jqxnotify.Info(data, true);//notificationInfo(data.Message, true);
            // app_dialog.alert(data.Message);
        }
    },
    //edit: function (id) {
    //    var slf = this;
    //    wizard.displayStep(slf.wizardStep);
    //    $.ajax({
    //        type: 'GET',
    //        url: '/System/_TaskComment',
    //        data: { "id": id },
    //        success: function (data) {
    //            $('#divPartial' + slf.wizardStep).html(data);
    //        }
    //    });
    //},
    //end: function (refresh) {
    //    wizard.displayStep(1);
    //    $('#divPartial' + this.wizardStep).html('');
    //    if (refresh)
    //        $('#jqxgrid1').jqxGrid('source').dataBind();
    //}


};

})(jQuery)