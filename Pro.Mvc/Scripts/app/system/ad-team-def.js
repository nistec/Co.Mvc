
//============================================================================================ app_ad_team_def

function app_ad_team_def(userInfo) {

    
    this.AccountId = userInfo.AccountId;
    this.UserRole = userInfo.UserRole;
    this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
    
    $("#AccountId").val(this.AccountId);
    this.gridWith = 500;
    this.sourceData = {};
    this.sourceUrl = '/System/AdTeamDefList';
    this.updateUrl = '/System/AdTeamDefUpdate';
    this.deleteUrl = '/System/AdTeamDefDelete';
    this.showMemmbersUrl = '/System/AdTeamShowMembers';
    this.fieldId = 'TeamId';
    this.RelUrl = '/System/AdTeamDefRel';
    this.RelToAddUrl = '/System/AdTeamDefRelToAdd';
    this.RelDeleteUrl = "/System/AdTeamDefRelDelete"
    this.RelUpdateUrl = "/System/AdTeamDefRelUpdate"
    this.rowEdit = -1;

    this.loadControls();
       
    var slf = this;

    this.createRowData = function () {
        var row = {
            TeamId: $("#TeamId").val(), TeamName: $("#TeamName").val()
        };
        return row;
    }

    this.setEditorInputData = function (dataRecord) {
        if (dataRecord === undefined || dataRecord===null) {
            $("#TeamId").val('');
            $("#TeamName").val('');
            $("#trcode").hide();
        }
        else {
            $("#trcode").show();
            $("#TeamId").val(dataRecord.TeamId);
            $("#TeamName").val(dataRecord.TeamName);
        }
    }

    this.createCoulmns = function () {

        var columns = [
           //{
           //    text: 'קוד צוות', datafield: 'TeamId', width: 60, cellsalign: 'right', align: 'center',
           //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
           //    return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="'+slf.showMemmbersUrl+'=' + value + '" title="הצג מנויים">' + value + '</a></div>'
           //}
           //},
            { text: 'שם צוות', datafield: 'TeamName', width: '75%',cellsalign: 'right', align: 'center' },
           { text: 'מנויים', datafield: 'MembersCount', width: '20%', cellsalign: 'right', align: 'center' }
        ];
       return columns;
    }
    
    this.createFields = function () {
        var datafields =
            [
                { name: 'TeamId', type: 'number' },
                { name: 'TeamName', type: 'string' },
                { name: 'TeamLead', type: 'number' },
                { name: 'MembersCount', type: 'number' }
            ];
        return datafields;
    }

    this.getDataCommand = function (rowid, rowdata, command) {

        if (command === 2)//delete
        {
            if (rowid <= 0) {
               app_dialog.alert("Invalid row id to delete!");
                return null;
            }
            return { 'TeamId': rowid };
        }
        else

            return { 'TeamId': command === 0 ? -1 : rowdata.TeamId, 'TeamName': rowdata.TeamName, 'command': command };

    }

    app_jqxgrid.loadEntityGrid(this);

    $("#jqxgrid").on('rowselect', function (event) {

        event.stopPropagation();
        var row = event.args.row;
        if (row) {
            var TeamId = row.TeamId;
            slf.nastedGridLoder(TeamId, false);
        }
    });

    slf.nastedGrid(-1);

    this.nastedGridLoder = function (id, all) {

        var src = $('#jqxgrid2').jqxGrid('source');

        if (all === false) {
            src._source.url = this.RelUrl;//'/System/AdDefRel';
            src._source.data = { 'id': id };
            $('#jqxgrid2').jqxGrid('source').dataBind();

            $("#jqxgrid2_Update").hide();
            $("#jqxgrid2_Cancel").hide();
            $("#jqxgrid2_Show").show();
            $("#liUsers").text("משתמשים");
            $("#pHelp").text("להסרת משתמש מקבוצה, יש לסמן את המשתמש הרצוי");
        }
        else {
            src._source.url = this.RelToAddUrl;// '/System/AdDefRelToAdd';
            src._source.data = { 'id': id };
            $('#jqxgrid2').jqxGrid('source').dataBind();


            $("#jqxgrid2_Update").show();
            $("#jqxgrid2_Cancel").show();
            $("#jqxgrid2_Show").hide();
            $("#liUsers").text("משתמשים להוספה");
            $("#pHelp").text("להוספת משתמשים לקבוצה, יש לסמן את המשתמשים הרצוים וללחוץ על עדכון");
        }
    };

    $("#jqxgrid").jqxGrid('selectrow', 0);
};

app_ad_team_def.prototype.loadControls = function () {

    // initialize the input fields.
    $("#TeamId").jqxInput().width(200);
    $("#TeamName").jqxInput().width(200);
   
    // initialize the popup window and buttons.
    $("#Cancel").jqxButton();
    $("#Save").jqxButton();

    $("#popupWindow").jqxWindow({
        width: 300, resizable: false, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.01
    });

    var input_rules = [
             { input: '#TeamName', message: 'חובה לציין צוות!', action: 'keyup, blur', rule: 'required' }
    ];

    //input_rules.push({ input: '#MemberId', message: 'חובה לציין ת.ז!', action: 'keyup, blur', rule: 'required' });

    $('#form').jqxValidator({
        rtl: true,
        //hintType: 'label',
        animationDuration: 0,
        rules: input_rules
    });

};

