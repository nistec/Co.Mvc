
//============================================================================================ app_signup_def

function mifkad_util(accid, ismember) {

    this.AccountId = 2;
    $("#AccountId").val(this.AccountId);


    $("#aDouble").attr("href", "signup2?m=n")
    $("#aSingle").attr("href", "signup1?m=n")


    $('input[type=radio][name=memq]').change(function () {
        if (this.value == 'yes') {
            $("#aDouble").attr("href", "signup2?m=y")
            $("#aSingle").attr("href", "signup1?m=y")
        }
        else if (this.value == 'no') {
            $("#aDouble").attr("href", "signup2?m=n")
            $("#aSingle").attr("href", "signup1?m=n")
        }
    });

}

function mifkad_single(accid,ismember) {

    this.AccountId = 2;
    $("#AccountId").val(this.AccountId);
    
    if (ismember == "n") {
        $("#hIsmem").hide();
    }

    $("#stp-2").hide();
    $('#Birthday').jqxDateTimeInput({ showCalendarButton: false, width: '120px' });

    //app_jqx_list.cityComboAdapter();

    app_jqxcombos.createComboAdapter("PropId", "PropName", "City", '/Mifkad/GetCityView', '100%', 120, false);
    app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender", '/Mifkad/GetGenderView', '100%', 0, false);

    this.loadControls();
    
    $("#aStatement").click(function () {
        app_dialog.dialogIframe("_Statement", '600px', "400px", "הצהרה", true);
    });
   
    $("#Next1").click(function (e) {
     
        e.preventDefault();
        var validationResult = function (isValid) {
            if (isValid) {
                $("#stp-1").hide();
                $("#stp-2").show();
            }
        }
        $('#form1').jqxValidator('validate', validationResult);
    });
 
    $("#Prev2").click(function () {
        $("#stp-1").show();
        $("#stp-2").hide();
    });

    $('#Submit').on('click', function (e) {
        e.preventDefault();
        var action = $("#form2").attr('action');

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
                            app.redirectTo("/Mifkad/Credit?m="+ data.Link);
                        }
                        else
                        {
                            alert(data.Message);
                        }
                    }
                });
            }
        }
        $('#form2').jqxValidator('validate', validationResult);
    });

    //this.doSubmit = function () {
    //    //e.preventDefault();
    //    var actionurl = $('#accForm').attr('action');
    //    //app_jqxcombos.renderCheckList("listCategory", "Categories");
    //    var validationResult = function (isValid) {
    //        if (isValid) {
    //            $.ajax({
    //                url: actionurl,
    //                type: 'post',
    //                dataType: 'json',
    //                data: $('#accForm').serialize(),
    //                success: function (data) {
    //                    alert(data.Message);
    //                    if (data.Status >= 0) {
    //                        app.refresh();
    //                    }
    //                },
    //                error: function (jqXHR, status, error) {
    //                    alert(error);
    //                }
    //            });
    //        }
    //    }
    //    $('#accForm').jqxValidator('validate', validationResult);
    //};


    
};

