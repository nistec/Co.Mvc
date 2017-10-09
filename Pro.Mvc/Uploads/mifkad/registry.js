
//============================================================================================ app_signup_def

function registry_index() {
    this.AccountId = 2;
};
function registry_credit() {
    this.AccountId = 2;
};
function registry_confirm() {
    this.AccountId = 2;
};

function registry_custom() {

    var ismember = app.requestQuery("m");
    this.AccountId = 2;
  
    if (ismember == "y") {
        $("#hIsmem").show();
        $("#hIsnone").hide();

    }
    else {
        $("#hIsmem").hide();
        $("#hIsnone").show();
    }

    $("#stp-2").hide();
    //$('#Birthday').jqxDateTimeInput({ width: '100%', rtl: true, formatString : "dd/MM/yyyy"});//showCalendarButton: false, 
    //$('#Birthday').val('');
    $('#Birthday').datepicker({
        regional: ["he"],
        isRTL: true,
        yearRange: "1925:1999",
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'dd/mm/yy',
        onSelect: function (dateText, inst) {
            $('#form1').jqxValidator('validateInput', '#Birthday');
        }
    });
 
    //$("#Birthday").focus(function () {
    //    //$(".ui-datepicker-calendar").hide();
    //    $(".ui-datepicker-close").click(function(){
    //        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
    //        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
    //        var day = $("#ui-datepicker-div .ui-datepicker-day :selected").val();
    //        $("#Birthday").datepicker('setDate', new Date(year, month, day));
    //    });
    //});


    //app_jqx_list.cityComboAdapter();

    app_jqxcombos.createComboAdapter("PropId", "PropName", "City", '/Registry/GetCityView', '100%', 120, false);
    app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender", '/Registry/GetGenderView', '100%', 0, false);

    this.loadControls();
    
    $("#aStatement").click(function () {
        app_dialog.dialogIframe("/Registry/_Statement/Mifkad", '600px', "400px", "הצהרה", true);
    });
   
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
            app_dialog.alert(data.Message);

            //if (data.Status = -1) {
            //    vapp_dialog.alert(data.Message);
            //    //app_jqxform.jqxForm_Redirect("#form1", "/Registry/Msg/Mifkad?m=MemberSaved", "/Registry/UpdateMember");
            //    //vex.alert(data.Message);
            //    //vex.dialog.confirm({
            //    //    message: data.Message + '\n' + ' האם לעדכן את פרטיך האישיים?',
            //    //    callback: function (value) {
            //    //        if (value) {
            //    //            addAccountTag("#form1");
            //    //            app_jqxform.jqxForm_Redirect("#form1", "/Registry/Msg/Mifkad?m=MemberSaved", "/Registry/UpdateMember");
            //    //        }
            //    //    }
            //    //});
            //}
            //else {
            //    vapp_dialog.alert(data.Message);
            //}
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

    function getCreditCardOwner(value)
    {
        switch(value)
        {

            case "1":
                return "המתפקד/ת";
            case "2":
                return "בן/בת זוגו של המתפקד/ת";
            case "3":
                return "הורה של המתפקד/ת (למתפקדים עד גיל 25 בלבד)";
            case "4":
                return "בנו/בתו של המתפקד/ת";
            default:
                return "המתפקד/ת";
        }

    }
        

    function setSignKey() {
        var acc = $("#AccountId").val();
        var g = app.UUID();
        var signKey = acc + "_" + g.toString().replace("-", "");
        $("#SignKey").val(signKey);
    }

    $('#Submit').on('click', function (e) {
        e.preventDefault();
        var action = $("#form2").attr('action');
        $("#ItemId").val(app_form.radioSelectedValue("__ItemId"));
        //$("#CreditCardOwner").val(app_form.radioSelectedValue("__CreditCardOwner"));
        $("#CreditCardOwner").val(getCreditCardOwner(app_form.radioSelectedValue("__CreditCardOwner")));

        setSignKey();

        var validationResult = function (isValid) {
            if (isValid) {
                var postData = app.getFormInputs(["#form1", "#form2"]);

                $.ajax({
                    url: action,
                    type: 'POST',
                    dataType: 'json',
                    data: postData,
                    success: function (data) {
                        if(data.Status>0)
                        {
                            app.redirectTo("/Registry/Credit/Mifkad?m=" + data.Link);
                        }
                        else
                        {
                            app_dialog.alert(data.Message);
                        }
                    },
                    error: function (jqXHR, status, error) {
                        app_dialog.alert(error);
                    }
                });

                setSignKey();
            }
        }
        $('#form2').jqxValidator('validate', validationResult);
    });

};

