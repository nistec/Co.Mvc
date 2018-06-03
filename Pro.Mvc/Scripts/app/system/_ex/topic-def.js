//'use strict';

//============================================================================================ app_topic

function app_topic_demo()
{
    var task = new app_task();
   
    task.doCancel();
    task.comment.end();
    app_task.triggerSubTaskCompleted();
    //app_task_trigger.
}

 

//============================================================ app_task_comment_control

var app_task_comment = (function () {

    var app_task_comment = function ($element) {
        this.$element = $($element);
        var slf = this;

        return this;
    }

    app_task_comment.tag = function ($element,option) {

        var pasive = option == "a" ? " pasive" : "";

        var html = '<div id="comment-Window" class="container" style="margin:5px">' +
                          '<hr style="width:100%;border:solid 1px #15C8D8">' +
                          '<div id="comment-Header">' +
                              '<span id="comment-Title" style="font-weight: bold;">מעקב טיפול</span>' +
                          '</div>' +
                          '<div id="comment-Body">' +
                              '<form class="fcForm" id="comment-Form" method="post" action="/System/TaskCommentUpdate">' +
                                  '<div style="direction: rtl; text-align: right">' +
                                      '<input type="hidden" id="CommentId" name="CommentId" value="0" />' +
                                      '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                      '<input type="hidden" id="UserId" name="UserId" value="" />' +
                                      '<div style="height:5px"></div>' +
                                      '<div id="tab-content" class="tab-content" dir="rtl">' +
                                            '<div class="form-group">' +
                                              '<div class="field">הערה:</div>' +
                                              '<textarea id="CommentText" name="CommentText" class="text-content-mid"></textarea>' +
                                            '</div>' +
                                            '<div class="form-group">' +
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

    app_task_comment.prototype.init = function (dataModel, userInfo,readonly) {
        this.TaskId = dataModel.PId;
        this.CommentId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf=this;

        app_task_comment.tag(this.$element,dataModel.Option);

        $('#ReminderDate').jqxDateTimeInput({ showCalendarButton: true, width: '150px', rtl: true });
        //$('#ReminderDate').val('');

        $('#comment-Submit').on('click', function (e) {
            e.preventDefault();
            slf.doSubmit();
        });
        $('#comment-Cancel').on('click', function (e) {
            slf.doCancel();
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
                    data: { 'taskid': slf.TaskId, 'id': slf.CommentId },
                    type: 'POST',
                    url: '/System/GetTaskCommentEdit'
                };

                var viewAdapter = new $.jqx.dataAdapter(view_source, {
                    loadComplete: function (record) {
                        app_jqxform.loadDataForm("comment-Form", record);
                        if (record.CommentDate > 0)
                            $('#CommentDate').val(record.CommentDate.toLocaleDateString());
                        $("#comment-Title").text("הערה: " + slf.CommentId);
                    },
                    loadError: function (jqXHR, status, error) {
                    },
                    beforeLoadComplete: function (records) {
                    }
                });
            //}
            //else {
            //    this.viewAdapter._source.data = { 'taskid': slf.TaskId, 'id': slf.CommentId };
            //}
            viewAdapter.dataBind();
        };

        if (this.CommentId > 0) {
            load(this);
        } else {
            $('#comment-Form input[name=Task_Id]').val(this.TaskId);
            $('#comment-Form input[name=UserId]').val(this.UserInfo.UserId);
            $("#comment-Title").text("הערה: " + "חדשה");
        }
    };

    app_task_comment.prototype.display = function () {
        this.$element.show();
        $("#jqxgrid1-bar").hide()
    };

    app_task_comment.prototype.doCancel = function () {

        app_task.triggerSubTaskCompleted('comment');
    };

    app_task_comment.prototype.doSubmit = function () {
        //e.preventDefault();
        var actionUrl = $('#comment-Form').attr('action');
        var formData = $('#comment-Form').serialize();
      
        app_query.doFormSubmit('#comment-Form', actionUrl, formData, function (data) {
            if (data.Status > 0) {
                app_task.triggerSubTaskCompleted('comment', data);
            }
            else
                app_messenger.Post(data, 'error');
        });

        //var validationResult = function (isValid) {
        //    if (isValid) {
        //        $.ajax({
        //            url: actionurl,
        //            type: 'post',
        //            dataType: 'json',
        //            data: formData,
        //            success: function (data) {

        //                if (data.Status > 0) {
        //                    app_task.triggerSubTaskCompleted('comment', data);

        //                    //if (slf.IsDialog) {
        //                    //    window.parent.triggerCommentCompleted(data.OutputId);
        //                    //    //$('#fcForm').reset();
        //                    //}
        //                    //else {
        //                    //    app.refresh();
        //                    //}
        //                    //$('#TaskId').val(data.OutputId);
        //                }
        //                else
        //                    app_messenger.Post(data, 'error');
        //            },
        //            error: function (jqXHR, status, error) {
        //                app_messenger.Post(error, 'error');
        //            }
        //        });
        //    }
        //}

        //$('#comment-Form').jqxValidator('validate', validationResult);

    };

    return app_task_comment;
}());


//============================================================ app_task_form_control

var app_task_form = (function () {

    var app_task_form = function ($element) {
        this.$element = $($element);
        var slf = this;
        return this;
    }

    app_task_form.tag = function ($element, dataModel) {
        var pasive = dataModel.Option == "a" ? " pasive" : "";

        var html = '<div id="form-Window" class="container" style="margin:5px">' +
                    '<hr style="width:100%;border:solid 1px #15C8D8">' +
                    '<div id="form-Header">' +
                        '<span id="form-Title" style="font-weight: bold;">פעולות</span>' +
                    '</div>' +
                    '<div id="form-Body">' +
                        '<form class="fcForm" id="form-Form" method="post" action="/System/TaskFormUpdate">' +
                            '<div style="direction: rtl; text-align: right">' +
                                '<input type="hidden" id="ItemId" name="ItemId" value="0" />' +
                                '<input type="hidden" id="Task_Id" name="Task_Id" value="0" />' +
                                '<input type="hidden" id="form-UserId" name="UserId" value="" />' +
                                '<input type="hidden" id="AssignBy" name="AssignBy" value="" />' +
                                '<input type="hidden" id="Duration" name="Duration" value="" />' +
                                '<div style="height:5px"></div>' +
                                '<div id="tab-content" class="tab-content" dir="rtl">' +
                                    '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">לביצוע</div>' +
                                     '<div class="form-group">' +
                                            '<div class="field">משימה עבור:</div>' +
                                            '<div id="ItemAssignTo" name="ItemAssignTo"></div>' +
                                     '</div>' +
                                     '<div class="form-group">' +
                                        '<div class="field">נושא:</div>' +
                                        '<input id="ItemSubject" name="ItemSubject" type="text" style="width:90%" />' +
                                    '</div>' +
                                    '<div class="form-group">' +
                                        '<div class="field">משימה:</div>' +
                                        '<textarea id="ItemText" name="ItemText" class="text-content"' + (dataModel.Option != "a" ? " readonly=\"true\"" : "") + '></textarea>' +
                                    '</div>' +
                                     '<div class="form-group">' +
                                            '<div class="field">מועד לביצוע:</div>' +
                                            '<div id="ItemDueDate" name="ItemDueDate"></div>' +
                                     '</div>' +
                                    '<div class="form-group' + (dataModel.Id == 0 ? " pasive" : "") + '">' +
                                        '<!--<div class="form-group">' +
                                            '<div class="field">נוצר בתאריך:</div>' +
                                            '<input id="ItemDate" name="ItemDate" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>-->' +
                                        '<div class="form-group">' +
                                            '<div class="field">יצירת משימה:</div>' +
                                            '<!--<input id="StartDate" name="StartDate" type="text" readonly="readonly" class="text-mid" />-->' +
                                            '<a id="form-Start" href="#" class="btn-bar"><i class="fa fa-chevron-left"></i>צור משימה</a> ' +
                                        '</div>' +
                                        '<div id="form-Done-group" class="form-group pasive">' +
                                        '<div id="fcTitle" class="panel-header pasive" style="font-weight: bold;">סיום ביצוע</div>' +
                                        '<div id="divDoneDate" class="form-group">' +
                                            '<div class="field">מועד סיום:</div>' +
                                            '<input id="DoneDate" name="DoneDate" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                        '<div id="form-Done" class="form-group pasive">' +
                                            '<div class="field"><a id="form-comment-toggle" href="#" ><i class="fa fa-plus-square-o"></i></a><span>הערות ביצוע:</span></div>' +
                                            '<div id="form-Comment">' +
                                            '<textarea id="DoneComment" name="DoneComment" class="text-content"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '></textarea>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div class="form-group pasive">' +
                                            '<div class="field">' +
                                                'בוצע: <input id="DoneStatus" name="DoneStatus" type="checkbox"' + (dataModel.Option != "e" ? " readonly=\"true\"" : "") + '/>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div id="divDisplayName" class="form-group">' +
                                            '<div class="field">בוצע ע"י:</div>' +
                                            '<input id="form-DisplayName" name="DisplayName" type="text" readonly="readonly" class="text-mid" />' +
                                        '</div>' +
                                        '</div>' +
                                    '</div>' +
                                '</div>' +
                                '<div>' +
                                    '<a id="form-Submit" class="btn-default btn7 w-60" href="#">עדכון</a> ' +
                                    '<a id="form-Cancel" class="btn-default btn7 w-60" href="#">ביטול</a>' +
                                '</div>' +
                            '</div>' +
                        '</form>' +
                    '</div>' +
        '</div>';
        $element.html(html);
    };

    app_task_form.prototype.init = function (dataModel, userInfo,readonly) {

        this.TaskId = dataModel.PId;
        this.ItemId = dataModel.Id || 0;
        this.AccountId = userInfo.AccountId;
        this.UserInfo = userInfo;
        this.AllowEdit = (userInfo.UserRole > 4) ? 1 : 0;
        this.ReadOnly = (readonly) ? true : false;
        var slf = this;
        app_task_form.tag(slf.$element, dataModel);

        //$("#timer-Form input[name=AccountId]").val(this.AccountId);

        var input_rules = [
             { input: '#ItemText', message: 'חובה לציין משימה!', action: 'none', rule: 'required' },
        ];

        $('#ItemDueDate').jqxDateTimeInput({ showCalendarButton: true, readonly: false, width: '150px', rtl: true });
        app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "ItemAssignTo", '/System/GetUserTeamList', 0, 120, false);
        //$("#ItemAssignTo").on('change', function (event) {
        //    var args = event.args;
        //    if (args) {
        //        var item = args.item;
        //        if (item.label.charAt(0) == '@') {
        //            $('#UserId').val(0);
        //            $('#TeamId').val(item.value);
        //        }
        //        else {
        //            $('#UserId').val(item.value);
        //            $('#TeamId').val(0);
        //        }

        //    }
        //});

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
                url: '/System/GetTaskFormEdit'
            };

            var viewAdapter = new $.jqx.dataAdapter(view_source, {
                loadComplete: function (record) {
                    app_jqxform.loadDataForm("form-Form", record);
                    // $("form#form-Form [name=UserId]").val(record.UserId);

                    //var itemDate = app.toLocalDateString(record.ItemDate);
                    $("#ItemText").prop("readonly",slf.ReadOnly);

                    //$('#Duration').val(record['Duration']);
                    $("#ItemDate").val(app.toLocalDateString(record.ItemDate));
                    //$('#StartDate').val(app.toLocalDateString(record.StartDate));
                    //$("#DoneDate").val(app.toLocalDateTimeString(record.DoneDate));
                    //$("#form-Comment").hide();
                    $("#form-Title").text("פעולה: " + slf.ItemId);

                    if (record.StartDate || record.ItemTask > 0) {
                        $("#form-Start").hide();
                        //$("#form-Done-group").show();
                    }
                    else {
                        $("#form-Start").show();
                        //$("#form-Done-group").hide();

                    }
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
            $("#form-Form input[name=Task_Id]").val(this.TaskId);
            $("#form-Form input[name=UserId]").val(this.UserInfo.UserId);
            $("#form-Title").text("פעולה: " + "חדשה");
            //$('#form-Submit').text("התחל");
        }
    };

    app_task_form.prototype.display = function () {
        this.$element.show();
        $("#jqxgrid4-bar").hide();
    };

    app_task_form.prototype.doCancel = function () {
        $("#jqxgrid4-bar").show();
        app_task.triggerSubTaskCompleted('form');
    };

    app_task_form.prototype.doSubmit = function () {
        //e.preventDefault();
        var formData = $("#form-Form").serialize();
        var actionurl = $('#form-Form').attr('action');
        app_query.doFormSubmit("#form-Form", actionurl, null, function (data) {

            if (data.Status > 0) {
                app_task.triggerSubTaskCompleted('form', data);
            }
            else
                app_messenger.Post(data, 'error');
        });

        //var started = $("#StartDate").val();

        //if (started) {

        //    app_query.doFormSubmit("#form-Form", actionurl, null, function (data) {

        //        if (data.Status > 0) {
        //            app_task.triggerSubTaskCompleted('form', data);
        //        }
        //        else
        //            app_messenger.Post(data, 'error');
        //    });
        //}
        //else {

        //    app_query.doDataPost("/System/TaskFormStart", { 'id': this.ItemId }, function (data) {
        //        if (data.Status > 0) {
        //            $('#StartDate').val(app.toLocalDateTimeString(Date.now()));
        //            $("#jqxgrid4").jqxGrid('source').dataBind();
        //            //app_task.triggerSubTaskCompleted('form', data);
        //        }
        //    });
        //}

        ////var AssignedTo = $('#AssignedTo').val();
        //var validationResult = function (isValid) {
        //    if (isValid) {
        //        $.ajax({
        //            url: actionurl,
        //            type: 'post',
        //            dataType: 'json',
        //            data: $('#form-Form').serialize(),
        //            success: function (data) {
        //                if (data.Status > 0) {
        //                    app_task.triggerSubTaskCompleted('form', data);
        //                }
        //                else
        //                    app_messenger.Post(data, 'error');
        //            },
        //            error: function (jqXHR, status, error) {
        //                app_messenger.Post(error, 'error');
        //            }
        //        });
        //    }
        //}
        //$('#form-Form').jqxValidator('validate', validationResult);
    };

    app_task_form.prototype.doStart = function () {
        //e.preventDefault();

        app_query.doDataPost("/System/TopicTaskFormStart", { 'id': this.TaskId, 'itemId': this.ItemId }, function (data) {
            if (data.Status > 0) {
                $('#StartDate').val(app.toLocalDateString(Date.now()));
                //$("#form-Done-group").show();
                $("#form-Start").hide();
                $("#jqxgrid4").jqxGrid('source').dataBind();
                //app_task.triggerSubTaskCompleted('form', data);
            }
        });
    };

    return app_task_form;

}());

