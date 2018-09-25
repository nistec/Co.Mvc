'use strict';

var members_grid;

class members_mv {

    constructor($element, dataModel) {
        this.$element = $element;
        this.Title = "מנויים";
        this.Name = "Members";

        //this.NContainer = {};
        //this.DataAdapter = {};
        //this.AllowEdit = 0;
        //this.IsMemberQuery = 0;
        ////CurrentId:0,
        //this.ExType = 0;
        //this.UInfo = null;
        //this.Control = null;
        //this.FilterMode = "filter";



        if (dataModel)
            this.init(dataModel);
    }

    _loadHtml(dataModel) {
        var html =`
<div class="global-view">
    <div id="wiz-container" class="container">

        <div id="hTitle" class="panel-page-header rtl">
            <span id="hTitle-text" style="margin:10px">רשימת מנויים</span><span><a href="javascript:location.reload()"><i class="fa fa-refresh"></i></a></span>
        </div>

        <!--<div id="wiz-1" class="wiz-tab active">-->
            
            <div class="grid-wrap rtl">
                <div id="grid-toolbar" class="grid-toolbar"></div>
                <div class="rtl">
                    <a id="member-item-add" class="btn-default btnIcon w-60 pasive" data-rule="a" href="#">הוספה</a>
                    <a id="member-item-edit" class="btn-default btnIcon w-60 pasive" data-rule="e" href="#">עריכה </a>
                    <a id="member-item-delete" class="btn-default btnIcon w-60 pasive" href="#">מחיקה </a>
                    <!--<a id="member-item-filetr-remove" class="btn-default btn7 w-80 font-90" href="#">הסר סינון</a>-->
                    <a id="member-item-query" class="btn-default btnIcon w-60" href="#">איתור</a>
                    <a id="member-item-query-cancel" title="ביטול סינון\איתור" class="btn-default btnIcon w-60" style="display:none" href="#">(X)</a>
                </div>
                <div id="jqxWidget">
                    <div id="jqxgrid" style="position:relative;z-index:1;"></div>
                </div>
                <div id="jqxGridColumns"></div>
             </div>
        <!--</div>-->
        
    </div>
</div>
`
        $(this.$element).html(html);

        this.Rule = app_perms.renderRule(this.Name, dataModel.Option);

        app_menu.activeLayoutMenu("liMain");
        app_menu.breadcrumbs("Main", "Members", 'en');

    }

    _loadData(Model) {

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
    }

    _loadGrid() {

        var _slf = this;

        this.NContainer = {};
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();

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
                title.html('<span style="margin:10px"> ' + datarecord.MemberName + ' </span><a title="עריכת מנוי" href="javascript:members_grid.edit(' + rcdid + ');" >...</a>');
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

        $("#jqxgrid").on("filter", function (event) {
            var filterinfo = $("#jqxgrid").jqxGrid('getfilterinformation');
            console.log(filterinfo);
            if (_slf.FilterMode == "query") {
                _slf.cancelQuery();
            }
        });

        var columnList = [
            { label: 'ת.ז המנוי', value: 'MemberId', checked: true },
            { label: 'שם המנוי', value: 'MemberName', checked: true },
            { label: 'טלפון נייד', value: 'CellPhone', checked: true },
            { label: 'דואר אלקטרוני', value: 'Email', checked: true },
            { label: 'עיר', value: 'CityName', checked: true },
            { label: 'כתובת', value: 'Address', checked: false },
            { label: 'שם חברה', value: 'CompanyName', checked: true }
            //{ label: 'מס רישום', value: 'SignupId', checked: false },
            //{ label: 'תוקף', value: 'ValidityMonth', checked: true },
            //{ label: 'המשלם', value: 'CreditCardOwner', checked: true },
            //{ label: 'חיוב חוזר', value: 'AutoCharge', checked: true },
            //{ label: 'קמפיין', value: 'CampaignName', checked: true },
            //{ label: 'מחירון', value: 'PriceName', checked: false },
            //{ label: 'מחיר', value: 'Price', checked: false },
            //{ label: 'מספר קבלה', value: 'PayId', checked: true }
        ];
        var dateList = [
            { label: 'מועד הצטרפות', value: 'JoiningDate', checked: false }
            //{ label: 'תאריך עדכון', value: 'LastUpdate', checked: false }
        ];

        //app_jqxgrid.gridColumnsBar(columnList);
        //$('#jqxgrid').on('bindingcomplete', function (event) {
        //    app_jqxgrid.gridColumnsInit(columnList, '#jqxgrid')
        //});

        app_jqxgrid.gridFilterEx(this, $("#grid-toolbar"), columnList, dateList);
    }

