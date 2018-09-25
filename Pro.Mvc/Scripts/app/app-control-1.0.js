//============================================================================================ app_common

var app_common = {

    refreshEntity: function (entity, callback) {
        app_query.doPost("/Common/RefreshEntity", { 'entity': entity }, callback);
    }
};

//============================================================================================ app_style
var app_style = {

    langAlign: function (lang) {

        switch (lang) {
            case "en":
                return 'left';
            default:
                return 'right';
        }
    }
};

//============================================================================================ app_rout

var app_open = {

    taskInfo: function (tag,id) {
        var window = new window_task_info(tag, id, 'T');
    },
    topicInfo: function (tag, id) {
        var window = new window_task_info(tag, id, 'P');
    },
    ticketInfo: function (tag, id) {
        var window = new window_task_info(tag, id, 'E');
    },
    docInfo: function (tag, id) {
        var window = new window_doc_info(tag, id);
    },
    modelInfo: function (tag,id, model) {
        switch (model) {
            case "T":
                taskInfo(tag,id); break;
            case "P":
                topicInfo(tag,id); break;
            case "E":
                ticketInfo(tag,id); break;
            case "D":
                docInfo(tag,id); break;
        }
    },
    modelEdit: function (id, model) {
        switch (model) {
            case "T":
                taskEdit(id); break;
            case "P":
                topicEdit(id); break;
            case "E":
                ticketEdit(id); break;
            case "D":
                docEdit(id); break;
        }
    },
    
    taskEdit: function (id) {
        app.redirectTo('/System/TaskEdit?id=' + id);
    },
    topicEdit: function (id) {
        app.redirectTo('/System/TopicEdit?id=' + id);
    },
    ticketEdit: function (id) {
        app.redirectTo('/System/TicketEdit?id=' + id);
    },
    docEdit: function (id) {
        app.redirectTo('/System/DocEdit?id=' + id);
    },
    taskNew: function (pid) {
        app.redirectTo('/System/TaskNew?pid=' + pid);
    },
    topicNew: function (pid) {
        app.redirectTo('/System/TopicNew?pid=' + pid);
    },
    ticketNew: function (pid) {
        app.redirectTo('/System/TicketNew?pid=' + pid);
    },
    docNew: function (pid) {
        app.redirectTo('/System/DocNew?pid=' + pid);
    },
};

//============================================================================================ app_rout

var app_rout = {

    isAllowEdit: function (allowEdit) {
        if (allowEdit === 0) {
            app_dialog.alert('You have no permission for this action.');
        }
    },

    redirectToMembers: function () {
        app.redirectTo("/Co/Members");
    }

};
//============================================================================================ app_lookups

var app_lookups = {
    setInput: function (form, tagName, value) {

        switch (tagName) {
            case "UserId":
            case "AssignBy":
                $('form#' + form + ' [name=' + tagName + ']').val(value);
                app_lookup.user_name('#' + tagName, value);
                break;
        }
    },
    lookup: function (tag,url, id) {

        if (id && id > 0)
            app_query.doLookup(url, { 'id': id }, function (content) {
                $(tag).val(content);
            });
    },
    user_role: function (form, tagLabel, tagValue, value) {

        app_query.doLookup('/System/GetUsersRoles', null, function (records) {

            var item = $.grep(records, function (item) {
                    return item.RoleId == value;
            });
            if (item && item.length > 0) {
                $('#' + tagLabel).val(item[0].RoleName);
                $('form#' + form + ' [name=' + tagValue + ']').val(value);
            }
        });

            //app_select.lookupFieldValue(form, tag, record, value)


            //$('form#' + form + ' [name=' + tagValue + ']').val(record.RoleId);
            //    $('#'+tagLabel).val(content.RoleName);
            //});
    },
    user_name: function (tag, id) {
        app_lookups.lookup(tag,'/Common/Lookup_UserDisplay', id);
    },
    member_name: function (tag,id) {
        app_lookups.lookup(tag,'/Common/Lookup_MemberDisplay', id);
    },
    project_name: function (tag,id) {
        app_lookups.lookup(tag,'/System/Lookup_ProjectName', id);
    }
};


//============================================================================================ app_select_form
var app_select_loader = {

    loadTag: function (tagName,tagId, args, selectValue, readonly) {

        switch (tagName) {
            case "Task_Type":
                app_control.select2Ajax('#' + tagId, 240, null, '/System/GetListsSelect', { 'model': args }, selectValue, readonly);
                break;
            case "Doc_Type":
                app_control.select2Ajax('#' + tagId, 240, null, '/System/GetListsSelect', { 'model': 7 }, selectValue, readonly);
                break;
        }
    }
};

//============================================================================================ app_select

