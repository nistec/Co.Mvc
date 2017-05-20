
//============================================================================================ app_cms_content_def

function app_cms_wizard_builder() {

    var form;
    var me = this;
    var wizform = window.parent.getFormWizard();
    this.input_rules = [];
    this.editorScript = [];
    

    //$("#validatorScript").append('$("#form1").jqxValidator({ rtl: true, hintType: "label", animationDuration: 0, rules: [');

    var tbl = $(wizform).find('#tbl-fields > tbody  > tr');
    for (var i = 1; i < tbl.length; i++) {
        var tr = tbl[i];
        var field = $(tr).attr('id');
        var type = $(tr).attr('data-type');

        var lbl = $(tr).find('td > span').text();
        var enabled = $(tr).find('td > input.enabled')[0].checked;
        var required = $(tr).find('td > input.required')[0].checked;

        //var enabled = $(tr).find('td > input#' + field + '-Enabled').val() == 'on';
        //var required = $(tr).find('td > input#' + field + '-Required').val() == 'on';
        if (enabled) {
            var elm = me.getElement(field, lbl);
            //me.form += elm;

            $("#editorHtml").append(elm);

            if (field == 'Birthday') {
                app_control.datepicker("#Birthday", "1925:1999");
                this.editorScript.push('app_control.datepickerBirthday("#Birthday", "1925:1999","#form1");');
            }
            else if (field == 'City') {
                app_jqxcombos.createComboAdapter("PropId", "PropName", "City", '/Registry/GetCityView', '100%', 120, false);
                this.editorScript.push('app_jqxcombos.createComboAdapter("PropId", "PropName", "City", "/Registry/GetCityView", "100%", 120, false);');
            }
            else if (field == 'Gender') {
                app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender", '/Registry/GetGenderView', '100%', 0, false);
                this.editorScript.push('app_jqxcombos.createComboAdapter("PropId", "PropName", "Gender", "/Registry/GetGenderView", "100%", 0, false);');
            }
        }  
      
        me.appendValidator(field, lbl, required);
    }
    
    var vldScript = this.input_rules.join();
    var jxscript = '$("#form1").jqxValidator({rtl: true, hintType: "label", animationDuration: 0, rules: [' + vldScript + '\n]});';


    var script = document.createElement("script");
    script.type = "text/javascript";
    //script.src = "path/to/your/javascript.js";  
    script.text = this.editorScript.join('');
    document.body.appendChild(script);
    
    var script = document.createElement("script");
    script.type = "text/javascript";
    //script.src = "path/to/your/javascript.js";  
    script.text = jxscript;
    document.body.appendChild(script);
    
};

