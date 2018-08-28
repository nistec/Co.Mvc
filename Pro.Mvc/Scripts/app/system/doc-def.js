'use strict';

class app_doc_base {

    constructor(dataModel, userInfo, docModel) {

        this.$element = $("#accordion");
        //this.$element = document.querySelectorAll(element);

        //if (!this.$element.is('div')) {
        //    $.error('app_wiztabs should be applied to DIV element');
        //    return;
        //}
        //// ensure to use the `new` operator
        //if (!(this instanceof app_doc))
        //    return new app_doc(element, DocId, userInfo, docModel);

        this.DocId = dataModel.Id;
        this.DocParentId = dataModel.PId;
        this.Model = dataModel;
        this.DocModel = docModel;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;
        this.UserRole = userInfo.UserRole;
        this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        this.IsInfo = dataModel.IsInfo;
        this.Title = app_tasks.taskTitle(this.DocModel);
        this.uploader;
        this.IsNew = (this.DocId === 0);
        this.IsOwner = (this.Model.UserId === this.UserInfo.UserId);
        this.Option = (dataModel.Option) ? dataModel.Option : 'e';
        this.AssignBy = (dataModel.AssignBy) ? dataModel.AssignBy : 0;
        this.DocStatus = (dataModel.DocStatus) ? dataModel.DocStatus : 0;
        this.IsEditable = (!this.IsInfo) && ((this.DocId === 0) || ((this.DocStatus < 8) && (this.AssignBy === this.UserInfo.UserId || this.Option === 'e')));
        this.EnableFormType = this.IsNew;
        this.PostLoaded = false;
        //this.ClientId = 0;
        //this.ProjectId = 0;
        //this.Tags = null;
        //this.AssignTo = null;
        this.Comment = null;
        this.Assign = null;
        this.Timer = null;
        this.Actions = null;
        this.Record = null;
        this.FormTemplate = null;
        if (this.Option === 'g') {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }
    }

    doSettings() {
        var record = this.Record;

        if (record.DocStatus <= 0)
            record.DocStatus = 1;

        this.AssignBy = record.AssignBy;
        this.DocStatus = record.DocStatus;
        this.IsEditable = (!this.IsInfo) && ((this.DocId === 0) || ((this.DocStatus < 8) && (this.AssignBy === this.UserInfo.UserId || this.Option === 'e')));

        //this.ProjectId = record.Project_Id;
        //this.ClientId = record.ClientId;
        //this.Tags = record.Tags;
        //this.AssignTo = record.AssignTo;
        //this.SrcUserId = record.UserId;


        if (record.UserId > 0)
            app_jqx_combo_async.userInputAdapter("#UserId", record.UserId);
        if (this.IsInfo) {
            //var model = this.Model.Data
            if (record.Comments === 0)
                $("#exp-1").hide();
            if (record.Items === 0)
                $("#exp-4").hide();
            if (record.Files === 0)
                $("#exp-5").hide();
        }
    }

    _loadData() {

        var _slf = this;
        var url = _slf.IsInfo ? "/System/GetDocInfo" : "/System/GetDocEdit";
        var view_source =
            {
                datatype: "json",
                id: 'DocId',
                data: { 'id': _slf.DocId },
                type: 'POST',
                url: url
            };

        var viewAdapter = new $.jqx.dataAdapter(view_source, {
            loadComplete: function (record) {
                _slf.Record = record;
                _slf.doSettings();
                _slf.loadControls();
                //slf.loadEvents();
            },
            loadError: function (jqXHR, status, error) {
                app_dialog.alert("error loading doc data");
            },
            beforeLoadComplete: function (records) {
            }
        });

        if (this.DocId > 0) {
            viewAdapter.dataBind();
        }
    }

    sectionSettings(id) {

        var slf = this;
        switch (id) {
            case 1:
                if (this.DocId === 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המסמך הנוכחי לפני הוספת הערות");
                    return false;
                }
                if (this.IsInfo) {
                    if ($("#jqxgrid1").is(':empty'))
                        app_repeater.doc_comments_adapter("#jqxgrid1", this.DocId);
                }
                else {
                    if (this.Comment == null) {
                        this.Comment = new app_doc_comment_grid(this.DocId, this.UserInfo, this.Option);
                    }
                }
                break;
            case 4:
                if (this.DocId === 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המסמך הנוכחי לפני הוספת פעולות");
                    return false;
                }

                if (this.IsInfo) {
                    if ($("#jqxgrid4").is(':empty'))
                        app_repeater.doc_form_adapter("#jqxgrid4", this.DocId);
                }
                else {
                    if (this.Actions === null)//($("#jqxgrid4")[0].childElementCount == 0)
                        this.Actions = new app_doc_form_grid(this.DocId, this.UserInfo, this.Option);
                
                    if (this.EnableFormType) {
                        var source = $('#Form_Type').jqxComboBox('source');
                        if (source === null)
                            app_jqxcombos.createComboAdapter("FormTypeId", "FormName", "Form_Type", '/System/GetDocFormTypeList', 0, 120, false);

                        //if (this.FormTemplate == null) {//($("#jqxgrid4")[0].childElementCount == 0)
                        //    this.FormTemplate = new app_doc_form_template(); //app_doc_base.form_template.load(this.DocId, this.UserInfo);
                        //    this.FormTemplate.load(this.DocId, this.UserInfo);
                        //}
                    }
                }
                break;
            case 5:
                if (this.DocId === 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המסמך הנוכחי לפני הוספת קבצים");
                    return false;
                }

                if (this.uploader === undefined ||   this.uploader == null) {
                    this.uploader = new app_media_uploader("#doc-files");
                    this.uploader.init(this.DocId, 'd', !this.IsEditable);
                    this.uploader.show();
                }

                //if ($("#iframe-files").attr('src') === undefined)
                //    var op = this.Model.Option;
                //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + this.DocId + '&refType=t&op=' + op, '100%', '350px', true);

                break;
        }
    }

