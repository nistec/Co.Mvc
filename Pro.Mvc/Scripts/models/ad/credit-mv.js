'use strict';

class model_credit {

    constructor($element) {
        this.$element = $element;
    }

    load(rules) {
        var html =
 `
<div class="global-view">
    <div id="wiz-container" class="container">

        <div id="hTitle" class="panel-page-header rtl">
            <span id="hTitle-text" style="margin:10px">דוח קרדיט</span><span><a href="javascript:location.reload()"><i class="fa fa-refresh"></i></a></span>
        </div>
            <div class="grid-wrap rtl">
                <div id="grid-toolbar-options" class="grid-toolbar" style="font-size: 80%;">
                    <input type="hidden" id="TaskState" value="0" />
                    <div style="height:5px"></div>
                    <div style="display:inline;float:right">
                        <span>לקוח</span>:<input type="text" id="AccountId" />
                        <span>פעולה</span>:<input type="text" id="ActionType" />
                        <span>מ-תאריך</span>:<input type="text" id="DateFrom" />
                        <span>עד-תאריך</span>:<input type="text" id="DateTo" />
                    </div>
                    <div class="rtl">
                        <a id="report-submit" class="btn-small w-60" href="#">הצג</a>
                        <a id="report-cancel" class="btn-small w-60" href="#">ביטול</a>
                    </div>
                </div>
                <div id="grid-toolbar" class="grid-toolbar"></div>
                <div class="rtl">
                    <a id="member-item-add" class="btn-default btnIcon w-60 #-a-#" href="#">הוספה</a>
                    <a id="member-item-edit" class="btn-default btnIcon w-60 #-w-#" href="#">עריכה</a>
                    <a id="member-item-delete" class="btn-default btnIcon w-60 #-d-#" href="#">מחיקה</a>
                    <!--<a id="member-item-filetr-remove" class="btn-default btn7 w-80 font-90" href="#">הסר סינון</a>-->
                    <a id="member-item-query" class="btn-default btnIcon w-60" href="#">איתור</a>
                </div>
                <div id="jqxWidget">
                    <div id="jqxgrid" style="position:relative;z-index:1;"></div>
                </div>
                <div id="jqxGridColumns"></div>
            </div>
     </div>
</div>
`

        if (rules) {

            app_perms.validateRules(rules, html,'pasive');

            //if (rules.indexOf("r") == -1)
            //    html = "אין הרשאה לצפייה";
            //if (rules.indexOf("w") == -1) {
            //    html = html.replace('<a id="member - item - add" class="btn -default btnIcon w-60" href="#">הוספה</a>', '');
            //    html = html.replace('<a id="member-item-edit" class="btn-default btnIcon w-60" href="#">עריכה</a>', '');
            //}
            //if (rules.indexOf("d") == -1) {
            //    html = html.replace('<a id="member-item-delete" class="btn-default btnIcon w-60" href="#">מחיקה</a>', '');
            //}
        }
        $(this.$element).html(html);

        app_menu.activeLayoutMenu("liReport");
        app_menu.breadcrumbs("Main", "CreditReport", 'en');
    }
  

    init(dataModel, userInfo) {

        this.load(dataModel.Rules);
        var _slf = this;

        JSON.useDateParser();
        var theme = 'nis_metro';

        var columnList = [
            { label: 'מס-רישום', value: 'BillingId', checked: true },
            { label: 'מס-לקוח', value: 'AccountId', checked: true },
            { label: 'מס-שליחה', value: 'TransId', checked: true },
            { label: 'מס-קמפיין', value: 'CampaignId', checked: true },
            { label: 'מס-משתמש', value: 'UserId', checked: true },
            { label: 'סוג פעולה', value: 'ActionName', checked: true },
            { label: 'פריט', value: 'MethodName', checked: true }
        ];
        var dateList = [
            { label: 'מועד עדכון', value: 'Creation', checked: false }
            //{ label: 'תאריך עדכון', value: 'LastUpdate', checked: false }
        ];

        //enable to select columns
        //app_jqxgrid.gridColumnsBar(columnList);
        //$('#jqxgrid').on('bindingcomplete', function (event) {
        //    app_jqxgrid.gridColumnsInit(columnList, '#jqxgrid')
        //});

        app_jqxgrid.gridFilterEx(this, $("#grid-toolbar"), columnList, dateList);

        this.creditGrid = new credit_grid();

        $('#report-submit').on('click', function (e) {
            e.preventDefault();
            var Model = { Url: dataModel.Url ,AccountId: $("#AccountId").val(), ActionType: $("#ActionType").val(), DateFrom: $("#DateFrom").val(), DateTo: $("#DateTo").val() };
            _slf.creditGrid.load(Model, userInfo);
        });
    }
}
    
class credit_grid {

    constructor() {
        this.Loaded = false;
    }