app_cms_wizard_builder.prototype.getElement = function (field,name) {


    if (field == 'LastName') {
        return  '<div class="form-group">' +
         '<div class="lable">*שם משפחה</div>' +
         '<input type="text" class="txt" id="LastName" name="LastName" />' +
     '</div>';
    }
    if (field == 'FirstName') {
        return   '<div class="form-group">' +
            '<div class="lable">*שם פרטי</div>' +
            '<input type="text" class="txt" id="FirstName" name="FirstName" />' +
       '</div>';
    }
    if (field == 'MemberId') {
        return '<div class="form-group">' +
            '<div class="lable">*ת.ז.</div>' +
            '<input type="text" class="txt" id="MemberId" name="MemberId" />' +
       '</div>';
    }
    if (field == 'CellPhone') {
        return '<div class="form-group">' +
            '<div class="lable">*טלפון נייד</div>' +
            '<input type="text" class="txt" id="CellPhone" name="CellPhone" />' +
       '</div>';
    }
    if (field == 'Phone') {
        return '<div class="form-group">' +
            '<div class="lable">טלפון בבית</div>' +
            '<input type="number" class="txt" id="Phone" name="Phone" />' +
       '</div>';
    }
    if (field == 'Email') {
        return  '<div class="form-group">' +
            '<div class="lable">*אימייל</div>' +
            '<input type="text" class="txt" id="Email" name="Email" />' +
       '</div>';
    }
    if (field == 'Address') {
        return '<div class="form-group">' +
            '<div class="lable">*כתובת (רחוב ומספר) </div>' +
            '<input type="text" class="txt" id="Address" name="Address" />' +
       '</div>';
    }
    if (field == 'City') {
        return '<div class="form-group">' +
            '<div class="lable">*עיר</div>' +
            '<div id="City" name="City"></div>' +
       '</div>';
    }
    if (field == 'Gender') {
        return '<div class="form-group">' +
            '<div class="lable">*מין</div>' +
            '<div id="Gender" name="Gender"></div>' +
       '</div>';
    }
    if (field == 'Birthday') {
        return '<div class="form-group">' +
            '<div class="lable">*תאריך לידה</div>' +
            '<input type="text" id="Birthday" name="Birthday" readonly />' +
       '</div>';
    } 
    //========  ex ===============================
    
    if (field == 'ExId') {
        return '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<input type="text" class="txt" id="ExId" name="ExId" />' +
       '</div>';
    }
    if (field == 'ExField1') {
        return '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<input type="text" class="txt" id="ExField1" name="ExField1" />' +
       '</div>';
    }
    if (field == 'ExField2') {
        return '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<input type="text" class="txt" id="ExField2" name="ExField2" />' +
       '</div>';
    }
    if (field == 'ExField3') {
        return  '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<input type="text" class="txt" id="ExField3" name="ExField3" />' +
       '</div>';
    }
    if (field == 'ExEnum1') {
        return '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<div id="ExEnum1" name="ExEnum1"></div>' +
       '</div>';
    }
    if (field == 'ExEnum2') {
        return '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<div id="ExEnum2" name="ExEnum2"></div>' +
       '</div>';
    }
    if (field == 'ExEnum3') {
        return '<div class="form-group">' +
            '<div class="lable">' + name + '</div>' +
            '<div id="ExEnum3" name="ExEnum3"></div>' +
       '</div>';
    }

};

