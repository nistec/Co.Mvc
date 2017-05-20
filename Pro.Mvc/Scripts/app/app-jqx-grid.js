

//============================================================================================ app_jqxgrid

var app_jqxgrid = {

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
    deleteSelectedRow: function () {
        var editrow = -1;
        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            editrow = selectedrowindex;
            var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
            var result = confirm("האם למחוק?");
            if (result == true) {
                //Logic to delete the item
                var commit = $("#jqxgrid").jqxGrid('deleterow', id);
            }
        }

        return editrow;
    },
    doRowEdit: function (selectedrowindex, mode, loadRecord,args) {
        var editrow = -1;
        var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
            // open the popup window when the user clicks a button.
            editrow = selectedrowindex;
            // get the clicked row's data and initialize the input fields.
            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
            if (dataRecord != null && dataRecord !== undefined) {
                loadRecord(dataRecord, args);
            }
            // show the popup window.
            //$("#popupWindow").jqxWindow('open');
            this.openPopupEditor(mode);
        }
        else {
            loadRecord(null, args);
            this.openPopupEditor(mode);
        }
        return editrow;
    },

    sendCommand: function (rowdata, command, commit, url,onsuccess,slf) {


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
    openPopupEditor: function (popupTag) {
        //if (mode !== undefined && mode != null) {
        //    $("#Mode").val(mode);
        //}

        var tag = (popupTag === undefined || popupTag == null) ? "popupWindow" : popupTag;

        // open the popup window when the user clicks a button.
        var offset = $("#jqxgrid").offset();
        var outerWidth = $("#jqxgrid").outerWidth();
        var popupWidth = $('#' + tag).outerWidth();
        var posx = parseInt(offset.left) + parseInt(outerWidth) / 2;
        posx = posx - (parseInt(popupWidth) / 2);
        var posy = parseInt(offset.top) + 100;
        $('#' + tag).jqxWindow({ position: { x: posx, y: posy } });
        // show the popup window.
        $('#' + tag).jqxWindow('open');
    },
    buildFilterPanel: function (filterPanel, datafield, filtercondition, filtertype) {

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

        if (filtertype === undefined)
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
            var column = $("#jqxgrid").jqxGrid('getcolumn', datafield);
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
                $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
                // apply the filters.
                $("#jqxgrid").jqxGrid('applyfilters');
                $("#jqxgrid").jqxGrid('closemenu');
            });
        }
        else 
        {
            filterbutton.click(function () {
                var filtergroup = new $.jqx.filter();
                var filter_or_operator = 0;
                var filtervalue = textInput.val();
                var filtercondition = 'contains';
                var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                filtergroup.addfilter(filter_or_operator, filter1);
                // add the filters.
                $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
                // apply the filters.
                $("#jqxgrid").jqxGrid('applyfilters');
                $("#jqxgrid").jqxGrid('closemenu');
            });

        }
        filterbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterbutton.trigger('click');
            }
        });
        filterclearbutton.click(function () {
            $("#jqxgrid").jqxGrid('removefilter', datafield);
            // apply the filters.
            $("#jqxgrid").jqxGrid('applyfilters');
            $("#jqxgrid").jqxGrid('closemenu');
        });
        filterclearbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterclearbutton.trigger('click');
            }
            textInput.val("");
        });
    },
    gridFieldsFilter: function (listSource) {

        $("#gridFields").jqxComboBox({ checkboxes: true, source: listSource, width: 200, height: 25 });
        $("#gridFields").jqxComboBox('checkIndex', 0);
        // subscribe to the checkChange event.
        $("#gridFields").on('checkChange', function (event) {
            $("#jqxgrid").jqxGrid('beginupdate');
            if (event.args.checked) {
                $("#jqxgrid").jqxGrid('showcolumn', event.args.value);
            }
            else {
                $("#jqxgrid").jqxGrid('hidecolumn', event.args.value);
            }
            $("#jqxgrid").jqxGrid('endupdate');
        });
    },
    gridSearchLtr: function (me,toolbar,columnsList) {

        $("#jqxgrid").jqxGrid("columns");

       
        //var me = this;
        var container = $("<div style='margin: 5px;'></div>");
        var span = $("<span style='float: left; margin-top: 5px; margin-right: 4px;'>Search : </span>");
        //var list = $("<select id='fieldsOption' style='float: left; margin-top: 5px; margin-right: 4px;'><option>Action</option><option>Folder</option><option>LogText</option><option>LogType</option><option>Client</option><option>Referrer</option></select>");
        var list = $("<div id='fieldsOption' style='float:left; display:inline;'></div>").jqxDropDownList({ source: columnsList, width: 200, height: 25 });
        var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 23px; float: left; width: 223px;' />");
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
                        app_jqxgrid.autosearchFilter($("#fieldsOption").val(), $("#searchField").val());
                    }, 1000);
                    oldVal = input.val();
                }
            }
            else {
                $("#jqxgrid").jqxGrid('updatebounddata');
            }
        });
    },
    gridSearchRtl: function (me, toolbar, columnsList) {

        $("#jqxgrid").jqxGrid("columns");

        //var me = this;
        var container = $("<div style='margin: 5px;direction:rtl'></div>");
        var span = $("<span style='float: right; margin-top: 5px; margin-right: 4px;'>איתור : </span>");
        //var list = $("<select id='fieldsOption' style='float: left; margin-top: 5px; margin-right: 4px;'><option>Action</option><option>Folder</option><option>LogText</option><option>LogType</option><option>Client</option><option>Referrer</option></select>");
        var list = $("<div id='fieldsOption' style='float:right; display:inline;'></div>").jqxDropDownList({ source: columnsList, width: 200, height: 25 });
        var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 23px; float: right; width: 223px;' />");
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
                        app_jqxgrid.autosearchFilter($("#fieldsOption").val(), $("#searchField").val());
                    }, 1000);
                    oldVal = input.val();
                }
            }
            else {
                $("#jqxgrid").jqxGrid('updatebounddata');
            }
        });
    },
    gridFilterSearchRtl: function (me, toolbar, columnsList, dateList) {

        var width = $("#jqxgrid").jqxGrid('width');
        toolbar.css('width',width);

        //var me = this;
        var container = $("<div style='margin: 5px;direction:rtl'></div>");
 
        if (columnsList) {

            var containerSearch = $("<div style='margin: 5px;direction:rtl'></div>");
            var collist = $("<div id='fieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
            var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 22px; float: right; width: 160px;' />");
            //var searchbutton = $("<div style='float: right; margin-left: 5px;' title='איתור' ><img src='../scripts/app/images/search.gif'></div>");
            //searchbutton.jqxButton({ width: 16, height: 16 });
            //searchbutton.click(function (event) {
            //    var field = $("#fieldsOption").val();
            //    var value = $("#searchField").val();
            //    if (field != null && value != null) {
            //        me.timer = setTimeout(function () {
            //            app_jqxgrid.searchFilter(field, value, 'contains');
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
                            app_jqxgrid.autosearchFilter($("#fieldsOption").val(), $("#searchField").val());
                        }, 1000);
                        oldVal = input.val();
                    }
                }
                else {
                    $("#jqxgrid").jqxGrid('updatebounddata');
                }
            });
            collist.on('change', function (event) {
                var value = event.args.item.label;
                $("#searchField").attr("placeholder", "הקלד\\י  " + value + ' לסינון');
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
            var datecollist = $("<div id='dateOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י תאריך לסינון" });
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
            var datepicker = $("<div id='datepicker' style='float:right; display:inline;' title='נא לבחור מתאריך עד תאריך ולהמתין לביצוע הסינון'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range', rtl: true });
            $(datepicker).jqxDateTimeInput('setRange', date1, date2);
            datepicker.on('change', function (event) {
                var selection = $("#datepicker").jqxDateTimeInput('getRange');
                if (selection.from != null) {
                    //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
                    me.timer = setTimeout(function () {
                        app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0]);
                    }, 1000);
                }
            });
            containerDate.append(datecollist);
            containerDate.append(datepicker);
            container.append(containerDate);
        }
        toolbar.append(container);
    },
    gridFilterRtl: function (me, toolbar, columnsList,dateList){

        var width = $("#jqxgrid").jqxGrid('width');
        toolbar.css('width', width);

        //var me = this;
        var container = $("<div style='margin: 5px;direction:rtl'></div>");

        if (columnsList) {
            var containerSearch = $("<div style='margin: 5px;direction:rtl'></div>");
            //var span = $("<span style='float: right; margin-top: 5px; margin-right: 4px;'>איתור : </span>");
            var collist = $("<div id='fieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
            var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 22px; float: right; width: 120px;' />");
            var searchbutton = $("<div style='float: right; margin-left: 5px;' title='סינון\\ביטול' ><img src='../scripts/app/images/search.gif'></div>");
            //var searchbutton = $("<div style='float: right; display:inline;' title='איתור' ><input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 23px; float: right; width: 200px;' /><img src='../scripts/app/images/search.gif'></div>");
            searchbutton.jqxButton({ width: 16, height: 16 });
            searchbutton.click(function (event) {
                var field = $("#fieldsOption").val();
                var value = $("#searchField").val();
                
                if (field != null && value != null) {
                    var filtercondition = value.match(/^=/) ? 'EQUAL' : 'contains';
                    me.timer = setTimeout(function () {
                        app_jqxgrid.searchFilter(field, value.replace(/^=/, ""), filtercondition);
                    }, 1000);
                }
                else {
                    app_jqxgrid.clearFilter();
                    if (dateList)
                        $(datepicker).jqxDateTimeInput('setRange', null, null);
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
                            app_jqxgrid.searchFilter($("#fieldsOption").val(), input.val().replace(/^=/, ""), filtercondition);
                        }, 1000);
                    }
                    else {
                        app_jqxgrid.clearFilter();
                    }
                }
            })
            collist.on('change', function (event) {
                var value = event.args.item.label;
                $("#searchField").attr("placeholder", "הקלד\\י  " + value + ' לסינון');
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
            var field = $("#fieldsOption").val();
            var value = $("#searchField").val();
            if (field != null && value !=null) {
                me.timer = setTimeout(function () {
                    app_jqxgrid.searchFilter(field, value,'contains');
                }, 1000);
            }
        });
        */

        if (dateList) {
            //date filter=====================================================
            var containerDate = $("<div style='margin: 5px;direction:rtl'></div>");
            var datelabel;
            var datecolumn;
            var datecollist = $("<div id='dateOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: dateList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י תאריך לסינון" });
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
            var datepicker = $("<div id='datepicker' style='float:right; display:inline;' title='נא לבחור מתאריך עד תאריך ולהמתין לביצוע הסינון'></div>").jqxDateTimeInput({ width: 160, height: 22, selectionMode: 'range', rtl: true });
            $(datepicker).jqxDateTimeInput('setRange', date1, date2);
            datepicker.on('change', function (event) {
                var selection = $("#datepicker").jqxDateTimeInput('getRange');
                if (selection.from != null) {
                    //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
                    me.timer = setTimeout(function () {
                        app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0]);
                    }, 1000);
                }
            });

            //containerDate.append(datespan);
            containerDate.append(datecollist);
            containerDate.append(datepicker);
            //containerDate.append(datebutton);
            container.append(containerDate);
        }

        //var datebutton = $("<div style='float: right; margin-left: 5px;' title='סינון' ><img src='../scripts/app/images/refresh.gif'></div>");
        //datebutton.jqxButton({ width: 16, height: 16 });
        //datebutton.click(function (event) {
        //    var selection = $("#dateOption").jqxDateTimeInput('getRange');
        //    if (selection.from != null) {
        //        //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
        //        me.timer = setTimeout(function () {
        //            app_jqxgrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0]);
        //        }, 1000);
        //    }
        //});


        /*
        var datebutton = $("<input id='datebutton' type='button' value='...' style='height: 23px; float: right; width: 30px;' />")
        .on('click', function (event) {
            var selection = $("#dateOption").jqxDateTimeInput('getRange');
            if (selection.from != null) {
                //$("#selection").html("<div>From: " + selection.from.toLocaleDateString() + " <br/>To: " + selection.to.toLocaleDateString() + "</div>");
                me.timer = setTimeout(function () {
                    app_jqxgrid.rangeDateFilter(datecolumn, selection.from, selection.to);
                }, 1000);
                
            }
        });
        */


        toolbar.append(container);
    },
    clearFilter:function(){
        $("#jqxgrid").jqxGrid('clearfilters');
    },
    autosearchFilter: function (datafield, filtervalue) {
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
        $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
        $("#jqxgrid").jqxGrid('applyfilters');
    },
    searchFilter: function (datafield, filtervalue, filtercondition) {
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
        $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
        $("#jqxgrid").jqxGrid('applyfilters');
    },
    rangeDateFilter: function (datafield, filtervalueFrom, filtervalueTo) {
        //$("#jqxgrid").jqxGrid('clearfilters');

        if (datafield === undefined || filtervalueFrom==null || filtervalueTo==null) {
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
        
        $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
        $("#jqxgrid").jqxGrid('applyfilters');
    }
};
