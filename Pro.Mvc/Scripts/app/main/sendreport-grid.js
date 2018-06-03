
//============================================================================================ app_members_grid

app_sendreport_grid = {

    //accType: 0,
    dataAdapter: {},

    source:
        {
            datatype: "json",
            async: false,
            //datafields: [
            //   { name: 'AccountId', type: 'number' },
            //   { name: 'MemberId', type: 'string' },
            //   { name: 'MemberName', type: 'string' },
            //   { name: 'CellPhone', type: 'string' },
               
            //   { name: 'PayId', type: 'number' },
            //   { name: 'Payed', type: 'number' },
            //   { name: 'Creation', type: 'date' },
            //   { name: 'TransIndex', type: 'string' },
            //   { name: 'ConfirmationCode', type: 'string' },

            //   { name: 'SignupDate', type: 'date' },
            //   { name: 'ValidityMonth', type: 'number' },
            //   { name: 'CreditCardOwner', type: 'bool' },
            //   { name: 'ExpirationDate', type: 'date' },
            //   { name: 'TotalRows', type: 'number' }
            //],
            id: 'PayId',
            type: 'POST',
            url: '/Main/GetSendReportGrid',
            //data:{},
            pagenum: 1,
            pagesize: 20,
            //root: 'Rows',
            pager: function (pagenum, pagesize, oldpagenum) {
                // callback called when a page or page size is changed.
            },
            //filter: function () {
            //    // update the grid and send a request to the server.
            //    $("#jqxgrid").jqxGrid('updatebounddata');
            //},
            //sort: function () {
            //    // update the grid and send a request to the server.
            //    $("#jqxgrid").jqxGrid('updatebounddata');
            //},
            //beforeprocessing: function (data) {
            //    this.totalrecords = data.TotalRows;
            //}
        },

    getTotalRows: function (data) {
        if (data) {
            return dataTotalRows(data);
        }
        return 0;
    },

    buildFilterPanel: function (filterPanel, datafield) {
        var textInput = $("<input style='margin:5px;'/>");
        var applyinput = $("<div class='filter' style='height: 25px; margin-left: 20px; margin-top: 7px;'></div>");
        var filterbutton = $('<span tabindex="0" style="padding: 4px 12px; margin-left: 2px;">סינון</span>');
        applyinput.append(filterbutton);
        var filterclearbutton = $('<span tabindex="0" style="padding: 4px 12px; margin-left: 5px;">הסר סינון</span>');
        applyinput.append(filterclearbutton);
        filterPanel.append(textInput);
        filterPanel.append(applyinput);
        filterbutton.jqxButton({ height: 20 });
        filterclearbutton.jqxButton({ height: 20 });
         var column = $("#jqxgrid").jqxGrid('getcolumn', datafield);
        textInput.jqxInput({ placeHolder: "הקלד " + column.text, maxLength: 25, popupZIndex: 9999999, height: 23, width: 155 });
        textInput.keyup(function (event) {
            if (event.keyCode === 13) {
                filterbutton.trigger('click');
            }
        });
        filterbutton.click(function () {
            var filtergroup = new $.jqx.filter();
            var filter_or_operator = 1;
            var filtervalue = textInput.val();
            var filtercondition = 'contains';
            var filter1 = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
            filtergroup.addfilter(filter_or_operator, filter1);
            // add the filters.
            $("#jqxgrid").jqxGrid('addfilter', datafield, filtergroup);
            // apply the filters.
            $("#jqxgrid").jqxGrid('applyfilters');
            $("#jqxgrid").jqxGrid('closemenu');
        });
        filterbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterbutton.trigger('click');
            }
        });
        filterclearbutton.click(function () {
            $("#jqxgrid").jqxGrid('removefilter', datafield);
            // apply the filters.
            $("#jqxgrid").jqxGrid('applyfilters');
            $("#jqxgrid").jqxGrid('closemenu');
        });
        filterclearbutton.keydown(function (event) {
            if (event.keyCode === 13) {
                filterclearbutton.trigger('click');
            }
            textInput.val("");
        });
    },

    grid: function () {
        var slf = this;

        var initrowdetails = function (index, parentElement, gridElement, datarecord) {

            var tabsdiv = null;
            var information = null;

            tabsdiv = $($(parentElement).children()[0]);
            if (tabsdiv != null) {
                information = tabsdiv.find('.information');
                var title = tabsdiv.find('.title');
                title.text(datarecord.MemberName);

                var container = $('<div style="margin: 5px;text-align:right;"></div>')
                container.rtl = true;
                container.appendTo($(information));

                var leftcolumn = $('<div style="float: left; width: 45%;direction:rtl;"></div>');
                var rightcolumn = $('<div style="float: right; width: 40%;direction:rtl;"></div>');
                container.append(leftcolumn);
                container.append(rightcolumn);

   
              var divLeft = $("<div style='margin: 10px;'><b>מספר תשלום:</b> " + datarecord.PayId + "</div>" +
                    "<div style='margin: 10px;'><b>מספר תשלום בחברת האשראי:</b> " + datarecord.TransIndex + "</div>" +
                    "<div style='margin: 10px;'><b>קוד אישור חברת האשראי:</b> " + datarecord.ConfirmationCode + "</div>" +
                    "<div style='margin: 10px;'><b>מועד תשלום:</b> " + datarecord.Creation.toLocaleDateString() + "</div>");

                divLeft.rtl = true;
                var divRight = $("<div style='margin: 10px;'><b>טלפון:</b> " + datarecord.CellPhone + "</div>" +
                    "<div style='margin: 10px;'><b>בעל הכרטיס:</b> " + datarecord.CreditCardOwner + "</div>" +
                    "<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.MemberId + "</div>");

                divRight.rtl = true;

                $(leftcolumn).append(divLeft);
                $(rightcolumn).append(divRight);

                $(tabsdiv).jqxTabs({ width: '95%', height: 170, rtl: true });
            }
        };

        var renderstatusbar = function (statusbar) {
            // appends buttons to the status bar.
            var container = $("<div style='overflow: hidden; position: relative; margin: 5px;float:right;'></div>");
            var reloadButton = $("<div style='float: left; margin-left: 5px;' title='רענון'><img src='../scripts/app/images/refresh.gif'><span style='margin-left: 4px; position: relative;'>רענון</span></div>");
            var clearFilterButton = $("<div style='float: left; margin-left: 5px;' title='הסר סינון' ><img src='../scripts/app/images/filterRemove.gif'><span style='margin-left: 4px; position: relative;'>הסר סינון</span></div>");
            //var queryButton = $("<div style='float: left; margin-left: 5px;' title='איתור' ><img src='../scripts/app/images/search.gif'><span style='margin-left: 4px; position: relative;'>איתור</span></div>");
            //var searchButton = $("<div style='float: left; margin-left: 5px;'><span style='margin-left: 4px; position: relative; top: -3px;'>Find</span></div>");
            container.append(reloadButton);
            container.append(clearFilterButton);
            //container.append(searchButton);
            //container.append(queryButton);
            statusbar.append(container);
            reloadButton.jqxButton({ width: 70, height: 20 });
            clearFilterButton.jqxButton({ width: 70, height: 20 });
            //queryButton.jqxButton({ width: 70, height: 20 });
            //searchButton.jqxButton({ width: 50, height: 20 });
            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
            //queryButton.click(function (event) {
            //    app.redirectTo('/Main/MembersQuery');
            //});

            // search for a record.
            //searchButton.click(function (event) {
            //    var offset = $("#jqxgrid").offset();
            //    $("#jqxwindow").jqxWindow('open');
            //    $("#jqxwindow").jqxWindow('move', offset.left + 30, offset.top + 30);
            //});
        };

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
        {
            width: '100%',
            autoheight: true,
            rtl: true,
            source: slf.dataAdapter,
            localization: getLocalization('he'),
            //virtualmode: true,
            rendergridrows: function (obj) {
                return slf.dataAdapter.records;
            },
            pageable: true,
            pagermode: 'simple',
            sortable: true,
            //showfilterrow: true,
            filterable: true,
            //rowdetails: true,
            //rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li class='title'></li></ul><div class='information'></div></div>", rowdetailsheight: 200 },
            //initrowdetails: initrowdetails,
            autoshowfiltericon: true,
            //showstatusbar: true,
            //renderstatusbar: renderstatusbar,

//Status, 
//StatusName, 
//NotifyStatus,
//AckStatus,
//MessageId, 
//BatchId, 
//Target, 
//CampaignId,
//Platform,
//Sender,
//ItemIndex,
//Personal,
//OperatorId,
//OpId, 
//Price, 
//Units,
//Size, 
//SentTime,
//AccountId, 
//MtId
            columns: [
              {
                  text: '*', dataField: 'Status', filterable: false, width: 100, cellsalign: 'right', align: 'center', cellsrenderer:
                   function (row, columnfield, value, defaulthtml, columnproperties) {
                       var bid = $('#jqxgrid').jqxGrid('getrowdata', row).BatchId;
                       return '<div style="text-align:center"><a href="#" onclick="app_popup.batchMessageView(' + bid + ')" >הצג</a></div>';
                   }
              },
               {
                   text: 'מספר פעולה', dataField: 'MessageId', width: 100, cellsalign: 'right', align: 'center'
               },
                {
                    text: '  נמען  ', dataField: 'Target', width: 150, cellsalign: 'right', align: 'center'
                },
              {
                  text: 'פרסונאל', dataField: 'Personal', width: 150, cellsalign: 'right', align: 'center'
              },
              {
                  text: 'יחי-חיוב', dataField: 'Units', width: 80, cellsalign: 'right', align: 'center'
              },
             {
                 text: 'מחיר', dataField: 'Price', width: 80, cellsalign: 'right', align: 'center'
             },   
             {
                 text: 'מס-שליחה', dataField: 'BatchId', width: 100, cellsalign: 'right', align: 'center', filtertype: 'equal'
             },
             {
                 text: 'מס-סידורי', dataField: 'ItemIndex', width: 100, cellsalign: 'right', align: 'center', filtertype: 'equal'
             },
              {
                  text: 'סטאטוס', dataField: 'StatusName', width: 80, cellsalign: 'right', align: 'center'
              },
              { text: 'מועד שליחה', type: 'date', dataField: 'SentTime', filtertype: 'range', width: 150, cellsformat: 'd', cellsalign: 'right', align: 'center' }
            ]
        });
    },
   
    load: function (Model, userInfo) {

            //this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
            this.source.data = {
                'AccountId': Model.AuthAccount,
                'Platform': Model.Platform,
                'DateFrom': Model.DateFrom,
                'DateTo': Model.DateTo,
                'BatchId': Model.BatchId,
                'Target': Model.Target
            };

        this.dataAdapter = new $.jqx.dataAdapter(this.source, {
            loadComplete: function (data) {
                //source.totalrecords = getTotalRows(data);
            },
            loadError: function (xhr, status, error) {app_dialog.alert(' status: ' + status + '\n error ' + error) }
        });

        this.grid();

        return this;
    }
};
