'use strict';

class dashboard_mv {

    constructor($element) {
        this.$element = $element;
    }

    load() {

        var html =
            `
<div class="global-view">

    <!--content-->
    <div class="container rtl">
        <div style="margin-left: auto; margin-right: auto; ">
            <h2 id="cms_1-text-main-1">מעקב מנויים</h2>
            <div id="chartMembers" class="graph" style="width:100%; height:500px;">

            </div>
            <div id="dataCounters">
                <div id="dataTable" style="z-index:1 !important"></div>
            </div>
           

        </div>
    </div>
    <div style="text-align:center">
        <ul>
            <li id="mngr"></li>
        </ul>
    </div>
  </div>
`
        $(this.$element).html(html);

        var style =
 `
<style id="spa_dynamic_style" type="text/css">
         p {
        font-family:Arial;
        }
        .title {
            font-family: Arial;
        }
         span {
            font-family: Arial;
        }
        .chart-inner-text {
            fill: #00BAFF;
            color: #00BAFF;
            font-size: 30px;
            font-family: Verdana;
        }    
    </style>
}
`
        $(style).appendTo("head");

    }

    init() {

        this.load();

        // time line
        var source =
            {
                datatype: "csv",
                type: 'POST',
                async: true,
                //contentType: "application/CSV; charset=utf-8",
                datafields: [
                    { name: 'ReportDate' },
                    { name: 'TotalMembers' },
                    { name: 'TotalSignup' },
                    { name: 'TotalPayment' }
                ],
                url: '/Co/DashboardMembers'
            };
        var dataAdapter = new $.jqx.dataAdapter(source, { async: false, autoBind: true, loadError: function (xhr, status, error) { alert('Error loading "' + source.url + '" : ' + error); } });
        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        // prepare jqxChart settings
        var settings = {
            title: "מעקב מנויים לפי סוג פעולה",
            description: "",
            enableAnimations: true,
            showLegend: true,
            padding: { left: 10, top: 5, right: 10, bottom: 5 },
            titlePadding: { left: 50, top: 0, right: 0, bottom: 10 },
            source: dataAdapter,
            xAxis:
                {
                    dataField: 'ReportDate',
                    //formatFunction: function (value) {
                    //    return value.getDate() + '-' + months[value.getMonth()] + '-' + value.getFullYear();
                    //},
                    type: 'date',
                    baseUnit: 'day',
                    valuesOnTicks: true,
                    //minValue: '01-01-2014',
                    //maxValue: '01-01-2015',
                    tickMarks: {
                        visible: true,
                        step: 1,
                        color: '#BCBCBC'
                    },
                    unitInterval: 1,
                    gridLines: {
                        visible: true,
                        step: 1,
                        color: '#BCBCBC'
                    },
                    labels: {
                        angle: -45,
                        rotationPoint: 'topright',
                        offset: { x: 0, y: -25 }
                    }
                },
            valueAxis:
                {
                    visible: true,
                    title: { text: 'מספר מנויים \\ פעולות<br>' },
                    tickMarks: { color: '#BCBCBC' }
                },
            colorScheme: 'scheme04',
            seriesGroups:
                [
                    {
                        type: 'line',
                        series: [
                            { dataField: 'TotalMembers', displayText: 'מנויים' },
                            { dataField: 'TotalSignup', displayText: 'נרשמו' },
                            { dataField: 'TotalPayment', displayText: 'תשלומים' }
                        ]
                    }
                ]
        };
        // setup the chart
        $('#chartMembers').jqxChart(settings);
        //$('#chartMembers').jqxChart({ localization: getLocalization('he') });


        //counters
        var source =
            {
                dataType: "csv",
                type: 'POST',
                async: true,
                dataFields: [
                    { name: 'LastDate', type: 'date' },
                    { name: 'TotalMembers', type: 'number' },
                    { name: 'TotalSignup', type: 'number' },
                    { name: 'TotalPayment', type: 'number' },
                    { name: 'TotalPayed', type: 'decimal' },
                    { name: 'TotalFailure', type: 'number' }
                ],
                //id: 'id',
                url: '/Co/DashboardCounters'
            };
        var dataAdapter = new $.jqx.dataAdapter(source);
        $("#dataTable").jqxDataTable(
            {
                width: '100%',
                rtl: true,
                pageable: false,
                source: dataAdapter,
                columnsResize: true,
                columns: [
                    { text: 'תאריך עדכון אחרון', dataField: 'LastDate', type: 'date', cellsformat: 'dd/MM/yyyy hh:mm:ss', cellsalign: 'right', align: 'center' },
                    { text: 'סה"כ מנויים', dataField: 'TotalMembers', cellsalign: 'right', align: 'center' },
                    { text: 'סה"כ נרשמו', dataField: 'TotalSignup', cellsalign: 'right', align: 'center' },
                    { text: 'סה"כ שילמו', dataField: 'TotalPayment', cellsalign: 'right', align: 'center' },
                    { text: 'סה"כ שולם בש"ח', dataField: 'TotalPayed', type: 'decimal', cellsformat: 'f2', cellsalign: 'right', align: 'center' },
                    { text: 'סה"כ נכשלו', dataField: 'TotalFailure', cellsalign: 'right', align: 'center' }
                ]
            });

        // auto update timer
        var counter = 0;
        var autorefresh = function () {
            var ttimer = setInterval(function () {
                try {
                    if (counter > 1200) {
                        clearInterval(ttimer);
                    }
                    counter++;
                    $('#chartMembers').jqxChart('source').dataBind();
                    $('#dataTable').jqxDataTable('source').dataBind();
                }
                catch (e) {

                }
            }, 180000);
        }
                //autorefresh();
    }

    display() {
        $(this.$element).show();
    }
}