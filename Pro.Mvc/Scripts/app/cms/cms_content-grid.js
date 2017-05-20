
//============================================================================================ app_cms_content_grid

app_cms_content_grid = {

    AccountId:0,

    refresh: function () {
        $("#jqxgrid").jqxGrid('source').dataBind();
    },
    grid: function (userInfo,accountId) {

        this.AccountId = (accountId === undefined) ? userInfo.AccountId : accountId;
        var slf = this;
        
        var source =
        {

            //updaterow: function (rowid, rowdata, commit) {
            //    //sendCommand(rowdata, 1, commit);
            //    app_popup.cmsEditor(rowdata.ExtId);
            //},
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
                { name: 'Section', type: 'string' },
                { name: 'ContentType', type: 'string' },
                { name: 'Description', type: 'string' },
                { name: 'Category', type: 'string' },
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
            width: '90%',
            autoheight: true,
            source: dataAdapter,
            localization: getLocalization('he'),
            pageable: false,
            showtoolbar: true,
            sortable: true,
            showfilterrow: true,
            filterable: true,
            //rowdetails: true,
            //rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li class='title'></li></ul><div class='information'></div></div>", rowdetailsheight: 200 },
            //initrowdetails: initrowdetails,
            rendertoolbar: function (toolbar) {
                var me = this;
                var container = $("<div style='margin: 5px;text-align:right'></div>");
                toolbar.append(container);
                container.append('<input id="clearcachebutton" type="button" value="ניקוי זכרון עמודי רישום" />');
                container.append('<input id="clearcacheallbutton" type="button" value="ניקוי זכרון כללי" />');
                //container.append('<input id="addrowbutton" type="button" value="הוספה" />');
                //container.append('<input style="margin-left: 5px;" id="deleterowbutton" type="button" value="מחיקה" title="מחיקת השורה המסומנת"/>');
                container.append('<input id="updaterowbuttonhtml" type="button" value="עורך תוכן" title="עריכת השורה המסומנת באמצעות עורך תוכן"/>');
                container.append('<input id="updaterowbuttontext" type="button" value="עריכת טקסט" title="עריכת השורה המסומנת באמצעות עורך טקסט"/>');
                container.append('<input id="refreshbutton" type="button" value="רענון" />');
                //container.append('<input id="refreshbutton" type="button" value="הגדרות" />');
                //$("#addrowbutton").jqxButton();
                //$("#deleterowbutton").jqxButton();
                $("#updaterowbuttonhtml").jqxButton();
                $("#updaterowbuttontext").jqxButton();
                $("#refreshbutton").jqxButton();
                //$("#pagesettingsbutton").jqxButton();
                $("#clearcachebutton").jqxButton({ disabled: true });
                $("#clearcacheallbutton").jqxButton({ disabled: true });

                if (userInfo.UserRole > 5) {
                    $('#clearcachebutton').jqxButton({ disabled: false });
                    $('#clearcacheallbutton').jqxButton({ disabled: false });
                }
                // update row.
                $("#updaterowbuttonhtml").on('click', function () {

                    var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                    var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                    if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                        $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
                        // open the popup window when the user clicks a button.
                        editrow = selectedrowindex;
                        // get the clicked row's data and initialize the input fields.
                        var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);

                        app_popup.cmsHtmlEditor(dataRecord.ExtId);

                        //$("#AccountId").val(dataRecord.AccountId);
                        //$("#ExtId").val(dataRecord.ExtId);
                        //$("#PageType").val(dataRecord.PageType);
                        //$("#Field").val(dataRecord.Field);
                        //$("#ContentType").val(dataRecord.ContentType);
                        //openEditor(1);
                    }
                });
                $("#updaterowbuttontext").on('click', function () {

                    var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                    var rowscount = $("#jqxgrid").jqxGrid('getdatainformation').rowscount;
                    if (selectedrowindex >= 0 && selectedrowindex < rowscount) {
                        $("#jqxgrid").jqxGrid('ensurerowvisible', selectedrowindex);
                        // open the popup window when the user clicks a button.
                        editrow = selectedrowindex;
                        // get the clicked row's data and initialize the input fields.
                        var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);

                        app_popup.cmsTextEditor(dataRecord.ExtId);

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

                $("#clearcachebutton").on('click', function () {
                    if (confirm("פעולה זו תסיר את כל התכנים של עמודי הרישום מהזכרון, האם להמשיך ?")) {
                        app_query.doPost('/Cms/CmsClearRegistryAccountCache', {});
                    }
                });
                $("#clearcacheallbutton").on('click', function () {
                    if (confirm("פעולה זו תסיר את כל התכנים מהזכרון, האם להמשיך ?")) {
                        app_query.doPost('/Cms/CmsClearAllAccountCache', {});
                    }
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
            columns: [
            {
                text: 'קוד תוכן', datafield: 'ExtId', width: 200, cellsalign: 'right', align: 'center',
                    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                    //var accid = $('#jqxgrid').jqxGrid('getrowdata', row).AccountId;
                        var links = '<div style="text-align:center">';
                        var category = $('#jqxgrid').jqxGrid('getrowdata', row).Category;
                        var pagetype = $('#jqxgrid').jqxGrid('getrowdata', row).PageType;

                        if (category == "Page") {
                            pagetype = "'" + pagetype + "'";
                            links += '<a href="#" onclick="app_popup.cmsAdminPageSettings(' +slf.AccountId+','+ pagetype + ')" >הגדרות</a> |';
                        }
                        var exid = "'" + value + "'";

                        return links + '<a href="#" onclick="app_popup.cmsHtmlEditor(' + exid + ')" >עורך תוכן</a> | <a href="#" onclick="app_popup.cmsTextEditor(' + exid + ')" >עורך טקסט</a></div>';
                }
            },
            { text: 'סוג עמוד', datafield: 'PageType', cellsalign: 'right', align: 'center' },
            { text: 'תאור השדה', datafield: 'Description', cellsalign: 'right', align: 'center' },
            { text: 'סוג תוכן', datafield: 'Section', width: 200, cellsalign: 'right', align: 'center' },
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
            if (dataRecord.ContentType == "html")
                app_popup.cmsHtmlEditor(dataRecord.ExtId);
            else
                app_popup.cmsTextEditor(dataRecord.ExtId);
        });

        var openCmsEditor = function (ExtId) {

            if (ExtId.includes('InputFields')) {
                return app_dialog.dialogIframe("/Cms/CmsContentFields?extid=" + extid, "850", "600", "Cms Editor")
            }
            else {
                return app_dialog.dialogIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600", "Cms Editor")
            }

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

    load: function (userInfo, accountId) {
   
        this.grid(userInfo, accountId);

        return this;
    }
};

/*
<script> 
	(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){ 
	    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o), 
        m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m) 
	})(window,document,'script','//www.google-analytics.com/analytics.js','ga'); 
	
ga('create', 'UA-75210637-1', 'auto'); 
ga('send', 'pageview'); 
	
</script>
*/