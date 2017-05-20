
//============================================================================================ app_campaign_def

function app_user_def(userInfo) {

    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

    $("#AccountId").val(this.AccountId);
    $("#regAccountId").val(this.AccountId);

   // this.loadControls();

    var slf = this;

    // prepare the data
    var roleSource =
    {
        //async:false,
        dataType: "json",
        dataFields: [
            { name: 'RoleId' },
            { name: 'RoleName' }
        ],
        data: {},
        type: 'POST',
        url: '/System/GetUsersRoles'
    };
    var roleAdapter = new $.jqx.dataAdapter(roleSource, {
        contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
            //alert("roleAdapter failed: " + error);
        },
        loadComplete: function (data) {
            //alert("roleAdapter is Loaded");
        }
    });
    // perform Data Binding.
    //roleAdapter.dataBind();
    $("#UserRole").jqxDropDownList(
          {
              rtl: true,
              source: roleAdapter,
              width: 120,
              autoDropDownHeight: true,
              dropDownHorizontalAlignment: 'right',
              promptText: "בחירת תפקיד",
              displayMember: 'RoleName',
              valueMember: 'RoleId'
          });
    $("#regUserRole").jqxDropDownList(
          {
              rtl: true,
              source: roleAdapter,
              width: 120,
              autoDropDownHeight: true,
              dropDownHorizontalAlignment: 'right',
              promptText: "בחירת תפקיד",
              displayMember: 'RoleName',
              valueMember: 'RoleId'
          });

 
    var sendCommand = function (rowdata, command, commit) {

        var url;
        if (command == 0)
            url = '/System/UserDefRegister'
        else if (command == 2)
            url = '/System/UserDefDelete'
        else
            url = '/System/UserDefUpdate';

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: url,
            data: rowdata,
            success: function (data, status, xhr) {
                if (data.Commit) {
                    //alert('עודכן בהצלחה');
                    commit(true);
                    dataAdapter.dataBind();
                }
                else {
                   app_dialog.alert('לא עודכנו נתונים');
                    if (command == 0)
                        $('#regResult').val(data.Description);
                    else
                        $('#signResult').val(data.Description);
                    commit(false);
                }
            },
            error: function () {
               app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                commit(false);
            }
        });
    };
    var source =
    {
        //async: false,
        updaterow: function (rowid, rowdata, commit) {
            sendCommand(rowdata, 1, commit);
        },
        addrow: function (rowid, rowdata, position, commit) {
            sendCommand(rowdata, 0, commit);
        },
        deleterow: function (rowid, commit) {
            var rowdata = { 'UserId': rowid }
            sendCommand(rowdata, 2, commit);
        },
        dataType: "json",
        datafields:
        [
           { name: 'DisplayName', type: 'string' },
           { name: 'UserId', type: 'number' },
           { name: 'UserRole', type: 'number' },
           { name: 'RoleName', type: 'string' },
           { name: 'UserName', type: 'string' },
           { name: 'Email', type: 'string' },
           { name: 'Phone', type: 'string' },
           { name: 'AccountId', type: 'number' },
           { name: 'Lang', type: 'string' },
           { name: 'Evaluation', type: 'number' },
           { name: 'IsBlocked', type: 'bool' },
           { name: 'Creation', type: 'date' }

        ],
        data: {},
        id: 'UserId',
        type: 'POST',
        url: '/System/GetUsersProfile'
    };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
            //alert("dataAdapter failed: " + error);
        },
        loadComplete: function (data) {
            //alert("dataAdapter is Loaded");
        }
    });
    dataAdapter.dataBind();
    var editrow = -1;
    // initialize jqxGrid
    $("#jqxgrid").jqxGrid(
    {
        rtl: true,
        width: '80%',
        autoheight: true,
        source: dataAdapter,
        localization: getLocalization('he'),
        pageable: false,
        showtoolbar: true,
        rendertoolbar: function (toolbar) {
            var me = this;
            var container = $("<div style='margin: 5px;text-align:right'></div>");
            toolbar.append(container);
            container.append('<input id="updateupsbutton" type="button" value="סיסמה" />');
            container.append('<input id="addrowbutton" type="button" value="הוספה" />');
            container.append('<input style="margin-left: 5px;" id="deleterowbutton" type="button" value="מחיקה" title="מחיקת השורה המסומנת"/>');
            container.append('<input id="updaterowbutton" type="button" value="עריכה" title="עריכת השורה המסומנת"/>');
            container.append('<input id="refreshbutton" type="button" value="רענון" />');
            $("#addrowbutton").jqxButton();
            $("#deleterowbutton").jqxButton();
            $("#updaterowbutton").jqxButton();
            $("#refreshbutton").jqxButton();
            $("#updateupsbutton").jqxButton();

            if(slf.AllowEdit==0)
            {
                $("#addrowbutton").jqxButton("disabled",true);
                $("#deleterowbutton").jqxButton("disabled", true);
            }
            // update row.
            $("#updaterowbutton").on('click', function () {

                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                doRowEdit(selectedrowindex);

            });
            // create new row.
            $("#addrowbutton").on('click', function () {
                // show the popup window.
                //$("#regUserId").val('');
                $("#regUserName").val('');
                $("#regDisplayName").val('');
                $("#regUserRole").val('');
                $("#regEmail").val('');
                $("#regPhone").val('');
                $("#regAccountId").val('');
                $("#regLang").val('');
                $("#regEvaluation").val('');
                $("#regIsBlocked").val(false);
                $("#regCreation").val('');
                $("#regPass").val('');
                $("#regPassConfirm").val('');
                openEditor(0);
            });
            // delete row.
            $("#deleterowbutton").on('click', function () {
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                    var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                    var result = confirm("האם למחוק?");
                    if (result == true) {
                        //Logic to delete the item
                        var commit = $("#jqxgrid").jqxGrid('deleterow', id);
                    }
                }
            });
            // refresh grid.
            $("#refreshbutton").on('click', function () {
                dataAdapter.dataBind();
            });
            // update ups.
            $("#updateupsbutton").on('click', function () {

                var getUps = function (i) {
                    $.ajax({
                        dataType: 'json',
                        type: 'POST',
                        url: '/System/GetUps',
                        data: { 'UserId': i },
                        success: function (data, status, xhr) {
                            upsCallback(data);
                        },
                        error: function () {
                           app_dialog.alert('אירעה שגיאה, לא נמצאו נתונים');
                        }
                    });
                };

                function upsCallback(data) {
                    $("#MemberId").val(dataRecord.UserId);
                    $("#MemberName").val(dataRecord.DisplayName);
                    $("#Ups").val(data.Password);
                    $("#UpsConfirm").val(data.Password);

                    // open the popup window when the user clicks a button.
                    var offset = $("#jqxgrid").offset();
                    $("#popupPass").jqxWindow({ position: { x: parseInt(offset.left) + parseInt(offset.width) / 2, y: parseInt(offset.top) + 60 } });
                    // show the popup window.
                    $("#popupPass").jqxWindow('open');

                }

                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                    $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
                    // open the popup window when the user clicks a button.
                    editrow = selectedrowindex;
                    // get the clicked row's data and initialize the input fields.
                    var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
                    getUps(dataRecord.UserId);
                }

            });


        },
        columns: [
        { text: 'קוד משתמש', datafield: 'UserId', width: 90, cellsalign: 'right', align: 'center' },
        //{ text: 'קוד תפקיד', datafield: 'UserRole',width: 60, cellsalign: 'right', align: 'center' },
        { text: 'תפקיד', datafield: 'RoleName', width: 90, cellsalign: 'right', align: 'center' },
        { text: 'שם משתמש', datafield: 'UserName', width: 120, cellsalign: 'right', align: 'center' },
        { text: 'פרטים', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
        { text: 'אימייל', datafield: 'Email', cellsalign: 'right', align: 'center' },
        { text: 'טלפון', datafield: 'Phone', width: 120, cellsalign: 'right', align: 'center' },
        //{ text: 'חשבון', datafield: 'AccountId', cellsalign: 'right', align: 'center' },
        //{ text: 'נסיון', datafield: 'Evaluation', width: 60,cellsalign: 'right', align: 'center' },
        { text: 'חסום', datafield: 'IsBlocked', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
        { text: 'נוצר ב', datafield: 'Creation', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
        ]
    });
    var openEditor = function (mode) {
        $("#insertFlag").val(mode);

        var popw = (mode == 1) ? '#popupWindow' : '#popupRegister';

        // open the popup window when the user clicks a button.
        var offset = $("#jqxgrid").offset();
        $(popw).jqxWindow({ position: { x: parseInt(offset.left) + parseInt(offset.width) / 2, y: parseInt(offset.top) + 60 } });
        // show the popup window.
        $(popw).jqxWindow('open');
    };
    var doRowEdit = function (selectedrowindex) {

        var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
            // open the popup window when the user clicks a button.
            editrow = selectedrowindex;
            // get the clicked row's data and initialize the input fields.
            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);

            $("#UserId").val(dataRecord.UserId);
            $("#UserName").val(dataRecord.UserName);
            $("#DisplayName").val(dataRecord.DisplayName);
            $("#UserRole").val(dataRecord.UserRole);
            $("#Email").val(dataRecord.Email);
            $("#Phone").val(dataRecord.Phone);
            $("#AccountId").val(dataRecord.AccountId);
            $("#Lang").val(dataRecord.Lang);
            $("#Evaluation").val(dataRecord.Evaluation);
            $("#IsBlocked").val(dataRecord.IsBlocked);
            $("#Creation").val(dataRecord.Creation);
            //$("#Pass").val('');
            //$("#PassConfirm").val('');
            // show the popup window.
            //$("#popupWindow").jqxWindow('open');
            openEditor(1);
        }
    };
    $('#jqxgrid').on('rowdoubleclick', function (event) {
        var args = event.args;
        var boundIndex = args.rowindex;
        var visibleIndex = args.visibleindex;
        doRowEdit(boundIndex);
    });
    //$("#jqxgrid").on('cellselect', function (event) {
    //    var column = $("#jqxgrid").jqxGrid('getcolumn', event.args.datafield);
    //    var value = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
    //    var displayValue = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
    //    //$("#eventLog").html("<div>Selected Cell<br/>Row: " + event.args.rowindex + ", Column: " + column.text + ", Value: " + value + ", Label: " + displayValue + "</div>");
    //});
    // initialize the input fields.
    $("#UserRole").jqxDropDownList().width(150);
    $("#IsBlocked").jqxCheckBox();
    if (slf.AllowEdit == 0) {
        $("#UserRole").jqxDropDownList("disabled", true);
        $("#IsBlocked").jqxCheckBox("disabled", true);
    }
    //================= popup Update ================.
    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });
    $("#popupWindow").on('open', function () {
        $("#UserName").jqxInput('selectAll');
    });
    $('#popupWindow').jqxValidator({
        hintType: 'label',
        animationDuration: 0,
        rules: [
              { input: '#UserName', message: 'נדרש שם משתמש!', action: 'keyup, blur', rule: 'required' },
              { input: '#UserName', message: 'שם משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup, blur', rule: 'length=3,12' },
              {
                  input: '#UserName', message: 'נדרש אותיות באנגלית בלבד!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^[A-Za-z][A-Za-z0-9]*$/
                              return re.test(input.val());
                          }
              },
              { input: '#DisplayName', message: 'נדרש פרטי משתמש!', action: 'keyup, blur', rule: 'required' },
              { input: '#DisplayName', message: 'פרטי משתמש אותיות בלבד!', action: 'keyup', rule: 'notNumber' },
              { input: '#DisplayName', message: 'פרטי משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup', rule: 'length=3,12' },
              { input: '#Email', message: 'נדרש כתובת אימייל!', action: 'keyup, blur', rule: 'required' },
              { input: '#Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
              //{ input: '#UserRole', message: 'נדרש תפקיד!', action: 'valuechanged, blur', rule: 'required' },
              {
                  input: '#UserRole', message: 'נדרש תפקיד!', action: 'keyup, select', rule: function (input) {
                      var index = $("#UserRole").jqxDropDownList('getSelectedIndex');
                      if (index >= 0) { return true; } return false;
                  }
              },
              { input: '#Phone', message: 'נדרש טלפון נייד!', action: 'keyup, blur', rule: 'required' },
              {
                  input: '#Phone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                              return re.test(input.val());
                          }
              }
        ]
    });
    $('#popupWindow').on('validationSuccess', function (event) {
        var row = {
            UserId: $("#UserId").val(), UserName: $("#UserName").val(), UserRole: $("#UserRole").val(),
            Email: $("#Email").val(), Phone: $("#Phone").val(),
            AccountId: $("#AccountId").val(), Lang: $("#Lang").val(),
            Evaluation: 0, IsBlocked: $("#IsBlocked").val(),
            DisplayName: $("#DisplayName").val()
        };
        if ($("#insertFlag").val() == '0') {
            $('#jqxgrid').jqxGrid('addrow', null, row);
        }
        else if (editrow >= 0) {
            var rowID = $('#jqxgrid').jqxGrid('getrowid', editrow);
            $('#jqxgrid').jqxGrid('updaterow', rowID, row);
        }
        $("#popupWindow").jqxWindow('hide');
    });
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();
    // update the edited row when the user clicks the 'Save' button.
    $("#Save").click(function () {
        $('#popupWindow').jqxValidator('validate');

    });

    //================= popup Register ================.
    $("#regUserRole").jqxDropDownList().width(150);
    $("#regIsBlocked").jqxCheckBox();
    if (slf.AllowEdit == 0) {
        $("#regUserRole").jqxDropDownList("disabled", true);
        $("#regIsBlocked").jqxCheckBox("disabled", true);
    }

    $("#popupRegister").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#regCancel"), modalOpacity: 0.01
    });
    $("#popupRegister").on('open', function () {
        $("#regUserName").jqxInput('selectAll');
    });
    $('#popupRegister').jqxValidator({
        hintType: 'label',
        animationDuration: 0,
        rules: [
              { input: '#regUserName', message: 'נדרש שם משתמש!', action: 'keyup, blur', rule: 'required' },
              { input: '#regUserName', message: 'שם משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup, blur', rule: 'length=3,12' },
              {
                  input: '#regUserName', message: 'נדרש אותיות באנגלית בלבד ללא רווחים!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^[A-Za-z][A-Za-z0-9]*$/;// /[^A-Za-z]/g
                              return re.test(input.val());
                          }
              },
              { input: '#regDisplayName', message: 'נדרש פרטי משתמש!', action: 'keyup, blur', rule: 'required' },
              { input: '#regDisplayName', message: 'פרטי משתמש אותיות בלבד!', action: 'keyup', rule: 'notNumber' },
              { input: '#regDisplayName', message: 'פרטי משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup', rule: 'length=3,12' },
              { input: '#regEmail', message: 'נדרש כתובת אימייל!', action: 'keyup, blur', rule: 'required' },
              { input: '#regEmail', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
              //{ input: '#UserRole', message: 'נדרש תפקיד!', action: 'valuechanged, blur', rule: 'required' },
              {
                  input: '#regUserRole', message: 'נדרש תפקיד!', action: 'keyup, select', rule: function (input) {
                      var index = $("#regUserRole").jqxDropDownList('getSelectedIndex');
                      if (index >= 0) { return true; } return false;
                  }
              },
              { input: '#regPhone', message: 'נדרש טלפון נייד!', action: 'keyup, blur', rule: 'required' },
              {
                  input: '#regPhone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                              return re.test(input.val());
                          }
              },
              { input: '#regPass', message: 'נדרש סיסמה!', action: 'keyup, blur', rule: 'required' },
              { input: '#regPass', message: 'סיסמה בין 4 ל 12 תוים!', action: 'keyup, blur', rule: 'length=4,12' },
              {
                  input: '#regPass', message: 'נדרש אותיות באנגלית בלבד ללא רווחים!', action: 'valuechanged, blur', rule:
                          function (input, commit) {
                              var re = /^[A-Za-z][A-Za-z0-9]*$/;// /[^A-Za-z]/g
                              return re.test(input.val());
                          }
              },
              { input: '#regPassConfirm', message: 'נדרש אישור סיסמה!', action: 'keyup, blur', rule: 'required' },
              {
                  input: '#regPassConfirm', message: 'אישור סיסמה אינו תואם לסיסמה!', action: 'keyup, focus', rule: function (input, commit) {
                      // call commit with false, when you are doing server validation and you want to display a validation error on this field. 
                      if (input.val() === $('#regPass').val()) {
                          return true;
                      }
                      return false;
                  }
              }

        ]
    });
    $('#popupRegister').on('validationSuccess', function (event) {
        var row = {
            UserName: $("#regUserName").val(), UserRole: $("#regUserRole").val(),
            Email: $("#regEmail").val(), Phone: $("#regPhone").val(),
            AccountId: $("#regAccountId").val(), Lang: $("#regLang").val(),
            Evaluation: 0, IsBlocked: $("#regIsBlocked").val(),
            DisplayName: $("#regDisplayName").val(), Password: $("#regPass").val()
        };

        $('#jqxgrid').jqxGrid('addrow', null, row);

        $("#popupRegister").jqxWindow('hide');
    });
    $("#regCancel").jqxButton();
    $("#regSave").jqxButton();
    // update the edited row when the user clicks the 'Save' button.
    $("#regSave").click(function () {
        $('#popupRegister').jqxValidator('validate');

    });

    //================= popup Pass ================.
    $("#popupPass").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#CancelUps"), modalOpacity: 0.01
    });
    $("#popupPass").on('open', function () {
        $("#Ups").jqxInput('selectAll');
    });
    $("#CancelUps").jqxButton();
    $("#SaveUps").jqxButton();
    // update the edited row when the user clicks the 'Save' button.
    $("#SaveUps").click(function () {
        $('#popupPass').jqxValidator('validate');
    });
    $('#popupPass').jqxValidator({
        hintType: 'label',
        animationDuration: 0,
        rules: [
              { input: '#Ups', message: 'נדרש סיסמה!', action: 'keyup, blur', rule: 'required' },
              { input: '#Ups', message: 'סיסמה בין 4 ל 12 תוים!', action: 'keyup, blur', rule: 'length=4,12' },
              {
                  input: '#Ups', message: 'נדרש אותיות באנגלית בלבד ללא רווחים!', action: 'valuechanged, blur', rule:
                           function (input, commit) {
                               var re = /^[a-z0-9]*$/i;// /[^A-Za-z]/g
                               return re.test(input.val());
                           }
              },
              { input: '#UpsConfirm', message: 'נדרש אישור סיסמה!', action: 'keyup, blur', rule: 'required' },
              {
                  input: '#UpsConfirm', message: 'אישור סיסמה אינו תואם לסיסמה!', action: 'keyup, focus', rule: function (input, commit) {
                      // call commit with false, when you are doing server validation and you want to display a validation error on this field. 
                      if (input.val() === $('#Ups').val()) {
                          return true;
                      }
                      return false;
                  }
              }
        ]
    });
    $('#popupPass').on('validationSuccess', function (event) {
        if (editrow >= 0) {
            var rowID = $('#jqxgrid').jqxGrid('getrowid', editrow);
            updateups(rowID);
        }
        $("#popupPass").jqxWindow('hide');
    });
    var updateups = function (id) {
        //alert(rowdata.UserName);
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/System/UserUpsUpdate',
            data: { 'UserId': id, 'Ups': $("#Ups").val(), 'command': 1 },
            success: function (data, status, xhr) {
                if (data.Status >0) {
                    app_dialog.alert(data.Description);
                }
                else
                    app_dialog.alert('לא עודכנו נתונים');
                //commit(true);
            },
            error: function () {
                app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                //commit(false);
            }
        });
    };

};
