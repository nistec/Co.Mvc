'use strict';

class app_accounts_base {


    members_usersRefresh() {
        members_grid.usersRefresh();
    }

    members_usersDelete(id, rcdid) {
        members_grid.usersDelete(id, rcdid);
    }

    triggerUsersComplete(memid) {
        members_usersRefresh();
        app_dialog.dialogClose(members_grid.NContainer.usersDialog);
    }

    triggerWizControlCompleted(control_name, OutputId) {

        $("#jqxgrid").jqxGrid('source').dataBind();
        //app_dialog.dialogIframClose();
        wizard.wizHome();
    }

    triggerWizControlCancel(control_name) {
        wizard.wizHome();
    }

    triggerWizControlQuery(dataModel) {
        //var Model = JSON.parse(dataModel);
        members_grid.loadQuery(dataModel)
        wizard.wizHome();
    }

    triggerGridCompleted(grid) {

        $(grid).jqxGrid('source').dataBind();

    }
   
}

//============================================================================================ app_members_grid

class app_accounts_grid {

    constructor() {
        //accType: 0,
        this.NContainer = {};
        this.DataAdapter = {};
        this.AllowEdit = 0;
        this.IsMemberQuery = 0;
        this.UInfo = null;
        this.CurrentAccount = 0;
        this.Control = null;
        this.UserControl = null;
    }

    init(Model, userInfo) {

        var _slf = this;

        //this.NContainer.nastedGrids = new Array();
        this.NContainer.UsersGrid = new Array();
        this.NContainer.LabelsGrid = new Array();
        this.NContainer.PricesGrid = new Array();
        this.NContainer.CreditGrid = new Array();

        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;

        this.DataSource =
            {
                datatype: "json",
            async: false,
   
                datafields: [
                    { name: 'AccountId', type: 'number' },
                    { name: 'ParentId', type: 'number' },
                    { name: 'ContactName', type: 'string' },
                    { name: 'IdNumber', type: 'string' },
                    { name: 'AccountName', type: 'string' },
                    { name: 'Address', type: 'string' },
                    { name: 'City', type: 'string' },
                    //{ name: 'CityName', type: 'string' },
                    { name: 'ZipCode', type: 'string' },
                    { name: 'CreationDate', type: 'date' },
                    { name: 'LastUpdate', type: 'date' },
                    { name: 'Mobile', type: 'string' },
                    { name: 'Phone', type: 'string' },
                    { name: 'Email', type: 'string' },
                    { name: 'Fax', type: 'string' },
                    { name: 'Details', type: 'string' },
                    { name: 'IsActive', type: 'bool' },
                    { name: 'Path', type: 'string' },
                    { name: 'Branch', type: 'string' },
                    { name: 'CountryName', type: 'string' },
                    { name: 'OwnerName', type: 'string' },

                    { name: 'AccTypeName', type: 'string' },
                    { name: 'AccountCategoryName', type: 'string' },
                    { name: 'Ephone_Id', type: 'number' },
                    { name: 'Member_Id', type: 'number' },
                    { name: 'TotalRows', type: 'number' }
                ],
                id: 'AccountId',
                type: 'POST',
                url: '/Admin/GetAccountsGrid',
                //data:{},
                //pagenum: 0,
                pagesize: 20,
                root: 'Rows',
                pager: function (pagenum, pagesize, oldpagenum) {
                    console.log(pagenum);
                    // callback called when a page or page size is changed.
                },
                filter: function () {
                    // update the grid and send a request to the server.
                    $("#jqxgrid").jqxGrid('updatebounddata');
                },
                sort: function () {
                    // update the grid and send a request to the server.
                    $("#jqxgrid").jqxGrid('updatebounddata');
                },
                beforeprocessing: function (data) {
                    this.totalrecords = data.TotalRows;
                }
            }

        this.DataSource.data = {
            'QueryType': Model.QueryType,
            'AccountId': Model.AccountId,
            'ParentId': Model.ParentId,
            'IdNumber': Model.IdNumber,
            'Mobile': Model.Mobile,
            'Email': Model.Email,
            'AccountName': Model.AccountName,
            'Address': Model.Address,
            'City': Model.City,
            'Category': Model.Category,
            'Branch': Model.Branch,
            'JoinedFrom': Model.JoinedFrom,
            'JoinedTo': Model.JoinedTo,
            'ContactRule': Model.ContactRule
        };

        this.DataAdapter = new $.jqx.dataAdapter(this.DataSource, {
            loadComplete: function (data) {
                //source.totalrecords = getTotalRows(data);
            },
            loadError: function (xhr, status, error) {
                app_dialog.alert(' status: ' + status + '\n error ' + error)
            }
        });

        this.initGrid();

        //if (this.AllowEdit == 0) {
        //    $("$member-item-delete").hide();
        //}

        $('#member-item-update').click(function () {
            //var iframe = wizard.getIframe();
            //if (iframe && iframe.def) {
            //    iframe.def.doSubmit();
            //}
            _slf.update();
        });
        $('#member-item-update-plus').click(function () {
            //var iframe = wizard.getIframe();
            //if (iframe && iframe.def) {
            //    iframe.def.doSubmit();
            //}
            _slf.update_plus();
        });
        $('#member-item-cancel').click(function () {
            //wizard.wizHome();
            _slf.cancel();
        });

        $('#member-item-add').click(function () {
            _slf.add();
        });
        $('#member-item-edit').click(function () {
            _slf.edit();
        });
        $('#member-item-delete').click(function () {
            _slf.remove();
        });
        $('#member-item-filetr-remove').click(function () {
            _slf.clear_filter();
        });
        $('#member-item-query').click(function () {
            _slf.query();
        });
    }

