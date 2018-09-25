
//============================================================================================ app_category_def

function app_category_def(userInfo) {

    
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    
    $("#AccountId").val(this.AccountId);
    this.gridWith = 500;
    this.sourceData = {};
    this.sourceUrl = '/Common/GetCategoryListView';
    this.updateUrl = '/Common/DefCategoryUpdate';
    this.deleteUrl = '/Common/DefCategoryDelete';
    this.rowEdit = -1;

    this.loadControls();
       
    var slf = this;

    this.createRowData = function () {
        var row = {
            PropId: $("#PropId").val(), PropName: $("#PropName").val()
        };
        return row;
    }

    this.setInputData = function (dataRecord) {
        if (dataRecord === undefined || dataRecord==null) {
            $("#PropId").val('');
            $("#PropName").val('');
        }
        else {
            $("#PropId").val(dataRecord.PropId);
            $("#PropName").val(dataRecord.PropName);
        }
    }

    this.createCoulmns = function () {

        var columns = [
           { text: 'קוד סיווג', datafield: 'PropId', width: 60, cellsalign: 'right', align: 'center' ,
           cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
               return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="/Co/Members?Category=' + value + '" title="הצג מנויים">' + value + '</a></div>'
           }
           },
           { text: 'שם סיווג', datafield: 'PropName', cellsalign: 'right', align: 'center' },
           { text: 'מנויים', datafield: 'MembersCount', width: 80, cellsalign: 'right', align: 'center' }
        ];
       return columns;
    }

    this.createFields = function () {
        var datafields =
            [
                { name: 'PropId', type: 'number' },
                { name: 'PropName', type: 'string' },
                { name: 'MembersCount', type: 'number' }
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

            return { 'PropId': command == 0 ? -1 : rowdata.PropId, 'PropName': rowdata.PropName, 'command': command };

    }

    this.genericEntity= app_genericEntity_def(this);
};

app_category_def.prototype.loadControls = function () {

    // initialize the input fields.
    $("#PropId").jqxInput().width(200);
    $("#PropName").jqxInput().width(200);
   
    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var input_rules = [
             { input: '#PropName', message: 'חובה לציין סיווג!', action: 'keyup, blur', rule: 'required' }
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};