var app_select = {
 
    lookupFieldValue: function (form, tag, records, value) {
        var tag = tag.replace('#', '');
        var item = $.grep(records, function (item) { return item.value == value; });
        if (item && item.length > 0) {
            $('#' + tag).val(item[0].text);
            $('form#' + form + ' [name=' + tag + ']').val(item[0].value);
        }
    },
    loadSelect: function (tag, selectValues, width, value, lookupForm) {
        if (lookupForm && value >= 0) {
            app_select.lookupFieldValue(lookupForm, tag, selectValues, value);
        }
        else {
            if (width === undefined || width == 0)
                width = 120;
            app_control.selectTag(tag, width);
            app_control.appendSelectOptions(tag, selectValues);
            if (value >= 0)
                $(tag).val(value);
        }
    },
    loadSelectInput: function (form, tag, selectValues, width, value) {
        if (width === undefined || width == 0)
            width = 120;
        app_control.selectTag(tag, width);
        app_control.appendSelectOptions(tag, selectValues);
        $(tag).change(function () {
            var tagname = tag.replace('#', '');
            $('form#' + form + ' [name=' + tagname + ']').val(this.value);
        });
        if (value >= 0)
            $(tag).val(value);
    },
    TaskStatus: function (form, tag, value) {
        if (tag === undefined)
            tag = "#Status";
        var selectValues = { "1": "פתוח", "2": "בטיפול", "4": "מושהה", "8": "מבוטל", "16": "סגור" };
        app_select.loadSelectInput(form, tag, selectValues, 0, value);
    },
    AdsStatus: function (form, tag, value) {
        if (tag === undefined)
            tag = "#Status";
        var selectValues = { "0": "ממתין", "1": "פעיל", "2": "דחוי" };
        app_select.loadSelectInput(form, tag, selectValues, 0, value);
    },
    PriceType: function (tag, value) {
        if (tag === undefined)
            tag = "#PriceType";
        var selectValues = { "1": "מעטפת", "2": "גמר מלא" };
        app_select.loadSelect(tag, selectValues, 150, value);
    },
    ManagementFeeType: function (tag, value) {
        if (tag === undefined)
            tag = "#ManagementFeeType";
        var selectValues = { "0": "לא ידוע", "1": "אין", "2": "ידוע" };
        app_select.loadSelect(tag, selectValues, 150, value);
    },
    OwnerType: function (tag, value, lookup) {
        if (tag === undefined)
            tag = "#OwnerType";
        var selectValues = { "1": "פרטית", "2": "מנהל", "3": "אחר" };
        app_select.loadSelect(tag, selectValues, 0, value, lookup);
    },
    PropertyType: function (tag, value, lookup) {
        if (tag === undefined)
            tag = "#PropertyType";
        var selectValues = { "0": "יחידה", "1": "עדכון מידע", "2": "סוכן", "3": "מגרש" };
        app_select.loadSelect(tag, selectValues, 0, value, lookup);
    },
    TransType: function (tag, value, lookup) {
        if (tag === undefined)
            tag = "#TransType";
        var selectValues = { "1": "עסקה מצד הדייר/רוכש", "2": "עסקה מצד בעל הנכס/מוכר", "3": "עסקת ייעוץ" };
        app_select.loadSelect(tag, selectValues, 0, value, lookup);
    },
    enumTypes_load: function (tag, model, selectValue) {

        app_control.selectTag(tag, 200);
        app_control.fillSelect(tag, '/System/GetEnumTypesList', { 'model': model }, "PropId", "PropName", selectValue);
    },
};
//============================================================================================ app_task

