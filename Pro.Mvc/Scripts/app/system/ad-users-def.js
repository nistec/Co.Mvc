
//============================================================================================ app_ad_user_def

function app_ad_user_def(userInfo) {

    
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    
    $("#AccountId").val(this.AccountId);
    this.gridWith = 500;
    this.sourceData = {};
    this.sourceUrl = '/System/AdUserDefList';
    this.updateUrl = '/System/AdUserDefUpdate';
    this.deleteUrl = '/System/AdUserDefDelete';
    this.insertUrl = '/System/AdUserDefInsert';
    this.rowEdit = -1;

    //this.showMemmbersUrl = '/System/AdUserShowMembers';
    this.fieldId = 'UserId';

    this.loadControls();
       
    var slf = this;

    this.createRowData = function () {
        var creation = $("#Creation").val();
        var item = $("#UserRole").jqxDropDownList('getSelectedItem');
        //var UserRole=item.value
        //var RoleName=item.label;

        var row = {
            UserId: $("#UserId").val(),
            UserName: $("#UserName").val(),
            UserRole: item.value,
            RoleName: item.label,
            Email: $("#Email").val(),
            Phone: $("#Phone").val(),
            AccountId: $("#AccountId").val(),
            Lang: $("#Lang").val(),
            Evaluation: 0,
            IsBlocked: $("#IsBlocked").val(),
            DisplayName: $("#DisplayName").val(),
            Creation: app.toLocalDateString(creation)
        };
        return row;
    }

    this.setEditorInputData = function (dataRecord) {

        var isnew = (dataRecord === undefined || dataRecord == null);
        if (isnew) {
            $("#trcode").hide();
            $("#isReset").hide();
        }
        else {
            $("#trcode").show();
            $("#isReset").show();
        }
        $("#UserId").val(isnew ? '' : dataRecord.UserId);
        $("#UserName").val(isnew ? '' : dataRecord.UserName);
        $("#DisplayName").val(isnew ? '' : dataRecord.DisplayName);
        $("#UserRole").val(isnew ? '' : dataRecord.UserRole);
        $("#Email").val(isnew ? '' : dataRecord.Email);
        $("#Phone").val(isnew ? '' : dataRecord.Phone);
        $("#AccountId").val(isnew ? '' : dataRecord.AccountId);
        $("#Lang").val(isnew ? '' : dataRecord.Lang);
        $("#Evaluation").val(isnew ? '' : dataRecord.Evaluation);
        $("#IsBlocked").val(isnew ? '' : dataRecord.IsBlocked);
        $("#Creation").val(isnew ? '' : dataRecord.Creation);
    }

    this.createCoulmns = function () {
        var columns = [
        { text: 'קוד משתמש', datafield: 'UserId', width: 90, cellsalign: 'right', align: 'center' },
        //{ text: 'קוד תפקיד', datafield: 'UserRole',width: 60, cellsalign: 'right', align: 'center' },
        { text: 'קוד הרשאה', datafield: 'RoleName', width: 90, cellsalign: 'right', align: 'center' },
        { text: 'שם משתמש', datafield: 'UserName', width: 120, cellsalign: 'right', align: 'center' },
        { text: 'פרטים', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
        { text: 'אימייל', datafield: 'Email', cellsalign: 'right', align: 'center' },
        { text: 'טלפון', datafield: 'Phone', width: 120, cellsalign: 'right', align: 'center' },
        //{ text: 'חשבון', datafield: 'AccountId', cellsalign: 'right', align: 'center' },
        //{ text: 'נסיון', datafield: 'Evaluation', width: 60,cellsalign: 'right', align: 'center' },
        { text: 'חסום', datafield: 'IsBlocked', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
        { text: 'נוצר ב', datafield: 'Creation', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
        ];
        return columns;
    }

    this.createFields = function () {
        var datafields =
            [
           { name: 'DisplayName', type: 'string' },
           { name: 'UserId', type: 'number' },
           { name: 'UserRole', type: 'number' },
           { name: 'RoleName', type: 'string' },
           { name: 'UserName', type: 'string' },
           { name: 'Email', type: 'string' },
           { name: 'Phone', type: 'string' },
           { name: 'AccountId', type: 'number' },
           { name: 'Lang', type: 'string' },
           { name: 'Evaluation', type: 'number' },
           { name: 'IsBlocked', type: 'bool' },
           { name: 'Creation', type: 'date' }
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
            return { 'UserId': rowid };
        }
        else
            return {
                'UserId': command == 0 ? -1 : rowdata.UserId,
                'UserName': rowdata.UserName,
                'DisplayName': rowdata.DisplayName,
                'UserRole': rowdata.UserRole,
                'Email': rowdata.Email,
                'Phone': rowdata.Phone,
                'AccountId': rowdata.AccountId,
                'Lang': rowdata.Lang,
                'Evaluation': rowdata.Evaluation,
                'IsBlocked': rowdata.IsBlocked,
                'Creation': rowdata.Creation
            };
    }
    //$("#jqxgrid").jqxGrid({ showstatusbar: true, pagerbuttonscount: 3, pageable: true, pagermode: 'simple' });
    
    app_jqxgrid.loadEntityGrid(this, "#jqxgrid", true);
    
    //this.genericEntity= app_genericEntity_def(this);
};

app_ad_user_def.prototype.loadControls = function () {

    var slf = this;

    // initialize the input fields.
    $("#UserRole").jqxDropDownList().width(150);
    $("#IsBlocked").jqxCheckBox();
    $("#IsResetPass").jqxCheckBox();
    if (this.AllowEdit == 0) {
        $("#UserRole").jqxDropDownList("disabled", true);
        $("#IsBlocked").jqxCheckBox("disabled", true);
    }
   

    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    //$("#ResetPass").jqxButton();
    
    //$("#ResetPass").click(function () {

    //    app_query.doDataPost(slf.RelUpdateUrl, data, function (data) {
    //        $('#jqxgrid').jqxGrid('source').dataBind();
    //        slf.nastedGridLoder(id, false);
    //    });

    //});

    $("#popupWindow").jqxWindow({
        width: 320, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var input_rules = [
              { input: '#UserName', message: 'נדרש שם משתמש!', action: 'keyup, blur', rule: 'required' },
              { input: '#UserName', message: 'שם משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup, blur', rule: 'length=3,12' },
              {
                  input: '#UserName', message: 'נדרש אותיות באנגלית בלבד!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^[A-Za-z][A-Za-z0-9]*$/
                              return re.test(input.val());
                          }
              },
              { input: '#DisplayName', message: 'נדרש פרטי משתמש!', action: 'keyup, blur', rule: 'required' },
              { input: '#DisplayName', message: 'פרטי משתמש אותיות בלבד!', action: 'keyup', rule: 'notNumber' },
              { input: '#DisplayName', message: 'פרטי משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup', rule: 'length=3,12' },
              { input: '#Email', message: 'נדרש כתובת אימייל!', action: 'keyup, blur', rule: 'required' },
              { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
              //{ input: '#UserRole', message: 'נדרש תפקיד!', action: 'valuechanged, blur', rule: 'required' },
              {
                  input: '#UserRole', message: 'נדרש תפקיד!', action: 'keyup, select', rule: function (input) {
                      var index = $("#UserRole").jqxDropDownList('getSelectedIndex');
                      if (index >= 0) { return true; } return false;
                  }
              },
              { input: '#Phone', message: 'נדרש טלפון נייד!', action: 'keyup, blur', rule: 'required' },
              {
                  input: '#Phone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                              return re.test(input.val());
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

    // prepare the data
    var roleSource =
    {
        //async:false,
        dataType: "json",
        dataFields: [
            { name: 'RoleId' },
            { name: 'RoleName' }
        ],
        data: {},
        type: 'POST',
        url: '/System/GetUsersRoles'
    };
    var roleAdapter = new $.jqx.dataAdapter(roleSource, {
        contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
            //alert("roleAdapter failed: " + error);
        },
        loadComplete: function (data) {
            //alert("roleAdapter is Loaded");
        }
    });
    // perform Data Binding.
    //roleAdapter.dataBind();
    $("#UserRole").jqxDropDownList(
          {
              rtl: true,
              source: roleAdapter,
              width: 120,
              autoDropDownHeight: true,
              dropDownHorizontalAlignment: 'right',
              promptText: "בחירת תפקיד",
              displayMember: 'RoleName',
              valueMember: 'RoleId'
          });

};
