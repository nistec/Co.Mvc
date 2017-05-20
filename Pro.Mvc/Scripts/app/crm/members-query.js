
//============================================================================================ app_members_query

function app_members_query(recordId,userInfo,isdialog) {

    this.RecordId = recordId;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    this.IsDialog = isdialog;

    $("#AccountId").val(this.AccountId);
    
    this.loadControls();
     
    this.doSubmit = function () {
        //e.preventDefault();
        var actionurl = $('#accForm').attr('action');
        app_jqxcombos.renderCheckList("listCategory", "Categories");
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
                            if (slf.IsDialog) {
                                window.parent.triggerMemberComplete(data.OutputId);
                                //$('#accForm').reset();
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
        $('#accForm').jqxValidator('validate', validationResult);
    };
    
    var slf = this;

    var branchAdapter = createComboAdapter("PropId", "PropName", "listBranch", "/Common/GetBranchView", 200, 120, true);
    $('#listBranch').on('change', function (event) {
        comboBoxToInput("listBranch", "Branch", "allBranch");
    });


    var placeAdapter = createComboAdapter("PropId", "PropName", "listPlace", "/Common/GetPlaceView", 200, 120, true);
    $('#listPlace').on('change', function (event) {
        comboBoxToInput("listPlace", "Place", "allPlace");
    });

    var categoryAdapter = createListAdapter("PropId", "PropName", "listCategory", "/Common/GetCategoriesView", 200, 200, true);
    $('#listCategory').jqxListBox({ multiple: true });
    $('#listCategory').on('change', function (event) {
        listBoxToInput("listCategory", "Category", "allCategory");
    });

    var statusAdapter = createComboAdapter("PropId", "PropName", "listStatus", "/Common/GetStatusView", 200, 120, false);
    $('#listStatus').on('change', function (event) {
        comboBoxToInput("listStatus", "Status", "allStatus");
    });

    var regionAdapter = createListAdapter("PropId", "PropName", "listRegion", "/Common/GetRegionView", 200, 120, false);
    $('#listRegion').on('change', function (event) {
        listBoxToInput("listRegion", "Region", "allRegion");
    });

    var currentRegion = function () {

        var reg = $("#Region").val();
        if (isNumeric(reg))
            return reg;
        return 0;
    }

    var citySource =
    {
        dataType: "json",
        dataFields: [
            { name: 'PropId' },
            { name: 'RegionId' },
            { name: 'PropName' }
        ],
        data: { 'region': currentRegion() },
        type: 'POST',
        url: '@Url.Action("GetCityRegionView", "Common")'
    };

    var cityAdapter = new $.jqx.dataAdapter(citySource, {
        //contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
            //alert("areaAdapter failed: " + error);
        },
        loadComplete: function (data) {
            //alert("areaAdapter is Loaded");
        }
    });
    // perform Data Binding.
    //areaAdapter.dataBind();


    $("#listCity").jqxListBox(
    {
        rtl: true,
        source: cityAdapter,
        width: 200,
        height: 120,
        multiple: true,
        displayMember: 'PropName',
        valueMember: 'PropId'
    });
    $('#listCity').on('change', function (event) {
        listBoxToInput("listCity", "City");//, "allCity");
    });

    $("#listRegion").bind('select', function (event) {
        if (event.args) {

            $("#listCity").jqxListBox({ disabled: false, selectedIndex: -1 });
            var value = event.args.item.value;
            citySource.data = { 'region': value };
            cityAdapter = new $.jqx.dataAdapter(citySource);
            $("#listCity").jqxListBox({ source: cityAdapter });
            $("#City").val("");
        }
    });



    $('#submit').on('click', function () {
        var action = $("#form").find('input[name=op]:checked').val();

        $("#form").attr('action', app.appPath() + action);
    });
    $("input[name=op]:radio").change(function () {
        var action = $("#form").find('input[name=op]:checked').val();
        $("#form").attr('action', app.appPath() + action);
    });
    $('#reset').on('click', function () {
        location.reload();
    });

    $("#allCity").click(function () {

        var items = $("#listCity").jqxListBox('getItems');
        jQuery.each(items, function (i, item) {
            $("#listCity").jqxListBox('selectItem', item);
        });
        listBoxToInput("listCity", "City");
    });
    $("#unselectAllCity").click(function () {
        $("#listCity").jqxListBox('clearSelection');
        $("#City").val("");
    });


};

app_members_query.prototype.syncData = function (record) {

    if (record) {

        app_jqxform.loadDataForm("accForm", record);
      
        app_jqxcombos.selectCheckList("listCategory", record.Categories);

    }
};

app_members_query.prototype.loadControls = function () {


    app_jqx_list.branchComboAdapter();
    //app_jqx_list.placeComboAdapter();
    //app_jqx_list.statusComboAdapter();
    app_jqx_list.regionComboAdapter();
    app_jqx_list.cityComboAdapter();
    app_jqx_list.genderComboAdapter();
    app_jqx_list.categoryCheckListAdapter();


    $('#accForm').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: [
              { input: '#FirstName', message: 'חובה לציין שם פרטי!', action: 'keyup, blur', rule: 'required' },
              { input: '#LastName', message: 'חובה לציין שם משפחה!', action: 'keyup, blur', rule: 'required' },
              { input: '#Address', message: 'חובה לציין כתובת!', action: 'keyup, blur', rule: 'required' },
               {
                   input: "#City", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
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

};
