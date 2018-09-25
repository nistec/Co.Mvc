'use strict';

class member_mv {

    constructor($element, dataModel) {
        this.$element = $element;
        this.Title = "עריכת מנוי";
        this.Name = "Member";
        this.Tag = "#member";
        this.TagForm = "#member-Form";

        if (dataModel)
            this.init(dataModel);
    }

    _loadHtml(dataModel) {

        var head =`
   <div class="panel-page-header">
         <h3 class="rtl" style="margin-right:5px">
                עריכת מנוי
         </h3>
   </div>
`

        var body=`
<div id="fcWindow" style="margin:5px">
    <div id="fcBody">
        <form class="fcForm" id="member-Form" method="post" action="/Co/MemberUpdate">
            <div style="direction: rtl; text-align: right;">
                <input type="hidden" id="ExType" />
                <input type="hidden" id="RecordId" name="RecordId" value="0" />
                <input type="hidden" id="member-AccountId" name="AccountId" />
                <input type="hidden" id="Categories" name="Categories" value="" />
                <div class="tab-container">
                    <div id="tab-personal" class="tab-group-toggle">
                        <h3 class="panel-area-title">פרטים אישיים</h3>
                        <div class="form-group-inline">
                            <div class="field">תעודת זהות :</div>
                            <input id="MemberId" name="MemberId" type="text" class="text-mid" />
                        </div>
                        <div class="form-group-inline">
                            <div class="field">שם פרטי :</div>
                            <input id="FirstName" name="FirstName" type="text" class="text-mid" />
                        </div>
                        <div class="form-group-inline">
                            <div class="field">שם משפחה :</div>
                            <input id="LastName" name="LastName" type="text" class="text-mid" />
                        </div>
                        <br />
                        <div class="form-group-inline">
                            <div class="field">כתובת :</div>
                            <input id="Address" name="Address" type="text" class="text-mid" />
                        </div>
                        <div class="form-group-inline">
                            <div class="field">עיר :</div>
                            <div id="City" name="City"></div>
                        </div>
                        <br />
                        <div class="form-group-inline">
                            <div class="field">טלפון נייד:</div>
                            <input id="CellPhone" name="CellPhone" type="text" class="text-mid" />
                        </div>
                        <div class="form-group-inline">
                            <div class="field">טלפון:</div>
                            <input id="Phone" name="Phone" type="text" class="text-mid" />
                        </div>
                        <div class="form-group-inline">
                            <div class="field">דאר אלקטרוני:</div>
                            <input id="Email" name="Email" type="text" class="text-mid" />
                        </div>
                        <br />
                        <div class="form-group-inline">
                            <div class="field">מגדר:</div>
                            <div id="Gender" name="Gender"></div>
                        </div>
                        <div class="form-group-inline">
                            <div class="field">תאריך לידה:</div>
                            <div id="Birthday" name="Birthday" data-type="date"></div>
                        </div>
                    </div>
                    <div id="tab-general" class="tab-group-toggle">
                        <h3 id="toggle-1" class="panel-area-title clickable">פרטים כלליים</h3>
                        <div class="tab-toggle">
                        <div class="form-group-inline">
                            <div class="field">חברה\\ארגון :</div>
                            <input id="CompanyName" name="CompanyName" type="text" class="text-mid" />
                        </div>
                        <div class="form-group-inline">
                            <div class="field">סניף :</div>
                            <div id="Branch" name="Branch"></div>
                        </div>
                        <br />
                        <div class="form-group-inline">
                            <div class="field">סווג :</div>
                            <div id="listCategory" name="listCategory" style="padding: 10px" data-type="checklist"></div>
                        </div>
                        <br />
                        <div id="divExId" class="form-group-inline field-ex">
                            <div id="lblExId" class="field">
                            </div>
                            <input type="text" id="ExId" name="ExId" class="text-mid" maxlength="50" />
                        </div>
                        <br />
                        <div id="divExEnum1" class="form-group-inline field-ex">
                            <div id="lblExEnum1" class="field">
                            </div>
                            <div id="ExEnum1" name="ExEnum1"></div>
                        </div>
                        <div id="divExEnum2" class="form-group-inline field-ex">
                            <div id="lblExEnum2" class="field">
                            </div>
                            <div id="ExEnum2" name="ExEnum2"></div>
                        </div>
                        <div id="divExEnum3" class="form-group-inline field-ex">
                            <div id="lblExEnum3" class="field">
                            </div>
                            <div id="ExEnum3" name="ExEnum3"></div>
                        </div>
                        <br />
                        <div id="divExField1" class="form-group-inline field-ex">
                            <div id="lblExField1" class="field">
                            </div>
                            <input type="text" id="ExField1" name="ExField1" class="text-mid" maxlength="250" />
                        </div>
                        <div id="divExField2" class="form-group-inline field-ex">
                            <div id="lblExField2" class="field">
                            </div>
                            <input type="text" id="ExField2" name="ExField2" class="text-mid" maxlength="250" />
                        </div>
                        <div id="divExField3" class="form-group-inline field-ex">
                            <div id="lblExField3" class="field">
                            </div>
                            <input type="text" id="ExField3" name="ExField3" class="text-mid" maxlength="250" />
                        </div>
                        <br />
                        <div id="divExRef1" class="form-group-inline field-ex">
                            <div id="lblExRef1" class="field">
                            </div>
                            <input type="number" id="ExRef1" name="ExRef1" />
                        </div>
                        <div id="divExRef2" class="form-group-inline field-ex">
                            <div id="lblExRef2" class="field">
                            </div>
                            <input type="number" id="ExRef2" name="ExRef2" />
                        </div>
                        <div id="divExRef3" class="form-group-inline field-ex">
                            <div id="lblExRef3" class="field">
                            </div>
                            <input type="number" id="ExRef3" name="ExRef3" />
                        </div>
                      </div>
                    </div>
                    <div id="tab-notes" class="tab-group-toggle">
                        <h3 id="toggle-2" class="panel-area-title clickable">הערות</h3>
                     <div class="tab-toggle">
                        <div class="form-group">
                            <div class="field">הערות:</div>
                            <textarea id="Note" name="Note" style="width:99%;height:60px"></textarea>
                        </div>
                        <div class="form-group-inline pasive" data-rule="e">
                            <div class="field">מועד הצטרפות:</div>
                            <input id="JoiningDate" name="JoiningDate" type="text" class="text-mid" readonly="readonly" />
                        </div>
                        <div class="form-group-inline pasive" data-rule="e">
                            <div class="field">מועד עדכון:</div>
                            <input id="LastUpdate" name="LastUpdate" type="text" readonly="readonly" class="text-mid" />
                        </div>
                    </div>
                    </div>
                </div>
            </div>
            <div style="height: 5px"></div>
            <div class="panel-area rtl">
                <input id="member-Submit" class="btn-default btn7" type="button" data-rule="u" style="display:none" value="עדכון" />
                <input id="member-Cancel" class="btn-default btn7" type="button" value="ביטול" />
                <input id="member-Clear" class="btn-default btn7" type="button" value="ניקוי" />
           </div>
        </form>
    </div>
</div>
`

        var html = dataModel.Inline ? body : '<div class="panel-window">' + head + body + '</div>';

        //var pasive = dataModel.Option == "a" ? " pasive" : "";
        //html.replace("#pasive#", pasive)
      
        if (dataModel.Inline == true) {
            app_panel.appendPanelAfter(this.$element, html, this.Title, '800px');
        }
        else
            $(this.$element).html(html);

        this.Rule = app_perms.renderRule(this.Name, dataModel.Option);

        $('#toggle-1').on('click', function (e) {
            $('#tab-general .tab-toggle').slideToggle();
            return false;
        });
        $('#toggle-2').on('click', function (e) {
            $('#tab-notes .tab-toggle').slideToggle();
            return false;
        });

        //buttons
        var _slf = this;

        $(this.Tag + "-Submit").on("click", function () {
            _slf.doSubmit();
        });
        $(this.Tag + "-Cancel").on("click", function () {
            _slf.doClose();
        });
        $(this.Tag + "-Clear").on("click", function () {
            _slf.doClear();
        });

        app_menu.activeLayoutMenu("liMain");
        app_menu.breadcrumbs("Main", "Members", 'en');
    }