    initGrid() {

        var _slf = this;

 

        var initrowdetails = function (index, parentElement, gridElement, datarecord) {

            _slf.NContainer.currentIndex = index;

            var tabsdiv = null;
            var information = null;
            var notes = null;

            tabsdiv = $($(parentElement).children()[0]);
            if (tabsdiv != null) {
                information = tabsdiv.find('.information');
                notes = tabsdiv.find('.notes');
                var users = tabsdiv.find('.users');
                var prices = tabsdiv.find('.prices');
                var credit = tabsdiv.find('.credit');
                var labels = tabsdiv.find('.labels');
                //var tabhistory = tabsdiv.find('.history');
                var tabaction = tabsdiv.find('.action');

                var memid = datarecord.IdNumber;
                var rcdid = datarecord.AccountId;

                var title = tabsdiv.find('.title');
                title.html('<span style="margin:10px"> ' + datarecord.AccountName + ' </span><a title="עריכת לקוח" href="javascript:members_grid.edit(' + rcdid + ');" >@</a>');

                var container = $('<div style="margin: 5px;text-align:right;"></div>')
                container.rtl = true;
                container.appendTo($(information));

                var leftcolumn = $('<div style="float: left; width: 45%;direction:rtl;"></div>');
                var rightcolumn = $('<div style="float: right; width: 40%;direction:rtl;"></div>');
                container.append(leftcolumn);
                container.append(rightcolumn);

                var divLeft = $(//"<div style='margin: 10px;'><b>קוד חיוב:</b> " + datarecord.ChargeName + "</div>" +
                    "<div style='margin: 10px;'><b>נתיב:</b> " + datarecord.Path + "</div>" +
                    "<div style='margin: 10px;'><b>סניף:</b> " + datarecord.Branch + "</div>" +
                    "<div style='margin: 10px;'><b>חשבון תקשורת:</b> " + datarecord.Ephone_Id + "</div>" +
                    "<div style='margin: 10px;'><b>מועד הצטרפות:</b> " + app.toLocalDateTimeString(datarecord.CreationDate) + "</div>" +
                    "<div style='margin: 10px;'><b>מועד עדכון:</b> " + app.toLocalDateTimeString(datarecord.LastUpdate) + "</div>");

                divLeft.rtl = true;
                var divRight = $(
                    "<div style='margin: 10px;'><b>איש קשר:</b> " + datarecord.ContactName + "</div>" +
                    "<div style='margin: 10px;'><b>דואל:</b> " + app.toEmailLink(datarecord.Email) + "</div>" +
                    "<div style='margin: 10px;'><b>טלפון:</b> " + app.toPhoneLink(datarecord.Phone) + "</div>" +
                    "<div style='margin: 10px;'><b>מנוי:</b> " + app.toPhoneLink(datarecord.Member_Id) + "</div>" +
                    "<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.IdNumber + "</div>");


                divRight.rtl = true;
                $(leftcolumn).append(divLeft);
                $(rightcolumn).append(divRight);

                var notescontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"><span>' + datarecord.Details + '</span></div>');
                notescontainer.rtl = true;
                $(notes).append(notescontainer);


                //actions
                var actsContainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"></div>');
                actsContainer.rtl = true;
                var act1 = $('<a href="#" class="btn-tab" style="white-space: normal; margin: 5px;text-align:right;">ניהול לקוח</a>')
                .on('click', function () {
                    if (rcdid) {
                        if (rcdid <= 1)
                            app_dialog.alert("לא ניתן לניהול");
                        else
                            app_query.doPost('/Admin/DoVirtualLoginSet', { 'accountId': rcdid });
                    }
                });
                var act2 = $('<a class="btn-tab" href="#" style="white-space: normal; margin: 5px;text-align:right;">ביטול ניהול לקוח</a>')
                .on('click', function () {
                    app_query.doPost('/Admin/DoVirtualLoginCancel');
                    });

                var act3 = $('<a class="btn-tab" href="#" style="white-space: normal; margin: 5px;text-align:right;">מחיקת זכרון מטמון</a>')
                    .on('click', function () {
                        app_dialog.confirm("פעולה זו תסיר את כל התכנים מהזכרון, האם להמשיך ?", function (args) {
                            app_query.doPost('/Cms/CmsClearAllCache', {});
                        });
                    });

                var act4 = $('<a class="btn-tab" href="#" style="white-space: normal; margin: 5px;text-align:right;">דוח בילינג</a>')
                    .on('click', function () {
                        
                    });
                var act5 = $('<a class="btn-tab" href="#" style="white-space: normal; margin: 5px;text-align:right;">דוח פעילות</a>')
                    .on('click', function () {

                    });

                $(actsContainer).append(act5);
                $(actsContainer).append(act4);
                $(actsContainer).append(act3);
                $(actsContainer).append(act2);
                $(actsContainer).append(act1);
                $(tabaction).append(actsContainer);


                initLabelsGrid(labels, index, rcdid, _slf.NContainer, "#jqxgrid-panel");
                initUsersGrid(users, index, rcdid, _slf.NContainer, "#jqxgrid-panel", _slf.UInfo);
                initPricesGrid(prices, index, rcdid, _slf.NContainer, "#jqxgrid-panel");
                initCreditGrid(credit, index, rcdid, _slf.NContainer, "#jqxgrid-panel");
                

                $(tabsdiv).jqxTabs({ width: '95%', height: 210, rtl: true });
            }
        };

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
            {
                width: '99%',
                autoheight: true,
                rtl: true,
                source: _slf.DataAdapter,
                localization: getLocalization('he'),
                virtualmode: true,
                rendergridrows: function (obj) {
                    return _slf.DataAdapter.records;
                },
                columnsresize: true,
                pageable: true,
                pagermode: 'simple',
                sortable: true,
                //groupable: true,
                //showfilterrow: true,
                //filterable: true,
                rowdetails: true,
                rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 20px;'><li class='title'></li><li>הערות</li><li>פרטים</li><li>משתמשים</li><li>מחירים</li><li>אשראי</li><li>אפשרויות</li></ul><div class='information'></div><div class='notes'></div><div class='labels'></div><div class='users'></div><div class='prices'></div><div class='credit'></div><div class='action'></div></div>", rowdetailsheight: 230 },
                //ready: function () {
                //    $("#jqxgrid").jqxGrid('showrowdetails', 0);
                //},
                initrowdetails: initrowdetails,
                //autoshowfiltericon: true,
                //columnmenuopening: function (menu, datafield, height) {

                //    var column = $("#jqxgrid").jqxGrid('getcolumn', datafield);
                //    if (column.filtertype === "custom") {
                //        menu.height(155);
                //        setTimeout(function () {
                //            menu.find('input').focus();
                //        }, 25);
                //    }
                //    else menu.height(height);
                //},
                //showstatusbar: true,
                //renderstatusbar: renderstatusbar,
                showtoolbar: false,
                //rendertoolbar: function (toolbar) {
                //    app_jqxgrid.gridFilterRtl(this, toolbar, columnList, dateList);
                //},
                columns: [
                    {
                        text: 'מס.לקוח', dataField: 'AccountId', filterable: false, width: 100, cellsalign: 'right', align: 'center'
                        //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                         //    var link = '<div style="text-align:center"><a href="#" onclick="memberEdit(' + value + ')" >הצג</a>';
                         //    if (_slf.AllowEdit == 1) {
                         //        var mid = $('#jqxgrid').jqxGrid('getrowdata', row).MemberId;
                         //        return link + ' | <a href="#" onclick="memberDelete(' + mid + ')" >הסר</a>';
                         //    }
                         //    else
                         //        return link + '</div>';
                         //    //return '<div style="text-align:center"><a href="#" onclick="memberEdit(' + value + ')" >הצג</a></div>';
                         //}
                    },
                    //{
                    //    text: 'ת.ז', dataField: 'IdNumber', width: 100, cellsalign: 'right', align: 'center',
                    //    filtertype: "custom",
                    //    createfilterpanel: function (datafield, filterPanel) {
                    //        app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                    //    }
                    //},
                    {
                        text: 'שם לקוח', dataField: 'AccountName', width: 200, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'קטגוריה', dataField: 'AccountCategoryName', width: 160, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: ' עיר   ', dataField: 'City', width: 120, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: ' כתובת ', dataField: 'Address', width: 160,cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'טלפון נייד', dataField: 'Mobile', width: 110, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'דואר אלקטרוני', dataField: 'Email', cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'מועד הצטרפות', type: 'date', dataField: 'CreationDate', width: 110,cellsformat: 'd', cellsalign: 'right', align: 'center',
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
            //var args = event.args;
            //var boundIndex = args.rowindex;
            //var visibleIndex = args.visibleindex;
            //var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex);
            //app_popup.memberEdit(id);
            _slf.edit();
        });
        $('#jqxgrid').on('rowselect', function (event) {
            // event arguments.
            var args = event.args;
            // row's bound index.
            var rowBoundIndex = args.rowindex;
            // row's data. The row's data object or null(when all rows are being selected or unselected with a single action). If you have a datafield called "firstName", to access the row's firstName, use var firstName = rowData.firstName;
            var rowData = args.row;
            _slf.CurrentAccount = rowData.AccountId;
        });
        $('#jqxgrid').on('rowexpand', function (event) {
            // event arguments.
            var args = event.args;
            // row details.
            //var details = args.details;
            // row's bound index.
            var rowBoundIndex = args.rowindex;
            _slf.CurrentAccount = $('#jqxgrid').jqxGrid('getrowdata', rowBoundIndex).AccountId;

            //var rowData = args.row;
            //_slf.CurrentAccount = rowData.AccountId;

        });
    }

    getTotalRows(data) {
        if (data) {
            return dataTotalRows(data);
        }
        return 0;
    }

    usersEdit(userId, index, row) {
        console.log('user edit');
        //var rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        //var accountId = $('#jqxgrid').jqxGrid('getrowdata', rowindex).AccountId;

        var accountId = this.CurrentAccount;
        var option = userId > 0 ? 'e' : 'a';
        var record;
        if (userId > 0) {

            var data = this.NContainer.UsersGrid[index].jqxGrid('source');
            record = data.records[row];

        }


        var data_model = { Id: userId, AccountId: accountId, Option: option, Action: 'edit' };
        //$("#divPartial2").empty();

        //var app = new app_user_def_control("#divPartial2");
        //app.init(data_model, this.UInfo);
        //app.display();

        wizard.displayStep(4);
   

        if (this.UserControl == null) {
            this.UserControl = new app_user_def_control("#divPartial4");
            // this.Control = control;
        }
        this.UserControl.init(data_model, this.UInfo, record);
        this.UserControl.display();
    }

    usersRefresh() {
        try {
            var i = this.NContainer.currentIndex;
            var g = this.NContainer.UsersGrid[i];
            g.jqxGrid('source').dataBind();
        }
        catch (e) {
            app_dialog.alert(e);
        }
    }

    usersDelete(userId) {
        //accountNewsRemove(id, memid);
        var _slf = this;
        if (confirm("האם להסיר משתמש " + userId )) {
            $.ajax({
                type: "POST",
                url: '/Admin/UserDefDelete',
                data: { 'UserId': userId },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    _slf.usersRefresh();
                    if (data.Status > 0)
                        app_dialog.alert('משתמש ' + userId + ' הוסר  ' );
                    else
                        app_dialog.alert(data.Message);
                },
                error: function (e) {
                    app_dialog.alert(e);
                }
            });
        }
        this.usersRefresh();
    }


    loadQuery(Model) {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.UsersGrid = new Array();
        this.IsMemberQuery = 1;

        this.DataSource.data = {
            'QueryType': Model.QueryType,
            'AccountId': Model.AccountId,
            'IdNumber': Model.IdNumber,
            'Mobile': Model.Mobile,
            'Email': Model.Email,
            'Name': Model.Name,
            'Address': Model.Address,
            'City': Model.City,
            'Category': Model.Category,
            'Branch': Model.Branch,
            'JoinedFrom': Model.JoinedFrom,
            'JoinedTo': Model.JoinedTo,
            'ContactRule': Model.ContactRule
        };

        this.DataAdapter.dataBind();

        wizard.displayStep(1);
    }

    cancelQuery() {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.UsersGrid = new Array();
        this.IsMemberQuery = 0;
        var accid = this.source.data.AccountId;
        
        this.DataSource.data = {
            'QueryType': 0,
            'AccountId': accid,
            'IdNumber': "",
            'Mobile': "",
            'Email': "",
            'Name': "",
            'Address': "",
            'City': "",
            'Category': "",
            'JoinedFrom': "",
            'JoinedTo': "",
            'ContactRule': ""
            
        };

        this.DataAdapter.dataBind();

        wizard.displayStep(1);
    }

    showControl(id, option, action) {

        var data_model = { Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_accounts_def_control("#divPartial2");
           // this.Control = control;
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }

    getrowId() {

        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
        return id;
    }

    selectRow(index) {
        $('#jqxgrid').jqxGrid('selectrow', index);
    }

    query() {
        //app.redirectTo('/Co/MembersQuery');
        if (!wizard.existsIframe(3)) {
            wizard.appendIframe(3, app.appPath() + "/Admin/_AccountsQuery", "100%", "680px", true, "#loader");
        }
        wizard.displayStep(3);
    }

    add() {
        $('#member-item-update').show();
        $("#member-item-update-plus").show();
        //wizard.appendIframe(2, app.appPath() + "/Co/_MemberAdd", "100%", "620px", true, "#loader");
        wizard.displayStep(2);
        this.showControl(0, 'a');
    }

    edit(id) {
        $("#member-item-update-plus").hide();
        if (id === undefined) id = this.getrowId();
        if (id > 0) {

            $('#member-item-update').show();
            //wizard.displayStep(2);
            //this.showControl(id, 'e');
            //wizard.appendIframe(2, app.appPath() + "/Admin/AccountEdit?id=" + id, "100%", "620px", true);//, "#loader");
            app.redirectTo("/Admin/AccountEdit?id=" + id);
        }
    }

    view (id) {
        $("#member-item-update-plus").hide();
            $('#member-item-update').hide();
            if (id === undefined) id = this.getrowId();
        if(id > 0) {
            wizard.displayStep(2);
            this.showControl(id, 'g');
            //$('#member-item-update').hide();
            //wizard.appendIframe(2, app.appPath() + "/Co/_MemberEdit?id=" + id, "100%", "620px", true, "#loader");
        }
    }

    befor_update() {
        //app_jqxcombos.renderCheckList("listCategory", "Categories");
    }

    update() {
        if (this.Control != null) {
            this.Control.doSubmit();
        }
        //var iframe = wizard.getIframe();
        //if (iframe) {
        //    iframe.app_query.doFormPost('#fcForm', this.end, this.befor_update,"#validator-message");
        //}
    }

    update_plus() {

        if (this.Control != null) {
            this.Control.doSubmit();
        }

        //var iframe = wizard.getIframe();
        //if (iframe) {
        //    iframe.app_query.doFormPost('#fcForm', this.end_plus, this.befor_update, "#validator-message");
        //}
    }

    remove(id) {
        if (id === undefined) id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק את המנוי ' + id)) {
                app_query.doPost(app.appPath() + "/Admin/AccountDelete", { 'AccountId': id }, this.end_internal);
            }
        }
    }

