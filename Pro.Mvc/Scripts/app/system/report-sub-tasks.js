
//============================================================================================ app_tasks_grid
(function ($) {

    var task_grid;
    var wizard;

    app_tasks_grid = {

        //accType: 0,
        dataAdapter: {},
        AllowEdit: 0,
        UserId:0,
        IsMobile: false,
        source:
            {

                datatype: "json",
                //async: false,
                datafields: [
                   { name: 'TaskId', type: 'number' },
                   { name: 'TaskParent', type: 'number' },
                   { name: 'SubType', type: 'string' },
                   { name: 'SubId', type: 'number' },
                   { name: 'SubDate', type: 'date' },
                   { name: 'SubText', type: 'string' },
                   { name: 'TaskSubject', type: 'string' },
                   { name: 'SubUserId', type: 'number' },
                   { name: 'SubDisplayName', type: 'string' },
                   { name: 'TaskStatus', type: 'number' },
                   { name: 'DueDate', type: 'date' },
                   { name: 'LastUpdate', type: 'date' },
                   { name: 'StatusName', type: 'string' },
                   { name: 'AssignByName', type: 'string' },
                   { name: 'CreatedDate', type: 'date' },
                   { name: 'RowId', type: 'number' },
                   { name: 'TotalRows', type: 'number' }
                ],
                id: 'RowId',
                type: 'POST',
                url: '/System/GetSubTaskGrid',
                data: { 'AccountId': 0, 'UserId': 0, 'assignMe': false, 'state': 0 },
                //hierarchy:
                //{
                //    //keyDataField: { name: 'TaskId' },
                //    //parentDataField: { name: 'TaskId' },
                //    groupingDataFields:[
                //                //{ name: "TaskId" },
                //                { name: "TaskSubject" }
                //    ]
                //},
               
                pagenum: 0,
                pagesize: 20
                //root: 'Rows',
                //pager: function (pagenum, pagesize, oldpagenum) {
                //    console.log(pagenum);
                //    // callback called when a page or page size is changed.
                //},
                //filter: function () {
                //    // update the grid and send a request to the server.
                //    $("#jqxgrid").jqxGrid('updatebounddata');
                //},
                //sort: function () {
                //    // update the grid and send a request to the server.
                //    $("#jqxgrid").jqxGrid('updatebounddata');
                //},
                //beforeprocessing: function (data) {
                //    this.totalrecords = data.TotalRows;
                //}
            },
        edit: function (id) {
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
  
        grid: function () {
            var slf = this;
            var subjectWidth = slf.IsMobile ? 250 : 400;
           
            // create Tree Grid
            $("#jqxgrid").jqxGrid(
            {
                width: '100%',
                //autoRowHeight: false,
                autoheight: true,
                enabletooltips: true,
                rtl: true,
                source: slf.dataAdapter,
                localization: getLocalization('he'),
                //virtualmode: true,
                rendergridrows: function (obj) {
                    return slf.dataAdapter.records;
                },
                //columnsresize: true,
                pageable: true,
                //pagermode: 'simple',
                sortable: true,
                groupable: true,
                //showfilterrow: true,
                //filterable: true,
                //rowdetails: true,
                //rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li>תוכן</li><li class='title'></li><li>הערות</li><li>העברות</li><li>מד-זמן</li><li>פעולות</li></ul><div class='body'></div><div class='information'></div><div class='comments'></div><div class='history'></div><div class='timers'></div><div class='form'></div></div>", rowdetailsheight: 200 },
                //rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li class='title'>פרטים</li></ul><div class='information'></div></div>", rowdetailsheight: 200 },
                //initrowdetails: initrowdetails,
                //showstatusbar: true,
                //renderstatusbar: renderstatusbar,
                //showtoolbar: false,
                //rendertoolbar: function (toolbar) {
                //    app_jqxgrid.gridFilterRtl(this, toolbar, columnList, dateList);
                //},
                columns: [
                  //{ text: 'רשומה', dataField: 'RowId', filterable: false, width: 80, cellsalign: 'right', align: 'center' },
                  //{text: 'משימת אב', dataField: 'TaskParent', filterable: false, width: 100, cellsalign: 'right', align: 'center'},
                  {
                      text: 'מס-משימה', dataField: 'TaskId', filterable: false, width: 120, cellsalign: 'right', align: 'center',
                    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                        var editlink = '';
                        //var asb = $('#jqxgrid').jqxGrid('getrowdata', row).AssignBy;
                        //if (slf.UserId == asb)
                        //    editlink = '<label> </label><a href="#" onclick="app_tasks_grid.taskEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';

                        //return '<div style="text-align:center">' + value + '<a href="#" onclick="app_tasks_grid.taskInfo(' + value + ')" ><label> </label><i class="fa fa-info-circle"></i></a>' + editlink + '</div>';

                        return '<div style="text-align:right;margin-top:6px;margin-right:10px"><a href="#" onclick="app_tasks_grid.taskInfo(' + value + ')" ><label> ' + value + ' </label><i class="fa fa-info-circle"></i></a></div>';
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
                      text: ' סוג   ', dataField: 'SubType', cellsalign: 'right', align: 'center', width: 100,
                      filtertype: "custom",
                      createfilterpanel: function (datafield, filterPanel) {
                          app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                      }
                  },
                  {
                      text: '  תאור פעולה  ', dataField: 'SubText', cellsalign: 'right', align: 'center', width: subjectWidth,
                      filtertype: "custom",
                      createfilterpanel: function (datafield, filterPanel) {
                          app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                      }
                  },
                {
                    text: 'מועד ביצוע', type: 'date', dataField: 'SubDate', cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                },
                  {
                      text: 'סטאטוס', dataField: 'StatusName', cellsalign: 'right', align: 'center', width: 120, cellsrenderer:
                          function (row, columnfield, value, defaulthtml, columnproperties) {
                              var color = app_tasks.statusColor(value);
                              return '<div style="text-align:right;margin-right:10px"><label> ' + value + ' </label><i class="fa fa-circle" style="font-size:16px;color:' + color + '"></i></div>';
                      },
                      //cellclassname: cellclass,
                      filtertype: "custom",
                      createfilterpanel: function (datafield, filterPanel) {
                          app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                      }
                  },
                  {
                      text: ' מבצע ', dataField: 'SubDisplayName', cellsalign: 'right', align: 'center', width: 120,
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
                      text: 'נוצר ב', type: 'date', dataField: 'CreatedDate',  cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                      filtertype: "custom",
                      createfilterpanel: function (datafield, filterPanel) {
                          app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                      }
                  },
                {
                    text: 'מועד לביצוע', type: 'date', dataField: 'DueDate', cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                },
                {
                    text: 'עודכן ב', type: 'date', dataField: 'LastUpdate',  cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                }
                ],
                groups: ['TaskSubject']
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
                return slf.taskEdit(id);
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
        taskInfo: function (id) {
            app.redirectTo('/System/TaskInfo?id=' + id);
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
                    //if (data.Status > 0)
                    //    dialogMessage('מנויים', 'מנוי ' + memid + ' הוסר מרשימת המנויים ', true);
                    //else
                    //   app_dialog.alert(data.Message);
                },
                completed: function (data) {
                    $('#jqxgrid').jqxGrid('source').dataBind();
                },
                error: function (e) {
                    app_dialog.alert(e);
                }
            });
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

        load: function (Model, userInfo) {
            this.IsMobile = app.IsMobile();
            this.UserId = userInfo.UserId;
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            //this.source.data = {
            //    //'QueryType': Model.QueryType,
            //    'AccountId': Model.AccountId,
            //    'UserId': Model.UserId
            //};
            this.source.data['AccountId']=Model.AccountId;
            this.source.data['UserId'] = Model.UserId;

            this.dataAdapter = new $.jqx.dataAdapter(this.source, {
                loadComplete: function (data) {
                    //source.totalrecords = getTotalRows(data);
                },
                loadError: function (xhr, status, error) {
                    app_dialog.alert(' status: ' + status + '\n error ' + error)
                }
            });

            this.grid();

            return this;
        },
        reload: function () {
            this.source.data['assignMe'] = $("#chkAssignBy").is(':checked');
            this.source.data['state'] = $('#TaskState').val();
            $('#jqxgrid').jqxGrid('source').dataBind();
        }
    };

})(jQuery)


    function taskEnd(refresh) {
        wizard.displayStep(1);
        $('#divPartial').html('');
        if (refresh)
            $('#jqxgrid').jqxGrid('source').dataBind();
    }

    function triggerCancelEdit() {
        task_grid.end(false);
    };

    function triggerTaskCompleted(id) {
        $("#jqxgrid").jqxGrid('source').dataBind();
        $("#divPartial").html('');
        //app_dialog.dialogIframClose();
    };
