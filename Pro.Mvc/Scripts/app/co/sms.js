'use strict';

class app_sms {

    constructor() {
     //this.AccountId
        this.IsEditable = true;
        this.CurrentTimming = 1;
        this.init();
    }

    init() {

        var _slf = this;

        this.personalList = [
            { label: 'תז', value: 'MemberId', checked: false },
            { label: 'שם פרטי', value: 'FirstName', checked: false },
            { label: 'שם משפחה', value: 'LastName', checked: false },
            { label: 'טלפון נייד', value: 'CellPhone', checked: false },
            { label: 'דואר אלקטרוני', value: 'Email', checked: false }
        ];

        $('#btnShow').hide();

        //app_jqx_list.categoryComboAdapter();
        //app_jqxcombos.createCheckListAdapter("PropId", "PropName", "listCategory", "/Common/GetCategoriesView", 240, 140, false);


        app_jqx_list.categoryCheckListAdapter("listCategory", "Category");
        $("#listCategory").jqxListBox({ height: 250 });
        $('#listCategory').on('checkChange', function (event) {
            app_jqxcombos.listCheckBoxToInput("listCategory", "Category", "allCategory");
            //app_jqxcombos.listCheckBoxOutput("listCategory", "CategoryValue", "Category", "allCategory");
            _slf.getTargetsCount();
        });

        $("#allCategory").change(function () {
            if (this.checked) {
                $("#listCategory").jqxListBox('uncheckAll');
            }
            _slf.getTargetsCount();
        });

        $("#listPersonalDisplay").jqxListBox({ source: _slf.personalList, width: 200, height: 200, checkboxes: true, rtl: true });
        $('#listPersonalDisplay').on('checkChange', function (event) {
            app_jqxcombos.listCheckBoxToPersonal("listPersonalDisplay", "PersonalDisplay", null, true);
            $("#divPersonalFields").text($("#PersonalDisplay").val());
        });

        $('#datePending').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });
        $('#dateStartBatch').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

        
        
        //initialize validator.
        $('#form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: [
                //{
                //    input: '#TargetCount', message: 'אין נמענים לשליחה!', action: 'keyup, blur', rule: function () {
                //        var value = $("#TargetCount").val();
                //        return value > 0;
                //    }
                //},
                { input: '#Message', message: 'חובה לציין נוסח הודעה!', action: 'keyup, blur', rule: 'required' }
                //{
                //    input: "#City1", message: 'חובה לציין עיר!', action: 'keyup, select', rule: function (input, commit) {
                //        var index = $("#City1").jqxComboBox('getSelectedIndex');
                //        return index != -1;
                //    }
                //},
            ]
        });

        $('#test').on('click', function (e) {

            app_dialog.confirm("הדיוור יישלח ל  נמענים, האם להמשיך ?",
                callBack = function (arg) {
                   app_dialog.alert(arg);
                }, 'ok');

        });

        $('#wizard-submit').on('click', function (e) {

            _slf.doSubmit();

        });

        $('#reset').on('click', function (e) {
            location.reload();
            //$('#form')[0].reset();
            //$('#form').jqxValidator('hide');
        });

        $('#btnShow').on('click', function (e) {
            var cat;
            if ($('#allCategory').prop('checked'))
                cat = "all";
            else
                cat = $("#Category").val();

            if (cat === null || cat === "")
                app_dialog.alert("נא לסמן סיווג");
            else if ($("#TotalCount").val() > 0)
                app_dialog.dialogIframe('/Common/_Targets?mode=catsms&cat=' + cat, "580", "500", "רשימת נמענים לדיוור");

        });

        $("#Message").change(function () {
            var len = $(this).val().length;
            $('#CharCount').val(len);
        });

        $('#Message').keyup(function () {
            var len = $(this).val().length;
            $('#CharCount').val(len);
        });

        $('input[type=radio][name=ScheduleType]').change(function () {

            var group = this.value;
            //alert(group);
            //selectScedulerGroup(group);
            $('#timming-group-' + _slf.CurrentTimming).hide();
            $('#timming-group-' + group).show();
            _slf.CurrentTimming = group;

        });

        //this.initWizard();

        $('#wizard').jcxWizard({
            name:"wizard",
            height: '480px',
            showSteps: true,
            validate_step: function (e,active) {
                switch (active) {
                    case 0:
                        var value = $("#TotalCount").val();
                        if (value <= 0) {
                            return this.displayWizardValidation(e,"אין נמענים לשליחה");
                        }
                        break;
                    case 2:
                        var size = _slf.getMessageSize();
                        if (size <= 0) {
                            return this.displayWizardValidation(e,"חובה לציין נוסח ההודעה");
                        }
                        break;
                }
                return true;
            }
        });


        //this.tabScheduler();

        //var selectScedulerGroup = function (group) {
        //    $('#timming-' + group).show();
        //}

        //$('input[type=radio][name=ScheduleType]').change(function () {

        //    var group = this.value;
        //    //selectScedulerGroup(group);
        //    $('#timming-' + group).show();

        //});

    }

    tabScheduler() {

        

        //$('input:radio[name="ScheduleType"]').change(function () {

        //    var group = $('input:radio[name="ScheduleType"]:checked').val();
        //    selectScedulerGroup(group);

        //});


        //$('#timming-1').change(function () {
        //    selectScedulerGroup(1);
        //});
        //$('#timming-2').change(function () {
        //    selectScedulerGroup(2);
        //});
        ////$('#timming-3').change(function () {
        ////    selectScedulerGroup(3);
        ////});
        //$('#timming-4').change(function () {
        //    selectScedulerGroup(4);
        //});
    }


    doSubmit() {

        var count = $("#TotalCount").val();
        app_jqxcombos.listCheckBoxToPersonalFields("#listPersonalDisplay", "#PersonalDisplay", "#PersonalFields")

        var validationResult = function (isValid) {
            if (isValid) {
                app_dialog.confirm("הדיוור יישלח ל " + count + " נמענים, האם להמשיך ?",
                    callBack = function () {
                        app_dialog.dialogProgress(null, 'Send Broadcast');
                        $('#form').submit();
                    });
            }
        }
        $('#form').jqxValidator('validate', validationResult);

    }

    doProgress() {
        var val = progressbar.progressbar("value") || 0;
        progressbar.progressbar("value", val + Math.floor(Math.random() * 3));
        if (val <= 99) {
            progressTimer = setTimeout(this.doProgress, 50);
        }
    }
    
    /*
    initWizard() {
        var _slf = this;
        //wizard ====================================
        
        var $tabs = $('#wizard').tabs();
        $('#wizard').css('height', '480px');
        $('.prev-tab').hide();
        $('.end-tab').hide();

        $('.next-tab').click(function () {

            var totalSize = $(".ui-tabs-panel").size() - 1;
            var active = $('#wizard').tabs("option", "active");
            switch (active) {
                case 0:
                    var value = $("#TotalCount").val();
                    if (value <= 0) {
                        return displayWizardValidation("אין נמענים לשליחה");
                    }
                    break;
                case 2:
                    var size = _slf.getMessageSize();
                    if (size <= 0) {
                        return displayWizardValidation("חובה לציין נוסח ההודעה");
                    }
                    break;
            }

            //$('#wizard-validation').val("");
            var next = active + 1;
            if (next >= totalSize) {
                $('.next-tab').hide();
                $('.end-tab').show();
            }
            $('.prev-tab').show();
            $('#wizard').tabs({ active: next });
            displayStep(next, totalSize);
            return false;
        });

        $('.prev-tab').click(function () {
            var active = $('#wizard').tabs("option", "active");
            var prev = active - 1;
            if (prev <= 0) {
                $('.prev-tab').hide();
            }
            $('.end-tab').hide();
            $('.next-tab').show();
            $('#wizard-validation').val("");
            $('#wizard').tabs({ active: prev });
            displayStep(prev);
            //$tabs.tabs('select', $(this).attr("rel"));
            return false;
        });

        var displayStep = function (step, totalSize) {
            step++;
            if (totalSize === undefined)
                totalSize = $(".ui-tabs-panel").size() - 1;
            totalSize++;
            $('#wizard-validation').val("שלב " + step + " מתוך " + totalSize);
        }

        var displayWizardValidation = function (message) {
            $('#wizard-validation').val(message);
            return false;
        }
        displayStep(0);

        //end wizard =====================================

    }
    */

    getMessageSize() {

      return  $("#Message").val().length;
    }


    setTargetsCount(value) {

        $("#TotalCount").val(value);
        if (value > 0)
            $('#btnShow').show();
        else
            $('#btnShow').hide();
    }


    getTargetsCount(isAll) {
        var _slf = this;
        var cat;
        if ($('#allCategory').prop('checked'))
            cat = "all";
        else
            cat = $("#Category").val();

        if (cat !== null && cat !== "") {

            $.ajax({
                url: '/Common/GetTargetsCount',
                type: 'post',
                dataType: 'json',
                data: { 'mode': 'catsms', 'cat': cat },
                success: function (data) {
                    _slf.setTargetsCount(data);
                },
                error: function (jqXHR, status, error) {
                    app_dialog.alert(error);
                }
            });
        }
        else {
            _slf.setTargetsCount(0);
        }
    }

}

    

 


    

 

 

  
