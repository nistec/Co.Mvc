'use strict';

class app_members_base {

    triggerMembersRefresh() {

        $("#jqxgrid").jqxGrid('source').dataBind();
    }

    members_categoriesRefresh() {
        members_grid.categoriesRefresh();
    }

    members_categoriesDelete(id, rcdid) {
        members_grid.categoriesDelete(id, rcdid);
    }

    triggerCategoriesComplete(memid) {
        members_categoriesRefresh();
        app_dialog.dialogClose(members_grid.NContainer.categoriesDialog);
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

}

//============================================================================================ app_members_grid

class app_members_grid {

    constructor() {
        //accType: 0,
        this.NContainer = {};
        this.DataAdapter = {};
        this.AllowEdit = 0;
        this.IsMemberQuery = 0;
        //CurrentId:0,
        this.ExType = 0;
        this.UInfo = null;
        this.Control = null;
        //NContainer.nastedGrids = new Array();
    }

    init(Model, userInfo) {

        var _slf = this;

        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();

        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.ExType = Model.ExType;

        this.DataSource =
            {
                datatype: "json",
                async: false,
                datafields: [
                    { name: 'MemberId', type: 'string' },
                    { name: 'MemberName', type: 'string' },
                    { name: 'Address', type: 'string' },
                    { name: 'CityName', type: 'string' },
                    { name: 'JoiningDate', type: 'date' },
                    { name: 'CompanyName', type: 'string' },
                    { name: 'BirthDateYear', type: 'number' },
                    // { name: 'ChargeName', type: 'string' },
                    { name: 'BranchName', type: 'string' },
                    { name: 'CellPhone', type: 'string' },
                    { name: 'Phone', type: 'string' },
                    { name: 'Email', type: 'string' },
                    //{ name: 'StatusName', type: 'string' },
                    { name: 'LastUpdate', type: 'date' },
                    { name: 'RegionName', type: 'string' },
                    { name: 'GenderName', type: 'string' },
                    { name: 'Note', type: 'string' },
                    { name: 'Birthday', type: 'string' },
                    { name: 'RecordId', type: 'number' },
                    { name: 'TotalRows', type: 'number' }
                ],
                id: 'RecordId',
                type: 'POST',
                url: '/Co/GetMembersGrid',
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
            'MemberId': Model.MemberId,
            'ExId': Model.ExId,
            'ExType': Model.ExType,
            'CellPhone': Model.CellPhone,
            'Email': Model.Email,
            'Name': Model.Name,
            'Address': Model.Address,
            'City': Model.City,
            'Category': Model.Category,
            'Region': Model.Region,
            'Branch': Model.Branch,
            'ExEnum1': Model.ExEnum1,
            'ExEnum2': Model.ExEnum2,
            'ExEnum3': Model.ExEnum3,
            'BirthdayMonth': Model.BirthdayMonth,
            'JoinedFrom': Model.JoinedFrom,
            'JoinedTo': Model.JoinedTo,
            'AgeFrom': Model.AgeFrom,
            'AgeTo': Model.AgeTo,
            'ContactRule': Model.ContactRule,
            'Items': Model.Items,
            'ReferralCode': Model.ReferralCode,
            'ValidityRemain': Model.ValidityRemain,
            'Campaign': Model.Campaign,
            'SignupDateFrom': app.formatDateString(Model.SignupDateFrom),
            'SignupDateTo': app.formatDateString(Model.SignupDateTo),
            'PriceFrom': Model.PriceFrom,
            'PriceTo': Model.PriceTo,
            'HasAutoCharge': Model.HasAutoCharge,
            'HasPayment': Model.HasPayment,
            'HasSignup': Model.HasSignup
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

        var initSginupGrid = function (tab, index, id) {

            var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
            nastedcontainer.rtl = true;

            var nastedsource = {
                datafields: [
                    { name: 'SignupId', type: 'number' },
                    { name: 'SignupDate', type: 'date' },
                    { name: 'ReferralCode', type: 'string' },
                    { name: 'AutoCharge', type: 'bool' },
                    { name: 'RegHostAddress', type: 'string' },
                    { name: 'RegReferrer', type: 'string' },
                    { name: 'CreditCardOwner', type: 'string' },
                    { name: 'ConfirmAgreement', type: 'boll' },
                    { name: 'SignKey', type: 'string' },
                    { name: 'ValidityMonth', type: 'number' },
                    { name: 'Campaign', type: 'number' },
                    { name: 'Price', type: 'number' },
                    { name: 'ItemId', type: 'number' },
                    //{ name: 'PayId', type: 'number' },
                    { name: 'ItemName', type: 'string' },
                    { name: 'CampaignName', type: 'string' },
                    { name: 'SignupOrder', type: 'number' },
                    { name: 'PayId', type: 'number' },
                    { name: 'Payed', type: 'number' },
                    { name: 'PayedDate', type: 'date' },
                    { name: 'PaymentOwner', type: 'bool' },
                    { name: 'ExpirationDate', type: 'date' }
                ],
                datatype: "json",
                //id: 'SignupId',
                type: 'POST',
                url: '/Co/GetMemberSignupHistory',
                data: { 'rcdid': id }
            }
            _slf.NContainer.SginupGrid[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                localization: getLocalization('he'),
                source: nastedAdapter, width: '99%', height: 130,
                columnsresize: true,
                rtl: true,
                columns: [
                    {
                        text: 'רישום', dataField: 'SignupId', width: 120, cellsalign: 'right', align: 'center',
                        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                            var record = "'" + $.param(nastedAdapter.records[row]) + "'";
                            var linkinfo = "<a class=\"signup-info btn-default btn7\" href=\"#\" onclick=\"members_grid.show_payment(" + record + ");\">" + value + "</a>";
                            return '<div style="text-align:center;direction:rtl;margin:5px;">' + linkinfo + '</div>'
                        }
                        //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                        //    var memid = id;//  $('#jqxgrid').jqxGrid('getrowdata', row).AccountId;
                        //    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">הצג</a>|<a href="#">הצג</a></div>'
                        //}
                    },
                    { text: 'מועד רישום', datafield: 'SignupDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                    { text: 'תוקף', datafield: 'ValidityMonth', cellsalign: 'right', align: 'center' },
                    { text: 'תפוגה', datafield: 'ExpirationDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                    { text: 'המשלם', datafield: 'CreditCardOwner', cellsalign: 'right', align: 'center' },
                    { text: 'חיוב חוזר', datafield: 'AutoCharge', columntype: 'checkbox', width: 100, cellsalign: 'right', align: 'center' },
                    { text: 'קמפיין', datafield: 'CampaignName', cellsalign: 'right', align: 'center' },
                    { text: 'מחירון', datafield: 'ItemName', cellsalign: 'right', align: 'center' },
                    { text: 'מחיר', datafield: 'Price', cellsalign: 'right', align: 'center' },
                    { text: 'משלם\ת', datafield: 'PaymentOwner', columntype: 'checkbox', width: 100, cellsalign: 'right', align: 'center' },
                    {
                        text: 'קבלה', datafield: 'PayId', width: 120, cellsalign: 'right', align: 'center'
                        //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                        //    var record = "'" + $.param(nastedAdapter.records[row]) + "'";
                        //    var linkinfo = "<a class=\"signup-info\" href=\"#\" onclick=\"members_grid.show_payment(" + record + ");\">" + value + "</a>";
                        //    return '<div style="text-align:center;direction:rtl;margin:5px;">'+linkinfo+'</div>'
                        //  }
                    }
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

        var initCategoriesGrid = function (tab, index, id) {

            var nastedcontainer = $('<div style="float:right;margin:5px"></div>');
            nastedcontainer.rtl = true;
            var nastedsource = {
                datafields: [
                    { name: 'PropId', type: 'number' },
                    { name: 'PropName', type: 'string' },
                ],
                datatype: "json",
                id: 'PropId',
                type: 'POST',
                url: '/Co/GetMemberCategories',
                data: { 'rcdid': id }
            }
            _slf.NContainer.CategoriesGrid[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                source: nastedAdapter, width: '70%', height: 130, showheader: false,
                rtl: true,
                columns: [
                    {
                        text: 'פעולות', dataField: 'PropId', width: 100, cellsalign: 'right', align: 'center',
                        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                            //var memid = id;//  $('#jqxgrid').jqxGrid('getrowdata', row).AccountId;
                            return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="members_categoriesDelete(' + value + ',' + id + ');" >הסר</a></div>'
                        }
                    },
                    { text: 'סיווג', datafield: 'PropName', cellsalign: 'right', align: 'center' },
                ]
            });
            var additem = $('<a title="הוספת סיווג" href="#" style="padding-left:10px;padding-right:10px;float:right;">הוסף</a>')
                .click(function () {
                    _slf.NContainer.categoriesDialog = app_dialog.dialogIframe(app.appPath() + "/Common/_MemberCategories?id=" + id, "580", "400", "סיווגים");
                });
            var refreshitem = $('<a title="רענון סיווגים"  href="#" style="padding-left:10px;float:right;">רענן</a>')
                .click(function () {
                    nastedAdapter.dataBind();
                });

            $(tab).append(additem);
            $(tab).append(refreshitem);
            $(tab).append(nastedcontainer);

        };

        var initrowdetails = function (index, parentElement, gridElement, datarecord) {

            _slf.NContainer.currentIndex = index;

            var tabsdiv = null;
            var information = null;
            var notes = null;

            tabsdiv = $($(parentElement).children()[0]);
            if (tabsdiv != null) {
                information = tabsdiv.find('.information');
                notes = tabsdiv.find('.notes');
                var tabcategories = tabsdiv.find('.categories');
                var tabhistory = tabsdiv.find('.history');
                var tabaction = tabsdiv.find('.action');

                var memid = datarecord.MemberId;//parseInt(datarecord.MemberId);
                var rcdid = datarecord.RecordId;//parseInt(datarecord.MemberId);

                var title = tabsdiv.find('.title');
                title.html('<span style="margin:10px"> ' + datarecord.MemberName + ' </span><a title="עריכת מנוי" href="javascript:members_grid.edit(' + rcdid + ');" >@</a>');
                //<a title="הסר מנוי" href="javascript:members_grid.remove(' + rcdid + ');" >X</a><span>  </span>

                var container = $('<div style="margin: 5px;text-align:right;"></div>')
                container.rtl = true;
                container.appendTo($(information));

                var leftcolumn = $('<div style="float: left; width: 45%;direction:rtl;"></div>');
                var rightcolumn = $('<div style="float: right; width: 40%;direction:rtl;"></div>');
                container.append(leftcolumn);
                container.append(rightcolumn);

                var divLeft = $(//"<div style='margin: 10px;'><b>קוד חיוב:</b> " + datarecord.ChargeName + "</div>" +
                    "<div style='margin: 10px;'><b>מחוז:</b> " + datarecord.RegionName + "</div>" +
                    "<div style='margin: 10px;'><b>סניף:</b> " + datarecord.BranchName + "</div>" +
                    "<div style='margin: 10px;'><b>מועד עדכון:</b> " + datarecord.LastUpdate.toLocaleDateString() + "</div>");

                divLeft.rtl = true;
                var divRight = $("<div style='margin: 10px;'><b>דואל:</b> " + datarecord.Email + "</div>" +
                    "<div style='margin: 10px;'><b>טלפון:</b> " + datarecord.Phone + "</div>" +
                    //"<div style='margin: 10px;'><b>ארץ לידה:</b> " + datarecord.PlaceName + "</div>" +
                    "<div style='margin: 10px;'><b>תאריך לידה:</b> " + datarecord.Birthday + "</div>" +
                    "<div style='margin: 10px;'><b>מגדר:</b> " + datarecord.GenderName + "</div>" +
                    "<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.MemberId + "</div>");


                divRight.rtl = true;
                $(leftcolumn).append(divLeft);
                $(rightcolumn).append(divRight);

                var notescontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"><span>' + datarecord.Note + '</span></div>');
                notescontainer.rtl = true;
                $(notes).append(notescontainer);


                //var actioncontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"></div>');
                //actioncontainer.rtl = true;
                //$(tabaction).append(actioncontainer);
                //var edititem = $('<div style="margin:10px"><a title="הצג" href="#" >עריכת מנוי</a></div>')
                // .click(function () {
                //     _slf.selectRow(index);
                //     _slf.edit();//.memberEdit(rcdid);
                // });

                //var deleteitem = $('<div style="margin:10px"><a title="הסר" href="#" >הסר מנוי</a></div>')
                // .click(function () {
                //     _slf.selectRow(index);
                //     _slf.remove(rcdid);// app_members_grid.memberDelete(rcdid);
                // });

                //actioncontainer.append(edititem);
                //actioncontainer.append(deleteitem);


                initCategoriesGrid(tabcategories, index, rcdid);
                initSginupGrid(tabhistory, index, rcdid);

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

            //if (_slf.AllowEdit == 1) {
            //    container.append(deleteButton);
            //}
            //container.append(editButton);
            //container.append(addButton);

            statusbar.append(container);
            //addButton.jqxButton({ width: 70, height: 20 });
            //editButton.jqxButton({ width: 70, height: 20 });
            //deleteButton.jqxButton({ width: 70, height: 20 });

            reloadButton.jqxButton({ width: 70, height: 20 });
            clearFilterButton.jqxButton({ width: 70, height: 20 });
            queryButton.jqxButton({ width: 70, height: 20 });
            //searchButton.jqxButton({ width: 50, height: 20 });
            // add new row.
            addButton.click(function (event) {
                //app_popup.memberEdit(0);
                _slf.add();
            });
            editButton.click(function (event) {
                //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                //if (selectedrowindex < 0)
                //    return;
                //var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                //app_popup.memberEdit(id);
                _slf.edit();
            });
            // delete selected row.
            deleteButton.click(function (event) {
                //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                //if (selectedrowindex < 0)
                //    return;
                //var rcdid = $('#jqxgrid').jqxGrid('getrowdata', selectedrowindex).RecordId;
                _slf.remove();//app_members_grid.memberDelete(rcdid);

            });
            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
            queryButton.click(function (event) {
                app.redirectTo('/Co/MembersQuery');
            });

            // search for a record.
            //searchButton.click(function (event) {
            //    var offset = $("#jqxgrid").offset();
            //    $("#jqxwindow").jqxWindow('open');
            //    $("#jqxwindow").jqxWindow('move', offset.left + 30, offset.top + 30);
            //});
        };

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
            {
                width: '100%',
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
                //showfilterrow: true,
                //filterable: true,
                rowdetails: true,
                rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li class='title'></li><li>הערות</li><li>סיווגים</li><li>היסטוריה</li><li>אפשרויות</li></ul><div class='information'></div><div class='notes'></div><div class='categories'></div><div class='history'></div><div class='action'></div></div>", rowdetailsheight: 200 },
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
                    //{
                    //    text: 'מס.סידורי', dataField: 'RecordId', filterable: false, width: 100, cellsalign: 'right', align: 'center'
                    //    //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    //     //    var link = '<div style="text-align:center"><a href="#" onclick="memberEdit(' + value + ')" >הצג</a>';
                    //     //    if (_slf.AllowEdit == 1) {
                    //     //        var mid = $('#jqxgrid').jqxGrid('getrowdata', row).MemberId;
                    //     //        return link + ' | <a href="#" onclick="memberDelete(' + mid + ')" >הסר</a>';
                    //     //    }
                    //     //    else
                    //     //        return link + '</div>';
                    //     //    //return '<div style="text-align:center"><a href="#" onclick="memberEdit(' + value + ')" >הצג</a></div>';
                    //     //}
                    //},

                    {
                        text: 'ת.ז', dataField: 'MemberId', width: 100, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'שם מלא', dataField: 'MemberName', width: 160, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'שם חברה', dataField: 'CompanyName', width: 160, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: ' עיר   ', dataField: 'CityName', cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: ' כתובת ', dataField: 'Address', cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'טלפון נייד', dataField: 'CellPhone', cellsalign: 'right', align: 'center',
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
                    //{
                    //    text: 'סטאטוס', dataField: 'StatusName', width: 100, cellsalign: 'right', align: 'center',
                    //    filtertype: "custom",
                    //    createfilterpanel: function (datafield, filterPanel) {
                    //        _slf.buildFilterPanel(filterPanel, datafield);
                    //    }
                    //},
                    {
                        text: 'מועד הצטרפות', type: 'date', dataField: 'JoiningDate', cellsformat: 'd', cellsalign: 'right', align: 'center',
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
    }

    getTotalRows(data) {
        if (data) {
            return dataTotalRows(data);
        }
        return 0;
    }

    categoriesRefresh() {
        try {
            var i = this.NContainer.currentIndex;
            var g = this.NContainer.CategoriesGrid[i];
            g.jqxGrid('source').dataBind();
        }
        catch (e) {
            app_dialog.alert(e);
        }
    }

    categoriesDelete(id, rcdid) {
        //accountNewsRemove(id, memid);
        var _slf = this;
        if (confirm("האם להסיר מנוי " + rcdid + " מקבוצת סיווג " + id)) {
            $.ajax({
                type: "POST",
                url: '/Co/DeleteMemberCategories',
                data: { 'rcdid': rcdid, 'propId': id },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    _slf.categoriesRefresh();
                    if (data.Status > 0)
                        app_dialog.alert('מנוי ' + rcdid + ' הוסר מקבוצת סייוג ' + id);
                    else
                        app_dialog.alert(data.Message);
                },
                error: function (e) {
                    app_dialog.alert(e);
                }
            });
        }
        this.categoriesRefresh();
    }

    loadQuery(Model) {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsMemberQuery = 1;
        //this.ExType = Model.ExType;

        this.DataSource.data = {
            'QueryType': Model.QueryType,
            'AccountId': Model.AccountId,
            'MemberId': Model.MemberId,
            'ExId': Model.ExId,
            //'ExType': Model.ExType,
            'CellPhone': Model.CellPhone,
            'Email': Model.Email,
            'Name': Model.Name,
            'Address': Model.Address,
            'City': Model.City,
            'Category': Model.Category,
            'Region': Model.Region,
            'Branch': Model.Branch,
            'ExEnum1': Model.ExEnum1,
            'ExEnum2': Model.ExEnum2,
            'ExEnum3': Model.ExEnum3,
            'BirthdayMonth': Model.BirthdayMonth,
            'JoinedFrom': Model.JoinedFrom,
            'JoinedTo': Model.JoinedTo,
            'AgeFrom': Model.AgeFrom,
            'AgeTo': Model.AgeTo,
            'ContactRule': Model.ContactRule,
            'Items': Model.Items,
            'ReferralCode': Model.ReferralCode,
            'ValidityRemain': Model.ValidityRemain,
            'Campaign': Model.Campaign,
            'SignupDateFrom': app.formatDateString(Model.SignupDateFrom),
            'SignupDateTo': app.formatDateString(Model.SignupDateTo),
            'PriceFrom': Model.PriceFrom,
            'PriceTo': Model.PriceTo,
            'HasAutoCharge': Model.HasAutoCharge,
            'HasPayment': Model.HasPayment,
            'HasSignup': Model.HasSignup
        };

        this.DataAdapter.dataBind();

        wizard.displayStep(1);
    }

    cancelQuery() {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsMemberQuery = 0;
        var accid = this.source.data.AccountId;
        var extype = this.source.data.ExType;
        this.ExType = extype;

        this.DataSource.data = {
            'QueryType': 0,
            'AccountId': accid,
            'MemberId': "",
            'ExId': "",
            'ExType': extype,
            'CellPhone': "",
            'Email': "",
            'Name': "",
            'Address': "",
            'City': "",
            'Category': "",
            'Region': "",
            'Branch': "",
            'ExEnum1': "",
            'ExEnum2': "",
            'ExEnum3': "",
            'BirthdayMonth': "",
            'JoinedFrom': "",
            'JoinedTo': "",
            'AgeFrom': "",
            'AgeTo': "",
            'ContactRule': "",
            'Items': "",
            'ReferralCode': "",
            'ValidityRemain': "",
            'Campaign': "",
            'SignupDateFrom': "",
            'SignupDateTo': "",
            'PriceFrom': "",
            'PriceTo': "",
            'HasAutoCharge': "",
            'HasPayment': "",
            'HasSignup': ""
        };

        this.DataAdapter.dataBind();

        wizard.displayStep(1);
    }

    showControl(id, option, action) {

        var data_model = { Id: id, Option: option, Action: action, Inline: true };

        if (this.Control == null) {
            this.Control = new model_member("#Members");
        }
        this.Control.init(data_model, this.UInfo, this.ExType);
        this.Control.display();

        //if (this.Control == null) {
        //    this.Control = new app_members_def_control("#divPartial2");
        //   // this.Control = control;
        //}
        //this.Control.init(data_model, this.UInfo, this.ExType);
        //this.Control.display();
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
            wizard.appendIframe(3, app.appPath() + "/Co/_MembersQuery", "100%", "680px", true, "#loader");
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
            this.showControl(id, 'e');
            //wizard.appendIframe(2, app.appPath() + "/Co/_MemberEdit?id=" + id, "100%", "620px", true, "#loader");
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
        app_jqxcombos.renderCheckList("listCategory", "Categories");
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
                app_query.doPost(app.appPath() + "/Co/MemberDelete", { 'RecordId': id }, this.end_internal);
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

//============================================================================================ app_members_def_control

class app_members_def_control {

    constructor(tagWindow) {

        this.TagWindow = tagWindow;
    }

    init(dataModel, userInfo, extype) {

        this.wizControl;
        this.dataSource;

        this.RecordId = dataModel.Id;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.ExType = extype;
        //this.UserRole = userInfo.UserRole;
        //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.dataSource =
            {
                datatype: "json",
                id: 'RecordId',
                data: { 'id': this.RecordId },
                type: 'POST',
                url: '/Co/GetMemberEdit'
            };
        var pasive = dataModel.Option == "a" ? " pasive" : "";
        var html =
            '<div id="fcWindow">' +
            '<div id="fcBody">' +
            '<form class="fcForm" id="fcForm" method="post" action="/Co/MemberUpdate">' +
            '<div style="direction: rtl; text-align: right;">' +
            '<input type="hidden" id="ExType" />' +
            '<input type="hidden" id="RecordId" name="RecordId" value="0" />' +
            '<input type="hidden" id="AccountId" name="AccountId" value="' + userInfo.AccountId + '" />' +
            '<input type="hidden" id="Categories" name="Categories" value="" />' +
            '<div class="tab-container">' +
            '<div id="tab-personal" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים אישיים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">תעודת זהות :</div>' +
            '<input id="MemberId" name="MemberId" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם פרטי :</div>' +
            '<input id="FirstName" name="FirstName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם משפחה :</div>' +
            '<input id="LastName" name="LastName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">כתובת :</div>' +
            '<input id="Address" name="Address" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">עיר :</div>' +
            '<div id="City" name="City"></div>' +
            '</div>' +
            '<br/>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון נייד:</div>' +
            '<input id="CellPhone" name="CellPhone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון:</div>' +
            '<input id="Phone" name="Phone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">דאר אלקטרוני:</div>' +
            '<input id="Email" name="Email" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">מגדר:</div>' +
            '<div id="Gender" name="Gender"></div>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">תאריך לידה:</div>' +
            '<div id="Birthday" name="Birthday" data-type="date"></div>' +
            '</div>' +
            '</div>' +
            '<div id="tab-general" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים כלליים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">חברה\ארגון :</div>' +
            '<input id="CompanyName" name="CompanyName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">סניף :</div>' +
            '<div id="Branch" name="Branch"></div>' +
            '</div>' +
            '<br/>' +
            '<div class="form-group-inline">' +
            '<div class="field">סווג :</div>' +
            '<div id="listCategory" name="listCategory" style="padding: 10px" data-type="checklist"></div>' +
            '</div>' +
            '<br/>' +
            '<div id="divExId" class="form-group-inline field-ex">' +
            '<div id="lblExId" class="field">' +
            '</div>' +
            '<input type="text" id="ExId" name="ExId" class="text-mid" maxlength="50" />' +
            '</div>' +
            '<br/>' +
            '<div id="divExEnum1" class="form-group-inline field-ex">' +
            '<div id="lblExEnum1" class="field">' +
            '</div>' +
            '<div id="ExEnum1" name="ExEnum1"></div>' +
            '</div>' +
            '<div id="divExEnum2" class="form-group-inline field-ex">' +
            '<div id="lblExEnum2" class="field">' +
            '</div>' +
            '<div id="ExEnum2" name="ExEnum2"></div>' +
            '</div>' +
            '<div id="divExEnum3" class="form-group-inline field-ex">' +
            '<div id="lblExEnum3" class="field">' +
            '</div>' +
            '<div id="ExEnum3" name="ExEnum3"></div>' +
            '</div>' +
            '<br/>' +
            '<div id="divExField1" class="form-group-inline field-ex">' +
            '<div id="lblExField1" class="field">' +
            '</div>' +
            '<input type="text" id="ExField1" name="ExField1" class="text-mid" maxlength="250" />' +
            '</div>' +
            '<div id="divExField2" class="form-group-inline field-ex">' +
            '<div id="lblExField2" class="field">' +
            '</div>' +
            '<input type="text" id="ExField2" name="ExField2" class="text-mid" maxlength="250" />' +
            '</div>' +
            '<div id="divExField3" class="form-group-inline field-ex">' +
            '<div id="lblExField3" class="field">' +
            '</div>' +
            '<input type="text" id="ExField3" name="ExField3" class="text-mid" maxlength="250" />' +
            '</div>' +
            '<br/>' +
            '<div id="divExRef1" class="form-group-inline field-ex">' +
            '<div id="lblExRef1" class="field">' +
            '</div>' +
            '<input type="number" id="ExRef1" name="ExRef1" />' +
            '</div>' +
            '<div id="divExRef2" class="form-group-inline field-ex">' +
            '<div id="lblExRef2" class="field">' +
            '</div>' +
            '<input type="number" id="ExRef2" name="ExRef2" />' +
            '</div>' +
            '<div id="divExRef3" class="form-group-inline field-ex">' +
            '<div id="lblExRef3" class="field">' +
            '</div>' +
            '<input type="number" id="ExRef3" name="ExRef3" />' +
            '</div>' +
            '</div>' +
            '<div id="tab-notes" class="tab-group-1">' +
            '<h3 class="panel-area-title">הערות</h3>' +
            '<div class="form-group">' +
            '<div class="field">הערות:</div>' +
            '<textarea id="Note" name="Note" style="width:99%;height:60px"></textarea>' +
            '</div>' +
            '<div class="form-group-inline ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="JoiningDate" name="JoiningDate" type="text" class="text-mid" readonly="readonly" />' +
            '</div>' +
            '<div class="form-group-inline ' + pasive + '">' +
            '<div class="field">מועד עדכון:</div>' +
            '<input id="LastUpdate" name="LastUpdate" type="text" readonly="readonly" class="text-mid" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<!--<div style="clear: both;"></div>-->' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>' +
            '</div>';

        //    '<div style="height: 5px"></div>' +
        //'<p id="validator-message" style="color:red"></p>' +
        //'<div style="display:none">' +
        //'<input id="fcSubmit" class="btn-default btn7" type="button" value="עדכון" />' +
        //'<input id="fcCancel" class="btn-default btn7" type="button" value="ניקוי" />' +
        //'</div>' +

        if (this.wizControl == null) {
            this.wizControl = new wiz_control("member_def", this.TagWindow);
            this.wizControl.init(html, this.ExType, function (data) {

                $('#Birthday').val('');
                $('#Birthday').jqxDateTimeInput({ width: '150px', rtl: true });


                app_jqx_list.enum1ComboAdapter();
                app_jqx_list.enum2ComboAdapter();
                app_jqx_list.enum3ComboAdapter();
                app_members.displayMemberFields();

                app_jqx_list.branchComboAdapter();
                app_jqx_list.cityComboAdapter();
                app_jqx_list.genderComboAdapter();
                app_jqx_list.categoryCheckListAdapter();
                //app_jqx_list.chargeComboAdapter('ChargeType');


                var exType = data;// $("#ExType").val();

                var input_rules = [
                    { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
                    { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
                    //{ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
                    //{
                    //    input: "#City", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
                    //        var index = $("#City").jqxComboBox('getSelectedIndex');
                    //       return index != -1;
                    //    }
                    //},
                    { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
                    {
                        input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
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
                    input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
                if (exType == 1)
                    input_rules.push({ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
                if (exType == 2)
                    input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });
                if (exType == 3)
                    input_rules.push({ input: '#Exid', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });

                $('#fcForm').jqxValidator({
                    rtl: true,
                    hintType: 'label',
                    animationDuration: 0,
                    rules: input_rules
                });
                $('#fcForm').jqxValidator('hide');
            });
        }
        else {
            this.wizControl.clearDataForm("fcForm");
            app_jqxcombos.clearCheckList("#listCategory");
        }

    }

    display() {
        $(this.TagWindow).show();
        $("#ExType").val(this.ExType);

        if (this.RecordId > 0) {
            this.wizControl.load("fcForm", this.dataSource, function (record) {

                app_jqxform.loadDataForm("fcForm", record);//, true, ["Birthday"]);

                //if (record.Birthday)
                //    $("#Birthday").val(record.Birthday)

                app_jqxcombos.selectCheckList("listCategory", record.Categories);

                app_jqxcombos.initComboValue('City', 0);

            });
        }
        else {
            $("#AccountId").val(this.AccountId);
            $("#RecordId").val(0);
        }
    }

    doCancel() {
        this.wizControl.doCancel();
    }

    doSubmit() {
        this.wizControl.doSubmit(
            function () {
                app_jqxcombos.renderCheckList("listCategory", "Categories");
            },
            function (data) {
                app_dialog.alert(data.Message);
                if (data.Status >= 0) {
                    //if (slf.IsDialog) {

                    if (window.parent.triggerWizControlCompleted)
                        window.parent.triggerWizControlCompleted("member_def", data.OutputId);
                    else if (triggerWizControlCompleted)
                        triggerWizControlCompleted("member_def", data.OutputId);

                    //    //$('#fcForm').reset();
                    //}
                    //else {
                    //    app.refresh();
                    //}
                    //$('#RecordId').val(data.OutputId);
                }
            }
        );
    }

    doClear() {
        this.wizControl.clearDataForm("fcForm");
        app_jqxcombos.clearCheckList("#listCategory");
    }

    doSubmitAdd() {
        this.wizControl.doSubmit(
            function () {
                app_jqxcombos.renderCheckList("listCategory", "Categories");
            },
            function (data) {
                app_dialog.alert(data.Message);
                //if (data.Status >= 0) {
                //    window.parent.triggerWizControlCompleted("member_def", data.OutputId);
                //}
            }
        );
    }
}

//============================================================================================ app_members_def

class app_member_def {

    constructor(recordId, userInfo, isdialog) {

        this.RecordId = recordId;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        this.IsDialog = isdialog;

        $("#AccountId").val(this.AccountId);

        this.loadControls();
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
                id: 'RecordId',
                data: { 'id': _slf.RecordId },
                type: 'POST',
                url: '/Co/GetMemberEdit'
            };

        this.viewAdapter = new $.jqx.dataAdapter(view_source, {
            loadComplete: function (record) {

                _slf.syncData(record);
            },
            loadError: function (jqXHR, status, error) {
            },
            beforeLoadComplete: function (records) {
            }
        });

        if (this.RecordId > 0) {
            this.viewAdapter.dataBind();
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

    syncData(record) {

        if (record) {

            app_jqxform.loadDataForm("fcForm", record);

            app_jqxcombos.selectCheckList("listCategory", record.Categories);

            app_jqxcombos.initComboValue('City', 0);
        }
    }

    loadControls() {

        $('#Birthday').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        $('#Birthday').val('');

        app_jqx_list.enum1ComboAdapter();
        app_jqx_list.enum2ComboAdapter();
        app_jqx_list.enum3ComboAdapter();
        app_members.displayMemberFields();

        app_jqx_list.branchComboAdapter();
        //app_jqx_list.placeComboAdapter();
        //app_jqx_list.statusComboAdapter();
        //app_jqx_list.regionComboAdapter();
        app_jqx_list.cityComboAdapter();
        app_jqx_list.genderComboAdapter();
        app_jqx_list.categoryCheckListAdapter();
        //app_jqx_list.chargeComboAdapter('ChargeType');


        var exType = $("#ExType").val();

        var input_rules = [
            { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
            { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
            //{ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
            //{
            //    input: "#City", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
            //        var index = $("#City").jqxComboBox('getSelectedIndex');
            //       return index != -1;
            //    }
            //},
            { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
            {
                input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
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
            input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
        if (exType == 1)
            input_rules.push({ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        if (exType == 2)
            input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });
        if (exType == 3)
            input_rules.push({ input: '#Exid', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });

        $('#fcForm').jqxValidator({
            rtl: true,
            hintType: 'tooltip',
            animationDuration: 0,
            rules: input_rules
        });


    }

}

//============================================================================================ app_members_query

class app_members_query {

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



function members_categoriesRefresh() {
    members_grid.categoriesRefresh();
};
function members_categoriesDelete(id, rcdid) {
    members_grid.categoriesDelete(id, rcdid);
}

function triggerCategoriesComplete(memid) {
    members_categoriesRefresh();
    app_dialog.dialogClose(members_grid.NContainer.categoriesDialog);
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