var app_tasks = {
    taskEdit: function (id, model) {
        var obj = model === undefined ? 'Task': app_tasks.taskModelObject(model);
        app.redirectTo('/System/' + obj +'Edit?id=' + id);
    },
    taskInfo: function (id, model) {
        var obj = model === undefined ? 'Task' : app_tasks.taskModelObject(model);
        app.redirectTo('/System/' + obj +'Info?id=' + id);
    },
    //topicEdit: function (id, model) {
    //    var obj = model === undefined ? 'Task' : app_tasks.taskModelObject(model);
    //    app.redirectTo('/System/' + obj + 'Edit?id=' + id);
    //},
    //topicInfo: function (id, model) {
    //    var obj = model === undefined ? 'Task' : app_tasks.taskModelObject(model);
    //    app.redirectTo('/System/' + obj + 'Info?id=' + id);
    //},
    //ticketEdit: function (id, model) {
    //    var obj = model === undefined ? 'Task' : app_tasks.taskModelObject(model);
    //    app.redirectTo('/System/' + obj + 'Edit?id=' + id);
    //},
    //ticketInfo: function (id, model) {
    //    var obj = model === undefined ? 'Task' : app_tasks.taskModelObject(model);
    //    app.redirectTo('/System/' + obj + 'Info?id=' + id);
    //},
    tasks_getStatus: function (arg_state) {
        if (arg_state == 'done')
            return 16;
        else if (arg_state == 'work')
            return 2;
        else if (arg_state == 'new')
            return 1;
        else if (arg_state == 'today')
            return 0;
        else
            return -1;
    },
    open_task: function (status, id, model) {

        switch (model) {
            case 'N':
            case 'T':
                if (status >= 8)
                    app.redirectTo('/System/TaskInfo?id=' + id);
                else if (status > 1 && status < 8)
                    app.redirectTo('/System/TaskEdit?id=' + id);
                else if (status == 0)//today
                    app.redirectTo('/System/TaskStart?id=' + id);
                else if (status <= 1) {
                    app_dialog.confirm("האם לאתחל משימה?", function () {
                        app.redirectTo('/System/TaskStart?id=' + id);
                    });

                    //if (confirm("האם לאתחל משימה?"))
                    //    app.redirectTo('/System/TaskStart?id=' + id);
                }

                break;
            case 'E':
                if (status >= 8)
                    app.redirectTo('/System/TicketInfo?id=' + id);
                else if (status > 1 && status < 8)
                    app.redirectTo('/System/TicketEdit?id=' + id);
                else if (status == 0) //today
                    app.redirectTo('/System/TicketStart?id=' + id);
                else if (status <= 1) {
                    app_dialog.confirm("האם לאתחל כרטיס?", function () {
                        app.redirectTo('/System/TicketStart?id=' + id);
                    });
                }
                //if (status >= 8)
                //    app.redirectTo('/System/TicketInfo?id=' + id);
                //else
                //    app.redirectTo('/System/TicketEdit?id=' + id);
                break;
            case 'R':
                if (status >= 8)
                    app.redirectTo('/System/ReminderInfo?id=' + id);
                else
                    app.redirectTo('/System/ReminderEdit?id=' + id);
                break;
            case 'C':
                break;
        }
        //return false;
    },
    open_task_info: function (status, id, model) {

        switch (model) {
            case 'N':
            case 'T':
                app.redirectTo('/System/TaskInfo?id=' + id);
                break;
            case 'E':
                app.redirectTo('/System/TicketInfo?id=' + id);
                break;
            case 'R':
                app.redirectTo('/System/ReminderInfo?id=' + id);
                break;
            case 'C':
                break;
        }
    },
    task_expired: function (status, id, model) {

        var item = '';
        switch (model) {
            case 'N':
            case 'T':
                item = 'משימה';
                break;
            case 'E':
                item = 'כרטיס';
                break;
            case 'R':
                item = 'תזכורת';
                break;
            case 'C':
                break;
        }

        if (model !== 'R' && status !== 16) {
            app_dialog.alert('לא ניתן לסגור ' + item);
            return;
        }

        app_dialog.confirm('האם לסגור ' + item, function (id) {

            //alert('expired ' + id)

            app_query.doDataPost('/System/TaskExpired', { 'TaskId': id }, function (data) {

                if (data.Status > 0) {
                    //app_tasks_active.reload();
                    //app_tasks_active.remove(id, true);
                }
            });

        }, id);

    },
    getTaskModelName: function (model) {
        switch (model) {
            case 'N':
            case 'T':
                return 'משימה';
            case 'E':
                return 'כרטיס';
            case 'R':
                return 'תזכורות';
            case 'C':
                return 'יומן';
        }
        return 'NA'
    },
    getTaskModelIcon: function (model) {
        switch (model) {
            case 'N':
            case 'T':
                return '../../Images/icons/task-24.png';
            case 'E':
                return '../../Images/icons/event-gold-24.png';
            case 'R':
                return '../../Images/icons/comment-green-24.png';
            case 'C':
                return '../../Images/icons/vi-orange-24.png';
        }
        return '../../Images/icons/task-24.png'
    },
    taskTitle:function(taskModel){
        switch (taskModel) {
            case "E":
                return 'כרטיס';
            case "T":
                return 'משימה';
            case "P":
                return 'סוגיה';
            case "R":
                return 'תזכורת';
            case "C":
                return 'יומן';
            case "D":
                return 'מסמך';
            default:
                return 'משימה';
        }
    },
    setTaskStatus: function (tag, value) {
        $(tag).val(value);
        var status = app_tasks.taskStatus(value);
        $(tag + '-display').text(status);
        $(tag + '-color').css('color', app_tasks.statusColor(status));
    },
    taskStatus: function (value) {
        /*
1	Open	פתוח	O	new	    #e82121
2	Started	בטיפול	S	work	#f0f20c
4	Paused	מושהה	P	work	#f19b60
8	None	מבוטל	N	done	#ba5cdb
16	Close	סגור	C	done	#6bbd49
        */
        switch (value) {
            case 1:
                return 'פתוח';
            case 2:
                return 'בטיפול';
            case 4:
                return 'מושהה';
            case 8:
                return 'מבוטל ';
            case 16:
                return 'סגור';
            default:
                return 'פתוח';
        }
    },
    statusColor:function(value){
        /*
1	Open	פתוח	O	new	    #e82121
2	Started	בטיפול	S	work	#f0f20c
4	Paused	מושהה	P	work	#f19b60
8	None	מבוטל	N	done	#ba5cdb
16	Close	סגור	C	done	#6bbd49

        */

        switch (value) {
            case "1":
            case "Open":
            case "פתוח":
                return '#e82121';
            case "2":
            case "Started":
            case "בטיפול":
                return '#f0f20c';
            case "4":
            case "Paused":
            case "מושהה":
                return '#f19b60';
            case "8":
            case "None":
            case "מבוטל":
                return '#ba5cdb ';
            case "16":
            case "Close":
            case "סגור":
                return '#6bbd49';
            default:
                return '#e82121';
        }
    },
    setPermsType: function (tag) {

        if (tag === undefined)
            tag = "#PermsType";
        var selectValues = { "0": "ללא", "1": "צוות", "2": "פרטי" };//, "3": "לפי בחירה" };
        app_control.selectTag(tag,120);
        app_control.appendSelectOptions(tag, selectValues);
    },
    setShareType: function (tag) {

        if (tag === undefined)
            tag = "#ShareType";
        var selectValues = { "0": "ללא", "1": "צוות", "2": "פרטי", "3": "לפי בחירה" };
        app_control.selectTag(tag,120);
        app_control.appendSelectOptions(tag, selectValues);
    },
    setColorFlag: function (tag) {
        if (tag === undefined)
            tag = "#ColorFlag";

        var selectValues = { "#46d6db": "Turquoise", "#7bd148": "Green", "#5484ed": "Bold blue", "#a4bdfc": "Blue", "#7ae7bf": "Light green", "#51b749": "Bold green", "#fbd75b": "Yellow", "#ffb878": "Orange", "#ff887c": "Red", "#dc2127": "Bold red", "#dbadff": "Purple" };
        app_control.appendSelectOptions(tag, selectValues);
    },
    taskModelObject: function (model) {
        /*
        TaskModel	0	T	Task	משימה
        TaskModel	1	E	Ticket	כרטיס
        TaskModel	2	R	Reminder	תזכורות
        TaskModel	3	C	Calendar	יומן
        TaskModel	4	P	Topic	סוגיה
        */
        switch (model) {
            case "T":
                return "Task";
            case "P":
                return "Topic";
            case "E":
                return "Ticket";
            case "R":
                return "Reminder";
            case "C":
                return "Calendar";
            case "D":
                return "Doc";
            default:
                return "Task";
        }
    },
    enumTypes_load: function (tag, model, selectValue) {

        app_control.selectTag(tag,200);
        app_control.fillSelect(tag, '/System/GetEnumTypesList', { 'model': model }, "PropId", "PropName", selectValue); 

        //app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag, '/System/GetEnumTypesList', { 'model': model }, 0, 120, true, null, function (status, records) {
        //    if (field && record[field] >= 0)
        //        $(tag).val(record[field]);
        //});
    }
};

//============================================================================================ app_ComboDlg


//function app_ComboMembersDlg(tagName) {

//    var dlg= new app_ComboDlg(tagName);
//    dlg.init("בחירת לקוח\\מנוי", "RecordId", "DisplayName", '/Common/GetMemberDlg');
//    return dlg;
//}

