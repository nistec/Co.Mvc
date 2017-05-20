
function app_uploads() {

    app_jqx_list.categoryComboAdapter();


    function progress() {
        var val = progressbar.progressbar( "value" ) || 0;
        progressbar.progressbar( "value", val + Math.floor( Math.random() * 3 ) );
        if ( val <= 99 ) {
            progressTimer = setTimeout( progress, 50 );
        }
    }

    $('#submit').on('click', function (e) {
        //var count= $("#TotalCount").val();
        //var validationResult = function (isValid) {
        //    if (isValid) {
        //        app_dialog.confirm("הדיוור יישלח ל " + count + " נמענים, האם להמשיך ?",
        //            callBack = function () {
        //            app_dialog.dialogProgress(null, 'Send Broadcast');
        //            $('#form').submit();
        //        });
        //    }
        //}
        //$('#form').jqxValidator('validate', validationResult);
    });

    $('#reset').on('click', function (e) {
        location.reload();
        //$('#form')[0].reset();
        //$('#form').jqxValidator('hide');
    });

    // column stg =====================================

    //var columnsstg = [
    //       { ColumnOrdinal: 0, CustomColumnName: "First Name", FieldName: "FirstName", DisplayName: "שם פרטי", SrcFormat: "",Condition:"",Enable:false },
    //       { ColumnOrdinal: 1, CustomColumnName: "Last Name", FieldName: "LastName", DisplayName: "שם משפחה", SrcFormat: "", Condition: "", Enable: false }
    //];
    var columnsstg = [];
    var columnsSource =
    {
        datatype: "array",
        datafields: [
            { name: 'CustomColumnName', type: 'string' },
            { name: 'ColumnOrdinal', type: 'number' },
            { name: 'FieldName', type: 'string' },
            { name: 'DisplayName', type: 'string' },
            { name: 'SrcFormat', type: 'string' },
            { name: 'Condition', type: 'string' },
            { name: 'Operator', type: 'string' },
            { name: 'FilterType', type: 'bool' },
            { name: 'Enable', type: 'bool' }
        ],
        localdata: columnsstg
    };
    //var columnsAdapter = new $.jqx.dataAdapter(columnsSource, {
    //    autoBind: true
    //});
    // prepare the data
    var fieldComboSource =
    {
        datatype: "array",
        localdata:
        [
            { value: "FirstName", label: "שם פרטי" },
            { value: "LastName", label: "שם משפחה" }
        ]
        //datafields: [
        //    // name - determines the field's name.
        //    // value - the field's value in the data source.
        //    // values - specifies the field's values.
        //    // values.source - specifies the foreign source. The expected value is an array.
        //    // values.value - specifies the field's value in the foreign source.
        //    // values.name - specifies the field's name in the foreign source.
        //    // When the adapter is loaded, each record will have a field called "Country". The "Country" for each record comes from the countriesAdapter where the record's "countryCode" from gridAdapter matches to the "value" from countriesAdapter.
        //    { name: 'DisplayName', value: 'FieldName', values: { source: columnsAdapter.records, value: 'value', name: 'label' } },
        //    { name: 'FieldName', type: 'string' }
        //]
    };
    var fieldComboAdapter = new $.jqx.dataAdapter(fieldComboSource, {
        autoBind: true
    });
 
    var operatorSource =
    {
        datatype: "array",
        //datafields: [
        //    { name: 'OpValue', type: 'string' },
        //    { name: 'OpName', type: 'string' }
        //],
        localdata: [
            { value: "", label: "ללא" },
            { value: "=", label: "שווה" },
            { value: ">=", label: "גדול שווה" },
            { value: ">", label: "גדול מ" },
            { value: "<=", label: "קטן שןןה" },
            { value: "<", label: "קטן מ" },
            { value: "^", label: "מכיל" },
            { value: "!", label: "אינו מכיל" },
            { value: "<>", label: "שונה" }
        ]
     };
    var operatorAdapter = new $.jqx.dataAdapter(operatorSource, {
        autoBind: true
    });

    var filterTypeSource =
    {
        datatype: "array",
        localdata: [
            { value: "0", label: "לסנן" },
            { value: "1", label: "לכלול" }
        ]
    };
    var filterTypeAdapter = new $.jqx.dataAdapter(filterTypeSource, {
        autoBind: true
    });

    var gridAdapter = new $.jqx.dataAdapter(columnsSource);
    $("#gridFields").jqxGrid(
    {
        width: '90%',
        rtl:true,
        source: gridAdapter,
        selectionmode: 'singlecell',
        autoheight: true,
        editable: true,
        editmode: 'click',
        columns: [
            { text: 'בחירה', datafield: 'Enable', columntype: 'checkbox', align: 'center' },
            { text: 'מס-סידורי', datafield: 'ColumnOrdinal', cellsalign: 'right', align: 'center',width:80 },
            { text: 'שם השדה בקובץ', datafield: 'CustomColumnName',cellsalign: 'right', align: 'center' },
            {
                text: 'שם השדה במערכת', datafield: 'FieldName', cellsalign: 'right', align: 'center', displayfield: 'DisplayName', columntype: 'dropdownlist',
                createeditor: function (row, value, editor) {
                    editor.jqxDropDownList({ autoDropDownHeight: true, source: fieldComboAdapter, displayMember: 'label', valueMember: 'value' });
                }
            },
            { text: 'פורמט', datafield: 'SrcFormat', align: 'center' },
            { text: 'לכלול', datafield: 'FilterType', columntype: 'checkbox', align: 'center' },
             //{
             //    text: 'סוג סינון', datafield: 'FilterType', align: 'center', displayfield: 'label', columntype: 'dropdownlist',
             //    createeditor: function (row, value, editor) {
             //        editor.jqxDropDownList({autoDropDownHeight: true, source: filterTypeAdapter, displayMember: 'label', valueMember: 'value' });
             //    }
             //},
            {
                text: 'אופרטור', datafield: 'Operator', align: 'center', displayfield: 'label', columntype: 'dropdownlist',
                createeditor: function (row, value, editor) {
                    editor.jqxDropDownList({ autoDropDownHeight: true, source: operatorAdapter, displayMember: 'label', valueMember: 'value' });
                }
            },
            { text: '  תנאים  ', datafield: 'Condition', cellsalign: 'right', align: 'center' }
        ]
    });
    $("#gridFields").on('cellselect', function (event) {
        var column = $("#gridFields").jqxGrid('getcolumn', event.args.datafield);
        var value = $("#gridFields").jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
        var displayValue = $("#gridFields").jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
        $("#eventLog").html("<div>Selected Cell<br/>Row: " + event.args.rowindex + ", Column: " + column.text + ", Value: " + value + ", Label: " + displayValue + "</div>");
    });

    //wizard ====================================
    //Creating wizard module
    var wizard = (function () {
        var _addHandlers = function () {
            $('.next-tab').click(function () {
                return wizard.next();
            });
            $('.prev-tab').click(function () {
                return wizard.prev();
            });
        };
        return {
            init: function () {
                var $tabs = $('#wizard').tabs();
                $('#wizard').css('height', '400px');
                $('.prev-tab').hide();
                $('.end-tab').hide();
                _addHandlers();
                wizard.goto(0);
            },
            size:function(){
                var totalSize = $(".ui-tabs-panel").size() - 1;
                return totalSize;
            },
            active: function () {
                var active = $('#wizard').tabs("option", "active");
                return active;
            },
            displayMessage: function (message) {
                $('#wizard-validation').val(message);
                return false;
            },
            upload: function (uploadKey) {
                wizard.goto(2);
                var src = '_UploadProc?uk=' + uploadKey;
                app_iframe.appendIframe("proc_iframe", src, "500px", "300px", "yes");
            },
            showHint: function (message, selector) {

                if (typeof selector === 'undefined') {
                    selector = '.hint';
                }

                if (message === undefined || message === '' || message == 'hint_message')
                    message = 'לטעינת מנויים יש ללחוץ על טעינת קובץ,להמתין להודעת סיום,לאחר מכן יש ללחוץ על הבא.';
                else if (message === 'wait_message')
                    message = 'נא להמתין להודעת סיום,לאחר מכן יש ללחוץ על הבא.';

                $(selector).html('<strong>' + message + '</strong>');
            },
            goto: function (index) {
                //0=file upload header
                //1=set mapping and upload stg
                //2=show stg
                //3=finished
                var totalSize = wizard.size();
                if (index >= totalSize) {
                    $('.prev-tab').hide();
                    $('.next-tab').hide();
                    $('.end-tab').show();
                }
                else if (index == 0) {
                    $('.prev-tab').hide();
                    $('.next-tab').show();
                }
                else {
                    $('.prev-tab').show();
                    $('.next-tab').show();
                    $('.end-tab').hide();
                }
                $('#wizard').tabs({ active: index });
                //wizard.displayStep(index, totalSize);

                var step = index+1;
                //if (totalSize === undefined)
                //    totalSize = $(".ui-tabs-panel").size() - 1;
                totalSize++;
                wizard.displayMessage("שלב " + step + " מתוך " + totalSize);

                return false;
            },
            next: function () {
                //var totalSize = wizard.size;
                var active = wizard.active();
                switch (active) {
                    case 0:
                        var value = $("#TotalCount").val();
                        if (value <= 0) {
                            return wizard.displayMessage("אין נמענים לשליחה");
                        }
                        break;
                }
                //$('#wizard-validation').val("");
                var next = active + 1;
                return wizard.goto(next);

                //if (next >= totalSize) {
                //    $('.next-tab').hide();
                //    $('.end-tab').show();
                //}
                //$('.prev-tab').show();
                //$('#wizard').tabs({ active: next });
                //wizard.displayStep(next, totalSize);
                //return false;
            },
            prev: function () {
                var active = wizard.active();
                var prev = active - 1;
                return wizard.goto(prev);

                //if (prev <= 0) {
                //    $('.prev-tab').hide();
                //}
                //$('.end-tab').hide();
                //$('.next-tab').show();
                //$('#wizard-validation').val("");
                //$('#wizard').tabs({ active: prev });
                //wizard.displayStep(prev);
                ////$tabs.tabs('select', $(this).attr("rel"));
                //return false;
            },
        }
    }());
    //Initializing the wizard
    wizard.init();


    //end wizard =====================================


    var mapUploadFields = function () {



    }

    ///////////////////////////// uploader
    $("#loader").hide();
    $('#fileupload').fileupload({
        maxFileSize: 10000000,
        url: '/Media/FileUploadHeader',
        //formData: {
        //    param1: $('#updateExists').val()
        //    //param2: $('#uploadUid').val(),
        //    //param3: $('#uploadPt').val()
        //},
        //dataType: 'json',
        done: function (e, data) {
            $("#loader").hide();
            //ObjectKeys(data);


            app_dialog.alert(data.result.Message.replace('<br/>', '\n'));//textStatus);
            $("#hintSection").html('<strong>' + data.result.Message + '</strong>');
            $("#divUploadResult").html(data.result.Message);
            $("#uploadKey").val(data.result.Args);
            $("#filename").val(data.files[0]);


            //connectUploadedGrid();

            $.each(data.files, function (index, file) {
                $('<p/>').text(file.name).appendTo('#files');
            });

            columnsSource.localdata =JSON.parse(data.result.Target);
            $("#gridFields").jqxGrid('source').dataBind();

            wizard.goto(1);
        },
        error: function (jqXHR, status, error) {
            $("#loader").hide();
            app_dialog.alert(error);
        },
        beforeSend: function (e, data) {
            wizard.showHint('wait_message','#hint-0');
        },
        progressall: function (e, data) {
            doPprogress(data);
            $("#loader").show();
        }
    }).prop('disabled', !$.support.fileInput)
         .parent().addClass($.support.fileInput ? undefined : 'disabled')
         .bind('fileuploadsubmit', function (e, data) {
             $("#loader").hide();
             resetPprogress();
             //fileuuid = generateUUID('16');
             //data.formData = {
             //    param1: $('#uploadBid').val(),
             //    param2: $('#uploadUid').val(),
             //    param3: $('#uploadPt').val()
             //};
         });

    //}).prop('disabled', !$.support.fileInput)
    //    .parent().addClass($.support.fileInput ? undefined : 'disabled');

    var doPprogress = function (data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progress .progress-bar').css(
            'width',
            progress + '%'
        );
    };
    var resetPprogress = function () {
        $('#progress .progress-bar').css(
            'width', '0%'
        );
    };

    var doUploadSync = function () {
        //var updateExists = $("#updateExists").val();
        var updateExists = $('#updateExists').is(":checked");

        var category = $("#listCategory").val();
        var uploadKey = $("#uploadKey").val();
        $.ajax({
            url: '/Media/ExecUploadAsync',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: "{'category':'" + category + "','uploadKey':'" + uploadKey + "', 'updateExists' : '" + updateExists + "'}",
            success: function (data) {
                if (data) {
                    if (data.Status < 0)
                        app_dialog.alert(data.Message);
                    else {

                        //var src = '_UploadProc?uk=' + uploadKey;
                        //app_iframe.appendIframe("proc_iframe", src, "500px", "300px", "yes");

                        //redirectTo('UploadProc?uk='+uploadKey);
                        wizard.upload(uploadKey);

                        //$("#final").html(data.Message);
                        ////$('#jqxTabs').jqxTabs('last');
                        //$('#jqxTabs').jqxTabs('enableAt', 3);
                        //$('#jqxTabs').jqxTabs('next');
                        //$('#jqxTabs').jqxTabs('disableAt', 2);
                        //$('#jqxTabs').jqxTabs('disableAt', 1);
                        //$('#jqxTabs').jqxTabs('disableAt', 0);
                        //$("#btnRefresh").show();
                    };
                }

            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
    };
    //==============================================================


    //$("#gridFields").on('cellendedit', function (event) {
    //    var column = $("#gridFields").jqxGrid('getcolumn', event.args.datafield);
    //    if (column.displayfield != column.datafield) {
    //        $("#eventLog").html("<div>Cell Edited:<br/>Index: " + event.args.rowindex + ", Column: " + column.text + "<br/>Value: " + event.args.value.value + ", Label: " + event.args.value.label
    //            + "<br/>Old Value: " + event.args.oldvalue.value + ", Old Label: " + event.args.oldvalue.label + "</div>"
    //            );
    //    }
    //    else {
    //        $("#eventLog").html("<div>Cell Edited:<br/>Row: " + event.args.rowindex + ", Column: " + column.text + "<br/>Value: " + event.args.value
    //            + "<br/>Old Value: " + event.args.oldvalue + "</div>"
    //            );
    //    }
    //});

    //column stg =====================================




};


