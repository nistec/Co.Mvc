
//============================================================================================ app_price_def

function app_genericEntity_def(entity) {

    //loadControls();

    //functions
    //var data = entity.getDataCommand();
    //var url = entity.getSourceUrl();
    //datafields: entity.getDataFields(),
    //url: entity.getViewUrl()
    //columns: entity.createCoulmns()
    //entity.setInputData(dataRecord);
    //var row = entity.createRowData();


    var updateCommand = function (rowid, rowdata, commit, command) {

        var data = entity.getDataCommand(rowid, rowdata, command);

        var url = command === 2 ? entity.deleteUrl : entity.updateUrl;

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: url,
            data: data,
            success: function (data, status, xhr) {
                if (data.Status > 0) {
                    //dataAdapter.dataBind();
                    app_jqxnotify.Info('עודכן בהצלחה', true);//app_dialog.alert('עודכן בהצלחה');
                }
                else
                    app_jqxnotify.Warning('לא עודכנו נתונים', true); //app_dialog.alert('לא עודכנו נתונים');
                commit(true);
            },
            complete: function (data) {
                $('#jqxgrid').jqxGrid('source').dataBind();
            },
            error: function () {
                app_jqxnotify.Error('אירעה שגיאה, לא עודכנו נתונים'); //app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                // cancel changes.
                commit(false);
            }
        });
    }

    var source =
    {
        updaterow: function (rowid, rowdata, commit) {
            updateCommand(rowid, rowdata, commit, 1);
        },
        addrow: function (rowid, rowdata, position, commit) {
            updateCommand(rowid, rowdata, commit, 0);
        },
        deleterow: function (rowid, commit) {
            updateCommand(rowid, null, commit, 2);
        },
        dataType: "json",
        datafields: entity.createFields(),
        id: 'PropId',
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
    $("#jqxgrid").jqxGrid(
    {
        rtl: true,
        width: '100%',//entity.gridWith,
        autoheight: false,
        source: dataAdapter,
        localization: getLocalization('he'),
        pageable: false,
        sortable: true,
        showtoolbar: true,
        rendertoolbar: function (toolbar) {
            var me = this;
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

            if (entity.AllowEdit === 0) {
                $("#addrowbutton").hide();
                $("#deleterowbutton").hide();
                $("#updaterowbutton").hide();
            }
            // update row.
            $("#updaterowbutton").on('click', function () {
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                doRowEdit(selectedrowindex);

            });
            // create new row.
            $("#addrowbutton").on('click', function () {
                // show the popup window.
                entity.setInputData();
                //var offset = $("#jqxgrid").offset();
                //$("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 60, y: parseInt(offset.top) + 60 } });
                //$("#popupWindow").jqxWindow('open');
                $("#insertFlag").val('0');
                $("#trcode").hide();
                $("#PropName").val('');


                app_jqxgrid.openPopupEditor();

            });
            // delete row.
            $("#deleterowbutton").on('click', function () {
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                    var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                    var result = confirm("האם למחוק?");
                    if (result === true) {
                        //Logic to delete the item
                        var commit = $("#jqxgrid").jqxGrid('deleterow', id);
                    }
                }
            });
            // refresh grid.
            $("#refreshbutton").on('click', function () {
                dataAdapter.dataBind();
            });
        },
        columns: entity.createCoulmns()
    });

    var doRowEdit = function (selectedrowindex) {

        //var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
            $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
            $("#insertFlag").val('1');
            $("#trcode").show();
            // open the popup window when the user clicks a button.
            editrow = selectedrowindex;

            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
            app_jqxgrid.openPopupEditor();
            entity.setInputData(dataRecord);

            //var offset = $("#jqxgrid").offset();
            //$("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 60, y: parseInt(offset.top) + 60 } });
            //// get the clicked row's data and initialize the input fields.
            //var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
            //entity.setInputData(dataRecord);

            //// show the popup window.
            //$("#popupWindow").jqxWindow('open');
        }
    };

    $("#jqxgrid").on('cellselect', function (event) {
        var column = $("#jqxgrid").jqxGrid('getcolumn', event.args.datafield);
        var value = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
        var displayValue = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
    });

    $('#jqxgrid').on('rowdoubleclick', function (event) {
        if (entity.AllowEdit === 0) {
            return;
        };
        var args = event.args;
        var boundIndex = args.rowindex;
        var visibleIndex = args.visibleindex;
        doRowEdit(boundIndex);
    });

    $("#popupWindow").on('open', function () {
        $("#PropName").jqxInput('selectAll');
    });

    // update the edited row when the user clicks the 'Save' button.
    $("#Save").click(function () {

        var validationResult = function (isValid) {
            if (isValid) {
                var row = entity.createRowData();

                if ($("#insertFlag").val() === '0') {
                    $('#jqxgrid').jqxGrid('addrow', null, row);
                }
                else if (editrow >= 0) {
                    var rowID = $('#jqxgrid').jqxGrid('getrowid', editrow);
                    $('#jqxgrid').jqxGrid('updaterow', rowID, row);
                }
                $("#popupWindow").jqxWindow('hide');
            }
        }
        $('#form').jqxValidator('validate', validationResult);

    });

    if (entity.AllowEdit === 0) {
        $("#Cancel").hide();
        $("#Save").hide();
        $(".note-edit").hide();
    }

    return this;
};

