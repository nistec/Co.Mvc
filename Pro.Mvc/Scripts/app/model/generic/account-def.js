
//============================================================================================ app_accountProperty_def

function app_account_def(userInfo) {

    
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole >= 9) ? 1 : 0;
    
    $("#AccountId").val(this.AccountId);

    this.gridWith = 800;
    this.sourceData = {};
    this.sourceUrl = '/Admin/GetAccountView';
    this.updateUrl = '/Admin/DefAccountUpdate';
    this.deleteUrl = '/Common/DefAccountUpdate';
    this.rowEdit = -1;

    this.loadControls();
       
    var slf = this;

    
    this.createRowData = function () {
        var row = {
            PropId: $("#PropId").val(), PropName: $("#PropName").val(),
            SmsSender: $("#SmsSender").val(),
            MailSender: $("#MailSender").val(),
            AuthUser: $("#AuthUser").val(),
            AuthAccount: $("#AuthAccount").val(),
            EnableSms: $("#EnableSms").val(),
            EnableMail: $("#EnableMail").val(),
            Path: $("#Path").val(),
            SignupPage: $("#SignupPage").val(),
            EnableInputBuilder: $("#EnableInputBuilder").val(),
            BlockCms: $("#BlockCms").val(),
            SignupOption: $("#SignupOption").val(),
            Design: $("#Design").val(),
            EnableSignupCredit: $("#EnableSignupCredit").val(),
            CreditTerminal: $("#CreditTerminal").val(),
            RecieptEvent: $("#RecieptEvent").val(),
            RecieptAddress: $("#RecieptAddress").val()
    };
        return row;
    }

    this.setInputData = function (dataRecord) {
        var isundefined = (dataRecord === undefined || dataRecord == null);

        $("#PropId").val(isundefined ? '': dataRecord.PropId);
        $("#PropName").val(isundefined ? '' : dataRecord.PropName);
        $("#SmsSender").val(isundefined ? '' : dataRecord.SmsSender);
        $("#MailSender").val(isundefined ? '' : dataRecord.MailSender);
        $("#AuthUser").val(isundefined ? '' : dataRecord.AuthUser);
        $("#AuthAccount").val(isundefined ? '' : dataRecord.AuthAccount);
        $("#EnableSms").val(isundefined ? '' : dataRecord.EnableSms);
        $("#EnableMail").val(isundefined ? '' : dataRecord.EnableMail);
        $("#Path").val(isundefined ? '' : dataRecord.Path);
        $("#SignupPage").val(isundefined ? '' : dataRecord.SignupPage);
        $("#EnableInputBuilder").val(isundefined ? '' : dataRecord.EnableInputBuilder);
        $("#BlockCms").val(isundefined ? '' : dataRecord.BlockCms);
        $("#SignupOption").val(isundefined ? '' : dataRecord.SignupOption);
        $("#Design").val(isundefined ? '' : dataRecord.Design);
        $("#EnableSignupCredit").val(isundefined ? '' : dataRecord.EnableSignupCredit);
        $("#CreditTerminal").val(isundefined ? '' : dataRecord.CreditTerminal);
        $("#RecieptEvent").val(isundefined ? '' : dataRecord.RecieptEvent);
        $("#RecieptAddress").val(isundefined ? '' : dataRecord.RecieptAddress);

     
        //if (dataRecord === undefined || dataRecord==null) {
        //    $("#PropId").val('');
        //    $("#PropName").val('');
        //    $("#SmsSender").val('');
        //    $("#MailSender").val('');
        //    $("#AuthUser").val('');
        //    $("#AuthAccount").val('');
        //    $("#EnableSms").val('');
        //    $("#EnableMail").val('');
        //    $("#Path").val('');
        //    $("#SignupPage").val('');
        //    $("#EnableInputBuilder").val('');
        //    //$("#GoogleAnalytics").val('');
        //    $("#ActiveCampaign").val('');
        //    $("#Design").val('');
        //    $("#EnableSignupCredit").val('');
        //    $("#CreditTerminal").val('');
        //    $("#CreditTerminal").val('');
        //    $("#CreditTerminal").val('');
        //}
        //else {
        //    $("#PropId").val(dataRecord.PropId);
        //    $("#PropName").val(dataRecord.PropName);
        //    $("#SmsSender").val(dataRecord.SmsSender);
        //    $("#MailSender").val(dataRecord.MailSender);
        //    $("#AuthUser").val(dataRecord.AuthUser);
        //    $("#AuthAccount").val(dataRecord.AuthAccount);
        //    $("#EnableSms").val(dataRecord.EnableSms);
        //    $("#EnableMail").val(dataRecord.EnableMail);
        //    $("#Path").val(dataRecord.Path);
        //    $("#SignupPage").val(dataRecord.SignupPage);
        //    $("#EnableInputBuilder").val(dataRecord.EnableInputBuilder);
        //    //$("#GoogleAnalytics").val(dataRecord.GoogleAnalytics);
        //    $("#ActiveCampaign").val(dataRecord.ActiveCampaign);
        //    $("#Design").val(dataRecord.Design);
        //    $("#EnableSignupCredit").val(dataRecord.EnableSignupCredit);
        //    $("#CreditTerminal").val(dataRecord.CreditTerminal);
        //    $("#CreditTerminal").val(dataRecord.CreditTerminal);
        //    $("#CreditTerminal").val(dataRecord.CreditTerminal);
        //}
    }

    this.createCoulmns = function () {

        var columns = [
           { text: 'קוד לקוח', datafield: 'PropId', width: 60, cellsalign: 'right', align: 'center' },
           { text: 'שם לקוח', datafield: 'PropName', cellsalign: 'right', align: 'center' },
           //{ text: 'מספר שולח', datafield: 'SmsSender', width: 100, cellsalign: 'right', align: 'center' },
           //{ text: 'שם מייל', datafield: 'MailSender', width: 100, cellsalign: 'right', align: 'center' },
           //{ text: 'מספר משתמש', datafield: 'AuthUser', width: 100, cellsalign: 'right', align: 'center' },
           { text: 'מספר חשבון', datafield: 'AuthAccount', width: 100, cellsalign: 'right', align: 'center' },
           { text: 'Sms', datafield: 'EnableSms', columntype: 'checkbox', width: 100, cellsalign: 'right', align: 'center' },
           { text: 'Email', datafield: 'EnableMail', columntype: 'checkbox', width: 100, cellsalign: 'right', align: 'center' },
           { text: 'כתובת עמוד רישום', datafield: 'Path', width: 120,  cellsalign: 'right', align: 'center' },
           //{ text: 'שם עמוד רישום', datafield: 'SignupPage', width: 100, cellsalign: 'right', align: 'center' },
           //{ text: 'האם טופס רישום דינאמי', datafield: 'EnableInputBuilder', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
           //{ text: 'קוד גוגל', datafield: 'GoogleAnalytics', width: 100, cellsalign: 'right', align: 'center' },
           //{ text: 'קמפיין פעיל', datafield: 'ActiveCampaign', width: 80, cellsalign: 'right', align: 'center' }
        ];
       return columns;
    }

    this.createFields = function () {
        var datafields =
            [
                { name: 'PropId', type: 'number' },
                { name: 'PropName', type: 'string' },
                { name: 'SmsSender', type: 'string' },
                { name: 'MailSender', type: 'string' },
                { name: 'AuthUser', type: 'number' },
                { name: 'AuthAccount', type: 'number' },
                { name: 'EnableSms', type: 'bool' },
                { name: 'EnableMail', type: 'bool' },
                { name: 'Path', type: 'string' },
                { name: 'SignupPage', type: 'string' },
                { name: 'EnableInputBuilder', type: 'bool' },
                { name: 'BlockCms', type: 'bool' },
                { name: 'SignupOption', type: 'number' },
                { name: 'Design', type: 'string' },
                { name: 'EnableSignupCredit', type: 'bool' },
                { name: 'RecieptEvent', type: 'string' },
                { name: 'RecieptAddress', type: 'string' },
                { name: 'CreditTerminal', type: 'string' }
            ];
        return datafields;
    }
 

    this.getDataCommand = function (rowid, rowdata, command) {

        if (command == 2 && (rowid <= 0))//delete
        {
           app_dialog.alert("Invalid row id to delete!");
            return null;
        }
               

        return {
            'PropId': command == 0 ? -1 : rowdata.PropId, 'PropName': rowdata.PropName,
            'SmsSender': rowdata.SmsSender,
            'MailSender': rowdata.MailSender,
            'AuthUser': rowdata.AuthUser,
            'AuthAccount': rowdata.AuthAccount,
            'EnableSms': rowdata.EnableSms,
            'EnableMail': rowdata.EnableMail,
            'Path': rowdata.Path,
            'SignupPage': rowdata.SignupPage,
            'EnableInputBuilder': rowdata.EnableInputBuilder,
            'BlockCms': rowdata.BlockCms,
            'SignupOption': rowdata.SignupOption,
            'Design': rowdata.Design,
            'EnableSignupCredit': rowdata.EnableSignupCredit,
            'CreditTerminal':  encodeURIComponent(rowdata.CreditTerminal),
            'RecieptEvent': rowdata.RecieptEvent,
            'RecieptAddress': rowdata.RecieptAddress,
            'command': command
        };
    }
 
    this.genericEntity= app_genericEntity_def(this);

};


app_account_def.prototype.loadControls = function () {

    // initialize the input fields.
    $("#PropId").jqxInput().width(200);
    $("#PropName").jqxInput().width(200);
    $("#SmsSender").jqxInput().width(200);
    $("#MailSender").jqxInput().width(200);
    $("#AuthUser").jqxInput().width(200);
    $("#AuthAccount").jqxInput().width(200);
    $("#EnableSms").jqxCheckBox().width(200);
    $("#EnableMail").jqxCheckBox().width(200);
    $("#Path").jqxInput().width(200);
    $("#SignupPage").jqxInput().width(200);
    $("#EnableInputBuilder").jqxCheckBox().width(200);
    $("#BlockCms").jqxCheckBox().width(200);
    //$("#SignupOption").jqxInput().width(200);
    $("#Design").jqxInput().width(200);
    $("#EnableSignupCredit").jqxCheckBox().width(200);
    $("#CreditTerminal").jqxInput().width(200);
    //$("#RecieptEvent").jqxdropdown().width(200);
    $("#RecieptAddress").jqxInput().width(200);

    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 600, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var input_rules = [
             { input: '#PropName', message: 'חובה לציין שם לקוח!', action: 'keyup, blur', rule: 'required' }
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