app_ad_team_def.prototype.nastedGrid = function (id) {


    var slf = this;

    var getCurrentId = function () {

        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        if (selectedrowindex >= 0) {
            var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
            return id;
        }
        return -1;
    }

    $("#CurrentId").val(id);

    $("#jqxgrid2_Show").show();
    $("#jqxgrid2_Update").hide();
    $("#jqxgrid2_Cancel").hide();


    var source =
    {
        //async:false,
        dataType: "json",
        datafields:
        [
            { name: 'UserId', type: 'number' },
            { name: 'DisplayName', type: 'string' },
            { name: 'ProfessionName', type: 'string' },
            { name: 'Phone', type: 'string' },
            { name: 'Email', type: 'string' },
            { name: 'TeamId', type: 'number' }
        ],
        //id: 'TeamId',
        data: { 'id': id },
        type: 'POST',
        url: '/System/AdDefRel'
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

    // initialize jqxGrid
    $('#jqxgrid2').jqxGrid(
   {
       rtl: true,
       scrollmode: 'default',
       width: '99%',//entity.gridWith,
       height: '99%',
       selectionmode: 'checkbox',
       //selectionmode: 'singlecell',
       //editmode: 'click',
       autoheight: false,
       source: dataAdapter,
       localization: getLocalization('he'),
       pageable: false,
       sortable: true,
       showstatusbar:true,
       //showtoolbar: true,
       //rendertoolbar: function (toolbar) {
       //    app_jqxgrid.renderToolbar(toolbar,entity)
       //},
       columns: [
             {
                 text: 'קוד משתמש', datafield: 'UserId', width: 60, cellsalign: 'right', align: 'center', hidden: true,
                 cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
                     return '<div style="text-align:center;direction:rtl;margin:5px;"><a href="showUserInfo=' + value + '" title="הצג משתמש">' + value + '</a></div>'
                 }
             },
             { text: ' שם ', datafield: 'DisplayName', cellsalign: 'right', align: 'center' },
              { text: 'תפקיד', datafield: 'ProfessionName', cellsalign: 'right', align: 'center' },
              { text: 'טלפון', datafield: 'Phone', cellsalign: 'right', align: 'center' },
              { text: 'דואל', datafield: 'Email', cellsalign: 'right', align: 'center' },
             { text: 'קבוצה', datafield: 'TeamId', width: 80, cellsalign: 'right', align: 'center', hidden: true }
       ]
   });

    //$("#jqxgrid2").on("bindingcomplete", function (event) {

    //   // var all = $("#liUsers").text() == "עריכה";
    //    if (all == true) {
    //        var records = $('#jqxgrid2').jqxGrid('getrows');

    //        var length = records.length;
    //        for (var i = 0; i < length; i++) {
    //            var record = records[i];
    //            if (record.TeamId == id) {
    //                $('#jqxgrid2').jqxGrid('selectrow', i);
    //            }
    //        }
    //    }
    //});




    $("#jqxgrid2_Show").on('click', function () {
        var id = getCurrentId();
        if (id < 0)
            return
        slf.nastedGridLoder(id, true);
    });

    $("#jqxgrid2_Update").on('click', function () {
        var id = getCurrentId();

        var rowindexes = $('#jqxgrid2').jqxGrid('getselectedrowindexes');
        var length = rowindexes.length;
        if (length > 0 && id > 0) {
            var records = $('#jqxgrid2').jqxGrid('getrows');
            var sel = [];
            for (var i = 0; i < length; i++) {
                var rowindex = rowindexes[i];
                var uid = $('#jqxgrid2').jqxGrid('getrowdata', rowindex).UserId;
                sel.push(uid)
            }
            var data = {
                TeamId: id, Users: sel.join()
            };
            //this.RelUpdateUrl = "/System/AdDefRelUpdate"

            app_query.doDataPost(slf.RelUpdateUrl, data, function (data) {
                $('#jqxgrid2').jqxGrid('clearselection');
                slf.nastedGridLoder(id, false);
            });
        }
    });

    $("#jqxgrid2_Cancel").on('click', function () {

        var id = getCurrentId();
        slf.nastedGridLoder(id, false);
    });

    //$("#jqxgrid2").on('rowunselect', function (event) {
    //    event.stopPropagation();
    //    //alert("Un Selected Row Indexes: " + event.args.rowindex);
    //    //$("#selectrowindex").text(event.args.rowindex);
    //    //app.cancelBubble(event);
    //});

    $("#jqxgrid2").on('rowselect', function (event) {
        event.stopPropagation();

        if ($("#liUsers").text() === "משתמשים") {
            var uid = $('#jqxgrid2').jqxGrid('getrowdata', event.args.rowindex).UserId;

            if (!confirm("האם להסיר את המשתמש המסומן מהקבוצה")) {
                $('#jqxgrid2').jqxGrid('unselectrow', event.args.rowindex);
                return;
            }
            var id = getCurrentId();
            var data = {
                TeamId: id, UserId: uid
            };
            //this.RelDeleteUrl = "/System/AdDefRelDelete"
            app_query.doDataPost(slf.RelDeleteUrl, data, function (data) {
                $('#jqxgrid2').jqxGrid('clearselection');
                $('#jqxgrid2').jqxGrid('source').dataBind();
            });
        }
    });
};