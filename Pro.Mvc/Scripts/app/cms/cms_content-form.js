
//============================================================================================ app_cms_content_grid

app_cms_content_grid = {

    
    AccountId:0,
    
    refresh:function(){

        $("#jqxgrid").jqxGrid('source').dataBind();

    },
    grid: function (userInfo) {

        this.AccountId=userInfo.AccountId;
        var slf = this;
        
        var source =
        {

            updaterow: function (rowid, rowdata, commit) {
                //sendCommand(rowdata, 1, commit);
                app_popup.cmsEditor(rowdata.ExtId);
            },
            //addrow: function (rowid, rowdata, position, commit) {
            //    sendCommand(rowdata, 0, commit);
            //},
            //deleterow: function (rowid, commit) {
            //    var rowdata = { 'AccountId': rowid }
            //    sendCommand(rowdata, 2, commit);
            //},
            dataType: "json",
            datafields:
            [
                { name: 'AccountId', type: 'number' },
                { name: 'PageType', type: 'string' },
                { name: 'Field', type: 'string' },
                { name: 'ContentType', type: 'string' },
                { name: 'Description', type: 'string' },
                { name: 'ExtId', type: 'string' }
            ],
            data: { 'acc': slf.AccountId },
            id: 'ExtId',
            type: 'POST',
            url: '/Cms/GetContentList'
        };
        var dataAdapter = new $.jqx.dataAdapter(source, {
            //contentType: "application/json; charset=utf-8",
            loadError: function (jqXHR, status, error) {
                //alert("dataAdapter failed: " + error);
            },
            loadComplete: function (data) {
                //alert("dataAdapter is Loaded");
            }
        });
        //dataAdapter.dataBind();

        var editrow = -1;
         // initialize jqxGrid
        $("#jqxgrid").jqxGrid(
        {
            rtl: true,
            width: '80%',
            autoheight: true,
            source: dataAdapter,
            localization: getLocalization('he'),
            pageable: false,
            showtoolbar: true,
            rendertoolbar: function (toolbar) {
                var me = this;
                var container = $("<div style='margin: 5px;text-align:right'></div>");
                toolbar.append(container);
                //container.append('<input id="addrowbutton" type="button" value="הוספה" />');
                //container.append('<input style="margin-left: 5px;" id="deleterowbutton" type="button" value="מחיקה" title="מחיקת השורה המסומנת"/>');
                container.append('<input id="updaterowbutton" type="button" value="עריכה" title="עריכת השורה המסומנת"/>');
                container.append('<input id="refreshbutton" type="button" value="רענון" />');
                //$("#addrowbutton").jqxButton();
                //$("#deleterowbutton").jqxButton();
                $("#updaterowbutton").jqxButton();
                $("#refreshbutton").jqxButton();
                
                // update row.
                $("#updaterowbutton").on('click', function () {

                    var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                    var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                    if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                        $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
                        // open the popup window when the user clicks a button.
                        editrow = selectedrowindex;
                        // get the clicked row's data and initialize the input fields.
                        var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);

                        app_popup.cmsEditor(dataRecord.ExtId);

                        //$("#AccountId").val(dataRecord.AccountId);
                        //$("#ExtId").val(dataRecord.ExtId);
                        //$("#PageType").val(dataRecord.PageType);
                        //$("#Field").val(dataRecord.Field);
                        //$("#ContentType").val(dataRecord.ContentType);
                        //openEditor(1);
                    }
                });

                /*
                // create new row.
                $("#addrowbutton").on('click', function () {
                    // show the popup window.
                    $("#ExtId").val('');
                    $("#PageType").val('');
                    $("#AccountId").val('');
                    $("#Field").val('');
                    $("#ContentType").val('');
                    openEditor(0);
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
                */
                // refresh grid.
                $("#refreshbutton").on('click', function () {
                    dataAdapter.dataBind();
                });
                //var openEditor = function (mode) {
                //    $("#insertFlag").val(mode);
                //    var popw = '#popupWindow';

                //    // open the popup window when the user clicks a button.
                //    var offset = $("#jqxgrid").offset();
                //    $(popw).jqxWindow({ position: { x: parseInt(offset.left) + parseInt(offset.width) / 2, y: parseInt(offset.top) + 60 } });
                //    // show the popup window.
                //    $(popw).jqxWindow('open');
                //};

            },
            [AccountId]
            ,[Field]
      ,[FieldName]
            ,[FieldOrder]
      ,[FieldType]
            ,[FieldLength]
      ,[InputType]
            ,[Enable]
      ,[Mandatory]
            ,[ValidateType]
      ,[MinLength]
            ,[ErrorRequirerMessage]
      ,[ErrorTypeMessage]

            columns: [
            {
                text: 'קוד תוכן', datafield: 'ExtId',  cellsalign: 'right', align: 'center'
                //    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                //    //var accid = $('#jqxgrid').jqxGrid('getrowdata', row).AccountId;
                //    //var field = $('#jqxgrid').jqxGrid('getrowdata', row).Field;
                //    return '<div style="text-align:center"><a href="#" onclick="openCmsEditor(' + value + ')" >עריכה</a></div>';
                //}
            },
            { text: 'תאור', datafield: 'Description', cellsalign: 'right', align: 'center' },
            //{ text: 'קטגוריה', datafield: 'PageType', cellsalign: 'right', align: 'center' },
            { text: 'סוג עמוד', datafield: 'Field', cellsalign: 'right', align: 'center' },
            { text: 'סוג נתונים', datafield: 'ContentType', width: 120, cellsalign: 'right', align: 'center' }
            ]
        });

        $('#jqxgrid').on('rowdoubleclick', function (event) 
        { 
            var args = event.args;
            // row's bound index.
            var boundIndex = args.rowindex;
            // row's visible index.
            //var visibleIndex = args.visibleindex;
            //// right click.
            //var rightclick = args.rightclick; 
            //// original event.
            //var ev = args.originalEvent;

            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', args.rowindex);
            app_popup.cmsEditor(dataRecord.ExtId);
        });

        var openCmsEditor = function (ExtId) {

            if (ExtId.includes('InputFields'))
            {

            }
            app_popup.cmsEditor(ExtId);

        }