registry_custom.prototype.loadControls = function () {

    var input_rules = [
              { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
                {
                    input: "#FirstName", message: 'שם פרטי אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                        var val = $("#FirstName").val();
                        if (val == null)
                            return false;
                        else if (val.length < 2)
                            return false;
                        else {
                            return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);
                            //var oke = /^[a-zA-Z]+$/.test(val);

                            //return okh || oke;
                        }
                    }
                },
              { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
                {
                    input: "#LastName", message: 'שם משפחה אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                        var val = $("#LastName").val();
                        if (val == null)
                            return false;
                        else if (val.length < 2)
                            return false;
                        else {
                            return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);
                        }
                    }
                },
              { input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
              { input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },
              { input: '#Email', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },
            //  {
            //    input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'valueChanged', rule: function (input, commit) {
            //        var date = $('#Birthday').jqxDateTimeInput('value');
            //        return !(date == null || date === undefined);
            //    }
            //   },
            //  {
            //    input: '#Birthday', message: 'תאריל לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
            //        var date = $('#Birthday').jqxDateTimeInput('value');
            //        if (date == null || date === undefined)
            //            return false;
            //        return (date.getFullYear() >= 1900 && date.getFullYear() <= 2000);
            //    }
            //},c
             { input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'valuechanged, blur', rule: 'required' },
            {
            input: '#Birthday', message: 'תאריך לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
                var date = $("#Birthday").datepicker("getDate");
                if (date == null || date === undefined)
                    return false;
                return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);
                }
            },
             {
                  input: "#City", message: 'חובה לציין עיר!', action: 'select', rule: function (input, commit) {
                       var index = $("#City").jqxComboBox('getSelectedIndex');
                       return index != -1;
                   }
              },
              {
                  input: "#Gender", message: 'חובה לציין מין!', action: 'select', rule: function (input, commit) {
                      var index = $("#Gender").jqxComboBox('getSelectedIndex');
                      return index != -1;
                  }
              },
              { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
              {
                  input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                            function (input, commit) {
                                var val = input.val();
                                var re = /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/
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
    input_rules.push({
        input: "#MemberId", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
            var value = $("#MemberId").val();
            return app_validation.memberId(value) == 1;//value.length > 4 && $.isNumeric(value);
        }
    });
    $('#form1').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

    var input_rules2 = [
        {
            input: '#ItemId1', message: 'חובה לציין דמי חבר', action: 'change', rule: function () {
                var checked = $("#ItemId1").prop("checked") || $("#ItemId2").prop("checked") || $("#ItemId3").prop("checked");
                return checked;
            }
        },
        {
            input: "#ConfirmAgreement", message: 'חובה לקרוא ולאשר הצהרת מתפקד!', action: 'change, blur', rule: function (input, commit) {
                return $("#ConfirmAgreement").prop("checked") == true;
            }
        }
    ];
    //input_rules2.push({ input: '#ConfirmAgreement', message: 'חובה לקרוא ולאשר הצהרת מתפקד!', action: 'checked', rule: 'required' });


    $('#form2').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules2
    });
};