app_cms_wizard_builder.prototype.appendValidator = function (field, name, required) {

    if (field == 'LastName') {
        if (required) {
            this.input_rules.push("\n{ input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' }");
            this.input_rules.push("\n{input: '#LastName', message: 'שם משפחה אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {" +
                    "var val = $('#LastName').val();" +
                    "if (val == null)" +
                        "return false;" +
                    "else if (val.length < 2)" +
                        "return false;" +
                    "else {return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);"+
                    "}}}");
        }
    }
    if (field == 'FirstName') {
        if (required) {
            this.input_rules.push("\n{ input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' }");
            this.input_rules.push("\n{input: '#FirstName', message: 'שם פרטי אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {" +
                    "var val = $('#FirstName').val();" +
                    "if (val == null)" +
                        "return false;" +
                    "else if (val.length < 2)" +
                        "return false;" +
                    "else {return "+ /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/+ ".test(val);"+
                "}}}");
        }
    }
    if (field == 'MemberId') {
        if (required)
            this.input_rules.push("\n{ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' }");
        this.input_rules.push("\n{input: '#MemberId', message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {" +
                "var value = $('#MemberId').val();" +
                "return app_validation.memberId(value) == 1;}}");
    }
    if (field == 'CellPhone') {
        if (required)
            this.input_rules.push("\n{ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' }");
        this.input_rules.push("\n{input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:" +
            "function (input, commit) {" +
                "var val = input.val();" +
                "var re = "+ /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/ +
                "\n return val ? re.test(val) : true;}}");
    }
    if (field == 'Phone') {
        if (required)
            this.input_rules.push("\n{ input: '#Phone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' }");
        this.input_rules.push("\n{input: '#Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:" +
            "function (input, commit) {" +
                "var val = input.val();" +
                "var re = "+ /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/ +
                "\n return val ? re.test(val) : true;}}");
    }
    if (field == 'Email') {
        if (required)
            this.input_rules.push("\n{ input: '#Email', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' }");
        this.input_rules.push("\n{ input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' }");
    }
    if (field == 'Address') {
        if (required)
            this.input_rules.push("\n{ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' }");
    }
    if (field == 'City') {
        if (required)
            this.input_rules.push("\n{input: '#City', message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#City').jqxComboBox('getSelectedIndex');" +
                    "return index != -1;}}");
    }
    if (field == 'Gender') {
        if (required)
            this.input_rules.push("\n{input: '#Gender', message: 'חובה לציין מין!', action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('Gender').jqxComboBox('getSelectedIndex');" +
                    "return index != -1;}}");
    }
    if (field == 'Birthday') {
        if (required)
            this.input_rules.push("\n{input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'valuechanged, blur', rule: 'required' }");
        this.input_rules.push("{input: '#Birthday', message: 'תאריך לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {" +
                "var date = $('#Birthday').datepicker('getDate');" +
                "if (date == null || date === undefined)" +
                    "return false;" +
                "return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);}}");
    }

    //========  ex ===============================

    if (field == 'ExId') {
        if (required)
            this.input_rules.push("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required'}");
    }
    if (field == 'ExField1') {
        if (required)
            this.input_rules.push("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required'}");
    }
    if (field == 'ExField2') {
        if (required)
            this.input_rules.push("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required'}");
    }
    if (field == 'ExField3') {
        if (required)
            this.input_rules.push("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required'}");
    }
    if (field == 'ExEnum1') {
        if (required)
            this.input_rules.push("\n{input: '#ExEnum1', message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#ExEnum1').jqxComboBox('getSelectedIndex');" +
                    "return index != -1;}}");
    }
    if (field == 'ExEnum2') {
        if (required)
            this.input_rules.push("\n{input: '#ExEnum2', message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#ExEnum2').jqxComboBox('getSelectedIndex');" +
                    "return index != -1;}}");
    }
    if (field == 'ExEnum3') {
        if (required)
            this.input_rules.push("\n{input: '#ExEnum3', message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#ExEnum3').jqxComboBox('getSelectedIndex');" +
                    "return index != -1;}}");
    }
    if (field == 'ExDate1') {
        if (required)
            this.input_rules.push("\n{ input: '#ExDate1', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required'}");
    }
    if (field == 'ExDate2') {
        if (required)
            this.input_rules.push("\n{ input: '#ExDate2', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required'}");
    }
    if (field == 'ExDate3') {
        if (required)
            this.input_rules.push("\n{ input: '#ExDate3', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required'}");
    }

    //$('#form1').jqxValidator({
    //    rtl: true,
    //    hintType: 'label',
    //    animationDuration: 0,
    //    rules: this.input_rules
    //});

};






