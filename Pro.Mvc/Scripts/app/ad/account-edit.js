'use strict';

//============================================================================================ app_accounts_def_control

var AccTrigger= {

    doEditLabel : function(row) {

        acc_def.Labels.doEdit(row);
    },
    doDeleteLabel: function (row) {

        acc_def.Labels.doDelete(row);
    }
}

class app_account_def {

    constructor(dataModel, userInfo) {

        this.$element = $("#accordion");
        //this.$element = document.querySelectorAll(element);

        //if (!this.$element.is('div')) {
        //    $.error('app_wiztabs should be applied to DIV element');
        //    return;
        //}
        //// ensure to use the `new` operator
        //if (!(this instanceof app_Account))
        //    return new app_Account(element, AccountId, userInfo, AccountModel);

        this.AccountId = dataModel.Id;
        this.AccountParentId = dataModel.PId;
        this.Model = dataModel;
        this.IsInfo = dataModel.IsInfo;
        this.UserInfo = userInfo;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;

        this.Title = "לקוח";//app_Accounts.AccountTitle(this.AccountModel);
        this.uploader = undefined;
        this.IsNew = (this.AccountId == 0);
        this.IsOwner = (this.Model.UserId == this.UserInfo.UserId);
        this.Option = (dataModel.Option) ? dataModel.Option : 'e';
        //this.AssignBy = (dataModel.AssignBy) ? dataModel.AssignBy : 0;
        //this.AccountStatus = (dataModel.AccountStatus) ? dataModel.AccountStatus : 0;

        this.IsEditable = (!this.IsInfo) && ((this.AccountId == 0) || ((this.AccountStatus < 8) && (this.Option == 'e')));
        this.EnableFormType = this.IsNew;
        this.Record = null;
        this.PostLoaded = false;
        this.Labels = null;
        this.Users = null;
        this.Prices = null;
        this.Credit = null;
        this.FormTemplate = null;
        if (this.Option == 'g') {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }
    }

    init() {

        //this.parentSettings(this.AccountParentId);

        $("#AccountId").val(this.AccountId);

        if (this.AccountId > 0) {

            this._loadData();

        }
        else {

            this.doSettings();
            this.loadControls();
            this.loadEvents();
        }

        this.tabSettings();
    }

    doSettings() {

        //var record = this.Record;

        //if (record.AccountStatus <= 0)
        //    record.AccountStatus = 1;

        //this.AccountStatus = record.AccountStatus;
        //this.IsEditable = (!this.IsInfo) && ((this.AccountId == 0) || ((this.AccountStatus < 8) && (this.Option == 'e')));
        
        //if (record.UserId > 0)
        //    app_jqx_combo_async.userInputAdapter("#UserId", record.UserId);
 
    }

    _loadData() {

        var _slf = this;
        var url = _slf.IsInfo ? "/Admin/GetAccountInfo" : "/Admin/GetAccountsEdit";
        var view_source =
            {
                datatype: "json",
                id: 'AccountId',
                data: { 'id': _slf.AccountId },
                type: 'POST',
                url: url
            };

        var viewAdapter = new $.jqx.dataAdapter(view_source, {
            loadComplete: function (record) {
                _slf.Record = record;
                _slf.doSettings();
                _slf.loadControls();
                _slf.loadEvents();
            },
            loadError: function (jqXHR, status, error) {
                app_dialog.alert("error loading Account data");
            },
            beforeLoadComplete: function (records) {
            }
        });

        if (this.AccountId > 0) {
            viewAdapter.dataBind();
        }
    }

    sectionSettings(id) {

        var slf = this;
        switch (id) {
            case 1://הגדרות כלליות
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את פרטי החשבון לפני הוספת פרטים");
                    return false;
                }

                if ($("#PropIdAccount").val())
                    return;

                //app_query.doDataAdapter("/Admin/AccountProperty_Get", { 'AccountId': this.AccountId }, function (record) {

                //});
                break;
            case 2://סליקה
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את פרטי החשבון לפני הוספת פרטים");
                    return false;
                }
                if ($("#PropIdClearing").val())
                    return;

                //app_query.doDataAdapter("/Admin/AccountClearing_Get", { 'AccountId': this.AccountId }, function (record) {

