co.my-t.co.il
=============


future-version: *
===========================
add-migrate all automation operation to current
add-migrate all tags web pages to current
add-central cellphone commonication
add-customers management operation
add-customer billing operation
add-customer credit payment operation
add-rule engine
add-upload actions
add-project management
add-budget management
add-forms builder
add-bugs and ticket management

next-version: 4.5.0.1_v-3.* due date: 30/06/2017
===========================
add-news management
add-calendar scheduler
add-send sms multimedia
add-cms design library
add-media uploader
add-user registration page trigger by link
add-upload members actions

working-version: 4.5.0.1_v-2.24 due date: 15/05/2016
===============================
task management
add-forgot user password
fix cms form builder
fix merge members category
fix-send email by category direct to ephone
fix-home page,about page and contact page

current-version: 4.5.0.1_v-2.18 : 30/03/2017
===============================
fixed-recharge members
fixed-send sms by category direct to ephone 
fixed-MembersQuery filters for screen and export
fixd-grid filters
fixed-grid filters mode equal and contains
fixed-upload members using columns mapper
fixed-notify credit transaction
fixed-send sms

version: 4.5.0.1_v-2.17
===============================
fixed-send sms and email using api
fixed-change send sms and email to wizard view
fixed-users managements
fixed-statistic reports
fixed-change dashboard to last 30 days


old-versions:
=============