    _loadRules() {

        var _slf = this;

        //$('#member-item-update').click(function () {
        //    //var iframe = wizard.getIframe();
        //    //if (iframe && iframe.def) {
        //    //    iframe.def.doSubmit();
        //    //}
        //    _slf.update();
        //});
        //$('#member-item-update-plus').click(function () {
        //    //var iframe = wizard.getIframe();
        //    //if (iframe && iframe.def) {
        //    //    iframe.def.doSubmit();
        //    //}
        //    _slf.update_plus();
        //});
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
        $('#member-item-query-cancel').click(function () {
            _slf.cancelQuery();
        });
    }

    init(dataModel,userInfo) {

        var _slf = this;

        this.UInfo = userInfo;
        //this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.ExType = userInfo.ExType;

        this._loadHtml(dataModel);

        this._loadData(dataModel);

        this._loadGrid();

        this._loadRules();
    }

    //grid methods

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
        this.ExType = Model.ExType;

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
        this.queryChanged("query");
        //this.DataAdapter.dataBind();
        //this.FilterMode = "query";
        //app_panel.panelAfterClose(TagSpa);
        //wizard.displayStep(1);
    }

    cancelQuery() {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsMemberQuery = 0;
        var accid = this.DataSource.data.AccountId;
        //var extype = this.DataSource.data.ExType;
        //this.ExType = extype;

        this.DataSource.data = {
            'QueryType': 0,
            'AccountId': accid,
            'MemberId': "",
            'ExId': "",
            //'ExType': extype,
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
        this.queryChanged("cancel");
        //this.DataAdapter.dataBind();
        //this.FilterMode = "filter";
        //$('#member-item-query-cancel').hide();
        //$('#grid-toolbar').show();
        //wizard.displayStep(1);
    }

    queryChanged(action) {

        this.DataAdapter.dataBind();
        if (action == "cancel") {
            $('#member-item-query-cancel').hide();
            $('#grid-toolbar').show();
            this.FilterMode = "filter";

        }
        else {
            $('#member-item-query-cancel').show();
            $('#grid-toolbar').hide();
            this.FilterMode = "query";

        }
    }

    showControl(id, option, action) {

        var data_model = { Id: id, Option: option, Action: action, Inline: true };

        if (this.Control == null) {
            this.Control = new member_mv(TagSpa);//("#Members");
        }
        this.Control.init(data_model, this.UInfo, this.ExType);
        this.Control.doDisplay();

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
        //if (!wizard.existsIframe(3)) {
        //    wizard.appendIframe(3, app.appPath() + "/Co/_MembersQuery", "100%", "680px", true, "#loader");
        //}
        //wizard.displayStep(3);

        var datamodel = new members_query(this.UInfo);
        var model = new member_query_mv(TagSpa);
        model.init(datamodel, this.UInfo);
        model.doDisplay();
        //$('#member-item-query-cancel').show();
        //$('#grid-toolbar').hide();
    }

    add() {
        $('#member-item-update').show();
        $("#member-item-update-plus").show();
        //wizard.appendIframe(2, app.appPath() + "/Co/_MemberAdd", "100%", "620px", true, "#loader");
        //wizard.displayStep(2);
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

    view(id) {
        $("#member-item-update-plus").hide();
        $('#member-item-update').hide();
        if (id === undefined) id = this.getrowId();
        if (id > 0) {
            //wizard.displayStep(2);
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
/*
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
        this.FilterMode = "filter";
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
        $('#member-item-query-cancel').click(function () {
            _slf.cancelQuery();
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
                title.html('<span style="margin:10px"> ' + datarecord.MemberName + ' </span><a title="עריכת מנוי" href="javascript:members_grid.edit(' + rcdid + ');" >...</a>');
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

        $("#jqxgrid").on("filter", function (event) {
            var filterinfo = $("#jqxgrid").jqxGrid('getfilterinformation');
            console.log(filterinfo);
            if (_slf.FilterMode == "query") {
                _slf.cancelQuery();
            }
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
        this.ExType = Model.ExType;

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
        this.queryChanged("query");
        //this.DataAdapter.dataBind();
        //this.FilterMode = "query";
        //app_panel.panelAfterClose(TagSpa);
        //wizard.displayStep(1);
    }

    cancelQuery() {
        //this.NContainer.nastedGrids = new Array();
        this.NContainer.SginupGrid = new Array();
        this.NContainer.CategoriesGrid = new Array();
        this.IsMemberQuery = 0;
        var accid = this.DataSource.data.AccountId;
        //var extype = this.DataSource.data.ExType;
        //this.ExType = extype;

        this.DataSource.data = {
            'QueryType': 0,
            'AccountId': accid,
            'MemberId': "",
            'ExId': "",
            //'ExType': extype,
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
        this.queryChanged("cancel");
        //this.DataAdapter.dataBind();
        //this.FilterMode = "filter";
        //$('#member-item-query-cancel').hide();
        //$('#grid-toolbar').show();
        //wizard.displayStep(1);
    }

    queryChanged(action) {

        this.DataAdapter.dataBind();
        if (action=="cancel") {
            $('#member-item-query-cancel').hide();
            $('#grid-toolbar').show();
            this.FilterMode = "filter";

        }
        else {
            $('#member-item-query-cancel').show();
            $('#grid-toolbar').hide();
            this.FilterMode = "query";

        }
    }

    showControl(id, option, action) {

        var data_model = { Id: id, Option: option, Action: action, Inline: true };

        if (this.Control == null) {
            this.Control = new model_member(TagSpa);//("#Members");
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
        //if (!wizard.existsIframe(3)) {
        //    wizard.appendIframe(3, app.appPath() + "/Co/_MembersQuery", "100%", "680px", true, "#loader");
        //}
        //wizard.displayStep(3);

        var datamodel = new members_query(this.UInfo);
        var model = new member_query_mv(TagSpa);
        model.init(datamodel, this.UInfo);
        model.display();
        //$('#member-item-query-cancel').show();
        //$('#grid-toolbar').hide();
    }

    add() {
        $('#member-item-update').show();
        $("#member-item-update-plus").show();
        //wizard.appendIframe(2, app.appPath() + "/Co/_MemberAdd", "100%", "620px", true, "#loader");
        //wizard.displayStep(2);
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

    view(id) {
        $("#member-item-update-plus").hide();
        $('#member-item-update').hide();
        if (id === undefined) id = this.getrowId();
        if (id > 0) {
            //wizard.displayStep(2);
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
*/

function members_query(userInfo) {
    this.Inline = true;
    this.QueryType = 1;
    this.AccountId = userInfo.AccountId;
    this.UserId = userInfo.UserId;
    this.ExType = userInfo.ExType;
    this.MemberId = null;
    this.ExId = null;
    this.CellPhone = null;
    this.Email = null;
    this.Name = null;
    this.Address = null;
    this.City = null;
    this.Category = null;
    this.Region = null;
    this.Branch = null;
    this.ExEnum1 = null;
    this.ExEnum2 = null;
    this.ExEnum3 = null;
    this.BirthdayMonth = null;
    this.JoinedFrom = null;
    this.JoinedTo = null;
    this.AgeFrom = null;
    this.AgeTo = null;
    this.ContactRule = null;
    this.Items = null;
    this.ReferralCode = null;
    this.ValidityRemain = null;
    this.Campaign = null;
    this.SignupDateFrom = null;
    this.SignupDateTo = null;
    this.PriceFrom = null;
    this.PriceTo = null;
    this.HasAutoCharge = false;
    this.HasPayment = false;
    this.HasSignup = false;
}


class member_query_mv {
    
    constructor($element) {
        this.$element = $element;
        this.Title = "איתור";
    }

    _loadHtml(dataModel) {

        var html = `
    <div class="container-box">
                        <div id="form-container">
                    <form id="form" action="./" method="post">

                        <input type="hidden" id="QueryType" name="QueryType" value="0" />
                        <input type="hidden" id="ExType" />
                        <input type="hidden" id="AccountId" name="AccountId" />
                        <input type="hidden" id="Category" name="Category" />
                        <input type="hidden" id="Branch" name="Branch" />
                        <input type="hidden" id="Region" name="Region" />
                        <input type="hidden" id="City" name="City" />

                        <div id="accordion" class="jcx-tabs rtl tab-content">
                            <ul id="tab-query" class="nav nav-tabs1">
                                <li class="active"><a href="#exp-1" data-toggle="tab">איתור כללי</a></li>
                                <li><a href="#exp-2" data-toggle="tab">איתור לפי כתובת</a></li>
                                <li><a href="#exp-3" data-toggle="tab">איתור לפי מזהה</a></li>
                                <li><a href="#exp-4" data-toggle="tab">איתור מתקדם</a></li>
                                <li><a href="#exp-5" data-toggle="tab">איתור מערכת רישום ותשלומים</a></li>
                            </ul>
                            <!--<div style="clear: both; height: 20px"></div>-->
                                <div id="exp-1" class="jcxtab-panel">
                                    <div class="panel-area expander-entry">
                                        <div class="panel-area-title">איתור כללי</div>
                                        <div>
                                            <label class="headline">סיווג-בחירה מרובה</label>
                                            <div id="listCategory" name="listCategory"></div>
                                            <input type="checkbox" id="allCategory" name="allCategory" />&nbsp;<span>הצג הכל</span>
                                        </div>
                                        <div>
                                            <div class="headline">סניף</div>
                                            <div id="listBranch" name="Branch"></div>
                                            <input type="checkbox" id="allBranch" name="allBranch" />&nbsp;<span>הצג הכל</span>
                                        </div>
                                        <div id="divExEnum1" class="field-ex">
                                            <div id="lblExEnum1" class="headline"></div>
                                            <div id="ExEnum1" name="ExEnum1"></div>
                                            <input type="checkbox" id="allEnum1" name="allEnum1" />&nbsp;<span>הצג הכל</span>
                                        </div>
                                        <div id="divExEnum2" class="field-ex">
                                            <div id="lblExEnum2" class="headline"></div>
                                            <div id="ExEnum2" name="ExEnum2"></div>
                                            <input type="checkbox" id="allEnum2" name="allEnum12" />&nbsp;<span>הצג הכל</span>
                                        </div>
                                        <div id="divExEnum3" class="field-ex">
                                            <div id="lblExEnum3" class="headline"></div>
                                            <div id="ExEnum3" name="ExEnum3"></div>
                                            <input type="checkbox" id="allEnum3" name="allEnum3" />&nbsp;<span>הצג הכל</span>
                                        </div>
                                    </div>
                                </div>
                                    <div id="exp-2" class="jcxtab-panel">
                                <div class="panel-area">
                                    <div class="panel-area-title">איתור לפי כתובת</div>
                                    <div class="expander-entry">
                                        <div>
                                            <div class="headline">מחוז</div>
                                            <div id="listRegion" name="listRegion"></div>
                                            <input type="checkbox" id="allRegion" name="allRegion" />&nbsp;<span>הצג הכל</span>
                                        </div>
                                        <div>
                                            <div class="headline">(שם היישוב (מתעדכן אוטומטית</div>
                                            <div id="listCity" name="listCity"></div>
                                            <a href="#" id="allCity">סמן הכל</a>&nbsp;|&nbsp;
                                            <a href="#" id="unselectAllCity">הסר</a>
                                        </div>
                                        <div>
                                            <div class="headline">כתובת</div>
                                            <input type="text" name="Address" style="width:240px" maxlength="30" />
                                        </div>
                                    </div>
                            </div>
                                </div>
                                <div id="exp-3" style="padding:0;margin:0" class="jcxtab-panel">
                                    <div class="panel-area">

                                        <div class="panel-area-title">איתור לפי מזהה</div>
                                        <div class="expander-entry">
                                            <div class="headline">תעודת זהות</div>
                                            <div><input type="text" name="MemberId" class="edit" maxlength="10" /></div>
                                            <div class="headline">טלפון נייד</div>
                                            <div><input type="text" name="CellPhone" class="edit" maxlength=" 15" /></div>
                                            <div class="headline">דואר אלקטרוני</div>
                                            <div><input type="text" name="Email" class="edit" maxlength="100" /></div>

                                            <div id="divExId" class="field-ex">
                                                <div id="lblExId" class="headline"></div>
                                                <input type="text" name="ExId" style="width:120px" maxlength="50" />
                                            </div>
                                            <!--
               <div id="divExField1" class="field-ex">
                   <div id="lblExField1" class="headline"></div>
                   <input type="text" name="ExField1" class="edit" maxlength="250" />
               </div>
               <div id="divExField2" class="field-ex">
                   <div id="lblExField2" class="headline"></div>
                   <input type="text" name="ExField2" class="edit" maxlength="250" />
               </div>
               <div id="divExField3" class="field-ex">
                   <div id="lblExField3" class="headline"></div>
                   <input type="text" name="ExField3" class="edit" maxlength="250" />
               </div>
            -->
                                        </div>
                                    </div>
                                </div>
                                <div id="exp-4" style="padding:0;margin:0" class="jcxtab-panel">
                                    <div class="panel-area">
                                        <div class="panel-area-title">איתור מתקדם</div>
                                        <div class="expander-entry">
                                            <ul>
                                                <li>
                                                    <div style="vertical-align:top">
                                                        <div class="headline">גיל</div>
                                                        <span>מ:</span>
                                                        <input type="number" name="AgeFrom" class="text-short" />
                                                        <span>עד:</span>
                                                        <input type="number" name="AgeTo" class="text-short" />
                                                    </div>
                                                </li>
                                                <li class="headline">הצטרפות לפני</li>
                                                <li><input type="number" name="monthlyTime" style="width:60px" /><span>חודשים</span></li>
                                                <li class="headline">שונות</li>
                                                <li><input type="checkbox" id="allBirthday" name="allBirthday" />&nbsp;<span>חוגגים החודש יום הולדת</span></li>
                                                <li><input type="checkbox" id="allCell" name="allCell" />&nbsp;<span>תקשורת לסלולאר</span></li>
                                                <li><input type="checkbox" id="allEmail" name="allEmail" />&nbsp;<span>תקשורת למייל</span></li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div id="exp-5" style="padding:0;margin:0" class="jcxtab-panel">
                                <div class="panel-area">
                                    <div class="panel-area-title">איתור מערכת רישום ותשלומים</div>
                                    <div class="expander-entry tab-container">
                                        <div class="tab-group1">
                                            <input type="hidden" id="Items" name="Items" />

                                            <div class="form-group-inline">
                                                <div class="field">קמפיין : <input type="checkbox" id="allCampaign" name="allCampaign" />&nbsp;<span>הצג הכל</span></div>
                                                <div id="Campaign" name="Campaign"></div>
                                            </div>
                                            <div class="form-group-inline">
                                                <div class="field">קוד הפנייה :</div>
                                                <input type="text" id="ReferralCode" name="ReferralCode" />
                                            </div>
                                            <br/>
                                            <div style="height:2px"></div>
                                            <div class="form-group">
                                                <div class="field">פריטים-בחירה מרובה :</div>
                                                <div id="listItems" name="listItems"></div>
                                                <input type="checkbox" id="allItems" name="allItems" />&nbsp;<span>הצג הכל</span>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">מחיר :</div>
                                                <div style="vertical-align:top">
                                                        <span>מ:</span>
                                                        <input type="number" name="PriceFrom" step="any" style="width:80px" />
                                                        <span>עד:</span>
                                                        <input type="number" name="PriceTo" step="any" style="width:80px" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">מועד רישום :</div>
                                                     <div style="vertical-align:top">
                                                        <span>מ:</span>
                                                        <div id="SignupDateFrom" name="SignupDateFrom"></div>
                                                        <span>עד:</span>
                                                        <div id="SignupDateTo" name="SignupDateTo"></div>
                                                    </div>
                                            </div>
<br/>
                                            <div class="form-group-inline">
                                                <div class="field">ייתרת תוקף מעל :</div>
                                                <input type="number" name="ValidityRemain" style="width:60px" /><span>חודשים</span>
                                                
                                            </div>
                                            <div style="height:2px"></div>
                                        </div>
                                        <div class="tab-group1">
                                           <div class="form-group-inline">
                                                <div class="field">מנויים שנרשמו :</div>
                                                 <div id="HasSignup-group" class="switch-group">
                                                        <button class="switch-item" id="HasSignup-ignore" data-val="0">התעלם</button>
                                                        <button class="switch-item" id="HasSignup-no" data-val="1">לא</button>
                                                        <button class="switch-item" id="HasSignup-yes" data-val="2">כן</button>
                                                    </div>
                                            </div>
                                            <div class="form-group-inline">
                                                <div class="field">מנויים ששילמו :</div>
                                                 <div id="HasPayment-group" class="switch-group">
                                                        <div class="switch-item" id="HasPayment-ignore" data-val="0">התעלם</div>
                                                        <div class="switch-item" id="HasPayment-no" data-val="1">לא</div>
                                                        <div class="switch-item" id="HasPayment-yes" data-val="2">כן</div>
                                                    </div>
                                            </div>
                                            <div class="form-group-inline">
                                                <div class="field">מנויים עם חיוב חוזר :</div>
                                                <div id="HasAutoCharge-group" class="switch-group">
                                                        <div class="switch-item" id="HasAutoCharge-ignore" data-val="0">התעלם</div>
                                                        <div class="switch-item" id="HasAutoCharge-no" data-val="1">לא</div>
                                                        <div class="switch-item" id="HasAutoCharge-yes" data-val="2">כן</div>
                                                    </div>
                                            </div>
                                            <input type="hidden" id="HasSignup" name="HasSignup" />
                                            <input type="hidden" id="HasPayment" name="HasPayment" />
                                            <input type="hidden" id="HasAutoCharge" name="HasAutoCharge" />
                                        </div>
                                    </div>
                                    <!--<div style="clear:both"></div>-->
                                </div>
                            </div>
                        </div>
                        <!--<div style="clear:both"></div>-->
                        <div style="height:10px"></div>
                        <div>
                           
                            <!--<input type="radio" id="rdMail" name="op" value="MailTargets" />&nbsp;<span>אתר נמענים לשליחת דואל</span>-->
                            
                        </div>
                        
                        <div>
                            <input type="button" id="submitScreen" value="הצג" class="btn-default btn7 w-60" />
                            <input type="button" id="submitExport" value="ייצוא לקובץ" class="btn-default btn7 w-80" />
                            <input type="reset" value="נקה" class="btn-default btn7 w-60" />
                            <input type="button" id="submitCancel" value="ביטול" class="btn-default btn7 w-60" />
                        </div>

                    </form>
<div style="height:10px"></div>
                </div>
        </div>
`

        //var pasive = dataModel.Option == "a" ? " pasive" : "";
        //html.replace("#pasive#", pasive)

        if (dataModel.Inline == true) {
            app_panel.appendPanelAfter(this.$element, html, this.Title, '800px');
        }
        else
            $(this.$element).html(html);
    }

    init(dataModel, userInfo) {

        this._loadHtml(dataModel);

        this.Inline = dataModel.Inline;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        $("#AccountId").val(this.AccountId);

        var _slf = this;

        $("#allBranch").prop('checked', true);
        $("#allCity").prop('checked', true);
        //$("#allPlace").prop('checked', true);
        $("#allCategory").prop('checked', true);
        //$("#allStatus").prop('checked', true);
        $("#allRegion").prop('checked', true);


        //$("#accordion").jcxTabs({
        //    rotate: false,
        //    startCollapsed: 'accordion',
        //    collapsible: 'accordion',
        //    //click: function (e, tab) {
        //    //    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
        //    //},
        //    activate: function (e, tab) {
        //        //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

        //        return false;
        //    },
        //    activateState: function (e, state) {
        //        //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
        //    }
        //});


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

        /*
        var currentRegion = function () {

            var reg = $("#Region").val();
            if (isNumeric(reg))
                return reg;
            return 0;
        }

        app_jqx.createListAdapterAsync("PropId", "PropName", "listRegion", '/Common/GetRegionView', null, 240, 120, function (record) {

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

        });//, beforeLoadedwidth, output)
        */

        
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
        

        $('#submitCancel').on('click', function () {
            //window.parent.triggerWizControlCancel("member_query");
            _slf.doClose();
        });
        $('#submitScreen').on('click', function () {

            _slf.doSubmit();

                //window.parent.triggerWizControlQuery(dataModel);
            //}
            //else {
            //    var action = '/Co/Members';
            //    $("#form").attr('action', app.appPath() + action);
            //    $("#form").submit();
            //}
        });
        $('#submitExport').on('click', function () {

            _slf.doSubmitExport();
          
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

        $("#HasSignup-group").jqxButtonGroup({ mode: 'radio', rtl: true,template: "primary", });
        $("#HasPayment-group").jqxButtonGroup({ mode: 'radio', rtl: true, template: "primary", });
        $("#HasAutoCharge-group").jqxButtonGroup({ mode: 'radio', rtl: true, template: "primary", });

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

    doDisplay() {
        if (this.Inline)
            app_panel.panelAfterShow(this.$element);
        else
            $(this.$element).show();

        $("#accordion").jcxTabs({
            rotate: false,
            active:0,
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
    }

    doSubmit() {

        $("#QueryType").val(0);

        //if (_slf.IsDialog) {
        var datamodel = app.serializeToJsonObject("#form");// $("#form").serialize();

        datamodel.JoinedFrom = 0;
        datamodel.JoinedTo = 0;

        var monthlyTime = datamodel.monthlyTime;
        if (monthlyTime && monthlyTime > 0) {
            datamodel.JoinedFrom = monthlyTime;
        }
        var allCell = datamodel.allCell == "on";
        var allEmail = datamodel.allEmail == "on";

        if (allCell && allEmail)
            datamodel.ContactRule = 3;
        if (allCell)
            datamodel.ContactRule = 1;
        if (allEmail)
            datamodel.ContactRule = 2;
        else
            datamodel.ContactRule = 0;

        datamodel.BirthdayMonth = 0;
        var allBirthday = datamodel.allBirthday == "on";
        if (allBirthday)
            datamodel.BirthdayMonth = DateTime.Now.Month;

        current_mv.loadQuery(datamodel)

        this.doClose();

        //window.parent.triggerWizControlQuery(dataModel);
        //}
        //else {
        //    var action = '/Co/Members';
        //    $("#form").attr('action', app.appPath() + action);
        //    $("#form").submit();
        //}
    }

    doSubmitExport() {

        var action = '/Co/MembersExport';
        $("#QueryType").val(20);
        $("#form").attr('action', app.appPath() + action);
        $("#form").submit();


    }

    doClose() {
        if(this.Inline)
            app_panel.panelAfterClose(this.$element);
        else
            $(this.$element).show();
    }
}


    

 

 

  