                //});

                
                break;
            case 3://Labels
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את פרטי החשבון לפני הוספת פרטים");
                    return false;
                }

                if (this.Labels == null) {
                    this.Labels = new initLabelsGrid("#jqxgrid3", 0, this.AccountId,null, "#jqxgrid3-panel");
                    //this.Labels = "#jqxgrid3"; 
                }
                break;
            case 4://Users
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את פרטי החשבון לפני הוספת פרטים");
                    return false;
                }

                if (this.Users == null) {
                    this.Users = new initUsersGrid("#jqxgrid4", 0, this.AccountId, null, "#jqxgrid4-panel", this.UserInfo);
                }
                break;
            case 5://credit
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את פרטי החשבון לפני הוספת פרטים");
                    return false;
                }

                if (this.Credit == null) {
                    this.Credit = new initCreditGrid("#jqxgrid5", 0, this.AccountId, null, "#jqxgrid5-panel");
                }
                break;
            case 6://Prices
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את פרטי החשבון לפני הוספת פרטים");
                    return false;
                }

                if (this.Prices == null) {
                    this.Prices = new initPricesGrid("#jqxgrid6", 0, this.AccountId, null, "#jqxgrid6-panel");
                }
                break;
            case 7:
                if (this.AccountId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                    return false;
                }

                if (this.uploader == null) {
                    this.uploader = new app_media_uploader("#Account-files");
                    this.uploader.init(this.AccountId, 't', !this.IsEditable);
                    this.uploader.show();
                }

                //if ($("#iframe-files").attr('src') === undefined)
                //    var op = this.Model.Option;
                //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + this.AccountId + '&refType=t&op=' + op, '100%', '350px', true);

                break;
        }
    }

    tabSettings() {
        var _slf = this;

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            //click: function (e, tab) {
            //    //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            //},
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                _slf.sectionSettings(tab.id);
                return false;
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });

        if (this.AccountId > 0) {
            $("#hxp-1").show();
            $("#hxp-2").show();
            $("#hxp-3").show();
            $("#hxp-4").show();
            $("#hxp-5").show();
            $("#hxp-6").show();
            $("#hxp-7").show();
        }
        else {
            $("#hxp-1").hide();
            $("#hxp-2").hide();
            $("#hxp-3").hide();
            $("#hxp-4").hide();
            $("#hxp-5").hide();
            $("#hxp-6").hide();
            $("#hxp-7").hide();
        }

        if (!this.IsEditable) {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }

       
    }

    doSubmit() {
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
                        //if (data.Status >= 0) {
                        //    if (_slf.IsDialog) {
                        //        window.parent.triggerMemberComplete(data.OutputId);
                        //        //$('#fcForm').reset();
                        //    }
                        //    else {
                        //        app.refresh();
                        //    }
                        //    //$('#RecordId').val(data.OutputId);
                        //}
                    },
                    error: function (jqXHR, status, error) {
                        app_dialog.alert(error);
                    }
                });
            }
        }
        $('#fcForm').jqxValidator('validate', validationResult);
    }

    doCancel() {

        app.redirectTo(app_account_def.getReferrer());
        //return this;
    }

    static getReferrer() {

        var referrer = document.referrer;
        if (referrer) {

            if (referrer.match(/Admin\/UsersDef/gi))
                return "/Admin/UsersDef";
            //else if (referrer.match(/Admin\/ReportAccounts/gi))
            //    return "/Admin/ReportAccounts";
            //else if (referrer.match(/Admin\/ReportSubAccount/gi))
            //    return "/Admin/ReportSubAccount";
            //else if (referrer.match(/Admin\/ReportTopics/gi))
            //    return "/Admin/ReportTopics";
            else {
                return "/Admin/Accounts";
            }
        }
        else {
            return "/Admin/Accounts";
        }
        //return this;
    }

    parentSettings(parentId) {
        $("#Account_Parent").val(parentId);
        if (parentId > 0) {
            $("#Account_Parent-group").show();
            $("#Account_Parent-link").click(function () {
                app_open.modelEdit(parentId, this.AccountModel);
                //app.redirectTo('/System/AccountInfo?id=' + parentId);
            });
        }
        else {
            $("#Account_Parent-group").hide();
        }
    }

    loadControls() {

        var record = this.Record;

        if (record) {

            //this.doSettings(record);
            app_form.loadDataForm("fcForm", record, ["AccountCategory"], this.IsInfo);

            $("#hTitle-text").text(this.Title + ": " + record.AccountName);
            //$("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));


            //this.parentSettings(record.Account_Parent);

            //if (this.Option !== 'g' && record.AccountStatus > 1 && record.AccountStatus < 8)
            //    $("#fcEnd").show();
            //else
            //    $("#fcEnd").hide();

            //app_Accounts.setAccountStatus("#AccountStatus", record.AccountStatus);

        }
        else {

            //app_select_loader.loadTag("Account_Type", "Account_Type", 4);
            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            //app_form.setDateTimeNow('#CreatedDate');
        }

        app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", "AccountCategory", "/Admin/GetAccountsCategories", null, 240, 140, "", record?record.AccountCategory:null);

    }

    loadEvents() {

        var _slf = this;
        //exp-0
        $("#exp-0-Submit").on('click', function () {
            _slf.doSubmit();
        });
        $("#exp-0-Cancel").on('click', function () {
           _slf.doCancel();
        });
        //jqxgrid3
        $("#jqxgrid3-add").on('click', function () {
            if (_slf.Labels)
            _slf.Labels.doAdd();
        });
        $("#jqxgrid3-edit").on('click', function () {
            if (_slf.Labels)
                _slf.Labels.doEdit();
        });
        $("#jqxgrid3-remove").on('click', function () {
            if (_slf.Labels)
                _slf.Labels.doDelete();
        });
        $("#jqxgrid3-refresh").on('click', function () {
            if (_slf.Labels)
                _slf.Labels.doRefresh();
        });
        //jqxgrid4
        $("#jqxgrid4-add").on('click', function () {
            if (_slf.Users)
                _slf.Users.doAdd();
        });
        $("#jqxgrid4-edit").on('click', function () {
            if (_slf.Users)
                _slf.Users.doEdit();
        });
        $("#jqxgrid4-remove").on('click', function () {
            if (_slf.Users)
                _slf.Users.doDelete();
        });
        $("#jqxgrid4-refresh").on('click', function () {
            if (_slf.Users)
                _slf.Users.doRefresh();
        });

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

    }

    //============================================================ app_Accounts global

    static Accounts_edit_view_comment(row) {

        var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
        content = content.replace("\n", "<br/>");
        app_jqx.toolTipClick(".Account-comment", '<p>' + content + '</p>', 350);
    };

    static setAccountButton(item, action, visible) {
        var name = 'עדכון';

        if (item == 'timer') {
            if (action == 'add')
                name = 'התחל';
            else if (action == 'update')
                name = 'סיום';


        }
        else if (action == 'add')
            $('#Account-item-update').val('');
        else if (action == 'update')
            $('#Account-item-update').val('עדכון');

        $('#Account-item-update').html(name);

        if (visible !== undefined) {
            if (visible)
                $('#Account-item-update').show();
            else
                $('#Account-item-update').hide();
        }
    };

    //============================================================ app_Accounts trigger

    triggerAccountCommentCompleted(data) {
        this.Comment.end(data);
    };
    triggerAccountAssignCompleted(data) {
        this.Assign.end(data);
    };
    triggerAccountTimerCompleted(data) {
        this.Assign.end(data);
    };
    triggerAccountFormCompleted(data) {
        this.Actions.end(data);
    };
    static triggerSubAccountCompleted(type, data) {
        var tag;
        switch (type) {
            case 'comment':
                tag = '#jqxgrid1'; break;
            case 'assign':
                tag = '#jqxgrid2'; break;
            case 'timer':
                tag = '#jqxgrid3'; break;
            case 'form':
                tag = '#jqxgrid4';
                break;
        }

        if (tag != null) {
            $(tag + '-window').hide();
            $(tag + '-bar').show();
            if (data != null && data.Status > 0)
                $(tag).jqxGrid('source').dataBind();
        }
    };
}


