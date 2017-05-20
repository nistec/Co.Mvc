
//============================================================================================ app_member_info

function app_member_info(accid) {

    $("#AccountId").val(accid);

    app_members.displayMemberFields('/Registry/GetMemberFieldsView', { 'accid': accid })

};

app_member_info.prototype.syncData = function (record,data) {

    if (record) {

        var rcd = JSON.parse(record);
        app_jqxform.loadDataForm("fcForm", rcd);
    }

    if (data) {
  
            var nastedsource = {
                datafields: [
                      { name: 'SignupId', type: 'number' },
                      { name: 'SignupDate', type: 'date' },
                      { name: 'ReferralCode', type: 'string' },
                      { name: 'AutoCharge', type: 'bool' },
                      { name: 'RegHostAddress', type: 'string' },
                      { name: 'RegReferrer', type: 'string' },
                      { name: 'CreditCardOwner', type: 'string' },
                      { name: 'ConfirmAgreement', type: 'boll' },
                      { name: 'SignKey', type: 'string' },
                      { name: 'ValidityMonth', type: 'number' },
                      { name: 'Campaign', type: 'number' },
                      { name: 'Price', type: 'number' },
                      { name: 'ItemId', type: 'number' },
                      { name: 'ItemName', type: 'string' },
                      { name: 'CampaignName', type: 'string' },
                      { name: 'SignupOrder', type: 'number' },
                      { name: 'PayId', type: 'number' },
                      { name: 'Payed', type: 'number' },
                      { name: 'PayedDate', type: 'date' },
                      { name: 'PaymentOwner', type: 'bool' },
                      { name: 'ExpirationDate', type: 'date' }
                ],
                datatype: "json",
                localdata: data
            }

            var dataAdapter = new $.jqx.dataAdapter(nastedsource, {
                loadComplete: function () {
                    // data is loaded.
                }
            });

            $("#dataTable").jqxDataTable(
            {
                width: '99%',
                rtl: true,
                editable: false,
                pageable: false,
                //pageSize: 10,
                //pagerButtonsCount: 5,
                localization: getLocalization('he'),
                theme: 'energyblue',
                sortable: true,
                height: 600,
                source: dataAdapter,
                columns: [
                       {
                           text: 'פרטים', align: 'center', dataField: 'SignupId', filterable: false,
                           // row - row's index.
                           // column - column's data field.
                           // value - cell's value.
                           // rowData - rendered row's object.
                           cellsRenderer: function (row, column, value, rowData) {


                               var container = '<div style="width: 100%; height: 100%;">'
                               var leftcolumn = '<div style="float: left; width: 50%;direction:rtl;text-align:right;">';
                               var rightcolumn = '<div style="float: left; width: 50%;direction:rtl;text-align:right;">';
                               //var allColumn = '<div style="width: 100%; height: 100%;">'



                               var payId = "<div style='margin: 10px;'><b>מס-קבלה:</b> " + rowData.PayId + "</div>";
                               var payedDate = "<div style='margin: 10px;'><b>מועד תשלום:</b> " + app.toLocalDateString(rowData.PayedDate) + "</div>";
                               var paymentOwner = "<div style='margin: 10px;'><b>משלם:</b> " + app.toYesNo(rowData.PaymentOwner) + "</div>";
                               var payed = (rowData.Payed > 0) ? "<div style='margin: 10px;'><b>סכום ששולם:</b> " + rowData.Payed + "</div>" : "";
                               leftcolumn += payId + payedDate + paymentOwner + payed;
                               leftcolumn += "</div>";

                               var signupDate = "<div style='margin: 10px;'><b>מועד רישום:</b> " + app.toLocalDateString(rowData.SignupDate) + "</div>";
                               var referralCode = "<div style='margin: 10px;'><b>קוד הפנייה:</b> " + rowData.ReferralCode + "</div>";
                               var itemName = "<div style='margin: 10px;'><b>פריט:</b> " + rowData.ItemName + "</div>";
                               var expirationDate = "<div style='margin: 10px;'><b>מועד תפוגה:</b> " + app.toLocalDateString(rowData.ExpirationDate) + "</div>";
                               rightcolumn += signupDate + referralCode + itemName + expirationDate;
                               rightcolumn += "</div>";

                               //allColumn="<div style='margin: 10px;'><b>Postal Code:</b> " + rowData.postalcode + "</div>";

                               container += leftcolumn;
                               container += rightcolumn;

                               //container += expColumn;
                               container += "</div>";

                               return container;
                           }
                       }
                ]
            });
     };
};