    refresh() {
        $('#jqxgrid').jqxGrid('source').dataBind();
    }

    clear_filter() {
        if (this.IsMemberQuery == 1)
            this.cancelQuery();
        else
            $("#jqxgrid").jqxGrid('clearfilters');
    }

    cancel() {
        wizard.wizHome();
    }

    end_internal(data) {
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            $('#jqxgrid').jqxGrid('source').dataBind();
        }
    }

    end(data) {
        //wizard.removeIframe(2);
        wizard.wizHome();
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            window.parent.members_grid.refresh();
        }
    }

    end_plus(data) {
        //wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            wizard.changeIframe(app.appPath() + "/Co/_MemberAdd");
        }
    }

    show_payment(args) {

        console.log(args);
        var data = decodeURIComponent(args);
        //var datarecord = JSON.parse(data);

        //ar search = location.search.substring(1);
        //var datarecord = JSON.parse('{"' + data.replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}')

        var datarecord = {};
        data.replace(/([^=&]+)=([^&]*)/g, function (m, key, value) {
            datarecord[key] = value;
        });

        JSON.useDateParser();

        var container = $('<div style="margin: 5px;text-align:right;direction:rtl"></div>')
        container.rtl = true;
        //container.appendTo($(information));
        var strdate = datarecord.ExpirationDate.replace(/\+/gi, " ").replace("GMT ", "GMT+");
        var d = new Date(strdate);
        var ExpirationDate = d.toLocaleDateString();

        var divLeft = $("<div style='margin: 10px;direction:rtl'><b>קוד רישום:</b> " + datarecord.SignupId + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>קוד הפנייה:</b> " + datarecord.ReferralCode + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>חיוב אוטומטי:</b> " + datarecord.AutoCharge + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>אישור הצהרה:</b> " + datarecord.ConfirmAgreement + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>מועד סיום:</b> " + ExpirationDate + "</div>");

        divLeft.rtl = true;
        var divRight = $("<div style='margin: 10px;'><b>כתובת אלקטרונית:</b> " + datarecord.RegHostAddress + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>מקור רישום:</b> " + datarecord.RegReferrer + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>תוקף:</b> " + datarecord.ValidityMonth + "</div>" +
            "<div style='margin: 10px;direction:rtl'><b>בעל כרטיס אשראי:</b> " + datarecord.CreditCardOwner + "</div>"
            //"<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.MemberId + "</div>"
        );

        divRight.rtl = true;
        container.append("<h3>רישום</h3>");
        container.append(divLeft);
        container.append(divRight);

        var paymentcontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;direction:rtl"></div>');
        paymentcontainer.rtl = true;

        if (datarecord.PayId !== undefined || datarecord.PayId > 0) {
            //paymentcontainer.append($("<div style='margin: 10px;text-align:center'><b>לא נמצאו נתונים</b></div>"));
            paymentcontainer.append(
                $("<div style='margin: 10px;'><b>מספר קבלה:</b> " + datarecord.PayId + "</div>" +
                    "<div style='margin: 10px;'><b>סכום ששולם:</b> " + datarecord.Payed + "</div>" +
                    "<div style='margin: 10px;'><b>משלם\\ת:</b> " + datarecord.PaymentOwner + "</div>" +
                    "<div style='margin: 10px;'><b>מועד תשלום:</b> " + datarecord.PayedDate + "</div>")
            );
            if (datarecord.PayedDate)
                paymentcontainer.append($("<div style='margin: 10px;'><b>מועד תשלום:</b> " + app.formatDateString(datarecord.PayedDate) + "</div>"));
        }
        //"<div style='margin: 10px;'><b>בעל כרטיס אשראי:</b> " + datarecord.Token + "</div>" +
        container.append("<h3>תשלום</h3>");
        container.append(paymentcontainer);

        //app.jsonToHtml(container)

        app_jqx.toolTipClick('.signup-info', container[0].outerHTML, 350);
    }

    OpenWindow(jsonString) {
        console.log(data);
        function createWindow(jsonString) {
            var jqxWidget = $('#jqxWidget');
            var offset = jqxWidget.offset();

            $('#window').jqxWindow({
                position: { x: offset.left + 50, y: offset.top + 50 },
                showCollapseButton: true, maxHeight: 400, maxWidth: 700, minHeight: 200, minWidth: 200, height: 300, width: 500,
                initContent: function () {
                    //$('#tab').jqxTabs({ height: '100%', width:  '100%' });
                    //var container = $('<div style="margin: 5px;text-align:right;"></div>')

                    var content = display(data);
                    $("#windowContent").append(content);
                    $('#window').jqxWindow('focus');
                }
            });
        };

        return {
            config: {
                dragArea: null
            },
            init: function () {
                //Creating all jqxWindgets except the window
                //createElements();
                //Attaching event listeners
                //addEventListeners();
                //Adding jqxWindow
                createWindow(jsonString);
            }
        };
    }

}





