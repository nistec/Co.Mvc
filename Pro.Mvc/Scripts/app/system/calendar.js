
function tasks_edit_view_comment(row) {

    var content = $('#jqxgrid1').jqxGrid('getrowdata', row).CommentText;
    content = content.replace("\n", "<br/>");
    app_jqx.toolTipClick(".task-comment", '<p>'+content+'</p>', 350);
}
function triggerTaskCommentCompleted(data) {
    app_tasks_comment.end(data);
}
function triggerTaskAssignCompleted(data) {
    app_tasks_assign.end(data);
}
function triggerTaskTimerCompleted(data) {
    app_tasks_timer.end(data);
}
function triggerTaskFormCompleted(data) {
    app_tasks_form.end(data);
}
function getCurrentDate(addDays) {
    var date = new Date();
    if (addDays !== undefined && addDays !== 0)
        date.setDate(date.getDate() + addDays);
    return date;
}
function getFirstDayOf(addDays) {
    var date = new Date();
    var diff = date.getDay() * -1;
    date.setDate(date.getDate() + diff);
    if (addDays !== undefined && addDays !== 0)
        date.setDate(date.getDate() + addDays);
    return date.toISOString();
}
function getFirstDayHourOf(timeFrom) {

    var d = timeFrom.split('T')[0];
    var sdatetime=d+'T08:00:00'
    var datetime = new Date(sdatetime);
    return datetime;
}




//============================================================================================ app_scheduler_def
//(function ($) {


////var task_def;

//app_scheduler = {
//    init: function (dataModel, userInfo) {
        
//        return new app_scheduler_def(dataModel, userInfo)
//    }
//};