app_cms_wizard_builder.prototype.setValidator = function (field, name, required) {

    if (field == 'LastName') {
        if (required) {
            $("#validatorScript").append("\n{ input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },");
            $("#validatorScript").append("\n{input: '#LastName', message: 'שם משפחה אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {" +
                    "var val = $('#LastName').val();"+
                    "if (val == null)"+
                        "return false;"+
                    "else if (val.length < 2)"+
                        "return false;"+
                    "else {return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);}}},");
        }
    }
    if (field == 'FirstName') {
        if (required) {
            $("#validatorScript").append("\n{ input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },");
            $("#validatorScript").append("\n{input: '#FirstName', message: 'שם פרטי אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {" +
                    "var val = $('#FirstName').val();"+
                    "if (val == null)"+
                        "return false;"+
                    "else if (val.length < 2)"+
                        "return false;"+
                    "else {return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);}}},");
        }
    }
    if (field == 'MemberId') {
        if (required)
            $("#validatorScript").append("\n{ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' },");
        $("#validatorScript").append("\n{input: '#MemberId', message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {" +
                "var value = $('#MemberId').val();"+
                "return app_validation.memberId(value) == 1;}},");
    }
    if (field == 'CellPhone') {
        if (required)
            $("#validatorScript").append("\n{ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },");
        $("#validatorScript").append("\n{input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:" +
            "function (input, commit) {"+
                "var val = input.val();"+
                "var re = /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/"+
                "return val ? re.test(val) : true;}},");
    }
    if (field == 'Phone') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Phone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' },");
        $("#validatorScript").append("\n{input: '#Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:" +
            "function (input, commit) {"+
                "var val = input.val();"+
                "var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/"+
                "return val ? re.test(val) : true;}},");
    }
    if (field == 'Email') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Email', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' },");
        $("#validatorScript").append("\n{ input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },");
    }
    if (field == 'Address') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },");
    }
    if (field == 'City') {
        if (required)
            $("#validatorScript").append("\n{input: '#City', message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#City').jqxComboBox('getSelectedIndex');"+
                    "return index != -1;}},");
    }
    if (field == 'Gender') {
        if (required)
            $("#validatorScript").append("\n{input: '#Gender', message: 'חובה לציין מין!', action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('Gender').jqxComboBox('getSelectedIndex');"+
                    "return index != -1;}},");
    }
    if (field == 'Birthday') {
        if (required)
            $("#validatorScript").append("\n{input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'valuechanged, blur', rule: 'required' },");
        $("#validatorScript").append("{input: '#Birthday', message: 'תאריך לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {" +
                "var date = $('#Birthday').datepicker('getDate');"+
                "if (date == null || date === undefined)"+
                    "return false;"+
                "return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);}},");
    }

    //========  ex ===============================

    if (field == 'ExId') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' },");
    }
    if (field == 'ExField1') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' },");
    }
    if (field == 'ExField2') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' },");
    }
    if (field == 'ExField3') {
        if (required)
            $("#validatorScript").append("\n{ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' },");
    }
    if (field == 'ExEnum1') {
        if (required)
            $("#validatorScript").append("\n{input: '#ExEnum1', message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#ExEnum1').jqxComboBox('getSelectedIndex');"+
                    "return index != -1;}},");
    }
    if (field == 'ExEnum2') {
        if (required)
            $("#validatorScript").append("\n{input: '#ExEnum2', message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#ExEnum2').jqxComboBox('getSelectedIndex');"+
                    "return index != -1;}},");
    }
    if (field == 'ExEnum3') {
        if (required)
            $("#validatorScript").append("\n{input: '#ExEnum3', message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {" +
                    "var index = $('#ExEnum3').jqxComboBox('getSelectedIndex');"+
                    "return index != -1;}},");
    }
    if (field == 'ExDate1') {
        if (required)
            $("#validatorScript").append("\n{ input: '#ExDate1', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required' },");
    }
    if (field == 'ExDate2') {
        if (required)
            $("#validatorScript").append("\n{ input: '#ExDate2', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required' },");
    }
    if (field == 'ExDate3') {
        if (required)
            $("#validatorScript").append("\n{ input: '#ExDate3', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required' },");
    }

    //$('#form1').jqxValidator({
    //    rtl: true,
    //    hintType: 'label',
    //    animationDuration: 0,
    //    rules: this.input_rules
    //});

};