    postLoad(waitAll) {

        //waitAll = true;

        if (this.PostLoaded)
            return;

        var _slf = this;
        var clientId, projectId, tags, assignTo;
        if (this.Record) {
            clientId = this.Record.ClientId;
            projectId = this.Record.ProjectId;
            tags = this.Record.Tags;
            assignTo = this.Record.AssignTo;
        }
        var promise1 = [0, 0, 0, 0];

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', clientId, function () {
            promise1[0] = 1;
        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', projectId, function () {
            promise1[1] = 1;
        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 225, 0, true, tags, function () {
            promise1[2] = 1;
        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, assignTo, function () {
            $("#AssignTo").jqxComboBox({ disabled: true });
            promise1[3] = 1;
        });

        function afterPostLoaded() {

            _slf.PostLoaded = true;

            if (!_slf.IsEditable) {
                //$("#ClientId").prop("readonly", true);
                //$("#Project_Id").prop("readonly", true);
                $("#Tags").jqxComboBox({ disabled: true });
                $("#AssignTo").jqxComboBox({ disabled: true });
                //$("#ShareType").jqxDropDownList({ enableSelection: false });
                app.disableSelect("#ShareType");
            }

        }

        if (waitAll) {

            function waitUntil() {

                async function promiseCompleted() {
                    var promiseMax = 4;
                    var sum = promise1.reduce(function (acc, val) { return acc + val; }, 0);
                    if (sum == promiseMax) {
                        afterPostLoaded();
                    }
                    return sum == promiseMax;
                }

                async function delay() {
                    return new Promise(resolve => setTimeout(resolve, 300));
                }

                async function processArray(array) {
                    var maxloop = 10;
                    var counter = 0;
                    while (! await promiseCompleted()) {
                        await delay();
                        counter++;
                        if (counter >= maxloop) {
                            break;
                        }
                    }
                    console.log('Done');
                }

                processArray(promise1);
            }

            waitUntil();
        }
        else {

            afterPostLoaded();
        }
    }
/*
    postLoad() {

        app_jqx_combo_async.lookupInputAdapter('#ClientId', 'lu_Members', this.ClientId, function () {

        });
        app_jqx_combo_async.systemLookupInputAdapter('#Project_Id', 'lu_Project', this.ProjectId, function () {

        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#Tags", '/System/GetTagsList', null, 225, 0, true, this.Tags, function () {

        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#AssignTo", '/System/GetUsersList', null, 225, 0, null, this.AssignTo, function () {
            $("#AssignTo").jqxComboBox({ disabled: true });
        });


        //$("#AssignTo").keydown(function (event) {
        //   if (event.which == 8) {
        //       event.preventDefault();
        //   }
        //});

        //app_lookups.member_name($('#ClientId').val(), '#ClientId-display');
        //app_lookups.project_name($('#Project_Id').val(), '#Project_Id-display');

        this.exp1_Inited = true;

        if (!this.IsEditable) {
            $("#ClientId").prop("readonly", true);
            $("#Project_Id").prop("readonly", true);
            $("#Tags").jqxComboBox({ enableSelection: false });
            $("#AssignTo").jqxComboBox({ enableSelection: false });
            //$("#ShareType").jqxDropDownList({ enableSelection: false });
            app.disableSelect("#ShareType");
        }
    }
*/
    doCancel() {

        app.redirectTo(app_doc_base.getReferrer());
        //return this;
    }

    static getReferrer() {

        var referrer = document.referrer;
        if (referrer) {

            //if (referrer.match(/System\/DocUser/gi))
            //    return "/System/DocUser";
            //else if (referrer.match(/System\/ReportTasks/gi))
            //    return "/System/ReportDocs";
            //else if (referrer.match(/System\/ReportSubTask/gi))
            //    return "/System/ReportSubDoc";
            //else if (referrer.match(/System\/ReportTopics/gi))
            //    return "/System/ReportTopics";
            //else {
                return "/System/ReportDocs";
            //}
        }
        else {
            return "/System/ReportDocs";
        }
        //return this;
    }

    loadControls(record) {

    }

    //============================================================ app_docs global

    static docs_edit_view_comment(row) {

        var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
        content = content.replace("\n", "<br/>");
        app_jqx.toolTipClick(".doc-comment", '<p>' + content + '</p>', 350);
    };

    static setDocButton(item, action, visible) {
        var name = 'עדכון';

        if (item === 'timer') {
            if (action === 'add')
                name = 'התחל';
            else if (action === 'update')
                name = 'סיום';


        }
        else if (action === 'add')
            $('#doc-item-update').val('');
        else if (action === 'update')
            $('#doc-item-update').val('עדכון');

        $('#doc-item-update').html(name);

        if (visible !== undefined) {
            if (visible)
                $('#doc-item-update').show();
            else
                $('#doc-item-update').hide();
        }
    };

    //============================================================ app_docs trigger

    triggerDocCommentCompleted(data) {
        this.Comment.end(data);
    };
    triggerDocAssignCompleted(data) {
        this.Assign.end(data);
    };
    triggerDocTimerCompleted(data) {
        this.Assign.end(data);
    };
    triggerDocFormCompleted(data) {
        this.Actions.end(data);
    };
    static triggerSubDocCompleted(type, data) {
        var tag;
        switch (type) {
            case 'comment':
                tag = '#jqxgrid1'; break;
            case 'form':
                tag = '#jqxgrid4';
                break;
        }

        if (tag !== null) {
            $(tag + '-window').hide();
            $(tag + '-bar').show();
            if (data !== null && data.Status > 0)
                $(tag).jqxGrid('source').dataBind();
        }
    };
}

class app_doc extends app_doc_base {

    constructor(dataModel, userInfo, docModel) {

        super(dataModel, userInfo, docModel);
        this.EnableFormType = false;
    }

    init() {

        if (!this.IsInfo) {

            this.tabSettings();
        }

        $("#hTitle-text").text(this.Title + ': ' + $("#DocSubject").val());
        $("#hxp-title").text(this.Title + ': ' + this.DocId);


        this.preLoad();

        if (this.DocId > 0) {
            this._loadData();
            //this.loadControls(this.Model.Data);
        }
        else {
            this.loadControls();
        }
        this.loadEvents();

        //return this;
    };

