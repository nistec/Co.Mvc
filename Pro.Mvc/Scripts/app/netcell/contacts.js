'use strict';

class app_contacts_base {


    contacts_categoriesRefresh() {
        contacts_grid.categoriesRefresh();
    }

    contacts_categoriesDelete(id, rcdid) {
        contacts_grid.categoriesDelete(id, rcdid);
    }

    triggerCategoriesComplete(memid) {
        contacts_categoriesRefresh();
        app_dialog.dialogClose(contacts_grid.NContainer.categoriesDialog);
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
        contacts_grid.loadQuery(dataModel)
        wizard.wizHome();
    }

}

//============================================================================================ app_contacts_grid

class app_contacts_grid {

    constructor() {
        //accType: 0,
        this.NContainer = {};
        this.DataAdapter = {};
        this.AllowEdit = 0;
        this.IsContactQuery = 0;
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
                    { name: 'ContactId', type: 'string' },
                    { name: 'ContactName', type: 'string' },
                    { name: 'Address', type: 'string' },
                    { name: 'City', type: 'string' },
                    { name: 'RegisterDate', type: 'date' },
                    { name: 'CreationDate', type: 'date' },
                    { name: 'Company', type: 'string' },
                    //{ name: 'BirthDateYear', type: 'number' },
                    //{ name: 'BranchName', type: 'string' },
                    { name: 'CellNumber', type: 'string' },
                    { name: 'Phone', type: 'string' },
                    { name: 'Email', type: 'string' },
                    { name: 'LastUpdate', type: 'date' },
                    //{ name: 'RegionName', type: 'string' },
                    { name: 'Sex', type: 'string' },
                    { name: 'Details', type: 'string' },
                    { name: 'BirthDate', type: 'string' },
                    { name: 'TotalRows', type: 'number' }
                ],
            id: 'ContactId',
                type: 'POST',
                url: '/Netcell/GetContactsGrid',
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
            "QueryType": Model.QueryType,
            "AccountId": Model.AccountId,
            "ContactId": Model.ContactId,
            "ExKey": Model.ExKey,
            "CellNumber": Model.CellNumber,
            "Email": Model.Email,
            "Name": Model.Name,
            "Address": Model.Address,
            "City": Model.City,
            "Category": Model.Category,
            "ExText1": Model.ExText1,
            "ExText2": Model.ExText2,
            "ExText3": Model.ExText3,
            "ExDate1": Model.ExDate1,
            "ExDate2": Model.ExDate2,
            "ExDate3": Model.ExDate3,
            "BirthDateMonth": Model.BirthDateMonth,
            "JoinedDateFrom": Model.JoinedDateFrom,
            "JoinedDateTo": Model.JoinedDateTo,
            "AgeFrom": Model.AgeFrom,
            "AgeTo": Model.AgeTo,
            "ContactRule": Model.ContactRule,
            "Sort": Model.Sort,
            "Filter": Model.Filter
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
        //    $("$contact-item-delete").hide();
        //}

        $('#contact-item-update').click(function () {
            //var iframe = wizard.getIframe();
            //if (iframe && iframe.def) {
            //    iframe.def.doSubmit();
            //}
            _slf.update();
        });
        $('#contact-item-update-plus').click(function () {
            //var iframe = wizard.getIframe();
            //if (iframe && iframe.def) {
            //    iframe.def.doSubmit();
            //}
            _slf.update_plus();
        });
        $('#contact-item-cancel').click(function () {
            //wizard.wizHome();
            _slf.cancel();
        });

