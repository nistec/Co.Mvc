
//============================================================================================ app_docs_grid

(function ($) {

    var doc_grid;
    var wizard;

    app_docs_grid = {

        //accType: 0,
        NScustomers: {},
        dataAdapter: {},
        AllowEdit: 0,
        UserId: 0,
        IsMobile: false,
        //NScustomers.nastedCategoriesGrids = new Array();
        source:
            {
                datatype: "json",
                async: false,
                datafields: [
                    { name: 'DocId', type: 'number' },
                    { name: 'DocSubject', type: 'string' },
                    { name: 'DocBody', type: 'string' },
                    { name: 'Doc_Type', type: 'number' },
                    { name: 'Doc_Parent', type: 'number' },
                    { name: 'Project_Id', type: 'number' },
                    { name: 'CreatedDate', type: 'date' },
                    { name: 'DueDate', type: 'date' },
                    { name: 'StartedDate', type: 'date' },
                    { name: 'EndedDate', type: 'date' },
                    { name: 'LastUpdate', type: 'date' },
                    { name: 'LastAct', type: 'string' },
                    { name: 'DocEstimateDays', type: 'number' },
                    { name: 'AccountId', type: 'number' },
                    { name: 'UserId', type: 'number' },
                    { name: 'AssignBy', type: 'number' },
                    { name: 'DisplayName', type: 'string' },
                    { name: 'AssignByName', type: 'string' },
                    { name: 'DocStatus', type: 'number' },
                    { name: 'StatusName', type: 'string' },
                    { name: 'DocTypeName', type: 'string' },
                    { name: 'Folder', type: 'string' },
                    { name: 'ProjectName', type: 'string' },
                    { name: 'IsShare', type: 'boolean' },
                    { name: 'TotalTimeView', type: 'string' },
                    { name: 'TotalRows', type: 'number' }
                ],
                id: 'DocId',
                type: 'POST',
                url: '/System/GetDocGrid',
                data: { 'AccountId': 0, 'UserId': 0, 'assignMe': false, 'state': 0 },
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
            app.redirectTo('/System/DocEdit?id=' + id);
            //wizard.displayStep(2);
            //$.ajax({
            //    type: 'GET',
            //    url: '/System/DocEdit',
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
            var subjectWidth = 250;// slf.IsMobile ? 250 : 800;
            var initDocFormGrid = function (tab, index, id) {

                var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
                nastedcontainer.rtl = true;
                var nastedsource = {
                    datafields: [
                        { name: 'ItemId', type: 'number' },
                        { name: 'UserId', type: 'number' },
                        { name: 'DisplayName', type: 'string' },
                        { name: 'AssignBy', type: 'number' },
                        { name: 'Doc_Id', type: 'number' },
                        { name: 'ItemDate', type: 'date' },
                        { name: 'DoneDate', type: 'date' },
                        { name: 'ItemLabel', type: 'string' },
                        { name: 'ItemValue', type: 'string' },
                        { name: 'DoneComment', type: 'string' },
                        { name: 'DoneStatus', type: 'bool' }
                    ],
                    datatype: "json",
                    id: 'ItemId',
                    type: 'POST',
                    url: '/System/GetDocsFormGrid',
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
                        { text: 'מועד רישום', datafield: 'ItemDate', width: '15%', cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                        { text: 'נושא', datafield: 'ItemLabel', width: '20%', cellsalign: 'right', align: 'center' },
                        { text: 'ערך', datafield: 'ItemValue', width: '20%', cellsalign: 'right', align: 'center' },
                        { text: 'מועד ביצוע', datafield: 'DoneDate', width: '15%', cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                        { text: 'הערה', datafield: 'DoneComment', width: '20%',cellsalign: 'right', align: 'center' },
                        { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: '10%', cellsalign: 'right', align: 'center' }
                        //{ text: 'מבצע', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center' }
                        //{
                        //    text: 'מספר רישום', datafield: 'ItemId', width: 120, cellsalign: 'right', align: 'center',
                        //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                        //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
                        //    }
                        //}
                    ]
                });

                //var refreshitem = $('<div style="margin:10px"><input type="button" value="רענן" title="רענון סיווגים" /></div>')
                //.click(function () {
                //    nastedAdapter.dataBind();
                //});

                $(tab).append(nastedcontainer);
                //$(tab).append(additem);
                //$(tab).append(refreshitem);

            };
              
            var initDocCommentsGrid = function (tab, index, id) {

                var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
                nastedcontainer.rtl = true;
                var nastedsource = {
                    datafields: [
                        { name: 'CommentId', type: 'number' },
                        { name: 'CommentDate', type: 'date' },
                        { name: 'CommentText', type: 'string' },
                        { name: 'Attachment', type: 'string' },
                        { name: 'DisplayName', type: 'string' },
                        { name: 'Doc_Id', type: 'number' },
                        { name: 'UserId', type: 'number' }
                    ],
                    datatype: "json",
                    id: 'CommentId',
                    type: 'POST',
                    url: '/System/GetDocsCommentGrid',
                    data: { 'pid': id }
                }
                slf.NScustomers.nastedCategoriesGrids[index] = nastedcontainer;

                var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
                nastedcontainer.jqxGrid({
                    localization: getLocalization('he'),
                    source: nastedAdapter, width: '99%', height: 130,
                    autorowheight: true,
                    autoheight: true,
                    columnsresize: true,
                    rtl: true,
                    columns: [
                        { text: 'מועד רישום', datafield: 'CommentDate', width: '15%', cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                        { text: 'הערה', datafield: 'CommentText', width: '70%', cellsalign: 'right', align: 'center' },
                        { text: 'שם', datafield: 'DisplayName', width: '15%', cellsalign: 'right', align: 'center' }
                        //{
                        //    text: 'מספר רישום', datafield: 'CommentId', width: 120, cellsalign: 'right', align: 'center',
                        //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                        //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
                        //    }
                        //}
                    ]
                });

                //var refreshitem = $('<div style="margin:10px"><input type="button" value="רענן" title="רענון סיווגים" /></div>')
                //.click(function () {
                //    nastedAdapter.dataBind();
                //});

                //var additem = $('<a title="הוספת הערה" href="#" style="padding-left:10px;padding-right:10px;float:right;">הוסף</a>')
                //.click(function () {
                //    slf.NScustomers.categoriesDialog = app_dialog.dialogIframe(app.appPath() + "/System/_DocCommentAdd?id=" + id, "580", "400", "הערות");
                //});
                //var refreshitem = $('<a title="רענון הערות"  href="#" style="padding-left:10px;float:right;">רענן</a>')
                //.click(function () {
                //    nastedAdapter.dataBind();
                //});

                //$(tab).append(additem);
                //$(tab).append(refreshitem);
                $(tab).append(nastedcontainer);

                //$(tab).append(additem);
                //$(tab).append(refreshitem);

            };

            var initrowdetails = function (index, parentElement, gridElement, datarecord) {

                slf.NScustomers.currentIndex = index;

                var tabsdiv = $($(parentElement).children()[0]);
                if (tabsdiv !== null) {
                    var tabinfo = tabsdiv.find('.information');
                    var tabnotes = tabsdiv.find('.body');
                    var tabcomments = tabsdiv.find('.comments');
                    //var tabhistory = tabsdiv.find('.history');
                    //var tabatimers = tabsdiv.find('.timers');
                    var tabform = tabsdiv.find('.form');

                    var title = tabsdiv.find('.title');
                    title.text('מסמך: ' + datarecord.DocId);

                    //tab notes
                    var notescontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"><span>' + app.htmlUnescape(datarecord.DocBody) + '</span></div>');
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
                        "<div style='margin: 10px;'><b>יוצר המסמך:</b> " + (datarecord.AssignByName || '') + "</div>" +
                        "<div style='margin: 10px;'><b>מועד עדכון:</b> " + datarecord.CreatedDate.toLocaleDateString() + "</div>" +
                        "<div style='margin: 10px;'><b>פרוייקט:</b> " + (datarecord.ProjectName || '') + "</div>");

                    divLeft.rtl = true;
                    var divRight = $("<div style='margin: 10px;'><b>לקוח\\מנוי:</b> " + (datarecord.ClientDetails || '') + "</div>" +
                        "<div style='margin: 10px;'><b>סוג מסמך:</b> " + (datarecord.DocTypeName || '') + "</div>" +
                        "<div style='margin: 10px;'><b>מועד התחלה:</b> " + app.toLocalDateString(datarecord.StartedDate) + "</div>" +
                        "<div style='margin: 10px;'><b>מועד סיום:</b> " + app.toLocalDateString(datarecord.EndedDate) + "</div>");


                    divRight.rtl = true;
                    $(leftcolumn).append(divLeft);
                    $(rightcolumn).append(divRight);

                    $(tabinfo).append(container);

                    var rcdid = datarecord.DocId;//parseInt(datarecord.DocId);

                    initDocCommentsGrid(tabcomments, index, rcdid);
                    //initDocTimersGrid(tabatimers, index, rcdid);
                    //initDocAssignmentsGrid(tabhistory, index, rcdid);
                    initDocFormGrid(tabform, index, rcdid);

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
                //var searchButton = $("<div style='float: left; margin-left: 5px;'><span style='margin-left: 4px; position: relative; top: -3px;'>Find</span></div>");
                container.append(reloadButton);
                container.append(clearFilterButton);
                //container.append(searchButton);
                container.append(queryButton);
                if (slf.AllowEdit === 1) {
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
                    app_docs_grid.memberDelete(rcdid);
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
                    app.redirectTo('/Main/DocsQuery');
                });

                // search for a record.
                //searchButton.click(function (event) {
                //    var offset = $("#jqxgrid").offset();
                //    $("#jqxwindow").jqxWindow('open');
                //    $("#jqxwindow").jqxWindow('move', offset.left + 30, offset.top + 30);
                //});
            };

            var cellclass = function (row, columnfield, value) {
                return app_tasks.statusColor(value);
                //switch (value)
                //{
                //    case "פתוח":
                //        return 'red'
                //    case "בטיפול":
                //        return 'yellow'
                //    case "סגור":
                //        return 'green'
                //}
            }

            // create Tree Grid
            $("#jqxgrid").jqxGrid(
                {
                    width: '100%',
                    autoheight: true,
                    enabletooltips: true,
                    rtl: true,
                    source: slf.dataAdapter,
                    localization: getLocalization('he'),
                    //virtualmode: true,
                    rendergridrows: function (obj) {
                        return slf.dataAdapter.records;
                    },
                    columnsresize: true,
                    pageable: true,
                    pagermode: 'simple',
                    sortable: true,
                    groupable: true,
                    //showfilterrow: true,
                    //filterable: true,
                    rowdetails: true,
                    rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li>תוכן</li><li class='title'></li><li>הערות</li><li>סיכומים</li></ul><div class='body'></div><div class='information'></div><div class='comments'></div><div class='form'></div></div>", rowdetailsheight: 200 },
                    //rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li class='title'>פרטים</li></ul><div class='information'></div></div>", rowdetailsheight: 200 },
                    initrowdetails: initrowdetails,
                    //showstatusbar: true,
                    //renderstatusbar: renderstatusbar,
                    //showtoolbar: false,
                    //rendertoolbar: function (toolbar) {
                    //    app_jqxgrid.gridFilterRtl(this, toolbar, columnList, dateList);
                    //},
                    columns: [
                         {
                            text: 'מס', dataField: 'DocId', filterable: false, width: 90, cellsalign: 'right', align: 'center',
                            cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                                var editlink = '';
                                var asb = $('#jqxgrid').jqxGrid('getrowdata', row).AssignBy;
                                if (slf.UserId === asb)
                                    editlink = '<label> </label><a href="#" onclick="app_docs_grid.docEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';


                                return '<div style="text-align:center;margin-top:6px">' + value + '<a href="#" onclick="app_docs_grid.docInfo(' + value + ')" ><label> </label><i class="fa fa-info-circle"></i></a>' + editlink + '</div>';
                            }
                        },
                        {
                            text: 'סוג מסמך', dataField: 'DocTypeName', cellsalign: 'right', align: 'center', width: 120,
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                        {
                            text: 'תיקייה', dataField: 'Folder', cellsalign: 'right', align: 'center', width: 120,
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                        {
                            text: '  נושא  ', dataField: 'DocSubject', cellsalign: 'right', align: 'center', width: subjectWidth,
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                       
                        //{
                        //    text: ' פרוייקט ', dataField: 'ProjectName', cellsalign: 'right', align: 'center',
                        //    filtertype: "custom",
                        //    createfilterpanel: function (datafield, filterPanel) {
                        //        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        //    }
                        //},
                        {
                            text: 'סטאטוס', dataField: 'StatusName', cellsalign: 'right', align: 'center', width: 120, cellsrenderer:
                                function (row, columnfield, value, defaulthtml, columnproperties) {
                                    var color = app_tasks.statusColor(value);
                                    return '<div style="text-align:right;margin-right:10px;margin-top:6px"><label> ' + value + ' </label><i class="fa fa-circle" style="font-size:16px;color:' + color + '"></i></div>';
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
                            text: 'נוצר ב', type: 'date', dataField: 'CreatedDate',  cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                        {
                            text: 'מועד לביצוע', type: 'date', dataField: 'DueDate',  cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
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
                        },
                        {
                            text: 'מועד התחלה', type: 'date', dataField: 'StartedDate', cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                        {
                            text: 'מועד סיום', type: 'date', dataField: 'EndedDate',  cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120,
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                        {
                            text: 'משך', dataField: 'TotalTimeView', cellsalign: 'right', align: 'center', width: 100,
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
                    ],
                    groups: ['DocTypeName','Folder']
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
                return slf.docEdit(id);
            });

        },
        docEdit: function (id) {
            app.redirectTo('/System/DocEdit?id=' + id);
            //wizard.displayStep(2);
            //$.ajax({
            //    type: 'GET',
            //    url: '/System/DocEdit',
            //    data: { "id": id },
            //    success: function (data) {
            //        $('#divPartial').html(data);
            //    }
            //});
        },
        docInfo: function (id) {
            app.redirectTo('/System/DocInfo?id=' + id);
            //wizard.displayStep(2);
            //$.ajax({
            //    type: 'GET',
            //    url: '/System/DocEdit',
            //    data: { "id": id },
            //    success: function (data) {
            //        $('#divPartial').html(data);
            //    }
            //});
        },
        docDelete: function (rcdid) {
            if (!confirm('האם למחוק את המשימה ' + rcdid)) {
                return;
            };
            $.ajax({
                type: "POST",
                url: '/System/DeleteDoc',
                data: { 'DocId': rcdid },
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
                    url: '/System/DeleteDocComment',
                    data: { 'DocId': rcdid, 'CommentId': id },
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        slf.categoriesRefresh();
                        if (data.Status > 0)
                            dialogMessage('הערות', 'תיעוד ' + rcdid + ' הוסר מתיעוד ' + id, true);
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
            this.NScustomers.nastedCategoriesGrids = new Array();
            this.UserId = userInfo.UserId;
            this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            //this.source.data = {
            //    //'QueryType': Model.QueryType,
            //    'AccountId': Model.AccountId,
            //    'UserId': Model.UserId
            //};
            this.source.data['AccountId'] = Model.AccountId;
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
            this.source.data['state'] = $('#DocState').val();
            $('#jqxgrid').jqxGrid('source').dataBind();
        }
    };

})(jQuery)

/*
(function ($) {

    var doc_grid;
    var wizard;

    app_docs_grid = {

        DataAdapter: {},
        AllowEdit: 0,
        UserId:0,
        IsMobile: false,
        source:
            {
                datatype: "json",
                datafields: [
                   { name: 'DocId', type: 'number' },
                   { name: 'DocSubject', type: 'string' },
                    { name: 'DocBody', type: 'string' },
                    { name: 'DocModel', type: 'string' },
                   { name: 'Doc_Type', type: 'number' },
                   { name: 'Doc_Parent', type: 'number' },
                   { name: 'Project_Id', type: 'number' },
                   { name: 'CreatedDate', type: 'date' },
                   { name: 'DueDate', type: 'date' },
                   { name: 'StartedDate', type: 'date' },
                   { name: 'EndedDate', type: 'date' },
                   { name: 'LastUpdate', type: 'date' },
                   { name: 'LastAct', type: 'string' },
                   { name: 'DocEstimateDays', type: 'number' },
                   { name: 'AccountId', type: 'number' },
                   {name: 'UserId', type: 'number' },
                   { name: 'AssignBy', type: 'number' },
                   { name: 'DisplayName', type: 'string' },
                   { name: 'AssignByName', type: 'string' },
                   { name: 'DocStatus', type: 'number' },
                   { name: 'StatusName', type: 'string' },
                   { name: 'DocTypeName', type: 'string' },
                   { name: 'ProjectName', type: 'string' },
                   { name: 'IsShare', type: 'boolean' },
                   { name: 'TotalTimeView', type: 'string' },
                   { name: 'TotalRows', type: 'number' }
                ],
                id: 'DocId',
                type: 'POST',
                url: '/System/GetDocGrid',
                data: { 'AccountId': 0, 'UserId': 0, 'assignMe': false, 'state': 0 },
                hierarchy:
                {
                    keyDataField: { name: 'DocId' },
                        parentDataField: { name: 'Doc_Type' }
                },
                pagenum: 0,
                pagesize: 20
                //root: 'Rows',
                //pager: function (pagenum, pagesize, oldpagenum) {
                //    console.log(pagenum);
                //    // callback called when a page or page size is changed.
                //},
                //filter: function () {
                //    // update the grid and send a request to the server.
                //    $("#jqxgrid").jqxTreeGrid('updatebounddata');
                //},
                //sort: function () {
                //    // update the grid and send a request to the server.
                //    $("#jqxgrid").jqxTreeGrid('updatebounddata');
                //},
                //beforeprocessing: function (data) {
                //    this.totalrecords = data.TotalRows;
                //}
            },
        edit: function (id) {
            app.redirectTo('/System/DocEdit?id=' + id);
            //wizard.displayStep(2);
            //$.ajax({
            //    type: 'GET',
            //    url: '/System/DocEdit',
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
                source: slf.DataAdapter,
                localization: getLocalization('he'),
                //virtualmode: true,
                //rendergridrows: function (obj) {
                //    return slf.DataAdapter.records;
                //},
                //columnsresize: true,
                pageable: true,
                pagerMode: 'advanced',
                sortable: true,
                //showfilterrow: true,
                //filterable: true,
                //rowdetails: true,
                //rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li>תוכן</li><li class='title'></li><li>הערות</li><li>העברות</li><li>מד-זמן</li><li>סיכומים</li></ul><div class='body'></div><div class='information'></div><div class='comments'></div><div class='history'></div><div class='timers'></div><div class='form'></div></div>", rowdetailsheight: 200 },
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
                            text: 'סוג מסמך', dataField: 'Doc_Type', cellsalign: 'right', align: 'center', 
                            filtertype: "custom",
                            createfilterpanel: function (datafield, filterPanel) {
                                app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                            }
                        },
                  {
                            text: 'מס', dataField: 'DocId', filterable: false, width: 120, cellsalign: 'right', align: 'center',
                        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                            return '<a href="#" onclick="app_docs_grid.docInfo(' + value + ')" ><label> ' + value + ' </label><i class="fa fa-info-circle"></i></a>';
                        }
                  },
                  {
                      text: '  נושא  ', dataField: 'DocSubject', cellsalign: 'right', align: 'center', width: subjectWidth,
                      filtertype: "custom",
                      createfilterpanel: function (datafield, filterPanel) {
                          app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                      }
                  },
 
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
                    }
                    // {
                    //     text: 'משך', dataField: 'TotalTimeView', cellsalign: 'right', align: 'center',width: 100,
                    //     filtertype: "custom",
                    //     createfilterpanel: function (datafield, filterPanel) {
                    //         app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    //     }
                    // },
                    //{
                    //    text: 'מועד התחלה משוער', type: 'date', dataField: 'EstimateStartTime', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', hidden: true,
                    //    filtertype: "custom",
                    //    createfilterpanel: function (datafield, filterPanel) {
                    //        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    //    }
                    //}
                  //{ text: 'משותף?', datafield: 'IsShare', threestatecheckbox: true, columntype: 'checkbox', width: 70 }
                    ]
                    //columnGroups: [
                    //    { text: 'DocTypeName', name: 'DocTypeName' }
                    //]
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
                return slf.docEdit(id);
            });

        },
        docEdit: function (id) {
            var model = app_jqx.findRecordValueByField(this.DataAdapter, id, "DocId", "DocModel");
            app_docs.docInfo(id, model);
            //app.redirectTo('/System/DocEdit?id=' + id);
        },
        docInfo: function (id) {
            var model = app_jqx.findRecordValueByField(this.DataAdapter, id, "DocId", "DocModel");
            app_docs.docInfo(id, model);
        },
        docDelete: function (rcdid) {
            if (!confirm('האם למחוק את המשימה ' + rcdid)) {
                return;
            };
            $.ajax({
                type: "POST",
                url: '/System/DeleteDoc',
                data: { 'DocId': rcdid },
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
                    url: '/System/DeleteDocComment',
                    data: { 'DocId': rcdid, 'CommentId': id },
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

            this.DataAdapter = new $.jqx.dataAdapter(this.source, {
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
            this.source.data['state'] = $('#DocState').val();
            $('#jqxgrid').jqxTreeGrid('source').dataBind();
        }
    };

})(jQuery)
*/

    function docEnd(refresh) {
        wizard.displayStep(1);
        $('#divPartial').html('');
        if (refresh)
            $('#jqxgrid').jqxTreeGrid('source').dataBind();
    }

    function triggerCancelEdit() {
        doc_grid.end(false);
    };

    function triggerDocCompleted(id) {
        $("#jqxgrid").jqxTreeGrid('source').dataBind();
        $("#divPartial").html('');
        //app_dialog.dialogIframClose();
    };
