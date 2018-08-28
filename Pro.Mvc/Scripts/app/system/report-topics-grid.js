
//============================================================================================ app_tasks_grid
(function ($) {

    var task_grid;
    var wizard;

    app_tasks_grid = {

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
                   { name: 'TaskSubject', type: 'string' },
                    { name: 'TaskBody', type: 'string' },
                    { name: 'TaskModel', type: 'string' },
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
                   {name: 'UserId', type: 'number' },
                   { name: 'AssignBy', type: 'number' },
                   { name: 'DisplayName', type: 'string' },
                   { name: 'AssignByName', type: 'string' },
                   { name: 'TaskStatus', type: 'number' },
                   { name: 'StatusName', type: 'string' },
                   { name: 'TaskTypeName', type: 'string' },
                   { name: 'ProjectName', type: 'string' },
                   { name: 'IsShare', type: 'boolean' },
                   { name: 'TotalTimeView', type: 'string' },
                   { name: 'TotalRows', type: 'number' }
                ],
                id: 'TaskId',
                type: 'POST',
                url: '/System/GetTopicGrid',
                data: { 'AccountId': 0, 'UserId': 0, 'assignMe': false, 'state': 0 },
                hierarchy:


                {
                    keyDataField: { name: 'TaskId' },
                    parentDataField: { name: 'Task_Parent' }
                },
                pagenum: 0,
                pagesize: 20
                ////root: 'Rows',
                //pager: function (pagenum, pagesize, oldpagenum) {
                //    console.log(pagenum);
                //    // callback called when a page or page size is changed.
                //}
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
                $('#jqxgrid').jqxTreeGrid('source').dataBind();
        },

        getTotalRows: function (data) {
            if (data) {
                return dataTotalRows(data);
            }
            return 0;
        },
  
        grid: function () {
            var slf = this;
            var subjectWidth = slf.IsMobile ? 250 : 800;
           
            // create Tree Grid
            $("#jqxgrid").jqxTreeGrid(
            {
                width: '100%',
                autoRowHeight: false,
                //autoheight: true,
                //enabletooltips: true,
                rtl: true,
                source: slf.dataAdapter,
                localization: getLocalization('he'),
                //virtualmode: true,
                //rendergridrows: function (obj) {
                //    return slf.dataAdapter.records;
                //},
                //columnsresize: true,
                pageable: true,
                //pagermode: 'simple',
                sortable: true,
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
                  {
                      text: 'מס', dataField: 'TaskId', filterable: false, width: 120, cellsalign: 'right', align: 'center',
                        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                            

                            //var model = $("#jqxgrid").jqxTreeGrid('getCellValue', 0, 'TaskModel');
                            //if (model=='P')
                            //    return '<a href="#" onclick="app_tasks_grid.taskInfo(' + value + ')" ><label> ' + value + ' </label><i class="fa fa-info-circle"></i></a>';


                            //var model = "";//$('#jqxgrid').jqxTreeGrid('getrowdata', row).TaskModel;
                            var editlink = '';
                            //var asb = $('#jqxgrid').jqxTreeGrid('getrowdata', row).AssignBy;
                            //if (slf.UserId == asb)
                            //    editlink = '<label> </label><a href="#" onclick="app_tasks_grid.taskEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';

                            //return '<div style="text-align:center">' + value + '<a href="#" onclick="app_tasks_grid.taskInfo(' + value + ')" ><label> </label><i class="fa fa-info-circle"></i></a>' + editlink + '</div>';
                            
                            return editlink + ' <a href="#" onclick="app_tasks_grid.taskInfo(' + value + ')" ><label> ' + value + ' </label><i class="fa fa-info-circle"></i></a>';
                        }
                  },
                  {
                      text: '  נושא  ', dataField: 'TaskSubject', cellsalign: 'right', align: 'center', width: subjectWidth,
                      filtertype: "custom",
                      createfilterpanel: function (datafield, filterPanel) {
                          app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                      }
                  },
                  //{
                  //    text: ' סוג   ', dataField: 'TaskTypeName', cellsalign: 'right', align: 'center',
                  //    filtertype: "custom",
                  //    createfilterpanel: function (datafield, filterPanel) {
                  //        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                  //    }
                  //},
                  //{
                  //    text: ' פרוייקט ', dataField: 'ProjectName', cellsalign: 'right', align: 'center',
                  //    filtertype: "custom",
                  //    createfilterpanel: function (datafield, filterPanel) {
                  //        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                  //    }
                  //},
                  {
                      text: 'סטאטוס', dataField: 'StatusName', width: 120, cellsalign: 'right', align: 'center', cellsrenderer:
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
                    text: 'עודכן ב', type: 'date', dataField: 'LastUpdate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', 
                    filtertype: "custom",
                    createfilterpanel: function (datafield, filterPanel) {
                        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    }
                },
                    {
                        text: 'מועד התחלה', type: 'date', dataField: 'StartedDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'מועד סיום', type: 'date', dataField: 'EndedDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', 
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                     {
                         text: 'משך', dataField: 'TotalTimeView', cellsalign: 'right', align: 'center',width: 100,
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
                  //{ text: 'משותף?', datafield: 'IsShare', threestatecheckbox: true, columntype: 'checkbox', width: 70 }
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
                var id = $("#jqxgrid").jqxTreeGrid('getrowid', boundIndex);
                return slf.taskEdit(id);
            });

        },
        taskEdit: function (id) {
            var model = app_jqx.findRecordValueByField(this.dataAdapter, id, "TaskId", "TaskModel");
            app_tasks.taskInfo(id, model);
            //app.redirectTo('/System/TaskEdit?id=' + id);
        },
        taskInfo: function (id) {
            var model = app_jqx.findRecordValueByField(this.dataAdapter, id, "TaskId", "TaskModel");
            app_tasks.taskInfo(id, model);
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
                    $('#jqxgrid').jqxTreeGrid('source').dataBind();
                    //if (data.Status > 0)
                    //    dialogMessage('מנויים', 'מנוי ' + memid + ' הוסר מרשימת המנויים ', true);
                    //else
                    //   app_dialog.alert(data.Message);
                },
                completed: function (data) {
                    $('#jqxgrid').jqxTreeGrid('source').dataBind();
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
            $('#jqxgrid').jqxTreeGrid('source').dataBind();
        }
    };

})(jQuery)


    function taskEnd(refresh) {
        wizard.displayStep(1);
        $('#divPartial').html('');
        if (refresh)
            $('#jqxgrid').jqxTreeGrid('source').dataBind();
    }

    function triggerCancelEdit() {
        task_grid.end(false);
    };

    function triggerTaskCompleted(id) {
        $("#jqxgrid").jqxTreeGrid('source').dataBind();
        $("#divPartial").html('');
        //app_dialog.dialogIframClose();
    };