        $('#contact-item-add').click(function () {
            _slf.add();
        });
        $('#contact-item-edit').click(function () {
            _slf.edit();
        });
        $('#contact-item-delete').click(function () {
            _slf.remove();
        });
        $('#contact-item-filetr-remove').click(function () {
            _slf.clear_filter();
        });
        $('#contact-item-query').click(function () {
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
                url: '/Netcell/GetContactSignupHistory',
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
                            var linkinfo = "<a class=\"signup-info btn-default btn7\" href=\"#\" onclick=\"contacts_grid.show_payment(" + record + ");\">" + value + "</a>";
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
                        //    var linkinfo = "<a class=\"signup-info\" href=\"#\" onclick=\"contacts_grid.show_payment(" + record + ");\">" + value + "</a>";
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
                url: '/Netcell/GetContactCategories',
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
                            return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="contacts_categoriesDelete(' + value + ',' + id + ');" >הסר</a></div>'
                        }
                    },
                    { text: 'סיווג', datafield: 'PropName', cellsalign: 'right', align: 'center' },
                ]
            });
            var additem = $('<a title="הוספת סיווג" href="#" style="padding-left:10px;padding-right:10px;float:right;">הוסף</a>')
                .click(function () {
                    _slf.NContainer.categoriesDialog = app_dialog.dialogIframe(app.appPath() + "/Common/_ContactCategories?id=" + id, "580", "400", "סיווגים");
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

                var memid = datarecord.ContactId;//parseInt(datarecord.ContactId);
                var rcdid = datarecord.ContactId;//parseInt(datarecord.ContactId);

                var title = tabsdiv.find('.title');
                title.html('<span style="margin:10px"> ' + datarecord.ContactName + ' </span><a title="עריכת מנוי" href="javascript:contacts_grid.edit(' + rcdid + ');" >@</a>');
                //<a title="הסר מנוי" href="javascript:contacts_grid.remove(' + rcdid + ');" >X</a><span>  </span>

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
                    "<div style='margin: 10px;'><b>תאריך לידה:</b> " + datarecord.BirthDate + "</div>" +
                    "<div style='margin: 10px;'><b>מגדר:</b> " + datarecord.GenderName + "</div>" +
                    "<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.ContactId + "</div>");


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
                //     _slf.edit();//.contactEdit(rcdid);
                // });

                //var deleteitem = $('<div style="margin:10px"><a title="הסר" href="#" >הסר מנוי</a></div>')
                // .click(function () {
                //     _slf.selectRow(index);
                //     _slf.remove(rcdid);// app_contacts_grid.contactDelete(rcdid);
                // });

                //actioncontainer.append(edititem);
                //actioncontainer.append(deleteitem);


                //initCategoriesGrid(tabcategories, index, rcdid);
                //initSginupGrid(tabhistory, index, rcdid);

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
                //app_popup.contactEdit(0);
                _slf.add();
            });
            editButton.click(function (event) {
                //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                //if (selectedrowindex < 0)
                //    return;
                //var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                //app_popup.contactEdit(id);
                _slf.edit();
            });
            // delete selected row.
            deleteButton.click(function (event) {
                //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                //if (selectedrowindex < 0)
                //    return;
                //var rcdid = $('#jqxgrid').jqxGrid('getrowdata', selectedrowindex).ContactId;
                _slf.remove();//app_contacts_grid.contactDelete(rcdid);

            });
            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
            queryButton.click(function (event) {
                app.redirectTo('/Netcell/ContactsQuery');
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
                    //    text: 'מס.סידורי', dataField: 'ContactId', filterable: false, width: 100, cellsalign: 'right', align: 'center'
                    //    //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    //     //    var link = '<div style="text-align:center"><a href="#" onclick="contactEdit(' + value + ')" >הצג</a>';
                    //     //    if (_slf.AllowEdit == 1) {
                    //     //        var mid = $('#jqxgrid').jqxGrid('getrowdata', row).ContactId;
                    //     //        return link + ' | <a href="#" onclick="contactDelete(' + mid + ')" >הסר</a>';
                    //     //    }
                    //     //    else
                    //     //        return link + '</div>';
                    //     //    //return '<div style="text-align:center"><a href="#" onclick="contactEdit(' + value + ')" >הצג</a></div>';
                    //     //}
                    //},

                    {
                        text: 'ת.ז', dataField: 'ContactId', width: 100, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'שם מלא', dataField: 'ContactName', width: 160, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: 'שם חברה', dataField: 'Company', width: 160, cellsalign: 'right', align: 'center',
                        filtertype: "custom",
                        createfilterpanel: function (datafield, filterPanel) {
                            app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        }
                    },
                    {
                        text: ' עיר   ', dataField: 'City', cellsalign: 'right', align: 'center',
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
                        text: 'טלפון נייד', dataField: 'CellNumber', cellsalign: 'right', align: 'center',
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
                        text: 'מועד הצטרפות', type: 'date', dataField: 'CreatedDate', cellsformat: 'd', cellsalign: 'right', align: 'center',
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
            //app_popup.contactEdit(id);
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
                url: '/Netcell/DeleteContactCategories',
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
        this.IsContactQuery = 1;
        //this.ExType = Model.ExType;

        this.DataSource.data = {
            "QueryType": Model.QueryType,
            "AccountId": Model.AccountId,
            "ContactId": Model.ContactId,
            "ExKey": Model.ExKey,
            "CellNumber": Model.CellNumber,
            "Email": Model.Email,
            "Name": Model.Name,
            "Address": Model.Address,
            "City": Model.City,
            "Category": Model.Category,
            "ExText1": Model.ExText1,
            "ExText2": Model.ExText2,
            "ExText3": Model.ExText3,
            "ExDate1": Model.ExDate1,
            "ExDate2": Model.ExDate2,
            "ExDate3": Model.ExDate3,
            "BirthDateMonth": Model.BirthDateMonth,
            "JoinedDateFrom": Model.JoinedDateFrom,
            "JoinedDateTo": Model.JoinedDateTo,
            "AgeFrom": Model.AgeFrom,
            "AgeTo": Model.AgeTo,
            "ContactRule": Model.ContactRule,
            "Sort": Model.Sort,
            "Filter": Model.Filter
        };

        this.DataAdapter.dataBind();

        wizard.displayStep(1);
    }

    cancelQuery() {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsContactQuery = 0;
        var accid = this.source.data.AccountId;
        var extype = this.source.data.ExType;
        this.ExType = extype;

        this.DataSource.data = {
            'QueryType': 0,
            'AccountId': accid,
            'ContactId': "",
            'ExKey': "",
            'ExType': extype,
            'CellNumber': "",
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
            'BirthDateMonth': "",
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

        var data_model = { Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_contacts_def_control("#divPartial2");
           // this.Control = control;
        }
        this.Control.init(data_model, this.UInfo, this.ExType);
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
        //app.redirectTo('/Netcell/ContactsQuery');
        if (!wizard.existsIframe(3)) {
            wizard.appendIframe(3, app.appPath() + "/Netcell/_ContactsQuery", "100%", "680px", true, "#loader");
        }
        wizard.displayStep(3);
    }

    add() {
        $('#contact-item-update').show();
        $("#contact-item-update-plus").show();
        //wizard.appendIframe(2, app.appPath() + "/Netcell/_ContactAdd", "100%", "620px", true, "#loader");
        wizard.displayStep(2);
        this.showControl(0, 'a');
    }

    edit(id) {
        $("#contact-item-update-plus").hide();
        if (id === undefined) id = this.getrowId();
        if (id > 0) {
            $('#contact-item-update').show();
            wizard.displayStep(2);
            this.showControl(id, 'e');
            //wizard.appendIframe(2, app.appPath() + "/Netcell/_ContactEdit?id=" + id, "100%", "620px", true, "#loader");
        }
    }

    view (id) {
        $("#contact-item-update-plus").hide();
            $('#contact-item-update').hide();
            if (id === undefined) id = this.getrowId();
        if(id > 0) {
            wizard.displayStep(2);
            this.showControl(id, 'g');
            //$('#contact-item-update').hide();
            //wizard.appendIframe(2, app.appPath() + "/Netcell/_ContactEdit?id=" + id, "100%", "620px", true, "#loader");
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
                app_query.doPost(app.appPath() + "/Netcell/ContactDelete", { 'ContactId': id }, this.end_internal);
            }
        }
    }

    refresh() {
        $('#jqxgrid').jqxGrid('source').dataBind();
    }

    clear_filter() {
        if (this.IsContactQuery == 1)
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
            window.parent.contacts_grid.refresh();
        }
    }

    end_plus(data) {
        //wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            wizard.changeIframe(app.appPath() + "/Netcell/_ContactAdd");
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
            //"<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.ContactId + "</div>"
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

//============================================================================================ app_contacts_def_control

class app_contacts_def_control {

    constructor(tagWindow) {

        this.TagWindow = tagWindow;
    }

    init(dataModel, userInfo, extype) {

        this.wizControl=null;
        this.dataSource=null;

        this.ContactId = dataModel.Id;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.ExType = extype;
        //this.UserRole = userInfo.UserRole;
        //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.dataSource =
            {
                datatype: "json",
                id: 'ContactId',
                data: { 'id': this.ContactId },
                type: 'POST',
                url: '/Netcell/GetContactEdit'
            };
        var pasive = dataModel.Option == "a" ? " pasive" : "";
        var html =
            '<div id="fcWindow">' +
            '<div id="fcBody">' +
            '<form class="fcForm" id="fcForm" method="post" action="/Netcell/ContactUpdate">' +
            '<div style="direction: rtl; text-align: right;">' +
            '<input type="hidden" id="ExType" />' +
            '<input type="hidden" id="ContactId" name="ContactId" value="0" />' +
            '<input type="hidden" id="AccountId" name="AccountId" value="' + userInfo.AccountId + '" />' +
            '<input type="hidden" id="Categories" name="Categories" value="" />' +
            '<div class="tab-container">' +

            '<div id="tab-personal" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים אישיים</h3>' +
            //'<div class="form-group-inline">' +
            //'<div class="field">תעודת זהות :</div>' +
            //'<input id="ContactId" name="ContactId" type="text" class="text-mid" />' +
            //'</div>' +
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
            '<input id="City" name="City" type="text" class="text-mid"/>' +
            '</div>' +
            '<br/>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון נייד:</div>' +
            '<input id="CellNumber" name="CellNumber" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון:</div>' +
            '<input id="Phone" name="Phone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">דאר אלקטרוני:</div>' +
            '<input id="Email" name="Email" type="text" class="text-mid" />' +
            '</div>' +
            '<br/>' +

            '<div class="form-group-inline">' +
            '<div class="field">מגדר:</div>' +
            '<select id="Sex" name="Sex">'+
            '<option value="U">לא ידוע</option>' +
            '<option value="M">זכר</option>' +
            '<option value="F">נקבה</option>' +
            '</select >' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">תאריך לידה:</div>' +
            '<div id="BirthDate" name="BirthDate" data-type="date"></div>' +
            '</div>' +
            '</div>' +

            '<div id="tab-general" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים כלליים</h3>' +

            '<div class="form-group-inline">' +
            '<div class="field">חברה\ארגון :</div>' +
            '<input id="Company" name="Company" type="text" class="text-mid" />' +
            '</div>' +
            //'<div class="form-group-inline">' +
            //'<div class="field">סניף :</div>' +
            //'<div id="Branch" name="Branch"></div>' +
            //'</div>' +
            '<br/>' +
            '<div class="form-group-inline">' +
            '<div class="field">סווג :</div>' +
            '<div id="listCategory" name="listCategory" style="padding: 10px" data-type="checklist"></div>' +
            '</div>' +
            
            '<div id="divExKey" class="form-group-inline field-ex">' +
            '<div id="lblExKey" class="field"> מזהה:' +
            '</div>' +
            '<input type="text" id="ExKey" name="ExKey" class="text-mid" maxlength="50" />' +
            '</div>' +

            '<br/>' +

            '<div id="divExText1" class="form-group-inline field-ex">' +
            '<div id="lblExText1" class="field">טקסט 1:' +
            '</div>' +
            '<input type="text" id="ExText1" name="ExText1" />' +
            '</div>' +

            '<div id="divExText2" class="form-group-inline field-ex">' +
            '<div id="lblExText2" class="field">טקסט 2:' +
            '</div>' +
            '<input type="text" id="ExText2" name="ExText2" />' +
            '</div>' +

            '<div id="divExText3" class="form-group-inline field-ex">' +
            '<div id="lblExText3" class="field">טקסט 3:' +
            '</div>' +
            '<input type="text" id="ExText3" name="ExText3" />' +

            '</div>' +
            '<br/>' +

            '<div id="divExDate1" class="form-group-inline field-ex">' +
            '<div id="lblExDate1" class="field">תאריך 1:' +
            '</div>' +
            '<input type="text" id="ExDate1" name="ExDate1" class="text-mid" maxlength="250" />' +
            '</div>' +

            '<div id="divExDate2" class="form-group-inline field-ex">' +
            '<div id="lblExDate2" class="field">תאריך 2:' +
            '</div>' +
            '<input type="text" id="ExDate2" name="ExDate2" class="text-mid" maxlength="250" />' +
            '</div>' +

            '<div id="divExDate3" class="form-group-inline field-ex">' +
            '<div id="lblExDate3" class="field">תאריך 3:' +
            '</div>' +
            '<input type="text" id="ExDate3" name="ExDate3" class="text-mid" maxlength="250" />' +
            '</div>' +
            '<br/>' +
  
            '</div>' +
            '<div id="tab-notes" class="tab-group-1">' +
            '<h3 class="panel-area-title">הערות</h3>' +
            '<div class="form-group">' +
            '<div class="field">הערות:</div>' +
            '<textarea id="Details" name="Details" style="width:99%;height:60px"></textarea>' +
            '</div>' +
            '<div class="form-group-inline ' + pasive + '">' +
            '<div class="field">מועד רישום בעמוד:</div>' +
            '<input id="RegisterDate" name="RegisterDate" type="text" class="text-mid" readonly="readonly" />' +
            '</div>' +
            '<div class="form-group-inline ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="CreationDate" name="CreationDate" type="text" class="text-mid" readonly="readonly" />' +
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
            this.wizControl = new wiz_control("contact_def", this.TagWindow);
            this.wizControl.init(html, this.ExType, function (data) {

                $('#BirthDate').val('');
                $('#BirthDate').jqxDateTimeInput({ width: '150px', rtl: true });


                //app_jqx_list.enum1ComboAdapter();
                //app_jqx_list.enum2ComboAdapter();
                //app_jqx_list.enum3ComboAdapter();
                //app_contacts.displayContactFields();

                //app_jqx_list.branchComboAdapter();
                //app_jqx_list.cityComboAdapter();
                //app_jqx_list.genderComboAdapter();
                //app_jqx_list.categoryCheckListAdapter();
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
                        input: '#CellNumber', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
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
                    input_rules.push({ input: '#ContactId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
                if (exType == 1)
                    input_rules.push({ input: '#CellNumber', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
                if (exType == 2)
                    input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });
                if (exType == 3)
                    input_rules.push({ input: '#ExKey', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });

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

        if (this.ContactId > 0) {
            this.wizControl.load("fcForm", this.dataSource, function (record) {

                app_form.loadDataForm("fcForm", record);//, true, ["BirthDate"]);

                //if (record.BirthDate)
                //    $("#BirthDate").val(record.BirthDate)

                app_jqxcombos.selectCheckList("listCategory", record.Categories);

                app_jqxcombos.initComboValue('City', 0);

            });
        }
        else {
            $("#AccountId").val(this.AccountId);
            $("#ContactId").val(0);
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
                        window.parent.triggerWizControlCompleted("contact_def", data.OutputId);
                    else if (triggerWizControlCompleted)
                        triggerWizControlCompleted("contact_def", data.OutputId);

                    //    //$('#fcForm').reset();
                    //}
                    //else {
                    //    app.refresh();
                    //}
                    //$('#ContactId').val(data.OutputId);
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
                //    window.parent.triggerWizControlCompleted("contact_def", data.OutputId);
                //}
            }
        );
    }
}

//============================================================================================ app_contacts_def

class app_contact_def {

    constructor(ContactId, userInfo, isdialog) {

        this.ContactId = ContactId;
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
                id: 'ContactId',
                data: { 'id': _slf.ContactId },
                type: 'POST',
                url: '/Netcell/GetContactEdit'
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

        if (this.ContactId > 0) {
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
                                window.parent.triggerContactComplete(data.OutputId);
                                //$('#fcForm').reset();
                            }
                            else {
                                app.refresh();
                            }
                            //$('#ContactId').val(data.OutputId);
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

        $('#BirthDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        $('#BirthDate').val('');

        app_jqx_list.enum1ComboAdapter();
        app_jqx_list.enum2ComboAdapter();
        app_jqx_list.enum3ComboAdapter();
        app_contacts.displayContactFields();

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
                input: '#CellNumber', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
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
        //if (exType == 0)
        //    input_rules.push({ input: '#ContactId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
        if (exType == 1)
            input_rules.push({ input: '#CellNumber', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        if (exType == 2)
            input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });
        if (exType == 3)
            input_rules.push({ input: '#ExKey', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });

        $('#fcForm').jqxValidator({
            rtl: true,
            hintType: 'tooltip',
            animationDuration: 0,
            rules: input_rules
        });


    }

}

//============================================================================================ app_contacts_query

class app_contacts_query {

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
        /*
        var branchAdapter = app_jqxcombos.createComboAdapter("PropId", "PropName", "listBranch", '/Common/GetBranchView', 240, 120, true);
        $('#listBranch').on('change', function (event) {
            app_jqxcombos.comboBoxToInput("listBranch", "Branch", "allBranch");
        });


        app_jqx_list.enum1ComboAdapter();
        app_jqx_list.enum2ComboAdapter();
        app_jqx_list.enum3ComboAdapter();
        app_contacts.displayContactFields();

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
                displayContact: 'PropName',
                valueContact: 'PropId'
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
        */

        //$('#submit').on('click', function () {
        //    var action = $("#form").find('input[name=op]:checked').val();

        //    $("#form").attr('action', app.appPath() + action);
        //});


        $('#submitCancel').on('click', function () {
            window.parent.triggerWizControlCancel("contact_query");
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

                deataModel.BirthDateMonth = 0;
                var allBirthDate = deataModel.allBirthDate == "on";
                if (allBirthDate)
                    deataModel.BirthDateMonth = DateTime.Now.Month;


                window.parent.triggerWizControlQuery(deataModel);
            }
            else {
                var action = '/Netcell/Contacts';
                $("#form").attr('action', app.appPath() + action);
                $("#form").submit();
            }
        });
        $('#submitExport').on('click', function () {
            var action = '/Netcell/ContactsExport';
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



function contacts_categoriesRefresh() {
    contacts_grid.categoriesRefresh();
};
function contacts_categoriesDelete(id, rcdid) {
    contacts_grid.categoriesDelete(id, rcdid);
}

function triggerCategoriesComplete(memid) {
    contacts_categoriesRefresh();
    app_dialog.dialogClose(contacts_grid.NContainer.categoriesDialog);
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
    contacts_grid.loadQuery(dataModel)
    wizard.wizHome();
}

