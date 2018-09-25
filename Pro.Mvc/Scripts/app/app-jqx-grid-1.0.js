

//============================================================================================ app_jqxgrid

var app_jqxgrid = {

    loadEntityDataGrid: function (entity, data, tag_grid, pageable) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (pageable === undefined || pageable == null)
            pageable = false;

        var source =
        {
            updaterow: function (rowid, rowdata, commit) {
                editrow = rowid;
                app_jqxgrid.doUpdateCommand(rowid, rowdata, commit, 1, entity, tag_grid);
            },
            addrow: function (rowid, rowdata, position, commit) {
                editrow = -1;
                app_jqxgrid.doUpdateCommand(rowid, rowdata, commit, 0, entity, tag_grid);
            },
            deleterow: function (rowid, commit) {
                editrow = rowid;
                app_jqxgrid.doUpdateCommand(rowid, null, commit, 2, entity, tag_grid);
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
        // initialize jqxGrid
        $(tag_grid).jqxGrid(
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
                app_jqxgrid.renderToolbar(toolbar, entity)
            },
            columns: entity.createCoulmns()
        });


        $(tag_grid).on('cellselect', function (event) {
            var column = $(tag_grid).jqxGrid('getcolumn', event.args.datafield);
            var value = $(tag_grid).jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
            var displayValue = $(tag_grid).jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
        });

        $(tag_grid).on('rowdoubleclick', function (event) {
            if (entity.AllowEdit == 0) {
                return;
            };
            var args = event.args;
            var boundIndex = args.rowindex;
            var visibleIndex = args.visibleindex;
            entity.rowEdit = boundIndex;
            app_jqxgrid.doRowEdit(boundIndex, entity, tag_grid);
        });
        //$(tag_grid).jqxGrid('autoresizecolumns');

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
                        $(tag_grid).jqxGrid('addrow', null, row);
                    }
                    else if (editrow >= 0) {
                        var rowID = $(tag_grid).jqxGrid('getrowid', editrow);
                        $(tag_grid).jqxGrid('updaterow', rowID, row);
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
                app_jqxgrid.doUpdateCommand(rowid, rowdata, commit, 1, entity, tag_grid);
            },
            addrow: function (rowid, rowdata, position, commit) {
                editrow = -1;
                app_jqxgrid.doUpdateCommand(rowid, rowdata, commit, 0, entity, tag_grid);
            },
            deleterow: function (rowid, commit) {
                editrow = rowid;
                app_jqxgrid.doUpdateCommand(rowid, null, commit, 2, entity, tag_grid);
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
        // initialize jqxGrid
        $(tag_grid).jqxGrid(
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
                app_jqxgrid.renderToolbar(toolbar, entity)
            },
            columns: entity.createCoulmns()
        });


        $(tag_grid).on('cellselect', function (event) {
            var column = $(tag_grid).jqxGrid('getcolumn', event.args.datafield);
            var value = $(tag_grid).jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
            var displayValue = $(tag_grid).jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
        });

        $(tag_grid).on('rowdoubleclick', function (event) {
            if (entity.AllowEdit == 0) {
                return;
            };
            var args = event.args;
            var boundIndex = args.rowindex;
            var visibleIndex = args.visibleindex;
            entity.rowEdit = boundIndex;
            app_jqxgrid.doRowEdit(boundIndex, entity, tag_grid);
        });
        //$(tag_grid).jqxGrid('autoresizecolumns');

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
                        $(tag_grid).jqxGrid('addrow', null, row);
                    }
                    else if (editrow >= 0) {
                        var rowID = $(tag_grid).jqxGrid('getrowid', editrow);
                        $(tag_grid).jqxGrid('updaterow', rowID, row);
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

        app_jqxgrid.createToolbar(toolbar);

        if (entity.AllowEdit == 0) {
            $("#addrowbutton").hide();
            $("#deleterowbutton").hide();
            $("#updaterowbutton").hide();
        }
        // update row.
        $("#updaterowbutton").on('click', function () {
            var selectedrowindex = $(tag_grid).jqxGrid('getselectedrowindex');
            entity.rowEdit = selectedrowindex;
            app_jqxgrid.doRowEdit(selectedrowindex, entity, tag_grid);

        });
        // create new row.
        $("#addrowbutton").on('click', function () {
            // show the popup window.
            entity.rowEdit = -1;
            entity.setEditorInputData(null);
            app_jqxgrid.openPopupEditor();
        });
        // delete row.
        $("#deleterowbutton").on('click', function () {
            var selectedrowindex = $(tag_grid).jqxGrid('getselectedrowindex');
            entity.rowEdit = selectedrowindex;
            var rowscount = $(tag_grid).jqxGrid('getdatainformation').rowscount;
            if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                var id = $(tag_grid).jqxGrid('getrowid', selectedrowindex);
                var result = confirm("האם למחוק?");
                if (result == true) {
                    //Logic to delete the item
                    var commit = $(tag_grid).jqxGrid('deleterow', id);
                }
            }
        });
        // refresh grid.
        $("#refreshbutton").on('click', function () {
            $(tag_grid).jqxGrid('source').dataBind();
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
        var selectedrowindex = $(tag_grid).jqxGrid('getselectedrowindex');
        var rowscount = $(tag_grid).jqxGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            editrow = selectedrowindex;
            var id = $(tag_grid).jqxGrid('getrowid', selectedrowindex);
            var result = confirm("האם למחוק?");
            if (result == true) {
                //Logic to delete the item
                var commit = $(tag_grid).jqxGrid('deleterow', id);
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
                    $(tag_grid).jqxGrid('source').dataBind();
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
        var rowscount = $(tag_grid).jqxGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            $(tag_grid).jqxGrid('ensurerowvisible', selectedrowindex);
            // open the popup window when the user clicks a button.
            editrow = selectedrowindex;
            // get the clicked row's data and initialize the input fields.
            var dataRecord = $(tag_grid).jqxGrid('getrowdata', editrow);
            if (dataRecord != null && dataRecord !== undefined) {
                entity.setEditorInputData(dataRecord);
                app_jqxgrid.openPopupEditor(null, tag_grid);
            }
            // show the popup window.
            //$("#popupWindow").jqxWindow('open');
            //app_jqxgrid.openPopupEditor(null, tag_grid);
        }
        else {
            entity.setEditorInputData(dataRecord);
            app_jqxgrid.openPopupEditor(null, tag_grid);
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
            var column = $(tag_grid).jqxGrid('getcolumn', datafield);
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
                $(tag_grid).jqxGrid('addfilter', datafield, filtergroup);
                // apply the filters.
                $(tag_grid).jqxGrid('applyfilters');
                $(tag_grid).jqxGrid('closemenu');
            });
        }
        else {
            filterbutton.click(function () {
                var filtergroup = new $.jqx.filter();
                var filter_or_operator = 0;
                var filtervalue = textInput.val();
                var filtercondition = 'contains';
                var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                filtergroup.addfilter(filter_or_operator, filter1);
                // add the filters.
                $(tag_grid).jqxGrid('addfilter', datafield, filtergroup);
                // apply the filters.
                $(tag_grid).jqxGrid('applyfilters');
                $(tag_grid).jqxGrid('closemenu');
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

            $(tag_grid).jqxGrid('removefilter', datafield);
            // apply the filters.
            $(tag_grid).jqxGrid('applyfilters');
            $(tag_grid).jqxGrid('closemenu');
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
            $(tag_grid).jqxGrid('beginupdate');
            if (event.args.checked) {
                $(tag_grid).jqxGrid('showcolumn', event.args.value);
            }
            else {
                $(tag_grid).jqxGrid('hidecolumn', event.args.value);
            }
            $(tag_grid).jqxGrid('endupdate');
        });
    },
    gridSearch: function (me, toolbar, columnsList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxGrid("columns");

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
                        app_jqxgrid.autosearchFilter($("#tbgridFieldsOption").val(), $("#tbgridSearchField").val(), tag_grid);
                    }, 1000);
                    oldVal = input.val();
                }
            }
            else {
                $(tag_grid).jqxGrid('updatebounddata');
            }
        });
    },
    gridFilterSearch: function (me, toolbar, columnsList, dateList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        var width = $(tag_grid).jqxGrid('width');
        toolbar.css('width', width);

        //var me = this;
        var container = (rtl) ? $("<div style='margin: 5px;direction:rtl'></div>") : $("<div style='margin: 5px;'></div>");

        if (columnsList) {

            var collist;
            var containerSearch;
            var input;

            if (rtl) {
                containerSearch = $("<div style='margin: 5px;direction:rtl'></div>");
                collist = $("<div id='tbgridFieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 122, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
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
            //            app_jqxgrid.searchFilter(field, value, 'contains',tag_grid);
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
                            app_jqxgrid.autosearchFilter($("#tbgridFieldsOption").val(), $("#tbgridSearchField").val(), tag_grid);
                        }, 1000);
                        oldVal = input.val();
                    }
                }
                else {
                    $(tag_grid).jqxGrid('updatebounddata');
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
                        app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0], tag_grid);
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

        var width = $(tag_grid).jqxGrid('width');
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
                collist = $("<div id='tbgridFieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 122, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
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
                        app_jqxgrid.searchFilter(field, value.replace(/^=/, ""), filtercondition, tag_grid);
                    }, 1000);
                }
                else {
                    app_jqxgrid.clearFilter(tag_grid);
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
                            app_jqxgrid.searchFilter($("#tbgridFieldsOption").val(), input.val().replace(/^=/, ""), filtercondition, tag_grid);
                        }, 1000);
                    }
                    else {
                        app_jqxgrid.clearFilter(tag_grid);
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
                    app_jqxgrid.searchFilter(field, value,'contains',tag_grid);
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
                        app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0], tag_grid);
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
        //            app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0],tag_grid);
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
                    app_jqxgrid.rangeDateFilter(datecolumn, selection.from, selection.to,tag_grid);
                }, 1000);
                
            }
        });
        */


        toolbar.append(container);
    },
    gridFilterEx: function (me, toolbar, columnsList, dateList, dateRangeList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (rtl === undefined || rtl == null)
            rtl = true;

        var width = $(tag_grid).jqxGrid('width');
        toolbar.css('width', width);

        var container = (rtl) ? $("<div style='margin: 5px;direction:rtl'></div>") : $("<div style='margin: 5px;'></div>");;

        if (columnsList) {
            var containerSearch;
            var input;
            var collist;
            var searchbutton;
            var cancelbutton;

            if (rtl) {
                containerSearch = $("<div style='margin: 5px;direction:rtl'></div>");
                input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 22px; float: right; width: 120px;' />");
                collist = $("<div id='tbgridFieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 122, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
                searchbutton = $("<div style='float: right; margin-left: 5px;' title='סינון\\ביטול' ><img src='../scripts/app/images/search.gif'></div>");
                cancelbutton = $("<div style='float: right; margin-left: 5px;' title='ביטול סינון' ><img src='../scripts/app/images/filterRemove.gif'></div>");
            }
            else {
                containerSearch = $("<div style='margin: 5px;'></div>");
                input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='tbgridSearchField' type='text' style='height: 22px; float: left; width: 120px;' />");
                collist = $("<div id='tbgridFieldsOption' style='float:left; display:inline;' title='Choose field for filter'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, autoDropDownHeight: true, placeHolder: "Choose Field" });
                searchbutton = $("<div style='float: left; margin-left: 5px;' title='Filter//Cancel' ><img src='../scripts/app/images/search.gif'></div>");
                cancelbutton = $("<div style='float: left; margin-left: 5px;' title='Cancel filter' ><img src='../scripts/app/images/filterRemove.gif'></div>");
            }
            searchbutton.jqxButton({ width: 16, height: 16 });
            searchbutton.click(function (event) {
                var field = $("#tbgridFieldsOption").val();
                var value = $("#tbgridSearchField").val();

                if (field != null && value != null) {
                    var filtercondition = value.match(/^=/) ? 'EQUAL' : 'contains';
                    me.timer = setTimeout(function () {
                        app_jqxgrid.searchFilter(field, value.replace(/^=/, ""), filtercondition, tag_grid);
                    }, 1000);
                }
                else {
                    app_jqxgrid.clearFilterEx(tag_grid);
                    if (dateList)
                        $(tbgridDatepicker).jqxDateTimeInput('setRange', null, null);
                }
            });
            cancelbutton.jqxButton({ width: 16, height: 16 });
            cancelbutton.click(function (event) {
                $("#tbgridSearchField").val('');
                //$("#tbgridFieldsOption").jqxDropDownList('selectIndex', -1);
                app_jqxgrid.clearFilterEx(tag_grid);
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
                            app_jqxgrid.searchFilter($("#tbgridFieldsOption").val(), input.val().replace(/^=/, ""), filtercondition, tag_grid);
                        }, 1000);
                    }
                    else {
                        app_jqxgrid.clearFilterEx(tag_grid);
                    }
                }
            })
            collist.on('change', function (event) {
                var value = event.args.item.label;
                if (rtl)
                    $("#tbgridSearchField").attr("placeholder", "הקלד\\י " + value + ' לסינון');
                else
                    $("#tbgridSearchField").attr("placeholder", "Type " + value + ' for filter');
                $(input).val('');
            });

            containerSearch.append(collist);
            containerSearch.append(input);
            containerSearch.append(searchbutton);
            containerSearch.append(cancelbutton);
            container.append(containerSearch);
        }

        if (dateList) {
            //date filter=====================================================
            var containerDate = (rtl) ? $("<div style='margin: 5px;direction:rtl'></div>") : $("<div style='margin: 5px;'></div>");
            var datelabel;
            var datecolumn;
            var datecollist;
            var tbgridDatepicker;
            var tbgridDaysSelect;
            if (rtl) {
                datecollist = $("<div id='tbgridDateOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י תאריך לסינון" });
                tbgridDatepicker = $("<div id='tbgridDatepicker' style='float:right; display:inline;' title='נא לבחור מתאריך עד תאריך ולהמתין לביצוע הסינון'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range', rtl: true });
                if (dateRangeList === undefined || dateRangeList == null)
                    dateRangeList = [
                     { label: 'יום', value: '1', checked: false },
                     { label: 'שבוע', value: '7', checked: false },
                     { label: 'שבועיים', value: '7', checked: false },
                     { label: 'חודש', value: '30', checked: false },
                     { label: 'חודשיים', value: '60', checked: false },
                     { label: 'רבעון', value: '90', checked: false },
                     { label: 'בחירה', value: 'range', checked: false }
                    ];
                tbgridDaysSelect = $("<div id='tbgridDaysSelect' style='float:right; display:inline' title='א לבחור תאריך ולהמתין לביצוע הסינון'></div>").jqxDropDownList({ source: dateRangeList, width: 70, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "תאריך" });
            }
            else {
                datecollist = $("<div id='tbgridDateOption' style='float:left; display:inline;' title='Choose field for filter'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, autoDropDownHeight: true, placeHolder: "Choose date" });
                tbgridDatepicker = $("<div id='tbgridDatepicker' style='float:left; display:inline;' title='Choose date range and wait'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range' });
                if (dateRangeList === undefined || dateRangeList == null)
                    dateRangeList = [
                     { label: 'Day', value: '1', checked: false },
                     { label: '7 Day', value: '7', checked: false },
                     { label: '14 Day', value: '30', checked: false },
                     { label: '30 Day', value: '30', checked: false },
                     { label: '60 Day', value: '60', checked: false },
                     { label: '90 Day', value: '90', checked: false },
                     { label: 'Range', value: 'range', checked: false }
                    ];
                tbgridDaysSelect = $("<div id='tbgridDaysSelect' style='float:left; display:inline;' title='Choose date range and wait'></div>").jqxDropDownList({ source: dateRangeList, width: 70, height: 22, autoDropDownHeight: true, placeHolder: "Choose date" });
            }


            datecollist.on('change', function (event) {
                datelabel = event.args.item.label;
                datecolumn = event.args.item.value;
            });
            datecollist.jqxDropDownList('selectIndex', 0);


            var date1 = new Date();
            var date2 = new Date();
            date1.setDate(date2.getDate() - 30);

            $(tbgridDatepicker).jqxDateTimeInput('setRange', date1, date2);
            tbgridDatepicker.on('change', function (event) {
                var selection = $("#tbgridDatepicker").jqxDateTimeInput('getRange');
                if (selection.from != null) {
                    me.timer = setTimeout(function () {
                        app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0], tag_grid);
                    }, 1000);
                }
            });

            tbgridDaysSelect.on('change', function (event) {
                if (event.args.item == null)
                    return;
                var days = event.args.item.value;
                if (days == 'range') {
                    $(tbgridDaysSelect).hide();
                    $(tbgridDatepicker).show();
                }
                else {
                    var d = new Date();
                    days = parseInt(days);
                    d.setDate(d.getDate() - days);
                    date1 = d;
                    me.timer = setTimeout(function () {
                        app_jqxgrid.rangeDateFilter(datecolumn, d.toISOString().split('T')[0], date2.toISOString().split('T')[0], tag_grid);
                    }, 1000)
                }
            });

            $(tbgridDaysSelect).show();
            $(tbgridDatepicker).hide();

            containerDate.append(datecollist);
            containerDate.append(tbgridDaysSelect);
            containerDate.append(tbgridDatepicker);
            container.append(containerDate);
        }

        //var datebutton = $("<div style='float: right; margin-left: 5px;' title='סינון' ><img src='../scripts/app/images/refresh.gif'></div>");
        //datebutton.jqxButton({ width: 16, height: 16 });
        //datebutton.click(function (event) {
        //    var selection = $("#tbgridDateOption").jqxDateTimeInput('getRange');
        //    if (selection.from != null) {
        //        me.timer = setTimeout(function () {
        //            app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0],tag_grid);
        //        }, 1000);
        //    }
        //});

        toolbar.append(container);
    },
    clearFilter: function (tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxGrid('clearfilters');
    },
    clearFilterEx: function (tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxGrid('clearfilters');
        if ($("#tbgridDaysSelect").length) {
            $("#tbgridDaysSelect").jqxDropDownList('selectIndex', -1);
            $("#tbgridDaysSelect").show();
            $("#tbgridDatepicker").hide();
        }
    },
    autosearchFilter: function (datafield, filtervalue, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        //$("#jqxgrid").jqxGrid('clearfilters');
        //if (datafield == 'date') filtertype = 'datefilter';
        //if (datafield == 'price' || datafield == 'quantity') filtertype = 'numericfilter';
        //var checkedItems = $("#filterbox").jqxListBox('getCheckedItems');
        if (datafield === undefined || filtervalue == null) {
            return;
        }
        var filtertype = 'stringfilter';
        var filtergroup = new $.jqx.filter();
        var filter_or_operator = 1;
        var filtercondition = 'STARTS_WITH';//'equal';
        var filter = filtergroup.createfilter(filtertype, filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);
        $(tag_grid).jqxGrid('addfilter', datafield, filtergroup);
        $(tag_grid).jqxGrid('applyfilters');
    },
    searchFilter: function (datafield, filtervalue, filtercondition, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        //$("#jqxgrid").jqxGrid('clearfilters');
        //if (datafield == 'date') filtertype = 'datefilter';
        //if (datafield == 'price' || datafield == 'quantity') filtertype = 'numericfilter';
        if (datafield === undefined || filtervalue == null) {
            return;
        }
        var filtertype = 'stringfilter';
        var filter_or_operator = 1;
        if (filtercondition === undefined)
            filtercondition = "contains";
        var filtergroup = new $.jqx.filter();

        var filter = filtergroup.createfilter(filtertype, filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);
        $(tag_grid).jqxGrid('addfilter', datafield, filtergroup);
        $(tag_grid).jqxGrid('applyfilters');
    },
    rangeDateFilter: function (datafield, filtervalueFrom, filtervalueTo, tag_grid) {
        //$("#jqxgrid").jqxGrid('clearfilters');

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        if (datafield === undefined || filtervalueFrom == null || filtervalueTo == null) {
            return;
        }
        var filtertype = 'datefilter';
        var filtergroup = new $.jqx.filter();
        var filter_or_operator = 0;

        // possible conditions for date filter: 'EQUAL', 'NOT_EQUAL', 'LESS_THAN', 'LESS_THAN_OR_EQUAL', 'GREATER_THAN', 'GREATER_THAN_OR_EQUAL', 'NULL', 'NOT_NULL' 
        var filter1 = filtergroup.createfilter(filtertype, filtervalueFrom, "GREATER_THAN_OR_EQUAL");
        var filter2 = filtergroup.createfilter(filtertype, filtervalueTo, "LESS_THAN_OR_EQUAL");
        filtergroup.addfilter(filter_or_operator, filter1);
        filtergroup.addfilter(filter_or_operator, filter2);

        $(tag_grid).jqxGrid('addfilter', datafield, filtergroup);
        $(tag_grid).jqxGrid('applyfilters');
    },
    gridColumnsBar: function (columnList, tag_columns, tag_grid) {
        if (tag_columns === undefined || tag_columns == null)
            tag_columns = "#jqxGridColumns";
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        $(tag_columns).jqxDropDownList({ source: columnList, width: 200, height: 25, checkboxes: true, rtl: true, autoDropDownHeight: true });
        $(tag_columns).on('checkChange', function (event) {
            $(tag_grid).jqxGrid('beginupdate');
            if (event.args.checked) {
                $(tag_grid).jqxGrid('showcolumn', event.args.value);
            }
            else {
                $(tag_grid).jqxGrid('hidecolumn', event.args.value);
            }
            $(tag_grid).jqxGrid('endupdate');
        });
    },
    gridColumnsInit: function (columnList, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(columnList).each(function (index, item) {
            $(tag_grid).jqxGrid('beginupdate');
            if (item.checked) {
                $(tag_grid).jqxGrid('showcolumn', item.value);
            }
            else {
                $(tag_grid).jqxGrid('hidecolumn', item.value);
            }
            $("#jqxgrid").jqxGrid('endupdate');
        });

    },
    gridExpandAll: function (columnList, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxGrid('expandallgroups');
    },
    griCollapseAll: function (columnList, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxGrid('collapseallgroups');
    },
    displayGridRecord: function (row, title, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = '#jqxgrid';

        var rowData = $(tag_grid).jqxGrid('getrowdata', row);
        var labels = labelLocalization("he");
        //rowData = JSON.parse(jsonText);
        var html = '<div style="direction: rtl;margin:5px">';
        var table = '';
        table += '<table border="1" direction="rtl">'
        for (x in rowData) {
            var label = labels[x] || x;
            table += '<tr><td>' + label + ': </td><td>' + rowData[x] + '</td></tr>';
        }
        table += '</table>'
        html += table + '</div>';

        app_dialog.dialogDiv(html, title, true);
    }
};


