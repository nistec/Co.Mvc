
//============================================================================================ app_dashboard

app_dashboard = {

    getSettings: function (title, description, xtitle, series, dataAdapter) {

        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        return {
            title: title,
            description: description,
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
                //baseUnit: 'month',
                valuesOnTicks: true,
                //minValue: '01-01-2014',
                //maxValue: '01-01-2015',
                tickMarks: {
                    visible: true,
                    interval: 1,
                    color: '#BCBCBC'
                },
                unitInterval: 1,
                gridLines: {
                    visible: true,
                    interval: 3,
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
                title: { text: xtitle + '<br>' },
                tickMarks: { color: '#BCBCBC' }
            },
            colorScheme: 'scheme04',
            seriesGroups:
                [
                    {
                        type: 'line',
                        series: series
                    }
                ]
        };
    },
 
    getAdapter: function (url) {
        var source=
            {
                datatype: "csv",
                type: 'POST',
                //async:true,
                datafields: [
                    { name: 'ReportDate' },
                    { name: 'TotalCount' }
                ],
                url: url
            };
        var dataAdapter = new $.jqx.dataAdapter(source, { async: false, autoBind: true, loadError: function (xhr, status, error) {app_dialog.alert('Error loading "' + source.url + '" : ' + error); } });

        return dataAdapter;
    },

    load: function () {
 
        var adapter = this.getAdapter('/Reports/DashboardMembers');
        var settings = this.getSettings("מעקב קליטת מנויים", "", "מספר מנויים שנקלטו", [{ dataField: 'Memebers', displayText: 'מנויים' }], adapter)

        $('#chartMembers').jqxChart(settings);


        return this;
    }
};
