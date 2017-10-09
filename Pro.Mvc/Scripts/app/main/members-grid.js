
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
function triggerWizControlCancel(control_name){
    wizard.wizHome();
}
function triggerWizControlQuery(dataModel) {
    //var Model = JSON.parse(dataModel);
    members_grid.loadQuery(dataModel)
    wizard.wizHome();
}
//============================================================================================ app_members_grid
(function ($) {

var members_grid;

app_members_grid = {

    //accType: 0,
    NContainer: {},
    dataAdapter: {},
    AllowEdit: 0,
    IsMemberQuery:0,
    //CurrentId:0,
    ExType:0,
    UInfo: null,
    Control: null,
    //NContainer.nastedGrids = new Array();
    source:
        {
            datatype: "json",
            async: false,
            datafields: [
               { name: 'MemberId', type: 'string' },
               { name: 'MemberName', type: 'string' },
               { name: 'Address', type: 'string' },
               { name: 'CityName', type: 'string' },
               { name: 'JoiningDate', type: 'date' },
              // { name: 'PlaceName', type: 'string' },
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
            url: '/Main/GetMembersGrid',
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
        },

    getTotalRows: function (data) {
        if (data) {
            return dataTotalRows(data);
        }
        return 0;
    },
    /*
    buildFilterPanel: function (filterPanel, datafield) {
        var textInput = $("<input style='margin:5px;'/>");
        var applyinput = $("<div class='filter' style='height: 25px; margin-left: 20px; margin-top: 7px;'></div>");
        var filterbutton = $('<span tabindex="0" style="padding: 4px 12px; margin-left: 2px;">סינון</span>');
        applyinput.append(filterbutton);
        var filterclearbutton = $('<span tabindex="0" style="padding: 4px 12px; margin-left: 5px;">הסר סינון</span>');
        applyinput.append(filterclearbutton);
        filterPanel.append(textInput);
        filterPanel.append(applyinput);
        filterbutton.jqxButton({ height: 20 });
        filterclearbutton.jqxButton({ height: 20 });

        var column = $("#jqxgrid").jqxGrid('getcolumn', datafield);
        textInput.jqxInput({ placeHolder: "הקלד " + column.text, maxLength: 25, popupZIndex: 9999999, height: 23, width: 155 });
        textInput.keyup(function (event) {
            if (event.keyCode === 13) {
                filterbutton.trigger('click');
            }
        });
        filterbutton.click(function () {
            var filtergroup = new $.jqx.filter();
            var filter_or_operator = 1;
            var filtervalue = textInput.val();
            var filtercondition = 'contains';
            var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
            filtergroup.addfilter(filter_or_operator, filter1);
            // add the filters.
            $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
            // apply the filters.
            $("#jqxgrid").jqxGrid('applyfilters');
            $("#jqxgrid").jqxGrid('closemenu');
        });
        filterbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterbutton.trigger('click');
            }
        });
        filterclearbutton.click(function () {
            $("#jqxgrid").jqxGrid('removefilter', datafield);
            // apply the filters.
            $("#jqxgrid").jqxGrid('applyfilters');
            $("#jqxgrid").jqxGrid('closemenu');
        });
        filterclearbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterclearbutton.trigger('click');
            }
            textInput.val("");
        });
    },
    */
    grid: function () {
        var slf = this;

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
                url: '/Main/GetMemberSignupHistory',
                data: { 'rcdid': id }
            }
            slf.NContainer.SginupGrid[index] = nastedcontainer;

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
                            return '<div style="text-align:center;direction:rtl;margin:5px;">'+linkinfo+'</div>'
                        }
                      //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                      //    var memid = id;//  $('#jqxgrid').jqxGrid('getrowdata', row).AccountId;
                      //    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">הצג</a>|<a href="#">הצג</a></div>'
                      //}
                  },
                  { text: 'מועד רישום', datafield: 'SignupDate',width:120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'תוקף', datafield: 'ValidityMonth',  cellsalign: 'right', align: 'center' },
                  { text: 'תפוגה', datafield: 'ExpirationDate', width: 120, cellsalign: 'right', type: 'date', cellsformat: 'd', align: 'center' },
                  { text: 'המשלם', datafield: 'CreditCardOwner', cellsalign: 'right', align: 'center' },
                  { text: 'חיוב חוזר', datafield: 'AutoCharge', columntype: 'checkbox', width: 100, cellsalign: 'right', align: 'center' },
                  { text: 'קמפיין', datafield: 'CampaignName', cellsalign: 'right', align: 'center' },
                  { text: 'מחירון', datafield: 'ItemName', cellsalign: 'right', align: 'center' },
                  { text: 'מחיר', datafield: 'Price',  cellsalign: 'right', align: 'center' },
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
                url: '/Main/GetMemberCategories',
                data: { 'rcdid': id }
            }
            slf.NContainer.CategoriesGrid[index] = nastedcontainer;

            var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
            nastedcontainer.jqxGrid({
                source: nastedAdapter, width: '70%', height: 130,showheader: false,
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
                slf.NContainer.categoriesDialog = app_dialog.dialogIframe(app.appPath() + "/Common/_MemberCategories?id=" + id, "580", "400","סיווגים");
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

            slf.NContainer.currentIndex = index;

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
                //     slf.selectRow(index);
                //     slf.edit();//.memberEdit(rcdid);
                // });

                //var deleteitem = $('<div style="margin:10px"><a title="הסר" href="#" >הסר מנוי</a></div>')
                // .click(function () {
                //     slf.selectRow(index);
                //     slf.remove(rcdid);// app_members_grid.memberDelete(rcdid);
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

            //if (slf.AllowEdit == 1) {
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
            queryButton.jqxButton({ width: 70, height: 20});
            //searchButton.jqxButton({ width: 50, height: 20 });
            // add new row.
            addButton.click(function (event) {
                //app_popup.memberEdit(0);
                slf.add();
            });
            editButton.click(function (event) {
                //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                //if (selectedrowindex < 0)
                //    return;
                //var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                //app_popup.memberEdit(id);
                slf.edit();
            });
            // delete selected row.
            deleteButton.click(function (event) {
                //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                //if (selectedrowindex < 0)
                //    return;
                //var rcdid = $('#jqxgrid').jqxGrid('getrowdata', selectedrowindex).RecordId;
                slf.remove();//app_members_grid.memberDelete(rcdid);
                
            });
            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
            queryButton.click(function (event) {
                app.redirectTo('/Main/MembersQuery');
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
            source: slf.dataAdapter,
            localization: getLocalization('he'),
            virtualmode: true,
            rendergridrows: function (obj) {
                return slf.dataAdapter.records;
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
              //     //    if (slf.AllowEdit == 1) {
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
              //        slf.buildFilterPanel(filterPanel, datafield);
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
            slf.edit();
        });

    },
    //memberDelete: function (rcdid) {
    //    if (!confirm('האם למחוק את המנוי ' + rcdid)) {
    //        return;
    //    };
    //    $.ajax({
    //        type: "POST",
    //        url: '/Common/DeleteMember',
    //        data: { 'RecordId': rcdid },
    //        //contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (data) {
    //           app_dialog.alert(data.Message);
    //            $('#jqxgrid').jqxGrid('source').dataBind();
    //            //if (data.Status > 0)
    //            //    dialogMessage('מנויים', 'מנוי ' + memid + ' הוסר מרשימת המנויים ', true);
    //            //else
    //            //   app_dialog.alert(data.Message);
    //        },
    //        completed: function (data) {
    //            $('#jqxgrid').jqxGrid('source').dataBind();
    //        },
    //        error: function (e) {
    //           app_dialog.alert(e);
    //        }
    //    });
    //},
    categoriesRefresh: function () {
        try {
            var i = this.NContainer.currentIndex;
            var g = this.NContainer.CategoriesGrid[i];
            g.jqxGrid('source').dataBind();
        }
        catch (e) {
           app_dialog.alert(e);
        }
    },
    //getNastedGrid: function () {
    //    try {
    //        var i = this.NContainer.currentIndex;
    //        var g = this.NContainer.nastedGrids[i];
    //        g.jqxGrid('source').dataBind();
    //    }
    //    catch (e) {
    //        app_dialog.alert(e);
    //    }
    //},
    categoriesDelete: function (id, rcdid) {
        //accountNewsRemove(id, memid);
        var slf = this;
        if (confirm("האם להסיר מנוי " + rcdid + " מקבוצת סיווג " + id)) {
            $.ajax({
                type: "POST",
                url: '/Main/DeleteMemberCategories',
                data: { 'rcdid': rcdid ,'propId': id },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    slf.categoriesRefresh();
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
    },

    load: function (Model, userInfo) {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();

        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.ExType = Model.ExType;
        
        this.source.data = {
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

        this.dataAdapter = new $.jqx.dataAdapter(this.source, {
            loadComplete: function (data) {
                //source.totalrecords = getTotalRows(data);
            },
            loadError: function (xhr, status, error) {
                app_dialog.alert(' status: ' + status + '\n error ' + error)
            }
        });

        this.grid();

        var slf = this;

        //if (this.AllowEdit == 0) {
        //    $("$member-item-delete").hide();
        //}

        $('#member-item-update').click(function () {
            //var iframe = wizard.getIframe();
            //if (iframe && iframe.def) {
            //    iframe.def.doSubmit();
            //}
            slf.update();
        });
        $('#member-item-update-plus').click(function () {
            //var iframe = wizard.getIframe();
            //if (iframe && iframe.def) {
            //    iframe.def.doSubmit();
            //}
            slf.update_plus();
        });
        $('#member-item-cancel').click(function () {
            //wizard.wizHome();
            slf.cancel();
        });

        return this;
    },
    loadQuery: function (Model) {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsMemberQuery = 1;
        //this.ExType = Model.ExType;

        this.source.data = {
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

        this.dataAdapter.dataBind();

        wizard.displayStep(1);

        return this;
    },
    cancelQuery: function () {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsMemberQuery = 0;
        var accid = this.source.data.AccountId;
        var extype = this.source.data.ExType;
        this.ExType = extype;

        this.source.data = {
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

        this.dataAdapter.dataBind();

        wizard.displayStep(1);

        return this;
    },
    showControl: function (id, option, action) {

        var data_model = { Id: id, Option: option, Action: action };

        if (this.Control == null) {
            this.Control = new app_members_def_control("#divPartial2");
        }
        this.Control.init(data_model, this.UInfo,this.ExType);
        this.Control.display();
    },
    getrowId: function () {

        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
        return id;
    },
    selectRow: function (index) {
        $('#jqxgrid').jqxGrid('selectrow', index);
    },
    //load: function (id, userInfo) {
    //    this.TaskId = id;
    //    this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
    //    this.grid(id);
    //    return this;
    //},
    query: function () {
        //app.redirectTo('/Main/MembersQuery');
        if (!wizard.existsIframe(3)) {
            wizard.appendIframe(3, app.appPath() + "/Main/_MembersQuery", "100%", "620px", true, "#loader");
        }
        wizard.displayStep(3);
    },
    add: function () {
        $('#member-item-update').show();
        $("#member-item-update-plus").show();
        //wizard.appendIframe(2, app.appPath() + "/Main/_MemberAdd", "100%", "620px", true, "#loader");
        wizard.displayStep(2);
        this.showControl(0, 'a');
    },
    edit: function (id) {
        $("#member-item-update-plus").hide();
        if(id===undefined) id = this.getrowId();
        if (id > 0) {
            $('#member-item-update').show();
            wizard.displayStep(2);
            this.showControl(id, 'e');
            //wizard.appendIframe(2, app.appPath() + "/Main/_MemberEdit?id=" + id, "100%", "620px", true, "#loader");
        }
    },
    view: function (id) {
        $("#member-item-update-plus").hide();
        $('#member-item-update').hide();
        if (id === undefined) id = this.getrowId();
        if (id > 0) {
            wizard.displayStep(2);
            this.showControl(id, 'g');
            //$('#member-item-update').hide();
            //wizard.appendIframe(2, app.appPath() + "/Main/_MemberEdit?id=" + id, "100%", "620px", true, "#loader");
        }
    },
    befor_update: function () {
        app_jqxcombos.renderCheckList("listCategory", "Categories");
    },
    update: function () {
        if (this.Control != null) {
            this.Control.doSubmit();
        }
        //var iframe = wizard.getIframe();
        //if (iframe) {
        //    iframe.app_query.doFormPost('#fcForm', this.end, this.befor_update,"#validator-message");
        //}
    },
    update_plus: function () {

        if (this.Control != null) {
            this.Control.doSubmit();
        }

        //var iframe = wizard.getIframe();
        //if (iframe) {
        //    iframe.app_query.doFormPost('#fcForm', this.end_plus, this.befor_update, "#validator-message");
        //}
    },
    remove: function (id) {
        if (id === undefined) id = this.getrowId();
        if (id > 0) {
            if (confirm('האם למחוק את המנוי ' + id)) {
                app_query.doPost(app.appPath() + "/Main/MemberDelete", { 'RecordId': id }, this.end_internal);
            }
        }
    },
    refresh: function () {
        $('#jqxgrid').jqxGrid('source').dataBind();
    },
    clear_filter : function () {
        if(this.IsMemberQuery == 1)
            this.cancelQuery();
        else
            $("#jqxgrid").jqxGrid('clearfilters');
    },
    cancel: function () {
        wizard.wizHome();
    },
    end_internal: function (data) {
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            $('#jqxgrid').jqxGrid('source').dataBind();
        }
    },
    end: function (data) {
        //wizard.removeIframe(2);
        wizard.wizHome();
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            window.parent.members_grid.refresh();
        }
    },
    end_plus: function (data) {
        //wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            wizard.changeIframe(app.appPath() + "/Main/_MemberAdd");
        }
    },
    show_payment:function(args){

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
    },
    OpenWindow: function (jsonString) {
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

};
var app_members_def_control = function (tagWindow) {

    this.wizControl,
    this.dataSource;
    this.init = function (dataModel, userInfo,extype) {
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
                     url: '/Main/GetMemberEdit'
                 };
        var pasive = dataModel.Option == "a" ? " pasive" : "";
        var html =
    '<div id="fcWindow" style="border-top:solid 2px #15C8D8">' +
         '<div id="fcBody" style="background-color:#fff;border:solid 1px #15C8D8">' +
            '<form class="fcForm" id="fcForm" method="post" action="/Main/MemberUpdate">' +
                '<div style="direction: rtl; text-align: right;">' +
                    '<input type="hidden" id="ExType" />' +
                    '<input type="hidden" id="RecordId" name="RecordId" value="0" />' +
                    '<input type="hidden" id="AccountId" name="AccountId" value="' + userInfo.AccountId + '" />' +
                    '<input type="hidden" id="Categories" name="Categories" value="" />' +
                    '<div class="tab-container">' +
                         '<div id="tab-personal" class="tab-group">' +
                            '<h3>פרטים אישיים</h3>' +
                            '<div class="form-group">' +
                                '<div class="field">תעודת זהות :</div>' +
                                '<input id="MemberId" name="MemberId" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">שם פרטי :</div>' +
                                '<input id="FirstName" name="FirstName" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">שם משפחה :</div>' +
                                '<input id="LastName" name="LastName" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">כתובת :</div>' +
                                '<input id="Address" name="Address" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">עיר :</div>' +
                                '<div id="City" name="City"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">מגדר:</div>' +
                                '<div id="Gender" name="Gender"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">תאריך לידה:</div>' +
                                '<div id="Birthday" name="Birthday"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">טלפון נייד:</div>' +
                                '<input id="CellPhone" name="CellPhone" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">טלפון:</div>' +
                                '<input id="Phone" name="Phone" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">דאר אלקטרוני:</div>' +
                                '<input id="Email" name="Email" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">חברה\ארגון :</div>' +
                                '<input id="CompanyName" name="CompanyName" type="text" class="text-mid" />' +
                            '</div>' +
                        '</div>' +
                        '<div id="tab-general" class="tab-group">' +
                            '<h3>פרטים כלליים</h3>' +
        '<div class="form-group">' +
            '<div class="field">סניף :</div>' +
            '<div id="Branch" name="Branch"></div>' +
        '</div>' +
        '<div class="form-group">' +
            '<div class="field">סווג :</div>' +
            '<div id="listCategory" name="listCategory" style="padding: 10px" data-type="checklist"></div>' +
        '</div>' +
        '<div id="divExId" class="form-group field-ex">' +
            '<div id="lblExId" class="column">' +
            '</div>' +
            '<input type="text" id="ExId" name="ExId" class="text-mid" maxlength="50" />' +
        '</div>' +
        '<div id="divExEnum1" class="form-group field-ex">' +
            '<div id="lblExEnum1" class="column">' +
            '</div>' +
            '<div id="ExEnum1" name="ExEnum1"></div>' +
        '</div>' +
        '<div id="divExEnum2" class="form-group field-ex">' +
            '<div id="lblExEnum2" class="column">' +
            '</div>' +
            '<div id="ExEnum2" name="ExEnum2"></div>' +
        '</div>' +
        '<div id="divExEnum3" class="form-group field-ex">' +
            '<div id="lblExEnum3" class="column">' +
            '</div>' +
            '<div id="ExEnum3" name="ExEnum3"></div>' +
        '</div>' +
        '<div id="divExField1" class="form-group field-ex">' +
            '<div id="lblExField1" class="column">' +
            '</div>' +
            '<input type="text" id="ExField1" name="ExField1" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExField2" class="form-group field-ex">' +
            '<div id="lblExField2" class="column">' +
            '</div>' +
            '<input type="text" id="ExField2" name="ExField2" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExField3" class="form-group field-ex">' +
            '<div id="lblExField3" class="column">' +
            '</div>' +
            '<input type="text" id="ExField3" name="ExField3" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExRef1" class="form-group field-ex">' +
            '<div id="lblExRef1" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef1" name="ExRef1" />' +
        '</div>' +
        '<div id="divExRef2" class="form-group field-ex">' +
            '<div id="lblExRef2" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef2" name="ExRef2" />' +
        '</div>' +
        '<div id="divExRef3" class="form-group field-ex">' +
            '<div id="lblExRef3" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef3" name="ExRef3" />' +
        '</div>' +
    '</div>' +
    '<div id="tab-notes" class="tab-group">' +
        '<h3>הערות</h3>' +
        '<div class="form-group">' +
            '<div class="field">הערות:</div>' +
            '<textarea id="Note" name="Note" style="width:100%;height:60px"></textarea>' +
        '</div>' +
        '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="JoiningDate" name="JoiningDate" type="text" class="text-mid" readonly="readonly" />' +
        '</div>' +
        '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד עדכון:</div>' +
            '<input id="LastUpdate" name="LastUpdate" type="text" readonly="readonly" class="text-mid" />' +
        '</div>' +
    '</div>' +
    '</div>' +
    '<div style="clear: both;"></div>'+
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
            this.wizControl = new wiz_control("member_def", tagWindow);
            this.wizControl.init(html, this.ExType, function (data) {


                $('#Birthday').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
                $('#Birthday').val('');

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

            });
        }
        else {
            this.wizControl.clearDataForm("fcForm");
            app_jqxcombos.clearCheckList("#listCategory");
        }
    },
    this.display = function () {
        $(tagWindow).show();
        $("#ExType").val(this.ExType);

        if (this.RecordId > 0) {
            this.wizControl.load("fcForm", this.dataSource, function (record) {

                app_jqxform.loadDataForm("fcForm", record);

                app_jqxcombos.selectCheckList("listCategory", record.Categories);

                app_jqxcombos.initComboValue('City', 0);

            });
        }
        else {
            $("#AccountId").val(this.AccountId);
            $("#RecordId").val(0);
        }
    },
    this.doCancel = function () {
        this.wizControl.doCancel();
    },
    this.doSubmit = function () {
        this.wizControl.doSubmit(
            function () {
                app_jqxcombos.renderCheckList("listCategory", "Categories");
            },
            function (data) {
                app_dialog.alert(data.Message);
                if (data.Status >= 0) {
                    //if (slf.IsDialog) {
                    window.parent.triggerWizControlCompleted("member_def",data.OutputId);

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
};

})(jQuery)

