
//============================================================================================ app_signup_grid

app_view_grid = {

   
    dataAdapter: {},
    AllowEdit:0,
    source:
        {
            datatype: "json",
            async: false,
            datafields: [
                { name: 'SignupId', type: 'number' },
                { name: 'SignupDate', type: 'date' },
                { name: 'ReferralCode', type: 'string' },
                { name: 'AutoCharge', type: 'bool' },
                { name: 'RegHostAddress', type: 'string' },
                { name: 'RegReferrer', type: 'string' },
                //{ name: 'CreditCardOwner', type: 'string' },
                //{ name: 'ConfirmAgreement', type: 'string' },
                { name: 'SignKey', type: 'string' },
                { name: 'Price', type: 'number' },
                { name: 'ItemId', type: 'number' },
                { name: 'ValidityMonth', type: 'number' },
                { name: 'MemberRecord', type: 'number' },
                //{ name: 'Campaign', type: 'number' },
                { name: 'SignupOrder', type: 'number' },
                { name: 'ExpirationDate', type: 'date' },
                { name: 'MemberId', type: 'string' },
                { name: 'MemberName', type: 'string' },
                { name: 'CellPhone', type: 'string' }
                //{ name: 'CampaignName', type: 'string' },
                //{ name: 'ItemName', type: 'string' },
                //{ name: 'CampaignName', type: 'string' }
                ],

            id: 'SignupId',
            type: 'POST',
            url: '/Common/UnpayedGridGet',
            //data:{},
            pagenum: 0,
            pagesize: 15
            //root: 'Rows',
            //pager: function (pagenum, pagesize, oldpagenum) {
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

    getTotalRows: function (data) {
        if (data) {
            return dataTotalRows(data);
        }
        return 0;
    },

     grid: function () {
        var slf = this;

        var renderstatusbar = function (statusbar) {
            // appends buttons to the status bar.
            var container = $("<div style='overflow: hidden; position: relative; margin: 5px;float:right;'></div>");
            var reloadButton = $("<div style='float: left; margin-left: 5px;' title='רענון'><img src='../scripts/app/images/refresh.gif'><span style='margin-left: 4px; position: relative;'>רענון</span></div>");
            var clearFilterButton = $("<div style='float: left; margin-left: 5px;' title='הסר סינון' ><img src='../scripts/app/images/filterRemove.gif'><span style='margin-left: 4px; position: relative;'>הסר סינון</span></div>");
            container.append(reloadButton);
            container.append(clearFilterButton);
            statusbar.append(container);

            reloadButton.jqxButton({ width: 70, height: 20 });
            clearFilterButton.jqxButton({ width: 70, height: 20 });

            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
        };

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
        {
            width: '99%',
            autoheight: true,
            rtl: true,
            source: slf.dataAdapter,
            localization: getLocalization('he'),
            virtualmode: false,
            rendergridrows: function (obj) {
                return slf.dataAdapter.records;
            },
            pageable: true,
            pagermode: 'simple',
            sortable: true,
            //showfilterrow: true,
            //filterable: true,
            rowdetails: false,
            showstatusbar: true,
            renderstatusbar:renderstatusbar,
            columns: [
              { text: 'מס-רישום', dataField: 'SignupId',width:90, cellsalign: 'right', align: 'center' },
              { text: 'ת.ז', dataField: 'MemberId', cellsalign: 'right', align: 'center' },
              { text: 'שם מלא', dataField: 'MemberName', cellsalign: 'right', align: 'center'},
              //{ text: 'קמפיין', dataField: 'CampaignName', cellsalign: 'right', align: 'center'},
              //{ text: 'מזהה רישום', dataField: 'SignKey', cellsalign: 'right', align: 'center' },
              { text: 'מחיר', dataField: 'Price', cellsalign: 'right', align: 'center'},
              { text: 'טלפון נייד', dataField: 'CellPhone', width: 120, cellsalign: 'right', align: 'center'},
              { text: 'סיום תוקף', type: 'date', dataField: 'ExpirationDate', width: 120, cellsformat: 'd',cellsalign: 'right', align: 'center' },
              { text: 'מועד רישום', type: 'date', dataField: 'SignupDate', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
            ]
        });
    },
     loadFilter:function(){

         var columnList = [
                { label: 'ת.ז המנוי', value: 'MemberId', checked: false },
                { label: 'שם המנוי', value: 'MemberName', checked: false },
                { label: 'טלפון נייד', value: 'CellPhone', checked: false },
                //{ label: 'דואר אלקטרוני', value: 'Email', checked: false },
                //{ label: 'עיר', value: 'CityName', checked: false },
                //{ label: 'כתובת', value: 'Address', checked: false },
                { label: 'מס רישום', value: 'SignupId', checked: false },
                { label: 'תוקף', value: 'ValidityMonth', checked: true },
                { label: 'המשלם', value: 'CreditCardOwner', checked: true },
                { label: 'חיוב חוזר', value: 'AutoCharge', checked: true },
                //{ label: 'קמפיין', value: 'CampaignName', checked: true },
                { label: 'מזהה רישום', value: 'SignKey', checked: false },
                { label: 'קוד הפנייה', value: 'ReferralCode', checked: false },
                //{ label: 'פריט', value: 'ItemName', checked: false },
                { label: 'מחיר', value: 'Price', checked: false },
         ];
         var dateList = [
          { label: 'מועד רישום', value: 'SignupDate', checked: false },
          { label: 'תאריך תפוגה', value: 'ExpirationDate', checked: false }
         ];
         app_jqxgrid.gridFilterEx($("#jqxgrid"), $("#grid-toolbar"), columnList, dateList);
     },
    load: function (Model,userInfo) {

        this.AllowEdit = userInfo.AllowEdit ? 1 : 0;
        this.source.data = {};
        this.dataAdapter = new $.jqx.dataAdapter(this.source, {
            loadComplete: function (data) {
                //source.totalrecords = getTotalRows(data);
            },
            loadError: function (xhr, status, error) {app_dialog.alert(' status: ' + status + '\n error ' + error) }
        });

        this.grid();
        this.loadFilter();

        return this;
    }
};
