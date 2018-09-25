
app_field_T = {
    type_text: "text",
    cls_textmid: "text-mid",
    cls_button7:'btn-default btn7',
    formBegin: function (id, action) {
        return '<form class="fcForm" id="' + id + '" method="post" action="' + action + '">';
    },
    formEnd: function () {
        return '</form>';
    },
    hidden: function (id, name, value) {
        return '<input type="hidden" id="' + id + '" name="' + name + '" value="' + value + '" />';
    },
    itemInput: function (label, id, type, cls) {
        if (type === undefined) type = 'text';
        if (cls === undefined) cls = '';
        return '<div class="form-group">' +
        '<div class="field">' + label + ':</div>' +
        '<input id="' + id + '" name="' + id + '" type="' + type + '" class="' + cls + '" />' +
        '</div>'
    },
    itemTextArea: function (label, id, cls) {
        if (cls === undefined) cls = '';
        return '<div class="form-group">' +
        '<div class="field">' + label + ':</div>' +
        '<textarea id="' + id + '" name="' + id + '" class="' + cls + '"></textarea>' +
        '</div>'
    },
    itemGeneric: function (label, id) {
        return '<div class="form-group">' +
        '<div class="field">' + label + ':</div>' +
        '<div id="' + id + '" name="' + id + '"></div>' +
        '</div>'
    },
    itemCheckList: function (label, id) {
        return '<div class="form-group">' +
        '<div class="field">' + label + ':</div>' +
        '<div id="' + id + '" name="' + id + '" style="padding: 10px" data-type="checklist"></div>' +
        '</div>'
    },
    divBegin: function (id, cls) {
        if (cls === undefined) cls = '';
        return '<div id="' + id + '" class="' + cls + '">';
    },
    sectionBegin: function (id, htext) {
        return '<div id="' + id + '" class="tab-group">' +
             '<h3>' + htext + '</h3>';
    },
    sectionEnd: function () {
        return '</div>';
    },
    button: function (id, value, cls) {
        if (cls === undefined) cls = 'btn-default btn7';
        return '<input id="' + id + '" class="' + cls + '" type="button" value="' + value + '" />';
    }
};

app_field_K = {
    firstName: function () {
        return app_field_T.itemInput("שם פרטי", "#FirstName", "text");
    },
    lastName: function () {
        return app_field_T.itemInput("שם משפחה", "#LastName", "text");
    },
    address: function (item) {
        return app_field_T.itemInput("כתובת", "#Address", "text");
    }
};

