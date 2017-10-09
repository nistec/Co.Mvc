

//============================================================================================ app_form

var app_jqxform = {

    setLinkHref: function (input, taglink, isEmail) {

        var val = $('#' + input).val();
        if (isEmail)
            val = 'mailto:' + val;
        if (val === undefined || val.length < 10) {
            $('a#' + taglink).attr('href', '');
            $('a#' + taglink).hide();
        }
        else {
            $('a#' + taglink).attr('href', val);
            $('a#' + taglink).show();
        }
    },

    setInputValue: function (tag, value, asEmpty) {
        if (value != asEmpty)
            $("#" + tag).val(value);
    },

    clearDataForm: function (form) {
        $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
            var input = $(this);
            var type = input.attr('type');
            if (type == 'check' || type == 'radio')
                input.checked = false;
            else
                input.val('');
        });
    },
    loadDataForm: function (form, record,isjqx) {
        
        if (isjqx === undefined)
            isjqx = true;

        $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
            var input = $(this);

            var tag = input.attr('name');
            if (tag !== undefined) {
                //var type=input.prop('type');
                //var id = input.attr('id');
                var value = record[tag];

                if (isjqx) {
                    if (value !== undefined && value != null) {
                        var str = value.toString();
                        if (str.match(/Date/gi)) {
                            var d = formatJsonShortDate(value)
                            $('form#' + form + ' [name=' + tag + ']').val(d);  //$('#' + tag).val(d);
                        }
                        else if (typeof value === 'boolean')
                            $('form#' + form + ' [name=' + tag + ']').attr("checked", value);  //$('#' + tag).attr("checked", value);
                        else
                            $('form#' + form + ' [name=' + tag + ']').val(value);  //$('#' + tag).val(value);
                    }
                    else {
                        $('form#' + form + ' [name=' + tag + ']').val(null);  //$('#' + tag).val(null);
                    }
                }
                else {
                    if (value !== undefined && value != null) {
                        var str = value.toString();
                        if (str.match(/Date/gi)) {
                            var d = formatJsonShortDate(value)
                            input.val(d);
                        }
                        else if (typeof value === 'boolean')
                            input.attr("checked", value);
                        else
                            input.val(value);
                    }
                    else {
                        input.val(null);
                    }
                }
            }
        });
    },
    getItemsInForm: function (form, tags, attrName) {
        if (attrName === undefined)
            attrName = 'name';

        var inputs = [];
        $('#' + form + ' input, #' + form + ' select, #' + form + ' textarea').each(function (index) {
            var input = $(this);
            var tag = input.attr(attrName);
            if (jQuery.inArray(tag, tags) >=0) {
                inputs.push(input);
            };
        });
        return inputs;
    },
    findInputInForm: function (form, tag, attrName, inputType) {
        if (attrName === undefined)
            attrName = 'name';
        if (inputType === undefined)
            inputType = 'input';

        return $(form).find(inputType+'[' + attrName + '=' + tag + ']');
    },
    CreateDateTimeInput: function (input) {
        $("#" + input).jqxDateTimeInput({ formatString: 'dd/MM/yyyy', value: null });
    },
    jqxForm_Redirect: function (form, redirectTo, action) {
        if (action == null || action === undefined)
            action = $(form).attr('action');

        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: action,
                    type: 'POST',
                    dataType: 'json',
                    data: $(form).serialize(),
                    success: function (data) {
                        if (data.Status > 0) {
                            app.redirectTo(redirectTo);
                        }
                        else {
                           app_dialog.alert(data.Message);
                        }
                    },
                    error: function (jqXHR, status, error) {
                       app_dialog.alert(error);
                    }
                });
            }
        }
        $(form).jqxValidator('validate', validationResult);
    },
    jqxForm_Funk: function (form, onsuccess, action) {
        if (action == null || action === undefined)
            action = $(form).attr('action');

        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: action,
                    type: 'POST',
                    dataType: 'json',
                    data: $(form).serialize(),
                    success: function (data) {
                        onsuccess(data);
                    },
                    error: function (jqXHR, status, error) {
                       app_dialog.alert(error);
                    }
                });
            }
        }
        $(form).jqxValidator('validate', validationResult);
    }
    //validateCombo: function (input) {
    //    var index = $("#" + input).jqxComboBox('getSelectedIndex');
    //    if (index >= 0) { return true; } return false;
    //},

    //validateNumber: function (input) {
    //    var value = $("#" + input).val();
    //    return value > 0;
    //},

    //validateNumeric: function (input) {
    //    var value = $("#" + input).val();
    //    return isNumeric(value);
    //},

    //validateDate: function (input, minYear, maxYear) {
    //    var date = $("#" + input).jqxDateTimeInput('value');
    //    if (date == null || date === undefined)
    //        return false;

    //    if (typeof minYear === 'undefined' || minYear == null) { minYear = 2000; }
    //    if (typeof maxYear === 'undefined' || maxYear == null) { maxYear = 2999; }

    //    return date.getFullYear() >= minYear && date.getFullYear() <= maxYear;
    //},

};