function app_scheduler_def(dataModel, userInfo) {
    this.TaskId = dataModel.PId;
    this.TaskModel = 'C';
    this.UserInfo = userInfo;
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.Title = 'יומן';//(this.TaskModel == 'E') ? 'כרטיס' : 'משימה';
    this.TimeFrom = getFirstDayOf();//getCurrentDate(-20).toISOString();
    this.TimeTo = getFirstDayOf(7);//getCurrentDate(20).toISOString();
    this.ActiveUser = userInfo.UserId;
    this.AllowEdit = 1;
    this.ViewMode = 1;
    this.Loaded = 0;

    //this.TimeFromStr = dateToISOString(this.TimeFrom);
    //this.TimeToStr = dateToISOString(this.TimeTo);

    $("#AccountId").val(this.AccountId);
    
    var slf = this;
    this.appointments = [];

    // prepare the data
     this.Source =
    {
        dataType: "array",
        //dataType: "json",
        dataFields: [
            { name: 'id', map: "RowId", type: 'number' },
            { name: 'description', map: 'Description', type: 'string' },
            { name: 'location', map: 'Location', type: 'string' },
            { name: 'subject', map: 'Subject', type: 'string' },
            { name: 'status', map: 'Status', type: 'string' },
            { name: 'hidden', map: 'IsHide', type: 'bool' },
            { name: 'readOnly', map: 'IsReadOnly', type: 'bool' },
            { name: 'calendar', map: 'DisplayName', type: 'string' },
            { name: 'tooltip', map: 'CalendarId', type: 'string' },
            { name: 'start', map: 'TimeFrom', type: 'date' },
            { name: 'end', map: 'TimeTo', type: 'date' }
        ],
        id: 'id',
        //type: 'POST',
        //data: { 'from': this.TimeFrom, 'to': this.TimeTo, 'view': this.ViewMode },
        //url: '/System/CalendarGetItems'
        localData: slf.appointments
    };

    var shareDataAdapter = app_jqx.createDataAdapterData("UserId", "ShareUser", '/System/AdShareList', true, { 'id': 3 });
    app_jqxcombos.setComboSourceAdapter("UserId", "ShareUser", "ActiveUser", shareDataAdapter, '200px');
    $("#ActiveUser").on('change', function (event) {
        var args = event.args;
        if (args) {
            var item = args.item;
            var arg = item.label.split('@');
            //$('#UserId').val(arg[0]);
            if (slf.ActiveUser !== item.value)
            {
                slf.ActiveUser = item.value;
                var edit = item.label.split('@')[1];
                slf.AllowEdit = app.ToInt(edit, 0);
                slf.reload();
            }
        }
    });


    this.reload = function () { 

        slf.appointments = new Array();

        $.ajax({
            url: '/System/CalendarGetItems',
            type: 'post',
            dataType: 'json',
            data: { 'from': this.TimeFrom, 'to': this.TimeTo, 'view': this.ViewMode, 'user': this.ActiveUser },
            success: function (record) {
                if (record && record.length > 0) {
                    slf.appointments = record;
                    //for (var i = 0; i < record.length; i++) {
                    //    slf.appointments.push(record[i]);
                    //    //$('#scheduler').jqxScheduler('addAppointment', record[i]);
                    //}
                }
                slf.Source.localData = slf.appointments;
                if (slf.Loaded === 0) {
                    
                    slf.load();
                    //$("#scheduler").jqxScheduler({ 'scrollTop': 280, editDialog: slf.AllowEdit == 1 });
                }
                else {
                    //var newadapter = new $.jqx.dataAdapter(slf.Source);
                    //var adapter = $("#scheduler").jqxScheduler('source');
                    //adapter._source.localData = slf.appointments;
                    //adapter.loadedData = slf.appointments;
                    //adapter.dataBind();
                    $("#scheduler").jqxScheduler({ source: new $.jqx.dataAdapter(slf.Source), editDialog: slf.AllowEdit === 1 });
                    var d = getFirstDayHourOf(slf.TimeFrom);
                    //alert(d.getFullYear() + ' , ' + (d.getMonth()+1) + ', ' + (d.getDate() +1) + ' , ' + d.getHours());
                    $("#scheduler").jqxScheduler('selectCell', new $.jqx.date(d.getFullYear(), d.getMonth() + 1, d.getDate()+1, 12, 0, 0));

                    //$('#scheduler').jqxScheduler('scrollTop', 280);
                }
                
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });

        //app_query.doDataPost('/System/CalendarGetItems', { 'from': this.TimeFrom, 'to': this.TimeTo, 'view': this.ViewMode }, function (record) {
        //    slf.appointments = new Array();

        //    if (record && record.length > 0) {
        //        for (var i = 0; i < record.length; i++) {
        //            slf.appointments.push(record[i]);
        //            //$('#scheduler').jqxScheduler('addAppointment', record[i]);
        //        }
        //    }
        //    //$("#scheduler").jqxScheduler('source').dataBind();
        //    slf.load();
        //});

    };
    this.reload();
    this.load = function () {

       

        //var adapter = new $.jqx.dataAdapter(source, {
        //      loadComplete: function (record) {

        //          //var d = getFirstDayHourOf(slf.TimeFrom);
        //          //////$('#jqxScheduler').jqxScheduler('selectCell', timeselect, false);//, resourceId);
        //          //alert(d.getFullYear() + ' , ' + d.getMonth() + ', ' + d.getDate() + ' , ' + d.getHours());

        //          //$("#scheduler").jqxScheduler('selectCell', new $.jqx.date(d.getFullYear(), d.getMonth() + 1, d.getDate(), 12, 0, 0));

        //          //if (record && record.length > 0)
        //          //{
        //          //    for (var i = 0; i < record.length; i++)
        //          //    {
        //          //        $('#jqxScheduler').jqxScheduler('addAppointment', record[i]);
        //          //    }
                      


        //          //    //var date = record[0].TimeFrom;
        //          //    //var tf = app.formatDateString(date);// date.toISOString();

        //          //    //var date=Date.parse(record[0].TimeFrom)
        //          //    //var tf = date.toISOString();
        //          //    //console.log(tf);
        //          //  //var tf=   record[0].TimeFrom.toString();

        //          //}


        //          //var src = $("#scheduler").jqxScheduler('source');
        //          //var length = src.records.length;
        //          //console.log(length);
        //      },
        //      loadError: function (jqXHR, status, error) {
        //      },
        //      beforeLoadComplete: function (records) {
        //      }
        //  });


        var adapter = new $.jqx.dataAdapter(slf.Source);
        //$('#scheduler').on('bindingComplete', function (event) {

        //    var args = event.args;
        //    console.log("bindingComplete is raised");
           
        //    //var d = getFirstDayHourOf(slf.TimeFrom);
        //    //alert(d.getFullYear() + ' , ' + d.getMonth() + ', ' + d.getDate() + ' , ' + d.getHours());

        //    //$("#scheduler").jqxScheduler('selectCell', new $.jqx.date(d.getFullYear(), d.getMonth() + 1, d.getDate(), 12, 0, 0));
        //});

       

        $("#scheduler").jqxScheduler({
            //date: new $.jqx.date('todayDate'),
            width: '100%',
            height: 600,
            enableHover: false,
            editDialog : slf.AllowEdit === 1,
            editDialogDateTimeFormatString: 'dd/MM/yyyy HH:mm',
            editDialogDateFormatString: 'dd/MM/yyyy',
            rtl: true,
            source: adapter,
            view: 1,
            showLegend: true,
            ready: function () {
                //$("#scheduler").jqxScheduler('ensureAppointmentVisible', 'id1');
                //$("#scheduler").jqxScheduler('ensureVisible', new $.jqx.date(2015, 11, 23));
                //var d = getFirstDayHourOf(slf.TimeFrom);
                //$('#jqxScheduler').jqxScheduler('selectCell', timeselect, false);//, resourceId);
                //alert(d.getFullYear()+' , '+d.getMonth()+', '+d.getDate()+' , '+d.getHours());

                //$("#scheduler").jqxScheduler('selectCell',   new $.jqx.date(d.getFullYear(),d.getMonth()+1,d.getDate(),12,0,0));


            },
            //rendered: function () {
            //    //var src = $("#scheduler").jqxScheduler('source');
            //    //var length = src.records.length;
            //    //console.log(length);
            //   app_dialog.alert("rendered");
            //},
 
            localization: {
                // separator of parts of a date (e.g. '/' in 11/05/1955)
                '/': "/",
                // separator of parts of a time (e.g. ':' in 05:44 PM)
                ':': ":",
                // the first day of the week (0 = Sunday, 1 = Monday, etc)
                firstDay: 0,
                days: {
                    // full day names
                    names: ["ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת"],
                    // abbreviated day names
                    namesAbbr: ["ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת"],
                    // shortest day names
                    namesShort: ["א", "ב", "ג", "ד", "ה", "ו", "ש"]
                },
                months: {
                    // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
                    names: ["ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר", "אוקטובר", "נובמבר", "דצמבר", ""],
                    // abbreviated month names
                    namesAbbr: ["ינו", "פבר", "מרס", "אפר", "מאי", "יונ", "יול", "אוג", "ספט", "אוק", "נוב", "דצמ", ""]
                },
                // AM and PM designators in one of these forms:
                // The usual view, and the upper and lower case versions
                //      [standard,lowercase,uppercase]
                // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
                //      null
                AM: ["AM", "am", "AM"],
                PM: ["PM", "pm", "PM"],
                eras: [
                // eras in reverse chronological order.
                // name: the name of the era in this culture (e.g. A.D., C.E.)
                // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
                // offset: offset in years from gregorian calendar
                { "name": "A.D.", "start": null, "offset": 0 }
                ],
                twoDigitYearMax: 2029,
                patterns: {
                    // short date pattern
                    d: "dd/MM/yyyy",
                    // long date pattern
                    D: "dddd, MMMM dd, yyyy",
                    // short time pattern
                    t: "hh:mm",//"h:mm tt",
                    // long time pattern
                    T: "hh:mm:ss",//"h:mm:ss tt",
                    // long date, short time pattern
                    f: "dddd, MMMM dd, yyyy hh:mm",//"dddd, MMMM dd, yyyy h:mm tt",
                    // long date, long time pattern
                    F: "dddd, MMMM dd, yyyy hh:mm:ss",//"dddd, MMMM dd, yyyy h:mm:ss tt",
                    // month/day pattern
                    M: "MMMM dd",
                    // month/year pattern
                    Y: "yyyy MMMM",
                    // S is a sortable format that does not vary by culture
                    S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
                    // formatting of dates in MySQL DataBases
                    ISO: "yyyy-MM-dd hh:mm:ss",
                    ISO2: "yyyy-MM-dd HH:mm:ss",
                    d1: "dd.MM.yyyy",
                    d2: "dd-MM-yyyy",
                    d3: "dd-MMMM-yyyy",
                    d4: "dd-MM-yy",
                    d5: "H:mm",
                    d6: "HH:mm",
                    d7: "HH:mm tt",
                    d8: "dd/MMMM/yyyy",
                    d9: "MMMM-dd",
                    d10: "MM-dd",
                    d11: "MM-dd-yyyy"
                },
                agendaViewString: "Agenda",
                agendaAllDayString: "all day",
                agendaDateColumn: "Date",
                agendaTimeColumn: "Time",
                agendaAppointmentColumn: "Appointment",
                backString: "אחורה",
                forwardString: "קדימה",
                toolBarPreviousButtonString: "הקודם",
                toolBarNextButtonString: "הבא",
                emptyDataString: "לא נמצאו נתונים",
                loadString: "Loading...",
                clearString: "Clear",
                todayString: "היום",
                dayViewString: "יום",
                weekViewString: "שבוע",
                monthViewString: "חודש",
                timelineDayViewString: "Timeline Day",
                timelineWeekViewString: "Timeline Week",
                timelineMonthViewString: "Timeline Month",
                loadingErrorMessage: "The data is still loading and you cannot set a property or call a method. You can do that once the data binding is completed. jqxScheduler raises the 'bindingComplete' event when the binding is completed.",
                editRecurringAppointmentDialogTitleString: "Edit Recurring Appointment",
                editRecurringAppointmentDialogContentString: "Do you want to edit only this occurrence or the series?",
                editRecurringAppointmentDialogOccurrenceString: "Edit Occurrence",
                editRecurringAppointmentDialogSeriesString: "Edit The Series",
                editDialogTitleString: "עריכת פגישה",
                editDialogCreateTitleString: "קביעת פגישה",
                contextMenuEditAppointmentString: "עריכת פגישה",
                contextMenuCreateAppointmentString: "קביעת פגישה",
                editDialogSubjectString: "נושא",
                editDialogLocationString: "מיקום",
                editDialogFromString: "מ",
                editDialogToString: "עד",
                editDialogAllDayString: "יום שלם",
                editDialogExceptionsString: "Exceptions",
                editDialogResetExceptionsString: "Reset on Save",
                editDialogDescriptionString: "תאור",
                editDialogResourceIdString: "מאת",
                editDialogStatusString: "סטאטוס",
                editDialogColorString: "צבע",
                editDialogColorPlaceHolderString: "Select Color",
                editDialogTimeZoneString: "אזור זמן",
                editDialogSelectTimeZoneString: "בחירת אזור זמן",
                editDialogSaveString: "שמירה",
                editDialogDeleteString: "מחיקה",
                editDialogCancelString: "ביטול",
                editDialogRepeatString: "מחזורי",
                editDialogRepeatEveryString: "חזור בכל",
                editDialogRepeatEveryWeekString: "שבוע(ות)",
                editDialogRepeatEveryYearString: "שנה(ים)",
                editDialogRepeatEveryDayString: "יום(ים)",
                editDialogRepeatNeverString: "אף פעם",
                editDialogRepeatDailyString: "יומי",
                editDialogRepeatWeeklyString: "שבועי",
                editDialogRepeatMonthlyString: "חודשי",
                editDialogRepeatYearlyString: "שנתי",
                editDialogRepeatEveryMonthString: "חודש(ים)",
                editDialogRepeatEveryMonthDayString: "Day",
                editDialogRepeatFirstString: "first",
                editDialogRepeatSecondString: "second",
                editDialogRepeatThirdString: "third",
                editDialogRepeatFourthString: "fourth",
                editDialogRepeatLastString: "last",
                editDialogRepeatEndString: "End",
                editDialogRepeatAfterString: "After",
                editDialogRepeatOnString: "On",
                editDialogRepeatOfString: "of",
                editDialogRepeatOccurrencesString: "occurrence(s)",
                editDialogRepeatSaveString: "Save Occurrence",
                editDialogRepeatSaveSeriesString: "Save Series",
                editDialogRepeatDeleteString: "Delete Occurrence",
                editDialogRepeatDeleteSeriesString: "Delete Series",
                editDialogStatuses:
                {
                    free: "Free",
                    tentative: "Tentative",
                    busy: "Busy",
                    outOfOffice: "Out of Office"
                }
                //loadingErrorMessage: "The data is still loading and you cannot set a property or call a method. You can do that once the data binding is completed. jqxScheduler raises the 'bindingComplete' event when the binding is completed.",
            },
            resources:
            {
                colorScheme: "scheme05",
                dataField: "calendar"
                //source: new $.jqx.dataAdapter(source)
            },
            appointmentDataFields:
            {
                from: "start",
                to: "end",
                id: "id",
                status: "status",
                description: "description",
                location: "place",
                subject: "subject",
                resourceId: "calendar",
                tooltip: "tooltip"
            },
            views:
            [
                //'dayView',
                //'weekView',
                //'monthView',
                { type: "dayView", showWeekends: false, timeRuler: { hidden: false } },
                { type: "weekView", workTime:{
                    fromDayOfWeek: 0,
                    toDayOfWeek: 4,
                    fromHour: 8,
                    toHour: 18
                    }
                },
                {
                    type: "monthView", workTime: {
                        fromDayOfWeek: 1,
                        toDayOfWeek: 5,
                        fromHour: 8,
                        toHour: 18
                    }
                }
            ]
        });

        $('#scheduler').jqxScheduler('scrollTop', 280);
        slf.Loaded = 1;
    };

        


    var dateToISOString = function (strdate) {
        /*
        var d = new Date();
        d + '';                // "Sun Dec 08 2013 18:55:38 GMT+0100"
        d.toDateString();      // "Sun Dec 08 2013"
        d.toISOString();       // "2013-12-08T17:55:38.130Z"
        d.toLocaleDateString() // "8/12/2013" on my system
        d.toLocaleString()     // "8/12/2013 18.55.38" on my system
        d.toUTCString()        // "Sun, 08 Dec 2013 17:55:38 GMT"
        */
        if (strdate === undefined || strdate === null || strdate === '')
            return '';
        if(strdate===typeof(Date))
            return strdate.toLocaleDateString();
        var d = new Date(strdate);
        if (d.toString() === "NaN" || d.toString() === "Invalid Date") {
                if ($.jqx.dataFormat) {
                    f = $.jqx.dataFormat.tryparsedate(new Date(strdate));
                    return f;
               }
            return '';
        }
        return d.toISOString();
    }

    var createAppointment = function (id,ap) {

        var record = { "CalendarId": id, 'RowId': ap.id, 'UserId': slf.ActiveUser, 'Description': ap.description, 'Location': ap.location, 'Subject': ap.subject, 'Status': ap.status, 'DisplayName': ap.resourceId, 'TimeFrom': ap.from.toString(), 'TimeTo': ap.to.toString() };

        return record;
    }

    var syncCalendar=function(args,eventType){

        var from = args.from;
        var to = args.to;
        var date = args.date;

        var fromstr = from.toString();
        var tostr = to.toString();

        var dfrom = from.toDate();
        var dto = to.toDate();
        
        var curfrom = new Date(slf.TimeFrom);
        var curto = new Date(slf.TimeTo);

        
        slf.TimeFrom = dfrom.toISOString();
        slf.TimeTo = dto.toISOString();
        slf.ViewMode = $('#scheduler').jqxScheduler('view');

        slf.reload();

        //var src = $("#scheduler").jqxScheduler('source');
        //if (src == null || src.records.length == 0)
        //    $("#scheduler").jqxScheduler({ 'source': adapter });
        //else {

        //    var adapter = $("#scheduler").jqxScheduler('source');
        //    adapter._source.data = { 'from': slf.TimeFrom, 'to': slf.TimeTo, 'view': slf.ViewMode };
        //adapter.dataBind();
            

            //$("#scheduler").jqxScheduler({ 'source': adapter });
            //$("#scheduler").jqxScheduler('source').dataBind();
        //}
    }
    // events

    $('#scheduler').on('appointmentAdd', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        var record = createAppointment(0, appointment);
        app_query.doDataPost('/System/AppointmentAdd', record);
    });
    $('#scheduler').on('appointmentDelete', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        id= app.toInt(appointment.tooltip);
        var record = createAppointment(id, appointment);
        app_query.doDataPost('/System/AppointmentDelete', record);
    });
    $('#scheduler').on('appointmentChange', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        id = app.toInt(appointment.tooltip);
        var record = createAppointment(id, appointment);
        app_query.doDataPost('/System/AppointmentChange', record);
    });
    

    //$("#scheduler").on('appointmentDelete', function (event) {
    //    var args = event.args;
    //    var appointment = args.appointment;
    //    console.log("appointmentDelete is raised");
    //});

    //$("#scheduler").on('appointmentAdd', function (event) {
    //    var args = event.args;
    //    var appointment = args.appointment;
    //    console.log("appointmentAdd is raised");
    //});

    $("#scheduler").on('appointmentDoubleClick', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        // appointment fields
        // originalData - the bound data.
        // from - jqxDate object which returns when appointment starts.
        // to - jqxDate objet which returns when appointment ends.
        // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
        // resourceId - String which returns the appointment's resouzeId
        // hidden - Boolean which returns whether the appointment is visible.
        // allDay - Boolean which returns whether the appointment is allDay Appointment.
        // resiable - Boolean which returns whether the appointment is resiable Appointment.
        // draggable - Boolean which returns whether the appointment is resiable Appointment.
        // id - String or Number which returns the appointment's ID.
        // subject - String which returns the appointment's subject.
        // location - String which returns the appointment's location.
        // description - String which returns the appointment's description.
        // tooltip - String which returns the appointment's tooltip.

        $("#log").html("appointmentDoubleClick is raised");
    });

    $("#scheduler").on('cellClick', function (event) {
        var args = event.args;
        var cell = args.cell;

        console.log("cellClick is raised");
    });

    $("#scheduler").on('cellDoubleClick', function (event) {
        var args = event.args;
        var cell = args.cell;

        console.log("cellDoubleClick is raised");
    });

    $("#scheduler").on('contextMenuOpen', function (event) {
        var args = event.args;
        var menu = args.menu;
        var appointment = args.appointment;

        console.log("contextMenuOpen is raised");
    });

    $("#scheduler").on('contextMenuClose', function (event) {
        var args = event.args;
        var menu = args.menu;
        var appointment = args.appointment;

        console.log("contextMenuClose is raised");
    });

    $("#scheduler").on('contextMenuItemClick', function (event) {
        var args = event.args;
        var menu = args.menu;
        var appointment = args.appointment;
        var item = args.item;

        console.log("contextMenuItemClick is raised");
    });

    $("#scheduler").on('viewChange', function (event) {
        var args = event.args;
        var from = args.from;
        var to = args.to;
        var date = args.date;

        syncCalendar(args, 'viewChange');

        console.log("viewChange is raised");
    });

    $("#scheduler").on('dateChange', function (event) {
        var args = event.args;
        var from = args.from;
        var to = args.to;
        var date = args.date;
        console.log("dateChange is raised");

        syncCalendar(args, 'dateChange');

        //var fromstr = from.toString();
        //var tostr = to.toString();

        

        ////var jfrom= formatJsonDate(fromstr, "yyy-MM-dd hh:mm:ss");
        ////var jto = formatJsonDate(tostr, "yyy-MM-dd hh:mm:ss");

        ////var sdfrom = dateToISOString(fromstr);
        ////var df = Date.parse(from.toString());
        //var dfrom = from.toDate();
        
        ////var sdto = dateToISOString(tostr);
        ////var dt = Date.parse(to.toString());
        //var dto = to.toDate();
        
        //var curfrom = new Date(slf.TimeFrom);
        //var curto = new Date(slf.TimeTo);

        ////var selection = $("#scheduler").jqxScheduler('getSelection');
        ////var from = selection.from;
        ////var to = selection.to;
        ////console.log(from.toString() + "-" + to.toString());



        ////if (dfrom > curfrom || dfrom < curfrom)//(dfrom > curfrom && dto > curto) || (dfrom < curfrom && dto < curto)) {
        //    slf.TimeFrom = dfrom.toISOString();
        //    slf.TimeTo = dto.toISOString();
        //    var adapter = $("#scheduler").jqxScheduler('source');
        //    adapter._source.data = { 'from': slf.TimeFrom, 'to': slf.TimeTo };
        //    //adapter.dataBind();
        //    $("#scheduler").jqxScheduler('source').dataBind();
            
        ////}
    });

    $("#scheduler").on('contextMenuCreate', function (event) {
        var args = event.args;
        var menu = args.menu;
        var appointment = args.appointment;
        var item = args.item;

        console.log("contextMenuCreate is raised");
    });

    $("#scheduler").on('editRecurrenceDialogOpen', function (event) {
        var args = event.args;
        var dialog = args.dialog;
        var appointment = args.appointment;

        console.log("editRecurrenceDialogOpen is raised");
    });

    $("#scheduler").on('editRecurrenceDialogClose', function (event) {
        var args = event.args;
        var dialog = args.dialog;
        var appointment = args.appointment;

        console.log("editRecurrenceDialogClose is raised");
    });

    $("#scheduler").on('editDialogCreate', function (event) {
        var args = event.args;
        var dialog = args.dialog;
        var appointment = args.appointment;
        var fields = args.fields;

        console.log("editDialogCreate is raised");
    });

    $("#scheduler").on('editDialogOpen', function (event) {
        var args = event.args;
        var dialog = args.dialog;
        var appointment = args.appointment;
        var fields = args.fields;

        console.log("editDialogOpen is raised");
    });

    $("#scheduler").on('editDialogClose', function (event) {
        var args = event.args;
        var dialog = args.dialog;
        var appointment = args.appointment;
        var fields = args.fields;

        console.log("editDialogClose is raised");
    });


    //$("#scheduler").on('appointmentChange', function (event) {
    //    var args = event.args;
    //    var appointment = args.appointment;
    //    // appointment fields
    //    // originalData - the bound data.
    //    // from - jqxDate object which returns when appointment starts.
    //    // to - jqxDate objet which returns when appointment ends.
    //    // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
    //    // resourceId - String which returns the appointment's resouzeId
    //    // hidden - Boolean which returns whether the appointment is visible.
    //    // allDay - Boolean which returns whether the appointment is allDay Appointment.
    //    // resiable - Boolean which returns whether the appointment is resiable Appointment.
    //    // draggable - Boolean which returns whether the appointment is resiable Appointment.
    //    // id - String or Number which returns the appointment's ID.
    //    // subject - String which returns the appointment's subject.
    //    // location - String which returns the appointment's location.
    //    // description - String which returns the appointment's description.
    //    // tooltip - String which returns the appointment's tooltip.

    //    console.log("appointmentChange is raised");
    //});

    $("#scheduler").on('appointmentClick', function (event) {
        var args = event.args;
        var appointment = args.appointment;
        // appointment fields
        // originalData - the bound data.
        // from - jqxDate object which returns when appointment starts.
        // to - jqxDate objet which returns when appointment ends.
        // status - String which returns the appointment's status("busy", "tentative", "outOfOffice", "free", "").
        // resourceId - String which returns the appointment's resouzeId
        // hidden - Boolean which returns whether the appointment is visible.
        // allDay - Boolean which returns whether the appointment is allDay Appointment.
        // resiable - Boolean which returns whether the appointment is resiable Appointment.
        // draggable - Boolean which returns whether the appointment is resiable Appointment.
        // id - String or Number which returns the appointment's ID.
        // subject - String which returns the appointment's subject.
        // location - String which returns the appointment's location.
        // description - String which returns the appointment's description.
        // tooltip - String which returns the appointment's tooltip.

        console.log("appointmentClick is raised");
    });

    return this;
};


//})(jQuery)