    tabSettings() {
        var _slf = this;

        if (this.DocId > 0) {
            $("#hxp-1").show();
            $("#hxp-4").show();
            $("#hxp-5").show();
        }
        else {
            $("#hxp-1").hide();
            $("#hxp-4").hide();
            $("#hxp-5").hide();
        }

        if (!this.IsEditable) {
            $("#fcSubmit").hide();
            $("#fcEnd").hide();
        }

        $("#accordion").jcxTabs({
            rotate: false,
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            click: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> clicked!');
            },
            activate: function (e, tab) {
                //$('.info').html('Tab <strong>' + tab.id + '</strong> activated!');

                switch (tab.selector) {
                    case "#exp-1":
                        _slf.sectionSettings(1);
                        break;
                    case "#exp-4":
                        _slf.sectionSettings(4);
                        break;
                    case "#exp-5":
                        _slf.sectionSettings(5);
                        break;
                }
            },
            activateState: function (e, state) {
                //$('.info').html('Switched from <strong>' + state.oldState + '</strong> state to <strong>' + state.newState + '</strong> state!');
            }
        });
    };

    doCancel() {
        app.redirectTo(app_doc_base.getReferrer());
        //return this;
    };

    doSubmit(act) {
        //e.preventDefault();
        var actionurl = $('#fcForm').attr('action');
        var status = this.DocStatus;// app_jqx.getInputAutoValue("#DocStatus", 1);
        var isnew = this.IsNew;

        var afterSubmit = function (slf, data) {

            if (isnew) {
                slf.DocId = data.OutputId;
                $("#DocId").val(data.OutputId);
                slf.tabSettings();
                $("#fcSubmit").val("עדכון");
                slf.IsNew = slf.DocId === 0;
                app_messenger.Notify(data, 'info');
            }
            else {
                app_messenger.Notify(data, 'info', app_doc_base.getReferrer());
            }

            if (act === 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {

            slf.postLoad(true);

            //app_jqx.setInputAutoValue("#DocStatus", status);
            app_tasks.setTaskStatus("#DocStatus", status);

            //var clientId = $("#ClientId").val();
            //if (clientId > 0) {
            //    $('#ClientDetails').val($("#ClientId-display").val())
            //}

            //var clientDetails = $('#ClientDetails').val();
            //, { key: 'ClientDetails', value: clientDetails }
            var value = $("#DocBody").jqxEditor('val');
            var args = [{ key: 'DocBody', value: app.htmlEscape(value) }];
            var formData = app.serializeEx('#fcForm input, #fcForm select, #fcForm hidden', args);

            app_query.doFormSubmit("#fcForm", actionurl, formData, function (data) {

                afterSubmit(slf, data);
            });
            return this;
        };

        if (this.IsNew) {
            this.DocStatus = 1;
            actionurl = '/System/UpdateNewDoc';
            RunSubmit(this, this.DocStatus, actionurl)
        }
        else {
            status = this.DocStatus;
            actionurl = '/System/UpdateDoc';
            if (status > 1 && status < 8 && act === 'end') {
                app_dialog.confirmYesNoCancel("האם לסיים משימה?", function (res) {
                    if (res === 'yes')
                        RunSubmit(this, status, '/System/DocCompleted')
                    else if (res === 'no')//update
                        RunSubmit(this, status, actionurl);

                });
            }
            else {//update
                RunSubmit(this, status, actionurl);
            }
        }
        return this;
    };

    preLoad() {

        var slf = this;


        $("#Doc_Parent").val(slf.DocParentId);
        if (slf.DocParentId > 0) {
            $("#Doc_Parent-group").show();
            $("#Doc_Parent-link").click(function () {
                app.redirectTo('/System/DocInfo?id=' + slf.DocParentId);
            });
        }
        else {
            $("#Doc_Parent-group").hide();
        }


        $("#AccountId").val(slf.AccountId);
  
        //app_tasks.setColorFlag();

        app_tasks.setPermsType();
       

        $('#a-jqxExp-1').on('click', function (e) {
            if (!slf.PostLoaded) {
                slf.postLoad();
            }
            $('#jqxExp-box').slideToggle();
            return false;
        });

        //app_control.selectTag("#Doc_Type");

        app_features.editorTag("#DocBody", slf.IsEditable);

        if (!this.IsInfo) {
            app_features.colorFlag("#ColorFlag", "#hTitle");
        }

        //$("#jqxWidget").slideDown('slow');
        $("#jqxWidget").toggleClass('box-slide');

        /*
        $("#ColorFlag").simplecolorpicker();
        $("#ColorFlag").on('change', function () {
            //$('select').simplecolorpicker('destroy');
            var color = $("#ColorFlag").val();
            $("#hTitle").css("background-color", color)
        });


        $('#DocBody').jqxEditor({
            height: '300px',
            //width: '100%',
            editable: slf.IsEditable,
            rtl: true,
            tools: 'bold italic underline | color background | left center right'
            //theme: 'arctic'
            //stylesheets: ['editor.css']
        });

        $('#DocBody-btn-view').on('click', function () {

            if ($('#DocBody-div').hasClass("editor-view")) {
                $('#DocBody-div').removeClass("editor-view");
                $('#DocBody').jqxEditor('height', '300px');
                $('#DocBody').css('height', '305px');
            }
            else {
                $('#DocBody-div').addClass("editor-view");
                $('#DocBody').css('height', '805px');
                $('#DocBody').jqxEditor('height', '800px');
            }
        });
        */
    }

    loadControls() {

        console.log('controls');


        $('#DueDate').jqxDateTimeInput({ showCalendarButton: this.IsEditable, readonly: !this.IsEditable, width: '150px', rtl: true });

        var record = this.Record;

        if (record) {

            app_form.loadDataForm("fcForm", record, ["DocStatus", "Folder"], this.IsInfo);//, "Project_Id", "ClientId", "Tags", "AssignTo"

            //$("#DocBody").jqxEditor('val', app.htmlUnescape(record.DocBody));

            //$("#DocSubject").val(record.DocSubject);
            $("#hTitle-text").text(this.Title + ": " + record.DocSubject);
            $("#hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

            // $('#DueDate').jqxDateTimeInput({ disabled: !this.IsEditable, showCalendarButton: this.IsEditable });

            if (this.Option !== 'g' && record.DocStatus > 1 && record.DocStatus < 8)
                $("#fcEnd").show();
            else
                $("#fcEnd").hide();

            $('#DocBody').css('text-align', app_style.langAlign(record.Lang))

            //app_tasks.enumTypes_load("#Doc_Type", "D", "Doc_Type");
            app_tasks.enumTypes_load("#Doc_Type", "D", record.Doc_Type);

            //app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Doc_Type", '/System/GetDocTypeList', { 'model': this.DocModel }, 0, 120, true, null, function (status, records) {
            //    if (record.Doc_Type >= 0)
            //        $("#Doc_Type").val(record.Doc_Type);
            //});


            //$("#Doc_Type").jqxComboBox({ enableSelection: this.IsEditable });
            //app_jqx_combo_async.docStatusInputAdapter("#DocStatus", record.DocStatus);
            app_tasks.setTaskStatus("#DocStatus", record.DocStatus);

            //app_jqx_combo_async.docFolderInputAdapter("#Folder", record.Folder, function (status, records) {

            //});
        }
        else {
            //app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", "#Doc_Type", '/System/GetDocTypeList', { 'model': this.DocModel }, 0, 120,true,"0");
            app_tasks.enumTypes_load("#Doc_Type", "D");
            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "IntendedTo", '/System/GetUserTeamList', 0, 120, false);
            app_form.setDateTimeNow('#CreatedDate');//

        }

        app_jqx_combo_async.docFolderInputAdapter("#Folder", record ? record.Folder : null);

        //return this;
    }

    loadEvents() {

        var slf = this;

        $('#reset').on('click', function () {
            location.reload();
        });

        $('#fcClear').on('click', function (e) {
            //app_messenger.Post("הודעה");
            $('#fcForm')[0].reset();
            $('#fcForm').jqxValidator('hide');
        });
        $('#fcNew').on('click', function (e) {
            app.refresh();
        });
        $('#fcCancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#fcSubmit').on('click', function (e) {
            slf.doSubmit('update');
        });

        if (this.IsNew) {

            $('#fcSubmit-plus').on('click', function (e) {
                slf.doSubmit('plus');
            });

            $("#Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    //app_docs_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/DocFormByTemplate", { 'DocId': slf.DocId, 'FormId': args.item.value },
                            function (data) {
                                if (data.Status > 0)
                                    $('#jqxgrid4').jqxGrid('source').dataBind();
                            });
                    });

                    //if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
                    //    app_query.doDataPost("/System/DocFormByTemplate", { 'DocId': slf.DocId, 'FormId': args.item.value },
                    //        function (data) {
                    //            if (data.Status > 0)
                    //                $('#jqxgrid4').jqxGrid('source').dataBind();
                    //        });
                    //}
                }
                $("#Form_Type").jqxComboBox({ selectedIndex: -1 });

            });
            $("#form_template-check").on('change', function (event) {

                if (this.checked) {//($('#form_template-check').is(":checked")) {

                    var rows = $('#jqxgrid4').jqxGrid('getrows');
                    if (rows && rows.length > 0)
                        $("#form_template-div").show();
                    else {
                        app_dialog.alert("לא נמצאו רשומות ליצירת תבנית");
                        this.checked = false;
                    }
                }
                else
                    $("#form_template-div").hide();
            });
            $('#form_template-save').click(function () {
                var name = $('#form_template-input').val();
                if (name === null || name === '') {
                    app_dialog.alert("נא לציין שם תבנית");
                    return;
                }
                name = "'" + name + "'";
                app_query.doDataPost("/System/DocFormTemplateCreate", { 'id': slf.DocId, 'name': name }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid4').jqxGrid('source').dataBind();
                });

                //app_query.doPost("/System/DocFormTemplateCreate", { 'id': slf.DocId, 'name': name }, function (data) {
                //    if (data.Status > 0) {
                //        app_messenger.Post(data);
                //        $('#jqxgrid4').jqxGrid('source').dataBind();
                //    }
                //    else
                //        app_messenger.Post(data, 'error');
                //});
            });
            $("#IntendedTo").on('change', function (event) {
                var args = event.args;
                if (args) {
                    var item = args.item;
                    if (item.label.charAt(0) === '@') {
                        $('#UserId').val(0);
                        $('#TeamId').val(item.value);
                    }
                    else {
                        $('#UserId').val(item.value);
                        $('#TeamId').val(0);
                    }

                }
            });
        }
        else {

            $("#Doc_Child-link").click(function () {

                app_dialog.confirm("האם ליצור תת משימה", function () {
                    app.redirectTo('/System/DocNew?pid=' + slf.DocId);
                });
            });

            $('#fcEnd').on('click', function (e) {
                slf.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    slf.doSubmit('plus');
            //});
            $('#doc-item-update').click(function () {
                var iframe = wizard.getIframe();
                if (iframe && iframe.triggerSubmit) {
                    iframe.triggerSubmit();
                }
            });
            $('#doc-item-cancel').click(function () {
                wizard.wizHome();
            });
        }

        var input_rules = [
            //{ input: '#DocSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
            {
                input: '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'none', rule: function (input, commit) {
                    //var value = $('#DueDate').jqxDateTimeInput('value');
                    var text = $('#DueDate').jqxDateTimeInput('getText');
                    return text !== null && text.length > 0;
                }
            },
            {
                input: "#DocBody", message: 'חובה לציין תוכן!', action: 'none', rule: function (input, commit) {
                    //var value = $("#DocBody").text();//.jqxEditor('val');
                    var value = $("#DocBody").jqxEditor('val');
                    value = app.htmlText(value);//.replace(/(<([^>]+)>)/ig, "");
                    return value.length > 1;
                }
            }
        ];

        $('#fcForm').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    }

}