app_Rule_T = {
    action_none: 'none',
    action_default: 'keyup, blur',
    action_valuechanged: 'valuechanged, blur',
    action_select: 'keyup, select',
    input: function (tag, message, rule, action) {
        if (rule === undefined) rule = 'required';
        if (action === undefined) action = 'none';
        return "\n{ input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule: '" + rule + "' }";
    },
    email: function (tag, message, action) {
        if (action === undefined) action = 'none';
        return "\n{ input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule: 'email' }";
    },
    phone: function (tag, message, action) {
        if (action === undefined) action = 'none';
        return "\n{input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule:" +
            "function (input, commit) {" +
                "var val = input.val();" +
                "var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/" +
                "return val ? re.test(val) : true;" +
            "}" +
        "}";
    },
    cell: function (tag, message, action) {
        if (action === undefined) action = 'none';
        return "\n{input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule:" +
        "function (input, commit) {" +
            "var val = input.val();" +
            "var re = " + /^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/ +
            "\n return val ? re.test(val) : true;}}";
    },
    combo: function (tag, message, action) {
        if (action === undefined) action = 'none';
        return "\n{input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule: function (input, commit) {" +
                    "var index = $('" + tag + "').jqxComboBox('getSelectedIndex');" +
                    "return index != -1;}}";
    },
    datepicker: function (tag, message, action) {
        return "\n{input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule: function (input, commit) {" +
                    "var date = $('" + tag + "').datepicker('getDate');" +
                    "if (date == null || date === undefined)" +
                        "return false;" +
                    "return (date.getFullYear() >= 1900 && date.getFullYear() <= 1999);}}";
    },
    inputLetterOnly: function (tag, message, minLetters, action) {
        if (action === undefined) action = 'none';
        if (minLetters === undefined) minLetters = 2;
        return "\n{input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule: function (input, commit) {" +
                            "var val = $('" + tag + "').val();" +
                            "if (val == null)" +
                                "return false;" +
                            "else if (val.length < " + minLetters + ")" +
                                "return false;" +
                            "else {return " + /^[a-zA-Z\u0590-\u05FF\uFB1D-\uFB40 ']+$/ + ".test(val);" +
                        "}}}";
    },
    memberId: function (tag, message, action) {
        if (action === undefined) action = 'none';
        return "\n{input: '" + tag + "', message: '" + message + "', action: '" + action + "', rule: function (input, commit) {" +
            "var value = $('" + tag + "').val();" +
            "return app_validation.memberId(value) == 1;}}";
    }
};

app_Rule_K = {
    firstName: function () {
        return app_Rule_T.input('חובה לציין שם פרטי!');
    },
    lastName: function () {
        return app_Rule_T.input('חובה לציין שם משפחה!');
    },
    address: function (item) {
        return app_Rule_T.input('חובה לציין כתובת!');
    }
};

app_Control_T = {

    datepicker: function (tag, form, yearRange) {//"1925:1999"
        return 'app_control.datepickerBirthday("' + tag + '", "' + yearRange + '","' + form + '");';
    },
    combo: function (valueMember, displayMember, tag, action) {
        return 'app_jqxcombos.createComboAdapter("' + valueMember + '", "' + displayMember + '", "' + tag + '", "' + action + '", "100%", 120, false);';
    }
};

(function ($) {

app_form_Builder = {
    tagRoot: '',
    tagForm: '',
    body: [],
    control: [],
    rules: [],
    init: function (root,form) {
        this.tagRoot = root;
        this.tagForm = form;
        this.body = [];
        this.rules = [];
        return this;
    },
    render: function () {

        if (this.body.length == 0) {
            return;
        }
        var html = this.body.join('');
        $(this.tagForm).html(html);

        if (this.control.length > 0) {
            var ctlScript = this.control.join('');
            this.appendScript(ctlScript);
        }
        if (this.rules.length > 0) {
            var vldScript = this.rules.join();
            var jxscript = '$("' + this.tagForm + '").jqxValidator({rtl: true, hintType: "label", animationDuration: 0, rules: [' + vldScript + '\n]});';
            this.appendScript(jxscript);
        }
    },
    appendScript: function (script) {
        if (script && script.length > 0) {
            var element = document.createElement("script");
            element.type = "text/javascript";
            //script.src = "path/to/your/javascript.js";  
            element.text = script;
            document.body.appendChild(element);
        }
    },
    addItem: function (item) {
        this.body.push(item);
    },
    addControl: function (item) {
        this.control.push(item);
    },
    addRule: function (item) {
        this.rules.push(item);
    },
    addItemInputwRule: function (label, id, type, message,action) {
        this.body.push(app_field_T.itemInput(label, id, type));
        this.rules.push(app_Rule_T.input('#' + id, message, 'required', action));
    }
};

app_rule_Builder = {
    tagForm:'',
    rules: [],
    init: function (tag) {
        this.tagForm = tag;
        this.rules = [];
        return this;
    },
    render: function () {
        if (this.rules.length > 0) {
            var vldScript = this.rules.join();
            var jxscript = '$("' + this.tagForm + '").jqxValidator({rtl: true, hintType: "label", animationDuration: 0, rules: [' + vldScript + '\n]});';
            this.appendScript(jxscript);
        }
    },
    appendScript: function (script) {
        if (script && script.length > 0) {
            var element = document.createElement("script");
            element.type = "text/javascript";
            //script.src = "path/to/your/javascript.js";  
            element.text = script;
            document.body.appendChild(element);
        }
    },
    addRule: function (item) {
        this.rules.push(item);
    },
    firstName: function () {
        this.rules.push(app_Rule_T.input('חובה לציין שם פרטי!'));
    },
    lastName: function () {
        this.rules.push(app_Rule_T.input('חובה לציין שם משפחה!'));
    },
    address: function (item) {
        this.rules.push(app_Rule_T.input('חובה לציין כתובת!'));
    },

};

})(jQuery)



/*
var app_form_BuilderSample = function (tag) {

    var builder = new app_form_Builder();
    builder.init('#formContainer');
    builder.addItem(app_field_T.formBegin("form", "/Co/Test"));
    builder.addItem(app_field_T.sectionBegin("section1", "פרטים 1"));
    builder.addItem(app_field_T.itemInput("שם", "name"));
    builder.addItem(app_field_T.addItemInputwRule("תעודת זהות","text", "IdNumber","חובה לציין תעודת זהות"));
    builder.addItem(app_field_T.itemInput("כתובת", "address"));

    builder.addItem(app_field_T.sectionEnd());
    builder.addItem(app_field_T.formEnd());
    builder.render();
};

*/