function app_ComboDlg(tagName,tagDisplay,callback) {
    this.selectedItem;
    this.inputTag = tagName + '-combo';
    this.submitTag = tagName + '-submit';
    this.windowTag = tagName + '-window';
    this.tagId = tagName.replace('#', '');
    if (tagDisplay === undefined || tagDisplay === null)
        tagDisplay = tagName + '-display';
    else

    this.title = '';

    this.initList = function (url, hideArrow) {
        this.init("Value", "Label", url, hideArrow);
    },
    this.init = function (valueField, displayField, url,type, hideArrow) {
        var slf = this;
        //var selectedItem;
        this.title = $(tagName + "-name").text();
        var container = $(tagName + "-container");
        var window = $('<div id="' + this.tagId + '-window"></div>');
        //window.append('<div id="' + this.tagId + '-title"></div>');
        window.append('<div id="' + this.tagId + '-combo"></div>');
        var button = $('<a href="#" id="' + this.tagId + '-submit" style="float:left">click Ok or Type Enter <i class="fa fa-check-square" style="font-size:20px" title="Ok"></i></a>')
        .on('click', function (ev) {
            slf.select();
        });
        window.append(button);
        container.append(window);

        var source =
        {
            datatype: "json",
            type: 'POST',
            data:{'type':type},
            url: url
        };
        var dataAdapter = new $.jqx.dataAdapter(source);

        app_jqxcombos.setComboSourceAdapter(valueField, displayField, this.inputTag, dataAdapter, '100%','200px');//, dropDownHeight, async) {
        if (hideArrow)
            $(slf.inputTag).jqxComboBox({ showArrow: false });
        //$(tagName + "-title").text(title);
       

        $(this.inputTag).on('keypress', function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode === '13') {
                slf.select();
            }
        })
 
        
        $(this.inputTag).on('select', function (event) {
            var args = event.args;
            if (args && args.index >= 0) {
                var type = args.type; // keyboard, mouse or null depending on how the item was selected.
                slf.selectedItem = args.item;
                //if (type == 'mouse') {
                //    slf.select();
                //}
                //else if (type == 'keyboard') {
                //    slf.select();
                //}
            }
        });
        
        //$(this.windowTag).on('open', function (event) {
        //    var value = $(tagName).val();
        //    if (value) {
        //        setTimeout(function () {
        //            var item = $(slf.inputTag).jqxComboBox('getItemByValue', value);
        //            $(slf.inputTag).jqxComboBox('selectItem', item);
        //        }, 1000);
        //    }
        //});
    },
    this.display = function () {
        var value = $(tagName).val();
        if (value) {
            var slf = this;
            setTimeout(function () {
                var item = $(slf.inputTag).jqxComboBox('getItemByValue', value);
                $(slf.inputTag).jqxComboBox('selectItem', item);
            }, 1000);
        }
        app_jqx.displayPopover(this.windowTag, tagDisplay, this.title);
        //$(this.inputTag).focus();
        $(this.inputTag).jqxComboBox('focus');

        //$(this.windowTag).jqxWindow('bringToFront');
        //$(this.windowTag).jqxWindow('focus');
    },
     this.select = function () {
         if (this.selectedItem) {
             $(tagName).val(this.selectedItem.value);
             $(tagDisplay).val(this.selectedItem.label);
         }
         else {
             $(tagName).val(0);
             $(tagDisplay).val(null);
         }
         //app_jqx.closeWindow(this.windowTag);
         $(this.windowTag).jqxPopover('close');
         if (callback)
             callback(this.selectedItem);
     },
    this.toggle = function () {
        if ($(this.windowTag).is(':visible'))
            $(this.windowTag).jqxPopover('close');//app_jqx.closeWindow(this.windowTag);
        else {
            this.display();
        }
    }
};

//============================================================================================ app_members

