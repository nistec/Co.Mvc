
//============================================================================================ app_task_form_template

var app_task_model = {

    taskBoard: function (tag) {
       
        var html = (function () {/*
         <div class="section-table">
         <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">משימות</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/TaskNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> משימה חדשה </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTasks">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול משימות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=task_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי משימות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=ticket_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי כרטיסים </span>
                        </a>
                    </li>
                </ul>
            </div>
            </div>
            <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">סוגיות</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/TopicNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגיה חדשה </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTopics">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול סוגיות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=topic_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי סוגיות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                    <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> * </span>
                    </li>
                </ul>
            </div>
            </div>
            <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">תיעוד</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/DocNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> מסמך חדש </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportDocs">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול מסמכים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=doc_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי מסמכים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                    <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> * </span>
                    </li>
                </ul>
            </div>
            </div>
            <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">דוחות</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/ReportSubTask">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> דוח פעילות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTasks">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול כרטיסים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                    <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> * </span>
                    </li>
                    <li class="padding-r10">
                    <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> * </span>
                    </li>
                </ul>
            </div></div></div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        $(tag).html(html);

    },
    taskBoard_old: function(tag) {

        var html = (function () {/*
         <div class="section-table">
         <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">משימות</div>
                <ul class="rtl">
                    <li class="padding-r10 pasive">
                        <a href="/System/ReminderNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> תזכורת </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/TaskNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> משימה חדשה </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTasks">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול משימות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportSubTask">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> דוח פעילות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=task_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי משימות </span>
                        </a>
                    </li>
                </ul>
            </div>
            </div>
            <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">סוגיות</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/TopicNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגיה חדשה </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTopics">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול סוגיות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=topic_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי סוגיות </span>
                        </a>
                    </li>
                </ul>
            </div>
            </div>
            <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">תיעוד</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/DocNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> מסמך חדש </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportDocs">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול מסמכים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=doc_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי מסמכים </span>
                        </a>
                    </li>
                </ul>
            </div>
            </div>
            <div class="section-cell">
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">כרטיסים</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/TicketNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> כרטיס חדש </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTasks">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול כרטיסים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=ticket_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי כרטיסים </span>
                        </a>
                    </li>
                </ul>
            </div></div></div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        $(tag).html(html);

    }

};

function window_doc_info (tagWindow, id) {

    var TagDiv = tagWindow;
    //var TagHide = tagHide;
    var DocId = id;
    var Option = "g";
    var PostLoaded = false;
    var Prefix = "window_doc_info-";
    var PrefixTag = "#" + Prefix;

    var init = function () {

        var html = (function () {/*
               <div id="jqxWidget" style="margin: 0 auto; display: block; direction: rtl">
                    <div id="exp-0" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-title">משימה</div>
                            <div class="expander-entry">
                                <form class="fcForm" id="form" method="post" action=".">
                                    <input type="hidden" id="DocId" name="DocId" value="0" />
                                    <input type="hidden" id="AccountId" name="AccountId" value="" />
                                    <input type="hidden" id="UserId" name="UserId" value="" />
                                    <input type="hidden" id="DocSubject" name="DocSubject" value="" />
                                    <input type="hidden" id="DocModel" name="DocModel" value="T" />

                                    <div id="tab-content" class="tab-content" dir="rtl">
                                        <div class="form-group">
                                            <div class="field">
                                                סוג מסמך:
                                            </div>
                                            <select id="Doc_Type" name="Doc_Type" data-type="select-loader" data-args="7"></select>
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                תיקייה:
                                            </div>
                                            <input type="text" id="Folder" name="Folder" class="text-mid"/>
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                סטאטוס :
                                            </div>
                                            <input type="hidden" id="DocStatus" name="DocStatus" />
                                            <label class="text-normal border white"><label id="DocStatus-display"></label>  -  <i id="DocStatus-color" class="fa fa-circle" style="font-size:16px;"></i></label>
                                        </div>
                                        <div class="form-group" id="DocBody-div">
                                            <div class="field">
                                                תיאור: <span><a id="DocBody-btn-view" href="#"><i class="fa fa-search-plus" style="font-size:16px"></i></a></span>
                                            </div>
                                            <textarea id="DocBody" name="DocBody" class="text-content" data-type="jqx-editor"></textarea>
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                נוצר ב:
                                            </div>
                                            <input type="text" id="CreatedDate" name="CreatedDate" class="text-mid label" readonly="readonly" data-type="datetime" />
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                תוקף:
                                            </div>
                                            <div id="DueDate" name="DueDate"></div>
                                        </div>

                                        <div class="form-group pasive">
                                            <div class="field">
                                                צבע :
                                            </div>
                                            <select id="ColorFlag" name="ColorFlag"></select>
                                        </div>
                                        <div id="jqxExp-1" class="panel-area rtl">
                                            <div class="panel-area-title">
                                                <a id="a-jqxExp-1" href="#">פרטים נוספים</a>
                                            </div>
                                            <div id="jqxExp-box" style="display:none">

                                                <div class="form-group">
                                                    <div class="field">
                                                        מועד התחלה:
                                                    </div>
                                                    <input id="StartedDate" name="StartedDate" type="text" readonly="readonly" class="text-mid label" data-type="datetime" />
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        מועד סיום:
                                                    </div>
                                                    <input id="EndedDate" name="EndedDate" type="text" readonly="readonly" class="text-mid label" data-type="datetime" />
                                                </div>
                                                <div id="Doc_Parent-group" class="form-group">
                                                    <div class="field">
                                                        מסמך אב:
                                                    </div>
                                                    <input type="text" id="Doc_Parent" name="Doc_Parent" readonly /><a id="Doc_Parent-link" href="#" title="הצג משימת אב"><i class="fa fa-search-plus"></i></a>
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        פרוייקט:
                                                    </div>
                                                    <input type="text" id="Project_Id" name="Project_Id" /><i class="fa fa-search"></i>
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        לקוח\מנוי:
                                                    </div>
                                                    <input type="text" id="ClientId" name="ClientId" /><i class="fa fa-search"></i>
                                                </div>
                                                <div class="form-group rtl">
                                                    <div class="field">
                                                        תגיות:
                                                    </div>
                                                    <div id="Tags" name="Tags"></div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        הרשאות :
                                                    </div>
                                                    <select id="PermsType" name="PermsType"></select>
                                                </div>
                                                <div class="form-group" style="display:none">
                                                    <div class="field">
                                                        שיתוף:
                                                    </div>
                                                    <div id="AssignTo"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div id="exp-1" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-1"><a id="a-exp-1" href="#">הערות</a></div>
                            <div class="expander-entry pasive" id="entry-exp-1">
                                <div class="grid-wrap rtl">
                                     <div id="jqxgrid1" style="position:relative;z-index:1;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="exp-4" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-4"><a id="a-exp-4" href="#">סיכומים</a></div>
                            <div class="expander-entry pasive" id="entry-exp-4">
                                <div class="grid-wrap rtl">
                                    <div id="jqxgrid4" style="position:relative;z-index:1;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="exp-5" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-5"><a id="a-exp-5" href="#">קבצים</a></div>
                            <div class="expander-entry1 pasive" id="entry-exp-5">
                                <div id="doc-files"></div>
                            </div>
                        </div>
                    </div>
                <div style="height: 5px"></div>
                <div class="panel-area">
                    <!--<input id="fcCancel" class="btn-default btn7 w-60" type="button" value="x" />-->
                    <a id="Doc_Child-link" style="margin-right:10px" href="#" title="יצירת תת משימה"><i class="fa fa-share-alt"></i></a>
                </div>
            </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        //if (slf.Option == "a") {
        //    html = html.replace('form-group active', 'form-group pasive')
        //}
        html = html.replace(/id=\"/g, "id=\"" + Prefix);
        //var container = $(html);

        //$(TagDiv).empty();
        //$(TagDiv).append(container);

        var width = $(TagDiv).parent().width();

        app_panel.appendPanelSwitch(TagDiv, html, width, 300, true, "מסמך: " + DocId);

        preLoad();

        app_query.doDataAdapter("/System/GetDocInfo", { 'id': DocId }, function (data) {

            loadControls(data);
        });

        //$(TagHide).show();
        //$(TagDiv).show();

        //return this;
    };

    var sectionSettings=function(id) {

        switch (id) {
            case 1:
                if (DocId === 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המסמך הנוכחי לפני הוספת הערות");
                    return false;
                }
                if ($("#" + Prefix + "jqxgrid1").is(':empty'))
                    app_repeater.doc_comments_adapter("#" + Prefix + "jqxgrid1", DocId);
                break;
            case 4:
                if (DocId === 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המסמך הנוכחי לפני הוספת פעולות");
                    return false;
                }

                if ($("#" + Prefix + "jqxgrid4").is(':empty'))
                    app_repeater.doc_form_adapter("#" + Prefix + "jqxgrid4", DocId);
 
                break;
            case 5:
                if (DocId === 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המסמך הנוכחי לפני הוספת קבצים");
                    return false;
                }

                if ($("#" + Prefix + "doc-files").is(':empty'))
                    app_repeater.media_files_adapter("#" + Prefix + "doc-files", DocId, "d");

                //if (this.uploader === undefined || this.uploader == null) {
                //    this.uploader = new app_media_uploader("#doc-files");
                //    this.uploader.init(this.DocId, 'd', !this.IsEditable);
                //    this.uploader.show();
                //}

                //if ($("#iframe-files").attr('src') === undefined)
                //    var op = this.Model.Option;
                //app_iframe.attachIframe('iframe-files', '/Media/_MediaFiles?refid=' + this.DocId + '&refType=t&op=' + op, '100%', '350px', true);

                break;
        }
    }

    var preLoad = function () {

        app_tasks.setPermsType("#" + Prefix + "PermsType");

        app_features.editorTag("#" + Prefix + "DocBody", false);

        $("#" + Prefix + "a-exp-1").on('click', function (e) {
            sectionSettings(1);
            $("#" + Prefix + "entry-exp-1").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-4").on('click', function (e) {
            sectionSettings(4);
            $("#" + Prefix + "entry-exp-4").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-5").on('click', function (e) {
            sectionSettings(5);
            $("#" + Prefix + "entry-exp-5").toggle();
            return false;
        });
    }

    var loadControls = function (record) {

        console.log('controls');

        //var slf = app_system_model;
        
        $("#" + Prefix + "a-jqxExp-1").on('click', function (e) {
            if (!PostLoaded) {
               postLoad(record);
            }
            $("#" + Prefix + "jqxExp-box").slideToggle();
            return false;
        });

        $("#" + Prefix + "Doc_Parent").val(record.Doc_Parent);
        if (record.Doc_Parent > 0) {
            $("#" + Prefix + "Doc_Parent-group").show();
            $("#" + Prefix + "Doc_Parent-link").click(function () {
                app.redirectTo('/System/DocInfo?id=' + record.Doc_Parent);
            });
        }
        else {
            $("#" + Prefix + "Doc_Parent-group").hide();
        }

        $("#" + Prefix + "AccountId").val(record.AccountId);

        if (record.UserId > 0)
            app_jqx_combo_async.userInputAdapter("#" + Prefix + "UserId", record.UserId);

        if (record.Comments === 0)
            $("#" + Prefix + "exp-1").hide();
        if (record.Items === 0)
            $("#" + Prefix + "exp-4").hide();
        if (record.Files === 0)
            $("#" + Prefix + "exp-5").hide();


        $("#" + Prefix + "DueDate").jqxDateTimeInput({ showCalendarButton: false, readonly: true, width: '150px', rtl: true });


        app_form.loadDataForm(Prefix + "form", record, ["DocStatus"], true);//, , "Folder", "Project_Id", "ClientId", "Tags", "AssignTo"

        $("#" + Prefix + "hTitle-text").text(this.Title + ": " + record.DocSubject);
        $("#" + Prefix + "hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

        $("#" + Prefix + "DocBody").css('text-align', app_style.langAlign(record.Lang))

        app_tasks.enumTypes_load("#" + Prefix + "Doc_Type", "D", record.Doc_Type);

        app_tasks.setTaskStatus("#" + Prefix + "DocStatus", record.DocStatus);
    }

    var postLoad = function (record) {

        if (this.PostLoaded)
            return;

        app_jqx_combo_async.lookupInputAdapter("#" + Prefix + "ClientId", 'lu_Members', record.ClientId, function () {
        });
        app_jqx_combo_async.systemLookupInputAdapter("#" + Prefix + "Project_Id", 'lu_Project', record.Project_Id, function () {
        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#" + Prefix + "Tags", '/System/GetTagsList', null, 225, 0, true, record.Tags, function () {
        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#" + Prefix + "AssignTo", '/System/GetUsersList', null, 225, 0, null, record.AssignTo, function () {
            $("#" + Prefix + "AssignTo").jqxComboBox({ disabled: true });
        });

        PostLoaded = true;

        $("#" + Prefix + "Tags").jqxComboBox({ disabled: true });
        $("#" + Prefix + "AssignTo").jqxComboBox({ disabled: true });
        app.disableSelect("#" + Prefix + "ShareType");
    }

    init();
}

function window_task_info(tagWindow, id, taskModel) {

    var TagDiv = tagWindow;
    var TaskModel = taskModel || 'T';
    //var TagHide = tagHide;
    var TaskId = id;
    var Option = "g";
    var PostLoaded = false;
    var Prefix = "window_task_info-";
    var PrefixTag = "#" + Prefix;

    var init = function () {

        var html = (function () {/*
        <div id="jqxWidget" style="margin: 0 auto; display: block; direction: rtl">
                <div id="exp-0" class="jcxtab-panel">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-title">משימה</div>
                        <div class="expander-entry" id="entry-exp-0">
                            <form class="fcForm" id="form" method="post" action="/System/UpdateTask">
                                <input type="hidden" id="TaskId" name="TaskId" value="0" />
                                <input type="hidden" id="AccountId" name="AccountId" value="" />
                                <input type="hidden" id="TeamId" name="TeamId" value="" />
                                <input type="hidden" id="TaskSubject" name="TaskSubject" value="" />
                                <input type="hidden" id="TaskModel" name="TaskModel" value="T" />
                                <input type="hidden" id="UserId" name="UserId" value="0" />
                                <div id="tab-content" class="tab-content" dir="rtl">
                                    <div class="form-group">
                                        <div class="field">
                                            סוג משימה:
                                        </div>
                                        <select id="Task_Type" name="Task_Type" data-type="select-loader" data-args="4"></select>
                                    </div>
                                    <div class="form-group">
                                        <div class="field">
                                            סטאטוס :
                                        </div>
                                        <input type="hidden" id="TaskStatus" name="TaskStatus" />
                                        <label class="text-normal border white"><label id="TaskStatus-display"></label>  -  <i id="TaskStatus-color" class="fa fa-circle" style="font-size:16px;"></i></label>
                                    </div>
                                    <div class="form-group" id="TaskBody-div">
                                        <div class="field">
                                            תיאור: <span><a id="TaskBody-btn-view" href="#"><i class="fa fa-search-plus" style="font-size:16px"></i></a></span>
                                        </div>
                                        <textarea id="TaskBody" name="TaskBody" class="text-content" data-type="jqx-editor"></textarea>
                                    </div>
                                    <div class="form-group">
                                        <div class="field">
                                            נוצר ב:
                                        </div>
                                        <input type="text" id="CreatedDate" name="CreatedDate" class="text-mid label" readonly="readonly" data-type="datetime" />
                                    </div>
                                    <div class="form-group">
                                        <div class="field">
                                            תאריך לביצוע:
                                        </div>
                                        <div id="DueDate" name="DueDate"></div>
                                    </div>
                                    <div class="form-group">
                                        <div class="field">
                                            משימה עבור:
                                        </div>
                                        <input type="text" id="DisplayName" readonly="readonly" data-field="DisplayName" />
                                    </div>
                                    <div class="form-group pasive">
                                        <div class="field">
                                            צבע :
                                        </div>
                                        <select id="ColorFlag" name="ColorFlag"></select>
                                    </div>
                                    <div id="jqxExp-1" class="panel-area rtl">

                                        <div class="panel-area-title">
                                            <a id="a-jqxExp-1" href="#">פרטים נוספים</a>
                                        </div>
                                        <div id="jqxExp-box" style="display:none">

                                            <div class="form-group">
                                                <div class="field">
                                                    מועד התחלה:
                                                </div>
                                                <input id="StartedDate" name="StartedDate" type="text" readonly="readonly" class="text-mid label" data-type="datetime" />
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    מועד סיום:
                                                </div>
                                                <input id="EndedDate" name="EndedDate" type="text" readonly="readonly" class="text-mid label" data-type="datetime" />
                                            </div>
                                            <div id="Task_Parent-group" class="form-group">
                                                <div class="field">
                                                    משימת אב:
                                                </div>
                                                <input type="text" id="Task_Parent" name="Task_Parent" readonly /><a id="Task_Parent-link" href="#" title="הצג משימת אב"><i class="fa fa-search-plus"></i></a>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    פרוייקט:
                                                </div>
                                                <input type="text" id="Project_Id" name="Project_Id" /><i class="fa fa-search"></i>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    לקוח\מנוי:
                                                </div>
                                                <input type="text" id="ClientId" name="ClientId" /><i class="fa fa-search"></i>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    שיתוף:
                                                </div>
                                                <select id="ShareType" name="ShareType"></select>
                                                <div style="height:5px"></div>
                                                <div id="AssignTo" name="AssignTo"></div>
                                            </div>
                                            <div class="form-group rtl">
                                                <div class="field">
                                                    תגיות:
                                                </div>
                                                <div id="Tags" name="Tags"></div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
                <div id="exp-1" class="jcxtab-panel">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-1"><a id="a-exp-1" href="#">הערות</a></div>
                        <div class="expander-entry pasive" id="entry-exp-1">
                            <div class="grid-wrap rtl">
                                <div id="jqxgrid1" style="position:relative;z-index:1;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="exp-2" class="jcxtab-panel">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-2"><a id="a-exp-2" href="#">העברות</a></div>
                        <div class="expander-entry pasive" id="entry-exp-2">
                            <div class="grid-wrap rtl">
                                 <div id="jqxgrid2" style="position:relative;z-index:1;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="exp-3" class="jcxtab-panel">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-3"><a id="a-exp-3" href="#">מד-זמן</a></div>
                        <div class="expander-entry pasive" id="entry-exp-3">
                            <div class="grid-wrap rtl">
                                <div id="jqxgrid3" style="position:relative;z-index:1;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="exp-4" class="jcxtab-panel">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-4"><a id="a-exp-4" href="#">פעולות</a></div>
                        <div class="expander-entry pasive" id="entry-exp-4">
                            <div class="grid-wrap rtl">
                                <div id="jqxgrid4" style="position:relative;z-index:1;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="exp-5" class="jcxtab-panel">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-5"><a id="a-exp-5" href="#">קבצים</a></div>
                        <div class="expander-entry1 pasive" id="entry-exp-5">
                            <div id="task-files"></div>
                        </div>
                    </div>
                </div>

                <div style="height: 5px"></div>
                <div class="panel-area">
                    <!--<input id="fcCancel" class="btn-default btn7 w-60" type="button" value="x" />-->
                    <a id="Task_Child-link" style="margin-right:10px" href="#" title="יצירת תת משימה"><i class="fa fa-share-alt"></i></a>
                </div>
            </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        html = html.replace(/id=\"/g, "id=\"" + Prefix);

        var width = $(TagDiv).parent().width();

        app_panel.appendPanelSwitch(TagDiv, html, width, 300, true, "משימה: " + TaskId);

        preLoad();

        var url = '/System/GetTaskInfo';

        switch (TaskModel) {
            case 'N':
            case 'T':
                url='/System/GetTaskInfo';
                break;
            case 'E':
                url ='/System/GetTicketInfo';
                break;
            //case 'R':
            //    app.redirectTo('/System/ReminderInfo?id=' + id);
            //    break;
            //case 'C':
            //    break;
        }
        app_query.doDataAdapter(url, { 'id': TaskId }, function (data) {

            loadControls(data);
        });
    };

    var sectionSettings=function(id) {

        var slf = this;
        switch (id) {
            case 1:
                if (TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                    return false;
                }
                if ($("#" + Prefix + "jqxgrid1").is(':empty'))
                    app_repeater.task_comments_adapter("#" + Prefix + "jqxgrid1", TaskId);
                  break;
            case 2:
                if ($("#" + Prefix + "jqxgrid2").is(':empty'))
                    app_repeater.task_assign_adapter("#" + Prefix + "jqxgrid2", TaskId);
                break;
            case 3:
                if ($("#" + Prefix + "jqxgrid3").is(':empty'))
                    app_repeater.task_timer_adapter("#" + Prefix + "jqxgrid3", TaskId);
                break;
            case 4:
                if (TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                    return false;
                }
                    if (TaskModel == 'P') {

                        if ($("#" + Prefix + "jqxgrid4").is(':empty'))
                            app_repeater.topic_form_adapter("#" + Prefix + "jqxgrid4", TaskId);
                    }
                    else {
                        if ($("#" + Prefix + "jqxgrid4").is(':empty'))
                            app_repeater.task_form_adapter("#" + Prefix + "jqxgrid4", TaskId);
                    }
                break;
            case 5:
                if (TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                    return false;
                }
                if ($("#" + Prefix + "task-files").is(':empty'))
                    app_repeater.media_files_adapter("#" + Prefix + "task-files", TaskId, "t");

                break;
        }
    }

    var preLoad = function () {

        app_tasks.setShareType("#" + Prefix + "ShareType");

        app_features.editorTag("#" + Prefix + "TaskBody", false);

        $("#" + Prefix + "a-exp-1").on('click', function (e) {
            sectionSettings(1);
            $("#" + Prefix + "entry-exp-1").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-2").on('click', function (e) {
            sectionSettings(2);
            $("#" + Prefix + "entry-exp-2").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-3").on('click', function (e) {
            sectionSettings(3);
            $("#" + Prefix + "entry-exp-3").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-4").on('click', function (e) {
            sectionSettings(4);
            $("#" + Prefix + "entry-exp-4").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-5").on('click', function (e) {
            sectionSettings(5);
            $("#" + Prefix + "entry-exp-5").toggle();
            return false;
        });
    }

    var loadControls = function (record) {

        console.log('controls');

        //var slf = app_system_model;

        $("#" + Prefix + "a-jqxExp-1").on('click', function (e) {
            if (!PostLoaded) {
                postLoad(record);
            }
            $("#" + Prefix + "jqxExp-box").slideToggle();
            return false;
        });

        $("#" + Prefix + "Task_Parent").val(record.Doc_Parent);
        if (record.Task_Parent > 0) {
            $("#" + Prefix + "Task_Parent-group").show();
            $("#" + Prefix + "Task_Parent-link").click(function () {
                app.redirectTo('/System/TaskInfo?id=' + record.Task_Parent);
            });
        }
        else {
            $("#" + Prefix + "Task_Parent-group").hide();
        }

        $("#" + Prefix + "AccountId").val(record.AccountId);

        if (record.UserId > 0)
            app_jqx_combo_async.userInputAdapter("#" + Prefix + "UserId", record.UserId);

        if (record.Comments === 0)
            $("#" + Prefix + "exp-1").hide();
        if (record.Assigns === 0)
            $("#" + Prefix + "exp-2").hide();
        if (record.Timers === 0)
            $("#" + Prefix + "exp-3").hide();
        if (record.Items === 0)
            $("#" + Prefix + "exp-4").hide();
        if (record.Files === 0)
            $("#" + Prefix + "exp-5").hide();


        $("#" + Prefix + "DueDate").jqxDateTimeInput({ showCalendarButton: false, readonly: true, width: '150px', rtl: true });


        app_form.loadDataForm(Prefix + "form", record, ["TaskStatus"], true);//, , "Folder", "Project_Id", "ClientId", "Tags", "AssignTo"

        $("#" + Prefix + "hTitle-text").text(this.Title + ": " + record.TaskSubject);
        $("#" + Prefix + "hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

        $("#" + Prefix + "TaskBody").css('text-align', app_style.langAlign(record.Lang))

        app_tasks.enumTypes_load("#" + Prefix + "Task_Type", "T", record.Task_Type);

        app_tasks.setTaskStatus("#" + Prefix + "TaskStatus", record.TaskStatus);
    }

    var postLoad = function (record) {

        if (this.PostLoaded)
            return;

        app_jqx_combo_async.lookupInputAdapter("#" + Prefix + "ClientId", 'lu_Members', record.ClientId, function () {
        });
        app_jqx_combo_async.systemLookupInputAdapter("#" + Prefix + "Project_Id", 'lu_Project', record.Project_Id, function () {
        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#" + Prefix + "Tags", '/System/GetTagsList', null, 225, 0, true, record.Tags, function () {
        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#" + Prefix + "AssignTo", '/System/GetUsersList', null, 225, 0, null, record.AssignTo, function () {
            $("#" + Prefix + "AssignTo").jqxComboBox({ disabled: true });
        });

        PostLoaded = true;

        $("#" + Prefix + "Tags").jqxComboBox({ disabled: true });
        $("#" + Prefix + "AssignTo").jqxComboBox({ disabled: true });
        app.disableSelect("#" + Prefix + "ShareType");
    }

    init();
}

function window_task_edit(tagWindow, id, taskModel) {

    var TagDiv = tagWindow;
    var TaskModel = taskModel || 'T';
    //var TagHide = tagHide;
    var TaskId = id;
    var Option = "g";
    var PostLoaded = false;
    var Prefix = "window_task_edit-";
    var PrefixTag = "#" + Prefix;

    var init = function () {

        var html = (function () {/*
   <div id="wiz-container" class="container rtl">

        <div id="hTitle" class="panel-page-header rtl">
            <span id="hTitle-text" style="margin:10px">משימה</span>
        </div>
        <div style="height:5px"></div>
        <div id="wiz-1" class="wiz-tab active">
            <div id="jqxWidget" class="box-hide">

                <div id="accordion" class="jcx-tabs rtl tab-content">
                    <ul id="tab-query" class="nav nav-tabs1">
                        <li class="active"><a id="hxp-0" href="#exp-0" data-toggle="tab">משימה</a></li>
                        <li><a id="hxp-1" href="#exp-1" data-toggle="tab">הערות</a></li>
                        <li><a id="hxp-2" href="#exp-2" data-toggle="tab">העברות</a></li>
                        <li><a id="hxp-3" href="#exp-3" data-toggle="tab">מד-זמן</a></li>
                        <li><a id="hxp-4" href="#exp-4" data-toggle="tab">פעולות</a></li>
                        <li><a id="hxp-5" href="#exp-5" data-toggle="tab">קבצים</a></li>
                    </ul>

                    <div id="exp-0" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-title">משימה</div>
                            <div class="expander-entry">
                                <form class="fcForm" id="fcForm" method="post" action="/System/UpdateTask">
                                    <input type="hidden" id="TaskId" name="TaskId" value="0" />
                                    <input type="hidden" id="AccountId" name="AccountId" value="" />
                                    <input type="hidden" id="UserId" name="UserId" value="" />
                                    <input type="hidden" id="TeamId" name="TeamId" value="" />
                                    <input type="hidden" id="TaskSubject" name="TaskSubject" value="" />
                                    <input type="hidden" id="TaskModel" name="TaskModel" value="T" />

                                    <div id="tab-content" class="tab-content" dir="rtl">
                                        <div class="form-group">
                                            <div class="field">
                                                סוג משימה:
                                            </div>
                                            <select id="Task_Type" name="Task_Type" data-type="select-loader" data-args="4"></select>
                                        </div>

                                        <div class="form-group">
                                            <div class="field">
                                                סטאטוס :
                                            </div>
                                            <input type="hidden" id="TaskStatus" name="TaskStatus" />
                                            <label class="text-normal border white"><label id="TaskStatus-display"></label>  -  <i id="TaskStatus-color" class="fa fa-circle" style="font-size:16px;"></i></label>
                                        </div>
                                        <div class="form-group" id="TaskBody-div">
                                            <div class="field">
                                                תיאור: <span><a id="TaskBody-btn-view" href="#"><i class="fa fa-search-plus" style="font-size:16px"></i></a></span>
                                            </div>
                                            <textarea id="TaskBody" name="TaskBody" class="text-content" data-type="jqx-editor"></textarea>
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                נוצר ב:
                                            </div>
                                            <input type="text" id="CreatedDate" name="CreatedDate" class="text-mid label" readonly="readonly" data-type="datetime" />
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                תאריך לביצוע:
                                            </div>
                                            <div id="DueDate" name="DueDate"></div>
                                        </div>
                                        <div class="form-group">
                                            <div class="field">
                                                צבע :
                                            </div>
                                            <select id="ColorFlag" name="ColorFlag"></select>
                                        </div>
                                        <div id="jqxExp-1" class="panel-area rtl">

                                            <div class="panel-area-title">
                                                <a id="a-jqxExp-1" href="#">פרטים נוספים</a>
                                            </div>

                                            <div id="jqxExp-box" style="display:none">

                                                <div class="form-group">
                                                    <div class="field">
                                                        מועד התחלה:
                                                    </div>
                                                    <input id="StartedDate" name="StartedDate" type="text" readonly="readonly" class="text-mid label" data-type="datetime" />
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        מועד סיום:
                                                    </div>
                                                    <input id="EndedDate" name="EndedDate" type="text" readonly="readonly" class="text-mid label" data-type="datetime" />
                                                </div>
                                                <div id="Task_Parent-group" class="form-group">
                                                    <div class="field">
                                                        משימת אב:
                                                    </div>
                                                    <input type="text" id="Task_Parent" name="Task_Parent" readonly /><a id="Task_Parent-link" href="#" title="הצג משימת אב"><i class="fa fa-search-plus"></i></a>
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        פרוייקט:
                                                    </div>
                                                    <input type="text" id="Project_Id" name="Project_Id" /><i class="fa fa-search"></i>
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        לקוח\מנוי:
                                                    </div>
                                                    <input type="text" id="ClientId" name="ClientId" /><i class="fa fa-search"></i>
                                                </div>
                                                <div class="form-group">
                                                    <div class="field">
                                                        שיתוף:
                                                    </div>
                                                    <select id="ShareType" name="ShareType" class="style-select"></select>
                                                    <div style="height:5px"></div>
                                                    <div id="AssignTo" name="AssignTo"></div>
                                                </div>
                                                <div class="form-group rtl">
                                                    <div class="field">
                                                        תגיות:
                                                    </div>
                                                    <div id="Tags" name="Tags"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div id="exp-1" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-1">הערות</div>
                            <div class="expander-entry">
                                <div class="grid-wrap rtl">
                                    <div id="jqxgrid1-bar" class="@(Model.Option == "g" ? "item-pasive" : null)">
                                        <a id="jqxgrid1-add" href="#" class="btn-default btnIcon"><i class="fa fa-plus-square-o"></i>הוספה</a>
                                        <a id="jqxgrid1-edit" href="#" class="btn-default btnIcon"><i class="fa fa-edit"></i>עריכה</a>
                                        <a id="jqxgrid1-remove" href="#" class="btn-default btnIcon"><i class="fa fa-remove"></i>הסרה</a>
                                        <a id="jqxgrid1-refresh" href="#" class="btn-default btnIcon"><i class="fa fa-refresh"></i>רענון</a>
                                    </div>
                                    <div id="jqxgrid1-window"></div>
                                    <div id="jqxgrid1" style="position:relative;z-index:1;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="exp-2" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-2">העברות</div>
                            <div class="expander-entry">
                                <div class="grid-wrap rtl">
                                    <div id="jqxgrid2-bar" class="@(Model.Option == "g" ? "item-pasive" : null)">
                                        <a id="jqxgrid2-add" href="#" class="btn-default btnIcon"><i class="fa fa-plus-square-o"></i>הוספה</a>
                                        <a id="jqxgrid2-refresh" href="#" class="btn-default btnIcon"><i class="fa fa-refresh"></i>רענון</a>
                                    </div>
                                    <div id="jqxgrid2-window"></div>
                                    <div id="jqxgrid2" style="position:relative;z-index:1;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="exp-3" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-3">מד-זמן</div>
                            <div class="expander-entry">
                                <div class="grid-wrap rtl">
                                    <div id="jqxgrid3-bar" class="@(Model.Option == "g" ? "item-pasive" : null)">
                                        <a id="jqxgrid3-add" href="#" class="btn-default btnIcon"><i class="fa fa-plus-square-o"></i>הוספה</a>
                                        <a id="jqxgrid3-edit" href="#" class="btn-default btnIcon"><i class="fa fa-edit"></i>עריכה</a>
                                        <a id="jqxgrid3-remove" href="#" class="btn-default btnIcon"><i class="fa fa-remove"></i>הסרה</a>
                                        <a id="jqxgrid3-refresh" href="#" class="btn-default btnIcon"><i class="fa fa-refresh"></i>רענון</a>
                                    </div>
                                    <div id="jqxgrid3-window"></div>
                                    <div id="jqxgrid3" style="position:relative;z-index:1;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="exp-4" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-4">פעולות</div>
                            <div class="expander-entry">
                                <div class="grid-wrap rtl">
                                    <div id="jqxgrid4-bar" class="@(Model.Option == "g" ? "item-pasive" : null)">
                                        <a id="jqxgrid4-add" href="#" class="btn-default btnIcon"><i class="fa fa-plus-square-o"></i>הוספה</a>
                                        <a id="jqxgrid4-edit" href="#" class="btn-default btnIcon"><i class="fa fa-edit"></i>עריכה</a>
                                        <a id="jqxgrid4-remove" href="#" class="btn-default btnIcon"><i class="fa fa-remove"></i>הסרה</a>
                                        <a id="jqxgrid4-refresh" href="#" class="btn-default btnIcon"><i class="fa fa-refresh"></i>רענון</a>
                                    </div>
                                    <div id="jqxgrid4-window"></div>
                                    <div id="jqxgrid4" style="position:relative;z-index:1;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="exp-5" class="jcxtab-panel">
                        <div class="panel-area">
                            <div class="panel-area-title" id="hxp-5">קבצים</div>
                            <div class="expander-entry1">
                                <div id="task-files"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="height: 5px"></div>
                <div class="@(ViewBag.UserInfo.UserId) panel-area">
                    <input id="fcSubmit" class="btn-default btn7 w-60" type="button" value="עדכון" />
                    <input id="fcEnd" class="btn-default btn7 w-60" type="button" value="סיום" />
                    <input id="fcCancel" class="btn-default btn7 w-60" type="button" value="x" />
                    <a id="Task_Child-link" style="margin-right:10px" href="#" title="יצירת תת משימה"><i class="fa fa-share-alt"></i></a>
                </div>

            </div>
        </div>
        <div id="wiz-2" class="wiz-tab">
            <h3 id="wiz-2-title" class="rtl">
                עריכת משימה
            </h3>
            <div id="divPartial2" class="wiz-partial"></div>
            <div class="rtl">
                <a id="task-item-update" class="btn-default btn7 w-60" href="#">עדכון</a>
                <a id="task-item-cancel" class="btn-default btn7 w-60" href="#">ביטול</a>
            </div>
        </div>
        <div id="wiz-3" class="wiz-tab">
            <div class="rtl">
                <a class="btn-default btn7 w-60" href="javascript:wizard.displayStep(1);">חזרה</a>
            </div>
            <div id="divPartial3"></div>
        </div>
        <div id="wiz-4" class="wiz-tab">
            <h3 class="rtl">מעקב זמנים</h3>
            <div id="divPartial4"></div>
        </div>
        <div id="wiz-5" class="wiz-tab">
            <h3 class="rtl">הערות-עריכה</h3>
            <div id="divPartial5"></div>
        </div>
        <div id="wiz-6" class="wiz-tab">
            <h3 class="rtl">העברה</h3>
            <div id="divPartial6"></div>
        </div>
        <div id="wiz-7" class="wiz-tab">
            <h3 class="rtl">מעקב זמנים - עריכה</h3>
            <div id="divPartial7"></div>
        </div>
    </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        html = html.replace(/id=\"/g, "id=\"" + Prefix);

        var width = $(TagDiv).parent().width();

        app_panel.appendPanelSwitch(TagDiv, html, width, 300, true, "משימה: " + TaskId);

        preLoad();

        var url = '/System/GetTaskInfo';

        switch (TaskModel) {
            case 'N':
            case 'T':
                url = '/System/GetTaskInfo';
                break;
            case 'E':
                url = '/System/GetTicketInfo';
                break;
            //case 'R':
            //    app.redirectTo('/System/ReminderInfo?id=' + id);
            //    break;
            //case 'C':
            //    break;
        }
        app_query.doDataAdapter(url, { 'id': TaskId }, function (data) {

            loadControls(data);
        });
    };

    var sectionSettings = function (id) {

        var slf = this;
        switch (id) {
            case 1:
                if (TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת הערות");
                    return false;
                }
                if ($("#" + Prefix + "jqxgrid1").is(':empty'))
                    app_repeater.task_comments_adapter("#" + Prefix + "jqxgrid1", TaskId);
                break;
            case 2:
                if ($("#" + Prefix + "jqxgrid2").is(':empty'))
                    app_repeater.task_assign_adapter("#" + Prefix + "jqxgrid2", TaskId);
                break;
            case 3:
                if ($("#" + Prefix + "jqxgrid3").is(':empty'))
                    app_repeater.task_timer_adapter("#" + Prefix + "jqxgrid3", TaskId);
                break;
            case 4:
                if (TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת פעולות");
                    return false;
                }
                if (TaskModel == 'P') {

                    if ($("#" + Prefix + "jqxgrid4").is(':empty'))
                        app_repeater.topic_form_adapter("#" + Prefix + "jqxgrid4", TaskId);
                }
                else {
                    if ($("#" + Prefix + "jqxgrid4").is(':empty'))
                        app_repeater.task_form_adapter("#" + Prefix + "jqxgrid4", TaskId);
                }
                break;
            case 5:
                if (TaskId == 0) {
                    //event.preventDefault();
                    app_dialog.alert("יש לשמור את המשימה הנוכחית לפני הוספת קבצים");
                    return false;
                }
                if ($("#" + Prefix + "task-files").is(':empty'))
                    app_repeater.media_files_adapter("#" + Prefix + "task-files", TaskId, "t");

                break;
        }
    }

    var preLoad = function () {

        app_tasks.setShareType("#" + Prefix + "ShareType");

        app_features.editorTag("#" + Prefix + "TaskBody", false);

        $("#" + Prefix + "a-exp-1").on('click', function (e) {
            sectionSettings(1);
            $("#" + Prefix + "entry-exp-1").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-2").on('click', function (e) {
            sectionSettings(2);
            $("#" + Prefix + "entry-exp-2").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-3").on('click', function (e) {
            sectionSettings(3);
            $("#" + Prefix + "entry-exp-3").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-4").on('click', function (e) {
            sectionSettings(4);
            $("#" + Prefix + "entry-exp-4").toggle();
            return false;
        });
        $("#" + Prefix + "a-exp-5").on('click', function (e) {
            sectionSettings(5);
            $("#" + Prefix + "entry-exp-5").toggle();
            return false;
        });
    }

    var loadControls = function (record) {

        console.log('controls');

        //var slf = app_system_model;

        $("#" + Prefix + "a-jqxExp-1").on('click', function (e) {
            if (!PostLoaded) {
                postLoad(record);
            }
            $("#" + Prefix + "jqxExp-box").slideToggle();
            return false;
        });

        $("#" + Prefix + "Task_Parent").val(record.Doc_Parent);
        if (record.Task_Parent > 0) {
            $("#" + Prefix + "Task_Parent-group").show();
            $("#" + Prefix + "Task_Parent-link").click(function () {
                app.redirectTo('/System/TaskInfo?id=' + record.Task_Parent);
            });
        }
        else {
            $("#" + Prefix + "Task_Parent-group").hide();
        }

        $("#" + Prefix + "AccountId").val(record.AccountId);

        if (record.UserId > 0)
            app_jqx_combo_async.userInputAdapter("#" + Prefix + "UserId", record.UserId);

        if (record.Comments === 0)
            $("#" + Prefix + "exp-1").hide();
        if (record.Assigns === 0)
            $("#" + Prefix + "exp-2").hide();
        if (record.Timers === 0)
            $("#" + Prefix + "exp-3").hide();
        if (record.Items === 0)
            $("#" + Prefix + "exp-4").hide();
        if (record.Files === 0)
            $("#" + Prefix + "exp-5").hide();


        $("#" + Prefix + "DueDate").jqxDateTimeInput({ showCalendarButton: false, readonly: true, width: '150px', rtl: true });


        app_form.loadDataForm(Prefix + "form", record, ["TaskStatus"], true);//, , "Folder", "Project_Id", "ClientId", "Tags", "AssignTo"

        $("#" + Prefix + "hTitle-text").text(this.Title + ": " + record.TaskSubject);
        $("#" + Prefix + "hTitle").css("background-color", (record.ColorFlag || config.defaultColor));

        $("#" + Prefix + "TaskBody").css('text-align', app_style.langAlign(record.Lang))

        app_tasks.enumTypes_load("#" + Prefix + "Task_Type", "T", record.Task_Type);

        app_tasks.setTaskStatus("#" + Prefix + "TaskStatus", record.TaskStatus);
    }

    var postLoad = function (record) {

        if (this.PostLoaded)
            return;

        app_jqx_combo_async.lookupInputAdapter("#" + Prefix + "ClientId", 'lu_Members', record.ClientId, function () {
        });
        app_jqx_combo_async.systemLookupInputAdapter("#" + Prefix + "Project_Id", 'lu_Project', record.Project_Id, function () {
        });
        app_jqx_adapter.createComboDisplayAsync("Tag", "#" + Prefix + "Tags", '/System/GetTagsList', null, 225, 0, true, record.Tags, function () {
        });
        app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#" + Prefix + "AssignTo", '/System/GetUsersList', null, 225, 0, null, record.AssignTo, function () {
            $("#" + Prefix + "AssignTo").jqxComboBox({ disabled: true });
        });

        PostLoaded = true;

        $("#" + Prefix + "Tags").jqxComboBox({ disabled: true });
        $("#" + Prefix + "AssignTo").jqxComboBox({ disabled: true });
        app.disableSelect("#" + Prefix + "ShareType");
    }

    var doSubmit=function(act) {
        //e.preventDefault();
        var actionurl = $("#" + Prefix + 'fcForm').attr('action');
        var status = this.TaskStatus;// app_jqx.getInputAutoValue("#TaskStatus", 1);
        var isnew = this.IsNew;

        var afterSubmit = function (slf, data) {

            if (isnew) {
                slf.TaskId = data.OutputId;
                $("#" + Prefix + "TaskId").val(data.OutputId);
                slf.tabSettings();
                $("#" + Prefix + "fcSubmit").val("עדכון");
                slf.IsNew = slf.TaskId == 0;
                app_messenger.Notify(data, 'info');
            }
            else {
                app_messenger.Notify(data, 'info', app_task_base.getReferrer());
            }

            if (act == 'plus') {
                app.refresh();
            }
        }

        var RunSubmit = function (slf, status, actionurl) {

            //app_jqx.setInputAutoValue("#TaskStatus", status);
            app_tasks.setTaskStatus("#" + Prefix + "TaskStatus", status);

            //var clientId = $("#ClientId").val();
            //if (clientId > 0) {
            //    $('#ClientDetails').val($("#ClientId-display").val())
            //}

            //var clientDetails = $('#ClientDetails').val();
            //, { key: 'ClientDetails', value: clientDetails }
            var value = $("#" + Prefix + "TaskBody").jqxEditor('val');
            var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }];
            var formData = app.serializeEx("#" + Prefix + "fcForm input, " + "#" + Prefix + "fcForm select, " + "#" + Prefix + "fcForm hidden", args);

            app_query.doFormSubmit("#fcForm", actionurl, formData, function (data) {

                afterSubmit(slf, data);
            });


            //return this;
        };

        var actionurl = $("#" + Prefix + 'fcForm').attr('action');

        if (this.IsNew) {
            status = 1;
            actionurl = '/System/UpdateNewTask';
            RunSubmit(this, status, actionurl)
        }
        else {
            //status =  2;
            actionurl = '/System/UpdateTask';
            if (status > 1 && status < 8 && act == 'end') {
                app_dialog.confirmYesNoCancel("האם לסיים משימה?", function (res) {
                    if (res == 'yes')
                        RunSubmit(this, status, '/System/TaskCompleted')
                    else if (res == 'no')//update
                        RunSubmit(this, status, actionurl);

                });
            }
            else {//update
                RunSubmit(this, status, actionurl);
            }
        }
        //return this;
    }

    var loadEvents=function() {

        var slf = this;

        $("#" + Prefix + "reset").on('click', function () {
            location.reload();
        });

        $("#" + Prefix + "fcClear").on('click', function (e) {
            //app_messenger.Post("הודעה");
            $("#" + Prefix +"fcForm")[0].reset();
            $("#" + Prefix + "fcForm").jqxValidator('hide');
        });
        $("#" + Prefix +"fcNew").on('click', function (e) {
            app.refresh();
        });
        $("#" + Prefix +"fcCancel").on('click', function (e) {
            slf.doCancel();
        });
        $("#" + Prefix +"fcSubmit").on('click', function (e) {
            slf.doSubmit('update');
        });

        if (this.IsNew) {

            $("#" + Prefix +"fcSubmit-plus").on('click', function (e) {
                slf.doSubmit('plus');
            });

            $("#" + Prefix +"Form_Type").on('change', function (event) {
                var args = event.args;

                if (args && args.index >= 0) {
                    //app_tasks_form_template.load(args.item.value);
                    app_dialog.confirm("האם ליצור משימות לביצוע מתבנית?", function () {

                        app_query.doDataPost("/System/TaskFormByTemplate", {
                            'TaskId': slf.TaskId, 'FormId': args.item.value
                        },
                            function (data) {
                                if (data.Status > 0)
                                    $("#" + Prefix +"jqxgrid4").jqxGrid('source').dataBind();
                            });
                    });

                    //if (confirm("האם ליצור משימות לביצוע מתבנית?")) {
                    //    app_query.doDataPost("/System/TaskFormByTemplate", { 'TaskId': slf.TaskId, 'FormId': args.item.value },
                    //        function (data) {
                    //            if (data.Status > 0)
                    //                $('#jqxgrid4').jqxGrid('source').dataBind();
                    //        });
                    //}
                }
                $("#" + Prefix +"Form_Type").jqxComboBox({ selectedIndex: -1 });

            });
            $("#" + Prefix +"form_template-check").on('change', function (event) {

                if (this.checked) {//($('#form_template-check').is(":checked")) {

                    var rows = $("#" + Prefix +"jqxgrid4").jqxGrid('getrows');
                    if (rows && rows.length > 0)
                        $("#" + Prefix +"form_template-div").show();
                    else {
                        app_dialog.alert("לא נמצאו רשומות ליצירת תבנית");
                        this.checked = false;
                    }
                }
                else
                    $("#form_template-div").hide();
            });
            $("#" + Prefix +"form_template-save").click(function () {
                var name = $("#" + Prefix +"form_template-input").val();
                if (name == null || name == '') {
                    app_dialog.alert("נא לציין שם תבנית");
                    return;
                }
                name = "'" + name + "'";
                app_query.doDataPost("/System/TaskFormTemplateCreate", {
                    'id': slf.TaskId, 'name': name
                }, function (data) {
                    if (data.Status > 0)
                        $("#" + Prefix +"jqxgrid4").jqxGrid('source').dataBind();
                });

                //app_query.doPost("/System/TaskFormTemplateCreate", { 'id': slf.TaskId, 'name': name }, function (data) {
                //    if (data.Status > 0) {
                //        app_messenger.Post(data);
                //        $('#jqxgrid4').jqxGrid('source').dataBind();
                //    }
                //    else
                //        app_messenger.Post(data, 'error');
                //});
            });
            $("#" + Prefix +"IntendedTo").on('change', function (event) {
                var args = event.args;
                if (args) {
                    var item = args.item;
                    if (item.label.charAt(0) == '@') {
                        $("#" + Prefix +"UserId").val(0);
                        $("#" + Prefix +"TeamId").val(item.value);
                    }
                    else {
                        $("#" + Prefix +"UserId").val(item.value);
                        $("#" + Prefix +"TeamId").val(0);
                    }

                }
            });
        }
        else {

            $("#" + Prefix +"Task_Child-link").click(function () {

                app_dialog.confirm("האם ליצור תת משימה", function () {
                    app.redirectTo('/System/TaskNew?pid=' + slf.TaskId);
                });
            });

            $("#" + Prefix +"fcEnd").on('click', function (e) {
                slf.doSubmit('end');
            });
            //$('#fcSubmit-plus').on('click', function (e) {
            //    slf.doSubmit('plus');
            //});
            $("#" + Prefix +"task-item-update").click(function () {
                var iframe = wizard.getIframe();
                if (iframe && iframe.triggerSubmit) {
                    iframe.triggerSubmit();
                }
            });
            $("#" + Prefix +"task-item-cancel").click(function () {
                wizard.wizHome();
            });
        }

        var input_rules = [
            //{ input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
            {
                input: "#" + Prefix + '#DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'none', rule: function (input, commit) {
                    //var value = $('#DueDate').jqxDateTimeInput('value');
                    var text = $("#" + Prefix + 'DueDate').jqxDateTimeInput('getText');
                    return text != null && text.length > 0;
                }
            },
            {
                input: "#" + Prefix + "TaskBody", message: 'חובה לציין תוכן!', action: 'none', rule: function (input, commit) {
                    //var value = $("#TaskBody").text();//.jqxEditor('val');
                    var value = $("#" + Prefix + "TaskBody").jqxEditor('val');
                    value = app.htmlText(value);//.replace(/(<([^>]+)>)/ig, "");
                    return value.length > 1;
                }
            }
        ];

        $("#" + Prefix + 'fcForm').jqxValidator({
            rtl: true,
            hintType: 'label',
            animationDuration: 0,
            rules: input_rules
        });
    }

    init();

}

var app_system_model = {
    TagDiv: undefined,
    Option: "a",
    Callback: undefined,
    doClose: function (tag) {
        $(tag).hide();
        $(tag).empty();
    },
    reminder_new: function (tagDiv, id, to, toName) {

        this.TagDiv = tagDiv;
        //this.Option = option;//option === undefined || option==null ? "a": option;
        this.RespId = id;
        this.RespTo = to;
        this.RespToName = toName;

        var init = function () {

            var slf = app_system_model;

            var html = (function () {/*
         <div style="margin: 0 auto; display: block; direction: rtl">
			<div class="panel-area">
				<div id="reminder-title" class="panel-area-title">תזכורת</div>
				<div class="expander-entry" style="margin:10px">
					<form class="fcForm-reminder" id="app_system_reminder-form" method="post" action="/System/ReminderUpdate">
						<input type="hidden" id="app_system_reminder-RemindId" name="RemindId" value="0" />
						<input type="hidden" id="app_system_reminder-AccountId" name="AccountId" value="" />
						<input type="hidden" id="app_system_reminder-UserId" name="UserId" value="" />
						<input type="hidden" id="app_system_reminder-TeamId" name="TeamId" value="" />
						<input type="hidden" id="app_system_reminder-Remind_Type" name="Remind_Type" value="0" />
                        <input type="hidden" id="app_system_reminder-ColorFlag" name="ColorFlag" value="#46d6db" />
                        <input type="hidden" id="app_system_reminder-RemindStatus" name="RemindStatus" value="0" />
                        <input type="hidden" id="app_system_reminder-Remind_Parent" name="Remind_Parent" />
                        <input type="hidden" id="app_system_reminder-Project_Id" name="Project_Id" />
                        <input type="hidden" id="app_system_reminder-ClientId" name="ClientId" />
                        <input type="hidden" id="app_system_reminder-Tags" name="Tags"  />

						<div id="tab-content" class="tab-content" dir="rtl">
							<div class="form-group">
								<div class="field">
									אל: <span id="respTo"></span>
								</div>
								<div id="app_system_reminder-AssignTo"></div>
							</div>
							<div class="form-group">
								<div class="field">
									הודעה:
								</div>
								<textarea id="app_system_reminder-RemindBody" name="RemindBody"  style="background-color:#fff;"></textarea>
							</div>
							 <div class="form-group">
								<div class="field">
									תאריך לביצוע:
								</div>
								<div id="app_system_reminder-DueDate" name="DueDate"></div>
							</div>
							<div class="form-group pasive">
								<div class="field">
									אופן התזכור:
								</div>
								<select id="app_system_reminder-RemindPlatform">
									<option selected value="0">מערכת</option>
									<option value="1">מסרון</option>
									<option value="2">דואר אלקטרוני</option>
								</select>
							</div>
							<div id="TaskStatus-group" class="form-group pasive">
								<div class="field">
									סטאטוס :
								</div>
								<input type="text" id="app_system_reminder-RemindStatus" name="RemindStatus" class="text-normal" readonly />
							</div>
							<div class="form-group pasive">
								<div class="field">
									נוצר ב:
								</div>
								<input type="text" id="app_system_reminder-CreatedDate" name="CreatedDate" class="text-mid label" readonly="readonly" data-type="datetime" />
							</div>
							<div style="height: 5px"></div>
							<div class="panel-areaB">
								<input id="app_system_reminder-Submit" class="btn-default btn7 w-60" type="button" value="עדכון" />
								<input id="app_system_reminder-Cancel" class="btn-default btn7 w-60" type="button" value="x" />
							</div>
						</div>
					</form>
				</div>
			</div>
    </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

            //if (slf.Option == "a") {
            //    html = html.replace('form-group active', 'form-group pasive')
            //}

            var container = $(html);

            $(slf.TagDiv).empty();
            $(slf.TagDiv).append(container);
            loadControls();

            $(slf.TagDiv).show();

            switch (slf.Option) {
                case "forward":
                    $("#reminder-title").text("העבר אל"); break
            }

            if (slf.RespId) {
                $("#app_system_reminder-Remind_Parent").val(slf.RespId);
                $("#reminder-title").text("העבר אל");
                if (slf.RespTo) {
                    $("#app_system_reminder-AssignTo").hide();
                    $("#reminder-title").text("השב אל");
                }
                if (slf.RespToName) {
                    if (slf.RespToName === '-ALL-')
                        $("#respTo").text("כל הנמענים להודעה זו");
                    else
                        $("#respTo").text(slf.RespToName);
                }
            }
            //return this;
        };

        var loadControls = function () {

            var slf = app_system_model;


            $('#app_system_reminder-RemindBody').jqxEditor({
                height: '140px',
                width: '100%',
                editable: true,
                rtl: true,
                tools: 'bold italic underline | color background | left center right | link |',
                theme: config.theme
                //stylesheets: ['editor.css']
            });

            //$('#a-jqxExp-1').on('click', function (e) {
            //    if (!slf.exp1_Inited) {
            //        slf.lazyLoad();
            //    }
            //    $('#jqxExp-box').slideToggle();
            //    return false;
            //});


            //$("#ColorFlag").simplecolorpicker();
            //$("#ColorFlag").on('change', function () {
            //    //$('select').simplecolorpicker('destroy');
            //    var color = $("#ColorFlag").val();
            //    $("#hTitle").css("background-color", color)
            //});


            $('#app_system_reminder-DueDate').jqxDateTimeInput({ showCalendarButton: true, readonly: true, width: '150px', rtl: true });

            app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#app_system_reminder-AssignTo", '/System/GetUsersList', null, 225, 0, null, null, function () {

            });

            app_form.setDateTimeNow('#app_system_reminder-CreatedDate');

            //$('#reset').on('click', function () {
            //    location.reload();
            //});

            //$('#fcClear').on('click', function (e) {
            //    //app_messenger.Post("הודעה");
            //    $('#fcForm')[0].reset();
            //    $('#fcForm').jqxValidator('hide');
            //});
            //$('#fcNew').on('click', function (e) {
            //    app.refresh();
            //});

            $('#app_system_reminder-Cancel').on('click', function (e) {
                slf.doClose(slf.TagDiv);
                //$(slf.TagDiv).hide();
                //$(slf.TagDiv).empty();
            });

            $('#app_system_reminder-Submit').on('click', function (e) {

                var actionurl = $('#app_system_reminder-form').attr('action');
                var status = app_jqx.getInputAutoValue("#app_system_reminder-RemindStatus", 1);
                app_jqx.setInputAutoValue("#app_system_reminder-RemindStatus", status);

                var value = $("#app_system_reminder-RemindBody").jqxEditor('val');
                var AssignTo = slf.RespTo ? slf.RespTo : app_jqxcombos.getComboCheckedValues("app_system_reminder-AssignTo");
                var args = [{ key: 'RemindBody', value: app.htmlEscape(value) }, { key: 'AssignTo', value: AssignTo }];
                var formData = app.serializeEx('#app_system_reminder-form input, #app_system_reminder-form select, #app_system_reminder-form hidden', args);

                app_query.doFormSubmit("#app_system_reminder-form", actionurl, formData, function (data) {

                    app_messenger.Post(data);
                    slf.doClose(slf.TagDiv);
                    if (slf.Callback)
                        slf.Callback(data);

                    //afterSubmit(slf, data);
                });

                //slf.doSubmit('update');
            });


            var input_rules = [
                //{ input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
                {
                    input: '#app_system_reminder-DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'keyup, blur', rule: function (input, commit) {
                        var text = $('#app_system_reminder-DueDate').jqxDateTimeInput('getText');
                        return text !== null && text.length > 0;
                    }
                },
                {
                    input: "#app_system_reminder-RemindBody", message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: function (input, commit) {
                        //var value = $("#app_system_reminder-TaskBody").val();
                        var value = $("#app_system_reminder-RemindBody").jqxEditor('val');
                        value = app.htmlText(value);
                        return value.length > 1;
                    }
                }
            ];

            $('#app_system_reminder-form').jqxValidator({
                rtl: true,
                hintType: 'label',
                animationDuration: 0,
                rules: input_rules
            });

            //app_jqx_combo_async.lookupInputAdapter('#app_system_reminder-ClientId', 'lu_Members', this.ClientId, function () {

            //});
            //app_jqx_combo_async.systemLookupInputAdapter('#app_system_reminder-Project_Id', 'lu_Project', this.ProjectId, function () {

            //});
            //app_jqx_adapter.createComboDisplayAsync("Tag", "#app_system_reminder-Tags", '/System/GetTagsList', null, 225, 0, true, this.Tags, function () {

            //});

            // this.exp1_Inited = true;

            //if (!this.IsEditable) {
            //    $("#ClientId").prop("readonly", true);
            //    $("#Project_Id").prop("readonly", true);
            //    $("#Tags").jqxComboBox({ enableSelection: false });
            //    $("#AssignTo").jqxComboBox({ enableSelection: false });
            //    //$("#ShareType").jqxDropDownList({ enableSelection: false });
            //    //app.disableSelect("#ShareType");
            //}

        }

        init();

    },//end reminder_new
    ticket_new: function (tagWindow) {

        this.TagDiv = tagWindow;
        this.Option = "a";
        this.Exp1_Loaded = false;

        var init = function () {

            var slf = app_system_model;

                var html = (function () {/*
  <div id="jqxWidget" style="margin: 0 auto; display: block; direction: rtl">
            <div id="accordion" class="jcx-tabs rtl tab-content" dir="rtl">
                <!--<ul id="tab-query" class="nav nav-tabs1">
                    <li class="active"><a id="hxp-0" href="#exp-0" data-toggle="tab">כרטיס</a></li>
                    <li><a id="hxp-4" href="#exp-4" data-toggle="tab">פעולות</a></li>
                    <li><a id="hxp-5" href="#exp-5" data-toggle="tab">קבצים</a></li>
                </ul>-->
                <div id="exp-0" class="jcxtab-panel active">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-title">כרטיס</div>
                        <div class="expander-entry" style="margin:10px">
                            <form class="fcForm" id="app_system_ticket-form" method="post" action="/System/UpdateNewTask">
                                <input type="hidden" id="app_system_ticket-TaskId" name="TaskId" value="" />
                                <input type="hidden" id="app_system_ticket-AccountId" name="AccountId" value="@ViewBag.UserInfo.ParentId" />
                                <input type="hidden" id="app_system_ticket-AssignBy" name="AssignBy" value="@ViewBag.UserInfo.UserId" />
                                <input type="hidden" id="app_system_ticket-UserId" name="UserId" value="" />
                                <!--<input type="hidden" id="app_system_ticket-TeamId" name="TeamId" value="" />-->
                                <input type="hidden" id="app_system_ticket-Task_Parent" name="Task_Parent" value="" />
                                <input type="hidden" id="app_system_ticket-TaskStatus" name="TaskStatus" value="1" />
                                <input type="hidden" id="app_system_ticket-TaskModel" name="TaskModel" value="E" />
                                <div id="tab-content" class="tab-content" dir="rtl">
                                    <div class="form-group">
                                        <div class="field">
                                            כרטיס עבור:
                                        </div>
                                        <select id="app_system_ticket-TeamId" name="TeamId"></select>
                                    </div>

                                    <div class="form-group">
                                        <div class="field">
                                            סוג דיווח:
                                        </div>
                                        <select id="app_system_ticket-Task_Type" name="Task_Type"></select>
                                    </div>
                                    <div class="form-group">
                                        <div class="field">
                                            נושא:
                                        </div>
                                        <input id="app_system_ticket-TaskSubject" name="TaskSubject" type="text" style="min-width:280px;width:70%;" />
                                    </div>
                                    <div class="form-group" id="TaskBody-div">
                                        <div class="field">
                                            תיאור: <span><a id="TaskBody-btn-view" href="#"><i class="fa fa-search-plus" style="font-size:16px"></i></a></span>
                                        </div>
                                        <textarea id="app_system_ticket-TaskBody" name="TaskBody" class="text-content"></textarea>
                                    </div>
                                    <div class="form-group pasive">
                                        <div class="field">
                                            נוצר ב:
                                        </div>
                                        <input type="text" id="app_system_ticket-CreatedDate" name="CreatedDate" class="text-mid label" readonly="readonly" data-type="datetime" />
                                    </div>
                                    <div class="form-group pasive">
                                        <div class="field">
                                            תאריך לביצוע:
                                        </div>
                                        <div id="app_system_ticket-DueDate" name="DueDate"></div>
                                    </div>

                                    <div class="form-group pasive">
                                        <div class="field">
                                            צבע :
                                        </div>
                                        <select id="app_system_ticket-ColorFlag" ></select>
                                        <input type="hidden" name="ColorFlag" value="#46d6db" />

                                    </div>
                                    <div id="jqxExp-1" class="pasive">
                                        <div class="panel-area-title">
                                            פרטים נוספים
                                        </div>
                                        <div class="panel-area rtl" style="border:none">

                                            <div class="form-group">
                                                <div class="field">
                                                    פרוייקט:
                                                </div>
                                                <input type="text" id="app_system_ticket-Project_Id" name="Project_Id" /><i class="fa fa-search"></i>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    לקוח\מנוי:
                                                </div>
                                                <input type="text" id="app_system_ticket-ClientId" name="ClientId" /><i class="fa fa-search"></i>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    תגיות:
                                                </div>
                                                <div id="app_system_ticket-Tags" name="Tags"></div>
                                            </div>
                                            <div class="form-group">
                                                <div class="field">
                                                    שיתוף:
                                                </div>
                                                <div id="app_system_ticket-AssignTo"></div>
                                            </div>
                                            <div class="form-group pasive">
                                                <input type="checkbox" id="app_system_ticket-IsShare" name="IsShare" />&nbsp;<span>משותף</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
                <div id="exp-4" class="jcxtab-panel pasive">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-4">פעולות</div>
                        <div class="expander-entry">
                            <div class="grid-wrap rtl">
                                <div>
                                    <a id="jqxgrid4-add" href="#" class="btn-default btnIcon"><i class="fa fa-plus-square-o"></i>הוספה</a>
                                    <a id="jqxgrid4-edit" href="#" class="btn-default btnIcon"><i class="fa fa-edit"></i>עריכה</a>
                                    <a id="jqxgrid4-remove" href="#" class="btn-default btnIcon"><i class="fa fa-remove"></i>הסרה</a>
                                    <a id="jqxgrid4-refresh" href="#" class="btn-default btnIcon"><i class="fa fa-refresh"></i>רענון</a>
                                    <a id="jqxgrid4-clear" href="#" class="btn-default btnIcon"><i class="fa fa-circle-thin"></i>ניקוי</a>
                                </div>
                                <div id="jqxgrid4-window" style="display:none">
                                    <div>עריכה</div>
                                    <div style="overflow: hidden;direction:rtl;text-align:right">
                                        <form id="fcTaskForm" method="post" action="/System/TaskFormUpdate">
                                            <input type="hidden" id="ItemId" name="ItemId" value="0" />
                                            <input type="hidden" id="Task_Id" name="Task_Id" value="0" />
                                            <div class="form-group">
                                                <div class="field">
                                                    תיאור:
                                                </div>
                                                <textarea id="ItemText" name="ItemText" class="text-content" style="height:120px;width:90%;"></textarea>
                                            </div>
                                            <div style="height: 5px"></div>
                                            <div>
                                                <input id="formSave" type="button" class="btn-default btn7 w-80" value="עדכון" />
                                                <input id="formCancel" type="button" class="btn-default btn7 w-80" value="ביטול" />
                                            </div>
                                            <div id="signResult"></div>
                                        </form>
                                    </div>
                                </div>
                                <div class="form-group pasive">
                                    <div class="field">
                                        הוספה מתבנית:
                                    </div>
                                    <div id="Form_Type" name="Form_Type"></div>
                                </div>
                                <div id="jqxgrid4" style="position:relative;z-index:1;"></div>
                                <div class="form-group pasive">
                                    <input id="form_template-check" type="checkbox" /><span>האם לשמור כתבנית?</span>
                                </div>
                                <div id="form_template-div" class="item-pasive">
                                    <div>שם התבנית</div>
                                    <input id="form_template-input" type="text" />
                                    <a id="form_template-save" href="#" class="btn-default btn7">שמירה</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="exp-5" class="jcxtab-panel pasive">
                    <div class="panel-area">
                        <div class="panel-area-title" id="hxp-5">קבצים</div>
                        <div class="expander-entry">
                            <div id="task-files"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="height: 5px"></div>
            <div class="panel-area">
                <input id="app_system_ticket-Submit" class="btn-default btn7 w-60" type="button" value="עדכון" />
                <span id="sp-plus" class="pasive"><input id="fcSubmit-plus" class="btn-default btn7 w-60" type="button" value="עדכון+" /></span>
                <span id="sp-new" class="pasive"><input id="fcNew" class="btn-default btn7 w-60" type="button" value="חדש" /></span>
                <input id="app_system_ticket-Cancel" class="btn-default btn7 w-60" type="button" value="ביטול" />
            </div>
        </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

            if (slf.Option == "a") {
                    html = html.replace('form-group active', 'form-group pasive')
             }

                var container = $(html);

            $(slf.TagDiv).empty();
            $(slf.TagDiv).append(container);

            loadControls();

            $(slf.TagDiv).show();

                //return this;
            };

        var loadControls = function () {

            var slf = app_system_model;


            $('#app_system_ticket-TaskBody').jqxEditor({
                height: '140px',
                width: '100%',
                editable: true,
                rtl: true,
                tools: 'bold italic underline | color background | left center right',
                theme: config.theme
                //stylesheets: ['editor.css']
            });

            $('#a-jqxExp-1').on('click', function (e) {
                if (!slf.exp1_Inited) {
                    slf.lazyLoad();
                }
                $('#jqxExp-box').slideToggle();
                return false;
            });


            //$("#app_system_ticket-ColorFlag").simplecolorpicker();
            //$("#app_system_ticket-ColorFlag").on('change', function () {
            //    //$('select').simplecolorpicker('destroy');
            //    var color = $("#ColorFlag").val();
            //    $("#hTitle").css("background-color", color)
            //});


            $('#app_system_ticket-DueDate').jqxDateTimeInput({ showCalendarButton: true, readonly: true, width: '150px', rtl: true });

            app_jqx_adapter.createComboCheckAdapterAsync("UserId", "DisplayName", "#app_system_ticket-AssignTo", '/System/GetUsersList', null, 225, 0, null, null, function () {

            });
            app_form.setDateTimeNow('#app_system_ticket-CreatedDate');

            $('#app_system_ticket-Cancel').on('click', function (e) {
                slf.doClose(slf.TagDiv);
            });

            $('#app_system_ticket-Submit').on('click', function (e) {

                var actionurl = $('#app_system_ticket-form').attr('action');
                var status = app_jqx.getInputAutoValue("#app_system_ticket-TaskStatus", 1);
                app_jqx.setInputAutoValue("#app_system_ticket-TaskStatus", status);

                var value = $("#app_system_ticket-TaskBody").jqxEditor('val');
                var AssignTo = app_jqxcombos.getComboCheckedValues("app_system_ticket-AssignTo");
                var args = [{ key: 'TaskBody', value: app.htmlEscape(value) }, { key: 'AssignTo', value: AssignTo }];
                var formData = app.serializeEx('#app_system_ticket-form input, #app_system_ticket-form select, #app_system_ticket-form hidden', args);

                app_query.doFormSubmit("#app_system_ticket-form", actionurl, formData, function (data) {

                    app_messenger.Post(data);
                    slf.doClose(slf.TagDiv);
                    if (slf.Callback)
                        slf.Callback(data);
                });
               
            });

           
            var input_rules = [
                //{ input: '#TaskSubject', message: 'חובה לציין נושא!', action: 'keyup, blur', rule: 'required' },
                {
                    input: '#app_system_ticket-DueDate', message: 'חובה לציין תאריך לביצוע!', action: 'keyup, blur', rule: function (input, commit) {
                        //var value = $('#DueDate').jqxDateTimeInput('value');
                        var text = $('#app_system_ticket-DueDate').jqxDateTimeInput('getText');
                        return text !== null && text.length > 0;
                    }
                },
                {
                    input: '#app_system_ticket-TeamId', message: 'חובה לציין עבור מי!', action: 'keyup, blur', rule: function (input, commit) {
                        var text = $('#app_system_ticket-TeamId').val();
                        return text !== null && text.length > 0;
                    }
                },
                {
                    input: '#app_system_ticket-Task_Type', message: 'חובה לציין עבור סוג דיווח!', action: 'keyup, blur', rule: function (input, commit) {
                        var text = $('#app_system_ticket-TeamId').val();
                        return text !== null && text.length > 0;
                    }
                },
                {
                    input: "#app_system_ticket-TaskBody", message: 'חובה לציין תוכן!', action: 'keyup, blur', rule: function (input, commit) {
                        //var value = $("#TaskBody").text();//.jqxEditor('val');
                        var value = $("#app_system_ticket-TaskBody").jqxEditor('val');
                        value = app.htmlText(value);//.replace(/(<([^>]+)>)/ig, "");
                        return value.length > 1;
                    }
                }
            ];

            $('#app_system_ticket-form').jqxValidator({
                rtl: true,
                hintType: 'label',
                animationDuration: 0,
                rules: input_rules
            });

            app_control.select2Ajax("#app_system_ticket-Task_Type", 240, null, '/System/GetListsSelect', { 'model': 5 });

            app_control.select2Ajax("#app_system_ticket-TeamId", 240, null, '/System/GetListsSelect', { 'model': 10 });


            //app_jqxcombos.createComboAdapter("UserTeamId", "DisplayName", "app_system_ticket-IntendedTo", '/System/GetUserTeamList', 0, 120, false);

            app_jqx_combo_async.lookupInputAdapter('#app_system_ticket-ClientId', 'lu_Members', this.ClientId, function () {

            });
            app_jqx_combo_async.systemLookupInputAdapter('#app_system_ticket-Project_Id', 'lu_Project', this.ProjectId, function () {

            });
            app_jqx_adapter.createComboDisplayAsync("Tag", "#app_system_ticket-Tags", '/System/GetTagsList', null, 225, 0, true, this.Tags, function () {

            });

            slf.Exp1_Loaded = true;

            //if (!this.IsEditable) {
            //    $("#ClientId").prop("readonly", true);
            //    $("#Project_Id").prop("readonly", true);
            //    $("#Tags").jqxComboBox({ enableSelection: false });
            //    $("#AssignTo").jqxComboBox({ enableSelection: false });
            //    //$("#ShareType").jqxDropDownList({ enableSelection: false });
            //    //app.disableSelect("#ShareType");
            //}

        }

        init();

    },//end ticketNew
    userDef: function (tagWindow) {

        this.TagDiv = tagWindow;
        this.Option = "a";

        var init = function () {

            var slf = app_system_model;

                var html = (function () {/*
                <div id="userDefInfo" class="panel-area">
                             <div cll="panel-area-title">פרטים אישיים</div>
                             <div style="overflow: hidden;direction:rtl;">
                                 <form id="app_system_user-form" action="/System/UserDefUpdate">
                                     <input type="hidden" id="app_system_user-insertFlag" value="0" />
                                     <!--<input type="hidden" id="app_system_user-ApplicationId" value="0" />-->
                                     <input type="hidden" id="app_system_user-AccountId" value="0" />
                                     <input type="hidden" id="app_system_user-Lang" value="" />
                                     <!--<input type="hidden" id="app_system_user-Perms" value="" />-->

                                     <input type="hidden" id="app_system_user-Evaluation" value="0" />
                                     <input type="hidden" id="app_system_user-Creation" value="" />

                                     <table style="border-spacing: 10px;border-collapse: separate;direction:rtl">
                                         <tr id ="trcode">
                                             <td>קוד משתמש:</td>
                                             <td><input id="app_system_user-UserId" name="UserId" type="text" disabled="disabled" style="direction:rtl;text-align:right;" /></td>
                                             </tr>
                                             <tr>
                                                 <td>שם משתמש:</td>
                                                 <td><input id="app_system_user-UserName" name="UserName" type="text" style="direction:ltr;text-align:left;" /></td>
                                             </tr>
                                             <tr>
                                                 <td>שם מלא:</td>
                                                 <td><input id="app_system_user-DisplayName" name="DisplayName" type="text" style="direction:rtl;text-align:right;" /></td>
                                             </tr>
                                             <tr>
                                                 <td>אימייל:</td>
                                                 <td><input id="app_system_user-Email" name="Email" type="text" style="direction:ltr;text-align:left;" /></td>
                                             </tr>
                                             <tr>
                                                 <td>טלפון:</td>
                                                 <td><input id="app_system_user-Phone" name="Phone" type="text" style="direction:ltr;text-align:left;" /></td>
                                             </tr>
                                             <tr>
                                                 <td>תפקיד:</td>
                                                 <td>
                                                    <!--<div id="app_system_user-UserRole" name="UserRole"></div>-->
                                                    <input id="app_system_user-UserRole" type="text" readonly class="disabled"/>
                                                    <input name="UserRole" type="hidden" />
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td>חסום:</td>
                                                 <td colspan="2"><div id="app_system_user-IsBlocked" name="IsBlocked" style="direction:rtl;text-align:right;"></div></td>
                                             </tr>
                                             <tr id="isReset">
                                                 <td>איפוס סיסמה:</td>
                                                 <td colspan ="2"><div id="app_system_user-IsResetPass" style="direction:rtl;text-align:right;"></div></td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2"></td>
                                             </tr>

                                             <tr>
                                                 <td></td>
                                                 <td style="padding-top: 10px;">
                                                     <input id="app_system_user-Save" type="button" class="btn-default btn7 w-60" value="עדכון" />
                                                     <input id="app_system_user-Cancel" type="button" class="btn-default btn7 w-60" value="ביטול" />
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2"></td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2"><div id="signResult"></div></td>
                                             </tr>
                                    </table>
                               </form>
                             </div>
                         </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

            if (slf.Option == "a") {
                    html = html.replace('form-group active', 'form-group pasive')
                }

                var container = $(html);

            $(slf.TagDiv).empty();
            $(slf.TagDiv).append(container);

            loadControls();
            loadData();

            $(slf.TagDiv).show();

                //return this;
            };

        var loadControls = function () {

            var slf = app_system_model;

            // initialize the input fields.
            //$("#app_system_user-UserRole").jqxDropDownList().width(150);
            $("#app_system_user-IsBlocked").jqxCheckBox();
            $("#app_system_user-IsResetPass").jqxCheckBox();
            if (this.AllowEdit == 0) {
                $("#app_system_user-UserRole").jqxDropDownList("disabled", true);
                $("#app_system_user-IsBlocked").jqxCheckBox("disabled", true);
            }

            // initialize the popup window and buttons.
            
            $("#app_system_user-Save").click(function (e) {

                app_form.doFormPost("#app_system_user-form", function (data) {
                    app_messenger.Post(data.Message);
                });
            });
            $("#app_system_user-Cancel").click(function (e) {

                slf.doClose(slf.TagDiv);
            });

            var input_rules = [
                { input: '#app_system_user-UserName', message: 'נדרש שם משתמש!', action: 'keyup, blur', rule: 'required' },
                { input: '#app_system_user-UserName', message: 'שם משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup, blur', rule: 'length=3,12' },
                {
                    input: '#app_system_user-UserName', message: 'נדרש אותיות באנגלית בלבד!', action: 'valuechanged, blur', rule:
                        function (input, commit) {
                            var re = /^[A-Za-z][A-Za-z0-9]*$/
                            return re.test(input.val());
                        }
                },
                { input: '#app_system_user-DisplayName', message: 'נדרש פרטי משתמש!', action: 'keyup, blur', rule: 'required' },
                { input: '#app_system_user-DisplayName', message: 'פרטי משתמש אותיות בלבד!', action: 'keyup', rule: 'notNumber' },
                { input: '#app_system_user-DisplayName', message: 'פרטי משתמש בין 3 ל 12 תוים לפחות!', action: 'keyup', rule: 'length=3,12' },
                { input: '#app_system_user-Email', message: 'נדרש כתובת אימייל!', action: 'keyup, blur', rule: 'required' },
                { input: '#app_system_user-Email', message: 'אימייל לא תקין!', action: 'keyup', rule: 'email' },
                //{ input: '#app_system_user-UserRole', message: 'נדרש תפקיד!', action: 'valuechanged, blur', rule: 'required' },
                {
                    input: '#app_system_user-UserRole', message: 'נדרש תפקיד!', action: 'keyup, select', rule: function (input) {
                        var index = $("#UserRole").jqxDropDownList('getSelectedIndex');
                        if (index >= 0) { return true; } return false;
                    }
                },
                { input: '#app_system_user-Phone', message: 'נדרש טלפון נייד!', action: 'keyup, blur', rule: 'required' },
                {
                    input: '#app_system_user-Phone', message: 'טלפון נייד אינו תקין!', action: 'valuechanged, blur', rule:
                        function (input, commit) {
                            var re = /^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/
                            return re.test(input.val());
                        }
                }
            ];

            $('#app_system_user-form').jqxValidator({
                rtl: true,
                //hintType: 'label',
                animationDuration: 0,
                rules: input_rules
            });

            //app_lookups.user_role('app_system_user-form', "app_system_user-UserRole", "UserRole");

            // prepare the data
        //    var roleSource =
        //        {
        //            //async:false,
        //            dataType: "json",
        //            dataFields: [
        //                { name: 'RoleId' },
        //                { name: 'RoleName' }
        //            ],
        //            data: {},
        //            type: 'POST',
        //            url: '/System/GetUsersRoles'
        //        };
        //    var roleAdapter = new $.jqx.dataAdapter(roleSource, {
        //        contentType: "application/json; charset=utf-8",
        //        loadError: function (jqXHR, status, error) {
        //            //alert("roleAdapter failed: " + error);
        //        },
        //        loadComplete: function (data) {
        //            //alert("roleAdapter is Loaded");
        //        }
        //    });
        //    $("#app_system_user-UserRole").jqxDropDownList(
        //        {
        //            rtl: true,
        //            source: roleAdapter,
        //            width: 120,
        //            autoDropDownHeight: true,
        //            dropDownHorizontalAlignment: 'right',
        //            promptText: "בחירת תפקיד",
        //            displayMember: 'RoleName',
        //            valueMember: 'RoleId'
        //        });
        };

        var loadData=function(){

            var source =
                {
                    //async: false,
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
                    url: '/System/GetUserInfo'
                };
            var dataAdapter = new $.jqx.dataAdapter(source, {
                contentType: "application/json; charset=utf-8",
                loadError: function (jqXHR, status, error) {

                    app_dialod.alert(error);

                },
                loadComplete: function (data) {

                    app_form.loadDataForm("app_system_user-form", data);
                    app_lookups.user_role('app_system_user-form', "app_system_user-UserRole", "UserRole",data.UserRole);
                }
            });
            dataAdapter.dataBind();

        };

        init();
    },//end userDef
    task_comments : function (tagDiv, id) {

        var source = {
            datatype: "json",
            id: 'CommentId',
            type: 'POST',
            url: '/System/GetTasksCommentGrid',
            data: { 'pid': id }
        }
        var DataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function (records) {
                // data is loaded.
                var html = "<h4>הערות</h4>";//"<table border='0'>";
                var item = "";
                var created;
                var dueDate;

                var length = records ? records.length : 0;
                if (length == 0) {
                    var msg = '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';

                    html += msg;
                }
                else {
                    for (var i = 0; i < length; i++) {
                        var rowData = records[i];

                        created = app.formatDateString(rowData.CommentDate);
                        dueDate = app.formatDateString(rowData.ReminderDate);
                        item =
                            '<div class="panel-area" style="font:12px arial, verdana">' +
                            '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                         '<label>מאת: <span style="color:#000">' + rowData.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + created + '</span></label>,,<label>לתזכורת: <span style="color:#000">' + dueDate + '</span></label>' +
                            '</div>' +
                         '<div style="color:#000">' + rowData.CommentText + '</div>' +
                            '</div>';
                        html += item;
                    }
                }
                $(tagDiv).html(html);
            }
        });
        DataAdapter.dataBind();
        return false;
    },
    task_sub: function (tagDiv, id) {

        var source = {
            datatype: "json",
            id: 'ItemId',
            type: 'POST',
            url: '/System/GetTasksFormGrid',
            data: { 'pid': id }
        }
        var DataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function (records) {
                // data is loaded.
                var html = "<h4>פעולות</h4>";//"<table border='0'>";
                var item = "";
                var created;
                var doneDate;

                var length = records ? records.length : 0;
                if (length == 0) {
                    var msg = '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';

                    html += msg;
                }
                else {
                    for (var i = 0; i < length; i++) {
                        var rowData = records[i];

                        created = app.formatDateString(rowData.ItemDate);
                        doneDate = app.formatDateString(rowData.DoneDate);
                        item =
                            '<div class="panel-area" style="font:12px arial, verdana">' +
                            '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                            '<label>מבצע: <span style="color:#000">' + rowData.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + created + '</span></label>,,<label>בוצע ב: <span style="color:#000">' + doneDate + '</span></label>' +
                            '</div>' +
                            '<div style="color:#000">' + rowData.ItemText + '</div>' +
                            '</div>';
                        html += item;
                    }
                }
                $(tagDiv).html(html);
            }
        });
        DataAdapter.dataBind();
        return false;
    },
    personal_container: function (tag) {

        var html = (function () {/*
            <div id="personal-header" style="width:100%;height:30px;display:block;">
                <label class="line-space-10"><a href="#" id="cmdReminder" title="שליחת הודעה"><i class="fa fa-send" style="font-size:20px;color:#60636b"></i></a></label>
                <label class="line-space-10"><a href="#" id="cmdTicket" title="דיווח כרטיס"><i class="fa fa-ticket" style="font-size:20px;color:#ff6a00"></i></a></label>
                <label class="line-space-10"><a href="#" id="cmdMe" title="פרטים אישיים"><i class="fa fa-user-md" style="font-size:20px;color:#60636b"></i></a></label>
                <label class="line-space-10"><a href="#" id="cmdPinGet" title="הצג"><i class="fa fa-thumb-tack" style="font-size:20px;color:#60636b"></i></a></label>
                <label class="line-space-10"><a href="#" id="cmdResycle" title="רענון"><i class="fa fa-recycle" style="font-size:20px;color:#60636b"></i></a></label>
            </div>
            <input type="hidden" id="personal-wiz-items" value="0" />
            <div id="personal-wiz"></div>
            <div id="personal-wiz-resp"></div>
            <!--Notifications-->
            <div id="notify_container" style="max-height:240px; overflow-y:auto; overflow-x:hidden;">
                <div id="app_Notification" style="direction:rtl;position:relative;margin:5px  5px;"></div>
            </div>
            <div id="jqxNotification" style="direction:rtl;position:relative; display:none;">
        </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        $(tag).html(html);

        //app_system_personal.personal_container(tag);

        app_messenger.Init('bottom-right', 'flat');

        app_notification_listener();

        $("#cmd-personal").click(function (e) {
            $("#personal-container").toggle();

            var isVisible = $("#personal-container").is(":visible");
            app_cookies.setItemConfig("#personal-container", isVisible ? "1" : "0");


            // The same works with hidden
            //$(element).is(":hidden");

        });

        //$("#cmdPinSet").click(function (e) {
        //    app_cookies.setItemConfig("#personal-pin", window.location.href);
        //});

        $("#cmdPinGet").click(function (e) {
            var pinpath = app_cookies.getItem("#personal-pin");
            if (pinpath) {
                app_iframe.appendPanel("#personal-wiz", pinpath, '100%', '400px', true, pinpath);
            }
        });


        $("#cmdResycle").click(function (e) {

            app_reminder_active.app_Notification(function (length) {

                if (length > 0) {

                    app_messenger.Post("רענון הודעות הסתיים");
                }
            });

        });
        $("#cmdReminder").click(function (e) {
            app_system_model.reminder_new("#personal-wiz");
            //model.init();
            //app_iframe.appendPanel("#personal-wiz", "/System/_DefUser", '100%', 100, true, "הגדרות אישיות");
        });
        $("#cmdTicket").click(function (e) {
            app_system_model.ticket_new("#personal-wiz");
            //app_iframe.appendPanel("personal-header", "/System/_TicketNew", '100%', 100, true, "דיווח");
        });
        $("#cmdMe").click(function (e) {
            app_system_model.userDef("#personal-wiz");
            //app_iframe.appendPanel("#personal-wiz", "/System/_DefUser", 1200, 200, true, "הגדרות אישיות");
        });

        var isVis = app_cookies.getItem("#personal-container");
        if (isVis == "1") {
            $("#personal-container").toggle();
        }

    },
    pinItem(page) {

        var cur = window.location.href;
        cur = cur.replace(page, page + "_Pin");
        app_cookies.setItemConfig("#personal-pin", cur);
        return false;
    }
    
}

var app_repeater = {

    kanban: function (item) {

        var html_item =
            '<div class="panel-area rtl" style="font:12px arial, verdana">' +
            '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
            '<label>מאת: <span style="color:#000">' + item.assignBy + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.createdDate) + '</span></label>' +
            ',<label><span style="color:#000"><a href="#" title="הערות" onclick="return app_repeater.kanban_task_comments(' + item.id + ');"><i class="fa fa-file-text-o" style="font-size:16px;color:#219f37"></i></a></span></label>' +
            ',<label><span style="color:#000"><a href="#" title="פעולות" onclick="return app_repeater.kanban_task_sub(' + item.id + ');"><i class="fa fa-tasks" style="font-size:16px;color:#219f37"></i></a></span></label>' +
            '</div>' +
            '<div style="color:#000"><label style="font-weight:bold">תאור:</label> ' + app.htmlUnescape(item.content) + '</div>' +
            '</div>' +
            '<div class="panel-area kanban-items-comments-' + item.id + '" style="color:#000;display:none"></div>' +
            '<div class="panel-area kanban-items-sub-' + item.id + '" style="color:#000;display:none"></div>';


    },
    kanban_task_comments: function (id) {

        if ($(".kanban-items-comments-" + id).is(':visible'))
            $(".kanban-items-comments-" + id).hide();
        else {
            $(".kanban-items-comments-" + id).show();
            app_system_model.task_comments(".kanban-items-comments-" + id, id);
        }
        return false;
    },
    kanban_task_sub: function (id) {
        if ($(".kanban-items-sub-" + id).is(':visible'))
            $(".kanban-items-sub-" + id).hide();
        else {
            $(".kanban-items-sub-" + id).show();
            app_system_model.task_sub(".kanban-items-sub-" + id, id);
        }
        return false;
    },
    task_comments_adapter: function (tag, taskid) {

        app_query.doDataAdapter("/System/GetTasksCommentGrid", { 'pid': taskid }, function (data) {
            app_repeater.task_comments(tag, data);

        });
    },
    task_comments: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מאת: <span style="color:#000">' + item.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.CommentDate) + '</span></label>' +
                    ',<label>תזכורת: <span style="color:#000">' + app.formatDateString(item.ReminderDate) + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">תוכן:</label> ' + app.textToHtml(item.CommentText) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    task_assign_adapter: function (tag, taskid) {

        app_query.doDataAdapter("/System/GetTasksAssignGrid", { 'pid': taskid }, function (data) {

            app_repeater.task_assign(tag, data);

        });
    },
    task_assign: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>נוצר: <span style="color:#000">' + app.formatDateString(item.AssignDate) + '</span></label>' +
                    ', <label>מאת: <span style="color:#000">' + item.AssignedByName + '</span></label>,<label>אל: <span style="color:#000">' + item.AssignedToName + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">נושא:</label> ' + app.textToHtml(item.AssignSubject) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    task_timer_adapter: function (tag, taskid) {

        app_query.doDataAdapter("/System/GetTasksTimerGrid", { 'pid': taskid }, function (data) {

            app_repeater.task_timer(tag, data);

        });
    },
    task_timer: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מבצע: <span style="color:#000">' + item.DisplayName + '</span></label>' +
                    ',<label>התחלה: <span style="color:#000">' + app.formatDateTimeString(item.StartTime) + '</span></label> , <label>סיום: <span style="color:#000">' + app.formatDateTimeString(item.EndTime) + '</span></label>' +
                    ',<label>משך הזמן: <span style="color:#000">' + item.DurationView + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">נושא:</label> ' + app.textToHtml(item.Subject) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    task_form_adapter: function (tag, taskid) {

        app_query.doDataAdapter("/System/GetTasksFormGrid", { 'pid': taskid }, function (data) {

            app_repeater.task_form(tag, data);

        });
    },
    task_form: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מאת: <span style="color:#000">' + item.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.ItemDate) + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">נושא:</label> ' + app.textToHtml(item.ItemText) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    topic_form_adapter: function (tag, taskid) {

        app_query.doDataAdapter("/System/GetTasksFormGrid", { 'pid': taskid }, function (data) {

            app_repeater.topic_form(tag, data);

        });
    },
    topic_form: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מאת: <span style="color:#000">' + item.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.ItemDate) + '</span></label>' +
                    ',<label><span style="color:#000"><a href="#" title="הצג" onclick="return app_tasks.taskEdit(' + item.ItemTask + ');"><i class="fa fa-tasks" style="font-size:16px;color:#219f37"></i> ' + item.ItemTask + '</a></span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">נושא:</label> ' + app.textToHtml(item.ItemText) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    doc_comments_adapter: function (tag, id) {

        app_query.doDataAdapter("/System/GetDocsCommentGrid", { 'pid': id }, function (data) {
            app_repeater.doc_comments(tag, data);

        });
    },
    doc_comments: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מאת: <span style="color:#000">' + item.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.CommentDate) + '</span></label>' +
                    ',<label>תזכורת: <span style="color:#000">' + app.formatDateString(item.ReminderDate) + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">תוכן:</label> ' + app.textToHtml(item.CommentText) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    doc_form_adapter: function (tag, id) {

        app_query.doDataAdapter("/System/GetDocsFormGrid", { 'pid': id }, function (data) {

            app_repeater.doc_form(tag, data);

        });
    },
    doc_form: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מאת: <span style="color:#000">' + item.DisplayName + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.ItemDate) + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">נושא:</label> ' + app.textToHtml(item.ItemLabel) + '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">תוכן:</label> ' + app.textToHtml(item.ItemValue) + '</div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
    media_files_adapter: function (tag, id, refType) {

        app_query.doDataAdapter("/Media/GetMediaRefFiles", { 'RefId': id, "RefType": refType }, function (data) {

            app_repeater.media_file_item(tag, data);
        });
    },
    media_file_item: function (tag, records) {

        var html = "<div style='max-height:200px;overflow:auto'>";

        var length = records ? records.length : 0;
        if (length == 0) {
            html += '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';
        }
        else {
            for (var i = 0; i < length; i++) {
                var item = records[i];

                var html_item =
                    '<div class="panel-area rtl" style="margin:2px;font:12px arial, verdana">' +
                    '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
                    '<label>מאת: <span style="color:#000">' + item.UserName + '</span></label>,<label>נוצר: <span style="color:#000">' + app.formatDateString(item.Creation) + '</span></label>' +
                    '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">נושא:</label> ' + app.textToHtml(item.FileSubject) + '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">פעולה:</label> ' + app.textToHtml(item.FileAction) + '</div>' +
                    '<div style="color:#000"><label style="font-weight:bold">להורדה:</label> <a href="' + item.FileFullPath.replace("~/", "/") + '">' + item.SrcName + '</a></div>' +
                    '</div>';
                html += html_item;

            }
        }
        html += '</div>';
        $(tag).html(html);
    },
}

var app_features = {

    editorTag: function (tag, editable, width, height) {

        if (height === undefined || height == 0)
            height = 220;
        if (width === undefined || width == 0)
            width = '100%';

        $(tag).jqxEditor({
            height: height,
            //width: '100%',
            editable: editable,
            rtl: true,
            tools: editable ? 'bold italic underline | color background | left center right' : '',
            theme: config.theme
            //stylesheets: ['editor.css']
        });

        $(tag+'-btn-view').on('click', function () {

            if ($(tag +'-div').hasClass("editor-view")) {
                $(tag +'-div').removeClass("editor-view");
                $(tag).jqxEditor('height', '220px');
                $(tag).css('height', '225px');
            }
            else {
                $(tag +'-div').addClass("editor-view");
                $(tag).css('height', '805px');
                $(tag).jqxEditor('height', '800px');
            }
        });
    },
    colorFlag: function (tag, dest) {
        if (tag === undefined)
            tag = "#ColorFlag";
        $(tag).simplecolorpicker();
        if (dest) {
            $(tag).on('change', function () {
                //$('select').simplecolorpicker('destroy');
                var color = $(tag).val();
                $(dest).css("background-color", color)
            });
        }
        var colors = { "#46d6db": "Turquoise", "#7bd148": "Green", "#5484ed": "Bold blue", "#a4bdfc": "Blue", "#7ae7bf": "Light green", "#51b749": "Bold green", "#fbd75b": "Yellow", "#ffb878": "Orange", "#ff887c": "Red", "#dc2127": "Bold red", "#dbadff": "Purple" };
        app_control.appendSelectOptions(tag, colors);
    }
}

//============================================================ app_notification

var app_notification_listener = function () {

    app_reminder_active.app_Notification();

    var delay = 10 * 6000;
    function listener() {
        let errors = 0;

        setTimeout(function go() {

            app_reminder_active.app_Notification();

            setTimeout(go, delay);

        }, delay);
    }
    listener();
}

function app_reminder_active(tag,caller) {

    this.TagDiv = tag;
    this.TaskStatus = 0;
    this.RelatedCaller = caller;
    //this.LastSycle =
};

app_reminder_active.reminder_item = function (rowData, caller) {


    //var tsk_modelname = app_tasks.getTaskModelName('R');
    var created = app.formatDateString(rowData.CreatedDate);
    var dueDate = app.formatDateString(rowData.DueDate);
    //var startedDate = app.formatDateString(rowData.StartedDate);
    var body = rowData.RemindBody && rowData.RemindBody != null ? rowData.RemindBody : '';
    //var bodyText = app.htmlText(app.htmlUnescape(body));
    var bodyText = app.htmlUnescape(body);

    var relatedInfo = '';
    var relatedEdit = '';
    var relatedForword = '';
    var relatedReply = '';
    var relatedReplyAll = '';
    var relatedCaller = '';
    var relatedParent = '';

    if (caller && caller !== rowData.Remind_Parent) {//forword

        relatedCaller = ' <label><span style="color:#000"><a href="#" title="עבור אל: ' + caller + '" onclick="return app_reminder.reminder_message(\'#personal-wiz-resp\',' + caller + ',' + rowData.RemindId + ');"><i class="fa fa-step-forward" style="font-size:16px;color:#000"></i></a></span></label>';
    }
    if (rowData.Remind_Parent) {//backword

        relatedParent = ' <label><span style="color:#000"><a href="#" title="עבור אל: ' + rowData.Remind_Parent + '" onclick="return app_reminder.reminder_message(\'#personal-wiz-resp\',' + rowData.Remind_Parent + ',' + rowData.RemindId + ');"><i class="fa fa-step-backward" style="font-size:16px;color:#000"></i></a></span></label>';
    }
    
    relatedForword = ' <label><span><a href="#" title="העבר אל" onclick="return app_system_model.reminder_new(\'#personal-wiz\',' + rowData.RemindId + ');"><i class="fa fa-mail-forward" style="font-size:16px;color:#ff6a00"></i></a></span></label>';

    if (rowData.AssignBy != rowData.RecipientId) {

        relatedReply = ' <label><span style="color:#000"><a href="#" title="השב ל: ' + rowData.AssignByName + '" onclick="return app_system_model.reminder_new(\'#personal-wiz\',' + rowData.RemindId + ',' + rowData.AssignBy + ',\'' + rowData.AssignByName + '\');"><i class="fa fa-reply" style="font-size:16px;"></i></a></span></label>';
        if (rowData.AssignTo && rowData.AssignTo.indexOf(',') > -1)
            relatedReplyAll = ' <label><span style="color:#000"><a href="#" title="השב ל: ' + rowData.AssignTo + '" onclick="return app_system_model.reminder_new(\'#personal-wiz\',' + rowData.RemindId + ',\'-ALL-\',\'-ALL-\');"><i class="fa fa-reply-all" style="font-size:16px;"></i></a></span></label>';
    }
    if (rowData.RelatedId > 0) {
        switch (rowData.RelatedType) {

            case 11://doc
                relatedInfo = ' <label><span style="color:#000"><a href="#" title="הצג מסמך: ' + rowData.RelatedId + '" onclick="return app_open.docInfo(\'#notify_container\',' + rowData.RelatedId + ');"><i class="fa fa-print" style="font-size:16px;"></i></a></span></label>';
                relatedEdit = ' <label><span style="color:#000"><a href="#" title="עריכת מסמך: ' + rowData.RelatedId + '" onclick="return app_open.docEdit(' + rowData.RelatedId + ');"><i class="fa fa-pencil" style="font-size:16px;"></i></a></span></label>';
                break;
            case 12://task
                relatedInfo = ' <label><span style="color:#000"><a href="#" title="הצג משימה: ' + rowData.RelatedId + '" onclick="return app_open.taskInfo(\'#body-wrapper\',' + rowData.RelatedId + ');"><i class="fa fa-print" style="font-size:16px;"></i></a></span></label>';
                relatedEdit = ' <label><span style="color:#000"><a href="#" title="עריכת משימה: ' + rowData.RelatedId + '" onclick="return app_open.taskEdit(' + rowData.RelatedId + ');"><i class="fa fa-pencil" style="font-size:16px;"></i></a></span></label>';
                break;
        }
    }
    var item =
        '<div class="panel-area" style="font:12px arial, verdana">' +
        '<div style="display:inline-block;border-bottom:solid 2px #f68330;color:#f68330">' +
        '<label>מאת: <span style="color:#000">' + rowData.AssignByName + '</span></label>,<label>נוצר: <span style="color:#000">' + created + '</span></label>,<label><span style="color:#000"><a href="#" title="סימון כנקרא: ' + rowData.RemindId +'" onclick="return app_reminder.reminder_readed(' + rowData.RemindId + ');"><i class="fa fa-check-circle" style="font-size:16px;color:#219f37"></i></a></span></label>' +
        relatedForword + relatedReply + relatedReplyAll + relatedInfo + relatedEdit + relatedCaller + relatedParent  +
        '</div>' +
        '<div style="color:#000">' + bodyText + '</div>' +
        '</div>';

    //if (rowData.IsNotify == false) {//(rowData.RemindId > slf.LastId) {
    //    //slf.LastId = rowData.RemindId;
    //    app_messenger.Post(item);
    //    app_reminder.reminder_notify(rowData.RemindId);
    //}

    return item;
};

app_reminder_active.prototype.init = function (onLoadComplete) {

    var slf = this;
    this.ReminderStatus = status;

    var source = {
        datatype: "json",
        id: 'RemindId',
        type: 'POST',
        url: '/System/GetReminderGrid',
        data: { 'Mode': 1 }
    }
    var DataAdapter = new $.jqx.dataAdapter(source, {
        loadComplete: function (records) {
            // data is loaded.
            var html = '';//'<div id="reminder_resp"></div>';//"<table border='0'>";

            var items = $("#personal-wiz-items").val();

            var length = records ? records.length : 0;
            if (length == 0) {
                var msg = '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';

                html += msg;
            }
            else {

                var dataItems = records[0].Items;
                if (dataItems == items)
                    return;
                else 
                    $("#personal-wiz-items").val(dataItems);
                

                for (var i = 0; i < length; i++) {
                    var rowData = records[i];

                    var item = app_reminder_active.reminder_item(rowData);
                    html += item;

                    if (rowData.IsNotify == false) {//(rowData.RemindId > slf.LastId) {
                        //slf.LastId = rowData.RemindId;

                        //var itemEs = app.htmlEscape(item);

                        app_messenger.Post(item);
                        app_reminder.reminder_notify(rowData.RemindId);
                    }

                }


                $(slf.TagDiv).html(html);

                if (onLoadComplete)
                    onLoadComplete(length);
            }
        }
    });
    DataAdapter.dataBind();
};

app_reminder_active.app_Notification = function (onLoadComplete) {
    try {
        var app = new app_reminder_active("#app_Notification");
        app.init(onLoadComplete);
    }
    catch (e) {
        errors++;
        app_messenger.Error("Error ocured in app Notification");
    }
}

app_reminder = {

    reminder_notify: function (id) {
        app_query.doDataPost("/System/ReminderNotify", { 'id': id })
        return false;
    },
    reminder_readed: function (id) {
        app_query.doDataPost("/System/ReminderReaded", { 'id': id })
        return false;
    },
    reminder_expired: function (id) {
        app_query.doDataPost("/System/ReminderExpired", { 'id': id })
        return false;
    },
    reminder_message: function (tagDiv, id, caller) {

        var source = {
            datatype: "json",
            id: 'RemindId',
            type: 'POST',
            //url: '/System/GetReminderGrid',
            //data: { 'Mode': 4 }
            url: '/System/GetReminderItem',
            data: { 'id': id }
        }
        var DataAdapter = new $.jqx.dataAdapter(source, {
            loadComplete: function (records) {
                // data is loaded.
                var content = '';
                //var tagId = "remind_message-" + id;
                var title = " הודעה: " + id;

                //var html = '<div id="' + tagId +'" class="panel-area">'+
                //    '<div class="panel-area-title" > הודעה: ' + id + '</div >';

                //$(tagDiv).empty();
                //var panel = $('<div class="panel-header"></div>');
                //panel.append('<span style="float:right">' + title + '</span>');
                //var close = $('<a href="#"><i class="fa fa-close" style="font-size:16px"></i></a>')
                //    .on("click", function (e) {
                //        e.preventDefault();
                //        $(tagDiv).empty();
                //    });
                //panel.append(close);

                var rowData = records;
                if (rowData === undefined) {
                    var msg = '<div style="text-align:center;margin: 5px;border: 0;width: 100%; height: 100%;">לא נמצאו נתונים</div>';

                    content += msg;
                }
                else {

                    var item = app_reminder_active.reminder_item(rowData, caller);
                    content += item;
                    //content += '</div>';

                    //if (rowData.IsNotify == false) {//(rowData.RemindId > slf.LastId) {
                    //    //slf.LastId = rowData.RemindId;
                    //    app_messenger.Post(item);
                    //    app_reminder.reminder_notify(rowData.RemindId);
                    //}

                }

                app_panel.appendPanelPop(tagDiv, content, title);
                //panel.append(content);
                //$(tagDiv).html(panel);

                //$(tagDiv).html(html);
            }
        });
        DataAdapter.dataBind();

    },//end reminder_message

}


