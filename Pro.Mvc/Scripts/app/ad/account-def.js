'use strict';

//============================================================================================ app_accounts_def_control

class app_accounts_def_control {

    constructor(tagWindow) {

        this.TagWindow = tagWindow;
    }

    init(dataModel, userInfo) {

        this.wizControl;
        this.dataSource;

        this.AccountId = dataModel.Id;
        this.UserInfo = userInfo;
        //this.AccountId = userInfo.AccountId;
        //this.UserRole = userInfo.UserRole;
        //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.dataSource =
            {
                datatype: "json",
                id: 'AccountId',
                data: { 'id': this.AccountId },
                type: 'POST',
                url: '/Admin/GetAccountsEdit'
            };
        var pasive = dataModel.Option == "a" ? " pasive" : "";
        var html =
            '<div id="fcWindow">' +
            '<div id="fcBody">' +
            '<form class="fcForm" id="fcForm" method="post" action="/Admin/AccountUpdate">' +
            '<div style="direction: rtl; text-align: right;">' +
            '<div class="tab-container-99">' +
            '<div id="tab-personal" class="tab-group">' +
            '<h3 class="panel-area-title">פרטים</h3>' +
            '<div class="form-group">' +
            '<div class="field">קוד לקוח :</div>' +
            '<input id="AccountId" name="AccountId" type="text" class="text-mid" readonly />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">ת.ז\ח.פ :</div>' +
            '<input id="IdNumber" name="IdNumber" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">שם לקוח :</div>' +
            '<input id="AccountName" name="AccountName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">כתובת :</div>' +
            '<input id="Address" name="Address" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">עיר :</div>' +
            '<input id="City" name="City" type="text" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">טלפון נייד:</div>' +
            '<input id="Mobile" name="Mobile" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">טלפון:</div>' +
            '<input id="Phone" name="Phone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">דאר אלקטרוני:</div>' +
            '<input id="Email" name="Email" type="text" class="text-mid" />' +
            '</div>' +
            '</div>' +

            '<div id="tab-general" class="tab-group">' +
            '<h3 class="panel-area-title">פרטים כלליים</h3>' +
            '<div class="form-group">' +
            '<div class="field">סניף :</div>' +
            '<input id="Branch" name="Branch" type="text" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">נתיב :</div>' +
            '<input id="Path" name="Path" type="text" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">קטגוריה :</div>' +
            '<div id="AccountCategory" name="AccountCategory"></div>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">חשבון תקשורת :</div>' +
            '<input id="AuthAccount" name="AuthAccount" type="text" class="text-mid"/>' +
            ' <a href="#" id="AuthAccount-btn" class="btn-small" title="עדכון חשבון תקשורת" >...</a>' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">מספר מנוי :</div>' +
            '<input id="CoMember" name="CoMember" type="text" class="text-mid"/>' +
            ' <a href="#" id="CoAccount-btn" class="btn-small" title="עדכון מנוי" >...</a>' +
            '</div>' +


            '</div>' +

            '<div id="tab-notes" class="tab-group">' +
            '<h3 class="panel-area-title">הערות</h3>' +
            '<div class="form-group">' +
            '<div class="field">הערות:</div>' +
            '<textarea id="Note" name="Note" style="width:100%;height:60px"></textarea>' +
            '</div>' +
            '<div class="form-group">' +
            '<input id="IsActive" name="IsActive" type="checkbox"/> <span> פעיל</span>' +
            '</div>' +
            '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="CreationDate" name="CreationDate" type="text" class="text-mid" readonly="readonly" />' +
            '</div>' +
            '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד עדכון:</div>' +
            '<input id="LastUpdate" name="LastUpdate" type="text" readonly="readonly" class="text-mid" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div style="clear: both;"></div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        //    '<div style="height: 5px"></div>' +
        //'<p id="validator-message" style="color:red"></p>' +
        //'<div style="display:none">' +
        //'<input id="fcSubmit" class="btn-default btn7" type="button" value="עדכון" />' +
        //'<input id="fcCancel" class="btn-default btn7" type="button" value="ניקוי" />' +
        //'</div>' +

        if (this.wizControl == null) {
            this.wizControl = new wiz_control("member_def", this.TagWindow);
            this.wizControl.init(html, null, function (data) {

                //app_jqx_list.cityComboAdapter();
                //app_jqx_list.categoryCheckListAdapter();
                //app_jqx_list.chargeComboAdapter('ChargeType');


                var input_rules = [
                    { input: '#AccountName', message: 'חובה לציין שם לקוח!', action: 'keyup, blur', rule: 'required' },
                    //{ input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
                    //{
                    //    input: "#City", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
                    //        var index = $("#City").jqxComboBox('getSelectedIndex');
                    //       return index != -1;
                    //    }
                    //},
                    { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
                    {
                        input: '#Mobile', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
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
                input_rules.push({ input: '#IdNumber', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
                input_rules.push({ input: '#Mobile', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
                input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });

                $('#fcForm').jqxValidator({
                    rtl: true,
                    hintType: 'label',
                    animationDuration: 0,
                    rules: input_rules
                });
                $('#fcForm').jqxValidator('hide');
            });
        }
        else {
            this.wizControl.clearDataForm("fcForm");
            //app_jqxcombos.clearCheckList("#listCategory");
        }

    }

    display() {
        $(this.TagWindow).show();

        if (this.AccountId > 0) {
            this.wizControl.load("fcForm", this.dataSource, function (record) {

                app_form.loadDataForm("fcForm", record, ["AccountCategory"]);//, true, ["Birthday"]);

                app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", "AccountCategory", "/Admin/GetAccountsCategories", null, 240, 140, "", record.AccountCategory);


                //if (record.Birthday)
                //    $("#Birthday").val(record.Birthday)

                //app_jqxcombos.selectCheckList("listCategory", record.Categories);

                //app_jqxcombos.initComboValue('City', 0);

            });
        }
        else {
            app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", "AccountCategory", "/Admin/GetAccountsCategories", null, 240, 140, "");

            //$("#AccountId").val(this.AccountId);
            $("#AccountId").val(0);
        }
    }

    doCancel() {
        this.wizControl.doCancel();
    }

    doSubmit() {
        this.wizControl.doSubmit(
            function () {
                //app_jqxcombos.renderCheckList("listCategory", "Categories");
            },
            function (data) {
                app_dialog.alert(data.Message);
                if (data.Status >= 0) {
                    //if (slf.IsDialog) {

                    if (window.parent.triggerWizControlCompleted)
                        window.parent.triggerWizControlCompleted("member_def", data.OutputId);
                    else if (triggerWizControlCompleted)
                        triggerWizControlCompleted("member_def", data.OutputId);

                    //    //$('#fcForm').reset();
                    //}
                    //else {
                    //    app.refresh();
                    //}
                    //$('#RecordId').val(data.OutputId);
                }
            }
        );
    }

    doClear() {
        this.wizControl.clearDataForm("fcForm");
        //app_jqxcombos.clearCheckList("#listCategory");
    }

    doSubmitAdd() {
        this.wizControl.doSubmit(
            function () {
                //app_jqxcombos.renderCheckList("listCategory", "Categories");
            },
            function (data) {
                app_dialog.alert(data.Message);
                //if (data.Status >= 0) {
                //    window.parent.triggerWizControlCompleted("member_def", data.OutputId);
                //}
            }
        );
    }
}


class app_user_def_control extends panel_dialog_base {

    constructor($element) {
        super($element, "user", "הגדרת משתמש");
    }

    loadHtml($element, readonly, callback) {

        var pasive = readonly ? " pasive" : "";

        var html = '<div id="user-Window" class="container" style="margin:5px;width:400px">' +
            //'<div id="fcWindow">' +
            '<div id="fcBody">' +
            '<form class="fcForm" id="user-Form" method="post" action="/Admin/UserDefUpdate">' +
            '<div style="direction: rtl; text-align: right;">' +
            '<input type="hidden" id="user-AccountId" name="AccountId" />' +
            '<div class="tab-container">' +
            '<div id="tab-personal" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">קוד משתמש :</div>' +
            '<input id="user-UserId" name="UserId" type="text" class="text-mid" readonly />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם לתצוגה :</div>' +
            '<input id="user-DisplayName" name="DisplayName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם משתמש :</div>' +
            '<input id="user-UserName" name="UserName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון:</div>' +
            '<input id="user-Phone" name="Phone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">דאר אלקטרוני:</div>' +
            '<input id="user-Email" name="Email" type="text" class="text-mid" />' +
            '</div>' +
            '</div>' +

            '<div id="tab-general" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים כלליים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">ימי נסיון :</div>' +
            '<input id="user-Evaluation" name="Evaluation" type="text" value="0" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שפה :</div>' +
            '<input id="user-Lang" name="Lang" type="text" value="he" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">תפקיד :</div>' +
            '<select id="user-UserRole" name="UserRole"></select>' +
            '</div>' +

            '<div class="form-group-inline">' +
            '<input id="user-IsBlocked" name="IsBlocked" type="checkbox"/> <span> חסום</span>' +
            '</div>' +
            '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="user-Creation" name="Creation" type="text" class="text-mid" readonly="readonly" />' +
            '</div>' +

            '</div>' +
            '</div>' +
            '<div>' +
            '<a id="user-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="user-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
            '</div>' +
            //'<div style="clear: both;"></div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        callback(html);
    };

    inputRules() {

        var input_rules = [
            { input: '#user-UserName', message: 'חובה לציין שם משתמש!', action: 'keyup, blur', rule: 'required' },
            { input: '#user-DisplayName', message: 'חובה לציין שם תצוגה!', action: 'keyup, blur', rule: 'required' },
            { input: '#user-Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
            {
                input: '#user-Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
                    function (input, commit) {
                        var val = input.val();
                        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                        return val ? re.test(val) : true;
                    }
            }
        ];
        input_rules.push({ input: '#user-Phone', message: 'חובה לציין טלפון!', action: 'keyup, blur', rule: 'required' });
        input_rules.push({ input: '#user-Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });


        $('#user-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
        $('#user-Form').jqxValidator('hide');

    }

    loadData(actModel, record) {
        app_control.selectTag("#user-UserRole", 120);
        app_control.fillSelect("#user-UserRole", "/Admin/GetUsersRoles", null, "RoleId", "RoleName", record ? record.UserRole : null);

        if (actModel.Option == "a") {

            $('#user-Form input[name=UserId]').val(0);
            $('#user-Form input[name=AccountId]').val(actModel.AccountId);
        }
        else {
            app_form.loadDataForm("user-Form", record, ["UserRole"]);

            if (actModel.UserRole <= record.UserRole && actModel.UserId != record.UserId ) {
                $("#user-Submit").hide();
            }
        }

    }
}

class app_user_def_control2 {

    constructor($element) {
        this.$element = $($element);
        this.Adapter = null;
        this.Title = "הגדרת משתמש";
    }

    tag($element, readonly) {

        var pasive = readonly ? " pasive" : "";

        var html = '<div id="user-Window" class="container" style="margin:5px;width:400px">' +
            //'<div id="fcWindow">' +
            '<div id="fcBody">' +
            '<form class="fcForm" id="user-Form" method="post" action="/Admin/UserDefUpdate">' +
            '<div style="direction: rtl; text-align: right;">' +
            '<input type="hidden" id="user-AccountId" name="AccountId" />' +
            '<div class="tab-container">' +
            '<div id="tab-personal" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">קוד משתמש :</div>' +
            '<input id="user-UserId" name="UserId" type="text" class="text-mid" readonly />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם לתצוגה :</div>' +
            '<input id="user-DisplayName" name="DisplayName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם משתמש :</div>' +
            '<input id="user-UserName" name="UserName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון:</div>' +
            '<input id="user-Phone" name="Phone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">דאר אלקטרוני:</div>' +
            '<input id="user-Email" name="Email" type="text" class="text-mid" />' +
            '</div>' +
            '</div>' +

            '<div id="tab-general" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים כלליים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">ימי נסיון :</div>' +
            '<input id="user-Evaluation" name="Evaluation" type="text" value="0" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שפה :</div>' +
            '<input id="user-Lang" name="Lang" type="text" value="he" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">תפקיד :</div>' +
            '<div id="user-UserRole" name="UserRole"></div>' +
            '</div>' +

            '<div class="form-group-inline">' +
            '<input id="user-IsBlocked" name="IsBlocked" type="checkbox"/> <span> חסום</span>' +
            '</div>' +
            '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="user-Creation" name="Creation" type="text" class="text-mid" readonly="readonly" />' +
            '</div>' +

            '</div>' +
            '</div>' +
            '<div>' +
            '<a id="user-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="user-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
            '</div>' +
            //'<div style="clear: both;"></div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        if ($element == null)
            return html;

        app_panel.appendPanelAfter($element, html, this.Title);

        //$element.html(html).hide();
    };

    inputRules() {

        var input_rules = [
            { input: '#user-UserName', message: 'חובה לציין שם משתמש!', action: 'keyup, blur', rule: 'required' },
            { input: '#user-DisplayName', message: 'חובה לציין שם תצוגה!', action: 'keyup, blur', rule: 'required' },
            { input: '#user-Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
            {
                input: '#user-Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
                    function (input, commit) {
                        var val = input.val();
                        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                        return val ? re.test(val) : true;
                    }
            }
        ];
        input_rules.push({ input: '#user-Phone', message: 'חובה לציין טלפון!', action: 'keyup, blur', rule: 'required' });
        input_rules.push({ input: '#user-Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });


        $('#user-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
        $('#user-Form').jqxValidator('hide');

    }

    init(actModel, record, dataAdapter, readonly) {
        this.AccountId = actModel.AccountId;
        this.Adapter = dataAdapter;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(this.$element.selector, readonly);

        if (actModel.Option == "a") {

            $('#user-Form input[name=UserId]').val(0);
            $('#user-Form input[name=AccountId]').val(actModel.AccountId);
        }
        else {

            app_form.loadDataForm("user-Form", record, ["UserRole"]);
        }

        $('#user-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#user-Cancel').on('click', function (e) {
            slf.doClose();
            return false;
        });

        this.inputRules();
    };

    display() {
        app_panel.panelAfterShow(this.$element.selector);
    }

    doClose() {
        app_panel.panelAfterClose(this.$element.selector);
    };

    doSubmit() {
        var slf = this;
        //e.preventDefault();
        var actionUrl = $('#user-Form').attr('action');
        var formData = $('#user-Form').serialize();

        app_query.doFormSubmit('#user-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {

                if (slf.Adapter)
                    slf.Adapter.dataBind();
                slf.doClose();
                //if (slf.Dialog)
                //    app_dialog.dialogClose(slf.Dialog);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };

    doClear() {
        app_form.clearDataForm("user-Form");
    }
}

class app_account_price_control {

    constructor($element) {
        this.$element = $($element);
        this.Adapter = null;
        this.Title = "הגדרת מחירים";
    }

    tag($element, readonly) {

        var pasive = readonly ? " pasive" : "";

        var html = '<div id="user-Window" class="container" style="margin:5px;width:400px">' +
            //'<div id="fcWindow">' +
            '<div id="fcBody">' +
            '<form class="fcForm" id="user-Form" method="post" action="/Admin/AccountPriceUpdate">' +
            '<div style="direction: rtl; text-align: right;">' +
            '<input type="hidden" id="user-AccountId" name="AccountId" />' +
            '<div class="tab-container">' +
            '<div id="tab-personal" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">קוד משתמש :</div>' +
            '<input id="user-UserId" name="UserId" type="text" class="text-mid" readonly />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם לתצוגה :</div>' +
            '<input id="user-DisplayName" name="DisplayName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שם משתמש :</div>' +
            '<input id="user-UserName" name="UserName" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">טלפון:</div>' +
            '<input id="user-Phone" name="Phone" type="text" class="text-mid" />' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">דאר אלקטרוני:</div>' +
            '<input id="user-Email" name="Email" type="text" class="text-mid" />' +
            '</div>' +
            '</div>' +

            '<div id="tab-general" class="tab-group-1">' +
            '<h3 class="panel-area-title">פרטים כלליים</h3>' +
            '<div class="form-group-inline">' +
            '<div class="field">ימי נסיון :</div>' +
            '<input id="user-Evaluation" name="Evaluation" type="text" value="0" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">שפה :</div>' +
            '<input id="user-Lang" name="Lang" type="text" value="he" class="text-mid"/>' +
            '</div>' +
            '<div class="form-group-inline">' +
            '<div class="field">תפקיד :</div>' +
            '<div id="user-UserRole" name="UserRole"></div>' +
            '</div>' +

            '<div class="form-group-inline">' +
            '<input id="user-IsBlocked" name="IsBlocked" type="checkbox"/> <span> חסום</span>' +
            '</div>' +
            '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="user-Creation" name="Creation" type="text" class="text-mid" readonly="readonly" />' +
            '</div>' +

            '</div>' +
            '</div>' +
            '<div>' +
            '<a id="user-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="user-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
            '</div>' +
            //'<div style="clear: both;"></div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        if ($element == null)
            return html;

        app_panel.appendPanelAfter($element, html, this.Title);

        //$element.html(html).hide();
    };

    inputRules() {

        var input_rules = [
            { input: '#user-UserName', message: 'חובה לציין שם משתמש!', action: 'keyup, blur', rule: 'required' },
            { input: '#user-DisplayName', message: 'חובה לציין שם תצוגה!', action: 'keyup, blur', rule: 'required' },
            { input: '#user-Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
            {
                input: '#user-Phone', message: 'טלפון אינו תקין!', action: 'valuechanged, blur', rule:
                    function (input, commit) {
                        var val = input.val();
                        var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                        return val ? re.test(val) : true;
                    }
            }
        ];
        input_rules.push({ input: '#user-Phone', message: 'חובה לציין טלפון!', action: 'keyup, blur', rule: 'required' });
        input_rules.push({ input: '#user-Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });


        $('#user-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
        $('#user-Form').jqxValidator('hide');

    }

    init(actModel, record, dataAdapter, readonly) {
        this.AccountId = actModel.AccountId;
        this.Adapter = dataAdapter;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(this.$element.selector, readonly);

        if (actModel.Option == "a") {

            $('#user-Form input[name=UserId]').val(0);
            $('#user-Form input[name=AccountId]').val(actModel.AccountId);
        }
        else {

            app_form.loadDataForm("user-Form", record, ["UserRole"]);
        }

        $('#user-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#user-Cancel').on('click', function (e) {
            slf.doClose();
            return false;
        });

        this.inputRules();
    };

    display() {
        app_panel.panelAfterShow(this.$element.selector);
    }

    doClose() {
        app_panel.panelAfterClose(this.$element.selector);
    };

    doSubmit() {
        var slf = this;
        //e.preventDefault();
        var actionUrl = $('#user-Form').attr('action');
        var formData = $('#user-Form').serialize();

        app_query.doFormSubmit('#user-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {

                if (slf.Adapter)
                    slf.Adapter.dataBind();
                slf.doClose();
                //if (slf.Dialog)
                //    app_dialog.dialogClose(slf.Dialog);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };

    doClear() {
        if (this.wizControl) {
            this.wizControl.clearDataForm("user-Form");
        }
        else {
            app_form.clearDataForm("user-Form");
        }
    }
}

class app_account_credit_control {

    constructor($element) {
        this.$element = $($element);
        this.Adapter = null;
        this.Title = "הגדרת קרדיט";
    }

    tag($element, readonly) {

        var pasive = readonly ? " pasive" : "";

        var html = '<div id="label-Window" class="container" style="margin:5px;width:400px">' +
            //'<hr style="width:100%;border:solid 1px #15C8D8">' +
            //'<div id="label-Header">' +
            //'<span id="label-Title" style="font-weight: bold;">פרטים</span>' +
            //'</div>' +
            '<div id="label-Body">' +
            '<form class="fcForm" id="label-Form" method="post" action="/Admin/AccountsCreditUpdate">' +
            '<div style="direction: rtl; text-align: right">' +
            '<div style="height:5px"></div>' +

            '<div id="tab-content" class="tab-content" dir="rtl">' +
            '<input type="hidden" id="label-AccountId" name="AccountId"/>' +
            '<input type="hidden" id="label-LabelId" name="LabelId"/>' +
            '<div class="form-group">' +
            '<div class="field">שדה:</div>' +
            '<input type="text" id="label-Label" name="Label" autocomplete="off" class="text-content-mid"/>' +
            '</div>' +

            '<div class="form-group">' +
            '<div class="field">פרטים:</div>' +
            '<textarea id="label-Val" name="Val" class="text-content-mid"></textarea>' +
            '</div>' +

            '</div>' +

            '<div>' +
            '<a id="label-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="label-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
            '</div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        if ($element == null)
            return html;

        app_panel.appendPanelAfter($element, html, this.Title);
    };

    init(actModel, record, dataAdapter, readonly) {
        this.AccountId = actModel.AccountId;
        this.Adapter = dataAdapter;
        this.ReadOnly = (readonly) ? true : false;

        var slf = this;

        this.tag(this.$element.selector, readonly);

        app_jqx_combo_async.labelsInputAdapter("#label-Label", record ? record.Label : null, actModel.AccountId, "*");

        if (actModel.Option == "a") {

            $('#label-Form input[name=LabelId]').val(0);
            $('#label-Form input[name=AccountId]').val(actModel.AccountId);
        }
        else {

            //app_form.loadDataForm("label-Form", record, ["Label"]);
            $('#label-Form input[name=LabelId]').val(record.LabelId);
            $('#label-Form input[name=AccountId]').val(record.AccountId);
            if (record.Label) {
                $('#label-Form input[name=Label]').val(record.Label);
                $('#label-Form textarea[name=Val]').val(record.Val);
            }
        }

        $("#label-Title").text("פרטים: ");

        $('#label-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#label-Cancel').on('click', function (e) {
            slf.doClose();
            return false;
        });

        var input_rules = [
            { input: '#label-Label', message: 'חובה לציין שדה!', action: 'none', rule: 'required' },
        ];

        $('#label-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    };

    display() {
        app_panel.panelAfterShow(this.$element.selector);
    }

    doClose() {
        app_panel.panelAfterClose(this.$element.selector);
    }

    doSubmit() {
        var slf = this;
        //e.preventDefault();
        var actionUrl = $('#label-Form').attr('action');
        var formData = $('#label-Form').serialize();

        app_query.doFormSubmit('#label-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {

                if (slf.Adapter)
                    slf.Adapter.dataBind();
                slf.doClose();
                //if (slf.Dialog)
                //    app_dialog.dialogClose(slf.Dialog);
                //app_doc_base.triggerSubDocCompleted('label', data);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };
}

class app_account_label_control {

    constructor($element) {
        this.$element = $($element);
        this.Adapter = null;
        this.Title = "הגדרת פרטים";
    }

    tag($element, readonly) {

        var pasive = readonly ? " pasive" : "";

        var html = '<div id="label-Window" class="container" style="margin:5px;width:400px">' +
            //'<hr style="width:100%;border:solid 1px #15C8D8">' +
            //'<div id="label-Header">' +
            //'<span id="label-Title" style="font-weight: bold;">פרטים</span>' +
            //'</div>' +
            '<div id="label-Body">' +
            '<form class="fcForm" id="label-Form" method="post" action="/Admin/UpsertAccountsLabels">' +
            '<div style="direction: rtl; text-align: right">' +
            '<div style="height:5px"></div>' +

            '<div id="tab-content" class="tab-content" dir="rtl">' +
            '<input type="hidden" id="label-AccountId" name="AccountId"/>' +
            '<input type="hidden" id="label-LabelId" name="LabelId"/>' +
            '<div class="form-group">' +
            '<div class="field">שדה:</div>' +
            '<input type="text" id="label-Label" name="Label" autocomplete="off" class="text-content-mid"/>' +
            '</div>' +

            '<div class="form-group">' +
            '<div class="field">פרטים:</div>' +
            '<textarea id="label-Val" name="Val" class="text-content-mid"></textarea>' +
            '</div>' +

            '</div>' +

            '<div>' +
            '<a id="label-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="label-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
            '</div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        if ($element == null)
            return html;

        app_panel.appendPanelAfter($element, html, this.Title);
    };

    init(actModel,record, dataAdapter, readonly) {
        this.AccountId = actModel.AccountId;
        this.Adapter = dataAdapter;
        this.ReadOnly = (readonly) ? true : false;

        var slf = this;

        this.tag(this.$element.selector, readonly);

        app_jqx_combo_async.labelsInputAdapter("#label-Label", record ? record.Label : null, actModel.AccountId, "*");

        if (actModel.Option == "a") {

            $('#label-Form input[name=LabelId]').val(0);
            $('#label-Form input[name=AccountId]').val(actModel.AccountId);
        }
        else {

            //app_form.loadDataForm("label-Form", record, ["Label"]);
            $('#label-Form input[name=LabelId]').val(record.LabelId);
            $('#label-Form input[name=AccountId]').val(record.AccountId);
            if (record.Label) {
                $('#label-Form input[name=Label]').val(record.Label);
                $('#label-Form textarea[name=Val]').val(record.Val);
            }
        }
       
        $("#label-Title").text("פרטים: ");

        $('#label-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#label-Cancel').on('click', function (e) {
            slf.doClose();
            return false;
        });

        var input_rules = [
            { input: '#label-Label', message: 'חובה לציין שדה!', action: 'none', rule: 'required' },
        ];

        $('#label-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    };

    display() {
        app_panel.panelAfterShow(this.$element.selector);
    }

    doClose() {
        app_panel.panelAfterClose(this.$element.selector);
    }
    
    doSubmit() {
        var slf = this;
        //e.preventDefault();
        var actionUrl = $('#label-Form').attr('action');
        var formData = $('#label-Form').serialize();

        app_query.doFormSubmit('#label-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {

                if (slf.Adapter)
                    slf.Adapter.dataBind();
                slf.doClose();
                //if (slf.Dialog)
                //    app_dialog.dialogClose(slf.Dialog);
                //app_doc_base.triggerSubDocCompleted('label', data);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };
}