
//============================================================================================ app_campaign_def

function app_campaign_def(userInfo) {

    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

    $("#AccountId").val(this.AccountId);
    this.gridWith = 700;
    this.sourceData = {};
    this.sourceUrl = '/Common/GetCampaignView';
    this.updateUrl = '/Common/DefCampaignUpdate';
    this.deleteUrl = '/Common/DefCampaignDelete';
    this.rowEdit = -1;

    this.loadControls();

    var slf = this;

    this.createRowData = function () {
        var row = {
            PropId: $("#PropId").val(), PropName: $("#PropName").val(), IsActive: $("#IsActive").val(), ValidityMonth: $("#ValidityMonth").val(), ValidDiff: $("#ValidDiff").val(), ValidityDate: $("#ValidityDate").val(), DefaultCategory: $("#DefaultCategory").val(), IsSignupCredit: $("#IsSignupCredit").val()
        };
        return row;
    }
    this.setInputData = function (dataRecord) {
        if (dataRecord === undefined || dataRecord == null) {
            $("#PropId").val('');
            $("#PropName").val('');
            $("#IsActive").val('');
            $("#ValidityMonth").val('');
            $("#ValidDiff").val('');
            $("#ValidityDate").val('');
            $("#DefaultCategory").val('');
            $("#IsSignupCredit").val('');
        }
        else {
            $("#PropId").val(dataRecord.PropId);
            $("#PropName").val(dataRecord.PropName);
            $("#IsActive").val(dataRecord.IsActive);
            $("#ValidityMonth").val(dataRecord.ValidityMonth);
            $("#ValidDiff").val(dataRecord.ValidDiff);
            $("#ValidityDate").val(dataRecord.ValidityDate);
            $("#DefaultCategory").val(dataRecord.DefaultCategory);
            $("#IsSignupCredit").val(dataRecord.IsSignupCredit);
        }
    }

    this.createCoulmns = function () {

        var columns = [
           { text: 'קוד קמפיין', datafield: 'PropId',  cellsalign: 'right', align: 'center' },
           { text: 'שם קמפיין', datafield: 'PropName', cellsalign: 'right', align: 'center' },
           { text: 'פעיל', datafield: 'IsActive',columntype: 'checkbox' ,cellsalign: 'right', align: 'center' },
           { text: 'תוקף בחודשים', datafield: 'ValidityMonth',  cellsalign: 'right', align: 'center' },
           { text: 'הפרש תקף בימים', datafield: 'ValidDiff', cellsalign: 'right', align: 'center' },
           { text: 'מועד פקיעה', datafield: 'ValidityDate', type: 'date', cellsformat: 'd', cellsalign: 'right', align: 'center' },
           { text: 'חיוב באשראי', datafield: 'IsSignupCredit',  columntype: 'checkbox', cellsalign: 'right', align: 'center' }
    ];
        return columns;
    }

    this.createFields = function () {
        var datafields =
            [
                { name: 'PropId', type: 'number' },
                { name: 'PropName', type: 'string' },
                { name: 'IsActive', type: 'bool' },
                { name: 'ValidityMonth', type: 'number' },
                { name: 'ValidDiff', type: 'number' },
                { name: 'ValidityDate', type: 'date' },
                { name: 'DefaultCategory', type: 'number' },
                { name: 'IsSignupCredit', type: 'bool' }
            ];
        return datafields;
    }

    this.getDataCommand = function (rowid, rowdata, command) {

        var category = 0;
        var isActive = false;
        if (rowdata != null) {
            if (rowdata.DefaultCategory == null || rowdata.DefaultCategory == "")
                category = 0;
            else
                category = rowdata.DefaultCategory

            isActive = rowdata.IsActive == null ? false : rowdata.IsActive;
        }
        if (command == 2)//delete
        {
            if (rowid <= 0) {
               app_dialog.alert("Invalid row id to delete!");
                return null;
            }
            return { 'PropId': rowid };
        }
        else
            return { 'PropId': command == 0 ? -1 : rowdata.PropId, 'PropName': rowdata.PropName, 'IsActive': isActive, 'ValidityMonth': rowdata.ValidityMonth, 'ValidDiff': rowdata.ValidDiff, 'ValidityDate': rowdata.ValidityDate, 'DefaultCategory': category, 'IsSignupCredit': rowdata.IsSignupCredit, 'command': command };
    }

    this.genericEntity = app_genericEntity_def(this);
};

app_campaign_def.prototype.loadControls = function () {

    // initialize the input fields.
    $("#PropId").jqxInput().width(200);
    $("#PropName").jqxInput().width(200);
    $("#IsActive").jqxCheckBox({ width: 200, hasThreeStates: false, checked: false });
    $("#ValidityMonth").jqxInput().width(200);
    $("#ValidDiff").jqxInput().width(200);
    $("#ValidityDate").jqxDateTimeInput();//'setMinDate', new Date());
    $("#IsSignupCredit").jqxCheckBox({ width: 200, hasThreeStates: false, checked: false });

    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var categorySource =
    {
        dataType: "json",
        dataFields: [
            { name: 'PropId' },
            { name: 'PropName' }
        ],
        data: {},
        type: 'POST',
        url: '/Common/GetCategoriesView'
    };
    var categoryAdapter = new $.jqx.dataAdapter(categorySource, {
        contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
        },
        loadComplete: function (data) {
            //alert("zoneAdapter is Loaded");
        }
    });
    // perform Data Binding.
    categoryAdapter.dataBind();
    $("#DefaultCategory").jqxDropDownList(
          {
              rtl: true,
              placeHolder: "בחירת סיווג:",
              source: categoryAdapter,
              width: 200,
              displayMember: 'PropName',
              valueMember: 'PropId'
          });

   
    var input_rules = [
             { input: '#PropName', message: 'חובה לציין שם קמפיין!', action: 'keyup, blur', rule: 'required' },
             { input: '#ValidDiff', message: 'חובה לציין הפרש תקף או 0!', action: 'keyup, blur', rule: 'required' },
             {
                 input: '#ValidityMonth', message: 'חובה לציין תוקף או לציין 0!', action: 'keyup, blur', rule: function () {
                     return app_jqx_validation.validateNumberWzero("ValidityMonth");
                 }
             }
            //{
            //    input: '#DefaultCategory', message: 'חובה לציין קטגוריה ברירת מחדל!', action: 'keyup, blur', rule: function () {
            //        return app_jqx_validation.validateDropDown("DefaultCategory");
            //    }
            //}
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
