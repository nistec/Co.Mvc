
//============================================================================================ app_signup_grid

app_signup_grid = {

    //accType: 0,
   
    dataAdapter: {},
    AllowEdit:0,
    source:
        {
            datatype: "json",
            async: false,
            datafields: [
               { name: 'SignupId', type: 'number' },
                { name: 'SignupDate', type: 'date' },
                { name: 'ReferralCode', type: 'string' },
                { name: 'AutoCharge', type: 'bool' },
                { name: 'RegHostAddress', type: 'string' },
                { name: 'RegReferrer', type: 'string' },
                { name: 'CreditCardOwner', type: 'string' },
                { name: 'ConfirmAgreement', type: 'string' },
                { name: 'SignKey', type: 'string' },
                { name: 'Price', type: 'number' },
                { name: 'ItemId', type: 'number' },
                { name: 'ValidityMonth', type: 'number' },
                { name: 'MemberRecord', type: 'number' },
                { name: 'Campaign', type: 'number' },
                { name: 'SignupOrder', type: 'number' },
                { name: 'ExpirationDate', type: 'date' },
                { name: 'MemberId', type: 'string' },
                { name: 'MemberName', type: 'string' },
                { name: 'CellPhone', type: 'string' },
                { name: 'CampaignName', type: 'string' },
                { name: 'ItemName', type: 'string' },
                { name: 'CampaignName', type: 'string' },
                { name: 'PayId', type: 'number' },
                { name: 'Payed', type: 'number' },
                { name: 'ConfirmationCode', type: 'string' },
                { name: 'Token', type: 'string' },
                { name: 'PayedDate', type: 'date' }
                ],

            id: 'SignupId',
            type: 'POST',
            url: '/Main/GetSignupGrid',
            //data:{},
            pagenum: 0,
            pagesize: 20,
            root: 'Rows',
            pager: function (pagenum, pagesize, oldpagenum) {
                // callback called when a page or page size is changed.
            },
            filter: function () {
                // update the grid and send a request to the server.
                $("#jqxgrid").jqxGrid('updatebounddata');
            },
            sort: function () {
                // update the grid and send a request to the server.
                $("#jqxgrid").jqxGrid('updatebounddata');
            },
            beforeprocessing: function (data) {
                this.totalrecords = data.TotalRows;
            }
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

            //slf.NScustomers.currentIndex = index;

            var tabsdiv = null;
            var information = null;
            var notes = null;

            tabsdiv = $($(parentElement).children()[0]);
            if (tabsdiv != null) {
                information = tabsdiv.find('.information');
                var title = tabsdiv.find('.title');
                title.text(datarecord.MemberName);
                var tabpayment = tabsdiv.find('.payment');
                var tabaction = tabsdiv.find('.action');

                var container = $('<div style="margin: 5px;text-align:right;"></div>')
                container.rtl = true;
                container.appendTo($(information));

                var leftcolumn = $('<div style="float: left; width: 45%;direction:rtl;"></div>');
                var rightcolumn = $('<div style="float: right; width: 40%;direction:rtl;"></div>');
                container.append(leftcolumn);
                container.append(rightcolumn);
 
                var divLeft = $("<div style='margin: 10px;'><b>קוד רישום:</b> " + datarecord.SignupId + "</div>" +
                    "<div style='margin: 10px;'><b>קוד הפנייה:</b> " + datarecord.ReferralCode + "</div>" +
                    "<div style='margin: 10px;'><b>חיוב אוטומטי:</b> " + datarecord.AutoCharge + "</div>" +
                    "<div style='margin: 10px;'><b>אישור הצהרה:</b> " + datarecord.ConfirmAgreement + "</div>" +
                    "<div style='margin: 10px;'><b>מועד סיום:</b> " + datarecord.ExpirationDate.toLocaleDateString() + "</div>");

                divLeft.rtl = true;
                var divRight = $("<div style='margin: 10px;'><b>כתובת אלקטרונית:</b> " + datarecord.RegHostAddress + "</div>" +
                    "<div style='margin: 10px;'><b>מקור רישום:</b> " + datarecord.RegReferrer + "</div>" +
                    "<div style='margin: 10px;'><b>תוקף:</b> " + datarecord.ValidityMonth + "</div>" +
                    "<div style='margin: 10px;'><b>בעל כרטיס אשראי:</b> " + datarecord.CreditCardOwner + "</div>"
                    //"<div style='margin: 10px;'><b>ת.ז:</b> " + datarecord.MemberId + "</div>"
                    );
               
                divRight.rtl = true;
                $(leftcolumn).append(divLeft);
                $(rightcolumn).append(divRight);


                var paymentcontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;direction:rtl"></div>');
                paymentcontainer.rtl = true;
                
                if (datarecord.PayId === undefined || datarecord.PayId==0) {
                    paymentcontainer.append($("<div style='margin: 10px;text-align:center'><b>לא נמצאו נתונים</b></div>"));
                }
                else {
                    paymentcontainer.append(
                       $("<div style='margin: 10px;'><b>מספר קבלה:</b> " + datarecord.PayId + "</div>" +
                        "<div style='margin: 10px;'><b>סכום ששולם:</b> " + datarecord.Payed + "</div>" +
                        "<div style='margin: 10px;'><b>קוד אישור:</b> " + datarecord.ConfirmationCode + "</div>")
                       );
                    //alert(datarecord.PayedDate);
                    if(datarecord.PayedDate)
                        paymentcontainer.append($("<div style='margin: 10px;'><b>מועד תשלום:</b> " + datarecord.PayedDate.toLocaleDateString() + "</div>"));
                }
                //"<div style='margin: 10px;'><b>בעל כרטיס אשראי:</b> " + datarecord.Token + "</div>" +
                $(tabpayment).append(paymentcontainer);
                
                var memid = datarecord.MemberId;//parseInt(datarecord.MemberId);
                var rcdid = datarecord.MemberRecord;//parseInt(datarecord.MemberId);

                var actioncontainer = $('<div style="white-space: normal; margin: 5px;text-align:right;"></div>');
                actioncontainer.rtl = true;
                $(tabaction).append(actioncontainer);
                var edititem = $('<div class="rtl" style="margin:10px"><a title="הצג" href="#" style="float:right;">הצגת מנוי</a></div>')
                 .click(function () {
                     app_popup.memberEdit(rcdid);
                 });
                actioncontainer.append(edititem);
 
                $(tabsdiv).jqxTabs({ width: '95%', height: 170, rtl: true });
            }
        };

        var renderstatusbar = function (statusbar) {
            // appends buttons to the status bar.
            var container = $("<div style='overflow: hidden; position: relative; margin: 5px;float:right;'></div>");
            var editButton = $("<div style='float: left; margin-left: 5px;' title='הצג את הרשומה המסומנת' ><img src='../scripts/app/images/edit.gif'><span style='margin-left: 4px; position: relative;'>הצג</span></div>");
            var reloadButton = $("<div style='float: left; margin-left: 5px;' title='רענון'><img src='../scripts/app/images/refresh.gif'><span style='margin-left: 4px; position: relative;'>רענון</span></div>");
            var clearFilterButton = $("<div style='float: left; margin-left: 5px;' title='הסר סינון' ><img src='../scripts/app/images/filterRemove.gif'><span style='margin-left: 4px; position: relative;'>הסר סינון</span></div>");
            var queryButton = $("<div style='float: left; margin-left: 5px;' title='איתור' ><img src='../scripts/app/images/search.gif'><span style='margin-left: 4px; position: relative;'>איתור</span></div>");
            container.append(reloadButton);
            container.append(clearFilterButton);
            container.append(queryButton);
            container.append(editButton);
            statusbar.append(container);

            editButton.jqxButton({ width: 70, height: 20 });
            reloadButton.jqxButton({ width: 70, height: 20 });
            clearFilterButton.jqxButton({ width: 70, height: 20 });
            queryButton.jqxButton({ width: 70, height: 20});

            editButton.click(function (event) {
                //var datarow = generatedata(1);
                //$("#jqxgrid").jqxGrid('addrow', null, datarow[0]);
                var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
                if (selectedrowindex < 0)
                    return;
                var id = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex).MemberRecord;
                app_popup.memberEdit(id);
            });

            // reload grid data.
            reloadButton.click(function (event) {
                $("#jqxgrid").jqxGrid('source').dataBind();
            });
            clearFilterButton.click(function (event) {
                $("#jqxgrid").jqxGrid('clearfilters');
            });
            queryButton.click(function (event) {
                app.redirectTo('/Main/SignupQuery');
            });
        };

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
        {
            width: '100%',
            autoheight: true,
            rtl: true,
            source: slf.dataAdapter,
            localization: getLocalization('he'),
            virtualmode: true,
            rendergridrows: function (obj) {
                return slf.dataAdapter.records;
            },
            pageable: true,
            pagermode: 'simple',
            sortable: true,
            //showfilterrow: true,
            //filterable: true,
            rowdetails: true,
            rowdetailstemplate: { rowdetails: "<div style='margin: 10px;'><ul style='margin-right: 30px;'><li class='title'></li><li>פרטי תשלום</li><li>אפשרויות</li></ul><div class='information'></div><div class='payment'></div><div class='action'></div></div>", rowdetailsheight: 200 },
            //ready: function () {
            //    $("#jqxgrid").jqxGrid('showrowdetails', 0);
            //},
            initrowdetails: initrowdetails,
            //autoshowfiltericon: true,
            //columnmenuopening: function (menu, datafield, height) {

            //    var column = $("#jqxgrid").jqxGrid('getcolumn', datafield);
            //    if (column.filtertype === "custom") {
            //        menu.height(155);
            //        setTimeout(function () {
            //            menu.find('input').focus();
            //        }, 25);
            //    }
            //    else menu.height(height);
            //},
            showstatusbar: true,
            renderstatusbar:renderstatusbar,
            columns: [
              //{
              //    text: 'מס.סידורי', dataField: 'RecordId', filterable: false, width: 100, cellsalign: 'right', align: 'center'
              //    //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
              //     //    var link = '<div style="text-align:center"><a href="#" onclick="memberEdit(' + value + ')" >הצג</a>';
              //     //    if (slf.AllowEdit == 1) {
              //     //        var mid = $('#jqxgrid').jqxGrid('getrowdata', row).MemberId;
              //     //        return link + ' | <a href="#" onclick="memberDelete(' + mid + ')" >הסר</a>';
              //     //    }
              //     //    else
              //     //        return link + '</div>';
              //     //    //return '<div style="text-align:center"><a href="#" onclick="memberEdit(' + value + ')" >הצג</a></div>';
              //     //}
              //},
               {
                   text: 'קוד הפנייה', dataField: 'ReferralCode', cellsalign: 'right', align: 'center', hidden: true
               },
                {
                    text: 'מספר קבלה', dataField: 'PayId', cellsalign: 'right', align: 'center', hidden: true
                },
              {
                  text: 'ת.ז', dataField: 'MemberId', cellsalign: 'right', align: 'center'
              },
              {
                  text: 'שם מלא', dataField: 'MemberName', cellsalign: 'right', align: 'center'
              },
              {
                  text: 'קמפיין', dataField: 'CampaignName', cellsalign: 'right', align: 'center'
              },
              {
                  text: 'פריט', dataField: 'ItemName', cellsalign: 'right', align: 'center'
              },
            {
                text: 'מחיר', dataField: 'Price', cellsalign: 'right', align: 'center'
            },
              {
                  text: 'טלפון נייד', dataField: 'CellPhone', width: 120, cellsalign: 'right', align: 'center'
              },
              { text: 'סיום תוקף', type: 'date', dataField: 'ExpirationDate', filtercondition: 'starts_with', width: 120, cellsformat: 'd',cellsalign: 'right', align: 'center' 
             },
             { text: 'מועד הצטרפות', type: 'date', dataField: 'SignupDate', filterable: false, width: 120, cellsformat: 'd', cellsalign: 'right', align: 'center' }
            ]
        });
        //$('#jqxgrid').on('rowdoubleclick', function (event) {
        //    var args = event.args;
        //    var boundIndex = args.rowindex;
        //    var visibleIndex = args.visibleindex;
        //    //var id = $("#jqxgrid").jqxGrid('getrowid', boundIndex).MemberId;
        //    var mid = $('#jqxgrid').jqxGrid('getrowdata', boundIndex).MemberId;
        //    app_popup.memberEdit(mid);
        //});

    },


    load: function (Model, userInfo) {

        this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
        this.source.data = {
            'QueryType': Model.QueryType,
            'MemberId': Model.MemberId,
            'Name': Model.Name,
            'Items': Model.Items,
            'ValidityRemain': Model.ValidityRemain,
            'Campaign': Model.Campaign,
            'SignupDateFrom':formatJsonShortDate( Model.SignupDateFrom),
            'SignupDateTo': formatJsonShortDate(Model.SignupDateTo),
            'PriceFrom': Model.PriceFrom,
            'PriceTo': Model.PriceTo,
            'ContactRule': Model.ContactRule
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