function initLabelsGrid(tab, index, id, NContainer, tagDialog) {

    var _slf = this;
    var gridWith = '100%';
    var gridHeight = 200;
    var alignStyle = (NContainer) ? "float:right" : "";
    var isHideGridCmd = true;
    var isEdited = false;
    var labelRecords;
    var isEnter = false;
    var isEscape = false;


    var loadLabeList = function () {
        app_query.doDataAdapter('/System/GetLabelList', { 'id': id, 'source': "*" }, function (data) {
            labelRecords = data;
        });
    }

    loadLabeList();

    var gridcontainer = $('<div style="margin:2px;text-align:right;' + alignStyle +'"></div>');
    gridcontainer.rtl = true;

    var nastedsource = {
        datafields: [
            { name: 'LabelId', type: 'number' },
            { name: 'Label', type: 'string' },
            { name: 'Val', type: 'string' },
            { name: 'AccountId', type: 'number' },
            { name: 'Modified', type: 'date' }
        ],
        updaterow: function (rowid, rowdata, commit) {
            // synchronize with the server - send update command
            // call commit with parameter true if the synchronization with the server is successful 
            // and with parameter false if the synchronization failder.

            if (isEdited == false) {
                commit(true);
                return;
            }

            //isEdited = rowHasChanged(rowdata);

            //if (isEdited == false) {
            //    commit(true);
            //    return;
            //}

            if (rowdata == null) {
                commit(false);
                refreshLabel();
                return;
            }

            isEdited = false;

            if (isEscape) {
                commit(false);
                isEscape = false;
                refreshLabel();
                return;
            }
                        
            var doPost = function () {

                app_query.doDataPost('/Admin/UpsertAccountsLabels', rowdata, function (data) {
                    if (data.Status > 0) {
                        commit(true);
                        loadLabeList();
                    }
                    else {
                        commit(false);
                        refreshLabel();
                    }
                });
            }

            if (isEnter) {
                isEnter = false;
                doPost();
            }
            else {
                app_dialog.confirm("האם לשמור את השינויים?", function () {
                    //on confirm
                    doPost();

                }, null,function () {
                    //on cancel
                    commit(false);
                    refreshLabel();
                });
            }
        },
        datatype: "json",
        id: 'LabelId',
        type: 'POST',
        url: '/Admin/GetAccountsLabel',
        data: { 'id': id }
    }
    if (NContainer) {
        NContainer.LabelsGrid[index] = gridcontainer;
        gridWith = '85%';
        gridHeight = 140;
        isHideGridCmd = true;
    }
    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

    gridcontainer.jqxGrid({
        editable: true,
        editmode: 'selectedrow',
        selectionmode: 'singlerow',//'singlecell',
        source: nastedAdapter, width: gridWith, height: gridHeight, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: '+', dataField: 'LabelId', width: 50, cellsalign: 'right', align: 'center', hidden: isHideGridCmd,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;">' +
                        '<a href="#" style="margin:2px;" title="עריכה" onclick="AccTrigger.doEditLabel(' + row + ');" ><i class="fa fa-edit" style="font-size:14px;color:dodgerblue;"></i></a> ' +
                        '<a href="#" style="margin:2px;" title="מחיקה" onclick="AccTrigger.doDeleteLabel(' + row + ');" ><i class="fa fa-remove" style="font-size:14px;color:red;"></i></a>' +
                        '</div>';

                    //return '<div style="margin:6px 20px;direction:rtl;">' +
                    //    '<label><a href="#" onclick="app_jqxgrid_menu.rowMenu(' + row + ',' + value + ');"><i class="fa fa-plus-square-o" style="font-size:14px;color:#000;"></i></a></label></div>';

                }
            },
            {
                text: 'שדה', datafield: 'Label', columntype: 'template', width: 120, cellsalign: 'right', align: 'center',
                validation: function (cell, value) {
                    if (value == null || value == "") {
                        return { result: false, message: "נדרש ערך בשדה" };
                    }
                    return true;
                },
                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                    // construct the editor. 
                    var inputElement = $("<input/>").prependTo(editor);
                    inputElement.css({ "width": "99%", "border-color": "#aaa" });
                    inputElement.keydown(function (e) {
                        switch (e.keyCode) {
                            case 13: isEnter = true; break;
                            case 27: isEscape = true; break;
                        }
                     });
                    inputElement.jqxInput({
                        height: 25,
                        source: labelRecords,
                        rtl: true,
                        placeHolder: "נא לציין תוית",
                        items: 10
                        //displayMember: "Label",
                        //valueMember: displayMember
                    });
                },
                initeditor: function (row, cellvalue, editor, celltext, pressedkey) {

                    var inputField = editor.find('input');
                    if (pressedkey) {
                        inputField.val(pressedkey);
                        inputField.jqxInput('selectLast');
                        //isEdited = true;
                    }
                    else {
                        inputField.val(cellvalue);
                        inputField.jqxInput('selectAll');
                    }
                },
                geteditorvalue: function (row, cellvalue, editor) {
                    // return the editor's value.
                    var value=editor.find('input').val();
                    if (value != cellvalue)
                        isEdited = true;
                    return value;
                }
            },
            {
                text: 'פרטים', datafield: 'Val', columntype: 'template', width: 400, cellsalign: 'right', align: 'center',
                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                    // construct the editor. 
                    var inputElement = $("<input/>").prependTo(editor);
                    inputElement.css({ "width": "99%", "border-color": "#aaa" });
                    inputElement.keydown(function (e) {
                        switch (e.keyCode) {
                            case 13: isEnter = true; break;
                            case 27: isEscape = true;break;
                        }
                    });

                    inputElement.jqxInput({ rtl: true, height: 25 });
                },
                initeditor: function (row, cellvalue, editor, celltext, pressedkey) {

                    var inputField = editor.find('input');
                    if (pressedkey) {
                        inputField.val(pressedkey);
                        inputField.jqxInput('selectLast');
                    }
                    else {
                        inputField.val(cellvalue);
                        inputField.jqxInput('selectAll');
                    }
                },
                geteditorvalue: function (row, cellvalue, editor) {
                    // return the editor's value.
                    var value = editor.find('input').val();
                    if (value != cellvalue)
                        isEdited = true;
                    return value;
                },
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    if (value) {
                        if (value.match(/^http/i))
                            return '<div style = "margin:5px;" ><a href="' + value + '" target="_blank"> ' + value + '</a></div>';
                    }
                    return '<div style = "direction:rtl;text-align:right;margin:5px;" >' + value + '</div>';
                }
            },
            { text: 'מועד עדכון', datafield: 'Modified', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', editable: false }
        ]
    }).on('rowselect', function (event) {
        // event arguments.
        var args = event.args;
        // row's bound index.
        var rowBoundIndex = args.rowindex;
        isEscape = false;
        isEnter = false;
        });

    /*
    .on('rowdoubleclick', function (event) {
        //var args = event.args;
        //var boundIndex = args.rowindex;
        //var visibleIndex = args.visibleindex;
        //var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex);
        //app_popup.memberEdit(id);
        //_slf.doEditLabel();
        });
    */
    
    //var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;display:none"></div>');
    //var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;direction:rtl"></div>');
    var ctlContainer = $('<div style="margin:2px;direction:rtl;text-align:right"></div>');

    //var dialogContainer = $('<div id="label-dialog"></div>');
    //$(ctlContainer).append(dialogContainer);


    var labelsCount = function () {

        var rows = gridcontainer.jqxGrid('getrows');
        return rows == null ? 0 : rows.length;
    }

    if (NContainer) {
        var add_item = $('<a class="btn-small w60" title="הוספת פרטים" href="#" >הוספה</a>')
            .click(function () {
                addLabel();
            });
        var update_item = $('<a class="btn-small w60" title="עדכון פרטים" href="#" >עריכה</a>')
            .click(function () {
                editLabel();
            });
        var delete_item = $('<a class="btn-small w60" title="מחיקת פרטים" href="#" >מחיקה</a>')
            .click(function () {

                deleteLabel();
            });
        var refresh_item = $('<a class="btn-small w60" title="רענון פרטים"  href="#" >רענון</a>')
            .click(function () {
                nastedAdapter.dataBind();
            });
        
        $(ctlContainer).append(add_item);
        $(ctlContainer).append(update_item);
        $(ctlContainer).append(delete_item);
        $(ctlContainer).append(refresh_item);
        $(ctlContainer).append('<br/>');
    }
    else {

        this.doAdd = function () {
            addItem();
        }

        this.doEdit = function (row) {
            editItem(row);
        };

        this.doDelete = function (row) {
            deleteItem(row);
        };

        this.doRefresh = function () {
            nastedAdapter.dataBind();
        };
    }

    $(tab).append(ctlContainer);

    $(tab).append(gridcontainer);

    var getRowData = function (row) {

        var rowindex = row;

        if (!row || row < 0) {
            var rowindex = gridcontainer.jqxGrid('getselectedrowindex');
            if (rowindex < 0)
                return;
        }
        var data = gridcontainer.jqxGrid('getrowdata', rowindex);
        return data;
    }

    var addItem = function () {

        var actModel = { Option: "a", AccountId: id };
        var item = new app_account_label_control(tagDialog);
        item.init(actModel, null, nastedAdapter, false);
        item.display();
    }
    
    var editItem = function (row) {
        var data = getRowData(row);
        if (!data) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        var actModel = { Option: "e", AccountId: id };
        var item = new app_account_label_control(tagDialog);//"#label-dialog");
        item.init(actModel, data, nastedAdapter, false);
        item.display();
    };

    var deleteItem = function (row) {

        var record = getRowData(row);
        if (!record) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        app_dialog.confirm("האם למחוק ? " + record.Label, function () {
            app_query.doDataPost("/Admin/DeleteAccountsLabels", { 'AccountId': record.AccountId, 'LabelId': record.LabelId }, function (data) {
                if (data.Status > 0) {
                    refreshLabel(true);
                }
                else
                    app_messenger.Post(data, 'error');
            });
        });
    };

    var refreshItem = function (loadLabels) {
        nastedAdapter.dataBind();
        if (loadLabels)
            loadLabeList();
    };

    return this;
};

