//mc-view version 20121026.1

var htmlDecode = function (val) {
    if (!val) return "";
    return val.replace(/&gt;/g, ">").replace(/&lt;/g, "<");
};

var htmlEncode=function(val) {
    if (!val) return "";
    return val.replace(/</g, "&lt;").replace(/>/g, "&gt;");
}

function loadCms(pid, url) {

    // prepare the data
    var view_source =
    {
        datatype: "json",
        datafields: [
                { name: 'SectionId', type: 'string' },
                { name: 'PageId', type: 'number' },
                { name: 'SectionContent', type: 'string' }
        ],
        id: 'PageId',
        data: { 'PageId': pid },
        type: 'POST',
        url: url// '@Url.Action("RenderCmsView", '+controller+')'
    };
    var viewAdapter = new $.jqx.dataAdapter(view_source, {
        loadComplete: function (records) {
            var length = records.length;
            for (var i = 0; i < length; i++) {
                var record = records[i];
                if (record) {

                    var secid = record.SectionId;
                    var content = record.SectionContent;
                    var attribs = record.SectionAttr;
                    var sectype = record.SectionType;

                    if (sectype == 'json') {
                        if (content) {
                            $.each(JSON.parse(content), function (idx, obj) {
                                $('#cms_' + obj.id).html(htmlDecode(obj.val));
                            });
                        }
                    }
                    else if (sectype == 'link') {
                        $('#cms_' + secid).html(content);
                        $('#cms_' + secid).attr('href', attribs);
                        //if (attribs) {
                        //    $.each(JSON.parse(attribs), function (idx, obj) {
                        //        $('#cms_' + secid).attr(obj.id, obj.val);
                        //    });
                        //}
                    }
                    else if (sectype == 'image') {
                        $('#cms_' + secid).attr('src', content);
                        if (attribs) {
                            $.each(JSON.parse(attribs), function (idx, obj) {
                                $('#cms_' + secid).attr(obj.id, obj.val);
                            });
                        }
                    }
                    else if (sectype == 'text') {
                        $('#cms_' + secid).html(content);
                        if (attribs) {
                            $.each(JSON.parse(attribs), function (idx, obj) {
                                $('#cms_' + secid).attr(obj.id, obj.val);
                            });
                        }
                    }
                    else if (sectype == 'input') {
                        $('#cms_' + secid).val(content);
                        if (attribs) {
                            $.each(JSON.parse(attribs), function (idx, obj) {
                                $('#cms_' + secid).attr(obj.id, obj.val);
                            });
                        }
                    }
                    else {
                        $('#cms_' + sid).html(htmlDecode(content));
                    }
                }
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });
    viewAdapter.dataBind();
}

//=========================================  editCmsMultiSite

function editCmsMultiSite(pages_url, sites_url) {

    if (sites_url) {
        var siteSource =
           {
               dataType: "json",
               dataFields: [
                   { name: 'SiteId' },
                   { name: 'SiteName' }
               ],
               data: {},
               type: 'POST',
               url: sites_url//'@Url.Action("GetSiteView", "Admin")'
           };
        var siteAdapter = new $.jqx.dataAdapter(siteSource, {
            contentType: "application/json; charset=utf-8",
            loadError: function (jqXHR, status, error) {
            },
            loadComplete: function (data) {
            }
        });

        $("#listSite").jqxDropDownList(
        {
            placeHolder: "Select Site:",
            rtl: true,
            source: siteAdapter,
            width: 200,
            height: 24,
            displayMember: 'SiteName',
            valueMember: 'SiteId'
        });

        $("#listSite").bind('select', function (event) {
            if (event.args) {
                //selectedSite = event.args.item.value;

                var value = event.args.item.value;
                //source.data = { 'SiteId': value };
                //dataAdapter = new $.jqx.dataAdapter(source);
                //$("#dataTable").jqxDataTable({ source: dataAdapter });

                editCmsSingleSite(pages_url, value);
            }
        });
    }
}

//=========================================  editCmsSingleSite

function editCmsSingleSite(pages_url, site_id) {

    var selectedSite = site_id;

    var source =
    {
        dataType: "json",
        datafields:
        [
            { name: 'SiteId', type: 'number' },
            { name: 'PageId', type: 'number' },
            { name: 'PageName', type: 'string' },
            { name: 'PageTitle', type: 'string' },
            { name: 'PageCategory', type: 'number' },
            { name: 'PageState', type: 'number' }
        ],
        id: 'PageId',
        data: { 'SiteId': selectedSite },
        type: 'POST',
        url: pages_url//'@Url.Action("GetCmsPages", "Admin")',
    };

    var dataAdapter = new $.jqx.dataAdapter(source);
    var selectedKey = 0;
    $("#dataTable").jqxDataTable(
    {
        width: 400,
        source: dataAdapter,
        rtl: true,
        sortable: true,
        pageable: true,
        pageSize: 20,
        pagerButtonsCount: 5,
        enableHover: false,
        selectionMode: 'none',

        rendered: function () {
            $(".button").jqxLinkButton({ width: '100', height: '25' });
        },
        columns: [
              {
                  text: 'Cms Pages', align: 'left', dataField: 'PageId',
                  // row - row's index.
                  // column - column's data field.
                  // value - cell's value.
                  // rowData - rendered row's object.
                  cellsRenderer: function (row, column, value, rowData) {
                      var container = "<div>";
                      var item = "<div class='section_item'>";
                      var info = "<div class='section_info'>";
                      info += "<div><i>" + rowData.PageId + "</i> - Page: " + rowData.PageName + "</div>";
                      info += "</div>";
                      var button = "<a class='button' href='CmsPage?pid=" + rowData.PageId + "' >Edit</a>"
                      item += info;
                      item += button;
                      item += "</div>";
                      container += item;
                      container += "</div>";
                      return container;
                  }

              }
        ]
    });

    $("#dataTable").on('rowSelect', function (event) {
        // event arguments
        var args = event.args;
        // row index
        var index = args.index;
        // row data
        var rowData = args.row;
        // row key
        selectedKey = args.key;
    });
}

//=========================================  editCmsSection


function editCmsSection(pid, sections_url, update_url) {

    if (pid <= 0) {
        return;
    }

    // create window.
    $('#window').jqxWindow({
        autoOpen: false, width: 810, position: 'top, center', height: 480, maxWidth: 960, resizable: false,
        initContent: function () {
            // create editor.
            $('#editor').jqxEditor({
                height: '400px',
                width: '800px',
                rtl: true
                //theme: 'arctic'
                //stylesheets: ['editor.css']
            });
        }
    });

    //tools:'bold italic underline | format font size | color background | left center right | outdent indent | ul ol | image | link | clean | html',

    $("#btnUpdtate").on('click', function () {
        var secid = $('#hdnSectionId').val();
        var contant = htmlEncode($('#editor').val());
        sendCommand({ 'PageId': pid, 'SectionId': secid, 'SectionContent': contant });
        $('#window').jqxWindow('close');
    });

    $("#btnCancel").on('click', function () {
        $('#window').jqxWindow('close');
    });


    var sendCommand = function (rowdata) {

        var url = update_url//'@Url.Action("CmsSectionUpdate", "Admin")'

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: url,
            data: rowdata,
            success: function (data, status, xhr) {
                dataAdapter.dataBind();
            },
            error: function () {
               app_dialog.alert('אירעה שגיאה, לא עודכנו נתונים');
            }
        });
    };

    var source =
    {
        dataType: "json",
        datafields:
        [
            { name: 'SectionId', type: 'string' },
            { name: 'PageId', type: 'number' },
            { name: 'SectionContent', type: 'string' },
            { name: 'SectionType', type: 'string' },
            { name: 'SectionAttr', type: 'string' }
        ],
        id: 'SectionId',
        data: { PageId: pid },
        type: 'POST',
        url: sections_url//'@Url.Action("GetCmsPage", "Admin")',
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    var selectedKey;
    var selectedContent;
    var selectedType;
    var selectedAttr;
    $("#dataTable").jqxDataTable(
    {
        width: '80%',
        source: dataAdapter,
        rtl: true,
        sortable: true,
        pageable: true,
        pageSize: 20,
        pagerButtonsCount: 5,
        enableHover: false,
        selectionMode: 'none',

        rendered: function () {
            $(".section_button").jqxButton();
            $(".section_button").click(function () {

                var i = 1;
                var maxAttr = 3;
                var winheight = 480;
                if (selectedType == 'attr') {
                    winheight = 200;
                    $('#titleAttr').val(selectedContent);
                    $('#lblAttr' + i).html(obj.id);
                    $('#valAttr' + i).val(obj.val);

                    for (i = 1; i <= maxAttr; i++) {
                        $('#liAttr' + i).hide();
                    }
                    i = 1;
                    if (selectedAttr) {
                        $.each(JSON.parse(selectedAttr), function (idx, obj) {
                            if (i <= maxAttr) {
                                $('#lblAttr' + i).html(obj.id);
                                $('#valAttr' + i).val(obj.val);
                                $('#liAttr' + i).show();
                            }
                        });
                    }
                    $('#editorHtml').hide();
                    $('#editorText').hide();
                    $('#editorAttr').show();
                    $('#editorLink').hide();
                }
                else if (selectedType == 'link') {
                    winheight = 240;
                    $('#valLinkTitle').val(selectedContent);
                    $('#valLink').val(selectedAttr);
                    $('#editorHtml').hide();
                    $('#editorText').hide();
                    $('#editorAttr').hide();
                    $('#editorLink').show();
                }
                else if (selectedType == 'text' || selectedType == 'image' || selectedType == 'input') {
                    winheight = 200;
                    $('#valText').val(selectedContent);
                    $('#lblText').html(selectedType);
                    $('#editorHtml').hide();
                    $('#editorText').show();
                    $('#editorAttr').hide();
                    $('#editorLink').hide();
                }
                else {
                    winheight = 480;
                    $('#editorLink').hide();
                    $('#editorAttr').hide();
                    $('#editorText').hide();
                    $('#editorHtml').show();
                    $('#editor').val(htmlDecode(selectedContent));
                }
                $('#hdnSectionId').val(selectedKey);
                $('#hdnSectionType').val(selectedType);
                $('#window').jqxWindow('open');
                $('#window').jqxWindow('height', winheight);

            });
        },
        columns: [
              {
                  text: 'sections', align: 'left', dataField: 'SectionId',
                  // row - row's index.
                  // column - column's data field.
                  // value - cell's value.
                  // rowData - rendered row's object.
                  cellsRenderer: function (row, column, value, rowData) {
                      var container = "<div>";
                      var section = rowData;//[row];// sections[i];
                      var item = "<div class='section_item'>";
                      var info = "<div class='section_info'>";
                      var sectype = section.SectionType;
                      if (sectype === null)
                          sectype = 'html';

                      info += "<div><i>" + section.SectionId + "</i> | <i>" + sectype + "</i>";// - SectionName: " + section.SectionName + "</div>";

                      info += "<div class='section_content'>" + htmlDecode(section.SectionContent) + "</div>";

                      info += "</div>";
                      var button = "<button class='section_button'>Edit</button>";
                      item += info;
                      item += button;
                      item += "</div>";
                      container += item;
                      container += "</div>";
                      return container;
                  }

              }
        ]
    });

    $("#dataTable").on('rowSelect', function (event) {
        // event arguments
        var args = event.args;

        // row index
        var index = args.index;
        // row data
        var rowData = args.row;
        // row key
        selectedKey = args.key;
        selectedContent = rowData.SectionContent;
        selectedType = rowData.SectionType;
        selectedAttr = rowData.SectionAttr;
    });

}