    _loadData() {

        $("#ExType").val(this.ExType);

        if (this.RecordId > 0) {

            this.dataSource =
                {
                    datatype: "json",
                    id: 'RecordId',
                    data: { 'id': this.RecordId },
                    type: 'POST',
                    url: '/Co/GetMemberEdit'
                };

            var adapter = app_jqx_adapter.create(this.dataSource, function (record) {

                app_form.loadDataForm("member-Form", record);//, true, ["Birthday"]);

                //if (record.Birthday)
                //    $("#Birthday").val(record.Birthday)

                app_jqxcombos.selectCheckList("listCategory", record.Categories);

                app_jqxcombos.initComboValue('City', 0);
            });
            adapter.dataBind();

        }
        else {
            $("#AccountId").val(this.AccountId);
            $("#RecordId").val(0);
        }
    }

    _loadRules() {

        var exType = this.ExType;

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
        if (exType == 0)
            input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });
        if (exType == 1)
            input_rules.push({ input: '#CellPhone', message: 'חובה לציין טלפון נייד!', action: 'keyup, blur', rule: 'required' });
        if (exType == 2)
            input_rules.push({ input: '#Email', message: 'חובה לציין דאר אלקטרוני!', action: 'keyup, blur', rule: 'required' });
        if (exType == 3)
            input_rules.push({ input: '#Exid', message: 'חובה לציין מזהה!', action: 'keyup, blur', rule: 'required' });

        $(this.TagForm).jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
        $(this.TagForm).jqxValidator('hide');

    }

    init(dataModel, userInfo) {

        this._loadHtml(dataModel);

        $("#AccountId").val(userInfo.AccountId);

        //this.wizControl;
        //this.dataSource;
        this.Inline = dataModel.Inline;
        this.RecordId = dataModel.Id;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.ExType = userInfo.ExType;
        //this.UserRole = userInfo.UserRole;
        //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        //if (this.wizControl == null) {
        //    this.wizControl = new wiz_control("member_def", this.TagWindow);
        //    this.wizControl.init(html, this.ExType, function (data) {

        $('#Birthday').val('');
        $('#Birthday').jqxDateTimeInput({ width: '150px', rtl: true });


        app_jqx_list.enum1ComboAdapter();
        app_jqx_list.enum2ComboAdapter();
        app_jqx_list.enum3ComboAdapter();
        app_members.displayMemberFields();

        app_jqx_list.branchComboAdapter();
        app_jqx_list.cityComboAdapter();
        app_jqx_list.genderComboAdapter();
        app_jqx_list.categoryCheckListAdapter();
        //app_jqx_list.chargeComboAdapter('ChargeType');

        this._loadData();
        this._loadRules();
    }

    doDisplay() {

        if (this.Inline)
            app_panel.panelAfterShow(this.$element);
        else
            $(this.$element).show();
    }

    doSubmit() {

        var _slf = this;
        app_jqxcombos.renderCheckList("listCategory", "Categories");

        app_form.doFormPost(this.TagForm, function (data) {

            _slf.doClose(true);

            //if (_slf.Inline) {
            //    app_members_base.triggerMembersRefresh();
            //    app_panel.panelAfterClose(_slf.$element);
            //}
            //else {
            //    app.redirectTo("/co/members");
            //}
        });//, preSubmit, validatorTag);
    }

    doClose(refresh) {
        if (this.Inline) {
            if (refresh)
                app_members_base.triggerMembersRefresh();
            app_panel.panelAfterClose(this.$element);
        }
        else {
            app.redirectTo("/co/members");
        }
    }

    doClear() {
        app_form.clearDataForm(this.TagForm, ["AccountId","RecordId"]);
    }

}

    

 


    

 

 

  