function initUsersGrid (tab, index, id, NContainer,tagDialog,uinfo) {

    var _slf = this;
    var gridWith = '100%';
    var gridHeight = 200;
    var alignStyle = (NContainer) ? "float:right" : "";
    var isHideGridCmd = true;
    var isEdited = false;
    var labelRecords;
    var isEnter = false;
    var isEscape = false;

    var gridcontainer = $('<div style="margin:2px;text-align:right;' + alignStyle + '"></div>');
    gridcontainer.rtl = true;
    var nastedsource = {
        datafields: [
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
        datatype: "json",
        id: 'UserId',
        type: 'POST',
        url: '/Admin/GetAdminUsersProfile',
        data: { 'accountId': id }
    }

    if (NContainer) {
        NContainer.UsersGrid[index] = gridcontainer;
        gridWith = '85%';
        gridHeight = 140;
        isHideGridCmd = true;

    }

    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
    gridcontainer.jqxGrid({
        source: nastedAdapter, width: gridWith, height: gridHeight, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: 'קוד משתמש', dataField: 'UserId', width: 100, cellsalign: 'right', align: 'center', hidden: isHideGridCmd,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="members_grid.usersEdit(' + value + ',' + index + ',' + row + ');" >הצג</a></div>'
                }
            },
            //{ text: 'חשבון', datafield: 'AccountId', cellsalign: 'right', align: 'center' },
            //{ text: 'קוד תפקיד', datafield: 'UserRole',width: 60, cellsalign: 'right', align: 'center' },
            { text: 'תפקיד', datafield: 'RoleName', width: 90, cellsalign: 'right', align: 'center' },
            { text: 'שם משתמש', datafield: 'UserName', width: 120, cellsalign: 'right', align: 'center' },
            { text: 'פרטים', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
            { text: 'אימייל', datafield: 'Email', cellsalign: 'right', align: 'center' },
            { text: 'טלפון', datafield: 'Phone', width: 120, cellsalign: 'right', align: 'center' },
            //{ text: 'נסיון', datafield: 'Evaluation', width: 60,cellsalign: 'right', align: 'center' },
            { text: 'חסום', datafield: 'IsBlocked', columntype: 'checkbox', width: 60, cellsalign: 'right', align: 'center' },
            { text: 'נוצר ב', datafield: 'Creation', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
        ]
    });

    //var ctlContainer = $('<div style="margin:10px auto;padding-left:10px;padding-right:10px;float:right;"></div>')
    var ctlContainer = $('<div  style="margin:2px;direction:rtl;text-align:right"></div>');
    $(ctlContainer).append('<div id="user-dialog" class="centered"></div>');

    if (NContainer) {
        var add_item = $('<a class="btn-small w60" title="הוספת משתמש" href="#" >הוסף</a>')
            .click(function () {
                addUser();
            });
        var edit_item = $('<a class="btn-small w60" title="עריכת משתמש" href="#" >עריכה</a>')
            .click(function () {
                editUser();
            });
        var delete_item = $('<a class="btn-small w60" title="מחיקת משתמש" href="#" >מחיקה</a>')
            .click(function () {
                editUser();
            });
        var refresh_item = $('<a class="btn-small w60" title="רענון משתמשים"  href="#" >רענן</a>')
            .click(function () {
                nastedAdapter.dataBind();
            });

        $(ctlContainer).append(add_item);
        $(ctlContainer).append(edit_item);
        $(ctlContainer).append(refresh_item);
        $(ctlContainer).append('<br/>');

    }
    else {

        this.doAdd = function () {
            addItem();
        }

        this.doEdit = function (row) {
            editItem(row);
        };

        this.doDelete = function (row) {
            deleteItem(row);
        };

        this.doRefresh = function () {
            nastedAdapter.dataBind();
        };

    }

    $(tab).append(ctlContainer);
    $(tab).append(gridcontainer);

    var getRowData = function (row) {

        var rowindex = row;

        if (!row || row < 0) {
            var rowindex = gridcontainer.jqxGrid('getselectedrowindex');
            if (rowindex < 0)
                return;
        }
        var data = gridcontainer.jqxGrid('getrowdata', rowindex);
        return data;
    }

    var addItem = function () {
        var actModel = { Option: "a", AccountId: id, UserId: uinfo.UserId, UserRole: uinfo.UserRole };
        var item = new app_user_def_control(tagDialog);
        item.init(actModel, null, nastedAdapter, false);
        item.display();
    }

    var editItem = function (row) {
         var data = getRowData(row);
        if (!data) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        var actModel = { Option: "e", AccountId: id, UserId: uinfo.UserId, UserRole: uinfo.UserRole };
        var item = new app_user_def_control(tagDialog);
        item.init(actModel, data, nastedAdapter, false);
        item.display();
    };

    var deleteItem = function (row) {

        var record = getRowData(row);
        if (!record) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }

        if (uinfo.UserRole <= row.UserRole) {
            app_dialog.alert("Action not allowed!");
            return;
        }

        app_dialog.confirm("האם למחוק ? " + record.UserName, function () {
            app_query.doDataPost("/Admin/AdUserDefDelete", { 'AccountId': record.AccountId, 'UserId': record.UserId }, function (data) {
                if (data.Status > 0) {
                    refreshLabel(true);
                }
                else
                    app_messenger.Post(data, 'error');
            });
        });
    };

    var refreshItem = function () {
        nastedAdapter.dataBind();
    };

    return this;
};

