

(function ($) {

    var media_model = function (element, model,readonly) { // core constructor
        this.$element = $(element);
        if (!this.$element.is('div')) {
            $.error('app_wiztabs should be applied to DIV element');
            return;
        }
        // ensure to use the `new` operator
        if (!(this instanceof media_model))
            return new media_model(element, model, readonly);
        // store an argument for this model
        this.Model = model;
        this.init(readonly);

        return this;
    };

    // create `fn` alias to `prototype` property
    media_model.fn = media_model.prototype = {
        loaded:false,
        init: function (readonly) {
            $('#uploadBid').val(this.Model.buildingId);
            $('#uploadUid').val(this.Model.propertyId);
            $('#uploadPt').val(this.Model.propertyType);

            var html = (function () {/*
                <div style="margin:5px;max-width:600px">
                    <input type="hidden" id="uploadBid" value="" />
                    <input type="hidden" id="uploadUid" value="" />
                    <input type="hidden" id="uploadPt" value="" />
                    <input type="hidden" id="uploadMid" value="" />
                    <input type="hidden" id="uploadMtype" value="" />
                    <input type="hidden" id="uploadPath" value="" />
                    <input type="hidden" id="uploadUuid" value="" />
               <div id="jqxWidget" style="margin: 0 auto; display: block; direction: rtl">
                <div id="splitter">
                    <div style="overflow: auto;" id="ContentPanel">
                    </div>
                    <div style="overflow:hidden;" id="MediaPanel">
                        <div style="border: none;" id="MediaList">
                        </div>
                    </div>
                </div>
                <div id="uploader" style="width:100%;display:block;">
                    <div style="width: 90%">
                        <div class="form-group active" style="padding: 10px">
                            <span class="btn-sm btn-success fileinput-button">
                                <i class="glyphicon glyphicon-plus"><span>הוספת קובץ</span></i>
                                <input id="fileupload" type="file" name="files[]" multiple>
                            </span><span>&nbsp;</span>
                            <span id="spnRemove" class="btn-sm btn-success fileinput-button">
                                <i class="glyphicon glyphicon-minus"><span>הסרת קובץ</span></i>
                                <input id="fileremove" type="button" value="הסרת קובץ" />
                            </span><span>&nbsp;</span>
                            <span id="spnRefresh" class="btn-sm btn-success fileinput-button">
                                <i class="glyphicon glyphicon-minus"><span>רענון</span></i>
                                <input id="filerefresh" type="button" value="רענון" />
                            </span>
                            <br>
                            <div id="progress" class="progress">
                                <div class="progress-bar progress-bar-success"></div>
                            </div>
                            </div>
                        <div id="files" class="files"></div>
                     <div>
                    </div>
                </div>
            </div>
        </div>
    </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

            if (readonly)
                html = html.replace('form-group active', 'form-group pasive')

            var container = $(html);
            this.$element.empty();
            this.$element.hide();
            this.$element.append(container);

            $('.group1').colorbox({ rel: 'group1' });

            $("#spnRemove").hide();
            $("#splitter").jqxSplitter({//orientation: 'horizontal',
                width: '100%', height: '300px',
                panels: [
                   { size: '85%', min: "5%" },
                   { size: "15%", min: "10%", collapsible: false }
                ]
            });
 
            $("#fileremove").on('click', function (event) {
                //e.preventDefault();
                var mediaId = $('#uploadMid').val();
                app_dialog.confirm("האם למחוק? קובץ " + mediaId, function () {
                    media_model.fn.doRemove();
                });
            });

            $('#filerefresh').click(function (e) {
                media_model.fn.doRefresh();
            });

            return this;
        },
        load: function () {
            if (this.loaded)
                return;

            var slf = this;

            var mediaSource =
            {
                dataType: "json",
                dataFields: [
                    { name: 'MediaId', type: 'number' },
                    { name: 'PropertyId', type: 'number' },
                    { name: 'BuildingId', type: 'number' },
                    { name: 'MediaType', type: 'string' },
                    { name: 'PropertyType', type: 'string' },
                    { name: 'MediaPath', type: 'string' }
                ],
                id: 'MediaId',
                type: 'POST',
                data: this.Model,//{ 'buildingId': this.Model.buildingId, 'propertyId': this.Model.propertyId, 'propertyType': this.Model.propertyType },
                url: '/Building/GetMediaView'
            };

            var mediaAdapter = new $.jqx.dataAdapter(mediaSource, {
                loadComplete: function (records) {
                    slf.validateMediaListCount();
                }
            });

            $("#MediaList").jqxListBox(
            {
                rtl: true,
                source: mediaAdapter,
                width: '100%',
                height: '100%',
                checkboxes: false,
                displayMember: 'MediaPath',
                valueMember: 'MediaId',
                selectedIndex: 0,
                itemHeight: 60,
                renderer: function (index, label, value) {
                    //var datarecord = mediaAdapter[index];
                    var icon = slf.getImgTumb(label);
                    var table = '<li><span title="Media Id : ' + value + ', Id :' + slf.Model.propertyId + '">' + icon + '</span></li>';
                    return table;
                }
            });

            $('#MediaList').on('select', function (event) {
                slf.updatePanel(event.args.item);
                //$("#fileremove").prop('disabled', false);
                $("#spnRemove").show();
            });

            $('#fileupload').fileupload({
                maxFileSize: 10400000,//10mb
                url: '/Media/MediaUpload',
                formData: {
                    param1: this.Model.buildingId,
                    param2: this.Model.propertyId,
                    param3: this.Model.propertyType
                },
                //dataType: 'json',
                done: function (e, data) {
                    if (data.result)
                        app_jqxnotify.notify(data.result.Message);
                    else
                        app_jqxnotify.notify("Unknown result!");

                    slf.resetPprogress();
                    slf.doRefresh();
                },
                error: function (jqXHR, status, error) {
                    alert(error);
                    //app_jqxnotify.notify(error, "error");
                },
                progressall: function (e, data) {
                    slf.doPprogress(data);
                }
            }).prop('disabled', !$.support.fileInput)
               .parent().addClass($.support.fileInput ? undefined : 'disabled')
               .bind('fileuploadsubmit', function (e, data) {
                   slf.resetPprogress();
               });

            this.loaded = true;

        },
        validateMediaListCount: function () {
            var items = $("#MediaList").jqxListBox('getItems');
            if (items == null || items.length == 0) {
                $("#fileupload").show();
                $("#spnRemove").hide();
                $("#ContentPanel").empty();
            }

            else if (items.length < 5) {
                $("#fileupload").show();
                $("#spnRemove").show();
            }
            else {
                $("#fileupload").hide();
                $("#spnRemove").show();
            }
        },
        UrlExists: function (url) {
            var http = new XMLHttpRequest();
            http.open('HEAD', url, false);
            http.send();
            return http.status != 404;
        },
        getPropertyType: function (t) {
            switch (t) {
                case 'u':
                    return 'Unit';
                case 'b':
                    return 'Building';
                case 'p':
                    return 'Plots';
            }
            return 'Property';
        },
        getImgUrl: function (mediaType, picUrl) {
            if (mediaType == 'img')
                return '/Uploads/img' + '/' + picUrl;
            else if (mediaType == 'video')
                return '/Uploads/img' + '/' + picUrl;
            else if (mediaType == 'doc')
                return '/Uploads/doc' + '/' + picUrl;
            else
                return picUrl;
        },
        getImgTumb: function (picUrl) {
            var mediaType = media_model.fn.getMediaType(picUrl);
            return media_model.fn.getImgMediaTumb(mediaType, picUrl);
        },
        getImgMediaTumb: function (mediaType, picUrl) {
            var imgurl;
            var imgtumb;
            if (mediaType == 'img') {
                imgurl = '/Uploads/img' + '/' + picUrl;
                //imgtumb = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>').colorbox({ rel: 'group1' });
                //imgtumb = '<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:80px; height:auto;"/></a>';


                imgtumb = '<img src="' + imgurl + '" style="max-width:80px;max-height:60px; height:auto;"/>'

            }
            else if (mediaType == 'video') {
                imgurl = '/Images/video-small.jpg'; //'/Uploads/img' + '/' + picUrl;
                imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>'
            }
            else if (mediaType == 'doc') {
                imgurl = '/Images/doc-small.jpg';
                imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>';
            }
            else if (mediaType == 'link') {
                imgurl = '/Images/link-small.jpg';
                imgtumb = '<span><img src="' + imgurl + '" style="max-width:80px; height:auto;"/>';
            }
            else {
                imgurl = '/Images/none-small.jpg';
                imgtumb = '<img src="' + imgurl + '" style="max-width:80px; height:auto;"/>';

            }
            return imgtumb;
        },
        getImgTag: function (mediaType, picUrl) {

            var imgurl;
            var imgtag;
            var imgtumb;

            if (mediaType == 'img') {
                imgurl = '/Uploads/img' + '/' + picUrl;
                imgtag = $('<div style="margin: 10px;"><b>תמונה:</b><br/></div><div style="margin: 10px;overflow:auto;"><img src="' + imgurl + '"/></div>');
            }
            else if (mediaType == 'video') {
                imgurl = '/Uploads/img' + '/' + picUrl;
                imgtumb = media_model.fn.getImgMediaTumb(mediaType, picUrl);
                imgtag = $('<div style="margin: 10px;"><b>וידאו:</b><br/></div><div style="margin: 10px;overflow:auto;"><img src="' + imgurl + '"/></div>');
            }
            else if (mediaType == 'doc') {
                imgurl = '~/Uploads/doc' + '/' + picUrl;
                imgtumb = media_model.fn.getImgMediaTumb(mediaType, picUrl);
                imgtag = $('<div style="margin: 10px;"><b>מסמך:</b><br/></div><div><a href="' + imgurl + '">' + imgtumb + '</a></div>');
            }
            else if (mediaType == 'link') {
                imgtumb = media_model.fn.getImgMediaTumb(mediaType, picUrl);
                imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">' + imgtumb + '</a></div>');
            }
            else {
                imgtumb = media_model.fn.getImgMediaTumb(mediaType, picUrl);
                imgtag = $('<div style="margin: 10px;"><b>קישור:</b><br/></div><div><a href="' + picUrl + '">' + imgtumb + '</a></div>');

            }
            return imgtag;

            //return '<div style="margin: 10px;overflow:auto;"><img src="' + getImgUrl(mediaType,picUrl) + '"/></div>';
        },
        getFileExtension: function (filename) {
            return filename.split('.').pop();
        },
        getMediaType: function (filename) {

            if (filename.substr(0, 4) == 'http')
                return 'link';
            var extension = media_model.fn.getFileExtension(filename);
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
                case 'txt':
                    return 'doc';
            }

            return "none";
        },
        updatePanel: function (item) {
            //var datarecord = data[index];

            var slf=media_model.fn;

            var mediaId = item.value;
            var picUrl = item.label;
            var mediaType = slf.getMediaType(picUrl);


            $('#uploadMid').val(mediaId);
            $('#uploadMtype').val(mediaType);
            $('#uploadPath').val(picUrl);

            var container = $('<div style="margin: 5px;"></div>')

            container.append('<br/>');
            //var imgtag = getImgTag(mediaType, picUrl);
            var imgurl;
            var imgtag;
            var imgtumb;

            if (mediaType == 'img') {
                imgurl = '/Uploads/img' + '/' + picUrl;
                if (slf.UrlExists(imgurl))
                    imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
            }
            else if (mediaType == 'video') {
                imgurl = '/Uploads/img' + '/' + picUrl;
                if (slf.UrlExists(imgurl))
                    imgtag = $('<a class="group1" href="' + imgurl + '"><img src="' + imgurl + '" style="max-width:400px; height:auto;"/></a>').colorbox({ rel: 'group1' });
            }
            else if (mediaType == 'doc') {
                imgurl = '/Uploads/doc' + '/' + picUrl;
                imgtumb = slf.getImgMediaTumb(mediaType, picUrl);
                if (slf.UrlExists(imgurl))
                    imgtag = $('<div style="margin: 10px;text-align:center"><b>מסמך:</b><br/><a href="' + imgurl + '">' + imgtumb + '</a></div>');
            }
            else if (mediaType == 'link') {
                imgtumb = slf.getImgMediaTumb(mediaType, picUrl);
                if (slf.UrlExists(imgurl))
                    imgtag = $('<div style="margin: 10px;text-align:center"><b>קישור:</b><br/><a href="' + picUrl + '">' + imgtumb + '</a></div>');
            }
            else {
                imgtumb = slf.getImgMediaTumb(mediaType, picUrl);
                if (slf.UrlExists(imgurl))
                    imgtag = $('<div style="margin: 10px;text-align:center"><b>קישור:</b><br/><a href="' + picUrl + '">' + imgtumb + '</a></div>');
            }
            container.append(imgtag);
            $("#ContentPanel").html(container.html());
        },
        generateUUIDv4: function () {
            var d = new Date().getTime();
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
            return uuid;
        },
        generateUUID: function (v) {
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
        picuuid: 0,
        doUpload: function (data) {
            $.each(data.files, function (index, file) {
                var extension = media_model.fn.getFileExtension(file.name);
                var uuid = media_model.fn.generateUUID('16');
                var fname = $('#uploadBid').val() + '_' + $('#uploadUid').val() + '_' + $('#uploadPt').val() + '_' + $('#picid').val() + '.' + extension;
                $('<p/>').text(file.name).appendTo('#files');
                $('#files' + picnum).prepend($('<img>', { style: 'max-width:80px;height:auto', src: '/Uploads/media' + '/' + fname }))//file.name }))
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
        doRemove: function () {
            var id = $('#uploadMid').val();
            var filename = $('#uploadPath').val();
            var mediaType = $('#uploadMtype').val();

            $.ajax({
                url: '/Media/MediaRemove',//Url.Content("~/Ws/UploadFiles.asmx/MediaRemove")',
                type: "POST",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: "{'id':'" + id + "', 'mediaType' : '" + mediaType + "', 'filename':'" + filename + "'}",
                success: function (data) {
                    if (data) {
                        app_jqxnotify.notify(data.Message);
                    }
                    //alert('הקובץ הוסר');
                    $('#files').html('');
                    //$('#media_images').jqxScrollView('refresh');

                    media_model.fn.doRefresh();
                    //mediaAdapter.dataBind();

                    //parent.triggerImageChanged();
                },
                error: function (jqXHR, status, error) {
                    alert(error);
                }
            });
        },
        doRefresh: function () {
           $("#MediaList").jqxListBox('source').dataBind();
            //mediaAdapter.dataBind();
        }
    };

    // expose the library
    window.media_model = media_model;

})(jQuery)



// Extension:
media_model.fn.mediaFiles = function () {

    var slf = this;
    $('#btnImages').click(function (e) {
        e.preventDefault();

        if (!slf.loaded)
            slf.load();

        $('#media-files').toggle();
    });
    return this; // return `this` for chainability
};

//media_model('#media-files', { 'buildingId': slf.PlotsId, 'propertyId': slf.PlotsId, 'propertyType': "p" }, this.allowEdit == 0).app_media();


var app_media = {
    propertyTypeToChar: function (propertyType) {

        if (propertyType == 1)
            return "u";
        if (propertyType == 2)
            return "b";
        if (propertyType == 3)
            return "p"
        //default
        return "u";
    },
    loadImages: function (id, pt) {
        if (id === undefined || id == 0)
            return;
        if (typeof pt === 'undefined' || pt == null) { pt = 'u'; }

        for (var i = 0; i < 3; i++) {
            $("#media_image" + i).html('');
        }

        $.ajax({
            async: true,
            type: "POST",
            url: "/Building/GetMediaView",
            data: { 'buildingId': 0, 'propertyId': id, 'propertyType': '' + pt + '' },
            dataType: "json",
            success: function (data) {
                if (data === undefined || data == null)
                    return;
                $.each(data, function (index, value) {
                    console.log(value.MediaPath);
                    if (value.MediaPath && value.MediaType == 'img') {
                        var src = app_rout.mediaPath() + value.MediaType + "/" + value.MediaPath;
                        $("#media_image" + index).html('<img id="theImg" src="' + src + '" style="max-width:240px;border:solid 1px #808080" />');
                    }
                    else if (value.MediaPath && value.MediaType == 'doc') {
                        var src = app_rout.mediaPath() + value.MediaType + "/" + value.MediaPath;
                        $("#media_image" + index).html('<a href="' + src + '"><img id="theImg" src="/Images/doc.jpg" style="max-width:240px;border:solid 1px #808080" /></a>');
                    }
                    else if (value.MediaPath && value.MediaType == 'video') {
                        var src = app_rout.mediaPath() + value.MediaType + "/" + value.MediaPath;
                        $("#media_image" + index).html('<a href="' + src + '"><img id="theImg" src="/Images/video.jpg" style="max-width:240px;border:solid 1px #808080" /></a>');
                    }
                    if (index > 2)
                        return;
                });

            },
            error: function (e) {
                //alert(e);
            }
        });

    }
};