//============================================================================================ app_combos

var app_jqxcombos = {

    getSelectedComboValue: function (tag, defaultValue) {
        var item = $("#" + tag).jqxComboBox('getSelectedItem');
        if (!item)
            return defaultValue;
        return item.value;
    },

    getSelectedListValue: function (tag, defaultValue) {
        var item = $("#" + tag.replace('#', '')).jqxListBox('getSelectedItem');
        if (!item)
            return defaultValue;
        return item.value;
    },

    initComboValue: function (tag, isValue) {
        if ($("#" + tag.replace('#', '')).val() == isValue)
            $("#" + tag.replace('#', '')).val('');
        //var item = $("#" + tag).jqxComboBox('getSelectedItem');
        //if (item && item.value == isValue)
        //    $("#" + tag).val('');
    },
    selectComboBoxValue: function (tag, value) {
        var item = $("#" + tag.replace('#', '')).jqxComboBox('getItemByValue', value);
        if (item)
            $("#" + tag.replace('#', '')).jqxComboBox('selectIndex', item.index);
    },

    selectComboCheckBoxValues: function (tag, values) {
        if (values) {
            var items = values.toString().split(",");
            for (index = 0; index < items.length; ++index) {
                var item = $("#" + tag.replace('#', '')).jqxComboBox('getItemByValue', items[index]);
                if (item)
                    $("#" + tag.replace('#', '')).jqxComboBox('checkIndex', item.index);
            }
        }
    },

    selectComboBoxValues: function (tag, values) {
        if (values) {
            var items = values.toString().split(",");
            for (index = 0; index < items.length; ++index) {
                var item = $("#" + tag.replace('#', '')).jqxComboBox('getItemByValue', items[index]);
                if (item)
                    $("#" + tag.replace('#', '')).jqxComboBox('selectIndex', item.index);
            }
        }
    },

    getFirstCheckedComboValue: function (tag, defaultValue) {
        var items = $("#" + tag.replace('#', '')).jqxComboBox('getCheckedItems');
        if (items && items.length > 0)
            return items[0].value;
        return defaultValue;
    },

    getSelectedDropDownValue: function (tag, defaultValue) {
        var item = $("#" + tag.replace('#', '')).jqxDropDownList('getSelectedItem');
        if (!item)
            return defaultValue;
        return item.value;
    },

    selectDropDownValue: function (tag, value) {
        var item = $("#" + tag.replace('#', '')).jqxDropDownList('getItemByValue', value);
        if (item)
            $("#" + tag.replace('#', '')).jqxDropDownList('selectIndex', item.index);
    },

    selectCheckListIndex: function (tag, value) {
        var item = $("#" + tag.replace('#', '')).jqxListBox('getItemByValue', value);
        if (item) {
            $("#" + tag.replace('#', '')).jqxListBox('checkIndex', item.index);
        }
    },

    selectCheckList: function (tag, value, output) {
        if (value) {
            var items = value.toString().split(",");
            for (index = 0; index < items.length; ++index) {
                app_jqxcombos.selectCheckListIndex(tag, items[index]);
            }
            if (output)
                $("#" + output.replace('#', '')).val(value);
        }
    },
    clearCheckList: function (tag) {
        $(tag).jqxListBox('uncheckAll');
    },
    getListCheckedValues: function (list) {
        var items = $("#" + list.replace('#', '')).jqxListBox('getCheckedItems');
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += items[i].value;
                if (i < items.length - 1) values += ",";
            }
        }
        return values;
    },

    getComboCheckedValues: function (list) {
        var items = $("#" + list.replace('#', '')).jqxComboBox('getCheckedItems');
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += items[i].value;
                if (i < items.length - 1) values += ",";
            }
        }
        return values;
    },

    getComboSelectedValues: function (list) {
        var items = $("#" + list.replace('#', '')).jqxComboBox('getSelectedItems');
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += items[i].value;
                if (i < items.length - 1) values += ",";
            }
        }
        return values;
    },

    renderCheckList: function (tag, dest) {
        var val = $("#" + tag.replace('#', '')).val();
        $("#" + dest.replace('#', '')).val(val);
    },

    getDataCheckedList: function (records, field) {
        var length = records.length;
        var newsType = '';
        for (var i = 0; i < length; i++) {
            var record = records[i];
            var val = record[field];
            newsType += val + ',';
        }
        if (newsType.length > 0)
            newsType = newsType.substring(0, newsType.length - 1);
        return newsType;
    },
    createComboBox: function (srcAdapter,valueMember, displayMember, tag, width, dropDownHeight) {
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $("#" + tag.replace('#', '')).jqxComboBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            displayMember: displayMember,
            valueMember: valueMember
        });
    },
    createListBox: function (srcAdapter,valueMember, displayMember, tagList, width, height, output) {
        $("#" + tagList.replace('#', '')).jqxListBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            height: height,
            displayMember: displayMember,
            valueMember: valueMember
        });
        if (output) {
            $("#" + tagList.replace('#', '')).on('change', function (event) {
                listBoxToInput(tagList, output);
            });
        }
    },
    createComboAdapter: function (valueMember, displayMember, tag, url, width, dropDownHeight, async) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $("#" + tag.replace('#', '')).jqxComboBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            displayMember: displayMember,
            valueMember: valueMember
        });
        return srcAdapter;
    },

    createComboSelectorAdapter: function (valueMember, displayMember, selector, url, width, dropDownHeight, async) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $(selector).jqxComboBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            displayMember: displayMember,
            valueMember: valueMember
        });
        return srcAdapter;
    },

    setComboSourceAdapter: function (valueMember, displayMember, tag, srcAdapter, width, dropDownHeight, async) {
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $("#" + tag.replace('#','')).jqxComboBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            displayMember: displayMember,
            valueMember: valueMember
        });
    },

    createComboCheckAdapter: function (valueMember, displayMember, tag, url, width, dropDownHeight, async, output) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $("#" + tag.replace('#', '')).jqxComboBox(
        {
            rtl: true,
            checkboxes: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            displayMember: displayMember,
            valueMember: valueMember
        });
        if (output) {
            $("#" + tag.replace('#', '')).on('checkChange', function (event) {
                app_jqxcombos.comboCheckBoxToInput(tag, output);
            });
        }
        return srcAdapter;
    },

    createDropDownAdapter: function (valueMember, displayMember, tag, url, width, dropDownHeight, async, placeHolder) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof placeHolder === 'undefined' || placeHolder == '') { placeHolder = 'נא לבחור'; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $("#" + tag.replace('#', '')).jqxDropDownList(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            placeHolder: placeHolder,
            displayMember: displayMember,
            valueMember: valueMember
        });
        return srcAdapter;
    },
    createDropDownAdapterTag: function (valueMember, displayMember, tag, url, width, dropDownHeight, async, placeHolder) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        var autoDropDownHeight = true;
        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof placeHolder === 'undefined' || placeHolder == '') { placeHolder = 'נא לבחור'; }
        if (typeof dropDownHeight === 'undefined' || dropDownHeight == 0)
            dropDownHeight = 200;
        else
            autoDropDownHeight = false;

        $(tag).jqxDropDownList(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            dropDownHeight: dropDownHeight,
            autoDropDownHeight: autoDropDownHeight,
            placeHolder: placeHolder,
            displayMember: displayMember,
            valueMember: valueMember
        });
        return srcAdapter;
    },
    createListAdapter: function (valueMember, displayMember, tagList, url, width, height, async, output) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        $("#" + tagList.replace('#', '')).jqxListBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            height: height,
            displayMember: displayMember,
            valueMember: valueMember
        });
        if (output) {
            $("#" + tagList.replace('#', '')).on('change', function (event) {
                listBoxToInput(tagList, output);
            });
        }
        return srcAdapter;
    },

    createCheckListAdapter: function (valueMember, displayMember, tagList, url, width, height, async, output) {
        var srcAdapter = app_jqx.createtDataAdapter(valueMember, displayMember, url, async);
        $("#" + tagList.replace('#', '')).jqxListBox(
        {
            rtl: true,
            source: srcAdapter,
            width: width,
            checkboxes: true,
            height: height,
            displayMember: displayMember,
            valueMember: valueMember
        });
        if (output) {
            $("#" + tagList.replace('#', '')).on('checkChange', function (event) {
                app_jqxcombos.listCheckBoxToInput(tagList, output);
            });
        }
        return srcAdapter;
    },

    listBoxToInput: function (list, input, checkbox, setlabel) {
        //$('#' + list).on('change', function (event) {
        var items = $("#" + list.replace('#', '')).jqxListBox('getSelectedItems');
        var checked = true;
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += setlabel ? items[i].label : items[i].value;
                if (i < items.length - 1) values += ",";
            }
            checked = false;
        }
        if (checkbox)
            $("#" + checkbox.replace('#', '')).prop('checked', checked);
        $("#" + input.replace('#', '')).val(values);
    },
    listCheckBoxOutput: function (list, inputValue,inputLabel, checkbox) {
        //$('#' + list).on('checkChange', function (event) {
        var items = $("#" + list.replace('#', '')).jqxListBox('getCheckedItems');
        var checked = true;
        var values = "";
        var labels = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += items[i].value;
                labels += items[i].label;
                if (i < items.length - 1) values += ",";
            }
            checked = false;
        }
        if (checkbox)
            $("#" + checkbox.replace('#', '')).prop('checked', checked);
        $("#" + inputValue.replace('#', '')).val(values);
        $("#" + inputLabel.replace('#', '')).val(labels);
    },
    listCheckBoxToInput: function (list, input, checkbox, setlabel) {
        //$('#' + list).on('checkChange', function (event) {
        var items = $("#" + list.replace('#', '')).jqxListBox('getCheckedItems');
        var checked = true;
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += setlabel? items[i].label : items[i].value;
                if (i < items.length - 1) values += ",";
            }
            checked = false;
        }
        if (checkbox)
            $("#" + checkbox.replace('#', '')).prop('checked', checked);
        $("#" + input.replace('#', '')).val(values);
    },
    listCheckBoxToPersonal: function (list, input, checkbox, setlabel) {
        //$('#' + list).on('checkChange', function (event) {
        var items = $("#" + list.replace('#', '')).jqxListBox('getCheckedItems');
        var checked = true;
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += "["
                values +=  setlabel ? items[i].label : items[i].value;
                values += "]";
                if (i < items.length - 1) values += ";";
            }
            checked = false;
        }
        if (checkbox)
            $("#" + checkbox.replace('#', '')).prop('checked', checked);
        $("#" + input.replace('#', '')).val(values);
    },
    listCheckBoxToPersonalFields: function (list, inputLabel,inputValue) {
        
        $("#" + inputLabel.replace('#', '')).val('');
        $("#" + inputValue.replace('#', '')).val('');

        var items = $("#" + list.replace('#', '')).jqxListBox('getCheckedItems');
        var values = "";
        var labels = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += "[" + items[i].value + "]";
                labels += "[" + items[i].label + "]";

                if (i < items.length - 1) {
                    values += ";";
                    labels += ";";
                }
            }
        }

        $("#" + inputLabel.replace('#', '')).val(labels);
        $("#" + inputValue.replace('#', '')).val(values);
    },
    comboCheckBoxToInput: function (list, input, checkbox, setlabel) {
        //$('#' + list).on('checkChange', function (event) {
        var items = $("#" + list.replace('#', '')).jqxComboBox('getCheckedItems');
        var checked = true;
        var values = "";
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                values += setlabel ? items[i].label : items[i].value;
                if (i < items.length - 1) values += ",";
            }
            checked = false;
        }
        if (checkbox)
            $("#" + checkbox.replace('#', '')).prop('checked', checked);
        $("#" + input.replace('#', '')).val(values);
    },
    comboBoxToInput: function (list, input, checkbox, setlabel) {
        //$('#' + list).on('change', function (event) {
        var item = $("#" + list.replace('#', '')).jqxComboBox('getSelectedItem');
        var checked = true;
        var value = "";
        if (item) {
            value = setlabel ? item.label : item.value;
            checked = false;
        }
        if (checkbox)
            $("#" + checkbox.replace('#', '')).prop('checked', checked);
        $("#" + input).val(value);
    },
    remoteCombo: function (valueMember, displayMember, tag, url, width, dropDownHeight, async, output) {

        if (typeof width === 'undefined' || width == 0) { width = 240; }
        if (typeof async === 'undefined') { async = true; }
        var source =
                {
                    dataType: "json",
                    async: async,
                    dataFields: [
                        { name: valueMember },
                        { name: displayMember }
                    ],
                    type: 'POST',
                    url: url
                };

        var dataAdapter = new $.jqx.dataAdapter(source,
            {
                formatData: function (data) {
                    if ($(tag).jqxComboBox('searchString') != undefined) {
                        data.name_startsWith = $(tag).jqxComboBox('searchString');
                        return data;
                    }
                }
            }
        );
        $(tag).jqxComboBox(
        {
            width: width,
            height: 25,
            source: dataAdapter,
            remoteAutoComplete: true,
            autoDropDownHeight: true,
            selectedIndex: 0,
            displayMember: displayMember,
            valueMember: valueMember,
            renderer: function (index, label, value) {
                var item = dataAdapter.records[index];
                if (item != null) {
                    //var label =  item.name + "(" + item.countryName + ", " + item.adminName1 + ")";
                    return item.label; //label;
                }
                return "";
            },
            renderSelectedItem: function (index, item) {
                var item = dataAdapter.records[index];
                if (item != null) {
                    //var label = item.name;
                    return item.label; //label;
                }
                return "";
            },
            search: function (searchString) {
                dataAdapter.dataBind();
            }
        });
    }

};