open-bugs:
==========


  loadEntityDataGrid: function (entity, data, tag_grid, pageable) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (pageable === undefined || pageable == null)
            pageable = false;

        var source =
        {
            updaterow: function (rowid, rowdata, commit) {
                editrow = rowid;
                app_jqxtreegrid.doUpdateCommand(rowid, rowdata, commit, 1, entity, tag_grid);
            },
            addrow: function (rowid, rowdata, position, commit) {
                editrow = -1;
                app_jqxtreegrid.doUpdateCommand(rowid, rowdata, commit, 0, entity, tag_grid);
            },
            deleterow: function (rowid, commit) {
                editrow = rowid;
                app_jqxtreegrid.doUpdateCommand(rowid, null, commit, 2, entity, tag_grid);
            },
            dataType: "json",
            datafields: entity.createFields(),
            id: entity.fieldId,
            localdata: data,
            type: 'POST',
            url: entity.sourceUrl
        };

        var dataAdapter = new $.jqx.dataAdapter(source, {
            //contentType: "application/json; charset=utf-8",
            loadError: function (jqXHR, status, error) {
                //alert("dealAdapter failed: " + error);
            },
            loadComplete: function (data) {
                //alert("dealAdapter is Loaded");
            }
        });

        var editrow = -1;
        // initialize jqxTreeGrid
        $(tag_grid).jqxTreeGrid(
        {
            rtl: true,
            width: '100%',//entity.gridWith,
            autoheight: false,
            source: dataAdapter,
            localization: getLocalization('he'),
            pageable: pageable,
            sortable: true,
            showtoolbar: true,
            rendertoolbar: function (toolbar) {
                app_jqxtreegrid.renderToolbar(toolbar, entity)
            },
            columns: entity.createCoulmns()
        });


        $(tag_grid).on('cellselect', function (event) {
            var column = $(tag_grid).jqxTreeGrid('getcolumn', event.args.datafield);
            var value = $(tag_grid).jqxTreeGrid('getcellvalue', event.args.rowindex, column.datafield);
            var displayValue = $(tag_grid).jqxTreeGrid('getcellvalue', event.args.rowindex, column.displayfield);
        });

        $(tag_grid).on('rowdoubleclick', function (event) {
            if (entity.AllowEdit == 0) {
                return;
            };
            var args = event.args;
            var boundIndex = args.rowindex;
            var visibleIndex = args.visibleindex;
            entity.rowEdit = boundIndex;
            app_jqxtreegrid.doRowEdit(boundIndex, entity, tag_grid);
        });
        //$(tag_grid).jqxTreeGrid('autoresizecolumns');

        //$("#popupWindow").on('open', function () {
        //    $("#PropName").jqxInput('selectAll');
        //});

        // update the edited row when the user clicks the 'Save' button.
        $("#Save").click(function () {

            var validationResult = function (isValid) {
                if (isValid) {
                    var row = entity.createRowData();
                    var editrow = entity.rowEdit;
                    if (editrow < 0) {//$("#insertFlag").val() == '0') {
                        $(tag_grid).jqxTreeGrid('addrow', null, row);
                    }
                    else if (editrow >= 0) {
                        var rowID = $(tag_grid).jqxTreeGrid('getrowid', editrow);
                        $(tag_grid).jqxTreeGrid('updaterow', rowID, row);
                    }

                    $("#popupWindow").jqxWindow('hide');
                }
            }
            $('#form').jqxValidator('validate', validationResult);

        });

        if (entity.AllowEdit == 0) {
            $("#Cancel").hide();
            $("#Save").hide();
            $(".note-edit").hide();
        }

        return this;

    },
    loadEntityGrid: function (entity, tag_grid, pageable) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (pageable === undefined || pageable == null)
            pageable = false;
        var source =
        {
            updaterow: function (rowid, rowdata, commit) {
                editrow = rowid;
                app_jqxtreegrid.doUpdateCommand(rowid, rowdata, commit, 1, entity, tag_grid);
            },
            addrow: function (rowid, rowdata, position, commit) {
                editrow = -1;
                app_jqxtreegrid.doUpdateCommand(rowid, rowdata, commit, 0, entity, tag_grid);
            },
            deleterow: function (rowid, commit) {
                editrow = rowid;
                app_jqxtreegrid.doUpdateCommand(rowid, null, commit, 2, entity, tag_grid);
            },
            dataType: "json",
            datafields: entity.createFields(),
            id: entity.fieldId,
            data: entity.sourceData,
            type: 'POST',
            url: entity.sourceUrl
        };

        var dataAdapter = new $.jqx.dataAdapter(source, {
            //contentType: "application/json; charset=utf-8",
            loadError: function (jqXHR, status, error) {
                //alert("dealAdapter failed: " + error);
            },
            loadComplete: function (data) {
                //alert("dealAdapter is Loaded");
            }
        });

        var editrow = -1;
        // initialize jqxTreeGrid
        $(tag_grid).jqxTreeGrid(
        {
            rtl: true,
            width: '100%',//entity.gridWith,
            autoheight: true,
            source: dataAdapter,
            localization: getLocalization('he'),
            pageable: pageable,
            sortable: true,
            showtoolbar: true,
            rendertoolbar: function (toolbar) {
                app_jqxtreegrid.renderToolbar(toolbar, entity)
            },
            columns: entity.createCoulmns()
        });


        $(tag_grid).on('cellselect', function (event) {
            var column = $(tag_grid).jqxTreeGrid('getcolumn', event.args.datafield);
            var value = $(tag_grid).jqxTreeGrid('getcellvalue', event.args.rowindex, column.datafield);
            var displayValue = $(tag_grid).jqxTreeGrid('getcellvalue', event.args.rowindex, column.displayfield);
        });

        $(tag_grid).on('rowdoubleclick', function (event) {
            if (entity.AllowEdit == 0) {
                return;
            };
            var args = event.args;
            var boundIndex = args.rowindex;
            var visibleIndex = args.visibleindex;
            entity.rowEdit = boundIndex;
            app_jqxtreegrid.doRowEdit(boundIndex, entity, tag_grid);
        });
        //$(tag_grid).jqxTreeGrid('autoresizecolumns');

        //$("#popupWindow").on('open', function () {
        //    $("#PropName").jqxInput('selectAll');
        //});

        // update the edited row when the user clicks the 'Save' button.
        $("#Save").click(function () {

            var validationResult = function (isValid) {
                if (isValid) {
                    var row = entity.createRowData();
                    var editrow = entity.rowEdit;
                    if (editrow < 0) {//$("#insertFlag").val() == '0') {
                        $(tag_grid).jqxTreeGrid('addrow', null, row);
                    }
                    else if (editrow >= 0) {
                        var rowID = $(tag_grid).jqxTreeGrid('getrowid', editrow);
                        $(tag_grid).jqxTreeGrid('updaterow', rowID, row);
                    }
                    $("#popupWindow").jqxWindow('hide');
                }
            }
            $('#form').jqxValidator('validate', validationResult);

        });

        if (entity.AllowEdit == 0) {
            $("#Cancel").hide();
            $("#Save").hide();
            $(".note-edit").hide();
        }

        return this;

    },
    renderToolbar: function (toolbar, entity, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        app_jqxtreegrid.createToolbar(toolbar);

        if (entity.AllowEdit == 0) {
            $("#addrowbutton").hide();
            $("#deleterowbutton").hide();
            $("#updaterowbutton").hide();
        }
        // update row.
        $("#updaterowbutton").on('click', function () {
            var selectedrowindex = $(tag_grid).jqxTreeGrid('getselectedrowindex');
            entity.rowEdit = selectedrowindex;
            app_jqxtreegrid.doRowEdit(selectedrowindex, entity, tag_grid);

        });
        // create new row.
        $("#addrowbutton").on('click', function () {
            // show the popup window.
            entity.rowEdit = -1;
            entity.setEditorInputData(null);
            app_jqxtreegrid.openPopupEditor();
        });
        // delete row.
        $("#deleterowbutton").on('click', function () {
            var selectedrowindex = $(tag_grid).jqxTreeGrid('getselectedrowindex');
            entity.rowEdit = selectedrowindex;
            var rowscount = $(tag_grid).jqxTreeGrid('getdatainformation').rowscount;
            if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                var id = $(tag_grid).jqxTreeGrid('getrowid', selectedrowindex);
                var result = confirm("האם למחוק?");
                if (result == true) {
                    //Logic to delete the item
                    var commit = $(tag_grid).jqxTreeGrid('deleterow', id);
                }
            }
        });
        // refresh grid.
        $("#refreshbutton").on('click', function () {
            $(tag_grid).jqxTreeGrid('source').dataBind();
        });
    },
    createToolbar: function (toolbar) {

        var container = $("<div style='margin: 5px;text-align:right'></div>");
        toolbar.append(container);
        container.append('<input id="addrowbutton" type="button" value="הוספה" />');
        container.append('<input style="margin-left: 5px;" id="deleterowbutton" type="button" value="מחיקה" title="מחיקת השורה המסומנת"/>');
        container.append('<input id="updaterowbutton" type="button" value="עריכה" title="עריכת השורה המסומנת"/>');
        container.append('<input id="refreshbutton" type="button" value="רענון" />');
        $("#addrowbutton").jqxButton();
        $("#deleterowbutton").jqxButton();
        $("#updaterowbutton").jqxButton();
        $("#refreshbutton").jqxButton();
    },
    deleteSelectedRow: function (tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        var editrow = -1;
        var selectedrowindex = $(tag_grid).jqxTreeGrid('getselectedrowindex');
        var rowscount = $(tag_grid).jqxTreeGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            editrow = selectedrowindex;
            var id = $(tag_grid).jqxTreeGrid('getrowid', selectedrowindex);
            var result = confirm("האם למחוק?");
            if (result == true) {
                //Logic to delete the item
                var commit = $(tag_grid).jqxTreeGrid('deleterow', id);
            }
        }

        return editrow;
    },
    doUpdateCommand: function (rowid, rowdata, commit, command, entity, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        var data = entity.getDataCommand(rowid, rowdata, command);

        var url;
        if (command == 2)
            url = entity.deleteUrl;
        else if (command == 0)
            url = (entity.insertUrl === undefined || entity.insertUrl == null) ? entity.updateUrl : entity.insertUrl;
        else
            url = entity.updateUrl;


        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: url,
            data: data,
            success: function (data, status, xhr) {
                app_messenger.Post(data);
                if (data.Status > 0)
                    $("#popupWindow").jqxWindow('hide');
                //if (data.Status > 0) {
                //    //dataAdapter.dataBind();
                //    app_jqxnotify.Info('עודכן בהצלחה', true);//app_dialog.alert('עודכן בהצלחה');
                //}
                //else
                //    app_jqxnotify.Warning('לא עודכנו נתונים', true); //app_dialog.alert('לא עודכנו נתונים');
                commit(true);
            },
            complete: function (data) {
                if (data.status == 200)//.Status > 0)
                    $(tag_grid).jqxTreeGrid('source').dataBind();
            },
            error: function () {
                app_messenger.Post('אירעה שגיאה, לא עודכנו נתונים', 'error');
                //app_jqxnotify.Error('אירעה שגיאה, לא עודכנו נתונים'); //app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                // cancel changes.
                commit(false);
            }
        });
    },
    doRowEdit: function (selectedrowindex, entity, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        var editrow = -1;
        var rowscount = $(tag_grid).jqxTreeGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            $(tag_grid).jqxTreeGrid('ensurerowvisible', selectedrowindex);
            // open the popup window when the user clicks a button.
            editrow = selectedrowindex;
            // get the clicked row's data and initialize the input fields.
            var dataRecord = $(tag_grid).jqxTreeGrid('getrowdata', editrow);
            if (dataRecord != null && dataRecord !== undefined) {
                entity.setEditorInputData(dataRecord);
                app_jqxtreegrid.openPopupEditor(null, tag_grid);
            }
            // show the popup window.
            //$("#popupWindow").jqxWindow('open');
            //app_jqxtreegrid.openPopupEditor(null, tag_grid);
        }
        else {
            entity.setEditorInputData(dataRecord);
            app_jqxtreegrid.openPopupEditor(null, tag_grid);
        }
        return editrow;
    },
    sendCommand: function (rowdata, command, commit, url, onsuccess, slf) {


        //var url;
        //if (command == 0)
        //    url = '/Common/LeadTraceAdd'
        //else if (command == 2)
        //    url = '/Common/LeadTraceDelete'
        //else
        //    url = '/Common/LeadTraceUpdate';

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: url,
            data: rowdata,
            success: function (data, status, xhr) {
                //alert(data.Message);
                if (data.Commit) {
                    //alert('עודכן בהצלחה');
                    commit(true);
                    //dataAdapter.dataBind();
                }
                onsuccess(slf);
                //dataAdapter.dataBind();
            },
            error: function () {
                app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                commit(false);
            }
        });
    },
    openPopupEditor: function (popupTag, tag_grid) {
        //if (mode !== undefined && mode != null) {
        //    $("#Mode").val(mode);
        //}
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        var tag = (popupTag === undefined || popupTag == null) ? "popupWindow" : popupTag;

        // open the popup window when the user clicks a button.
        var offset = $(tag_grid).offset();
        var outerWidth = $(tag_grid).outerWidth();
        var popupWidth = $('#' + tag).outerWidth();
        var posx = parseInt(offset.left) + parseInt(outerWidth) / 2;
        posx = posx - (parseInt(popupWidth) / 2);
        var posy = parseInt(offset.top) + 100;
        $('#' + tag).jqxWindow({ position: { x: posx, y: posy } });
        // show the popup window.
        $('#' + tag).jqxWindow('open');
    },
    buildFilterPanel: function (filterPanel, datafield, filtercondition, filtertype, tag_grid) {

        // filtertype - numericfilter, stringfilter, datefilter or booelanfilter. 
        // condition
        // possible conditions for string filter: 'EMPTY', 'NOT_EMPTY', 'CONTAINS', 'CONTAINS_CASE_SENSITIVE',
        // 'DOES_NOT_CONTAIN', 'DOES_NOT_CONTAIN_CASE_SENSITIVE', 'STARTS_WITH', 'STARTS_WITH_CASE_SENSITIVE',
        // 'ENDS_WITH', 'ENDS_WITH_CASE_SENSITIVE', 'EQUAL', 'EQUAL_CASE_SENSITIVE', 'NULL', 'NOT_NULL'
        // possible conditions for numeric filter: 'EQUAL', 'NOT_EQUAL', 'LESS_THAN', 'LESS_THAN_OR_EQUAL', 'GREATER_THAN', 'GREATER_THAN_OR_EQUAL', 'NULL', 'NOT_NULL'
        // possible conditions for date filter: 'EQUAL', 'NOT_EQUAL', 'LESS_THAN', 'LESS_THAN_OR_EQUAL', 'GREATER_THAN', 'GREATER_THAN_OR_EQUAL', 'NULL', 'NOT_NULL' 
        //this.filtercondition = filtercondition;
        //this.filtertype = filtertype;

        //if (filtercondition === undefined)
        //    this.filtercondition = 'contains';
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (filtertype === undefined || filtertype == null)
            filtertype = 'stringfilter';

        var textInput = $("<input style='margin:5px;'/>");
        var applyinput = $("<div class='filter' style='height: 25px; margin-left: 20px; margin-top: 7px;'></div>");
        var filterbutton = $('<span tabindex="0" style="padding: 4px 12px; margin-left: 2px;">סינון</span>');
        applyinput.append(filterbutton);
        var filterclearbutton = $('<span tabindex="0" style="padding: 4px 12px; margin-left: 5px;">הסר סינון</span>');
        applyinput.append(filterclearbutton);
        //filterPanel.append(textInput);


        if (filtertype == "stringfilter" || filtertype == "booelanfilter") {
            filterPanel.append(textInput);
            var column = $(tag_grid).jqxTreeGrid('getcolumn', datafield);
            textInput.jqxInput({ placeHolder: "הקלד " + column.text, maxLength: 25, popupZIndex: 9999999, height: 23, width: 155 });
            textInput.keyup(function (event) {
                if (event.keyCode === 13) {
                    filterbutton.trigger('click');
                }
            });
        }

        filterPanel.append(applyinput);
        filterbutton.jqxButton({ height: 20 });
        filterclearbutton.jqxButton({ height: 20 });
        var slf = this;

        if (filtercondition == 'EQUAL') {

            filterbutton.click(function () {
                var filtergroup = new $.jqx.filter();
                var filter_or_operator = 0;
                var filtervalue = textInput.val();
                var filtercondition = 'EQUAL';
                var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                filtergroup.addfilter(filter_or_operator, filter1);
                // add the filters.
                $(tag_grid).jqxTreeGrid('addfilter', datafield, filtergroup);
                // apply the filters.
                $(tag_grid).jqxTreeGrid('applyFilters');
                $(tag_grid).jqxTreeGrid('closemenu');
            });
        }
        else {
            filterbutton.click(function () {
                var filtergroup = new $.jqx.filter();
                var filter_or_operator = 0;
                var filtervalue = textInput.val();
                var filtercondition = 'contains';
                var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                filtergroup.addFilter(filter_or_operator, filter1);
                // add the filters.
                $(tag_grid).jqxTreeGrid('addFilter', datafield, filtergroup);
                // apply the filters.
                $(tag_grid).jqxTreeGrid('applyFilters');
                $(tag_grid).jqxTreeGrid('closemenu');
            });

        }
        filterbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterbutton.trigger('click');
            }
        });
        filterclearbutton.click(function (tag_grid) {
            if (tag_grid === undefined || tag_grid == null)
                tag_grid = "#jqxgrid";

            $(tag_grid).jqxTreeGrid('removefilter', datafield);
            // apply the filters.
            $(tag_grid).jqxTreeGrid('applyFilters');
            $(tag_grid).jqxTreeGrid('closemenu');
        });
        filterclearbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterclearbutton.trigger('click');
            }
            textInput.val("");
        });
    },
    gridFieldsFilter: function (listSource, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $("#gridFields").jqxComboBox({ checkboxes: true, source: listSource, width: 200, height: 25 });
        $("#gridFields").jqxComboBox('checkIndex', 0);
        // subscribe to the checkChange event.
        $("#gridFields").on('checkChange', function (event) {
            $(tag_grid).jqxTreeGrid('beginupdate');
            if (event.args.checked) {
                $(tag_grid).jqxTreeGrid('showcolumn', event.args.value);
            }
            else {
                $(tag_grid).jqxTreeGrid('hidecolumn', event.args.value);
            }
            $(tag_grid).jqxTreeGrid('endupdate');
        });
    },
    gridSearch: function (me, toolbar, columnsList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxTreeGrid("columns");

        var container;
        var span;
        var list;
        var input;

        if (rtl) {
            //var me = this;
            container = $("<div style='margin: 5px;direction:rtl'></div>");
            span = $("<span style='float: right; margin-top: 5px; margin-right: 4px;'>איתור : </span>");
            //var list = $("<select id='tbgridFieldsOption' style='float: left; margin-top: 5px; margin-right: 4px;'><option>Action</option><option>Folder</option><option>LogText</option><option>LogType</option><option>Client</option><option>Referrer</option></select>");
            list = $("<div id='tbgridFieldsOption' style='float:right; display:inline;'></div>").jqxDropDownList({ source: columnsList, width: 200, height: 25 });
            input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 23px; float: right; width: 223px;' />");
        }
        else {
            //var me = this;
            var container = $("<div style='margin: 5px;'></div>");
            var span = $("<span style='float: left; margin-top: 5px; margin-right: 4px;'>Search : </span>");
            //var list = $("<select id='tbgridFieldsOption' style='float: left; margin-top: 5px; margin-right: 4px;'><option>Action</option><option>Folder</option><option>LogText</option><option>LogType</option><option>Client</option><option>Referrer</option></select>");
            var list = $("<div id='tbgridFieldsOption' style='float:left; display:inline;'></div>").jqxDropDownList({ source: columnsList, width: 200, height: 25 });
            var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 23px; float: left; width: 223px;' />");
        }

        toolbar.append(container);

        container.append(span);
        container.append(list);
        container.append(input);
        //if (theme != "") {
        //    input.addClass('jqx-widget-content-' + theme);
        //    input.addClass('jqx-rc-all-' + theme);
        //}
        var oldVal = "";
        input.on('keydown', function (event) {
            if (input.val().length >= 2) {
                if (me.timer) {
                    clearTimeout(me.timer);
                }
                if (oldVal != input.val()) {
                    me.timer = setTimeout(function () {
                        app_jqxtreegrid.autosearchFilter($("#tbgridFieldsOption").val(), $("#tbgridSearchField").val(), tag_grid);
                    }, 1000);
                    oldVal = input.val();
                }
            }
            else {
                $(tag_grid).jqxTreeGrid('updatebounddata');
            }
        });
    },
    gridFilterSearch: function (me, toolbar, columnsList, dateList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        var width = $(tag_grid).jqxTreeGrid('width');
        toolbar.css('width', width);

        //var me = this;
        var container = (rtl) ? $("<div style='margin: 5px;direction:rtl'></div>") : $("<div style='margin: 5px;'></div>");

        if (columnsList) {

            var collist;
            var containerSearch;
            var input;

            if (rtl) {
                containerSearch = $("<div style='margin: 5px;direction:rtl'></div>");
                collist = $("<div id='tbgridFieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
                input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 22px; float: right; width: 160px;' />");
            }
            else {
                containerSearch = $("<div style='margin: 5px;'></div>");
                collist = $("<div id='tbgridFieldsOption' style='float:left; display:inline;' title='Please choose field for filter'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, autoDropDownHeight: true, placeHolder: "Choose Field" });
                input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 22px; float: left; width: 160px;' />");
            }


            //var searchbutton = $("<div style='float: right; margin-left: 5px;' title='איתור' ><img src='../scripts/app/images/search.gif'></div>");
            //searchbutton.jqxButton({ width: 16, height: 16 });
            //searchbutton.click(function (event) {
            //    var field = $("#tbgridFieldsOption").val();
            //    var value = $("#tbgridSearchField").val();
            //    if (field != null && value != null) {
            //        me.timer = setTimeout(function () {
            //            app_jqxtreegrid.searchFilter(field, value, 'contains',tag_grid);
            //        }, 1000);
            //    }
            //});
            var oldVal = "";
            input.on('keydown', function (event) {
                if (input.val().length >= 2) {
                    if (me.timer) {
                        clearTimeout(me.timer);
                    }
                    if (oldVal != input.val()) {
                        me.timer = setTimeout(function () {
                            app_jqxtreegrid.autosearchFilter($("#tbgridFieldsOption").val(), $("#tbgridSearchField").val(), tag_grid);
                        }, 1000);
                        oldVal = input.val();
                    }
                }
                else {
                    $(tag_grid).jqxTreeGrid('updatebounddata');
                }
            });
            collist.on('change', function (event) {
                var value = event.args.item.label;
                if (rtl)
                    $("#tbgridSearchField").attr("placeholder", "הקלד\\י  " + value + ' לסינון');
                else
                    $("#tbgridSearchField").attr("placeholder", "Type  " + value + ' for filter');
            });

            containerSearch.append(collist);
            containerSearch.append(input);
            //containerSearch.append(searchbutton);
            container.append(containerSearch);
        }
        //date filter=====================================================
        if (dateList) {
            var containerDate = $("<div style='margin: 5px;direction:rtl'></div>");
            var datelabel;
            var datecolumn;
            var datecollist;
            if (rtl)
                datecollist = $("<div id='tbgridDateOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י תאריך לסינון" });
            else
                datecollist = $("<div id='tbgridDateOption' style='float:left; display:inline;' title='Please choose field for filter'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, autoDropDownHeight: true, placeHolder: "Choose Date" });

            datecollist.on('change', function (event) {
                datelabel = event.args.item.label;
                datecolumn = event.args.item.value;
            });
            datecollist.jqxDropDownList('selectIndex', 0);

            //if (datelabel === undefined)
            //    datelabel = "תאריך";
            //var datespan = $("<span style='float: right; margin-top: 5px; margin-right: 4px;'>מ-עד " + datelabel + " : </span>");

            var date1 = null;// new Date();
            var date2 = null;//new Date();
            //date1.setDate(date2.getDate() - 30);
            var tbgridDatepicker;
            if (rtl)
                var tbgridDatepicker = $("<div id='tbgridDatepicker' style='float:right; display:inline;' title='נא לבחור מתאריך עד תאריך ולהמתין לביצוע הסינון'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range', rtl: true });
            else
                var tbgridDatepicker = $("<div id='tbgridDatepicker' style='float:left; display:inline;' title='Choose date range and wait'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range' });

            $(tbgridDatepicker).jqxDateTimeInput('setRange', date1, date2);
            tbgridDatepicker.on('change', function (event) {
                var selection = $("#tbgridDatepicker").jqxDateTimeInput('getRange');
                if (selection.from != null) {
                    //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
                    me.timer = setTimeout(function () {
                        app_jqxtreegrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0], tag_grid);
                    }, 1000);
                }
            });
            containerDate.append(datecollist);
            containerDate.append(tbgridDatepicker);
            container.append(containerDate);
        }
        toolbar.append(container);
    },
    gridFilter: function (me, toolbar, columnsList, dateList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        var width = $(tag_grid).jqxTreeGrid('width');
        toolbar.css('width', width);

        //var me = this;
        var container = (rtl) ? $("<div style='margin: 5px;direction:rtl'></div>") : $("<div style='margin: 5px;'></div>");;

        if (columnsList) {
            //var span = $("<span style='float: right; margin-top: 5px; margin-right: 4px;'>איתור : </span>");
            var containerSearch;
            var input;
            var collist;
            var searchbutton;

            if (rtl) {
                containerSearch = $("<div style='margin: 5px;direction:rtl'></div>");
                input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 22px; float: right; width: 120px;' />");
                collist = $("<div id='tbgridFieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
                searchbutton = $("<div style='float: right; margin-left: 5px;' title='סינון\\ביטול' ><img src='../scripts/app/images/search.gif'></div>");
            }
            else {
                containerSearch = $("<div style='margin: 5px;'></div>");
                input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 22px; float: left; width: 120px;' />");
                collist = $("<div id='tbgridFieldsOption' style='float:left; display:inline;' title='Choose field for filter'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, autoDropDownHeight: true, placeHolder: "Choose Field" });
                searchbutton = $("<div style='float: left; margin-left: 5px;' title='Filter//Cancel' ><img src='../scripts/app/images/search.gif'></div>");
            }
            //var searchbutton = $("<div style='float: right; display:inline;' title='איתור' ><input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 23px; float: right; width: 200px;' /><img src='../scripts/app/images/search.gif'></div>");
            searchbutton.jqxButton({ width: 16, height: 16 });
            searchbutton.click(function (event) {
                var field = $("#tbgridFieldsOption").val();
                var value = $("#tbgridSearchField").val();

                if (field != null && value != null) {
                    var filtercondition = value.match(/^=/) ? 'EQUAL' : 'contains';
                    me.timer = setTimeout(function () {
                        app_jqxtreegrid.searchFilter(field, value.replace(/^=/, ""), filtercondition, tag_grid);
                    }, 1000);
                }
                else {
                    app_jqxtreegrid.clearFilter(tag_grid);
                    if (dateList)
                        $(tbgridDatepicker).jqxDateTimeInput('setRange', null, null);
                }
            });
            input.keypress(function (ev) {
                var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                if (keycode == '13') {
                    if (input.val().length >= 2) {
                        if (me.timer) {
                            clearTimeout(me.timer);
                        }
                        var filtercondition = input.val().match(/^=/) ? 'EQUAL' : 'contains';
                        me.timer = setTimeout(function () {
                            app_jqxtreegrid.searchFilter($("#tbgridFieldsOption").val(), input.val().replace(/^=/, ""), filtercondition, tag_grid);
                        }, 1000);
                    }
                    else {
                        app_jqxtreegrid.clearFilter(tag_grid);
                    }
                }
            })
            collist.on('change', function (event) {
                var value = event.args.item.label;
                if (rtl)
                    $("#tbgridSearchField").attr("placeholder", "הקלד\\י " + value + ' לסינון');
                else
                    $("#tbgridSearchField").attr("placeholder", "Type " + value + ' for filter');
            });

            //container.append(span);
            containerSearch.append(collist);
            containerSearch.append(input);
            containerSearch.append(searchbutton);
            container.append(containerSearch);
        }
        /*
        var searchbutton = $("<input id='searchbutton' type='button' value='...' style='height: 23px; float: right; width: 30px;' />")
        .on('click', function (event) {
            var field = $("#tbgridFieldsOption").val();
            var value = $("#tbgridSearchField").val();
            if (field != null && value !=null) {
                me.timer = setTimeout(function () {
                    app_jqxtreegrid.searchFilter(field, value,'contains',tag_grid);
                }, 1000);
            }
        });
        */

        if (dateList) {
            //date filter=====================================================
            var containerDate = (rtl) ? $("<div style='margin: 5px;direction:rtl'></div>") : $("<div style='margin: 5px;'></div>");
            var datelabel;
            var datecolumn;
            var datecollist;
            if (rtl)
                datecollist = $("<div id='tbgridDateOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י תאריך לסינון" });
            else
                datecollist = $("<div id='tbgridDateOption' style='float:left; display:inline;' title='Choose field for filter'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, autoDropDownHeight: true, placeHolder: "Choose date" });

            datecollist.on('change', function (event) {
                datelabel = event.args.item.label;
                datecolumn = event.args.item.value;
            });
            datecollist.jqxDropDownList('selectIndex', 0);

            //if (datelabel === undefined)
            //    datelabel = "תאריך";
            //var datespan = $("<span style='float: right; margin-top: 5px; margin-right: 4px;'>מ-עד " + datelabel + " : </span>");
            var date1 = new Date();
            var date2 = new Date();
            date1.setDate(date2.getDate() - 30);
            var tbgridDatepicker;
            if (rtl)
                tbgridDatepicker = $("<div id='tbgridDatepicker' style='float:right; display:inline;' title='נא לבחור מתאריך עד תאריך ולהמתין לביצוע הסינון'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range', rtl: true });
            else
                tbgridDatepicker = $("<div id='tbgridDatepicker' style='float:left; display:inline;' title='Choose date range and wait'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range' });

            $(tbgridDatepicker).jqxDateTimeInput('setRange', date1, date2);
            tbgridDatepicker.on('change', function (event) {
                var selection = $("#tbgridDatepicker").jqxDateTimeInput('getRange');
                if (selection.from != null) {
                    //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
                    me.timer = setTimeout(function () {
                        app_jqxtreegrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0], tag_grid);
                    }, 1000);
                }
            });

            //containerDate.append(datespan);
            containerDate.append(datecollist);
            containerDate.append(tbgridDatepicker);
            //containerDate.append(datebutton);
            container.append(containerDate);
        }

        //var datebutton = $("<div style='float: right; margin-left: 5px;' title='סינון' ><img src='../scripts/app/images/refresh.gif'></div>");
        //datebutton.jqxButton({ width: 16, height: 16 });
        //datebutton.click(function (event) {
        //    var selection = $("#tbgridDateOption").jqxDateTimeInput('getRange');
        //    if (selection.from != null) {
        //        //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
        //        me.timer = setTimeout(function () {
        //            app_jqxtreegrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0],tag_grid);
        //        }, 1000);
        //    }
        //});


        /*
        var datebutton = $("<input id='datebutton' type='button' value='...' style='height: 23px; float: right; width: 30px;' />")
        .on('click', function (event) {
            var selection = $("#tbgridDateOption").jqxDateTimeInput('getRange');
            if (selection.from != null) {
                //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
                me.timer = setTimeout(function () {
                    app_jqxtreegrid.rangeDateFilter(datecolumn, selection.from, selection.to,tag_grid);
                }, 1000);
                
            }
        });
        */


        toolbar.append(container);
    },