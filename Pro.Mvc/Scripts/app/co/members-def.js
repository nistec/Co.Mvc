
//============================================================================================ app_members_def

function app_members_def(recordId, userInfo, isdialog) {

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
    url: '/Main/GetMemberEdit'
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



/*
var app_members_def_control = function (tagWindow) {
    this.member_sync,
    this.init = function (dataModel, userInfo) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";
        var html =
    '<div id="fcWindow">' +
                '<div id="fcHeader" style="display:none">' +
                    '<h3 id="fcTitle" class="rtl">עריכת מנוי</h3>' +
                '</div>' +
        '<div id="fcBody" style="background-color:#fff">' +
            '<form class="fcForm" id="fcForm" method="post" action="/Main/MemberUpdate">' +
                '<div style="direction: rtl; text-align: right;">' +
                    '<input type="hidden" id="ExType" />' +
                    '<input type="hidden" id="RecordId" name="RecordId" value="0" />' +
                    '<input type="hidden" id="AccountId" name="AccountId" value="" />' +
                    '<input type="hidden" id="Categories" name="Categories" value="" />' +
                    '<div id="tabs" class="tab-container">' +
                         '<div id="tab-personal" class="tab-group">' +
                            '<h3>פרטים אישיים</h3>' +
                            '<div class="form-group">' +
                                '<div class="field">תעודת זהות :</div>' +
                                '<input id="MemberId" name="MemberId" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">שם פרטי :</div>' +
                                '<input id="FirstName" name="FirstName" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">שם משפחה :</div>' +
                                '<input id="LastName" name="LastName" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">כתובת :</div>' +
                                '<input id="Address" name="Address" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">עיר :</div>' +
                                '<div id="City" name="City"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">מגדר:</div>' +
                                '<div id="Gender" name="Gender"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">תאריך לידה:</div>' +
                                '<div id="Birthday" name="Birthday"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">טלפון נייד:</div>' +
                                '<input id="CellPhone" name="CellPhone" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">טלפון:</div>' +
                                '<input id="Phone" name="Phone" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">דאר אלקטרוני:</div>' +
                                '<input id="Email" name="Email" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">חברה\ארגון :</div>' +
                                '<input id="CompanyName" name="CompanyName" type="text" class="text-mid" />' +
                            '</div>' +
                        '</div>' +
                        '<div id="tab-general" class="tab-group">' +
                            '<h3>פרטים כלליים</h3>' +
        '<div class="form-group">' +
            '<div class="field">סניף :</div>' +
            '<div id="Branch" name="Branch"></div>' +
        '</div>' +
        '<div class="form-group">' +
            '<div class="field">סווג :</div>' +
            '<div id="listCategory" name="listCategory" style="padding: 10px" data-type="checklist"></div>' +
        '</div>' +
        '<div id="divExId" class="form-group field-ex">' +
            '<div id="lblExId" class="column">' +
            '</div>' +
            '<input type="text" id="ExId" name="ExId" class="text-mid" maxlength="50" />' +
        '</div>' +
        '<div id="divExEnum1" class="form-group field-ex">' +
            '<div id="lblExEnum1" class="column">' +
            '</div>' +
            '<div id="ExEnum1" name="ExEnum1"></div>' +
        '</div>' +
        '<div id="divExEnum2" class="form-group field-ex">' +
            '<div id="lblExEnum2" class="column">' +
            '</div>' +
            '<div id="ExEnum2" name="ExEnum2"></div>' +
        '</div>' +
        '<div id="divExEnum3" class="form-group field-ex">' +
            '<div id="lblExEnum3" class="column">' +
            '</div>' +
            '<div id="ExEnum3" name="ExEnum3"></div>' +
        '</div>' +
        '<div id="divExField1" class="form-group field-ex">' +
            '<div id="lblExField1" class="column">' +
            '</div>' +
            '<input type="text" id="ExField1" name="ExField1" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExField2" class="form-group field-ex">' +
            '<div id="lblExField2" class="column">' +
            '</div>' +
            '<input type="text" id="ExField2" name="ExField2" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExField3" class="form-group field-ex">' +
            '<div id="lblExField3" class="column">' +
            '</div>' +
            '<input type="text" id="ExField3" name="ExField3" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExRef1" class="form-group field-ex">' +
            '<div id="lblExRef1" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef1" name="ExRef1" />' +
        '</div>' +
        '<div id="divExRef2" class="form-group field-ex">' +
            '<div id="lblExRef2" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef2" name="ExRef2" />' +
        '</div>' +
        '<div id="divExRef3" class="form-group field-ex">' +
            '<div id="lblExRef3" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef3" name="ExRef3" />' +
        '</div>' +
    '</div>' +
    '<div id="tab-notes" class="tab-group">' +
        '<h3>הערות</h3>' +
        '<div class="form-group">' +
            '<div class="field">הערות:</div>' +
            '<textarea id="Note" name="Note" style="width:100%;height:60px"></textarea>' +
        '</div>' +
        '<div class="form-group '+ pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="JoiningDate" name="JoiningDate" type="text" class="text-mid" readonly="readonly" />' +
        '</div>' +
        '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד עדכון:</div>' +
            '<input id="LastUpdate" name="LastUpdate" type="text" readonly="readonly" class="text-mid" />' +
        '</div>' +
    '</div>' +
    '</div>' +
    '<div style="height: 5px"></div>' +
    '<p id="validator-message" style="color:red"></p>' +
    '<div style="display:none">' +
    '<input id="fcSubmit" class="btn-default btn7" type="button" value="עדכון" />' +
    '<input id="fcCancel" class="btn-default btn7" type="button" value="ניקוי" />' +
    '</div>' +
    '</div>' +
    '</form>' +
    '</div>' +
    '</div>';

        $(tagWindow).html(html).hide();

        var slf = this;
        //$('#comment-Submit').on('click', function (e) {
        //    e.preventDefault();
        //    slf.comment_sync.doSubmit();
        //});
        //$('#comment-Cancel').on('click', function (e) {
        //    slf.comment_sync.doCancel();
        //});
        if (this.member_sync == null)
            this.member_sync = new app_members_def_sync(dataModel.Id, userInfo);

        this.member_sync.init(dataModel.Id, userInfo);
        this.member_sync.load();
    },
    this.display = function () {
        $(tagWindow).show();
    }

}

var app_members_def_sync = function (recordId, userInfo) {

    var slf = this;

    this.init = function (recordId, userInfo) {
        this.RecordId = recordId;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        //this.IsDialog = isdialog;

        $("#AccountId").val(this.AccountId);

        $('#Birthday').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        $('#Birthday').val('');

        app_jqx_list.enum1ComboAdapter();
        app_jqx_list.enum2ComboAdapter();
        app_jqx_list.enum3ComboAdapter();
        app_members.displayMemberFields();

        app_jqx_list.branchComboAdapter();
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
        if (exType == 0)
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
    },
    this.load = function () {
        if (this.RecordId > 0) {
            if (this.viewAdapter == null) {
                var view_source =
                 {
                     datatype: "json",
                     id: 'RecordId',
                     data: { 'id': slf.RecordId },
                     type: 'POST',
                     url: '/Main/GetMemberEdit'
                 };

                this.viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        if (record) {

                            app_jqxform.loadDataForm("fcForm", record);

                            app_jqxcombos.selectCheckList("listCategory", record.Categories);

                            app_jqxcombos.initComboValue('City', 0);
                        }
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            }
            else {
                this.viewAdapter._source.data = { 'id': slf.RecordId };
            }
            this.viewAdapter.dataBind();
        }
        else {
            $('#RecordId').val(this.RecordId);
            $('#UserId').val(this.UserInfo.UserId);
            $('#AccountId').val(this.UserInfo.AccountId);
        }
    },

    this.doCancel = function () {

        window.parent.triggerSubTaskCompleted('comment');
    },

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
                            //if (slf.IsDialog) {
                                window.parent.triggerMemberComplete(data.OutputId);
                            //    //$('#fcForm').reset();
                            //}
                            //else {
                            //    app.refresh();
                            //}
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

    //if (userInfo !== undefined) {
    //    this.init(recordId, userInfo);
    //    this.load();
    //}
}

*/


var app_members_def_control = function (tagWindow) {

    this.wizControl,
    this.dataSource;

    this.init = function (dataModel, userInfo, extype) {
        this.RecordId = dataModel.Id;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.ExType = extype;
        //this.UserRole = userInfo.UserRole;
        //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.dataSource =
                 {
                     datatype: "json",
                     id: 'RecordId',
                     data: { 'id': this.RecordId },
                     type: 'POST',
                     url: '/Main/GetMemberEdit'
                 };
        var pasive = dataModel.Option == "a" ? " pasive" : "";
        var html =
    '<div id="fcWindow" style="border-top:solid 2px #15C8D8">' +
         '<div id="fcBody" style="background-color:#fff;border:solid 1px #15C8D8">' +
            '<form class="fcForm" id="fcForm" method="post" action="/Main/MemberUpdate">' +
                '<div style="direction: rtl; text-align: right;">' +
                    '<input type="hidden" id="ExType" />' +
                    '<input type="hidden" id="RecordId" name="RecordId" value="0" />' +
                    '<input type="hidden" id="AccountId" name="AccountId" value="' + userInfo.AccountId + '" />' +
                    '<input type="hidden" id="Categories" name="Categories" value="" />' +
                    '<div class="tab-container">' +
                         '<div id="tab-personal" class="tab-group">' +
                            '<h3>פרטים אישיים</h3>' +
                            '<div class="form-group">' +
                                '<div class="field">תעודת זהות :</div>' +
                                '<input id="MemberId" name="MemberId" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">שם פרטי :</div>' +
                                '<input id="FirstName" name="FirstName" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">שם משפחה :</div>' +
                                '<input id="LastName" name="LastName" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">כתובת :</div>' +
                                '<input id="Address" name="Address" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">עיר :</div>' +
                                '<div id="City" name="City"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">מגדר:</div>' +
                                '<div id="Gender" name="Gender"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">תאריך לידה:</div>' +
                                '<div id="Birthday" name="Birthday"></div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">טלפון נייד:</div>' +
                                '<input id="CellPhone" name="CellPhone" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">טלפון:</div>' +
                                '<input id="Phone" name="Phone" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">דאר אלקטרוני:</div>' +
                                '<input id="Email" name="Email" type="text" class="text-mid" />' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<div class="field">חברה\ארגון :</div>' +
                                '<input id="CompanyName" name="CompanyName" type="text" class="text-mid" />' +
                            '</div>' +
                        '</div>' +
                        '<div id="tab-general" class="tab-group">' +
                            '<h3>פרטים כלליים</h3>' +
        '<div class="form-group">' +
            '<div class="field">סניף :</div>' +
            '<div id="Branch" name="Branch"></div>' +
        '</div>' +
        '<div class="form-group">' +
            '<div class="field">סווג :</div>' +
            '<div id="listCategory" name="listCategory" style="padding: 10px" data-type="checklist"></div>' +
        '</div>' +
        '<div id="divExId" class="form-group field-ex">' +
            '<div id="lblExId" class="column">' +
            '</div>' +
            '<input type="text" id="ExId" name="ExId" class="text-mid" maxlength="50" />' +
        '</div>' +
        '<div id="divExEnum1" class="form-group field-ex">' +
            '<div id="lblExEnum1" class="column">' +
            '</div>' +
            '<div id="ExEnum1" name="ExEnum1"></div>' +
        '</div>' +
        '<div id="divExEnum2" class="form-group field-ex">' +
            '<div id="lblExEnum2" class="column">' +
            '</div>' +
            '<div id="ExEnum2" name="ExEnum2"></div>' +
        '</div>' +
        '<div id="divExEnum3" class="form-group field-ex">' +
            '<div id="lblExEnum3" class="column">' +
            '</div>' +
            '<div id="ExEnum3" name="ExEnum3"></div>' +
        '</div>' +
        '<div id="divExField1" class="form-group field-ex">' +
            '<div id="lblExField1" class="column">' +
            '</div>' +
            '<input type="text" id="ExField1" name="ExField1" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExField2" class="form-group field-ex">' +
            '<div id="lblExField2" class="column">' +
            '</div>' +
            '<input type="text" id="ExField2" name="ExField2" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExField3" class="form-group field-ex">' +
            '<div id="lblExField3" class="column">' +
            '</div>' +
            '<input type="text" id="ExField3" name="ExField3" class="text-mid" maxlength="250" />' +
        '</div>' +
        '<div id="divExRef1" class="form-group field-ex">' +
            '<div id="lblExRef1" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef1" name="ExRef1" />' +
        '</div>' +
        '<div id="divExRef2" class="form-group field-ex">' +
            '<div id="lblExRef2" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef2" name="ExRef2" />' +
        '</div>' +
        '<div id="divExRef3" class="form-group field-ex">' +
            '<div id="lblExRef3" class="column">' +
            '</div>' +
            '<input type="number" id="ExRef3" name="ExRef3" />' +
        '</div>' +
    '</div>' +
    '<div id="tab-notes" class="tab-group">' +
        '<h3>הערות</h3>' +
        '<div class="form-group">' +
            '<div class="field">הערות:</div>' +
            '<textarea id="Note" name="Note" style="width:100%;height:60px"></textarea>' +
        '</div>' +
        '<div class="form-group ' + pasive + '">' +
            '<div class="field">מועד הצטרפות:</div>' +
            '<input id="JoiningDate" name="JoiningDate" type="text" class="text-mid" readonly="readonly" />' +
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
    '</div>' +
    '</div>';

        //    '<div style="height: 5px"></div>' +
        //'<p id="validator-message" style="color:red"></p>' +
        //'<div style="display:none">' +
        //'<input id="fcSubmit" class="btn-default btn7" type="button" value="עדכון" />' +
        //'<input id="fcCancel" class="btn-default btn7" type="button" value="ניקוי" />' +
        //'</div>' +

        if (this.wizControl == null) {
            this.wizControl = new wiz_control("member_def", tagWindow);
            this.wizControl.init(html, this.ExType, function (data) {


                $('#Birthday').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
                $('#Birthday').val('');

                app_jqx_list.enum1ComboAdapter();
                app_jqx_list.enum2ComboAdapter();
                app_jqx_list.enum3ComboAdapter();
                app_members.displayMemberFields();

                app_jqx_list.branchComboAdapter();
                app_jqx_list.cityComboAdapter();
                app_jqx_list.genderComboAdapter();
                app_jqx_list.categoryCheckListAdapter();
                //app_jqx_list.chargeComboAdapter('ChargeType');


                var exType = data;// $("#ExType").val();

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

                $('#fcForm').jqxValidator({
                    rtl: true,
                    hintType: 'label',
                    animationDuration: 0,
                    rules: input_rules
                });

            });
        }
        else {
            this.wizControl.clearDataForm("fcForm");
            app_jqxcombos.clearCheckList("#listCategory");
        }
    },
    this.display = function () {
        $(tagWindow).show();
        $("#ExType").val(this.ExType);

        if (this.RecordId > 0) {
            this.wizControl.load("fcForm", this.dataSource, function (record) {

                app_jqxform.loadDataForm("fcForm", record);

                app_jqxcombos.selectCheckList("listCategory", record.Categories);

                app_jqxcombos.initComboValue('City', 0);

            });
        }
        else {
            $("#AccountId").val(this.AccountId);
            $("#RecordId").val(0);
        }
    },
    this.doCancel = function () {
        this.wizControl.doCancel();
    },
    this.doSubmit = function () {
        this.wizControl.doSubmit(
            function () {
                app_jqxcombos.renderCheckList("listCategory", "Categories");
            },
            function (data) {
                app_dialog.alert(data.Message);
                if (data.Status >= 0) {
                    //if (slf.IsDialog) {
                    window.parent.triggerWizControlCompleted("member_def", data.OutputId);

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
};