function initCreditGrid(tab, index, id, NContainer, tagDialog) {

    var _slf = this;
    var gridWith = '100%';
    var gridHeight = 200;
    var alignStyle = (NContainer) ? "float:right" : "";
    var isHideGridCmd = true;
    var isEdited = false;
    var labelRecords;
    var isEnter = false;
    var isEscape = false;

    var gridcontainer = $('<div style="margin:2px;text-align:right;' + alignStyle + '"></div>');
    gridcontainer.rtl = true;
    /*
    var nastedsource = {
        datafields: [
            { name: 'LabelId', type: 'number' },
            { name: 'Label', type: 'string' },
            { name: 'Val', type: 'string' },
            { name: 'AccountId', type: 'number' },
            { name: 'Modified', type: 'date' }
        ],
        updaterow: function (rowid, rowdata, commit) {
            // synchronize with the server - send update command
            // call commit with parameter true if the synchronization with the server is successful 
            // and with parameter false if the synchronization failder.

            if (isEdited == false) {
                commit(true);
                return;
            }

            if (rowdata == null) {
                commit(false);
                refreshLabel();
                return;
            }

            isEdited = false;

            if (isEscape) {
                commit(false);
                isEscape = false;
                refreshLabel();
                return;
            }

            var doPost = function () {

                app_query.doDataPost('/Admin/UpsertAccountsLabels', rowdata, function (data) {
                    if (data.Status > 0) {
                        commit(true);
                        loadLabeList();
                    }
                    else {
                        commit(false);
                        refreshLabel();
                    }
                });
            }

            if (isEnter) {
                isEnter = false;
                doPost();
            }
            else {
                app_dialog.confirm("האם לשמור את השינויים?", function () {
                    //on confirm
                    doPost();

                }, null, function () {
                    //on cancel
                    commit(false);
                    refreshLabel();
                });
            }
        },
        datatype: "json",
        id: 'LabelId',
        type: 'POST',
        url: '/Admin/GetAccountsLabel',
        data: { 'id': id }
    }
    */
    if (NContainer) {
        NContainer.CreditGrid[index] = gridcontainer;
        gridWith = '85%';
        gridHeight = 140;
        isHideGridCmd = true;
    }
     /*
    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
   
    gridcontainer.jqxGrid({
        editable: true,
        editmode: 'selectedrow',
        selectionmode: 'singlerow',//'singlecell',
        source: nastedAdapter, width: gridWith, height: gridHeight, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: '+', dataField: 'LabelId', width: 50, cellsalign: 'right', align: 'center', hidden: isHideGridCmd,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;">' +
                        '<a href="#" style="margin:2px;" title="עריכה" onclick="AccTrigger.doEditLabel(' + row + ');" ><i class="fa fa-edit" style="font-size:14px;color:dodgerblue;"></i></a> ' +
                        '<a href="#" style="margin:2px;" title="מחיקה" onclick="AccTrigger.doDeleteLabel(' + row + ');" ><i class="fa fa-remove" style="font-size:14px;color:red;"></i></a>' +
                        '</div>';

                    //return '<div style="margin:6px 20px;direction:rtl;">' +
                    //    '<label><a href="#" onclick="app_jqxgrid_menu.rowMenu(' + row + ',' + value + ');"><i class="fa fa-plus-square-o" style="font-size:14px;color:#000;"></i></a></label></div>';

                }
            },
            {
                text: 'שדה', datafield: 'Label', columntype: 'template', width: 120, cellsalign: 'right', align: 'center',
                validation: function (cell, value) {
                    if (value == null || value == "") {
                        return { result: false, message: "נדרש ערך בשדה" };
                    }
                    return true;
                },
                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                    // construct the editor. 
                    var inputElement = $("<input/>").prependTo(editor);
                    inputElement.css({ "width": "99%", "border-color": "#aaa" });
                    inputElement.keydown(function (e) {
                        switch (e.keyCode) {
                            case 13: isEnter = true; break;
                            case 27: isEscape = true; break;
                        }
                    });
                    inputElement.jqxInput({
                        height: 25,
                        source: labelRecords,
                        rtl: true,
                        placeHolder: "נא לציין תוית",
                        items: 10
                        //displayMember: "Label",
                        //valueMember: displayMember
                    });
                },
                initeditor: function (row, cellvalue, editor, celltext, pressedkey) {

                    var inputField = editor.find('input');
                    if (pressedkey) {
                        inputField.val(pressedkey);
                        inputField.jqxInput('selectLast');
                        //isEdited = true;
                    }
                    else {
                        inputField.val(cellvalue);
                        inputField.jqxInput('selectAll');
                    }
                },
                geteditorvalue: function (row, cellvalue, editor) {
                    // return the editor's value.
                    var value = editor.find('input').val();
                    if (value != cellvalue)
                        isEdited = true;
                    return value;
                }
            },
            {
                text: 'פרטים', datafield: 'Val', columntype: 'template', width: 400, cellsalign: 'right', align: 'center',
                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                    // construct the editor. 
                    var inputElement = $("<input/>").prependTo(editor);
                    inputElement.css({ "width": "99%", "border-color": "#aaa" });
                    inputElement.keydown(function (e) {
                        switch (e.keyCode) {
                            case 13: isEnter = true; break;
                            case 27: isEscape = true; break;
                        }
                    });

                    inputElement.jqxInput({ rtl: true, height: 25 });
                },
                initeditor: function (row, cellvalue, editor, celltext, pressedkey) {

                    var inputField = editor.find('input');
                    if (pressedkey) {
                        inputField.val(pressedkey);
                        inputField.jqxInput('selectLast');
                    }
                    else {
                        inputField.val(cellvalue);
                        inputField.jqxInput('selectAll');
                    }
                },
                geteditorvalue: function (row, cellvalue, editor) {
                    // return the editor's value.
                    var value = editor.find('input').val();
                    if (value != cellvalue)
                        isEdited = true;
                    return value;
                },
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    if (value) {
                        if (value.match(/^http/i))
                            return '<div style = "margin:5px;" ><a href="' + value + '" target="_blank"> ' + value + '</a></div>';
                    }
                    return '<div style = "direction:rtl;text-align:right;margin:5px;" >' + value + '</div>';
                }
            },
            { text: 'מועד עדכון', datafield: 'Modified', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', editable: false }
        ]
    }).on('rowselect', function (event) {
        // event arguments.
        var args = event.args;
        // row's bound index.
        var rowBoundIndex = args.rowindex;
        isEscape = false;
        isEnter = false;
    });
    */
    /*
    .on('rowdoubleclick', function (event) {
        //var args = event.args;
        //var boundIndex = args.rowindex;
        //var visibleIndex = args.visibleindex;
        //var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex);
        //app_popup.memberEdit(id);
        //_slf.doEditLabel();
        });
    */
    var ctlContainer = $('<div style="margin:2px;direction:rtl;text-align:right"></div>');

    if (NContainer) {
        var add_item = $('<a class="btn-small w60" title="הוספת פרטים" href="#" >הוספה</a>')
            .click(function () {
                addLabel();
            });
        var update_item = $('<a class="btn-small w60" title="עדכון פרטים" href="#" >עריכה</a>')
            .click(function () {
                editLabel();
            });
        var delete_item = $('<a class="btn-small w60" title="מחיקת פרטים" href="#" >מחיקה</a>')
            .click(function () {

                deleteLabel();
            });
        var refresh_item = $('<a class="btn-small w60" title="רענון פרטים"  href="#" >רענון</a>')
            .click(function () {
                nastedAdapter.dataBind();
            });

        $(ctlContainer).append(add_item);
        $(ctlContainer).append(update_item);
        $(ctlContainer).append(delete_item);
        $(ctlContainer).append(refresh_item);
        $(ctlContainer).append('<br/>');
    }
    else {

        this.doAdd = function () {
            addItem();
        }

        this.doEdit = function (row) {
            editItem(row);
        };

        this.doDelete = function (row) {
            deleteItem(row);
        };

        this.doRefresh = function () {
            nastedAdapter.dataBind();
        };
    }


    $(tab).append(ctlContainer);

    $(tab).append(gridcontainer);

    var getRowData = function (row) {

        var rowindex = row;

        if (!row || row < 0) {
            var rowindex = gridcontainer.jqxGrid('getselectedrowindex');
            if (rowindex < 0)
                return;
        }
        var data = gridcontainer.jqxGrid('getrowdata', rowindex);
        return data;
    }

    var addItem = function () {

        var actModel = { Option: "a", AccountId: id };
        var item = new app_account_credit_control(tagDialog);
        item.init(actModel, null, nastedAdapter, false);
        item.display();
    }

    var editItem = function (row) {
        var data = getRowData(row);
        if (!data) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        var actModel = { Option: "e", AccountId: id };
        var item = new app_account_credit_control(tagDialog);//"#label-dialog");
        item.init(actModel, data, nastedAdapter, false);
        item.display();
    };

    var deleteItem = function (row) {

        var record = getRowData(row);
        if (!record) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        app_dialog.confirm("האם למחוק ? " + record.Label, function () {
            app_query.doDataPost("/Admin/DeleteAccountsLabels", { 'AccountId': record.AccountId, 'LabelId': record.LabelId }, function (data) {
                if (data.Status > 0) {
                    refreshLabel(true);
                }
                else
                    app_messenger.Post(data, 'error');
            });
        });
    };

    var refreshItem = function (loadLabels) {
        nastedAdapter.dataBind();
        if (loadLabels)
            loadLabeList();
    };

    return this;
};