//============================================================================================ app_jqxgrid_menu
var app_jqxgrid_menu = {

    rowIndex: 0,
    rowValue: 0,
    originalEvent: undefined,
    contextMenu: undefined,
    rowMenu: function (row, value) {
        this.rowIndex = row;
        this.rowValue = value;
        //$("#jqxgrid").jqxGrid('selectrow', row);
        var scrollTop = $(window).scrollTop();
        var scrollLeft = $(window).scrollLeft();
        this.contextMenu.jqxMenu('open', parseInt(this.originalEvent.clientX) - 100 + scrollLeft, parseInt(this.originalEvent.clientY) + 5 + scrollTop);
        return false;
    },
    init: function (tag_grid, width) {

        var _slf = this;
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (width === undefined || width == 0)
            width = 120;


        this.contextMenu = $(tag_grid + "-rowMenu").jqxMenu({ width: width, autoOpenPopup: false, mode: 'popup', rtl: true });

        $(tag_grid).on('rowclick', function (event) {
            var args = event.args;
            _slf.originalEvent = args.originalEvent;

            // row's bound index.
            var boundIndex = args.rowindex;
            // row's visible index.
            var visibleIndex = args.visibleindex;
            // right click.
            var rightclick = args.rightclick;
            // original event.
            var ev = args.originalEvent;
        });

        return this;
    }
}