//============================================================ app_doc_comment

class app_doc_comment_grid {

    constructor(docId, userInfo, option) {
        this.wizardStep = 2;
        this.DocId = docId;
        this.Option = (option) ? option : 'e';
        this.UInfo = userInfo;
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.Control = null;
        this.init();
    }
    init() {
        var slf = this;
        var docid = this.DocId;

        var nastedsource = {
            datafields: [
                {
                    name: 'CommentId', type: 'number'
                },
                {
                    name: 'CommentDate', type: 'date'
                },
                {
                    name: 'CommentText', type: 'string'
                },
                {
                    name: 'ReminderDate', type: 'date'
                },
                {
                    name: 'Attachment', type: 'string'
                },
                {
                    name: 'DisplayName', type: 'string'
                },
                {
                    name: 'Doc_Id', type: 'number'
                },
                { name: 'UserId', type: 'number' }
            ],
            datatype: "json",
            id: 'CommentId',
            type: 'POST',
            url: '/System/GetDocsCommentGrid',
            data: {
                'pid': docid
            }
        }
        var nastedAdapter = new $.jqx.dataAdapter(nastedsource);

        //var nastedAdapter = new $.jqx.dataAdapter(nastedsource, {
        //    loadComplete (record) {
        //        $("#hxp-1").text("הערות : " + record.length);
        //    },
        //    loadError (jqXHR, status, error) {
        //    },
        //    beforeLoadComplete (records) {
        //    }
        //});


        $("#jqxgrid1").jqxGrid({
            scrollbarsize: 1,
            autoheight: true,
            autorowheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: nastedAdapter, width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
                {
                    text: 'מועד רישום', datafield: 'CommentDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center'
                },
                {
                    text: 'הערה', datafield: 'CommentText', cellsalign: 'right', align: 'center'
                },
                { text: 'שם', datafield: 'DisplayName', cellsalign: 'right', align: 'center', width: 100, hidden: app.IsMobile() }
                //{
                //    text: '...', datafield: 'CommentId', width: 120, cellsalign: 'right', align: 'center',
                //              cellsrenderer (row, columnfield, value, defaulthtml, columnproperties) {
                //                  //var value = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;

                //                  //app_jqx.toolTipClick(null, value, 350);

                //                  //var drop = '<a class="drop-target drop-theme-hubspot-popovers">הצג</a>' +
                //                  //      '<div class="drop-content">' +
                //                  //          '<div class="drop-content-inner">' +
                //                  //              '<h3 class="title">Drop.js</h3>' +
                //                  //              '<p>Drop.js is a fast and capable dropdown library built on <a href="/tether/docs/welcome" target="_blank" style="color: inherit">Tether</a>.</p>' +
                //                  //          '</div>' +
                //                  //      '</div>';

                //                  return '<div class="doc-comment" style="text-align:center;direction:rtl;margin:5px;"><a href="#" onclick="docs_edit_view_comment('+row+')">' + 'הצג' + '</a></div>';
                //              }
                //}
            ]
        });
        $('#jqxgrid1').on('rowdoubleclick', function (event) {
            slf.edit();
        });
        $('#jqxgrid1-add').click(function () {
            slf.add();
        });