/*
         // initialize the input fields.
        $("#PageType").jqxDropDownList({ theme: theme }).width(150);
        //$("#RemovePowerBy").jqxCheckBox({ theme: theme });

         //================= popup Update ================.
        $("#popupWindow").jqxWindow({
            width: 400, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
        });
        $("#popupWindow").on('open', function () {
            $("#ExtId").jqxInput('selectAll');
        });
        $('#popupWindow').jqxValidator({
            hintType: 'label',
            animationDuration: 0,
        });
        $('#popupWindow').on('validationSuccess', function (event) {
            var row = {
                ExtId: $("#ExtId").val(),
                PageType: $("#PageType").val(),
                AccountId: $("#AccountId").val(),
                Field: $("#Field").val(),
                ContentType: $("#ContentType").val()
            };
            if ($("#insertFlag").val() == '0') {
                $('#jqxgrid').jqxGrid('addrow', null, row);
            }
            else if (editrow >= 0) {
                var rowID = $('#jqxgrid').jqxGrid('getrowid', editrow);
                $('#jqxgrid').jqxGrid('updaterow', rowID, row);
            }
            $("#popupWindow").jqxWindow('hide');
        });
        $("#Cancel").jqxButton({ theme: theme });
        $("#Save").jqxButton({ theme: theme });
         // update the edited row when the user clicks the 'Save' button.
        $("#Save").click(function () {
            $('#popupWindow').jqxValidator('validate');

        });
*/
    },

    load: function (accountId) {
   
        this.grid(accountId);

        return this;
    }
};
