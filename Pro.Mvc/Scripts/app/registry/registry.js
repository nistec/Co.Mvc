
//============================================================================================ app_signup_def


function registry_index() {
    
};
function registry_credit() {
    
};
function registry_confirm() {
    
};

function registry_signup(accountId) {

    this.AccountId = accountId;
    
    $("#AccountId").val(accountId);
    //CreateDateTimeInput("Birthday");
    $('#Birthday').jqxDateTimeInput({ showCalendarButton: false, width: '120px' });

    app_jqxcombos.createComboAdapter("PropId", "PropName", "City", '/Registry/GetCityView?aid=' + accountId, 0, 120, false);
    app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender", '/Registry/GetGenderView?aid=' + accountId, 0, 0, false);

    this.loadControls();

    //var len = $('#accForm').jqxValidator({rules});
    //alert(len)

    //var r={ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' }

    //$('#accForm').jqxValidator().rules.add(r);

    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#accForm').attr('action');
        //app_jqxcombos.renderCheckList("listCategory", "Categories");
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#accForm').serialize(),
                    success: function (data) {
                       app_dialog.alert(data.Message);
                        if (data.Status >= 0) {
                                app.refresh();
                        }
                    },
                    error: function (jqXHR, status, error) {
                       app_dialog.alert(error);
                    }
                });
            }
        }
        $('#accForm').jqxValidator('validate', validationResult);
    };

};

registry_signup.prototype.loadControls = function () {


    //app_jqx_list.branchComboAdapter();
    //app_jqx_list.placeComboAdapter();
    //app_jqx_list.statusComboAdapter();
    //app_jqx_list.regionComboAdapter();



    //app_jqx_list.cityComboAdapter("City");
    //app_jqx_list.genderComboAdapter();
    //app_jqx_list.categoryCheckListAdapter();

    var input_rules = [
              { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
              { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
              { input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
              {
                  input: "#City", message: 'חובה לציין עיר!', action: 'select', rule: function (input, commit) {
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
    ];

    input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#accForm').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