        $('#jqxgrid1-edit').click(function () {
            slf.edit();
        });

        $('#jqxgrid1-remove').click(function () {
            slf.remove();
        });
        $('#jqxgrid1-refresh').click(function () {
            slf.refresh();
        });
    }
    showControl(id, option, action) {

        var data_model = {
            PId: this.DocId, Id: id, Option: option, Action: action
        };

        if (this.Control === null) {
            this.Control = new app_doc_comment_item("#jqxgrid1-window");//app_doc_comment_control("#jqxgrid1-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
    getrowId() {

        var selectedrowindex = $("#jqxgrid1").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid1").jqxGrid('getrowid', selectedrowindex);
        return id;
    }
    add() {
        //setDocButton('comment', 'add', true);
        //wizard.appendIframe(2, app.appPath() + "/System/_DocCommentAdd?pid=" + this.DocId, "100%", "500px");

        //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_DocCommentAdd?pid=" + this.DocId, "100%", "280px");
        this.showControl(0, 'a');

    }
    edit() {
        if (this.Option !== "e")
            return;
        var id = this.getrowId();
        if (id > 0) {
            //setDocButton('comment', 'update', true);
            //wizard.appendIframe(2, app.appPath() + "/System/_DocCommentEdit?id=" + id, "100%", "500px");

            //app_iframe.appendEmbed("jqxgrid1-window", app.appPath() + "/System/_DocCommentEdit?id=" + id, "100%", "350px");

            this.showControl(id, 'e');
        }
    }
    remove() {
        var id = this.getrowId();
        if (id > 0) {
            app_dialog.confirm('האם למחוק הערה ' + id, function () {
                app_query.doPost(app.appPath() + "/System/DocCommentDelete", {
                    'id': id
                }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid1').jqxGrid('source').dataBind();
                });
            });

        }
    }
    refresh() {
        $('#jqxgrid1').jqxGrid('source').dataBind();
    }
    cancel() {
        wizard.wizHome();
    }
    end(data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
            // app_dialog.alert(data.Message);
        }
    }
};

class app_doc_comment_item {

    constructor($element) {
        this.$element = $($element);
        //var slf = this;
        //return this;
    }

    tag($element, option) {

        var pasive = option === "a" ? " pasive" : "";

        var html = '<div id="comment-Window" class="container" style="margin:5px">' +
            '<hr style="width:100%;border:solid 1px #15C8D8">' +
            '<div id="comment-Header">' +
            '<span id="comment-Title" style="font-weight: bold;">מעקב טיפול</span>' +
            '</div>' +
            '<div id="comment-Body">' +
            '<form class="fcForm" id="comment-Form" method="post" action="/System/DocCommentUpdate">' +
            '<div style="direction: rtl; text-align: right">' +
            '<input type="hidden" id="CommentId" name="CommentId" value="0" />' +
            '<input type="hidden" id="Doc_Id" name="Doc_Id" value="0" />' +
            '<input type="hidden" id="UserId" name="UserId" value="" />' +
            '<div style="height:5px"></div>' +
            '<div id="tab-content" class="tab-content" dir="rtl">' +
            '<div class="form-group">' +
            '<div class="field">הערה:</div>' +
            '<textarea id="CommentText" name="CommentText" class="text-content-mid"></textarea>' +
            '</div>' +
            '<div class="form-group">'+
            '<input type="checkbox" id="IsSchedule" name="IsSchedule" />&nbsp;<span>תזכורת?</span>'+
            '</div>'+
            '<div id="ReminderDate-div" class="form-group pasive">' +
            '<div class="field">תזכורת ל:</div>' +
            '<div id="ReminderDate" name="ReminderDate"></div>' +
            '</div>' +
            '<div class="form-group' + pasive + '">' +
            '<div class="field">נוצר ב:</div>' +
            '<input id="CommentDate" name="CommentDate" type="text" readonly="readonly" class="text-mid" />' +
            '</div>' +
            '<div class="form-group' + pasive + '">' +
            '<div class="field">נוצר ע"י":</div>' +
            '<input id="DisplayName" name="DisplayName" type="text" readonly="readonly" class="text-mid" />' +
            '</div>' +
            '</div>' +
            '<div>' +
            '<a id="comment-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="comment-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
            '</div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';

        $element.html(html).hide();

    };

