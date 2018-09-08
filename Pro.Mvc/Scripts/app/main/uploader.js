'use strict';

class app_uploader {

    constructor(uploadType) {
        this.UploadType = uploadType;

        this.UploadProcUrl = '/Media/_UploadProc?uk=';

        if (uploadType == 'contacts') {
            this.FileUploadUrl = '/Media/ContactsFileUpload';
            this.ExecUploadUrl = '/Media/ContactsExecUploadAsync';
            this.LoadUploadedUrl = '/Media/ContactsLoadUploaded';
        }
        else {
            this.FileUploadUrl= '/Media/MembersFileUpload';
            this.ExecUploadUrl = '/Media/MembersExecUploadAsync';
            this.LoadUploadedUrl = '/Media/MembersLoadUploaded';
        }
    }

    ///////////////////////////// uploader

    init() {
        var _self = this;

        $("#loader").hide();
        var wait_message = 'נא להמתין להודעת סיום,לאחר מכן יש ללחוץ על הבא.';
        var hint_message = 'לטעינת מנויים יש ללחוץ על טעינת קובץ,להמתין להודעת סיום,לאחר מכן יש ללחוץ על הבא.';
        var categoryAdapter = null;// app_jqxcombos.createComboAdapter("PropId", "PropName", "listCategory", '/Common/GetValidCategoriesView', 200, 120, false);

        if (this.UploadType == 'contacts') 
            var categoryAdapter = app_jqxcombos.createComboAdapter("PropId", "PropName", "listCategory", '/Common/GetValidCategoriesView', 200, 120, false);
        else
            var categoryAdapter = app_jqxcombos.createComboAdapter("PropId", "PropName", "listCategory", '/Common/GetValidCategoriesView', 200, 120, false);



        $('#fileupload').fileupload({
            maxFileSize: 10000000,
            url: _self.FileUploadUrl,//'/Media/MembersFileUpload',
            //formData: {
            //    param1: $('#updateExists').val()
            //    //param2: $('#uploadUid').val(),
            //    //param3: $('#uploadPt').val()
            //},
            //dataType: 'json',
            done: function (e, data) {
                $("#loader").hide();
                //ObjectKeys(data);


                //alert(data.result.Message.replace('<br/>', '\n'));//textStatus);
                $("#hint-0").html('<strong>' + data.result.Message + '</strong>');
                $("#divUploadResult").html(data.result.Message);
                $("#uploadKey").val(data.result.Args);
                $("#TotalCount").val(data.result.Status);
                //connectUploadedGrid();
                if (data.result.Status > 0) {
                    wizard.next();
                    _self.initMembersGrid(data.result.Args);
                }
                //$.each(data.result.files, function (index, file) {
                //    $('<p/>').text(file.name).appendTo('#files');
                //});
            },
            error: function (jqXHR, status, error) {
                $("#loader").hide();
                alert(error);
            },
            beforeSend: function (e, data) {
                //$("#hint-0").html(wait_message);
                wizard.showHint(wait_message);
            },
            progressall: function (e, data) {
                doPprogress(data);
                $("#loader").show();
            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled')
            .bind('fileuploadsubmit', function (e, data) {
                $("#loader").hide();
                resetPprogress();
                //fileuuid = generateUUID('16');
                //data.formData = {
                //    param1: $('#uploadBid').val(),
                //    param2: $('#uploadUid').val(),
                //    param3: $('#uploadPt').val()
                //};
            });

        //}).prop('disabled', !$.support.fileInput)
        //    .parent().addClass($.support.fileInput ? undefined : 'disabled');

        var doPprogress = function (data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css(
                'width',
                progress + '%'
            );
        };
        var resetPprogress = function () {
            $('#progress .progress-bar').css(
                'width', '0%'
            );
        };

        var doUploadSync = function (slf) {
            
            //var updateExists = $("#updateExists").val();
            var updateExists = $('#updateExists').is(":checked");

            var category = $("#listCategory").val();
            var uploadKey = $("#uploadKey").val();
            var count = $("#TotalCount").val();

            //wizard.upload(uploadKey);

            $.ajax({
                url: slf.ExecUploadUrl,//'/Media/MembersExecUploadAsync',
                type: "POST",
                dataType: 'json',
                //contentType: "application/json; charset=utf-8",
                data: { 'category': category, 'uploadKey': '' + uploadKey + '', 'updateExists': updateExists, 'count': count  },
                success: function (data) {
                    if (data) {
                        if (data.Status < 0)
                            alert(data.Message);
                        else {

                            //redirectTo('UploadProc?uk='+uploadKey);
                            wizard.upload(uploadKey);

                            //$("#final").html(data.Message);

                        };
                    }

                },
                error: function (jqXHR, status, error) {
                    $("#loader").hide();
                    alert(error);
                }
            });
        };
        //==============================================================

        //wizard ====================================
        //Creating wizard module
        var wizard = (function () {
            var _addHandlers = function () {
                $('.next-tab').click(function () {
                    return wizard.next();
                });
                $('.prev-tab').click(function () {
                    return wizard.prev();
                });
            };
            return {
                init: function () {
                    var $tabs = $('#wizard').tabs();
                    $('#wizard').css('height', '400px');
                    $('.prev-tab').hide();
                    $('.end-tab').hide();
                    _addHandlers();
                    wizard.goto(0);
                },
                size: function () {
                    var totalSize = $(".ui-tabs-panel").size() - 1;
                    return totalSize;
                },
                active: function () {
                    var active = $('#wizard').tabs("option", "active");
                    return active;
                },
                displayMessage: function (message) {
                    $('#wizard-validation').val(message);
                    return false;
                },
                upload: function (uploadKey) {
                    wizard.goto(3);
                    var src = _self.UploadProcUrl + uploadKey;// '/Media/_UploadProc?uk=' + uploadKey;
                    app_iframe.appendIframe("proc_iframe", src, "500px", "300px", "yes");
                },
                showHint: function (message, selector) {

                    if (typeof selector === 'undefined') {
                        selector = '.hint';
                    }

                    if (message === undefined || message === '' || message == 'hint_message')
                        message = 'לטעינת מנויים יש ללחוץ על טעינת קובץ,להמתין להודעת סיום,לאחר מכן יש ללחוץ על הבא.';
                    else if (message === 'wait_message')
                        message = 'נא להמתין להודעת סיום,לאחר מכן יש ללחוץ על הבא.';

                    $(selector).html('<strong>' + message + '</strong>');
                },
                goto: function (index) {
                    //0=file upload header
                    //1=set mapping and upload stg
                    //2=show stg
                    //3=finished
                    var totalSize = wizard.size();
                    if (index >= totalSize) {
                        $('.prev-tab').hide();
                        $('.next-tab').hide();
                        $('.end-tab').show();
                    }
                    else if (index == 0) {
                        $('.prev-tab').hide();
                        $('.next-tab').show();
                    }
                    else {
                        $('.prev-tab').show();
                        $('.next-tab').show();
                        $('.end-tab').hide();
                    }
                    $('#wizard').tabs({ active: index });
                    //wizard.displayStep(index, totalSize);

                    var step = index + 1;
                    //if (totalSize === undefined)
                    //    totalSize = $(".ui-tabs-panel").size() - 1;
                    totalSize++;
                    wizard.displayMessage("שלב " + step + " מתוך " + totalSize);

                    return false;
                },
                next: function () {
                    //var totalSize = wizard.size;
                    var active = wizard.active();
                    switch (active) {
                        case 0:
                            var value = $("#TotalCount").val();
                            if (value <= 0) {
                                return wizard.displayMessage("אין מנויים לקליטה");
                            }
                            break;
                    }
                    //$('#wizard-validation').val("");
                    var next = active + 1;
                    return wizard.goto(next);

                    //if (next >= totalSize) {
                    //    $('.next-tab').hide();
                    //    $('.end-tab').show();
                    //}
                    //$('.prev-tab').show();
                    //$('#wizard').tabs({ active: next });
                    //wizard.displayStep(next, totalSize);
                    //return false;
                },
                prev: function () {
                    var active = wizard.active();
                    var prev = active - 1;
                    return wizard.goto(prev);

                    //if (prev <= 0) {
                    //    $('.prev-tab').hide();
                    //}
                    //$('.end-tab').hide();
                    //$('.next-tab').show();
                    //$('#wizard-validation').val("");
                    //$('#wizard').tabs({ active: prev });
                    //wizard.displayStep(prev);
                    ////$tabs.tabs('select', $(this).attr("rel"));
                    //return false;
                },
            }
        }());
        //Initializing the wizard
        wizard.init();


        //end wizard =====================================

        $('#btnUpload').click(function () {
            doUploadSync(_self);
        });

        $('#members-upload-refresh').click(function () {
            var uploadKey=$("#uploadKey").val();
            _self.initMembersGrid(uploadKey);
        });


    }

    initMembersGrid(uploadKey) {

        var _slf = this;

        var dataSource =
            {
                datatype: "json",
                //async: false,
                datafields: [
                    { name: 'MemberId', type: 'string' },
                    { name: 'MemberName', type: 'string' },
                    { name: 'Address', type: 'string' },
                    { name: 'City', type: 'string' },
                    { name: 'JoiningDate', type: 'date' },
                    { name: 'CompanyName', type: 'string' },
                    { name: 'Branch', type: 'string' },
                    { name: 'CellPhone', type: 'string' },
                    { name: 'Phone', type: 'string' },
                    { name: 'Email', type: 'string' },
                    { name: 'LastUpdate', type: 'date' },
                    { name: 'Gender', type: 'string' },
                    { name: 'Note', type: 'string' },
                    { name: 'Birthday', type: 'string' },
                    { name: 'RecordId', type: 'number' },
                    { name: 'TotalRows', type: 'number' }
                ],
                id: 'RecordId',
                type: 'POST',
                url: _slf.LoadUploadedUrl,//'/Media/LoadUploadedMembers',
                data: { 'uploadkey': uploadKey},
                //pagenum: 0,
                pagesize: 10
               
            }


        var dataAdapter = new $.jqx.dataAdapter(dataSource, {
            loadComplete: function (data) {
                //source.totalrecords = getTotalRows(data);
            },
            loadError: function (xhr, status, error) {
                app_dialog.alert(' status: ' + status + '\n error ' + error)
            }
        });

        // create Tree Grid
        $("#jqxgrid").jqxGrid(
            {
                width: '100%',
                autoheight: true,
                rtl: true,
                source: dataAdapter,
                localization: getLocalization('he'),
                rendergridrows: function (obj) {
                    return _slf.DataAdapter.records;
                },
                columnsresize: true,
                pageable: true,
                pagermode: 'simple',
                sortable: true,
                rowdetails: false,
                columns: [
                    {
                        text: 'מס.סידורי', dataField: 'RecordId', filterable: false, width: 100, cellsalign: 'right', align: 'center'
                    },

                    {
                        text: 'ת.ז', dataField: 'MemberId', width: 100, cellsalign: 'right', align: 'center'
                    },
                    {
                        text: 'שם מלא', dataField: 'MemberName', width: 160, cellsalign: 'right', align: 'center'
                    },
                    {
                        text: 'שם חברה', dataField: 'CompanyName', width: 160, cellsalign: 'right', align: 'center'
                    },
                    {
                        text: ' עיר   ', dataField: 'CityName', cellsalign: 'right', align: 'center'
                    },
                    {
                        text: ' כתובת ', dataField: 'Address', cellsalign: 'right', align: 'center'
                    },
                    {
                        text: 'טלפון נייד', dataField: 'CellPhone', cellsalign: 'right', align: 'center'
                    },
                    {
                        text: 'דואר אלקטרוני', dataField: 'Email', cellsalign: 'right', align: 'center'
                    },
                    {
                        text: 'מועד הצטרפות', type: 'date', dataField: 'JoiningDate', cellsformat: 'd', cellsalign: 'right', align: 'center'
                    }
                ]
            });
    }

}
