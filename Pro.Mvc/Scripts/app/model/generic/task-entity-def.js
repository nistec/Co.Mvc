
function getEntityTypeName(value) {

    switch (value) {

        case "task_type": return "סוג משימה";
        case "ticket_type": return "סוג כרטיס";
        case "topic_type": return "סוג נושא";
        case "doc_type": return "סוג מסמך";
        default:
            return "סוג משימה";
    }


}


//============================================================================================ app_entity_def

function app_task_entity_def(userInfo, entityType, tagPropTitle) {
       
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    this.EntityType = entityType;
    $("#AccountId").val(this.AccountId);
    this.gridWith = 500;
    this.sourceData = { 'entity': entityType };
    this.sourceUrl = '/System/DefEntityView';
    this.updateUrl = '/System/DefEntityUpdate';
    this.deleteUrl = '/System/DefEntityUpdate';
    this.rowEdit = -1;

    this.tagPropId = "קוד";
    this.tagPropName = "תאור";
    this.tagPropTitle = tagPropTitle;

    var slf = this;

    this.loadControls();
    this.loadEntity(entityType, tagPropTitle);
        
    this.createRowData = function () {
        var row = {
            PropId: $("#PropId").val(), PropName: $("#PropName").val()
        };
        return row;
    }

    this.setInputData = function (dataRecord) {
        if (dataRecord === undefined || dataRecord === null) {
            $("#PropId").val('');
            $("#PropName").val('');
        }
        else {
            $("#PropId").val(dataRecord.PropId);
            $("#PropName").val(dataRecord.PropName);
        }
    }

    this.createCoulmns = function () {
        var slf = this;
        var columns = [
           //{ text: slf.tagPropId, datafield: 'PropId', width: 60, cellsalign: 'right', align: 'center' },
           { text: slf.tagPropName, datafield: 'PropName', cellsalign: 'right', align: 'center' }
        ];
       return columns;
    }

    this.createFields = function () {
        var datafields =
            [
                { name: 'PropId', type: 'number' },
                { name: 'PropName', type: 'string' }
            ];
        return datafields;
    }

    this.getDataCommand = function (rowid, rowdata, command) {

        var slf = this;

        if (command === 2)//delete
        {
            if (rowid <= 0)
            {
               app_dialog.alert("Invalid row id to delete!");
                return null;
            }
            return { 'PropId': rowid, 'PropName': null, 'EntityType': slf.EntityType, 'command': command };
        }
        else if (command === 1 && rowid > 0)//edit
            return { 'PropId': rowdata.PropId, 'PropName': rowdata.PropName, 'EntityType': slf.EntityType, 'command': command };
        else if (command === 0)//add
            return { 'PropId': -1, 'PropName': rowdata.PropName, 'EntityType': slf.EntityType, 'command': command };
    }

    //entity grig ===================================================

    this.updateCommand = function (rowid, rowdata, commit, command) {

        var data = slf.getDataCommand(rowid, rowdata, command);

        var url = command === 2 ? slf.deleteUrl : slf.updateUrl;

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: url,
            data: data,
            success: function (data, status, xhr) {
                if (data.Status > 0) {
                    //dataAdapter.dataBind();
                    //alert('עודכן בהצלחה');
                }
                else
                   app_dialog.alert('לא עודכנו נתונים');
                commit(true);
            },
            complete: function (data) {
                $('#jqxgrid').jqxGrid('source').dataBind();
            },
            error: function () {
               app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                // cancel changes.
                commit(false);
            }
        });
    };

    this.source =
    {
        updaterow: function (rowid, rowdata, commit) {
            slf.updateCommand(rowid, rowdata, commit, 1);
        },
        addrow: function (rowid, rowdata, position, commit) {
            slf.updateCommand(rowid, rowdata, commit, 0);
        },
        deleterow: function (rowid, commit) {
            slf.updateCommand(rowid, null, commit, 2);
        },
        dataType: "json",
        datafields: slf.createFields(),
        id: 'PropId',
        data: slf.sourceData,
        type: 'POST',
        url: slf.sourceUrl
    };

    this.dataAdapter = new $.jqx.dataAdapter(this.source, {
        async: false,
        //contentType: "application/json; charset=utf-8",
        loadError: function (jqXHR, status, error) {
            //alert("dealAdapter failed: " + error);
        },
        loadComplete: function (data) {
            //alert("dealAdapter is Loaded");
        }
    });

    this.editrow = -1;

        // initialize jqxGrid
        $("#jqxgrid").jqxGrid(
        {
            rtl: true,
            width: '100%',//entity.gridWith,
            autoheight: false,
            source: slf.dataAdapter,
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

                if (slf.AllowEdit === 0) {
                    $("#addrowbutton").hide();
                    $("#deleterowbutton").hide();
                    $("#updaterowbutton").hide();
                }
                // update row.
                $("#updaterowbutton").on('click', function () {
                    var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                    slf.doRowEdit(selectedrowindex);

                });
                // create new row.
                $("#addrowbutton").on('click', function () {
                    // show the popup window.
                    slf.setInputData();
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
                    slf.dataAdapter.dataBind();
                });
            },
            columns: slf.createCoulmns()
        });

    this.doRowEdit = function (selectedrowindex) {

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
            slf.setInputData(dataRecord);
        }
    };

    $("#jqxgrid").on('cellselect', function (event) {
        var column = $("#jqxgrid").jqxGrid('getcolumn', event.args.datafield);
        var value = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
        var displayValue = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
    });

    $('#jqxgrid').on('rowdoubleclick', function (event) {

        if (slf.AllowEdit === 0) {
            return;
        };
        var args = event.args;
        var boundIndex = args.rowindex;
        var visibleIndex = args.visibleindex;
        slf.doRowEdit(boundIndex);
    });

    $("#popupWindow").on('open', function () {
        $("#PropName").jqxInput('selectAll');
    });

    // update the edited row when the user clicks the 'Save' button.
    $("#Save").click(function () {

        var validationResult = function (isValid) {
            if (isValid) {
                var row = slf.createRowData();

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

    if (this.AllowEdit === 0) {
        $("#Cancel").hide();
        $("#Save").hide();
        $(".note-edit").hide();
    }

    return this;

};
app_task_entity_def.prototype.loadEntity = function (entityType, tagPropTitle) {

    this.EntityType = entityType;
    this.sourceData = { 'entity': entityType };
    this.tagPropTitle = tagPropTitle;

    switch (entityType) {
        case "branch":
            this.tagPropId = "קוד סניף";
            this.tagPropName = "שם סניף";
            break;
        case "category":
            this.tagPropId = "קוד סיווג";
            this.tagPropName = "שם סיווג";
            break;
        default:
            this.tagPropId = "קוד";
            this.tagPropName = "תאור";
            break;
    }

    $('#tagPropId').html(this.tagPropId);
    $('#tagPropName').html(this.tagPropName);
    $('#tagPropTitle').html(this.tagPropTitle);
};
app_task_entity_def.prototype.loadControls = function () {


    // initialize the input fields.
    //$("#PropId").jqxInput().width(200);
    //$("#PropName").jqxInput().width(200);
    $("#PropId").prop('disabled', true);

    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var input_rules = [
             { input: '#PropName', message: 'חובה לציין תאור!', action: 'keyup, blur', rule: 'required' }
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
app_task_entity_def.prototype.reloadData = function () {
    var slf = this;
    this.dataAdapter._source.datafields = this.createFields();
    this.dataAdapter._source.data = this.sourceData;

    //this.dataAdapter = new $.jqx.dataAdapter(this.source, {
    //    async: false,
    //    //contentType: "application/json; charset=utf-8",
    //    loadError: function (jqXHR, status, error) {
    //        //alert("dealAdapter failed: " + error);
    //    },
    //    loadComplete: function (data) {
    //        //alert("dealAdapter is Loaded");
    //    }
    //});

    $("#jqxgrid").jqxGrid('source').dataBind();//: this.dataAdapter });

    $("#jqxgrid").jqxGrid(
        {
            columns: slf.createCoulmns()
        });

}