    grid(dataModel) {

        this.source =
            {
                datatype: "json",
                //async: false,
                datafields: [
                    { name: 'Creation', type: 'date' },
                    { name: 'AccountId', type: 'number' },
                    { name: 'TransId', type: 'number' },
                    { name: 'CampaignId', type: 'number' },
                    { name: 'CreditValue', type: 'number' },
                    { name: 'CreditStatus', type: 'number' },
                    { name: 'Units', type: 'number' },
                    { name: 'UserId', type: 'number' },
                    { name: 'BillingId', type: 'number' },
                    { name: 'ActionName', type: 'string' },
                    { name: 'MethodName', type: 'string' },
                    { name: 'ActualCredit', type: 'number' },
                    { name: 'TotalValue', type: 'number' },
                    { name: 'TotalRows', type: 'number' },
                    { name: 'row', type: 'number' }
                ],
                id: 'row',
                type: 'POST',
                url: dataModel.Url,
                data: { 'AccountId': dataModel.AccountId, 'ActionType': dataModel.ActionType },
                pagenum: 0,
                pagesize: 20,
                root: 'Result',
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
                    if (data.Result.length > 0)
                        this.totalrecords = data.Result[0].TotalRows;
                }
            };

        //this.source.data = {
        //    'QueryType': Model.QueryType,
        //    'AccountId': dataModel.AccountId,
        //    'ActionType': dataModel.ActionType
        //    'DateFrom': Model.DateFrom,
        //    'DateTo': Model.DateTo
        //};

        var getTotalRows = function (data) {
            if (data) {
                return dataTotalRows(data);
            }
            return 0;
        }

        this.dataAdapter = new $.jqx.dataAdapter(this.source, {
            loadComplete: function (data) {
                //source.totalrecords = getTotalRows(data);
            },
            loadError: function (xhr, status, error) {
                app_dialog.alert(' status: ' + status + '\n error ' + error)
            }
        });

        var slf = this;
        var subjectWidth = slf.IsMobile ? 250 : 400;

        // create Grid
        $("#jqxgrid").jqxGrid(
            {
                width: '100%',
                //autoRowHeight: false,
                autoheight: true,
                enabletooltips: true,
                rtl: true,
                source: slf.dataAdapter,
                localization: getLocalization('he'),
                virtualmode: true,
                rendergridrows: function (obj) {
                    return slf.dataAdapter.records;
                },
                columnsresize: true,
                pageable: true,
                //pagermode: 'simple',
                sortable: true,
                filterable: true,
                autoshowfiltericon: true,
                groupable: true,
                columns: [
                    {
                        text: 'מס-רשומה', dataField: 'BillingId', filterable: false, width: 120, cellsalign: 'right', align: 'center',
                        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                            //var editlink = '';
                            //var asb = $('#jqxgrid').jqxGrid('getrowdata', row).AssignBy;
                            //if (slf.UserId == asb)
                            //    editlink = '<label> </label><a href="#" onclick="app_tasks_grid.taskEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';

                            var title = " קרדיט ";
                            return '<div style="text-align:right;margin-top:6px;margin-right:10px"><a href="#" onclick="app_jqxgrid.displayGridRecord(' + row + ',\'' + title+'\')" ><label> ' + value + ' </label><i class="fa fa-info-circle"></i></a></div>';
                        }
                    },
                    {
                        text: 'ערך', dataField: 'CreditValue', cellsalign: 'right', align: 'center', width: 120
                        //filtertype: "number"
                    },
                    {
                        text: 'ייתרה', dataField: 'CreditStatus', cellsalign: 'right', align: 'center', width: 100
                        //filtertype: "custom",
                        //createfilterpanel: function (datafield, filterPanel) {
                        //    app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        //}
                    },
                    {
                        text: 'יחי-חיוב', dataField: 'Units', cellsalign: 'right', align: 'center', width: 120
                        //filtertype: "number"
                    },
                    {
                        text: 'סוג פעולה', dataField: 'ActionName', cellsalign: 'right', align: 'center', width: 120
                        //filtertype: "checkedlist"
                    },
                    {
                        text: 'פריט', dataField: 'MethodName', cellsalign: 'right', align: 'center', width: 120
                        //filtertype: "checkedlist"
                    },
                    {
                        text: ' מס-שליחה ', dataField: 'TransId', cellsalign: 'right', align: 'center'//, width: 120
                        //filtertype: "custom",
                        //createfilterpanel: function (datafield, filterPanel) {
                        //    app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        //}
                    },
                    {
                        text: 'קמפיין', dataField: 'CampaignId', cellsalign: 'right', align: 'center'//, width: 120
                        //filtertype: "custom",
                        //createfilterpanel: function (datafield, filterPanel) {
                        //    app_jqxgrid.buildFilterPanel(filterPanel, datafield);
                        //}
                    },
                    {
                        text: 'עודכן ב', type: 'date', dataField: 'Creation', cellsformat: 'd', cellsalign: 'right', align: 'center', width: 120
                        //filtertype: "date"
                    }
                ]
                //groups: ['TaskSubject']
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

        this.Loaded = true;
    }

    load(dataModel, userInfo) {

        this.IsMobile = app.IsMobile();
        this.UserId = userInfo.UserId;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;

        if (this.Loaded) {

            this.source.data = {
                //'QueryType': Model.QueryType,
                'AccountId': dataModel.AccountId,
                'ActionType': dataModel.ActionType
                //'DateFrom': Model.DateFrom,
                //'DateTo': Model.DateTo
            };

            $('#jqxgrid').jqxGrid('source').dataBind();
        }
        else {
             this.grid(dataModel);
        }
    }

    reload(dataModel) {

        this.source.data = {
            //'QueryType': Model.QueryType,
            'AccountId': dataModel.AccountId,
            'ActionType': dataModel.ActionType
            //'DateFrom': Model.DateFrom,
            //'DateTo': Model.DateTo
        };

        $('#jqxgrid').jqxGrid('source').dataBind();
    }
}
