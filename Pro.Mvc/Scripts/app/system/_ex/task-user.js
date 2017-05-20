

//============================================================ app_tasks_user
(function ($) {

app_tasks_user = {


    grid: function () {
        var slf = this;

        //var fields = [
        //{ name: "id", type: "string" },
        //{ name: "status", map: "state", type: "string" },
        //{ name: "text", map: "label", type: "string" },
        //{ name: "tags", type: "string" },
        //{ name: "color", map: "hex", type: "string" },
        //{ name: "resourceId", type: "number" }
        //];

        //var source =
        //{
        //    localData: [
        //            { id: "1161", state: "new", label: "Combine Orders", tags: "orders, combine", hex: "#5dc3f0", resourceId: 3 },
        //            { id: "1645", state: "work", label: "Change Billing Address", tags: "billing", hex: "#f19b60", resourceId: 1 },
        //            { id: "9213", state: "new", label: "One item added to the cart", tags: "cart", hex: "#5dc3f0", resourceId: 3 },
        //            { id: "6546", state: "done", label: "Edit Item Price", tags: "price, edit", hex: "#5dc3f0", resourceId: 4 },
        //            { id: "9034", state: "new", label: "Login 404 issue", tags: "issue, login", hex: "#6bbd49" }
        //    ],
        //    dataType: "array",
        //    dataFields: fields
        //};

       
        var source = {
            datafields: [
                  { name: 'TaskId', type: 'number' },
                  { name: 'TaskSubject', type: 'date' },
                  { name: 'TaskBody', type: 'string' },
                  { name: 'CreatedDate', type: 'date' },
                  { name: 'DueDate', type: 'date' },
                  { name: 'TaskHex', type: 'string' },
                  { name: 'StatusNameLocal', type: 'string' },
                  { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'TaskId',
            type: 'POST',
            url: '/System/GetTaskUser'
            //data: { 'id': taskid }
        }

        var dataAdapter = new $.jqx.dataAdapter(source);
                

        $('#kanban').jqxKanban({
            width: 600,
            height: 500,
            //resources: resourcesAdapterFunc(),
            source: dataAdapter,
            columns: [
                { text: "Backlog", dataField: "new", maxItems: 5 },
                { text: "In Progress", dataField: "work", maxItems: 5 },
                { text: "Done", dataField: "done", maxItems: 5, collapseDirection: "right" }
            ]
        });
        $('#jqxKanban').on('itemAttrClicked', function (event) {
            var args = event.args;
            var itemId = args.itemId;
            var attribute = args.attribute; // template, colorStatus, content, keyword, text, avatar
            slf.edit(itemId);
        });

        $('#kanban').on('itemMoved', function (event) {
            var args = event.args;
            var itemId = args.itemId;
            var oldParentId = args.oldParentId;
            var newParentId = args.newParentId;
            var itemData = args.itemData;
            var oldColumn = args.oldColumn;
            var newColumn = args.newColumn;
        });

        //$('#jqxgrid1').on('rowdoubleclick', function (event) {
        //    slf.edit();
        //});
        //$('#jqxgrid1-add').click(function () {
        //    slf.add();
        //});

        //$('#jqxgrid1-edit').click(function () {
        //    slf.edit();
        //});

        //$('#jqxgrid1-remove').click(function () {
        //    slf.remove();
        //});
        //$('#jqxgrid1-refresh').click(function () {
        //    slf.refresh();
        //});

        //$('#task-item-update').click(function () {
        //    var iframe = wizard.getIframe();
        //    if (iframe && iframe.triggerSubmit) {
        //        iframe.triggerSubmit();
        //    }
        //});
        //$('#task-item-cancel').click(function () {
        //    wizard.wizHome();
        //});

    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    load: function (userInfo) {
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.grid();
        return this;
    },
    add: function () {
        wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentAdd?id=" + this.TaskId, "100%", "500px");
    },
    edit: function () {
        var id = this.getrowId();
        if (id > 0)
            wizard.appendIframe(2, app.appPath() + "/System/_TaskCommentEdit?id=" + id, "100%", "500px");
    }
    //remove: function () {
    //    var id = this.getrowId();
    //    if (id > 0) {
    //        if (confirm('האם למחוק הערה ' + id)) {
    //            app_query.doPost(app.appPath() + "/System/TaskCommentDelete", { 'id': id });
    //            $('#jqxgrid1').jqxGrid('source').dataBind();
    //        }
    //    }
    //},
    //refresh: function () {
    //    $('#jqxgrid1').jqxGrid('source').dataBind();
    //},
    //cancel: function () {
    //    wizard.wizHome();
    //},
    //end: function (data) {
    //    wizard.wizHome();
    //    //wizard.removeIframe(2);
    //    if (data && data.Status > 0) {
    //        this.refresh();
    //        app_jqxnotify.Info(data, true);//notificationInfo(data.Message, true);
    //        // app_dialog.alert(data.Message);
    //    }
    //},
 };

})(jQuery)