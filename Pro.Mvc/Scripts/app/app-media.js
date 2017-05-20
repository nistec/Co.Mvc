//======================== upload
app_media = {
    filePrefix: '',
    Folder: '',
    RefId: '',
    RefType: '',
    CurrentFileId:0,
    CurrentFilePath:null,
    CurrentMediaType:null,

    load: function (Model) {
        this.filePrefix = Model.FilePrefix;
        this.Folder=Model.Folder,
        this.RefId = Model.RefId;
        this.RefType = Model.RefType;
        this.loadControl(Model.Folder);

        return this;
    },

    loadControl: function (folder) {

        var slf=this;
        
        var mediaSource =
         {
             dataType: "json",
             dataFields: [
                  { name: 'FileId', type: 'number' },
                 { name: 'FileName', type: 'string' },
                 { name: 'Pid', type: 'number' },
                 { name: 'MediaType', type: 'string' },
                 { name: 'FileSubject', type: 'string' },
                 { name: 'FileAction', type: 'string' },
                 { name: 'RefType', type: 'string' },
                 { name: 'RefId', type: 'string' },
                 { name: 'FileInfo', type: 'string' },
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
             displayMember: 'FileName',
             valueMember: 'FileInfo',
             selectedIndex: 0,
             itemHeight: 50,
             renderer: function (index, label, value) {
                 //var datarecord = mediaAdapter[index];
                 var args = value.split('|');
                 var path = args[0];
                 var fileId =(args.length > 1)? args[1]:slf.RefId;
                 var table = '<table style="min-width: 130px;"><tr><td style="width: 40px;padding:8px;" rowspan="2">' + slf.getImgTumb(label) + '</td><td>File name : ' + label + '</td></tr><tr><td>Item Id :' + fileId + '</td></tr></table>';
                 return table;
             }
         });
   
        $('#fileupload').fileupload({
            url: '/Media/MediaUpload',
            formData: {
                //param1: folder
                param1: slf.RefId,// $('#uploadFolder').val()
                param2: 0,// $('#uploadUid').val(),
                param3: slf.RefType,//$('#uploadPt').val()
            },
            //dataType: 'json',
            done: function (e, data) {
                //var keys = Object.keys(data);
                app_dialog.alert(data.textStatus);
                slf.resetPprogress();
                mediaAdapter.dataBind();
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            },
            progressall: function (e, data) {
                slf.doPprogress(data);
            }
        }).prop('disabled', !$.support.fileInput)
             .parent().addClass($.support.fileInput ? undefined : 'disabled')
             .bind('fileuploadsubmit', function (e, data) {
                 slf.resetPprogress();
                 //picuuid = generateUUID('16');
                 //data.formData = {
                 //    param1: $('#uploadBid').val(),
                 //    param2: $('#uploadUid').val(),
                 //    param3: $('#uploadPt').val()
                 //};
             });

        //$('#fileremove').click(function (e) {
        //    e.preventDefault();
        //    var fileId = $('#uploadMid').val();
        //    var filename = $('#uploadFilePath').val();
        //    var mediaType = $('#uploadMediaType').val();
        //    def.doRemove(fileId, mediaType, filename);
        //});
    },

    //getImgUrl:function (baseUrl, folder, mediaType, picUrl) {
    //    if (mediaType == 'img')
    //        return baseUrl + '/Uploads/' + folder + '/' + picUrl;
    //    else if (mediaType == 'doc')
    //        return baseUrl + '/Uploads/' + folder + '/' + picUrl;
    //    else
    //        return picUrl;
    //},
    getImgUrl:function (mediaType, picUrl) {

        if (mediaType == 'img') {
            return this.Folder.replace("~/","/") + "/img/" + picUrl;
        }
        else if (mediaType == 'video') {
            return this.Folder.replace("~/", "/") + "/video/" + picUrl;
        }
        else if (mediaType == 'doc') {
            return this.Folder.replace("~/", "/") + "/doc/" + picUrl;
        }
        else
            return picUrl;
    },

    getImgTumb: function (picUrl){//baseUrl, folder, picUrl) {
        var mediaType = app_media.getMediaType(picUrl);
        baseUrl = app.appPath();
        var imgurl;
        var imgtumb;
        if (mediaType == 'img') {
            imgurl = this.getImgUrl(mediaType, picUrl);// baseUrl + '/Uploads/' + folder + '/' + picUrl;
            //imgtumb = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>').colorbox({ rel: 'group1' });
            //imgtumb = '<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>';
            imgtumb = '<img src="' + imgurl + '" style="max-width:20px; height:auto;"/>'

        }
        else if (mediaType == 'video') {
            imgurl = this.getImgUrl(mediaType, picUrl);
            imgtumb = '<img src="' + imgurl + '" style="max-width:20px; height:auto;"/>'
        }
        else if (mediaType == 'doc') {
            imgurl = baseUrl + '/Images/icons/doc.gif';
            imgtumb = '<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>'
            //imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }
        else if (mediaType == 'script') {
            imgurl = baseUrl + '/Images/icons/script.gif';
            imgtumb = '<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>'
            //imgtag = $('<div style="margin: 10px;"><b>קובץ:</b><br/></div><div><a href="' + imgurl + '">לצפיה</a></div>');
        }
        else if (mediaType == 'link') {
            imgurl = baseUrl + '/Images/icons/link.gif';
            imgtumb = '<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>'
            //imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');
        }
        else {
            imgurl = baseUrl + '/Images/icons/info.gif';
            imgtumb = '<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>'
            //imgtumb = $('<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></span>');

        }
        return imgtumb;
    },

    getImgTag:function (baseUrl,folder, mediaType, picUrl) {

        var imgurl;
        var imgtag;

        if (mediaType == 'img') {
            imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
            imgtag = $('<div style="margin: 10px;"><b>תמונה:</b><br/></div><div style="margin: 10px;overflow:auto;"><img src="' + imgurl + '"/></div>');
        }
        else if (mediaType == 'doc') {
            imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
            imgtag = $('<div style="margin: 10px;"><b>מסמך:</b><br/></div><div><a href="' + imgurl + '">לצפיה</a></div>');
        }
        else if (mediaType == 'script') {
            imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
            imgtag = $('<div style="margin: 10px;"><b>קובץ:</b><br/></div>');
        }
        else if (mediaType == 'link') {
            imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">לצפיה</a></div>');
        }
        else {
            imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div>');

        }
        return imgtag;

        //return '<div style="margin: 10px;overflow:auto;"><img src="' + getImgUrl(mediaType,picUrl) + '"/></div>';
    },

    getFileExtension:function (filename) {
        return filename.split('.').pop();
    },

    getMediaType:function (filename) {
        if (filename.substr(0, 4) == 'http')
            return 'link';
        var extension = app_media.getFileExtension(filename);
        switch (extension.toLowerCase()) {
            case 'jpg':
            case 'jpeg':
            case 'png':
            case 'gif':
            case 'tif':
                return 'img';
            case 'pdf':
            case 'doc':
            case 'docx':
            case 'xls':
            case 'xlsx':
            case 'csv':
            case 'txt':
                return 'doc';
            case 'css':
            case 'js':
                return 'script';
        }
        return "none";
    },

    getPropertyType : function (t) {
        switch (t) {
            case 't':
                return 'Task';
            case 'p':
                return 'Project';
            case 'l':
                return 'Leads';
        }
        return 'Task';
    },

    generateUUIDv4:function () {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },

    generateUUID:function (v) {
        if (v == 'v4')
            return Math.uuid() // RFC4122 v4 UUID
        //"4FAC90E7-8CF1-4180-B47B-09C3A246CB67"
        if (v == '62')
            return Math.uuid(17) // 17 digits, base 62 (0-9,a-Z,A-Z)
        //"GaohlDbGYvOKd11p2"

        if (v == '10')
            return Math.uuid(5, 10) // 5 digits, base 10
        //"84274"

        if (v == '16')
            return Math.uuid(8, 16) // 8 digits, base 16
        //"19D954C3"
    },

    //var picuuid;
    doUpload : function (data) {
        $.each(data.files, function (index, file) {
            var extension = app_media.getFileExtension(file.name);
            var uuid = app_media.generateUUID('16');
            //var fname = $('#uploadBid').val() + '_' + $('#uploadUid').val() + '_' + $('#uploadPt').val() + '_' + $('#picid').val() + '.' + extension;
            $('<p/>').text(file.name).appendTo('#files');
            $('#files' + picnum).prepend($('<img>', { style: 'max-width:80px;height:auto', src: '@Url.Content("~/Uploads/media")' + '/' + file.name }))//file.name }))
        });
    },

    doPprogress: function (data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progress .progress-bar').css(
            'width',
            progress + '%'
        );
    },

    resetPprogress: function () {
        $('#progress .progress-bar').css(
            'width', '0%'
        );
    },

    getKeys: function (obj) {
        var keys = [];
        for (var key in obj) {
            keys.push(key);
        }
        return keys;
    },

    updatePanel: function (item) {
        //baseUrl, folder, 
        //var datarecord = data[index];

        var fileinfo = item.value;
        if (fileinfo === undefined || fileinfo == null)
            return;
        var args= fileinfo.split('|');
        var path = args[0];
        var fileId = args[1];
        this.CurrentFileId = fileId;

        var filename = item.label;
        var mediaType = this.getMediaType(filename);

        //$('#uploadFileId').val(fileId);
        //$('#uploadMediaType').val(mediaType);
        //$('#uploadFilePath').val(path);

        var container = $('<div style="margin: 5px;height:200px;border:solid 1px red;"></div>');

         container.append('<br/>');
        //var imgtag = getImgTag(mediaType, picUrl);

        var baseUrl = app.appPath();
        var imgurl = path.replace("~/", "/") + "/" + filename;
        var imgtag;

        if (mediaType == 'img') {
            //imgurl = this.getImgUrl(mediaType, picUrl);// baseUrl + '/Uploads/' + folder + '/' + picUrl;
            imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
        }
        else if (mediaType == 'video') {

            //imgurl = this.getImgUrl(mediaType, picUrl);
            imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
        }
        else if (mediaType == 'doc') {
            //imgurl = this.getImgUrl(mediaType, picUrl);
            //imgurl = imgurl.replace(baseUrl,"");
            //target="_blank"

            var subject = args[2] || "";
            var action = args[3] || "";

            imgtag = $('<div class="rtl" style="margin: 10px;"><b>מסמך:</b>' + filename + '<br/><div>' +
                '<a href="/Media/DownloadFile?f=' + imgurl + '">להורדה</a>' +
                '<form id="formFile" method="post" action="/Media/UpdateFileInfo" onsubmit="return app_media.doUpdateFileInfo(this)">' +
                '<br/><b>נושא:</b><br/><input type="text" name="FileSubject" value="'+subject+'"/>' +
                '<br/><b>סוג פעולה:</b><br/><input type="text" name="FileAction" value="'+action+'"/>' +
                '<input type="Hidden" name="FileName" value="' + filename + '"/>' +
                '<input type="Hidden" name="FileId" value="' + fileId + '"/>' +
                '<br/><input type="submit" value="עדכון"/>' +
                '</form>'+
                '</div></div>');
        }
        else if (mediaType == 'script') {
            //imgurl = this.getImgUrl(mediaType, picUrl);
            imgtag = $('<div class="rtl" style="margin: 10px;"><b>קובץ:</b><br/><div><span>' + imgurl + '</span></div></div>');
        }
        else if (mediaType == 'link') {
            imgtag = $('<div class="rtl" style="margin: 10px;"><b>קישור:</b><br/><div><a href="' + filename + '">לצפיה</a></div></div>');
        }
        else {
            imgtag = $('<div class="rtl" style="margin: 10px;"><b>קישור:</b></div>');
        }

        //if (mediaType == 'img') {
        //    imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
        //    imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
        //}
        //else if (mediaType == 'video') {
        //    imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
        //    imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
        //}
        //else if (mediaType == 'doc') {
        //    imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
        //    imgtag = $('<div class="rtl" style="margin: 10px;"><b>מסמך:</b><br/><div><a href="' + imgurl + '">לצפיה</a></div></div>');
        //}
        //else if (mediaType == 'script') {
        //    imgurl = baseUrl + '/Uploads/' + folder + '/' + picUrl;
        //    imgtag = $('<div class="rtl" style="margin: 10px;"><b>קובץ:</b><br/><div><span>' + imgurl + '</span></div></div>');
        //}
        //else if (mediaType == 'link') {
        //    imgtag = $('<div class="rtl" style="margin: 10px;"><b>קישור:</b><br/><div><a href="' + picUrl + '">לצפיה</a></div></div>');
        //}
        //else {
        //    imgtag = $('<div class="rtl" style="margin: 10px;"><b>קישור:</b></div>');
        //}
        container.append(imgtag);


        //container.append(imgtag);

        //container.append("<div style='margin: 10px;'><b>תמונה:</b><br/></div>" + getImgTag(picUrl));

        $("#ContentPanel").html(container.html());

    },
    doUpdateFileInfo: function (form) {
        var slf = this;
        if (form == null)
        {
            app_dialog.alert("incorrect form");
            return false;
        }
        //e.preventDefault();
        var action = form.action;// $("#formFile").attr('action');
        var postData = $(form).serialize();// app.serializeForm(form);//getFormInputs(["#formFile"]);
        $.ajax({
            url: '/Media/UpdateFileInfo',
            type: "POST",
            dataType: 'json',
            //contentType: "application/json; charset=utf-8",
            //contentType: "application/x-www-form-urlencoded;charset=utf-8",
            data: postData,
            success: function (data) {
                //if (data) {
                //    app_dialog.alert(data.Message);
                //}
                //alert('הקובץ הוסר');
                $("#MediaList").jqxListBox('source').dataBind();
                app_dialog.alert(data.Message);
            },
            error: function (jqXHR, status, error) {
                app_dialog.alert(error);
            }
        });
        return false;
    },
    doRemove: function () {//folder, mediaType, filename) {

        var item = $("#MediaList").jqxListBox('getSelectedItem');
        if (item == null)
            return;

        var fileinfo = item.value;
        if (fileinfo === undefined || fileinfo == null)
            return;
        var args = fileinfo.split('|');
        var path = args[0];
        var fileId = args[1];

        var filename = item.label;
        var mediaType = this.getMediaType(filename);

        if (confirm("האם להסיר את הקובץ מהרשימה " + filename) == false)
            return;

        var slf = this;
        $.ajax({
            url: '/Media/MediaRemove',
            type: "POST",
            dataType: 'json',
            //contentType: "application/json; charset=utf-8",
            data: {id: fileId , mediaType :  mediaType, filename: filename },
            success: function (data) {
                if (data) {
                    app_messenger.Notify(data);
                }
                $('#files').html('');
                $("#MediaList").jqxListBox('source').dataBind();
            },
            error: function (jqXHR, status, error) {
               app_dialog.alert(error);
            }
        });
    },
    doRefresh: function () {
        $("#MediaList").jqxListBox('source').dataBind();
    }
}