mifkad_single.prototype.loadControls = function () {

    var input_rules = [
              { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
              { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
              { input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
              //{ input: '#City', message: 'חובה לציין עיר!', action: 'keyup, blur', rule: 'required' },
              { input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },
              { input: '#Email', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },
              //{ input: '#Gender', message: 'חובה לציין מין!', action: 'keyup, blur', rule: 'required' },
              { input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'keyup, blur', rule: 'required' },
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
    input_rules.push({
        input: "#MemberId", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
            var value = $("#MemberId").val();
            return value.length > 4 && $.isNumeric(value);
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


function mifkad_double(ismember) {

    this.AccountId = 2;

    if (ismember == "n") {
        $("#hIsmem").hide();
    }

    $("#stp-2").hide();
    $("#stp-3").hide();
    $('.Birthday').jqxDateTimeInput({ showCalendarButton: false, width: '120px' });

    app_jqxcombos.createComboSelectorAdapter("PropId", "PropName", ".City", '/Registry/GetCityView?aid=' + AccountId, 0, 120, false);
    app_jqxcombos.createComboSelectorAdapter("PropId", "PropName", ".Gender", '/Registry/GetGenderView?aid=' + AccountId, 0, 0, false);

    this.loadControls();

    $("#aStatement").click(function () {
        app_dialog.dialogIframe("_Statement", '60%', "300px", "הצהרה", true);
    });

    $("#Next1").click(function () {
        e.preventDefault();
        var validationResult = function (isValid) {
            if (isValid) {
                $("#stp-1").hide();
                $("#stp-2").show();
            }
        }
        $('#form1').jqxValidator('validate', validationResult);
    });
    $("#Next2").click(function () {
        e.preventDefault();
        var validationResult = function (isValid) {
            if (isValid) {
                $("#stp-2").hide();
                $("#stp-3").show();
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

    $('#Submit').on('click', function (e) {
        e.preventDefault();
        var action = $("#form3").attr('action');

        var validationResult = function (isValid) {
            if (isValid) {
                var postData = app.getFormInputs(["#form1", "#form2", "form3"]);

                $.ajax({
                    url: action,
                    type: 'POST',
                    dataType: 'json',
                    data: postData,
                    success: function (data) {
                        if (data.Status > 0) {
                            app.redirectTo("/Mifkad/Credit?m=" + data.Link);
                        }
                        else {
                            alert(data.Message);
                        }
                    }
                });
            }
        }
        $('#form3').jqxValidator('validate', validationResult);
    });

    //$('#form3').submit(function () {
    //    var action = $(this).attr('action');
    //    //if (!EntryCheck()) return false;
    //    var validationResult = function (isValid) {
    //        if (isValid) {

    //            $.ajax({
    //                url: action,
    //                type: 'POST',
    //                dataType: 'json',
    //                data: $('#form1, #form2, #form3').serialize(),
    //                success: function () {
    //                    window.location.replace(action);
    //                }
    //            });
    //            //return false;

    //        }
    //    }
    //    $('#form3').jqxValidator('validate', validationResult);
    //});



    //$('#form1').submit(function () {
    //    var action = $(this).attr('action');
    //    if (!EntryCheck()) return false;
    //    $.ajax({
    //        url: action,
    //        type: 'POST',
    //        data: $('#form1, #form2').serialize(),
    //        success: function () {
    //            window.location.replace(action);
    //        }
    //    });
    //    return false;
    //});



    //this.doSubmit = function () {
    //    //e.preventDefault();
    //    var actionurl = $('#accForm').attr('action');
    //    //app_jqxcombos.renderCheckList("listCategory", "Categories");
    //    var validationResult = function (isValid) {
    //        if (isValid) {
    //            $.ajax({
    //                url: actionurl,
    //                type: 'post',
    //                dataType: 'json',
    //                data: $('#accForm').serialize(),
    //                success: function (data) {
    //                    alert(data.Message);
    //                    if (data.Status >= 0) {
    //                        app.refresh();
    //                    }
    //                },
    //                error: function (jqXHR, status, error) {
    //                    alert(error);
    //                }
    //            });
    //        }
    //    }
    //    $('#accForm').jqxValidator('validate', validationResult);
    //};

};

mifkad_double.prototype.loadControls = function () {

    var input_rules1 = [
          { input: '#FirstName1', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
          { input: '#LastName1', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
          { input: '#Address1', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
          { input: '#CellPhone1', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },
          { input: '#Email1', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },
          { input: '#Birthday1', message: 'חובה לציין תאריך לידה!', action: 'keyup, blur', rule: 'required' },
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
                            var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
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
            return value.length > 4 && $.isNumeric(value);
        }
    });


    var input_rules2 = [
      { input: '#FirstName2', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
      { input: '#LastName2', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
      { input: '#Address2', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
      { input: '#CellPhone2', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },
      { input: '#Email2', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },
      { input: '#Birthday2', message: 'חובה לציין תאריך לידה!', action: 'keyup, blur', rule: 'required' },
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
                        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
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

    input_rules1.push({ input: '#MemberId2', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
    input_rules1.push({
        input: "#MemberId2", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
            var value = $("#MemberId2").val();
            return value.length > 4 && $.isNumeric(value);
        }
    });


    $('#form1').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules1
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