//============================================================================================ app_jqx

var app_jqx = {
    openWindow: function (tagWindow, tagBody) {
        if (tagBody === undefined || tagBody == null)
            tagBody = 'body';
        //var cancelBtn;
        //if (tagCancel && tagCancel != null)
        //    cancelBtn=  $(tagCancel)
        //$(tagWindow).jqxWindow({
        //    width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: cancelBtn, modalOpacity: 0.01
        //});

        // open the popup window when the user clicks a button.
        var offset = $(tagBody).offset();
        var outerWidth = $(tagBody).outerWidth();
        var popupWidth = $(tagWindow).outerWidth();
        var posx = parseInt(offset.left) + parseInt(outerWidth) / 2;
        posx = posx - (parseInt(popupWidth) / 2);
        var posy = parseInt(offset.top) + 100;
        $(tagWindow).jqxWindow({ position: { x: posx, y: posy } });
        // show the popup window.
        $(tagWindow).jqxWindow('open');

        //var offset = $(tagBody).offset();
        //$(tagWindow).jqxWindow({ position: { x: parseInt(offset.left) + parseInt(offset.width) / 2, y: parseInt(offset.top) + 60 } });
        //$(tagWindow).jqxWindow('open');
    },
    displayWindow: function (tagWindow, tagBtn,ismodal) {

        var size = app_global.documentWidth();
        var height = size[1];
        var width = size[0];
        
        var offset = $(tagBtn).offset();
        var btnWidth = $(tagBtn).outerWidth();
        var btnHeight = $(tagBtn).outerHeight();

        var maxWidth = Math.min(width-50, $(tagWindow).outerWidth());
        var posx = Math.max(0, parseInt(btnWidth) + parseInt(offset.left) - maxWidth);
        var posy = parseInt(offset.top) + btnHeight;
        $(tagWindow).jqxWindow({ rtl: true, isModal: ismodal, width: maxWidth, maxWidth: maxWidth, position: { x: posx, y: posy } });
        $(tagWindow).jqxWindow('open');
    },
    closeWindow: function (tagWindow) {
        $(tagWindow).jqxWindow('close');
    },
    displayPopover: function (tagWindow, tagBtn,title,ismodal) {

        if (ismodal === undefined)
            ismodal = false;
        if (title === undefined)
            title = '';

        var size = app_global.documentWidth();
        var height = size[1];
        var width = size[0];
        
        var offset = $(tagBtn).offset();
        var btnWidth = $(tagBtn).outerWidth();
        var btnHeight = $(tagBtn).outerHeight();

        var maxWidth = Math.min(width-50, $(tagWindow).outerWidth());
        var posx = Math.max(0, parseInt(btnWidth) + parseInt(offset.left) - maxWidth);
        var posy = parseInt(offset.top) + btnHeight;

        //$("#popover").jqxPopover({ offset: { left: -50, top: 0 }, arrowOffsetValue: 50, title: "Employees", showCloseButton: true, selector: $("#button") });

        var offest_left = parseInt($(tagBtn).outerWidth()) / 2 * -1;

        $(tagWindow).jqxPopover({ rtl: true, showCloseButton: true, autoClose: true, width: maxWidth, title: title, selector: $(tagBtn), position: 'bottom', offset: { left: offest_left, top: 0 } });
        $(tagWindow).jqxPopover('open');
    },
    closePopover: function (tagWindow) {
        $(tagWindow).jqxPopover('close');
    },

    createtDataAdapter: function (valueMember, displayMember, url, async) {
        if (typeof async === 'undefined') { async = true; }
        var source =
                {
                    dataType: "json",
                    async: async,
                    dataFields: [
                        { name: valueMember },
                        { name: displayMember }
                    ],
                    type: 'POST',
                    url: url
                };
        var srcAdapter = new $.jqx.dataAdapter(source);
        return srcAdapter;
    },
    createtDataAdapterData: function (valueMember, displayMember, url, async,data) {
        if (typeof async === 'undefined') { async = true; }
        var source =
                {
                    dataType: "json",
                    async: async,
                    dataFields: [
                        { name: valueMember },
                        { name: displayMember }
                    ],
                    type: 'POST',
                    data:data,
                    url: url
                };
        var srcAdapter = new $.jqx.dataAdapter(source);
        return srcAdapter;
    },
    loaderOpen: function (loaderTag) {
        if (loaderTag === undefined)
            loaderTag = "#loader";
        $(loaderTag).jqxLoader({ width: 100, height: 60, imagePosition: 'top', autoOpen: true });
        $(loaderTag).jqxLoader('open');
    },
    loaderClose: function (loaderTag) {
        if (loaderTag === undefined)
            loaderTag = "#loader";
        $(loaderTag).jqxLoader('close');
    },
    toolTipClick: function (tag, content, width) {
        //if (tag === undefined || tag == null || tag == 'div') {
        //   var tagDiv = $('<div class="popover" style="direction:rtl;">' + content + '</div>');
        //   tag = '.popover';
        //}
        $(tag).jqxTooltip({
            trigger: 'click' ,
            //showArrow:true, 
            autoHide: false ,
            closeOnClick: true, 
            position: 'top',
            theme:'energyblue',
            content: content,
            width: width
            //height:30
        });
    },
    createDropDown: function (tag, source, width, tagTargetOnChange) {
        //var source = [{
        //    text: "Affogato",
        //    value: 0
        //}, 
        //{
        //    text: "Americano",
        //    value: 1
        //} 
        //];
        if (typeof width === 'undefined' || width == 0) { width = 240; }

        // Create a jqxDropDownList
        $(tag).jqxDropDownList({
            rtl: true,
            autoDropDownHeight:true,
            source: source,
            theme: 'energyblue',
            width: width,
            height: '25px',
            displayMember: 'text',
            selectedIndex: 0,
            valueMember: 'value'
        });
        if (tagTargetOnChange)
            $(tag).on('change', function (event) {
                $(tagTarget).val(event.args.item.value);
            });
    }
};