    init(dataModel, userInfo, readonly) {
        this.DocId = dataModel.PId;
        this.CommentId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(this.$element, dataModel.Option);

        $('#ReminderDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        //$('#ReminderDate').val('');

        $('#comment-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#comment-Cancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#IsSchedule').on('click', function (e) {
            $("#ReminderDate-div").toggle();
        });

        

        var input_rules = [
            { input: '#CommentText', message: 'חובה לציין תוכן!', action: 'none', rule: 'required' },
        ];

        $('#comment-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });

        var load = function (slf) {

            //if (this.viewAdapter == null) {
            var view_source =
                {
                    datatype: "json",
                    id: 'CommentId',
                    data: { 'docid': slf.DocId, 'id': slf.CommentId },
                    type: 'POST',
                    url: '/System/GetDocCommentEdit'
                };

            var viewAdapter = new $.jqx.dataAdapter(view_source, {
                loadComplete: function (record) {
                    app_jqxform.loadDataForm("comment-Form", record);
                    if (record.CommentDate > 0)
                        $('#CommentDate').val(record.CommentDate.toLocaleDateString());
                    if (record.IsSchedule)
                        $("#ReminderDate-div").show();
                    else
                        $("#ReminderDate-div").hide();

                    $("#comment-Title").text("הערה: " + slf.CommentId);
                },
                loadError: function (jqXHR, status, error) {
                },
                beforeLoadComplete: function (records) {
                }
            });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'docid': slf.DocId, 'id': slf.CommentId };
            //}
            viewAdapter.dataBind();
        };

        if (this.CommentId > 0) {
            load(this);
        } else {
            $('#comment-Form input[name=Doc_Id]').val(this.DocId);
            $('#comment-Form input[name=UserId]').val(this.UserInfo.UserId);
            $("#comment-Title").text("הערה: " + "חדשה");
        }
    };

    display() {
        this.$element.show();
        $("#jqxgrid1-bar").hide()
    };

    doCancel() {

        app_doc_base.triggerSubDocCompleted('comment');
    };

    doSubmit() {
        //e.preventDefault();
        var actionUrl = $('#comment-Form').attr('action');
        var formData = $('#comment-Form').serialize();

        app_query.doFormSubmit('#comment-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {
                app_doc_base.triggerSubDocCompleted('comment', data);
            }
            else
                app_messenger.Post(data, 'error');
        });
    };

    //return app_doc_comment;
}
//============================================================ app_doc_form

class app_doc_form_grid {

    //constructor(docId, userInfo, option) {

    //    super(docId, userInfo, option);
    //}

    constructor(docId, userInfo, option) {
        this.wizardStep = 2;
        this.DocId = docId;//dataModel.Id;
        this.UserId = userInfo.UserId;
        this.Option = (option) ? option : 'e';
        this.UInfo = userInfo;
        this.isMobile = app.IsMobile();
        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.Control = null;
        this.Import = null;
        this.init();
        //return this;
    }