app_cms_wizard_builder.prototype.buildValidator = function (field, name, required) {

    if (field == 'LastName') {
        if (required) {
            this.input_rules.push({ input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' });
            this.input_rules.push({
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
            });
        }
    }
    if (field == 'FirstName') {
        if (required) {
            this.input_rules.push({ input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' });
            this.input_rules.push({
                input: "#FirstName", message: 'שם פרטי אינו תקין נדרש 2 אותיות ומעלה!', action: 'keyup, blur', rule: function (input, commit) {
                    var val = $("#FirstName").val();
                    if (val == null)
                        return false;
                    else if (val.length < 2)
                        return false;
                    else {
                        return /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/.test(val);
                    }
                }
            });
        }
    }
    if (field == 'MemberId') {
        if (required)
            this.input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
        this.input_rules.push({
            input: "#MemberId", message: 'ת.ז אינה תקינה!', action: 'blur', rule: function (input, commit) {
                var value = $("#MemberId").val();
                return app_validation.memberId(value) == 1;//value.length > 4 && $.isNumeric(value);
            }
        });
    }
    if (field == 'CellPhone') {
        if (required)
            this.input_rules.push({ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        this.input_rules.push({
            input: '#CellPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
            function (input, commit) {
                var val = input.val();
                var re = /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/
                return val ? re.test(val) : true;
            }
        });
    }
    if (field == 'Phone') {
        if (required)
            this.input_rules.push({ input: '#Phone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        this.input_rules.push({
            input: '#Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
            function (input, commit) {
                var val = input.val();
                var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                return val ? re.test(val) : true;
            }
        });
    }
    if (field == 'Email') {
        if (required)
            this.input_rules.push({ input: '#Email', message: 'חובה לציין אימייל!', action: 'keyup, blur', rule: 'required' });
        this.input_rules.push({ input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' });
    }
    if (field == 'Address') {
        if (required)
            this.input_rules.push({ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' });
    }
    if (field == 'City') {
        if (required)
            this.input_rules.push({
                input: "#City", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
                    var index = $("#City").jqxComboBox('getSelectedIndex');
                    return index != -1;
                }
            });
    }
    if (field == 'Gender') {
        if (required)
            this.input_rules.push({
                input: "#Gender", message: 'חובה לציין מין!', action: 'keyup, select', rule: function (input, commit) {
                    var index = $("#Gender").jqxComboBox('getSelectedIndex');
                    return index != -1;
                }
            });
    }
    if (field == 'Birthday') {
        if (required)
            this.input_rules.push({ input: '#Birthday', message: 'חובה לציין תאריך לידה!', action: 'valuechanged, blur', rule: 'required' });
        this.input_rules.push({
            input: '#Birthday', message: 'תאריך לידה אינו בטווח!', action: 'valueChanged', rule: function (input, commit) {
                var date = $("#Birthday").datepicker("getDate");
                if (date == null || date === undefined)
                    return false;
                return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);
            }
        });
    }

    //========  ex ===============================

    if (field == 'ExId') {
        if (required)
            this.input_rules.push({ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' });
    }
    if (field == 'ExField1') {
        if (required)
            this.input_rules.push({ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' });
    }
    if (field == 'ExField2') {
        if (required)
            this.input_rules.push({ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' });
    }
    if (field == 'ExField3') {
        if (required)
            this.input_rules.push({ input: '#Address', message: 'חובה לציין ' + name, action: 'keyup, blur', rule: 'required' });
    }
    if (field == 'ExEnum1') {
        if (required)
            this.input_rules.push({
                input: "#ExEnum1", message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {
                    var index = $("#ExEnum1").jqxComboBox('getSelectedIndex');
                    return index != -1;
                }
            });
    }
    if (field == 'ExEnum2') {
        if (required)
            this.input_rules.push({
                input: "#ExEnum2", message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {
                    var index = $("#ExEnum2").jqxComboBox('getSelectedIndex');
                    return index != -1;
                }
            });
    }
    if (field == 'ExEnum3') {
        if (required)
            this.input_rules.push({
                input: "#ExEnum3", message: 'חובה לציין ' + name, action: 'keyup, select', rule: function (input, commit) {
                    var index = $("#ExEnum3").jqxComboBox('getSelectedIndex');
                    return index != -1;
                }
            });
    }
    if (field == 'ExDate1') {
        if (required)
            this.input_rules.push({ input: '#ExDate1', message: 'חובה לציין תאריך ' + name, action: 'valuechanged, blur', rule: 'required' });
    }
    if (field == 'ExDate2') {
        if (required)
            this.input_rules.push({ input: '#ExDate2', message: 'חובה לציין תאריך '+ name, action: 'valuechanged, blur', rule: 'required' });
    }
    if (field == 'ExDate3') {
        if (required)
            this.input_rules.push({ input: '#ExDate3', message: 'חובה לציין תאריך '+ name, action: 'valuechanged, blur', rule: 'required' });
    }

    //$('#form1').jqxValidator({
    //    rtl: true,
    //    hintType: 'label',
    //    animationDuration: 0,
    //    rules: this.input_rules
    //});

};