//============================================================================================ app_jqx_validation
var app_jqx_validation = {

    comboSelectedIndex: function (tag, input, commit) {
         var index = $(tag).jqxComboBox('getSelectedIndex');
        return index != -1;
    },
    validateCombo: function (input) {
        var index = $("#" + input.replace('#', '')).jqxComboBox('getSelectedIndex');
        if (index >= 0) { return true; } return false;
    },
    validateDropDown: function (input) {
        var index = $("#" + input.replace('#', '')).jqxDropDownList('getSelectedIndex');
        if (index >= 0) { return true; } return false;
    },
    validateNumber: function (input) {
        var value = $("#" + input.replace('#', '')).val();
        return value > 0;
    },
    validateNumberWzero: function (input) {
        var value = $("#" + input.replace('#', '')).val();
        return value >= 0;
    },
    validateNumeric: function (input) {
        var value = $("#" + input.replace('#', '')).val();
        return $.isNumeric(value);
    },
    validateDate: function (input, minYear, maxYear) {
        var date = $("#" + input.replace('#', '')).jqxDateTimeInput('value');
        if (date == null || date === undefined)
            return false;

        if (typeof minYear === 'undefined' || minYear == null) { minYear = 2000; }
        if (typeof maxYear === 'undefined' || maxYear == null) { maxYear = 2999; }

        return date.getFullYear() >= minYear && date.getFullYear() <= maxYear;
    }
};