function registry_custom_ex() {

    var ismember = app.requestQuery("m");

    this.AccountId = 2;
    //$("#AccountId").val(this.AccountId);

    if (ismember == "y") {
        $("#hIsmem").hide();
    }

    $("#stp-2").hide();
    $("#stp-3").hide();
    //$('.Birthday').jqxDateTimeInput({ width: '100%', rtl: true, formatString: "dd/MM/yyyy" });//showCalendarButton: false,
    //$('#Birthday1').val('');
    //$('#Birthday2').val('');
    $('#Birthday1').datepicker({
        regional: ["he"],
        isRTL: true,
        yearRange: "1925:1999",
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'dd/mm/yy',
        onSelect: function (dateText, inst) {
            $('#form1').jqxValidator('validateInput', '#Birthday1');
        }
    });
    //$("#Birthday1").focus(function () {
    //    //$(".ui-datepicker-calendar").hide();
    //    $(".ui-datepicker-close").click(function () {
    //        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
    //        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
    //        var day = $("#ui-datepicker-div .ui-datepicker-day :selected").val();
    //        $("#Birthday1").datepicker('setDate', new Date(year, month, day));
    //    });
    //});

    $('#Birthday2').datepicker({
        regional: ["he"],
        isRTL: true,
        //dayNames: ["ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת"],
        //monthNames: ["ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר", "אוקטובר", "נובמבר", "דצמבר"],
        //monthNamesShort: [ "ינו", "פבר", "מרץ", "אפר", "מאי", "יונ", "יול", "אוג", "ספט", "אוק", "נוב", "דצמ" ],
        yearRange: "1925:1999",
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'dd/mm/yy',
        onSelect: function (dateText, inst) {
            $('#form2').jqxValidator('validateInput', '#Birthday2');
        }
    });
    //$("#Birthday2").focus(function () {
    //    //$(".ui-datepicker-calendar").hide();
    //    $(".ui-datepicker-close").click(function () {
    //        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
    //        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
    //        var day = $("#ui-datepicker-div .ui-datepicker-day :selected").val();
    //        $("#Birthday2").datepicker('setDate', new Date(year, month, day));
    //    });
    //});

    // $('#SignKey').val(app.guid());

    //app_jqxcombos.createComboSelectorAdapter("PropId", "PropName", ".City", '/Registry/GetCityView?aid=' + AccountId, 0, 120, false);
    //app_jqxcombos.createComboSelectorAdapter("PropId", "PropName", ".Gender", '/Registry/GetGenderView?aid=' + AccountId, 0, 0, false);

    app_jqxcombos.createComboAdapter("PropId", "PropName", "City1", '/Registry/GetCityView', '100%', 120, false);
    app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender1", '/Registry/GetGenderView', '100%', 0, false);
    app_jqxcombos.createComboAdapter("PropId", "PropName", "City2", '/Registry/GetCityView', '100%', 120, false);
    app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender2", '/Registry/GetGenderView', '100%', 0, false);

    this.loadControls();

    $("#aStatement").click(function () {
        app_dialog.dialogIframe("/Registry/_Statement/Mifkad", '600px', "400px", "הצהרה", true);
    });

    function addAccountTag(form) {
        var accid = $("#AccountId").val();
        if ($(form).has("#AccountId").length == false) {
            $('<input>').attr({
                type: 'hidden',
                id: 'AccountId',
                name: 'AccountId',
                value: accid
            }).appendTo(form);
        }
    }

    function onNext1Validation1(data, form) {
        if (data.Status < 0) {

            app_dialog.alert(data.Message);

            //if (data.Status = -1) {
            //    //vex.alert(data.Message);
            //    vex.dialog.confirm({
            //        message: data.Message + '\n' + ' האם לעדכן את פרטיך האישיים?',
            //        callback: function (value) {
            //            if (value) {
            //                addAccountTag('#form1');
            //                app_jqxform.jqxForm_Redirect("#form1", "/Registry/Msg/Mifkad?m=MemberSaved", "/Registry/UpdateMember");
            //            }
            //        }
            //    });
            //}
            //else
            //vex.dialog.alert(data.Message);
        }
        else //if(status==0)
        {
            $("#stp-1").hide();
            $("#stp-2").show();

        }
    }

    function onNext1Validation2(data) {
        if (data.Status < 0) {
            app_dialog.alert(data.Message);
            //if (data.Status = -1) {
            //    //vex.alert(data.Message);
            //    vex.dialog.confirm({
            //        message: data.Message + '\n' + ' האם לעדכן את פרטיך האישיים?',
            //        callback: function (value) {
            //            if (value) {
            //                addAccountTag('#form2');
            //                app_jqxform.jqxForm_Redirect("#form2", "/Registry/Msg/Mifkad?m=MemberSaved", "/Registry/UpdateMember");
            //            }
            //        }
            //    });
            //}
            //else
            //vex.dialog.alert(data.Message);
        }
        else //if(status==0)
        {
            $("#stp-2").hide();
            $("#stp-3").show();
        }
    }

    $("#Next1").click(function () {

        //e.preventDefault();
        var validationResult = function (isValid) {
            if (isValid) {
                app_validation.signupValididty($("#AccountId").val(), $("#MemberId1").val(), $("#CellPhone1").val(), $("#Email1").val(), $("#ExId1").val(), onNext1Validation1);
            }
        }
        $('#form1').jqxValidator('validate', validationResult);
    });
    $("#Next2").click(function () {
        //e.preventDefault();

        if ($("#MemberId2").val() == $("#MemberId1").val()) {
            app_dialog.alert("כפילות בתעודות זהות!")
            return;
        }

        var validationResult = function (isValid) {
            if (isValid) {
                app_validation.signupValididty($("#AccountId").val(), $("#MemberId2").val(), $("#CellPhone2").val(), $("#Email2").val(), $("#ExId2").val(), onNext1Validation2);
            }
        }
        $('#form2').jqxValidator('validate', validationResult);
    });
    $("#Prev2").click(function () {
        $("#stp-1").show();
        $("#stp-2").hide();
    });
    $("#Prev3").click(function () {
        $("#stp-2").show();
        $("#stp-3").hide();
    });

    //$('#Submit').on('click', function (e) {
    //    e.preventDefault();
    //    var action = $("#form3").attr('action');
    //    $("#ItemId").val(app_form.radioSelectedValue("__ItemId"));
    //    $("#CreditCardOwner").val(app_form.radioSelectedValue("__CreditCardOwner"));

    //    var validationResult = function (isValid) {
    //        if (isValid) {
    //            var postData = app.getFormInputs(["#form1", "#form2", "form3"]);

    //            $.ajax({
    //                url: action,
    //                type: 'POST',
    //                dataType: 'json',
    //                data: postData,
    //                success: function (data) {
    //                    if (data.Status > 0) {
    //                        app.redirectTo("/Registry/Credit/Mifkad?m=" + data.Link);
    //                    }
    //                    else {
    //                       app_dialog.alert(data.Message);
    //                    }
    //                },
    //                error: function (jqXHR, status, error) {
    //                   app_dialog.alert(error);
    //                }
    //            });
    //        }
    //    }
    //    $('#form3').jqxValidator('validate', validationResult);
    //});


    function getCreditCardOwner(value) {
        switch (value) {

            case "1":
                return "המתפקד/ת";
            case "2":
                return "בן/בת זוגו של המתפקד/ת";
            case "3":
                return "הורה של המתפקד/ת (למתפקדים עד גיל 25 בלבד)";
            case "4":
                return "בנו/בתו של המתפקד/ת";
            default:
                return "המתפקד/ת";
        }
    }

    //function setSignKey() {
    //    var acc = $("#AccountId").val();
    //    var g = app.UUID('v4');
    //    var signKey = acc + "_" + g.toString().replace("-", "");
    //    $("#SignKey").val(signKey);
    //}

    function setSignKey() {
        var acc = $("#form1 [name=AccountId]").val();
        var g = app.UUID();
        var signKey = acc + "_" + g.toString().replace("-", "");
        $("#form1 [name=SignKey]").val(signKey);
        $("#form2 [name=SignKey]").val(signKey);
    }

    $('#Submit').on('click', function (e) {
        e.preventDefault();
        var action = $("#form3").attr('action');
        $("#ItemId").val(app_form.radioSelectedValue("__ItemId"));
        //$("#CreditCardOwner").val(app_form.radioSelectedValue("__CreditCardOwner"));
        var owner = app_form.radioSelectedValue("__CreditCardOwner");
        $("#CreditCardOwner").val(getCreditCardOwner(owner));

        $("#SignupOrder1").val(1);
        $("#SignupOrder2").val(2);

        setSignKey();

        //if (owner == "1") {
        //    $("#SignupOrder1").val(1);
        //    $("#SignupOrder2").val(2);
        //}
        //else {
        //    $("#SignupOrder1").val(2);
        //    $("#SignupOrder2").val(1);
        //}

        var validationResult = function (isValid) {
            if (isValid) {
                doSubmit(action, owner);
                setSignKey();
            }
        }
        $('#form3').jqxValidator('validate', validationResult);
    });


    var doSubmit = function (action, owner) {

        var postDataOwner = app.getFormInputs(["#form1", "#form3"]);
        var postDataSub= app.getFormInputs(["#form2", "#form3"]);

        var actionOwner = action;
        var actionSub = action + 'Ex';

        //if (owner == "1") {
        //    postDataOwner = app.getFormInputs(["#form1", "#form3"]);
        //    postDataSub = app.getFormInputs(["#form2", "#form3"]);
        //    actionOwner = action;
        //    actionSub = action + 'Ex';
        //}
        //else {
        //    postDataOwner = app.getFormInputs(["#form2", "#form3"]);
        //    postDataSub = app.getFormInputs(["#form1", "#form3"]);
        //    actionOwner = action + 'Ex';
        //    actionSub = action
        //}

        $.post(actionSub, postDataSub,
        function (data) {
            if (data.Status <= 0) {
                app_dialog.alert(data.Message);
            }
            else {
                $.post(actionOwner, postDataOwner,
                    function (data) {
                        if (data.Status > 0) {
                            app.redirectTo("/Registry/Credit/Mifkad?m=" + data.Link);
                        }
                        else {
                            app_dialog.alert(data.Message);
                        }
                    });
            }
        });
    };

};