var app_members = {

    displayMemberFields: function (url, data) {

        if (url === undefined || url === null)
            url = '/Common/GetMemberFieldsView';

        $.ajax({
            url: url,
            type: 'post',
            dataType: 'json',
            data: data,
            success: function (data) {

                $(".field-ex").hide();

                if (data) {
                    $("#ExType").val(data.ExType);

                    app_members.displayExField(data.ExId, "ExId");
                    app_members.displayExField(data.ExEnum1, "ExEnum1");
                    app_members.displayExField(data.ExEnum2, "ExEnum2");
                    app_members.displayExField(data.ExEnum3, "ExEnum3");
                    app_members.displayExField(data.ExField1, "ExField1");
                    app_members.displayExField(data.ExField2, "ExField2");
                    app_members.displayExField(data.ExField3, "ExField3");
                    app_members.displayExField(data.ExRef1, "ExRef1");
                    app_members.displayExField(data.ExRef2, "ExRef2");
                    app_members.displayExField(data.ExRef3, "ExRef3");

                    //app.hideOrData("#divEnum1", data.ExEnum1, "");

                    //app_members.displayExField(data.ExId, "#divExId", "#lblExId");
                    //app_members.displayExField(data.ExEnum1, "#divEnum1", "#lblEnum1");
                    //app_members.displayExField(data.ExEnum2, "#divEnum2", "#lblEnum2");
                    //app_members.displayExField(data.ExEnum3, "#divEnum3", "#lblEnum3");
                    //app_members.displayExField(data.ExField1, "#divField1", "#lblExField1");
                    //app_members.displayExField(data.ExField2, "#divField2", "#lblExField2");
                    //app_members.displayExField(data.ExField3, "#divField3", "#lblExField3");
                    //app_members.displayExField(data.ExRef1, "#divExRef1", "#lblExRef1");
                    //app_members.displayExField(data.ExRef2, "#divExRef2", "#lblExRef2");
                    //app_members.displayExField(data.ExRef3, "#divExRef3", "#lblExRef3");

                    /*
					if (data.ExEnum1 == "")
						$("#divEnum1").hide();
					else
						$("#lblEnum1").text(data.ExEnum1);

					if (data.ExEnum2 == "")
						$("#divEnum2").hide();
					else
						$("#lblEnum2").text(data.ExEnum2);

					if (data.ExEnum3 == "")
						$("#divEnum3").hide();
					else
						$("#lblEnum3").text(data.ExEnum3);

					if (data.ExField1 == "")
						$("#divField1").hide();
					else
						$("#lblExField1").text(data.ExField1);

					if (data.ExField2 == "")
						$("#divField2").hide();
					else
						$("#lblExField2").text(data.ExField2);

					if (data.ExField3 == "")
						$("#divField3").hide();
					else
						$("#lblExField3").text(data.ExField3);

					if (data.ExId == "")
						$("#divExId").hide();
					else
					    $("#lblExId").text(data.ExId);

					if (data.ExRef1 == "")
					    $("#divExRef1").hide();
					else
					    $("#lblExRef1").text(data.ExRef1);

					if (data.ExRef2 == "")
					    $("#divExRef2").hide();
					else
					    $("#lblExRef2").text(data.ExRef2);

					if (data.ExRef3 == "")
					    $("#divExRef3").hide();
					else
					    $("#lblExRef3").text(data.ExRef3);
                    */
                }
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
    },
    displayExField: function (exval,fieldname,match) {

        if (exval === "")
            $("#div" + fieldname).hide();
        else {
            $("#lbl" + fieldname).text(exval);
            $("#div" + fieldname).show();
        }
    },
    initMembersDlg: function (tagName) {

        if (tagName === undefined)
            return;

        //var source = $('#' + tagName + '-combo').jqxComboBox('source');

        //if (source != null)
        //{
        //    app_jqx.displayWindow('#' + tagName + '-window', '#' + tagName + '-button');
        //    return;
        //}

        var selectedItem;
        var source =
        {
            datatype: "json",
            datafields: [
                { name: 'RecordId', type: 'number' },
                { name: 'DisplayName', type: 'string' },
                { name: 'AccountId', type: 'number' }
            ],
            url: '/Common/GetMemberDlg',
        };
        var dataAdapter = new $.jqx.dataAdapter(source);

        app_jqxcombos.setComboSourceAdapter("RecordId", "DisplayName", tagName + '-combo', dataAdapter, 240);//, dropDownHeight, async) {

        $('#' + tagName + '-combo').on('select', function (event) {
            var args = event.args;
            if (args) {
                // index represents the item's index.                       
                var index = args.index;
                var item = args.item;
                // get item's label and value.
                var label = item.label;
                var value = item.value;
                var type = args.type; // keyboard, mouse or null depending on how the item was selected.

                selectedItem = item;
                $('#' + tagName).val(item.value);
                $('#' + tagName + '-display').val(item.label);
                app_jqx.closeWindow('#' + tagName + '-window');
            }
        });

        $('#' + tagName + '-button').on('click', function (event) {
            app_jqx.displayWindow('#' + tagName + '-window', '#' + tagName + '-button');
        });
    }

};

//============================================================================================ app_member_edit

var app_member_edit = {

	getrowId: function (gridTag) {

		var selectedrowindex = $(grid).jqxGrid('getselectedrowindex');
		if (selectedrowindex < 0)
			return -1;
		var id = $(gridTag).jqxGrid('getrowid', selectedrowindex);
		return id;
	},
	//load: function (id, userInfo) {
	//	this.TaskId = id;
	//	this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
	//	this.grid(id);
	//	return this;
	//},
	add: function (wizard,wizTab, updateTag) {
		if (updateTag)
			$(updateTag).show();
		wizard.appendIframe(wizTab, app.appPath() + "/Co/_MemberAdd", "100%", "500px");
	},
	edit: function (wizard, recordId, wizTab, updateTag) {
		if (updateTag)
			$(updateTag).show();
		if (recordId > 0)
			wizard.appendIframe(wizTab, app.appPath() + "/Co/_MemberEdit?id=" + recordId, "100%", "500px");
	},
	view: function (wizard, recordId, wizTab, updateTag) {
		if (updateTag)
			$(updateTag).hide();
		if (recordId > 0)
			wizard.appendIframe(wizTab, app.appPath() + "/Co/_MemberView?id=" + id, "100%", "500px");
	},
	remove: function (gridTag) {
		var id = app_member_edit.getrowId(gridTag);
		if (id > 0) {
			if (confirm('האם למחוק מנוי ' + id)) {
				app_query.doPost(app.appPath() + "/Co/TaskAssignDelete", { 'id': id });
				$('#jqxgrid2').jqxGrid('source').dataBind();
			}
		}
	},
	refresh: function (gridTag) {
		$(gridTag).jqxGrid('source').dataBind();
	},
	cancel: function (wizard) {
		wizard.wizHome();
	},
	end: function (wizard,data) {
		wizard.wizHome();
		//wizard.removeIframe(2);
		if (data && data.Status > 0) {
			app_member_edit.refresh();
			app_jqxnotify.Info(data, true);
		}
	}
}
//============================================================================================ app_validator

var app_validator = {
    signupValididty: function (AccountId, MemberId, CellPhone, Email, ExId,funk) {

        $.ajax({
            url: "/Registry/SignupValidity",
            type: 'POST',
            dataType: 'json',
            data: { 'AccountId': AccountId, 'MemberId': MemberId, 'CellPhone': CellPhone, 'Email': Email, 'ExId': ExId },
            success: function (data) {
                funk(data);
            }
        });
    }
};

//============================================================================================ app_popup

var app_popup = {

    memberEdit: function (id) {
        //return popupIframe(app.appPath() + "/Common/_MemberEdit?id=" + id, "500", "600");
        
        return app_dialog.dialogIframe(app.appPath() + "/Common/_MemberEdit?id=" + id, "500", "600","מנוי "+ id);
    },
    managementEdit: function (id) {
        //return popupIframe(app.appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600");
        return app_dialog.dialogIframe(app.appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600", "מנוי " + id);
    },
    cmsHtmlEditor: function (extid) {
        if (app_validation.notForMobile())
            return;
        //return popupIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600");

        return app_dialog.dialogIframe("/Cms/CmsHtmlEdit?extid=" + extid, "850", "600", "Cms Html Editor")
    },
    cmsTextEditor: function (extid) {
        if (app_validation.notForMobile())
            return;
        //return popupIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600");

        return app_dialog.dialogIframe("/Cms/CmsTextEdit?extid=" + extid, "850", "600", "Cms Text Editor")
    },
    cmsPageSettings: function (pageType) {
        if (app_validation.notForMobile())
            return;
        return app_dialog.dialogIframe("/Cms/CmsPageSettings?pageType=" + pageType, "850", "600", "Cms Text Editor")
    },
    cmsAdminPageSettings: function (accountId,pageType) {
        if (app_validation.notForMobile())
            return;
        return app_dialog.dialogIframe("/Cms/CmsAdminPageSettings?accountId=" + accountId + "&pageType=" + pageType, "850", "600", "Cms Text Editor")
    },
    cmsPreview: function (folder, pageType) {
        if (app_validation.notForMobile())
            return;
        var path = "/Preview/" + pageType + "/" + folder;
        return app_dialog.dialogIframe(path, "850", "600", "Cms Preview",true)
    },
    batchMessageView: function (id) {
       
        return app_dialog.dialogIframe(app.appPath() + "/Common/_BatchMessageView?id=" + id, "400", "400", "נוסח הודעה " + id);
    },
    gridView: function (src,title) {
        return app_dialog.dialogIframe(app.appPath() + src, "850", "600", title);
    }
}

//============================================================================================ app_const

var app_const = {

    adminLink: '<a href="/Admin/Manager">מנהל מערכת</a>',
    accountsLink: '<a href="/Admin/DefAccount">ניהול לקוחות</a>'
};

//============================================================================================ app_menu

var app_menu = {

     activeLayoutMenu: function (li) {
        //$("#cssmenu>ul>li.active").removeClass("active");
        //$("#cssmenu>ul>li#" + li).addClass("active");

         $("#mainnav>ul>li.active").removeClass("active");
         $("#mainnav>ul>li#" + li).addClass("active");

    },

    printObject: function (obj) {
        //debugObjectKeys(obj);
        var o = obj;
    },

    breadcrumbs: function (section, page, lang) {

        var breadcrumbs = $(".breadcrumbs");
        breadcrumbs.text('');
        var b = $('<div style="text-align:left;direction:ltr;"></div>')

        var apage = page;

        switch (section.toLowerCase()) {

            case "system":
                apage = '<a href="/System/SystemBoard">' + page + '</a>';
                break;
        }

        var asection = section;

        switch (section.toLowerCase()) {

            case "system":
                asection = '<a href="/System/SystemBoard">' + section + '</a>';
                break;
        }

        //if (page[0] == '@') {
        //    apage = '<a href="/' + section + '/' + page + '/' + section + 'Board">' + page + '</a>';
        //}


        if (lang === undefined || lang === 'en') {
            b.append($('<a href="/home/index">Home</a>'));
            b.append($('<span> >> </span>'));
            b.append($('<a href="/Co/dashboard">Dashboard</a>'));
            b.append($('<span> >> </span>'));
            if (section.substr(0, 1) === '/') {
                b.append($('<a href="' + section + '">' + apage + '</a>'));
                b.append($('<span> | </span>'));
            }
            else
                b.append('' + section + ' >> ' + apage + ' |  ');


            b.append('<a href="javascript:parent.history.back()">Back</a>');

            //var path = document.referrer;
            //var page = app.getUrlPage(path);
            //b.append($('<a href="' + path + '">' + page.split('?')[0] + '</a>'));
            //b.append($('<span> >> </span>'));
            //var curPage = app.getUrlPage(location.href);
            //b.append($('<span> ' + curPage.split('?')[0] + ' </span>'));
        }
        else {
            b.append($('<a href="/home/index">דף הבית</a>'));
            b.append($('<span> >> </span>'));
            b.append($('<a href="/Co/dashboard">ראשי</a>'));
            b.append($('<span> >> </span>'));
            if (section.substr(0, 1) === '/') {
                b.append($('<a href="' + section + '">' + page + '</a>'));
                b.append($('<span> | </span>'));
            }
            else
                b.append('' + section + ' >> ' + page + ' |  ');
            b.append('<a href="javascript:parent.history.back()">חזרה</a>');

        }
        b.appendTo(breadcrumbs);
    },
    footer: function (type, tag) {
        if (tag === undefined)
            tag = 'footer';
        var html = '';
        if (type === 'task') {
             html = (function () {/*
        <div class="container">
        <div class="thumbnail-footer">
            <figure><img src="/images/site/page1_icon1.png" alt=""></figure>
        </div>
        <div class="col-lg-12">
            <ul class="follow_icon rtl">
                <li class="padding-r10">
                    <a href="/System/Calendar" title="יומן">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> יומן
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/TaskUser" title="משימות">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> משימות
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/SystemBoard">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> דוחות
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/AdUsersDef" title="משתמשים">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> משתמשים
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/AdDef" title="קבוצות">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> קבוצות
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/AdTeamDef" title="צוותים">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> צוותים
                    </a>
                </li>
            </ul>
        </div>
        <hr style="width:50%;">
        <div class="col-lg-12">
            <ul class="follow_icon rtl">
                <li class="padding-r10">
                    <a href="/System/TopicNew?pid=0">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> סוגיה
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/TaskNew?pid=0">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> משימה חדשה
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/TicketNew?pid=0">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> כרטיס
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/ReminderNew?pid=0">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> תזכורת
                    </a>
                </li>
                <li class="padding-r10">
                    <a href="/System/ReportTasks">
                        <i class="fa fa-angle-double-left" style="font-size:20px"></i> דוח משימות
                    </a>
                </li>
            </ul>
        </div>
    </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];
        }

        $(tag).html(html);
    }
};


//============================================================================================ app_jqx_list

var app_jqx_list = {

    branchComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Branch" : tag, '/Common/GetBranchView', 0, 120, false) },

    //placeComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "PlaceOfBirth" : tag, '/Common/GetPlaceView', 0, 120, false) },

    statusComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Status" : tag, '/Common/GetStatusView', 0, 0, false) },

    chargeComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Charge" : tag, '/Common/GetChargeView', 0, 0, false) },

    regionComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Region" : tag, '/Common/GetRegionView', 0, 120, false) },

    cityComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "City" : tag, '/Common/GetCityView', 0, 120, false) },

    genderComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Gender" : tag, '/Common/GetGenderView', 150, 0, false) },

    categoryCheckListAdapter: function (tag, output) {
        return app_jqxcombos.createCheckListAdapter("PropId", "PropName", tag === undefined ? "listCategory" : tag, "/Common/GetCategoriesView", 240, 140, false, output === undefined ? "Categories" : output)
    },
    categoryComboCheckAdapter: function (tag, output) { return app_jqxcombos.createComboCheckAdapter("PropId", "PropName", tag === undefined ? "listCategory" : tag, "/Common/GetCategoriesView", 240, 140, false, output === undefined ? "Categories" : output) },

    categoryComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Category" : tag, '/Common/GetCategoriesView', 0, 120, false) },

    entityEnumComboAdapter: function (tag) { return app_jqxcombos.createDropDownAdapter("FieldName", "FieldValue", tag === undefined ? "EntityEnumType" : tag, '/Common/GetMembersEnumFields', 0, 120, false) },

    enum1ComboAdapter: function (tag, async) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "ExEnum1" : tag, '/Common/GetEnum1View', 0, 120, async === undefined ? true: async) },
    enum2ComboAdapter: function (tag, async) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "ExEnum2" : tag, '/Common/GetEnum2View', 0, 120, async === undefined ? true : async) },
    enum3ComboAdapter: function (tag, async) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "ExEnum3" : tag, '/Common/GetEnum3View', 0, 120, async === undefined ? true : async) },
    userRoleComboAdapter: function (tag, async) { return app_jqxcombos.createDropDownAdapter("RoleId", "RoleName", tag === undefined ? "UserRole" : tag, '/Admin/GetUsersRoles', 0, 120, async === undefined ? false : async) },
    campaignComboAdapter: function (tag, async) { return app_jqxcombos.createDropDownAdapterTag("PropId", "PropName", tag === undefined ? "#Campaign" : tag, '/Common/GetCampaignView', 200, 0, async === undefined ? false : async, 'נא לבחור קמפיין') },
    //taskTypeComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Task_Type" : tag, '/System/GetTaskTypeList', 0, 120, false) },
    taskStatusComboAdapter: function (tag, async) { return app_jqxcombos.createDropDownAdapter("PropId", "PropName", tag === undefined ? "TaskStatus" : tag, '/System/GetTaskStatusList', 150, 120, async === undefined ? false : async) }

};



//============================================================================================ app_jqx_combo_async

var app_jqx_combo_async = {

    labelsInputAdapter: function (tag, value, id,source, callback) {
        tag = (tag === undefined || tag == null) ? "#Label" : tag;
        return app_jqx_adapter.createArrayInputAutoAdapterAsync(tag, '/System/GetLabelList', { 'id': id, 'source': source}, 200, 10, null, value, callback);
    },
    docFolderInputAdapter: function (tag, value, callback) {
        tag = (tag === undefined || tag == null) ? "#Folder" : tag;
        return app_jqx_adapter.createArrayInputAutoAdapterAsync(tag, '/System/GetTaskFolderList', null, 200, 10, null, value, callback);
    },
    systemLookupInputAdapter: function (tag, type, value, callback) {
        var adapter = app_jqx_adapter.createInputAutoAdapterAsync("Value", "Label", tag, '/System/Lookup_DisplayList', { 'type': type }, 0, 10, null, value, callback);
        return adapter;
    },
    lookupInputAdapter: function (tag, type, value, callback) {
        var adapter = app_jqx_adapter.createInputAutoAdapterAsync("Value", "Label", tag, '/Common/Lookup_GetList', { 'type': type }, 0, 10, null, value, callback);
        return adapter;
    },
    branchComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Branch" : tag, '/Common/GetBranchView',null, 0, 120, true, value, callback)
    },

    //placeComboAdapter: function (tag) { return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "PlaceOfBirth" : tag, '/Common/GetPlaceView', 0, 120, false) },

    statusComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Status" : tag, '/Common/GetStatusView', null, 0, 0, true, value, callback)
    },
    chargeComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Charge" : tag, '/Common/GetChargeView', null, 0, 0, true, value, callback)
    },

    regionComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Region" : tag, '/Common/GetRegionView', null, 0, 120, true, value, callback)
    },

    cityComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "City" : tag, '/Common/GetCityView', null, 150, 120, true, value, callback)
    },

    genderComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Gender" : tag, '/Common/GetGenderView', null, 150, 0, true, value, callback)
    },

    categoryCheckListAdapter: function (tag, output) {
            return app_jqx_adapter.createListAdapterAsync("PropId", "PropName", tag === undefined ? "listCategory" : tag, "/Common/GetCategoriesView",null, 240, 140, true, output === undefined ? "Categories" : output,value, callback)
    },
    categoryComboCheckAdapter: function (tag, output) {
        return app_jqxcombos.createComboCheckAdapter("PropId", "PropName", tag === undefined ? "listCategory" : tag, "/Common/GetCategoriesView", null, 240, 140, false, output === undefined ? "Categories" : output)
    },

    categoryComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Category" : tag, '/Common/GetCategoriesView', null, 0, 120, true, value, callback)
    },

    entityEnumComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createDropDownAdapterAsync("FieldName", "FieldValue", tag === undefined ? "EntityEnumType" : tag, '/Common/GetMembersEnumFields', null, 0, 120, null, value, callback)
    },

    enum1ComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "ExEnum1" : tag, '/Common/GetEnum1View', null, 0, 120, true, value, callback)
    },
    enum2ComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "ExEnum2" : tag, '/Common/GetEnum2View', null, 0, 120, true, value, callback)
    },
    enum3ComboAdapter: function (tag, value, callback) { return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "ExEnum3" : tag, '/Common/GetEnum3View', 0, 120, true, value, callback) },
    userRoleComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createDropDownAdapterAsync("RoleId", "RoleName", tag === undefined ? "UserRole" : tag, '/Admin/GetUsersRoles', 0, 120, null, value, callback)
    },
    campaignComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", tag === undefined ? "#Campaign" : tag, '/Common/GetCampaignView', null, 200, 0, 'נא לבחור קמפיין', value, callback)
    },
    taskTypeComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createComboAdapterAsync("PropId", "PropName", tag === undefined ? "Task_Type" : tag, '/System/GetEnumTypesList', {'model':'T'}, 0, 120, true, value, callback)
    },
    taskStatusInputAdapter: function (tag, value, callback) {
        var adapter = app_jqx_adapter.createInputAutoAdapterAsync("PropId", "PropName", tag === undefined ? "TaskStatus" : tag, '/System/GetTaskStatusList', null, 120, 10, null, value, callback);
        return adapter;
    },
    taskStatusComboAdapter: function (tag, value, callback) {
        return app_jqx_adapter.createDropDownAdapterAsync("PropId", "PropName", tag === undefined ? "TaskStatus" : tag, '/System/GetTaskStatusList', null, 150, 120, null, value, callback)
    },
    userInputAdapter: function (tag, value, callback) {
        var adapter = app_jqx_adapter.createInputAutoAdapterAsync("UserId", "DisplayName", tag, '/System/GetUsersList', null, 0, 10, null, value, callback);
        return adapter;
    },
    projectInputAdapter: function (tag, value, callback) {
        var adapter = app_jqx_adapter.createInputAutoAdapterAsync("ProjectId", "ProjectName", tag, '/System/Lookup_DisplayList', {'type':'project_name'}, 0, 10, null, value, callback);
        return adapter;
    },
    clientInputAdapter: function (tag, value, callback) {
        var adapter = app_jqx_adapter.createInputAutoAdapterAsync("RecordId", "ProjectName", tag, '/System/Lookup_DisplayList', {'type':'members'}, 0, 10, null, value, callback);
        return adapter;
    },
};



var window_main_menu = {

    //var items = [
    //    { label: '<li><a href="/Co/Members" title="מנויים">' + icon + ' רשימת מנויים</a></li>' },
    //    { label: '<li><a href="/Co/Members" title="מנויים">' + icon + ' רשימת מנויים</a></li>' },
    //    { label: '<li><a href="/Co/Members" title="מנויים">' + icon + ' רשימת מנויים</a></li>' },
    //    { label: '<li><a href="/Co/Members" title="מנויים">' + icon + ' רשימת מנויים</a></li>' },
    //    { label: '<li><a href="/Co/Members" title="מנויים">' + icon + ' רשימת מנויים</a></li>' },
    //    { label: '<li><a href="/Co/Members" title="מנויים">' + icon + ' רשימת מנויים</a></li>' },
    //]
    //var array = [1, 1, 1, 0];
    //var ul = '<ul class="submeunav">';

    //array.forEach(function (item,index) {
    //    if (item == 1)
    //        ul += memberList[index].label;
    //});
    //ul += '</ul>';
    //return ul;


    do_menu: function (accName, uname, menus) {

        var menu_main = (function () {/*
      <li id="liMain" class="nbr-has-children overview">
        <a href="#0">מנויים</a>
        <ul class="submeunav">
            <li><a href="/Co/Members" title="מנויים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> רשימת מנויים</a></li>
            <li><a href="/Co/MembersAdd" title="הוספת מנוי"><i class="fa fa-angle-double-left" style="font-size:20px"></i> הוספת מנוי</a></li>
            <li><a href="/Co/MembersUpload" title="קליטה מקובץ"><i class="fa fa-angle-double-left" style="font-size:20px"></i> קליטה מקובץ</a></li>
            <li><a href="/Co/PaymentsQuery" title="תשלומים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> איתור תשלומים</a></li>
        </ul>
    </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var menu_payments = (function () {/*
      <li id="liPay" class="nbr-has-children overview">
        <a href="#0">תשלומים</a>
        <ul class="submeunav">
            <li><a href="/Co/PaymentsQuery" title="תשלומים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> איתור תשלומים</a></li>
            <li><a href="/Common/DefPrice" title="מחירון"><i class="fa fa-angle-double-left" style="font-size:20px"></i> מחירון</a></li>
            <li><a href="/Common/DefCampaign" title="קמפיין"><i class="fa fa-angle-double-left" style="font-size:20px"></i> קמפיין</a></li>
        </ul>
    </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var menu_comm = (function () {/*
      <li id="liCom" class="nbr-has-children notifications">
        <a href="#0">תקשורת</a>
        <ul class="submeunav">
            <li><a href="/Co/SmsBroadcast" title="שליחת מסרון"><i class="fa fa-angle-double-left" style="font-size:20px"></i> שליחת מסרון</a></li>
            <li><a href="/Co/EmailBroadcast" title="שליחת דואר אלקטרוני"><i class="fa fa-angle-double-left" style="font-size:20px"></i> שליחת דואר אלקטרוני</a></li>
            <li><a href="/Co/SendQuery" title="דוחות דיוור"><i class="fa fa-angle-double-left" style="font-size:20px"></i> דוחות דיוור</a></li>
        </ul>
     </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var menu_def = (function () {/*
      <li id="liEntity" class="nbr-has-children comments">
        <a href="#0">הגדרות</a>
        <ul class="submeunav">
            <li><a href="/Common/DefCategory" title="סיווגים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> סיווגים</a></li>
            <li><a href="/Common/DefEntity?entity=branch" title="הגדרות כלליות"><i class="fa fa-angle-double-left" style="font-size:20px"></i> הגדרות כלליות</a></li>
            <li><a href="/Common/DefCity" title="יישובים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> יישובים</a></li>
        </ul>
     </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var menu_report = (function () {/*
      <li id="liReport" class="nbr-has-children bookmarks">
        <a href="#0">הגדרות</a>
        <ul class="submeunav">
            <li><a href="/Report/ReportQuery" title="דוחות התפלגות נתונים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> דוחות התפלגות נתונים</a></li>
        </ul>
     </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var menu_cms = (function () {/*
      <li id="liCms" class="nbr-has-children images">
        <a href="#0">מערכת תוכן</a>
        <ul class="submeunav">
            <li><a href="/Cms/CmsWizardPages" title="עריכת תוכן כללי"><i class="fa fa-angle-double-left" style="font-size:20px"></i> עריכת תוכן כללי</a></li>
            <li><a href="/Cms/CmsWizardForm" title="עריכת טופס רישום"><i class="fa fa-angle-double-left" style="font-size:20px"></i> עריכת טופס רישום</a></li>
            <li><a href="/Cms/CmsMedia" title="מדיה"><i class="fa fa-angle-double-left" style="font-size:20px"></i> מדיה</a></li>
        </ul>
     </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var menu_system = (function () {/*
      <li id="liSystem" class="nbr-has-children users">
        <a id="liSystem-a" href="#0">מערכת ניהול</a>
        <ul class="submeunav">
            <li><a href="/System/Calendar" title="יומן"><i class="fa fa-angle-double-left" style="font-size:20px"></i> יומן</a></li>
            <li><a href="/System/TaskUser" title="משימות"><i class="fa fa-angle-double-left" style="font-size:20px"></i> משימות</a></li>
            <li><a href="/System/AdUsersDef" title="משתמשים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> משתמשים</a></li>
            <li><a href="/System/AdDef" title="קבוצות"><i class="fa fa-angle-double-left" style="font-size:20px"></i> קבוצות</a></li>
            <li><a href="/System/AdTeamDef" title="צוותים"><i class="fa fa-angle-double-left" style="font-size:20px"></i> צוותים</a></li>
            <li><a href="/Account/Logoff" title="יציאה"><i class="fa fa-angle-double-left" style="font-size:20px"></i> יציאה</a></li>
        </ul>
     </li>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        var array = [];
        array.push(menu_main);
        array.push(menu_payments);
        array.push(menu_comm);
        array.push(menu_def);
        array.push(menu_report);
        array.push(menu_cms);
        array.push(menu_system);

        var html = '<ul style="direction:rtl">' +
            '<li class="nbr-label"><label class="nbr-label-text"><span>' + accName + '</span><span>@@</span><span>' + uname + '</span></label></li>';

        var menu_array = menus.split(',');

        menu_array.forEach(function (item, index) {
            if (item == "1")
                html += array[index];
        });

        html += '</ul>';

        $("#mainnav").html(html);
    }
}