
//============================================================================================ app_members_def

function app_members_def(recordId,userInfo,isdialog) {

    this.RecordId = recordId;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    this.IsDialog = isdialog;

    $("#AccountId").val(this.AccountId);
    

    this.loadControls();
     
    this.doSubmit = function () {
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
                            if (slf.IsDialog) {
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
    
    var slf = this;

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
    data: { 'id': slf.RecordId },
    type: 'POST',
    url: '/Crm/GetMemberEdit'
  };

    this.viewAdapter = new $.jqx.dataAdapter(view_source, {
        loadComplete: function (record) {

            slf.syncData(record);
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });

    if (this.RecordId > 0) {
        this.viewAdapter.dataBind();
    }


};

app_members_def.prototype.syncData = function (record) {

    if (record) {

        app_jqxform.loadDataForm("fcForm", record);
      
        app_jqxcombos.selectCheckList("listCategory", record.Categories);

        app_jqxcombos.initComboValue('City', 0);
    }
};

app_members_def.prototype.loadControls = function () {

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
    if (exType==0)
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




    /*

    $('#fcForm').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: [
              { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
              { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
              { input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
               {
                   input: "#City", message: 'חובה לציין עיר!', action: 'blur', rule: function (input, commit) {
                       var index = $("#City").jqxComboBox('getSelectedIndex');
                       return index != -1;
                   }
               },
              //{ input: '#City', message: 'חובה לציין עיר!', action: 'keyup, blur', rule: 'required' },
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
        ]
    });
*/
};