//============================================================================================ app_members_def
/*
class app_accounts_def {

    constructor(recordId, userInfo, isdialog) {

        this.AccountId = recordId;
        //this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        this.IsDialog = isdialog;

        $("#AccountId").val(this.AccountId);

        //this.loadControls();
    }

    init() {

        var _slf = this;

        var view_source =
            {
                datatype: "json",
                //datafields: [
                //        { name: 'AccountId', type: 'number' },
                //        { name: 'AccountName', type: 'string' },
                //        { name: 'AccountType', type: 'string' },
                //        { name: 'AccountCategory', type: 'number' },
                //        //{ name: 'CompanyName', type: 'string' },
                //        //{ name: 'ContactName', type: 'string' },
                //        { name: 'Street', type: 'string' },
                //        { name: 'City', type: 'string' },
                //        { name: 'Phone1', type: 'string' },
                //        { name: 'Phone2', type: 'string' },
                //        //{ name: 'Mobile', type: 'string' },
                //        //{ name: 'Email', type: 'string' },
                //        { name: 'Fax', type: 'string' },
                //        { name: 'WebSite', type: 'string' },
                //        { name: 'ZipCode', type: 'string' },
                //        { name: 'Details', type: 'string' }
                //],
                id: 'AccountId',
                data: { 'id': _slf.AccountId },
                type: 'POST',
                url: '/Admin/GetAccountEdit'
            };

        this.viewAdapter = new $.jqx.dataAdapter(view_source, {
            loadComplete: function (record)  {

                _slf.this.loadControls(record);
            },
            loadError: function (jqXHR, status, error) {
            },
            beforeLoadComplete: function (records) {
            }
        });

        if (this.AccountId > 0) {
            this.viewAdapter.dataBind();
        }
        else {

        }
    }

    doSubmit() {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        app_jqxcombos.renderCheckList("listCategory", "Categories");
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#fcForm').serialize(),
                    success: function (data) {
                        app_dialog.alert(data.Message);
                        if (data.Status >= 0) {
                            if (_slf.IsDialog) {
                                window.parent.triggerMemberComplete(data.OutputId);
                                //$('#fcForm').reset();
                            }
                            else {
                                app.refresh();
                            }
                            //$('#RecordId').val(data.OutputId);
                        }
                    },
                    error: function (jqXHR, status, error) {
                        app_dialog.alert(error);
                    }
                });
            }
        }
        $('#fcForm').jqxValidator('validate', validationResult);
    };

    //syncData(record) {

    //    if (record) {

            

    //        app_jqxcombos.selectCheckList("listCategory", record.Categories);

    //        app_jqxcombos.initComboValue('City', 0);
    //    }
    //}

    loadControls(record) {

        
        //app_members.displayMemberFields();

        //app_jqx_list.branchComboAdapter();
        //app_jqx_list.cityComboAdapter();

 
        if (record) {
 
            app_form.loadDataForm("fcForm", record);

            //app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", "AccountCategory", "/Admin/GetAccountsCategories", null, 240, 140, "", record.AccountCategory, function () {
            //});
        }
        else {
            app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", "AccountCategory", "/Admin/GetAccountsCategories", null, 240, 140, "");
            //app_jqxcombos.initComboValue('City', 0);
        }
        //app_jqx_list.categoryCheckListAdapter();
        //app_jqx_list.chargeComboAdapter('ChargeType');
        //var exType = $("#ExType").val();

        var input_rules = [
            { input: '#AccountName', message: 'חובה לציין שם לקוח!', action: 'keyup, blur', rule: 'required' },
            //{ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
            //{
            //    input: "#City", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
            //        var index = $("#City").jqxComboBox('getSelectedIndex');
            //       return index != -1;
            //    }
            //},
            { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
            {
                input: '#Mobile', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                function (input, commit) {
                    var val = input.val();
                    var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                    return val ? re.test(val) : true;
                }
            },
            {
                input: '#Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
                function (input, commit) {
                    var val = input.val();
                    var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                    return val ? re.test(val) : true;
                }
            }
        ];
        if (exType == 0)
            input_rules.push({ input: '#IdNumber', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
        if (exType == 1)
            input_rules.push({ input: '#Mobile', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        if (exType == 2)
            input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });
        //if (exType == 3)
        //    input_rules.push({ input: '#Exid', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });

        $('#fcForm').jqxValidator({
            rtl: true,
            hintType: 'tooltip',
            animationDuration: 0,
            rules: input_rules
        });


    }

}

//============================================================================================ app_members_query

class app_accounts_query {

    constructor(userInfo, isdialog){
        this.IsDialog = isdialog;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        
        $("#AccountId").val(this.AccountId);

        this.init();

        //$("#accordion").show();
        //$("#accordion").jcxTabs({ active: 'exp-1'});
    }

    init() {
        var _slf = this;

        $("#allBranch").prop('checked', true);
        $("#allCity").prop('checked', true);
        //$("#allPlace").prop('checked', true);
        $("#allCategory").prop('checked', true);
        //$("#allStatus").prop('checked', true);
        $("#allRegion").prop('checked', true);


        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            //click: function (e, tab) {
            //    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            //},
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                return false;
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });

        //$("#accordion").accordion({ heightStyle: "content" });
        //$("#accordion-container").accordion({ heightStyle: "content" });

        //$("#exp-1").jqxExpander({ width: 'auto', rtl: true, expanded: true });
        //$("#exp-2").jqxExpander({ width: 'auto', rtl: true, expanded: false });
        //$("#exp-3").jqxExpander({ width: 'auto', rtl: true, expanded: false });
        //$("#exp-4").jqxExpander({ width: 'auto', rtl: true, expanded: false });

        var branchAdapter = app_jqxcombos.createComboAdapter("PropId", "PropName", "listBranch", '/Common/GetBranchView', 240, 120, true);
        $('#listBranch').on('change', function (event) {
            app_jqxcombos.comboBoxToInput("listBranch", "Branch", "allBranch");
        });


        app_jqx_list.enum1ComboAdapter();
        app_jqx_list.enum2ComboAdapter();
        app_jqx_list.enum3ComboAdapter();
        app_members.displayMemberFields();

        var categoryAdapter = app_jqxcombos.createListAdapter("PropId", "PropName", "listCategory", '/Common/GetCategoriesView', 240, 200, true);
        $('#listCategory').jqxListBox({ multiple: true });
        $('#listCategory').on('change', function (event) {
            app_jqxcombos.listBoxToInput("listCategory", "Category", "allCategory");
        });


        var regionAdapter = app_jqxcombos.createListAdapter("PropId", "PropName", "listRegion", '/Common/GetRegionView', 240, 120, false);
        $('#listRegion').on('change', function (event) {
            app_jqxcombos.listBoxToInput("listRegion", "Region", "allRegion");
        });

        var currentRegion = function () {

            var reg = $("#Region").val();
            if (isNumeric(reg))
                return reg;
            return 0;
        }

        var citySource =
            {
                dataType: "json",
                dataFields: [
                    { name: 'PropId' },
                    { name: 'RegionId' },
                    { name: 'PropName' }
                ],
                data: { 'region': currentRegion() },
                type: 'POST',
                url: '/Common/GetCityRegionView'
            };

        var cityAdapter = new $.jqx.dataAdapter(citySource, {
            //contentType: "application/json; charset=utf-8",
            loadError: function (jqXHR, status, error) {
                //alert("areaAdapter failed: " + error);
            },
            loadComplete: function (data) {
                //alert("areaAdapter is Loaded");
            }
        });
        // perform Data Binding.
        //areaAdapter.dataBind();


        $("#listCity").jqxListBox(
            {
                rtl: true,
                source: cityAdapter,
                width: 240,
                height: 120,
                multiple: true,
                displayMember: 'PropName',
                valueMember: 'PropId'
            });
        $('#listCity').on('change', function (event) {
            app_jqxcombos.listBoxToInput("listCity", "City");//, "allCity");
        });

        $("#listRegion").bind('select', function (event) {
            if (event.args) {

                $("#listCity").jqxListBox({ disabled: false, selectedIndex: -1 });
                var value = event.args.item.value;
                citySource.data = { 'region': value };
                cityAdapter = new $.jqx.dataAdapter(citySource);
                $("#listCity").jqxListBox({ source: cityAdapter });
                $("#City").val("");
            }
        });
        // perform Data Binding.
        //placeAdapter.dataBind();

        //$("#listBranch").jqxListBox('insertAt', "בחר הכל", 1);
        //$("#listCharge").jqxListBox('insertAt', { label: "בחר הכל", value: "-1" }, 1);
        //$("#listCity").jqxListBox('insertAt', { label: "בחר הכל", value: "-1" }, 1);
        //$("#listPlace").jqxListBox('insertAt', { label: "בחר הכל", value: "-1" }, 1);


        //$('#submit').on('click', function () {
        //    var action = $("#form").find('input[name=op]:checked').val();

        //    $("#form").attr('action', app.appPath() + action);
        //});


        $('#submitCancel').on('click', function () {
            window.parent.triggerWizControlCancel("member_query");
        });
        $('#submitScreen').on('click', function () {

            $("#QueryType").val(0);

            if (_slf.IsDialog) {
                var deataModel = app.serializeToJsonObject("#form");// $("#form").serialize();

                deataModel.JoinedFrom = 0;
                deataModel.JoinedTo = 0;

                var monthlyTime = deataModel.monthlyTime;
                if (monthlyTime && monthlyTime > 0) {
                    deataModel.JoinedFrom = monthlyTime;
                }
                var allCell = deataModel.allCell == "on";
                var allEmail = deataModel.allEmail == "on";

                if (allCell && allEmail)
                    deataModel.ContactRule = 3;
                if (allCell)
                    deataModel.ContactRule = 1;
                if (allEmail)
                    deataModel.ContactRule = 2;
                else
                    deataModel.ContactRule = 0;

                deataModel.BirthdayMonth = 0;
                var allBirthday = deataModel.allBirthday == "on";
                if (allBirthday)
                    deataModel.BirthdayMonth = DateTime.Now.Month;


                window.parent.triggerWizControlQuery(deataModel);
            }
            else {
                var action = '/Co/Members';
                $("#form").attr('action', app.appPath() + action);
                $("#form").submit();
            }
        });
        $('#submitExport').on('click', function () {
            var action = '/Co/MembersExport';
            $("#QueryType").val(20);
            $("#form").attr('action', app.appPath() + action);
            $("#form").submit();
        });

        $("input[name=op]:radio").change(function () {
            //$("#rdDefault, #rdSms", "#rdMail").change(function () {
            var action = $("#form").find('input[name=op]:checked').val();
            $("#form").attr('action', app.appPath() + action);
        });
        $('#reset').on('click', function () {
            location.reload();
        });

        $("#allCity").click(function () {

            var items = $("#listCity").jqxListBox('getItems');
            jQuery.each(items, function (i, item) {
                $("#listCity").jqxListBox('selectItem', item);
            });
            app_jqxcombos.listBoxToInput("listCity", "City");
        });
        $("#unselectAllCity").click(function () {
            $("#listCity").jqxListBox('clearSelection');
            $("#City").val("");
        });


        // sginup and payment
        $("#allItems").prop('checked', true);
        $("#allCampaign").prop('checked', true);

        $('#SignupDateFrom').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        $('#SignupDateTo').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });

        $('#SignupDateFrom').val('');
        $('#SignupDateTo').val('');

        var itemsAdapter = app_jqxcombos.createListAdapter("PropId", "PropName", "listItems", '/Common/GetPriceListView', 200, 100, true);
        $('#listItems').jqxListBox({ multiple: true, rtl: true });
        $('#listItems').on('change', function (event) {
            app_jqxcombos.listBoxToInput("listItems", "Items", "allItems");
        });

        var campaignAdapter = app_jqxcombos.createComboAdapter("PropId", "PropName", "Campaign", '/Common/GetCampaignView', 200, 120, false);
        $('#Campaign').on('change', function (event) {
            $("#allCampaign").prop('checked', false);
        });

        $("#HasSignup-group").jqxButtonGroup({ mode: 'radio', rtl: true });
        $("#HasPayment-group").jqxButtonGroup({ mode: 'radio', rtl: true });
        $("#HasAutoCharge-group").jqxButtonGroup({ mode: 'radio', rtl: true });

        $("#HasSignup-group").on('buttonclick', function (event) {
            var val = $(event.args.button).data('val');
            $("#HasSignup").val(val);
        });

        $("#HasPayment-group").on('buttonclick', function (event) {
            var val = $(event.args.button).data('val');
            $("#HasPayment").val(val);
        });

        $("#HasAutoCharge-group").on('buttonclick', function (event) {
            var val = $(event.args.button).data('val');
            $("#HasAutoCharge").val(val);
        });
    }

}
*/


function members_usersRefresh() {
    members_grid.usersRefresh();
};
function members_usersDelete(id, rcdid) {
    members_grid.usersDelete(id, rcdid);
}

function triggerusersComplete(memid) {
    members_usersRefresh();
    app_dialog.dialogClose(members_grid.NContainer.usersDialog);
};

function triggerWizControlCompleted(control_name, OutputId) {

    $("#jqxgrid").jqxGrid('source').dataBind();
    //app_dialog.dialogIframClose();
    wizard.wizHome();
}
function triggerWizControlCancel(control_name) {
    wizard.wizHome();
}
function triggerWizControlQuery(dataModel) {
    //var Model = JSON.parse(dataModel);
    members_grid.loadQuery(dataModel)
    wizard.wizHome();
}