function initPricesGrid(tab, index, id, NContainer, tagDialog) {

    var _slf = this;
    var gridWith = '100%';
    var gridHeight = 200;
    var alignStyle = (NContainer) ? "float:right" : "";
    var isHideGridCmd = true;
    var isEdited = false;
    var labelRecords;
    var isEnter = false;
    var isEscape = false;
    
    var gridcontainer = $('<div style="margin:2px;text-align:right;' + alignStyle + '"></div>');
    gridcontainer.rtl = true;
    /*
    var nastedsource = {
        datafields: [
            { name: 'LabelId', type: 'number' },
            { name: 'Label', type: 'string' },
            { name: 'Val', type: 'string' },
            { name: 'AccountId', type: 'number' },
            { name: 'Modified', type: 'date' }
        ],
        updaterow: function (rowid, rowdata, commit) {
            // synchronize with the server - send update command
            // call commit with parameter true if the synchronization with the server is successful 
            // and with parameter false if the synchronization failder.

            if (isEdited == false) {
                commit(true);
                return;
            }

            if (rowdata == null) {
                commit(false);
                refreshLabel();
                return;
            }

            isEdited = false;

            if (isEscape) {
                commit(false);
                isEscape = false;
                refreshLabel();
                return;
            }

            var doPost = function () {

                app_query.doDataPost('/Admin/UpsertAccountsLabels', rowdata, function (data) {
                    if (data.Status > 0) {
                        commit(true);
                        loadLabeList();
                    }
                    else {
                        commit(false);
                        refreshLabel();
                    }
                });
            }

            if (isEnter) {
                isEnter = false;
                doPost();
            }
            else {
                app_dialog.confirm("האם לשמור את השינויים?", function () {
                    //on confirm
                    doPost();

                }, null, function () {
                    //on cancel
                    commit(false);
                    refreshLabel();
                });
            }
        },
        datatype: "json",
        id: 'LabelId',
        type: 'POST',
        url: '/Admin/GetAccountsLabel',
        data: { 'id': id }
    }
    */
    if (NContainer) {
        NContainer.PricesGrid[index] = gridcontainer;
        gridWith = '85%';
        gridHeight = 140;
        isHideGridCmd = true;
    }
    /*
    var nastedAdapter = new $.jqx.dataAdapter(nastedsource);
    
    gridcontainer.jqxGrid({
        editable: true,
        editmode: 'selectedrow',
        selectionmode: 'singlerow',//'singlecell',
        source: nastedAdapter, width: gridWith, height: gridHeight, //showheader: true,
        localization: getLocalization('he'),
        rtl: true,
        columns: [
            {
                text: '+', dataField: 'LabelId', width: 50, cellsalign: 'right', align: 'center', hidden: isHideGridCmd,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    return '<div style="text-align:center;direction:rtl;margin:5px;">' +
                        '<a href="#" style="margin:2px;" title="עריכה" onclick="AccTrigger.doEditLabel(' + row + ');" ><i class="fa fa-edit" style="font-size:14px;color:dodgerblue;"></i></a> ' +
                        '<a href="#" style="margin:2px;" title="מחיקה" onclick="AccTrigger.doDeleteLabel(' + row + ');" ><i class="fa fa-remove" style="font-size:14px;color:red;"></i></a>' +
                        '</div>';

                    //return '<div style="margin:6px 20px;direction:rtl;">' +
                    //    '<label><a href="#" onclick="app_jqxgrid_menu.rowMenu(' + row + ',' + value + ');"><i class="fa fa-plus-square-o" style="font-size:14px;color:#000;"></i></a></label></div>';

                }
            },
            {
                text: 'שדה', datafield: 'Label', columntype: 'template', width: 120, cellsalign: 'right', align: 'center',
                validation: function (cell, value) {
                    if (value == null || value == "") {
                        return { result: false, message: "נדרש ערך בשדה" };
                    }
                    return true;
                },
                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                    // construct the editor. 
                    var inputElement = $("<input/>").prependTo(editor);
                    inputElement.css({ "width": "99%", "border-color": "#aaa" });
                    inputElement.keydown(function (e) {
                        switch (e.keyCode) {
                            case 13: isEnter = true; break;
                            case 27: isEscape = true; break;
                        }
                    });
                    inputElement.jqxInput({
                        height: 25,
                        source: labelRecords,
                        rtl: true,
                        placeHolder: "נא לציין תוית",
                        items: 10
                        //displayMember: "Label",
                        //valueMember: displayMember
                    });
                },
                initeditor: function (row, cellvalue, editor, celltext, pressedkey) {

                    var inputField = editor.find('input');
                    if (pressedkey) {
                        inputField.val(pressedkey);
                        inputField.jqxInput('selectLast');
                        //isEdited = true;
                    }
                    else {
                        inputField.val(cellvalue);
                        inputField.jqxInput('selectAll');
                    }
                },
                geteditorvalue: function (row, cellvalue, editor) {
                    // return the editor's value.
                    var value = editor.find('input').val();
                    if (value != cellvalue)
                        isEdited = true;
                    return value;
                }
            },
            {
                text: 'פרטים', datafield: 'Val', columntype: 'template', width: 400, cellsalign: 'right', align: 'center',
                createeditor: function (row, cellvalue, editor, cellText, width, height) {
                    // construct the editor. 
                    var inputElement = $("<input/>").prependTo(editor);
                    inputElement.css({ "width": "99%", "border-color": "#aaa" });
                    inputElement.keydown(function (e) {
                        switch (e.keyCode) {
                            case 13: isEnter = true; break;
                            case 27: isEscape = true; break;
                        }
                    });

                    inputElement.jqxInput({ rtl: true, height: 25 });
                },
                initeditor: function (row, cellvalue, editor, celltext, pressedkey) {

                    var inputField = editor.find('input');
                    if (pressedkey) {
                        inputField.val(pressedkey);
                        inputField.jqxInput('selectLast');
                    }
                    else {
                        inputField.val(cellvalue);
                        inputField.jqxInput('selectAll');
                    }
                },
                geteditorvalue: function (row, cellvalue, editor) {
                    // return the editor's value.
                    var value = editor.find('input').val();
                    if (value != cellvalue)
                        isEdited = true;
                    return value;
                },
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    if (value) {
                        if (value.match(/^http/i))
                            return '<div style = "margin:5px;" ><a href="' + value + '" target="_blank"> ' + value + '</a></div>';
                    }
                    return '<div style = "direction:rtl;text-align:right;margin:5px;" >' + value + '</div>';
                }
            },
            { text: 'מועד עדכון', datafield: 'Modified', width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center', editable: false }
        ]
    }).on('rowselect', function (event) {
        // event arguments.
        var args = event.args;
        // row's bound index.
        var rowBoundIndex = args.rowindex;
        isEscape = false;
        isEnter = false;
    });
    */
    /*
    .on('rowdoubleclick', function (event) {
        //var args = event.args;
        //var boundIndex = args.rowindex;
        //var visibleIndex = args.visibleindex;
        //var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex);
        //app_popup.memberEdit(id);
        //_slf.doEditLabel();
        });
    */
    var ctlContainer = $('<div style="margin:2px;direction:rtl;text-align:right"></div>');

    if (NContainer) {
        var add_item = $('<a class="btn-small w60" title="הוספת פרטים" href="#" >הוספה</a>')
            .click(function () {
                addLabel();
            });
        var update_item = $('<a class="btn-small w60" title="עדכון פרטים" href="#" >עריכה</a>')
            .click(function () {
                editLabel();
            });
        var delete_item = $('<a class="btn-small w60" title="מחיקת פרטים" href="#" >מחיקה</a>')
            .click(function () {

                deleteLabel();
            });
        var refresh_item = $('<a class="btn-small w60" title="רענון פרטים"  href="#" >רענון</a>')
            .click(function () {
                nastedAdapter.dataBind();
            });

        $(ctlContainer).append(add_item);
        $(ctlContainer).append(update_item);
        $(ctlContainer).append(delete_item);
        $(ctlContainer).append(refresh_item);
        $(ctlContainer).append('<br/>');
    }
    else {

        this.doAdd = function () {
            addItem();
        }

        this.doEdit = function (row) {
            editItem(row);
        };

        this.doDelete = function (row) {
            deleteItem(row);
        };

        this.doRefresh = function () {
            nastedAdapter.dataBind();
        };
    }


    $(tab).append(ctlContainer);

    $(tab).append(gridcontainer);

    var getRowData = function (row) {

        var rowindex = row;

        if (!row || row < 0) {
            var rowindex = gridcontainer.jqxGrid('getselectedrowindex');
            if (rowindex < 0)
                return;
        }
        var data = gridcontainer.jqxGrid('getrowdata', rowindex);
        return data;
    }

    var addItem = function () {

        var actModel = { Option: "a", AccountId: id };
        var item = new app_account_label_control(tagDialog);
        item.init(actModel, null, nastedAdapter, false);
        item.display();
    }

    var editItem = function (row) {
        var data = getRowData(row);
        if (!data) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        var actModel = { Option: "e", AccountId: id };
        var item = new app_account_label_control(tagDialog);//"#label-dialog");
        item.init(actModel, data, nastedAdapter, false);
        item.display();
    };

    var deleteItem = function (row) {

        var record = getRowData(row);
        if (!record) {
            app_dialog.alert("לא נמצאו נתונים");
            return;
        }
        app_dialog.confirm("האם למחוק ? " + record.Label, function () {
            app_query.doDataPost("/Admin/DeleteAccountsLabels", { 'AccountId': record.AccountId, 'LabelId': record.LabelId }, function (data) {
                if (data.Status > 0) {
                    refreshLabel(true);
                }
                else
                    app_messenger.Post(data, 'error');
            });
        });
    };

    var refreshItem = function () {
        nastedAdapter.dataBind();
    };

    return this;
};


