
//============================================================================================ app_city_def

function app_city_def(userInfo) {

    
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = 0;// (this.UserRole > 4) ? 1 : 0;
    
    $("#AccountId").val(0);//this.AccountId);
    this.gridWith = 500;
    this.sourceData = {};
    this.sourceUrl='/Common/GetCityRegionViewAll';
    this.updateUrl = '';// '/Common/DefCityUpdate';
    this.deleteUrl = '';// '/Common/DefCityDelete';
    this.rowEdit = -1;

    this.loadControls();
       
    var slf = this;

    this.createRowData = function () {
        var row = {
            PropId: $("#PropId").val(), PropName: $("#PropName").val(), RegionId: $("#RegionId").val()
        };
        return row;
    }

    this.setInputData = function (dataRecord) {
        if (dataRecord === undefined || dataRecord==null) {
            $("#PropId").val('');
            $("#PropName").val('');
            $("#RegionId").val('');
        }
        else {
            $("#PropId").val(dataRecord.PropId);
            $("#PropName").val(dataRecord.PropName);
            $("#RegionId").val(dataRecord.RegionId);
        }
    }

    this.createCoulmns = function () {

        var columns = [
           { text: 'קוד עיר', datafield: 'PropId', width: 60, cellsalign: 'right', align: 'center' },
           { text: 'שם עיר', datafield: 'PropName', cellsalign: 'right', align: 'center' },
           { text: 'אזור', datafield: 'RegionName', width: 100, cellsalign: 'right', align: 'center' }
        ];
       return columns;
    }

    this.createFields = function () {
        var datafields =
            [
                { name: 'PropId', type: 'number' },
                { name: 'PropName', type: 'string' },
                { name: 'RegionId', type: 'number' },
                { name: 'RegionName', type: 'string' }
            ];
        return datafields;
    }

    this.getDataCommand = function (rowid, rowdata, command) {

        if (command == 2)//delete
        {
            if (rowid <= 0) {
               app_dialog.alert("Invalid row id to delete!");
                return null;
            }
            return { 'PropId': rowid };
        }
        else
            return { 'PropId': command == 0 ? -1 : rowdata.PropId, 'PropName': rowdata.PropName, 'RegionId': rowdata.RegionId, 'command': command };
    }
       
       

    this.genericEntity= app_genericEntity_def(this);
};

app_city_def.prototype.loadControls = function () {

    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var zoneSource =
        {
            dataType: "json",
            dataFields: [
                { name: 'PropId' },
                { name: 'PropName' }
            ],
            data: {},
            type: 'POST',
            url: '/Common/GetRegionView'
        };
    var zoneAdapter = new $.jqx.dataAdapter(zoneSource, {
        contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
        },
        loadComplete: function (data) {
            //alert("zoneAdapter is Loaded");
        }
    });
    // perform Data Binding.
    zoneAdapter.dataBind();
    $("#RegionId").jqxDropDownList(
          {
              rtl: true,
              source: zoneAdapter,
              width: 200,
              displayMember: 'PropName',
              valueMember: 'PropId'
          });


    // initialize the input fields.
    $("#PropId").jqxInput().width(200);
    $("#PropName").jqxInput().width(200);
   
   
    var input_rules = [
             { input: '#PropName', message: 'חובה לציין שם עיר!', action: 'keyup, blur', rule: 'required' },
             {
                 input: '#RegionId', message: 'חובה לציין אזור!', action: 'keyup, blur', rule: function () {
                     return app_jqx_validation.validateDropDown("RegionId");
                 }
             }
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
