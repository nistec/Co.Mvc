
//============================================================================================ app_signup_def

function registry_index() {
    
};
function registry_credit() {
    
};
function registry_confirm() {
    
};

function registry_signup() {

    //$("#stp-2").hide();
    if ($("#Birthday").length) {
        $('#Birthday').jqxDateTimeInput({ showCalendarButton: false, width: '100%', rtl: true });
        $('#Birthday').val('');
    }
    if ($("#City").length)
        app_jqxcombos.createComboAdapter("PropId", "PropName", "City", '/Registry/GetCityView', '100%', 120, false);
    if ($("#Gender").length)
        app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender", '/Registry/GetGenderView', '100%', 0, false);

    if ($("#aStatement").length) {
        var folder = $("#model-folder").val();
        $("#aStatement").click(function () {
            app_dialog.dialogIframe("/Registry/_Statement/" + folder, '600px', "400px", "הצהרה", true);
        });
    }

    //this.loadControls();

    /*
        function addAccountTag(form) {
            var accid = $("#AccountId").val();
            if ($(form).has("#AccountId").length==false) {
                $('<input>').attr({
                    type: 'hidden',
                    id: 'AccountId',
                    name: 'AccountId',
                    value: accid
                }).appendTo(form);
            }
        }
        function onNext1Validation(data) {
    
            if (data.Status < 0)
            {
                if (data.Status = -1) {
                    //vex.alert(data.Message);
                    vex.dialog.confirm({
                        message: data.Message + '\n' + ' האם לעדכן את פרטיך האישיים?',
                        callback: function (value) {
                            if (value) {
                                addAccountTag("#form1");
                                app_jqxform.jqxForm_Redirect("#form1", "/Registry/Msg/"+folder+"?m=MemberSaved", "/Registry/UpdateMember");
                            }
                        }
                    });
                }
                else
                    vex.dialog.alert(data.Message);
            }
            else //if(status==0)
            {
                $("#stp-1").hide();
                $("#stp-2").show();
    
            }
        }
          
        $("#Next1").click(function (e) {
         
            //e.preventDefault();
            var validationResult = function (isValid) {
                if (isValid) {
                    app_validation.signupValididty($("#AccountId").val(), $("#MemberId").val(), $("#CellPhone").val(), $("#Email").val(), $("#ExId").val(), onNext1Validation);
                }
            }
            $('#form1').jqxValidator('validate', validationResult);
        });
     
        $("#Prev2").click(function () {
            $("#stp-1").show();
            $("#stp-2").hide();
        });
    */

    $('#Submit').on('click', function (e) {

        e.preventDefault();
        var action = $("#form1").attr('action');
        var folder = $("#model-folder").val();
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: action,
                    type: 'POST',
                    dataType: 'json',
                    data: $('#form1').serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            if( $("#model-credit").val()=="1")
                                app.redirectTo("/Registry/Credit/" + folder + "?m=" + data.Link);
                            else
                                app.redirectTo("/Registry/Msg/" + folder + "?m=SignupSuccess");
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    error: function (jqXHR, status, error) {
                        alert(error);
                    }
                });
            }
        }
        $('#form1').jqxValidator('validate', validationResult);
    });
};

registry_signup.prototype.loadControls = function () {

    var input_rules = [];

    if ($("#FirstName").length) 
        input_rules.push({ input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' });
    if ($("#LastName").length) 
        input_rules.push( { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' });
    if ($("#Address").length) 
        input_rules.push({ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' });
    if ($("#CellPhone").length) {
        input_rules.push({ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        input_rules.push({
            input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
            function (input, commit) {
                var val = input.val();
                var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                return val ? re.test(val) : true;
            }
        });
    }
    if ($("#Email").length) {
        input_rules.push({ input: '#Email', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' });
        input_rules.push({ input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' });

    }
    if ($("#Birthday").length) {
        input_rules.push({
            input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'valueChanged', rule: function (input, commit) {
                var date = $('#Birthday').jqxDateTimeInput('value');
                return !(date == null || date === undefined);
            }
        });
        input_rules.push({
            input: '#Birthday', message: 'תאריל לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
                var date = $('#Birthday').jqxDateTimeInput('value');
                if (date == null || date === undefined)
                    return false;
                return (date.getFullYear() >= 1900 && date.getFullYear() <= 2000);
            }
        });
    }

    
    if ($("#City").length) {
        input_rules.push({
            input: "#City", message: 'חובה לציין עיר!', action: 'select', rule: function (input, commit) {
                var index = $("#City").jqxComboBox('getSelectedIndex');
                return index != -1;
            }
        });
    }
    if ($("#Gender").length) {
 
        input_rules.push({
            input: "#Gender", message: 'חובה לציין מין!', action: 'select', rule: function (input, commit) {
                var index = $("#Gender").jqxComboBox('getSelectedIndex');
                return index != -1;
            }
        });
    }
    if ($("#Phone").length) {
        input_rules.push({
            input: '#Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
            function (input, commit) {
                var val = input.val();
                var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                return val ? re.test(val) : true;
            }
        });
    }
    if ($("#MemberId").length) {

        input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
        input_rules.push({
            input: "#MemberId", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
                var value = $("#MemberId").val();
                return app_validation.memberId(value) == 1;//value.length > 4 && $.isNumeric(value);
            }
        });
    }
    if ($("#ExId").length) {
        input_rules.push({ input: '#ExId', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });
    }
    if ($("#ItemId").length) {
        input_rules.push({
            input: "#ItemId", message: 'חובה לציין דמי מנוי!', action: 'select', rule: function (input, commit) {
                var index = $("#ItemId").jqxComboBox('getSelectedIndex');
                return index != -1;
            }
        });
    }
    if ($("#ConfirmAgreement").length) {
        input_rules.push({
            input: "#ConfirmAgreement", message: 'חובה לקרוא ולאשר הצהרת מנוי!', action: 'change, blur', rule: function (input, commit) {
                return $("#ConfirmAgreement").prop("checked") == true;
            }
        });
    }

    $('#form1').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });
};

