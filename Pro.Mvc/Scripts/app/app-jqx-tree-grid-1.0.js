

//============================================================================================ app_jqxtreegrid

var app_jqxtreegrid = {
    
  
    
    gridFilterEx: function (me, toolbar, columnsList, dateList, dateRangeList, rtl, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        if (rtl === undefined || rtl == null)
            rtl = true;

        var width = $(tag_grid).jqxTreeGrid('width');
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
                collist = $("<div id='tbgridFieldsOption' style='float:right; display:inline;' title='נא לבחור את השדה הרצוי לפיו יבוצע הסינון'></div>").jqxDropDownList({ source: columnsList, width: 120, height: 22, rtl: true, autoDropDownHeight: true, placeHolder: "בחר\\י שדה לסינון" });
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
                        app_jqxtreegrid.searchFilter(field, value.replace(/^=/, ""), filtercondition, tag_grid);
                    }, 1000);
                }
                else {
                    app_jqxtreegrid.clearFilterEx(tag_grid);
                    if (dateList)
                        $(tbgridDatepicker).jqxDateTimeInput('setRange', null, null);
                }
            });
            cancelbutton.jqxButton({ width: 16, height: 16 });
            cancelbutton.click(function (event) {
                $("#tbgridSearchField").val('');
                //$("#tbgridFieldsOption").jqxDropDownList('selectIndex', -1);
                app_jqxtreegrid.clearFilterEx(tag_grid);
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
                        app_jqxtreegrid.clearFilterEx(tag_grid);
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
                        app_jqxtreegrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0], tag_grid);
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
                        app_jqxtreegrid.rangeDateFilter(datecolumn, d.toISOString().split('T')[0], date2.toISOString().split('T')[0], tag_grid);
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
        //            app_jqxtreegrid.rangeDateFilter(datecolumn, selection.from.toISOString().split('T')[0], selection.to.toISOString().split('T')[0],tag_grid);
        //        }, 1000);
        //    }
        //});

        toolbar.append(container);
    },
    clearFilter: function (tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxTreeGrid('clearFilters');
    },
    clearFilterEx: function (tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxTreeGrid('clearFilters');
        if ($("#tbgridDaysSelect").length) {
            $("#tbgridDaysSelect").jqxDropDownList('selectIndex', -1);
            $("#tbgridDaysSelect").show();
            $("#tbgridDatepicker").hide();
        }
    },
    autosearchFilter: function (datafield, filtervalue, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        //$("#jqxgrid").jqxTreeGrid('clearFilters');
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
        $(tag_grid).jqxTreeGrid('addFilter', datafield, filtergroup);
        $(tag_grid).jqxTreeGrid('applyFilters');
    },
    searchFilter: function (datafield, filtervalue, filtercondition, tag_grid) {

        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        //$("#jqxgrid").jqxTreeGrid('clearFilters');
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
        $(tag_grid).jqxTreeGrid('addFilter', datafield, filtergroup);
        $(tag_grid).jqxTreeGrid('applyFilters');
    },
    rangeDateFilter: function (datafield, filtervalueFrom, filtervalueTo, tag_grid) {
        //$("#jqxgrid").jqxTreeGrid('clearFilters');

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

        $(tag_grid).jqxTreeGrid('addFilter', datafield, filtergroup);
        $(tag_grid).jqxTreeGrid('applyFilters');
    },
    gridColumnsBar: function (columnList, tag_columns, tag_grid) {
        if (tag_columns === undefined || tag_columns == null)
            tag_columns = "#jqxGridColumns";
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";

        $(tag_columns).jqxDropDownList({ source: columnList, width: 200, height: 25, checkboxes: true, rtl: true, autoDropDownHeight: true });
        $(tag_columns).on('checkChange', function (event) {
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
    gridColumnsInit: function (columnList, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(columnList).each(function (index, item) {
            $(tag_grid).jqxTreeGrid('beginupdate');
            if (item.checked) {
                $(tag_grid).jqxTreeGrid('showcolumn', item.value);
            }
            else {
                $(tag_grid).jqxTreeGrid('hidecolumn', item.value);
            }
            $("#jqxgrid").jqxTreeGrid('endupdate');
        });

    },
    gridExpandAll: function (columnList, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxTreeGrid('expandallgroups');
    },
    griCollapseAll: function (columnList, tag_grid) {
        if (tag_grid === undefined || tag_grid == null)
            tag_grid = "#jqxgrid";
        $(tag_grid).jqxTreeGrid('collapseallgroups');
    }
};
