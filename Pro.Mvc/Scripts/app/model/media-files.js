
function app_media_file(Model) {

    this.fileFrefix = Model.FilePrefix;
    this.RefId = Model.RefId;
    this.RefType = Model.RefType;
    var slf = this;

    this.getList = function () {

       var mediaSource =
       {
           dataType: "json",
           dataFields: [
               { name: 'FileName', type: 'string' },
               { name: 'Pid', type: 'number' },
               { name: 'MediaType', type: 'string' },
               { name: 'FileSubject', type: 'string' },
               { name: 'RefType', type: 'string' },
               { name: 'RefId', type: 'string' },
               { name: 'FilePath', type: 'string' }
           ],
           id: 'FileName',
           type: 'POST',
           data: { 'RefId': slf.RefId, 'RefType': slf.RefType },
           url: '/Media/GetMediaRefFiles'
       };

       var mediaAdapter = new $.jqx.dataAdapter(mediaSource);

       $("#MediaList").jqxListBox(
        {
            rtl: true,
            source: mediaAdapter,
            width: '100%',
            height: '100%',
            checkboxes: false,
            displayMember: 'FilePath',
            valueMember: 'FileName',
            selectedIndex: 0,
            itemHeight: 70,
            renderer: function (index, label, value) {
                //var datarecord = mediaAdapter[index];
                var table = '<table style="min-width: 130px;"><tr><td style="width: 40px;padding:8px;" rowspan="2">' + slf.getImgTumb(label) + '</td><td>File Id : ' + value + '</td></tr><tr><td>Item Id :' + slf.RefId + '</td></tr></table>';
                return table;
            }
        });
    };

    this.getPropertyType = function (t) {
        switch (t) {
            case 't':
                return 'Task';
            case 'p':
                return 'Project';
            case 'l':
                return 'Leads';
        }
        return 'Task';
    };

    this.getImgTumb = function (picUrl) {

        var mediaType = slf.getMediaType(picUrl);

        var imgurl;
        var imgtumb;
        if (mediaType == 'img') {
            imgurl = slf.getImgUrl(mediaType, picUrl);
            //imgtumb = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>').colorbox({ rel: 'group1' });
            //imgtumb = '<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>';
            imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>'

        }
        else if (mediaType == 'video') {
            imgurl = slf.getImgUrl(mediaType, picUrl);
            imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>'
        }
        else if (mediaType == 'doc') {
            imgurl = '/Images/doc.jpg';
            imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }
        else if (mediaType == 'link') {
            imgurl = '/Images/link.jpg';
            imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }
        else {
            imgurl = '/Images/none.jpg';
            imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');

        }
        return imgtumb;
    };

    this.getImgUrl = function (mediaType, picUrl) {

        if (mediaType == 'img') {
            return "/" + filePrefix + "/img/" + picUrl;
        }
        else if (mediaType == 'video') {
            return "/" + filePrefix + "/video/" + picUrl;
        }
        else if (mediaType == 'doc') {
            return "/" + filePrefix + "/doc/" + picUrl;
        }
        else
            return picUrl;
    };

    this.getFileExtension = function (filename) {
        return filename.split('.').pop();
    };

    this.getMediaType = function (filename) {
        if (filename.substr(0, 4) == 'http')
            return 'link';
        var extension = getFileExtension(filename);
        switch (extension.toLowerCase()) {
            case 'jpg':
            case 'jpeg':
            case 'png':
            case 'gif':
            case 'tif':
                return 'img';
            case 'mp4':
            case 'avi':
                return 'video';
            case 'pdf':
            case 'doc':
            case 'docx':
            case 'xls':
            case 'xlsx':
            case 'csv':
            case 'txt':
                return 'doc';
        }
        return "none";
    };

    this.updatePanel = function (item) {

        //var datarecord = data[index];

        var mediaId = item.value;
        var picUrl = item.label;
        var mediaType = slf.getMediaType(picUrl);


        $('#uploadMid').val(mediaId);
        $('#uploadMtype').val(mediaType);
        $('#uploadPath').val(picUrl);

        var container = $('<div style="margin: 5px;"></div>')

        //container.append("<input type='button' value='הסרת קובץ' onclick='doRemove()' />");
        //.on('click', function () {
        //     //e.preventDefault();
        //     if (confirm("האם למחוק?")) {
        //         doRemove();
        //     }
        // });

        container.append('<br/>');
        //var imgtag = getImgTag(mediaType, picUrl);
        var imgurl;
        var imgtag;
        var imgtumb;

        if (mediaType == 'img') {
            imgurl = "/" + filePrefix + "/img/" + picUrl;
            imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
            imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>'
        }
        else if (mediaType == 'video') {
            imgurl = "/" + filePrefix + "/video/" + picUrl;
            imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
            imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>'
        }
        else if (mediaType == 'doc') {
            imgurl = "/" + filePrefix + "/doc/" + picUrl;
            imgtag = $('<div style="margin: 10px;"><b>מסמך:</b><br/></div><div><a href="' + imgurl + '">לצפיה</a></div>');
            imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }
        else if (mediaType == 'link') {
            imgurl = "/Images/link.jpg";
            imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">לצפיה</a></div>');
            imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }
        else {
            imgurl = "/Images/none.jpg";
            imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">לצפיה</a></div>');
            imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }

        container.append(imgtag);


        //container.append(imgtag);

        //container.append("<div style='margin: 10px;'><b>תמונה:</b><br/></div>" + getImgTag(picUrl));

        $("#ContentPanel").html(container.html());

    };
       

    $('#MediaList').on('select', function (event) {
        slf.updatePanel(event.args.item);
        //$("#fileremove").prop('disabled', false);
        $("#spnRemove").show();
    });

    //$("#fileremove").on('click', function (event) {
    //    //e.preventDefault();
    //    var mediaId = $('#uploadMid').val();
    //    if (confirm("האם למחוק? קובץ " + mediaId)) {
    //        slf.doRemove();
    //    }
    //});

    $('#fileupload').fileupload({
        url: '/Media/MediaUpload',
        formData: {
            param1: $('#uploadPid').val(),
            param2: $('#uploadUid').val(),
            param3: $('#uploadPtype').val()
        },
        //dataType: 'json',
        done: function (e, data) {
            //var keys = Object.keys(data);
           app_dialog.alert(data.textStatus);
            resetPprogress();
            mediaAdapter.dataBind();
        },
        error: function (jqXHR, status, error) {
           app_dialog.alert(error);
        },
        progressall: function (e, data) {
            doPprogress(data);
        }
    }).prop('disabled', !$.support.fileInput)
     .parent().addClass($.support.fileInput ? undefined : 'disabled')
     .bind('fileuploadsubmit', function (e, data) {
         resetPprogress();
         //picuuid = generateUUID('16');
         //data.formData = {
         //    param1: $('#uploadBid').val(),
         //    param2: $('#uploadUid').val(),
         //    param3: $('#uploadPt').val()
         //};
     });

    this.doUpload = function (data) {

        //uploader
        var picuuid;
        $.each(data.files, function (index, file) {
            var extension = slf.getFileExtension(file.name);
            var uuid = app.generateUUID('16');
            var fname = slf.filePrefix + $('#picid').val() + '.' + extension;
            $('<p/>').text(file.name).appendTo('#files');
            $('#files' + picnum).prepend($('<img>', { style: 'max-width:80px;height:auto', src: '/Media/Uploads/' + fname }))//file.name }))
        });

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
        var getKeys = function (obj) {
            var keys = [];
            for (var key in obj) {
                keys.push(key);
            }
            return keys;
        }
    }

    $('#fileremove').click(function (e) {
        e.preventDefault();
        var fileId = $('#uploadMid').val();
        var filename = $('#uploadPath').val();
        var mediaType = $('#uploadMtype').val();
        slf.doRemove(fileId,mediaType, filename);
    });

    this.doRemove = function (fileId,mediaType, filename) {
   
        $.ajax({
            url: '/Media/MediaRemove',
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: "{'id':'" + fileId + "', 'mediaType' : '" + mediaType + "', 'filename':'" + filename + "'}",
            success: function (data) {
                if (data) {
                   app_dialog.alert(data.Message);

                }
                //alert('הקובץ הוסר');
                $('#files').html('');
                mediaAdapter.dataBind();
            },
            error: function (jqXHR, status, error) {
               app_dialog.alert(error);
            }
        });
    };
}