    init() {
        var slf = this;
        var docid = this.DocId;

        var docsource = {
            datafields: [
                { name: 'ItemId', type: 'number' },
                { name: 'ItemDate', type: 'date' },
                { name: 'ItemValue', type: 'string' },
                { name: 'DoneDate', type: 'date' },
                { name: 'DoneStatus', type: 'bool' },
                { name: 'DisplayName', type: 'string' },
                { name: 'Doc_Id', type: 'number' },
                { name: 'ItemLabel', type: 'string' },
                { name: 'UserId', type: 'number' }
            ],
            updaterow: function (rowid, rowdata, commit) {
                // that function is called after each edit.
                var rowindex = $("#jqxgrid4").jqxGrid('getrowboundindexbyid', rowid);
                //editedRows.push({ index: rowindex, data: rowdata });
                app_query.doDataPost('/System/DocFormUpdate', rowdata, function (data) {

                    if (data.Status > 0) {
                        commit(true);
                        //app_doc_base.triggerSubDocCompleted('form', data);
                    }
                    else {
                        app_messenger.Post(data, 'error');
                        commit(false);
                    }
                });
                commit(true);
            },
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetDocsFormGrid',
            data: { 'pid': docid }
        };

        var docAdapter = new $.jqx.dataAdapter(docsource);
        // validation function
        var validateFunc = function (datafield, value) {
            switch (datafield) {
                case "ItemLabel":
                    if (value.length ==0) {
                        return { message: "נדרש נושא", result: false };
                    }
                    return true;
                case "ItemValue":
                    if (value.length == 0) {
                        return { message: "נדרש תאור", result: false };
                    }
                    return true;
                //case "DoneDate":
                //    if (new Date(value) == "Invalid Date") {
                //        return { message: "נדרש תאריך ביצוע", result: false };
                //    }
                //    return true;
                //    break;
            }
            return true;
        }

        $("#jqxgrid4").jqxGrid({
            editable: slf.Option === "e",
            //editmode: 'click',
            autoheight: true,
            autorowheight: true,
            enabletooltips: true,
            localization: getLocalization('he'),
            source: docAdapter,
            width: '99%', height: 130,
            columnsresize: true,
            rtl: true,
            columns: [
                //{
                //    text: 'מס', dataField: 'ItemDoc', filterable: false, width: 90, cellsalign: 'right', align: 'center',
                //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                //        var editlink = '';
                //        //var asb = $('#jqxgrid').jqxGrid('getrowdata', row).AssignBy;
                //        //if (slf.UserId == asb)
                //        //    editlink = '<label> </label><a href="#" onclick="app_doc.docTopicEdit(' + value + ')" ><label> </label><i class="fa fa-plus-square-o"></i></a>';
                //        if (value > 0)
                //            return '<div style="text-align:center">' + value + '<a href="#" onclick="app_tasks.docEdit(' + value + ',\"D\")" ><label> </label><i class="fa fa-plus-square-o"></i></a>' + editlink + '</div>';
                //        else
                //            return '';
                //    }
                //},
                //{ text: 'מועד לביצוע', datafield: 'ItemDueDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
                {
                    text: 'נושא', datafield: 'ItemLabel', width: 200, cellsalign: 'right', align: 'center', editable: false,
                    validateEverPresentRowWidgetValue: validateFunc,
                    validation: validateFunc
                 },
                {
                    text: 'תאור', datafield: 'ItemValue', cellsalign: 'right', align: 'center', editable: false,
                    validateEverPresentRowWidgetValue: validateFunc,
                    validation: validateFunc
                },
                { text: 'מועד סיום', datafield: 'DoneDate', width: 150, cellsalign: 'right', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm', align: 'center', editable: false, hidden: slf.isMobile },
                { text: 'בוצע', datafield: 'DoneStatus', columntype: 'checkbox', width: 120, cellsalign: 'right', align: 'center' },
                { text: 'שם', datafield: 'DisplayName', width: 120, cellsalign: 'right', align: 'center', editable: false, hidden: slf.isMobile }
                //{
                //    text: '...', datafield: 'ItemId', width: 120, cellsalign: 'right', align: 'center',
                //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                //        return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="#">' + 'הצג' + '</a></div>'
                //    }
                //}
            ]
        });

        $("#jqxgrid4").on('cellvaluechanged', function (event) {
            // event arguments.
            var args = event.args;
            // column data field.
            var datafield = event.args.datafield;
            if (datafield == "DoneStatus") {
                // row's bound index.
                var rowBoundIndex = args.rowindex;
                // new cell value.
                var value = args.newvalue;
                // old cell value.
                var oldvalue = args.oldvalue;

                var id = args.owner.rows.records[args.rowindex].bounddata.ItemId;
                slf.update(id, value);
            }

        });
        $('#jqxgrid4').on('rowselect', function (event) {
            // event arguments.
            var args = event.args;
            // row's bound index.
            var rowBoundIndex = args.rowindex;
            // row's data. The row's data object or null(when all rows are being selected or unselected with a single action). If you have a datafield called "firstName", to access the row's firstName, use var firstName = rowData.firstName;
            var rowData = args.row;

            if (rowData) {
                var editable = (rowData.UserId === slf.UserId);
                app.showIf("#jqxgrid4-remove", editable);
                //app.showIf("#jqxgrid4-edit",editable);
            }

        });

        $('#jqxgrid4').on('rowdoubleclick', function (event) {
            slf.edit();
        });
        $('#jqxgrid4-add').click(function () {
            slf.add();
        });

        $('#jqxgrid4-edit').click(function () {
            slf.edit();
        });

        $('#jqxgrid4-remove').click(function () {
            slf.remove();
        });
        $('#jqxgrid4-refresh').click(function () {
            slf.refresh();
        });

    }
    showControl(id, option, action) {

        var data_model = {
            PId: this.DocId, Id: id, Option: option, Action: action
        };

        if (this.Control === null) {
            this.Control = new app_doc_form_item("#jqxgrid4-window");
        }
        this.Control.init(data_model, this.UInfo);
        this.Control.display();
    }
    getRowData() {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return null;
        var data = $('#jqxgrid4').jqxGrid('getrowdata', selectedrowindex);
        return data;
    }
    getrowId() {

        var selectedrowindex = $("#jqxgrid4").jqxGrid('getselectedrowindex');
        if (selectedrowindex < 0)
            return -1;
        var id = $("#jqxgrid4").jqxGrid('getrowid', selectedrowindex);
        return id;
    }
    add() {
        this.showControl(0, 'a', 'add');
    }
    edit() {
        if (this.Option != "e")
            return;
        var rowData = this.getRowData();
        if (rowData) {
            var opt = (rowData.UserId == this.UserId) ? 'e' : 'r';

            var act = 'edit';
            if (rowData.StartDate == null)
                act = 'start';
            else if (rowData.DoneStatus)
                act = 'done';

            this.showControl(rowData.ItemId, opt, act);
        }
    }
    doImport() {

        var _slf = this;
        this.Import = null;
        /*
        var appimport = new app_doc_form_import(this.DocId, this.UserId, 0);

        appimport.init(function (data) {

            if (data.result.Status > 0) {

                _slf.refresh();
            }

            $("#jqxgrid4-import_window").hide();
            $("#jqxgrid4-bar").show();
            app_doc_base.triggerSubDocCompleted('form');

        });

        this.Import = appimport;
        */
    }
    remove() {

        var data = this.getRowData();
        if (data == null)
            return;
        if (this.UserId != data.UserId) {
            app_dialog.alert("לא ניתן למחוק רשומה שהוקצתה על ידי משתמש אחר");
            return;
        }
        var id = data.ItemId;// this.getrowId();
        if (id > 0) {
            app_dialog.confirm('האם למחוק רשומה ' + id, function () {
                app_query.doPost(app.appPath() + "/System/DocFormDelete", {
                    'id': id
                }, function (data) {
                    if (data.Status > 0)
                        $('#jqxgrid4').jqxGrid('source').dataBind();
                });
            });
        }
    }
    update(id, value) {

        $.ajax({
            url: '/System/DocFormChecked',
            type: 'post',
            dataType: 'json',
            data: {
                'id': id, 'done': value
            },
            success: function (data) {
                if (data.Status > 0) {
                    app_doc_base.triggerSubDocCompleted('form', data);
                }
                else
                    app_messenger.Post(data, 'error');
            },
            error: function (jqXHR, status, error) {
                app_messenger.Post(error, 'error');
            }
        });
    }
    update_row(id, rowData) {

        app_query.doDataPost('/System/DocFormUpdate', rowData, function (data) {

            if (data.Status > 0) {
                app_doc_base.triggerSubDocCompleted('form', data);
            }
            else
                app_messenger.Post(data, 'error');
        });
    }
    refresh() {
        $('#jqxgrid4').jqxGrid('source').dataBind();
    }
    cancel() {
        wizard.wizHome();
    }
    end(data) {
        wizard.wizHome();
        //wizard.removeIframe(2);
        app_messenger.Post(data);
        if (data && data.Status > 0) {
            this.refresh();
        }
    }
}

class app_doc_form_item {

    constructor($element) {
        this.$element = $($element);
    }

