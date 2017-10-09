
//============================================================================================ app_price_def

function app_price_def(userInfo) {

    
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    
    $("#AccountId").val(this.AccountId);
    this.gridWith = 500;
    this.sourceData = {};
    this.sourceUrl='/Common/GetPriceListView';
    this.updateUrl = '/Common/DefPriceUpdate';
    this.deleteUrl = '/Common/DefPriceDelete';
    this.rowEdit = -1;

    this.loadControls();
       
    var slf = this;

    this.createRowData = function () {
        var row = {
            PropId: $("#PropId").val(), PropName: $("#PropName").val(), Quota: $("#Quota").val(), Price: $("#Price").val()
        };
        return row;
    }

    this.setInputData = function (dataRecord) {
        if (dataRecord === undefined || dataRecord==null) {
            $("#PropId").val('');
            $("#PropName").val('');
            $("#Quota").val('');
            $("#Price").val('');
        }
        else {
            $("#PropId").val(dataRecord.PropId);
            $("#PropName").val(dataRecord.PropName);
            $("#Quota").val(dataRecord.Quota);
            $("#Price").val(dataRecord.Price);
        }
    }

    this.createCoulmns = function () {

        var columns = [
           { text: 'קוד מחיר', datafield: 'PropId', width: 60, cellsalign: 'right', align: 'center' },
           { text: 'שם מחיר', datafield: 'PropName', cellsalign: 'right', align: 'center' },
           { text: 'מכסה', datafield: 'Quota', width: 100, cellsalign: 'right', align: 'center' },
           { text: 'מחיר', datafield: 'Price', width: 100, cellsalign: 'right', align: 'center' }
        ];
       return columns;
    }

    this.createFields = function () {
        var datafields =
            [
                { name: 'PropId', type: 'number' },
                { name: 'PropName', type: 'string' },
                { name: 'Quota', type: 'number' },
                { name: 'Price', type: 'number' }
            ];
        return datafields;
    }

    this.getDataCommand = function (rowid, rowdata, command) {

        if (command == 2)//delete
        {
            if (rowid <= 0) {
               app_dialog.alert("Invalid row id to delete!");
                return null;
            }
            return { 'PropId': rowid };
        }
        else

            return { 'PropId': command == 0 ? -1 : rowdata.PropId, 'PropName': rowdata.PropName, 'Quota': rowdata.Quota, 'Price': rowdata.Price, 'command': command };

    }
       
       

    this.genericEntity= app_genericEntity_def(this);


    /*
    var sendCommand = function (rowid, rowdata, commit, command) {
        var data;
        if (command == 2)//delete
            var data = { 'PropId': rowdata.PropId, 'PropName': rowdata.PropName, 'Quota': rowdata.Quota, 'Price': rowdata.Price, 'command': command };
        else if (command == 1)//edit
            var data = { 'PropId': rowdata.PropId, 'PropName': rowdata.PropName, 'Quota': rowdata.Quota, 'Price': rowdata.Price, 'command': command };
        else if (command == 0)//add
            var data = { 'PropId': rowdata.PropId, 'PropName': rowdata.PropName, 'Quota': rowdata.Quota, 'Price': rowdata.Price, 'command': command };

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/Admin/DefPriceUpdate',
            data: data,
            success: function (data, status, xhr) {
                if (data.Status > 0) {
                    //dataAdapter.dataBind();
                    //alert('עודכן בהצלחה');
                    $('#jqxgrid').jqxGrid('source').dataBind();
                }
                else
                   app_dialog.alert('לא עודכנו נתונים');
                commit(true);
            },
            error: function () {
               app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
                // cancel changes.
                commit(false);
            }
        });
    }
    
    var source =
    {
        updaterow: function (rowid, rowdata, commit) {
            sendCommand(rowid, rowdata, commit,1);
        },
        addrow: function (rowid, rowdata, position, commit) {
            sendCommand(rowid, rowdata, commit,0);
        },
        deleterow: function (rowid, commit) {
            sendCommand(rowid, rowdata, commit,2);
        },
        dataType: "json",
        datafields: createFields(),
        id: 'PropId',
        data: {},
        type: 'POST',
        url: '/Common/GetPriceListViewAll'
    };



    var dataAdapter = new $.jqx.dataAdapter(source, {
        contentType: "application/json; charset=utf-8",
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
        width: 500,
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
            // update row.
            $("#updaterowbutton").on('click', function () {
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                doRowEdit(selectedrowindex);
 
            });
            // create new row.
            $("#addrowbutton").on('click', function () {
                // show the popup window.
                var offset = $("#jqxgrid").offset();
                $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 60, y: parseInt(offset.top) + 60 } });
                $("#popupWindow").jqxWindow('open');
                $("#insertFlag").val('0');
                $("#trcode").hide();
                $("#PropName").val('');
            });
            // delete row.
            $("#deleterowbutton").on('click', function () {
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                    var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
                    var result = confirm("האם למחוק?");
                    if (result == true) {
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
        columns: createCoulmns()
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
            var offset = $("#jqxgrid").offset();
            $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 60, y: parseInt(offset.top) + 60 } });
            // get the clicked row's data and initialize the input fields.
            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
            setInputData(dataRecord);

            // show the popup window.
            $("#popupWindow").jqxWindow('open');
        }
    };

    $("#jqxgrid").on('cellselect', function (event) {
        var column = $("#jqxgrid").jqxGrid('getcolumn', event.args.datafield);
        var value = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.datafield);
        var displayValue = $("#jqxgrid").jqxGrid('getcellvalue', event.args.rowindex, column.displayfield);
    });

    $('#jqxgrid').on('rowdoubleclick', function (event) {
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
        var row = createRowData();

        if ($("#insertFlag").val() == '0') {
            $('#jqxgrid').jqxGrid('addrow', null, row);
        }
        else if (editrow >= 0) {
            var rowID = $('#jqxgrid').jqxGrid('getrowid', editrow);
            $('#jqxgrid').jqxGrid('updaterow', rowID, row);
        }
        $("#popupWindow").jqxWindow('hide');
    });
*/
};

app_price_def.prototype.loadControls = function () {

    // initialize the input fields.
    $("#PropId").jqxInput().width(200);
    $("#PropName").jqxInput().width(200);
    $("#Quota").jqxInput().width(200);
    $("#Price").jqxInput().width(200);
 
    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var input_rules = [
             { input: '#PropName', message: 'חובה לציין שם פריט!', action: 'keyup, blur', rule: 'required' },
             { input: '#Quota', message: 'חובה לציין מכסה!', action: 'keyup, blur', rule: 'required' },
             {
                 input: '#Price', message: 'חובה לציין מחיר!', action: 'keyup, blur', rule: function () {
                     return app_jqx_validation.validateNumber("Price");
                 }
             }
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