registry_custom_ex.prototype.loadControls = function () {
    
    var input_rules1 = [
          { input: '#FirstName1', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
          { input: '#LastName1', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
         {
             input: "#FirstName1", message: 'שם פרטי אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                 var val = $("#FirstName1").val();
                if (val == null)
                    return false;
                else if (val.length < 2)
                    return false;
                else {
                    return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);
                }
            }
        },
        {
            input: "#LastName1", message: 'שם משפחה אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                var val = $("#LastName1").val();
                if (val == null)
                    return false;
                else if (val.length < 2)
                    return false;
                else {
                    return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);
                }
            }
        },
          { input: '#Address1', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
          { input: '#CellPhone1', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },
          { input: '#Email1', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },
          //{ input: '#Birthday1', message: 'חובה לציין תאריך לידה!', action: 'keyup, blur', rule: 'required' },
         // {
         //   input: '#Birthday1', message: 'חובה לציין תאריך לידה!', action: 'valueChanged', rule: function (input, commit) {
         //       var date = $('#Birthday1').jqxDateTimeInput('value');
         //       return !(date == null || date === undefined);
         //   }
         //},
         // {
         //     input: '#Birthday1', message: 'תאריל לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
         //         var date = $('#Birthday1').jqxDateTimeInput('value');
         //         if(date == null || date === undefined)
         //             return false;
         //         return (date.getFullYear() >= 1900 && date.getFullYear() <= 2000);
         //     }
         //  },
          { input: '#Birthday1', message: 'חובה לציין תאריך לידה!', action: 'valuechanged, blur', rule: 'required' },
          {
              input: '#Birthday1', message: 'תאריך לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
                  var date = $("#Birthday1").datepicker("getDate");
                  if (date == null || date === undefined)
                      return false;
                  return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);
              }
          },
          {
              input: "#City1", message: 'חובה לציין עיר!', action: 'select', rule: function (input, commit) {
                  var index = $("#City1").jqxComboBox('getSelectedIndex');
                  return index != -1;
              }
          },
          {
              input: "#Gender1", message: 'חובה לציין מין!', action: 'select', rule: function (input, commit) {
                  var index = $("#Gender1").jqxComboBox('getSelectedIndex');
                  return index != -1;
              }
          },
          { input: '#Email1', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
          {
              input: '#CellPhone1', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                        function (input, commit) {
                            var val = input.val();
                            var re = /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/
                            return val ? re.test(val) : true;
                        }
          },
          {
              input: '#Phone1', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
                        function (input, commit) {
                            var val = input.val();
                            var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                            return val ? re.test(val) : true;
                        }
          }
    ];

    input_rules1.push({ input: '#MemberId1', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
    input_rules1.push({
        input: "#MemberId1", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
            var value = $("#MemberId1").val();
            return app_validation.memberId(value) == 1;//value.length > 4 && $.isNumeric(value);
        }
    });
    $('#form1').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules1
    });

    var input_rules2 = [
      { input: '#FirstName2', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
      { input: '#LastName2', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
         {
             input: "#FirstName2", message: 'שם פרטי אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                 var val = $("#FirstName2").val();
                 if (val == null)
                     return false;
                 else if (val.length < 2)
                     return false;
                 else {
                     return /^[a-zA-Z\u00C0-\u1FFF\u2C00-\uD7FF ']+$/.test(val);
                 }
             }
         },
        {
            input: "#LastName2", message: 'שם משפחה אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                var val = $("#LastName2").val();
                if (val == null)
                    return false;
                else if (val.length < 2)
                    return false;
                else {
                    return /^[a-zA-Z\u00C0-\u1FFF\u2C00-\uD7FF ']+$/.test(val);
                }
            }
        },
      { input: '#Address2', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
      { input: '#CellPhone2', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },
      { input: '#Email2', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },
      //{ input: '#Birthday2', message: 'חובה לציין תאריך לידה!', action: 'keyup, blur', rule: 'required' },
        //{
        //    input: '#Birthday2', message: 'חובה לציין תאריך לידה!', action: 'valueChanged', rule: function (input, commit) {
        //        var date = $('#Birthday2').jqxDateTimeInput('value');
        //        return !(date == null || date === undefined);
        //    }
        //},
        //{
        //    input: '#Birthday2', message: 'תאריל לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
        //        var date = $('#Birthday2').jqxDateTimeInput('value');
        //        if (date == null || date === undefined)
        //            return false;
        //        return (date.getFullYear() >= 1900 && date.getFullYear() <= 2000);
        //    }
        //},
         { input: '#Birthday2', message: 'חובה לציין תאריך לידה!', action: 'valuechanged, blur', rule: 'required' },
        {
            input: '#Birthday2', message: 'תאריך לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
                var date = $("#Birthday2").datepicker("getDate");
                if (date == null || date === undefined)
                    return false;
                return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);
            }
        },
         {
          input: "#City2", message: 'חובה לציין עיר!', action: 'select', rule: function (input, commit) {
              var index = $("#City2").jqxComboBox('getSelectedIndex');
              return index != -1;
          }
      },
      {
          input: "#Gender2", message: 'חובה לציין מין!', action: 'select', rule: function (input, commit) {
              var index = $("#Gender2").jqxComboBox('getSelectedIndex');
              return index != -1;
          }
      },
      { input: '#Email2', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
      {
          input: '#CellPhone2', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                    function (input, commit) {
                        var val = input.val();
                        var re = /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/
                        return val ? re.test(val) : true;
                    }
      },
      {
          input: '#Phone2', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
                    function (input, commit) {
                        var val = input.val();
                        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                        return val ? re.test(val) : true;
                    }
      }
    ];

    input_rules2.push({ input: '#MemberId2', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
    input_rules2.push({
        input: "#MemberId2", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
            var value = $("#MemberId2").val();
            return value.length > 4 && $.isNumeric(value);
        }
    });

    $('#form2').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules2
    });

    var input_rules3 = [
       {
           input: '#ItemId4', message: 'חובה לציין דמי חבר', action: 'change', rule: function () {
               var checked = $("#ItemId4").prop("checked");
               return checked;
           }
       },
       {
           input: "#ConfirmAgreement", message: 'חובה לקרוא ולאשר הצהרת מתפקד!', action: 'change, blur', rule: function (input, commit) {
               return $("#ConfirmAgreement").prop("checked") == true;
           }
       }
    ];

    $('#form3').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules3
    });
};