    tag($element, dataModel) {
        var pasive = dataModel.Option === "a" ? " pasive" : "";

        var html = '<div id="form-Window" class="container" style="margin:5px">' +
            '<hr style="width:100%;border:solid 1px #15C8D8">' +
            '<div id="form-Header">' +
            '<span id="form-Title" style="font-weight: bold;">סיכומים</span>' +
            '</div>' +
            '<div id="form-Body">' +
            '<form class="fcForm" id="form-Form" method="post" action="/System/DocFormUpdate">' +
            '<div style="direction: rtl; text-align: right">' +
            '<input type="hidden" id="ItemId" name="ItemId" value="0" />' +
            '<input type="hidden" id="Doc_Id" name="Doc_Id" value="0" />' +
            '<input type="hidden" id="form-UserId" name="UserId" value="" />' +
            '<div style="height:5px"></div>' +
            '<div id="tab-content" class="tab-content" dir="rtl">' +
            '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">לביצוע</div>' +
            '<div class="form-group">' +
            '<div class="field">נושא:</div>' +
            '<input id="ItemLabel" name="ItemLabel" type="text" style="width:90%" />' +
            '</div>' +
            '<div class="form-group">' +
            '<div class="field">תאור:</div>' +
            '<textarea id="ItemValue" name="ItemValue" class="text-content"' + (dataModel.Option !== "a" ? " readonly=\"true\"" : "") + '></textarea>' +
            '</div>' +
            //'<div class="form-group">' +
            //'<div class="field">מועד לביצוע:</div>' +
            //'<div id="ItemDueDate" name="ItemDueDate"></div>' +
            //'</div>' +
            '<div class="form-group' + (dataModel.Id === 0 ? " pasive" : "") + '">' +
            '<!--<div class="form-group">' +
            '<div class="field">נוצר בתאריך:</div>' +
            '<input id="ItemDate" name="ItemDate" type="text" readonly="readonly" class="text-mid" data-type="date" />' +
            '</div>-->' +
            '<div id="form-Done-group" class="form-group pasive">' +
            '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">סיום ביצוע</div>' +
            '<div id="divDoneDate" class="form-group">' +
            '<div class="field">מועד ביצוע:</div>' +
            '<input id="DoneDate" name="DoneDate" type="text" readonly="readonly" class="text-mid" data-type="datetime" />' +
            '</div>' +
            '<div id="form-Done" class="form-group pasive">' +
            '<div class="field"><a id="form-comment-toggle" href="#" ><i class="fa fa-plus-square-o"></i></a><span>הערות ביצוע:</span></div>' +
            '<div id="form-Comment">' +
            '<textarea id="DoneComment" name="DoneComment" class="text-content"' + (dataModel.Option !== "e" ? " readonly=\"true\"" : "") + '></textarea>' +
            '</div>' +
            '</div>' +
            '<div class="form-group pasive">' +
            '<div class="field">' +
            'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option !== "e" ? " readonly=\"true\"" : "") + '/>' +
            '</div>' +
            '</div>' +
            '<div id="divDisplayName" class="form-group">' +
            '<div class="field">בוצע ע"י:</div>' +
            '<input id="DisplayName" type="text" readonly="readonly" class="text-mid" data-field="DisplayName"/>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div>' +
            '<a id="form-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
            '<a id="form-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a> ' +
            '</div>' +
            '</div>' +
            '</form>' +
            '</div>' +
            '</div>';
        $element.html(html);
    };

    init(dataModel, userInfo, readonly) {

        this.DocId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;

        this.tag(slf.$element, dataModel);

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        var input_rules = [
            { input: '#ItemValue', message: 'חובה לציין תאור!', action: 'none', rule: 'required' },
            { input: '#ItemLabel', message: 'חובה לציין נושא!', action: 'none', rule: 'required' }
        ];

        $('#DoneDate').jqxDateTimeInput({ showCalendarButton: true, readonly: false, width: '150px', rtl: true });
        //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "ItemAssignTo", '/System/GetUserTeamList', 0, 120, false);

        $('#form-Form').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });

        $('#form-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#form-Cancel').on('click', function (e) {
            slf.doCancel();
        });
        $('#form-Start').on('click', function (e) {
            slf.doStart();
        });
        //$('#form-comment-toggle').on('click', function (e) {
        //    $("#form-Comment").toggle();
        //});

        var load = function (slf) {

            //if (this.viewAdapter == null) {
            var view_source =
                {
                    datatype: "json",
                    id: 'ItemId',
                    data: { 'id': slf.ItemId },
                    type: 'POST',
                    url: '/System/GetDocFormEdit'
                };

            var viewAdapter = new $.jqx.dataAdapter(view_source, {
                loadComplete: function (record) {
                    app_form.loadDataForm("form-Form", record);

                    //app_jqxform.loadDataForm("form-Form", record);
                    // $("form#form-Form [name=UserId]").val(record.UserId);

                    //var itemDate = app.toLocalDateString(record.ItemDate);
                    $("#ItemText").prop("readonly", slf.ReadOnly);

                    //$('#Duration').val(record['Duration']);
                    //$("#ItemDate").val(app.toLocalDateString(record.ItemDate));
                    //$('#StartDate').val(app.toLocalDateString(record.StartDate));
                    //$("#DoneDate").val(app.toLocalDateTimeString(record.DoneDate));
                    //$("#form-Comment").hide();
                    $("#form-Title").text("פעולה: " + slf.ItemId);

                    //if (record.StartDate || record.ItemDoc > 0) {
                    //    $("#form-Start").hide();
                    //    //$("#form-Done-group").show();
                    //}
                    //else {
                    //    $("#form-Start").show();
                    //    //$("#form-Done-group").hide();

                    //}

                    //if (record.DoneStatus == false) {
                    //    $("#divDoneDate").hide();
                    //    $("#divDisplayName").hide();
                    //}
                    //else {
                    //    //$("#DoneStatus").prop('disabled', true)
                    //    $("#DoneComment").prop('readonly', true)
                    //    $("#form-Submit").hide();
                    //    $("#form-Start").hide();
                    //}
                },
                loadError: function (jqXHR, status, error) {
                },
                beforeLoadComplete: function (records) {
                }
            });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'id': slf.ItemId };
            //}
            viewAdapter.dataBind();

        };

        if (this.ItemId > 0) {
            load(this);
        }
        else {
            $("#form-Form input[name=Doc_Id]").val(this.DocId);
            $("#form-Form input[name=UserId]").val(this.UserInfo.UserId);
            $("#form-Title").text("רשומה: " + "חדשה");
            //$('#form-Submit').text("התחל");
        }
    };
    display() {
        this.$element.show();
        $("#jqxgrid4-bar").hide();
    };

    doCancel() {
        $("#jqxgrid4-bar").show();
        app_doc_base.triggerSubDocCompleted('form');
    };
    doSubmit() {
        //e.preventDefault();
        var formData = $("#form-Form").serialize();
        var actionurl = $('#form-Form').attr('action');
        app_query.doFormSubmit("#form-Form", actionurl, null, function (data) {

            if (data.Status > 0) {
                app_doc_base.triggerSubDocCompleted('form', data);
            }
            else
                app_messenger.Post(data, 'error');
        });

    };
    doStart() {
        //e.preventDefault();

        app_query.doDataPost("/System/DocFormStart", { 'id': this.DocId, 'itemId': this.ItemId }, function (data) {
            if (data.Status > 0) {
                app_form.setDateTimeNow('#StartDate');
                //$('#StartDate').val(app.toLocalDateString(Date.now()));
                //$("#form-Done-group").show();
                $("#form-Start").hide();
                $("#jqxgrid4").jqxGrid('source').dataBind();
                //app_doc.triggerSubDocCompleted('form', data);
            }
        });
    };
}