//============================================================================================ app_notify

var app_jqxnotify = {

    openNotification: function (msg, container, auto, onClose) {
        if (typeof auto === 'undefined') { auto = false; }

        var d = $('<div>' + msg + '</div>').jqxNotification({
            width: '92%', position: "top-right", opacity: 0.9, rtl: true, appendContainer: container, browserBoundsOffset: 60,
            autoOpen: true, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: "info"
        });
        if (onClose) {
            d.on('close', function () { onClose() });
        }
        //d.jqxNotification("open");

        return d;
    },

    Error: function (data, callback, args) {
        app_jqxnotify.Dialog(data, "error", false, callback, args);
    },
    Warning: function (data, auto, callback, args) {
        app_jqxnotify.Dialog(data, "warning", auto, callback, args);
    },
    Info: function (data, auto, callback, args) {
        app_jqxnotify.Dialog(data, "info", auto, callback, args);
    },
    Success: function (data, auto, callback, args) {
        app_jqxnotify.Dialog(data, "success", auto, callback, args);
    },

    //template='info''warning''success''error''mail''time'null
    /*
    notificationData: function (data, auto, template, offset, onClose) {
        if (typeof auto === 'undefined') { auto = false; }
        if (typeof template === 'undefined') { template = "info"; }
        if (typeof offset === 'undefined') { offset = 0; }
        var width = (offset > 0) ? '92%' : '99%';
        var msg = (typeof data === 'string') ? data : data.Message;
        //var msg = (data instanceof Object) ? data.Message : data;

        var d = $('<div>' + msg + '</div>').jqxNotification({
            width: width, position: "top-right", opacity: 0.9, rtl: true, browserBoundsOffset: offset,
            autoOpen: true, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template
        });
        if (onClose && template != "error") {
            d.on('close', function () { onClose(data) });
        }
        if (auto)
            d.jqxNotification("open");
        return d;
    },
    */
    Dialog: function (data, template, auto, callback, args) {
        if (typeof auto === 'undefined') { auto = false; }
        if (typeof template === 'undefined') { template = "info"; }
        var msg = (typeof data === 'string') ? data : data.Message;
        //var template = isError ? "error" : "info";
        //appendTo('body').

        //var d = $('<div>' + msg + '</div>').css('text-align', 'center').jqxNotification({
        //    width: '100%', position: "top-right", opacity: 0.9, rtl: true, browserBoundsOffset: 0,
        //    autoOpen: false, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template
        //});
        //var d = $('#jqxNotification').css('text-align', 'center').jqxNotification({
        //    width: '100%', position: "top-right", opacity: 0.9, rtl: true, browserBoundsOffset: 0,
        //    autoOpen: false, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template
        //});

        //var notif = document.getElementById('jqxNotify');//$('body').find('#jqxNotify');
        ////if (part.length > 0)
        ////    part.children().remove();

        //if (notif === null) {
        //    notif = $('<div />').appendTo('body');
        //    notif.attr('id', 'jqxNotify');
        //}
        //var content = $('<div />').appendTo(notif);
        //content.attr('id', 'notifContent');

        var notif = $('#jqxNotification');
        //var content = $('#jqxNotifContent');
        //notif.jqxNotification({ autoOpen: true, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template });
        notif.jqxNotification({
            width: '100%', position: "top-right", opacity: 0.9, rtl: true, browserBoundsOffset: 0,
            autoOpen: true, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template
        });

        var d = $('<div>' + msg + '</div>').css('text-align', 'center');
        if (callback && template != "error") {
            d.on('close', function () { (args) ? callback(args) : callback(data) });
        }

        notif.empty();
        d.appendTo(notif);

        //content.empty();
        //d.appendTo(content);

        //var $divNotif = $('<div />').appendTo('body');
        //$divNotif.attr('id', 'jqxNotify');

        //var $divContent = $('<div />').appendTo('body');
        //$divContent.attr('id', 'notifContent');




        //if (auto)
        //var d = $('<div>' + msg + '</div>').css('text-align', 'center').appendTo("#jqxNotification");

        //$('#jqxNotification').append(msg);
        //var notif=$('#jqxNotification');
        //notif.jqxNotification({ animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template });
        //notif.jqxNotification({
        //    width: '100%', position: "top-right", opacity: 0.9, rtl: true, browserBoundsOffset: 0,
        //    autoOpen: false, animationOpenDelay: 800, autoClose: auto, autoCloseDelay: 3000, template: template
        //});
        //d.appendTo(notif);
        //notif.append(d);
        notif.jqxNotification("open");
        //return d;
    }
};


//debug
function debugObjectKeys(obj) {

    for (var o in obj) {
        var i = o;
    }
 
    // Visit non-inherited enumerable keys
    Object.keys(obj).forEach(function (key) {
        console.log(key, obj[key]);
    });

    for (var key in validation_messages) {
        if (validation_messages.hasOwnProperty(key)) {
            var obj = validation_messages[key];
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    //alert(prop + " = " + obj[prop]);
                    console.log(prop + " = " + obj[prop]);
                }
            }
        }
    }
}
